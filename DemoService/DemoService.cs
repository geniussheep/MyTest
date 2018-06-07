using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace DemoService
{
    public partial class DemoService : ServiceBase
    {
        public DemoService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DemoMain.Start();
        }

        protected override void OnStop()
        {
            DemoMain.Stop();

        }
    }
}
