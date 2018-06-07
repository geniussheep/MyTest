using System;
using System.DirectoryServices;
using System.Linq;
using System.Threading;
using Benlai.Common;
using Microsoft.Web.Administration;
using Microsoft.Win32;

namespace AutoDeployConsole
{
    public class IisDeployModel
    {

    }

    public enum OperateAppPoolType
    {
        Recycle = 0,
        Stop = 1,
        Start = 2
    }

    public enum NetVersion
    {
        V2 = 2,
        V4 = 4
    }

    public class IisUtility
    {
        private const string NetVersion2 = "v2.0";
        private const string NetVersion4 = "v4.0";

        public static string ApplicationHostConfigurationPath { get; set; }

        static IisUtility()
        {
            if (String.IsNullOrWhiteSpace(ApplicationHostConfigurationPath))
            {
                ApplicationHostConfigurationPath = @"C:\Windows\System32\inetsrv\config\applicationHost.config";
            }
        }

        /// <summary>
        /// 线程等待时间
        /// </summary>
        /// <param name="waitInterval">等待时间（毫秒）</param>
        private static void ThreadWait(int waitInterval)
        {
            using (var done = new AutoResetEvent(false))
            {
                done.WaitOne(waitInterval);
            }
        }

        /// <summary>
        /// 操作应用程序池 （回收、停止、重启）
        /// </summary>
        /// <param name="operateAppPoolType">操作类型</param>
        /// <param name="appPoolName">应用程序池名称</param>
        /// <param name="logInsance">日志实例</param>
        /// <returns></returns>
        private static bool OperateAppPool(OperateAppPoolType operateAppPoolType, string appPoolName, string logInsance)
        {
            LogInfoWriter.GetInstance(logInsance).Info($"{operateAppPoolType}AppPool info appName:{appPoolName}");
            var appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
            var findPool = appPool.Children.Find(appPoolName, "IIsApplicationPool");
            findPool.Invoke(operateAppPoolType.ToString(), null);
            appPool.CommitChanges();
            appPool.Close();
            return true;
        }

