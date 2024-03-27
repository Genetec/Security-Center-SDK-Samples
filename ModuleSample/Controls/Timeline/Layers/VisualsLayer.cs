// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Controls.Timeline.Events;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModuleSample.Controls.Timeline.Layers
{

    /// <summary>
    /// Canvas control that displays Visual objects
    /// </summary>
    public class VisualsLayer : Canvas
    {

        #region Private Fields

        private readonly VisualCollection m_colVisuals;

        #endregion Private Fields

        #region Protected Properties

        protected double InternalHeight
        {
            get
            {
                var nHeight = ActualHeight;
                if (nHeight > 0)
                    return nHeight;

                return 0;
            }
        }

        protected double InternalWidth
        {
            get
            {
                var nWidth = ActualWidth;
                if (nWidth > 0)
                    return nWidth;

                return 0;
            }
        }

        protected override int VisualChildrenCount => m_colVisuals.Count;

        #endregion Protected Properties

        #region Public Constructors

        public VisualsLayer() => m_colVisuals = new VisualCollection(this);
       
        #endregion Public Constructors

        #region Public Methods

        public void AddChild(Visual visual) => m_colVisuals.Add(visual);
        
        public void Clear()
        {
            m_colVisuals.Clear();
            Children.Clear();
        }

        #endregion Public Methods

        #region Protected Methods

        protected Rect GetConstraint(ITimelineEvent timelineEvent, double maxWidth, double maxHeight, DateTime beginTime, DateTime endTime)
        {
            var posX = GetXFromPos(timelineEvent.EventTime, beginTime, endTime, maxWidth);

            var constraint = new Rect(posX, 0, 3, maxHeight);
            var x = GetXFromPos(timelineEvent.EventTime + timelineEvent.Duration, beginTime, endTime, maxWidth);
            constraint.Width = Math.Max(0, x - constraint.Left);

            return constraint;
        }

        protected override Visual GetVisualChild(int index) => m_colVisuals[index];
       
        protected double GetXFromPos(DateTime dt, DateTime beginTime, DateTime endTime, double width)
        {
            if (endTime == beginTime)
                return 0;

            var tsPos = dt - beginTime;
            var tsTotal = endTime - beginTime;

            return (tsPos.TotalMilliseconds * width / tsTotal.TotalMilliseconds);
        }

        #endregion Protected Methods

    }

}