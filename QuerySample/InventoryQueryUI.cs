using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.LicensePlateManagement.Inventory;
using Genetec.Sdk.Samples.SamplesLibrary;

namespace QuerySample
{
    public partial class InventoryQueryUI : UserControl
    {

        #region Fields

        private Guid m_guidSource = Guid.Empty;

        private InventoryQuery m_inventoryQuery;

        private Engine m_sdkEngine;

        #endregion

        #region Constructors

        public InventoryQueryUI()
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
                dlg.EntityTypeFilter.Add(EntityType.LprMlpiRule);
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
            m_inventoryQuery.ResetQuery();
            m_inventoryQuery.QueryTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
            m_inventoryQuery.SourceCriteria = new InventorySourceCriteria(m_guidSource, InventorySourceType.Latest);
            m_inventoryQuery.RetrieveImagesOptions = RetrieveImagesOptions.None;

            if (m_includeFullSizeImages.Checked)
                m_inventoryQuery.RetrieveImagesOptions = RetrieveImagesOptions.FullSize;

            else if (m_includeThumbnails.Checked)
                m_inventoryQuery.RetrieveImagesOptions = RetrieveImagesOptions.Thumbnail;

            m_inventoryQuery.BeginQuery(null, null);
        }

        #endregion

        #region Public Methods

        public void Initialize(Engine sdkEngine, InventoryQuery query)
        {
            m_sdkEngine = sdkEngine;
            m_inventoryQuery = query;
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (FindByPlate fbp = new FindByPlate())
            {
                fbp.Initialize(m_sdkEngine);
                fbp.ShowDialog();
            }
        }
    }
}
