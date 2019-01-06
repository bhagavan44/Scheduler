using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Jobs.UI.Models;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace Jobs.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly IScheduler scheduler;

        public MainWindow()
        {
            InitializeComponent();

            var config = (NameValueCollection)ConfigurationManager.GetSection("quartz");
            scheduler = new StdSchedulerFactory(config).GetScheduler().Result;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = GetJobs();
            data.ContinueWith(d =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    ListView.ItemsSource = d.Result;
                }), DispatcherPriority.Background);
            });
        }

        private async Task<IList<JobModel>> GetJobs()
        {
            IList<JobModel> jobs = new List<JobModel>();
            var groups = await scheduler.GetJobGroupNames();
            foreach (var group in groups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = await scheduler.GetJobKeys(groupMatcher);
                foreach (JobKey jobKey in jobKeys)
                {
                    IJobDetail detail = await scheduler.GetJobDetail(jobKey);
                    var triggers = await scheduler.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        jobs.Add(new JobModel()
                        {
                            Id = jobKey.Name,
                            Group = jobKey.Group,
                            Description = detail.Description,
                            TriggerKey = trigger.Key.Name,
                            TiggerGroup = trigger.Key.Group,
                            TriggerType = trigger.GetType().Name,
                            State = await scheduler.GetTriggerState(trigger.Key),
                            NextRunTime = trigger.GetNextFireTimeUtc(),
                            LastRunTime = trigger.GetPreviousFireTimeUtc()
                        });
                    }
                }
            }
            return jobs;
        }
    }
}