        /// <summary>
        /// 新建网站站点
        /// </summary>
        /// <param name="siteName">站点名</param>
        /// <param name="protocol">站点协议，如：http</param>
        /// <param name="bindingInformation">绑定的相关信息 "*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example></param> 
        /// <param name="physicalPath">物理路径</param>
        /// <param name="createAppPool">是否新建应用程序池</param>
        /// <param name="appPoolName">应用程序池名称</param>
        /// <param name="queueLength">队列长度</param>
        /// <param name="identityType">进程模型标识</param>
        /// <param name="idleTimeout">闲着超时时间(秒)</param>
        /// <param name="appPoolUserName">应用程序池特殊用户的用户名</param>
        /// <param name="appPoolPassword">应用程序池特殊用户的密码</param>
        /// <param name="maxProcesses">最大工作进程数</param>
        /// <param name="appPoolPipelineMode">应用程序池托管管道模式</param>
        /// <param name="managedRuntimeVersion">.net clr版本</param>
        /// <param name="rapidFailProtectionMaxCrashes">最大故障数</param>
        /// <param name="logDirectoryPath">IIS日志目录路径</param>
        /// <param name="logFormat">日志格式</param>
        /// <param name="logExtFileFlags">日志存储的字段</param>
        /// <param name="loggingRolloverPeriod">日志的存储计划</param>
        /// <param name="logTruncateSize">日志单个文件最大大小（MB） 最小为1MB<paramref name="loggingRolloverPeriod">LoggingRolloverPeriod.MaxSize</paramref> </param>
        public static void CreateSite(string siteName, string protocol, string bindingInformation, string physicalPath, bool createAppPool, string appPoolName, long queueLength, ProcessModelIdentityType identityType, long idleTimeout, string appPoolUserName, string appPoolPassword, long maxProcesses, ManagedPipelineMode appPoolPipelineMode, string managedRuntimeVersion, long rapidFailProtectionMaxCrashes, string logDirectoryPath, LogFormat logFormat, LogExtFileFlags logExtFileFlags, LoggingRolloverPeriod loggingRolloverPeriod, long logTruncateSize)
        {
            if (logTruncateSize < 1)
            {
                throw new Exception("日志单个文件最大大小的值必须>=1MB");
            }
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                if (site == null)
                {
                    site = mgr.Sites.Add(siteName, protocol, bindingInformation, physicalPath);
                    site.LogFile.Enabled = true;
                    site.ServerAutoStart = true;
                    site.LogFile.Directory = logDirectoryPath;
                    site.LogFile.Period = loggingRolloverPeriod;
                    site.LogFile.LogExtFileFlags = logExtFileFlags;
                    site.LogFile.TruncateSize = logTruncateSize * 1024 * 1024;
                    site.LogFile.LogFormat = logFormat;

                    if (createAppPool)
                    {
                        var pool = mgr.ApplicationPools.Add(siteName);
                        pool.Name = appPoolName;
                        pool.ManagedRuntimeVersion = managedRuntimeVersion;
                        pool.QueueLength = queueLength;
                        pool.ProcessModel.MaxProcesses = maxProcesses;
                        if (pool.ProcessModel.IdentityType != identityType)
                        {
                            pool.ProcessModel.IdentityType = identityType;
                        }
                        pool.ProcessModel.IdleTimeout = TimeSpan.FromSeconds(idleTimeout);
                        if (!String.IsNullOrEmpty(appPoolUserName))
                        {
                            pool.ProcessModel.UserName = appPoolUserName;
                            pool.ProcessModel.Password = appPoolPassword;
                        }
                        if (pool.ManagedPipelineMode != appPoolPipelineMode)
                        {
                            pool.ManagedPipelineMode = appPoolPipelineMode;
                        }
                        pool.Failure.RapidFailProtectionMaxCrashes = rapidFailProtectionMaxCrashes;
                        mgr.Sites[siteName].Applications[0].ApplicationPoolName = pool.Name;
                    }
                    mgr.CommitChanges();
                }
                else
                {
                    throw new Exception($"the web site:{siteName} is already exist");
                }
            }
        }

        /// <summary> 
        /// 新建网站站点 
        /// </summary> 
        /// <param name="siteName">站点名</param> 
        /// <param name="bindingInfo">绑定的相关信息 "*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example></param> 
        /// <param name="physicalPath">物理路径</param>
        /// <param name="queueLength">队列长度</param>
        /// <param name="maxProcesses">最大工作进程数</param>
        /// <param name="netVersion">.net clr版本</param>
        /// <param name="appPoolPipelineMode">应用程序池托管管道模式</param>
        /// <param name="rapidFailProtectionMaxCrashes">最大故障数</param>
        /// <param name="logDirectoryPath">IIS日志目录路径</param>
        /// <param name="logFormat">日志格式</param>
        /// <param name="logExtFileFlags">日志存储的字段</param>
        /// <param name="loggingRolloverPeriod">日志的存储计划</param>
        /// <param name="logTruncateSize">日志单个文件最大大小（字节）</param>
        public static void CreateSite(string siteName, string bindingInfo, string physicalPath, long queueLength, long maxProcesses, NetVersion netVersion, ManagedPipelineMode appPoolPipelineMode,long rapidFailProtectionMaxCrashes, string logDirectoryPath, LogFormat logFormat, LogExtFileFlags logExtFileFlags, LoggingRolloverPeriod loggingRolloverPeriod, long logTruncateSize)
        {
            var managedRuntimeVersion = netVersion == NetVersion.V2 ? NetVersion2 : NetVersion4;
            CreateSite(siteName, "http", bindingInfo, physicalPath, true, siteName, queueLength, ProcessModelIdentityType.ApplicationPoolIdentity, 120, null, null, maxProcesses, appPoolPipelineMode, managedRuntimeVersion, rapidFailProtectionMaxCrashes, logDirectoryPath, logFormat, logExtFileFlags, loggingRolloverPeriod, logTruncateSize);
        }

