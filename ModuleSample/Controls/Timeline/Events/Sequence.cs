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

    public class Sequence : ITimelineEvent
    {

        #region Public Properties

        public TimeSpan Duration { get; private set; }

        public DateTime EventTime { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public Sequence(DateTime beginTime, DateTime endTime)
                    : this(beginTime, endTime - beginTime)
        {
        }

        public Sequence(DateTime eventTime, TimeSpan duration)
        {
            EventTime = eventTime;
            Duration = duration;
        }

        #endregion Public Constructors

        #region Public Methods

        public Visual GetVisual(Rect constraint)
        {
            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();

            var color = Colors.White;
            dc.DrawRectangle(new SolidColorBrush(color), null, constraint);
            dc.Close();

            return visual;
        }

        #endregion Public Methods

    }

}