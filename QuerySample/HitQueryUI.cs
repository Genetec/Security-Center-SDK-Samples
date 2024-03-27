using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.LicensePlateManagement;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
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

    public partial class HitQueryUI : UserControl
    {
        #region Fields

        private Guid m_guidLprRule = Guid.Empty;

        private HitQuery m_hitQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public HitQueryUI()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnButtonBrowseSourceClick(object sender, EventArgs e)
        {
            m_guidLprRule = Guid.Empty;

            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for specific types...
                dlg.EntityTypeFilter.Add(EntityType.Permit);
                dlg.EntityTypeFilter.Add(EntityType.OvertimeRule);
                dlg.EntityTypeFilter.Add(EntityType.PermitRule);
                dlg.EntityTypeFilter.Add(EntityType.HotlistRule);
                dlg.EntityTypeFilter.Add(EntityType.SharedPermitRule);
                dlg.Initialize(m_sdkEngine);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (Guid guid in dlg.SelectedItems)
                    {
                        m_guidLprRule = guid;
                        break;
                    }
                }
            }
            RefreshUI();
        }

        private void OnButtonQueryClick(object sender, EventArgs e)
        {
            m_hitQuery.ResetQuery();

            m_hitQuery.LprRuleIds = new Collection<Guid> { m_guidLprRule };
            m_hitQuery.QueryTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

            m_hitQuery.TimeRange.SetTimeRange(m_startDateAndTime.Value, m_endDateAndTime.Value);

            //This will effectively query all the system's patrollers and sharps
            m_hitQuery.SourceIds.Add(SystemConfiguration.SystemConfigurationGuid);
            m_hitQuery.RetrieveImagesOptions = RetrieveImagesOptions.None;

            if (m_fullSizeImages.Checked)
                m_hitQuery.RetrieveImagesOptions = RetrieveImagesOptions.FullSize;

            else if (m_thumbnails.Checked)
                m_hitQuery.RetrieveImagesOptions = RetrieveImagesOptions.Thumbnail;

            m_hitQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine, HitQuery query)
        {
            m_sdkEngine = sdkEngine;
            m_hitQuery = query;
        }

        #endregion

        #region Private Methods

        private void RefreshUI()
        {
            if (m_guidLprRule != Guid.Empty)
            {
                Entity objEntity = m_sdkEngine.GetEntity(m_guidLprRule);
                if (objEntity != null)
                {
                    m_source.Text = objEntity.Name;
                }
                else
                {
                    m_guidLprRule = Guid.Empty;
                }
            }
            if (m_guidLprRule == Guid.Empty)
            {
                m_source.Text = "";
            }
        }

        #endregion
    }

    #endregion
}

