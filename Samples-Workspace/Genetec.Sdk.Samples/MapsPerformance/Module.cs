// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using MapsPerformance.Builders;
using MapsPerformance.Providers;

namespace MapsPerformance
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private PerformanceMapLayerBuilder m_performanceMapLayerBuilder;
        private PerformanceMapObjectProvider m_performanceMapObjectProvider;
        private PerformanceMapObjectViewBuilder m_performanceMapObjectViewBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_performanceMapLayerBuilder = new PerformanceMapLayerBuilder();
            m_performanceMapLayerBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_performanceMapLayerBuilder);

            m_performanceMapObjectProvider = new PerformanceMapObjectProvider();
            m_performanceMapObjectProvider.Initialize(Workspace);
            Workspace.Components.Register(m_performanceMapObjectProvider);

            m_performanceMapObjectViewBuilder = new PerformanceMapObjectViewBuilder();
            m_performanceMapObjectViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_performanceMapObjectViewBuilder);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_performanceMapLayerBuilder != null)
            {
                Workspace.Components.Unregister(m_performanceMapLayerBuilder);
                m_performanceMapLayerBuilder = null;
            }

            if (m_performanceMapObjectProvider != null)
            {
                Workspace.Components.Unregister(m_performanceMapObjectProvider);
                m_performanceMapObjectProvider = null;
            }

            if (m_performanceMapObjectViewBuilder != null)
            {
                Workspace.Components.Unregister(m_performanceMapObjectViewBuilder);
                m_performanceMapObjectViewBuilder = null;
            }
        }

        #endregion Public Methods

    }
}