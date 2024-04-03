// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Genetec.Sdk;
using Genetec.Sdk.Entities;

namespace Controls
{
    /// <summary>
    /// Interaction logic for EntityTree.xaml
    /// </summary>
    public partial class EntityTree : IDisposable
    {

        #region Public Fields

        // Dependency properties
        public static readonly DependencyProperty IsCheckableProperty =
            DependencyProperty.Register("IsCheckable", typeof(bool), typeof(EntityTree), new PropertyMetadata(true));

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
            get => (bool)GetValue(IsCheckableProperty);
            set => SetValue(IsCheckableProperty, value);
        }

        public ReadOnlyObservableCollection<EntityItem> Items { get; private set; }

        public Guid SelectedItem
        {
            get => (Guid)GetValue(SelectedItemPropertyKey.DependencyProperty);
            private set => SetValue(SelectedItemPropertyKey, value);
        }

        public ICollection<Guid> SelectedItems
        {
            get
            {
                var lst = new HashSet<Guid>();
                foreach (var item in m_rootItems)
                {
                    GetSelectedItems(item, lst);
                }
                return lst;
            }
        }

        #endregion Public Properties

        #region Private Properties

        private IEngine Sdk { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public EntityTree()
        {
            Items = new ReadOnlyObservableCollection<EntityItem>(m_rootItems);
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose() => UnsubscribeSdk();

        public void Initialize(IEngine engine)
        {
            Sdk = engine ?? throw new ArgumentNullException(nameof(engine));
            SubscribeSdk();
            Refresh();
        }

        public void Refresh()
        {
            ClearItems();
            m_rootItems.Clear();
            var entity = Sdk.GetEntity(SdkGuids.SystemConfiguration);
            if (entity != null)
            {
                m_rootItems.Add(new EntityItem(Sdk, this, entity));
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal void AddItem(EntityItem item)
        {
            var items = GetItemList(item.Guid);
            items.Add(item);
        }

        internal void ClearItems() => m_items.Clear();

        internal void OnItemIsCheckedChanged(EntityItem item) => OnSelectionChanged();

        internal void RemoveItem(EntityItem item)
        {
            var items = GetItemList(item.Guid);
            items.Remove(item);
        }

        #endregion Internal Methods

        #region Protected Methods

        protected virtual void OnSelectedItemChanged(Guid item)
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
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
            if (item.ChildrenView == null) return;
            foreach (EntityItem child in item.ChildrenView)
            {
                GetSelectedItems(child, selectedItems);
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

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            foreach (var updateInfo in e.Entities)
            {
                if (!m_items.TryGetValue(updateInfo.EntityGuid, out var items)) continue;
                foreach (var item in items)
                {
                    item.Update();
                }
            }
        }

        private void OnLoggedOff(object sender, LoggedOffEventArgs e) => Refresh();

        private void OnLoggedOn(object sender, LoggedOnEventArgs e) => Refresh();

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (m_treeView.SelectedItem is EntityItem item)
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
            if (m_isDragging || (e.LeftButton != MouseButtonState.Pressed)) return;
            var current = e.GetPosition((IInputElement)sender);
            var startDrag = Math.Abs(current.X - m_lastMouseDown.X) >= SystemParameters.MinimumHorizontalDragDistance ||
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

        private void SubscribeSdk()
        {
            if (Sdk == null) return;
            Sdk.LoggedOn += OnLoggedOn;
            Sdk.LoggedOff += OnLoggedOff;
            Sdk.EntitiesInvalidated += OnEntitiesInvalidated;
        }

        private void UnsubscribeSdk()
        {
            if (Sdk == null) return;
            Sdk.LoggedOn -= OnLoggedOn;
            Sdk.LoggedOff -= OnLoggedOff;
            Sdk.EntitiesInvalidated -= OnEntitiesInvalidated;
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

            public ReadOnlyObservableCollection<EntityItem> Children { get; }

            public ICollectionView ChildrenView { get; private set; }

            public string Description
            {
                get => (string)GetValue(DescriptionPropertyKey.DependencyProperty);
                private set => SetValue(DescriptionPropertyKey, value);
            }

            public Guid Guid { get; }

            public ImageSource Icon
            {
                get => (ImageSource)GetValue(IconPropertyKey.DependencyProperty);
                private set => SetValue(IconPropertyKey, value);
            }

            public bool IsChecked
            {
                get => (bool)GetValue(IsCheckedProperty);
                set => SetValue(IsCheckedProperty, value);
            }

            public bool IsExpanded
            {
                get => (bool)GetValue(IsExpandedProperty);
                set => SetValue(IsExpandedProperty, value);
            }

            public string Name
            {
                get => (string)GetValue(NamePropertyKey.DependencyProperty);
                private set => SetValue(NamePropertyKey, value);
            }

            public EntityItem Parent { get; }

            public EntityTree Tree { get; }

            #endregion Public Properties

            #region Internal Constructors

            internal EntityItem(IEngine sdk, EntityTree tree, Entity entity)
                            : this(sdk, tree, entity, null) { }

            internal EntityItem(IEngine sdk, EntityTree tree, Entity entity, EntityItem parent)
            {
                m_children.Add(Empty);
                Children = new ReadOnlyObservableCollection<EntityItem>(m_children);
                ChildrenView = new ListCollectionView(m_children);

                m_sdk = sdk ?? throw new ArgumentNullException(nameof(sdk));
                m_entity = entity ?? throw new ArgumentNullException(nameof(entity));
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
            private EntityItem() { }

            #endregion Private Constructors

            #region Public Methods

            public void Collapse()
            {
                foreach (var item in m_children)
                {
                    Tree.RemoveItem(item);
                }

                m_children.Clear();
                m_children.Add(Empty);
            }

            public void Expand()
            {
                m_children.Clear();
                foreach (var id in m_entity.HierarchicalChildren)
                {
                    var entity = m_sdk.GetEntity(id);
                    if (entity == null) continue;
                    var item = new EntityItem(m_sdk, Tree, entity, this);
                    m_children.Add(item);
                }
            }

            public override string ToString()
            {
                var text = m_entity != null ? $"{m_entity.Name} ({m_entity.EntityType})" : base.ToString();
                return text;
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
                if (d is EntityItem instance)
                {
                    instance.OnIsCheckedChanged();
                }
            }

            private static void OnPropertyIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            {
                var instance = d as EntityItem;
                instance?.OnIsExpandedChanged();
            }

            private void InheritCheck()
            {
                if (Parent == null) return;
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

            private void OnIsCheckedChanged()
            {
                foreach (var item in m_children)
                {
                    item.InheritCheck();
                }

                if (!m_internalCheck)
                {
                    // Inform the tree that we have been explicitly checked
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