        /// <summary> 
        /// 新建网站站点 
        /// </summary> 
        /// <param name="siteName">站点名</param>
        /// <param name="ip">ip 空字符串则为则直接默认 * </param>
        /// <param name="port">端口</param>
        /// <param name="hostName">主机名</param>
        /// <param name="physicalPath">物理路径</param>
        /// <param name="queueLength">队列长度</param>
        /// <param name="maxProcesses">最大工作进程数</param>
        /// <param name="netVersion">.net clr版本</param>
        /// <param name="appPoolPipelineMode">应用程序池托管管道模式</param>
        /// <param name="rapidFailProtectionMaxCrashes">最大故障数</param>
        /// <param name="logDirectoryPath">IIS日志目录路径</param>
        /// <param name="logFormat">日志格式</param>
        /// <param name="logExtFileFlags">日志存储的字段</param>
        /// <param name="loggingRolloverPeriod">日志的存储计划</param>
        /// <param name="logTruncateSize">日志单个文件最大大小（字节）</param>
        public static void CreateSite(string siteName,string ip, int port, string hostName, string physicalPath, long queueLength, long maxProcesses, NetVersion netVersion, ManagedPipelineMode appPoolPipelineMode, long rapidFailProtectionMaxCrashes, string logDirectoryPath, LogFormat logFormat, LogExtFileFlags logExtFileFlags, LoggingRolloverPeriod loggingRolloverPeriod, long logTruncateSize)
        {
            ip = String.IsNullOrWhiteSpace(ip) ? "*" : ip;
            CreateSite(siteName, $"{ip}:{port}:{hostName}", physicalPath, queueLength, maxProcesses, netVersion, appPoolPipelineMode,rapidFailProtectionMaxCrashes, logDirectoryPath, logFormat, logExtFileFlags, loggingRolloverPeriod, logTruncateSize);
        }

