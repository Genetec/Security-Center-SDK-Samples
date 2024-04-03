using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.Video.MotionDetection;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Media;
using MotionDetectionConfig.Dialogs;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MotionDetectionConfig
{
    #region Classes

    /// <summary>
    /// =============================================================
    ///  USING GENETEC.SDK.MEDIA
    /// =============================================================
    /// Projects requiring the usage of the Genetec.Sdk.Media assembly
    /// should add the following "Post-Build step":
    /// xcopy /R /Y "$(GSC_SDK)avcodec*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)avformat*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)avutil*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.*MediaComponent*" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.Nvidia.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.QuickSync.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)swscale*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)swresample*.dll" "$(TargetDir)"
    /// This command will copy to the output of the project the EXE
    /// and configuration files required for out-of-process decoding
    /// for native and federated video streams.  Out-of-process 
    /// decoding is a feature that provides:
    ///  - Improved memory usage for video operations by spreading
    ///    the memory usage over several processes
    ///  - Enhanced fault isolation when decoding video streams.  
    /// =============================================================
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        public static readonly DependencyProperty CurrentCameraProperty =
                                    DependencyProperty.Register
                                    ("CurrentCamera", typeof(Camera), typeof(MainWindow),
                                    new PropertyMetadata(null));

        public static readonly DependencyProperty IsLoggedOnProperty =
                                    DependencyProperty.Register
                                    ("IsLoggedOn", typeof(bool), typeof(MainWindow),
                                    new PropertyMetadata(false, OnPropertyIsLoggedOnChanged));

        public static readonly DependencyProperty SelectedConfigurationCtlProperty =
                                    DependencyProperty.Register
                                    ("SelectedConfigurationCtl", typeof(MotionDetectionConfigurationCtl), typeof(MainWindow),
                                    new PropertyMetadata(null));

        /// <summary>
        /// Mapping of the different MotionDetectionConfigurationCtls to the configuration they represent
        /// </summary>
        private readonly Dictionary<MotionDetectionConfigurationCtl, IMotionDetectionConfiguration> m_ctlConfigMappings = new Dictionary<MotionDetectionConfigurationCtl, IMotionDetectionConfiguration>();

        /// <summary>
        /// The instance of the SDK Engine we are using to connect with and query the server
        /// </summary>
        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields

        /// <summary>
        /// AsyncResult used when logging on (used to cancel the logon)
        /// </summary>
        private IAsyncResult m_logOnAsyncResult;

        /// <summary>
        /// The instance of the media player used to display the video of the camera and configure the motion detection map
        /// </summary>
        private MediaPlayer m_mediaPlayer;

        /// <summary>
        /// The capabilities indicating what is supported by this camera in the motion detection configuration
        /// </summary>
        private MotionDetectionCapabilities m_motionDetectionCapabilities;

        /// <summary>
        /// The list of the different motion detection configurations (per schedule) of the current camera
        /// </summary>
        private List<IMotionDetectionConfiguration> m_motionDetectionConfigurations = new List<IMotionDetectionConfiguration>();

        /// <summary>
        /// The currently selected item of the tab control displaying the different motion detection configurations
        /// </summary>
        private TabItem m_selectedTabItem;

        #endregion

        #region Properties

        /// <summary>
        /// The camera that is currently being configured
        /// </summary>
        private Camera CurrentCamera
        {
            get { return (Camera)GetValue(CurrentCameraProperty); }
            set { SetValue(CurrentCameraProperty, value); }
        }

        /// <summary>
        /// Flag indicating if we are currently logged on
        /// </summary>
        private bool IsLoggedOn
        {
            get { return (bool)GetValue(IsLoggedOnProperty); }
            set { SetValue(IsLoggedOnProperty, value); }
        }

        /// <summary>
        /// The currently selected MotionDetectionConfigurationCtl
        /// </summary>
        private MotionDetectionConfigurationCtl SelectedConfigurationCtl
        {
            get { return (MotionDetectionConfigurationCtl)GetValue(SelectedConfigurationCtlProperty); }
            set { SetValue(SelectedConfigurationCtlProperty, value); }
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            //Instantiate the SDK Engine and subscribe to its log on events
            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            m_mediaPlayer.Dispose();
            MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        private static void OnPropertyIsLoggedOnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MainWindow instance = d as MainWindow;
            if (instance != null)
            {
                instance.OnIsLoggedOnChanged();
            }
        }

        private void OnButtonAddScheduleClick(object sender, RoutedEventArgs e)
        {
            //Get the schedules that are already configured
            List<Guid> schedules = new List<Guid>();
            foreach (IMotionDetectionConfiguration configuration in m_motionDetectionConfigurations)
            {
                schedules.Add(configuration.Schedule);
            }

            //Display the Add schedule dialog (and pass the already configured schedule so that they are not available for selection)
            AddScheduleDlg dlg = new AddScheduleDlg();
            dlg.Initialize(m_sdkEngine, schedules);
            dlg.Owner = this;
            bool? result = dlg.ShowDialog();

            if (result.HasValue && result.Value && (dlg.SelectedSchedule != null))
            {
                //If the user selected a schedule, create a new MotionDetectionConfiguration and add an item in the tab control to represent it
                Schedule schedule = dlg.SelectedSchedule;
                IMotionDetectionConfiguration configuration = MotionDetectionConfigurationBuilder.CreateMotionDetectionConfigurationInstance(dlg.SelectedSchedule.Guid);
                m_motionDetectionConfigurations.Add(configuration);

                TabItem item = new TabItem {Header = schedule.Name};
                MotionDetectionConfigurationCtl configurationCtl = new MotionDetectionConfigurationCtl();
                configurationCtl.Initialize(m_sdkEngine, configuration, m_motionDetectionCapabilities, m_mediaPlayer);
                item.Content = configurationCtl;
                m_ctlConfigMappings.Add(configurationCtl, configuration);
                m_tabControl.Items.Add(item);

                //Select the tab of the new schedule
                m_tabControl.SelectedIndex = m_tabControl.Items.Count - 1;
            }
        }

        private void OnButtonConnectionClick(object sender, RoutedEventArgs e)
        {
            if (m_logOnAsyncResult != null)
            {
                //If we are already trying to connect, cancel the log on
                m_btnConnection.Content = "Connect";
                m_sdkEngine.LoginManager.EndLogOn(m_logOnAsyncResult);
                m_logOnAsyncResult = null;
                return;
            }

            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                //If we are not connected, try to connect to the server
                m_btnConnection.Content = "Cancel";
                m_logOnAsyncResult = m_sdkEngine.LoginManager.BeginLogOn(m_tbDirectory.Text, m_tbUsername.Text, m_tbPassword.Password);
            }
            else
            {
                //If we are already connected, log off
                m_sdkEngine.LoginManager.LogOff();
            }
        }

        private void OnButtonRemoveScheduleClick(object sender, RoutedEventArgs e)
        {
            if (SelectedConfigurationCtl != null)
            {
                //Remove the currently selected schedule's configuration
                m_motionDetectionConfigurations.Remove(m_ctlConfigMappings[SelectedConfigurationCtl]);
                m_ctlConfigMappings.Remove(SelectedConfigurationCtl);
                m_tabControl.Items.Remove(m_selectedTabItem);
            }
        }

        private void OnButtonSaveClick(object sender, RoutedEventArgs e)
        {
            if (CurrentCamera != null)
            {
                //Start a transaction to update the config
                m_sdkEngine.TransactionManager.CreateTransaction();

                try
                {
                    //List all the schedules configurations of the camera
                    List<Guid> schedulesToRemove = new List<Guid>();
                    ReadOnlyCollection<IMotionDetectionConfiguration> configs = CurrentCamera.MotionDetectionConfigurations;
                    foreach (IMotionDetectionConfiguration config in configs)
                    {
                        schedulesToRemove.Add(config.Schedule);
                    }

                    //Go through every MotionDetectionConfigurationCtl, save its settings in the config and update the config of the camera
                    foreach (KeyValuePair<MotionDetectionConfigurationCtl, IMotionDetectionConfiguration> item in m_ctlConfigMappings)
                    {
                        item.Key.Save();
                        CurrentCamera.UpdateMotionDetectionConfiguration(item.Value);

                        //If we updated a config, we do not need to remove its schedule
                        if (schedulesToRemove.Contains(item.Value.Schedule))
                        {
                            schedulesToRemove.Remove(item.Value.Schedule);
                        }
                    }

                    //All the remaining schedules in the list need to be removed from the config
                    foreach (Guid schedule in schedulesToRemove)
                    {
                        CurrentCamera.RemoveMotionDetectionConfiguration(schedule);
                    }

                    //End the transaction to push all the new settings to the directory
                    m_sdkEngine.TransactionManager.CommitTransaction();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    //An error occurred, roll back the changes
                    m_sdkEngine.TransactionManager.RollbackTransaction();
                }
            }
        }

        private void OnButtonUpdateCameraClick(object sender, RoutedEventArgs e)
        {
            m_ctlConfigMappings.Clear();
            m_tabControl.Items.Clear();

            if (m_mediaPlayer == null)
            {
                m_mediaPlayer = new MediaPlayer();
                m_mediaPlayer.HardwareAccelerationEnabled = true;
            }

            int cameraId;
            if (Int32.TryParse(m_tbCameraId.Text, out cameraId))
            {
                //Try to get the camera corresponding to the provided logical id
                CurrentCamera = m_sdkEngine.GetEntity(EntityType.Camera, cameraId) as Camera;
                if (CurrentCamera != null)
                {
                    //If successful, get the motion detection configurations of the camera and initialize the media player to display its video
                    m_motionDetectionConfigurations = new List<IMotionDetectionConfiguration>(CurrentCamera.MotionDetectionConfigurations);
                    m_motionDetectionCapabilities = CurrentCamera.MotionDetectionCapabilities;
                    m_mediaPlayer.Stop();
                    m_mediaPlayer.Initialize(m_sdkEngine, CurrentCamera.Guid);
                    m_mediaPlayer.PlayLive();
                }

                //Go through all the configrations and add a tab to represent them
                foreach (IMotionDetectionConfiguration configuration in m_motionDetectionConfigurations)
                {
                    Entity schedule = m_sdkEngine.GetEntity(configuration.Schedule);
                    if (schedule != null)
                    {
                        TabItem item = new TabItem {Header = schedule.Name};
                        MotionDetectionConfigurationCtl configurationCtl = new MotionDetectionConfigurationCtl();
                        configurationCtl.Initialize(m_sdkEngine, configuration, m_motionDetectionCapabilities, m_mediaPlayer);
                        item.Content = configurationCtl;
                        m_ctlConfigMappings.Add(configurationCtl, configuration);
                        m_tabControl.Items.Add(item);
                    }
                }

                if (m_tabControl.Items.Count > 0)
                {
                    //Select the first tab
                    m_tabControl.SelectedIndex = 0;
                }
            }
        }

        private void OnDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                            "The certificate is not from a trusted certifying authority. \n" +
                            "Do you trust this server?", "Secure Communication", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_logOnAsyncResult = null;
                m_btnConnection.Content = "Connect";
                IsLoggedOn = false;
            }));
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_btnConnection.Content = "Disconnect";
                IsLoggedOn = true;
            }));
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_logOnAsyncResult = null;
                m_btnConnection.Content = "Connect";
                IsLoggedOn = false;
                MessageBox.Show(e.FormattedErrorMessage);
            }));
        }

        private void OnIsLoggedOnChanged()
        {
            if (!IsLoggedOn)
            {
                //When we log off, clear the selected camera to hide its config
                CurrentCamera = null;

                m_ctlConfigMappings.Clear();
                m_tabControl.Items.Clear();
            }
        }

        private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                TabItem item = e.RemovedItems[0] as TabItem;
                if (item != null)
                {
                    MotionDetectionConfigurationCtl configCtl = item.Content as MotionDetectionConfigurationCtl;
                    if (configCtl != null)
                    {
                        //Hide the video from the last selected tab
                        configCtl.HideVideo();
                        SelectedConfigurationCtl = null;
                    }
                }
            }
            if (e.AddedItems.Count > 0)
            {
                TabItem item = e.AddedItems[0] as TabItem;
                if (item != null)
                {
                    m_selectedTabItem = item;
                    MotionDetectionConfigurationCtl configCtl = item.Content as MotionDetectionConfigurationCtl;
                    if (configCtl != null)
                    {
                        //Show the video in the new selected tab
                        configCtl.ShowVideo();
                        SelectedConfigurationCtl = configCtl;
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}

