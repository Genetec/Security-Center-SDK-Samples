using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Services;
using VisualMapObject.Components;
using VisualMapObject.Maps;
using VisualMapObject.Maps.Layers;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject
{
    /// <summary>
    /// The module class purpose is to register ours components, layers, objects, tasks, pages...
    /// </summary>
    public class VisualMapObjectModule : Module
    {
        /// <summary>
        /// Accident components this is mainly to compare against visual map object.
        /// </summary>
        private AccidentMapObjectProvider m_accidentMapObjectProvider;
        private AccidentMapObjectBuilder m_accidentMapObjectBuilder;

        /// <summary>
        /// Visuals map objects.
        /// </summary>
        private CameraVisualBuilder m_cameraVisualBuilder;

        /// <summary>
        /// Visual map object requires layer so it needs its factory to create an appropriate layer.
        /// </summary>
        private CameraVisualLayerBuilder m_cameraVisualLayerBuilder;

        /// <summary>
        /// Methods called when the workspace have been initialized.
        /// This mean the the user is login.
        /// Once we are log in and everything is setup, we can register our layer on wich the accident will happen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
            var mapService = Workspace.Services.Get<IMapService>();
            if (mapService != null)
            {
                mapService.RegisterLayer(new LayerDescriptor(AccidentMapObjectView.AccidentsLayerId, "Accidents"));
            }
        }

        /// <summary>
        /// Methods called for the module to loads and register it's components.
        /// </summary>
        public override void Load()
        {
            if (Workspace != null)
            {
                Workspace.Initialized += OnWorkspaceInitialized;
            }
            RegisterComponents();
        }

        /// <summary>
        /// Methods called for the module to unloads and register it's components.
        /// </summary>
        public override void Unload()
        {
            if (Workspace != null)
            {
                Workspace.Initialized -= OnWorkspaceInitialized;
            }
            UnregisterComponents();
        }

        /// <summary>
        /// This methods register the components that this module require to perform it's actions.
        /// </summary>
        private void RegisterComponents()
        {
            //Components
            m_accidentMapObjectProvider = new AccidentMapObjectProvider();
            m_accidentMapObjectProvider.Initialize(Workspace);
            Workspace.Components.Register(m_accidentMapObjectProvider);

            m_accidentMapObjectBuilder = new AccidentMapObjectBuilder();
            m_accidentMapObjectBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_accidentMapObjectBuilder);

            //Visuals
            m_cameraVisualLayerBuilder = new CameraVisualLayerBuilder();
            m_cameraVisualLayerBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_cameraVisualLayerBuilder);

            m_cameraVisualBuilder = new CameraVisualBuilder();
            m_cameraVisualBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_cameraVisualBuilder);
        }

        /// <summary>
        /// Unregister and free memory.
        /// The module was unloaded we don't need this anymore.
        /// </summary>
        private void UnregisterComponents()
        {
            if (m_accidentMapObjectProvider != null)
            {
                Workspace.Components.Unregister(m_accidentMapObjectProvider);
                m_accidentMapObjectProvider.Dispose();
                m_accidentMapObjectProvider = null;
            }

            if (m_accidentMapObjectBuilder != null)
            {
                Workspace.Components.Unregister(m_accidentMapObjectBuilder);
                m_accidentMapObjectBuilder = null;
            }

            if (m_cameraVisualLayerBuilder != null)
            {
                Workspace.Components.Unregister(m_cameraVisualLayerBuilder);
                m_cameraVisualLayerBuilder = null;
            }

            if (m_cameraVisualBuilder != null)
            {
                Workspace.Components.Unregister(m_cameraVisualBuilder);
                m_cameraVisualBuilder = null;
            }
        }
    }
}
