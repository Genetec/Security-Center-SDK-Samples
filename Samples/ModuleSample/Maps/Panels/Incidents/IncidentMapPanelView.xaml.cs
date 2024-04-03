// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace.Components.MapPanel;
using ModuleSample.Maps.MapObjects.Accidents;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModuleSample.Maps.Panels.Incidents
{
    /// <summary>
    /// Interaction logic for IncidentMapPanelView.xaml
    /// </summary>
    public partial class IncidentMapPanelView
    {

        #region Public Fields

        public static readonly DependencyProperty IncidentsProperty =
                    DependencyProperty.Register("Incidents", typeof(ObservableCollection<AccidentMapObject>), typeof(IncidentMapPanelView), new PropertyMetadata(null));

        public static readonly DependencyProperty LatitudeProperty =
                    DependencyProperty.Register("Latitude", typeof(double), typeof(IncidentMapPanelView), new PropertyMetadata(0.0d));

        public static readonly DependencyProperty LongitudeProperty =
                    DependencyProperty.Register("Longitude", typeof(double), typeof(IncidentMapPanelView), new PropertyMetadata(0.0d));

        #endregion Public Fields

        #region Private Fields

        private readonly MapPanel m_panel;

        #endregion Private Fields

        #region Public Properties

        public ObservableCollection<AccidentMapObject> Incidents
        {
            get => (ObservableCollection<AccidentMapObject>)GetValue(IncidentsProperty);
            set => SetValue(IncidentsProperty, value);
        }

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

        #endregion Public Properties

        #region Public Constructors

        public IncidentMapPanelView(MapPanel panel)
        {
            InitializeComponent();
            Incidents = new ObservableCollection<AccidentMapObject>();

            m_panel = panel;
            RefreshIncidents();
        }

        #endregion Public Constructors

        #region Public Methods

        public void RefreshIncidents()
        {
            Incidents.Clear();
            var accidents = AccidentMapObjectProvider.GetAccidents();
            foreach (var accident in accidents)
            {
                Incidents.Add(accident);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void OnAddIncidentClick(object sender, RoutedEventArgs e)
        {
            using (var popup = new AddIncidentPopup("New incident", Latitude.ToString(CultureInfo.InvariantCulture), Longitude.ToString(CultureInfo.InvariantCulture)))
            {
                popup.ShowDialog();
            }
            RefreshIncidents();
        }

        private void OnIncidentDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ListViewItem)sender).Content is AccidentMapObject incident)
            {
                m_panel.MapControl.ViewArea = new GeoBounds(new GeoCoordinate(incident.Latitude, incident.Longitude), 100);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                RefreshIncidents();
            }
        }

        private void OnRefreshClick(object sender, RoutedEventArgs e)
        {
            RefreshIncidents();
        }

        private void OnViewChanged(object sender, EventArgs e)
        {
            try
            {
                if (m_panel?.MapControl?.Center != null)
                {
                    Latitude = m_panel.MapControl.Center.Latitude;
                    Longitude = m_panel.MapControl.Center.Longitude;
                }
            }
            catch (NullReferenceException)
            {
                // Center get -> geographicmapcanvas.Center
                // But geographicmapcanvas can be null when disposed
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