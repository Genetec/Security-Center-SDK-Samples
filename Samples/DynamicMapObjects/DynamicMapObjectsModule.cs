using DynamicMapObjects.Components;
using DynamicMapObjects.Maps;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Services;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace DynamicMapObjects
{
    #region Classes

    /// <summary>
    /// Main entry point for the module. Creates the workspace module components and registers/unregisters them.
    /// </summary>
    public sealed class DynamicMapObjectsModule : Module
    {
        #region Fields

        private OfficerMapObjectProvider m_officerMapObjectProvider;

        #endregion

        #region Event Handlers

        void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
            var mapService = Workspace.Services.Get<IMapService>();
            if (mapService != null)
            {
                mapService.RegisterLayer(new LayerDescriptor(OfficerMapObject.OfficerLayerId, OfficerMapObjectView.OfficerLayerName));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            RegisterComponents();
            if (Workspace != null)
            {
                Workspace.Initialized += OnWorkspaceInitialized;
            }
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (Workspace != null)
            {
                UnregisterComponents();
            }
        }

        #endregion

        #region Private Methods

        private void RegisterComponents()
        {
            m_officerMapObjectProvider = new OfficerMapObjectProvider(Workspace);
            Workspace.Components.Register(m_officerMapObjectProvider);
        }

        private void UnregisterComponents()
        {
            if (m_officerMapObjectProvider != null)
            {
                Workspace.Components.Unregister(m_officerMapObjectProvider);
                m_officerMapObjectProvider.Dispose();
                m_officerMapObjectProvider = null;
            }

        }

        #endregion
    }

    #endregion
}

