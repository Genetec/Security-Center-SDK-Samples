using System;
using Genetec.Sdk;
using System.Windows;
using SdkHelpers.Common;

namespace OverlaySample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App 
    {
        #region Event Handlers

        static App()
        {
            SdkAssemblyLoader.Start();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            SdkAssemblyLoader.Stop();
            AppDomain.CurrentDomain.UnhandledException -= OnCurrentDomainUnhandledException;
            base.OnExit(e);
        }

        void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // In your SDK application, use your own logic to manage unhandled exception.  
            // This handler simply helps troubleshoot issues
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        #endregion

        public static new App Current
        {
            get { return (App) Application.Current; }
        }

        public Engine Sdk { get; private set; }

        public App()
        {
            Sdk = new Engine
            {
                LoginManager =
                {
                    ClientCertificate = "KxsD11z743Hf5Gq9mv3+5ekxzemlCiUXkTFY5ba1NOGcLCmGstt2n0zYE9NsNimv",
                    ConnectionRetry = -1
                }
            };

            new MainWindow().Show();
        }
    }
}
