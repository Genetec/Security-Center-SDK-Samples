using Genetec.Sdk.Media.Export;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Data;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for June 21:
//  1749 – Halifax, Nova Scotia, is founded.
//  1898 – The United States captures Guam from Spain.
//  2006 – Pluto's newly discovered moons are officially named Nix and Hydra.
// ==========================================================================
namespace MediaConverterSample
{
    #region Enumerations

    // Enumeration for the states of the converter used by the UI to display/hide information. 
    public enum ConversionState
    {
        Converting,
        Cancelled,
        Error,
        Idle
    }

    #endregion

    #region Classes

    /// <summary>
    /// Interaction logic for MediaConverterWindow.xaml
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
    public partial class MediaConverterWindow
    {
        #region Constants

        // Used to get the selected index on the comboBox of the asf profiles.
        public static readonly DependencyProperty AsfProfilesSelectedIndexProperty = DependencyProperty.Register(
            nameof(AsfProfilesSelectedIndex), typeof(int), typeof(MediaConverterWindow), new PropertyMetadata(default(int)));

        // Used to get the converter choice format which is currently selected.
        public static readonly DependencyProperty ConverterChoiceSelectedIndexProperty = DependencyProperty.Register(
            nameof(ConverterChoiceSelectedIndex), typeof(int), typeof(MediaConverterWindow), new PropertyMetadata(default(int)));

        // The current conversion state of the Converter based on the ConversionState enum.
        public static readonly DependencyProperty CurrentConversionStateProperty = DependencyProperty.Register(
            nameof(CurrentConversionState), typeof(ConversionState), typeof(MediaConverterWindow), new PropertyMetadata(default(ConversionState)));

        public static readonly DependencyProperty CurrentFileCountStatusProperty = DependencyProperty.Register(
            nameof(CurrentFileCountStatus), typeof(string), typeof(MediaConverterWindow), new PropertyMetadata("Converting 0 of 0 files"));

        // Used to change the elapsed time on the UI with the new data.
        public static readonly DependencyProperty ElapsedTimeProperty = DependencyProperty.Register(
            nameof(ElapsedTime), typeof(string), typeof(MediaConverterWindow), new PropertyMetadata("Elapsed time: N/A"));

        // Used to get the value of the checkbox ConvertAudio.
        public static readonly DependencyProperty IsConvertAudioCheckedProperty = DependencyProperty.Register(
            nameof(IsConvertAudioChecked), typeof(bool), typeof(MediaConverterWindow), new PropertyMetadata(true));

        // Used to get the value of the checkbox ConvertAudio.
        public static readonly DependencyProperty IsConvertAllFilesCheckedProperty = DependencyProperty.Register(
            nameof(IsConvertAllFilesChecked), typeof(bool), typeof(MediaConverterWindow), new PropertyMetadata(false));

        // Used to get the value of the checkbox DisplayCaneraNAme.
        public static readonly DependencyProperty IsDisplayCameraNameCheckedProperty = DependencyProperty.Register(
            nameof(IsDisplayCameraNameChecked), typeof(bool), typeof(MediaConverterWindow), new PropertyMetadata(true));

        // Used to get the value of the checkbox DisplayDateTime.
        public static readonly DependencyProperty IsDisplayDateTimeCheckedProperty = DependencyProperty.Register(
            nameof(IsDisplayDateTimeChecked), typeof(bool), typeof(MediaConverterWindow), new PropertyMetadata(true));

        // Dependency to update the progress bar in the UI with the new data received.
        public static readonly DependencyProperty ProgressBarValueProperty = DependencyProperty.Register(
            nameof(ProgressBarValue), typeof(double), typeof(MediaConverterWindow), new PropertyMetadata(default(double)));

        // Source path to the video to convert.
        public static readonly DependencyProperty SourcePathProperty = DependencyProperty.Register(
            nameof(SourcePath), typeof(string), typeof(MediaConverterWindow), new PropertyMetadata(default(string)));

