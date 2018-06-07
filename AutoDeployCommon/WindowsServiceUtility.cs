using System;
using System.Collections;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using AutoDeployCommon;
using AutoDeployCommon.Models;
using Microsoft.Win32;

namespace Benlai.Application.AutoPublish.Common.Utility
{
    public class WindowServiceInstallUtility
    {

//        /// <summary>
//        /// 新建网站站点
//        /// </summary>
//        /// <param name="deployAppInfo">应用部署信息</param>
//        /// <param name="pai"></param>
//        public static bool InstallWinService(DeployAppInfo deployAppInfo, PublishAppInfo pai)
//        {
//            var serviceName = pai.AppSitePoolList.FirstOrDefault();
//            if (String.IsNullOrWhiteSpace(serviceName))
//            {
//                throw new Exception("deploy windows service error,the windows service program dir path is null or empty");
//            }
//            if (String.IsNullOrWhiteSpace(deployAppInfo.winsvr_programfullname))
//            {
//                throw new Exception("deploy windows service error,the windows service program file name is null or empty");
//            }
//            return InstallWinService(serviceName, Path.GetFullPath(Path.Combine(pai.AppPath, deployAppInfo.winsvr_programfullname)), Convert.ToInt32(deployAppInfo.winsvr_starttype));
//        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="serviceProgramFileFullPath"></param>
        /// <param name="windowServiceStartType"></param>
        /// <returns></returns>
        public static bool InstallWinService(string serviceName, string serviceProgramFileFullPath, int windowServiceStartType)
        {
            var app = AppDomain.CreateDomain($"InstallWinService_{serviceName}");

            System.Runtime.Remoting.ObjectHandle objLoader =
                app.CreateComInstanceFrom(System.Reflection.Assembly.GetExecutingAssembly().Location,  "Benlai.Application.AutoPublish.Common.Utility.WindowsServiceUtility");

            var loader = objLoader.Unwrap() as WindowsServiceUtility;

            var result = loader?.InstallWinService(serviceName,serviceProgramFileFullPath,windowServiceStartType);

            AppDomain.Unload(app);
            app = null;
            return result ?? false;
        }
    }

    public class WindowsServiceUtility
    {
        public static string StartWinService(string serviceName, int waitTime)
        {
            waitTime = Math.Max(waitTime, 1);
            try
            {
                var sc = new ServiceController(serviceName);
                if (sc.Status.Equals(ServiceControllerStatus.Stopped) ||
                    sc.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, waitTime));
                    sc.Refresh();
                }
            }
            catch (Exception e)
            {
                CommonUtility.ThreadWait(waitTime*1000);
                var sc = new ServiceController(serviceName);
                if (sc.Status.Equals(ServiceControllerStatus.Stopped) ||
                    sc.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, waitTime));
                    sc.Refresh();
                }
            }

            return "ok";
        }

        public static string StopWinService(string serviceName, int waitTime)
        {
            waitTime = Math.Max(waitTime, 1);
            var sc = new ServiceController(serviceName);
            if (sc.Status.Equals(ServiceControllerStatus.Running)|| sc.Status.Equals(ServiceControllerStatus.StartPending))
            {
                sc.Stop();
                //sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                sc.Refresh();
            }

            CommonUtility.ThreadWait(waitTime*1000);
            sc.Refresh();

            if (sc.Status.Equals(ServiceControllerStatus.Running) || sc.Status.Equals(ServiceControllerStatus.StartPending))
            {
                throw new Exception(serviceName + "该服务停止失败");
            }

            return "ok";
        }

        /// <summary>
        /// 安装windowservice
        /// </summary>
        /// <param name="serviceName">WindowService名称</param>
        /// <param name="serviceProgramFileFullPath">服务的执行程序完整路径</param>
        /// <param name="windowServiceStartType"></param>    
        /// <returns>成功返回 true,否则返回 false;</returns>
        public bool InstallWinService(string serviceName, string serviceProgramFileFullPath,int windowServiceStartType)
        {
            if (CheckWinServiceIsExisted(serviceName))
            {
                return true;
            }
            TransactedInstaller transactedInstaller = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceProgramFileFullPath, new string[] { });
            transactedInstaller.Installers.Add(assemblyInstaller);
            transactedInstaller.Install(new Hashtable());
            assemblyInstaller.Dispose();
            transactedInstaller.Dispose();
            ChangeWinServiceStartType(serviceName, windowServiceStartType);
            return CheckWinServiceIsExisted(serviceName);
        }

        /// <summary>
        /// 卸载windowservice
        /// </summary>
        /// <param name="serviceName">WindowService名称</param>
        /// <param name="serviceProgramFullPath">服务的执行程序完整路径</param>
        /// <returns>成功返回 true,否则返回 false;</returns>
        public bool UninstallWinService(string serviceName, string serviceProgramFullPath)
        {
            if (!CheckWinServiceIsExisted(serviceName))
            {
                return true;
            }
            string[] cmdline = { };
            TransactedInstaller transactedInstaller = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceProgramFullPath, cmdline);
            transactedInstaller.Installers.Add(assemblyInstaller);
            transactedInstaller.Uninstall(null);
            assemblyInstaller.Dispose();
            transactedInstaller.Dispose();
            return !CheckWinServiceIsExisted(serviceName);
        }

        /// <summary>  
        /// 检查服务存在的存在性  
        /// </summary>  
        /// <param name="serviceName">WindowService名称</param>
        /// <returns>存在返回 true,否则返回 false;</returns>  
        public static bool CheckWinServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Any(s => String.Equals(s.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>  
        /// 判断某个Windows服务是否启动  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsWinServiceStart(string serviceName)
        {
            ServiceController psc = new ServiceController(serviceName);
            bool bStartStatus = false;
            try
            {
                if (!psc.Status.Equals(ServiceControllerStatus.Stopped))
                {
                    bStartStatus = true;
                }

                return bStartStatus;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>    
        /// 修改服务的启动类型    
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="windowServiceStartType"></param>
        /// <returns></returns>    
        private void ChangeWinServiceStartType(string serviceName, int windowServiceStartType)
        {
            RegistryKey regist = Registry.LocalMachine;
            RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
            RegistryKey currentControlSet = sysReg?.OpenSubKey("CurrentControlSet");
            RegistryKey services = currentControlSet?.OpenSubKey("Services");
            RegistryKey registryKey = services?.OpenSubKey(serviceName, true);
            registryKey?.SetValue("Start", windowServiceStartType);
        }
    }
}