using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Tasks
{
    public class SyncUsersDataJobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SyncUsersDataJob>().Build();

            //TODO: update trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(
                    s => s.WithIntervalInSeconds(10)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(10, 17)))
                    .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}