using Genetec.Sdk;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using System;
using System.Windows;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace CardholderAndCredentialStatusSample
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Fields

        private readonly Logger m_logger;

        #endregion

        #region Properties

        public CredentialStatus CredentialStatus { get; set; }
        public CardholderStatus CardholderStatus { get; set; }

        #endregion

        #region Constructors

        static MainWindow()
        {
            ActivateLogging();
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            m_logger = Logger.CreateInstanceLogger(this);
            // Register to important events
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggingOff += OnEngineLoggingOff;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            this.Closed += OnWindowClosed;

            // Logon to Sdk engine
            string server = "";
            string username = "admin";
            string password = "";
            m_sdkEngine.LoginManager.LogOn(server, username, password);

            CredentialStatus = new CredentialStatus();
            CredentialStatus.Initialize(m_sdkEngine);

            CardholderStatus = new CardholderStatus();
            CardholderStatus.Initialize(m_sdkEngine);
        }

        #endregion

        #region Event Handlers

        private void OnEngineLoggingOff(object sender, LoggingOffEventArgs e)
        {
            Console.WriteLine("Sdk is logging off");
            m_logger.TraceDebug("Sdk is logging off");
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine("Sdk has logged off");
            m_logger.TraceDebug("Sdk has logged off");
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + " has logged on to " + e.ServerName);
            m_logger.TraceDebug(e.UserName + " has logged on to " + e.ServerName);
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
            m_logger.TraceDebug(e.FormattedErrorMessage);
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Console.WriteLine("Server : " + e.ServerName + ". Status changed to " + e.Status);
            m_logger.TraceDebug("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            CredentialStatus.Dispose();
            CardholderStatus.Dispose();
        }

        #endregion

        #region Public Methods

        public static void ActivateLogging()
        {
            // Logs can be found in the Logs subfolder
            DiagnosticServer logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("CredentialStatus*", LogSeverity.Full) });
        }

        #endregion
    }

    #endregion
}

