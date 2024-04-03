using Genetec.Sdk.Workspace;
using System;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 25:
//  1915 – World War I: The Second Battle of Champagne begins.
//  1955 – The Royal Jordanian Air Force is founded.
//  1964 – The Mozambican War of Independence against Portugal begins.
// ==========================================================================
namespace ACEClient.Pages.Configuration
{
    #region Classes

    /// <summary>
    /// Interaction logic for CustomEntityConfigPageView.xaml
    /// </summary>
    public partial class CustomCameraConfigPageView : UserControl
    {
        #region Properties

        public CustomCameraConfigPage Presenter { get; private set; }

        protected Workspace Workspace { get; private set; }

        #endregion

        #region Constructors

        public CustomCameraConfigPageView(CustomCameraConfigPage presenter)
        {
            Presenter = presenter;
            InitializeComponent();

        }

        #endregion

        #region Public Methods

        public void Initialize(Workspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("Null Workspace");

            Workspace = workspace;
        }

        #endregion

        private void m_childrenComboBox_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Presenter.RefreshAvailableChildren();
        }
    }

    #endregion
}

