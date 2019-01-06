using Jobs.Service.Jobs;
using Quartz;
using Quartz.Impl;
using System.ServiceProcess;

namespace Jobs.Service
{
    internal partial class Scheduler : ServiceBase
    {
        private IScheduler scheduler;

        public Scheduler()
        {
            InitializeComponent();
            scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        }

        protected override void OnStart(string[] args)
        {
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
        }

        protected override void OnStop()
        {
            scheduler.Shutdown();
        }
    }
}