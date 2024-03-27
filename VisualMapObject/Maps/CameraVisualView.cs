using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Maps;
using Genetec.Sdk.Workspace.Pages.Contents;
using VisualMapObject.Maps.Layers;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Genetec.Sdk;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Maps
{
    public sealed class CameraVisualView : MapObjectVisual
    {
        private const int SIZE16 = 16;
        private const int SIZE32 = 32;

        private static readonly BitmapImage s_camera_open;
        private static readonly BitmapImage s_camera_close;
        private static readonly ImageDrawing s_drawingCamera_open16;
        private static readonly ImageDrawing s_drawingCamera_close16;
        private static readonly ImageDrawing s_drawingCamera_open32;
        private static readonly ImageDrawing s_drawingCamera_close32;
        private Workspace m_workspace;

        private bool m_isOpened = false;
        private int m_renderSize = SIZE16;

        /// <summary>
        /// Gets or sets the render size of the visual.
        /// </summary>
        public int RenderSize
        {
            get { return m_renderSize; }
            set
            {
                if (value != m_renderSize)
                {
                    m_renderSize = value;
                    Render();
                }
            }
        }

        /// <summary>
        /// Static constructors is used to load ressources.
        /// </summary>
        static CameraVisualView()
        {
            //**IMPORTANT**
            //images should be mark as ressources.
            //To ensure they are ressources, click on the image Properties and select Resources for the build action.

            //Load the images
            s_camera_open = new BitmapImage(new Uri(@"pack://application:,,,/VisualMapObject;Component/Resources/open.jpg", UriKind.RelativeOrAbsolute));
            s_camera_close = new BitmapImage(new Uri(@"pack://application:,,,/VisualMapObject;Component/Resources/close.png", UriKind.RelativeOrAbsolute));

            //Create the drawings for the size 16
            var rect16 = new Rect(0, 0, SIZE16, SIZE16);
            s_drawingCamera_open16 = new ImageDrawing(s_camera_open, rect16);
            s_drawingCamera_open16.Freeze();
            s_drawingCamera_close16 = new ImageDrawing(s_camera_close, rect16);
            s_drawingCamera_close16.Freeze();

            //Create the drawings for the size 32
            var rect32 = new Rect(0, 0, SIZE32, SIZE32);
            s_drawingCamera_open32 = new ImageDrawing(s_camera_open, rect32);
            s_drawingCamera_open32.Freeze();
            s_drawingCamera_close32 = new ImageDrawing(s_camera_close, rect32);
            s_drawingCamera_close32.Freeze();
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CameraVisualView"/> class.
        /// </summary>
        public CameraVisualView()
        {
            LayerId = CameraVisualLayer.Identifier;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="CameraVisualView"/> class.
        /// This also initializes the visual, using this constructor we should not call the Initialize method.
        /// </summary>
        /// <param name="workspace">The <see cref="Workspace"/> in wich the visual lives.</param>
        /// <param name="mapObject">The <see cref="MapObject"/> linked to this visual.</param>
        public CameraVisualView(Workspace workspace, MapObject mapObject)
            : this()
        {
            Initialize(workspace, mapObject);
        }

        /// <summary>
        /// Initializes the visual.
        /// </summary>
        /// <param name="workspace">The <see cref="Workspace"/> in wich the visual lives.</param>
        /// <param name="mapObject">The <see cref="MapObject"/> linked to this visual.</param>
        public void Initialize(Workspace workspace, MapObject mapObject)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException("workspace");
            }

            m_workspace = workspace;
            Initialize(mapObject);
        }

        /// <summary>
        /// Methods that render the visual.
        /// </summary>
        public override void Render()
        {
            base.Render();

            using (var dc = RenderOpen())
            {
                switch (RenderSize)
                {
                    case SIZE16:
                        dc.DrawDrawing(m_isOpened ? s_drawingCamera_open16 : s_drawingCamera_close16);
                        break;
                    case SIZE32:
                        dc.DrawDrawing(m_isOpened ? s_drawingCamera_open32 : s_drawingCamera_close32);
                        break;
                    default:
                        Debug.Assert(false, "Invalid size");
                        break;
                }
            }
        }

        /// <summary>
        /// Repositions the visual on the map.
        /// </summary>
        public override void Reposition()
        {
            if (Dispatcher.CheckAccess())
            {
                var rect = DesiredPosition.GetValueOrDefault(Rect.Empty);
                if (!rect.IsEmpty)
                {
                    Transform = new TranslateTransform(rect.X - rect.Width/2, rect.Y - rect.Height/2);
                    VisualOpacity = 1;
                }
                else
                {
                    VisualOpacity = 0;
                }
            }
            else
            {
                Dispatcher.Invoke(Reposition);
            }
        }

        /// <summary>
        /// Method raise when the mouse cursor is on the visual.
        /// </summary>
        /// <param name="e">The mouse event.</param>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            Action pFunc = delegate
            {
                //uncomment this to change the image when the mouse is over the visual
                //m_isOpened = true;
                Render();
            };
            Dispatcher.BeginInvoke(pFunc, DispatcherPriority.Render);
        }

        /// <summary>
        /// Method raise when the mouse cursor quits the visual.
        /// </summary>
        /// <param name="e">The mouse event.</param>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            Action pFunc = delegate
            {
                //uncomment this to change the image when the mouse is over the visual
                //m_isOpened = false;
                Render();
            };
            Dispatcher.BeginInvoke(pFunc, DispatcherPriority.Render);
        }

        /// <summary>
        /// Method raise when the mouse button is clicked.
        /// </summary>
        /// <param name="e">The mouse event.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            Action pFunc = delegate
            {
                try
                {
                    var cg = new ContentGroup();
                    cg.Initialize(m_workspace);
                    var vc = new VideoContent(MapObject.LinkedEntity); //Gets the associated camera map object.
                    vc.Initialize(m_workspace);
                    vc.Title = "Title of the camera";
                    cg.Contents.Add(vc);

                    DisplayTile(cg);

                    m_isOpened = true;
                }
                catch (Exception)
                {
                    m_isOpened = false;
                }
            };
            m_workspace.Dispatcher.BeginInvoke(pFunc);
        }
    }
}
