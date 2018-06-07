using System.ComponentModel;
using System.Configuration.Install;

namespace Benlai.Application.AutoPublish.Install
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}