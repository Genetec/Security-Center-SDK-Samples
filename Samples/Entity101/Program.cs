using System;
using System.Windows.Forms;
using SdkHelpers.Common;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Entity101
{
    #region Classes

    static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SdkAssemblyLoader.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Entity101());
            SdkAssemblyLoader.Stop();
        }

        #endregion
    }

    #endregion
}

