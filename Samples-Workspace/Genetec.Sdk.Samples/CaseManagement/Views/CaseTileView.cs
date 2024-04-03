// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.TileView;

namespace CaseManagement.Views
{
    public sealed class CaseTileView : TileView
    {

        #region Private Fields

        private readonly CaseTileViewer m_view;

        #endregion Private Fields

        #region Public Properties

        public override System.Windows.UIElement View => m_view;

        #endregion Public Properties

        #region Public Constructors

        public CaseTileView(Genetec.Sdk.Workspace.Workspace workspace) => m_view = new CaseTileViewer(workspace);

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Update the content displayed in the tile viewer's component
        /// </summary>
        public override void Update(Genetec.Sdk.Workspace.Components.TilePluginContext context) => m_view.Update(context.Content as CaseContent);

        #endregion Public Methods

    }
}