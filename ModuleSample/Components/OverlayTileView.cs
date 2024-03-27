// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.TileView;
using Genetec.Sdk.Workspace.Pages.Contents;
using System;

namespace ModuleSample.Components
{

    public sealed class OverlayTileView : TileView
    {

        #region Private Fields

        private readonly OverlayTileViewerView m_view;

        #endregion Private Fields

        #region Public Properties

        public override System.Windows.UIElement View => m_view;

        #endregion Public Properties

        #region Public Constructors

        public OverlayTileView(Workspace workspace)
        {
            Placement = Placement.Extended; // Takes the full tile
            m_view = new OverlayTileViewerView(workspace);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(TilePluginContext context) => m_view.Update(context.Content as VideoContent);
        
        #endregion Public Methods

    }

    public sealed class OverlayTileViewBuilder : TileViewBuilder
    {

        #region Public Properties

        public override string Name => "Overlay tile viewer";

        public override Guid UniqueId => new Guid("{7E403264-6EA4-438B-9C8E-10520990DDBC}");

        #endregion Public Properties

        #region Public Methods

        public override TileView CreateView() => new OverlayTileView(Workspace);
        
        public override bool IsSupported(TilePluginContext context) => context.Content is VideoContent;
        
        #endregion Public Methods

    }

}