// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Genetec.Sdk.Workspace.Maps;
using MapsPerformance.Visuals;

namespace MapsPerformance.Layers
{
    public sealed class PerformanceLayer : MapLayer, IDisposable
    {

        #region Public Fields

        public static readonly Guid Identifier = new Guid("{569612D5-3559-4A50-9DD8-70BAA1AF090C}");

        #endregion Public Fields

        #region Private Fields

        private static readonly ManualResetEvent s_mutex = new ManualResetEvent(false);
        private static volatile int s_counter;
        private static Dispatcher s_dispatcher;
        private static Thread s_thread;
        private readonly HostVisual m_hostVisual;
        private readonly object m_syncRoot = new object();
        private readonly List<IMapObjectView> m_toAdd = new List<IMapObjectView>();
        private readonly VisualWrapper m_visualWrapper = new VisualWrapper();
        private readonly Genetec.Sdk.Workspace.Workspace m_workspace;
        private Canvas m_canvas;
        /// <summary>
        /// Create a collection of child visual objects that is used for optimized layers
        /// </summary>
        private VisualsPanel m_layer;

        private DispatcherOperation m_repositionOperation;
        private DispatcherTimer m_timerDigest;

        #endregion Private Fields

        #region Public Properties

        public static Dispatcher LocalDispatcher => s_dispatcher;

        /// <summary>
        /// Gets a flag indicating if the map control should perform a custom hit test on this layer
        /// </summary>
        public override bool EnableHitTest => true;

        /// <summary>
        /// Gets the layer's unique identifier
        /// </summary>
        public override Guid Id => Identifier;

        /// <summary>
        /// Gets the layer's name
        /// </summary>
        public override string Name => "Performance";

        #endregion Public Properties

        #region Public Constructors

        public PerformanceLayer(Genetec.Sdk.Workspace.Workspace workspace)
        {
            EnsureThreadRunning();

            m_workspace = workspace;
            m_hostVisual = new HostVisual();
            m_visualWrapper.Child = m_hostVisual;

            s_mutex.WaitOne();
            // Init on our custom thread
            s_dispatcher.Invoke(Init);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Add(IEnumerable<IMapObjectView> items)
        {
            lock (m_syncRoot)
            {
                m_toAdd.AddRange(items);
            }
        }

        public override void Clear()
        {
            Action pFunc = delegate
            {
                lock (m_syncRoot)
                {
                    m_toAdd.Clear();
                    m_layer.Clear();
                }
            };
            s_dispatcher?.Invoke(pFunc);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (m_layer != null)
            {
                Clear();
                m_layer = null;
            }

            s_counter--;

            if ((s_counter == 0) && (s_dispatcher != null))
            {
                s_dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);

                s_thread = null;
                s_mutex.Reset();
                s_dispatcher = null;
            }
        }

        public override void Remove(IEnumerable<IMapObjectView> items)
        {
        }

