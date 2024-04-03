using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Genetec.Sdk.Media.Reader;
using System.Text;
using System.Text.RegularExpressions;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace PlaybackStreamReaderSample
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
    public partial class PlaybackStreamReaderSampleForm : Form
    {
        #region Constants

        private readonly Engine m_sdkEngine;

        private readonly Guid m_videoGuid = Guid.NewGuid();

        private readonly Guid m_audioGuid = Guid.NewGuid();

        #endregion

        #region Fields

        
        private CancellationTokenSource m_cts = new CancellationTokenSource();
        
        private PlaybackStreamReader m_reader;

        private List<string> m_pendingTraces = new List<string>();
        private Dictionary<string, object> m_readTraces = new Dictionary<string, object>();

        private DateTime m_lastUpdate = DateTime.MinValue;

        private PlaybackSequenceQuerier m_querier;

        private Guid m_currentCamera;

        private Guid m_currentStream;

        #endregion

        #region Properties

        #endregion

        #region Constructors

        public PlaybackStreamReaderSampleForm()
        {
            InitializeComponent();

            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineProxyLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineProxyLogonFailed;

        }

        #endregion

        #region Event Handlers

        private void OnEngineProxyLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_grpCamInfo.Enabled = true;
            btnConnectReader.Enabled = true;
            m_grpConnectionInfo.Enabled = false;
            Cursor = Cursors.Default;

            //For the sake of simplicity, the following operation is done on the UI thread instead of 
            //doing the queries on a background thread and bringing the results back into the UI, 
            //hence in this sample de UI might freeze while the query completes.
            PopulateCameraList();

        }

        private void OnEngineProxyLogonFailed(object sender, LogonFailedEventArgs e)
        {
            Cursor = Cursors.Default;
            MessageBox.Show("Could not log in: " + e.FailureCode);
        }

        private void OnComboBoxCameraSelectedIndexChanged(object sender, EventArgs e)
        {
            var cameraId = (Guid)((ComboBoxItem)cameraGuidComboBox.SelectedItem).Tag;
            
            PopulateStreamList(cameraId);
            HandleExceptions(PopulateSourceList(cameraId));
        }

        private void PopulateStreamList(Guid cameraId)
        {
            var camera = m_sdkEngine.EntityManager.GetEntity<Camera>(cameraId);

            comboBoxStream.Items.Clear();
            ComboBoxItem item = new ComboBoxItem();
            item.Content = "Video";
            item.Tag = m_videoGuid;
            comboBoxStream.Items.Add(item);

            item = new ComboBoxItem();
            item.Content = "Audio";
            item.Tag = m_audioGuid;
            comboBoxStream.Items.Add(item);

            foreach (var metadataStream in camera.MetadataStreams)
            {
                item = new ComboBoxItem();
                item.Content = metadataStream.Name;
                item.Tag = metadataStream.StreamId;
                comboBoxStream.Items.Add(item);
            }

            comboBoxStream.SelectedIndex = 0;
        }

        private async Task PopulateSourceList(Guid cameraId)
        {
            comboBoxSource.Items.Clear();
            var loading = new ComboBoxItem { Content = "Loading...", Tag = Guid.Empty };
            comboBoxSource.Items.Add(loading);
            comboBoxSource.SelectedIndex = 0;
            var querier = PlaybackSequenceQuerier.CreateVideoQuerier(m_sdkEngine, cameraId);
            try
            {
                var sources = await querier.QuerySequencesBySourcesAsync(new Genetec.Sdk.Media.DateTimeRange(DateTime.MinValue, DateTime.MaxValue));
                comboBoxSource.Items.Clear();
                var allSources = new ComboBoxItem()
                {
                    Content = "All sources",
                    Tag = Guid.Empty
                };
                comboBoxSource.Items.Add(allSources);

                foreach (var source in sources)
                {
                    var item = new ComboBoxItem()
                    {
                        Content = source.PlaybackSourceGuid.ToString("B"),
                        Tag = source.PlaybackSourceGuid
                    };
                    comboBoxSource.Items.Add(item);
                }

                comboBoxSource.SelectedIndex = 0;
            }
            finally
            {
                await querier.DisposeAsync();
            }
        }

        private void OnButtonConnectClick(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            m_sdkEngine.LoginManager.BeginLogOn(m_txtDirectory.Text, m_txtUserName.Text, m_txtPassword.Text);
        }

        private void OnButtonConnectReaderClick(object sender, EventArgs e)
        {
            HandleExceptions(ConnectAsync());

            async Task ConnectAsync()
            {
                Cursor = Cursors.WaitCursor;

                var cameraId = (Guid)((ComboBoxItem)cameraGuidComboBox.SelectedItem).Tag;
                var streamId = (Guid)((ComboBoxItem)comboBoxStream.SelectedItem).Tag;
                var sourceId = (Guid)((ComboBoxItem)comboBoxSource.SelectedItem).Tag;

                if (streamId == m_videoGuid)
                {
                    m_reader = sourceId != Guid.Empty ? PlaybackStreamReader.CreateVideoReader(m_sdkEngine, cameraId, sourceId)
                                                      : PlaybackStreamReader.CreateVideoReader(m_sdkEngine, cameraId);
                }
                else if (streamId == m_audioGuid)
                {
                    m_reader = sourceId != Guid.Empty ? PlaybackStreamReader.CreateAudioReader(m_sdkEngine, cameraId, sourceId)
                                                      : PlaybackStreamReader.CreateAudioReader(m_sdkEngine, cameraId);
                }
                else
                {
                    m_reader = sourceId != Guid.Empty ? PlaybackStreamReader.CreateReader(m_sdkEngine, cameraId, streamId, sourceId)
                                                      : PlaybackStreamReader.CreateReader(m_sdkEngine, cameraId, streamId);
                }

                await m_reader.ConnectAsync(m_cts.Token);

                Cursor = Cursors.Default;
                btnConnectReader.Enabled = false;
                btnDisconnectReader.Enabled = true;
                btnSeek.Enabled = true;
                dtpSeekDate.Enabled = true;
            }
        }

        private void btnDisconnectReader_Click(object sender, EventArgs e)
        {
            m_cts?.Cancel();
            m_cts?.Dispose();
            m_reader?.Dispose();

            m_cts = new CancellationTokenSource();

            btnConnectReader.Enabled = true;
            btnDisconnectReader.Enabled = false;
            btnSeek.Enabled = false;
            dtpSeekDate.Enabled = false;
            btnStartRead.Enabled = false;
        }

        private void OnButtonSeekClick(object sender, EventArgs e)
        {
            HandleExceptions(SeekAsync());
            async Task SeekAsync()
            {
                Cursor = Cursors.WaitCursor;

                var seekTime = dtpSeekDate.Value.ToUniversalTime();
                try
                {
                    await m_reader.SeekAsync(seekTime, m_cts.Token);
                    Cursor = Cursors.Default;
                    btnStartRead.Enabled = true;
                    btnSeek.Enabled = true;
                }
                catch (Exception e)
                {
                    AddTrace($"Seek failed: {e}");
                    Cursor = Cursors.Default;
                    return;
                }
            }
        }

        private void OnButtonStartReadClick(object sender, EventArgs e)
        {
            lstFrames.Items.Clear();
            
            btnConnectReader.Enabled = false;
            btnSeek.Enabled = false;
            dtpSeekDate.Enabled = false;
            btnStartRead.Enabled = false;
            btnStopRead.Enabled = true;

            Task.Run(ReadFrame);
        }

        private void OnButtonStopReadClick(object sender, EventArgs e)
        {
            m_cts.Cancel();
            m_cts.Dispose();
            m_cts = new CancellationTokenSource();
            
            btnDisconnectReader.Enabled = true;
            btnSeek.Enabled = true;
            dtpSeekDate.Enabled = true;
            btnStartRead.Enabled = true;
            btnStopRead.Enabled = false;
        }

        private void btnQuerySequences_Click(object sender, EventArgs e)
        {
            HandleExceptions(QuerySequences());

            async Task QuerySequences()
            {
                lstSequences.Items.Clear();
                CreateQuerier();

                var sequences = await m_querier.QuerySequencesAsync(new Genetec.Sdk.Media.DateTimeRange(DateTime.MinValue, DateTime.MaxValue));
                foreach (var s in sequences)
                {
                    lstSequences.Items.Add($"Start: {s.Range.StartTime:O} End: {s.Range.EndTime:O}");
                }
            }
        }

        private void btnQueryBySource_Click(object sender, EventArgs e)
        {
            HandleExceptions(QuerySources());

            async Task QuerySources()
            {
                lstSequences.Items.Clear();
                CreateQuerier();

                var sources = await m_querier.QuerySequencesBySourcesAsync(new Genetec.Sdk.Media.DateTimeRange(DateTime.MinValue, DateTime.MaxValue));
                foreach (var src in sources)
                {
                    lstSequences.Items.Add($"Source: {src.PlaybackSourceGuid} IsOnline: {src.IsOnline}");
                    foreach (var s in src.Sequences)
                        lstSequences.Items.Add($"   Start: {s.Range.StartTime:O} End: {s.Range.EndTime:O}");
                }
            }
        }

        private void OnFrameItemDoubleClick(object sender, MouseEventArgs e)
        {
            int index = lstFrames.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                if (m_readTraces.TryGetValue(lstFrames.SelectedItem.ToString(), out var data))
                {
                    var frameData = data as byte[];
                    if (frameData != null)
                    {
                        frameData = frameData.Take(5000).ToArray(); // Trim to avoid too long string for the window.

                        string hexData = BitConverter.ToString(frameData);
                        MessageBox.Show(
                             "Hexa: " + Environment.NewLine + hexData
                            + Environment.NewLine + Environment.NewLine
                            + "Text: " + Environment.NewLine + GetPrintableUtf16String(hexData));
                    }
                }
            }
        }

        private string GetPrintableUtf16String(string hexData)
        {
            var hexCharacters = hexData.Split('-');
            var stringBuilder = new StringBuilder();
            foreach (var hex in hexCharacters)
            {
                stringBuilder.Append(Convert.ToChar(Convert.ToInt32(hex, 16)));
            }
            
            // Remove most non-printable ASCII and line breaks
            return Regex.Replace(stringBuilder.ToString(), @"[^\u0020-\u007F]", "");
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            m_querier?.DisposeAsync();

            m_cts?.Cancel();
            m_reader?.Dispose();
            m_cts?.Dispose();
        }

        #endregion

        #region Private Methods

        private void AddTrace(string formattedString, bool updateNow = true, byte[] additionalData = null)
        {
            formattedString = $"#{m_readTraces.Count} {formattedString}";
            m_readTraces.Add(formattedString, additionalData);
            m_pendingTraces.Add(formattedString);

            bool update = updateNow;
            if (!updateNow && DateTime.Now - m_lastUpdate > TimeSpan.FromMilliseconds(100))
            {
                update = true;
                m_lastUpdate = DateTime.Now;
            }

            if (update)
            {
                var copyList = m_pendingTraces.ToList();
                m_pendingTraces.Clear();
                BeginInvoke((Action) (() =>
                {
                    copyList.ForEach(x => lstFrames.Items.Add(x));
                    lstFrames.TopIndex = lstFrames.Items.Count - 1;
                }));
            }
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

        private async Task ReadFrame()
        {
            RawDataContent data = null;
            bool endReached = false;
            bool connectionLost = false;
            int nbFrames = 0;
            var sw = Stopwatch.StartNew();
            do
            {
                try
                {
                    data = await m_reader.ReadAsync(m_cts.Token);
                    if (data != null)
                    {
                        nbFrames++;
                        byte[] additionalData = CopyDataSegment(data.Data);
                        AddTrace($"Frame time: {data.FrameTime}, type: {data.Format}, Data size: {data.Data.Count}, Source: {data.PlaybackSourceGuid}", false, additionalData);
                    }
                    else
                    {
                        AddTrace("End reached");
                        endReached = true;
                    }
                }
                catch (TaskCanceledException)
                {
                    AddTrace("Reader stopped");
                    break;
                }
                catch (ConnectionLostException)
                {
                    AddTrace("ConnectionLost");
                    endReached = true;
                    connectionLost = true;
                    break;
                }
                catch (Exception e)
                {
                    AddTrace($"{e}");
                    endReached = true;
                    break;
                }
                
                data?.Dispose();

            } while (data != null);

            sw.Stop();
            AddTrace($"{nbFrames} frames read in {sw.Elapsed}");

            BeginInvoke((Action) (() =>
            {
                btnDisconnectReader.Enabled = true;
                btnSeek.Enabled = !connectionLost;
                dtpSeekDate.Enabled = !connectionLost;
                btnStartRead.Enabled = !endReached;
                btnStopRead.Enabled = false;
            }));
        }

        private static byte[] CopyDataSegment(ArraySegment<byte> arraySegment)
        {
            byte[] additionalData = null;
            if (arraySegment != null)
            {
                additionalData = new byte[arraySegment.Count];
                Array.Copy(arraySegment.Array, arraySegment.Offset, additionalData, 0, arraySegment.Count);
            }

            return additionalData;
        }

        private void CreateQuerier()
        {
            var cameraId = (Guid)((ComboBoxItem)cameraGuidComboBox.SelectedItem).Tag;
            var streamId = (Guid)((ComboBoxItem)comboBoxStream.SelectedItem).Tag;

            if (m_currentCamera != cameraId || m_currentStream != streamId)
            {
                HandleExceptions(m_querier?.DisposeAsync() ?? Task.CompletedTask);
                
                m_currentCamera = cameraId;
                m_currentStream = streamId;

                if (streamId == m_videoGuid)
                {
                    m_querier = PlaybackSequenceQuerier.CreateVideoQuerier(m_sdkEngine, cameraId);
                }
                else if (streamId == m_audioGuid)
                {
                    m_querier = PlaybackSequenceQuerier.CreateAudioQuerier(m_sdkEngine, cameraId);
                }
                else
                {
                    m_querier = PlaybackSequenceQuerier.CreateQuerier(m_sdkEngine, cameraId, streamId);
                }
            }
        }

        private void HandleExceptions(Task t)
        {
            var _ = Handle();
            async Task Handle()
            {
                try { await t; }
                catch (Exception e) { MessageBox.Show(this, e.ToString(), "Unhandled exception"); }
            }
        }


        #endregion
    }

    #endregion
}

