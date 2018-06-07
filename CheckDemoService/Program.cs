using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace CheckDemoService
{
    class Program
    {
        private const string serviceName = "DemoTestService";

        static void Main(string[] args)
        {
            Console.Read();

            Console.WriteLine(GetServiceInstallPath("serviceName"));

            Console.WriteLine("停止服务。。。");
            Console.WriteLine("停止：" + ServiceUtility.StopWinService(serviceName, 90));
            Console.WriteLine("停止服务 end");
            Console.WriteLine("启动服务。。。");
            Console.WriteLine("启动：" + ServiceUtility.StartWinService(serviceName, 90));
            Console.WriteLine("停止服务 end");
            //var MachineName = sc.MachineName;
            //string registryPath = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            //RegistryKey keyHKLM = Registry.LocalMachine;

            //RegistryKey key;
            //if (MachineName != "")
            //{
            //    key = RegistryKey.OpenRemoteBaseKey
            //      (RegistryHive.LocalMachine, MachineName).OpenSubKey(registryPath);
            //}
            //else
            //{
            //    key = keyHKLM.OpenSubKey(registryPath);
            //}

            //string value = key.GetValue("ImagePath").ToString();
            //key.Close();
            //return ExpandEnvironmentVariables(value);
            Console.Read();
        }

        private static string GetServiceInstallPath(string serviceName)
        {
            RegistryKey regkey;
            regkey = Registry.LocalMachine.OpenSubKey(string.Format(@"SYSTEM\CurrentControlSet\services\{0}", serviceName));

            if (regkey.GetValue("ImagePath") == null)
                return "Not Found";
            else
                return regkey.GetValue("ImagePath").ToString();
        }
    }
}
