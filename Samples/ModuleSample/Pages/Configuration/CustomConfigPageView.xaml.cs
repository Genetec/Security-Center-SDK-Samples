// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using System;
using System.Windows.Controls;

namespace ModuleSample.Pages.Configuration
{

    /// <summary>
    /// Interaction logic for CustomConfigPageView.xaml
    /// </summary>
    public partial class CustomConfigPageView : UserControl
    {

        #region Public Properties

        public CustomConfigPage Presenter { get; private set; }

        #endregion Public Properties

        #region Protected Properties

        protected Workspace Workspace { get; private set; }

        #endregion Protected Properties

        #region Public Constructors

        public CustomConfigPageView(CustomConfigPage presenter)
        {
            Presenter = presenter;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentNullException("Null Workspace");
        }

        #endregion Public Methods

    }

}