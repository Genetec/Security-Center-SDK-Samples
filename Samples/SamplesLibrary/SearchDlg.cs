using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Forms;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    public partial class SearchDlg : Form
    {
        #region Fields

        /// <summary>
        /// Represent the entity types to query for
        /// </summary>
        private Collection<EntityType> m_colEntityTypes = new Collection<EntityType>();

        /// <summary>
        /// Represent the query lauched to Security Center
        /// </summary>
        private EntityConfigurationQuery m_objQuery;

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the entity types to query for
        /// </summary>
        public Collection<EntityType> EntityTypeFilter
        {
            get { return m_colEntityTypes; }
            set { m_colEntityTypes = value; }
        }

        /// <summary>
        /// Gets or Sets a flag that indicates if the multiple selection is enabled
        /// </summary>
        public bool MutlipleSelection
        {
            get { return m_entitiesList.MultiSelect; }
            set { m_entitiesList.MultiSelect = value; }
        }

        /// <summary>
        /// Gets the list of selected items
        /// </summary>
        public List<Guid> SelectedItems
        {
            get
            {
                List<Guid> lst = new List<Guid>();
                foreach (EntityViewItem lvItem in m_entitiesList.SelectedItems)
                {
                    lst.Add(lvItem.Entity.Guid);
                }
                return lst;
            }
        }

        #endregion

        #region Constructors

        public SearchDlg()
        {
            InitializeComponent();

            // Set the image list to display icons in the listview
            m_entitiesList.SmallImageList = ResourcesManager.EntityImageList;

            // By default, insert all the entity type
            foreach (EntityType type in Enum.GetValues(typeof(EntityType)))
            {
                m_colEntityTypes.Add(type);
            }
        }

        #endregion

        #region Event Handlers

        private void OnButtonSearchClick(object sender, EventArgs e)
        {
            Search();
        }

        private void OnListEntitiesMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_entitiesList.SelectedItems.Count > 0)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void OnReportQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            if (e.Success)
            {
                MethodInvoker pFunc = delegate
                {
                    try
                    {
                        m_entitiesList.BeginUpdate();
                        m_entitiesList.Items.Clear();

                        foreach (DataRow row in e.Data.Rows)
                        {
                            if (row[0] is Guid)
                            {
                                Guid guidEntity = (Guid)row[0];
                                Entity entity = m_sdkEngine.GetEntity(guidEntity);
                                if (entity != null)
                                {
                                    EntityViewItem lvItem = new EntityViewItem(entity);
                                    m_entitiesList.Items.Add(lvItem);
                                }
                            }
                        }
                    }
                    finally
                    {
                        m_entitiesList.EndUpdate();
                    }
                };
                Invoke(pFunc);
            }
        }

        private void OnTextFieldKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar == Keys.Return)
            {
                e.Handled = true;
                Search();
            }
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine)
        {
            m_sdkEngine = sdkEngine;
            if (m_colEntityTypes.Count > 0)
            {
                // Prelaunch a query
                Search();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Launch the query
        /// </summary>
        private void Search()
        {
            if (m_objQuery == null)
            {
                m_objQuery = (EntityConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);
                m_objQuery.QueryCompleted += OnReportQueryCompleted;
            }

            m_objQuery.EntityTypeFilter.Clear();
            m_objQuery.EntityTypeFilter.AddRange(m_colEntityTypes);
            m_objQuery.Name = m_name.Text.Length > 0 ? m_name.Text : null;
            m_objQuery.Description = m_descriptionInput.Text.Length > 0 ? m_descriptionInput.Text : null;

            m_objQuery.BeginQuery(null, null);
        }

        #endregion
    }

    #endregion
}

