// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows;

namespace ModuleSample.Pages.Controls
{
    /// <summary>
    /// Interaction logic for Buttons.xaml
    /// </summary>
    public partial class Buttons
    {

        #region Public Constructors

        public Buttons()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnDownButtonClicked(object sender, RoutedEventArgs e)
        {
            m_downButton.IsEnabled = false;

            m_upButton.IsEnabled = true;
        }

        private void OnLeftButtonClicked(object sender, RoutedEventArgs e)
        {
            m_leftButton.IsEnabled = false;

            m_rightButton.IsEnabled = true;
            m_middleButton.IsEnabled = true;
        }

        private void OnMiddleButtonClicked(object sender, RoutedEventArgs e)
        {
            m_middleButton.IsEnabled = false;

            m_leftButton.IsEnabled = true;
            m_rightButton.IsEnabled = true;
        }
        private void OnRightButtonClicked(object sender, RoutedEventArgs e)
        {
            m_rightButton.IsEnabled = false;
            m_leftButton.IsEnabled = true;
            m_middleButton.IsEnabled = true;
        }

        private void OnUpButtonClicked(object sender, RoutedEventArgs e)
        {
            m_upButton.IsEnabled = false;
            m_downButton.IsEnabled = true;
        }

        #endregion Private Methods
    }
}