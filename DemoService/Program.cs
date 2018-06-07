using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new Service1()
            //};
            //ServiceBase.Run(ServicesToRun);



            #region 服务直接运行
            if (args.Length == 0)
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new DemoService() };
                ServiceBase.Run(ServicesToRun);
            }
            #endregion
            #region 调试启动
            else if (args[0].ToLower() == "/d" || args[0].ToLower() == "-d")
            {
                DemoMain.Start();
                while (true)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            #endregion
            #region 安装服务
            else if (args[0].ToLower() == "/i" || args[0].ToLower() == "-i")
            {
                try
                {
                    string[] cmdline = { };
                    string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    TransactedInstaller transactedInstaller = new TransactedInstaller();
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);
                    transactedInstaller.Installers.Add(assemblyInstaller);
                    transactedInstaller.Install(new System.Collections.Hashtable());
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            #endregion
            #region 删除服务
            else if (args[0].ToLower() == "/u" || args[0].ToLower() == "-u")
            {
                try
                {
                    string[] cmdline = { };
                    string serviceFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    TransactedInstaller transactedInstaller = new TransactedInstaller();
                    AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceFileName, cmdline);
                    transactedInstaller.Installers.Add(assemblyInstaller);
                    transactedInstaller.Uninstall(null);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
            #endregion

        }
    }
}
