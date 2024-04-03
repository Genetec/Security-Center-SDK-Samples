// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using Workspace.Explorer.Managers;
using Workspace.Explorer.SampleSource;

namespace Workspace.Explorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region Public Fields

        public static readonly DependencyPropertyKey TestDependencyPropertyKey =
            DependencyProperty.RegisterReadOnly("NumInstalled",
                typeof(int),
                typeof(MainWindow),
                new PropertyMetadata());

        public static DependencyProperty IsScRunningProperty =
                    DependencyProperty.Register("IsScRunning",
                typeof(bool),
                typeof(MainWindow),
                new PropertyMetadata(false));
        public ObservableCollection<SampleObject> SampleObjects;

        #endregion Public Fields

        #region Private Fields

        private readonly IInstallationManager m_installationManager = new InstallationManager();

        private readonly ISamplesManager m_samplesManager = new SamplesManager();

        private readonly ListCollectionView m_samplesView;

        private readonly ISecurityCenterManager m_securityCenterManager = new SecurityCenterManager();

        #endregion Private Fields

        #region Public Properties

        public bool IsScRunning
        {
            get => (bool)GetValue(IsScRunningProperty);
            set => SetValue(IsScRunningProperty, value);
        }

        public int NumInstalled
        {
            get => (int)GetValue(TestDependencyPropertyKey.DependencyProperty);
            set => SetValue(TestDependencyPropertyKey, value);
        }

        public ICollectionView Samples => m_samplesView;

        public SampleObject SelectedSample { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public MainWindow()
        {
            SampleObjects = new ObservableCollection<SampleObject>(m_samplesManager.GetSampleObjects().OrderBy(x => x.Title));
            m_samplesView = new ListCollectionView(SampleObjects);
            m_samplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            InitializeComponent();
            DataContext = this;
        }

        #endregion Public Constructors

        #region Private Methods

        private void MComboSamples_OnDropDownOpened(object sender, EventArgs e)
        {
            foreach (var sampleObject in SampleObjects)
            {
                sampleObject.IsInstalled = m_installationManager.CheckIsInstalled(sampleObject);
                sampleObject.IsEnabled = m_installationManager.CheckIsEnabled(sampleObject);
            }

            MComboSamples.Items.Refresh();
        }

        private void MComboSamples_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedSample == null) return;
            SelectedSample.IsInstalled = m_installationManager.CheckIsInstalled(SelectedSample);
            SelectedSample.IsEnabled = m_installationManager.CheckIsEnabled(SelectedSample);
            NumInstalled = SampleObjects.Count(x => x.IsInstalled);
        }

        private void OnButtonDisableClick(object sender, RoutedEventArgs e)
        {
            if (SelectedSample == null) return;

            SelectedSample.IsEnabled = !m_installationManager.DisableSample(SelectedSample);
        }

        private void OnButtonEnableClick(object sender, RoutedEventArgs e)
        {
            if (SelectedSample == null) return;

            SelectedSample.IsEnabled = m_installationManager.EnableSample(SelectedSample);
        }

        private void OnButtonInstallClick(object sender, RoutedEventArgs e)
        {
            var sample = (SampleObject)((ToggleButton)sender).DataContext;

            if (sample != null)
                sample.IsInstalled = sample.IsEnabled = m_installationManager.InstallSample(sample);
            NumInstalled = SampleObjects.Count(x => x.IsInstalled);
        }

        private void OnButtonOpenFolderClick(object sender, RoutedEventArgs e)
        {
            if (SelectedSample?.SourceFolder != null)
                Process.Start(SelectedSample.SourceFolder);
        }

        private void OnButtonStartClick(object sender, RoutedEventArgs e)
                                                            => IsScRunning = m_securityCenterManager.Start(SelectedSample);

        private void OnButtonStopClick(object sender, RoutedEventArgs e)
            => IsScRunning = !m_securityCenterManager.Stop();

        private void OnButtonUninstallClick(object sender, RoutedEventArgs e)
        {
            var sample = (SampleObject)((ToggleButton)sender).DataContext;

            if (sample != null)
                sample.IsInstalled = sample.IsEnabled = !m_installationManager.UnInstallSample(sample);
            NumInstalled = SampleObjects.Count(x => x.IsInstalled);
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if(e.Delta > 0)
                scrollViewer.LineUp();
            else
                scrollViewer.LineDown();
            e.Handled = true;
        }
        #endregion Private Methods
    }
}