using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Jobs.UI.Services;
using Quartz;

namespace Jobs.UI.Job
{
    [DisallowConcurrentExecution]
    internal class Job1 : ICustomJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await Task.Run(() => WriteToFile("Service is recall at " + DateTime.Now));
        }

        public void WriteToFile(string message)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(message);
                }
            }
            Thread.Sleep(TimeSpan.FromSeconds(8));
        }
    }
}