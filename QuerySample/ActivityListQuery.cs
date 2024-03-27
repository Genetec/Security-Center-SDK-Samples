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

    public partial class ActivityListQuery : UserControl
    {
        #region Fields

        private EntityType m_eEntityType;

        private ActivityReportQuery m_objQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public ActivityListQuery()
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
                dlg.EntityTypeFilter.Add(m_eEntityType);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        if (m_objQuery.QueryEntities.Contains(guid) == false)
                        {
                            m_objQuery.QueryEntities.Add(guid);
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
                m_objQuery.QueryEntities.Remove(guidEntity);
            }
            RefreshList();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            // Everything that happened during the last day

            m_objQuery.TimeRange.DateTime = DateTime.UtcNow;
            m_objQuery.TimeRange.TimeSpan = new TimeSpan(-365, 0, 0, 0, 0);
            m_objQuery.BeginQuery(OnQueryCompleted, OnResultReceived, m_objQuery);
        }

        // Through this callback, you can access the data once all queries are complete.
        private void OnQueryCompleted(IAsyncResult ar)
        {
            ActivityReportQuery query = ar.AsyncState as ActivityReportQuery;

            if (query != null)
            {
                query.EndQuery(ar);
            }
        }

        // Through this callback, you can access the data for each query once its results are received.
        private void OnResultReceived(IAsyncResult ar)
        {

        }

        #endregion

        #region Public Methods

        public void Initialize(Engine objSdk, EntityType eType, ActivityReportQuery objQuery)
        {
            m_sdkEngine = objSdk;
            m_eEntityType = eType;
            m_objQuery = objQuery;
        }

        public void RefreshList()
        {
            m_entityList.Items.Clear();
            foreach (Guid guid in m_objQuery.QueryEntities)
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

