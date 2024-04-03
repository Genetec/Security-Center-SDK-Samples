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

    sealed internal class EnumMatchToBooleanConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is Enum) && (parameter != null) && !string.IsNullOrEmpty(parameter.ToString()))
            {
                if (value.ToString().Equals(parameter.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value is bool) && (parameter != null))
            {
                if ((bool)value)
                {
                    return Enum.Parse(targetType, parameter.ToString(), true);
                }
            }

            return null;
        }

        #endregion
    }

    #endregion
}

