// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Genetec.Sdk.Workspace.Components.TimelineProvider;
using TimelineProvider.Extensions;

namespace TimelineProvider.Events
{
    internal class RandomTimelineEvent : TimelineEvent
    {

        #region Private Fields

        /// <summary>
        /// Every second, there is a chance of getting 0 to 10 events in the FakeVideoAnalytic.
        /// </summary>
        private const double MAX_NUMBER_OF_EVENTS_DISPLAYED = 10;

        /// <summary>
        /// The displayed color on the timeline for this event.
        /// </summary>
        private static readonly Brush RandomEventColor = new SolidColorBrush(Color.FromRgb(255, 50, 50));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The number of events for this event.
        /// </summary>
        public long EventCount { get; set; }

        /// <summary>
        /// We trim the milliseconds, because processing every milliseconds would take a lot of resources.
        /// </summary>
        public DateTime TimestampNoMs => Timestamp.TrimMilliseconds();

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Constructor of the class RandomEvents.
        /// </summary>
        /// <param name="timestamp">The timestamp of the event.</param>
        /// <param name="eventCount">The number of events.</param>
        public RandomTimelineEvent(DateTime timestamp, long eventCount) : base(timestamp)
            => EventCount = eventCount;

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Overriding the equals makes this class handles the equals.
        /// Equals will be called when trying to clear the cache.
        /// If it always returns false, none of them will ever be removed from the tile.
        /// </summary>
        /// <param name="other">The other to compare with.</param>
        /// <returns>True if equal, false otherwise.</returns>
        public override bool Equals(TimelineEvent other)
        {
            var randomTimelineEvent = other as RandomTimelineEvent;
            return EventCount == randomTimelineEvent?.EventCount && TimestampNoMs == randomTimelineEvent.TimestampNoMs;
        }

        /// <summary>
        /// Method to get the visual from this Event.
        /// </summary>
        /// <param name="constraint">The constraint,</param>
        /// <param name="msPerPixel">The number of ms per pixel.</param>
        /// <returns>TimelineVisual</returns>
        public override TimelineVisual GetVisual(Rect constraint, double msPerPixel)
            => new TimelineVisual(BuildVisual(constraint, msPerPixel))
            {
                AlignmentY = AlignmentY.Bottom,
                AlignmentX = AlignmentX.Center
            };

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// We create a rectangle here to put on the timeline.
        /// </summary>
        /// <param name="constraint">The constraint,</param>
        /// <param name="msPerPixel">The number of ms per pixel.</param>
        /// <returns>The rectangle to draw.</returns>
        private Rectangle BuildVisual(Rect constraint, double msPerPixel)
            => new Rectangle
            {
                Fill = RandomEventColor,
                Opacity = 0.75,
                Height = Math.Max(0, EventCount / MAX_NUMBER_OF_EVENTS_DISPLAYED * (constraint.Height - 10)),
                Width = Math.Max(5, 1000 / msPerPixel)
            };

        #endregion Private Methods

    }
}