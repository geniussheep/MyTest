using Microsoft.Web.Administration;

namespace AutoDeployCommon.Models
{
    public class IisDeployInfo
    {
        /// <summary>
        /// 站点名
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// 站点协议，如：http
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 绑定的相关信息 "*:&lt;port&gt;:&lt;hostname&gt;" <example>"*:80:myhost.com"</example>
        /// </summary>
        public string BindingInformation { get; set; }

        /// <summary>
        /// 物理路径
        /// </summary>
        public string PhysicalPath { get; set; }

        /// <summary>
        /// 是否新建应用程序池
        /// </summary>
        public bool CreateAppPool { get; set; }

        /// <summary>
        /// 应用程序池名称
        /// </summary>
        public string AppPoolName { get; set; }

        /// <summary>
        /// 队列长度
        /// </summary>
        public long QueueLength { get; set; }

        /// <summary>
        /// 进程模型标识
        /// </summary>
        public ProcessModelIdentityType IdentityType { get; set; }

        /// <summary>
        /// 闲着超时时间(秒)
        /// </summary>
        public long IdleTimeout { get; set; }

        /// <summary>
        /// 应用程序池特殊用户的用户名
        /// </summary>
        public string AppPoolUserName { get; set; }

        /// <summary>
        /// 应用程序池特殊用户的密码
        /// </summary>
        public string AppPoolPassword { get; set; }

        /// <summary>
        /// 最大工作进程数
        /// </summary>
        public long MaxProcesses { get; set; }

        /// <summary>
        /// 应用程序池托管管道模式
        /// </summary>
        public ManagedPipelineMode AppPoolPipelineMode { get; set; }

        /// <summary>
        /// .net clr版本
        /// </summary>
        public string ManagedRuntimeVersion { get; set; }

        /// <summary>
        /// 最大故障数
        /// </summary>
        public long RapidFailProtectionMaxCrashes { get; set; }

        /// <summary>
        /// IIS日志目录路径
        /// </summary>
        public string LogDirectoryPath { get; set; }

        /// <summary>
        /// IIS日志格式
        /// </summary>
        public LogFormat LogFormat { get; set; }

        /// <summary>
        /// 日志存储的字段
        /// </summary>
        public LogExtFileFlags LogExtFileFlags { get; set; }

        /// <summary>
        /// 日志的存储计划
        /// </summary>
        public LoggingRolloverPeriod LoggingRolloverPeriod { get; set; }

        /// <summary>
        /// 日志单个文件最大大小（KB） 最小为1KB
        /// </summary>
        public long LogTruncateSize { get; set; }
    }
}