using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Media;
using Genetec.Sdk.Queries;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using File = System.IO.File;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MediaSdkSample
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
    public partial class MainWindow
    {
        #region Constants

        public static readonly DependencyProperty AdditionnalContentProperty = DependencyProperty.Register(
                                                            "AdditionnalContent", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        /// The Dependencies properties allow to use Front End data in the back end. Many objects in 
        /// the front ends have multiple dependencies. If we called all the *.Enabled in here, it 
        /// would be way bigger, more confusing and less maintainable. 
        public static readonly DependencyProperty ApplyZoomProperty = DependencyProperty.Register(
                                                            "ApplyZoom", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty BitRateProperty = DependencyProperty.Register(
                                                            "BitRate", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        // Property for the comboBox CameraGuids selected index.
        public static readonly DependencyProperty CameraGuidsSelectedIndexProperty = DependencyProperty.Register(
                                                            "CameraGuidsSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        // Property for setting the content for Connect Button.
        public static readonly DependencyProperty ConnectContentProperty = DependencyProperty.Register(
                                                            "ConnectContent", typeof(string), typeof(MainWindow), new PropertyMetadata("Connect"));

        // Property for the Playing mode. Type is the Enum type PlayingMode. Used for UI.
        public static readonly DependencyProperty CurrentPlayingModeProperty = DependencyProperty.Register(
                                                            "CurrentPlayingMode", typeof(PlayingMode), typeof(MainWindow), new PropertyMetadata(default(PlayingMode)));

        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register(
                                                            "Directory", typeof(string), typeof(MainWindow), new PropertyMetadata("localhost"));

        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register(
                                                            "FilePath", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty FrameRateProperty = DependencyProperty.Register(
                                                            "FrameRate", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty HeightDigitalZoomProperty = DependencyProperty.Register(
                                                            "HeightDigitalZoom", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public static readonly DependencyProperty IpSourceProperty = DependencyProperty.Register(
                                                            "IpSource", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        // Property for the Live playing. If the Media playing is playing a video in live, this will be true. Used for UI.
        public static readonly DependencyProperty IsLivePlayProperty = DependencyProperty.Register(
                                                            "IsLivePlay", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Property to know if the Media Player is currently playing. Used for UI.
        public static readonly DependencyProperty IsMediaStartedProperty = DependencyProperty.Register(
                                                            "IsMediaStarted", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        // Property for paused Media player. If it is paused, value will be true. Used for UI.
        public static readonly DependencyProperty IsPausedProperty = DependencyProperty.Register(
                                                            "IsPaused", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Property for the connection to the Sdk. Used for UI.
        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                                                            "IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        public static readonly DependencyProperty LastFrameReceivedProperty = DependencyProperty.Register(
                                                            "LastFrameReceived", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty LeftDigitalZoomProperty = DependencyProperty.Register(
                                                            "LeftDigitalZoom", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        // Property for the comboBox NetworkCard selected index.
        public static readonly DependencyProperty NetworkCardSelectedIndexProperty = DependencyProperty.Register(
                                                            "NetworkCardSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        // Property for the comboBox Network selected index.
        public static readonly DependencyProperty NetworkSelectedIndexProperty = DependencyProperty.Register(
                                                            "NetworkSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public static readonly DependencyProperty PlayerCurrentStateProperty = DependencyProperty.Register(
                                                            "PlayerCurrentState", typeof(PlayerState), typeof(MainWindow), new PropertyMetadata(PlayerState.NoVideoSequenceAvailable));

        // Property for the comboBox PlaySpeed selected index.
        public static readonly DependencyProperty PlaySpeedSelectedIndexProperty = DependencyProperty.Register(
                                                            "PlaySpeedSelectedIndex", typeof(int), typeof(MainWindow), new PropertyMetadata(DefaultPlaySpeedIndex));

        public static readonly DependencyProperty RtpPacketsLostProperty = DependencyProperty.Register(
                                                            "RtpPacketsLost", typeof(string), typeof(MainWindow), new PropertyMetadata("0 packets lost"));

        public static readonly DependencyProperty StartedSinceProperty = DependencyProperty.Register(
                                                            "StartedSince", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TopDigitalZoomProperty = DependencyProperty.Register(
                                                            "TopDigitalZoom", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public static readonly DependencyProperty UsernameProperty = DependencyProperty.Register(
                                                            "Username", typeof(string), typeof(MainWindow), new PropertyMetadata("admin"));

        public static readonly DependencyProperty VideoDimensionsProperty = DependencyProperty.Register(
                                                            "VideoDimensions", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty WidthDigitalZoomProperty = DependencyProperty.Register(
                                                            "WidthDigitalZoom", typeof(int), typeof(MainWindow), new PropertyMetadata(0));

        public static readonly DependencyProperty WindowsCredentialsProperty = DependencyProperty.Register(
                                                            "WindowsCredentials", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Property for Write date time. A Checkbox in Snapshot support. Used to get its value.
        public static readonly DependencyProperty WriteDateTimeProperty = DependencyProperty.Register(
                                                            "WriteDateTime", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        // Default value for the play speed is 1 which is at the index 4 in the Combobox on the UI.
        private const int DefaultPlaySpeedIndex = 4;

        private const string G64FileExtension = ".g64";

        private const string G64FilesFilter = "G64 files|*" + G64FileExtension;

        private const string G64XFileExtension = ".g64x";

        private const string G64XFilesFilter = "G64X files|*" + G64XFileExtension;

        private const string GenetecVideoFiles = "Genetec video files|*" + G64FileExtension + ";*" + G64XFileExtension;

        // The engine from Genetec Sdk.
        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields

        private OverlayType m_currentOverlayType;

        private IAsyncResult m_loggingOnResult;

        #endregion

        #region Properties

        public string AdditionnalContent
        {
            get { return (string)GetValue(AdditionnalContentProperty); }
            set { SetValue(AdditionnalContentProperty, value); }
        }

        public bool ApplyZoom
        {
            get { return (bool)GetValue(ApplyZoomProperty); }
            set { SetValue(ApplyZoomProperty, value); }
        }

        public string BitRate
        {
            get { return (string)GetValue(BitRateProperty); }
            set { SetValue(BitRateProperty, value); }
        }

        /// Collection for the items which CameraGuids combobox will display.
        public ObservableCollection<ComboBoxItem> CameraGuidsItems { get; private set; }

        public int CameraGuidsSelectedIndex
        {
            get { return (int)GetValue(CameraGuidsSelectedIndexProperty); }
            set { SetValue(CameraGuidsSelectedIndexProperty, value); }
        }

        public string ConnectContent
        {
            get { return (string)GetValue(ConnectContentProperty); }
            set { SetValue(ConnectContentProperty, value); }
        }

        public PlayingMode CurrentPlayingMode
        {
            get { return (PlayingMode)GetValue(CurrentPlayingModeProperty); }
            set { SetValue(CurrentPlayingModeProperty, value); }
        }

        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public string FrameRate
        {
            get { return (string)GetValue(FrameRateProperty); }
            set { SetValue(FrameRateProperty, value); }
        }

        public int HeightDigitalZoom
        {
            get { return (int)GetValue(HeightDigitalZoomProperty); }
            set { SetValue(HeightDigitalZoomProperty, value); }
        }

        public string IpSource
        {
            get { return (string)GetValue(IpSourceProperty); }
            set { SetValue(IpSourceProperty, value); }
        }

        public bool IsLivePlay
        {
            get { return (bool)GetValue(IsLivePlayProperty); }
            set { SetValue(IsLivePlayProperty, value); }
        }

        public bool IsMediaStarted
        {
            get { return (bool)GetValue(IsMediaStartedProperty); }
            set { SetValue(IsMediaStartedProperty, value); }
        }

        public bool IsPaused
        {
            get { return (bool)GetValue(IsPausedProperty); }
            set { SetValue(IsPausedProperty, value); }
        }

        public bool IsSdkEngineConnected
        {
            get { return (bool)GetValue(IsSdkEngineConnectedProperty); }
            set { SetValue(IsSdkEngineConnectedProperty, value); }
        }

        public string LastFrameReceived
        {
            get { return (string)GetValue(LastFrameReceivedProperty); }
            set { SetValue(LastFrameReceivedProperty, value); }
        }

        public int LeftDigitalZoom
        {
            get { return (int)GetValue(LeftDigitalZoomProperty); }
            set { SetValue(LeftDigitalZoomProperty, value); }
        }

        /// This is what the UI will bind to for NetworkCard combobox.
        public ObservableCollection<ComboBoxItem> NetworkCardItems { get; private set; }

        public int NetworkCardSelectedIndex
        {
            get { return (int)GetValue(NetworkCardSelectedIndexProperty); }
            set { SetValue(NetworkCardSelectedIndexProperty, value); }
        }

        /// This is what the UI will bind to for Network combobox.
        public ObservableCollection<ComboBoxItem> NetworkItems { get; private set; }

        public int NetworkSelectedIndex
        {
            get { return (int)GetValue(NetworkSelectedIndexProperty); }
            set { SetValue(NetworkSelectedIndexProperty, value); }
        }

        public PlayerState PlayerCurrentState
        {
            get { return (PlayerState)GetValue(PlayerCurrentStateProperty); }
            set { SetValue(PlayerCurrentStateProperty, value); }
        }

        public int PlaySpeedSelectedIndex
        {
            get { return (int)GetValue(PlaySpeedSelectedIndexProperty); }
            set { SetValue(PlaySpeedSelectedIndexProperty, value); }
        }

        public string RtpPacketsLost
        {
            get { return (string)GetValue(RtpPacketsLostProperty); }
            set { SetValue(RtpPacketsLostProperty, value); }
        }

        public string StartedSince
        {
            get { return (string)GetValue(StartedSinceProperty); }
            set { SetValue(StartedSinceProperty, value); }
        }

        public int TopDigitalZoom
        {
            get { return (int)GetValue(TopDigitalZoomProperty); }
            set { SetValue(TopDigitalZoomProperty, value); }
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public string VideoDimensions
        {
            get { return (string)GetValue(VideoDimensionsProperty); }
            set { SetValue(VideoDimensionsProperty, value); }
        }

        public int WidthDigitalZoom
        {
            get { return (int)GetValue(WidthDigitalZoomProperty); }
            set { SetValue(WidthDigitalZoomProperty, value); }
        }

        public bool WindowsCredentials
        {
            get { return (bool)GetValue(WindowsCredentialsProperty); }
            set { SetValue(WindowsCredentialsProperty, value); }
        }

        public bool WriteDateTime
        {
            get { return (bool)GetValue(WriteDateTimeProperty); }
            set { SetValue(WriteDateTimeProperty, value); }
        }

        #endregion

        #region Enumerations

        // Enum for the Playing mode which are possible.
        public enum PlayingMode
        {
            NotPlaying,
            Camera,
            Video
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the class MainWindow. It attaches all the event handlers nessary 
        /// for the engine to logon, logoff and etc to private methods from this class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            m_sdkEngine = new Engine();

            //Sdk engine events
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

            // Media player Events
            m_mediaPlayer.BitRateRefreshed += OnMediaPlayerBitRateRefreshed;
            m_mediaPlayer.FrameRateRefreshed += OnMediaPlayerFrameRateRefreshed;
            m_mediaPlayer.FrameRendered += OnMediaPlayerFrameRendered;
            m_mediaPlayer.IpSourceRefreshed += OnMediaPlayerIpSourceRefreshed;
            m_mediaPlayer.LivePlaybackModeToggled += OnMediaPlayerLivePlaybackModeToggled;
            m_mediaPlayer.PlayerStateChanged += OnMediaPlayerStateChanged;
            m_mediaPlayer.RtpPacketsLostRefreshed += OnMediaPlayerRtpPacketsLostRefreshed;
            m_mediaPlayer.Started += OnMediaPlayerStarted;
            m_mediaPlayer.UnhandledException += OnMediaPlayerUnhandledException;
            m_mediaPlayer.VideoDimensionRefreshed += OnMediaPlayerVideoDimensionRefreshed;

            m_mediaPlayer.HardwareAccelerationEnabled = true;

            //Initializing the Observable collections
            CameraGuidsItems = new ObservableCollection<ComboBoxItem>();
            NetworkCardItems = new ObservableCollection<ComboBoxItem>();
            NetworkItems = new ObservableCollection<ComboBoxItem>();

            DataContext = this;

        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            m_mediaPlayer.Dispose();
            MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        private void OnButtonClearDigitalZoomnClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.ClearViewWindow();
            WidthDigitalZoom = (int)(m_mediaPlayer.RenderingDimension.Width - 1);
            HeightDigitalZoom = (int)(m_mediaPlayer.RenderingDimension.Height - 1);
            LeftDigitalZoom = 0;
            TopDigitalZoom = 0;
        }

        private void OnButtonConnectClick(object sender, RoutedEventArgs e)
        {
            if (m_loggingOnResult != null)
            {
                ConnectContent = "Connect";
                m_sdkEngine.LoginManager.EndLogOn(m_loggingOnResult);
                m_loggingOnResult = null;
                return;
            }

            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                ConnectContent = "Connecting... Click to cancel";
                if (WindowsCredentials)
                {
                    m_loggingOnResult = m_sdkEngine.LoginManager.BeginLogOnUsingWindowsCredential(Directory);
                }
                else
                {
                    m_loggingOnResult = m_sdkEngine.LoginManager.BeginLogOn(Directory, Username, m_password.Password);
                }
            }
            else
            {
                m_sdkEngine.LoginManager.LogOff();
            }
        }

        private void OnButtonFileBrowseClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select the path to the video",
                Filter = string.Join("|", new[] { GenetecVideoFiles, G64FilesFilter, G64XFilesFilter })
            };

            if ((bool)fileDialog.ShowDialog(this))
            {
                FilePath = fileDialog.FileName;
            }
        }

        /// <summary>
        /// Starting video from a file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonFileStartClick(object sender, RoutedEventArgs e)
        {

            // Prevents crash from empty or not valid filename
            if (!File.Exists(FilePath))
            {
                MessageBox.Show("Invalid File Name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            m_mediaPlayer.OpenFile(FilePath);
            m_mediaPlayer.PlayFile();
            CurrentPlayingMode = PlayingMode.Video;
        }

        private void OnButtonFileStopClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.Stop();
            IsMediaStarted = false;
            CurrentPlayingMode = PlayingMode.NotPlaying;
            PlaySpeedSelectedIndex = DefaultPlaySpeedIndex;
        }

        /// <summary>
        /// Method to call the next frame in the MediaPlayer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonNextFrameClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.PlayNextFrame();
        }

        /// <summary>
        /// Pauses the Media player. If in Live mode, calling <see cref="OnButtonPauseClick"/>
        /// will not toggle the player from live to playback. It will stay in the buffer from the live mode.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonPauseClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.Pause();
        }

        private void OnButtonPlayClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.ResumePlaying();
        }

        private void OnButtonPreviousFrameClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.PlayPreviousFrame();
        }

        private void OnButtonRewindClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.Rewind();
            PlaySpeedSelectedIndex = DefaultPlaySpeedIndex;
        }

        private void OnButtonSeekBackOneMinClick(object sender, RoutedEventArgs e)
        {
            if (m_mediaPlayer.LastRenderedFrameTime > DateTime.MinValue)
            {
                m_mediaPlayer.Seek(m_mediaPlayer.LastRenderedFrameTime - TimeSpan.FromMinutes(1));
            }
        }

        private void OnButtonSetDigitalZoomClick(object sender, RoutedEventArgs e)
        {
            // Input correction : clamp between 0/1 and image dimension
            var maxLeft = (int)(m_mediaPlayer.RenderingDimension.Width - 1);
            var maxTop = (int)(m_mediaPlayer.RenderingDimension.Height - 1);

            LeftDigitalZoom = Math.Min(maxLeft, Math.Max(0, LeftDigitalZoom));
            TopDigitalZoom = Math.Min(maxTop, Math.Max(0, TopDigitalZoom));

            var maxWidth = (int)(m_mediaPlayer.RenderingDimension.Width - LeftDigitalZoom);
            var maxHeight = (int)(m_mediaPlayer.RenderingDimension.Height - TopDigitalZoom);

            WidthDigitalZoom = Math.Min(maxWidth, Math.Max(1, WidthDigitalZoom));
            HeightDigitalZoom = Math.Min(maxHeight, Math.Max(1, HeightDigitalZoom));

            m_mediaPlayer.SetViewWindow(LeftDigitalZoom, TopDigitalZoom, WidthDigitalZoom, HeightDigitalZoom);
        }

        private void OnButtonSnapshotClick(object sender, RoutedEventArgs e)
        {

            Bitmap image = m_mediaPlayer.SnapShot(ApplyZoom, WriteDateTime, AdditionnalContent);
            if (image == null)
            {
                return;
            }

            const string tempDirectory = @".\Temp";

            if (!System.IO.Directory.Exists(tempDirectory))
            {
                System.IO.Directory.CreateDirectory(tempDirectory);
            }

            const string imageFileName = @".\Temp\capturedImage.jpeg";
            image.Save(imageFileName, ImageFormat.Jpeg);

            using (Process p = new Process())
            {
                p.StartInfo.FileName = imageFileName;
                p.Start();
            }
        }

        private void OnButtonStartClick(object sender, RoutedEventArgs e)
        {
            ComboBoxItem cameraItem = CameraGuidsItems[CameraGuidsSelectedIndex];
            if (cameraItem == null)
            {
                return;
            }
            Guid cameraGuid = (Guid)(cameraItem).Tag;

            Camera camera = m_sdkEngine.GetEntity(cameraGuid) as Camera;

            if (camera != null && !camera.IsOnline)
            {
                MessageBox.Show("The selected camera is offline. Viewing its video feed is not possible.");
                return;
            }

            if (camera != null && (camera.Synchronised && camera.OwnerRoleType == RoleType.Omnicast))
            {
                MessageBox.Show("The selected camera comes from a federated Omnicast system. You may have difficulty viewing its video feed if the necessary CPack is not installed on your machine.");
            }

            Guid subnetGuid = (Guid)(NetworkItems[NetworkSelectedIndex]).Tag;
            PhysicalAddress cardPhysicalAddress = (PhysicalAddress)(NetworkCardItems[NetworkCardSelectedIndex]).Tag;
            m_mediaPlayer.Initialize(m_sdkEngine, cameraGuid, StreamingType.Live, subnetGuid, cardPhysicalAddress);

            m_mediaPlayer.PlayLive();

            CurrentPlayingMode = PlayingMode.Camera;
        }

        private void OnButtonStopClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.Stop();
            IsMediaStarted = false;
            CurrentPlayingMode = PlayingMode.NotPlaying;
            PlaySpeedSelectedIndex = DefaultPlaySpeedIndex;
        }

        private void OnButtonSwitchToLiveClick(object sender, RoutedEventArgs e)
        {
            m_mediaPlayer.PlayLive();
        }

        private void OnButtonToggleSpecialOverlaysClick(object sender, RoutedEventArgs e)
        {
            if (m_mediaPlayer != null && m_mediaPlayer.State == PlayerState.Playing)
            {
                switch (m_currentOverlayType)
                {
                    case OverlayType.None:
                        m_currentOverlayType = OverlayType.Statistics;
                        break;
                    case OverlayType.Statistics:
                        m_currentOverlayType = OverlayType.Status;
                        break;
                    case OverlayType.Status:
                        m_currentOverlayType = OverlayType.None;
                        break;
                    default:
                        m_currentOverlayType = OverlayType.None;
                        break;
                }

                m_mediaPlayer.ShowSpecialOverlay(m_currentOverlayType);
            }
        }

        private void OnComboboxPlaySpeedChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = e.AddedItems[0] as ComboBoxItem;
            if (item != null)
            {
                int enumValue = int.Parse(item.Tag.ToString());
                PlaySpeed playSpeed = (PlaySpeed)enumValue;
                m_mediaPlayer.PlaySpeed = playSpeed;
            }
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

        /// <summary>
        /// Event called when the engine is logged off.
        /// See default constructor from this class to see how to attach this method to the engine Event corresponding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_loggingOnResult = null;
                CameraGuidsItems.Clear();

                NetworkCardItems.Clear();
                NetworkItems.Clear();
                IsSdkEngineConnected = false;
                ConnectContent = "Connect";
            }));
        }

        /// <summary>
        /// Event called when Engine succesfully logged on.
        /// See default constructor from this class to see how to attach this method to the engine Event corresponding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(LogOnSuccess));
            IsSdkEngineConnected = true;
        }

        /// <summary>
        /// Event called when Engine failed to log on.
        /// See default constructor from this class to see how to attach this method to the engine Event corresponding.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_loggingOnResult = null;
                ConnectContent = "Connect";
                MessageBox.Show(e.FormattedErrorMessage);
            }));
        }

        private void OnMediaPlayerBitRateRefreshed(object sender, BitRateRefreshedEventArgs e)
        {
            BitRate = e.BitRate.ToString(e.BitRate < 100 ? "N1" : "N0") + " kbps";
        }

        private void OnMediaPlayerFrameRateRefreshed(object sender, FrameRateRefreshedEventArgs e)
        {
            FrameRate = e.FrameRate.ToString("N") + " fps";
        }

        private void OnMediaPlayerFrameRendered(object sender, FrameRenderedEventArgs e)
        {
            LastFrameReceived = e.FrameTime.ToLocalTime().ToString("HH:mm:ss.ffff");
        }

        private void OnMediaPlayerIpSourceRefreshed(object sender, IpSourceRefreshedEventArgs e)
        {
            IpSource = e.IpSource.Address + ":" + e.IpSource.Port;
        }

        /// <summary>
        /// <see cref="MediaPlayer.Pause"/> will not trigger the toggle. If the current mode is live and <see cref="OnButtonPauseClick"/>
        /// is called, it will pause in the buffer from the live. So if you are on live play, and you pause, you are still in live play.
        /// </summary>
        /// <param name="sender"><see cref="MediaPlayer"/></param>
        /// <param name="e"></param>
        private void OnMediaPlayerLivePlaybackModeToggled(object sender, EventArgs e)
        {
            IsLivePlay = ((MediaPlayer)sender).IsPlayingLiveStream;
        }

        private void OnMediaPlayerRtpPacketsLostRefreshed(object sender, RtpPacketsLostRefreshedEventArgs e)
        {
            RtpPacketsLost = e.RtpPacketsLost + " packets lost";
        }

        private void OnMediaPlayerStarted(object sender, EventArgs e)
        {
            IsMediaStarted = true;
            WidthDigitalZoom = (int)(m_mediaPlayer.RenderingDimension.Width - 1);
            HeightDigitalZoom = (int)(m_mediaPlayer.RenderingDimension.Height - 1);
            LeftDigitalZoom = 0;
            TopDigitalZoom = 0;
        }

        private void OnMediaPlayerStateChanged(object sender, PlayerStateChangedEventArgs e)
        {
            PlayerCurrentState = e.State;
            IsPaused = ((PlayerCurrentState == PlayerState.BeginReached) || (PlayerCurrentState == PlayerState.Paused) ||
                        (PlayerCurrentState == PlayerState.NoVideoSequenceAvailable));
        }

        private void OnMediaPlayerUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void OnMediaPlayerVideoDimensionRefreshed(object sender, VideoDimensionRefreshedEventArgs e)
        {
            VideoDimensions = e.Dimension.Width + " x " + e.Dimension.Height;
        }

        #endregion

        #region Private Methods

        private void LogOnSuccess()
        {
            m_loggingOnResult = null;
            ConnectContent = "Disconnect";

            //For the sake of simplicity, the following operations are done on the UI thread instead of 
            //doing the queries on a background thread and bringing the results back into the UI, 
            //hence in this sample de UI might freeze while the queries complete.
            PopulateCameraList();
            PopulateNetworkAdapterList();
            PopulateNetworkList();
        }

        private void PopulateCameraList()
        {
            CameraGuidsItems.Clear();
            CameraGuidsSelectedIndex = (int)CameraGuidsSelectedIndexProperty.DefaultMetadata.DefaultValue;
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Camera);
                query.BeginQuery(OnGetCameraQueryDone, query);                
            }
        }

        private void OnGetCameraQueryDone(IAsyncResult ar)
        {
            var query = ar.AsyncState as EntityConfigurationQuery;
            var results = query.EndQuery(ar);
            if(results != null)
            {
                foreach (DataRow row in results.Data.Rows)
                {
                    Camera cam = (Camera)m_sdkEngine.GetEntity((Guid)row[0]);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        CameraGuidsItems.Add(new ComboBoxItem
                        {
                            Content = cam.Name,
                            Tag = cam.Guid
                        });
                    }));
                }

            }
        }

        private void PopulateNetworkAdapterList()
        {
            //Clear the comboBox Item list.
            NetworkCardItems.Clear();
            NetworkCardSelectedIndex = (int)NetworkCardSelectedIndexProperty.DefaultMetadata.DefaultValue;

            //Iterate in all the NetworkInterfaces and add them to the comboBox.
            foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces().Where(i =>
                i.OperationalStatus == OperationalStatus.Up && i.NetworkInterfaceType != NetworkInterfaceType.Loopback && !string.IsNullOrEmpty(i.Name)))
            {
                NetworkCardItems.Add(new ComboBoxItem
                {
                    Content = networkInterface.Name,
                    Tag = networkInterface.GetPhysicalAddress()
                });
            }
        }

        private void PopulateNetworkList()
        {

            //Clear the comboBox Item list.
            NetworkItems.Clear();
            NetworkSelectedIndex = (int)NetworkSelectedIndexProperty.DefaultMetadata.DefaultValue;

            NetworkItems.Add(new ComboBoxItem { Content = "<Not specified>", Tag = Guid.Empty });
            EntityConfigurationQuery query = (EntityConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            if(query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Network);
                query.BeginQuery(OnGetNetworkQueryDone, query);
            }

        }

        private void OnGetNetworkQueryDone(IAsyncResult ar)
        {
            var query = ar.AsyncState as EntityConfigurationQuery;
            var results = query.EndQuery(ar);
            if(results != null)
            {
                foreach (DataRow row in results.Data.Rows)
                {
                    Network network = (Network)m_sdkEngine.GetEntity((Guid)row[0]);

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        NetworkItems.Add(new ComboBoxItem
                        {
                            Content = network.Name,
                            Tag = network.Guid
                        });
                        //If it is the default Network, we set it selected.
                        if (network.IsDefaultNetwork)
                        {
                            NetworkSelectedIndex = NetworkItems.Count() - 1;
                        }
                    }));
                }

            }
        }

        #endregion
    }

    #endregion
}

