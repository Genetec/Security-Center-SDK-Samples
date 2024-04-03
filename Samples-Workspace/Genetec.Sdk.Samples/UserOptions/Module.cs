// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using UserOptions.Extensions;

namespace UserOptions
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private MyOptionsExtension m_myOptionsExtension;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_myOptionsExtension = new MyOptionsExtension();
            m_myOptionsExtension.Initialize(Workspace);
            Workspace.Options.Register(m_myOptionsExtension);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_myOptionsExtension != null)
            {
                Workspace.Options.Unregister(m_myOptionsExtension);
                m_myOptionsExtension = null;
            }
        }

        #endregion Public Methods

    }
}