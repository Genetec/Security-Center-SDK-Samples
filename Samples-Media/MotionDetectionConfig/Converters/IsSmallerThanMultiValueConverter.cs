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

    public sealed class IsSmallerThanMultiValueConverter : IMultiValueConverter
    {
        #region Public Methods

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2)
            {
                if ((values[0] is int) && (values[1] is int))
                {
                    return (int)values[0] < (int)values[1];
                }
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { };
        }

        #endregion
    }

    #endregion
}

