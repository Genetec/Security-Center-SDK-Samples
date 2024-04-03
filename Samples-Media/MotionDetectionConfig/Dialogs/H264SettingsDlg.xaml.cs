using System.Windows;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2012 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for November 9:
//  1857 – The Atlantic is founded in Boston.
//  1914 – SMS&#160;Emden is sunk by HMAS&#160;Sydney in the Battle of Cocos.
//  1953 – Cambodia becomes independent from France.
// ==========================================================================
namespace MotionDetectionConfig.Dialogs
{
    #region Classes

    /// <summary>
    /// Interaction logic for H264SettingsDlg.xaml
    /// </summary>
    public partial class H264SettingsDlg : Window
    {
        #region Constants

        public static readonly DependencyProperty ChromaWeightProperty =
            DependencyProperty.Register
            ("ChromaWeight", typeof(int), typeof(H264SettingsDlg),
            new PropertyMetadata(0, OnPropertyChromaWeightChanged, OnPropertyChromaWeightCoerce));

        public static readonly DependencyProperty LumaWeightProperty =
            DependencyProperty.Register
            ("LumaWeight", typeof(int), typeof(H264SettingsDlg),
            new PropertyMetadata(0, OnPropertyLumaWeightChanged, OnPropertyLumaWeightCoerce));

        public static readonly DependencyProperty MacroblockWeightProperty =
            DependencyProperty.Register
            ("MacroblockWeight", typeof(int), typeof(H264SettingsDlg),
            new PropertyMetadata(0, OnPropertyMacroblockWeightChanged, OnPropertyMacroblockWeightCoerce));

        public static readonly DependencyProperty VectorWeightProperty =
            DependencyProperty.Register
            ("VectorWeight", typeof(int), typeof(H264SettingsDlg),
            new PropertyMetadata(0, OnPropertyVectorWeightChanged, OnPropertyVectorWeightCoerce));

        private const int LumaPresetChromaWeight = 0;

        private const int LumaPresetLumaWeight = 30;

        private const int LumaPresetMacroblockWeight = 0;

        private const int LumaPresetVectorWeight = 10;

        private const int VectorPresetChromaWeight = 0;

        private const int VectorPresetLumaWeight = 10;

        private const int VectorPresetMacroblockWeight = 10;

        private const int VectorPresetVectorWeight = 50;

        #endregion

        #region Fields

        /// <summary>
        /// Flag indicating that we want to ignore the changes and not update the presets
        /// </summary>
        private bool m_ignoreUpdatePresets;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value representing how the difference in chroma (color) values between consecutive frames affects motion detection
        /// </summary>
        public int ChromaWeight
        {
            get { return (int)GetValue(ChromaWeightProperty); }
            private set { SetValue(ChromaWeightProperty, value); }
        }

        /// <summary>
        /// Gets a value representing how the difference in luma (brightness) values between consecutive frames affects motion detection
        /// </summary>
        public int LumaWeight
        {
            get { return (int)GetValue(LumaWeightProperty); }
            private set { SetValue(LumaWeightProperty, value); }
        }

        /// <summary>
        /// Gets a value representing how the macroblocks (intra-macroblocks) affects motion detection
        /// </summary>
        public int MacroblockWeight
        {
            get { return (int)GetValue(MacroblockWeightProperty); }
            private set { SetValue(MacroblockWeightProperty, value); }
        }

        /// <summary>
        /// Gets a value representing how the difference in vector (movement) values between consecutive frames affects motion detection
        /// </summary>
        public int VectorWeight
        {
            get { return (int)GetValue(VectorWeightProperty); }
            private set { SetValue(VectorWeightProperty, value); }
        }

        #endregion

        #region Constructors

