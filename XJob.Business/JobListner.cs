using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using Common.Logging;
namespace XJob.Business
{
    /*
     * 使用方法
     * scheduler.ListenerManager.AddTriggerListener(myJobListener, GroupMatcher<TriggerKey>.AnyGroup());
     */

    internal class JobListner : IJobListener
    {

        private static ILog _log = LogManager.GetLogger("Sys");

        public string Name =>this.GetType().Name;

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            //throw new NotImplementedException();
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            try
            {
                if (context == null) return;
                IJobDetail job = null;
                if (context.JobDetail == null) return;
                job = context.JobDetail;

                _log.Info($"job.Key.Name: {job.Key.Name} context.NextFireTimeUtc: {context.NextFireTimeUtc}context.PreviousFireTimeUtc:{context.PreviousFireTimeUtc} context.FireTimeUtc:{context.FireTimeUtc} context.JobRunTime.TotalMilliseconds:{context.JobRunTime.TotalMilliseconds}");
                QuartzNetService.Proxy.UpdateJob(job.Key.Name, job.Key.Group, ((DateTimeOffset)context.NextFireTimeUtc).DateTime, ((DateTimeOffset)context.PreviousFireTimeUtc).DateTime, ((DateTimeOffset)context.FireTimeUtc).DateTime, context.JobRunTime.TotalMilliseconds);
            }
            catch(Exception ex)
            {
                _log.Error($"{ex.Message}");
            }
           

        }
    }
}
