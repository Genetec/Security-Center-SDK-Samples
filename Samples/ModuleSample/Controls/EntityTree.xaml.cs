// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ModuleSample.Controls
{

    /// <summary>
    /// Interaction logic for EntityTree.xaml
    /// </summary>
    public partial class EntityTree : IDisposable
    {

        #region Public Fields

        // Dependency properties
        public static readonly DependencyProperty IsCheckableProperty =
                    DependencyProperty.Register
                    ("IsCheckable", typeof(bool), typeof(EntityTree),
                        new PropertyMetadata(true));

        #endregion Public Fields

        #region Private Fields

        private static readonly DependencyPropertyKey SelectedItemPropertyKey =
                    DependencyProperty.RegisterReadOnly
                    ("SelectedItem", typeof(Guid), typeof(EntityTree),
                        new PropertyMetadata(default(Guid)));

        private readonly Dictionary<Guid, List<EntityItem>> m_items = new Dictionary<Guid, List<EntityItem>>();

        private readonly ObservableCollection<EntityItem> m_rootItems = new ObservableCollection<EntityItem>();

        private bool m_isDragging;

        private Point m_lastMouseDown;

        #endregion Private Fields

        #region Public Events

        public event EventHandler SelectedItemChanged;

        public event EventHandler SelectionChanged;

        #endregion Public Events

        #region Public Properties

        public bool IsCheckable
        {
            get { return (bool)GetValue(IsCheckableProperty); }
            set { SetValue(IsCheckableProperty, value); }
        }

        public ReadOnlyObservableCollection<EntityItem> Items { get; private set; }

        public Guid SelectedItem
        {
            get { return (Guid)GetValue(SelectedItemPropertyKey.DependencyProperty); }
            private set { SetValue(SelectedItemPropertyKey, value); }
        }

        public ICollection<Guid> SelectedItems
        {
            get
            {
                HashSet<Guid> lst = new HashSet<Guid>();
                foreach (EntityItem item in m_rootItems)
                {
                    GetSelectedItems(item, lst);
                }
                return lst;
            }
        }

        #endregion Public Properties

        #region Private Properties

        private IEngine Sdk { get; set; }

        private Workspace Workspace { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public EntityTree()
        {
            Items = new ReadOnlyObservableCollection<EntityItem>(m_rootItems);

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            if (Sdk != null)
            {
                Sdk.LoginManager.LoggedOn -= OnEngineLoggedOn;
                Sdk.LoginManager.LoggedOff -= OnEngineLoggedOff;
                Sdk.EntitiesInvalidated -= OnEngineEntitiesInvalidated;
            }
        }

        public void Initialize(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");

            Workspace = workspace;
            Sdk = workspace.Sdk;

            Sdk.LoginManager.LoggedOn += OnEngineLoggedOn;
            Sdk.LoginManager.LoggedOff += OnEngineLoggedOff;
            Sdk.EntitiesInvalidated += OnEngineEntitiesInvalidated;

            Refresh();
        }

        public void Refresh()
        {
            ClearItems();
            m_rootItems.Clear();
            Entity entity = Sdk.GetEntity(SdkGuids.SystemConfiguration);
            if (entity != null)
            {
                m_rootItems.Add(new EntityItem(Sdk, this, entity));
            }
        }

        #endregion Public Methods

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

        internal void OnItemIsCheckedChanged(EntityItem item)
        {
            OnSelectionChanged();
        }

        internal void RemoveItem(EntityItem item)
        {
            List<EntityItem> items = GetItemList(item.Guid);
            items.Remove(item);
        }

        #endregion Internal Methods

        #region Protected Methods

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

        #endregion Protected Methods

        #region Private Methods

        private static void GetSelectedItems(EntityItem item, ICollection<Guid> selectedItems)
        {
            if (item.IsChecked)
            {
                selectedItems.Add(item.Guid);
            }

            // The Empty item doesn't have children
            if (item.ChildrenView != null)
            {
                foreach (EntityItem child in item.ChildrenView)
                {
                    GetSelectedItems(child, selectedItems);
                }
            }
        }

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

        private void OnEngineEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            foreach (EntityUpdateInfo updateInfo in e.Entities)
            {
                List<EntityItem> items;
                if (m_items.TryGetValue(updateInfo.EntityGuid, out items))
                {
                    foreach (EntityItem item in items)
                    {
                        item.Update();
                    }
                }
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Refresh();
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Refresh();
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            EntityItem item = m_treeView.SelectedItem as EntityItem;
            if (item != null)
            {
                SelectedItem = item.Guid;
            }
            OnSelectedItemChanged(SelectedItem);
        }

        private void OnTreeViewItemMouseDown(object sender, MouseEventArgs e)
        {
            m_lastMouseDown = e.GetPosition((IInputElement)sender);
        }

        private void OnTreeViewItemMouseMove(object sender, MouseEventArgs e)
        {
            if (!m_isDragging && (e.LeftButton == MouseButtonState.Pressed))
            {
                Point current = e.GetPosition((IInputElement)sender);

                bool startDrag = Math.Abs(current.X - m_lastMouseDown.X) >= SystemParameters.MinimumHorizontalDragDistance ||
                                 Math.Abs(current.Y - m_lastMouseDown.Y) >= SystemParameters.MinimumVerticalDragDistance;

                if (startDrag)
                {
                    try
                    {
                        m_isDragging = true;
                        DragDrop.DoDragDrop(this, SelectedItem, DragDropEffects.Copy | DragDropEffects.Link);
                    }
                    finally
                    {
                        m_isDragging = false;
                    }
                }
                e.Handled = true;
            }
        }

        #endregion Private Methods

        #region Public Classes

        public sealed class EntityItem : DependencyObject
        {

            #region Public Fields

            public static readonly DependencyProperty IsCheckedProperty =
                            DependencyProperty.Register
                            ("IsChecked", typeof(bool), typeof(EntityItem),
                                new PropertyMetadata(false, OnPropertyIsCheckedChanged));

            public static readonly DependencyProperty IsExpandedProperty =
                            DependencyProperty.Register
                            ("IsExpanded", typeof(bool), typeof(EntityItem),
                                new PropertyMetadata(false, OnPropertyIsExpandedChanged));

            #endregion Public Fields

            #region Private Fields

            // Dependency properties
            private static readonly DependencyPropertyKey DescriptionPropertyKey =
                            DependencyProperty.RegisterReadOnly
                            ("Description", typeof(string), typeof(EntityItem),
                                new PropertyMetadata());

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

            private readonly IEngine m_sdk;

            private bool m_internalCheck;

            #endregion Private Fields

            #region Public Properties

            public ReadOnlyObservableCollection<EntityItem> Children { get; private set; }

            public ICollectionView ChildrenView { get; private set; }

            public string Description
            {
                get { return (string)GetValue(DescriptionPropertyKey.DependencyProperty); }
                private set { SetValue(DescriptionPropertyKey, value); }
            }

            public Guid Guid { get; private set; }

            public ImageSource Icon
            {
                get { return (ImageSource)GetValue(IconPropertyKey.DependencyProperty); }
                private set { SetValue(IconPropertyKey, value); }
            }

            public bool IsChecked
            {
                get { return (bool)GetValue(IsCheckedProperty); }
                set { SetValue(IsCheckedProperty, value); }
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

            public EntityItem Parent { get; private set; }

            public EntityTree Tree { get; private set; }

            #endregion Public Properties

            #region Internal Constructors

            internal EntityItem(IEngine sdk, EntityTree tree, Entity entity)
                            : this(sdk, tree, entity, null)
            {
            }

            internal EntityItem(IEngine sdk, EntityTree tree, Entity entity, EntityItem parent)
            {
                if (sdk == null)
                    throw new ArgumentNullException("sdk");

                if (entity == null)
                    throw new ArgumentNullException("entity");

                m_children.Add(Empty);
                Children = new ReadOnlyObservableCollection<EntityItem>(m_children);
                ChildrenView = new ListCollectionView(m_children);

                m_sdk = sdk;
                m_entity = entity;
                Tree = tree;
                Parent = parent;
                Guid = m_entity.Guid;

                Update();
                InheritCheck();

                tree.AddItem(this);
            }

            #endregion Internal Constructors

            #region Private Constructors

            /// <summary>
            /// Constructor used for the empty item
            /// </summary>
            private EntityItem()
            {
            }

            #endregion Private Constructors

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
                    Entity entity = m_sdk.GetEntity(id);
                    if (entity != null)
                    {
                        EntityItem item = new EntityItem(m_sdk, Tree, entity, this);
                        m_children.Add(item);
                    }
                }
            }

            public override string ToString()
            {
                return string.Format("{0} ({1})", m_entity.Name, m_entity.EntityType);
            }

            public void Update()
            {
                Name = m_entity.Name;
                Description = m_entity.Description;
                Icon = m_entity.GetIcon(true);
            }

            #endregion Public Methods

            #region Private Methods

            private static void OnPropertyIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var instance = d as EntityItem;
                if (instance != null)
                {
                    instance.OnIsCheckedChanged();
                }
            }

            private static void OnPropertyIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var instance = d as EntityItem;
                if (instance != null)
                {
                    instance.OnIsExpandedChanged();
                }
            }

            private void InheritCheck()
            {
                if (Parent != null)
                {
                    try
                    {
                        m_internalCheck = true;
                        IsChecked = Parent.IsChecked;
                    }
                    finally
                    {
                        m_internalCheck = false;
                    }
                }
            }

            private void OnIsCheckedChanged()
            {
                foreach (EntityItem item in m_children)
                {
                    item.InheritCheck();
                }

                if (!m_internalCheck)
                {
                    // Inform the tree that we have been explcitly checked
                    Tree.OnItemIsCheckedChanged(this);
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

            #endregion Private Methods
        }

        #endregion Public Classes
    }

}