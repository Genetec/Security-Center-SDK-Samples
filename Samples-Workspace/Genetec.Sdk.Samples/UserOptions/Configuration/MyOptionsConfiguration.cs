// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows.Media;

namespace UserOptions.Configuration
{
    public class MyOptionsConfiguration
    {
        #region Public Properties

        public Color Color { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsChecked { get; set; }
        public int NumOfItems { get; set; }

        #endregion Public Properties
    }
}