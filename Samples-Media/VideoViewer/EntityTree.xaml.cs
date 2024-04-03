using Genetec.Sdk;
using Genetec.Sdk.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

// ==========================================================================
// Copyright (C) 2015 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 30:
//  1813 – Battle of Bárbula: Simón Bolívar defeats Santiago Bobadilla.
//  1860 – Britain's first tram service begins in Birkenhead, Merseyside.
//  2009 – The 2009 Sumatra earthquakes occur, killing over 1,115 people.
// ==========================================================================
namespace VideoViewer
{
    #region Classes

    /// <summary>
    /// Interaction logic for EntityTree.xaml
    /// </summary>
    public partial class EntityTree : IDisposable
    {
        #region Constants

        public static readonly DependencyProperty IsCheckableProperty =
                                    DependencyProperty.Register
                                    ("IsCheckable", typeof(bool), typeof(EntityTree),
                                    new PropertyMetadata(true));

        private static readonly DependencyPropertyKey SelectedItemPropertyKey =
                                    DependencyProperty.RegisterReadOnly
                                    ("SelectedItem", typeof(Guid), typeof(EntityTree),
                                    new PropertyMetadata(default(Guid)));

        private readonly Dictionary<Guid, List<EntityItem>> m_items = new Dictionary<Guid, List<EntityItem>>();

        private readonly ObservableCollection<EntityItem> m_rootItems = new ObservableCollection<EntityItem>();

        #endregion

        #region Fields

        public bool m_canDragDrop = false;

        #endregion

        #region Properties

        public ICollection<EntityType> EntityFilter
        {
            get;
            set;
        }

        public ReadOnlyObservableCollection<EntityItem> Items
        {
            get;
            private set;
        }

        public Guid SelectedItem
        {
            get { return (Guid)GetValue(SelectedItemPropertyKey.DependencyProperty); }
            private set { SetValue(SelectedItemPropertyKey, value); }
        }

        private IEngine SdkEngine
        {
            get;
            set;
        }

        #endregion

        #region Events and Delegates

        public event EventHandler SelectedItemChanged;

        public event EventHandler SelectionChanged;

        #endregion

        #region Nested Classes and Structures

        public sealed class EntityItem : DependencyObject
        {
            #region Constants

            public static readonly DependencyProperty IsExpandedProperty =
                                                    DependencyProperty.Register
                                                    ("IsExpanded", typeof(bool), typeof(EntityItem),
                                                    new PropertyMetadata(false, OnPropertyIsExpandedChanged));

            [ThreadStatic]
            private static readonly EntityItem Empty = new EntityItem();

            private static readonly DependencyPropertyKey IconPropertyKey =
                                                    DependencyProperty.RegisterReadOnly
                                                    ("Icon", typeof(ImageSource), typeof(EntityItem),
                                                    new PropertyMetadata(null));

            private static readonly DependencyPropertyKey NamePropertyKey =
                                                    DependencyProperty.RegisterReadOnly
                                                    ("Name", typeof(string), typeof(EntityItem),
                                                    new PropertyMetadata());

            private readonly ObservableCollection<EntityItem> m_children = new ObservableCollection<EntityItem>();

            private readonly Entity m_entity;

            private readonly IEngine m_sdkEngine;

            #endregion

            #region Properties

            public ReadOnlyObservableCollection<EntityItem> Children
            {
                get;
                private set;
            }

            public ICollectionView ChildrenView
            {
                get;
                private set;
            }

            public Guid Guid
            {
                get;
                private set;
            }

            public ImageSource Icon
            {
                get { return (ImageSource)GetValue(IconPropertyKey.DependencyProperty); }
                private set { SetValue(IconPropertyKey, value); }
            }

            public bool IsExpanded
            {
                get { return (bool)GetValue(IsExpandedProperty); }
                set { SetValue(IsExpandedProperty, value); }
            }

            public string Name
            {
                get { return (string)GetValue(NamePropertyKey.DependencyProperty); }
                private set { SetValue(NamePropertyKey, value); }
            }

            public EntityItem Parent
            {
                get;
                private set;
            }

            public EntityTree Tree
            {
                get;
                private set;
            }

            #endregion

            #region Constructors

            internal EntityItem(IEngine sdkEngine, EntityTree tree, Entity entity)
                : this(sdkEngine, tree, entity, null)
            {
            }

            internal EntityItem(IEngine sdkEngine, EntityTree tree, Entity entity, EntityItem parent)
            {
                if (sdkEngine == null)
                    throw new ArgumentNullException("sdkEngine");

                if (entity == null)
                    throw new ArgumentNullException("entity");

                m_children.Add(Empty);
                Children = new ReadOnlyObservableCollection<EntityItem>(m_children);
                ChildrenView = new ListCollectionView(m_children);

                m_sdkEngine = sdkEngine;
                m_entity = entity;
                Tree = tree;
                Parent = parent;
                Guid = m_entity.Guid;

                Update();

                tree.AddItem(this);
            }

            /// <summary>
            /// Constructor used for the empty item
            /// </summary>
            private EntityItem()
            {
            }

            #endregion

            #region Event Handlers

            private static void OnPropertyIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var instance = d as EntityItem;
                if (instance != null)
                {
                    instance.OnIsExpandedChanged();
                }
            }

