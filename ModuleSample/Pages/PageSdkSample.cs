// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Pages;
using System;

namespace ModuleSample.Pages
{

    [Page(typeof(PageSdkSampleDescriptor))]
    public class PageSdkSample : Page
    {

        #region Private Fields

        private readonly PageSdkViewSample m_view = new PageSdkViewSample();

        #endregion Private Fields

        #region Public Constructors

        public PageSdkSample()
        {
            View = m_view;
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
        protected override void Initialize() => m_view.Initialize(Workspace);
       
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

    /// <summary>
    /// Describes the attributes of PageSdkSample.
    /// </summary>
    public class PageSdkSampleDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ModuleTest.CustomCategoryId;

        public override string Description 
            => "This task opens the Custom SDK page. " +
               "It shows the basic class structure to have a custom Page use the .Net SDK.  " +
               "Look inside ModuleTest to see how this page is created as a singleton.";

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Custom SDK Page";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{69E2B611-D6E6-4962-BDF7-831828AD0452}");

        #endregion Public Properties

    }

}