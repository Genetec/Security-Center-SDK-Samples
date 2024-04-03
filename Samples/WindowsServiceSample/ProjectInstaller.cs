using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;

namespace WindowsServiceSample
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            // If you have custom things to do after installation such as registering registry keys, do them here.
        }
    }
}
