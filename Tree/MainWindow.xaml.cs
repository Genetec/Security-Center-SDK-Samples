using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using System;
using System.Collections.Generic;
using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Tree
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private Engine m_sdkEngine = new Engine();

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            m_tree.IsCheckable = false;
            m_tree.EntityFilter = new List<EntityType> { EntityType.Area, EntityType.Camera, EntityType.Door, EntityType.Zone };
            m_tree.Initialize(m_sdkEngine);
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

        }

        #endregion

        #region Event Handlers

        private void OnButtonLogOffClick(object sender, RoutedEventArgs e)
        {
            m_sdkEngine.LoginManager.BeginLogOff();
        }

        private void OnButtonLogonClick(object sender, RoutedEventArgs e)
        {
            m_sdkEngine.LoginManager.BeginLogOn("", "admin", "");
        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                      "The certificate is not from a trusted certifying authority. \n" +
                                                      "Do you trust this server?", "Secure Communication", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs loggedOffEventArgs)
        {
            UpdateLogonButtons(true);
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs loggedOnEventArgs)
        {
            UpdateLogonButtons(false);
        }

        private void OnTreeSelecetedItemChanged(object sender, EventArgs e)
        {
            Entity entity = m_sdkEngine.GetEntity(m_tree.SelectedItem);
            if (entity != null)
            {
                m_treeInformation.AppendText(string.Format("{0}, {1}, {2} \r\n", entity.Name, entity.EntityType, entity.Description));
            }
        }

        #endregion

        #region Private Methods

        private void UpdateLogonButtons(bool enabled)
        {
            m_logOn.IsEnabled = enabled;
            m_logOff.IsEnabled = !enabled;
        }

        #endregion
    }

    #endregion
}

