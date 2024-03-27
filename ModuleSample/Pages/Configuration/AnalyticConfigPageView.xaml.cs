// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using System;

namespace ModuleSample.Pages.Configuration
{
    /// <summary>
    /// Interaction logic for AnalyticConfigPageView.xaml
    /// </summary>
    public partial class AnalyticConfigPageView
    {

        #region Public Properties

        public AnalyticConfigPage Presenter
        {
            get;
        }

        #endregion Public Properties

        #region Protected Properties

        protected Workspace Workspace
        {
            get;
            private set;
        }

        #endregion Protected Properties

        #region Public Constructors

        public AnalyticConfigPageView(AnalyticConfigPage presenter)
        {
            Presenter = presenter;
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        }

        #endregion Public Methods

    }
}