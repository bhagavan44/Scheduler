using System;
using Quartz;

namespace Jobs.UI.Models
{
    internal class JobModel
    {
        public string Id { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }
        public string TriggerKey { get; set; }
        public string TiggerGroup { get; set; }
        public string TriggerType { get; set; }
        public TriggerState State { get; set; }
        public DateTimeOffset? NextRunTime { get; set; }
        public DateTimeOffset? LastRunTime { get; set; }
    }
}