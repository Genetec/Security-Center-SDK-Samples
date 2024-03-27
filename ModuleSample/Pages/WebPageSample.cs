// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Pages;
using System;

namespace ModuleSample.Pages
{

    [Page(typeof(WebPageSampleDescriptor))]
    public class WebPageSample : Page
    {

        #region Public Constructors

        public WebPageSample()
        {
            View = new WebPageViewSample();
        }

        #endregion Public Constructors

        #region Protected Methods

        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        /// <remarks>At this step, the <see cref="Genetec.Sdk.Workspace.Workspace"/> is available.</remarks>
        protected override void Initialize()
        {
            ((WebPageViewSample)View).Initialize();
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            return null;
        }

        #endregion Protected Methods

    }

    public class WebPageSampleDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ModuleTest.CustomCategoryId;

        public override string Description => "This task opens a new page containing a browser.";

        public override TaskIconColor IconColor => TaskIconColor.VideoIconColor;

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Web page";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{12B0B1B6-3FB9-43FC-9461-A86043E7207C}");

        #endregion Public Properties

    }

}