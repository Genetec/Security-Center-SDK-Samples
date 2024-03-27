// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Options;
using System.Windows;

namespace RMModule.Options
{

    public class ChatOptionsPage : OptionPage
    {

        #region Public Fields

        public static readonly DependencyProperty CanSaveProperty =
                            DependencyProperty.Register
                            ("CanSave", typeof(bool), typeof(ChatOptionsPage),
                            new PropertyMetadata(true, OnPropertiesChanged));

        public static readonly DependencyProperty ShowPopupProperty =
                            DependencyProperty.Register
                            ("ShowPopup", typeof(bool), typeof(ChatOptionsPage),
                            new PropertyMetadata(true, OnPropertiesChanged));

        #endregion Public Fields

        #region Private Fields

        private readonly ChatOptionsExtension m_extension;

        private readonly ChatOptionsPageView m_view;

        #endregion Private Fields

        #region Public Properties

        public bool CanSave
        {
            get => (bool)GetValue(CanSaveProperty);
            set => SetValue(CanSaveProperty, value);
        }

        public bool ShowPopup
        {
            get => (bool)GetValue(ShowPopupProperty);
            set => SetValue(ShowPopupProperty, value);
        }

        public override UIElement View => m_view;

        #endregion Public Properties

        #region Public Constructors

        public ChatOptionsPage(ChatOptionsExtension extension)
        {
            m_extension = extension;
            m_view = new ChatOptionsPageView(this);
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Load()
        {
            CanSave = m_extension.CanSave;
            ShowPopup = m_extension.ShowPopup;
        }

        public override void Save()
        {
            m_extension.CanSave = CanSave;
            m_extension.ShowPopup = ShowPopup;
        }

        // User selection is always valid in this sample. Return true to be able to save the config
        public override bool Validate() => true;
       
        #endregion Public Methods

        #region Private Methods

        private static void OnPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChatOptionsPage instance)
            {
                instance.OnModified();
            }
        }

        #endregion Private Methods
    }

}