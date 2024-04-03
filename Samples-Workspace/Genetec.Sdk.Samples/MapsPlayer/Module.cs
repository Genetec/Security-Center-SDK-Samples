// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using MapsPlayer.Builders;

namespace MapsPlayer
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private CameraMapObjectViewBuilder m_cameraMapObjectViewBuilder;
        private DoorMapObjectViewBuilder m_doorMapObjectViewBuilder;
        private PerformanceMapLayerBuilder m_performanceMapLayerBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_cameraMapObjectViewBuilder = new CameraMapObjectViewBuilder();
            m_cameraMapObjectViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_cameraMapObjectViewBuilder);

            m_performanceMapLayerBuilder = new PerformanceMapLayerBuilder();
            m_performanceMapLayerBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_performanceMapLayerBuilder);

            m_doorMapObjectViewBuilder = new DoorMapObjectViewBuilder();
            m_doorMapObjectViewBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_doorMapObjectViewBuilder);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_cameraMapObjectViewBuilder != null)
            {
                Workspace.Components.Unregister(m_cameraMapObjectViewBuilder);
                m_cameraMapObjectViewBuilder = null;
            }
        }

        #endregion Public Methods

    }
}