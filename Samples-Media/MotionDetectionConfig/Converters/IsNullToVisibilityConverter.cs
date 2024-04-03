using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MotionDetectionConfig.Converters
{
    #region Classes

    public sealed class IsNullToVisibilityConverter : IValueConverter
    {
        #region Properties

        public bool IsInverted { get; set; }

        public Visibility NotVisibleState { get; set; }

        #endregion

        #region Constructors

        public IsNullToVisibilityConverter()
        {
            NotVisibleState = Visibility.Collapsed;
        }

        #endregion

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return IsInverted ? NotVisibleState : Visibility.Visible;
            }

            return IsInverted ? Visibility.Visible : NotVisibleState;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    #endregion
}

