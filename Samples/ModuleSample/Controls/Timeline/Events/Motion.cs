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

    public class Motion : ITimelineEvent
    {

        #region Public Properties

        public TimeSpan Duration { get; private set; }

        public DateTime EventTime { get; private set; }

        public uint Value { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public Motion(DateTime eventTime, TimeSpan duration, uint value)
        {
            EventTime = eventTime;
            Duration = duration;
            Value = value;
        }

        #endregion Public Constructors

        #region Public Methods

        public Visual GetVisual(Rect constraint)
        {
            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();

            var rect = constraint;
            rect.Height = constraint.Height * Value / 100;
            rect.Y = constraint.Height - rect.Height;

            var color = Colors.Green;
            color.A = 192;
            dc.DrawRectangle(new SolidColorBrush(color), null, rect);
            dc.Close();

            return visual;
        }

        #endregion Public Methods

    }

}