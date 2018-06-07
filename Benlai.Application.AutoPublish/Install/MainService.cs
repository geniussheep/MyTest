using System.ServiceProcess;
using Benlai.Application.AutoPublish.AppStart;

namespace Benlai.Application.AutoPublish.Install
{
    public partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppServer.Instance.Start();
        }

        protected override void OnStop()
        {
            AppServer.Instance.Stop();
        }
    }
}