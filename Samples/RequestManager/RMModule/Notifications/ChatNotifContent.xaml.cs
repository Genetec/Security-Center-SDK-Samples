// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.PinnableContentBuilder;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RMModule.Notifications
{

    /// <summary>
    /// Interaction logic for ChatNotificationContent.xaml
    /// </summary>
    public partial class ChatNotifContent : UserControl
    {

        #region Public Events

        public event EventHandler HideClicked;

        public event EventHandler ShowChatClicked;

        #endregion Public Events

        #region Public Constructors

        public ChatNotifContent() => InitializeComponent();
       
        #endregion Public Constructors

        #region Private Methods

        private void OnButtonHideClicked(object sender, RoutedEventArgs e) => HideClicked?.Invoke(this, e);
       
        private void OnButtonShowChatClicked(object sender, RoutedEventArgs e) => ShowChatClicked?.Invoke(this, e);
       
        #endregion Private Methods

    }

    public sealed class ChatNotifContentBuilder : PinnableContentBuilder
    {

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Chat Module Sample Description";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{2E266CDB-EC15-4361-9912-F1AA4B4BFAC3}");

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create a content that can be pinned.
        /// </summary>
        public override FrameworkElement CreateContent() => new ChatNotifContent();
      
        #endregion Public Methods

    }

}