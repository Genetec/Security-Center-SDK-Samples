using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.LprRules;
using Genetec.Sdk.Queries;
using Genetec.Sdk.Queries.LicensePlateManagement;
using Genetec.Sdk.Queries.LicensePlateManagement.Inventory;
using Genetec.Sdk.Queries.LicensePlateManagement.ParkingSession;
using Genetec.Sdk.Samples.SamplesLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using Genetec.Sdk.Queries.Video;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    public partial class MainDlg : Form
    {
        #region Fields

        private AlarmActivityQuery m_alarmActivityQuery;

        private AreaActivityQuery m_areaActivityQuery;

        private BookmarkEventQuery m_bookmarkEventQuery;

        private CardholderActivityQuery m_cardholderActivityQuery;

        private CardholderConfigurationQuery m_cardholderQuery;

        private CredentialActivityQuery m_credentialActivityQuery;

        private DoorActivityQuery m_doorActivityQuery;

        private HitQuery m_hitQuery;

        private LpmDailyUsageQuery m_lpmDailyUsageQuery;

        private LpmLoginRecorderPerPatrollerQuery m_lpmLoginRecorderPerPatrollerQuery;

        private LpmReadsHitsStatsPerDayQuery m_lpmReadsHitsStatsPerDayQuery;

        private ParkingSessionByIdQuery m_parkingSessionByIdQuery;

        private PatrollerPositionQuery m_patrollerPositionQuery;

        private ReadQuery m_readQuery;

        private SequenceQuery m_sequencesEventQuery;

        private BlockingVideoEventQuery m_blockingEventQuery;

        private InventoryQuery m_inventoryQuery;

        private CameraIntegrityQuery m_camaraIntegrityQuery;

        /// <summary>
        /// Represent the SDK class used to control Security Center
        /// </summary>
        private Engine m_sdkEngine = new Engine();

        private TimeAttendanceQuery m_timeAttendanceQuery;

        private VisitorQuery m_visitorQuery;

        private ZoneActivityQuery m_zoneActivityQuery;

        private VideoFileQuery m_videoFileQuery;

        private VideoFileProtectionUI m_videoFileProtection;

        #endregion

        #region Constructors

        public MainDlg()
        {
            InitializeComponent();
            // Subscribe to the Engine events
            SubscribeEngine();
            m_querySampleTabs.Enabled = false;
        }

        #endregion

        #region Event Handlers

        private void OnBlockingQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a blocking event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Camera");
                m_entityList.Columns.Add("Source");
                m_entityList.Columns.Add("EventTime");
                m_entityList.Columns.Add("Type");
                m_entityList.Columns.Add("User Level");
                m_entityList.Columns.Add("Block Level");

                foreach (DataRow row in e.Data.Rows)
                {
                    Guid cameraId = (Guid)row[0];
                    DateTime time = new DateTime(((DateTime)row[2]).Ticks, DateTimeKind.Utc);
                    EventType type = (EventType) int.Parse(row[3].ToString());
                    string userLevel = "";
                    string blockLevel = "";
                    string xml = (string)row[6];
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xml);

                    foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                    {
                        string text = node.InnerText; 
                        if (node.Name == "BlockLevel")
                            blockLevel = text;
                        if (node.Name == "UserLevel")
                            userLevel = text;
                    }
                    Camera camera = m_sdkEngine.GetEntity(cameraId) as Camera;

                    ListViewItem listViewItem = new ListViewItem { Text = camera != null ? camera.Name : "Unknown" };
                    listViewItem.SubItems.Add(camera != null && camera.Role != null ? camera.Role.Name : "Unknown");
                    listViewItem.SubItems.Add(time.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(type.ToString());
                    listViewItem.SubItems.Add(userLevel);
                    listViewItem.SubItems.Add(blockLevel);

                    m_entityList.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnBookmarkQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a bookmark event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Camera");
                m_entityList.Columns.Add("Source");
                m_entityList.Columns.Add("Event timestamp");
                m_entityList.Columns.Add("Message");

                foreach (DataRow row in e.Data.Rows)
                {
                    Guid cameraId = (Guid)row[0];
                    DateTime eventTime = new DateTime(((DateTime)row[2]).Ticks, DateTimeKind.Utc);
                    string message = (string)row[5];

                    Camera camera = m_sdkEngine.GetEntity(cameraId) as Camera;

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = camera != null ? camera.Name : "Unknown";
                    listViewItem.SubItems.Add(camera != null && camera.Role != null ? camera.Role.Name : "Unknown");
                    listViewItem.SubItems.Add(eventTime.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(message);
                    m_entityList.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnCardholderQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a cardholder e...
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("FirstName");
                m_entityList.Columns.Add("LastName");
                m_entityList.Columns.Add("Description");

                if (e.Success)
                {
                    foreach (DataRow rows in e.Data.Rows)
                    {
                        Guid guidEntity = (Guid)rows[0];
                        Cardholder boCardholder = m_sdkEngine.GetEntity(guidEntity) as Cardholder;
                        if (boCardholder != null)
                        {
                            ListViewItem lvItem = new ListViewItem();
                            lvItem.Text = boCardholder.FirstName;
                            lvItem.SubItems.Add(boCardholder.LastName);
                            lvItem.SubItems.Add(boCardholder.Description);
                            m_entityList.Items.Add(lvItem);
                        }
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnConnectClick(object sender, EventArgs e)
        {
            using (LogonDlg dlg = new LogonDlg())
            {
                dlg.Initialize(m_sdkEngine);
                dlg.ShowDialog(this);
            }
        }

        private void OnDisconnectClick(object sender, EventArgs e)
        {
            m_sdkEngine.LoginManager.BeginLogOff();
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            if (e.AutoReconnect == false)
            {
                m_connect.Enabled = true;
                m_disconnect.Enabled = false;
                m_querySampleTabs.Enabled = false;
            }
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            m_connect.Enabled = false;
            m_disconnect.Enabled = true;
            m_querySampleTabs.Enabled = true;

            m_cardholderQuery = (CardholderConfigurationQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.CardholderConfiguration);
            m_cardholderQuery.QueryCompleted += OnCardholderQueryCompleted;

            m_doorActivityQuery = (DoorActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.DoorActivity);
            m_doorActivityQuery.QueryCompleted += ActivityQueryCompleted;

            m_areaActivityQuery = (AreaActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.AreaActivity);
            m_areaActivityQuery.QueryCompleted += ActivityQueryCompleted;

            m_cardholderActivityQuery = (CardholderActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.CardholderActivity);
            m_cardholderActivityQuery.QueryCompleted += ActivityQueryCompleted;

            m_credentialActivityQuery = (CredentialActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.CredentialActivity);
            m_credentialActivityQuery.QueryCompleted += ActivityQueryCompleted;

            m_zoneActivityQuery = (ZoneActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.ZoneActivity);
            m_zoneActivityQuery.QueryCompleted += ZoneActivityQueryCompleted;

            m_alarmActivityQuery = (AlarmActivityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.AlarmActivity);
            m_alarmActivityQuery.QueryCompleted += AlarmQueryCompleted;

            m_bookmarkEventQuery = (BookmarkEventQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.Bookmark);
            m_bookmarkEventQuery.QueryCompleted += OnBookmarkQueryCompleted;

            m_sequencesEventQuery = (SequenceQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.VideoSequence);
            m_sequencesEventQuery.QueryCompleted += OnSequenceQueryCompleted;

            m_camaraIntegrityQuery = (CameraIntegrityQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.CameraIntegrity);
            m_camaraIntegrityQuery.QueryCompleted += OnCameraIntegrityQueryCompleted;

            m_blockingEventQuery = (BlockingVideoEventQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.BlockingVideoEvent);
            m_blockingEventQuery.QueryCompleted += OnBlockingQueryCompleted;

            m_videoFileQuery = (VideoFileQuery) m_sdkEngine.ReportManager.CreateReportQuery(ReportType.VideoFile);
            m_videoFileQuery.QueryCompleted += OnvideoFileQueryCompleted;

            m_visitorQuery = (VisitorQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.Visitor);
            m_visitorQuery.QueryCompleted += OnVisitorQueryCompleted;

            m_timeAttendanceQuery = (TimeAttendanceQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.TimeAttendanceActivity);
            m_timeAttendanceQuery.QueryCompleted += OnTimeAttendanceQueryCompleted;

            m_readQuery = (ReadQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LprRead);
            m_readQuery.QueryCompleted += OnLprReadQueryCompleted;

            m_inventoryQuery = (InventoryQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.Inventory);
            m_inventoryQuery.QueryCompleted += OnInventoryQueryCompleted;

            m_hitQuery = (HitQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LprHit);
            m_hitQuery.QueryCompleted += OnLprHitQueryCompleted;

            m_parkingSessionByIdQuery = (ParkingSessionByIdQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.ParkingSessionById);
            m_parkingSessionByIdQuery.QueryCompleted += OnParkingSessionByIdQueryCompleted;

            m_patrollerPositionQuery = (PatrollerPositionQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.PatrollerPositions);
            m_patrollerPositionQuery.QueryCompleted += OnPatrollerPositionQueryCompleted;

            m_lpmDailyUsageQuery = (LpmDailyUsageQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LpmDailyUsage);
            m_lpmDailyUsageQuery.QueryCompleted += OnLpmDailyUsageQueryCompleted;

            m_lpmLoginRecorderPerPatrollerQuery = (LpmLoginRecorderPerPatrollerQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LpmLoginRecorderPerPatroller);
            m_lpmLoginRecorderPerPatrollerQuery.QueryCompleted += OnLpmLoginRecorderPerPatrollerQueryCompleted;

            m_lpmReadsHitsStatsPerDayQuery = (LpmReadsHitsStatsPerDayQuery)m_sdkEngine.ReportManager.CreateReportQuery(ReportType.LpmReadsHitsStatsPerDay);
            m_lpmReadsHitsStatsPerDayQuery.QueryCompleted += OnLpmReadsHitsStatsPerDayQueryCompleted;

            m_doorActivityList.Initialize(m_sdkEngine, EntityType.Door, m_doorActivityQuery);
            m_areaActivityList.Initialize(m_sdkEngine, EntityType.Area, m_areaActivityQuery);
            m_cardholderActivityList.Initialize(m_sdkEngine, EntityType.Cardholder, m_cardholderActivityQuery);
            m_credentialActivityList.Initialize(m_sdkEngine, EntityType.Credential, m_credentialActivityQuery);
            m_zoneActivityList.Initialize(m_sdkEngine, EntityType.Zone, m_zoneActivityQuery);
            m_visitors.Initialize(m_sdkEngine, m_visitorQuery);
            m_alarmActivity.Initialize(m_sdkEngine, m_alarmActivityQuery);
            m_timeAttendanceList.Initialize(m_sdkEngine, m_timeAttendanceQuery);
            m_bookmark.Initialize(m_sdkEngine, m_bookmarkEventQuery);
            m_sequence.Initialize(m_sdkEngine, m_sequencesEventQuery);
            m_cameraIntegrity.Initialize(m_sdkEngine, m_camaraIntegrityQuery);
            m_blockingQuery.Initialize(m_sdkEngine, m_blockingEventQuery);
            m_videoFileProtection.Initialize(m_sdkEngine, m_videoFileQuery);
            m_lprRead.Initialize(m_sdkEngine, m_readQuery);
            m_inventory.Initialize(m_sdkEngine, m_inventoryQuery);
            m_lprHit.Initialize(m_sdkEngine, m_hitQuery);
            m_parkingSessionById.Initialize(m_sdkEngine, m_parkingSessionByIdQuery);
            m_patrollerPosition.Initialize(m_sdkEngine, m_patrollerPositionQuery);
        }

        private void OnCameraIntegrityQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a CameraIntegrity
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Camera");
                m_entityList.Columns.Add("EventTime");
                m_entityList.Columns.Add("EventType");
                m_entityList.Columns.Add("Notes");
                m_entityList.Columns.Add("Thumbnail");
                m_entityList.Columns.Add("Value");

                foreach (DataRow row in e.Data.Rows)
                {
                    Guid cameraId = (Guid)row[0];
                    DateTime eventTime = new DateTime(((DateTime)row[1]).Ticks, DateTimeKind.Utc);
                    EventType type = (EventType)int.Parse(row[2].ToString());
                    string notes = (string)row[3] ;
                    byte[] thumbnail = (byte[]) row[4];
                    uint value = (uint) row[5];

                    Camera camera = m_sdkEngine.GetEntity(cameraId) as Camera;

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = camera != null ? camera.Name : "Unknown";
                    //listViewItem.SubItems.Add(camera != null && camera.Role != null ? camera.Role.Name : "Unknown");
                    listViewItem.SubItems.Add(eventTime.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(type.ToString());
                    listViewItem.SubItems.Add(notes.ToString());
                    listViewItem.SubItems.Add(thumbnail.ToString());
                    listViewItem.SubItems.Add(value.ToString());

                    m_entityList.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnvideoFileQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a bookmark event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Camera");
                m_entityList.Columns.Add("Source");
                m_entityList.Columns.Add("Start");
                m_entityList.Columns.Add("Stop");
                m_entityList.Columns.Add("Path");
                m_entityList.Columns.Add("Size");
                m_entityList.Columns.Add("ProtectionStatus");
                m_entityList.Columns.Add("InfiniteProtection");
                //m_entityList.Columns.Add("Encrypted");
                m_entityList.Columns.Add("ProtectionEndDateTime");

                foreach (DataRow row in e.Data.Rows)
                {
                    Guid cameraId = (Guid)row[0];
                    DateTime start = new DateTime(((DateTime)row[2]).Ticks, DateTimeKind.Utc);
                    DateTime stop = new DateTime(((DateTime)row[3]).Ticks, DateTimeKind.Utc);
                    string path = (string)row[4];
                    decimal size = (decimal)row[5];
                    // 6 MetadataPath
                    VideoProtectionState protectionStatus = (VideoProtectionState)((uint)row[7]);
                    bool infiniteProtection = (bool)row[8];
                    // 9 Drive
                    // 10 Error (I don't know what the Archiver puts there, it was always 0 while I tested.)
                    DateTime protectionEndDateTime = new DateTime(((DateTime)row[11]).Ticks, DateTimeKind.Utc); // this is datetime Max if not protected.
                    string protectionEndDateTimeString = (protectionStatus & VideoProtectionState.Unprotected) != 0 ? "" : 
                        infiniteProtection ? "infinite" : protectionEndDateTime.ToString();

                    Camera camera = m_sdkEngine.GetEntity(cameraId) as Camera;

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Text = camera != null ? camera.Name : "Unknown";
                    listViewItem.SubItems.Add(camera != null && camera.Role != null ? camera.Role.Name : "Unknown");
                    listViewItem.SubItems.Add(start.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(stop.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(path);
                    listViewItem.SubItems.Add(size.ToString());
                    listViewItem.SubItems.Add(protectionStatus.ToString()); 
                    listViewItem.SubItems.Add(infiniteProtection.ToString());
                    listViewItem.SubItems.Add(protectionEndDateTimeString); 
                    m_entityList.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnLaunchQueryClick(object sender, EventArgs e)
        {
            // Setting the search field to null tells the report engine to ignore them...
            m_cardholderQuery.FirstName = null;
            m_cardholderQuery.LastName = null;
            m_cardholderQuery.Description = null;

            m_cardholderQuery.FirstNameSearchMode = StringSearchMode.Contains;
            m_cardholderQuery.LastNameSearchMode = StringSearchMode.Contains;
            m_cardholderQuery.DescriptionSearchMode = StringSearchMode.Contains;

            if (m_firstName.Text.Length > 0)
            {
                m_cardholderQuery.FirstName = m_firstName.Text;
            }
            if (m_lastName.Text.Length > 0)
            {
                m_cardholderQuery.LastName = m_lastName.Text;
            }
            if (m_descriptionInput.Text.Length > 0)
            {
                m_cardholderQuery.Description = m_descriptionInput.Text;
            }
            m_cardholderQuery.BeginQuery(null, null);
        }

        private void OnLpmDailyUsageQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Date");
                m_entityList.Columns.Add("MaxShutdownTime");
                m_entityList.Columns.Add("MaxShutdownTimePercent");
                m_entityList.Columns.Add("MaxStopTime");
                m_entityList.Columns.Add("MaxStopTimePercent");
                m_entityList.Columns.Add("OperatingTime");
                m_entityList.Columns.Add("TotalShutdownTime");
                m_entityList.Columns.Add("TotalShutdownTimePercent");
                m_entityList.Columns.Add("TotalStopTime");
                m_entityList.Columns.Add("TotalStopTimePercent");
                m_entityList.Columns.Add("Instances");


                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        DateTime date = (DateTime)row["Date"];
                        double maxShutdownTime = (double)row["MaxShutdownTime"];
                        double maxShutdownTimePercent = (double)row["MaxShutdownTimePercent"];
                        double maxStopTime = (double)row["MaxStopTime"];
                        double maxStopTimePercent = (double)row["MaxStopTimePercent"];
                        double operatingTime = (double)row["OperatingTime"];
                        double totalShutdownTime = (double)row["TotalShutdownTime"];
                        double totalShutdownTimePercent = (double)row["TotalShutdownTimePercent"];
                        double totalStopTime = (double)row["TotalStopTime"];
                        double totalStopTimePercent = (double)row["TotalStopTimePercent"];
                        int instances = (int)row["Instances"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = date.ToLocalTime().ToString();
                        listViewItem.SubItems.Add(maxShutdownTime.ToString());
                        listViewItem.SubItems.Add(maxShutdownTimePercent.ToString());
                        listViewItem.SubItems.Add(maxStopTime.ToString());
                        listViewItem.SubItems.Add(maxStopTimePercent.ToString());
                        listViewItem.SubItems.Add(operatingTime.ToString());
                        listViewItem.SubItems.Add(totalShutdownTime.ToString());
                        listViewItem.SubItems.Add(totalShutdownTimePercent.ToString());
                        listViewItem.SubItems.Add(totalStopTime.ToString());
                        listViewItem.SubItems.Add(totalStopTimePercent.ToString());
                        listViewItem.SubItems.Add(instances.ToString());
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnLpmLoginRecorderPerPatrollerQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Date");
                m_entityList.Columns.Add("User");
                m_entityList.Columns.Add("LoginTimestamp");
                m_entityList.Columns.Add("LogoutTimestamp");


                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        Guid userId = (Guid)row["UserId"];
                        User user = null;

                        if (userId != Guid.Empty)
                            user = m_sdkEngine.GetEntity(userId) as User;

                        DateTime date = (DateTime)row["Date"];
                        DateTime loginTimestamp = (DateTime)row["LoginTimestamp"];
                        DateTime logoutTimestamp = (DateTime)row["LogoutTimestamp"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = date.ToLocalTime().ToString();
                        listViewItem.SubItems.Add(user != null ? user.Name : string.Empty);
                        listViewItem.SubItems.Add(loginTimestamp.ToLocalTime().ToString());
                        listViewItem.SubItems.Add(logoutTimestamp.ToLocalTime().ToString());
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnLpmReadsHitsStatsPerDayQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Date");
                m_entityList.Columns.Add("NbHits");
                m_entityList.Columns.Add("NbHitsEnforced");
                m_entityList.Columns.Add("NbHitsNotEnforced");
                m_entityList.Columns.Add("NbHitsRejected");
                m_entityList.Columns.Add("NbReads");


                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        DateTime date = (DateTime)row["Date"];
                        int numberOfHits = (int)row["NbHits"];
                        int numberOfHitsEnforced = (int)row["NbHitsEnforced"];
                        int numberOfHitsNotEnforced = (int)row["NbHitsNotEnforced"];
                        int numberOfHitsRejected = (int)row["NbHitsRejected"];
                        int numberOfReads = (int)row["NbReads"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = date.ToLocalTime().ToString();
                        listViewItem.SubItems.Add(numberOfHits.ToString());
                        listViewItem.SubItems.Add(numberOfHitsEnforced.ToString());
                        listViewItem.SubItems.Add(numberOfHitsNotEnforced.ToString());
                        listViewItem.SubItems.Add(numberOfHitsRejected.ToString());
                        listViewItem.SubItems.Add(numberOfReads.ToString());
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnLprHitQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("LPR Rule");
                m_entityList.Columns.Add("Plate");
                m_entityList.Columns.Add("Event timestamp");
                m_entityList.Columns.Add("Hit Type");
                m_entityList.Columns.Add("Unit");
                m_entityList.Columns.Add("Patroller");
                m_entityList.Columns.Add("Read1HasLprImage");
                m_entityList.Columns.Add("Read1HasLprImageThumbnail");
                m_entityList.Columns.Add("Read1HasContextImage");
                m_entityList.Columns.Add("Read1HasContextImageThumbnail");
                m_entityList.Columns.Add("Read1HasOverviewOrTireImage");
                m_entityList.Columns.Add("Read1HasOverviewOrTireImageThumbnail");
                m_entityList.Columns.Add("Read2HasLprImage");
                m_entityList.Columns.Add("Read2HasLprImageThumbnail");
                m_entityList.Columns.Add("Read2HasContextImage");
                m_entityList.Columns.Add("Read2HasContextImageThumbnail");
                m_entityList.Columns.Add("Read2HasOverviewOrTireImage");
                m_entityList.Columns.Add("Read2HasOverviewOrTireImageThumbnail");

                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        Guid ruleId = (Guid)row["LprRuleId"];
                        Entity lprRule = m_sdkEngine.GetEntity(ruleId);

                        Guid unitId = (Guid)row["Read1UnitId"];
                        LprUnit lprUnit = m_sdkEngine.GetEntity(unitId) as LprUnit;

                        Guid patrollerId = (Guid)row["Read1PatrollerId"];
                        Patroller patroller = m_sdkEngine.GetEntity(patrollerId) as Patroller;

                        DateTime eventTime = new DateTime(((DateTime)row["TimestampUtc"]).Ticks, DateTimeKind.Utc);
                        string plate = (string)row["Read1Plate"];

                        HitType hitType = (HitType)row["HitType"];

                        byte[] lprImage1 = (byte[])row["Read1LprImage"];
                        byte[] lprThumbnailImage1 = (byte[])row["Read1LprThumbnailImage"];
                        byte[] contextImage1 = (byte[])row["Read1ContextImage"];
                        byte[] contextThumbnailImage1 = (byte[])row["Read1ContextThumbnailImage"];
                        byte[] overviewOrTireImage1 = (byte[])row["Read1OverviewOrTireImage"];
                        byte[] overviewOrTireThumbnailImage1 = (byte[])row["Read1OverviewOrTireThumbnailImage"];
                        byte[] lprImage2 = (byte[])row["Read2LprImage"];
                        byte[] lprThumbnailImage2 = (byte[])row["Read2LprThumbnailImage"];
                        byte[] contextImage2 = (byte[])row["Read2ContextImage"];
                        byte[] contextThumbnailImage2 = (byte[])row["Read2ContextThumbnailImage"];
                        byte[] overviewOrTireImage2 = (byte[])row["Read2OverviewOrTireImage"];
                        byte[] overviewOrTireThumbnailImage2 = (byte[])row["Read2OverviewOrTireThumbnailImage"];



                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = lprRule.Name;
                        listViewItem.SubItems.Add(plate);
                        listViewItem.SubItems.Add(eventTime.ToLocalTime().ToString());
                        listViewItem.SubItems.Add(hitType.ToString());
                        listViewItem.SubItems.Add(lprUnit != null ? lprUnit.Name : "Unknown");
                        listViewItem.SubItems.Add(patroller != null ? patroller.Name : string.Empty);
                        listViewItem.SubItems.Add(lprImage1 != null && lprImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(lprThumbnailImage1 != null && lprThumbnailImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextImage1 != null && contextImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextThumbnailImage1 != null && contextThumbnailImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireImage1 != null && overviewOrTireImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireThumbnailImage1 != null && overviewOrTireThumbnailImage1.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(lprImage2 != null && lprImage2.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(lprThumbnailImage2 != null && lprThumbnailImage2.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextImage2 != null && contextImage2.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextThumbnailImage2 != null && contextThumbnailImage2.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireImage2 != null && overviewOrTireImage2.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireThumbnailImage2 != null && overviewOrTireThumbnailImage2.Length > 0 ? "true" : "false");

                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnLprReadQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Plate");
                m_entityList.Columns.Add("Location");
                m_entityList.Columns.Add("Event timestamp");
                m_entityList.Columns.Add("Unit");
                m_entityList.Columns.Add("Patroller");
                m_entityList.Columns.Add("HasLprImage");
                m_entityList.Columns.Add("HasLprImageThumbnail");
                m_entityList.Columns.Add("HasContextImage");
                m_entityList.Columns.Add("HasContextImageThumbnail");
                m_entityList.Columns.Add("HasOverviewOrTireImage");
                m_entityList.Columns.Add("HasOverviewOrTireImageThumbnail");

                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        Guid unitId = (Guid)row["UnitId"];
                        LprUnit lprUnit = m_sdkEngine.GetEntity(unitId) as LprUnit;

                        Guid patrollerId = (Guid)row["PatrollerId"];
                        Patroller patroller = m_sdkEngine.GetEntity(patrollerId) as Patroller;

                        DateTime eventTime = new DateTime(((DateTime)row["TimestampUtc"]).Ticks, DateTimeKind.Utc);
                        string plate = (string)row["Plate"];
                        string location = (int)row["CivicNumber"] + " " + (string)row["Street"];

                        byte[] lprImage = (byte[])row["LprImage"];
                        byte[] lprThumbnailImage = (byte[])row["LprThumbnailImage"];
                        byte[] contextImage = (byte[])row["ContextImage"];
                        byte[] contextThumbnailImage = (byte[])row["ContextThumbnailImage"];
                        byte[] overviewOrTireImage = (byte[])row["OverviewOrTireImage"];
                        byte[] overviewOrTireThumbnailImage = (byte[])row["OverviewOrTireThumbnailImage"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = plate;
                        listViewItem.SubItems.Add(location);
                        listViewItem.SubItems.Add(eventTime.ToLocalTime().ToString());
                        listViewItem.SubItems.Add(lprUnit != null ? lprUnit.Name : "Unknown");
                        listViewItem.SubItems.Add(patroller != null ? patroller.Name : string.Empty);
                        listViewItem.SubItems.Add(lprImage != null && lprImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(lprThumbnailImage != null && lprThumbnailImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextImage != null && contextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextThumbnailImage != null && contextThumbnailImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireImage != null && overviewOrTireImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(overviewOrTireThumbnailImage != null && overviewOrTireThumbnailImage.Length > 0 ? "true" : "false");
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnInventoryQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Plate");
                m_entityList.Columns.Add("PlateOrigin");
                m_entityList.Columns.Add("Sector");
                m_entityList.Columns.Add("Row");
                m_entityList.Columns.Add("Action");
                m_entityList.Columns.Add("Arrival");
                m_entityList.Columns.Add("ElapsedTime");
                m_entityList.Columns.Add("ManualCapture");
                m_entityList.Columns.Add("Inventory");
                m_entityList.Columns.Add("ContextImage");
                m_entityList.Columns.Add("ContextThumbnailImage");
                m_entityList.Columns.Add("Edited");
                m_entityList.Columns.Add("Timestamp");
                m_entityList.Columns.Add("ManuallyRemoved");
                m_entityList.Columns.Add("Parking");
                m_entityList.Columns.Add("Patroller");
                m_entityList.Columns.Add("LprImage");
                m_entityList.Columns.Add("LprThumbnailImage");

                if (e.Success)
                {
                    foreach (DataRow dr in e.Data.Rows)
                    {
                        string plate = (string)dr["PlateRead"];
                        string plateState = (string)dr["PlateOrigin"];
                        string sector = (string)dr["Sector"];
                        string rowName = (string)dr["Row"];
                        string action = (string)dr["Action"];
                        DateTime? arrival = (DateTime?)(dr.IsNull("Arrival") ? null : dr["Arrival"]);
                        TimeSpan? elapsedTime = (TimeSpan?)(dr.IsNull("ElapsedTime") ? null : dr["ElapsedTime"]);
                        bool manualCapture = (bool)dr["ManualCapture"];
                        short inventory = (short)dr["Inventory"];
                        byte[] contextImage = (byte[])dr["ContextImage"];
                        byte[] contextThumb = (byte[])dr["ContextThumbnailImage"];
                        bool edited = (bool)dr["Edited"];
                        DateTime? timestamp = (DateTime?)(dr.IsNull("Timestamp") ? null : dr["Timestamp"]);
                        bool manuallyRemoved = (bool)dr["ManuallyRemoved"];
                        Guid parking = (Guid)dr["Parking"];
                        Guid patroller = (Guid)dr["Patroller"];
                        byte[] lprImage = (byte[])dr["LprImage"];
                        byte[] lprThumbnailImage = (byte[])dr["LprThumbnailImage"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = plate;
                        listViewItem.SubItems.Add(plateState);
                        listViewItem.SubItems.Add(sector);
                        listViewItem.SubItems.Add(rowName);
                        listViewItem.SubItems.Add(action);
                        listViewItem.SubItems.Add(arrival.HasValue ? arrival.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(elapsedTime.HasValue ? elapsedTime.Value.TotalSeconds.ToString() : "");
                        listViewItem.SubItems.Add(manualCapture.ToString());
                        listViewItem.SubItems.Add(inventory.ToString());
                        listViewItem.SubItems.Add(contextImage != null && contextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(contextThumb != null && contextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(edited.ToString());
                        listViewItem.SubItems.Add(timestamp.HasValue ? timestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(manuallyRemoved.ToString());
                        listViewItem.SubItems.Add(parking.ToString());
                        listViewItem.SubItems.Add(patroller.ToString());
                        listViewItem.SubItems.Add(lprImage != null && contextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(lprThumbnailImage != null && contextImage.Length > 0 ? "true" : "false");
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnParkingSessionByIdQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                m_entityList.Columns.Clear();
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ParkingZoneIdColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ParkingZoneRuleIdColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.EntryPlateNumberColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.EntryContextImageColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.EntryLprImageColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ExitPlateNumberColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.StartTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ViolationTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.CompletedTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.TotalDurationColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.SessionStateColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.StateReasonColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ConvenienceTimeTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.EnforcedDurationColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.EnforcedTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ExitContextImageColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ExitLprImageColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.GracePeriodDurationColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.GracePeriodTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.PaidDurationColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.PaidTimestampColumnName);
                m_entityList.Columns.Add(ParkingSessionByIdQuery.ViolationDurationColumnName);

                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        Guid parkingZoneId = (Guid)row[ParkingSessionByIdQuery.ParkingZoneIdColumnName];
                        ParkingZone parkingZone = m_sdkEngine.GetEntity(parkingZoneId) as ParkingZone;

                        Guid parkingZoneRuleId = (Guid)row[ParkingSessionByIdQuery.ParkingZoneRuleIdColumnName];
                        ParkingZoneRule parkingZoneRule = m_sdkEngine.GetEntity(parkingZoneRuleId) as ParkingZoneRule;

                        string entryPlateNumber = (string)row[ParkingSessionByIdQuery.EntryPlateNumberColumnName];
                        byte[] entryContextImage = (byte[])row[ParkingSessionByIdQuery.EntryContextImageColumnName];
                        byte[] entryLPRImage = (byte[])row[ParkingSessionByIdQuery.EntryLprImageColumnName];

                        string exitPlateNumber = (string)row[ParkingSessionByIdQuery.ExitPlateNumberColumnName];

                        DateTime? startTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.StartTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.StartTimestampColumnName]);

                        DateTime? violationTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.ViolationTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.ViolationTimestampColumnName]);

                        DateTime? completedTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.CompletedTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.CompletedTimestampColumnName]);

                        TimeSpan? totalDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.TotalDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.TotalDurationColumnName]);

                        int sessionState = (int)row[ParkingSessionByIdQuery.SessionStateColumnName];
                        int stateReason = (int)row[ParkingSessionByIdQuery.StateReasonColumnName];

                        TimeSpan? convenienceDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName]);

                        DateTime? convenienceTimeTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.ConvenienceTimeTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.ConvenienceTimeTimestampColumnName]);

                        TimeSpan? enforcedDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.EnforcedDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.EnforcedDurationColumnName]);

                        DateTime? enforcedTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.EnforcedTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.EnforcedTimestampColumnName]);

                        byte[] exitContextImage = (byte[])row[ParkingSessionByIdQuery.ExitContextImageColumnName];
                        byte[] exitLprImage = (byte[])row[ParkingSessionByIdQuery.ExitLprImageColumnName];

                        TimeSpan? gracePeriodDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName]);

                        DateTime? gracePeriodTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.ConvenienceTimeTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.ConvenienceTimeTimestampColumnName]);

                        TimeSpan? paidDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.EnforcedDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.EnforcedDurationColumnName]);

                        DateTime? paidTimestamp = (DateTime?)(row.IsNull(ParkingSessionByIdQuery.EnforcedTimestampColumnName)
                            ? null : row[ParkingSessionByIdQuery.EnforcedTimestampColumnName]);

                        TimeSpan? violationDuration = (TimeSpan?)(row.IsNull(ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName)
                            ? null : row[ParkingSessionByIdQuery.ConvenienceTimeDurationColumnName]);

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = parkingZone.Name;
                        listViewItem.SubItems.Add(parkingZoneRule != null ? parkingZoneRule.Name : "Unknown");
                        listViewItem.SubItems.Add(entryPlateNumber);
                        listViewItem.SubItems.Add(entryContextImage != null && entryContextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(entryLPRImage != null && entryLPRImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(exitPlateNumber);
                        listViewItem.SubItems.Add(startTimestamp.HasValue ? startTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(violationTimestamp.HasValue ? violationTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(completedTimestamp.HasValue ? completedTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(totalDuration.HasValue ? totalDuration.Value.ToString() : "");
                        listViewItem.SubItems.Add(sessionState.ToString());
                        listViewItem.SubItems.Add(stateReason.ToString());
                        listViewItem.SubItems.Add(convenienceDuration.HasValue ? convenienceDuration.Value.ToString() : "");
                        listViewItem.SubItems.Add(convenienceTimeTimestamp.HasValue ? convenienceTimeTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(enforcedDuration.HasValue ? enforcedDuration.Value.ToString() : "");
                        listViewItem.SubItems.Add(enforcedTimestamp.HasValue ? enforcedTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(exitContextImage != null && exitContextImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(exitLprImage != null && exitLprImage.Length > 0 ? "true" : "false");
                        listViewItem.SubItems.Add(gracePeriodDuration.HasValue ? gracePeriodDuration.Value.ToString() : "");
                        listViewItem.SubItems.Add(gracePeriodTimestamp.HasValue ? gracePeriodTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(paidDuration.HasValue ? paidDuration.Value.ToString() : "");
                        listViewItem.SubItems.Add(paidTimestamp.HasValue ? paidTimestamp.Value.ToLocalTime().ToString() : "");
                        listViewItem.SubItems.Add(violationDuration.HasValue ? violationDuration.Value.ToString() : "");
                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnPatrollerPositionQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a read event
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Patroller");
                m_entityList.Columns.Add("Timestamp");
                m_entityList.Columns.Add("MapType");
                m_entityList.Columns.Add("X");
                m_entityList.Columns.Add("Y");
                m_entityList.Columns.Add("Heading");
                m_entityList.Columns.Add("Speed");

                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        Guid patrollerId = (Guid)row["PatrollerId"];
                        Patroller patroller = m_sdkEngine.GetEntity(patrollerId) as Patroller;

                        DateTime eventTime = new DateTime(((DateTime)row["TimestampUtc"]).Ticks, DateTimeKind.Utc);
                        int mapType = (int)row["MapType"];
                        double x = (double)row["X"];
                        double y = (double)row["Y"];
                        double heading = (double)row["Heading"];
                        double speed = (double)row["Speed"];

                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = patroller != null ? patroller.Name : string.Empty;
                        listViewItem.SubItems.Add(eventTime.ToLocalTime().ToString());
                        listViewItem.SubItems.Add(mapType.ToString());
                        listViewItem.SubItems.Add(x.ToString());
                        listViewItem.SubItems.Add(y.ToString());
                        listViewItem.SubItems.Add(heading.ToString());
                        listViewItem.SubItems.Add(speed.ToString());

                        m_entityList.Items.Add(listViewItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnSequenceQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a sequence
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Camera");
                m_entityList.Columns.Add("Source");
                m_entityList.Columns.Add("Start");
                m_entityList.Columns.Add("Stop");

                foreach (DataRow row in e.Data.Rows)
                {
                    Guid cameraId = (Guid)row[0];
                    DateTime start = new DateTime(((DateTime)row[2]).Ticks, DateTimeKind.Utc);
                    DateTime stop = new DateTime(((DateTime)row[3]).Ticks, DateTimeKind.Utc);
                    string message = (string)row[5];

                    Camera camera = m_sdkEngine.GetEntity(cameraId) as Camera;

                    ListViewItem listViewItem = new ListViewItem {Text = camera != null ? camera.Name : "Unknown"};
                    listViewItem.SubItems.Add(camera != null && camera.Role != null ? camera.Role.Name : "Unknown");
                    listViewItem.SubItems.Add(start.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(stop.ToLocalTime().ToString());
                    listViewItem.SubItems.Add(message);
                    m_entityList.Items.Add(listViewItem);
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void OnTimeAttendanceQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a Time & attendance e...
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Time");
                m_entityList.Columns.Add("Cardholder");
                m_entityList.Columns.Add("Area");
                m_entityList.Columns.Add("TotalMinutes");

                if (e.Success)
                {
                    foreach (DataRow rows in e.Data.Rows)
                    {
                        DateTime date = new DateTime(((DateTime)rows[0]).Ticks, DateTimeKind.Utc);
                        Guid cardholder = (Guid)rows[1];
                        Guid area = (Guid)rows[2];
                        int totalMinute = (int)rows[3];

                        string cardholderName = "";
                        string areaName = "";

                        Entity boEntity = m_sdkEngine.GetEntity(cardholder);
                        if (boEntity != null)
                        {
                            cardholderName = boEntity.Name;
                        }

                        boEntity = m_sdkEngine.GetEntity(area);
                        if (boEntity != null)
                        {
                            areaName = boEntity.Name;
                        }

                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = date.ToString();
                        lvItem.SubItems.Add(cardholderName);
                        lvItem.SubItems.Add(areaName);
                        lvItem.SubItems.Add(totalMinute.ToString());
                        m_entityList.Items.Add(lvItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        

        private async void OnVisitorQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            //The visitors and their Company custom field.
            var results = new List<Tuple<Visitor, string>>();

            if (e.Success)
            {
                //Collect the guids from the query result.
                var guids = e.Data.Rows.Cast<DataRow>()
                    .Select(row => (Guid)row[0])
                    .ToList(); //Force the evaluation to avoid executing lookup on the thread pool.

                //Avoid opening a transaction for no reason.
                if (guids.Any())
                {
                    try
                    {
                        //Execute the getting of the entities and their custom fields on a transaction to avoid multiple round-trip to the directory.
                        results.AddRange(await m_sdkEngine.TransactionManager.ExecuteTransactionAsync(() =>
                        {
                            //Get the CustomFieldService.
                            var systemConfiguration = m_sdkEngine.GetEntity<SystemConfiguration>(SdkGuids.SystemConfiguration);
                            var service = systemConfiguration.CustomFieldService;

                            //Get the entity and the Company Custom Field.
                            return (from guid in guids
                                    let visitor = m_sdkEngine.GetEntity<Visitor>(guid)
                                    let value = service.GetValue<string>("Company", visitor.Guid)
                                    select Tuple.Create(visitor, value))
                                .ToList();
                        }));
                    }
                    catch (SdkException exception)
                    {
                        if (exception.ErrorCode == SdkError.UnableToRetrieveEntity)
                            MessageBox.Show(exception.Message, "Error");
                        else
                            throw;
                    }
                }
            }

            //Update the List View.
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                //Format the list view to receive a cardholder.
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("FirstName");
                m_entityList.Columns.Add("LastName");
                m_entityList.Columns.Add("Company");

                //Lets build the ListViewItems for the visitors.
                var visitorInfo = results.Select(visitor_company =>
                    {
                        var visitor = visitor_company.Item1;
                        var company = visitor_company.Item2;
                        return new ListViewItem
                        {
                            Text = visitor.FirstName,
                            SubItems = { visitor.LastName, company ?? string.Empty }
                        };
                    })
                    .ToArray();
                m_entityList.Items.AddRange(visitorInfo);
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        #endregion

        #region Private Methods

        private void ActivityQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a swipe activity e...
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Timestamp");
                m_entityList.Columns.Add("Door");
                m_entityList.Columns.Add("Side");
                m_entityList.Columns.Add("Event");
                m_entityList.Columns.Add("Cardholder");
                m_entityList.Columns.Add("Credential");
                m_entityList.Columns.Add("Unit");
                m_entityList.Columns.Add("Device");

                if (e.Success)
                {
                    foreach (DataRow rows in e.Data.Rows)
                    {
                        DateTime date = new DateTime(((DateTime)rows[0]).Ticks, DateTimeKind.Utc);
                        EventType eventType = (EventType)rows[1];
                        Guid unit = (Guid)rows[2];
                        Guid device = (Guid)rows[3];
                        Guid ap = (Guid)rows[4];
                        Guid apg = (Guid)rows[5];
                        Guid credential = (Guid)rows[6];
                        Guid cardholder = (Guid)rows[7];

                        string unitName = "";
                        string apgName = "";
                        string credentialName = "";
                        string cardholderName = "";
                        string apName = "";
                        string deviceName = "";

                        Entity boEntity = m_sdkEngine.GetEntity(apg);
                        if (boEntity is AccessPointGroup)
                        {
                            AccessPointGroup boAPG = boEntity as AccessPointGroup;
                            apgName = boAPG.Name;

                            boEntity = m_sdkEngine.GetEntity(ap);
                            if (boEntity != null)
                            {
                                apName = boEntity.Name;
                            }
                        }
                        boEntity = m_sdkEngine.GetEntity(credential);
                        if (boEntity != null)
                        {
                            credentialName = boEntity.Name;
                        }
                        Cardholder boCardholder = m_sdkEngine.GetEntity(cardholder) as Cardholder;
                        if (boCardholder != null)
                        {
                            cardholderName = string.Format("{0} {1}", boCardholder.FirstName, boCardholder.LastName);
                        }
                        boEntity = m_sdkEngine.GetEntity(unit);
                        if (boEntity != null)
                        {
                            unitName = boEntity.Name;
                        }
                        boEntity = m_sdkEngine.GetEntity(device);
                        if (boEntity != null)
                        {
                            deviceName = boEntity.Name;
                        }

                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = date.ToLocalTime().ToString();
                        lvItem.SubItems.Add(apgName);
                        lvItem.SubItems.Add(apName);
                        lvItem.SubItems.Add(eventType.ToString());
                        lvItem.SubItems.Add(cardholderName);
                        lvItem.SubItems.Add(credentialName);
                        lvItem.SubItems.Add(unitName);
                        lvItem.SubItems.Add(deviceName);
                        m_entityList.Items.Add(lvItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        private void AlarmQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive an alarm activity e...
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("InstanceId");
                m_entityList.Columns.Add("Alarm");
                m_entityList.Columns.Add("TriggerEntity");
                m_entityList.Columns.Add("TriggerEvent");
                m_entityList.Columns.Add("TriggerTime");
                m_entityList.Columns.Add("AckTime");
                m_entityList.Columns.Add("AckBy");

                if (e.Success)
                {
                    foreach (DataRow row in e.Data.Rows)
                    {
                        int instanceId = (int)row[0];
                        Guid alarm = (Guid)row[1];
                        Guid triggerEntity = (Guid)row[2];
                        EventType triggerEvent = (EventType)((int)row[3]);
                        DateTime triggerTime = new DateTime(((DateTime)row[4]).Ticks, DateTimeKind.Utc);
                        DateTime ackTime = new DateTime(((DateTime)row[5]).Ticks, DateTimeKind.Utc);
                        Guid ackBy = (Guid)row[6];

                        string alarmName = "";
                        string triggerEntityName = "";
                        string ackByName = "";

                        Entity entity = m_sdkEngine.GetEntity(alarm);
                        if (entity != null)
                        {
                            alarmName = entity.Name;
                        }
                        entity = m_sdkEngine.GetEntity(triggerEntity);
                        if (entity != null)
                        {
                            triggerEntityName = entity.Name;
                        }
                        entity = m_sdkEngine.GetEntity(ackBy);
                        if (entity != null)
                        {
                            ackByName = entity.Name;
                        }

                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = string.Format("{0}", instanceId);
                        lvItem.SubItems.Add(alarmName);
                        lvItem.SubItems.Add(triggerEntityName);
                        lvItem.SubItems.Add(triggerEvent.ToString());
                        lvItem.SubItems.Add(triggerTime.ToString());
                        lvItem.SubItems.Add(ackTime.ToString());
                        lvItem.SubItems.Add(ackByName);
                        m_entityList.Items.Add(lvItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        /// <summary>
        /// Subscribe to the Engine events
        /// </summary>
        private void SubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
        }

        /// <summary>
        /// Unsubscribe from the Engine events
        /// </summary>
        private void UnsubscribeEngine()
        {
            m_sdkEngine.LoginManager.LoggedOn -= OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff -= OnEngineLoggedOff;
        }

        private void ZoneActivityQueryCompleted(object sender, QueryCompletedEventArgs e)
        {
            try
            {
                m_entityList.BeginUpdate();
                m_entityList.Items.Clear();

                // Format the list view to receive a swipe activity e...
                m_entityList.Columns.Clear();
                m_entityList.Columns.Add("Event");
                m_entityList.Columns.Add("Zone");
                m_entityList.Columns.Add("Timestamp");
                m_entityList.Columns.Add("OfflinePeriod");

                if (e.Success)
                {
                    foreach (DataRow rows in e.Data.Rows)
                    {
                        DateTime timestampLocal = new DateTime(((DateTime)rows[2]).Ticks, DateTimeKind.Utc);
                        EventType eventType = (EventType)rows[4];
                        Guid zone = (Guid)rows[5];
                        OfflinePeriodType offlinePeriod = (OfflinePeriodType)rows[6];

                        string zoneName = "";

                        Entity boEntity = m_sdkEngine.GetEntity(zone);
                        if (boEntity is Zone)
                        {
                            Zone boZone = boEntity as Zone;
                            zoneName = boZone.Name;
                        }

                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = eventType.ToString();
                        lvItem.SubItems.Add(zoneName);
                        lvItem.SubItems.Add(timestampLocal.ToString());
                        lvItem.SubItems.Add(offlinePeriod.ToString());
                        m_entityList.Items.Add(lvItem);
                    }
                }
            }
            finally
            {
                m_entityList.EndUpdate();
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing && m_components != null)
            {
                m_components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Always dispose the SDK object when closing the application
            if (m_sdkEngine != null)
            {
                // Unsubscribe from the Engine events
                UnsubscribeEngine();

                m_sdkEngine.Dispose();
                m_sdkEngine = null;
            }
        }

        private void m_cardholderActivityList_Load(object sender, EventArgs e)
        {

        }
    }

    #endregion
}

