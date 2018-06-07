using System;
using System.ServiceProcess;
using System.Threading;
using Benlai.Configuration.Install;

namespace Benlai.Application.AutoPublish.AppStart
{
    public class SOAServiceHost : ServiceHost
    {
        protected override void RunAsConsole(string[] args)
        {
            var exitEvent = new ManualResetEvent(false);

            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                exitEvent.Set();
            };

            AppServer.Instance.Start();

            exitEvent.WaitOne();

            AppServer.Instance.Stop();

            Console.WriteLine("press any key to quit.");
            Console.ReadKey();
        }

        protected override void RunAsService(string[] args)
        {
            ServiceBase.Run(new ServiceBase[] { new Install.MainService() });
        }
    }
}