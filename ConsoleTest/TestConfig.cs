using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Benlai.Common;

namespace ConsoleTest
{
    public class TestConfig
    {
        private static string GetSyncServiceFullUrl(string actionPath,string from)
        {
            var syncServiceRootUrl = SyncServiceDomain;

            if (syncServiceRootUrl.EndsWith("/"))
            {
                return syncServiceRootUrl + actionPath + from;
            }
            else
            {
                return syncServiceRootUrl + "/" + actionPath + from;
            }
        }

        public static readonly string SyncServiceDomain = ConfigManager.GetConfigObject("ApplicationSyncDomain", "http://sync.plf.benlai.com");

        public static readonly int SyncServiceDomainTimeout = ConfigManager.GetConfigObject("ApplicationSyncDomainTimeout", 10000);

        public static string SyncConfigApiUrl = GetSyncServiceFullUrl("Application/SyncConfig","field");

        public static string SyncFirstPublishApiUrl => GetSyncServiceFullUrl("Application/SyncPublish","property");

    }
}
