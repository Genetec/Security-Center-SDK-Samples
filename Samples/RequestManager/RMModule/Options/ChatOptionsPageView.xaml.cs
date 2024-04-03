// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows.Controls;

namespace RMModule.Options
{

    /// <summary>
    /// Interaction logic for CustomOptionsPage.xaml
    /// </summary>
    public partial class ChatOptionsPageView : UserControl
    {

        #region Public Constructors

        public ChatOptionsPageView(ChatOptionsPage pg)
        {
            DataContext = pg;
            InitializeComponent();
        }

        #endregion Public Constructors

    }

}