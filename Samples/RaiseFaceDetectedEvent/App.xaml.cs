using System;
using System.Windows;
using SdkHelpers.Common;


namespace RaiseFaceDetectedEvent
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Event Handlers

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

        #endregion
    }
}
