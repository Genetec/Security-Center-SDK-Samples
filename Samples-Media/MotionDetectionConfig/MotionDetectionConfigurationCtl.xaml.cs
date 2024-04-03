using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Genetec.Sdk;
using Genetec.Sdk.Entities.Video.MotionDetection;
using Genetec.Sdk.Media;
using MotionDetectionConfig.Dialogs;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for June 22:
//  1898 – Spanish–American War: United States Marines land in Cuba.
//  1945 – The Battle of Okinawa comes to an end.
//  1990 – Checkpoint Charlie is dismantled in Berlin.
// ==========================================================================
namespace MotionDetectionConfig
{
    #region Classes

    /// <summary>
    /// Interaction logic for MotionDetectionConfigurationCtl.xaml
    /// </summary>
    public partial class MotionDetectionConfigurationCtl : UserControl
    {
        #region Constants

        public static readonly DependencyProperty ConsecutiveFrameHitsProperty =
                            DependencyProperty.Register
                            ("ConsecutiveFrameHits", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(1, null, OnPropertyConsecutiveFrameHitsCoerce));

        public static readonly DependencyProperty H264ChromaWeightProperty =
                            DependencyProperty.Register
                            ("H264ChromaWeight", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(0));

        public static readonly DependencyProperty H264LumaWeightProperty =
                            DependencyProperty.Register
                            ("H264LumaWeight", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(0));

        public static readonly DependencyProperty H264MacroblockWeightProperty =
                            DependencyProperty.Register
                            ("H264MacroblockWeight", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(0));

        public static readonly DependencyProperty H264VectorWeightProperty =
                            DependencyProperty.Register
                            ("H264VectorWeight", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(0));

        public static readonly DependencyProperty IsConsecutiveFrameHitsSupportedProperty =
                            DependencyProperty.Register
                            ("IsConsecutiveFrameHitsSupported", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(false));

        public static readonly DependencyProperty IsH264Property =
                            DependencyProperty.Register
                            ("IsH264", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(false));

        public static readonly DependencyProperty IsHardwareDetectionSupportedProperty =
                            DependencyProperty.Register
                            ("IsHardwareDetectionSupported", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(false));

        public static readonly DependencyProperty IsMotionDetectionEnabledProperty =
                            DependencyProperty.Register
                            ("IsMotionDetectionEnabled", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(true));

        public static readonly DependencyProperty IsSensitivitySupportedProperty =
                            DependencyProperty.Register
                            ("IsSensitivitySupported", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(false));

        public static readonly DependencyProperty IsSoftwareDetectionSupportedProperty =
                            DependencyProperty.Register
                            ("IsSoftwareDetectionSupported", typeof(bool), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(false));

        public static readonly DependencyProperty MaxZoneCountSupportedProperty =
                            DependencyProperty.Register
                            ("MaxZoneCountSupported", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(0));

        public static readonly DependencyProperty MotionDetectionTypeProperty =
                            DependencyProperty.Register
                            ("MotionDetectionType", typeof(MotionDetectionType), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(MotionDetectionType.Archiver, OnPropertyMotionDetectionTypeChanged));

        public static readonly DependencyProperty SelectedZoneConfigurationCtlProperty =
                            DependencyProperty.Register
                            ("SelectedZoneConfigurationCtl", typeof(MotionDetectionZoneConfigurationCtl), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(null));

        public static readonly DependencyProperty SensitivityProperty =
                            DependencyProperty.Register
                            ("Sensitivity", typeof(int), typeof(MotionDetectionConfigurationCtl),
                            new PropertyMetadata(100, null, OnPropertySensitivityCoerce));

        public static readonly DependencyProperty TotalTabsProperty = 
                            DependencyProperty.Register(
                            "TotalTabs", typeof(int), typeof(MotionDetectionConfigurationCtl), 
                            new PropertyMetadata(default(int)));

        /// <summary>
        /// Mapping of the different MotionDetectionZoneConfigurationCtls to the configuration they represent
        /// </summary>
        private readonly Dictionary<MotionDetectionZoneConfigurationCtl, IMotionDetectionZoneConfiguration> m_zoneCtlConfigMappings = new Dictionary<MotionDetectionZoneConfigurationCtl, IMotionDetectionZoneConfiguration>();

