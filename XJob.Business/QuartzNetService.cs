using System;
using System.Collections.Generic;
using GlueNet.Bussiness.Entities;
using GlueNet.Extensions;
using GlueNet.Services;
using GlueNet.DataAccess;
using System.Linq;
using LinqToDB.Data;
using LinqToDB;
using Quartz;
using Quartz.Impl;
using System.Collections.Specialized;
using System.Reflection;
using Quartz.Impl.Matchers;
using XJob.Business.Entities;
namespace XJob.Business
{

    /// <summary>
    /// 
    /// </summary>
    public class QuartzNetService : GlueService<QuartzNetService>
    {


        public virtual List<TsJobs> QueryAllJobs()
        {

            return (from n in this.GetDbContext().Query<TsJobs>() select n).ToList();
        }


        /// <summary>
        /// 更新job执行
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="jobGroup"></param>
        /// <param name="nextDate"></param>
        /// <param name="prevDate"></param>
        /// <param name="executeDate"></param>
        /// <param name="executeTimespan"></param>
        /// <param name="executeNum"></param>
        /// <returns></returns>
        public virtual ResultInfo UpdateJob(string jobName, string jobGroup, DateTime nextDate, DateTime prevDate, DateTime executeDate, double executeTimespan)
        {
             
            var rs = new ResultInfo();
            try
            {
                //更新执行时间、执行时长、执行次数、下次执行时间、
                int i = this.GetDbContext().Query<TsJobs>().Where(m => m.CJobId == jobName && m.CJobGroup == jobGroup)
                     .Set(m => m.DExecuteTime, executeDate)
                     .Set(m => m.NExecuteTime, executeTimespan)
                     .Set(m => m.NExecuteNum, m => m.NExecuteNum + 1)
                     .Set(m => m.DNextTime, nextDate)
                     .Set(m => m.DPrevTime, prevDate)
                     .Update();
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }
             

        }


        /// <summary>
        /// 移除Job
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ResultInfo RemoveJob(TsJobs item)
        {
            var rs = new ResultInfo();
            try
            {
                var sche = this.getScheduler();
                if (sche == null) throw new Exception($"scheduler {item.CServerName}  not exist     !");
                IJobDetail job = null;
                job = sche.GetJobDetail(JobKey.Create(item.CJobId, item.CJobGroup));
                if (job == null) throw new Exception($"not exist job {item.CJobId}  in scheduler !");
                var trigger = new TriggerKey(item.CJobId, item.CJobGroup);
                sche.PauseTrigger(trigger);//停止触发器
                sche.UnscheduleJob(trigger); //移除触发器
                var result = sche.DeleteJob(JobKey.Create(item.CJobId, item.CJobGroup));
                this.GetDbContext().Query<TsJobs>().Where(m => m.Id == item.Id).Delete();
                return rs;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }

        }

        /// <summary>
        /// 恢复Job
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ResultInfo ResumeJob(TsJobs item)
        {
            var rs = new ResultInfo();
            try
            {
                var sche = this.getScheduler();
                if(sche==null) throw new Exception($"scheduler {item.CServerName}  not exist     !");
                IJobDetail job = null;
                job = sche.GetJobDetail(JobKey.Create(item.CJobId, item.CJobGroup));
                if (job == null) throw new Exception($"not exist job {item.CJobId}  in scheduler !");
                this.GetDbContext().Query<TsJobs>().Set(m => m.CJobStatus, "启动").Update();
                sche.ResumeJob(JobKey.Create(item.CJobId, item.CJobGroup));

                return rs;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }
        }
        /// <summary>
        /// 暂停Job
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ResultInfo PauseJob(TsJobs item)
        {
            var rs = new ResultInfo();
            try
            {
                var sche = this.getScheduler();
                if (sche == null) throw new Exception($"scheduler {item.CServerName}  not exist     !");
                IJobDetail job = null;
                job = sche.GetJobDetail(JobKey.Create(item.CJobId, item.CJobGroup));
                if (job == null) throw new Exception($"not exist job {item.CJobId}  in scheduler !");

                this.GetDbContext().Query<TsJobs>().Set(m => m.CJobStatus, "暂停").Update();

                sche.PauseJob(JobKey.Create(item.CJobId, item.CJobGroup));

                return rs;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }

        }

