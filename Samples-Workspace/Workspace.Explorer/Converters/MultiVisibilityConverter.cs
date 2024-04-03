// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Workspace.Explorer.Converters
{
    internal class MultiVisibilityConverter : IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility1 = (Visibility)values[0];
            var visibility2 = (Visibility)values[1];

            if (visibility1 == Visibility.Visible && visibility2 == Visibility.Visible)
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods
    }
}