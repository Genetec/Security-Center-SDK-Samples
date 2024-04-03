// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using DronesTracker.Maps;
using DronesTracker.Maps.Layers;

namespace DronesTracker
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private DemoMapObjectProvider m_demoMapObjectProvider;
        private DemoMapObjectViewBuilder m_demoMapObjectViewBuilder;
        private MotionHeatMapLayerBuilder m_heatMapLayerBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_demoMapObjectProvider = new DemoMapObjectProvider();
            m_demoMapObjectProvider.Initialize(Workspace);
            Workspace.Components.Register(m_demoMapObjectProvider);

            m_demoMapObjectViewBuilder = new DemoMapObjectViewBuilder();
            m_demoMapObjectViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_demoMapObjectViewBuilder);

            m_heatMapLayerBuilder = new MotionHeatMapLayerBuilder();
            m_heatMapLayerBuilder.Initialize(Workspace, m_demoMapObjectProvider);
            Workspace.Components.Register(m_heatMapLayerBuilder);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_demoMapObjectProvider != null)
            {
                Workspace.Components.Unregister(m_demoMapObjectProvider);
                m_demoMapObjectProvider.Dispose();
                m_demoMapObjectProvider = null;
            }

            if (m_demoMapObjectViewBuilder != null)
            {
                Workspace.Components.Unregister(m_demoMapObjectViewBuilder);
                m_demoMapObjectViewBuilder = null;
            }

            if (m_heatMapLayerBuilder != null)
            {
                Workspace.Components.Unregister(m_heatMapLayerBuilder);
                m_heatMapLayerBuilder = null;
            }
        }

        #endregion Public Methods

    }
}