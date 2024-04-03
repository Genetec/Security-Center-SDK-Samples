// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Controls.Timeline.Events;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace ModuleSample.Controls.Timeline.Layers
{
    /// <summary>
    /// Canvas control that displays timeline events
    /// </summary>
    public class EventsLayer : VisualsLayer
    {

        #region Public Methods

        public void Draw(DateTime beginTime, DateTime endTime, List<ITimelineEvent> events)
        {
            Clear();
            foreach (var sequence in events)
            {
                InsertEvent(sequence, beginTime, endTime);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void InsertEvent(ITimelineEvent timelineEvent, DateTime beginTime, DateTime endTime)
        {
            // Determine the UI constraints
            var constraint = GetConstraint(timelineEvent, InternalWidth, InternalHeight, beginTime, endTime);

            // Ensure the visual's constraint would appear inside the timeline's current view
            var maxWidth = InternalWidth;
            if (((constraint.Left < 0) && (constraint.Right < 0)) || (constraint.Left > maxWidth))
            {
                // Exit!
                return;
            }

            // Determine the X canvas position
            var posX = constraint.X;

            // Extreme negative canvas position can affect the visual appearance
            if (posX < 0 && constraint.Width + posX >= 0)
            {
                constraint.Width += posX;
                constraint.X = 0;
                posX = 0;
            }

            // Retrieve a visual object representing the event
            var visual = timelineEvent.GetVisual(constraint);
            if (visual == null)
                return;

            var width = constraint.Width;
            posX -= width;

            // Determine the Y canvas position
            const double posY = 0.0;

            // Set the canvas positions
            visual.SetValue(LeftProperty, posX);
            visual.SetValue(TopProperty, posY);

            AddChild(visual);
        }

        #endregion Private Methods

    }
}