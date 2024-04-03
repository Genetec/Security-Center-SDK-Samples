// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using RMModule.Services;
using RMSerialization;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace RMModule.Pages
{

    /// <summary>
    /// Interaction logic for ChatPageView.xaml
    /// </summary>
    public partial class ChatPageView : UserControl
    {

        #region Public Fields

        public static readonly DependencyProperty MessageToSendProperty = 
            DependencyProperty.Register("MessageToSend", typeof(string), typeof(ChatPageView), new PropertyMetadata(""));

        public static readonly DependencyProperty ReceivedTextProperty = 
            DependencyProperty.Register("ReceivedText", typeof(string), typeof(ChatPageView), new PropertyMetadata(""));

        public List<Guid> m_sendGuids = new List<Guid>();

        #endregion Public Fields

        #region Public Properties

        public string MessageToSend
        {
            get => (string)GetValue(MessageToSendProperty);
            set => SetValue(MessageToSendProperty, value);
        }

        public string ReceivedText
        {
            get => (string)GetValue(ReceivedTextProperty);
            set => SetValue(ReceivedTextProperty, value);
        }

        /// <summary>
        /// Gets the application's workspace.
        /// </summary>
        public Workspace Workspace { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public ChatPageView() => InitializeComponent();
        
        #endregion Public Constructors

        #region Public Methods

        public void AddSendGuid(Guid sendGuid)
        {
            if (sendGuid != Workspace.Sdk.ClientGuid && !m_sendGuids.Contains(sendGuid))
            {
                m_sendGuids.Add(sendGuid);
            }
        }

        public void Initialize(Workspace workspace) => Workspace = workspace;
        
        #endregion Public Methods

        #region Private Methods

        private void OnButtonSendClick(object sender, RoutedEventArgs e) => SendMessage();
       
        private void OnTextBoxKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                SendMessage();
            }
        }
        private void SendMessage()
        {
            var msg = new ChatMessage { Content = MessageToSend, TimeStamp = DateTime.UtcNow, AppGuid = Workspace.Sdk.ClientGuid };
            var chatService = Workspace.Services.Get<IChatService>();
            var invalidGuids = chatService.SendMessage(msg, m_sendGuids);
            foreach (var guid in invalidGuids)
            {
                m_sendGuids.Remove(guid);
            }
            if (invalidGuids.Count == 0)
            {
                MessageToSend = "";
            }
        }

        #endregion Private Methods

    }

}