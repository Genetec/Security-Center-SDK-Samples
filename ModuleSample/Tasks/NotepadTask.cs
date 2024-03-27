// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace.Tasks;
using System;
using System.Windows.Media.Imaging;

namespace ModuleSample.Tasks
{

    public class NotepadTask : Task
    {

        #region Public Fields

        /// <summary>
        /// The privilege that needs to be allowed in order to execute the task, as specified in ModuleSample.privileges.xml.
        /// </summary>
        public const string PRIVILEGE = "{175AD7DC-A9A9-4B6B-A19E-367AF9547DC9}";

        #endregion Public Fields

        #region Private Fields

        private readonly IEngine m_sdk;

        #endregion Private Fields

        #region Public Constructors

        public NotepadTask(IEngine sdk)
        {
            m_sdk = sdk;
            Icon = Thumbnail = new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Notepad.png", UriKind.RelativeOrAbsolute));
            Name = "Launch notepad";
            Description = "Launch the Notepad application. This sample illustrates a Task that executes without opening a page in the Security Desk or Config Tool.";
            CategoryId = ModuleTest.CustomCategoryId;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Indicates if the current task can be executed or not. Default value is false.
        /// </summary>
        public override bool CanExecute()
        {
            return HasPrivilege();
        }

        /// <summary>
        /// Execute the current task.
        /// </summary>
        public override void Execute()
        {
            HideHomePageAfterExecution = false;
            System.Diagnostics.Process.Start("notepad.exe");
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets if the current user has the privilege to see the page.
        /// </summary>
        /// <returns>True if allowed; Otherwise, false.</returns>
        private bool HasPrivilege()
        {
            if (m_sdk.LoginManager.IsConnected)
            {
                return m_sdk.SecurityManager.IsPrivilegeGranted(new Guid(PRIVILEGE));
            }

            return false;
        }

        #endregion Private Methods

    }

}