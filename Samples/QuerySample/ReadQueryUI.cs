using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.LicensePlateManagement;
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

    public partial class ReadQueryUI : UserControl
    {
        #region Fields

        private Guid m_guidSource = Guid.Empty;

        private ReadQuery m_readQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public ReadQueryUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonBrowseSourceClick(object sender, EventArgs e)
        {
            m_guidSource = Guid.Empty;

            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for a specific boEntity type...
                dlg.EntityTypeFilter.Add(EntityType.Patroller);
                dlg.EntityTypeFilter.Add(EntityType.LprUnit);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        m_guidSource = guid;
                        break;
                    }
                }
            }
            RefreshUI();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            m_readQuery.ResetQuery();
            m_readQuery.QueryTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            m_readQuery.TimeRange.SetTimeRange(m_startDateAndTime.Value, m_endDateAndTime.Value);
            m_readQuery.SourceIds.Add(m_guidSource);

            m_readQuery.RetrieveImagesOptions = RetrieveImagesOptions.None;

            if (m_includeFullSizeImages.Checked)
                m_readQuery.RetrieveImagesOptions = RetrieveImagesOptions.FullSize;

            else if (m_includeThumbnails.Checked)
                m_readQuery.RetrieveImagesOptions = RetrieveImagesOptions.Thumbnail;

            m_readQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine, ReadQuery query)
        {
            m_sdkEngine = sdkEngine;
            m_readQuery = query;
        }

        #endregion

        #region Private Methods

        private void RefreshUI()
        {
            if (m_guidSource != Guid.Empty)
            {
                Entity objEntity = m_sdkEngine.GetEntity(m_guidSource);
                if (objEntity != null)
                {
                    m_source.Text = objEntity.Name;
                }
                else
                {
                    m_guidSource = Guid.Empty;
                }
            }
            if (m_guidSource == Guid.Empty)
            {
                m_source.Text = "";
            }
        }

        #endregion
    }

    #endregion
}

