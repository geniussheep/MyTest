using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml;
using Benlai.Common;
using Benlai.RiskControl.Logging.Client;
using Newtonsoft.Json.Linq;

namespace ConsoleJenkins
{
    class Program
    {
        private const String JkbtResultSuccess = "<result>SUCCESS</result>";
        private const String JkbtResultFailure = "<result>FAILURE</result>";
        private const double OneMinute = 60*1000;

        static void Main(string[] args)
        {
            TestCreateBuildTask();
            Console.Read();
        }

        #region 新建编译任务

        private static void TestCreateBuildTask()
        {
            var appName = "Benlai.Coupon.BatchBindCouponJob";
            var environment = "trunk";

            CreateJkbt(appName, environment);
            //            CreateJkbt(appName, environment);
            CreateJkbt(appName, environment);

            var result = WaitJkbtAndGetResult(appName, environment, "Test");

            Console.WriteLine(result.Item1
                ? $"success--JenkinsBuildId:{result.Item2}--SvnNumber:{result.Item3}"
                : $"failure:JenkinsBuildId:{result.Item2}--ErrorMsg:{result.Item3}");
        }

        /// <summary>
        /// 新建JenkinsBuildTask
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="environment">所属环境</param>
        private static void CreateJkbt(string appName, string environment)
        {
            var createjkbtApiUrl = GetJenkinsConfig("JenkinsCreateBuildTaskApiUrl", "");
            createjkbtApiUrl = String.Format(createjkbtApiUrl, appName, environment,
                ConfigManager.GetConfigObject("JenkinsCreateBuildTaskDeplayTime", 0));
            Console.WriteLine($"JenkinsCreateBuildTaskApiUrl:{createjkbtApiUrl}");
            HttpClientUtils.Post(createjkbtApiUrl, new Dictionary<string, string> { { "Version", "HEAD" } });
        }

        /// <summary>
        /// 等待JenkinsBuildTask并返回任务结果
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="environment">所属环境</param>
        /// <param name="logDirName">日志输出文件夹名</param>
        /// <returns>Tuple bool--是否编译成功, string--成功则为任务id 则是异常信息或空, string--编译的SVN的版本 </returns>
        private static Tuple<bool, string, string> WaitJkbtAndGetResult(string appName, string environment,
            string logDirName)
        {
            var jkbtId = "";
            try
            {
                //第一步：检查Jenkins的任务队列是否有等待执行的任务，没人排队任务则继续第二步
                var jenkinsQueueUrl = GetJenkinsConfig("JenkinsQueueApiUrl", "");
                LogInfoWriter.GetInstance(logDirName).Info($"start check jenkins build task queue is empty, JenkinsQueueApiUrl:{jenkinsQueueUrl}");
                var isJkQueueEmpty = ExecExtensions.RetryUntilTrueWithTimeout(() =>
                {
                    var queueInfo = HttpClientUtils.GetJson(jenkinsQueueUrl);
                    var queueInfoJObj = JObject.Parse(queueInfo);
                    Console.WriteLine("jenkins build task queue is not empty");
                    return !queueInfoJObj["items"].Values<JObject>().Any();
                }, ConfigManager.GetConfigObject("JenkinsQueueCheckTimeout", 5) * OneMinute);
                if (!isJkQueueEmpty)
                    throw new Exception("wait jenkins build task finish error; ErrorMsg:check wether jenkins build task queue is empty timout");
                LogInfoWriter.GetInstance(logDirName).Info("check jenkins build task queue is empty end");

                //第二步：获取Jenkins的编译任务Id
                var getJkbtNumberUrl = GetJenkinsConfig("JenkinsGetBuildTaskIdApiUrl", "");
                getJkbtNumberUrl = String.Format(getJkbtNumberUrl, appName, environment);
                LogInfoWriter.GetInstance(logDirName).Info($"start get jenkins build task id, JenkinsGetBuildTaskIdApiUrl:{getJkbtNumberUrl}");
                var jkbtNumberInfo = HttpClientUtils.Get(getJkbtNumberUrl);
                jkbtId = GetJenkinsXmlValue(jkbtNumberInfo);
                if (String.IsNullOrWhiteSpace(jkbtId))
                    throw new Exception("get jenkins build task id error,the jenkins build task id is null or empty!");
                LogInfoWriter.GetInstance(logDirName).Info($"get jenkins build task id:{jkbtId} end");

                //第三步：根据Jenkins的编译任务Id获取其执行结果
                var getJkbtResultUrl = GetJenkinsConfig("JenkinsGetBuildTaskResultApiUrl", "");
                getJkbtResultUrl = String.Format(getJkbtResultUrl, appName, environment, jkbtId);
                LogInfoWriter.GetInstance(logDirName).Info($"start get jenkins build task:{jkbtId} result, JenkinsGetBuildTaskResultApiUrl:{getJkbtResultUrl}");
                var isJkbtFinish = ExecExtensions.RetryUntilTrueWithTimeout(() =>
                {
                    if (String.Equals(JkbtResultSuccess, HttpClientUtils.Get(getJkbtResultUrl),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return ExecExtensions.ResultType.Success;
                    }
                    if (String.Equals(JkbtResultFailure, HttpClientUtils.Get(getJkbtResultUrl),
                        StringComparison.OrdinalIgnoreCase))
                    {
                        return ExecExtensions.ResultType.Failure;
                    }
                    return ExecExtensions.ResultType.Continue;
                }, ConfigManager.GetConfigObject("JenkinsGetBuildTaskResultTimeout", 10) * OneMinute);
                LogInfoWriter.GetInstance(logDirName).Info($"get jenkins build task:{jkbtId} result end");

                //第四步：根据Jenkins编译任务结果返回最终结果
                switch (isJkbtFinish)
                {
                    case ExecExtensions.ResultType.Success:
                        var getJkbtSvnNumberUrl = GetJenkinsConfig("JenkinsGetBuildTaskSvnNumberUrl", "");
                        getJkbtSvnNumberUrl = String.Format(getJkbtSvnNumberUrl, appName, environment, jkbtId);
                        LogInfoWriter.GetInstance(logDirName).Info($"start get jenkins build task:{jkbtId} svn number,JenkinsGetBuildTaskSvnNumberUrl:{getJkbtSvnNumberUrl}");
                        var jkbtSvnNumberInfo = HttpClientUtils.Get(getJkbtSvnNumberUrl);
                        var jkbtSvnNumber = GetJenkinsXmlValue(jkbtSvnNumberInfo);
                        if (String.IsNullOrWhiteSpace(jkbtSvnNumber))
                            throw new Exception("$the jenkins build task:{jkbtId} svn number is null or empty!");
                        return Tuple.Create(true, jkbtId, jkbtSvnNumber);
                    case ExecExtensions.ResultType.Failure:
                        return Tuple.Create(false, jkbtId, "jenkins build task failure");
                    case ExecExtensions.ResultType.Timeout:
                        return Tuple.Create(false, jkbtId, "jenkins build task timeout");

                }
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, jkbtId, ex.ToString());
            }
            return Tuple.Create(false, jkbtId, "unknow error");
        }

