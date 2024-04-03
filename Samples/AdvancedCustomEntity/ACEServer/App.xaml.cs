using SdkHelpers.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ACEServer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            SdkAssemblyLoader.Stop();
            AppDomain.CurrentDomain.UnhandledException -= OnCurrentDomainUnhandledException;
            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SdkAssemblyLoader.Start();
            AppDomain.CurrentDomain.UnhandledException += OnCurrentDomainUnhandledException;
            base.OnStartup(e);
        }

        void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // In your SDK application, use your own logic to manage unhandled exception.  
            // This handler simply helps troubleshoot issues
            MessageBox.Show(e.ExceptionObject.ToString());
        }
    }
}
