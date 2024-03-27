// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;

namespace ModuleSample.Controls.Timeline.Events
{

    /// <summary>
    /// Represents a timeline event
    /// </summary>
    public interface ITimelineEvent
    {
        #region Public Properties

        /// <summary>
        /// Gets the event duration
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets the event time
        /// </summary>
        DateTime EventTime { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Gets the visual object to be displayed in the timeline
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        Visual GetVisual(Rect constraint);

        #endregion Public Methods
    }

}