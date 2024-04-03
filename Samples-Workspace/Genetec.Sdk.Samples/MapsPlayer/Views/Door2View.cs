// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Services;

namespace MapsPlayer.Views
{
    internal enum OverlayPosition
    {
        TopLeft = 0,
        TopRight = 1,
        BottomRight = 2,
        BottomLeft = 3,
        Center = 4
    }

    public class Door2View : MapObjectView
    {

        #region Private Fields

        private static readonly Brush CoolBlueBrush = new SolidColorBrush { Color = Color.FromRgb(0, 100, 255) };

        private static readonly Brush LightBlueBrush = new SolidColorBrush { Color = Color.FromArgb(80, 30, 144, 255) };

        private static readonly Pen LightBluePen = new Pen(LightBlueBrush, 1);

        private static readonly RadialGradientBrush SelectedEntityBrush = new RadialGradientBrush
        {
            Center = new Point(0.5, 0.5),
            RadiusX = 0.5,
            RadiusY = 0.5,
            GradientStops = new GradientStopCollection
            {
                new GradientStop
                {
                    Color = Color.FromArgb(0, 30, 144, 255),
                    Offset = 1
                },
                new GradientStop
                {
                    Color = Color.FromArgb(80, 30, 144, 255)
                }
            }
        };

        private static readonly Brush SolidRedBrush = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };

        private readonly ImageSource m_doorClosedImageOffline;

        private readonly ImageSource m_doorClosedImageOnline;

        private readonly ImageSource m_doorOpenImageOffline;

        private readonly ImageSource m_doorOpenImageOnline;

        private readonly Guid m_entityId;

        private readonly EntityType m_entityType;

        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;

        private double m_currentHeightScale = 1;

        private double m_currentWidthScale = 1;

        private bool m_disposed;

        #endregion Private Fields

        #region Private Properties

        //Thicknezz instead of Thickness because Thickness hides inherited member Control.BorderThickness and we don't want that.
        private double BorderThicknezz => 5 * m_currentWidthScale;

        private Pen CoolBluePen => new Pen(CoolBlueBrush, BorderThicknezz);

        private Pen SolidRedPen => new Pen(SolidRedBrush, BorderThicknezz);

        #endregion Private Properties

        #region Public Constructors

        public Door2View(Genetec.Sdk.Workspace.Workspace workspace, MapObject mapObject)
        {
            m_workspace = workspace;
            m_entityId = mapObject.LinkedEntity;

            Initialize(mapObject);

            InitializeDoor(ref m_doorOpenImageOnline, "Large_door_opened.png");
            InitializeDoor(ref m_doorClosedImageOnline, "Large_door_opened_red.png");
            InitializeDoor(ref m_doorOpenImageOffline, "Large_door_opened.png");
            InitializeDoor(ref m_doorClosedImageOffline, "Large_door_opened_red.png");

            MinWidth = 16;
            MinHeight = 16;
            MaxWidth = 128;
            MaxHeight = 128;

            Cursor = Cursors.Hand;
            SetSizeModeAndDimensions();

            var entity = m_workspace.Sdk.GetEntity(m_entityId);
            m_entityType = entity.EntityType;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (m_disposed)
            {
                return;
            }

            m_disposed = true;
        }

        protected override bool IsUpdateRequired(EntityInvalidatedEventArgs args)
        {
            if (!(args.EntityType == EntityType.Door || args.EntityType == EntityType.Unit)) return false;
            return args.EntityGuid == m_entityId;
        }

        protected override void OnClick(RoutedEventArgs e)
        {
            IsSelected = !IsSelected;
            e.Handled = true;
        }

        protected override void OnDoubleClick(RoutedEventArgs e)
        {
            base.OnDoubleClick(e);
        }

        protected override void OnIsSelectedChanged()
        {
            base.OnIsSelectedChanged();

            if (IsSelected)
            {
                var service = m_workspace.Services.Get<IContentBuilderService>();
                var contentGroup = service?.Build(MapObject.LinkedEntity);
                if (contentGroup != null)
                {
                    DisplayTile(contentGroup);
                }
            }
            else
            {
                HideTile();
            }
        }

        protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
        {
            var items = BuildMenuItems();
            if ((items != null) && (items.Count > 0))
            {
                ContextMenu = new ContextMenu();
                foreach (var item in items)
                {
                    ContextMenu.Items.Add(item);
                }
                ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            SetSizeModeAndDimensions();

            // selected circle
            if (IsSelected)
            {
                dc.DrawEllipse(SelectedEntityBrush, LightBluePen, new Point(ActualWidth / 2, ActualHeight / 2), 48, 48);
            }

            // main icon
            DrawMainIcon(dc);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Initializes the door graphical object
        /// </summary>
        private static void InitializeDoor(ref ImageSource resource, string resourceName)
        {
            if (resource != null)
            {
                return;
            }

            try
            {
                var bitmapImage = new BitmapImage(new Uri($"pack://application:,,,/MapsPlayer;;;component/Resources/{resourceName}", UriKind.RelativeOrAbsolute));

                resource = bitmapImage;
                resource.Freeze();
            }
            catch (IOException)
            {
            }
        }

        private void DrawMainIcon(DrawingContext dc)
        {
            dc.DrawImage(m_doorOpenImageOnline, new Rect(0, 0, ActualWidth, ActualHeight));
        }

        private void SetSizeModeAndDimensions()
        {
            RelativeSizeMode = RelativeSizeMode.Auto;
            RelativeWidth = 192;
            RelativeHeight = 192;
            m_currentWidthScale = ActualWidth / RelativeWidth;
            m_currentHeightScale = ActualHeight / RelativeWidth;
        }

        #endregion Private Methods

        #region Private Destructors

        ~Door2View() => Dispose(false);

        #endregion Private Destructors
    }
}