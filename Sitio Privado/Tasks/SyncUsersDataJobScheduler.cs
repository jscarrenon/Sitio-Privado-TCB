using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Tasks
{
    public class SyncUsersDataJobScheduler
    {
        private static int IntervalInHours = Int32.Parse(ConfigurationManager.AppSettings["sync:IntervalInHours"]);
        private static int ExecutionHour = Int32.Parse(ConfigurationManager.AppSettings["sync:ExecutionTimeHour"]);
        private static int ExecutionMinutes = Int32.Parse(ConfigurationManager.AppSettings["sync:ExecutionTimeMinutes"]);

        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<SyncUsersDataJob>().Build();

            //TODO: update trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithDailyTimeIntervalSchedule(
                    s => s.WithIntervalInHours(IntervalInHours)
                    .OnEveryDay()
                    .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(ExecutionHour, ExecutionMinutes)))
                    .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}