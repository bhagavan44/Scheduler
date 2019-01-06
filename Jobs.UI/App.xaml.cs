using System.ServiceProcess;
using System.Windows;
using Jobs.UI.Installer;

namespace Jobs.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                var service = new Scheduler();
                var servicesToRun = new ServiceBase[]
                {
                    service
                };
                ServiceBase.Run(servicesToRun);
                service.Disposed += Service_Disposed;
            }
            else
            {
                var window = new MainWindow();
                window.Show();
                window.Closing += Window_Closing;
            }
        }

        private void Service_Disposed(object sender, System.EventArgs e)
        {
            this.Shutdown(0);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Shutdown(0);
        }
    }
}