// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using CaseManagement.Views;
using Genetec.Sdk.Workspace.Components.TileView;

namespace CaseManagement.Builders
{
    public sealed class CaseTileViewBuilder : TileViewBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{171E5B6E-BC8E-4086-9681-E74857C6000B}"));

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the name of the component
        /// </summary>
        public override string Name => "Case TileView builder";

        /// <summary>
        /// Gets the unique identifier of the component
        /// </summary>
        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create a view that will be inserted in the tile
        /// </summary>
        /// <returns>View to be displayed in the tile</returns>
        public override Genetec.Sdk.Workspace.Components.TileView.TileView CreateView() => new CaseTileView(Workspace);

        /// <summary>
        /// Gets a flag indicating if the specified content is supported by the tile viewer
        /// </summary>
        /// <param name="context">Tile plugin context wanting to be displayed</param>
        /// <returns>True if the content is supported; otherwise, false</returns>
        public override bool IsSupported(Genetec.Sdk.Workspace.Components.TilePluginContext context) => context.Content is CaseContent;

        #endregion Public Methods

    }
}