        // Target path of the converted video.
        public static readonly DependencyProperty TargetPathProperty = DependencyProperty.Register(
            nameof(TargetPath), typeof(string), typeof(MediaConverterWindow), new PropertyMetadata("c:\\temp\\"));

        private const double MaxProgressBarValue = 100;

        private const string G64FilesFilter = "G64 files|*.g64";

        private const string G64XFilesFilter = "G64X files|*.g64x";

        private const string GenetecVideoFiles = "Genetec video files|*.g64;*g64x";

        private readonly Stopwatch m_elapsedtimeStopwatch;

        private readonly DispatcherTimer m_elapsedTimeTimer;

        private Queue<string> m_filesToConvert;

        private int m_totalFilesToConvert;

        private bool m_Converting;

        #endregion

        #region Fields

        private G64ConverterBase m_converter;

        #endregion

        #region Properties

        // Collection for the AsfProfiles
        public ObservableCollection<AsfProfile> AsfProfilesItems { get; private set; }

        public int AsfProfilesSelectedIndex
        {
            get { return (int)GetValue(AsfProfilesSelectedIndexProperty); }
            set { SetValue(AsfProfilesSelectedIndexProperty, value); }
        }

        // Collection for the converter choice format.
        public ObservableCollection<ConverterTypes> ConverterChoiceItems { get; private set; }

        public int ConverterChoiceSelectedIndex
        {
            get { return (int)GetValue(ConverterChoiceSelectedIndexProperty); }
            set { SetValue(ConverterChoiceSelectedIndexProperty, value); }
        }

        public ConversionState CurrentConversionState
        {
            get { return (ConversionState)GetValue(CurrentConversionStateProperty); }
            set { SetValue(CurrentConversionStateProperty, value); }
        }

        public string ElapsedTime
        {
            get { return (string)GetValue(ElapsedTimeProperty); }
            set { SetValue(ElapsedTimeProperty, value); }
        }

        public bool IsConvertAllFilesChecked
        {
            get { return (bool)GetValue(IsConvertAllFilesCheckedProperty); }
            set { SetValue(IsConvertAllFilesCheckedProperty, value); }
        }

        public bool IsConvertAudioChecked
        {
            get { return (bool)GetValue(IsConvertAudioCheckedProperty); }
            set { SetValue(IsConvertAudioCheckedProperty, value); }
        }

        public bool IsDisplayCameraNameChecked
        {
            get { return (bool)GetValue(IsDisplayCameraNameCheckedProperty); }
            set { SetValue(IsDisplayCameraNameCheckedProperty, value); }
        }

        public bool IsDisplayDateTimeChecked
        {
            get { return (bool)GetValue(IsDisplayDateTimeCheckedProperty); }
            set { SetValue(IsDisplayDateTimeCheckedProperty, value); }
        }

        public double ProgressBarValue
        {
            get { return (double)GetValue(ProgressBarValueProperty); }
            set { SetValue(ProgressBarValueProperty, value); }
        }

        public string SourcePath
        {
            get { return (string)GetValue(SourcePathProperty); }
            set { SetValue(SourcePathProperty, value); }
        }

        public string CurrentFileCountStatus
        {
            get { return (string)GetValue(CurrentFileCountStatusProperty); }
            set { SetValue(CurrentFileCountStatusProperty, value); }
        }

        public string TargetPath
        {
            get { return (string)GetValue(TargetPathProperty); }
            set { SetValue(TargetPathProperty, value); }
        }

        #endregion

        #region Enumerations

        // All the possible converterTypes.
        public enum ConverterTypes
        {
            G64xPasswordProtected,
            Mp4,
            Asf
        };

        #endregion

        #region Constructors

