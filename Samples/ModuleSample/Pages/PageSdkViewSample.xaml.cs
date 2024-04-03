// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Workflows;
using Genetec.Sdk.Workspace;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Map = Genetec.Sdk.Entities.Map;

namespace ModuleSample.Pages
{

    /// <summary>
    /// Interaction logic for PageSdkSample.xaml
    /// </summary>
    public partial class PageSdkViewSample
    {

        #region Protected Properties

        protected Workspace Workspace { get; private set; }

        #endregion Protected Properties

        #region Public Constructors

        public PageSdkViewSample() => InitializeComponent();
        
        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            if (m_mapControl != null)
            {
                m_mapControl.Dispose();
                m_mapControl = null;
            }
        }

        /// <summary>
        /// Initialize the view.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        public void Initialize(Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentNullException("workspace");
            m_mapControl.Initialize(workspace);
            PopulateMapList();
        }

        #endregion Public Methods

        #region Private Methods

        private void OnButtonClearTileContentClick(object sender, RoutedEventArgs e)
        {
            var actionManager = Workspace.Sdk.ActionManager;
            actionManager.ClearTile(int.Parse(m_monitorId.Text), int.Parse(m_tileId.Text));
        }

        private void OnButtonGetTileContentClick(object sender, RoutedEventArgs e)
        {
            var actionManager = Workspace.Sdk.ActionManager;
            actionManager.BeginGetTile(int.Parse(m_monitorId.Text), int.Parse(m_tileId.Text), OnEndGetTile, null);
        }

        private void OnButtonSetCenterClick(object sender, RoutedEventArgs e)
            => m_mapControl.Center = new GeoCoordinate(double.Parse(latitudeTextBox.Text), double.Parse(longitudeTextBox.Text));
      
        private void OnButtonSetTileContentClick(object sender, RoutedEventArgs e)
            => Workspace.Sdk.ActionManager.DisplayInTile(int.Parse(m_monitorId.Text), int.Parse(m_tileId.Text), m_tileContent.Text);
      
        private void OnButtonShowMapClick(object sender, RoutedEventArgs e) 
            => m_mapControl.Map = (Guid)((ComboBoxItem)mapGuids.SelectedItem).Tag;
        
        private void OnButtonZoomInClick(object sender, RoutedEventArgs e) 
            => m_mapControl.ZoomIn();
      
        private void OnButtonZoomOutClick(object sender, RoutedEventArgs e) 
            => m_mapControl.ZoomOut();
        
        private void OnEndGetTile(IAsyncResult ar)
        {
            try
            {
                var xmlTileContent = Workspace.Sdk.ActionManager.EndGetTile(ar);
                if (xmlTileContent != null)
                {
                    Action pfunc = () => m_tileContent.Text = xmlTileContent;
                    if (Dispatcher.CheckAccess())
                        pfunc();
                    else
                        Dispatcher.BeginInvoke(pfunc);
                }
            }
            catch (SdkException exception)
            {
                MessageBox.Show(exception.Message, "An error occurred while fetching tile content");
            }
        }
        private void PopulateMapList()
        {
            mapGuids.Items.Clear();
            if (Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) is EntityConfigurationQuery query)
            {
                query.EntityTypeFilter.Add(EntityType.Map);

                var results = query.Query();
                foreach (DataRow row in results.Data.Rows)
                {
                    var item = new ComboBoxItem();
                    var map = (Map)Workspace.Sdk.GetEntity((Guid)row[0]);
                    item.Content = map.Name;
                    item.Tag = map.Guid;
                    mapGuids.Items.Add(item);
                }

                mapGuids.Items.SortDescriptions.Add(new SortDescription("Content", ListSortDirection.Ascending));

                mapGuids.SelectedIndex = 0;
            }
        }

        #endregion Private Methods

    }

}