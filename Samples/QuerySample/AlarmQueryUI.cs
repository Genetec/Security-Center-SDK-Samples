using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Drawing;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class AlarmQueryUI : UserControl
    {
        #region Fields

        private Guid m_guidAlarm = Guid.Empty;

        private AlarmActivityQuery m_objQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Nested Classes and Structures

        private class AlarmStateData
        {
            #region Fields

            public AlarmState m_eState = AlarmState.None;

            #endregion

            #region Constructors

            public AlarmStateData(AlarmState eState)
            {
                m_eState = eState;
            }

            #endregion

            #region Public Methods

            public override string ToString()
            {
                switch (m_eState)
                {
                    case AlarmState.None:
                        return "Ignore state";
                    case AlarmState.Active:
                        return "Active";
                    case AlarmState.Acked:
                        return "Acknowledge";
                    case AlarmState.AknowledgeRequired:
                        return "Aknowledgement required";
                    case AlarmState.SourceConditionInvestigating:
                        return "Under investigation";
                    default:
                        return m_eState.ToString();
                }
            }

            #endregion
        }

        #endregion

        #region Constructors

        public AlarmQueryUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonBrowseClick(object sender, EventArgs e)
        {
            m_guidAlarm = Guid.Empty;

            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for a specific boEntity type...
                dlg.EntityTypeFilter.Add(EntityType.Alarm);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        m_guidAlarm = guid;
                        break;
                    }
                }
            }
            RefreshUI();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            int maxResults = GetMaxResults();

            if (maxResults > -1)
            {
                // The sample UI only allows 1 alarm to be selected...
                m_objQuery.Alarms.Clear();
                m_objQuery.Alarms.Add(m_guidAlarm);
                m_objQuery.MaximumResultCount = maxResults;

                // The sample UI only allows 1 state to be selected...
                AlarmStateData objData = (AlarmStateData)m_alarmState.SelectedItem;
                m_objQuery.States.Clear();
                if (objData != null)
                {
                    if (objData.m_eState != AlarmState.None)
                    {
                        m_objQuery.States.Add(objData.m_eState);
                    }
                }
                m_objQuery.BeginQuery(null, null);
            }
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine objSdk, AlarmActivityQuery objQuery)
        {
            m_sdkEngine = objSdk;
            m_objQuery = objQuery;

            m_alarmState.Items.Clear();
            // Field the combo box with the available alarm states
            foreach (AlarmState eState in Enum.GetValues(typeof(AlarmState)))
            {
                m_alarmState.Items.Add(new AlarmStateData(eState));
            }
        }

        #endregion

        #region Private Methods

        private int GetMaxResults()
        {
            int maxResults = -1;

            if (!int.TryParse(m_maximumResultsInput.Text, out maxResults))
            {
                m_maximumResultsInput.BackColor = Color.Red;
            }
            else
            {
                m_maximumResultsInput.BackColor = Color.White;
            }

            return maxResults;
        }

        private void RefreshUI()
        {
            if (m_guidAlarm != Guid.Empty)
            {
                Entity objEntity = m_sdkEngine.GetEntity(m_guidAlarm);
                if (objEntity != null)
                {
                    m_alarmName.Text = objEntity.Name;
                }
                else
                {
                    m_guidAlarm = Guid.Empty;
                }
            }
            if (m_guidAlarm == Guid.Empty)
            {
                m_alarmName.Text = "";
            }
        }

        #endregion
    }

    #endregion
}

