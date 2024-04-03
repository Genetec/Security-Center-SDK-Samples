// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using UserOptions.Pages;

namespace UserOptions.Views
{
    /// <summary>
    /// Interaction logic for MyOptionsPageView.xaml
    /// </summary>
    public partial class MyOptionsPageView
    {

        #region Public Constructors

        public MyOptionsPageView(MyOptionsPage page)
        {
            DataContext = page;
            InitializeComponent();
        }

        #endregion Public Constructors

    }
}