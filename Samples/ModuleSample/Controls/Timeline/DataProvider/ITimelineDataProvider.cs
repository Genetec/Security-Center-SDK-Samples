// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Controls.Timeline.Events;
using System;
using System.Collections.Generic;

namespace ModuleSample.Controls.Timeline.DataProvider
{

    /// <summary>
    /// Represents a provider of events to be displayed in the timeline
    /// </summary>
    public interface ITimelineDataProvider
    {
        #region Public Events

        /// <summary>
        /// Event fired when motions are received
        /// </summary>
        event EventHandler MotionsReceived;

        /// <summary>
        /// Event fired when sequences are received
        /// </summary>
        event EventHandler SequencesReceived;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Gets the current begin time
        /// </summary>
        DateTime BeginTime { get; }

        /// <summary>
        /// Gets Sets the current camera
        /// </summary>
        Guid Camera { get; set; }

        /// <summary>
        /// Gets the current end time
        /// </summary>
        DateTime EndTime { get; }

        /// <summary>
        /// Gets the received motions
        /// </summary>
        List<ITimelineEvent> Motions { get; }

        /// <summary>
        /// Gets the received sequences
        /// </summary>
        List<ITimelineEvent> Sequences { get; }

        /// <summary>
        /// Gets the camera time zone
        /// </summary>
        TimeZoneInfo TimeZone { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Set the current timeline range
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        void SetTimelineRange(DateTime begin, DateTime end);

        #endregion Public Methods
    }

}