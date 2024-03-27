// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Controls;
using Genetec.Sdk.Workspace;
using RMModule.Pages;
using RMModule.Services;
using System;
using System.Windows;
using System.Windows.Input;

namespace RMModule.Notifications
{

    /// <summary>
    /// Interaction logic for GuardTourTrayView.xaml
    /// </summary>
    public partial class ChatTrayView : IDisposable
    {

        #region Public Fields

        public static readonly DependencyProperty MessagesArchivedProperty =
                                            DependencyProperty.Register("MessagesArchived", typeof(string), typeof(ChatTrayView), new PropertyMetadata(""));

        #endregion Public Fields

        #region Private Fields

        private PinnablePopup m_pinnablePopup;

        private ChatNotifContent m_popupContent;

        #endregion Private Fields

        #region Public Properties

        public string MessagesArchived
        {
            get => (string)GetValue(MessagesArchivedProperty);
            set => SetValue(MessagesArchivedProperty, value);
        }

        /// <summary>
        /// Gets the application's workspace.
        /// </summary>
        public Workspace Workspace { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ChatTrayView() => InitializeComponent();
       
        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            var receiver = Workspace.Services.Get<IChatService>();
            if (receiver != null)
            {
                receiver.MessagesArchived -= HandleMessagesArchived;
            }

            if (m_popupContent != null)
            {
                m_popupContent.ShowChatClicked -= OnShowChatClicked;
                m_popupContent.HideClicked -= OnHideClicked;
                m_popupContent = null;
            }

            if (m_pinnablePopup != null)
            {
                m_pinnablePopup.Dispose();
                m_pinnablePopup = null;
            }
        }

        /// <summary>
        /// Initialize the tray and update its status.
        /// </summary>
        /// <param name="workspace">The application's workspace.</param>
        public void Initialize(Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentNullException("workspace");
            var receiver = Workspace.Services.Get<IChatService>();
            receiver.MessagesArchived += HandleMessagesArchived;
        }

        public void ShowPopUp()
        {
            if (m_pinnablePopup == null)
            {
                var builder = new ChatNotifContentBuilder();

                m_popupContent = builder.CreateContent() as ChatNotifContent;
                if (m_popupContent == null)
                    return;

                m_popupContent.HideClicked += OnHideClicked;
                m_popupContent.ShowChatClicked += OnShowChatClicked;

                m_pinnablePopup = new PinnablePopup(builder.UniqueId);
                m_pinnablePopup.Initialize(Workspace);
                m_pinnablePopup.PopupContent = m_popupContent;
                m_pinnablePopup.Header = builder.Name;

                m_pinnablePopup.PlacementTarget = this;
            }
            m_pinnablePopup.Show();
        }

        #endregion Public Methods

        #region Private Methods

        private void HandleMessagesArchived(object sender, int nbMessages)
        {
            MessagesArchived = nbMessages > 0 ? nbMessages.ToString() : "";
            if (nbMessages > 9)
            {
                var margins = new Thickness(1, 0, 0, 0);
                textBlock.Margin = margins;
            }
            else
            {
                var margins = new Thickness(5, 0, 0, 0);
                textBlock.Margin = margins;
            }
        }

        private void OnHideClicked(object sender, EventArgs eventArgs) => m_pinnablePopup?.Hide();
        
        /// <summary>
        /// Occurs when the user left-click on the tray.
        /// </summary>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShowPopUp();
            e.Handled = true;
        }

        private void OnShowChatClicked(object sender, EventArgs eventArgs)
        {
            if (m_pinnablePopup != null)
            {
                m_pinnablePopup.Hide();
                var pageToOpen = new Genetec.Sdk.Workspace.Tasks.CreatePageTask<ChatPage>(true);
                pageToOpen.Initialize(Workspace);
                pageToOpen.Execute();
            }
        }

        #endregion Private Methods
    }

}