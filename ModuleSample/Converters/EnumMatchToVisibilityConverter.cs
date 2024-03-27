// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ModuleSample.Converters
{
    public class EnumMatchToVisibilityConverter : IValueConverter
    {

        #region Public Properties

        public bool Invert { get; set; }

        public Visibility VisibilityWhenMatch { get; set; }

        public Visibility VisibilityWhenNoMatch { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public EnumMatchToVisibilityConverter()
        {
            VisibilityWhenMatch = Visibility.Visible;
            VisibilityWhenNoMatch = Visibility.Collapsed;
        }

        #endregion Public Constructors

        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            var isMatch = false;

            if (value is Enum enumValue && parameter is Enum parameterValue)
            {
                // Enum should be of the same type
                if (enumValue.GetType() == parameterValue.GetType())
                {
                    isMatch = enumValue.Equals(parameterValue);
                }
                else
                {
                    Debug.Assert(false, "EnumMatchToVisibilityConverter -> parameter is a different type than the value.");
                }
            }
            else
            {
                if (value is Enum && !string.IsNullOrEmpty(parameter.ToString()))
                {
                    isMatch = (value.ToString().Equals(parameter.ToString(), StringComparison.InvariantCultureIgnoreCase));
                }
            }

            return isMatch ^ Invert ? VisibilityWhenMatch : VisibilityWhenNoMatch;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion Public Methods

    }
}