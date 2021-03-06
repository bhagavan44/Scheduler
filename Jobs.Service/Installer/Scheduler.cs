﻿using System.ServiceProcess;
using Jobs.Service.Jobs;
using Quartz;
using Quartz.Impl;

namespace Jobs.Service.Installer
{
    public partial class Scheduler : ServiceBase
    {
        private readonly IScheduler scheduler;

        public Scheduler()
        {
            InitializeComponent();
            scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<Job1>().WithIdentity("Job1", "Group1").Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .RepeatForever())
                  .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        protected override void OnStop()
        {
            scheduler.Shutdown();
            base.OnStop();
        }

        protected override void OnShutdown()
        {
            scheduler.Shutdown();
            base.OnShutdown();
        }
    }
}