        public H264SettingsDlg()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private static void OnPropertyChromaWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                // When a setting changes, update the preset accordingly
                instance.UpdatePreset();
            }
        }

        private static void OnPropertyLumaWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                // When a setting changes, update the preset accordingly
                instance.UpdatePreset();
            }
        }

        private static void OnPropertyMacroblockWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                // When a setting changes, update the preset accordingly
                instance.UpdatePreset();
            }
        }

        private static void OnPropertyVectorWeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                // When a setting changes, update the preset accordingly
                instance.UpdatePreset();
            }
        }

        private void OnButtonOkClicked(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();

        }

        private void OnComboBoxPresetsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (m_ignoreUpdatePresets)
            {
                return;
            }

            m_ignoreUpdatePresets = true;
            try
            {
                if (m_cbPresets.SelectedItem == m_presetLuma)
                {
                    //If luma preset selected, update with luma preset values
                    LumaWeight = LumaPresetLumaWeight;
                    ChromaWeight = LumaPresetChromaWeight;
                    VectorWeight = LumaPresetVectorWeight;
                    MacroblockWeight = LumaPresetMacroblockWeight;
                }
                else if (m_cbPresets.SelectedItem == m_presetVector)
                {
                    //If vector preset selected, update with vector preset values
                    LumaWeight = VectorPresetLumaWeight;
                    ChromaWeight = VectorPresetChromaWeight;
                    VectorWeight = VectorPresetVectorWeight;
                    MacroblockWeight = VectorPresetMacroblockWeight;
                }
            }
            finally
            {
                m_ignoreUpdatePresets = false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initializes the dialog with with the configured H264 motion detection settings
        /// </summary>
        public void Initialize(int lumaWeight, int chromaWeight, int vectorWeight, int macroblockWeight)
        {
            LumaWeight = lumaWeight;
            ChromaWeight = chromaWeight;
            VectorWeight = vectorWeight;
            MacroblockWeight = macroblockWeight;

            //Update the preset
            UpdatePreset();
        }

        #endregion

        #region Private Methods

        private static object OnPropertyChromaWeightCoerce(DependencyObject d, object baseValue)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                return instance.OnChromaWeightCoerce(baseValue);
            }

            return baseValue;
        }

        private static object OnPropertyLumaWeightCoerce(DependencyObject d, object baseValue)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                return instance.OnLumaWeightCoerce(baseValue);
            }

            return baseValue;
        }

        private static object OnPropertyMacroblockWeightCoerce(DependencyObject d, object baseValue)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                return instance.OnMacroblockWeightCoerce(baseValue);
            }

            return baseValue;
        }

        private static object OnPropertyVectorWeightCoerce(DependencyObject d, object baseValue)
        {
            H264SettingsDlg instance = d as H264SettingsDlg;
            if (instance != null)
            {
                return instance.OnVectorWeightCoerce(baseValue);
            }

            return baseValue;
        }

        /// <summary>
        /// Coerce used to make sure the value is between 0 and 100
        /// </summary>
        private object OnChromaWeightCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int chromaWeight = (int)baseValue;
                if ((chromaWeight > -1) && (chromaWeight < 101))
                {
                    return chromaWeight;
                }
            }

            return ChromaWeight;
        }

        /// <summary>
        /// Coerce used to make sure the value is between 0 and 100
        /// </summary>
        private object OnLumaWeightCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int lumaWeight = (int)baseValue;
                if ((lumaWeight > -1) && (lumaWeight < 101))
                {
                    return lumaWeight;
                }
            }

            return LumaWeight;
        }

        /// <summary>
        /// Coerce used to make sure the value is between 0 and 100
        /// </summary>
        private object OnMacroblockWeightCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int macroblockWeight = (int)baseValue;
                if ((macroblockWeight > -1) && (macroblockWeight < 101))
                {
                    return macroblockWeight;
                }
            }

            return MacroblockWeight;
        }

        /// <summary>
        /// Coerce used to make sure the value is between 0 and 100
        /// </summary>
        private object OnVectorWeightCoerce(object baseValue)
        {
            if (baseValue is int)
            {
                int vectorWeight = (int)baseValue;
                if ((vectorWeight > -1) && (vectorWeight < 101))
                {
                    return vectorWeight;
                }
            }

            return VectorWeight;
        }

        /// <summary>
        /// Update the selected preset depending on the setting values
        /// </summary>
        private void UpdatePreset()
        {
            if (m_ignoreUpdatePresets)
            {
                return;
            }

            m_ignoreUpdatePresets = true;
            try
            {
                if ((LumaWeight == LumaPresetLumaWeight) && (ChromaWeight == LumaPresetChromaWeight) &&
                    (VectorWeight == LumaPresetVectorWeight) && (MacroblockWeight == LumaPresetMacroblockWeight))
                {
                    //If all the values match the luma preset value, select it
                    m_cbPresets.SelectedItem = m_presetLuma;
                }
                else if ((LumaWeight == VectorPresetLumaWeight) && (ChromaWeight == VectorPresetChromaWeight) &&
                         (VectorWeight == VectorPresetVectorWeight) && (MacroblockWeight == VectorPresetMacroblockWeight))
                {
                    //If all the values match the vector preset value, select it
                    m_cbPresets.SelectedItem = m_presetVector;
                }
                else
                {
                    //If not preset values are matched, select the custom preset
                    m_cbPresets.SelectedItem = m_presetCustom;
                }
            }
            finally
            {
                m_ignoreUpdatePresets = false;
            }
        }

        #endregion
    }

    #endregion
}

