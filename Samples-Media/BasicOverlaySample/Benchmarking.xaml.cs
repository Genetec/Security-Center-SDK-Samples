using System.Windows;
using System.Windows.Controls;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 13:
//  1501 – Michelangelo begins work on his statue of David.
//  1906 – First flight of a fixed-wing aircraft in Europe.
//  1968 – Albania leaves the Warsaw Pact.
// ==========================================================================
namespace BasicOverlaySample
{
    #region Classes

    /// <summary>
    /// Interaction logic for Benchmarking.xaml
    /// </summary>
    public partial class Benchmarking : UserControl
    {
        #region Constants

        public static readonly DependencyProperty DeltaXProperty =
                                                    DependencyProperty.Register("DeltaX", typeof(int), typeof(Benchmarking), new PropertyMetadata(1));

        public static readonly DependencyProperty DeltaYProperty =
                                                    DependencyProperty.Register("DeltaY", typeof(int), typeof(Benchmarking), new PropertyMetadata(1));

        public static readonly DependencyProperty DurationProperty =
                                                    DependencyProperty.Register("Duration", typeof(int), typeof(Benchmarking), new PropertyMetadata(1));

        public static readonly DependencyProperty FrequencyProperty =
                                                    DependencyProperty.Register("Frequency", typeof(double), typeof(Benchmarking), new PropertyMetadata(0.01));

        public static readonly DependencyProperty PositionXProperty =
                                                    DependencyProperty.Register("PositionX", typeof(int), typeof(Benchmarking), new PropertyMetadata(0));

        public static readonly DependencyProperty PositionYProperty =
                                                    DependencyProperty.Register("PositionY", typeof(int), typeof(Benchmarking), new PropertyMetadata(0));

        public static readonly DependencyProperty SizeHProperty =
                                                    DependencyProperty.Register("SizeH", typeof(int), typeof(Benchmarking), new PropertyMetadata(4));

        public static readonly DependencyProperty SizeWProperty =
                                                    DependencyProperty.Register("SizeW", typeof(int), typeof(Benchmarking), new PropertyMetadata(6));

        #endregion

        #region Properties

        public int DeltaX
        {
            get { return (int)GetValue(DeltaXProperty); }
            set { SetValue(DeltaXProperty, value); }
        }

        public int DeltaY
        {
            get { return (int)GetValue(DeltaYProperty); }
            set { SetValue(DeltaYProperty, value); }
        }

        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        public double Frequency
        {
            get { return (double)GetValue(FrequencyProperty); }
            set { SetValue(FrequencyProperty, value); }
        }

        public int PositionX
        {
            get { return (int)GetValue(PositionXProperty); }
            set { SetValue(PositionXProperty, value); }
        }

        public int PositionY
        {
            get { return (int)GetValue(PositionYProperty); }
            set { SetValue(PositionYProperty, value); }
        }

        public int SizeH
        {
            get { return (int)GetValue(SizeHProperty); }
            set { SetValue(SizeHProperty, value); }
        }

        public int SizeW
        {
            get { return (int)GetValue(SizeWProperty); }
            set { SetValue(SizeWProperty, value); }
        }

        #endregion

        #region Events and Delegates

        public delegate void StartButtonClickHandler(object sender, RoutedEventArgs e);

        public delegate void StopButtonClickHandler(object sender, RoutedEventArgs e);

        public event StartButtonClickHandler StartButtonClicked;

        public event StopButtonClickHandler StopButtonClicked;

        #endregion

        #region Nested Classes and Structures

        public struct BenchmarkParameters
        {
            public int PositionX, PositionY, DeltaX, DeltaY, SizeW, SizeH, Duration;

            /// <summary>
            /// The time (in seconds) to wait between commands
            /// </summary>
            public double Frequency;
        }

        #endregion

        #region Constructors

        public Benchmarking()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonStartBenchmarkClick(object sender, RoutedEventArgs e)
        {
            if (StartButtonClicked != null)
            {
                StartButtonClicked(sender, e);
            }
        }

        private void OnButtonStopBenchmarkClick(object sender, RoutedEventArgs e)
        {
            if (StopButtonClicked != null)
            {
                StopButtonClicked(sender, e);
            }
        }

        #endregion

        #region Public Methods

        public void Lock()
        {
            m_startButton.IsEnabled = false;
        }

        public void ReportProgress(int percentage)
        {
            m_benchmarkProgressBar.Value = percentage;
        }

        public void Unlock()
        {
            m_startButton.IsEnabled = true;
        }

        #endregion
    }

    #endregion
}

