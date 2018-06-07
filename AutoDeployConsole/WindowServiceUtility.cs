using System;
using System.Collections;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Win32;

namespace AutoDeployConsole
{
    class WindowServiceUtility
    {
        private static void ThreadWait(int waitInterval)
        {
            using (AutoResetEvent done = new AutoResetEvent(false))
            {
                done.WaitOne(waitInterval);
            }
        }

        /// <summary>
        /// 启动指定WindowService
        /// </summary>
        /// <param name="serviceName">WindowService名称</param>
        /// <param name="waitTime">等待服务启动时间（秒）</param>
        /// <returns>是否成功 成功返回“ok”</returns>
        public static string StartService(string serviceName, int waitTime)
        {
            try
            {
                ServiceController service = new ServiceController(serviceName);
                if (service.Status.Equals(ServiceControllerStatus.Stopped) ||
                    service.Status.Equals(ServiceControllerStatus.StopPending))
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, waitTime));
                    service.Refresh();
                }
            }
            catch (Exception)
            {
                ThreadWait(waitTime*1000);
                ServiceController service = new ServiceController(serviceName);
                if (service.Status != ServiceControllerStatus.Running &&
                    service.Status != ServiceControllerStatus.StartPending)
                    return "ok";
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, waitTime));
                service.Refresh();
            }
            return "ok";
        }

        public static string StopService(string serviceName, int waitTime)
        {
            ServiceController service = new ServiceController(serviceName);
            if (service.Status.Equals(ServiceControllerStatus.Running))
            {
                service.Stop();
                //sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                service.Refresh();
            }

            ThreadWait(waitTime * 1000);
            service.Refresh();

            if (service.Status.Equals(ServiceControllerStatus.Running))
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
        /// <returns>成功返回 true,否则返回 false;</returns>
        public static bool InstallService(string serviceName, string serviceProgramFileFullPath)
        {
            if (IsServiceIsExisted(serviceName))
            {
                return true;
            }
            TransactedInstaller transactedInstaller = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceProgramFileFullPath, new string[] {});
            transactedInstaller.Installers.Add(assemblyInstaller);
            transactedInstaller.Install(new Hashtable());
            assemblyInstaller.Dispose();
            transactedInstaller.Dispose();
            return IsServiceIsExisted(serviceName);
        }

        /// <summary>
        /// 卸载windowservice
        /// </summary>
        /// <param name="serviceName">WindowService名称</param>
        /// <param name="serviceProgramFullPath">服务的执行程序完整路径</param>
        /// <returns>成功返回 true,否则返回 false;</returns>
        public static bool UninstallService(string serviceName, string serviceProgramFullPath)
        {
            if (!IsServiceIsExisted(serviceName))
            {
                return true;
            }
            string[] cmdline = {};
            TransactedInstaller transactedInstaller = new TransactedInstaller();
            AssemblyInstaller assemblyInstaller = new AssemblyInstaller(serviceProgramFullPath, cmdline);
            transactedInstaller.Installers.Add(assemblyInstaller);
            transactedInstaller.Uninstall(null);
            assemblyInstaller.Dispose();
            transactedInstaller.Dispose();
            return !IsServiceIsExisted(serviceName);
        }

        /// <summary>  
        /// 检查服务存在的存在性  
        /// </summary>  
        /// <param name="serviceName">WindowService名称</param>
        /// <returns>存在返回 true,否则返回 false;</returns>  
        public static bool IsServiceIsExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            return services.Any(s => String.Equals(s.ServiceName, serviceName, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>  
        /// 判断某个Windows服务是否启动  
        /// </summary>  
        /// <returns></returns>  
        public static bool IsServiceStart(string serviceName)
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
        /// <param name="windowServiceStartType"></param>    
        /// <param name="serviceName"></param>    
        /// <returns></returns>    
        public static bool ChangeServiceStartType(WindowServiceStartType windowServiceStartType, string serviceName)
        {
            try
            {
                RegistryKey regist = Registry.LocalMachine;
                RegistryKey sysReg = regist.OpenSubKey("SYSTEM");
                RegistryKey currentControlSet = sysReg?.OpenSubKey("CurrentControlSet");
                RegistryKey services = currentControlSet?.OpenSubKey("Services");
                RegistryKey registryKey = services?.OpenSubKey(serviceName, true);
                registryKey?.SetValue("Start", (int)windowServiceStartType);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// WinService启动类型
    /// </summary>
    public enum WindowServiceStartType
    {
        /// <summary>
        /// 自动（延时启动）
        /// </summary>
        AutomaticDelayedStart = 1,

        /// <summary>
        /// 自动
        /// </summary>
        Automatic = 2,

        /// <summary>
        /// 手动
        /// </summary>
        Manual = 3,

        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 4
    }
}
