using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VideoViewer
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
    ///   
    /// This command will copy to the output of the project the EXE
    /// and configuration files required for out-of-process decoding
    /// for native and federated video streams.  Out-of-process 
    /// decoding is a feature that provides:
    /// - Improved memory usage for video operations by spreading
    /// the memory usage over several processes
    /// - Enhanced fault isolation when decoding video streams.  
    /// =============================================================
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        public static readonly DependencyProperty IsInPlaybackProperty =
                                DependencyProperty.Register("IsInPlayback", typeof(bool), typeof(MainWindow), new PropertyMetadata(false));

        // Dependency property on the connection of the Sdk engine.
        public static readonly DependencyProperty IsSdkEngineConnectedProperty =
                                DependencyProperty.Register("IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty OverlayNameProperty =
                                DependencyProperty.Register("OverlayName", typeof(string), typeof(MainWindow), new PropertyMetadata("Show Status"));

        public static readonly DependencyProperty PlayPauseProperty =
                                DependencyProperty.Register("PlayPause", typeof(string), typeof(MainWindow), new PropertyMetadata("Pause"));

        public static readonly DependencyProperty TimeStampProperty =
                                DependencyProperty.Register("TimeStamp", typeof(string), typeof(MainWindow), new PropertyMetadata(""));

        public static readonly DependencyProperty TogglePlaybackProperty =
                                DependencyProperty.Register("TogglePlayback", typeof(string), typeof(MainWindow), new PropertyMetadata("Switch to Playback"));
        public static readonly DependencyProperty IsAtLeastOneTilePlayingProperty =
                                DependencyProperty.Register("IsAtLeastOneTilePlaying", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));
        private readonly Engine m_sdkEngine = new Engine();

        private readonly List<Tile> m_tiles = new List<Tile>();

        #endregion

        #region Fields

        private bool m_canDragDrop;

        private bool m_isFirstLoad;

        private bool m_isRewinding;

        private OverlayType m_nextOverlayType = OverlayType.Status;

        private PlaySpeed m_playSpeed;

        private Tile m_selectedTile;

        #endregion

        #region Properties

        public bool IsInPlayback
        {
            get { return (bool)GetValue(IsInPlaybackProperty); }
            set { SetValue(IsInPlaybackProperty, value); }
        }

        public bool IsAtLeastOneTilePlaying
        {
            get { return (bool)GetValue(IsAtLeastOneTilePlayingProperty); }
            set { SetValue(IsAtLeastOneTilePlayingProperty, value); }
        }

        public bool IsSdkEngineConnected
        {
            get { return (bool)GetValue(IsSdkEngineConnectedProperty); }
            set { SetValue(IsSdkEngineConnectedProperty, value); }
        }

        public string OverlayName
        {
            get { return (string)GetValue(OverlayNameProperty); }
            set { SetValue(OverlayNameProperty, value); }
        }

        public string PlayPause
        {
            get { return (string)GetValue(PlayPauseProperty); }
            set { SetValue(PlayPauseProperty, value); }
        }

        public string TimeStamp
        {
            get { return (string)GetValue(TimeStampProperty); }
            set { SetValue(TimeStampProperty, value); }
        }

        public string TogglePlayback
        {
            get { return (string)GetValue(TogglePlaybackProperty); }
            set { SetValue(TogglePlaybackProperty, value); }
        }

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tree.EntityFilter = new List<EntityType> { EntityType.Area, EntityType.Camera };
            tree.Initialize(m_sdkEngine);
            IsAtLeastOneTilePlaying = false;
            foreach (Tile tile in grid.Children.OfType<Tile>())
            {
                m_tiles.Add(tile);
                tile.player.HardwareAccelerationEnabled = true;
                tile.player.LivePlaybackModeToggled += OnPlayerLivePlaybackModeToggled;
                tile.player.FrameRendered += OnPlayerFrameRendered;
            }
            CheckIfShouldDisableCheckBox();
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLogonSuccess;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach (var tile in m_tiles)
                tile.player.Dispose();

            Genetec.Sdk.Media.MediaPlayer.CleanUpStaticResources();
            base.OnClosing(e);
        }

        private void OnButtonPlayClick(object sender, RoutedEventArgs e)
        {
            Tile tile = GetFirstPopulatedTile();
            if (tile.player.State == PlayerState.Paused)
            {
                if (m_isRewinding)
                {
                    tile.player.Rewind();
                }
                else
                {
                    tile.player.ResumePlaying();
                }
                tile.player.PlaySpeed = m_playSpeed;
                PlayPause = "Pause";
            }
            else
            {
                tile.player.Pause();
                PlayPause = "Play";
            }

            CheckIfShouldDisableCheckBox();
        }

        private void CheckIfShouldDisableCheckBox()
        {
            IsAtLeastOneTilePlaying = false;
            foreach (Tile aTile in m_tiles)
            {
                IsAtLeastOneTilePlaying |= aTile.IsStarted();
            }
        }
        private void OnButtonReverseClick(object sender, RoutedEventArgs e)
        {
            Tile tile = GetFirstPopulatedTile();
            tile.player.Rewind();
            tile.player.PlaySpeed = m_playSpeed;
            PlayPause = "Pause";
            m_isRewinding = true;
        }

        private void OnButtonToggleClick(object sender, RoutedEventArgs e)
        {
            if (PlayersPlayingLive())
            {
                // playback - 1 minute
                Tile tempTile = GetFirstPopulatedTile();
                tempTile.player.PlayArchive(DateTime.UtcNow.AddMinutes(-1));
                foreach (Tile tile in m_tiles.Where(tile => tile.IsStarted()))
                {
                    tile.player.AddToSynchronizedPlayback();
                }
                PlayPause = "Pause";
                m_isRewinding = false;
                TogglePlayback = "Switch to Live";
                playSpeedComboBox.Text = "1X";
            }
            else
            {
                foreach (Tile tile in m_tiles)
                {
                    tile.player.PlayLive();
                }
                TogglePlayback = "Switch to Playback";
            }
        }

        private void OnButtonToggleOverlayClick(object sender, RoutedEventArgs e)
        {
            foreach (Tile tile in m_tiles)
            {
                tile.player.ShowSpecialOverlay(m_nextOverlayType);
            }
            switch (m_nextOverlayType)
            {
                case OverlayType.None:
                    m_nextOverlayType = OverlayType.Status;
                    OverlayName = "Show Status";
                    break;
                case OverlayType.Status:
                    m_nextOverlayType = OverlayType.None;
                    OverlayName = "Hide Overlay";
                    break;
                    //A statistics overlay can also be shown
            }
        }

        private void OnComboBoxPlaySpeedChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!m_isFirstLoad)
            {
                ComboBoxItem item = e.AddedItems[0] as ComboBoxItem;
                if (item != null)
                {
                    int enumValue = int.Parse(item.Tag.ToString());
                    m_playSpeed = (PlaySpeed)enumValue;
                    Tile tile = GetFirstPopulatedTile();
                    if (tile != null)
                    {
                        tile.player.PlaySpeed = m_playSpeed;
                    }
                }
            }
            else
            {
                m_isFirstLoad = false;
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
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
        }

        private void OnEngineLogonSuccess(object sender, LoggedOnEventArgs e)
        {
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
        }

        private void OnButtonFfClick(object sender, RoutedEventArgs e)
        {
            Tile tile = GetFirstPopulatedTile();

            tile.player.PlaySpeed = PlaySpeed.Speed1X;
            tile.player.ResumePlaying();
            tile.player.PlaySpeed = m_playSpeed;
            m_isRewinding = false;
            PlayPause = "Pause";
        }

        private void OnButtonLogOffClick(object sender, RoutedEventArgs e)
        {
            if (checkboxCloseStreams.IsChecked == true)
            {
                foreach (Tile tile in m_tiles)
                {
                    tile.StopTile();
                }
            }
            CheckIfShouldDisableCheckBox();
            m_sdkEngine.LoginManager.BeginLogOff();
        }

        private void OnCheckBoxHardwareAccelerationChanged(object sender, RoutedEventArgs e)
        {
            foreach (Tile tile in m_tiles.Where(tile => !tile.IsStarted()))
            {
                tile.player.HardwareAccelerationEnabled = checkboxEnableHardwareAcceleration.IsChecked == true;
            }
        }
        private void OnButtonLogonClick(object sender, RoutedEventArgs e)
        {
            if (checkboxWindowsCredentials.IsChecked == true)
            {
                m_sdkEngine.LoginManager.BeginLogOnUsingWindowsCredential(textboxDirectory.Text);
            }
            else
            {
                m_sdkEngine.LoginManager.BeginLogOn(textboxDirectory.Text, textboxUsername.Text, passwordBox.Password);
            }
        }

        private void OnPlayerFrameRendered(object sender, FrameRenderedEventArgs e)
        {
            TimeStamp = e.FrameTime.ToLocalTime().ToString("HH:mm:ss.ffff");
        }

        private void OnPlayerLivePlaybackModeToggled(object sender, EventArgs e)
        {
            IsInPlayback = !(PlayersPlayingLive() || GetFirstPopulatedTile() == null);
        }

        private void OnTileDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(Guid)))
            {
                Entity entity = m_sdkEngine.GetEntity((Guid)e.Data.GetData(typeof(Guid)));
                if ((entity != null) && (entity.EntityType == EntityType.Camera))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
            CheckIfShouldDisableCheckBox();
        }

        // NOTE : This sample is meant to demonstrate the possibility to use the WPF drag and drop feature as well as multiple _tiles to show video feeds.
        // It isn't meant to be a guide on how to use the WPF drag and drop. The implementation may therefore not be bulletproof.
        private void OnTileDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Guid)))
            {
                Entity entity = m_sdkEngine.GetEntity((Guid)e.Data.GetData(typeof(Guid)));
                if ((entity != null) && (entity.EntityType == EntityType.Camera))
                {
                    Tile tile = sender as Tile;
                    //Take the playing state of the cameras before adding one
                    bool wasLive = PlayersPlayingLive();
                    if (tile != null)
                    {
                        tile.StopTile();
                        tile.InitializeTile(entity, m_sdkEngine);
                        //If all _tiles are live or if we're adding the first camera
                        if (wasLive || GetFirstPopulatedTile() == null)
                        {
                            tile.player.PlayLive();
                        }
                        else
                        {
                            tile.player.AddToSynchronizedPlayback();
                        }
                    }
                }
            }
            CheckIfShouldDisableCheckBox();
            m_canDragDrop = false;
        }

        private void OnTileMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (m_selectedTile != null)
            {
                m_selectedTile.BorderBrush = Brushes.Black;
            }
            m_selectedTile = sender as Tile;
            if (m_selectedTile != null)
            {
                m_selectedTile.BorderBrush = Brushes.Red;
                m_canDragDrop = true;
            }
            CheckIfShouldDisableCheckBox();
        }

        private void OnTileMouseMove(object sender, MouseEventArgs e)
        {
            Tile tile = sender as Tile;
            if (tile != null && e.LeftButton == MouseButtonState.Pressed && m_canDragDrop)
            {
                Guid tileCopyGuid = tile.m_playerGuid;
                tile.StopTile();
                if (DragDrop.DoDragDrop(this, tileCopyGuid, DragDropEffects.Copy) == DragDropEffects.None)
                {
                    m_canDragDrop = false;
                }
            }
            CheckIfShouldDisableCheckBox();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                m_selectedTile.StopTile();
            }
            CheckIfShouldDisableCheckBox();
        }

        private void OnWindowMouseUp(object sender, MouseEventArgs e)
        {
            m_canDragDrop = false;
            tree.m_canDragDrop = false;
        }

        #endregion

        #region Private Methods

        private Tile GetFirstPopulatedTile()
        {
            return m_tiles.FirstOrDefault(tile => tile.IsStarted());
        }

        private bool PlayersPlayingLive()
        {
            if (m_tiles.Count(tile => tile.IsStarted()) == 0)
            {
                return false;
            }
            //Returns true if all initialized players are live
            return m_tiles.Where(tile => tile.IsStarted()).All(tile => tile.player.IsPlayingLiveStream);
        }

        #endregion
    }

    #endregion
}

