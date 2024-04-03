// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Workspace.Explorer.Theme
{
    public class VectorButton : Button
    {

        #region Public Fields

        public static readonly DependencyPropertyKey IconColorMouseOverPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IconColorMouseOver), typeof(Brush), typeof(VectorButton), new UIPropertyMetadata());
        public static readonly DependencyPropertyKey IconColorPressedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IconColorPressed), typeof(Brush), typeof(VectorButton), new UIPropertyMetadata());
        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(VectorButton), new UIPropertyMetadata(null, new PropertyChangedCallback(OnPropertyIconColorChanged)));
        public static readonly DependencyProperty IconGeometryProperty = DependencyProperty.Register(nameof(IconGeometry), typeof(Geometry), typeof(VectorButton), new UIPropertyMetadata(null));
        public static readonly DependencyProperty IconPositionProperty = DependencyProperty.Register(nameof(IconPosition), typeof(Dock), typeof(VectorButton), new UIPropertyMetadata(Dock.Left));
        public static readonly DependencyProperty IconRotationProperty = DependencyProperty.Register(nameof(IconRotation), typeof(double), typeof(VectorButton), new UIPropertyMetadata(0.0));
        public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(double), typeof(VectorButton), new UIPropertyMetadata(16.0));
        public static readonly DependencyProperty IsBackgroundAnimatedProperty = DependencyProperty.Register(nameof(IsBackgroundAnimated), typeof(bool), typeof(VectorButton), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsDropDownVisibleProperty = DependencyProperty.Register(nameof(IsDropDownVisible), typeof(bool), typeof(VectorButton), new UIPropertyMetadata(false));
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(VectorButton), new UIPropertyMetadata(null));

        #endregion Public Fields

        #region Public Properties

        public Brush IconColor
        {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public Brush IconColorMouseOver
        {
            get => (Brush)GetValue(IconColorMouseOverPropertyKey.DependencyProperty);
            private set => SetValue(IconColorMouseOverPropertyKey, value);
        }

        public Brush IconColorPressed
        {
            get => (Brush)GetValue(IconColorPressedPropertyKey.DependencyProperty);
            private set => SetValue(IconColorPressedPropertyKey, value);
        }

        public Geometry IconGeometry
        {
            get => (Geometry)GetValue(IconGeometryProperty);
            set => SetValue(IconGeometryProperty, value);
        }

        public Dock IconPosition
        {
            get => (Dock)GetValue(IconPositionProperty);
            set => SetValue(IconPositionProperty, value);
        }

        public double IconRotation
        {
            get => (double)GetValue(IconRotationProperty);
            set => SetValue(IconRotationProperty, value);
        }

        public double IconSize
        {
            get => (double)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public bool IsBackgroundAnimated
        {
            get => (bool)GetValue(IsBackgroundAnimatedProperty);
            set => SetValue(IsBackgroundAnimatedProperty, value);
        }

        public bool IsDropDownVisible
        {
            get => (bool)GetValue(IsDropDownVisibleProperty);
            set => SetValue(IsDropDownVisibleProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public VectorButton()
        {
            Loaded += OnLoaded;
        }

        #endregion Public Constructors

        #region Private Methods

        private static void OnPropertyIconColorChanged(
          DependencyObject d,
          DependencyPropertyChangedEventArgs e)
        {
            var vectorButton = d as VectorButton;
            vectorButton?.OnIconColorChanged();
        }

        private void OnIconColorChanged()
        {
            var iconColor = (SolidColorBrush)IconColor;
            if (iconColor == null)
                return;
            var color = iconColor.Color;
            var num1 = (int)(byte)Math.Min(color.R + 16, byte.MaxValue);
            color = iconColor.Color;
            var num2 = (int)(byte)Math.Min(color.G + 16, byte.MaxValue);
            color = iconColor.Color;
            var num3 = (int)(byte)Math.Min(color.B + 16, byte.MaxValue);
            IconColorMouseOver = new SolidColorBrush(Color.FromRgb((byte)num1, (byte)num2, (byte)num3));
            color = iconColor.Color;
            var num4 = (int)(byte)Math.Max(color.R - 16, 0);
            color = iconColor.Color;
            var num5 = (int)(byte)Math.Max(color.G - 16, 0);
            color = iconColor.Color;
            var num6 = (int)(byte)Math.Max(color.B - 16, 0);
            IconColorPressed = new SolidColorBrush(Color.FromRgb((byte)num4, (byte)num5, (byte)num6));
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (IconColor != null)
                return;
            SetResourceReference(IconColorProperty, "AccentColorBrush");
        }

        #endregion Private Methods

    }
}