// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using Genetec.Sdk.Workspace.Pages;
using MapControl.Views;

namespace MapControl.Pages
{
    [Page(typeof(MapPageDescriptor))]
    public sealed class MapPage : Page
    {

        #region Private Fields

        private readonly MapPageView m_view = new MapPageView();

        #endregion Private Fields

        #region Protected Methods

        protected override void Initialize()
        {
            base.Initialize();

            m_view.Initialize(Workspace);
            View = m_view;
        }

        #endregion Protected Methods
    }

    public sealed class MapPageDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Maps";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{FEB576B6-3B90-4458-8FC7-E1644C12C70B}");

        #endregion Public Properties

    }
}