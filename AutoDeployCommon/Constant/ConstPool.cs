using System.Collections.Generic;
using System.Configuration;
using Benlai.Application.Model;

namespace Benlai.Application.AutoPublish.Common.Constant
{
    public class ConstPool
    {
        public const string FileSeparator = "\\";

        public const string BackupSuccessTxtName = "success.txt";
        public const string LastestVersion = "lastest_version";

        public const string PublishTypeDeploy = "deploy";

        public const int TestLoadRetryCount = 3;

        public static readonly string IncrementPathConfig = ConfigurationManager.AppSettings["IncrementPath"] ?? @"increment";
        public static readonly string CheckOutPathConfig = ConfigurationManager.AppSettings["CheckOutPath"] ?? @"newCheckout";
        public static readonly string BackupPathConfig = ConfigurationManager.AppSettings["BackupPath"] ?? @"backup";

        public static readonly string RollBackPathConfig = ConfigurationManager.AppSettings["RollBackPath"] ?? @"rollback";

        public static readonly string TaskManagerUrl = ConfigurationManager.AppSettings["TaskManagerUrl"];

        public static readonly string RemotePublishPath = ConfigurationManager.AppSettings["RemotePublishPath"] ?? @"publish";

        public static readonly string RemoteBackupPath = ConfigurationManager.AppSettings["RemoteBackupPath"] ?? @"backup";

        public static readonly string DeleteFileBackupPath = ConfigurationManager.AppSettings["DeleteFileBackupPath"] ?? @"deleteFileBackup";


        public static readonly string RemoteIp = ConfigurationManager.AppSettings["RemoteIp"] ?? "";

        public static readonly string RemoteUserName = ConfigurationManager.AppSettings["RemoteUserName"] ?? "";

        public static readonly string RemotePassword = ConfigurationManager.AppSettings["RemotePassword"] ?? "";

        public static readonly string RemoteBasePath = ConfigurationManager.AppSettings["RemoteBasePath"] ?? "";

        public static readonly Dictionary<string, string> MainStepErrorDic = new Dictionary<string, string> {
            { ApplicationPublishStatus.Rollbacking.ToString().ToLower(), ApplicationPublishStatus.Rollbackerror.ToString().ToLower() },

            { ApplicationPublishStatus.Backuping.ToString().ToLower(), ApplicationPublishStatus.Backuperror.ToString().ToLower() },

            { ApplicationPublishStatus.Prepublishing.ToString().ToLower(), ApplicationPublishStatus.Prepublisherror.ToString().ToLower() },

            { ApplicationPublishStatus.Publishing.ToString().ToLower(), ApplicationPublishStatus.Publisherror.ToString().ToLower() },

            { ApplicationPublishStatus.Localrollback.ToString().ToLower(), ApplicationPublishStatus.Localrollbackerror.ToString().ToLower() },

        };
    }
}
