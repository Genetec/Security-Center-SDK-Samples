// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using LogonProvider.Providers;

namespace LogonProvider
{
    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private MouseGestureLogonProvider m_mouseGestureLogonProvider;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            m_mouseGestureLogonProvider = new MouseGestureLogonProvider();
            m_mouseGestureLogonProvider.Initialize(Workspace);
            Workspace.Components.Register(m_mouseGestureLogonProvider);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_mouseGestureLogonProvider != null)
            {
                Workspace.Components.Unregister(m_mouseGestureLogonProvider);
                m_mouseGestureLogonProvider.Dispose();
                m_mouseGestureLogonProvider = null;
            }
        }

        #endregion Public Methods

    }
}