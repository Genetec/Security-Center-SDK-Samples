// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Monitors.Notifications;
using System;
using System.Windows;

namespace ModuleSample.Notifications
{

    public sealed class ConfigToolStartTray : Notification
    {

        #region Private Fields

        private readonly ConfigToolStartTrayView m_view = new ConfigToolStartTrayView();

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the notification
        /// </summary>
        public override string Name => "Start ConfigTool";

        /// <summary>
        /// Gets or sets the view representing the notification's user interface
        /// </summary>
        public override UIElement View => m_view;

        #endregion Public Properties

        #region Protected Properties

        /// <summary>
        /// Gets the notification tray's unique ID
        /// </summary>
        protected override Guid UniqueId => new Guid("{461292FC-EFFA-4429-8119-ABEDD4F66BE2}");

        #endregion Protected Properties

        #region Protected Methods

        /// <summary>
        /// Performs initialization code
        /// </summary>
        protected override void Initialize()
        {
            m_view.Initialize(Workspace);
        }

        #endregion Protected Methods

    }

}