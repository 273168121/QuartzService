using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using System.Timers;
using Topshelf;
using GlueNet;
using GlueNet.Services;
using XJob.Business;
using Quartz;
using GlueNet.Dependency;
using GlueNet.Bussiness;
namespace XJob.Service
{
    class Program
    {

        
        static void Main(string[] args)
        {
            HostFactory.Run(x =>                                 
            {
                x.Service<XJobs>(s =>                        
                {
                    s.ConstructUsing(name => new XJobs());     
                    s.WhenStarted(tc => tc.Start());               
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();   
                x.SetDescription("调度任务");        
                x.SetDisplayName("调度任务");                      
                x.SetServiceName("调度任务");                        
            });
        }
    }

    public class XJobs
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(XJobs));

        private IScheduler sche = null;

        public XJobs()
        {
            GlueCoreInstaller.Install();
            GlueBussinessCoreInstaller.Install();
            sche= QuartzNetService.Proxy.getScheduler();
            QuartzNetService.Proxy.AddListner(sche);




        }
        public void Start() {

            sche.Start();

          var sch=   QuartzNetService.Proxy.getScheduler();

            log.Info($"schedulerName111111111111111:{sch.SchedulerName}");
        }
        public void Stop() {
            sche.Shutdown();
        }
    }
}
