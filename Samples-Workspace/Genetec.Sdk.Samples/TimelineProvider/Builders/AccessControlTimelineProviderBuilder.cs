// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Workspace.Components.TimelineProvider;
using TimelineProvider.Providers;

namespace TimelineProvider.Builders
{

    public sealed class AccessControlTimelineProviderBuilder : TimelineProviderBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{48029C68-2A1E-4E76-A9EE-24BD2074A05D}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Access Control TimelineProviderBuilder";

        /// <summary>
        /// Gets the title of the provider if it must be display tin the user interface
        /// </summary>
        public override string Title => "Access Control events";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates the AccessControl timelineProvider.
        /// </summary>
        /// <returns>The created TimelineProvider.</returns>
        public override Genetec.Sdk.Workspace.Components.TimelineProvider.TimelineProvider CreateProvider()
        {
            var provider = new AccessControlTimelineProvider();
            provider.Initialize(Workspace);
            return provider;
        }

        #endregion Public Methods

    }

}