        /// <summary>
        /// Reposition all the objects on the layer
        /// </summary>
        public override void Reposition(IEnumerable<IMapObjectView> items)
        {
            if (m_repositionOperation != null)
            {
                if (m_repositionOperation.Status != DispatcherOperationStatus.Completed)
                {
                    m_repositionOperation.Abort();
                    m_repositionOperation = null;
                }
            }

            var scale = GetScaleFactor();

            Action pFunc = delegate
            {
                var renderSize = 16;

                if (scale < 0.0001)
                    renderSize = 0;
                else if (scale > 0.001)
                    renderSize = 32;

                foreach (var item in items)
                {
                    if (item is CameraVisual visual)
                    {
                        visual.RenderSize = renderSize;
                        visual.Reposition();
                    }
                }
            };
            m_repositionOperation = s_dispatcher.BeginInvoke(pFunc);
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Create a panel for hosting the map objects
        /// </summary>
        /// <returns>Panel created</returns>
        protected override UIElement CreatePanel() => m_visualWrapper;

        protected override object HitTest(Point pos)
        {
            Visual visual = null;

            Action pFunc = delegate
            {
                var result = VisualTreeHelper.HitTest(m_layer, pos);
                if (result != null)
                {
                    visual = result.VisualHit as Visual;
                }
            };
            s_dispatcher.Invoke(pFunc);

            return visual;
        }

        protected override void OnSizeChanged()
        {
            var size = Size;

            if (!double.IsInfinity(size.Height) && !double.IsInfinity(size.Width))
            {
                Action pFunc = delegate
                {
                    m_canvas.Height = size.Height;
                    m_canvas.Width = size.Width;
                };
                s_dispatcher.Invoke(pFunc);
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private static void OnThreadStarted(object arg)
        {
            // Run a dispatcher for this worker thread.  This is the central processing loop for WPF.
            s_dispatcher = Dispatcher.CurrentDispatcher;
            s_mutex.Set();

            Dispatcher.Run();
        }

        private void EnsureThreadRunning()
        {
            if (s_thread == null)
            {
                // Spin up a worker thread
                s_thread = new Thread(OnThreadStarted);
                s_thread.SetApartmentState(ApartmentState.STA);
                s_thread.IsBackground = true;
                s_thread.Start();
            }
        }

        private void Init()
        {
            // Create the VisualTargetPresentationSource and then signal the
            // calling thread, so that it can continue without waiting for us.
            var visualTargetPS = new VisualTargetPresentationSource(m_hostVisual);

            m_canvas = new Canvas();

            // Create the visual collection used for optimized layers
            m_layer = new VisualsPanel();

            // Create a MediaElement and use it as the root visual for the VisualTarget.
            visualTargetPS.RootVisual = m_layer;

            m_timerDigest = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, OnTimerDigestTick, s_dispatcher);
            s_counter++;
        }

        private void OnTimerDigestTick(object sender, EventArgs e)
        {
            lock (m_syncRoot)
            {
                foreach (var item in m_toAdd)
                {
                    if (item is Visual visual)
                    {
                        m_layer.Add(visual);
                    }
                }
                m_toAdd.Clear();
            }
        }

        #endregion Private Methods

        #region Private Classes

        private sealed class VisualsPanel : Visual
        {

            #region Private Fields

            private readonly VisualCollection m_visuals;

            #endregion Private Fields

            #region Public Properties

            public IEnumerable<Visual> Visuals
            {
                get
                {
                    foreach (var visual in m_visuals)
                    {
                        yield return visual;
                    }
                }
            }

            #endregion Public Properties

            #region Protected Properties

            protected override int VisualChildrenCount => m_visuals.Count;

            #endregion Protected Properties

            #region Public Constructors

            public VisualsPanel() => m_visuals = new VisualCollection(this);

            #endregion Public Constructors

            #region Public Methods

            public void Add(Visual visual) => m_visuals.Add(visual);

            public void Clear() => m_visuals.Clear();

            public void Remove(Visual visual) => m_visuals.Remove(visual);

            #endregion Public Methods

            #region Protected Methods

            protected override Visual GetVisualChild(int index) => m_visuals[index];

            #endregion Protected Methods

        }

        private sealed class VisualTargetPresentationSource : PresentationSource
        {

            #region Private Fields

            private readonly VisualTarget m_visualTarget;

            #endregion Private Fields

            #region Public Properties

            public override bool IsDisposed => false;

            public override Visual RootVisual
            {
                get => m_visualTarget.RootVisual;
                set
                {
                    var oldRoot = m_visualTarget.RootVisual;

                    // Set the root visual of the VisualTarget.  This visual will
                    // now be used to visually compose the scene.
                    m_visualTarget.RootVisual = value;

                    // Tell the PresentationSource that the root visual has
                    // changed.  This kicks off a bunch of stuff like the
                    // Loaded event.
                    RootChanged(oldRoot, value);

                    // Kickoff layout…
                    var rootElement = value as UIElement;
                    if (rootElement != null)
                    {
                        rootElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                        rootElement.Arrange(new Rect(rootElement.DesiredSize));
                    }
                }
            }

            #endregion Public Properties

            #region Public Constructors

            public VisualTargetPresentationSource(HostVisual hostVisual) => m_visualTarget = new VisualTarget(hostVisual);

            #endregion Public Constructors

            #region Protected Methods

            protected override CompositionTarget GetCompositionTargetCore() => m_visualTarget;

            #endregion Protected Methods

        }

        [ContentProperty("Child")]
        private sealed class VisualWrapper : FrameworkElement
        {

            #region Private Fields

            private Visual m_child;

            #endregion Private Fields

            #region Public Properties

            public Visual Child
            {
                get => m_child;
                set
                {
                    if (m_child != null)
                    {
                        RemoveVisualChild(m_child);
                    }

                    m_child = value;

                    if (m_child != null)
                    {
                        AddVisualChild(m_child);
                    }
                }
            }

            #endregion Public Properties

            #region Protected Properties

            protected override int VisualChildrenCount => m_child != null ? 1 : 0;

            #endregion Protected Properties

            #region Protected Methods

            protected override Visual GetVisualChild(int index)
            {
                if (m_child != null && index == 0)
                {
                    return m_child;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
            }

            //dc.DrawRectangle(Brushes.Transparent, new Pen(), new Rect(new Point(), new Size(2000, 2000)));
            protected override void OnRender(DrawingContext dc) => base.OnRender(dc);

            #endregion Protected Methods

        }

        #endregion Private Classes
    }
}