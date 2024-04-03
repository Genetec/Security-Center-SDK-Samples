// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Controls;

namespace ModuleSample.Pages
{

    /// <summary>
    /// Interaction logic for WebPageViewSample.xaml
    /// </summary>
    public partial class WebPageViewSample
    {

        #region Public Constructors

        public WebPageViewSample()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize()
        {
            m_genetecWebBrowser.WebBrowserType = WebBrowserType.Chrome;
            m_genetecWebBrowser.Navigate("www.genetec.com");
        }

        #endregion Public Methods

    }

}