// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using RMModule.Services;
using RMSerialization;
using System;
using System.Windows;

namespace RMModule.Components
{

    /// <summary>
    /// Interaction logic for ChatEventCtl.xaml
    /// </summary>
    public partial class ChatEventCtl
    {

        #region Public Fields

        public static readonly DependencyProperty MessageProperty =
                    DependencyProperty.Register("Message", typeof(string), typeof(ChatEventCtl), new PropertyMetadata(""));

        #endregion Public Fields

        #region Public Properties

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        /// <summary>
        /// Gets the application's workspace.
        /// </summary>
        public Workspace Workspace
        {
            get;
            private set;
        }

        #endregion Public Properties

        #region Public Constructors

        public ChatEventCtl(ChatMessage msg)
        {
            InitializeComponent();
            Message = msg.ToString();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            var receiver = Workspace.Services.Get<IChatService>();
            if (receiver != null)
            {
                receiver.MessageTimeline -= HandleMessageReceived;
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
            receiver.MessageTimeline += HandleMessageReceived;
        }

        #endregion Public Methods

        #region Private Methods

        private void HandleMessageReceived(object sender, ChatMessage message)
        {
            Message = message.ToString();
        }

        #endregion Private Methods

    }

}