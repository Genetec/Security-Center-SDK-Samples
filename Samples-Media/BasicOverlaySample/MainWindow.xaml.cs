using Genetec.Sdk;
using Genetec.Sdk.Entities;
using MediaPlayer = Genetec.Sdk.Media.MediaPlayer;
using Genetec.Sdk.Media.Overlay;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 14:
//  1808 – Finnish War: Russians defeat the Swedes at the Battle of Oravais.
//  1917 – Russia is officially proclaimed a republic.
//  2000 – Microsoft releases Windows ME.
// ==========================================================================
namespace BasicOverlaySample
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private const int DrawingHeight = 150;

        private const int DrawingWidth = 200;

        private readonly MediaPlayer m_mediaPlayer = new MediaPlayer();

        private readonly Engine m_sdkEngine = new Engine();

        private readonly BackgroundWorker m_workerThread = new BackgroundWorker();

        private const string OverlayName = "BasicOverlay";

        #endregion

        #region Fields

        private Layer m_benchmarkLayer;

        private Layer m_ellipseLayer;

        private Layer m_imageLayer;

        private LoginWindow m_loginWindow;

        private Overlay m_overlay;

        private Layer m_rectangleLayer;

        private CameraModel m_selectedCameraModel;

        private Layer m_textLayer;

        #endregion

        #region Properties

        public ObservableCollection<CameraModel> Cameras { get; set; }

        public CameraModel SelectedCamera
        {
            get
            {
                return m_selectedCameraModel;
            }
            set
            {
                if (m_selectedCameraModel != null && m_selectedCameraModel.CameraGuid == value.CameraGuid) return;
                m_selectedCameraModel = value;
                OnSelectedCameraChanged();
            }
        }

        private Guid SelectedCameraGuid => SelectedCamera?.CameraGuid ?? Guid.Empty;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Cameras = new ObservableCollection<CameraModel>();

            // Initialize the thread used for benchmarking
            m_workerThread.WorkerSupportsCancellation = true;
            m_workerThread.WorkerReportsProgress = true;
            m_workerThread.DoWork += OnWorkerThread_DoWork;
            m_workerThread.ProgressChanged += OnWorkerThread_ProgressChanged;
            m_workerThread.RunWorkerCompleted += OnWorkerThread_RunWorkerCompleted;

            m_mediaPlayer.HardwareAccelerationEnabled = true;
            m_mediaPlayerContainer.Content = m_mediaPlayer;
            m_sdkEngine.LoginManager.LoggedOn += OnSdkengine_LoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnSdkengine_LogonFailed;
        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            DisposeOverlay();
            m_mediaPlayer.Dispose();
            MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        private void OnBenchmarking_StartButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!m_workerThread.IsBusy)
            {
                m_benchmarking.Lock();
                var benchmarkValues = new Benchmarking.BenchmarkParameters
                {
                    PositionX = m_benchmarking.PositionX,
                    PositionY = m_benchmarking.PositionY,
                    DeltaX = m_benchmarking.DeltaX,
                    DeltaY = m_benchmarking.DeltaY,
                    SizeW = m_benchmarking.SizeW,
                    SizeH = m_benchmarking.SizeH,
                    Duration = m_benchmarking.Duration,
                    Frequency = m_benchmarking.Frequency
                };

                // Starts the BackgroundWorker for benchmarking and fires the DoWork event.
                m_workerThread.RunWorkerAsync(benchmarkValues);
            }
        }

        private void OnBenchmarking_StopButtonClicked(object sender, RoutedEventArgs e)
        {
            m_workerThread.CancelAsync();
        }

        private void OnButtonClearAllClick(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void OnButtonClearEllipseClick(object sender, RoutedEventArgs e)
        {
            ClearEllipses();
        }

        private void OnButtonClearImageClick(object sender, RoutedEventArgs e)
        {
            ClearImages();
        }

        private void OnButtonClearRectangleClick(object sender, RoutedEventArgs e)
        {
            ClearRectangles();
        }

        private void OnButtonClearTextClick(object sender, RoutedEventArgs e)
        {
            ClearText();
        }

        private void OnButtonDrawEllipseClick(object sender, RoutedEventArgs e)
        {
            DrawEllipse();
        }

        private void OnButtonDrawImageClick(object sender, RoutedEventArgs e)
        {
            DrawImage();
        }

        private void OnButtonDrawRectangleClick(object sender, RoutedEventArgs e)
        {
            DrawRectangle();
        }

        private void OnButtonDrawTextClick(object sender, RoutedEventArgs e)
        {
            DrawText();
        }

        private void OnButtonRestartOverlayClick(object sender, RoutedEventArgs e)
        {
            InitializeOverlay();
        }

        private void OnBatchDestroyOverlaysClick(object sender, RoutedEventArgs e)
        {
            DisposeOverlay();
            BatchRemoveAllOverlays();
        }

        private void OnDestroyOverlaysClick(object sender, RoutedEventArgs e)
        {
            DisposeOverlay();
            RemoveAllOverlays();
        }

        private void OnSdkengine_LoggedOn(object sender, LoggedOnEventArgs e)
        {
            //With version 5.5 and above, this must be initialized after the login to use overlays.
            OverlayFactory.Initialize(m_sdkEngine);

            m_mediaPlayer.PreInitialize(m_sdkEngine, StreamingType.Live);

            RefreshCameraList();
        }

        private void OnSdkengine_LogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show("Logon Failed. Error code: " + e.FailureCode);
        }

        private void OnSelectedCameraChanged()
        {
            if (SelectedCamera != null)
            {
                m_mediaPlayer.Stop();
                var camId = SelectedCamera.CameraGuid;
                var cam = (Camera)m_sdkEngine.GetEntity(SelectedCamera.CameraGuid);
                if (cam?.RunningState == State.Running)
                {
                    // Display the media player
                    m_mediaPlayer.Initialize(m_sdkEngine, camId);
                    m_mediaPlayer.PlayLive();
                }
                InitializeOverlay();
            }
        }

        private void OnWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoginNow();
        }

        /// <summary>
        /// Event Handler for the BackgroundWorker's DoWork event. This contains all of the tasks that the BackgroundWorker
        /// must perform for benchmarking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWorkerThread_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            Benchmarking.BenchmarkParameters parameters = (Benchmarking.BenchmarkParameters)e.Argument;

            DateTime startTime = DateTime.Now;
            DateTime previousTime = DateTime.Now;
            do
            {
                // Check to see if CancelAsync() has been called on  this BackgroundWorker
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                // Do not draw again until the specified interval has passed
                if ((DateTime.Now - previousTime).TotalSeconds < parameters.Frequency)
                    continue;

                previousTime = DateTime.Now;
                m_benchmarkLayer.DrawEllipse("Red", "Red", parameters.PositionX, parameters.PositionY, parameters.SizeW, parameters.SizeH);
                m_benchmarkLayer.Update();

                parameters.PositionX = (parameters.PositionX + parameters.DeltaX) % DrawingWidth;
                parameters.PositionY = (parameters.PositionY + parameters.DeltaY) % DrawingHeight;

                worker.ReportProgress(Convert.ToInt32(1.0 + 1.0 * (DateTime.Now - startTime).TotalSeconds / parameters.Duration * 100.0));

            } while ((DateTime.Now - startTime).TotalSeconds < parameters.Duration);

            worker.ReportProgress(Convert.ToInt32(1.0 + 1.0 * (DateTime.Now - startTime).TotalSeconds / parameters.Duration * 100.0));
            m_benchmarkLayer.Clear();
        }

        private void OnWorkerThread_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            m_benchmarking.ReportProgress(e.ProgressPercentage);
        }

        private void OnWorkerThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            m_benchmarking.Unlock();
        }

        #endregion

        #region Private Methods

        private void ClearAll()
        {
            ClearText();
            ClearImages();
            ClearEllipses();
            ClearRectangles();
            ClearBenchmark();
        }

        private void ClearEllipses()
        {
            m_ellipseLayer.Clear();
            m_ellipseLayer.Update();
        }

        private void ClearImages()
        {
            m_imageLayer.Clear();
            m_imageLayer.Update();
        }

        private void ClearRectangles()
        {
            m_rectangleLayer.Clear();
            m_rectangleLayer.Update();
        }

        private void ClearText()
        {
            m_textLayer.Clear();
            m_textLayer.Update();
        }

        private void ClearBenchmark()
        {
            m_benchmarkLayer.Clear();
            m_benchmarkLayer.Update();
        }

        /// <summary>
        /// Dispose all Overlay streams and layers for the currently selected camera.
        /// </summary>
        private void DisposeOverlay()
        {
            if (m_overlay != null)
            {
                m_textLayer.Dispose();
                m_imageLayer.Dispose();
                m_ellipseLayer.Dispose();
                m_rectangleLayer.Dispose();
                m_benchmarkLayer.Dispose();

                m_overlay.Dispose();
                m_overlay = null;
            }
        }

        private void BatchRemoveAllOverlays()
        {
            var cameras = m_sdkEngine.GetEntities(EntityType.Camera).ToList();
            var removeCamera = new List<Guid>();

            foreach (var c in cameras)
            {
                if (((Camera)c).MetadataStreams.Any(s => s.Name.Equals(OverlayName)))
                    removeCamera.Add(((Camera)c).Guid);
            }

            if (removeCamera.Any())
            {
                OverlayFactory.Remove(removeCamera, OverlayName, true);
            }
        }

        private void RemoveAllOverlays()
        {
            var camera = m_sdkEngine.GetEntity(SelectedCameraGuid) as Camera;
            if (camera != null)
            {
                foreach (MetadataStreamInfo stream in camera.MetadataStreams)
                {
                    try
                    {
                        OverlayFactory.Remove(camera, stream.Name, true);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"Could not remove overlay stream {stream.Name} ({stream.StreamId}): {ex}");
                    }
                }
            }
        }

        private void DrawEllipse()
        {
            // Set up the ellipse parameters
            const double xRadius = 12;
            const double yRadius = 6;
            var random = new Random();
            var randomColor = Color.FromRgb((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
            var randomPoint = new Point(random.Next(DrawingWidth), random.Next(DrawingHeight));
            var pen = new Pen(Brushes.Transparent, 0);
            Brush brush = new SolidColorBrush(randomColor);

            // Must freeze animatable WPF objects
            brush.Freeze();
            pen.Freeze();

            // Draw the ellipse and update the layer
            m_ellipseLayer.DrawEllipse(brush, pen, randomPoint, xRadius, yRadius);
            m_ellipseLayer.Update();
        }

        private void DrawImage()
        {
            var random = new Random();
            var randomPoint = new Point(random.Next(DrawingWidth), random.Next(DrawingHeight));

            // Get the bmp
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new Uri("pack://application:,,,/Resources/GenetecLogo.bmp");
            bmp.EndInit();

            // Draw the image and update the layer
            m_imageLayer.DrawImage(bmp, randomPoint.X, randomPoint.Y, bmp.Width / 3, bmp.Height / 3);
            m_imageLayer.Update();
        }

        private void DrawRectangle()
        {
            // Set up the Rect parameters
            const double rectWidth = 25;
            const double rectHeight = 12;
            var rand = new Random();
            var randomColor = Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255));
            var randomPoint = new Point(rand.Next(DrawingWidth), rand.Next(DrawingHeight));
            var rect = new Rect(randomPoint, new Size(rectWidth, rectHeight));
            var pen = new Pen(Brushes.Transparent, 0);
            Brush brush = new SolidColorBrush(randomColor);

            // Must freeze animatable WPF objects
            pen.Freeze();
            brush.Freeze();

            // Draw the ellipse and update the layer
            m_rectangleLayer.DrawRectangle(brush, pen, rect);
            m_rectangleLayer.Update();
        }

        private void DrawText()
        {
            var random = new Random();
            var randomPoint = new Point(random.Next(DrawingWidth), random.Next(DrawingHeight));

            // Draw text by using FormattedText
            var formattedText = new FormattedText(
                "This is overlay text",
                CultureInfo.GetCultureInfo("en-CA"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                7,
                Brushes.White,
                VisualTreeHelper.GetDpi(this).PixelsPerDip);
            m_textLayer.DrawText(formattedText, randomPoint);

            // Draw text by passing in all the individual arguments
            m_textLayer.DrawText("This is also overlay text", "Segoe UI", 6, "Red", random.Next(DrawingWidth), random.Next(DrawingHeight));
            m_textLayer.Update();
        }

        /// <summary>
        /// Start an overlay stream for the selected camera, and creates some layers for drawing.
        /// </summary>
        private void InitializeOverlay()
        {
            DisposeOverlay();

            if (SelectedCameraGuid == Guid.Empty)
                return;

            // Gets (or creates if it does not exist) the overlay for the camera, then initializes it
            m_overlay = OverlayFactory.Get(SelectedCameraGuid, OverlayName);
            m_overlay.Initialize(DrawingHeight, DrawingWidth);

            // Initialize the layers for this overlay
            // Layers should have fixed guid, so the same layers will be cleared the next time the BasicOverlaySample starts.
            m_textLayer = m_overlay.CreateLayer(Guid.Parse($"{"Text".GetHashCode():x8}{0:x24}"), "SampleTextLayer");
            m_imageLayer = m_overlay.CreateLayer(Guid.Parse($"{"Image".GetHashCode():x8}{0:x24}"), "SampleImageLayer");
            m_ellipseLayer = m_overlay.CreateLayer(Guid.Parse($"{"Ellipse".GetHashCode():x8}{0:x24}"), "SampleEllipseLayer");
            m_rectangleLayer = m_overlay.CreateLayer(Guid.Parse($"{"Rectangle".GetHashCode():x8}{0:x24}"), "SampleRectangleLayer");
            m_benchmarkLayer = m_overlay.CreateLayer(Guid.Parse($"{"Benchmark".GetHashCode():x8}{0:x24}"), "SampleBenchmarkLayer");
        }

        private void LoginNow()
        {
            m_loginWindow = new LoginWindow(m_sdkEngine) { Owner = this };
            bool userAborted = m_loginWindow.ShowDialog() != true && !m_sdkEngine.LoginManager.IsConnected;
            if (userAborted)
            {
                Close();
            }
            else
            {
                Focus();
            }
        }

        /// <summary>
        /// Query the SDK Engine to get a list of available cameras.
        /// </summary>
        private void RefreshCameraList()
        {
            var query = (EntityConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
            query.EntityTypeFilter.Add(EntityType.Camera);
            QueryCompletedEventArgs results = query.Query();

            Dispatcher.BeginInvoke(new Action(() =>
                                              {
                                                  Cameras.Clear();
                                                  Cameras.Add(new CameraModel
                                                  {
                                                      CameraGuid = Guid.Empty,
                                                      CameraName = "<No Camera>",
                                                      CameraIcon = null,
                                                  });
                                                  if (SelectedCamera == null)
                                                  {
                                                      SelectedCamera = Cameras.First();
                                                      comboBox.SelectedItem = SelectedCamera;
                                                  }
                                              }));
            if (results.Data != null)
            {
                List<Guid> camGuids = results.Data.Rows.Cast<DataRow>().Select(row => (Guid)row[0]).ToList();
                
                // Add all cameras found by the SDK engine to the local list
                foreach (Guid camId in camGuids)
                {
                    var cam = (Camera)m_sdkEngine.GetEntity(camId);
                    if (cam == null)
                        continue;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Cameras.Add(new CameraModel
                        {
                            CameraGuid = camId,
                            CameraName = cam.Name,
                            CameraIcon = cam.GetIcon(true)
                        });
                    }));
                }
            }
        }

        #endregion
    }

    #endregion
}

