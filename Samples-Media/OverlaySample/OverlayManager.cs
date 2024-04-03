using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Media.Overlay;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace OverlaySample
{
    #region Classes

    /// <summary>
    /// Regroup all overlay manipulation logic
    /// </summary>
    internal class OverlayManager
    {
        #region Constants

        public const int DrawingHeight = 480;

        public const int DrawingWidth = 640;

        private const string HourLayerGuid = "E452862F-195F-46B6-8516-8257EABBB5B8";

        private const int LayerPoolSize = 25;

        private readonly Pen m_contourPen = new Pen(Brushes.Transparent, 0);

        private readonly double m_pixelsPerDip;

        private readonly Typeface m_font = new Typeface("Calibri");

        private readonly Brush m_limeGreenBrush = new SolidColorBrush(Colors.LimeGreen);

        private readonly Random m_random = new Random();

        private readonly Engine m_sdkEngine;

        private const string MinuteLayerGuid = "2EB51D7D-9F65-4CD2-BF56-750A7F61AEE4";

        private const string SecondLayerGuid = "B56D9DFF-47D1-4F81-9829-A7C7BF90F4CC";

        #endregion

        #region Constructors

        public OverlayManager(Engine sdkEngine, double pixelsPerDip)
        {
            m_sdkEngine = sdkEngine;
            m_pixelsPerDip = pixelsPerDip;

            // It is important to free all Pens and Brushes used to generate the layers because 
            // overlay final composition is done on a different dispatcher.
            m_contourPen.Freeze();
            m_limeGreenBrush.Freeze();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize the overlay and layers for manual editing of the stream
        /// </summary>
        public void CreateEditLayers(Guid camera, MetadataStreamModel stream)
        {
            // Get the concrete overlay stream selected
            stream.Overlay = OverlayFactory.Get(camera, stream.EntityName);

            // Initialize the stream size
            stream.Overlay.Initialize(DrawingHeight, DrawingWidth);

            // Create a pool of layers for drawing that we will reuse
            // Those are short lived layers, we don't reuse their guids since we won't need to modify them
            for (int i = 0; i < LayerPoolSize; i++)
            {
                Layer l = stream.Overlay.CreateLayer(Guid.NewGuid(), stream.EntityName + i);
                l.Duration = TimeSpan.FromSeconds(i);
                stream.EditingLayers.Enqueue(l);
            }
        }

        /// <summary>
        /// Create a new metadata stream for a camera
        /// </summary>
        public void CreateNewStream(Guid camera, string name)
        {
            // Creating a metadata stream simply consist in getting an inexistant name from the factory
            // If the name doesn't exist, the stream will be created and will begin to appear in the entity metadata streams list
            OverlayFactory.Get(camera, name).Dispose();
        }

        /// <summary>
        /// Initialize the overlay and layers for the automatic time generation
        /// </summary>
        public void CreateTimeLayers(Guid camera, MetadataStreamModel stream)
        {
            // Get the concrete overlay stream selected
            stream.Overlay = OverlayFactory.Get(camera, stream.EntityName);

            // Initialize the stream size
            stream.Overlay.Initialize(DrawingHeight, DrawingWidth);

            // We use constant Guids so we can remove and update them. This can be useful if we want to invalidate
            // for example the HourLayer before the hour is passed
            stream.HourLayer = stream.Overlay.CreateLayer(new Guid(HourLayerGuid), "Hour layer");
            stream.MinuteLayer = stream.Overlay.CreateLayer(new Guid(MinuteLayerGuid), "Minute layer");
            stream.SecondLayer = stream.Overlay.CreateLayer(new Guid(SecondLayerGuid), "Second layer");

            // Update all three streams immediately with current time
            UpdateHourLayer(stream.HourLayer, DateTime.Now);
            UpdateMinuteLayer(stream.MinuteLayer, DateTime.Now);
            UpdateSecondLayer(stream.SecondLayer, DateTime.Now);
        }

        /// <summary>
        /// Dispose the overlay and layers used for manual editing of the stream
        /// </summary>
        /// <param name="stream"></param>
        public void DisposeEditLayers(MetadataStreamModel stream)
        {
            while (stream.EditingLayers.Count > 0)
                stream.EditingLayers.Dequeue().Dispose();
            if (stream.Overlay != null)
            {
                stream.Overlay.Dispose();
                stream.Overlay = null;
            }
        }

        /// <summary>
        /// Dispose all overlay related data from this stream
        /// </summary>
        public void DisposeOverlay(MetadataStreamModel stream)
        {
            while (stream.EditingLayers.Count > 0)
                stream.EditingLayers.Dequeue().Dispose();
            DisposeTimeLayers(stream);
        }

        public void DisposeTimeLayers(MetadataStreamModel stream)
        {
            if (stream.HourLayer != null)
            {
                stream.HourLayer.Dispose();
                stream.MinuteLayer.Dispose();
                stream.SecondLayer.Dispose();

                stream.HourLayer = null;
                stream.MinuteLayer = null;
                stream.SecondLayer = null;
            }

            if (stream.Overlay != null)
            {
                stream.Overlay.Dispose();
                stream.Overlay = null;
            }
        }

        /// <summary>
        /// Draw a randomly colored point at the specified coordinates on this stream
        /// </summary>
        public void DrawPoint(MetadataStreamModel stream, Point position)
        {
            Layer nextLayer = stream.EditingLayers.Dequeue();
            stream.EditingLayers.Enqueue(nextLayer);

            Brush brush = new SolidColorBrush(GetRandomColor());
            brush.Freeze();

            nextLayer.DrawEllipse(brush, m_contourPen, position, 5, 5);
            nextLayer.Update();
            nextLayer.Clear();
        }

        /// <summary>
        /// Get all the metadata streams associated to the camera guid received
        /// </summary>
        public ReadOnlyCollection<MetadataStreamInfo> GetEntityMetadataStreams(Guid entity)
        {
            Camera camera = m_sdkEngine.GetEntity(entity) as Camera;
            if (camera != null)
                return camera.MetadataStreams;
            return null;
        }

        /// <summary>
        /// Update the time layers of this stream
        /// </summary>
        public void UpdateTime(MetadataStreamModel stream)
        {
            DateTime now = DateTime.Now;
            if (now.Minute == 0)
                UpdateHourLayer(stream.HourLayer, now);
            if (now.Second == 0)
                UpdateMinuteLayer(stream.MinuteLayer, now);
            UpdateSecondLayer(stream.SecondLayer, now);
        }

        #endregion

        #region Private Methods

        private Color GetRandomColor()
        {
            return Color.FromRgb((byte)m_random.Next(255), (byte)m_random.Next(255), (byte)m_random.Next(255));
        }

        private void UpdateHourLayer(Layer layer, DateTime time)
        {
            var text = new FormattedText(time.Hour.ToString("00h"), CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight, m_font, 40, m_limeGreenBrush, m_pixelsPerDip);

            layer.Duration = time.AddHours(1)
                .Subtract(TimeSpan.FromSeconds(time.Second))
                .Subtract(TimeSpan.FromMinutes(time.Minute)) - time; //Until next hour
            layer.DrawText(text, new Point(5, 20));
            layer.Update();
            layer.Clear();
        }

        private void UpdateMinuteLayer(Layer layer, DateTime time)
        {
            var text = new FormattedText(time.Minute.ToString("00"), CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight, m_font, 40, m_limeGreenBrush, m_pixelsPerDip);

            layer.Duration = time.AddMinutes(1).Subtract(TimeSpan.FromSeconds(time.Second)) - time; //Until next minute
            layer.DrawText(text, new Point(65, 20));
            layer.Update();
            layer.Clear();
        }

        private void UpdateSecondLayer(Layer layer, DateTime time)
        {
            var text = new FormattedText(time.Second.ToString(":00"), CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight, m_font, 40, m_limeGreenBrush, m_pixelsPerDip);

            layer.Duration = TimeSpan.FromSeconds(1);
            layer.DrawText(text, new Point(105, 20));
            layer.Update();
            layer.Clear();
        }

        #endregion
    }

    #endregion
}

