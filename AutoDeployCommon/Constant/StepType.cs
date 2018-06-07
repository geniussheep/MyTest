namespace Benlai.Application.AutoPublish.Common.Constant
{
    public class StepType
    {
        public const string MainStepBackup = "MainStep_Backup";
        public const string MainStepLocalRollback = "MainStep_LocalRollback";
        public const string MainStepPublish = "MainStep_Publish";
        public const string MainStepRollback = "MainStep_Rollback";
        public const string MainStepDeploy = "MainStep_Deploy";
        public const string MainStepAutoUpdate = "MainStep_AutoUpdate";

        public const string SubStepDeletePath = "SubStep_DeletePath";
        public const string SubStepDeployApp = "SubStep_DeployApp";
        public const string SubStepDownScanv = "SubStep_DownScanv";
        public const string SubStepUpScanv = "SubStep_UpScanv";
        public const string SubStepOfflineCheck = "SubStep_OfflineCheck";
        public const string SubStepOnlineCheck = "SubStep_OnlineCheck";
        public const string SubStepRestartIisWeb = "SubStep_RestartIISWeb";
        public const string SubStepRestartTaskJob = "SubStep_RestartTaskJob";
        public const string SubStepStartIisWeb = "SubStep_StartIISWeb";
        public const string SubStepStartWinService = "SubStep_StartWinService";
        public const string SubStepStopIisWeb = "SubStep_StopIISWeb";
        public const string SubStepStopWinService = "SubStep_StopWinService";
        public const string SubStepSyncVersion = "SubStep_SyncVersion";
        public const string SubStepSvnUpdate = "SubStep_SvnUpdate";
        public const string SubStepSyncVersionWithCdn = "SubStep_SyncVersionWithCdn";
        public const string SubStepTestLoad = "SubStep_TestLoad";
        public const string SubStepPublishConfigFile = "SubStep_PublishConfigFile";
        public const string SubStepMandatoryWait = "SubStep_MandatoryWait";
    }
}
