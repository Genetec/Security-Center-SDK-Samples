using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Entity101
{
    #region Classes

    public partial class Entity101 : Form
    {
        #region Constants

        /// <summary>
        /// Password for the connection to the Engine.
        /// </summary>
        private static readonly string Password = string.Empty;

        /// <summary>
        /// Server for the connection to the Engine.
        /// </summary>
        private static readonly string Server = "localhost";

        /// <summary>
        /// Username for the connection to the Engine.
        /// </summary>
        private static readonly string Username = "Admin";

        /// <summary>
        /// Represent the SDK Engine.
        /// </summary>
        private readonly Engine m_sdkEngine = new Engine();

        /// <summary>
        /// Maximum of entity to create in this method : <see cref="OnButtonCreateClick"/>.
        /// </summary>
        private const int MaximumEntity = 100;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor for the class Entity101.
        /// </summary>
        public Entity101()
        {
            InitializeComponent();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.EntitiesInvalidated += OnEngineEntitiesInvalidated;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Event raised when the user clicks on the button to do a query. It will begin a query to the directory.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonAsyncQueryClick(object sender, EventArgs e)
        {
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.Alarm);
            query.Name = "a";
            query.NameSearchMode = StringSearchMode.StartsWith;

            // Begin the query.
            query.BeginQuery(OnQueryCompleted, query);
        }

        /// <summary>
        /// Creates the numbers of entities equals to the variable <see cref="MaximumEntity"/>.
        /// Event raised when the button Create query is clicked.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">The Event.</param>
        /// <remarks>
        /// We use a transaction to create all the entities at the same time.
        /// </remarks>
        private void OnButtonCreateClick(object sender, EventArgs e)
        {
            //Create Transaction
            m_sdkEngine.TransactionManager.CreateTransaction();

            //Fill the transaction
            for (int i = 0; i < MaximumEntity; ++i)
            {
                Alarm alarm = m_sdkEngine.CreateEntity(string.Format("Alarm_{0}", i), EntityType.Alarm) as Alarm;
                alarm.DistributionMode = AlarmDistributionMode.Simultaneous;
                alarm.Recipients.Add(m_sdkEngine.LoggedUser.Guid, TimeSpan.Zero);
            }

            //Send the transaction
            m_sdkEngine.TransactionManager.CommitTransaction();
        }

        /// <summary>
        /// Event raised when the button delete is clicked. It will delete all the entities which are Alarms.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            //Create the query.
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

            // A filter is set to find all the desired entities.
            query.EntityTypeFilter.Add(EntityType.Alarm);
            query.Name = "Alarm_";
            query.NameSearchMode = StringSearchMode.StartsWith;

            // Send the query.
            QueryCompletedEventArgs result = query.Query();

            // Delete them if successfully queried them.
            if ((result.Success) && (result.Data.Rows.Count > 0))
            {
                m_sdkEngine.TransactionManager.CreateTransaction();
                foreach (DataRow dr in result.Data.Rows)
                {
                    m_sdkEngine.DeleteEntity((Guid)dr[0]);
                }
                m_sdkEngine.TransactionManager.CommitTransaction();
            }
        }

        /// <summary>
        /// Event raised when the button Logoff is clicked.
        /// It will logoff the engine.
        /// </summary>
        /// <param name="sender">The button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonLogOffClick(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.LogOff();
        }

        /// <summary>
        /// Event raised when the button Logon is clicked.
        /// It will logon to the Engine.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonLogOnClick(object sender, EventArgs e)
        {
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                m_sdkEngine.LoginManager.LogOn(Server, Username, Password);
            }
        }

        /// <summary>
        /// Event raised when the button update is clicked.
        /// </summary>
        /// <param name="sender">The Button.</param>
        /// <param name="e">The Event.</param>
        private void OnButtonUpdateClick(object sender, EventArgs e)
        {
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            query.EntityTypeFilter.Add(EntityType.Alarm);
            query.Name = "a";
            query.NameSearchMode = StringSearchMode.StartsWith;
            QueryCompletedEventArgs result = query.Query();

            if (result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)
                {
                    Alarm alarm = m_sdkEngine.GetEntity((Guid)dr[0]) as Alarm;
                    if (alarm != null)
                    {
                        alarm.Name += "a";
                        alarm.Description += "a";
                    }
                    else
                    {
                        PrintMessage("GetEntity failed");
                    }
                }
            }
            else
            {
                PrintMessage("The query has failed");
            }
        }

        /// <summary>
        /// This event will be raised if a certificate validation is asked for.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            DialogResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                  "The certificate is not from a trusted certifying authority. \n" +
                                                  "Do you trust this server?", "Secure Communication", MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Question);
            e.AcceptDirectory = (result == DialogResult.Yes);
        }

        /// <summary>
        /// Event raised when there is an entity invalidated in the Engine.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineEntitiesInvalidated(object sender, EntitiesInvalidatedEventArgs e)
        {
            List<string> entitiesModified = new List<string>();
            foreach (EntityUpdateInfo entityUpdateInfo in e.Entities)
            {
                Entity entity = m_sdkEngine.GetEntity(entityUpdateInfo.EntityGuid);
                if (entity != null)
                {
                    entitiesModified.Add(string.Format("{0}, {1} was modified {2}", entity.Name, entityUpdateInfo.EntityType, e.IsLocalUpdate ? "locally" : "remotely"));
                }
            }

            //This method invoker is useful because we are modifying UI Controls on a worker thread.
            BeginInvoke(new Action(() =>
            {
                foreach (string entityModified in entitiesModified)
                {
                    PrintMessage(entityModified);
                }
            }));
        }

        /// <summary>
        /// Event raised when the engine is logged off.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            PrintMessage("Engine is now logged off.");
            ChangeButtonState(false);
        }

        /// <summary>
        /// Event raised when the Engine is logged on.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            PrintMessage("Engine is now logged on.");
            ChangeButtonState(true);
        }

        /// <summary>
        /// Event raised when the Engine logon as failed.
        /// </summary>
        /// <param name="sender">The Engine.</param>
        /// <param name="e">The Event.</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            PrintMessage(e.FormattedErrorMessage);
        }

        /// <summary>
        /// This event is raised when the query is completed.
        /// </summary>
        /// <param name="result">The result of the query.</param>
        private void OnQueryCompleted(IAsyncResult result)
        {
            var query = result.AsyncState as EntityConfigurationQuery;

            if (query == null)
                return;

            var queryResult = query.EndQuery(result);

            //This method invoker is useful because we are modifying UI Controls on a worker thread.
            BeginInvoke(new Action(() => PostQueryResults(queryResult)));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This method is for the UI control. It controls the states of the differents button to prevent them from being clicked when not logged in.
        /// </summary>
        /// <param name="enable">The boolean to change the property Enabled of the buttons.</param>
        private void ChangeButtonState(bool enable)
        {
            m_buttonCreate.Enabled = enable;
            m_buttonAsyncQuery.Enabled = enable;
            m_buttonDelete.Enabled = enable;
            m_buttonUpdate.Enabled = enable;
            m_buttonLogOff.Enabled = enable;
            m_buttonLogOn.Enabled = !enable;
        }

        /// <summary>
        /// Post the query results.
        /// </summary>
        /// <param name="queryResult">The query results.</param>
        private void PostQueryResults(QueryCompletedEventArgs queryResult)
        {
            if (queryResult.Success)
            {
                PrintMessage(string.Format("Found {0} entities", queryResult.Data.Rows.Count));
                foreach (DataRow dr in queryResult.Data.Rows)
                {
                    Alarm alarm = m_sdkEngine.GetEntity((Guid)dr[0]) as Alarm;
                    if (alarm != null)
                    {
                        PrintMessage(string.Format("\tName: {0}", alarm.Name));
                        PrintMessage(string.Format("\tDescription: {0}", alarm.Description));
                        PrintMessage(string.Format("\tPriority: {0}", alarm.Priority));
                        PrintMessage(string.Format("\tDistribution mode: {0}", alarm.DistributionMode));
                        PrintMessage(string.Format("\tNumber of recipients: {0}", alarm.Recipients.Count));
                        foreach (AlarmRecipient recipient in alarm.Recipients)
                        {
                            PrintMessage(string.Format("\t\tRecipient: {0} ({1})", recipient.Entity.Name,
                                recipient.Entity.EntityType));
                        }
                        //This will make a newline
                        PrintMessage("");
                    }
                    else
                    {
                        PrintMessage("GetEntity failed");
                    }
                }
            }
            else
            {
                PrintMessage("The query has failed");
            }
        }

        /// <summary>
        /// Method to print the message on the UI.
        /// </summary>
        private void PrintMessage(string message)
        {
            // Pint the message on the textbox console.
            m_textboxConsole.Text += message + Environment.NewLine;

            // Scroll to the end of the TextBox.
            m_textboxConsole.SelectionStart = m_textboxConsole.Text.Length;
            m_textboxConsole.ScrollToCaret();
        }

        #endregion
    }

    #endregion
}

