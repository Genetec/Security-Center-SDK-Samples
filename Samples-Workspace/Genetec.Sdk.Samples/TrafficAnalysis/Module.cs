// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using TrafficAnalysis.Builders;
using TrafficAnalysis.Services;

namespace TrafficAnalysis
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private AnalysisService m_analysisService;
        private OverlayTileViewBuilder m_overlayTileViewer;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_analysisService = new AnalysisService();
            m_analysisService.Initialize(Workspace);
            Workspace.Services.Register(m_analysisService);

            m_overlayTileViewer = new OverlayTileViewBuilder();
            m_overlayTileViewer.Initialize(Workspace);
            Workspace.Components.Register(m_overlayTileViewer);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_analysisService != null)
            {
                Workspace.Services.Unregister(m_analysisService);
                m_analysisService = null;
            }

            if (m_overlayTileViewer != null)
            {
                Workspace.Components.Unregister(m_overlayTileViewer);
                m_overlayTileViewer = null;
            }
        }

        #endregion Public Methods

    }
}