        /// <summary>
        /// 添加Job
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ResultInfo AddJob(TsJobs item)
        {
            var rs = new ResultInfo();
            try
            {
                item.CJobGroup = "DEFAULT";

                var sche = getScheduler();
                if (sche == null) throw new Exception($"scheduler {item.CServerName}  not exist     !");
                IJobDetail job = null;
                job = sche.GetJobDetail(JobKey.Create(item.CJobId, item.CJobGroup));

                if (job != null) throw new Exception($"job {item.CJobId} is already exis! ");

                if (job == null)
                {

                    var type = Type.GetType(item.CIjobName, true);

                    job = JobBuilder.Create(type)
    .WithIdentity(item.CJobId, item.CJobGroup)
    .Build();
                    /*
                    CronTrigger

withMisfireHandlingInstructionDoNothing
——不触发立即执行
——等待下次Cron触发频率到达时刻开始按照Cron频率依次执行

withMisfireHandlingInstructionIgnoreMisfires
——以错过的第一个频率时间立刻开始执行
——重做错过的所有频率周期后
——当下一次触发频率发生时间大于当前时间后，再按照正常的Cron频率依次执行

withMisfireHandlingInstructionFireAndProceed
——以当前时间为触发频率立刻触发一次执行
——然后按照Cron频率依次执行
*/
                    ITrigger trigger = TriggerBuilder.Create()
                     .WithIdentity(item.CJobId, item.CJobGroup).StartAt((DateTime)item.DStartTime).WithCronSchedule(item.CCron,x=>x.WithMisfireHandlingInstructionFireAndProceed()).Build();
                    item.CJobStatus = "启动";
                    DateTimeOffset next = (DateTimeOffset)trigger.GetFireTimeAfter((DateTimeOffset)item.DStartTime);
                    item.DNextTime = next.DateTime;
                    item.CServerName = sche.SchedulerInstanceId;
                    item.NExecuteNum = 0;
                    item.DCrtDate = DateTime.Now;
                    item.CCrtEmp = GlueNet.ApplicationContext.Current.User.UserID;
                    this.GetDbContext().Insert(item);
                    sche.ScheduleJob(job, trigger);

                }

                return rs;
            }
            catch (Exception ex)
            {
                rs.IsSuccess = false;
                rs.Message = ex.Message;
                return rs;
            }





        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="sched"></param>
        
          [RunLocal]
        public virtual void   AddListner(IScheduler sched)
        {

            IJobListener listener = new JobListner();

            sched.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());
        }

        /// <summary>
        /// 查询配置表中任务配置
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public virtual List<TsKeyValue> QueryScheduleList(string pcode)
        {

            return (from n in this.GetDbContext().Query<TsKeyValue>() where n.CPcode == pcode select n).ToList();
        }

         

        /// <summary>
        /// 获取任务
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        
        [RunLocal]
        public virtual IScheduler getScheduler()
        {


            var lstKeys = this.QueryScheduleList("Scheduler");

            NameValueCollection properties = new NameValueCollection();

            lstKeys.ForEach(m =>
            {
                properties[m.CCode] = m.CName;
            }); 
            properties["quartz.dataSource.default.connectionString"]= System.Configuration.ConfigurationManager.ConnectionStrings[properties["quartz.jobStore.dataSource"]].ConnectionString; 

            ISchedulerFactory schedf = new StdSchedulerFactory(properties);
            IScheduler sched = schedf.GetScheduler(properties["quartz.scheduler.instanceName"]);
            if (sched == null)
            {
                sched = schedf.GetScheduler();
            }


            return sched;


        }

    }
}