        private static string GetJenkinsConfig(string key, string defaultValue)
        {
            var value = ConfigManager.GetConfigObject(key, defaultValue);
            if (String.IsNullOrWhiteSpace(value)) throw new Exception($"missing configuration,the config key:{key}");
            return value;
        }

        private static string GetJenkinsXmlValue(string xmlText)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlText);
            return xmlDoc.InnerText;
        }

        #endregion

        #region 新建Jenkins的项目

        

        #endregion

    }



    public class ExecExtensions
    {
        public enum ResultType
        {
            Success ,
            Failure ,
            Timeout ,
            Continue,
            OutOfRetryCount
        }

        public static bool RetryUntilTrueWithTimeout(Func<bool> callable, double timeoutMs)
        {
            var hiPerfTimer = new HiPerfTimer();
            double remaining;
            do
            {
                hiPerfTimer.Start();
                Thread.Sleep(1000);
                if (callable()) return true;
                hiPerfTimer.Stop();
                remaining = timeoutMs - hiPerfTimer.DurationDouble;
            } while (remaining > 0);
            return false;
        }

        public static bool RetryUntilTrueWithRetryCount(Func<bool> callable, int retryMaxCount)
        {
            int retryCount = 0;
            while (retryCount < retryMaxCount)
            {
                retryCount++;
                Thread.Sleep(1000);
                if (callable()) return true;
            }
            return false;
        }

        public static ResultType RetryUntilTrueWithTimeout(Func<ResultType> callable, double timeoutMs)
        {
            var hiPerfTimer = new HiPerfTimer();
            double remaining;
            do
            {
                hiPerfTimer.Start();
                Thread.Sleep(1000);
                var callableResultType = callable();
                if (callableResultType != ResultType.Continue)
                {
                    return callableResultType;
                }
                hiPerfTimer.Stop();
                remaining = timeoutMs - hiPerfTimer.DurationDouble;
            } while (remaining > 0);
            return ResultType.Timeout;
        }

        public static ResultType RetryUntilTrueWithRetryCount(Func<ResultType> callable, int retryMaxCount)
        {
            int retryCount = 0;
            while (retryCount < retryMaxCount)
            {
                retryCount++;
                Thread.Sleep(1000);
                var callableResultType = callable();
                if (callableResultType != ResultType.Continue)
                {
                    return callableResultType;
                }
            }
            return ResultType.OutOfRetryCount;
        }
    }
}

