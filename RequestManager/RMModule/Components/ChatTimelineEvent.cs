// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.TimelineProvider;
using RMSerialization;
using System;
using System.Windows;
using System.Windows.Media;

namespace RMModule.Components
{

    public sealed class ChatTimelineEvent : TimelineEvent
    {

        #region Private Fields

        private readonly ChatEventCtl m_control;

        #endregion Private Fields

        #region Public Constructors

        public ChatTimelineEvent(DateTime timestamp, ChatMessage chatMsg)
            : base(timestamp)
        {
            m_control = new ChatEventCtl(chatMsg);
        }

        #endregion Public Constructors

        #region Public Methods

        public override TimelineVisual GetVisual(Rect constraint, double msPerPixel)
        {
            m_control.Height = Math.Max(0, constraint.Height - 4); // Leave some space above and below
            m_control.Width = m_control.Height;

            var visual = new TimelineVisual(m_control) {AlignmentY = AlignmentY.Center, AlignmentX = AlignmentX.Center};
            return visual;
        }

        #endregion Public Methods

    }

}