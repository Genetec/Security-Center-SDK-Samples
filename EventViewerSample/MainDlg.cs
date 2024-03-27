using Genetec.Sdk.Entities;
using Genetec.Sdk.Events.AccessPoint;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples
{
    #region Classes

    public partial class MainDlg : Form
    {
        #region Fields

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine = new Engine();

        #endregion

        #region Nested Classes and Structures

        private sealed class AlarmViewItem : ListViewItem
        {
            #region Constants

            private readonly int m_instanceId;

            #endregion

            #region Properties

            /// <summary>
            /// Gets the alarm Guid.
            /// </summary>
            public Guid Guid { get; private set; }

            /// <summary>
            /// Gets the alarm instance ID.
            /// </summary>
            public int InstanceId
            {
                get { return m_instanceId; }
            }

            #endregion

            #region Constructors

            public AlarmViewItem(AlarmTriggeredEventArgs e, string alarmName)
            {
                m_instanceId = e.InstanceId;
                Guid = e.AlarmGuid;

                Text = e.InstanceId.ToString();
                SubItems.Add(alarmName);
                SubItems.Add(e.TriggerEvent.ToString());
                SubItems.Add(e.TriggerTimestamp.ToString());
            }

            #endregion
        }

        private sealed class EventViewItem : ListViewItem
        {
            #region Constructors

            public EventViewItem(DateTime timestampUtc, EventType eventType, string description)
            {
                Text = timestampUtc.ToLocalTime().ToString("G");
                SubItems.Add(eventType.ToString());
                SubItems.Add(description);
            }

            #endregion
        }

        #endregion

        #region Constructors

        public MainDlg()
        {
            InitializeComponent();

            // Subscribe to the Engine events
            SubscribeEngine();
        }

        #endregion

        #region Event Handlers

        private void OnButtonAckClick(object sender, EventArgs e)
        {
            // Create a collection of all the alarm instances ids to acknowledge
            var alarmsToAcknowledge = new Dictionary<int, Guid>();
            foreach (AlarmViewItem lvItem in m_alarmList.SelectedItems)
            {
                alarmsToAcknowledge.Add(lvItem.InstanceId, lvItem.Guid);
            }

            // Acknowledge the alarms
            // Note: Acknowledging a batch of alarms is much faster than acknowledging them one by one.
            m_sdkEngine.AlarmManager.AcknowledgeAlarm(alarmsToAcknowledge, AcknowledgementType.Ack);
        }

        private void OnButtonConnect_Click(object sender, EventArgs e)
        {
            using (LogonDlg dlg = new LogonDlg())
            {
                dlg.Initialize(m_sdkEngine);
                dlg.ShowDialog(this);
            }
        }

        private void OnButtonDisconnectClick(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.LogOff();
        }

        private void OnButtonNackClick(object sender, EventArgs e)
        {
            // Create a collection of all the alarm instances ids to nacknowledge
            var alarmsToAcknowledge = new Dictionary<int, Guid>();
            foreach (AlarmViewItem lvItem in m_alarmList.SelectedItems)
            {
                alarmsToAcknowledge.Add(lvItem.InstanceId, lvItem.Guid);
            }

            // Acknowledge the alarms
            // Note: Nacknowledging a batch of alarms is much faster than acknowledging them one by one.
            m_sdkEngine.AlarmManager.AcknowledgeAlarm(alarmsToAcknowledge, AcknowledgementType.Nack);
        }

        private void OnEngineAlarmTriggered(object sender, AlarmTriggeredEventArgs e)
        {
            // Insert the received alarm in the listview
            Alarm alarm = m_sdkEngine.GetEntity(e.AlarmGuid) as Alarm;
            string alarmName = alarm.Name;

            AlarmViewItem lvItem = new AlarmViewItem(e, alarmName);
            m_alarmList.Items.Add(lvItem);

            // Scroll the listview to the end
            m_alarmList.EnsureVisible(lvItem.Index);
        }

        private void OnEngineEventReceived(object sender, EventReceivedEventArgs e)
        {
            string description = String.Empty;

            switch (e.EventType)
            {
                case EventType.AccessGranted:
                case EventType.AccessRefused:
                    {
                        AccessEvent accessEvent = e.Event as AccessEvent;

                        // The source is the access point
                        Entity boSource = m_sdkEngine.GetEntity(accessEvent.SourceEntity);
                        AccessPoint boAccessPoint = m_sdkEngine.GetEntity(accessEvent.SourceEntity) as AccessPoint;
                        string strAccessPoint = "???";
                        if (boAccessPoint != null)
                        {
                            strAccessPoint = boAccessPoint.Name;
                        }

                        Cardholder boCardholder = m_sdkEngine.GetEntity(accessEvent.Cardholder) as Cardholder;
                        var guidCredential = (accessEvent.Credentials.Count > 0 ? accessEvent.Credentials[0] : Guid.Empty);
                        Credential boCredential = m_sdkEngine.GetEntity(guidCredential) as Credential;
                        if ((boCardholder != null) && (boCredential != null))
                        {
                            description = String.Format("Access requested by {0} {1} with {2} on {3} ({4})", boCardholder.FirstName, boCardholder.LastName, boCredential.Format, boSource, strAccessPoint);
                        }
                    }
                    break;

                case EventType.AccessUnassignedCredential:
                    {
                        AccessPointCredentialStatusEvent credentialStatusEvent = e.Event as AccessPointCredentialStatusEvent;

                        // The source is the access point
                        Entity boSource = m_sdkEngine.GetEntity(credentialStatusEvent.SourceEntity);
                        AccessPoint boAccessPoint = m_sdkEngine.GetEntity(credentialStatusEvent.SourceEntity) as AccessPoint;
                        string strAccessPoint = "???";
                        if (boAccessPoint != null)
                        {
                            strAccessPoint = boAccessPoint.Name;
                        }

                        Credential boCredential = m_sdkEngine.GetEntity(credentialStatusEvent.CredentialGuid) as Credential;
                        if (boCredential != null)
                        {
                            description = String.Format("Access requested with {0} on {1} ({2})", boCredential.Format, boSource, strAccessPoint);
                        }
                    }
                    break;
            }

            // Create an item representing the event
            EventViewItem lvItem = new EventViewItem(e.Timestamp, e.EventType, description);

            // Insert the received event in the listview
            m_eventList.Items.Add(lvItem);

            // Scroll the listview to the end
            m_eventList.EnsureVisible(lvItem.Index);
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            if (e.AutoReconnect == false)
            {
                m_connect.Enabled = true;
                m_disconnect.Enabled = false;
            }
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_connect.Enabled = false;
            m_disconnect.Enabled = true;
            DownloadAll();
        }

        private void OnListAlarmsSelectedIndexChanged(object sender, EventArgs e)
        {
            // Enable the acknowledgment buttons only if at least one alarms is selected
            m_acknowledgeAlarm.Enabled = m_alarmNotAcknowledged.Enabled = (m_alarmList.SelectedItems.Count > 0);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Download all the entities in the Sdk cache. Note that downloading all the
        /// entities in the Sdk cache will cause poor performances on large systems of
        /// over 10000 entities.
        /// </summary>
        private void DownloadAll()
        {
            // Create a query to download Security Center's entities in the SDK cache
            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;

            // Add all the entity type in the query
            foreach (EntityType entityType in Enum.GetValues(typeof(EntityType)))
            {
                // EntityType None must not be added.
                if(entityType != EntityType.None)
                    query.EntityTypeFilter.Add(entityType);
            }

            // Can be dangerous in a huge environment on a query that queries everything.
            query.DownloadAllRelatedData = true;

            // Launch the query. Note that when the queried entities will be downloaded 
            // in the Sdk cache, the EntityAdded event will be called.
            query.BeginQuery(null, null);
        }

        /// <summary>
        /// Subscribe to the Engine events
        /// </summary>
        private void SubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.EventReceived += OnEngineEventReceived;
            m_sdkEngine.AlarmTriggered += OnEngineAlarmTriggered;
        }

        /// <summary>
        /// Unsubscribe from the Engine events
        /// </summary>
        private void UnsubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn -= OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff -= OnEngineLoggedOff;
            m_sdkEngine.EventReceived -= OnEngineEventReceived;
            m_sdkEngine.AlarmTriggered -= OnEngineAlarmTriggered;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Always dispose the SDK object when closing the application
            if (m_sdkEngine != null)
            {
                // Unsubscribe from the Engine events
                UnsubscribeEngine();

                m_sdkEngine.Dispose();
                m_sdkEngine = null;
            }
        }
    }

    #endregion
}

