using System;
// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 27:
//  1959 – Typhoon Vera kills nearly 5,000 people in Japan.
//  1993 – The Sukhumi massacre takes place in Abkhazia.
//  2014 – Eruption of Mount Ontake in Japan occurs.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class MainDlg
    {
        #region Fields

        private AlarmQueryUI m_alarmActivity;

        private System.Windows.Forms.TabPage m_alarmActivityTab;

        private ActivityListQuery m_areaActivityList;

        private System.Windows.Forms.TabPage m_areaActivityTab;

        private BookmarkQueryUI m_bookmark;

        private System.Windows.Forms.TabPage m_bookmarkTab;

        private System.Windows.Forms.TabPage m_cardholder;

        private ActivityListQuery m_cardholderActivityList;

        private System.Windows.Forms.TabPage m_cardholderActivityTab;

        private System.Windows.Forms.ListView m_entityList;

        private System.Windows.Forms.ColumnHeader m_chEvent;

        private System.Windows.Forms.ColumnHeader m_chTimestamp;

        private SequenceQueryUI m_sequence;

        private System.Windows.Forms.TabPage m_sequenceTab;

        private CameraIntegrityQueryUI m_cameraIntegrity;

        private System.Windows.Forms.TabPage m_cameraIntegrityTab;

        private BlockingQueryUI m_blockingQuery;

        private System.Windows.Forms.TabPage m_blockingTab;

        private System.Windows.Forms.TabPage m_videoProtectionTab;
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer m_components = null;

        private System.Windows.Forms.ToolStripButton m_connect;

        private ActivityListQuery m_credentialActivityList;

        private System.Windows.Forms.TabPage m_credentialActivityTab;

        private System.Windows.Forms.Label m_description;

        private System.Windows.Forms.TextBox m_descriptionInput;

        private System.Windows.Forms.ToolStripButton m_disconnect;

        private ActivityListQuery m_doorActivityList;

        private System.Windows.Forms.TabPage m_doorActivityTab;

        private System.Windows.Forms.Label m_familyName;

        private System.Windows.Forms.TextBox m_firstName;

        private InventoryQueryUI m_inventory;

        private System.Windows.Forms.TabPage m_inventoryTab;

        private System.Windows.Forms.TextBox m_lastName;

        private HitQueryUI m_lprHit;

        private System.Windows.Forms.TabPage m_lprHitTab;

        private ReadQueryUI m_lprRead;

        private System.Windows.Forms.TabPage m_lprReadTab;

        private System.Windows.Forms.Label m_name;

        private ParkingSessionByIdQueryUI m_parkingSessionById;

        private System.Windows.Forms.TabPage m_parkingSessionByIdTab;

        private PatrollerPositionQueryUI m_patrollerPosition;

        private System.Windows.Forms.TabPage m_patrollerPositionTab;

        private System.Windows.Forms.Button m_query;

        private System.Windows.Forms.TabControl m_querySampleTabs;

        private System.Windows.Forms.TabPage m_tabLpmDailyUsage;

        private TimeAttendanceUI m_timeAttendanceList;

        private System.Windows.Forms.TabPage m_timeAttendanceTab;

        private System.Windows.Forms.ToolStrip m_tsMain;

        private VisitorQueryUI m_visitors;

        private System.Windows.Forms.TabPage m_visitorTab;

        private ActivityListQuery m_zoneActivityList;

        private System.Windows.Forms.TabPage m_zoneActivityTab;

        #endregion

        #region Initialize Component

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.m_chEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_chTimestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_entityList = new System.Windows.Forms.ListView();
            this.m_tsMain = new System.Windows.Forms.ToolStrip();
            this.m_connect = new System.Windows.Forms.ToolStripButton();
            this.m_disconnect = new System.Windows.Forms.ToolStripButton();
            this.m_tabLpmDailyUsage = new System.Windows.Forms.TabPage();
            this.m_patrollerPositionTab = new System.Windows.Forms.TabPage();
            this.m_patrollerPosition = new QuerySample.PatrollerPositionQueryUI();
            this.m_lprHitTab = new System.Windows.Forms.TabPage();
            this.m_lprHit = new QuerySample.HitQueryUI();
            this.m_inventoryTab = new System.Windows.Forms.TabPage();
            this.m_inventory = new QuerySample.InventoryQueryUI();
            this.m_lprReadTab = new System.Windows.Forms.TabPage();
            this.m_lprRead = new QuerySample.ReadQueryUI();
            this.m_bookmarkTab = new System.Windows.Forms.TabPage();
            this.m_bookmark = new QuerySample.BookmarkQueryUI();
            this.m_timeAttendanceTab = new System.Windows.Forms.TabPage();
            this.m_timeAttendanceList = new QuerySample.TimeAttendanceUI();
            this.m_visitorTab = new System.Windows.Forms.TabPage();
            this.m_visitors = new QuerySample.VisitorQueryUI();
            this.m_alarmActivityTab = new System.Windows.Forms.TabPage();
            this.m_alarmActivity = new QuerySample.AlarmQueryUI();
            this.m_zoneActivityTab = new System.Windows.Forms.TabPage();
            this.m_zoneActivityList = new QuerySample.ActivityListQuery();
            this.m_credentialActivityTab = new System.Windows.Forms.TabPage();
            this.m_credentialActivityList = new QuerySample.ActivityListQuery();
            this.m_cardholderActivityTab = new System.Windows.Forms.TabPage();
            this.m_cardholderActivityList = new QuerySample.ActivityListQuery();
            this.m_areaActivityTab = new System.Windows.Forms.TabPage();
            this.m_areaActivityList = new QuerySample.ActivityListQuery();
            this.m_querySampleTabs = new System.Windows.Forms.TabControl();
            this.m_cardholder = new System.Windows.Forms.TabPage();
            this.m_name = new System.Windows.Forms.Label();
            this.m_query = new System.Windows.Forms.Button();
            this.m_description = new System.Windows.Forms.Label();
            this.m_familyName = new System.Windows.Forms.Label();
            this.m_descriptionInput = new System.Windows.Forms.TextBox();
            this.m_lastName = new System.Windows.Forms.TextBox();
            this.m_firstName = new System.Windows.Forms.TextBox();
            this.m_doorActivityTab = new System.Windows.Forms.TabPage();
            this.m_doorActivityList = new QuerySample.ActivityListQuery();
            this.m_sequenceTab = new System.Windows.Forms.TabPage();
            this.m_sequence = new QuerySample.SequenceQueryUI();
            this.m_cameraIntegrityTab = new System.Windows.Forms.TabPage();
            this.m_cameraIntegrity = new QuerySample.CameraIntegrityQueryUI();
            this.m_blockingTab = new System.Windows.Forms.TabPage();
            this.m_blockingQuery = new QuerySample.BlockingQueryUI();
            this.m_videoProtectionTab = new System.Windows.Forms.TabPage();
            this.m_videoFileProtection = new QuerySample.VideoFileProtectionUI();
            this.m_parkingSessionByIdTab = new System.Windows.Forms.TabPage();
            this.m_parkingSessionById = new QuerySample.ParkingSessionByIdQueryUI();
            this.m_tsMain.SuspendLayout();
            this.m_patrollerPositionTab.SuspendLayout();
            this.m_lprHitTab.SuspendLayout();
            this.m_inventoryTab.SuspendLayout();
            this.m_lprReadTab.SuspendLayout();
            this.m_bookmarkTab.SuspendLayout();
            this.m_timeAttendanceTab.SuspendLayout();
            this.m_visitorTab.SuspendLayout();
            this.m_alarmActivityTab.SuspendLayout();
            this.m_zoneActivityTab.SuspendLayout();
            this.m_credentialActivityTab.SuspendLayout();
            this.m_cardholderActivityTab.SuspendLayout();
            this.m_areaActivityTab.SuspendLayout();
            this.m_querySampleTabs.SuspendLayout();
            this.m_cardholder.SuspendLayout();
            this.m_doorActivityTab.SuspendLayout();
            this.m_sequenceTab.SuspendLayout();
            this.m_cameraIntegrityTab.SuspendLayout();
            this.m_blockingTab.SuspendLayout();
            this.m_videoProtectionTab.SuspendLayout();
            this.m_parkingSessionByIdTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_chEvent
            // 
            this.m_chEvent.Text = "Event";
            this.m_chEvent.Width = 250;
            // 
            // m_chTimestamp
            // 
            this.m_chTimestamp.Text = "Timestamp";
            this.m_chTimestamp.Width = 150;
            // 
            // m_entityList
            // 
            this.m_entityList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_entityList.GridLines = true;
            this.m_entityList.HideSelection = false;
            this.m_entityList.Location = new System.Drawing.Point(296, 50);
            this.m_entityList.Name = "m_entityList";
            this.m_entityList.Size = new System.Drawing.Size(357, 331);
            this.m_entityList.TabIndex = 3;
            this.m_entityList.UseCompatibleStateImageBehavior = false;
            this.m_entityList.View = System.Windows.Forms.View.Details;
            // 
            // m_tsMain
            // 
            this.m_tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_connect,
            this.m_disconnect});
            this.m_tsMain.Location = new System.Drawing.Point(0, 0);
            this.m_tsMain.Name = "m_tsMain";
            this.m_tsMain.Size = new System.Drawing.Size(665, 25);
            this.m_tsMain.TabIndex = 5;
            this.m_tsMain.Text = "toolStrip1";
            // 
            // m_connect
            // 
            this.m_connect.Image = ((System.Drawing.Image)(resources.GetObject("m_connect.Image")));
            this.m_connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_connect.Name = "m_connect";
            this.m_connect.Size = new System.Drawing.Size(72, 22);
            this.m_connect.Text = "Connect";
            this.m_connect.Click += new System.EventHandler(this.OnConnectClick);
            // 
            // m_disconnect
            // 
            this.m_disconnect.Enabled = false;
            this.m_disconnect.Image = ((System.Drawing.Image)(resources.GetObject("m_disconnect.Image")));
            this.m_disconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_disconnect.Name = "m_disconnect";
            this.m_disconnect.Size = new System.Drawing.Size(86, 22);
            this.m_disconnect.Text = "Disconnect";
            this.m_disconnect.Click += new System.EventHandler(this.OnDisconnectClick);
            // 
            // m_tabLpmDailyUsage
            // 
            this.m_tabLpmDailyUsage.Location = new System.Drawing.Point(4, 22);
            this.m_tabLpmDailyUsage.Name = "m_tabLpmDailyUsage";
            this.m_tabLpmDailyUsage.Size = new System.Drawing.Size(274, 327);
            this.m_tabLpmDailyUsage.TabIndex = 13;
            // 
            // m_patrollerPositionTab
            // 
            this.m_patrollerPositionTab.Controls.Add(this.m_patrollerPosition);
            this.m_patrollerPositionTab.Location = new System.Drawing.Point(4, 22);
            this.m_patrollerPositionTab.Name = "m_patrollerPositionTab";
            this.m_patrollerPositionTab.Size = new System.Drawing.Size(274, 327);
            this.m_patrollerPositionTab.TabIndex = 12;
            this.m_patrollerPositionTab.Text = "Patroller Position";
            this.m_patrollerPositionTab.UseVisualStyleBackColor = true;
            // 
            // m_patrollerPosition
            // 
            this.m_patrollerPosition.AutoSize = true;
            this.m_patrollerPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_patrollerPosition.Location = new System.Drawing.Point(0, 0);
            this.m_patrollerPosition.Name = "m_patrollerPosition";
            this.m_patrollerPosition.Size = new System.Drawing.Size(274, 327);
            this.m_patrollerPosition.TabIndex = 0;
            // 
            // m_lprHitTab
            // 
            this.m_lprHitTab.Controls.Add(this.m_lprHit);
            this.m_lprHitTab.Location = new System.Drawing.Point(4, 22);
            this.m_lprHitTab.Name = "m_lprHitTab";
            this.m_lprHitTab.Size = new System.Drawing.Size(274, 327);
            this.m_lprHitTab.TabIndex = 11;
            this.m_lprHitTab.Text = "LPR Hit";
            this.m_lprHitTab.UseVisualStyleBackColor = true;
            // 
            // m_lprHit
            // 
            this.m_lprHit.AutoSize = true;
            this.m_lprHit.Location = new System.Drawing.Point(0, 0);
            this.m_lprHit.Name = "m_lprHit";
            this.m_lprHit.Size = new System.Drawing.Size(265, 294);
            this.m_lprHit.TabIndex = 0;
            // 
            // m_inventoryTab
            // 
            this.m_inventoryTab.Controls.Add(this.m_inventory);
            this.m_inventoryTab.Location = new System.Drawing.Point(4, 22);
            this.m_inventoryTab.Name = "m_inventoryTab";
            this.m_inventoryTab.Size = new System.Drawing.Size(274, 327);
            this.m_inventoryTab.TabIndex = 10;
            this.m_inventoryTab.Text = "Inventory";
            this.m_inventoryTab.UseVisualStyleBackColor = true;
            // 
            // m_inventory
            // 
            this.m_inventory.AutoSize = true;
            this.m_inventory.Location = new System.Drawing.Point(0, 0);
            this.m_inventory.Name = "m_inventory";
            this.m_inventory.Size = new System.Drawing.Size(274, 327);
            this.m_inventory.TabIndex = 0;
            // 
            // m_lprReadTab
            // 
            this.m_lprReadTab.Controls.Add(this.m_lprRead);
            this.m_lprReadTab.Location = new System.Drawing.Point(4, 22);
            this.m_lprReadTab.Name = "m_lprReadTab";
            this.m_lprReadTab.Size = new System.Drawing.Size(274, 327);
            this.m_lprReadTab.TabIndex = 10;
            this.m_lprReadTab.Text = "LPR Read";
            this.m_lprReadTab.UseVisualStyleBackColor = true;
            // 
            // m_lprRead
            // 
            this.m_lprRead.AutoSize = true;
            this.m_lprRead.Location = new System.Drawing.Point(0, 0);
            this.m_lprRead.Name = "m_lprRead";
            this.m_lprRead.Size = new System.Drawing.Size(274, 327);
            this.m_lprRead.TabIndex = 0;
            // 
            // m_bookmarkTab
            // 
            this.m_bookmarkTab.Controls.Add(this.m_bookmark);
            this.m_bookmarkTab.Location = new System.Drawing.Point(4, 22);
            this.m_bookmarkTab.Name = "m_bookmarkTab";
            this.m_bookmarkTab.Size = new System.Drawing.Size(274, 327);
            this.m_bookmarkTab.TabIndex = 9;
            this.m_bookmarkTab.Text = "Bookmark";
            this.m_bookmarkTab.UseVisualStyleBackColor = true;
            // 
            // m_bookmark
            // 
            this.m_bookmark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_bookmark.Location = new System.Drawing.Point(0, 0);
            this.m_bookmark.Name = "m_bookmark";
            this.m_bookmark.Size = new System.Drawing.Size(274, 327);
            this.m_bookmark.TabIndex = 0;
            // 
            // m_timeAttendanceTab
            // 
            this.m_timeAttendanceTab.Controls.Add(this.m_timeAttendanceList);
            this.m_timeAttendanceTab.Location = new System.Drawing.Point(4, 22);
            this.m_timeAttendanceTab.Name = "m_timeAttendanceTab";
            this.m_timeAttendanceTab.Size = new System.Drawing.Size(274, 327);
            this.m_timeAttendanceTab.TabIndex = 8;
            this.m_timeAttendanceTab.Text = "TimeAttendance";
            this.m_timeAttendanceTab.UseVisualStyleBackColor = true;
            // 
            // m_timeAttendanceList
            // 
            this.m_timeAttendanceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_timeAttendanceList.Location = new System.Drawing.Point(0, 0);
            this.m_timeAttendanceList.Name = "m_timeAttendanceList";
            this.m_timeAttendanceList.Size = new System.Drawing.Size(274, 327);
            this.m_timeAttendanceList.TabIndex = 1;
            // 
            // m_visitorTab
            // 
            this.m_visitorTab.Controls.Add(this.m_visitors);
            this.m_visitorTab.Location = new System.Drawing.Point(4, 22);
            this.m_visitorTab.Name = "m_visitorTab";
            this.m_visitorTab.Size = new System.Drawing.Size(274, 327);
            this.m_visitorTab.TabIndex = 7;
            this.m_visitorTab.Text = "Visitor";
            this.m_visitorTab.UseVisualStyleBackColor = true;
            // 
            // m_visitors
            // 
            this.m_visitors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_visitors.Location = new System.Drawing.Point(0, 0);
            this.m_visitors.Name = "m_visitors";
            this.m_visitors.Size = new System.Drawing.Size(274, 327);
            this.m_visitors.TabIndex = 0;
            // 
            // m_alarmActivityTab
            // 
            this.m_alarmActivityTab.Controls.Add(this.m_alarmActivity);
            this.m_alarmActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_alarmActivityTab.Name = "m_alarmActivityTab";
            this.m_alarmActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_alarmActivityTab.TabIndex = 6;
            this.m_alarmActivityTab.Text = "Alarm Activity";
            this.m_alarmActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_alarmActivity
            // 
            this.m_alarmActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_alarmActivity.Location = new System.Drawing.Point(0, 0);
            this.m_alarmActivity.Name = "m_alarmActivity";
            this.m_alarmActivity.Size = new System.Drawing.Size(274, 327);
            this.m_alarmActivity.TabIndex = 0;
            // 
            // m_zoneActivityTab
            // 
            this.m_zoneActivityTab.Controls.Add(this.m_zoneActivityList);
            this.m_zoneActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_zoneActivityTab.Name = "m_zoneActivityTab";
            this.m_zoneActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_zoneActivityTab.TabIndex = 5;
            this.m_zoneActivityTab.Text = "Zone Activity";
            this.m_zoneActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_zoneActivityList
            // 
            this.m_zoneActivityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_zoneActivityList.Location = new System.Drawing.Point(0, 0);
            this.m_zoneActivityList.Name = "m_zoneActivityList";
            this.m_zoneActivityList.Size = new System.Drawing.Size(274, 327);
            this.m_zoneActivityList.TabIndex = 0;
            // 
            // m_credentialActivityTab
            // 
            this.m_credentialActivityTab.Controls.Add(this.m_credentialActivityList);
            this.m_credentialActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_credentialActivityTab.Name = "m_credentialActivityTab";
            this.m_credentialActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_credentialActivityTab.TabIndex = 4;
            this.m_credentialActivityTab.Text = "Credential Activity";
            this.m_credentialActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_credentialActivityList
            // 
            this.m_credentialActivityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_credentialActivityList.Location = new System.Drawing.Point(0, 0);
            this.m_credentialActivityList.Name = "m_credentialActivityList";
            this.m_credentialActivityList.Size = new System.Drawing.Size(274, 327);
            this.m_credentialActivityList.TabIndex = 0;
            // 
            // m_cardholderActivityTab
            // 
            this.m_cardholderActivityTab.Controls.Add(this.m_cardholderActivityList);
            this.m_cardholderActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_cardholderActivityTab.Name = "m_cardholderActivityTab";
            this.m_cardholderActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_cardholderActivityTab.TabIndex = 3;
            this.m_cardholderActivityTab.Text = "Cardholder Activity";
            this.m_cardholderActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_cardholderActivityList
            // 
            this.m_cardholderActivityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_cardholderActivityList.Location = new System.Drawing.Point(0, 0);
            this.m_cardholderActivityList.Name = "m_cardholderActivityList";
            this.m_cardholderActivityList.Size = new System.Drawing.Size(274, 327);
            this.m_cardholderActivityList.TabIndex = 0;
            // 
            // m_areaActivityTab
            // 
            this.m_areaActivityTab.Controls.Add(this.m_areaActivityList);
            this.m_areaActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_areaActivityTab.Name = "m_areaActivityTab";
            this.m_areaActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_areaActivityTab.TabIndex = 2;
            this.m_areaActivityTab.Text = "Area Activity";
            this.m_areaActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_areaActivityList
            // 
            this.m_areaActivityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_areaActivityList.Location = new System.Drawing.Point(0, 0);
            this.m_areaActivityList.Name = "m_areaActivityList";
            this.m_areaActivityList.Size = new System.Drawing.Size(274, 327);
            this.m_areaActivityList.TabIndex = 0;
            // 
            // m_querySampleTabs
            // 
            this.m_querySampleTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.m_querySampleTabs.Controls.Add(this.m_cardholder);
            this.m_querySampleTabs.Controls.Add(this.m_doorActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_areaActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_cardholderActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_credentialActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_zoneActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_alarmActivityTab);
            this.m_querySampleTabs.Controls.Add(this.m_visitorTab);
            this.m_querySampleTabs.Controls.Add(this.m_timeAttendanceTab);
            this.m_querySampleTabs.Controls.Add(this.m_bookmarkTab);
            this.m_querySampleTabs.Controls.Add(this.m_sequenceTab);
            this.m_querySampleTabs.Controls.Add(this.m_cameraIntegrityTab);
            this.m_querySampleTabs.Controls.Add(this.m_blockingTab);
            this.m_querySampleTabs.Controls.Add(this.m_videoProtectionTab);
            this.m_querySampleTabs.Controls.Add(this.m_lprReadTab);
            this.m_querySampleTabs.Controls.Add(this.m_inventoryTab);
            this.m_querySampleTabs.Controls.Add(this.m_parkingSessionByIdTab);
            this.m_querySampleTabs.Controls.Add(this.m_lprHitTab);
            this.m_querySampleTabs.Controls.Add(this.m_patrollerPositionTab);
            this.m_querySampleTabs.Controls.Add(this.m_tabLpmDailyUsage);
            this.m_querySampleTabs.Location = new System.Drawing.Point(8, 28);
            this.m_querySampleTabs.Name = "m_querySampleTabs";
            this.m_querySampleTabs.SelectedIndex = 0;
            this.m_querySampleTabs.Size = new System.Drawing.Size(282, 353);
            this.m_querySampleTabs.TabIndex = 4;
            // 
            // m_cardholder
            // 
            this.m_cardholder.Controls.Add(this.m_name);
            this.m_cardholder.Controls.Add(this.m_query);
            this.m_cardholder.Controls.Add(this.m_description);
            this.m_cardholder.Controls.Add(this.m_familyName);
            this.m_cardholder.Controls.Add(this.m_descriptionInput);
            this.m_cardholder.Controls.Add(this.m_lastName);
            this.m_cardholder.Controls.Add(this.m_firstName);
            this.m_cardholder.Location = new System.Drawing.Point(4, 22);
            this.m_cardholder.Name = "m_cardholder";
            this.m_cardholder.Padding = new System.Windows.Forms.Padding(3);
            this.m_cardholder.Size = new System.Drawing.Size(274, 327);
            this.m_cardholder.TabIndex = 0;
            this.m_cardholder.Text = "Cardholder";
            this.m_cardholder.UseVisualStyleBackColor = true;
            // 
            // m_name
            // 
            this.m_name.Location = new System.Drawing.Point(13, 6);
            this.m_name.Name = "m_name";
            this.m_name.Size = new System.Drawing.Size(80, 20);
            this.m_name.TabIndex = 8;
            this.m_name.Text = "First Name";
            this.m_name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_query.Location = new System.Drawing.Point(80, 102);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(128, 40);
            this.m_query.TabIndex = 14;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.OnLaunchQueryClick);
            // 
            // m_description
            // 
            this.m_description.Location = new System.Drawing.Point(13, 70);
            this.m_description.Name = "m_description";
            this.m_description.Size = new System.Drawing.Size(80, 20);
            this.m_description.TabIndex = 13;
            this.m_description.Text = "Description";
            this.m_description.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_familyName
            // 
            this.m_familyName.Location = new System.Drawing.Point(13, 38);
            this.m_familyName.Name = "m_familyName";
            this.m_familyName.Size = new System.Drawing.Size(80, 20);
            this.m_familyName.TabIndex = 12;
            this.m_familyName.Text = "Last Name";
            this.m_familyName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_descriptionInput
            // 
            this.m_descriptionInput.Location = new System.Drawing.Point(101, 70);
            this.m_descriptionInput.Name = "m_descriptionInput";
            this.m_descriptionInput.Size = new System.Drawing.Size(160, 20);
            this.m_descriptionInput.TabIndex = 11;
            // 
            // m_lastName
            // 
            this.m_lastName.Location = new System.Drawing.Point(101, 38);
            this.m_lastName.Name = "m_lastName";
            this.m_lastName.Size = new System.Drawing.Size(160, 20);
            this.m_lastName.TabIndex = 10;
            // 
            // m_firstName
            // 
            this.m_firstName.Location = new System.Drawing.Point(101, 6);
            this.m_firstName.Name = "m_firstName";
            this.m_firstName.Size = new System.Drawing.Size(160, 20);
            this.m_firstName.TabIndex = 9;
            // 
            // m_doorActivityTab
            // 
            this.m_doorActivityTab.Controls.Add(this.m_doorActivityList);
            this.m_doorActivityTab.Location = new System.Drawing.Point(4, 22);
            this.m_doorActivityTab.Name = "m_doorActivityTab";
            this.m_doorActivityTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_doorActivityTab.Size = new System.Drawing.Size(274, 327);
            this.m_doorActivityTab.TabIndex = 1;
            this.m_doorActivityTab.Text = "Door Activity";
            this.m_doorActivityTab.UseVisualStyleBackColor = true;
            // 
            // m_doorActivityList
            // 
            this.m_doorActivityList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_doorActivityList.Location = new System.Drawing.Point(3, 3);
            this.m_doorActivityList.Name = "m_doorActivityList";
            this.m_doorActivityList.Size = new System.Drawing.Size(268, 321);
            this.m_doorActivityList.TabIndex = 0;
            // 
            // m_cameraIntegrityTab
            // 
            this.m_cameraIntegrityTab.Controls.Add(this.m_cameraIntegrity);
            this.m_cameraIntegrityTab.Location = new System.Drawing.Point(4, 22);
            this.m_cameraIntegrityTab.Name = "m_cameraIntegrityTab";
            this.m_cameraIntegrityTab.Size = new System.Drawing.Size(274, 327);
            this.m_cameraIntegrityTab.TabIndex = 16;
            this.m_cameraIntegrityTab.Text = "CameraIntegrity";
            this.m_cameraIntegrityTab.UseVisualStyleBackColor = true;
            // 
            // m_cameraIntegrity
            // 
            this.m_cameraIntegrity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_cameraIntegrity.Location = new System.Drawing.Point(0, 0);
            this.m_cameraIntegrity.Name = "m_cameraIntegrity";
            this.m_cameraIntegrity.Size = new System.Drawing.Size(274, 327);
            this.m_cameraIntegrity.TabIndex = 0;
            // 
            // m_sequenceTab
            // 
            this.m_sequenceTab.Controls.Add(this.m_sequence);
            this.m_sequenceTab.Location = new System.Drawing.Point(4, 22);
            this.m_sequenceTab.Name = "m_sequenceTab";
            this.m_sequenceTab.Size = new System.Drawing.Size(274, 327);
            this.m_sequenceTab.TabIndex = 16;
            this.m_sequenceTab.Text = "Sequence";
            this.m_sequenceTab.UseVisualStyleBackColor = true;
            // 
            // m_sequence
            // 
            this.m_sequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_sequence.Location = new System.Drawing.Point(0, 0);
            this.m_sequence.Name = "m_sequence";
            this.m_sequence.Size = new System.Drawing.Size(274, 327);
            this.m_sequence.TabIndex = 0;
            // 
            // m_blockingTab
            // 
            this.m_blockingTab.Controls.Add(this.m_blockingQuery);
            this.m_blockingTab.Location = new System.Drawing.Point(4, 22);
            this.m_blockingTab.Name = "m_blockingTab";
            this.m_blockingTab.Size = new System.Drawing.Size(274, 327);
            this.m_blockingTab.TabIndex = 17;
            this.m_blockingTab.Text = "Blocking";
            this.m_blockingTab.UseVisualStyleBackColor = true;
            // 
            // m_blockingQuery
            // 
            this.m_blockingQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_blockingQuery.Location = new System.Drawing.Point(0, 0);
            this.m_blockingQuery.Name = "m_blockingQuery";
            this.m_blockingQuery.Size = new System.Drawing.Size(274, 327);
            this.m_blockingQuery.TabIndex = 0;
            // 
            // m_videoProtectionTab
            // 
            this.m_videoProtectionTab.Controls.Add(this.m_videoFileProtection);
            this.m_videoProtectionTab.Location = new System.Drawing.Point(4, 22);
            this.m_videoProtectionTab.Name = "m_videoProtectionTab";
            this.m_videoProtectionTab.Size = new System.Drawing.Size(274, 327);
            this.m_videoProtectionTab.TabIndex = 17;
            this.m_videoProtectionTab.Text = "VideoProtection";
            this.m_videoProtectionTab.UseVisualStyleBackColor = true;
            // 
            // m_videoFileProtection
            // 
            this.m_videoFileProtection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_videoFileProtection.Location = new System.Drawing.Point(0, 0);
            this.m_videoFileProtection.Name = "m_videoFileProtection";
            this.m_videoFileProtection.Size = new System.Drawing.Size(274, 327);
            this.m_videoFileProtection.TabIndex = 0;
            // 
            // m_parkingSessionByIdTab
            // 
            this.m_parkingSessionByIdTab.Controls.Add(this.m_parkingSessionById);
            this.m_parkingSessionByIdTab.Location = new System.Drawing.Point(4, 22);
            this.m_parkingSessionByIdTab.Name = "m_parkingSessionByIdTab";
            this.m_parkingSessionByIdTab.Size = new System.Drawing.Size(274, 327);
            this.m_parkingSessionByIdTab.TabIndex = 15;
            this.m_parkingSessionByIdTab.Text = "ParkingSessionById";
            this.m_parkingSessionByIdTab.UseVisualStyleBackColor = true;
            // 
            // m_parkingSessionById
            // 
            this.m_parkingSessionById.Location = new System.Drawing.Point(0, 0);
            this.m_parkingSessionById.Name = "m_parkingSessionById";
            this.m_parkingSessionById.Size = new System.Drawing.Size(265, 294);
            this.m_parkingSessionById.TabIndex = 0;
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 393);
            this.Controls.Add(this.m_tsMain);
            this.Controls.Add(this.m_querySampleTabs);
            this.Controls.Add(this.m_entityList);
            this.Name = "MainDlg";
            this.Text = "Security Center Query Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.m_tsMain.ResumeLayout(false);
            this.m_tsMain.PerformLayout();
            this.m_patrollerPositionTab.ResumeLayout(false);
            this.m_patrollerPositionTab.PerformLayout();
            this.m_lprHitTab.ResumeLayout(false);
            this.m_lprHitTab.PerformLayout();
            this.m_inventoryTab.ResumeLayout(false);
            this.m_inventoryTab.PerformLayout();
            this.m_lprReadTab.ResumeLayout(false);
            this.m_lprReadTab.PerformLayout();
            this.m_bookmarkTab.ResumeLayout(false);
            this.m_timeAttendanceTab.ResumeLayout(false);
            this.m_visitorTab.ResumeLayout(false);
            this.m_alarmActivityTab.ResumeLayout(false);
            this.m_zoneActivityTab.ResumeLayout(false);
            this.m_credentialActivityTab.ResumeLayout(false);
            this.m_cardholderActivityTab.ResumeLayout(false);
            this.m_areaActivityTab.ResumeLayout(false);
            this.m_querySampleTabs.ResumeLayout(false);
            this.m_cardholder.ResumeLayout(false);
            this.m_cardholder.PerformLayout();
            this.m_doorActivityTab.ResumeLayout(false);
            this.m_sequenceTab.ResumeLayout(false);
            this.m_cameraIntegrityTab.ResumeLayout(false);
            this.m_blockingTab.ResumeLayout(false);
            this.m_videoProtectionTab.ResumeLayout(false);
            this.m_parkingSessionByIdTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

