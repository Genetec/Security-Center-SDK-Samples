// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Monitors;
using Genetec.Sdk.Workspace.Pages;
using RMModule.Options;
using RMModule.Services;
using RMSerialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace RMModule.Pages
{

    [Page(typeof(ChatPageDescriptor))]
    public class ChatPage : Page
    {

        #region Private Fields

        private readonly ChatPageView m_view = new ChatPageView();

        private bool m_canSaveValue = true;

        #endregion Private Fields

        #region Protected Methods

        /// <summary>
        /// Gets if the page can be saved as a Public/Private task.
        /// </summary>
        /// <returns>True if the page can be saved; Otherwise, false.</returns>
        protected override bool CanSave()
        {
            if (m_view == null)
                return false;

            return m_canSaveValue;
        }

        /// <summary>
        /// Deserializes the data contained by the specified byte array.
        /// </summary>
        /// <param name="data">A byte array that contains the data.</param>
        protected override void Deserialize(byte[] data)
        {
            if (data == null)
            {
                return;
            }

            var pageData = PageData.Deserialize(data);
            if (pageData != null)
            {
                m_view.ReceivedText = pageData.m_message;
            }
        }

        protected override void Initialize()
        {
            View = m_view;
            m_view.Initialize(Workspace);
        }

        protected override void OnActivated(Monitor monitor)
        {
            var receiver = Workspace.Services.Get<IChatService>();
            receiver.MessagesReceived += ShowMessageUi;
            ShowMessageUi(this, receiver.GetMessages());

            var chatOption = Workspace.Options[ChatOptionsExtension.ExtensionName];
            if (chatOption != null)
            {
                m_canSaveValue = (bool)chatOption[ChatOptionsExtension.CanSaveProperty];
            }
        }

        protected override void OnDeactivated(Monitor monitor)
        {
            var receiver = Workspace.Services.Get<IChatService>();
            receiver.MessagesReceived -= ShowMessageUi;
        }

        /// <summary>
        /// Serializes the data to a byte array.
        /// </summary>
        /// <returns>A byte array that contains the data.</returns>
        protected override byte[] Serialize()
        {
            if (m_view != null)
            {
                PageData pageData = new PageData {m_message = m_view.ReceivedText};

                return pageData.Serialize();
            }

            return null;
        }

        #endregion Protected Methods

        #region Private Methods

        private void ShowMessageUi(object sender, List<ChatMessage> msg)
        {
            foreach (var chatMessage in msg)
            {
                m_view.AddSendGuid(chatMessage.AppGuid);
                m_view.ReceivedText += (chatMessage.AppGuid == Workspace.Sdk.ClientGuid ? "Me : " : "Them : ") + chatMessage + "\n";
            }
            m_view.textboxRec.ScrollToEnd();
        }

        #endregion Private Methods

        #region Public Classes

        /// <summary>
        /// Class that contains the data that needs to be persisted for the page.
        /// </summary>
        [Serializable]
        [XmlRoot(ElementName = "PageData")]
        public class PageData
        {

            #region Public Fields

            public string m_message;

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

    public class ChatPageDescriptor : PageDescriptor
    {

        #region Public Properties

        /// <summary>
        /// Gets the page's task group to which it is associated.
        /// </summary>
        public override Guid CategoryId => ChatModule.CustomCategoryId;

        public override string Description => "This page from Request Manager Module sample demonstrates communication with a stand-alone SDK application.";

        /// <summary>
        /// Gets the page's default name.
        /// </summary>
        public override string Name => "Chat page";

        /// <summary>
        /// Gets the page's unique ID.
        /// </summary>
        public override Guid Type => new Guid("{A63065C7-98A1-4628-9676-07B2CFFA9C33}");

        #endregion Public Properties

    }

}