        #endregion

        #region Fields

        /// <summary>
        /// The capabilities indicating what is supported by this camera in the motion detection configuration
        /// </summary>
        private MotionDetectionCapabilities m_capabilities;

        /// <summary>
        /// The motion detection configuration represented by this control
        /// </summary>
        private IMotionDetectionConfiguration m_configuration;

        /// <summary>
        /// The instance of the media player used to display the video of the camera and configure the motion detection map
        /// </summary>
        private MediaPlayer m_mediaPlayer;

        /// <summary>
        /// The instance of the SDK Engine we are using to connect with and query the server
        /// </summary>
        private Engine m_sdkEngine;

        /// <summary>
        /// The currently selected item of the tab control displaying the different motion detection zone configurations
        /// </summary>
        private TabItem m_selectedTabItem;

        #endregion

        #region Properties

        public int TotalTabs
        {
            get { return (int)GetValue(TotalTabsProperty); }
            set { SetValue(TotalTabsProperty, value); }
        }

        /// <summary>
        /// Value representing the number of consecutive frames with motion higher than the motion on threshold
        /// that are needed for motion detection to be considered valid
        /// </summary>
        private int ConsecutiveFrameHits
        {
            get { return (int)GetValue(ConsecutiveFrameHitsProperty); }
            set { SetValue(ConsecutiveFrameHitsProperty, value); }
        }

        /// <summary>
        /// Value representing how the difference in chroma (color) values between consecutive frames affects motion detection
        /// </summary>
        private int H264ChromaWeight
        {
            get { return (int)GetValue(H264ChromaWeightProperty); }
            set { SetValue(H264ChromaWeightProperty, value); }
        }

        /// <summary>
        /// Value representing how the difference in luma (brightness) values between consecutive frames affects motion detection
        /// </summary>
        private int H264LumaWeight
        {
            get { return (int)GetValue(H264LumaWeightProperty); }
            set { SetValue(H264LumaWeightProperty, value); }
        }

        /// <summary>
        /// Value representing how macroblocks (intra-macroblocks) affects motion detection
        /// </summary>
        private int H264MacroblockWeight
        {
            get { return (int)GetValue(H264MacroblockWeightProperty); }
            set { SetValue(H264MacroblockWeightProperty, value); }
        }

