// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;

namespace ModuleSample.Components.ClockWidget
{
    /// <summary>
    /// Interaction logic for ClockWidgetView.xaml
    /// </summary>
    public partial class ClockWidgetView : IDisposable
    {

        #region Public Constructors

        public ClockWidgetView() => InitializeComponent();
        
        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
        }

        #endregion Public Methods
    }
}