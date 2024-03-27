// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using SdkHelpers.Common;
using System;
using System.ServiceProcess;

namespace WindowsServiceSample
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(String[] args)
        {
            SdkAssemblyLoader.Start();

            var servicesToRun = new ServiceBase[] { new WindowsServiceSample(args) };
            ServiceBase.Run(servicesToRun);

            SdkAssemblyLoader.Stop();
        }

        #endregion Private Methods
    }
}