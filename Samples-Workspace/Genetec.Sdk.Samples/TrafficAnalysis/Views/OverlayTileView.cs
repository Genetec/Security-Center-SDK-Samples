// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.TileView;
using Genetec.Sdk.Workspace.Pages.Contents;

namespace TrafficAnalysis.Views
{
    public sealed class OverlayTileView : TileView
    {

        #region Private Fields

        private readonly OverlayTileViewer m_view;

        #endregion Private Fields

        #region Public Properties

        public override System.Windows.UIElement View => m_view;

        #endregion Public Properties

        #region Public Constructors

        public OverlayTileView(Genetec.Sdk.Workspace.Workspace workspace)
        {
            Placement = Placement.OverVideo;
            m_view = new OverlayTileViewer(workspace);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Update(TilePluginContext context) => m_view.Update(context.Content as VideoContent);

        #endregion Public Methods
    }
}