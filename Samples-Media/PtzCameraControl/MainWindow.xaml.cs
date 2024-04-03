using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Genetec.Sdk.Media.Ptz;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace PtzCameraControl
{
    #region Classes

    /// <summary>
    /// Class example on how to control a Ptz Camera for Pan or Zoom 
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

        // Property for the Camera selected index.
        public static readonly DependencyProperty CamerasSelectedIndexProperty = DependencyProperty.Register(
                            "CamerasSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(-1));

        // Property for the Group button to control the Ptz Camera.
        public static readonly DependencyProperty IsPtzCameraSelectedProperty = DependencyProperty.Register(
                            "IsPtzCameraSelected", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Property for the connection to the sdk engine.
        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                            "IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Property for the message about the user locking the camera.
        public static readonly DependencyProperty PtzLockedMessageProperty = DependencyProperty.Register(
                            "PtzLockedMessage", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields

        /// <summary>
        /// Represent the object used to verify the capabilities of the cam (zoom, pan etc.) 
        /// </summary>
        private IPtzCapabilities m_cap;

        /// <summary>
        /// Represent the Ptz Camera Controller used to move the Camera
        /// </summary>
        private Genetec.Sdk.Media.Ptz.PtzCoordinatesManager m_pcm;

        /// <summary>
        /// Represent the selected Camera object that we wish to control
        /// </summary>
        private Genetec.Sdk.Entities.Camera m_selectedCam;

        #endregion

        #region Properties

        public ObservableCollection<Camera> CamerasItems { get; private set; }

        public int CamerasSelectedIndex
        {
            get => (int)GetValue(CamerasSelectedIndexProperty);
            set => SetValue(CamerasSelectedIndexProperty, value);
        }

        public bool IsPtzCameraSelected
        {
            get => (bool)GetValue(IsPtzCameraSelectedProperty);
            set => SetValue(IsPtzCameraSelectedProperty, value);
        }

        public bool IsSdkEngineConnected
        {
            get => (bool)GetValue(IsSdkEngineConnectedProperty);
            set => SetValue(IsSdkEngineConnectedProperty, value);
        }

        public ObservableCollection<ListBoxItem> ListboxMessageItems { get; private set; }

        public string PtzLockedMessage
        {
            get => (string)GetValue(PtzLockedMessageProperty);
            set => SetValue(PtzLockedMessageProperty, value);
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            m_sdkEngine = new Engine();

            m_selectedCam = null;
            m_cap = null;

            //these events are used to subscribe to login and logout events
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLogonSuccess;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggingOff += OnEngineLoggingOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

            m_player.HardwareAccelerationEnabled = true;

            ListboxMessageItems = new ObservableCollection<ListBoxItem>();
            CamerasItems = new ObservableCollection<Camera>();
            DisableMediaPlayer();

            DataContext = this;
        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            m_player.Dispose();
            Genetec.Sdk.Media.MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        ///  <summary>
        ///  Rotate the camera downwards
        /// </summary>
        private void OnBtnDownMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartPanTilt))
            {
                m_pcm.ControlPtz(PtzCommandType.StartPanTilt, 0, -50);
            }
        }

        ///  <summary>
        ///  Rotate the Camera to the left
        /// </summary>
        private void OnBtnLeftMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartPanTilt))
            {
                m_pcm.ControlPtz(PtzCommandType.StartPanTilt, -50, 0);
            }
        }

        ///  <summary>
        ///  This event is responsible for starting the pan on a camera that has pan capability
        /// using the button event PreviewMouseLeftButtonDown (where you need to hold down the button pressed)
        /// </summary>
        private void OnBtnRightMouseDown(object sender, MouseButtonEventArgs e)
        {
            //first we verify that the camera supports pan (horizontal and vertical rotation)
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartPanTilt))
            {
                //then we make sure to start the pan when the button is pressed down
                m_pcm.ControlPtz(PtzCommandType.StartPanTilt, 50, 0);
            }
        }

        ///  <summary>
        ///  This will stop the camera from zooming or panning by putting the speed at 0
        /// </summary>
        private void OnBtnStopMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (m_pcm != null && m_selectedCam != null)
            {
                //important to verify that the camera has a stop pan tilt capability
                if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StopPanTilt))
                {
                    //we stop the pan when the button is released
                    m_pcm.ControlPtz(PtzCommandType.StopPanTilt, 0, 0);
                }

                //we verify that the camera has a stop zoom capability
                if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StopZoom))
                {
                    //we stop the zoom when the button is released
                    m_pcm.ControlPtz(PtzCommandType.StopZoom, 0, 0);
                }
            }
        }

        ///  <summary>
        ///  Rotate the Camera Upward
        /// </summary>
        private void OnBtnUpMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartPanTilt))
            {
                m_pcm.ControlPtz(PtzCommandType.StartPanTilt, 0, 50);
            }
        }

        ///  <summary>
        ///  This method will control the camera by zooming in
        /// </summary>
        private void OnBtnZoomInMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartZoom))
            {
                m_pcm.ControlPtz(PtzCommandType.StartZoom, 0, 30);
            }
        }

        private void OnBtnZoomOutMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_cap != null && m_cap.IsSupportedCommand(PtzCommandType.StartZoom))
            {
                m_pcm.ControlPtz(PtzCommandType.StartZoom, 1, 30);
            }
        }

        ///  <summary>
        ///  This method will set the selected camera to the ComboBox selection
        /// </summary>
        private void OnCamerasSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsPtzCameraSelected = false;
            //If a camera was selected previously, make sure to dispose of the ptz coordinates manager correctly
            if (m_selectedCam != null)
            {
                //here we dispose of the ptz manager before we select another camera to control
                m_pcm.CoordinatesReceived -= OnPtzCoordinatesReceived;
                m_pcm.UnhandledException -= OnException;
                m_pcm.Dispose();
                m_cap = null;
                m_player.Stop();
            }

            //depending on the combobox selected, this will set the selected camera to the one selected from the combobox
            m_selectedCam = (CamerasSelectedIndex >= 0) ? CamerasItems[CamerasSelectedIndex] : null;

            //we make sure to retrieve it's capabilities
            if (m_selectedCam != null)
            {
                m_cap = m_selectedCam.PtzCapabilities;
                IsPtzCameraSelected = true;
            }

            //here we make sure to initialize the ptz coordinate manager for future use
            if (m_selectedCam != null && m_cap != null
                && (m_cap.IsSupportedCommand(PtzCommandType.StartPanTilt) || m_cap.IsSupportedCommand(PtzCommandType.StartZoom)))
            {
                m_pcm = new Genetec.Sdk.Media.Ptz.PtzCoordinatesManager();

                m_pcm.Initialize(m_sdkEngine, m_selectedCam.Guid);

                m_player.Initialize(m_sdkEngine, m_selectedCam.Guid);
                m_player.PlayLive();

                EnableMediaPlayer();

                m_pcm.CoordinatesReceived += OnPtzCoordinatesReceived;
                m_pcm.UnhandledException += OnException;
            }
            else // If the camera does not support pan/tilt or zoom we disable it since other PTZ controls like "presets" are not on the UI.
            {
                m_cap = null;
            }
        }

        private void OnException(object sender, UnhandledExceptionEventArgs e)
        {
            PostMessage($"Unhandled Exception: {e.ExceptionObject}");
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

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            PostMessage("SDK Logged Off");
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
        }

        private void OnEngineLoggingOff(object sender, LoggingOffEventArgs e)
        {
            PostMessage("SDK Logging Off");
        }

        ///  <summary>
        ///  Notify the user that the login failed
        /// </summary>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            PostMessage(e.FormattedErrorMessage);
        }

        ///  <summary>
        ///  Notify the user in case we loose the connection with the server
        /// </summary>
        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            PostMessage(string.Format("SDK Logon Status Changed: {0} on {1}\r\n", e.Status.ToString(), e.ServerName));
        }

        /// <summary>
        /// Called when logon has succeeded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="LoggedOnEventArgs"/> instance containing the event data.</param>        
        private void OnEngineLogonSuccess(object sender, LoggedOnEventArgs e)
        {
            //refresh the cameras and add them to a combobox
            RefreshPtzCameras();
            PostMessage("Login Successful!");
            statusMessage.Content = "Connected!";
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
        }

        /// <summary>
        /// Login to the server, retrieve all the ptz cameras and refresh the camera list for selection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            ListboxMessageItems.Clear();
            PostMessage("Logging in to " + textBoxServer.Text);
            try
            {
                m_sdkEngine.LoginManager.BeginLogOn(textBoxServer.Text, textBoxUsername.Text, passwordBoxPassword.Password);

            }
            catch (SdkException ex)
            {
                PostMessage(ex.Message);
            }
        }

        private void OnLogoutClick(object sender, RoutedEventArgs e)
        {
            //dispose of the object to release memory
            if (m_pcm != null)
            {
                m_pcm.Dispose();
                m_player.Stop();
            }

            //make sure to clear all the fields
            m_pcm = null;
            m_selectedCam = null;

            statusMessage.Content = "Disconnected";
            CamerasSelectedIndex = -1;
            CamerasItems.Clear();
            ListboxMessageItems.Clear();
            statusMessage.Content = "";

            DisableMediaPlayer();

            IsPtzCameraSelected = false;

            //log off from the sdk engine
            m_sdkEngine.LoginManager.LogOff();

        }

        ///  <summary>
        ///  An example of releasing memory
        /// </summary>
        private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //important to dispose of the PtzCoordinatesManager Correctly
            if (m_pcm != null)
                m_pcm.Dispose();

        }

        /// <summary>
        /// Update the displayed message about the user that is currently locking the PTZ camera.
        /// <remarks>This provides an example of how to use Multilevel PTZ on the SDK with the LockingUserMultilevelInfo field on the Camera entity.</remarks>
        /// </summary>
        private void OnPtzCoordinatesReceived(object sender, PtzCoordinatesEventArgs e)
        {
            if (m_selectedCam == null || !m_selectedCam.LockingUserMultilevelInfo.Any())
            {
                // No one is currently locking the camera
                PtzLockedMessage = string.Empty;
                return;
            }

            // Get the user from the current system in the LockingUserInfo list on the camera
            var lockingUserId = m_selectedCam.LockingUserMultilevelInfo.FirstOrDefault(info => info.IsUserFromCurrentSystem)?.UserId ?? Guid.Empty;
            var boLockingUser = m_sdkEngine.GetEntity<User>(lockingUserId);

            string lockingUserName;
            if (boLockingUser != null)
            {
                lockingUserName = boLockingUser.Name;
            }
            else
            {
                // If we can't get the user, display a generic message
                lockingUserName = m_selectedCam.FederationUserMultilevelInfo.Any() ? "Federation" : "another user";
            }

            var boLockingApp = m_sdkEngine.GetEntity(m_selectedCam.LockingApplicationId);

            PtzLockedMessage = boLockingApp != null ? $"Locked by {lockingUserName} on {boLockingApp.Name}" : $"Locked by {lockingUserName}";
        }

        #endregion

        #region Private Methods

        private void DisableMediaPlayer()
        {
            m_player.Background = new SolidColorBrush(Colors.Gray);
        }

        private void EnableMediaPlayer()
        {
            m_player.Background = new SolidColorBrush(Colors.Black);
        }

        private void PostMessage(string message)
        {
            ListboxMessageItems.Add(new ListBoxItem { Content = message });

            //allow the scroll bar to always be showing the last message
            listboxMessage.ScrollIntoView(ListboxMessageItems[ListboxMessageItems.Count - 1]);

        }

        ///  <summary>
        ///  Refresh the list of Ptz Cameras available on the server and adds it to a ComboBox
        /// </summary>
        private void RefreshPtzCameras()
        {
            var query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.Camera);

            QueryCompletedEventArgs result = query.Query();
            if (result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)
                {
                    Camera camera = m_sdkEngine.GetEntity((Guid)dr[0]) as Camera;
                    if ((camera != null) && (camera.IsPtz) && (camera.IsOnline) && (!camera.IsGhostCamera) && (!camera.IsSequence))
                    {
                        CamerasItems.Add(camera);
                    }
                }
            }
            else
            {
                PostMessage("The query has failed");
            }
        }

        #endregion
    }

    #endregion
}

