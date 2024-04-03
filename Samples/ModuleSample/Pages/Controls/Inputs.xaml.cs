// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;

namespace ModuleSample.Pages.Controls
{
    /// <summary>
    /// Interaction logic for Inputs.xaml
    /// </summary>
    public partial class Inputs
    {

        #region Public Fields

        public static readonly DependencyProperty EditableNumericUpDownFormattedTextProperty =
            DependencyProperty.Register
            ("EditableNumericUpDownFormattedText", typeof(string), typeof(Inputs),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty EditableNumericUpDownValueProperty =
            DependencyProperty.Register
            ("EditableNumericUpDownValue", typeof(long), typeof(Inputs),
            new PropertyMetadata((long)0));

        #endregion Public Fields

        #region Public Properties

        public string EditableNumericUpDownFormattedText
        {
            get => (string)GetValue(EditableNumericUpDownFormattedTextProperty);
            set => SetValue(EditableNumericUpDownFormattedTextProperty, value);
        }

        public long EditableNumericUpDownValue
        {
            get => (long)GetValue(EditableNumericUpDownValueProperty);
            set => SetValue(EditableNumericUpDownValueProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public Inputs()
        {
            InitializeComponent();

            EditableNumericUpDownValue = 5;
        }

        #endregion Public Constructors

    }
}