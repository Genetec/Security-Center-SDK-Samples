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

    internal sealed class IsNullToBooleanConverter : IValueConverter
    {
        #region Properties

        public bool IsInverted { get; set; }

        #endregion

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (IsInverted)
            {
                return value != null;
            }

            return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }

    #endregion
}

