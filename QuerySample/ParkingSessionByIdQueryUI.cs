using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries.LicensePlateManagement.ParkingSession;
using Genetec.Sdk.Samples.SamplesLibrary;

namespace QuerySample
{
    public partial class ParkingSessionByIdQueryUI : UserControl
    {
        private Guid m_guidParkingZone = Guid.Empty;

        private Guid m_guidParkingSession = Guid.Empty;

        private ParkingSessionByIdQuery m_parkingSessionByIdQuery;

        private Engine m_sdkEngine;

        public ParkingSessionByIdQueryUI()
        {
            InitializeComponent();
        }

        public void Initialize(Engine sdkEngine, ParkingSessionByIdQuery query)
        {
            m_sdkEngine = sdkEngine;
            m_parkingSessionByIdQuery = query;
        }

        private void m_browseSourcesParkingZone_Click(object sender, EventArgs e)
        {
            m_guidParkingZone = Guid.Empty;
            m_guidParkingSession = Guid.Empty;

            using (SearchDlg dlg = new SearchDlg())
            {
                dlg.EntityTypeFilter.Clear(); // only want to search for specific types...
                dlg.EntityTypeFilter.Add(EntityType.ParkingZone);
                dlg.Initialize(m_sdkEngine);

                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    m_guidParkingZone = dlg.SelectedItems[0];
                }
            }

            ParkingSessionQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.ParkingSession) as ParkingSessionQuery;
            ParkingZone ParkingZone = m_sdkEngine.GetEntity(m_guidParkingZone) as ParkingZone;
            query.ParkingZones.Add(m_guidParkingZone);
            query.TimeRange.SetTimeRange(TimeSpan.FromDays(31));
            query.RetrieveImagesOptions = RetrieveImagesOptions.None;
            QueryCompletedEventArgs result = query.Query();
            Collection<string> ParkingSessions = new Collection<string>();
            foreach (DataRow dr in result.Data.Rows)
            {
                ParkingSessions.Add(dr[2].ToString());
            }
            m_parkingSessionsComboBox.Text = "";
            foreach (string ParkingSession in ParkingSessions)
            {
                m_parkingSessionsComboBox.Items.Add(ParkingSession);
            }
            RefreshUI();
        }

        #region Private Methods

        private void RefreshUI()
        {
            if (m_guidParkingZone != Guid.Empty)
            {
                Entity objEntity = m_sdkEngine.GetEntity(m_guidParkingZone);
                if (objEntity != null)
                {
                    m_parkingZoneTextBox.Text = objEntity.Name;
                }
                else
                {
                    m_guidParkingZone = Guid.Empty;
                }
            }
            if (m_guidParkingZone == Guid.Empty)
            {
                m_parkingZoneTextBox.Text = "";
            }
        }

        #endregion

        private void m_parkingZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void m_query_Click(object sender, EventArgs e)
        {
            m_parkingSessionByIdQuery.ParkingZone = m_guidParkingZone;
            m_parkingSessionByIdQuery.ParkingSession = new Guid(m_parkingSessionsComboBox.Text);
            m_parkingSessionByIdQuery.RetrieveImagesOptions = RetrieveImagesOptions.None;
            m_parkingSessionByIdQuery.BeginQuery(null, null);
        }
    }
}
