using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Media.Export;
using Genetec.Sdk.Queries;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Threading;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MediaExporterSample
{
    #region Classes

    /// <summary>
    /// Interaction logic for Window1.xaml
    ///  =============================================================
    ///   USING GENETEC.SDK.MEDIA
    ///  =============================================================
    ///  
    ///  Projects requiring the usage of the Genetec.Sdk.Media assembly
    ///  should add the following "Post-Build step":
    ///  
    ///  xcopy /R /Y "$(GSC_SDK)avcodec*.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)avformat*.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)avutil*.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)Genetec.*MediaComponent*" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)Genetec.Nvidia.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)Genetec.QuickSync.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)swscale*.dll" "$(TargetDir)"
    ///  xcopy /R /Y "$(GSC_SDK)swresample*.dll" "$(TargetDir)"
    ///  
    ///  This command will copy to the output of the project the EXE
    ///  and configuration files required for out-of-process decoding
    ///  for native and federated video streams.  Out-of-process 
    ///  decoding is a feature that provides:
    ///   - Improved memory usage for video operations by spreading
    ///     the memory usage over several processes
    ///   - Enhanced fault isolation when decoding video streams.  
    ///  
    ///  =============================================================
    /// </summary>
    public partial class MediaExporterWindow
    {
        #region Constants

        // Dependency property for the Bit rate content for the UI.
        public static readonly DependencyProperty BitRateContentProperty = DependencyProperty.Register(
                            "BitRateContent", typeof(string), typeof(MediaExporterWindow), new PropertyMetadata(default(string)));

        // Dependency property for the Bytes transfered content for the UI.
        public static readonly DependencyProperty BytesTransferedContentProperty = DependencyProperty.Register(
                            "BytesTransferedContent", typeof(string), typeof(MediaExporterWindow), new PropertyMetadata(default(string)));

        // Dependency property for the selected camera index.
        public static readonly DependencyProperty CameraSelectedIndexProperty = DependencyProperty.Register(
                            "CameraSelectedIndex", typeof(int), typeof(MediaExporterWindow), new PropertyMetadata(default(int)));

        // Dependency property for the frame rate content.
        public static readonly DependencyProperty FrameRateContentProperty = DependencyProperty.Register(
                          "FrameRateContent", typeof(string), typeof(MediaExporterWindow), new PropertyMetadata(default(string)));

        // Dependency property to see if we are exporting right now.
        public static readonly DependencyProperty IsExportingInProgressProperty = DependencyProperty.Register(
                            "IsExportingInProgress", typeof(bool), typeof(MediaExporterWindow), new PropertyMetadata(default(bool)));

        // Dependency property for the Sdk Engine connection.
        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                            "IsSdkEngineConnected", typeof(bool), typeof(MediaExporterWindow), new PropertyMetadata(default(bool)));

        // Dependency property for the progress bar value.
        public static readonly DependencyProperty ProgressPercentProperty = DependencyProperty.Register(
                            "ProgressPercent", typeof(double), typeof(MediaExporterWindow), new PropertyMetadata(default(double)));

        // Dependency property for the target export folder content.
        public static readonly DependencyProperty TargetExportFolderProperty = DependencyProperty.Register(
                            "TargetExportFolder", typeof(string), typeof(MediaExporterWindow), new PropertyMetadata("c:\\"));

        private const long GigabyteValue = MegabyteValue * 1024;

        private const long KilobyteValue = 1024;

        private readonly MediaExporter m_mediaExporter;

        private readonly Engine m_sdkEngine;

        private const long MegabyteValue = KilobyteValue * 1024;

        #endregion

        #region Fields

        private double m_bitrateValue;

        private double m_exportProgressionValue;

        private long m_exportSizeValue;

        private double m_framerateValue;

        #endregion

        #region Properties

        public string BitRateContent
        {
            get { return (string)GetValue(BitRateContentProperty); }
            set { SetValue(BitRateContentProperty, value); }
        }

        public string BytesTransferedContent
        {
            get { return (string)GetValue(BytesTransferedContentProperty); }
            set { SetValue(BytesTransferedContentProperty, value); }
        }

        public ObservableCollection<ComboBoxItem> CameraItems { get; private set; }

        public int CameraSelectedIndex
        {
            get { return (int)GetValue(CameraSelectedIndexProperty); }
            set { SetValue(CameraSelectedIndexProperty, value); }
        }

        public string FrameRateContent
        {
            get { return (string)GetValue(FrameRateContentProperty); }
            set { SetValue(FrameRateContentProperty, value); }
        }

        public bool IsExportingInProgress
        {
            get { return (bool)GetValue(IsExportingInProgressProperty); }
            set { SetValue(IsExportingInProgressProperty, value); }
        }

        public bool IsSdkEngineConnected
        {
            get { return (bool)GetValue(IsSdkEngineConnectedProperty); }
            set { SetValue(IsSdkEngineConnectedProperty, value); }
        }

        public double ProgressPercent
        {
            get { return (double)GetValue(ProgressPercentProperty); }
            set { SetValue(ProgressPercentProperty, value); }
        }

        public string TargetExportFolder
        {
            get { return (string)GetValue(TargetExportFolderProperty); }
            set { SetValue(TargetExportFolderProperty, value); }
        }

        #endregion

        #region Constructors

        public MediaExporterWindow()
        {
            InitializeComponent();

            //Media Exporter
            m_mediaExporter = new MediaExporter();
            m_mediaExporter.StatisticsReceived += OnExportStatisticsReceived;

            LoadStartEndTime();

            //Engine of the sdk
            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;

            //Observable collection for the camera combobox
            CameraItems = new ObservableCollection<ComboBoxItem>();

            //Set the datacontext to this class to use Dependency properties and Observable collections
            DataContext = this;
        }

        #endregion

        #region Event Handlers

        private void OnButtonBrowseClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog { Description = "Select a folder to export to." };

            if (folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetExportFolder = folderBrowser.SelectedPath;
            }
        }

        private void OnButtonCancelClicked(object sender, RoutedEventArgs e)
        {
            m_mediaExporter.CancelExport(true);
        }

        private void OnButtonConnectClick(object sender, RoutedEventArgs e)
        {
            try
            {
                m_sdkEngine.LoginManager.LogOn(m_directory.Text, m_userName.Text, m_password.Password);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not log in: " + ex);
            }
        }

        private void OnButtonExportClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToFile(MediaExportFileFormat.G64);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OnButtonExportToG64XClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ExportToFile(MediaExportFileFormat.G64X);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            IsSdkEngineConnected = true;
            PopulateCameraList();
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
        }

        private void OnExportStatisticsReceived(object sender, ExportStatisticsEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Background, (Action)(() =>
            {
                UpdateExport();
                m_bitrateValue = e.BitRate;
                m_exportProgressionValue = e.ExportPercentComplete;
                m_exportSizeValue = e.TotalBytesTransfered;
                m_framerateValue = e.FrameRate;
            }));
        }

        #endregion

        #region Private Methods

        private void EndExport(ExportEndedResult e)
        {
            IsExportingInProgress = false;
            UpdateExport();
            string message;

            //Check the export result to set a message depending on what happend.
            switch (e.Results)
            {
                case ExportResults.Successful:
                    ProgressPercent = 100;
                    message = "Export was successful";
                    break;
                case ExportResults.Cancelled:
                    message = "Export Cancelled";
                    break;
                case ExportResults.NoVideoAvailable:
                    message = "There was not video available for the specified time range.";
                    break;
                case ExportResults.SpecifiedServerUnavailable:
                    message = "The archiver is currently unavailable.";
                    break;
                case ExportResults.ConnectionLost:
                    message = "Lost connection to archiver during export.";
                    break;
                case ExportResults.EntityNotFound:
                    message = "The entity with the specified GUID was not found on the server.";
                    break;
                case ExportResults.InsufficientPrivilege:
                    message = "The user don't have the privilege from exporting the specified video(s)" + (e.ExceptionDetails.Message != null ? ": " + e.ExceptionDetails.Message : "");
                    break;
                case ExportResults.ErrorLegacyExportAllSequenceEncrypted:
                    message = "Export failed: All sequences are encrypted.";
                    break;
                default:
                    message = String.Format("An error occurred. ErrorMessage = {0}\n{1}", e.Results.ToString(), e.ErrorMessage ?? e.ExceptionDetails?.Message);
                    break;
            }

            MessageBox.Show(message);
            ResetStatistics();
        }

        /// <summary>
        /// Method to begin the export.
        /// </summary>
        /// <param name="format">The format to export of type <see cref="MediaExportFileFormat"/></param>
        private void ExportToFile(MediaExportFileFormat format)
        {
            DateTime start, end;

            if (DateTime.TryParse(m_startTime.Text, out start) && DateTime.TryParse(m_endTime.Text, out end))
            {
                IsExportingInProgress = true;

                var range = new Genetec.Sdk.Media.DateTimeRange
                {
                    StartTime = start.ToUniversalTime(),
                    EndTime = end.ToUniversalTime()
                };

                var selectedCamera = new Guid((CameraItems[CameraSelectedIndex]).Tag.ToString());
                bool exportOriginalVideo = m_exportOriginalVideo.IsChecked.HasValue && m_exportOriginalVideo.IsChecked.Value;
                m_mediaExporter.Initialize(m_sdkEngine, TargetExportFolder, Guid.Empty, exportOriginalVideo);
                m_mediaExporter.SetExportFileFormat(format);
                bool useWatermark = m_watermark.IsChecked.HasValue && m_watermark.IsChecked.Value;


                string textOverlayString = null;
                if (m_hasOverlay.IsChecked.HasValue && m_hasOverlay.IsChecked.Value && !string.IsNullOrWhiteSpace(m_textOverlay.Text))
                {
                    textOverlayString = m_textOverlay.Text;
                }
                bool canReExport = m_canReExport.IsChecked.HasValue && m_canReExport.IsChecked.Value;

                var exportedCamera = new CameraExportConfig(selectedCamera, new[] { range });
                var exportTask = m_mediaExporter.ExportAsync(exportedCamera, PlaybackMode.AllAtOnce, "FromSdk", useWatermark, canReExport, textOverlayString);

                exportTask.ContinueWith(x => Dispatcher.BeginInvoke((Action)(() => EndExport(x.Result))));
            }
        }

        /// <summary>
        /// Method used to write a default time for the start time and the end time on the UI.
        /// </summary>
        private void LoadStartEndTime()
        {
            DateTime time = DateTime.Now;
            m_startTime.Text = time.Subtract(TimeSpan.FromMinutes(10)).ToString("o");
            m_endTime.Text = time.ToString("o");
        }

        /// <summary>
        /// Populate the Camera list to be able to select one and export it.
        /// </summary>
        private void PopulateCameraList()
        {
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Camera);

                QueryCompletedEventArgs results = query.Query();
                foreach (DataRow row in results.Data.Rows)
                {
                    Camera camera = (Camera)m_sdkEngine.GetEntity((Guid)row[0]);
                    CameraItems.Add(new ComboBoxItem { Content = camera.Name, Tag = camera.Guid });
                }

                CameraSelectedIndex = 0;
            }
        }

        /// <summary>
        /// Reset the statistics on the UI. Takes the default values from the dependency properties and reasign them to the property.
        /// </summary>
        private void ResetStatistics()
        {
            BitRateContent = (string)BitRateContentProperty.DefaultMetadata.DefaultValue;
            BytesTransferedContent = (string)BytesTransferedContentProperty.DefaultMetadata.DefaultValue;
            FrameRateContent = (string)FrameRateContentProperty.DefaultMetadata.DefaultValue;
            ProgressPercent = (double)ProgressPercentProperty.DefaultMetadata.DefaultValue;
        }

        /// <summary>
        /// Update the UI with the most recent values.
        /// </summary>
        private void UpdateExport()
        {
            FrameRateContent = String.Format("{0} fps", m_framerateValue.ToString("F"));
            BitRateContent = String.Format("{0} kbps", m_bitrateValue.ToString("F"));

            if (m_exportSizeValue >= GigabyteValue)
            {
                BytesTransferedContent = String.Format("{0} Gigabytes", (m_exportSizeValue / GigabyteValue).ToString("F"));
            }
            else if (m_exportSizeValue >= MegabyteValue)
            {
                BytesTransferedContent = String.Format("{0} Megabytes", (m_exportSizeValue / MegabyteValue).ToString("F"));
            }
            else if (m_exportSizeValue >= KilobyteValue)
            {
                BytesTransferedContent = String.Format("{0} Kilobytes", (m_exportSizeValue / KilobyteValue).ToString("F"));
            }
            else
            {
                BytesTransferedContent = String.Format("{0} bytes", m_exportSizeValue.ToString("F"));
            }

            ProgressPercent = m_exportProgressionValue;
        }

        #endregion
    }

    #endregion
}

