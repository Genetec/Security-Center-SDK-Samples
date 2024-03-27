using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace QuerySample
{
    public partial class CameraIntegrityQueryUI : UserControl
    {

        private readonly List<Guid> m_camerasToQuery = new List<Guid>();
        private Engine m_sdkEngine;
        private CameraIntegrityQuery m_objQuery;
        public CameraIntegrityQueryUI()
        {
            InitializeComponent();
        }

        public void Initialize(Engine sdkEngine, CameraIntegrityQuery cameraIntegrityQuery)
        {
            m_sdkEngine = sdkEngine;
            m_objQuery = cameraIntegrityQuery;
        }

        private void m_removeCamera_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in m_cameraList.SelectedItems)
            {
                Guid cameraGuid = (Guid)listViewItem.Tag;
                m_camerasToQuery.Remove(cameraGuid);
            }
            RefreshList();
        }

        private void m_addCamera_Click(object sender, EventArgs e)
        {
            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear();
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

        private void m_browseQuery_Click(object sender, EventArgs e)
        {
            m_objQuery.Cameras = new Collection<Guid>(m_camerasToQuery);
            
            m_objQuery.BeginQuery(null, null);
        }
    }
}
