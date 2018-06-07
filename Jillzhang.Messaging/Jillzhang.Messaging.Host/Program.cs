using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Jillzhang.Messaging.Service;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Uri tcpAddress = new Uri("net.tcp://localhost:6987/Service");  
                ServiceHost onewayHost = new ServiceHost(typeof(OneWayJob), tcpAddress);
                ServiceHost normalHost = new ServiceHost(typeof(NormalJob), tcpAddress);
                ServiceHost duplexHost = new ServiceHost(typeof(Job), tcpAddress);
                onewayHost.Open();
                duplexHost.Open();
                normalHost.Open();
               Console.WriteLine("服务已经启动！");    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.Read();
        }
    }
}
