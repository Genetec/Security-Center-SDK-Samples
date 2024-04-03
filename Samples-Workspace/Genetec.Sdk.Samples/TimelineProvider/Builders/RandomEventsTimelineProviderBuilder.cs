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

    public class RandomEventsTimelineProviderBuilder : TimelineProviderBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{21b98367-d780-4320-bf99-feec875e546a}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The name of the Provider Builder.
        /// </summary>
        public override string Name => "Custom Timeline Provider Builder";

        /// <summary>
        /// The Title of the Provider builder.
        /// </summary>
        public override string Title => "Random events detection";

        /// <summary>
        /// The Unique Id of the Provider Builder.
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Method to create a provider.
        /// </summary>
        /// <returns>The created Provider.</returns>
        public override Genetec.Sdk.Workspace.Components.TimelineProvider.TimelineProvider CreateProvider()
        {
            var provider = new RandomEventsTimelineProvider();
            provider.Initialize(Workspace);
            return provider;
        }

        #endregion Public Methods

    }

}