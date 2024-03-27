// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Pages;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModuleSample.Pages
{

    [Page(typeof(PageSampleDescriptor))]
    public class PageSample : Page
    {

        #region Private Fields

        private readonly PageViewSample m_view = new PageViewSample();

        #endregion Private Fields

        #region Public Constructors

        public PageSample()
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
        protected override void Initialize()
        {
            m_view.Initialize(Workspace, this);
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

    public class PageSampleDescriptor : PageDescriptor
    {

        #region Public Fields

        /// <summary>
        /// The privilege that needs to be allowed in order to execute the task, as specified in ModuleSample.privileges.xml.
        /// </summary>
        public const string Privilege = "{D1EE90DF-88CC-4ABF-A92E-1B0F57F8CF79}";

        #endregion Public Fields

        #region Private Fields

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ModuleTest.CustomCategoryId;

        /// <summary>
        /// Gets the page's description.
        /// </summary>
        public override string Description => "This page interacts with a Workspace SDK service. It shows how a page can get or set its name and get information on the host monitor.";

        /// <summary>
        /// Gets the icon representing the page.
        /// </summary>
        /// <remarks>Optimal resolution is 16x16.</remarks>
        public override ImageSource Icon { get; }

        /// <summary>
        /// Gets the icon's color.
        /// </summary>
        public override TaskIconColor IconColor => TaskIconColor.AccessControlIconColor;

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Custom personalized page";

        /// <summary>
        /// Gets the thumbnail representing the page.
        /// </summary>
        /// <remarks>Optimal resolution is 256x256.</remarks>
        public override ImageSource Thumbnail { get; }

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{12B0B1C7-3FB9-43FC-9461-A86043E7207C}");

        #endregion Public Properties

        #region Public Constructors

        public PageSampleDescriptor()
        {
            Icon = new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Task - small.png", UriKind.RelativeOrAbsolute));
            Thumbnail = new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Task.png", UriKind.RelativeOrAbsolute));
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Gets if the current user has the privilege to see the page.
        /// </summary>
        /// <returns>True if allowed; Otherwise, false.</returns>
        public override bool HasPrivilege()
        {
            if (m_sdk.LoginManager.IsConnected)
            {
                return m_sdk.SecurityManager.IsPrivilegeGranted(new Guid(Privilege));
            }

            return false;
        }

        #endregion Public Methods

    }

}