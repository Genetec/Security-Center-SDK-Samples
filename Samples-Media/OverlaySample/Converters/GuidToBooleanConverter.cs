using System;
using System.Windows.Data;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace OverlaySample.Converters
{
    #region Classes

    [ValueConversion(typeof(Guid), typeof(bool))]
    public class GuidToBooleanConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Guid)value != Guid.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

    #endregion
}

