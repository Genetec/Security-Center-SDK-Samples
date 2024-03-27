// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Controls.Charts;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace ModuleSample.Pages.Controls
{
    /// <summary>
    /// Interaction logic for Charts.xaml
    /// </summary>
    public partial class Charts
    {

        #region Public Fields

        public static readonly DependencyProperty CurrentChartTypeProperty =
                    DependencyProperty.Register("CurrentChartType", typeof(ChartType), typeof(Charts), new PropertyMetadata(ChartType.Bar, OnCurrentChartTypePropertyChanged));

        public static readonly DependencyProperty DisableAnimationsProperty =
                            DependencyProperty.Register("DisableAnimations", typeof(bool), typeof(Charts), new PropertyMetadata(false, OnDisableAnimationsPropertyChanged));

        public static readonly DependencyProperty PerformanceModeProperty =
            DependencyProperty.Register("PerformanceMode", typeof(bool), typeof(Charts), new PropertyMetadata(false, OnPerformanceModePropertyChanged));

        public static readonly DependencyProperty SeriesCountProperty =
                                    DependencyProperty.Register("SeriesCount", typeof(int), typeof(Charts), new PropertyMetadata(4, OnSeriesCountPropertyChanged));

        public static readonly DependencyProperty ShowLegendProperty =
                            DependencyProperty.Register("MyProperty", typeof(bool), typeof(Charts), new PropertyMetadata(true, OnShowLegendPropertyChanged));

        #endregion Public Fields

        #region Private Fields

        private readonly string[] m_labels = { "2014", "2015", "2016", "2017" };

        private readonly Dictionary<string, ChartSeries> m_values = new Dictionary<string, ChartSeries>
        {
            {"USA", new ChartSeries(new[] {1, 2, 3, 4}, Brushes.DarkRed)},
            {"Canada", new ChartSeries(new[] {5, 7, 8, 3}, Brushes.Green)},
            {"China", new ChartSeries(new[] {12, 1, 14,23}, Brushes.YellowGreen)},
            {"Japan", new ChartSeries(new[] {1, 1, 7, 6})},
            {"Mexico", new ChartSeries(new[] {18, 3, 29, 2})},
            {"Algeria", new ChartSeries(new[] {19, 4, 31, 1})},
            {"Jamaica", new ChartSeries(new[] {7, 14, 44, 23})},
            {"Romania", new ChartSeries(new[] {1, 5, 29, 2})},
            {"Yemen" , new ChartSeries(new[] {3, 8, 21, 32})},
            {"Thailand", new ChartSeries(new[] {2, 39, 25, 53})},
            {"Panama", new ChartSeries(new[] {18, 23, 19, 49}, Brushes.Aqua)},
            {"Kazakhstan", new ChartSeries(new[] {62, 1, 46, 73}, Brushes.DarkOliveGreen)},
            {"Laos", new ChartSeries(new[] {62, 42, 12, 5}, Brushes.Khaki)},
            {"Denmark", new ChartSeries(new[] {5, 2, 62, 3}, Brushes.Magenta)},
            {"Dominican Republic", new ChartSeries(new[] {35, 0, 29, 29}, Brushes.PaleGoldenrod)},
            {"The Bahamas", new ChartSeries(new[] {92, 28, 28, 85}, Brushes.PapayaWhip)},
            {"Albania", new ChartSeries(new[] {27, 12, 6, 3}, Brushes.DarkOrchid)},
            {"Aruba", new ChartSeries(new[] {60, 28, 15, 55}, Brushes.LemonChiffon)},
            {"Nigeria", new ChartSeries(new[] {2, 14, 53, 29}, Brushes.MediumAquamarine)},
            {"Poland", new ChartSeries(new[] {95, 21, 20, 18}, Brushes.PowderBlue)},
        };

        private Chart m_chart;

        #endregion Private Fields

        #region Public Properties

        public ChartType CurrentChartType
        {
            get => (ChartType)GetValue(CurrentChartTypeProperty);
            set => SetValue(CurrentChartTypeProperty, value);
        }

        public bool DisableAnimations
        {
            get => (bool)GetValue(DisableAnimationsProperty);
            set => SetValue(DisableAnimationsProperty, value);
        }

        public bool PerformanceMode
        {
            get => (bool)GetValue(PerformanceModeProperty);
            set => SetValue(PerformanceModeProperty, value);
        }

        public int SeriesCount
        {
            get => (int)GetValue(SeriesCountProperty);
            set => SetValue(SeriesCountProperty, value);
        }

        public bool ShowLegend
        {
            get => (bool)GetValue(ShowLegendProperty);
            set => SetValue(ShowLegendProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public Charts()
        {
            InitializeComponent();
            UpdateChart();
        }

        #endregion Public Constructors

        #region Public Methods

        public void UpdateChart()
        {
            m_chart = null;
            m_chartContainer.Content = null;
            var values = new Dictionary<string, ChartSeries>();

            var enumerator = m_values.GetEnumerator();
            for (var i = 0; i < SeriesCount; i++)
            {
                enumerator.MoveNext();
                var current = enumerator.Current;
                var currentValue = current.Value;

                // For pie chart, only one result per series
                if (CurrentChartType == ChartType.Pie || CurrentChartType == ChartType.Doughnut)
                {
                    currentValue = new ChartSeries(current.Value.Values.FirstOrDefault(), current.Value.Color);
                }
                values.Add(current.Key, currentValue);
            }
            enumerator.Dispose();

            m_chart = ChartFactory.CreateChart(CurrentChartType, m_labels, values);
            m_chart.DisableAnimations = DisableAnimations;
            m_chart.ShowLegend = ShowLegend;
            m_chartContainer.Content = m_chart;

            if (PerformanceMode)
            {
                m_chart.PerformanceMode = PerformanceMode;
                m_chart.Update(m_labels, values);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnCurrentChartTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Charts instance)
                instance.OnCurrentChartTypeChanged();
        }

        private static void OnDisableAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Charts instance)
                instance.OnDisableAnimationsChanged();
        }

        private static void OnPerformanceModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Charts instance)
                instance.OnPerformanceModeChanged();
        }

        private static void OnSeriesCountPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Charts instance)
                instance.OnSeriesCountChanged();
        }

        private static void OnShowLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Charts instance)
                instance.OnShowLegendChanged();
        }
        private void OnCurrentChartTypeChanged()
        {
            UpdateChart();
        }

        private void OnDisableAnimationsChanged()
        {
            m_chart.DisableAnimations = DisableAnimations;
        }

        private void OnPerformanceModeChanged()
        {
            UpdateChart();
        }

        private void OnSeriesCountChanged()
        {
            UpdateChart();
        }

        private void OnShowLegendChanged()
        {
            m_chart.ShowLegend = ShowLegend;
        }

        #endregion Private Methods
    }
}