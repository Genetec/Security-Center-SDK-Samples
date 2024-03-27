// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Controls.Timeline.DataProvider;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace ModuleSample.Controls.Timeline
{

    /// <summary>
    /// Timeline user control
    /// </summary>
    public partial class Timeline : IDisposable
    {

        #region Private Fields

        /// <summary>
        /// Used to throttle cursor time changed
        /// </summary>
        private const int CursorTimeChangedRefreshRate = 500;

        /// <summary>
        /// Maximum timeline range in seconds
        /// </summary>
        private const int MaximumRange = 86400;

        /// <summary>
        /// Minimum timeline range in seconds
        /// </summary>
        private const int MinimumRange = 30;

        /// <summary>
        /// Timeline auto-scroll threshold
        /// </summary>
        private const double ScrollThreshold = 0.05;

        private bool m_cursorDragged;

        private DateTime m_cursorTime = DateTime.MinValue;

        private bool m_snapToCursor;

        private DispatcherTimer m_timerCursorTimeChanged;

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Indicates that the user released the cursor.
        /// Good moment to resume paused video.
        /// </summary>
        public event EventHandler CursorDragCompleted;

        /// <summary>
        /// Indicates that the user started a drag.
        /// Good moment to pause video.
        /// </summary>
        public event EventHandler CursorDragStarted;

        /// <summary>
        /// Indicates that the user changed the cursor position.
        /// Good moment to seek video.
        /// </summary>
        public event EventHandler CursorTimeChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Should be set to the current camera displayed.
        /// </summary>
        public Guid Camera
        {
            get => DataProvider.Camera;
            set => DataProvider.Camera = value;
        }

        /// <summary>
        /// Timeline cursor time. Should be set when a frame is rendered in playback mode.
        /// </summary>
        public DateTime CursorTime
        {
            get => m_cursorTime;
            set
            {
                // Cannot change cursor time externally while dragging
                if (IsDragging)
                    return;

                // Should always be utc
                if (value.Kind != DateTimeKind.Utc)
                {
                    m_cursorTime = new DateTime(value.Ticks, DateTimeKind.Utc);
                }
                else
                {
                    m_cursorTime = value;
                }

                AutoScroll();

                // Re-enable snap to cursor if cursor time is within the current time range
                if (m_cursorTime >= BeginTime && m_cursorTime <= EndTime)
                {
                    IsSnapToCursor = true;
                }

                UpdateCursorPosition();
            }
        }

        /// <summary>
        /// Gets the timeline data provider
        /// </summary>
        public ITimelineDataProvider DataProvider { get; private set; }

        /// <summary>
        /// Should be set to True when the video is in rewind
        /// </summary>
        public bool IsRewind { get; set; }

        #endregion Public Properties

        #region Private Properties

        private DateTime BeginTime => DataProvider.BeginTime;

        private DateTime EndTime => DataProvider.EndTime;

        private double InternalHeight
        {
            get
            {
                var nHeight = m_gridLayers.ActualHeight;
                if (nHeight > 0)
                    return nHeight;

                return 0;
            }
        }

        private double InternalWidth
        {
            get
            {
                var nWidth = m_gridLayers.ActualWidth;
                if (nWidth > 0)
                    return nWidth;

                return 0;
            }
        }

        private bool IsDragging { get; set; }

        private bool IsSnapToCursor
        {
            get => m_snapToCursor;
            set
            {
                var oldValue = m_snapToCursor;
                m_snapToCursor = value;
                if (m_snapToCursor && oldValue != m_snapToCursor)
                {
                    AutoScroll();
                }
            }
        }

        private double MillisecondsPerPixel
        {
            get
            {
                if (InternalWidth == 0)
                    return 1;

                var tsTotal = EndTime - BeginTime;
                return tsTotal.TotalMilliseconds / InternalWidth;
            }
        }

        #endregion Private Properties

        #region Public Constructors

        public Timeline() => InitializeComponent();
        

        #endregion Public Constructors

        #region Public Methods

        public void Dispose() => UnsubscribeProvider();
        
        /// <summary>
        /// Initialize the timeline with your preferred data provider.
        /// </summary>
        /// <param name="dataProvider"></param>
        public void Initialize(ITimelineDataProvider dataProvider)
        {
            DataProvider = dataProvider;
            SubscribeProvider();
        }

        /// <summary>
        /// Set the current timeline range.
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        public void SetTimelineRange(DateTime begin, DateTime end)
        {
            DataProvider.SetTimelineRange(begin, end);
            DrawLayers();
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            DrawLayers();
        }

        #endregion Protected Methods

        #region Private Methods

        private void AutoScroll()
        {
            // Protect against invalid values
            if (BeginTime == DateTime.MinValue || EndTime == DateTime.MinValue || CursorTime == DateTime.MinValue || EndTime <= BeginTime)
                return;

            // Do not auto-scroll if snap is disabled
            if (!IsSnapToCursor)
                return;

            var bRefreshTimeline = false;
            var tsTotal = EndTime - BeginTime;

            // Calculate the threshold in seconds
            var nThreshold = (int)Math.Floor(tsTotal.TotalSeconds * ScrollThreshold);
            var nThresholdInverse = (int)Math.Ceiling(tsTotal.TotalSeconds * (1 - ScrollThreshold));

            var newBeginTime = BeginTime;
            var newEndTime = EndTime;

            // Check if we are in reverse mode
            if (IsRewind && (CursorTime < BeginTime + new TimeSpan(0, 0, 0, nThreshold) || CursorTime > EndTime))
            {
                newBeginTime = CursorTime - new TimeSpan(0, 0, 0, nThresholdInverse);
                newEndTime = CursorTime + new TimeSpan(0, 0, 0, nThreshold);
                bRefreshTimeline = true;
            }
            else if (!IsRewind && (CursorTime > EndTime - new TimeSpan(0, 0, 0, nThreshold) || CursorTime < BeginTime))
            {
                newBeginTime = CursorTime - new TimeSpan(0, 0, 0, nThreshold);
                newEndTime = CursorTime + new TimeSpan(0, 0, 0, nThresholdInverse);
                bRefreshTimeline = true;
            }

            if (!bRefreshTimeline)
                return;

            // Update the range of the timeline
            SetTimelineRange(newBeginTime, newEndTime);
        }

        private void DrawFuture()
        {
            m_layerFuture.Clear();

            var visual = new DrawingVisual();
            var dc = visual.RenderOpen();

            // Determine the position of the future start
            var futurePos = (int)GetXFromPos(DateTime.UtcNow);
            if (futurePos < 0)
            {
                futurePos = 0;
            }

            var futureWidth = (int)InternalWidth - futurePos;
            if (futureWidth < 0)
            {
                futureWidth = 0;
            }

            // Draw the future rectangle
            if (futureWidth > 0)
            {
                dc.DrawRectangle(new SolidColorBrush(Color.FromRgb(102, 51, 255)), null, new Rect(futurePos, 0, futureWidth, InternalHeight));
            }

            dc.Close();

            m_layerFuture.AddChild(visual);
        }

        private void DrawLayers()
        {
            DrawTicks();
            DrawFuture();
            DrawSequences();
            DrawMotions();
            UpdateCursorPosition();
        }

        private void DrawMotions() => m_layerMotions.Draw(BeginTime, EndTime, DataProvider.Motions);
       
        private void DrawSequences() => m_layerSequences.Draw(BeginTime, EndTime, DataProvider.Sequences);
       
        private void DrawTicks() => m_layerTicks.Draw(BeginTime, EndTime, DataProvider.TimeZone);
        
        private DateTime GetPosFromX(double nX)
        {
            var tsTotal = EndTime - BeginTime;
            var dtPos = BeginTime + new TimeSpan(0, 0, 0, 0, (int)(nX * tsTotal.TotalMilliseconds / InternalWidth));
            return dtPos;
        }

        private double GetXFromPos(DateTime dt) => GetXFromPos(dt, BeginTime, EndTime, InternalWidth);
        
        private double GetXFromPos(DateTime dt, DateTime beginTime, DateTime endTime, double width)
        {
            if (endTime == beginTime)
                return 0;

            var tsPos = dt - beginTime;
            var tsTotal = endTime - beginTime;

            return tsPos.TotalMilliseconds * width / tsTotal.TotalMilliseconds;
        }

        private void OnButtonScrollLeft(object sender, RoutedEventArgs e)
        {
            IsSnapToCursor = false;
            var range = EndTime - BeginTime;
            SetTimelineRange(BeginTime - range, BeginTime);
        }

        private void OnButtonScrollRight(object sender, RoutedEventArgs e)
        {
            IsSnapToCursor = false;
            var range = EndTime - BeginTime;
            SetTimelineRange(EndTime, EndTime + range);
        }

        private void OnButtonZoomIn(object sender, RoutedEventArgs e)
        {
            var origin = IsSnapToCursor ? GetXFromPos(CursorTime) / InternalWidth : 0.5;
            var tsLength = EndTime - BeginTime;
            if (tsLength.TotalSeconds <= MinimumRange)
                return;

            var nNewLength = (int)tsLength.TotalSeconds / 2;
            if (nNewLength < MinimumRange)
            {
                nNewLength = MinimumRange;
            }

            var tsNewLength = new TimeSpan(0, 0, nNewLength);
            var centerTime = BeginTime + TimeSpan.FromTicks((long)(tsLength.Ticks * origin));
            var offset = TimeSpan.FromTicks((long)(tsNewLength.Ticks * origin));
            SetTimelineRange(centerTime - offset, centerTime + tsNewLength - offset);
        }

        private void OnButtonZoomOut(object sender, RoutedEventArgs e)
        {
            const double origin = 0.5;
            var tsLength = EndTime - BeginTime;
            if (tsLength.TotalSeconds >= MaximumRange)
                return;

            var nNewLength = (int)tsLength.TotalSeconds * 2;
            if (nNewLength > MaximumRange)
            {
                nNewLength = MaximumRange;
            }

            var tsNewLength = new TimeSpan(0, 0, nNewLength);
            var centerTime = BeginTime + TimeSpan.FromTicks((long)(tsLength.Ticks * origin));
            var offset = TimeSpan.FromTicks((long)(tsNewLength.Ticks * origin));
            SetTimelineRange(centerTime - offset, centerTime + tsNewLength - offset);
        }

        private void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pt = e.GetPosition(m_gridLayers);
            m_cursorTime = GetPosFromX(pt.X);
            UpdateCursorPosition();

            CursorTimeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnProviderMotionsReceived(object sender, EventArgs e) => DrawMotions();
       
        private void OnProviderSequencesReceived(object sender, EventArgs e) => DrawSequences();
       
        private void OnThumbCursorDragCompleted(object sender, DragCompletedEventArgs e)
        {
            IsDragging = false;

            m_timerCursorTimeChanged.Stop();
            m_cursorDragged = false;

            CursorTimeChanged?.Invoke(this, EventArgs.Empty);

            CursorDragCompleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnThumbCursorDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (IsDragging)
            {
                m_cursorTime = m_cursorTime + new TimeSpan(0, 0, 0, 0, (int)(e.HorizontalChange * MillisecondsPerPixel));
                m_cursorDragged = true;
                UpdateCursorPosition();
            }
        }

        private void OnThumbCursorDragStarted(object sender, DragStartedEventArgs e)
        {
            IsDragging = true;

            CursorDragStarted?.Invoke(this, EventArgs.Empty);

            m_cursorDragged = true;
            m_timerCursorTimeChanged = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, CursorTimeChangedRefreshRate), DispatcherPriority.Normal, OnTimerCursorTimeChanged, Dispatcher);
        }

        private void OnTimerCursorTimeChanged(object sender, EventArgs e)
        {
            if (m_cursorDragged)
            {
                m_cursorDragged = false;
                CursorTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void SubscribeProvider()
        {
            DataProvider.SequencesReceived += OnProviderSequencesReceived;
            DataProvider.MotionsReceived += OnProviderMotionsReceived;
        }

        private void UnsubscribeProvider()
        {
            DataProvider.SequencesReceived -= OnProviderSequencesReceived;
            DataProvider.MotionsReceived -= OnProviderMotionsReceived;
        }

        private void UpdateCursorPosition()
        {
            var nX = GetXFromPos(CursorTime);
            m_thumb.SetValue(Canvas.LeftProperty, nX - m_thumb.ActualWidth / 2);
        }

        #endregion Private Methods

    }

}