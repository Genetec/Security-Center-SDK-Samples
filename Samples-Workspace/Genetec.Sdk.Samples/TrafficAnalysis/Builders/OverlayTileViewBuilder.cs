// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.TileView;
using Genetec.Sdk.Workspace.Pages.Contents;
using TrafficAnalysis.Views;

namespace TrafficAnalysis.Builders
{
    public sealed class OverlayTileViewBuilder : TileViewBuilder
    {

        #region Private Fields

        private readonly Lazy<Guid> m_uniqueLazyId = new Lazy<Guid>(() => new Guid("{7B3F4C36-8636-4FF4-8183-3D3EBDF9C9E6}"));

        #endregion Private Fields

        #region Public Properties

        public override string Name => "Overlay tile viewer";

        public override Guid UniqueId => m_uniqueLazyId.Value;

        #endregion Public Properties

        #region Public Methods

        public override TileView CreateView() => new OverlayTileView(Workspace);

        public override bool IsSupported(TilePluginContext context) => context.Content is VideoContent;

        #endregion Public Methods
    }
}