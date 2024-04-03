using System;
using System.Globalization;
using System.Windows.Data;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MotionDetectionConfig.Converters
{
    #region Classes

    public sealed class MinimumCountToBooleanConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int minimumCount = 0;
            if (parameter is int)
            {
                minimumCount = (int)parameter;
            }

            if (value is int)
            {
                return (int)value > minimumCount;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    #endregion
}