            private void OnIsExpandedChanged()
            {
                if (IsExpanded)
                {
                    Expand();
                }
                else
                {
                    Collapse();
                }
            }

            #endregion

            #region Public Methods

            public void Collapse()
            {
                foreach (EntityItem item in m_children)
                {
                    Tree.RemoveItem(item);
                }

                m_children.Clear();
                m_children.Add(Empty);
            }

            public void Expand()
            {
                m_children.Clear();
                foreach (Guid id in m_entity.HierarchicalChildren)
                {
                    // Note: for clarity, this sample doesn't show proper pre-fetching of entities
                    // Refer to the help file for more information on queries (EntityConfigurationQuery) and scalability
                    Entity entity = m_sdkEngine.GetEntity(id);
                    if ((entity != null) && (Tree.EntityFilter.Contains(entity.EntityType)))
                    {
                        EntityItem item = new EntityItem(m_sdkEngine, Tree, entity, this);
                        m_children.Add(item);
                    }
                }
            }

            public override string ToString()
            {
                return String.Format("{0} ({1})", m_entity.Name, m_entity.EntityType);
            }

            public void Update()
            {
                Name = m_entity.Name;
                Icon = m_entity.GetIcon(true);
            }

            #endregion
        }

        #endregion

        #region Constructors

        public EntityTree()
        {
            Items = new ReadOnlyObservableCollection<EntityItem>(m_rootItems);

            InitializeComponent();
        }

        #endregion

        #region Destructors and Dispose Methods

        public void Dispose()
        {
            UnsubscribeSdkEngine();
        }

        #endregion

        #region Event Handlers

        protected virtual void OnSelectedItemChanged(Guid item)
        {
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            List<EntityItem> entityItems = new List<EntityItem>();
            foreach (EntityUpdateInfo entityUpdateInfo in e.Entities)
            {
                List<EntityItem> items;
                if (m_items.TryGetValue(entityUpdateInfo.EntityGuid, out items))
                {
                    entityItems.AddRange(items);
                }
            }
            //Update the Ui thread
            Dispatcher.BeginInvoke(new Action(() => {
                    foreach (EntityItem item in entityItems)
                    {
                        item.Update();
                    }
                }));
        }

        private void OnLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Refresh();
        }

        private void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Refresh();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EntityItem item = treeView.SelectedItem as EntityItem;
            if (item != null)
            {
                SelectedItem = item.Guid;
            }
            OnSelectedItemChanged(SelectedItem);
        }

        private void OnTreeViewItemMouseDown(object sender, MouseButtonEventArgs e)
        {
            m_canDragDrop = true;
        }

        private void OnTreeViewItemMouseMove(object sender, MouseEventArgs e)
        {
            TreeViewItem treeViewItem = sender as TreeViewItem;
            if (treeViewItem != null && treeViewItem.IsSelected && m_canDragDrop && e.LeftButton == MouseButtonState.Pressed)
            {
                if (DragDrop.DoDragDrop(this, SelectedItem, DragDropEffects.Copy) == DragDropEffects.None)
                {
                    m_canDragDrop = false;
                }
            }
        }
        
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var bookmarkDialog = new AddBookmark(SdkEngine, SelectedItem);
            bookmarkDialog.ShowDialog();
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdk)
        {
            if (sdk == null)
                throw new ArgumentNullException("sdk");

            SdkEngine = sdk;

            SubscribeSdkEngine();
            Refresh();
        }

        public void Refresh()
        {
            ClearItems();
            m_rootItems.Clear();
            Entity entity = SdkEngine.GetEntity(SdkGuids.SystemConfiguration);
            if (entity != null)
            {
                m_rootItems.Add(new EntityItem(SdkEngine, this, entity));
            }
        }

        #endregion

        #region Internal Methods

        internal void AddItem(EntityItem item)
        {
            List<EntityItem> items = GetItemList(item.Guid);
            items.Add(item);
        }

        internal void ClearItems()
        {
            m_items.Clear();
        }

        internal void RemoveItem(EntityItem item)
        {
            List<EntityItem> items = GetItemList(item.Guid);
            items.Remove(item);
        }

        #endregion

        #region Private Methods

        private List<EntityItem> GetItemList(Guid id)
        {
            List<EntityItem> lst;
            if (m_items.ContainsKey(id))
            {
                lst = m_items[id];
            }
            else
            {
                lst = new List<EntityItem>();
                m_items.Add(id, lst);
            }
            return lst;
        }

        private void SubscribeSdkEngine()
        {
            if (SdkEngine != null)
            {
                SdkEngine.LoginManager.LoggedOn += OnLoggedOn;
                SdkEngine.LoginManager.LoggedOff += OnLoggedOff;
                SdkEngine.EntitiesInvalidated += OnEntitiesInvalidated;
            }
        }

        private void UnsubscribeSdkEngine()
        {
            if (SdkEngine != null)
            {
                SdkEngine.LoginManager.LoggedOn -= OnLoggedOn;
                SdkEngine.LoginManager.LoggedOff -= OnLoggedOff;
                SdkEngine.EntitiesInvalidated -= OnEntitiesInvalidated;
            }
        }

        #endregion
    }

    #endregion
}

