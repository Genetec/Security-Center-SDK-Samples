// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Pages;
using System;

namespace ModuleSample.Pages
{

    [Page(typeof(SdkControlsPageDescriptor))]
    public class SdkControlsPage : Page
    {

        #region Protected Methods

        protected override void Deserialize(byte[] data)
        {
        }

        protected override void Initialize()
        {
            View = new SdkControlsPageView();
        }

        protected override byte[] Serialize()
        {
            return null;
        }

        #endregion Protected Methods

    }

    public class SdkControlsPageDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ModuleTest.CustomCategoryId;

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Sdk controls";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{6D7392A0-BD0A-4683-B85F-725C6CB21288}");

        #endregion Public Properties

    }

}