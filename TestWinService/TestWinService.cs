using System;
using System.ServiceProcess;
using System.Threading;
using Benlai.Common;

namespace TestWinService
{
    public partial class TestWinService : ServiceBase
    {
        private bool _isStop;

        public TestWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogInfoWriter.GetInstance("TestWinService").Info($"Start TestWinService {DateTime.Now}");
        }

        protected override void OnStop()
        {
            LogInfoWriter.GetInstance("TestWinService").Info($"Stop TestWinService {DateTime.Now}");
        }
    }
}