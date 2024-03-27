using SdkHelpers.Common;
using System.Windows;

namespace Listener
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SdkAssemblyLoader.Start();
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                SdkAssemblyLoader.Stop();
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}
