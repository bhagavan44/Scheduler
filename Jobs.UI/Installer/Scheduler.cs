using System.Collections.Specialized;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;
using Jobs.UI.Job;
using Quartz;
using Quartz.Impl;

namespace Jobs.UI.Installer
{
    internal partial class Scheduler : ServiceBase
    {
        private readonly IScheduler scheduler;

        public Scheduler()
        {
            InitializeComponent();
            var config = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            scheduler = new StdSchedulerFactory(config).GetScheduler().Result;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            scheduler.Start();

            IJobDetail jobDetail = JobBuilder.Create<Job1>().WithIdentity("Job1", "Group1").StoreDurably(true).Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("myTrigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .WithMisfireHandlingInstructionIgnoreMisfires()
                    .RepeatForever())
                  .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
        }

        protected override void OnStop()
        {
            CloseScheduler().Wait();
            base.OnStop();
            this.Dispose();
        }

        protected override void OnShutdown()
        {
            CloseScheduler().Wait();
            base.OnShutdown();
            this.Dispose();
        }

        private async Task CloseScheduler()
        {
            var executingJobs = await scheduler.GetCurrentlyExecutingJobs();
            foreach (IJobExecutionContext job in executingJobs)
            {
                await scheduler.Interrupt(job.JobDetail.Key);
                await scheduler.UnscheduleJob(job.Trigger.Key);
                await scheduler.DeleteJob(job.JobDetail.Key);
            }
            await scheduler.Clear();
            await scheduler.Shutdown();
        }
    }
}