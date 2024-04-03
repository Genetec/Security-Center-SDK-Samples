using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Media;
using MediaPlayer = Genetec.Sdk.Media.MediaPlayer;
using Genetec.Sdk.Media.Overlay;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using System.Windows.Threading;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace OverlaySample
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

        public static readonly DependencyProperty CurrentVideoTimeProperty =
                                    DependencyProperty.Register("CurrentVideoTime", typeof(string), typeof(MainWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty IsNotLoggedOnProperty =
                                    DependencyProperty.Register("IsNotLoggedOn", typeof(bool), typeof(MainWindow), new PropertyMetadata(true));

        public static readonly DependencyProperty NotLoggedOnTextProperty =
                                    DependencyProperty.Register("NotLoggedOnText", typeof(string), typeof(MainWindow), new PropertyMetadata("Not logged on"));

        public static readonly DependencyProperty SelectedCameraIdProperty =
                                    DependencyProperty.Register("SelectedCameraId", typeof(Guid), typeof(MainWindow), new PropertyMetadata(Guid.Empty, OnSelectedCameraChanged));

        private const string AppTitle = "Overlay";

        private readonly ObservableCollection<CameraModel> m_cameras = new ObservableCollection<CameraModel>();

        private readonly MediaPlayer m_mediaPlayer = new MediaPlayer();

        private readonly OverlayManager m_overlayManager;

        private readonly List<Guid> m_pendingSubscribeMetadataStreams = new List<Guid>();

        private readonly ObservableCollection<MetadataStreamModel> m_streams = new ObservableCollection<MetadataStreamModel>();

        private readonly DispatcherTimer m_timeGeneratorTimer;

        #endregion

        #region Fields

        private MetadataStreamModel m_editedStream;

        private bool m_isSelectedCameraRunning;

        private DateTime m_lastDrawTime;

        private Window m_loginWindow;

        #endregion

        #region Properties

        public ObservableCollection<CameraModel> Cameras { get { return m_cameras; } }

        public string CurrentVideoTime
        {
            get { return (string)GetValue(CurrentVideoTimeProperty); }
            set { SetValue(CurrentVideoTimeProperty, value); }
        }

        public bool IsNotLoggedOn
        {
            get { return (bool)GetValue(IsNotLoggedOnProperty); }
            set { SetValue(IsNotLoggedOnProperty, value); }
        }

        public string NotLoggedOnText
        {
            get { return (string)GetValue(NotLoggedOnTextProperty); }
            set { SetValue(NotLoggedOnTextProperty, value); }
        }

        public Guid SelectedCameraId
        {
            get { return (Guid)GetValue(SelectedCameraIdProperty); }
            set { SetValue(SelectedCameraIdProperty, value); }
        }

        public ObservableCollection<MetadataStreamModel> Streams { get { return m_streams; } }

        #endregion

        #region Constructors

        public MainWindow()
        {
            DpiScale dpiScale = VisualTreeHelper.GetDpi(this);
            double pixelsPerDip = dpiScale.PixelsPerDip;

            m_cameras.Add(new CameraModel { Id = Guid.Empty, EntityName = "None" });
            m_overlayManager = new OverlayManager(App.Current.Sdk, pixelsPerDip);
            InitializeComponent();
            m_mediaPlayer.HardwareAccelerationEnabled = true;
            m_mediaPlayerContainer.Content = m_mediaPlayer;

            m_timeGeneratorTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            m_timeGeneratorTimer.Tick += OnTimeGeneratorTimerTick;
        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            m_mediaPlayer.Dispose();
            MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        private static void OnSelectedCameraChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((MainWindow)sender).OnSelectedCameraChanged();
        }

        private void OnButtonNewStreamClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCameraId != Guid.Empty)
            {
                if (m_streams.Any(s => s.EntityName == m_newStreamNameTextbox.Text))
                    return; // We already have a stream on this camera with this name
                m_overlayManager.CreateNewStream(SelectedCameraId, m_newStreamNameTextbox.Text);
            }
        }

        private void OnEditStreamChanged(object sender, RoutedEventArgs e)
        {
            if (m_editedStream != null)
            {
                m_editedStream.IsEditing = false;
            }

            var stream = (MetadataStreamModel)((FrameworkElement)e.OriginalSource).DataContext;

            if (stream.IsEditing)
            {
                m_editedStream = stream;
                stream.IsShowingTime = false;
                // Create overlays
                m_overlayManager.CreateEditLayers(SelectedCameraId, stream);
            }
            else
            {
                // Dispose overlays
                m_overlayManager.DisposeEditLayers(stream);
                m_editedStream = null;
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

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            NotLoggedOnText = "Not logged on";
            Title = AppTitle + " [Not Logged On]";
            IsNotLoggedOn = true;
            foreach (CameraModel cam in m_cameras.Where(c => c.Id != Guid.Empty).ToList()) // keep None
            {
                m_cameras.Remove(cam);
            }
            SelectedCameraId = Guid.Empty;
            m_streams.Clear();

            LoginNow();
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            //With version 5.5 and above, this must be initialised after the login to use overlays.
            OverlayFactory.Initialize(App.Current.Sdk);

            NotLoggedOnText = null;
            IsNotLoggedOn = false;

            if (m_loginWindow != null)
                m_loginWindow.Close();


            m_mediaPlayer.PreInitialize(App.Current.Sdk, StreamingType.Live);
            m_mediaPlayer.VideoDimensionRefreshed += OnMediaPlayerDimensionsChanged;
            m_mediaPlayer.PlayerStateChanged += OnMediaPlayerStateChanged;
            m_mediaPlayer.FrameRendered += OnFrameRendered;

            RefreshCameraList();
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            NotLoggedOnText = "Logon failed : " + e.FailureCode;
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Title = AppTitle + " [" + e.Status + " @ " + e.ServerName + "]";
            if (e.Status != ConnectionStateCode.Success)
            {
                NotLoggedOnText = e.Status.ToString();
                IsNotLoggedOn = true;
            }
        }

        private void OnEntitiesAdded(object sender, EntitiesAddedEventArgs e)
        {
            if (e.Entities.Any(entityUpdateInfo => entityUpdateInfo.EntityType == EntityType.Camera))
            {
                Dispatcher.BeginInvoke(new Action(RefreshCameraList));
            }
        }

        private void OnEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            List<EntityUpdateInfo> entitiesUpdateInfo = e.Entities.Where(entityUpdateInfo => entityUpdateInfo.EntityType == EntityType.Camera).ToList();

            if (entitiesUpdateInfo.Count == 0)
                return;

            Dispatcher.Invoke(() =>
            {
                foreach (EntityUpdateInfo entityUpdateInfo in entitiesUpdateInfo)
                {
                    RefreshCamera(entityUpdateInfo.EntityGuid);

                    if (entityUpdateInfo.EntityGuid == SelectedCameraId)
                    {
                        if (!m_isSelectedCameraRunning)
                        {
                            OnSelectedCameraChanged();
                        }

                        RefreshStreams();
                    }
                }
            });
        }

        private void OnEntitiesRemoved(object sender, EntitiesRemovedEventArgs e)
        {
            List<EntityUpdateInfo> entitiesUpdateInfo = e.Entities.Where(entityUpdateInfo => entityUpdateInfo.EntityType == EntityType.Stream || entityUpdateInfo.EntityType == EntityType.Camera).ToList();

            if (entitiesUpdateInfo.Count == 0)
                return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (EntityUpdateInfo entityUpdateInfo in entitiesUpdateInfo)
                {
                    if (entityUpdateInfo.EntityType == EntityType.Stream)
                    {
                        m_streams.Remove(m_streams.FirstOrDefault(s => s.Id == entityUpdateInfo.EntityGuid));
                    }
                    else
                    {
                        m_cameras.Remove(m_cameras.FirstOrDefault(c => c.Id == entityUpdateInfo.EntityGuid));
                    }
                }
            }));
            
        }

        private void OnFrameRendered(object sender, FrameRenderedEventArgs e)
        {
            CurrentVideoTime = e.FrameTime.ToLocalTime().ToString("HH:mm:ss.ffff");
        }

        private void OnMediaPlayerDimensionsChanged(object sender, VideoDimensionRefreshedEventArgs e)
        {
            m_drawingSurface.Height = e.RenderingDimension.Height;
            m_drawingSurface.Width = e.RenderingDimension.Width;
        }

        private void OnMediaPlayerStateChanged(object sender, PlayerStateChangedEventArgs e)
        {
            if (e.State == PlayerState.Playing)
            {
                // Register any pending metadata stream
                foreach (var id in m_pendingSubscribeMetadataStreams)
                    m_mediaPlayer.SubscribeMetadataStream(id);
                m_pendingSubscribeMetadataStreams.Clear();
            }
        }

        private void OnMouseMoveOnDrawingSurface(object sender, MouseEventArgs e)
        {
            if (m_editedStream != null && e.LeftButton == MouseButtonState.Pressed && (DateTime.Now - m_lastDrawTime).TotalMilliseconds > 50)
            {
                var absolutePosition = e.GetPosition(m_drawingSurface);
                var scaledPosition = new Point(absolutePosition.X / m_drawingSurface.ActualWidth * OverlayManager.DrawingWidth,
                    absolutePosition.Y / m_drawingSurface.ActualHeight * OverlayManager.DrawingHeight);

                m_overlayManager.DrawPoint(m_editedStream, scaledPosition);

                m_lastDrawTime = DateTime.Now;
            }
        }

        private void OnSelectedCameraChanged()
        {
            // First, stop everything we could be running on the old camera
            m_mediaPlayer.Stop();

            foreach (MetadataStreamModel stream in m_streams)
            {
                stream.IsShowingTime = false;
                stream.IsEditing = false;
            }

            m_editedStream = null;
            m_streams.Clear();

            if (!App.Current.Sdk.LoginManager.IsConnected)
                return;

            if (SelectedCameraId != Guid.Empty)
            {
                var camId = SelectedCameraId;

                Camera cam = (Camera)App.Current.Sdk.GetEntity(SelectedCameraId);

                m_isSelectedCameraRunning = cam.RunningState == State.Running;
                if (m_isSelectedCameraRunning)
                {
                    m_mediaPlayer.Initialize(camId);
                    m_mediaPlayer.PlayLive();
                }

                RefreshStreams();
            }
        }

        private void OnShowTimeStreamChanged(object sender, RoutedEventArgs e)
        {
            var stream = (MetadataStreamModel)((FrameworkElement)e.OriginalSource).DataContext;

            if (stream.IsShowingTime)
            {
                stream.IsEditing = false;
                m_timeGeneratorTimer.Start();
                m_overlayManager.CreateTimeLayers(SelectedCameraId, stream);
            }
            else
            {
                // Dispose overlays
                m_overlayManager.DisposeTimeLayers(stream);
                if (!m_streams.Any(s => s.IsShowingTime))
                {
                    m_timeGeneratorTimer.Stop();
                }
            }
        }

        private void OnTimeGeneratorTimerTick(object sender, EventArgs e)
        {
            foreach (MetadataStreamModel stream in m_streams.Where(s => s.IsShowingTime))
            {
                m_overlayManager.UpdateTime(stream);
            }
        }

        private void OnViewStreamChanged(object sender, RoutedEventArgs e)
        {
            if (m_mediaPlayer == null)
                return;

            var stream = (MetadataStreamModel)((FrameworkElement)e.OriginalSource).DataContext;
            if (stream.IsViewing)
            {
                if (m_mediaPlayer.State == PlayerState.Playing)
                    m_mediaPlayer.SubscribeMetadataStream(stream.Id);
                else
                    m_pendingSubscribeMetadataStreams.Add(stream.Id);
            }
            else
            {
                if (m_pendingSubscribeMetadataStreams.Contains(stream.Id))
                    m_pendingSubscribeMetadataStreams.Remove(stream.Id);
                else
                    m_mediaPlayer.UnsubscribeMetadataStream(stream.Id);
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            App.Current.Sdk.LoginManager.LoggedOn += OnEngineLoggedOn;
            App.Current.Sdk.EntitiesInvalidated += OnEntitiesInvalidated;
            App.Current.Sdk.EntitiesRemoved += OnEntitiesRemoved;
            App.Current.Sdk.EntitiesAdded += OnEntitiesAdded;
            App.Current.Sdk.LoginManager.LogonFailed += OnEngineLogonFailed;
            App.Current.Sdk.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            App.Current.Sdk.LoginManager.LoggedOff += OnEngineLoggedOff;
            App.Current.Sdk.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
            LoginNow();
        }

        #endregion

        #region Private Methods

        private void LoginNow()
        {
            m_loginWindow = new LoginWindow { Owner = this };
            bool userAborted = m_loginWindow.ShowDialog() != true && !App.Current.Sdk.LoginManager.IsConnected;

            if (userAborted)
            {
                Close();
            }
            else
            {
                Focus();
            }
        }

        private void RefreshCamera(Guid camId)
        {
            Camera cam = (Camera)App.Current.Sdk.GetEntity(camId);

            CameraModel model = m_cameras.FirstOrDefault(c => c.Id == camId);
            if (model == null)
            {
                m_cameras.Add(model = new CameraModel());
            }

            model.EntityName = cam.Name;
            model.Id = cam.Guid;
            model.CurrentIcon = cam.GetIcon(true);
        }

        private void RefreshCameraList()
        {
            var query = (EntityConfigurationQuery)App.Current.Sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            query.EntityTypeFilter.Add(EntityType.Camera);
            QueryCompletedEventArgs results = query.Query();

            if (results.Data != null)
            {
                List<Guid> camGuids = results.Data.Rows.Cast<DataRow>().Select(row => (Guid)row[0]).ToList();
                for (int i = 0; i < m_cameras.Count; i++)
                {
                    if (!camGuids.Contains(m_cameras[i].Id) && m_cameras[i].Id != Guid.Empty)
                    {
                        m_cameras.RemoveAt(i--);
                    }
                }

                foreach (Guid camId in camGuids)
                {
                    RefreshCamera(camId);
                }
            }
        }

        private void RefreshStreams()
        {
            ReadOnlyCollection<MetadataStreamInfo> metadataStreams = m_overlayManager.GetEntityMetadataStreams(SelectedCameraId);

            // Remove all streams not present on the camera anymore
            foreach (MetadataStreamModel stream in m_streams.Where(s => metadataStreams.All(ms => ms.Name != s.EntityName)).ToList())
            {
                m_streams.Remove(stream);
                m_overlayManager.DisposeOverlay(stream);
            }

            // Add all camera metadata stream not present in the list
            foreach (MetadataStreamInfo streamInfo in metadataStreams.Where(ms => m_streams.All(s => s.EntityName != ms.Name)))
            {
                MetadataStreamModel streamObject = new MetadataStreamModel
                {
                    Id = streamInfo.StreamId,
                    EntityName = streamInfo.Name
                };

                m_streams.Add(streamObject);
            }
        }

        #endregion
    }

    #endregion
}

