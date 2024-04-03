// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.MapPanel;
using OfficerMapObject.MapObjects.Officers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace OfficerMapObject.Panels
{
    /// <summary>
    /// Interaction logic for OfficerMapPanelView.xaml
    /// </summary>
    public partial class OfficerMapPanelView
    {
        #region Public Fields

        public static readonly DependencyProperty LatitudeProperty =
            DependencyProperty.Register("Latitude", typeof(double), typeof(OfficerMapPanelView), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty LongitudeProperty =
            DependencyProperty.Register("Longitude", typeof(double), typeof(OfficerMapPanelView), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty OfficersProperty =
            DependencyProperty.Register("Officers", typeof(ObservableCollection<MapObjects.Officers.OfficerMapObject>), typeof(OfficerMapPanelView), new PropertyMetadata(null));

        #endregion Public Fields

        #region Private Fields

        private readonly MapPanel m_panel;

        #endregion Private Fields

        #region Public Properties

        public double Latitude
        {
            get => (double)GetValue(LatitudeProperty);
            set => SetValue(LatitudeProperty, value);
        }

        public double Longitude
        {
            get => (double)GetValue(LongitudeProperty);
            set => SetValue(LongitudeProperty, value);
        }

        public ObservableCollection<MapObjects.Officers.OfficerMapObject> Officers
        {
            get => (ObservableCollection<MapObjects.Officers.OfficerMapObject>)GetValue(OfficersProperty);
            set => SetValue(OfficersProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public OfficerMapPanelView(MapPanel panel)
        {
            InitializeComponent();
            Officers = new ObservableCollection<MapObjects.Officers.OfficerMapObject>();
            m_panel = panel;
            RefreshOfficers();
        }

        #endregion Public Constructors

        #region Public Methods

        public void RefreshOfficers()
        {
            Officers.Clear();
            var officers = OfficerMapObjectProvider.GetOfficers();
            foreach (var officer in officers)
            {
                Officers.Add(officer);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void DeleteAll(object sender, RoutedEventArgs e)
            => OfficerMapObjectProvider.DeleteAllOfficers();

        private void DeleteEarliest(object sender, RoutedEventArgs e)
            => OfficerMapObjectProvider.DeleteEarliestOfficer();

        private void DeleteLatest(object sender, RoutedEventArgs e)
            => OfficerMapObjectProvider.DeleteLatestOfficer();

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                RefreshOfficers();
            }
        }

        private void OnViewChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_panel?.MapControl?.Center == null) return;
                Latitude = m_panel.MapControl.Center.Latitude;
                Longitude = m_panel.MapControl.Center.Longitude;
            }
            catch (NullReferenceException)
            {
            }
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            if (m_panel.MapControl != null)
                m_panel.MapControl.ViewChanged += OnViewChanged;
        }

        #endregion Private Methods
    }
}