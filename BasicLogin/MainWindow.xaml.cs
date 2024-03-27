using Genetec.Sdk;
using System;
using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace BasicLogin
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constants

        public static readonly DependencyProperty IsSdkEngineConnectedProperty = DependencyProperty.Register(
                    "IsSdkEngineConnected", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        /// <summary>
        /// This dependency is to prevent the user from pressing multiple time on the button Logon.
        /// </summary>
        public static readonly DependencyProperty LogonAttemptProperty = DependencyProperty.Register(
                    "LogonAttempt", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty MessageConsoleTextProperty = DependencyProperty.Register(
                    "MessageConsoleText", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        /// <summary>
        /// The Engine of the Sdk.
        /// </summary>
        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Fields

        // Password of the said User.
        static string Password = "";

        // This is the server of the Directory.
        static string Server = "";

        // This is the Username for the connection. Must be a User in the directory.
        static string Username = "admin";

        #endregion

        #region Properties

        public bool IsSdkEngineConnected
        {
            get { return (bool)GetValue(IsSdkEngineConnectedProperty); }
            set { SetValue(IsSdkEngineConnectedProperty, value); }
        }

        public bool LogonAttempt
        {
            get { return (bool)GetValue(LogonAttemptProperty); }
            set { SetValue(LogonAttemptProperty, value); }
        }

        public string MessageConsoleText
        {
            get { return (string)GetValue(MessageConsoleTextProperty); }
            set { SetValue(MessageConsoleTextProperty, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Register to important events
            //This event will be raised when the Engine is logged on.
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            //This event will be raised when the Engine is logged off.
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            //This event is raised when the Engine logon failed.
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            //This event is raised when there is a status change on the Engine connection.
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            // The dataContext is used for the Dependency properties. If you set it to "this",
            // it means that this class will be the data context of the .xaml file. So when the UI looks
            // for a dependency property, it will search this class.
            DataContext = this;

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Log off from the Engine.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonLogOffClick(object sender, RoutedEventArgs e)
        {
            //Login off
            m_sdkEngine.LoginManager.LogOff();
        }

        /// <summary>
        /// This method initiate an Async logon to the Engine.
        /// </summary>
        /// <param name="sender">The Button</param>
        /// <param name="e">The Event</param>
        private void OnButtonLogOnAsyncClick(object sender, RoutedEventArgs e)
        {
            LogonAttempt = true;
            //The Async logon
            m_sdkEngine.LoginManager.BeginLogOn(Server, Username, Password);
        }

        /// <summary>
        /// Logon which is not Async. This logon will freeze the UI if it is on the UI thread.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event</param>
        private void OnButtonLogOnClick(object sender, RoutedEventArgs e)
        {
            LogonAttempt = true;
            // Doing a logon on the Engine
            m_sdkEngine.LoginManager.LogOn(Server, Username, Password);
        }

        /// <summary>
        /// Event raised when the Engine Logged off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
            PrintMessage("Sdk Engine has logged off");
        }

        /// <summary>
        /// Event raised when the Engine Successfully logged on.
        /// </summary>
        /// <param name="sender">The Engine</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            IsSdkEngineConnected = m_sdkEngine.LoginManager.IsConnected;
            LogonAttempt = false;
            PrintMessage(e.UserName + " has logged on to " + e.ServerName);
        }

        /// <summary>
        /// Event is raised by the Engine when the logon has failed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            LogonAttempt = false;
            PrintMessage(e.FormattedErrorMessage);
        }

        /// <summary>
        /// When the engine is trying to logon, this event will be raised when there is a status change.
        /// </summary>
        /// <param name="sender">The engine.</param>
        /// <param name="e">The event.</param>
        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            PrintMessage("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to print the message on the UI.
        /// </summary>
        private void PrintMessage(string message)
        {
            // Pint the message on the textbox console.
            MessageConsoleText += message + Environment.NewLine;
            // Scroll to the end of the TextBox.
            m_messageConsole.CaretIndex = MessageConsoleText.Length;
            m_messageConsole.ScrollToEnd();
        }

        #endregion
    }

    #endregion
}

