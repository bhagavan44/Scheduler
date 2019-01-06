using Jobs.Service.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading;

namespace Jobs.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
            scheduler.Start();

            IJobDetail JobDetail = JobBuilder.Create<Job1>().WithIdentity("Job1", "Group1").Build();

            ITrigger trigger = TriggerBuilder.Create()
  .WithIdentity("myTrigger", "group1")
  .StartNow()
  .WithSimpleSchedule(x => x
      .WithIntervalInSeconds(5)
      .RepeatForever())
  .Build();

            scheduler.ScheduleJob(JobDetail, trigger);

            //scheduler.GetCurrentlyExecutingJobs()

            Thread.Sleep(TimeSpan.FromSeconds(20));
        }
    }
}