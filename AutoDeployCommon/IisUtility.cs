using System;
using System.DirectoryServices;
using AutoDeployCommon.Constant;
using AutoDeployCommon.Models;
using Microsoft.Web.Administration;

namespace AutoDeployCommon
{
    public class IisUtility
    {
        public static string ApplicationHostConfigurationPath { get; set; }

        static IisUtility()
        {
            if (String.IsNullOrWhiteSpace(ApplicationHostConfigurationPath))
            {
                ApplicationHostConfigurationPath = @"C:\Windows\System32\inetsrv\config\applicationHost.config";
            }
        }

        /// <summary>
        /// 操作应用程序池 （回收、停止、重启）
        /// </summary>
        /// <param name="operateAppPoolType">操作类型</param>
        /// <param name="appPoolName">应用程序池名称</param>
        /// <param name="logInstanceName">日志实例</param>
        /// <returns></returns>
        private static bool OperateAppPool(OperateAppPoolType operateAppPoolType, string appPoolName,
            string logInstanceName)
        {
            //LogInfoWriter.GetInstance(logInstanceName).Info($"{operateAppPoolType}AppPool info appName:{appPoolName}");
//            var appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
//            var findPool = appPool.Children.Find(appPoolName, "IIsApplicationPool");
//            findPool.Invoke(operateAppPoolType.ToString(), null);
//            appPool.CommitChanges();
//            appPool.Close();
            try
            {
                using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
                {
                    var pool = mgr.ApplicationPools[appPoolName];
                    if (pool == null)
                    {
                        throw new Exception($"the {operateAppPoolType} AppPool is not exist!");
                    }
                    switch (operateAppPoolType)
                    {
                        case OperateAppPoolType.Recycle:
                            if (pool.State == ObjectState.Started|| pool.State == ObjectState.Starting)
                            {
                                pool.Recycle();
                            }
                            break;
                        case OperateAppPoolType.Start:
                            pool.Start();
                            break;
                        case OperateAppPoolType.Stop:
                            if (pool.State == ObjectState.Started || pool.State == ObjectState.Starting)
                            {
                                pool.Stop();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            return true;
        }

//        public static string RestartAppPool(string appPoolName, int waitTime, string logInstanceName)
//        {
//            waitTime = Math.Max(waitTime, 1);
//            try
//            {
//                var m = RecycleAppPool(appPoolName,logInstanceName);
//                m = StopAppPool(appPoolName, logInstanceName);
//                m = StartAppPool(appPoolName, logInstanceName);
//
//                return m;
//            }
//            catch (Exception e)
//            {
//                LogInfoWriter.GetInstance(logInstanceName).Error($"RestartAppPool error and ReTry appName:{appPoolName}" + "\r\n" + e);
//
//                CommonUtility.ThreadWait(waitTime*1000);
//                try
//                {
//                    StopAppPool(appPoolName, logInstanceName);
//                    return StartAppPool(appPoolName, logInstanceName);
//                }
//                catch (Exception ex)
//                {
//                    LogInfoWriter.GetInstance(logInstanceName).Error($"StopAppPool error and TryStart appName:{appPoolName}" + "\r\n" + ex);
//                    throw ex;
//                }
//            }
//        }
//
//        public static string StopAppPool(string appPoolName, string logInstanceName)
//        {
//            var method = "Stop";
//
//            LogInfoWriter.GetInstance(logInstanceName).Info($"StopAppPool info appPoolName:{appPoolName}");
//
//            var appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
//            var findPool = appPool.Children.Find(appPoolName, "IIsApplicationPool");
//            findPool.Invoke(method, null);
//            appPool.CommitChanges();
//            appPool.Close();
//            return "ok";
//        }
//
//        public static string StartAppPool(string appPoolName, string logInstanceName)
//        {
//            //如果应用程序池不存在,则会报错系统找不到指定路径
//            var method = "Start";
//
//            LogInfoWriter.GetInstance(logInstanceName).Info($"StartAppPool info appPoolName:{appPoolName}");
//
//            var appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
//            var findPool = appPool.Children.Find(appPoolName, "IIsApplicationPool");
//            findPool.Invoke(method, null);
//            appPool.CommitChanges();
//            appPool.Close();
//            return "ok";
//        }
//
//        public static string RecycleAppPool(string appPoolName, string logInstanceName)
//        {
//            //如果应用程序池当前状态为停止,则会发生异常报错
//            var method = "Recycle";
//
//            LogInfoWriter.GetInstance(logInstanceName).Info($"RecycleAppPool info appPoolName:{appPoolName}");
//
//            var appPool = new DirectoryEntry("IIS://localhost/W3SVC/AppPools");
//            var findPool = appPool.Children.Find(appPoolName, "IIsApplicationPool");
//            findPool.Invoke(method, null);
//            appPool.CommitChanges();
//            appPool.Close();
//            return "ok";
//        }

        /// <summary>
        /// 重启应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="waitTime">等待时间</param>
        /// <param name="logInstanceName">日志输出名</param>
        /// <returns></returns>
        public static bool RestartAppPool(string appPoolName, int waitTime, string logInstanceName)
        {
            try
            {
                return RecycleAppPool(appPoolName, logInstanceName) && 
                       StopAppPool(appPoolName, logInstanceName) &&
                       StartAppPool(appPoolName, logInstanceName);
            }
            catch (Exception e)
            {
                //LogInfoWriter.GetInstance(logInstanceName).Error($"RestartAppPool error and ReTry appName:{appPoolName}\r\n{e}");

                CommonUtility.ThreadWait(waitTime * 1000);
                try
                {
                    StopAppPool(appPoolName, logInstanceName);
                    return StartAppPool(appPoolName, logInstanceName);
                }
                catch (Exception ex)
                {
                    //LogInfoWriter.GetInstance(logInstanceName).Error($"StopAppPool error and TryStart appName:{appPoolName}\r\n{ex}");
                    throw;
                }
            }
        }

        /// <summary>
        /// 停止应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInstanceName">日志输出名</param>
        /// <returns></returns>
        public static bool StopAppPool(string appPoolName, string logInstanceName)
        {
            return OperateAppPool(OperateAppPoolType.Stop, appPoolName, logInstanceName);
        }

        /// <summary>
        /// 启动应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInstanceName">日志输出名</param>
        /// <returns></returns>
        public static bool StartAppPool(string appPoolName, string logInstanceName)
        {
            return OperateAppPool(OperateAppPoolType.Start, appPoolName, logInstanceName);
        }

        /// <summary>
        /// 回收应用池
        /// </summary>
        /// <param name="appPoolName">应用程序池名称</param> 
        /// <param name="logInstanceName">日志输出名</param>
        /// <returns></returns>
        public static bool RecycleAppPool(string appPoolName, string logInstanceName)
        {
            return OperateAppPool(OperateAppPoolType.Recycle, appPoolName, logInstanceName);
        }

        /// <summary>
        /// 新建网站站点
        /// </summary>
        /// <param name="iisDeployInfo">iis部署信息</param>
        public static bool CreateSite(IisDeployInfo iisDeployInfo)
        {
            if (iisDeployInfo.LoggingRolloverPeriod == LoggingRolloverPeriod.MaxSize &&iisDeployInfo.LogTruncateSize < 1)
            {
                throw new Exception("日志单个文件最大大小的值必须>=1KB");
            }
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[iisDeployInfo.SiteName];
                if (site == null)
                {
                    site = mgr.Sites.Add(iisDeployInfo.SiteName, iisDeployInfo.Protocol, iisDeployInfo.BindingInformation, iisDeployInfo.PhysicalPath);
                    site.LogFile.Enabled = true;
                    site.ServerAutoStart = true;
                    site.LogFile.Directory = iisDeployInfo.LogDirectoryPath;
                    site.LogFile.Period = iisDeployInfo.LoggingRolloverPeriod;
                    site.LogFile.LogExtFileFlags = iisDeployInfo.LogExtFileFlags;
                    if (site.LogFile.Period == LoggingRolloverPeriod.MaxSize)
                    {
                        site.LogFile.TruncateSize = iisDeployInfo.LogTruncateSize*1024;
                    }
                    site.LogFile.LogFormat = iisDeployInfo.LogFormat;

                    if (iisDeployInfo.CreateAppPool)
                    {
                        var pool = mgr.ApplicationPools[iisDeployInfo.SiteName] ??
                                   mgr.ApplicationPools.Add(iisDeployInfo.SiteName);
                        pool.Name = iisDeployInfo.AppPoolName;
                        pool.ManagedRuntimeVersion = iisDeployInfo.ManagedRuntimeVersion;
                        pool.QueueLength = iisDeployInfo.QueueLength;
                        pool.ProcessModel.MaxProcesses = iisDeployInfo.MaxProcesses;
                        if (pool.ProcessModel.IdentityType != iisDeployInfo.IdentityType)
                        {
                            pool.ProcessModel.IdentityType = iisDeployInfo.IdentityType;
                        }
                        pool.ProcessModel.IdleTimeout = TimeSpan.FromMinutes(iisDeployInfo.IdleTimeout);
                        if (!String.IsNullOrEmpty(iisDeployInfo.AppPoolUserName))
                        {
                            pool.ProcessModel.UserName = iisDeployInfo.AppPoolUserName;
                            pool.ProcessModel.Password = iisDeployInfo.AppPoolPassword;
                        }
                        if (pool.ManagedPipelineMode != iisDeployInfo.AppPoolPipelineMode)
                        {
                            pool.ManagedPipelineMode = iisDeployInfo.AppPoolPipelineMode;
                        }
                        pool.Failure.RapidFailProtectionMaxCrashes = iisDeployInfo.RapidFailProtectionMaxCrashes;
                        mgr.Sites[iisDeployInfo.SiteName].Applications[0].ApplicationPoolName = pool.Name;
                    }
                    mgr.CommitChanges();
                }
                site = mgr.Sites[iisDeployInfo.SiteName];
                return site != null;
//                else
//                {
//                    throw new Exception($"the web site:{iisDeployInfo.SiteName} is already exist");
//                }
            }
        }

        /// <summary> 
        /// 删除站点 
        /// </summary> 
        /// <param name="siteName">站点名</param>
        /// <param name="deleteAppPool">是否同时删除应用池</param> 
        public static bool DeleteSite(string siteName, bool deleteAppPool = true)
        {
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                if (site == null) return true;
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
                site = mgr.Sites[siteName];
                return site == null;
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
        /// 新建网站站点
        /// </summary>
        /// <param name="siteName">iis部署信息</param>
        public static bool CheckSiteIsExisted(string siteName)
        {
            if (String.IsNullOrWhiteSpace(siteName))
            {
                throw new Exception("check web site is existed error, the web site name is null or empty");
            }
            using (var mgr = new ServerManager(ApplicationHostConfigurationPath))
            {
                var site = mgr.Sites[siteName];
                return site != null;
            }
        }
    }
}