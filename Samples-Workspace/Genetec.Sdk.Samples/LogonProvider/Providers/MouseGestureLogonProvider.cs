// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Threading;
using Genetec.Sdk;
using Genetec.Sdk.Workspace.Components.LogonProvider;
using LogonProvider.Views;

namespace LogonProvider.Providers
{
    public sealed class MouseGestureLogonProvider : Genetec.Sdk.Workspace.Components.LogonProvider.LogonProvider
    {

        #region Private Fields

        /// <summary>
        /// Represent the dialog used to logon an user to the Directory.
        /// </summary>
        [ThreadStatic]
        private static MouseGestureLogonDlg s_logonWindow;

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{CA768866-E361-4F1B-B2FA-41F88D93D761}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public override string Name => "Mouse Gesture LogonProvider";

        /// <summary>
        /// Gets the priority of the provider (highest is better).
        /// </summary>
        public override int Priority => -1000;

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Provide the specific logon mechanism.
        /// </summary>
        public override void LogOn()
        {
            // Make sure the logon window hasn't been initialized before.
            if (s_logonWindow == null)
            {
                // Creation of the Dialog.
                s_logonWindow = new MouseGestureLogonDlg { Owner = Workspace.ActiveMonitor.Window };

                // Events for the Dialog.
                s_logonWindow.Closed += delegate { s_logonWindow = null; };
                s_logonWindow.ReadyForLogon += Connect;

                // Show the dialog.
                s_logonWindow.ShowDialog();
            }
            else
            {
                // If the dialog is just hidden, only need to show it. That is what the Reset do.
                s_logonWindow.Reset();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Method to attempt a logon to the directory.
        /// </summary>
        /// <param name="sender">The Dialog</param>
        /// <param name="e">The eventArgs</param>
        private async void Connect(object sender, EventArgs e)
        {
            // This is where the information is set to connect to the Directory.
            // Some information are essential, and others are optional.
            var logonInfo = new LogonInfo(s_logonWindow.Directory, s_logonWindow.Username, string.Empty) { RetryCount = 5 };

            // This is the cancellation Token source, used to cancel the logonAsync process.
            // If the cancellation is done, the result will be LogonAborted.
            var cts = new CancellationTokenSource();

            // Cancel automatically after 5 seconds.
            cts.CancelAfter(5000);

            // Attempt to logon.
            var result = await LogOnAsync(logonInfo, cts.Token);

            // Reset the window for another attempt.
            s_logonWindow.Reset();
            // Show the error Message
            s_logonWindow.Message = result.ToString();

            if (result == ConnectionStateCode.Success)
            {
                // Close the window if the logon is success.
                s_logonWindow.Message = string.Empty;
                s_logonWindow.Close();
            }
        }

        #endregion Private Methods
    }
}