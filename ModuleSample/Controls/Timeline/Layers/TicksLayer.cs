// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ModuleSample.Controls.Timeline.Layers
{

    /// <summary>
    /// Canvas control that displays time ticks
    /// </summary>
    public class TicksLayer : VisualsLayer
    {

        #region Private Fields

        private const int TickSpacing = 10;

        /// <summary>
        /// All tick intervals
        /// </summary>
        private static readonly List<TickInterval> s_tickIntervals = new List<TickInterval>();
        /// <summary>
        /// Represent the current tick interval used depending on the time range
        /// </summary>
        private TickInterval m_tickInterval;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the list of tick intervals used depending on the time range
        /// </summary>
        public static List<TickInterval> TickIntervals => s_tickIntervals;

        #endregion Public Properties

        #region Public Structs

        public struct TickInterval
        {

            #region Private Fields

            #endregion Private Fields

            #region Public Properties

            public int MajorTick { get; }

            public int MilliSec { get; }

            #endregion Public Properties

            #region Public Constructors

            public TickInterval(int nMilliSec, int nMajorTick)
            {
                MilliSec = nMilliSec;
                MajorTick = nMajorTick;
            }

            #endregion Public Constructors

        }

        #endregion Public Structs

        #region Public Constructors

        public TicksLayer()
        {
            // Build the list of intervals
            TickIntervals.Add(new TickInterval(-1, 0));
            TickIntervals.Add(new TickInterval(1, 5)); //   1 milliseconds
            TickIntervals.Add(new TickInterval(2, 5)); //   2 milliseconds
            TickIntervals.Add(new TickInterval(10, 5)); //  10 milliseconds
            TickIntervals.Add(new TickInterval(50, 5)); //  50 milliseconds
            TickIntervals.Add(new TickInterval(100, 5)); // 0.1 second
            TickIntervals.Add(new TickInterval(200, 5)); // 0.2 second
            TickIntervals.Add(new TickInterval(1000, 5)); //   1 second
            TickIntervals.Add(new TickInterval(2000, 5)); //   2 seconds
            TickIntervals.Add(new TickInterval(5000, 6)); //  30 seconds
            TickIntervals.Add(new TickInterval(10000, 6)); //   1 minute
            TickIntervals.Add(new TickInterval(30000, 4)); //   2 minutes
            TickIntervals.Add(new TickInterval(60000, 5)); //   5 minutes
            TickIntervals.Add(new TickInterval(120000, 5)); //  10 minutes
            TickIntervals.Add(new TickInterval(300000, 6)); //  30 minutes
            TickIntervals.Add(new TickInterval(600000, 6)); //   1 hour
            TickIntervals.Add(new TickInterval(1800000, 4)); //   2 hours
            TickIntervals.Add(new TickInterval(3600000, 5)); //   5 hours
            TickIntervals.Add(new TickInterval(7200000, 6)); //  12 hours
            TickIntervals.Add(new TickInterval(14400000, 6)); //   1 day
            TickIntervals.Add(new TickInterval(28800000, 6)); //   2 days
            TickIntervals.Add(new TickInterval(86400000, 7)); //   1 week
            TickIntervals.Add(new TickInterval(172800000, 14)); //   2 weeks
        }

        #endregion Public Constructors

        #region Public Methods

        public void Draw(DateTime beginTime, DateTime endTime, TimeZoneInfo timeZone)
        {
            // Update the visuals
            Clear();

            if (InternalWidth == 0 || InternalHeight == 0 || beginTime == DateTime.MinValue || endTime == DateTime.MinValue)
                return;

            var currentCulture = CultureInfo.CurrentCulture;
            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();
            var penTicks = new Pen(new SolidColorBrush(Colors.DarkGray), 1.0);

            // Recalculate the best tick spacing for the current time range
            UpdateTickSpacing(beginTime, endTime);

            var bShowMilliseconds = m_tickInterval.MilliSec < 200;
            var bShowSeconds = m_tickInterval.MilliSec < 10000;
            var bShowMinutes = m_tickInterval.MilliSec < 600000;
            var bShowHours = m_tickInterval.MilliSec < 14400000;

            var nOffset = beginTime.Millisecond;
            if (!bShowMilliseconds) nOffset += (beginTime.Second * 1000);
            if (!bShowSeconds) nOffset += (beginTime.Minute * 60000);
            if (!bShowMinutes) nOffset += (beginTime.Hour * 3600000);
            if (!bShowHours) nOffset += (beginTime.Day * 86400000);

            // Determine the time format to display
            var dateTimeFormat = currentCulture.DateTimeFormat;
            var strTimeFormat = dateTimeFormat.ShortTimePattern;
            if (bShowHours && !bShowMinutes && !bShowSeconds)
                strTimeFormat = dateTimeFormat.AMDesignator == string.Empty ? "HH" : "h tt";
            else if (bShowHours && bShowMinutes && !bShowSeconds)
                strTimeFormat = dateTimeFormat.AMDesignator == string.Empty ? "HH:mm" : "h:mm";
            else if (bShowHours && bShowMinutes && bShowSeconds)
                strTimeFormat = dateTimeFormat.AMDesignator == string.Empty ? "HH:mm:ss" : "h:mm:ss";

            var tsTotal = endTime - beginTime;
            var nTickCount = (tsTotal.TotalMilliseconds + nOffset) / m_tickInterval.MilliSec;

            var dpiScale = VisualTreeHelper.GetDpi(this);
            var pixelsPerDip = dpiScale.PixelsPerDip;

            // Draw every tick
            for (var i = 0; i <= nTickCount; ++i)
            {
                var dt = beginTime + new TimeSpan(0, 0, 0, 0, i * m_tickInterval.MilliSec - nOffset);
                var posX = GetXFromPos(dt, beginTime, endTime, InternalWidth);

                // Convert to local time before displaying
                dt = TimeZoneInfo.ConvertTimeFromUtc(dt, timeZone);

                double nLength = 1;

                if (i % m_tickInterval.MajorTick == 0)
                {
                    nLength = 3;

                    var textTime = new FormattedText(dt.ToString(strTimeFormat, currentCulture),
                        currentCulture, FlowDirection.LeftToRight, new Typeface("Segoe UI"), 9, Brushes.White,
                        pixelsPerDip);
                    textTime.SetFontWeight(FontWeights.Normal);
                    var textTimeWidth = textTime.Width;

                    var textPosX = posX - textTimeWidth / 2;
                    if (textPosX > 0 && textPosX + textTimeWidth <= InternalWidth)
                    {
                        dc.DrawText(textTime, new Point((int)textPosX, 1));
                    }
                }

                if (i > 0 && posX >= 0 && posX <= InternalWidth)
                {
                    dc.DrawLine(penTicks, new Point((int)posX, 0), new Point((int)posX, nLength));
                    dc.DrawLine(penTicks, new Point((int)posX, InternalHeight - nLength), new Point((int)posX, InternalHeight));
                }
            }

            // Persist the drawing content.
            dc.Close();

            AddChild(visual);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Update the current tick spacing depending of the current time range
        /// </summary>
        private void UpdateTickSpacing(DateTime beginTime, DateTime endTime)
        {
            var tsTotal = endTime - beginTime;
            var nTickSpacing = TickSpacing * tsTotal.TotalMilliseconds / (InternalWidth - 2);

            // Find the best tick interval for the current time range
            for (var i = 0; i < TickIntervals.Count; ++i)
            {
                if (i < TickIntervals.Count - 1)
                {
                    if ((nTickSpacing >= TickIntervals[i].MilliSec) && (nTickSpacing < TickIntervals[i + 1].MilliSec))
                    {
                        m_tickInterval = TickIntervals[i + 1];
                        break;
                    }
                }
                else
                {
                    m_tickInterval = TickIntervals[i];
                    break;
                }
            }
        }

        #endregion Private Methods

    }

}