        /// <summary> 
        /// 删除站点 
        /// </summary> 
        /// <param name="siteName">站点名</param>
        /// <param name="deleteAppPool">是否同时删除应用池</param> 
        public static void DeleteSite(string siteName, bool deleteAppPool = true)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                if (site == null) return;
                if (deleteAppPool)
                {
                    var pool = mgr.ApplicationPools[siteName];
                    if (pool != null)
                    {
                        mgr.ApplicationPools.Remove(pool);
                    }
                }
                mgr.Sites.Remove(site);
                mgr.CommitChanges();
            }
        }


        /// <summary> 
        /// 删除应用程序池
        /// </summary> 
        /// <param name="appPoolName">应用程序池名称</param> 
        public static void DeleteAppPool(string appPoolName)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var pool = mgr.ApplicationPools[appPoolName];
                if (pool == null) return;
                mgr.ApplicationPools.Remove(pool);
                mgr.CommitChanges();
            }
        }


        /// <summary>
        /// 重启应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="waitTime">等待时间</param>
        /// <param name="logInsance">日志输出名</param>
        /// <returns></returns>
        public static bool RestartAppPool(string appPoolName, int waitTime, string logInsance)
        {
            try
            {
                return RecycleAppPool(appPoolName, logInsance) && StopAppPool(appPoolName, logInsance) &&
                       StartAppPool(appPoolName, logInsance);
            }
            catch (Exception e)
            {
                LogInfoWriter.GetInstance(logInsance).Error($"RestartAppPool error and ReTry appName:{appPoolName}\r\n{e}");

                ThreadWait(waitTime*1000);
                try
                {
                    StopAppPool(appPoolName, logInsance);
                    return StartAppPool(appPoolName, logInsance);
                }
                catch (Exception ex)
                {
                    LogInfoWriter.GetInstance(logInsance).Error($"StopAppPool error and TryStart appName:{appPoolName}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// 停止应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInsance">日志输出名</param>
        /// <returns></returns>
        public static bool StopAppPool(string appPoolName, string logInsance)
        {
            return OperateAppPool(OperateAppPoolType.Stop, appPoolName, logInsance);
        }

        /// <summary>
        /// 启动应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInsance">日志输出名</param>
        /// <returns></returns>
        public static bool StartAppPool(string appPoolName, string logInsance)
        {
            return OperateAppPool(OperateAppPoolType.Start, appPoolName, logInsance);
        }

        /// <summary>
        /// 回收应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInsance">日志输出名</param>
        /// <returns></returns>
        public static bool RecycleAppPool(string appPoolName, string logInsance)
        {
            return OperateAppPool(OperateAppPoolType.Recycle, appPoolName, logInsance);
        }

        /// <summary>
        /// 在站点上添加默认文档名
        /// </summary>
        /// <param name="siteName">站点名</param>
        /// <param name="defaultDocName">默认文档</param>
        public static void AddDefaultDocument(string siteName, string defaultDocName)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                Configuration cfg = mgr.GetWebConfiguration(siteName);
                ConfigurationSection defaultDocumentSection = cfg.GetSection("system.webServer/defaultDocument");
                ConfigurationElement filesElement = defaultDocumentSection.GetChildElement("files");
                ConfigurationElementCollection filesCollection = filesElement.GetCollection();

                if (filesCollection.Any(elt => elt.Attributes["value"].Value.ToString() == defaultDocName))
                {
                    return;
                }

                try
                {
                    ConfigurationElement docElement = filesCollection.CreateElement();
                    docElement.SetAttributeValue("value", defaultDocName);
                    filesCollection.Add(docElement);
                }
                catch (Exception)
                {
                    // ignored
                    //this will fail if existing 
                }

                mgr.CommitChanges();
            }
        }

        /// <summary>
        /// 新建虚拟目录
        /// </summary>
        /// <param name="siteName">站点名</param> 
        /// <param name="vDirName">虚拟目录名</param>
        /// <param name="physicalPath">物理路径</param>
        public static void CreateVDir(string siteName, string vDirName, string physicalPath)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                if (site == null)
                {
                    throw new ApplicationException($"Web site {siteName} does not exist");
                }
                site.Applications.Add("/" + vDirName, physicalPath);
                mgr.CommitChanges();
            }
        }

        /// <summary>
        /// 删除虚拟目录
        /// </summary>
        /// <param name="siteName">站点名</param> 
        /// <param name="vDirName">虚拟目录名</param>
        public static void DeleteVDir(string siteName, string vDirName)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                var app = site?.Applications["/" + vDirName];
                if (app == null) return;
                site.Applications.Remove(app);
                mgr.CommitChanges();
            }
        }

        /// <summary>
        /// 检查虚拟目录是否存在
        /// </summary>
        /// <param name="siteName">站点名</param>
        /// <param name="path">虚拟目录路径</param>
        /// <returns>bool</returns>
        public static bool VerifyVirtualPathIsExist(string siteName, string path)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                if (site == null) return false;
                if (site.Applications.Any(app => app.Path.ToUpper().Equals(path.ToUpper())))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 检查站点是否存在
        /// </summary>
        /// <param name="siteName">站点名</param>
        /// <returns>bool</returns>
        public static bool VerifyWebSiteIsExist(string siteName)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                if (mgr.Sites.Any(site => site.Name.ToUpper().Equals(siteName.ToUpper())))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取IIS版本
        /// </summary>
        /// <returns>IIS版本 version</returns>
        public Version GetIisVersion()
        {
            using (var componentsKey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (componentsKey == null) return new Version(0, 0);
                var majorVersion = (int) componentsKey.GetValue("MajorVersion", -1);
                var minorVersion = (int) componentsKey.GetValue("MinorVersion", -1);

                if (majorVersion != -1 && minorVersion != -1)
                {
                    return new Version(majorVersion, minorVersion);
                }

                return new Version(0, 0);
            }
        }
    }
}
