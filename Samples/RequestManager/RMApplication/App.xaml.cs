// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using SdkHelpers.Common;
using System;
using System.Windows;

namespace RMApplication
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Protected Methods

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

        #endregion Protected Methods

        #region Private Methods

        private static void OnCurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // In your SDK application, use your own logic to manage unhandled exception.
            // This handler simply helps troubleshoot issues
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        #endregion Private Methods

    }

}