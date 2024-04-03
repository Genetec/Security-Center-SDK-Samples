// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ModuleSample.Pages.Controls
{
    /// <summary>
    /// Interaction logic for ScrollViewers.xaml
    /// </summary>
    public partial class ScrollViewers
    {

        #region Public Constructors

        public ScrollViewers()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnCheckBoxDisplayBackgroundClick(object sender, RoutedEventArgs e)
        {
            if (m_checkBox.IsChecked == true)
            {
                m_image.Source = new BitmapImage(new Uri("pack://application:,,,/ModuleSample;component/Resources/Genetec.jpg"));
            }
            else
            {
                m_image.Source = null;
            }
        }

        #endregion Private Methods

    }
}