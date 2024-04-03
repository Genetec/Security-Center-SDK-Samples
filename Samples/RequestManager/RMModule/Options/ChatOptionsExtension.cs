// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Options;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RMModule.Options
{

    public sealed class ChatOptionsExtension : OptionsExtension
    {

        #region Public Fields

        public const string CanSaveProperty = "CanSave";

        public const string ExtensionName = "Chat";

        public const string ShowPopupProperty = "ShowPopup";

        #endregion Public Fields

        #region Private Fields

        private const string Filename = "ChatOptions.xml";

        #endregion Private Fields

        #region Public Properties

        public bool CanSave
        {
            get => (bool)this[CanSaveProperty];
            set => this[CanSaveProperty] = value;
        }

        public override ImageSource Icon => new BitmapImage(new Uri(@"pack://application:,,,/RMModule;component/Resources/Chat_message.png", UriKind.RelativeOrAbsolute));

        public override string Name => ExtensionName;

        public bool ShowPopup
        {
            get => (bool)this[ShowPopupProperty];
            set => this[ShowPopupProperty] = value;
        }

        public bool StoreLocally => false;

        public override string Title => "Chat";

        #endregion Public Properties

        #region Public Constructors

        public ChatOptionsExtension()
        {
            RegisterProperty(CanSaveProperty, typeof(bool), true);
            RegisterProperty(ShowPopupProperty, typeof(bool), true);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Create a store using the options assembly and user
        /// </summary>
        /// <returns>Created store</returns>
        public static IsolatedStorageFile GetStore()
        {
            var store = IsolatedStorageFile.GetStore(IsolatedStorageScope.Assembly | IsolatedStorageScope.User, null, null);
            return store;
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Initialize()
        {
            base.Initialize();
            AddOptionPage(new ChatOptionsPage(this));
        }

        /// <summary>
        /// Retrieves the values persisted through the Save() method.
        /// </summary>
        protected override void Load()
        {
            var store = GetStore();
            if ((store != null) && store.FileExists(Filename))
            {
                using (var stream = store.OpenFile(Filename, FileMode.Open))
                using (var br = new BinaryReader(stream))
                {
                    CanSave = br.ReadBoolean();
                    ShowPopup = br.ReadBoolean();
                }
            }
        }

        /// <summary>
        /// Defines the way in which the options will be persisted on your system
        /// In this case, the options are saved in a XML file in a store given by the GetStore() method
        /// </summary>
        protected override void Save()
        {
            var store = GetStore();
            if (store != null)
            {
                using (var stream = store.CreateFile(Filename))
                using (var bw = new BinaryWriter(stream))
                {
                    bw.Write(CanSave);
                    bw.Write(ShowPopup);
                }
            }
        }

        #endregion Protected Methods

    }

}