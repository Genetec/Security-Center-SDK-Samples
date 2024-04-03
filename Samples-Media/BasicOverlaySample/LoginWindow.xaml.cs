using Genetec.Sdk;
using System.Windows;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 13:
//  379 – Yax Nuun Ahiin I is crowned as 15th Ajaw of Tikal
//  1584 – San Lorenzo del Escorial Palace in Madrid is finished.
//  1989 – Largest anti-Apartheid march in South Africa, led by Desmond Tutu.
// ==========================================================================
namespace BasicOverlaySample
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

        private readonly Engine m_engine;

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

        public LoginWindow(Engine engine)
        {
            m_engine = engine;
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnLoginClicked(object sender, RoutedEventArgs e)
        {
            m_engine.LoginManager.BeginLogOn(Directory, Username, passwordBox.Password);
            DialogResult = true;
        }

        #endregion
    }

    #endregion
}

