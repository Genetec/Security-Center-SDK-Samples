// ==========================================================================
// Copyright (C) 2018 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsServiceSample
{
    public partial class WindowsServiceSample : ServiceBase
    {
        #region Fields

        private readonly Logger m_logger;
        private static Engine m_sdkEngine = new Engine();
        private CustomFieldCreator m_customFieldCreator;

        #endregion Fields

        #region Public methods

        public WindowsServiceSample(string[] args)
        {
            ActivateLogging();
            InitializeComponent();
            m_logger = Logger.CreateInstanceLogger(this);
            string eventSourceName = "MySource";
            string logName = "WindowsServiceSampleLogs";
            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        #endregion Public methods

        #region Protected methods

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");

            Task.Factory.StartNew(InitializeEngine());
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In onStop.");

            m_sdkEngine.LoginManager.LogOff();
            Dispose();
        }

        #endregion Protected methods

        #region Private methods

        private Action InitializeEngine()
        {
            void Logon()
            {
                Console.WriteLine("Logging on...");

                // Register to important events
                m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
                m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
                m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
                m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

                m_sdkEngine.LoginManager.BeginLogOn("localhost", "admin", string.Empty);
            }

            return Logon;
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + " has logged on to " + e.ServerName);
            m_logger.TraceDebug(e.UserName + " has logged on to " + e.ServerName);

            m_customFieldCreator = new CustomFieldCreator(m_sdkEngine);
            m_customFieldCreator.CreateCustomFields();
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

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine("Sdk has logged off");
            m_logger.TraceDebug("Sdk has logged off");
        }

        private new void Dispose()
        {
            UnsubscribeEngine();
            m_customFieldCreator?.Dispose();
            m_sdkEngine.Dispose();
            m_sdkEngine = null;
            m_logger.Dispose();
            base.Dispose();
        }

        public static void ActivateLogging()
        {
            // Logs can be found in the Logs subfolder
            DiagnosticServer logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("WindowsServiceSample*", LogSeverity.Full) });
        }

        /// <summary>
        /// Unsubscribe from Engine events
        /// </summary>
        private void UnsubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn -= OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff -= OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed -= OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged -= OnEngineLogonStatusChanged;
        }

        #endregion Private methods
    }
}