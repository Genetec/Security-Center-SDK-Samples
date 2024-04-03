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
    public partial class VideoFileProtectionUI : UserControl
    {
        private readonly List<Guid> m_camerasToQuery = new List<Guid>();
        private Engine m_sdkEngine;
        private VideoFileQuery m_objQuery;

        public VideoFileProtectionUI()
        {
            InitializeComponent();
            m_start.Value = DateTime.UtcNow - TimeSpan.FromDays(30);
            m_end.Value = DateTime.UtcNow;
        }

        private void OnButtonAddClick(object sender, EventArgs e)
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

        private void OnButtonDeleteClick(object sender, EventArgs e)
        {
            foreach (ListViewItem listViewItem in m_cameraList.SelectedItems)
            {
                Guid cameraGuid = (Guid)listViewItem.Tag;
                m_camerasToQuery.Remove(cameraGuid);
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

        public void Initialize(Engine sdkEngine, VideoFileQuery videoFileQuery)
        {
            m_sdkEngine = sdkEngine;
            m_objQuery = videoFileQuery;
        }

        private void protectBtn_Click(object sender, EventArgs e)
        {
            var span = m_infiniteProtection.Checked ? TimeSpan.MaxValue : TimeSpan.FromDays(5);
            m_sdkEngine.ActionManager.AddVideoProtection(m_camerasToQuery, m_startProtect.Value.ToUniversalTime(), m_stopProtect.Value.ToUniversalTime(), span);
        }

        private void m_browseQuery_Click(object sender, EventArgs e)
        {
            m_objQuery.Cameras = new Collection<Guid>(m_camerasToQuery);
            m_objQuery.TimeRange.SetTimeRange(m_start.Value, m_end.Value);

            m_objQuery.BeginQuery(null, null);
        }

        private void Unprotect_Click(object sender, EventArgs e)
        {
            m_sdkEngine.ActionManager.RemoveVideoProtection(m_camerasToQuery, m_startProtect.Value.ToUniversalTime(), m_stopProtect.Value.ToUniversalTime(), TimeSpan.FromDays(5));
        }
    }
}
