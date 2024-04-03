// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Services;
using RMSerialization;
using System;
using System.Collections.Generic;

namespace RMModule.Services
{

    /// <summary>
    /// Basic interface for chat service
    /// Abstraction of the ChatService that components can access
    /// </summary>
    public interface IChatService : IService
    {
        #region Public Events

        event EventHandler<int> MessagesArchived;

        event EventHandler<List<ChatMessage>> MessagesReceived;

        event EventHandler<ChatMessage> MessageTimeline;

        #endregion Public Events

        #region Public Methods

        List<ChatMessage> GetMessages();

        List<Guid> SendMessage(ChatMessage msg, List<Guid> sendGuids);

        #endregion Public Methods
    }

}