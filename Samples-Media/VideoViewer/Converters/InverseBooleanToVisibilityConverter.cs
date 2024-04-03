using System;
using System.Windows;
using System.Windows.Data;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VideoViewer.Converters
{
    #region Classes

    /// <summary>
    /// Inverse a bool to visibility.
    /// True  = Hidden
    /// False = Visible
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        // Not used
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    #endregion
}

