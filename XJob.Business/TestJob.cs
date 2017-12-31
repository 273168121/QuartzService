using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.IO;
using Common.Logging;
namespace XJob.Business
{
    public class TestJob : IJob
    {
        private static ILog _log = LogManager.GetLogger("Sys");
        public virtual void Execute(IJobExecutionContext context)
        {

            _log.Info($"Execute {context.JobDetail.Key.Name} at {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}" );

        }
    }
}
