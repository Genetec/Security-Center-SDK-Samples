using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Genetec.Sdk;
using Genetec.Sdk.Entities.Video.MotionDetection;
using Genetec.Sdk.Media;
using MotionDetectionConfig.Dialogs;
using MotionDetectionConfig.MotionMap;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace MotionDetectionConfig
{
    #region Classes

    /// <summary>
    /// Interaction logic for MotionDetectionZoneConfigurationCtl.xaml
    /// </summary>
    public partial class MotionDetectionZoneConfigurationCtl : UserControl
    {
        #region Constants

        public static readonly DependencyProperty AreIrregularZoneShapeSupportedProperty =
                    DependencyProperty.Register
                    ("AreIrregularZoneShapeSupported", typeof(bool), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(false));

        public static readonly DependencyProperty IsMapConfigurationSupportedProperty =
                    DependencyProperty.Register
                    ("IsMapConfigurationSupported", typeof(bool), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(false));

        public static readonly DependencyProperty IsMotionOffThresholdSupportedProperty =
                    DependencyProperty.Register
                    ("IsMotionOffThresholdSupported", typeof(bool), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(false));

        public static readonly DependencyProperty IsMotionOnThresholdSupportedProperty =
                    DependencyProperty.Register
                    ("IsMotionOnThresholdSupported", typeof(bool), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(false));

        public static readonly DependencyProperty MotionOffThresholdBlocksProperty =
                    DependencyProperty.Register
                    ("MotionOffThresholdBlocks", typeof(int), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(0));

        public static readonly DependencyProperty MotionOffThresholdProperty =
                    DependencyProperty.Register
                    ("MotionOffThreshold", typeof(double), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(0.0, OnPropertyMotionOffThresholdChanged, OnPropertyMotionOffThresholdCoerce));

        public static readonly DependencyProperty MotionOnThresholdBlocksProperty =
                    DependencyProperty.Register
                    ("MotionOnThresholdBlocks", typeof(int), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(0));

        public static readonly DependencyProperty MotionOnThresholdProperty =
                    DependencyProperty.Register
                    ("MotionOnThreshold", typeof(double), typeof(MotionDetectionZoneConfigurationCtl),
                    new PropertyMetadata(0.0, OnPropertyMotionOnThresholdChanged, OnPropertyMotionOnThresholdCoerce));

        #endregion

        #region Fields

        /// <summary>
        /// The capabilities indicating what is supported by this camera in the motion detection configuration
        /// </summary>
        private MotionDetectionCapabilities m_capabilities;

        /// <summary>
        /// The motion detection configuration
        /// </summary>
        private IMotionDetectionConfiguration m_motionDetectionConfiguration;

        /// <summary>
        /// Value representing the height of the map in macroblocks (the vertical number macro blocks)
        /// </summary>
        private int m_motionMapHeight;

        /// <summary>
        /// Value representing the height of a macro block
        /// </summary>
        private int m_motionMapMacroBlockHeight;

        /// <summary>
        /// Value representing the width of a macro block
        /// </summary>
        private int m_motionMapMacroBlockWidth;

        /// <summary>
        /// Value representing the outlining of the area in which to detect motion
        /// </summary>
        private BitArray m_motionMapValues;

        /// <summary>
        /// Value representing the width of the map in macroblocks (the horizontal number of macro blocks)
        /// </summary>
        private int m_motionMapWidth;

        /// <summary>
        /// Value representing the id of the motion off event
        /// </summary>
        private int m_motionOffEvent;

        /// <summary>
        /// Value representing the id of the motion off event
        /// </summary>
        private int m_motionOnEvent;

        /// <summary>
        /// The instance of the SDK Engine
        /// </summary>
        private Engine m_sdkEngine;

        /// <summary>
        /// The motion detection zone configuration represented by this control
        /// </summary>
        private IMotionDetectionZoneConfiguration m_zoneConfiguration;

        #endregion

        #region Properties

        /// <summary>
        /// Flag indicating if only rectangular zones are supported
        /// </summary>
        private bool AreIrregularZoneShapeSupported
        {
            get { return (bool)GetValue(AreIrregularZoneShapeSupportedProperty); }
            set { SetValue(AreIrregularZoneShapeSupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the configuration of the motion map is supported
        /// </summary>
        private bool IsMapConfigurationSupported
        {
            get { return (bool)GetValue(IsMapConfigurationSupportedProperty); }
            set { SetValue(IsMapConfigurationSupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the configuration of the motion off threshold is supported
        /// </summary>
        private bool IsMotionOffThresholdSupported
        {
            get { return (bool)GetValue(IsMotionOffThresholdSupportedProperty); }
            set { SetValue(IsMotionOffThresholdSupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the configuration of the motion on threshold is supported
        /// </summary>
        private bool IsMotionOnThresholdSupported
        {
            get { return (bool)GetValue(IsMotionOnThresholdSupportedProperty); }
            set { SetValue(IsMotionOnThresholdSupportedProperty, value); }
        }

        /// <summary>
        /// Value representing the percentage of motion that is needed to trigger the Motion off event.
        /// Should be lower than the motion on threshold
        /// </summary>
        private double MotionOffThreshold
        {
            get { return (double)GetValue(MotionOffThresholdProperty); }
            set { SetValue(MotionOffThresholdProperty, value); }
        }

        /// <summary>
        /// Value representing the motion off threshold in motion blocks
        /// </summary>
        private int MotionOffThresholdBlocks
        {
            get { return (int)GetValue(MotionOffThresholdBlocksProperty); }
            set { SetValue(MotionOffThresholdBlocksProperty, value); }
        }

        /// <summary>
        /// Value representing representing the percentage of motion that is needed to trigger the Motion on event.
        /// Should be higher than the motion off threshold
        /// </summary>
        private double MotionOnThreshold
        {
            get { return (double)GetValue(MotionOnThresholdProperty); }
            set { SetValue(MotionOnThresholdProperty, value); }
        }

        /// <summary>
        /// Value representing the motion on threshold in motion blocks
        /// </summary>
        private int MotionOnThresholdBlocks
        {
            get { return (int)GetValue(MotionOnThresholdBlocksProperty); }
            set { SetValue(MotionOnThresholdBlocksProperty, value); }
        }

        #endregion

        #region Constructors

        public MotionDetectionZoneConfigurationCtl()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private static void OnPropertyMotionOffThresholdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MotionDetectionZoneConfigurationCtl instance = d as MotionDetectionZoneConfigurationCtl;
            if (instance != null)
            {
                instance.OnMotionOffThresholdChanged();
            }
        }

        private static void OnPropertyMotionOnThresholdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MotionDetectionZoneConfigurationCtl instance = d as MotionDetectionZoneConfigurationCtl;
            if (instance != null)
            {
                instance.OnMotionOnThresholdChanged();
            }
        }

        private void OnButtonClearClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.ClearMask();
        }

        private void OnButtonEraserClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.DrawingMode = VideoMotionMapDrawingMode.Eraser;
            UpdateDrawingTools();
        }

        private void OnButtonFillClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.FillMask();
        }

        private void OnButtonInvertClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.InvertMask();
        }

        private void OnButtonMotionEventsClicked(object sender, RoutedEventArgs e)
        {
            //Display the Motion events dialog and initialize it with the sdk engine and motion event values from the configuration
            MotionEventsDlg dlg = new MotionEventsDlg {Owner = Window.GetWindow(this)};
            dlg.Initialize(m_sdkEngine, m_motionOnEvent, m_motionOffEvent);

            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                //If the user selected motion on and/or off events, take the new values
                m_motionOnEvent = dlg.MotionOnEvent;
                m_motionOffEvent = dlg.MotionOffEvent;
            }
        }

        private void OnButtonPenClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.DrawingMode = VideoMotionMapDrawingMode.Pen;
            UpdateDrawingTools();
        }

        private void OnButtonRectangleClicked(object sender, RoutedEventArgs e)
        {
            m_videoMotionMapCtl.DrawingMode = VideoMotionMapDrawingMode.Rectangle;
            UpdateDrawingTools();
        }

        private void OnMotionOffThresholdChanged()
        {
            MotionOffThresholdBlocks = m_zoneConfiguration.GetMotionThresholdInBlocks(MotionOffThreshold);
        }

        private void OnMotionOnThresholdChanged()
        {
            MotionOnThresholdBlocks = m_zoneConfiguration.GetMotionThresholdInBlocks(MotionOnThreshold);
        }

        private void OnVideoMotionMapMotionMaskChanged(object sender, MotionMaskChangedEventArgs e)
        {
            MotionMap.MotionMap mask = m_videoMotionMapCtl.Mask;
            int blocksWidth = (int)m_videoMotionMapCtl.ActualMaskWidth / mask.Width;
            int blocksHeight = (int)m_videoMotionMapCtl.ActualMaskHeight / mask.Height;

            m_motionMapHeight = mask.Height;
            m_motionMapWidth = mask.Width;
            m_motionMapMacroBlockHeight = blocksHeight;
            m_motionMapMacroBlockWidth = blocksWidth;
            m_motionMapValues = new BitArray(mask.Values);
        }

        #endregion

        #region Public Methods

        public void HideVideo()
        {
            m_videoMotionMapCtl.HideVideo();
        }

        /// <summary>
        /// Initializes the control with the right configurations
        /// </summary>
        /// <param name="sdkEngine">The instance of the SDK Engine</param>
        /// <param name="configuration">The motion detection configuration</param>
        /// <param name="zoneConfiguration">The motion detection zone configuration</param>
        /// <param name="capabilities">The motion detection capabilities</param>
        /// <param name="mediaPlayer">The instance of the media player used to display the video of the camera</param>
        public void Initialize(Engine sdkEngine, IMotionDetectionConfiguration configuration, IMotionDetectionZoneConfiguration zoneConfiguration, MotionDetectionCapabilities capabilities, MediaPlayer mediaPlayer)
        {
            m_motionDetectionConfiguration = configuration;
            m_zoneConfiguration = zoneConfiguration;
            m_capabilities = capabilities;
            m_sdkEngine = sdkEngine;

            mediaPlayer.HardwareAccelerationEnabled = true;
            m_videoMotionMapCtl.Initialize(mediaPlayer);
            m_videoMotionMapCtl.SetMask(zoneConfiguration);
            m_videoMotionMapCtl.MotionMaskChanged += OnVideoMotionMapMotionMaskChanged;

            UpdateConfig();
            UpdateCapabilities();
        }

        /// <summary>
        /// Copy the configured values in the motion detection zone configuration
        /// </summary>
        public void Save()
        {
            try
            {
                m_zoneConfiguration.MotionOffThreshold = MotionOffThreshold;
                m_zoneConfiguration.MotionOnThreshold = MotionOnThreshold;
                m_zoneConfiguration.Map = new BitArray(m_motionMapValues);
                m_zoneConfiguration.MapHeight = m_motionMapHeight;
                m_zoneConfiguration.MapWidth = m_motionMapWidth;
                m_zoneConfiguration.MacroBlockHeight = m_motionMapMacroBlockHeight;
                m_zoneConfiguration.MacroBlockWidth = m_motionMapMacroBlockWidth;
                m_zoneConfiguration.MotionOnEvent = m_motionOnEvent;
                m_zoneConfiguration.MotionOffEvent = m_motionOffEvent;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ShowVideo()
        {
            m_videoMotionMapCtl.ShowVideo();
        }

        /// <summary>
        /// Update the capabilities depending on the capabilites of the camera and the motion detection type
        /// </summary>
        public void UpdateCapabilities()
        {
            if (m_motionDetectionConfiguration.MotionDetectionType == MotionDetectionType.Archiver)
            {
                AreIrregularZoneShapeSupported = true;
                IsMapConfigurationSupported = true;
                IsMotionOffThresholdSupported = true;
                IsMotionOnThresholdSupported = true;
            }
            else if (m_motionDetectionConfiguration.MotionDetectionType == MotionDetectionType.Unit)
            {
                AreIrregularZoneShapeSupported = m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareIrregularZoneShapeSupported);

                IsMapConfigurationSupported = m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareDetectionZoneSupported);

                IsMotionOffThresholdSupported = !m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareDetectionThresholdOffDisabled);

                IsMotionOnThresholdSupported = !m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareDetectionThresholdDisabled);
            }

            if (!AreIrregularZoneShapeSupported)
            {
                m_videoMotionMapCtl.DrawingMode = VideoMotionMapDrawingMode.Rectangle;
                UpdateDrawingTools();
            }
        }

        #endregion

        #region Private Methods

        private static object OnPropertyMotionOffThresholdCoerce(DependencyObject d, object baseValue)
        {
            MotionDetectionZoneConfigurationCtl instance = d as MotionDetectionZoneConfigurationCtl;
            if (instance != null)
            {
                return instance.OnMotionOffThresholdCoerce(baseValue);
            }

            return baseValue;
        }

        private static object OnPropertyMotionOnThresholdCoerce(DependencyObject d, object baseValue)
        {
            MotionDetectionZoneConfigurationCtl instance = d as MotionDetectionZoneConfigurationCtl;
            if (instance != null)
            {
                return instance.OnMotionOnThresholdCoerce(baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// Coerce the motion off threshold value to make sure it is between 0 and 100
        /// </summary>
        private object OnMotionOffThresholdCoerce(object baseValue)
        {
            if (baseValue is double)
            {
                double motionOffThreshold = (double)baseValue;
                if ((motionOffThreshold >= 0.0) && (motionOffThreshold <= 100.0))
                {
                    return motionOffThreshold;
                }
            }

            return MotionOffThreshold;
        }

        /// <summary>
        /// Coerce the motion on threshold value to make sure it is between 0 and 100
        /// </summary>
        private object OnMotionOnThresholdCoerce(object baseValue)
        {
            if (baseValue is double)
            {
                double motionOnThreshold = (double)baseValue;
                if ((motionOnThreshold >= 0.0) && (motionOnThreshold <= 100.0))
                {
                    return motionOnThreshold;
                }
            }

            return MotionOnThreshold;
        }

        private void UpdateConfig()
        {
            m_motionMapHeight = m_zoneConfiguration.MapHeight;
            m_motionMapWidth = m_zoneConfiguration.MapWidth;
            m_motionMapMacroBlockHeight = m_zoneConfiguration.MacroBlockHeight;
            m_motionMapMacroBlockWidth = m_zoneConfiguration.MacroBlockWidth;
            m_motionMapValues = m_zoneConfiguration.Map;
            m_motionOffEvent = m_zoneConfiguration.MotionOffEvent;
            m_motionOnEvent = m_zoneConfiguration.MotionOnEvent;
            MotionOffThreshold = m_zoneConfiguration.MotionOffThreshold;
            MotionOnThreshold = m_zoneConfiguration.MotionOnThreshold;
        }

        private void UpdateDrawingTools()
        {
            m_togglePen.IsChecked = (m_videoMotionMapCtl.DrawingMode == VideoMotionMapDrawingMode.Pen);
            m_toggleEraser.IsChecked = (m_videoMotionMapCtl.DrawingMode == VideoMotionMapDrawingMode.Eraser);
            m_toggleRectangle.IsChecked = (m_videoMotionMapCtl.DrawingMode == VideoMotionMapDrawingMode.Rectangle);
        }

        #endregion
    }

    /// <summary>
    /// Converter used to format the block count into the desired string
    /// </summary>
    sealed internal class BlockCountToStringConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format("{0} blocks", value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    #endregion
}

