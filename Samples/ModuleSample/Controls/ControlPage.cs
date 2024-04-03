// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ModuleSample.Controls
{

    public class ControlPage : DependencyObject
    {

        #region Public Fields

        public static readonly DependencyProperty ControlProperty = DependencyProperty.Register(
                    "Control", typeof(UserControl), typeof(ControlPage),
                    new PropertyMetadata(null));

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
                    "Icon", typeof(BitmapSource), typeof(ControlPage),
                    new PropertyMetadata(null));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
                    "Title", typeof(string), typeof(ControlPage),
                    new PropertyMetadata(string.Empty));

        #endregion Public Fields

        #region Public Properties

        public UserControl Control
        {
            get => (UserControl)GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }

        public BitmapSource Icon
        {
            get => (BitmapSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        #endregion Public Properties

    }

}