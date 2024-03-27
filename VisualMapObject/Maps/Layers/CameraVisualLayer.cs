using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Maps.Layers
{
    /// <summary>
    /// Maps has multiple layers, each of wich is use to render diffrent types of map objects.
    /// In this sample this layer render camera visual elements.
    /// </summary>
    public sealed class CameraVisualLayer : MapLayer, IDisposable
    {
        #region Constants

        /// <summary>
        /// The unique identifier that is use to register the layer.
        /// </summary>
        public static readonly Guid Identifier = new Guid("{BF82E3EE-4EBD-4289-B6D9-D16745734D46}");

        #endregion

        #region Fields

        /// <summary>
        /// The count of reference of the layer.
        /// </summary>
        private static int s_counter;

        /// <summary>
        /// List of elements to add to the layer.
        /// Elements are not directly added, they are queue for better thread and ressources management.
        /// </summary>
        private readonly List<IMapObjectView> m_toAdd = new List<IMapObjectView>();

        /// <summary>
        /// Synchronisation object for the m_add collection.
        /// </summary>
        private readonly object m_syncRoot = new object();

        /// <summary>
        /// Reference on the current workspace.
        /// </summary>
        private readonly Workspace m_workspace;

        /// <summary>
        /// The host that contains.
        /// </summary>
        private readonly HostVisual m_hostVisual;

        /// <summary>
        /// The visual wrapper that help managing the childs visual element.
        /// </summary>
        private readonly VisualWrapper m_visualWrapper = new VisualWrapper();

        /// <summary>
        /// Create a collection of child visual objects that is used for optimized layers
        /// </summary>
        private VisualsPanel m_layer;

        /// <summary>
        /// Canvas, this is mostly used to manage size of the element.
        /// </summary>
        private Canvas m_canvas;

        /// <summary>
        /// Timer thread that is used to add items on the layer at it own speed.
        /// </summary>
        private DispatcherTimer m_timerDigest;

        /// <summary>
        /// The operation of repositioning.
        /// Repositioning can be queue multiple time and can invalidate each other.
        /// Using this 
        /// </summary>
        private DispatcherOperation m_repositionOperation;

        /// <summary>
        /// This class represents the thread that is use to render the layer.
        /// Because layers and map object have a hight thread affinity.
        /// </summary>
        private sealed class LayerThread : IDisposable
        {
            #region Constants

            /// <summary>
            /// This manual reset event is used to be sure that the thread is initialized before using it.
            /// </summary>
            private static readonly ManualResetEvent WaitForThreadReady = new ManualResetEvent(false);

            /// <summary>
            /// Lazy initialization of the instance of the LayerThread class.
            /// This assure that the instance is thread safe.
            /// </summary>
            private static readonly Lazy<LayerThread> s_lazyInstance = new Lazy<LayerThread>(() => new LayerThread());

            #endregion

            #region Fields

            /// <summary>
            /// The dispatcher that manage the queue system of the layer thread.
            /// </summary>
            private static Dispatcher s_dispather;

            /// <summary>
            /// The thread that is use to render the elements of the layer.
            /// </summary>
            private static Thread s_thread;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the dispather that manage the queue system of the layer thread.
            /// </summary>
            public Dispatcher Dispatcher {get { return s_dispather; }}

            /// <summary>
            /// The instance of the <see cref="LayerThread"/>.
            /// </summary>
            public static LayerThread Instance {get { return s_lazyInstance.Value; }}

            #endregion

            #region Constructors


            /// <summary>
            /// Initializes a new instance of the <see cref="LayerThread"/> class.
            /// </summary>
            private LayerThread()
            {
                s_thread = new Thread(() =>
                {
                    s_dispather = Dispatcher.CurrentDispatcher;
                    WaitForThreadReady.Set();
                    Dispatcher.Run();
                });
                s_thread.SetApartmentState(ApartmentState.STA);
                s_thread.IsBackground = true;
                s_thread.Start();
                WaitForThreadReady.WaitOne();
            }

            #endregion

            #region Destructors and Dispose Methods

            /// <summary>
            /// Dispose the ressource use by the <see cref="LayerThread"/>.
            /// </summary>
            public void Dispose()
            {
                Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
                s_thread = null;
                WaitForThreadReady.Reset();
                s_dispather = null;
            }

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a flag indicating if the map control should perform a custom hit test on this layer
        /// </summary>
        public override bool EnableHitTest {get { return true; }}

        /// <summary>
        /// Gets the layer's unique identifier
        /// </summary>
        public override Guid Id {get { return Identifier; }}

        /// <summary>
        /// Gets the layer's name.
        /// </summary>
        public override string Name {get { return "Camera Visual Layer"; }}

        /// <summary>
        /// Invokes of the appropriate thread the <see cref="Action"/>.
        /// </summary>
        /// <param name="action">The action to execute on the layer dispatcher thread.</param>
        public static void Invoke(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            LayerThread.Instance.Dispatcher.Invoke(action);
        }

        /// <summary>
        /// Invokes the func on the layers thread.
        /// </summary>
        /// <typeparam name="T">The type of the return value of the func.</typeparam>
        /// <param name="func">The function to executes on the layer thread.</param>
        /// <returns>The value return by the func.</returns>
        public static T Invoke<T>(Func<T> func)
        {
            if (func == null)
            {
                throw new ArgumentNullException("func");
            }
            var result = default(T);
            Invoke(() => { result = func(); });
            return result;
        }

        /// <summary>
        /// Queue the <see cref="Action"/> on the layer thread.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to queue on the layer thread.</param>
        /// <returns>The <see cref="DispatcherOperation"/> that represents the operation being perform.</returns>
        public static DispatcherOperation BeginInvoke(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            return LayerThread.Instance.Dispatcher.BeginInvoke(action);
        }

        #endregion

        #region Nested Classes and Structures

        /// <summary>
        /// The panel that hols the visual to be displayed in the layer.
        /// </summary>
        private sealed class VisualsPanel : Visual
        {
            #region Fields

            private readonly VisualCollection m_visuals;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the list of the visual.
            /// </summary>
            public IEnumerable<Visual> Visuals {get{return m_visuals.Cast<Visual>();}}

            /// <summary>
            /// The count of visual children elements.
            /// </summary>
            protected override int VisualChildrenCount {get{return m_visuals.Count;}}

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="VisualsPanel"/> class.
            /// </summary>
            public VisualsPanel()
            {
                m_visuals = new VisualCollection(this);
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Adds a visual items.
            /// </summary>
            /// <param name="visual"></param>
            public void Add(Visual visual)
            {
                if (visual == null)
                {
                    throw new ArgumentNullException("visual");
                }
                m_visuals.Add(visual);
            }

            /// <summary>
            /// Removes a visual item.
            /// </summary>
            /// <param name="visual"></param>
            public void Remove(Visual visual)
            {
                if (visual == null)
                {
                    throw new ArgumentNullException("visual");
                }
                m_visuals.Remove(visual);
            }

            /// <summary>
            /// Clears the visual elements.
            /// </summary>
            public void Clear()
            {
                m_visuals.Clear();
            }

            #endregion

            #region Private and Protected Methods

            /// <summary>
            /// Gets the visual child at the specified index.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>The visual child element at the specified index.</returns>
            protected override Visual GetVisualChild(int index)
            {
                return m_visuals[index];
            }

            #endregion
        }

        /// <summary>
        /// Wrapper class for the child elements.
        /// </summary>
        [ContentProperty("Child")]
        private sealed class VisualWrapper : FrameworkElement
        {
            #region Fields

            private Visual m_child;

            #endregion

            #region Properties

            public Visual Child
            {
                get
                {
                    return m_child;
                }
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

            protected override int VisualChildrenCount {get { return m_child != null ? 1 : 0; }}

            #endregion

            #region Private and Protected Methods

            /// <summary>
            /// Gets the child of the visual wrapper.
            /// </summary>
            /// <param name="index">The index.</param>
            /// <returns>Returns the <see cref="Visual"/> at the index.</returns>
            protected override Visual GetVisualChild(int index)
            {
                if (m_child != null && index == 0)
                {
                    return m_child;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("index");
                }
            }

            #endregion
        }

        /// <summary>
        /// Class that is use to present content and manage the visual tree.
        /// </summary>
        private sealed class VisualTargetPresentationSource : PresentationSource
        {
            #region Fields

            /// <summary>
            /// The target.
            /// </summary>
            private readonly VisualTarget m_visualTarget;

            #endregion

            #region Properties

            // We don’t support disposing this object.
            public override bool IsDisposed {get { return false; }}

            /// <summary>
            /// Gets the root of the visual.
            /// </summary>
            public override Visual RootVisual
            {
                get
                {
                    return m_visualTarget.RootVisual;
                }
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
                        rootElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                        rootElement.Arrange(new Rect(rootElement.DesiredSize));
                    }
                }
            }

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="VisualTargetPresentationSource"/> class.
            /// </summary>
            /// <param name="hostVisual"></param>
            public VisualTargetPresentationSource(HostVisual hostVisual)
            {
                m_visualTarget = new VisualTarget(hostVisual);
            }

            #endregion

            #region Private and Protected Methods

            /// <summary>
            /// When overridden in a derived class, returns a visual target for the given source.
            /// </summary>
            /// <returns>Returns a <see cref="T:System.Windows.Media.CompositionTarget"/> that is target for rendering the visual.</returns>
            protected override CompositionTarget GetCompositionTargetCore()
            {
                return m_visualTarget;
            }

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraVisualLayer"/> class.
        /// </summary>
        /// <param name="workspace">The current workspace.</param>
        public CameraVisualLayer(Workspace workspace)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException("workspace");
            }

            //Force initialization of the thread.
            LayerThread.Instance.Dispatcher.Invoke(() => { });

            //Build UI element that don't have thread affinity.
            m_workspace = workspace;
            m_hostVisual = new HostVisual();
            m_visualWrapper.Child = m_hostVisual;

            // Init on our custom thread
            Invoke(() =>
            {
                // Create the VisualTargetPresentationSource and then signal the
                // calling thread, so that it can continue without waiting for us.
                var visualTargetPS = new VisualTargetPresentationSource(m_hostVisual);

                m_canvas = new Canvas();

                // Create the visual collection used for optimized layers
                m_layer = new VisualsPanel();

                // Create a MediaElement and use it as the root visual for the VisualTarget.
                visualTargetPS.RootVisual = m_layer;

                //Thread that manages the adding of the elements.
                m_timerDigest = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, OnTimerDigestTick, LayerThread.Instance.Dispatcher);

                //A new happy customer let's increment the count !
                Interlocked.Increment(ref s_counter);
            });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the items to the visual tree.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public override void Add(IEnumerable<IMapObjectView> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            lock (m_syncRoot)
            {
                m_toAdd.AddRange(items);
            }
        }

        /// <summary>
        /// Clears all the elements from the visual.
        /// </summary>
        public override void Clear()
        {
            Invoke(() =>
            {
                lock (m_syncRoot)
                {
                    m_toAdd.Clear();
                    m_layer.Clear();
                }
            });
        }

        /// <summary>
        /// Removes the items from the visual.
        /// </summary>
        /// <param name="items">The items to remove.</param>
        public override void Remove(IEnumerable<IMapObjectView> items)
        {
            Invoke(() =>
            {
                lock (m_syncRoot)
                {
                    items.OfType<Visual>().ToList().ForEach(m_layer.Remove);
                }
            });
        }

        /// <summary>
        /// Reposition all the objects on the layer
        /// </summary>
        public override void Reposition(IEnumerable<IMapObjectView> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            if (m_repositionOperation != null &&
                m_repositionOperation.Status != DispatcherOperationStatus.Completed)
            {
                m_repositionOperation.Abort();
                m_repositionOperation = null;
            }

            var scale = GetScaleFactor();

            m_repositionOperation = BeginInvoke(() =>
            {
                int renderSize = scale < 0.0001 ? 0 : (scale > 0.001 ? 32 : 16);
                items.OfType<CameraVisualView>().ToList().ForEach(visual =>
                {
                    visual.RenderSize = renderSize;
                    visual.Reposition();
                });
            });
        }

        #endregion

        #region Private and Protected Methods

        /// <summary>
        /// Creates a panel for hosting the map objects
        /// </summary>
        /// <returns>Panel created</returns>
        protected override UIElement CreatePanel()
        {
            return m_visualWrapper;
        }

        /// <summary>
        /// Called to test for a hit.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns>Return true if the pos is in the visual object.</returns>
        protected override object HitTest(Point pos)
        {
            return Invoke(() =>
            {
                var result = VisualTreeHelper.HitTest(m_layer, pos);
                if (result == null) return null;
                return result.VisualHit as Visual;
            });
        }

        /// <summary>
        /// Called when the size is changed.
        /// </summary>
        protected override void OnSizeChanged()
        {
            var size = Size;

            Invoke(() =>
            {
                m_canvas.Height = size.Height;
                m_canvas.Width = size.Width;
            });
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// The callback that add element to the visual tree.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event.</param>
        private void OnTimerDigestTick(object sender, EventArgs e)
        {
            lock (m_syncRoot)
            {
                if (m_layer != null)
                {
                    m_toAdd.OfType<Visual>().ToList().ForEach(m_layer.Add);
                    m_toAdd.Clear();
                }
            }
        }

        #endregion

        #region Destructors and Dispose Methods

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

            if (Interlocked.Decrement(ref s_counter) == 0)
            {
                LayerThread.Instance.Dispose();
            }
        }

        #endregion
    }
}

