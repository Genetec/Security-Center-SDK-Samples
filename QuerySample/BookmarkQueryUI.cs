﻿using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.Video;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class BookmarkQueryUI : UserControl
    {
        #region Constants

        private readonly List<Guid> m_camerasToQuery = new List<Guid>();

        #endregion

        #region Fields

        private BookmarkEventQuery m_objQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public BookmarkQueryUI()
        {
            InitializeComponent();
            m_startDateAndTime.Value = DateTime.UtcNow - TimeSpan.FromDays(30);
            m_endDateAndTime.Value = DateTime.UtcNow;
        }

        #endregion

        #region Event Handlers

        private void OnButtonAddClick(object sender, EventArgs e)
        {
            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for a specific boEntity type...
                dlg.EntityTypeFilter.Add(EntityType.Camera);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        if (m_camerasToQuery.Contains(guid) == false)
                        {
                            m_camerasToQuery.Add(guid);
                        }
                    }
                }
            }
            RefreshList();
        }

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in m_cameraList.SelectedItems)
            {
                Guid cameraGuid = (Guid)listViewItem.Tag;
                m_camerasToQuery.Remove(cameraGuid);
            }
            RefreshList();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            m_objQuery.Cameras = new Collection<Guid>(m_camerasToQuery);
            m_objQuery.TimeRange.SetTimeRange(m_startDateAndTime.Value, m_endDateAndTime.Value);

            m_objQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine, BookmarkEventQuery objQuery)
        {
            m_sdkEngine = sdkEngine;
            m_objQuery = objQuery;
        }

        public void RefreshList()
        {
            m_cameraList.Items.Clear();
            foreach (Guid guid in m_camerasToQuery)
            {
                Camera camera = m_sdkEngine.GetEntity(guid) as Camera;
                if (camera != null)
                {
                    ListViewItem listViewItem = new ListViewItem
                    {
                        Text = camera.Name,
                        Tag = camera.Guid
                    };

                    m_cameraList.Items.Add(listViewItem);
                }
            }
        }

        #endregion
    }

    #endregion
}

