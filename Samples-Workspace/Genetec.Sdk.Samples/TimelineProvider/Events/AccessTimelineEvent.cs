// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;
using Genetec.Sdk.Workspace.Components.TimelineProvider;

namespace TimelineProvider.Events
{
    public sealed class AccessTimelineEvent : TimelineEvent
    {

        #region Private Fields

        private readonly Guid m_cardholderId;

        private readonly bool m_isGranted;

        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;

        #endregion Private Fields

        #region Public Constructors

        public AccessTimelineEvent(Genetec.Sdk.Workspace.Workspace workspace, Guid cardholderId, DateTime timestamp, bool isGranted)
            : base(timestamp)
        {
            m_workspace = workspace;
            m_cardholderId = cardholderId;
            m_isGranted = isGranted;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Gets a visual to display on the timeline
        /// </summary>
        /// <param name="constraint">Position constraint in the timeline</param>
        /// <param name="msPerPixel">Current timeline scale (Number of milliseconds per pixel)</param>
        public override TimelineVisual GetVisual(Rect constraint, double msPerPixel)
        {
            var ctl = new AccessTimelineEventView(m_workspace, m_cardholderId, m_isGranted)
            {
                Width = 10,
                Height = Math.Max(0, constraint.Height - 4)
            };

            var visual = new TimelineVisual(ctl)
            {
                AlignmentY = AlignmentY.Center,
                AlignmentX = AlignmentX.Center
            };
            return visual;
        }

        #endregion Public Methods

    }
}