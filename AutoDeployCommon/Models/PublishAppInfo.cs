using System.Collections.Generic;

namespace Benlai.Application.AutoPublish.Common.Models
{
    public class PublishAppInfo
    {
        /// <summary>
        /// App的ID
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// App的key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// App的服务ID
        /// </summary>
        public int AppSvrId { get; set; }

        /// <summary>
        /// App的服务器Ip
        /// </summary>
        public string AppSvrIp { get; set; }

        /// <summary>
        /// App名称
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        ///  dr["site_pool_name"].ToString().Split(',')
        /// App名称列表
        /// Site:应用地址池的名称
        /// Service：服务名
        /// Tomcat：Tomcat路径
        /// </summary>
        public List<string> AppSitePoolList { get; set; }

        /// <summary>
        /// App的文件所在路径
        /// </summary>
        public string AppPath { get; set; }

        /// <summary>
        /// App的类型
        /// </summary>
        public string AppType { get; set; }

        /// <summary>
        /// 测试App是否可访问的测试地址
        /// </summary>
        public string AppHealthCheckUrl { get; set; }

        public int AppWaitTime { get; set; }

        /// <summary>
        /// 当前App发布的状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 是否需要被设置需要备份
        /// </summary>
        public bool IsBackup { get; set; }

        /// <summary>
        /// 是否是部署任务
        /// </summary>
        public bool IsDeployMission { get; set; }

        /// <summary>
        /// 备份App原文件列表的备份地址
        /// </summary>
        public string BackupPath { get; set; }

        /// <summary>
        /// 迁出App代码的本地路径
        /// </summary>
        public string CheckoutPath { get; set; }

        /// <summary>
        /// 增量文件路径
        /// </summary>
        public string IncrementPath { get; set; }

        /// <summary>
        /// 本地回滚的文件存放路径（带版本号的）
        /// </summary>
        public string RollbackPath { get; set; }

        /// <summary>
        /// 本地回滚的文件存放根目录
        /// </summary>
        public string RollbackRootPath { get; set; }

        /// <summary>
        /// 同步删除的文件备份路径
        /// </summary>
        public string DeleteFileBackupPath { get; set; }

        /// <summary>
        /// 发布id
        /// </summary>
        public long PublishId { get; set; }

        /// <summary>
        /// 上一次发布完成的版本
        /// </summary>
        public string LastPublishVersion { get; set; }

        /// <summary>
        /// 本次发布的版本
        /// </summary>
        public string CurrPublishVersion { get; set; }

        /// <summary>
        /// 要回滚发布的的版本
        /// </summary>
        public string RollbackPublishVersion { get; set; }

        /// <summary>
        /// 远程代码备份路径
        /// </summary>
        public string RemoteBackupPath { get; set; }

        /// <summary>
        /// 远程回滚版本的文件路径
        /// </summary>
        public string RemoteRollbackPath { get; set; }

        /// <summary>
        /// 远程代码获取路径
        /// </summary>
        public string RemotePublishPath { get; set; }

        /// <summary>
        /// 备份时要跳过的文件列表
        /// </summary>
        public List<string> SkipFileListBackup { get; set; }

        /// <summary>
        /// 发布时要跳过的文件列表
        /// </summary>
        public List<string> SkipFileListPublish { get; set; }

        /// <summary>
        /// App Scanv判断文件
        /// </summary>
        public string ScanvCheckFileName { get; set; }

        /// <summary>
        /// App环境类型
        /// </summary>
        public string EnvironmentType { get; set; }

        /// <summary>
        /// App版本ID
        /// </summary>
        public int VersionId { get; set; }

    }
}
