// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media;
using Genetec.Sdk.Workspace.Options;
using UserOptions.Extensions;
using UserOptions.Views;

namespace UserOptions.Pages
{
    public sealed class MyOptionsPage : OptionPage
    {

        #region Public Fields

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                "Color", typeof(Color), typeof(MyOptionsPage),
                new PropertyMetadata(default(Color), OnPropertyChanged));

        public static readonly DependencyProperty DateTimeProperty =
            DependencyProperty.Register(
                "DateTime", typeof(DateTime), typeof(MyOptionsPage),
                new PropertyMetadata(default(DateTime), OnPropertyChanged));

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(
                "IsChecked", typeof(bool), typeof(MyOptionsPage),
                new PropertyMetadata(default(bool), OnPropertyChanged));

        public static readonly DependencyProperty NumOfItemsProperty =
            DependencyProperty.Register(
            "NumOfItems", typeof(int), typeof(MyOptionsPage),
            new PropertyMetadata(default(int), OnPropertyChanged));

        #endregion Public Fields

        #region Private Fields

        private readonly MyOptionsExtension m_extension;
        private readonly MyOptionsPageView m_view;

        #endregion Private Fields

        #region Public Properties

        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public DateTime DateTime
        {
            get => (DateTime)GetValue(DateTimeProperty);
            set => SetValue(DateTimeProperty, value);
        }

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        // Dependency properties
        public int NumOfItems
        {
            get => (int)GetValue(NumOfItemsProperty);
            set => SetValue(NumOfItemsProperty, value);
        }
        public override UIElement View => m_view;

        #endregion Public Properties

        #region Public Constructors

        public MyOptionsPage(MyOptionsExtension extension)
        {
            m_extension = extension;
            m_view = new MyOptionsPageView(this);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load()
        {
            NumOfItems = m_extension.NumOfItems;
            DateTime = m_extension.DateTime;
            IsChecked = m_extension.IsChecked;
            Color = m_extension.Color;
        }

        public override void Save()
        {
            m_extension.NumOfItems = NumOfItems;
            m_extension.DateTime = DateTime;
            m_extension.IsChecked = IsChecked;
            m_extension.Color = Color;
        }

        public override bool Validate() => true;

        #endregion Public Methods

        #region Private Methods

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as MyOptionsPage;
            instance?.OnModified();
        }

        #endregion Private Methods
    }
}