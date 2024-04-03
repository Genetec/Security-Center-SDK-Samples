// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Pages;
using System;
using System.IO;
using System.Xml.Serialization;

namespace ModuleSample.Pages
{

    [Page(typeof(PagePersistenceSampleDescriptor))]
    public class PagePersistenceSample : Page
    {

        #region Private Fields

        private readonly PagePersistenceViewSample m_view = new PagePersistenceViewSample();

        #endregion Private Fields

        #region Protected Methods

        /// <summary>
        /// Gets if the page can be saved as a Public/Private task.
        /// </summary>
        /// <returns>True if the page can be saved; Otherwise, false.</returns>
        protected override bool CanSave() => m_view != null && m_view.CanSaveAs;
       
        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
            if (data == null)
                return;

            var pageData = PageData.Deserialize(data);
            if (pageData != null)
            {
                m_view.Message = pageData.Message;
            }
        }

        /// <summary>
        /// Initialize the page.
        /// </summary>
        /// <remarks>At this step, the <see cref="Genetec.Sdk.Workspace.Workspace"/> is available.</remarks>
        protected override void Initialize()
        {
            View = m_view;
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            if (m_view != null)
            {
                var pageData = new PageData {Message = m_view.Message};

                return pageData.Serialize();
            }

            return null;
        }

        #endregion Protected Methods

        #region Public Classes

        /// <summary>
        /// Class that contains the data that needs to be persisted for the page.
        /// </summary>
        [Serializable]
        public class PageData
        {

            #region Public Fields

            public string Message;

            #endregion Public Fields

            #region Public Methods

            /// <summary>
            /// Converts the byte array to the PageData.
            /// </summary>
            /// <param name="serializedData"></param>
            /// <returns></returns>
            public static PageData Deserialize(byte[] serializedData)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        var serializer = new XmlSerializer(typeof(PageData));
                        ms.Write(serializedData, 0, serializedData.Length);
                        ms.Seek(0, SeekOrigin.Begin);
                        var obj = (PageData)serializer.Deserialize(ms);

                        return obj;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return null;
            }

            /// <summary>
            /// Convert the data to a byte array.
            /// </summary>
            public byte[] Serialize()
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        var serializer = new XmlSerializer(typeof(PageData));
                        serializer.Serialize(ms, this);

                        return ms.ToArray();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return null;
            }

            #endregion Public Methods

        }

        #endregion Public Classes

    }

    /// <summary>
    /// Describes the attributes of PagePersistenceSample.
    /// </summary>
    public class PagePersistenceSampleDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ModuleTest.CustomCategoryId;

        public override string Description => "A sample page showing how data can be persisted to be retrieved when reopening the page.";

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Custom persistence Page";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{F8615F5D-5BB7-47B8-ADE1-9A2996B331BB}");

        #endregion Public Properties

    }

}