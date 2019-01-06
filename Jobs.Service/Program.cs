using System.ServiceProcess;

namespace Jobs.Service
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Scheduler()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}