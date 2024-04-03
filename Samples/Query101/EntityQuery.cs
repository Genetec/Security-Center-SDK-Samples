using Genetec.Sdk;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Data;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace EntityQuery
{
    #region Classes

    public partial class EntityQuery : Form
    {
        #region Constants

        /// <summary>
        /// Represents the Sdk engine.
        /// </summary>
        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the class EntityQuery.
        /// </summary>
        public EntityQuery()
        {
            InitializeComponent();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggingOff += OnEngineLoggingOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event raised when the Button Async query is clicked.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonAsyncQueryClick(object sender, EventArgs e)
        {
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.Alarm);
            query.Name = "a";
            query.NameSearchMode = StringSearchMode.StartsWith;
            query.BeginQuery(OnQueryCompleted, query);
        }

        /// <summary>
        /// Event raised when the button Logoff is clicked.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonLogOffClick(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.LogOff();
        }

        /// <summary>
        /// Event raised when the Button Logon is clicked.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonLogOnClick(object sender, EventArgs e)
        {
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                m_sdkEngine.LoginManager.LogOn("", "admin", "");
                ChangeButtonState(true);
            }
        }

        /// <summary>
        /// On button Query Async clicked.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonQuerySyncClick(object sender, EventArgs e)
        {
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.Alarm);
            query.Name = "a";
            query.NameSearchMode = StringSearchMode.StartsWith;
            QueryCompletedEventArgs result = query.Query();

            if (result.Success)
            {
                m_console.Text += string.Format("Found {0} entities\r\n", result.Data.Rows.Count);
                foreach (DataRow dr in result.Data.Rows)
                {
                    m_console.Text += string.Format("\t{0}\r\n", dr[0]);
                }
            }
            else
            {
                m_console.Text += "The query has failed";
            }
        }

        /// <summary>
        /// Event raised by the Engine when a directory certificate validation is required.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            DialogResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                  "The certificate is not from a trusted certifying authority. \n" +
                                                  "Do you trust this server?", "Secure Communication", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        /// <summary>
        /// Event raised when the Engine logged off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            m_console.Text += "OnEngineLoggedOff\r\n";
            ChangeButtonState(false);
        }

        /// <summary>
        /// Event raised when the Engine successfully logged on.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_console.Text += "OnEngineLoggedOn\r\n";
            ChangeButtonState(true);
        }

        /// <summary>
        /// Event raised when the Engine is logging off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggingOff(object sender, LoggingOffEventArgs e)
        {
            m_console.Text += "OnEngineLoggingOff\r\n";
        }

        /// <summary>
        /// Event raised when the Engine logon failed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            m_console.Text += e.FormattedErrorMessage + "\r\n";
        }

        /// <summary>
        /// Event raised when the Engine logon status changed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            m_console.Text += string.Format("OnEngineLogonStatusChanged: {0} on {1}\r\n", e.Status, e.ServerName);
        }

        /// <summary>
        /// Event raised when the Query is completed.
        /// </summary>
        /// <param name="result">The result of the query.</param>
        void OnQueryCompleted(IAsyncResult result)
        {
            var query = result.AsyncState as EntityConfigurationQuery;

            if (query == null)
            {
                return;
            }

            var queryResult = query.EndQuery(result);

            //This method invoker is useful because we are modifying UI Controls on a worker thread.
            BeginInvoke(new Action(() => PostQueryResults(queryResult)));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// The button state for the UI. It will enable/disable accordingly to the connection with the Engine.
        /// </summary>
        /// <param name="enable">Enable or disable. Enable is the value is true.</param>
        private void ChangeButtonState(bool enable)
        {
            m_asyncQuery1.Enabled = enable;
            m_querySync.Enabled = enable;
            m_logOn.Enabled = !enable;
            m_logOff.Enabled = enable;
        }

        /// <summary>
        /// Post the result of the query.
        /// </summary>
        /// <param name="queryResult">The results of the query.</param>
        private void PostQueryResults(QueryCompletedEventArgs queryResult)
        {
            if (queryResult.Success)
            {
                m_console.Text += string.Format("Found {0} entities\r\n",
                    queryResult.Data.Rows.Count);
                foreach (DataRow dr in queryResult.Data.Rows)
                {
                    m_console.Text += string.Format("\t{0}\r\n", dr[0]);
                }
            }
            else
            {
                m_console.Text += "The query has failed";
            }
        }

        #endregion
    }

    #endregion
}

