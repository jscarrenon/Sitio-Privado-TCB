using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Tasks
{
    public class SyncUsersDataJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //TODO: Implement task
            System.Diagnostics.Debug.WriteLine("Test Message");
        }
    }
}