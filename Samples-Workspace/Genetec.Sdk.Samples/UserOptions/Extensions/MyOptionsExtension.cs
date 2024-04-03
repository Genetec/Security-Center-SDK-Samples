// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Entities.Utilities;
using Genetec.Sdk.Workspace.Options;
using UserOptions.Configuration;
using UserOptions.Pages;

namespace UserOptions.Extensions
{
    public sealed class MyOptionsExtension : OptionsExtension
    {

        #region Public Fields

        public const string ExtensionName = "MyOptions";
        public static string ColorProperty = "Color";
        public static string DateTimeProperty = "DateTime";
        public static string IsCheckedProperty = "IsChecked";
        public static string NumOfItemsProperty = "NumOfItems";
        public MyOptionsConfiguration Config;

        #endregion Public Fields

        #region Private Fields

        private const string Filename = "UserOptionsConfiguration.json";
        private static readonly ImageSource s_icon;

        #endregion Private Fields

        #region Public Events

        public event EventHandler<DependencyPropertyChangedEventArgs> Modified;

        #endregion Public Events

        #region Public Properties

        public Color Color
        {
            get => (Color)this[ColorProperty];
            set => this[ColorProperty] = value;
        }

        public DateTime DateTime
        {
            get => (DateTime)this[DateTimeProperty];
            set => this[DateTimeProperty] = value;
        }

        public override ImageSource Icon => s_icon;

        public bool IsChecked
        {
            get => (bool)this[IsCheckedProperty];
            set => this[IsCheckedProperty] = value;
        }

        public override string Name => ExtensionName;

        public int NumOfItems
        {
            get => (int)this[NumOfItemsProperty];
            set => this[NumOfItemsProperty] = value;
        }
        public override string PageTitle => "Custom Page Title";
        public override string Title => "My Options";

        #endregion Public Properties

        #region Public Constructors

        static MyOptionsExtension()
        {
            s_icon = new BitmapImage(new Uri("pack://application:,,,/UserOptions;Component/Resources/options.png"));
            s_icon.Freeze();
        }

        public MyOptionsExtension()
        {
            RegisterProperty(NumOfItemsProperty, typeof(int), 12);
            RegisterProperty(DateTimeProperty, typeof(DateTime), DateTime.Now);
            RegisterProperty(IsCheckedProperty, typeof(bool), false);
            RegisterProperty(ColorProperty, typeof(Color), Colors.Black);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Initialize()
        {
            base.Initialize();
            AddOptionPage(new MyOptionsPage(this));
        }

        protected override void Load()
        {
            NumOfItems = LoadConfiguration().NumOfItems;
            DateTime = LoadConfiguration().DateTime;
            IsChecked = LoadConfiguration().IsChecked;
            Color = LoadConfiguration().Color;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            OnModified(e);
        }

        protected override void Save()
        {
            SaveConfiguration();
        }

        #endregion Protected Methods

        #region Private Methods

        private static IsolatedStorageFile GetStorageFile()
        {
            return IsolatedStorageFile.GetStore(IsolatedStorageScope.Roaming | IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        }

        private MyOptionsConfiguration LoadConfiguration()
        {
            IsolatedStorageFile storage = GetStorageFile();
            if (storage == null || !storage.FileExists(Filename))
            {
                return new MyOptionsConfiguration();
            }

            using (var stream = storage.OpenFile(Filename, FileMode.Open))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                Config = SerializationHelper.DeserializeFromString<MyOptionsConfiguration>(reader.ReadString());

                return Config;
            }
        }
        private void OnModified(DependencyPropertyChangedEventArgs e) => Modified?.Invoke(this, e);

        private void SaveConfiguration()
        {
            Config = new MyOptionsConfiguration
            {
                NumOfItems = NumOfItems,
                DateTime = DateTime,
                IsChecked = IsChecked,
                Color = Color
            };

            IsolatedStorageFile storage = GetStorageFile();
            if (storage == null) return;

            using (var stream = storage.CreateFile(Filename))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(SerializationHelper.SerializeToString(Config));
            }
        }

        #endregion Private Methods
    }
}