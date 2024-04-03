using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Media;
using Genetec.Sdk.Queries;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AudioTransmitterSample
{
    #region Classs

    /// <summary>
    /// =============================================================
    ///  USING GENETEC.SDK.MEDIA
    /// =============================================================
    ///  
    /// Projects requiring the usage of the Genetec.Sdk.Media assembly
    /// should add the following "Post-Build step":
    ///  
    /// xcopy /R /Y "$(GSC_SDK)avcodec*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)avformat*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)avutil*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.*MediaComponent*" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.Nvidia.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)Genetec.QuickSync.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)swscale*.dll" "$(TargetDir)"
    /// xcopy /R /Y "$(GSC_SDK)swresample*.dll" "$(TargetDir)"
    ///  
    /// This command will copy to the output of the project the EXE
    /// and configuration files required for out-of-process decoding
    /// for native and federated video streams.  Out-of-process 
    /// decoding is a feature that provides:
    ///  - Improved memory usage for video operations by spreading
    ///    the memory usage over several processes
    ///  - Enhanced fault isolation when decoding video streams.  
    ///  
    /// =============================================================
    /// </summary>

    public partial class AudioTransmitterSampleForm : Form
    {
        #region Constants

        private readonly AudioTransmitter m_audioTransmitter;

        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields
        
        private readonly PcmAudioGenerator m_audioGenerator;

        #endregion

        #region Constructors

        public AudioTransmitterSampleForm()
        {
            InitializeComponent();

            m_audioTransmitter = new AudioTransmitter();
            m_audioGenerator = new PcmAudioGenerator(OnAudioFrameGenerated);

            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineProxyLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineProxyLogonFailed;

            m_audioTransmitter.UnhandledException += OnAudioTransmitterUnhandledException;

        }

        #endregion

        #region Event Handlers

        private void OnAudioFrameGenerated(byte[] data, int offset, int datasize)
        {
            // queue the audio data in the audio transmitter
            m_audioTransmitter.QueueBuffer(data, offset, datasize);

            // OnAudioFrameGenerated is invoked on the threadpool by the audio generator,
            // if you want to update UI in this method, you will have to forward the execution
            // on the UI thread.
            BeginInvoke((Action)(() =>
            {
                string formattedString =
                    String.Format("Audio frame queued for transmission: DataOffset: {0}, Data size: {1}, UtcNow: {2}",
                        offset,
                        datasize,
                        DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));

                lstFrames.Items.Add(formattedString);
                lstFrames.TopIndex = lstFrames.Items.Count - 1;
            }));
        }

        private void OnButtonConnectClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            m_sdkEngine.LoginManager.BeginLogOn(m_txtDirectory.Text, m_txtUserName.Text, m_txtPassword.Text);
        }

        private async void OnButtonStartVideoSourceClick(object sender, EventArgs e)
        {
            lstFrames.Items.Clear();
            var initialCursor = Cursor;
            Cursor = Cursors.WaitCursor;

            btnStopVideoSource.Enabled = true;
            btnStartVideoSource.Enabled = false;

            try
            {
                // Initialize and start audio transmitter
                m_audioTransmitter.Initialize(m_sdkEngine, (Guid)((ComboBoxItem)cameraGuidComboBox.SelectedItem).Tag);
                await m_audioTransmitter.StartTransmitting();

                // Set the generator payload size to optimize the audio transmission. If queued payload is smaller than IdealPayloadSize, 
                // data will be accumulated until we have enough data. If queued payload is bigger, the data will be sliced in order to send 
                // what we can and keep the leftover accumulated.
                m_audioGenerator.GeneratedPayloadSize = m_audioTransmitter.IdealPayloadSize;

                // Start audio generator
                m_audioGenerator.Start();
            }
            catch (FormatException formatException)
            {
                MessageBox.Show(this,
                    formatException.Message,
                    "Format exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Cursor = initialCursor;
            }
            catch (FileNotFoundException fileException)
            {
                MessageBox.Show(this,
                    fileException.Message,
                    "File not found exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Cursor = initialCursor;
            }
            Cursor = initialCursor;
        }

        private void OnButtonStopVideoSourceClick(object sender, EventArgs e)
        {
            m_audioTransmitter.StopTransmitting();
            m_audioGenerator.Stop();

            btnStopVideoSource.Enabled = false;
            btnStartVideoSource.Enabled = true;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            m_audioTransmitter.Dispose();
        }

        private void OnEngineProxyLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_grpCamInfo.Enabled = true;
            btnStartVideoSource.Enabled = true;
            m_grpConnectionInfo.Enabled = false;
            Cursor = Cursors.Default;

            //For the sake of simplicity, the following operation is done on the UI thread instead of 
            //doing the queries on a background thread and bringing the results back into the UI, 
            //hence in this sample de UI might freeze while the query complete.
            PopulateCameraList();

        }

        private void OnEngineProxyLogonFailed(object sender, LogonFailedEventArgs e)
        {
            Cursor = Cursors.Default;
            MessageBox.Show("Could not log in: " + e.FailureCode);
        }

        private void OnAudioTransmitterUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Handle any AudioTransmitter unhandled exception
            BeginInvoke((Action)(() =>
            {
                Cursor = Cursors.Default;

                Exception exception = e.ExceptionObject as Exception;

                if (exception != null)
                {
                    MessageBox.Show(this,
                        "An unhandled exception occurred in the player : \r\n" + exception,
                        "Unhandled exception",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(this,
                        "An unhandled exception occurred in the player",
                        "Unhandled exception",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                btnStopVideoSource.Enabled = false;
                btnStartVideoSource.Enabled = true;
            }));
        }

        #endregion

        #region Private Methods

        private void PopulateCameraList()
        {
            cameraGuidComboBox.Items.Clear();
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Camera);

                QueryCompletedEventArgs results = query.Query();
                foreach (DataRow row in results.Data.Rows)
                {
                    ComboBoxItem item = new ComboBoxItem();
                    Camera cam = (Camera)m_sdkEngine.GetEntity((Guid)row[0]);
                    item.Content = cam.Name;
                    item.Tag = cam.Guid;
                    cameraGuidComboBox.Items.Add(item);
                }

                cameraGuidComboBox.SelectedIndex = 0;
            }
        }

        #endregion

        private void AudioTransmitterSampleForm_Load(object sender, EventArgs e)
        {

        }
    }
    #endregion
}