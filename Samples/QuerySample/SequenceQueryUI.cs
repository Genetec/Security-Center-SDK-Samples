using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.Video;
using Genetec.Sdk.Samples.SamplesLibrary;

namespace QuerySample
{
    public partial class SequenceQueryUI : UserControl
    {
        private readonly List<Guid> m_camerasToQuery = new List<Guid>();

        private SequenceQuery m_objQuery;

        private Engine m_sdkEngine;

        public SequenceQueryUI()
        {
            InitializeComponent();
            m_start.Value = DateTime.UtcNow - TimeSpan.FromDays(30);
            m_end.Value = DateTime.UtcNow;
        }

        private void SequenceQueryUI_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(Engine sdkEngine, SequenceQuery sequencesEventQuery)
        {
            m_sdkEngine = sdkEngine;
            m_objQuery = sequencesEventQuery;
        }

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
            m_objQuery.TimeRange.SetTimeRange(m_start.Value, m_end.Value);

            m_objQuery.BeginQuery(null, null);
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
    }
}
