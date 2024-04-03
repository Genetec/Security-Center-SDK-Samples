// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Pages.Contents;
using Genetec.Sdk.Workspace.Services;
using MapsPlayer.Layers;

namespace MapsPlayer.Views
{
    public class DoorVisualView : MapObjectVisual
    {

        #region Private Fields

        private static ImageDrawing s_door16;
        /// <summary>
        /// The image of the door
        /// </summary>
        private static ImageDrawing s_door32;

        private static ImageDrawing s_unlinkedDoor16;
        /// <summary>
        /// The image of the door while linked object has been removed
        /// </summary>
        private static ImageDrawing s_unlinkedDoor32;

        private readonly Guid m_linkedAeosEntityId;

        /// <summary>
        /// The tooltip object to display on map
        /// </summary>
        private readonly ToolTip m_tooltip = new ToolTip();

        /// <summary>
        /// A timer used to decide if the m_tooltip has to be displayed
        /// </summary>
        private readonly Timer m_tooltipTimer = new Timer(800);

        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;
        /// <summary>
        /// Used by the dispose pattern to know if the resources have been disposed
        /// </summary>
        private bool m_isDisposed;

        private int m_renderSize = 16;

        #endregion Private Fields

        #region Public Properties

        public int RenderSize
        {
            get => m_renderSize;
            set
            {
                if (value != m_renderSize)
                {
                    m_renderSize = value;
                    Render();
                }
            }
        }

        #endregion Public Properties

        #region Public Constructors

        public DoorVisualView(Genetec.Sdk.Workspace.Workspace workspace, MapObject mapObject)
        {
            m_workspace = workspace;
            m_linkedAeosEntityId = mapObject.LinkedEntity;

            LayerId = PerformanceLayer.Identifier;

            InitializeDoor(ref s_door32, "Large_door_opened.png", 32);
            InitializeDoor(ref s_unlinkedDoor32, "Large_door_opened_red.png", 32);
            InitializeDoor(ref s_door16, "Large_door_opened.png", 16);
            InitializeDoor(ref s_unlinkedDoor16, "Large_door_opened_red.png", 16);

            m_tooltip.Background = Brushes.Transparent;
            m_tooltip.BorderThickness = new Thickness(0.0);
            m_tooltip.BorderBrush = Brushes.Transparent;
            m_tooltip.HasDropShadow = false;
            m_tooltipTimer.Elapsed += DisplayTooltip;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override void Render()
        {
            base.Render();

            using (DrawingContext context = RenderOpen())
            {
                ProcessRendering(context);
            }
        }

        public override void Reposition()
        {
            Action pFunc = () =>
            {
                var rect = DesiredPosition.GetValueOrDefault(Rect.Empty);
                if (!rect.IsEmpty)
                {
                    Transform = new TranslateTransform(rect.X - rect.Width / 2, rect.Y - rect.Height / 2);
                    VisualOpacity = 1;
                }
                else
                {
                    VisualOpacity = 0;
                }
            };

            PerformanceLayer.LocalDispatcher.Invoke(pFunc);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnMouseEnter(MouseEventArgs e) => m_tooltipTimer.Start();

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            m_tooltipTimer.Stop();
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e) => Refresh();

        protected override void OnPreviewMouseDown(MouseButtonEventArgs args)
        {
            m_tooltipTimer.Stop();

            if (args.ChangedButton == MouseButton.Left)
            {
                var contentBuilder = m_workspace.Services.Get<IContentBuilderService>();
                if (contentBuilder == null)
                {
                    return;
                }

                ContentGroup contentGroup = contentBuilder.Build(m_linkedAeosEntityId);
                DisplayTile(contentGroup);
                args.Handled = true;
                return;
            }

            if (args.ChangedButton != MouseButton.Right)
            {
                return;
            }

            Refresh();
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initializes the door graphical object
        /// </summary>
        private static void InitializeDoor(ref ImageDrawing resource, string resourceName, uint size)
        {
            if (resource != null)
            {
                return;
            }

            try
            {
                var bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/MapsPlayer;;;component/Resources/{resourceName}", UriKind.RelativeOrAbsolute));

                var rect = new Rect(new Size(size, size));

                resource = new ImageDrawing(bitmapImage, rect);
                resource.Freeze();
            }
            catch (IOException)
            {
            }
        }

        /// <summary>
        /// Processes the display of the m_tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayTooltip(object sender, ElapsedEventArgs e)
        {
            m_tooltipTimer.Stop();
            Refresh();
        }

        private void Dispose(bool disposing)
        {
            if (m_isDisposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed resources here
                m_tooltipTimer.Elapsed -= DisplayTooltip;
                m_tooltipTimer.Dispose();
            }

            // Dispose unmanaged resources here
            m_isDisposed = true;
        }

        private void ProcessRendering(DrawingContext context) => RenderDoor(context);

        /// <summary>
        /// Refreshes the display of the object. Call this method when modifications have to be taken into account.
        /// </summary>
        private void Refresh()
        {
            Action pFunc = Render;
            PerformanceLayer.LocalDispatcher.Invoke(pFunc);
        }

        private void RenderDoor(DrawingContext context) => context.DrawDrawing(m_renderSize == 16 ? s_door16 : s_door32);

        #endregion Private Methods

    }
}