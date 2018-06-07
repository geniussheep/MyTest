using Benlai.Common;

namespace Benlai.Application.AutoPublish.Configuration
{
    public class AppConfig
    {
        public static readonly bool CatEnable = ConfigManager.GetConfigObject("CatEnable", false);
        public static readonly string CatDomain = ConfigManager.GetConfigValue("CatDomain");
        public static readonly string CatServer = ConfigManager.GetConfigValue("CatServer");
        public static readonly int HttpPort = ConfigManager.GetConfigObject("HttpPort", 0);
        public static readonly int TcpPort = ConfigManager.GetConfigObject("TcpPort", 0);
    }
}