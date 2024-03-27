// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workflows;
using Genetec.Sdk.Workspace;
using RMSerialization;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RMModule.Services
{

    public sealed class ChatService : DependencyObject, IChatService
    {

        #region Private Fields

        private readonly List<ChatMessage> m_archives = new List<ChatMessage>();

        private Workspace m_workspace;

        #endregion Private Fields

        #region Public Events

        public event EventHandler<int> MessagesArchived;

        public event EventHandler<List<ChatMessage>> MessagesReceived;
        public event EventHandler<ChatMessage> MessageTimeline;

        #endregion Public Events

        #region Public Methods

        public List<ChatMessage> GetMessages()
        {
            var tempList = new List<ChatMessage>(m_archives);
            m_archives.Clear();
            MessagesArchived?.Invoke(this, m_archives.Count);
            return tempList;
        }

        public void Initialize(Workspace workspace)
        {
            m_workspace = workspace;
            Action<ChatMessage, RequestCompletion<bool>> handleRequest = HandleRequest;
            m_workspace.Sdk.RequestManager.RemoveRequestHandler(handleRequest);
            m_workspace.Sdk.RequestManager.AddRequestHandler(handleRequest);
        }

        public List<Guid> SendMessage(ChatMessage msg, List<Guid> sendGuids)
        {
            var invalidGuids = new List<Guid>();
            if (sendGuids.Count > 0)
            {
                var received = false;
                foreach (var sendGuid in sendGuids)
                {
                    try
                    {
                        // In this sample, the return value is a boolean confirming that the message was received.
                        // In fact, if the message is not received, the call throws an exception.
                        received = m_workspace.Sdk.RequestManager.SendRequest<ChatMessage, bool>(sendGuid, msg);
                    }
                    catch (Exception)
                    {
                        invalidGuids.Add(sendGuid);
                        ArchiveMessage(new ChatMessage { Content = "Invalid end point", TimeStamp = DateTime.UtcNow, AppGuid = m_workspace.Sdk.ClientGuid });
                    }
                }
                if (received)
                {
                    ArchiveMessage(msg);
                }
            }
            else
            {
                ArchiveMessage(new ChatMessage { Content = "No client connected, send a message from a RMApplication", TimeStamp = DateTime.UtcNow, AppGuid = m_workspace.Sdk.ClientGuid });
            }
            return invalidGuids;
        }

        #endregion Public Methods

        #region Private Methods

        private void ArchiveMessage(ChatMessage msg)
        {
            m_archives.Add(msg);
            if (MessageTimeline != null)
            {
                if (msg.AppGuid != m_workspace.Sdk.ClientGuid)
                {
                    MessageTimeline(this, msg);
                }
            }
            if (MessagesReceived != null)
            {
                MessagesReceived(this, m_archives);
                m_archives.Clear();
            }

            MessagesArchived?.Invoke(this, m_archives.Count);
        }

        private void HandleRequest(ChatMessage req, RequestCompletion<bool> ret)
        {
            ret.SetResponse(true);
            Dispatcher?.BeginInvoke((Action)(() => ArchiveMessage(req)));
        }

        #endregion Private Methods

    }

}