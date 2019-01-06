using System.ComponentModel;

namespace Jobs.Service.Installer
{
    [RunInstaller(true)]
    public partial class SchedulerInstaller : System.Configuration.Install.Installer
    {
        public SchedulerInstaller()
        {
            InitializeComponent();
        }
    }
}