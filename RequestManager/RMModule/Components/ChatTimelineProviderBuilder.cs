// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.TimelineProvider;
using System;

namespace RMModule.Components
{

    public sealed class ChatTimelineProviderBuilder : TimelineProviderBuilder
    {

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "ChatTimelineProviderBuilder";

        /// <summary>
        /// Gets the title of the provider if it must be display tin the user interface
        /// </summary>
        public override string Title => "Chat events";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => new Guid("{01867B7D-DB26-46D0-9648-714C1E770CFA}");

        #endregion Public Properties

        #region Public Methods

        public override TimelineProvider CreateProvider()
        {
            var provider = new ChatTimelineProvider();
            provider.Initialize(Workspace);
            return provider;
        }

        #endregion Public Methods

    }

}