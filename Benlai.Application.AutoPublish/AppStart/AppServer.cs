using System;
using System.Threading.Tasks;
using Benlai.Common;
using Benlai.Performance.Cat;
using Benlai.SOA.Framework.Server;
using Benlai.Application.AutoPublish.Configuration;

namespace Benlai.Application.AutoPublish.AppStart
{
    public class AppServer
    {
        private static readonly Lazy<AppServer> InstanceLazy = new Lazy<AppServer>();

        private WebApp _webApp;
        private TcpApp _tcpApp;

        public static AppServer Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        public void Start()
        {
            CatConfig.Init(AppConfig.CatDomain, AppConfig.CatEnable, AppConfig.CatServer);

            MapArea.RegisterMapArea();

            if (AppConfig.HttpPort > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    _webApp = new WebApp();
                    _webApp.Start(AppConfig.HttpPort);
                }, TaskCreationOptions.LongRunning)
                    .ContinueWith(t =>
                    {
                        if (!t.IsFaulted) return;
                        LogInfoWriter.GetInstance().Error("TcpApp start error", t.Exception);
                        if (t.Exception != null) throw t.Exception;
                    });
            }

            if (AppConfig.TcpPort > 0)
            {
                Task.Factory.StartNew(() =>
                {
                    _tcpApp = new TcpApp();
                    _tcpApp.Start(AppConfig.TcpPort);
                }, TaskCreationOptions.LongRunning)
                .ContinueWith(t =>
                {
                    if (!t.IsFaulted) return;
                    LogInfoWriter.GetInstance().Error("TcpApp start error", t.Exception);
                    if (t.Exception != null) throw t.Exception;
                });
            }
        }

        public void Stop()
        {
            try
            {
                if (_webApp != null)
                {
                    _webApp.Stop();
                }
                if (_tcpApp != null)
                {
                    _tcpApp.Stop();
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}