        public MediaConverterWindow()
        {
            InitializeComponent();

            m_filesToConvert = new Queue<string>();
            m_totalFilesToConvert = 0;
            m_Converting = false;

            ConverterChoiceItems = new ObservableCollection<ConverterTypes> { ConverterTypes.Asf, ConverterTypes.Mp4, ConverterTypes.G64xPasswordProtected };
            ConverterChoiceSelectedIndex = 0;

            AsfProfilesItems = new ObservableCollection<AsfProfile>(G64ToAsfConverter.GetAsfProfiles());
            AsfProfilesSelectedIndex = 0;

            m_elapsedtimeStopwatch = new Stopwatch();
            m_elapsedTimeTimer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, OnElapsedTimerTick, Dispatcher);

            DataContext = this;
        }

        #endregion

        #region Event Handlers

        private void OnComboBoxConverterChoiceSelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CreateConverter();
        }

        private void OnConversionFinished(object sender, ConversionFinishedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => { OnConversionFinished(sender, e); }));
                return;
            }

            if (m_filesToConvert.Count > 0)
            {
                ProgressBarValue = 0;
                StartConversionClick(null,null);
            }
            else
            {

                m_elapsedtimeStopwatch.Stop();
                UpdateElapsedTimeLabel();
                m_elapsedTimeTimer.Stop();
                m_elapsedtimeStopwatch.Reset();
                ProgressBarValue = MaxProgressBarValue;

                switch (e.Result)
                {
                    case ConversionResult.Successful:
                        CurrentConversionState = ConversionState.Idle;
                        break;
                    case ConversionResult.Cancelled:
                        CurrentConversionState = ConversionState.Cancelled;
                        break;
                    default:
                        CurrentConversionState = ConversionState.Error;
                        //Show a message with the result and an error message if there is one. If it is empty, it won't show the details.
                        string errorDetails = e.ErrorMessage != null
                            ? String.Format(", Details: {0}", e.ErrorMessage)
                            : String.Empty;
                        MessageBox.Show(this, String.Format("An error occurred: {0}" + errorDetails, e.Result),
                            "Error occurred", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            
                m_Converting = false;
            }

            UpdateLayout();
        }

        private void OnConversionProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => { OnConversionProgressChanged(sender, e); }));
                return;
            }
            ProgressBarValue = e.ProgressPercentage;
        }

        private void OnElapsedTimerTick(object sender, EventArgs e)
        {
            UpdateElapsedTimeLabel();
        }

        #endregion

        #region Private Methods

        private static Guid FindGuidForProfileName(string name)
        {
            foreach (AsfProfile profile in G64ToAsfConverter.GetAsfProfiles())
            {
                if (profile.Name == name)
                {
                    return profile.Profile;
                }
            }

            return Guid.Empty;
        }

        private void BrowseSourceButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = string.Join("|", new[] { GenetecVideoFiles, G64FilesFilter, G64XFilesFilter })
            };

            if ((bool)fileDialog.ShowDialog(this))
            {
                SourcePath = fileDialog.FileName;
            }
        }

        private void BrowseTargetButtonClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
            {
                SelectedPath = TargetPathProperty.DefaultMetadata.DefaultValue as string ?? string.Empty
            };

            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TargetPath = folderBrowserDialog.SelectedPath + "\\";
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            m_converter.CancelConversion(true);
            m_totalFilesToConvert = 0;
            m_filesToConvert.Clear();
            m_Converting = false;
        }

        private void CreateConverter()
        {
            switch (ConverterChoiceItems[ConverterChoiceSelectedIndex])
            {
                case ConverterTypes.Asf:
                    // Set-up Asf converter
                    m_converter = new G64ToAsfConverter();
                    break;
                case ConverterTypes.Mp4:
                    // Set-up Mp4 converter
                    m_converter = new G64ToMp4Converter();
                    break;
                case ConverterTypes.G64xPasswordProtected:
                    // Set-up Password Protected G64x converter
                    m_converter = new G64xPasswordProtectionConverter();
                    break;
                default:
                    throw new InvalidOperationException("There is no converter of this type.");
            }

            //Set events on the new converter.
            m_converter.ProgressChanged += OnConversionProgressChanged;
            m_converter.ConversionFinished += OnConversionFinished;

            //Conversion tate is now idle
            CurrentConversionState = ConversionState.Idle;
        }

        private void BuildFileToConvertList()
        {
            if (m_Converting || SourcePath == null)
                return;

            m_filesToConvert.Clear();
            if (IsConvertAllFilesChecked)
            {
                string inputFilePath = Path.GetDirectoryName(SourcePath);

                DirectoryInfo dir = new DirectoryInfo(inputFilePath);
                if (dir.Exists)
                {
                    m_totalFilesToConvert = 0;
                    foreach (var file in dir.EnumerateFiles())
                    {
                        m_filesToConvert.Enqueue(file.Name);
                        m_totalFilesToConvert++;
                    }
                }
            }
            else
            {
                string inputFileName = Path.GetFileName(SourcePath);
                m_filesToConvert.Enqueue(inputFileName);
                m_totalFilesToConvert = 1;
            }
        }

        private void UpdateFileToConvertStatus()
        {
            int convertedFileCount = m_totalFilesToConvert - m_filesToConvert.Count;
            CurrentFileCountStatus = "Converting " + convertedFileCount.ToString() + " of " + m_totalFilesToConvert.ToString() + " files";
        }

        private string GetNextFileToConvert()
        {
            string nextFile = "";

            if (m_filesToConvert.Count > 0)
            {
                nextFile = m_filesToConvert.Dequeue();
            }
            else
            {
                m_Converting = false;
            }

            UpdateFileToConvertStatus();
            return nextFile;
        }

        private void StartConversionClick(object sender, RoutedEventArgs e)
        {
            UpdateLayout();

            if (!m_Converting)
            {
                BuildFileToConvertList();
                m_Converting = true;
            }

            string fileName = GetNextFileToConvert();
            string inputFilePath = Path.GetDirectoryName(SourcePath);
            //SourcePath;
            string sourceFileNameAndPath = string.Format(inputFilePath + "\\{0}", fileName);
            //target
            string targetFileNameAndPath = string.Format(TargetPath + "\\{0}", fileName);

            if (fileName == "")
            {
                m_Converting = false;
                return;
            }

            switch (m_converter)
            {
                case G64ToAsfConverter toAsfConverter:
                    toAsfConverter.Initialize(sourceFileNameAndPath, IsDisplayDateTimeChecked, IsDisplayCameraNameChecked, IsConvertAudioChecked, targetFileNameAndPath,
                        FindGuidForProfileName(AsfProfilesItems[AsfProfilesSelectedIndex].ToString()));
                    break;
                case G64ToMp4Converter toMp4Converter:
                    toMp4Converter.Initialize( sourceFileNameAndPath, IsConvertAudioChecked, targetFileNameAndPath);
                    break;
                case G64xPasswordProtectionConverter encryptionConverter:
                    if (string.IsNullOrEmpty(m_password.Password))
                    {
                        MessageBox.Show("Password must be set", "Password error", MessageBoxButton.OK, MessageBoxImage.Error);
                        m_Converting = false;
                        return;
                    }
                    if (m_password.Password != m_confirmedPassword.Password)
                    {
                        m_confirmedPassword.Clear();
                        MessageBox.Show("Password and confirmed password are not the same.", "Password error", MessageBoxButton.OK, MessageBoxImage.Error);
                        m_Converting = false;
                        return;
                    }
                    encryptionConverter.Initialize(sourceFileNameAndPath, targetFileNameAndPath, m_password.Password);
                    break;
            }

            m_converter.ConvertAsync();
            m_elapsedTimeTimer.Start();
            m_elapsedtimeStopwatch.Start();
            CurrentConversionState = ConversionState.Converting;
        }

        private void UpdateElapsedTimeLabel()
        {
            ElapsedTime = String.Format("Elapsed time: {0}", m_elapsedtimeStopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
        }

        #endregion
    }

    internal class FormatToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MediaConverterWindow.ConverterTypes type && parameter is string formats)
            {
                if (formats.Split('|').Contains(type.ToString()))
                    return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    #endregion
}