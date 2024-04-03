// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Maps;
using HeatMapLayer.Providers;

namespace HeatMapLayer.Layers
{
    public sealed class MotionHeatMapLayer : MapLayer
    {

        #region Private Fields

        private readonly Dictionary<GeoCoordinate, int> m_positions = new Dictionary<GeoCoordinate, int>();
        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;
        private HeatMapPanel m_panel;
        private CarMapObjectProvider m_provider;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the layer's unique identifier
        /// </summary>
        public override Guid Id => new Guid("{6EEFFC9C-FF4D-4134-9164-8503E9EF9A82}");

        /// <summary>
        /// Gets the layer's initial priority
        /// </summary>
        public override int InitialPriority => -10000;

        /// <summary>
        /// Gets the layer's name
        /// </summary>
        public override string Name => "Heat Map";

        #endregion Public Properties

        #region Internal Properties

        internal IDictionary<GeoCoordinate, int> Positions => m_positions;

        internal object SyncRoot { get; } = new object();

        #endregion Internal Properties

        #region Public Constructors

        public MotionHeatMapLayer(Genetec.Sdk.Workspace.Workspace workspace, CarMapObjectProvider provider)
        {
            m_workspace = workspace;
            m_provider = provider;
            m_provider.NewPosition += OnProviderNewPosition;
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(MapContext context)
        {
            base.Update(context);
            InvalidateRender();
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Create a panel for hosting the map objects
        /// </summary>
        /// <returns>Panel created</returns>
        protected override UIElement CreatePanel() => m_panel ?? (m_panel = new HeatMapPanel(m_workspace, this));

        protected override void OnMapChanged()
        {
            base.OnMapChanged();
            InvalidateRender();
        }

        protected override void OnViewAreaChanged()
        {
            base.OnViewAreaChanged();
            InvalidateRender();
        }

        #endregion Protected Methods

        #region Private Methods

        private void InvalidateRender() => Dispatcher.BeginInvoke(new Action(m_panel.InvalidateVisual));

        private void OnProviderNewPosition(object sender, GeoCoordinate coord)
        {
            lock (SyncRoot)
            {
                if (!m_positions.TryGetValue(coord, out var amplitude))
                {
                    m_positions.Add(coord, amplitude + 1);
                }
                else
                {
                    m_positions[coord] = amplitude + 1;
                }
            }

            if (m_panel != null)
            {
                InvalidateRender();
            }
        }

        #endregion Private Methods

        #region Private Classes

        private sealed class HeatMapPanel : UIElement
        {

            #region Private Fields

            private readonly MotionHeatMapLayer m_layer;
            private readonly Genetec.Sdk.Workspace.Workspace m_workspace;

            #endregion Private Fields

            #region Private Properties

            private static double HeatRadius => 30.0;

            #endregion Private Properties

            #region Public Constructors

            public HeatMapPanel(Genetec.Sdk.Workspace.Workspace workspace, MotionHeatMapLayer layer)
            {
                m_workspace = workspace;
                m_layer = layer;
                IsHitTestVisible = false;
            }

            #endregion Public Constructors

            #region Protected Methods

            protected override void OnRender(DrawingContext dc)
            {
                var radialBrush = new RadialGradientBrush();

                if (m_layer != null)
                {
                    var map = m_workspace.Sdk.GetEntity(m_layer.Map) as Map;
                    if ((map != null) && map.IsGeoReferenced)
                    {
                        var scale = m_layer.GetScaleFactor();
                        if (scale > 0.0025)
                        {
                            lock (m_layer.SyncRoot)
                            {
                                foreach (var kv in m_layer.Positions)
                                {
                                    var coord = kv.Key;
                                    var amplitude = kv.Value;

                                    radialBrush.GradientStops.Clear();
                                    radialBrush.GradientStops.Add(new GradientStop(Color.FromArgb((byte)(amplitude * 80), 0, 0, 0), 0.0));
                                    radialBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0, 0, 0, 0), 1));

                                    var pixel = m_layer.CoordinateToPixel(coord);
                                    dc.DrawRectangle(radialBrush, null, new Rect(pixel.X - HeatRadius / 2, pixel.Y - HeatRadius / 2, HeatRadius * 2, HeatRadius * 2));
                                }
                            }
                        }
                    }
                }
            }

            #endregion Protected Methods

        }

        #endregion Private Classes
    }
}