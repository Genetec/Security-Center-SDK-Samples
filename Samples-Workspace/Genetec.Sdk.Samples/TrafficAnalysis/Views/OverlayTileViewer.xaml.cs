// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Genetec.Sdk.Workspace.Pages.Contents;
using TrafficAnalysis.Services;

namespace TrafficAnalysis.Views
{
    /// <summary>
    /// Interaction logic for OverlayTileViewer.xaml
    /// </summary>
    public partial class OverlayTileViewer
    {

        #region Private Fields

        private static readonly SolidColorBrush s_background;
        private static readonly TimeSpan s_offset = TimeSpan.FromMinutes(5);
        private static readonly Pen s_pen;
        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;
        private VideoContent m_content;
        private DispatcherTimer m_timer;

        #endregion Private Fields

        #region Public Constructors

        static OverlayTileViewer()
        {
            s_background = new SolidColorBrush(Color.FromArgb(0x30, 0x90, 0xEE, 0x90));
            s_background.Freeze();
            s_pen = new Pen(Brushes.White, 0.5);
            s_pen.Freeze();
        }

        public OverlayTileViewer(Genetec.Sdk.Workspace.Workspace workspace)
        {
            m_workspace = workspace;
            InitializeComponent();
            m_timer = new DispatcherTimer(TimeSpan.FromMilliseconds(100), DispatcherPriority.Render, OnTimerRenderTick, Dispatcher);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Update(VideoContent content) => m_content = content;

        #endregion Public Methods

        #region Protected Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (m_content != null)
            {
                var analysisService = m_workspace.Services.Get<IAnalysisService>();
                if (analysisService != null)
                {
                    var timestamp = m_content.VideoTime;

                    var data = analysisService.GetAnalysisData(m_content.EntityId, timestamp - s_offset, timestamp + s_offset);
                    if (data != null)
                    {
                        var renderSize = RenderSize;
                        var count = data.Count;
                        var samplesPerPixel = count / renderSize.Width;

                        var streamGeometry = new StreamGeometry();
                        var x = 0.0;
                        var y = renderSize.Height;
                        using (var geometryContext = streamGeometry.Open())
                        {
                            geometryContext.BeginFigure(new Point(x, y), true, true);
                            var points = new PointCollection(count);

                            foreach (var pt in data)
                            {
                                x += 1 / samplesPerPixel;
                                y = renderSize.Height - (pt.Value * 5.0);
                                points.Add(new Point(x, y));
                            }
                            points.Add(new Point(renderSize.Width, renderSize.Height));
                            geometryContext.PolyLineTo(points, true, true);
                        }

                        drawingContext.DrawGeometry(s_background, s_pen, streamGeometry);
                    }
                }
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private void OnTimerRenderTick(object sender, EventArgs eventArgs) => InvalidateVisual();

        #endregion Private Methods

    }
}