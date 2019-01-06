using System.ServiceProcess;
using Jobs.Service.Installer;

namespace Jobs.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var servicesToRun = new ServiceBase[]
            {
                new Scheduler()
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}