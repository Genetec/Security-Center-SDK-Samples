// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using MapControl.Pages;

namespace MapControl
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private Genetec.Sdk.Workspace.Tasks.Task m_task;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_task = new Genetec.Sdk.Workspace.Tasks.CreatePageTask<MapPage>();
            m_task.Initialize(Workspace);
            Workspace.Tasks.Register(m_task);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_task != null)
            {
                Workspace.Tasks.Unregister(m_task);
                m_task.Dispose();
                m_task = null;
            }
        }

        #endregion Public Methods

    }
}