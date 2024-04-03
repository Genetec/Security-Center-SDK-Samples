// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using CaseManagement.Builders;

namespace CaseManagement
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private CaseContentBuilder m_caseContentBuilder;
        private CaseIncidentBuilder m_caseIncidentBuilder;
        private CaseTileViewBuilder m_caseTileViewBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_caseIncidentBuilder = new CaseIncidentBuilder();
            m_caseIncidentBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_caseIncidentBuilder);

            m_caseTileViewBuilder = new CaseTileViewBuilder();
            m_caseTileViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_caseTileViewBuilder);

            m_caseContentBuilder = new CaseContentBuilder();
            m_caseContentBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_caseContentBuilder);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering its workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_caseIncidentBuilder != null)
            {
                Workspace.Components.Unregister(m_caseIncidentBuilder);
                m_caseIncidentBuilder = null;
            }

            if (m_caseTileViewBuilder != null)
            {
                Workspace.Components.Unregister(m_caseTileViewBuilder);
                m_caseTileViewBuilder = null;
            }

            if (m_caseContentBuilder != null)
            {
                Workspace.Components.Unregister(m_caseContentBuilder);
                m_caseContentBuilder = null;
            }
        }

        #endregion Public Methods

    }
}