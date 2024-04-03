// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ModuleSample.Maps.MapObjects.Accidents;
using System;
using System.Windows;

namespace ModuleSample.Maps.Panels.Incidents
{
    /// <summary>
    /// Interaction logic for AddIncidentPopup.xaml
    /// </summary>
    public partial class AddIncidentPopup : IDisposable
    {

        #region Public Fields

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
                    DependencyProperty.Register("Description", typeof(string), typeof(AddIncidentPopup), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Latitude.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LatitudeProperty =
                    DependencyProperty.Register("Latitude", typeof(string), typeof(AddIncidentPopup), new PropertyMetadata(string.Empty));

        // Using a DependencyProperty as the backing store for Longitude.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LongitudeProperty =
                    DependencyProperty.Register("Longitude", typeof(string), typeof(AddIncidentPopup), new PropertyMetadata(string.Empty));

        #endregion Public Fields

        #region Public Properties

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public string Latitude
        {
            get => (string)GetValue(LatitudeProperty);
            set => SetValue(LatitudeProperty, value);
        }

        public string Longitude
        {
            get => (string)GetValue(LongitudeProperty);
            set => SetValue(LongitudeProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public AddIncidentPopup(string description, string latitude, string longitude)
        {
            InitializeComponent();

            Description = description;
            Latitude = latitude;
            Longitude = longitude;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
        }

        #endregion Public Methods

        #region Private Methods

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnOkClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Latitude, out var lat) && double.TryParse(Longitude, out var lon))
            {
                AccidentMapObjectProvider.AddAccident(new AccidentMapObject(lat, lon, Description));
                Close();
            }
            else
            {
                MessageBox.Show("Invalid coordinates.", "Error");
            }
        }

        #endregion Private Methods

    }
}