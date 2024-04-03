// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples
{
    #region Classes

    partial class MainDlg
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_acknowledgeAlarm;

        private System.Windows.Forms.FlowLayoutPanel m_alarmButtons;

        private System.Windows.Forms.ListView m_alarmList;

        private System.Windows.Forms.Button m_alarmNotAcknowledged;

        private System.Windows.Forms.TabPage m_alarms;

        private System.Windows.Forms.ToolStripButton m_connect;

        private System.Windows.Forms.ToolStrip m_connectOrDisconnect;

        private System.Windows.Forms.ToolStripButton m_disconnect;

        private System.Windows.Forms.ListView m_eventList;

        private System.Windows.Forms.TabPage m_events;

        private System.Windows.Forms.TabControl m_eventViewerTabs;

        #endregion

        #region Initialize Component

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ColumnHeader m_instanceColumn;
            System.Windows.Forms.ColumnHeader m_alarmNameColumn;
            System.Windows.Forms.ColumnHeader m_alarmEventColumn;
            System.Windows.Forms.ColumnHeader m_triggerTimeColumn;
            System.Windows.Forms.ColumnHeader m_timeStampColumn;
            System.Windows.Forms.ColumnHeader m_eventColumn;
            System.Windows.Forms.ColumnHeader m_descriptionColumn;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.m_eventList = new System.Windows.Forms.ListView();
            this.m_eventViewerTabs = new System.Windows.Forms.TabControl();
            this.m_events = new System.Windows.Forms.TabPage();
            this.m_alarms = new System.Windows.Forms.TabPage();
            this.m_alarmList = new System.Windows.Forms.ListView();
            this.m_alarmButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.m_acknowledgeAlarm = new System.Windows.Forms.Button();
            this.m_alarmNotAcknowledged = new System.Windows.Forms.Button();
            this.m_connectOrDisconnect = new System.Windows.Forms.ToolStrip();
            this.m_connect = new System.Windows.Forms.ToolStripButton();
            this.m_disconnect = new System.Windows.Forms.ToolStripButton();
            m_instanceColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_alarmNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_alarmEventColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_triggerTimeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_timeStampColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_eventColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_eventViewerTabs.SuspendLayout();
            this.m_events.SuspendLayout();
            this.m_alarms.SuspendLayout();
            this.m_alarmButtons.SuspendLayout();
            this.m_connectOrDisconnect.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_instanceColumn
            // 
            m_instanceColumn.Text = "Instance";
            m_instanceColumn.Width = 64;
            // 
            // m_alarmNameColumn
            // 
            m_alarmNameColumn.Text = "Alarm";
            m_alarmNameColumn.Width = 90;
            // 
            // m_alarmEventColumn
            // 
            m_alarmEventColumn.Text = "Event";
            m_alarmEventColumn.Width = 104;
            // 
            // m_triggerTimeColumn
            // 
            m_triggerTimeColumn.Text = "Trigger Time";
            m_triggerTimeColumn.Width = 109;
            // 
            // m_timeStampColumn
            // 
            m_timeStampColumn.Text = "Timestamp";
            m_timeStampColumn.Width = 150;
            // 
            // m_eventColumn
            // 
            m_eventColumn.Text = "Event";
            m_eventColumn.Width = 250;
            // 
            // m_descriptionColumn
            // 
            m_descriptionColumn.Text = "Description";
            m_descriptionColumn.Width = 263;
            // 
            // m_eventList
            // 
            this.m_eventList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            m_timeStampColumn,
            m_eventColumn,
            m_descriptionColumn});
            this.m_eventList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_eventList.FullRowSelect = true;
            this.m_eventList.GridLines = true;
            this.m_eventList.Location = new System.Drawing.Point(12, 12);
            this.m_eventList.Name = "m_eventList";
            this.m_eventList.Size = new System.Drawing.Size(696, 270);
            this.m_eventList.TabIndex = 1;
            this.m_eventList.UseCompatibleStateImageBehavior = false;
            this.m_eventList.View = System.Windows.Forms.View.Details;
            // 
            // m_eventViewerTabs
            // 
            this.m_eventViewerTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_eventViewerTabs.Controls.Add(this.m_events);
            this.m_eventViewerTabs.Controls.Add(this.m_alarms);
            this.m_eventViewerTabs.Location = new System.Drawing.Point(8, 32);
            this.m_eventViewerTabs.Name = "m_eventViewerTabs";
            this.m_eventViewerTabs.SelectedIndex = 0;
            this.m_eventViewerTabs.Size = new System.Drawing.Size(728, 320);
            this.m_eventViewerTabs.TabIndex = 2;
            // 
            // m_events
            // 
            this.m_events.Controls.Add(this.m_eventList);
            this.m_events.Location = new System.Drawing.Point(4, 22);
            this.m_events.Name = "m_events";
            this.m_events.Padding = new System.Windows.Forms.Padding(12);
            this.m_events.Size = new System.Drawing.Size(720, 294);
            this.m_events.TabIndex = 0;
            this.m_events.Text = "Events";
            this.m_events.UseVisualStyleBackColor = true;
            // 
            // m_alarms
            // 
            this.m_alarms.Controls.Add(this.m_alarmList);
            this.m_alarms.Controls.Add(this.m_alarmButtons);
            this.m_alarms.Location = new System.Drawing.Point(4, 22);
            this.m_alarms.Name = "m_alarms";
            this.m_alarms.Padding = new System.Windows.Forms.Padding(10);
            this.m_alarms.Size = new System.Drawing.Size(720, 294);
            this.m_alarms.TabIndex = 1;
            this.m_alarms.Text = "Alarms";
            this.m_alarms.UseVisualStyleBackColor = true;
            // 
            // m_alarmList
            // 
            this.m_alarmList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            m_instanceColumn,
            m_alarmNameColumn,
            m_alarmEventColumn,
            m_triggerTimeColumn});
            this.m_alarmList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_alarmList.FullRowSelect = true;
            this.m_alarmList.GridLines = true;
            this.m_alarmList.Location = new System.Drawing.Point(10, 10);
            this.m_alarmList.Name = "m_alarmList";
            this.m_alarmList.Size = new System.Drawing.Size(649, 274);
            this.m_alarmList.TabIndex = 2;
            this.m_alarmList.UseCompatibleStateImageBehavior = false;
            this.m_alarmList.View = System.Windows.Forms.View.Details;
            this.m_alarmList.SelectedIndexChanged += new System.EventHandler(this.OnListAlarmsSelectedIndexChanged);
            // 
            // m_alarmButtons
            // 
            this.m_alarmButtons.AutoSize = true;
            this.m_alarmButtons.Controls.Add(this.m_acknowledgeAlarm);
            this.m_alarmButtons.Controls.Add(this.m_alarmNotAcknowledged);
            this.m_alarmButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.m_alarmButtons.Location = new System.Drawing.Point(659, 10);
            this.m_alarmButtons.Name = "m_alarmButtons";
            this.m_alarmButtons.Size = new System.Drawing.Size(51, 274);
            this.m_alarmButtons.TabIndex = 3;
            // 
            // m_acknowledgeAlarm
            // 
            this.m_acknowledgeAlarm.Enabled = false;
            this.m_acknowledgeAlarm.Location = new System.Drawing.Point(3, 3);
            this.m_acknowledgeAlarm.Name = "m_acknowledgeAlarm";
            this.m_acknowledgeAlarm.Size = new System.Drawing.Size(45, 37);
            this.m_acknowledgeAlarm.TabIndex = 0;
            this.m_acknowledgeAlarm.Text = "Ack";
            this.m_acknowledgeAlarm.UseVisualStyleBackColor = true;
            this.m_acknowledgeAlarm.Click += new System.EventHandler(this.OnButtonAckClick);
            // 
            // m_alarmNotAcknowledged
            // 
            this.m_alarmNotAcknowledged.Enabled = false;
            this.m_alarmNotAcknowledged.Location = new System.Drawing.Point(3, 46);
            this.m_alarmNotAcknowledged.Name = "m_alarmNotAcknowledged";
            this.m_alarmNotAcknowledged.Size = new System.Drawing.Size(45, 37);
            this.m_alarmNotAcknowledged.TabIndex = 1;
            this.m_alarmNotAcknowledged.Text = "Nack";
            this.m_alarmNotAcknowledged.UseVisualStyleBackColor = true;
            this.m_alarmNotAcknowledged.Click += new System.EventHandler(this.OnButtonNackClick);
            // 
            // m_connectOrDisconnect
            // 
            this.m_connectOrDisconnect.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_connect,
            this.m_disconnect});
            this.m_connectOrDisconnect.Location = new System.Drawing.Point(0, 0);
            this.m_connectOrDisconnect.Name = "m_connectOrDisconnect";
            this.m_connectOrDisconnect.Size = new System.Drawing.Size(747, 25);
            this.m_connectOrDisconnect.TabIndex = 5;
            this.m_connectOrDisconnect.Text = "toolStrip1";
            // 
            // m_connect
            // 
            this.m_connect.Image = ((System.Drawing.Image)(resources.GetObject("m_connect.Image")));
            this.m_connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_connect.Name = "m_connect";
            this.m_connect.Size = new System.Drawing.Size(72, 22);
            this.m_connect.Text = "Connect";
            this.m_connect.Click += new System.EventHandler(this.OnButtonConnect_Click);
            // 
            // m_disconnect
            // 
            this.m_disconnect.Enabled = false;
            this.m_disconnect.Image = ((System.Drawing.Image)(resources.GetObject("m_disconnect.Image")));
            this.m_disconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_disconnect.Name = "m_disconnect";
            this.m_disconnect.Size = new System.Drawing.Size(86, 22);
            this.m_disconnect.Text = "Disconnect";
            this.m_disconnect.Click += new System.EventHandler(this.OnButtonDisconnectClick);
            // 
            // MainDlg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(747, 366);
            this.Controls.Add(this.m_connectOrDisconnect);
            this.Controls.Add(this.m_eventViewerTabs);
            this.Name = "MainDlg";
            this.Text = "Event Viewer Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.m_eventViewerTabs.ResumeLayout(false);
            this.m_events.ResumeLayout(false);
            this.m_alarms.ResumeLayout(false);
            this.m_alarms.PerformLayout();
            this.m_alarmButtons.ResumeLayout(false);
            this.m_connectOrDisconnect.ResumeLayout(false);
            this.m_connectOrDisconnect.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

