using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace OverlaySample
{
    #region Classes

    public partial class LoginWindow
    {
        #region Constants

        // Dependency property for the Directory field.
        public static readonly DependencyProperty DirectoryProperty =
                    DependencyProperty.Register("Directory", typeof(string), typeof(LoginWindow), new PropertyMetadata("localhost"));

        // Dependency property for the Username field.
        public static readonly DependencyProperty UsernameProperty =
                    DependencyProperty.Register("Username", typeof(string), typeof(LoginWindow), new PropertyMetadata("admin"));

        #endregion

        #region Properties

        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        #endregion

        #region Constructors

        public LoginWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            App.Current.Sdk.LoginManager.BeginLogOn(Directory, Username, passwordBox.Password);
            DialogResult = true;
        }

        #endregion
    }

    #endregion
}

