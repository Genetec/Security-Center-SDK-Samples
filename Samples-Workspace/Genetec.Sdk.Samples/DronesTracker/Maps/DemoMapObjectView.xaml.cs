// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows;
using System.Windows.Media;

namespace DronesTracker.Maps
{
    /// <summary>
    /// Interaction logic for DemoMapObjectView.xaml
    /// </summary>
    public partial class DemoMapObjectView
    {

        #region Public Fields

        // Dependency properties
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register
            ("Color", typeof(SolidColorBrush), typeof(DemoMapObjectView),
            new PropertyMetadata(Brushes.Blue));

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register
            ("Image", typeof(ImageSource), typeof(DemoMapObjectView),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register
            ("Title", typeof(string), typeof(DemoMapObjectView));

        #endregion Public Fields

        #region Public Properties

        public SolidColorBrush Color
        {
            get => (SolidColorBrush)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public DemoMapObjectView()
        {
            InitializeComponent();
            RenderOptions.SetBitmapScalingMode(m_image, BitmapScalingMode.Fant);
        }

        public DemoMapObjectView(DemoMapObject mapObject)
            : this()
        {
            Color = new SolidColorBrush(mapObject.Color);
            Image = mapObject.Image;
            Title = mapObject.Name;

            Initialize(mapObject);
        }

        #endregion Public Constructors

    }
}