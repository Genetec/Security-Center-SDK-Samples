// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using TimelineProvider.Builders;

namespace TimelineProvider
{

    public sealed class Module : Genetec.Sdk.Workspace.Modules.Module
    {

        #region Private Fields

        private AccessControlTimelineProviderBuilder m_accessControlTimelineProviderBuilder;
        private RandomEventsTimelineProviderBuilder m_randomEventsTimelineProviderBuilder;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            // The Random Events TimelineProvider
            m_randomEventsTimelineProviderBuilder = new RandomEventsTimelineProviderBuilder();
            m_randomEventsTimelineProviderBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_randomEventsTimelineProviderBuilder);

            // The Access Control TimelineProvider
            m_accessControlTimelineProviderBuilder = new AccessControlTimelineProviderBuilder();
            m_accessControlTimelineProviderBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_accessControlTimelineProviderBuilder);
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (m_randomEventsTimelineProviderBuilder != null)
            {
                Workspace.Components.Unregister(m_randomEventsTimelineProviderBuilder);
                m_randomEventsTimelineProviderBuilder = null;
            }

            if (m_accessControlTimelineProviderBuilder != null)
            {
                Workspace.Components.Unregister(m_accessControlTimelineProviderBuilder);
                m_accessControlTimelineProviderBuilder = null;
            }
        }

        #endregion Public Methods

    }

}