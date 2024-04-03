using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Media;
using Genetec.Sdk.Queries;
using System;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Forms;
using File = System.IO.File;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VideoSourceFilterSample
{
    #region Classes

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
    public partial class AudioVideoSourceFilterSampleForm : Form
    {
        #region Constants

        private readonly AudioVideoSourceFilter m_decodedSourceFilter;

        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields

        private bool m_callSeekAfterPlaybackSequenceListRetreived;

        private DateTime m_lastDecodedFrameTime;

        private volatile bool m_waitingForVideoStartedEvent;

        #endregion

        #region Properties

        private bool IsCamera { get { return comboBoxSource.SelectedIndex == 0; } }

        #endregion

        #region Constructors

        public AudioVideoSourceFilterSampleForm()
        {
            InitializeComponent();

            comboBoxSource.SelectedIndex = 0;

            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineProxyLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineProxyLogonFailed;

            m_decodedSourceFilter = new AudioVideoSourceFilter();
            m_decodedSourceFilter.Started += OnVideoStarted;
            m_decodedSourceFilter.AudioDataPlayed += OnAudioFrameDecoded;
            m_decodedSourceFilter.FrameDecoded += OnFrameDecoded;
            m_decodedSourceFilter.PlaybackSequenceListRetreived += OnPlaybackSequenceListRetreived;
            m_decodedSourceFilter.PlayerStateChanged += OnPlayerStateChanged;
            m_decodedSourceFilter.UnhandledException += OnSourceFilterUnhandledException;

        }

        #endregion

        #region Event Handlers

        private void OnAudioFrameDecoded(object sender, AudioFrameDecodedEventArgs e)
        {
            // OnAudioFrameDecoded is invoked on an internal thread of the player,
            // if you want to update UI in this method, you will have to forward the execution
            // on the UI thread.
            BeginInvoke((Action)(() =>
            {
                if (e.DecodedAudioFrameContent.IsDisposed)
                {
                    return;
                }

                try
                {
                    m_lastDecodedFrameTime = e.FrameTime;

                    string formattedString = String.Format("Frame time (audio): {0}, DataOffset: {1}, Data size: {2}",
                        e.FrameTime,
                        e.DecodedAudioFrameContent.DataOffset,
                        e.DecodedAudioFrameContent.DataSize);

                    lstFrames.Items.Add(formattedString);
                    lstFrames.TopIndex = lstFrames.Items.Count - 1;

                    // do something with the decoded audio (PCM format)
                }
                finally
                {
                    // This is very important.. if the frame memory is not released,
                    // memory usage will not be optimal and performance will be
                    // degraded significantly.
                    e.DecodedAudioFrameContent.Dispose();
                }
            }));
        }

        private void OnButtonBrosweClick(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Title = "Select the path to the video",
                Filter = "Video Files (g64, g64x)|*.g64;*.g64x"
            };

            if (fileDialog.ShowDialog() ?? false)
            {
                m_txtCamGuid.Text = fileDialog.FileName;
            }
        }

        private void OnButtonConnectClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            m_sdkEngine.LoginManager.BeginLogOn(m_txtDirectory.Text, m_txtUserName.Text, m_txtPassword.Text);
        }

        private void OnButtonSeekMinusOneMinuteClick(object sender, EventArgs e)
        {
            SeekOneMinuteBehind();
        }

        private void OnButtonStartVideoSourceClick(object sender, EventArgs e)
        {
            lstFrames.Items.Clear();
            var initialCursor = Cursor;
            Cursor = Cursors.WaitCursor;
            m_waitingForVideoStartedEvent = true;

            try
            {
                if (IsCamera)
                {
                    m_decodedSourceFilter.Initialize(m_sdkEngine, (Guid)((ComboBoxItem)cameraGuidComboBox.SelectedItem).Tag);
                }
                else
                {
                    m_decodedSourceFilter.OpenFile(m_txtCamGuid.Text);
                }

                if (IsCamera)
                {
                    m_decodedSourceFilter.PlayLive();
                }
                else
                {
                    //Starting with 5.3, call PlayFile instead of ResumePlaying
                    //m_decodedSourceFilter.ResumePlaying();
                    m_decodedSourceFilter.PlayFile();
                }
            }
            catch (FormatException formatException)
            {
                MessageBox.Show(this,
                    formatException.Message,
                    "Format exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Cursor = initialCursor;
                m_waitingForVideoStartedEvent = false;
            }
            catch (FileNotFoundException fileException)
            {
                MessageBox.Show(this,
                    fileException.Message,
                    "File not found exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Cursor = initialCursor;
                m_waitingForVideoStartedEvent = false;
            }
        }

        private void OnButtonStopVideoSourceClick(object sender, EventArgs e)
        {
            m_decodedSourceFilter.Stop();

            btnStopVideoSource.Enabled = false;
            btnStartVideoSource.Enabled = true;
            btnSeekMinusOneMinute.Enabled = false;
        }

        private void OnComboBoxSourceSelectedIndexChanged(object sender, EventArgs e)
        {
            if (IsCamera)
            {
                m_txtCamGuid.Visible = false;
                browseFileButton.Visible = false;
                cameraGuidComboBox.Visible = true;
            }
            else
            {
                m_txtCamGuid.Visible = true;
                browseFileButton.Visible = true;
                cameraGuidComboBox.Visible = false;
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_decodedSourceFilter.State != PlayerState.Stopped)
            {
                m_decodedSourceFilter.Stop();
                m_decodedSourceFilter.AudioDataPlayed -= OnAudioFrameDecoded;
                m_decodedSourceFilter.FrameDecoded -= OnFrameDecoded;
                m_decodedSourceFilter.PlaybackSequenceListRetreived -= OnPlaybackSequenceListRetreived;
                m_decodedSourceFilter.Started -= OnVideoStarted;
                m_decodedSourceFilter.PlayerStateChanged -= OnPlayerStateChanged;
                m_decodedSourceFilter.UnhandledException -= OnSourceFilterUnhandledException;
            }

            m_decodedSourceFilter.Dispose();
        }

        private void OnFrameDecoded(object sender, FrameDecodedEventArgs e)
        {
            m_lastDecodedFrameTime = e.FrameTime;
            // OnFrameDecoded is invoked on an internal thread of the player,
            // if you want to update UI in this method, you will have to forward the execution
            // on the UI thread.
            BeginInvoke((Action)(() =>
            {
                if (e.DecodedFrameContent.IsDisposed)
                {
                    return;
                }

                try
                {
                    string formattedString = String.Format("Frame time: {0}, type: {1}, Data size: {2}",
                        e.FrameTime,
                        e.DecodedFrameContent.Format,
                        e.DecodedFrameContent.DataSize);

                    AddTrace(formattedString);

                    RgbDecodedFrame decodedFrame = e.DecodedFrameContent.GetBitmap();
                    //decodedFrame.Bitmap.Save(@"D:\temp\" + DateTime.UtcNow.ToString("hh-mm-ss-ffff") + ".jpg", ImageFormat.Jpeg);
                    decodedFrame.Dispose();
                }
                finally
                {
                    // This is very important.. if the frame memory is not released,
                    // memory usage will not be optimal and performance will be
                    // degraded significantly.
                    e.DecodedFrameContent.Dispose();
                }
            }));
        }

        private void OnPlaybackSequenceListRetreived(object sender, EventArgs e)
        {
            if (m_callSeekAfterPlaybackSequenceListRetreived)
            {
                SeekOneMinuteBehind();
            }
        }

        private void OnPlayerStateChanged(object sender, PlayerStateChangedEventArgs e)
        {
            if (m_waitingForVideoStartedEvent)
            {
                BeginInvoke((Action)(() => AddTrace(string.Format("Player state: {0}", e.State))));
                if (e.State == PlayerState.Error)
                {
                    BeginInvoke((Action)(() =>
                    {
                        m_waitingForVideoStartedEvent = false;
                        Cursor = Cursors.Default;

                        var errorDetails = m_decodedSourceFilter.ErrorDetails;

                        MessageBox.Show(this,
                            "An error occurred (" + errorDetails.Code + "): " +
                            errorDetails.Details,
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        btnStopVideoSource.Enabled = false;
                        btnStartVideoSource.Enabled = true;
                    }));
                }
            }
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

        private void OnSourceFilterUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                m_waitingForVideoStartedEvent = false;
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

        private void OnVideoStarted(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                if (m_waitingForVideoStartedEvent)
                {
                    Cursor = Cursors.Default;

                    btnStopVideoSource.Enabled = true;
                    btnStartVideoSource.Enabled = false;
                    btnSeekMinusOneMinute.Enabled = true;
                    m_waitingForVideoStartedEvent = false;
                }
            }));
        }

        #endregion

        #region Private Methods

        private void AddTrace(string formattedString)
        {
            lstFrames.Items.Add(formattedString);
            lstFrames.TopIndex = lstFrames.Items.Count - 1;
        }

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

                if (cameraGuidComboBox.Items.Count > 0)
                {
                cameraGuidComboBox.SelectedIndex = 0;
            }
                    
            }
        }

        private void SeekOneMinuteBehind()
        {
            DateTime seekTime = m_lastDecodedFrameTime - TimeSpan.FromMinutes(1);

            // Seek time must always be between the current boundaries of the TimeLineRange.
            // If its not, the SetTimeLineRange method must be called with boundaries that includes
            // the seek time, and we must wait for the PlaybackSequenceListRetreived event to be invoked.
            if (m_callSeekAfterPlaybackSequenceListRetreived)
            {
                m_callSeekAfterPlaybackSequenceListRetreived = false;
                m_decodedSourceFilter.Seek(seekTime);
            }
            else
            {
                // Make sure the timeline range contains the seek time before seeking
                m_callSeekAfterPlaybackSequenceListRetreived = true;
                m_decodedSourceFilter.SetTimeLineRange(m_lastDecodedFrameTime - TimeSpan.FromHours(1), DateTime.UtcNow);
            }
        }

        #endregion

        private void checkBoxAudioEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAudioEnabled.CheckState == CheckState.Checked)
            {
                checkBoxAudioEnabled.Text = "Disable audio";
                m_decodedSourceFilter.IsAudioEnabled = true;
            }
            else
            {
                checkBoxAudioEnabled.Text = "Enable audio";
                m_decodedSourceFilter.IsAudioEnabled = false;
            }
        }
    }

    #endregion
}

