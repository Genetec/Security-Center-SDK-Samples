// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows;

namespace ModuleSample.Pages
{

    /// <summary>
    /// Interaction logic for PagePersistenceViewSample.xaml
    /// </summary>
    public partial class PagePersistenceViewSample
    {

        #region Public Fields

        public static readonly DependencyProperty CanSaveAsProperty =
                    DependencyProperty.Register
                    ("CanSaveAs", typeof(bool), typeof(PagePersistenceViewSample),
                        new PropertyMetadata(false));

        public static readonly DependencyProperty MessageProperty =
                    DependencyProperty.Register
                    ("Message", typeof(string), typeof(PagePersistenceViewSample),
                        new PropertyMetadata(string.Empty));

        #endregion Public Fields

        #region Public Properties

        public bool CanSaveAs
        {
            get => (bool)GetValue(CanSaveAsProperty);
            set => SetValue(CanSaveAsProperty, value);
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public PagePersistenceViewSample()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

    }

}