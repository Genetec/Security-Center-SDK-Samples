using SdkHelpers.Common;
using System.Windows;

namespace BasicOverlaySample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            SdkAssemblyLoader.Start();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            SdkAssemblyLoader.Stop();
            base.OnExit(e);
        }
    }
}
