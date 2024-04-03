// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Diagnostics;
using Genetec.Sdk.Entities;

namespace MapControl.Views
{
    /// <summary>
    /// Interaction logic for MapPageView.xaml
    /// </summary>
    public partial class MapPageView
    {
        #region Private Fields

        private Genetec.Sdk.Workspace.Workspace m_workspace;

        #endregion Private Fields

        #region Public Constructors

        public MapPageView() => InitializeComponent();

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Initialize the view.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        public void Initialize(Genetec.Sdk.Workspace.Workspace workspace)
        {
            m_workspace = workspace;
            m_entityTree.Initialize(workspace.Sdk);
            m_mapControl.Initialize(workspace);
        }

        #endregion Public Methods

        #region Private Methods

        private void OnEntityTreeSelectedItemChanged(object sender, EventArgs e)
        {
            var mapId = Guid.Empty;
            var area = m_workspace.Sdk.GetEntity(m_entityTree.SelectedItem) as Area;
            if (area != null)
            {
                mapId = area.MapId;
            }
            m_mapControl.Map = mapId;
        }

        private void OnMapViewChanged(object sender, EventArgs e)
        {
            var text = $"{m_mapControl.ViewArea} ({m_mapControl.GetScaleFactor(m_mapControl.Center.Latitude)} px/m)";
            Debug.WriteLine(text);
        }

        #endregion Private Methods
    }
}