        /// <summary>
        /// Value representing how the difference in vector (movement) values between consecutive frames affects motion detection
        /// </summary>
        private int H264VectorWeight
        {
            get { return (int)GetValue(H264VectorWeightProperty); }
            set { SetValue(H264VectorWeightProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the configuration of consecurtive frame hits is supported
        /// </summary>
        private bool IsConsecutiveFrameHitsSupported
        {
            get { return (bool)GetValue(IsConsecutiveFrameHitsSupportedProperty); }
            set { SetValue(IsConsecutiveFrameHitsSupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the current stream is an H264 stream and if the H264 specific settings are to be used
        /// </summary>
        private bool IsH264
        {
            get { return (bool)GetValue(IsH264Property); }
            set { SetValue(IsH264Property, value); }
        }

        /// <summary>
        /// Flag indicating if hardware (Unit) motion detection is supported
        /// </summary>
        private bool IsHardwareDetectionSupported
        {
            get { return (bool)GetValue(IsHardwareDetectionSupportedProperty); }
            set { SetValue(IsHardwareDetectionSupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the motion detection is enabled
        /// </summary>
        private bool IsMotionDetectionEnabled
        {
            get { return (bool)GetValue(IsMotionDetectionEnabledProperty); }
            set { SetValue(IsMotionDetectionEnabledProperty, value); }
        }

        /// <summary>
        /// Flag indicating if the configuration of the sensitivity is supported
        /// </summary>
        private bool IsSensitivitySupported
        {
            get { return (bool)GetValue(IsSensitivitySupportedProperty); }
            set { SetValue(IsSensitivitySupportedProperty, value); }
        }

        /// <summary>
        /// Flag indicating if software (Archiver) motion detection is supported
        /// </summary>
        private bool IsSoftwareDetectionSupported
        {
            get { return (bool)GetValue(IsSoftwareDetectionSupportedProperty); }
            set { SetValue(IsSoftwareDetectionSupportedProperty, value); }
        }

        /// <summary>
        /// Value representing the maximum number of different motion detection zones that are supported (can change with the MotionDetectionType)
        /// </summary>
        private int MaxZoneCountSupported
        {
            get { return (int)GetValue(MaxZoneCountSupportedProperty); }
            set { SetValue(MaxZoneCountSupportedProperty, value); }
        }

        /// <summary>
        /// Value representing whether the motion detection is made on the Archiver (software) or the Unit (hardware)
        /// </summary>
        private MotionDetectionType MotionDetectionType
        {
            get { return (MotionDetectionType)GetValue(MotionDetectionTypeProperty); }
            set { SetValue(MotionDetectionTypeProperty, value); }
        }

        /// <summary>
        /// The motion zone configuration control of the currently selected tab
        /// </summary>
        private MotionDetectionZoneConfigurationCtl SelectedZoneConfigurationCtl
        {
            get { return (MotionDetectionZoneConfigurationCtl)GetValue(SelectedZoneConfigurationCtlProperty); }
            set { SetValue(SelectedZoneConfigurationCtlProperty, value); }
        }

        /// <summary>
        /// Value representing the sensitivity of the motion detection
        /// </summary>
        private int Sensitivity
        {
            get { return (int)GetValue(SensitivityProperty); }
            set { SetValue(SensitivityProperty, value); }
        }

        #endregion

        #region Constructors

        public MotionDetectionConfigurationCtl()
        {
            InitializeComponent();

            //WPF RadioButtons do not support binding if there are multiple instances of the same control using the same GroupName
            //So giving the GroupName a guid solves this issue
            m_rbSoftware.GroupName = Guid.NewGuid().ToString();
            m_rbHardware.GroupName = Guid.NewGuid().ToString();
        }

        #endregion

        #region Event Handlers

        private static void OnPropertyMotionDetectionTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MotionDetectionConfigurationCtl instance = d as MotionDetectionConfigurationCtl;
            if (instance != null)
            {
                instance.OnMotionDetectionTypeChanged();
            }
        }

        private void OnButtonAddMotionZoneClicked(object sender, RoutedEventArgs e)
        {
            //Create a new motion detection zone configuration and add an item in the tab control to represent it
            TabItem item = new TabItem {Header = string.Format("Motion zone {0}", m_zoneCtlConfigMappings.Count + 1)};
            MotionDetectionZoneConfigurationCtl zoneConfigurationCtl = new MotionDetectionZoneConfigurationCtl();
            IMotionDetectionZoneConfiguration zoneConfiguration = MotionDetectionConfigurationBuilder.CreateMotionDetectionZoneConfigurationInstance();
            zoneConfigurationCtl.Initialize(m_sdkEngine, m_configuration, zoneConfiguration, m_capabilities, m_mediaPlayer);
            item.Content = zoneConfigurationCtl;
            m_zoneCtlConfigMappings.Add(zoneConfigurationCtl, zoneConfiguration);
            m_tabControl.Items.Add(item);

            TotalTabs = m_tabControl.Items.Count;

            //Select the tab of the new zone configuration
            m_tabControl.SelectedIndex = m_tabControl.Items.Count - 1;
        }

        private void OnButtonH264SettingsClicked(object sender, RoutedEventArgs e)
        {
            //Display the H264 settings dialog and initialize it with the values from the configuration
            H264SettingsDlg dlg = new H264SettingsDlg();
            dlg.Owner = Window.GetWindow(this);
            dlg.Initialize(H264LumaWeight, H264ChromaWeight, H264VectorWeight, H264MacroblockWeight);

            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                //IF the user pressed selected new values, use them
                H264LumaWeight = dlg.LumaWeight;
                H264ChromaWeight = dlg.ChromaWeight;
                H264VectorWeight = dlg.VectorWeight;
                H264MacroblockWeight = dlg.MacroblockWeight;
            }
        }

        private void OnButtonRemoveMotionZoneClicked(object sender, RoutedEventArgs e)
        {
            //Remove the motion zone of the currently selected tab from the config
            m_zoneCtlConfigMappings.Remove(SelectedZoneConfigurationCtl);
            m_tabControl.Items.Remove(m_selectedTabItem);
            m_configuration.UpdateZones(m_zoneCtlConfigMappings.Values);
            TotalTabs = m_tabControl.Items.Count;
        }

        private void OnMotionDetectionTypeChanged()
        {
            //Since some capabilities are different wheter motion detection is done on the Unit or the Archiver, update the capabilities
            UpdateCapabilities();
            foreach (MotionDetectionZoneConfigurationCtl zoneConfigurationCtl in m_zoneCtlConfigMappings.Keys)
            {
                zoneConfigurationCtl.UpdateCapabilities();
            }
        }

        private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                TabItem item = e.RemovedItems[0] as TabItem;
                if (item != null)
                {
                    m_selectedTabItem = item;
                    MotionDetectionZoneConfigurationCtl zoneConfigCtl = item.Content as MotionDetectionZoneConfigurationCtl;
                    if (zoneConfigCtl != null)
                    {
                        //Hide the video from the last selected tab
                        zoneConfigCtl.HideVideo();
                        SelectedZoneConfigurationCtl = null;
                    }
                }
            }
            if (e.AddedItems.Count > 0)
            {
                TabItem item = e.AddedItems[0] as TabItem;
                if (item != null)
                {
                    m_selectedTabItem = item;
                    MotionDetectionZoneConfigurationCtl zoneConfigCtl = item.Content as MotionDetectionZoneConfigurationCtl;
                    if (zoneConfigCtl != null)
                    {
                        //Show the video in the new selected tab
                        zoneConfigCtl.ShowVideo();
                        SelectedZoneConfigurationCtl = zoneConfigCtl;
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void HideVideo()
        {
            if (SelectedZoneConfigurationCtl != null)
            {
                SelectedZoneConfigurationCtl.HideVideo();
            }
        }

        /// <summary>
        /// Initializes the control with the right configurations
        /// </summary>
        /// <param name="sdkEngine">The instance of the SDK Engine</param>
        /// <param name="motionDetectionConfiguration">The configuration represented by this control</param>
        /// <param name="motionDetectionCapabilities">The motion detection capabilities</param>
        /// <param name="mediaPlayer">The instance of the media player used to display the video of the camera</param>
        public void Initialize(Engine sdkEngine, IMotionDetectionConfiguration motionDetectionConfiguration, MotionDetectionCapabilities motionDetectionCapabilities, MediaPlayer mediaPlayer)
        {
            m_configuration = motionDetectionConfiguration;
            m_capabilities = motionDetectionCapabilities;
            m_mediaPlayer = mediaPlayer;
            m_mediaPlayer.HardwareAccelerationEnabled = true;
            m_sdkEngine = sdkEngine;

            UpdateConfig();
            UpdateCapabilities();
        }

        /// <summary>
        /// Copy the configured values in the motion detection configuration
        /// </summary>
        public void Save()
        {
            try
            {
                m_configuration.ConsecutiveFrameHits = ConsecutiveFrameHits;
                m_configuration.H264ChromaWeight = H264ChromaWeight;
                m_configuration.H264LumaWeight = H264LumaWeight;
                m_configuration.H264MacroblockWeight = H264MacroblockWeight;
                m_configuration.H264VectorWeight = H264VectorWeight;
                m_configuration.IsEnabled = IsMotionDetectionEnabled;
                m_configuration.MotionDetectionType = MotionDetectionType;
                m_configuration.Sensitivity = Sensitivity;

                List<IMotionDetectionZoneConfiguration> zones = new List<IMotionDetectionZoneConfiguration>();
                foreach (KeyValuePair<MotionDetectionZoneConfigurationCtl, IMotionDetectionZoneConfiguration> item in m_zoneCtlConfigMappings)
                {
                    //Also copy the values of the zone configurations
                    item.Key.Save();
                    zones.Add(item.Value);
                }

                m_configuration.UpdateZones(zones);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ShowVideo()
        {
            if (SelectedZoneConfigurationCtl != null)
            {
                SelectedZoneConfigurationCtl.ShowVideo();
            }
        }

        #endregion

        #region Private Methods

        private static object OnPropertyConsecutiveFrameHitsCoerce(DependencyObject d, object baseValue)
        {
            MotionDetectionConfigurationCtl instance = d as MotionDetectionConfigurationCtl;
            if (instance != null)
            {
                return instance.OnConsecutiveFrameHitsCoerce(baseValue);
            }

            return baseValue;
        }

        private static object OnPropertySensitivityCoerce(DependencyObject d, object baseValue)
        {
            MotionDetectionConfigurationCtl instance = d as MotionDetectionConfigurationCtl;
            if (instance != null)
            {
                return instance.OnSensitivityCoerce(baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// Coerce to make sure the value is between 1 and 127
        /// </summary>
        private object OnConsecutiveFrameHitsCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int consecutiveFrameHits = (int)baseValue;
                if ((consecutiveFrameHits > 0) && (consecutiveFrameHits < 128))
                {
                    return consecutiveFrameHits;
                }
            }

            return ConsecutiveFrameHits;
        }

        /// <summary>
        /// Coerce to make sure the value is between 0 and 100
        /// </summary>
        private object OnSensitivityCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int sensitivity = (int)baseValue;
                if ((sensitivity > -1) && (sensitivity < 101))
                {
                    return sensitivity;
                }
            }

            return Sensitivity;
        }

        /// <summary>
        /// Update the capabilities depending on the capabilites of the camera and the motion detection type
        /// </summary>
        private void UpdateCapabilities()
        {
            IsHardwareDetectionSupported = m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareDetectionSupported);
            IsSensitivitySupported = !m_capabilities.HasFlag(MotionDetectionCapabilities.MotionDetectionSensitivityDisabled);
            IsSoftwareDetectionSupported = m_capabilities.HasFlag(MotionDetectionCapabilities.SoftwareDetectionSupported);

            if (MotionDetectionType == MotionDetectionType.Archiver)
            {
                IsConsecutiveFrameHitsSupported = true;
                MaxZoneCountSupported = m_configuration.MaximumZoneCount;
            }
            else if (MotionDetectionType == MotionDetectionType.Unit)
            {
                IsConsecutiveFrameHitsSupported = !m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareDetectionConsecutiveFrameHitDisabled);
                MaxZoneCountSupported = m_capabilities.HasFlag(MotionDetectionCapabilities.HardwareMultipleZoneSupported) ? m_configuration.MaximumZoneCount : 1;
            }

            if (!IsHardwareDetectionSupported)
            {
                MotionDetectionType = MotionDetectionType.Archiver;
            }

            if (!IsSoftwareDetectionSupported)
            {
                MotionDetectionType = MotionDetectionType.Unit;
            }
        }

        /// <summary>
        /// Update the config with the values from the motion detection configuration
        /// </summary>
        private void UpdateConfig()
        {
            ConsecutiveFrameHits = m_configuration.ConsecutiveFrameHits;
            H264ChromaWeight = m_configuration.H264ChromaWeight;
            H264LumaWeight = m_configuration.H264LumaWeight;
            H264MacroblockWeight = m_configuration.H264MacroblockWeight;
            H264VectorWeight = m_configuration.H264VectorWeight;
            IsH264 = m_configuration.IsH264;
            IsMotionDetectionEnabled = m_configuration.IsEnabled;
            MotionDetectionType = m_configuration.MotionDetectionType;
            Sensitivity = m_configuration.Sensitivity;

            m_tabControl.Items.Clear();
            foreach (IMotionDetectionZoneConfiguration zoneConfiguration in m_configuration.Zones)
            {
                //Create a control to represent the configuration of each motion detection zone
                TabItem item = new TabItem
                {
                    Header = string.Format("Motion zone {0}", m_zoneCtlConfigMappings.Count + 1)
                };
                MotionDetectionZoneConfigurationCtl zoneConfigurationCtl = new MotionDetectionZoneConfigurationCtl();
                zoneConfigurationCtl.Initialize(m_sdkEngine, m_configuration, zoneConfiguration, m_capabilities, m_mediaPlayer);
                item.Content = zoneConfigurationCtl;
                m_zoneCtlConfigMappings.Add(zoneConfigurationCtl, zoneConfiguration);
                m_tabControl.Items.Add(item);
            }

            TotalTabs = m_tabControl.Items.Count;

            //Select the first tab
            if (m_tabControl.Items.Count > 0)
            {
                m_tabControl.SelectedIndex = 0;
            }
        }

        #endregion
    }

    #endregion
}

