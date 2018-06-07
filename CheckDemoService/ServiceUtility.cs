using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.DirectoryServices;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CheckDemoService
{
    class ServiceUtility
    {
        public static bool ThreadWait(int waitTime,Func<bool> checkStatus)
        {
            if (waitTime == 0) return true;

            var maxRetryCount = waitTime;

            var retryCount = 0;

            while (checkStatus != null && !checkStatus())
            {
                using (AutoResetEvent done = new AutoResetEvent(false))
                {
                    Console.WriteLine("尝试次数：{0}", retryCount);
                    done.WaitOne(waitTime);
                }
                if (++retryCount >= maxRetryCount)
                {
                    return false;
                }
            }
            return true;
        }

        public static string StartWinService(string name, int waitTime)
        {
            try
            {
                ServiceController sc = new ServiceController(name);
                if (ThreadWait(waitTime,() =>
                {
                    sc.Refresh();
                    return (sc.Status.Equals(ServiceControllerStatus.Stopped)) || (sc.Status.Equals(ServiceControllerStatus.StopPending));
                }))
                {
                    sc.Start();
                    return ThreadWait(waitTime, () =>
                    {
                        sc.Refresh();
                        return sc.Status.Equals(ServiceControllerStatus.Running);
                    }) ? "ok" :"服务启动超时" ;
                }
                return "服务一直处于启动状态，无法在进行启动！";
            }
            catch (Exception e)
            {
                
            }

            return "ok";
        }

        public static string StopWinService(string name, int waitTime)
        {
            ServiceController sc = new ServiceController(name);
            if (sc.Status.Equals(ServiceControllerStatus.Running))
            {
                sc.Stop();
                //sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                sc.Refresh();
            }

            return !ThreadWait(waitTime, () =>
            {
                sc.Refresh();
                return !sc.Status.Equals(ServiceControllerStatus.Running);
            }) ? "该服务停止超时" : "ok";
        }

        /// <summary>
        /// 返回指定枚举类型的指定值的描述
        /// </summary>
        /// <param name="t">枚举类型</param>
        /// <param name="v">枚举值</param>
        /// <returns></returns>
        private static string GetDescription(System.Type t, object v)
        {
            try
            {
                if (!Enum.IsDefined(t, v))
                    return "未知";

                FieldInfo fi = t.GetField(GetName(t, v));
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return (attributes.Length > 0) ? attributes[0].Description : GetName(t, v);
            }
            catch
            {
                return "未知";
            }
        }

        private static string GetName(System.Type t, object v)
        {
            try
            {
                return Enum.GetName(t, v);
            }
            catch (Exception e)
            {
                return "未知";//UNKNOWN
            }
        }


        public static string ProcessCmd(string cmdPath, string arguments)
        {
            string m_result = "";
            Process process = new Process();
            process.StartInfo.FileName = cmdPath;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = arguments;

            try
            {

                process.Start();
                int m_ProcessId = process.Id;
                //进程终止
                process.WaitForExit();

                if (process.HasExited)
                {
                    int exitCode = process.ExitCode;
                    if (exitCode == 0)
                    {
                        m_result = "ok";
                    }
                    else
                    {
                        throw new Exception("process failure");
                    }
                }
            }
            catch (Exception e)
            {
                m_result = e.Message;
            }
            finally
            {
                //释放资源
                process.Close();

            }

            return m_result;
        }
    }
}
