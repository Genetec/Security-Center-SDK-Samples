using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Media;
using System;
using System.Windows;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2015 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 30:
//  1860 – Britain's first tram service begins in Birkenhead, Merseyside.
//  1945 – The Bourne End rail crash, in Hertfordshire, England, kills 43
//  1949 – The Berlin Airlift ends.
// ==========================================================================
namespace VideoViewer
{
    #region Classes

    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        #region Constants

        public static readonly DependencyProperty BitRateProperty =
                            DependencyProperty.Register("BitRate", typeof(String), typeof(Tile), new PropertyMetadata("0 kbps"));

        public static readonly DependencyProperty CameraNameProperty =
                            DependencyProperty.Register("CameraName", typeof(String), typeof(Tile), new PropertyMetadata(""));

        public static readonly DependencyProperty IsStreamingProperty =
                            DependencyProperty.Register("IsStreaming", typeof(bool), typeof(Tile), new PropertyMetadata(false));

        #endregion

        #region Fields

        public Guid m_playerGuid;

        #endregion

        #region Properties

        public string BitRate
        {
            get { return (string)GetValue(BitRateProperty); }
            set { SetValue(BitRateProperty, value); }
        }

        public string CameraName
        {
            get { return (string)GetValue(CameraNameProperty); }
            set { SetValue(CameraNameProperty, value); }
        }

        public bool IsStreaming
        {
            get { return (bool)GetValue(IsStreamingProperty); }
            set { SetValue(IsStreamingProperty, value); }
        }

        #endregion

        #region Constructors

        public Tile()
        {
            InitializeComponent();
            DataContext = this;
            player.BitRateRefreshed += OnPlayerBitRateRefreshed;
            player.UnhandledException += OnMediaPlayerUnhandledException;
        }

        #endregion

        #region Event Handlers

        void OnMediaPlayerUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void OnPlayerBitRateRefreshed(object sender, BitRateRefreshedEventArgs e)
        {
            BitRate = (int)(player.BitRate / 1024) + " kbps";
        }

        #endregion

        #region Public Methods

        public void InitializeTile(Entity camera, Engine sdk)
        {
            player.Initialize(sdk, camera.Guid);
            m_playerGuid = camera.Guid;
            CameraName = camera.Name;
            IsStreaming = true;
        }

        // The LastRenderedFrameTime is up to date instantaneously whereas the IPSource takes time to get updated
        public bool IsStarted()
        {
            // The IpSource is checked because when switching from playback, the state passes by starting, it doesn't remain on playing.
            return (player.State == PlayerState.Playing) || (player.IpSource != null);
        }

        public void StopTile()
        {
            if (player.IsInitialized)
            {
                player.Stop();
            }
            m_playerGuid = Guid.Empty;
            IsStreaming = false;
        }

        #endregion
    }

    #endregion
}

