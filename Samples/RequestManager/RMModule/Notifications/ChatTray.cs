// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace.Monitors.Notifications;
using RMModule.Options;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RMModule.Notifications
{

    public sealed class ChatTray : Notification
    {

        #region Private Fields

        private ChatTrayView m_view;

        #endregion Private Fields

        #region Public Properties

        public override string Name => "Open Chat";

        public override UIElement View
        {
            get
            {
                if (m_view == null)
                {
                    m_view = new ChatTrayView();
                    m_view.Initialize(Workspace);
                }

                return m_view;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        protected override bool HasPrivileges
        {
            get
            {
                if (Workspace != null)
                {
                    return Workspace.Sdk.LoginManager.IsConnected;
                }

                return false;
            }
        }

        protected override IList<NotificationTrayBehavior> SupportedBehaviors =>
            new List<NotificationTrayBehavior>
            {
                NotificationTrayBehavior.Hide,
                NotificationTrayBehavior.Show
            };

        protected override Guid UniqueId => new Guid("{06DF95ED-E8F6-4738-8A8C-678B4A6C1E53}");

        #endregion Protected Properties

        #region Public Methods

        public override void Dispose()
        {
            if (Workspace?.Sdk != null)
            {
                Workspace.Sdk.LoginManager.LoggedOn -= OnLoggedOn;
            }

            if (m_view != null)
            {
                m_view.Dispose();
                m_view = null;
            }

            base.Dispose();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Initialize()
        {
            if (Workspace?.Sdk != null)
            {
                Workspace.Sdk.LoginManager.LoggedOn += OnLoggedOn;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            RefreshVisibility();
            var chatOption = Workspace.Options[ChatOptionsExtension.ExtensionName];
            if (chatOption != null)
            {
                if ((bool)chatOption[ChatOptionsExtension.ShowPopupProperty])
                {
                    // In this sample, a popup welcomes the user with explanations on the sample
                    m_view.ShowPopUp();
                }
            }
        }

        #endregion Private Methods
    }

}