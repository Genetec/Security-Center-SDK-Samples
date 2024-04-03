// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.TimelineProvider;
using Genetec.Sdk.Workspace.Pages.Contents;
using RMModule.Services;
using RMSerialization;
using System;

namespace RMModule.Components
{

    public sealed class ChatTimelineProvider : TimelineProvider
    {

        #region Private Fields

        private Workspace m_workspace;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Initialize the timeline provider
        /// </summary>
        /// <param name="workspace">Application's workspace</param>
        public void Initialize(Workspace workspace)
        {
            m_workspace = workspace;
            var receiver = workspace.Services.Get<IChatService>();
            if (receiver != null)
            {
                receiver.MessageTimeline += OnMessageReceived;
            }
        }

        /// <summary>
        /// Query timeline event for the specified content within the specified time range
        /// </summary>
        /// <param name="contentGroup">Content group currently hooked in the timeline</param>
        /// <param name="startTime">Timeline's range start time</param>
        /// <param name="endTime">Timeline's range end time</param>
        public override void Query(ContentGroup contentGroup, DateTime startTime, DateTime endTime)
        {
            // Here, launch any queries asynchronously to retrieve the information to display in the timeline
            // Then, use InsertEvent for each item to display
        }

        #endregion Public Methods

        #region Private Methods

        private void OnMessageReceived(object sender, ChatMessage chatMessage)
        {
            InsertEvent(new ChatTimelineEvent(chatMessage.TimeStamp, chatMessage));
        }

        #endregion Private Methods
    }

}