using System;

namespace AutoDeployCommon.Models
{
    public class DeployAppInfo
    {
        public string iis_netversion { get; set; }
        public string iis_maxprocesscount { get; set; }
        public string iis_queuecount { get; set; }
        public string iis_webport { get; set; }
        public string iis_rapidfail_protection_maxcrashes { get; set; }
        public string iis_apppool_pipeline_mode { get; set; }
        public string iis_log_dirpath { get; set; }
        public string iis_log_truncatesize { get; set; }
        public string iis_logging_rolloverperiod { get; set; }
        public string winsvr_programfullname { get; set; }
        public string winsvr_starttype { get; set; }
        public bool CheckIisDeployInfo()
        {
            return String.IsNullOrWhiteSpace(iis_apppool_pipeline_mode) ||
                   String.IsNullOrWhiteSpace(iis_log_dirpath) ||
                   String.IsNullOrWhiteSpace(iis_logging_rolloverperiod) ||
                   (iis_logging_rolloverperiod == "0" && (String.IsNullOrWhiteSpace(iis_log_truncatesize) || iis_log_truncatesize == "0")) ||
                   String.IsNullOrWhiteSpace(iis_maxprocesscount) || iis_maxprocesscount == "0" ||
                   String.IsNullOrWhiteSpace(iis_netversion) ||
                   String.IsNullOrWhiteSpace(iis_queuecount) || iis_queuecount == "0" ||
                   String.IsNullOrWhiteSpace(iis_rapidfail_protection_maxcrashes) || iis_rapidfail_protection_maxcrashes == "0" ||
                   String.IsNullOrWhiteSpace(iis_webport) || iis_webport == "0";

        }

        public bool CheckWinSvrDeployInfo()
        {
            return String.IsNullOrWhiteSpace(winsvr_programfullname) ||
                   String.IsNullOrWhiteSpace(winsvr_starttype);
        }
    }


}
