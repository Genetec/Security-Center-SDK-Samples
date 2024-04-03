#region Using Statements
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SdkHelpers.Common;
#endregion

// ==========================================================================
// Copyright (C) 1989-2007 by Genetec Information Systems, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            SdkAssemblyLoader.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault( false );
            Application.Run( new MainDlg() );
            SdkAssemblyLoader.Stop();
        }
    }
}