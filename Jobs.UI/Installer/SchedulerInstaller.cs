using System.Collections;
using System.ComponentModel;

namespace Jobs.UI.Installer
{
    [RunInstaller(true)]
    public partial class SchedulerInstaller : System.Configuration.Install.Installer
    {
        public SchedulerInstaller()
        {
            InitializeComponent();
        }

        protected override void OnBeforeInstall(IDictionary savedState)
        {
            const string parameter = "-t service";
            Context.Parameters["assemblypath"] = "\"" + Context.Parameters["assemblypath"] + "\"" + parameter;
            base.OnBeforeInstall(savedState);
        }
    }
}