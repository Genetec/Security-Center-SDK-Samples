using SdkHelpers.Common;
using System;
using System.Windows;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 12:
//  1793 – Jean Sylvain Bailly, the first Mayor of Paris, is guillotined.
//  1918 – Austria becomes a republic.
//  1933 – Hugh Gray takes the first known photos of the Loch Ness Monster.
// ==========================================================================
namespace MotionDetectionConfig
{
    #region Classes

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

    #endregion
}

