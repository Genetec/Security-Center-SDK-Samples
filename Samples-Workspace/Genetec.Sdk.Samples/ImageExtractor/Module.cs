// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using ImageExtractor.Extractors;

namespace ImageExtractor
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private GenetecImageExtractor m_genetecImageExtractor;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_genetecImageExtractor = new GenetecImageExtractor();
            m_genetecImageExtractor.Initialize(Workspace);
            Workspace.Components.Register(m_genetecImageExtractor);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_genetecImageExtractor != null)
            {
                Workspace.Components.Unregister(m_genetecImageExtractor);
                m_genetecImageExtractor = null;
            }
        }

        #endregion Public Methods

    }
}