using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class TimeAttendanceUI : UserControl
    {
        #region Fields

        private TimeAttendanceQuery m_objQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public TimeAttendanceUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonAddClick(object sender, EventArgs e)
        {
            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for a specific boEntity type...
                dlg.EntityTypeFilter.Add(EntityType.Area);
                dlg.EntityTypeFilter.Add(EntityType.Cardholder);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        Entity objEntity = m_sdkEngine.GetEntity(guid);
                        if (objEntity.EntityType == EntityType.Cardholder)
                        {
                            if (m_objQuery.Cardholders.Contains(guid) == false)
                            {
                                m_objQuery.Cardholders.Add(guid);
                            }
                        }
                        else
                        {
                            if (m_objQuery.Areas.Contains(guid) == false)
                            {
                                m_objQuery.Areas.Add(guid);
                            }
                        }
                    }
                }
            }
            RefreshList();
        }

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            foreach (ListViewItem objItem in m_entityList.SelectedItems)
            {
                Guid guidEntity = (Guid)objItem.Tag;
                Entity objEntity = m_sdkEngine.GetEntity(guidEntity);
                if (objEntity.EntityType == EntityType.Cardholder)
                {
                    m_objQuery.Cardholders.Remove(guidEntity);
                }
                else
                {
                    m_objQuery.Areas.Remove(guidEntity);
                }
            }
            RefreshList();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            // Everything that happen during the last 7 days
            m_objQuery.TimeRange.DateTime = DateTime.UtcNow;
            m_objQuery.TimeRange.TimeSpan = new TimeSpan(-7, 0, 0, 0, 0);

            m_objQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine objSdk, TimeAttendanceQuery objQuery)
        {
            m_sdkEngine = objSdk;
            m_objQuery = objQuery;
        }

        public void RefreshList()
        {
            m_entityList.Items.Clear();
            foreach (Guid guid in m_objQuery.Cardholders)
            {
                Entity objEntity = m_sdkEngine.GetEntity(guid);
                if (objEntity != null)
                {
                    ListViewItem objData = new ListViewItem();
                    objData.Text = objEntity.Name;
                    objData.Tag = objEntity.Guid;
                    m_entityList.Items.Add(objData);
                }
            }
            foreach (Guid guid in m_objQuery.Areas)
            {
                Entity objEntity = m_sdkEngine.GetEntity(guid);
                if (objEntity != null)
                {
                    ListViewItem objData = new ListViewItem();
                    objData.Text = objEntity.Name;
                    objData.Tag = objEntity.Guid;
                    m_entityList.Items.Add(objData);
                }
            }
        }

        #endregion
    }

    #endregion
}

