// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Pages.Contents;
using MapsPerformance.Layers;

namespace MapsPerformance.Visuals
{
    public sealed class CameraVisual : MapObjectVisual
    {

        #region Private Fields

        private static readonly ImageDrawing s_drawingClosed16;
        private static readonly ImageDrawing s_drawingClosed32;
        private static readonly BitmapImage s_iconClosed;
        private int m_renderSize = 16;
        private Genetec.Sdk.Workspace.Workspace m_workspace;

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

        static CameraVisual()
        {
            s_iconClosed = new BitmapImage(new Uri(@"pack://application:,,,/MapsPerformance;Component/Resources/Camera.png", UriKind.RelativeOrAbsolute));

            var x = 0;
            var y = 0;
            const int width = 16;
            const int height = 16;
            var rect = new Rect(x, y, width, height);

            s_drawingClosed16 = new ImageDrawing(s_iconClosed, rect);
            s_drawingClosed16.Freeze();

            rect.Height = 32;
            rect.Width = 32;
            s_drawingClosed32 = new ImageDrawing(s_iconClosed, rect);
            s_drawingClosed32.Freeze();
        }

        public CameraVisual() => LayerId = PerformanceLayer.Identifier;

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace, MapObject mapObject)
        {
            m_workspace = workspace;
            Initialize(mapObject);
        }

        public override void Render()
        {
            base.Render();

            using (var dc = RenderOpen())
            {
                switch (m_renderSize)
                {
                    case 16:
                        dc.DrawDrawing(s_drawingClosed16);
                        break;

                    case 32:
                        dc.DrawDrawing(s_drawingClosed32);
                        break;
                }
            }
        }

        public override void Reposition()
        {
            Action pFunc = delegate
            {
                var rect = DesiredPosition.GetValueOrDefault(Rect.Empty);
                if (!rect.IsEmpty)
                {
                    Transform = new TranslateTransform(rect.X - rect.Width / 2, rect.Y - rect.Height / 2);
                }
            };
            PerformanceLayer.LocalDispatcher.Invoke(pFunc);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override Rect GetBounds() => new Rect(new Point(), new Size(m_renderSize, m_renderSize));

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            //Action pFunc = delegate
            //{
            //    m_isOpened = true;
            //    Render();
            //};
            //Dispatcher.BeginInvoke(pFunc, DispatcherPriority.Render);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            //Action pFunc = delegate
            //{
            //    m_isOpened = false;
            //    Render();
            //};
            //Dispatcher.BeginInvoke(pFunc, DispatcherPriority.Render);
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Action pFunc = delegate
            {
                var cg = new ContentGroup();
                cg.Initialize(m_workspace);
                VideoContent vc = new VideoContent(new Guid("00000001-0000-babe-0000-00408cb9a67a"));
                vc.Initialize(m_workspace);
                vc.Title = "Allo";
                cg.Contents.Add(vc);

                DisplayTile(cg);
            };
            m_workspace.Dispatcher.BeginInvoke(pFunc);
        }

        #endregion Protected Methods

    }
}