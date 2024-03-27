// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Schedules
{
    #region Classes

    partial class Schedules
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_21stOfJuly;

        private System.Windows.Forms.TextBox m_accessRuleName;

        private System.Windows.Forms.GroupBox m_accessRuleSchedule;

        private System.Windows.Forms.Label m_acessRule;

        private System.Windows.Forms.Label m_alarm;

        private System.Windows.Forms.TextBox m_alarmName;

        private System.Windows.Forms.GroupBox m_alarmSchedule;

        private System.Windows.Forms.Button m_attachAccessRuleSchedule;

        private System.Windows.Forms.Button m_attachAlarmSchedule;

        private System.Windows.Forms.Label m_attachScheduleToAccessRule;

        private System.Windows.Forms.Label m_attachScheduleToAlarm;

        private System.Windows.Forms.GroupBox m_createSchedule;

        private System.Windows.Forms.Button m_dailyRange;

        private System.Windows.Forms.Button m_dailyTwilight;

        private System.Windows.Forms.Button m_deleteSchedule;

        private System.Windows.Forms.Button m_lastDayOfMonth;

        private System.Windows.Forms.Button m_login;

        private System.Windows.Forms.GroupBox m_modificationOrDeletion;

        private System.Windows.Forms.Button m_modifyOrdinalSchedule;

        private System.Windows.Forms.Label m_password;

        private System.Windows.Forms.TextBox m_passwordInput;

        private System.Windows.Forms.Label m_schdeuleModificationOrDeletion;

        private System.Windows.Forms.Label m_schedule;

        private System.Windows.Forms.TextBox m_scheduleName;

        private System.Windows.Forms.Label m_schedulesCreation;

        private System.Windows.Forms.Label m_scheduleStatus;

        private System.Windows.Forms.Button m_secondNonWeekdayOfMonth;

        private System.Windows.Forms.TextBox m_server;

        private System.Windows.Forms.Label m_serverName;

        private System.Windows.Forms.Button m_specificRange;

        private System.Windows.Forms.Button m_specificTwilight;

        private System.Windows.Forms.Label m_status;

        private System.Windows.Forms.Label m_username;

        private System.Windows.Forms.TextBox m_usernameInput;

        private System.Windows.Forms.Button m_weeklyRange;

        #endregion

        #region Destructors and Dispose Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Initialize Component

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.m_login = new System.Windows.Forms.Button();
            this.m_usernameInput = new System.Windows.Forms.TextBox();
            this.m_username = new System.Windows.Forms.Label();
            this.m_password = new System.Windows.Forms.Label();
            this.m_serverName = new System.Windows.Forms.Label();
            this.m_server = new System.Windows.Forms.TextBox();
            this.m_scheduleStatus = new System.Windows.Forms.Label();
            this.m_status = new System.Windows.Forms.Label();
            this.m_dailyRange = new System.Windows.Forms.Button();
            this.m_schedulesCreation = new System.Windows.Forms.Label();
            this.m_dailyTwilight = new System.Windows.Forms.Button();
            this.m_specificTwilight = new System.Windows.Forms.Button();
            this.m_weeklyRange = new System.Windows.Forms.Button();
            this.m_specificRange = new System.Windows.Forms.Button();
            this.m_lastDayOfMonth = new System.Windows.Forms.Button();
            this.m_secondNonWeekdayOfMonth = new System.Windows.Forms.Button();
            this.m_21stOfJuly = new System.Windows.Forms.Button();
            this.m_schdeuleModificationOrDeletion = new System.Windows.Forms.Label();
            this.m_modifyOrdinalSchedule = new System.Windows.Forms.Button();
            this.m_schedule = new System.Windows.Forms.Label();
            this.m_scheduleName = new System.Windows.Forms.TextBox();
            this.m_deleteSchedule = new System.Windows.Forms.Button();
            this.m_attachAlarmSchedule = new System.Windows.Forms.Button();
            this.m_attachScheduleToAlarm = new System.Windows.Forms.Label();
            this.m_alarmName = new System.Windows.Forms.TextBox();
            this.m_alarm = new System.Windows.Forms.Label();
            this.m_acessRule = new System.Windows.Forms.Label();
            this.m_accessRuleName = new System.Windows.Forms.TextBox();
            this.m_attachAccessRuleSchedule = new System.Windows.Forms.Button();
            this.m_attachScheduleToAccessRule = new System.Windows.Forms.Label();
            this.m_createSchedule = new System.Windows.Forms.GroupBox();
            this.m_modificationOrDeletion = new System.Windows.Forms.GroupBox();
            this.m_alarmSchedule = new System.Windows.Forms.GroupBox();
            this.m_accessRuleSchedule = new System.Windows.Forms.GroupBox();
            this.m_passwordInput = new System.Windows.Forms.TextBox();
            this.m_createSchedule.SuspendLayout();
            this.m_modificationOrDeletion.SuspendLayout();
            this.m_alarmSchedule.SuspendLayout();
            this.m_accessRuleSchedule.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_login
            // 
            this.m_login.Location = new System.Drawing.Point(608, 12);
            this.m_login.Name = "m_login";
            this.m_login.Size = new System.Drawing.Size(75, 23);
            this.m_login.TabIndex = 0;
            this.m_login.Text = "Login";
            this.m_login.UseVisualStyleBackColor = true;
            this.m_login.Click += new System.EventHandler(this.OnButtonLoginClick);
            // 
            // m_usernameInput
            // 
            this.m_usernameInput.Location = new System.Drawing.Point(266, 14);
            this.m_usernameInput.Name = "m_usernameInput";
            this.m_usernameInput.Size = new System.Drawing.Size(100, 20);
            this.m_usernameInput.TabIndex = 1;
            this.m_usernameInput.Text = "admin";
            // 
            // m_username
            // 
            this.m_username.AutoSize = true;
            this.m_username.Location = new System.Drawing.Point(191, 17);
            this.m_username.Name = "m_username";
            this.m_username.Size = new System.Drawing.Size(55, 13);
            this.m_username.TabIndex = 3;
            this.m_username.Text = "Username";
            // 
            // m_password
            // 
            this.m_password.AutoSize = true;
            this.m_password.Location = new System.Drawing.Point(400, 17);
            this.m_password.Name = "m_password";
            this.m_password.Size = new System.Drawing.Size(53, 13);
            this.m_password.TabIndex = 4;
            this.m_password.Text = "Password";
            // 
            // m_serverName
            // 
            this.m_serverName.AutoSize = true;
            this.m_serverName.Location = new System.Drawing.Point(19, 17);
            this.m_serverName.Name = "m_serverName";
            this.m_serverName.Size = new System.Drawing.Size(38, 13);
            this.m_serverName.TabIndex = 6;
            this.m_serverName.Text = "Server";
            // 
            // m_server
            // 
            this.m_server.Location = new System.Drawing.Point(67, 14);
            this.m_server.Name = "m_server";
            this.m_server.Size = new System.Drawing.Size(100, 20);
            this.m_server.TabIndex = 5;
            // 
            // m_scheduleStatus
            // 
            this.m_scheduleStatus.AutoSize = true;
            this.m_scheduleStatus.Location = new System.Drawing.Point(709, 18);
            this.m_scheduleStatus.Name = "m_scheduleStatus";
            this.m_scheduleStatus.Size = new System.Drawing.Size(37, 13);
            this.m_scheduleStatus.TabIndex = 8;
            this.m_scheduleStatus.Text = "Status";
            // 
            // m_status
            // 
            this.m_status.AutoSize = true;
            this.m_status.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_status.Location = new System.Drawing.Point(772, 18);
            this.m_status.Name = "m_status";
            this.m_status.Size = new System.Drawing.Size(61, 13);
            this.m_status.TabIndex = 9;
            this.m_status.Text = "Loged off";
            // 
            // m_dailyRange
            // 
            this.m_dailyRange.Enabled = false;
            this.m_dailyRange.Location = new System.Drawing.Point(45, 53);
            this.m_dailyRange.Name = "m_dailyRange";
            this.m_dailyRange.Size = new System.Drawing.Size(100, 23);
            this.m_dailyRange.TabIndex = 10;
            this.m_dailyRange.Text = "Daily Range";
            this.m_dailyRange.UseVisualStyleBackColor = true;
            this.m_dailyRange.Click += new System.EventHandler(this.OnButtonDailyRangeClick);
            // 
            // m_schedulesCreation
            // 
            this.m_schedulesCreation.AutoSize = true;
            this.m_schedulesCreation.Location = new System.Drawing.Point(357, 0);
            this.m_schedulesCreation.Name = "m_schedulesCreation";
            this.m_schedulesCreation.Size = new System.Drawing.Size(98, 13);
            this.m_schedulesCreation.TabIndex = 11;
            this.m_schedulesCreation.Text = "Schedules creation";
            // 
            // m_dailyTwilight
            // 
            this.m_dailyTwilight.Enabled = false;
            this.m_dailyTwilight.Location = new System.Drawing.Point(356, 53);
            this.m_dailyTwilight.Name = "m_dailyTwilight";
            this.m_dailyTwilight.Size = new System.Drawing.Size(100, 23);
            this.m_dailyTwilight.TabIndex = 12;
            this.m_dailyTwilight.Text = "Daily Twilight";
            this.m_dailyTwilight.UseVisualStyleBackColor = true;
            this.m_dailyTwilight.Click += new System.EventHandler(this.OnButtonDailyTwilightClick);
            // 
            // m_specificTwilight
            // 
            this.m_specificTwilight.Enabled = false;
            this.m_specificTwilight.Location = new System.Drawing.Point(356, 95);
            this.m_specificTwilight.Name = "m_specificTwilight";
            this.m_specificTwilight.Size = new System.Drawing.Size(100, 23);
            this.m_specificTwilight.TabIndex = 13;
            this.m_specificTwilight.Text = "Specific Twilight";
            this.m_specificTwilight.UseVisualStyleBackColor = true;
            this.m_specificTwilight.Click += new System.EventHandler(this.OnButtonSpecificTwlilightClick);
            // 
            // m_weeklyRange
            // 
            this.m_weeklyRange.Enabled = false;
            this.m_weeklyRange.Location = new System.Drawing.Point(45, 95);
            this.m_weeklyRange.Name = "m_weeklyRange";
            this.m_weeklyRange.Size = new System.Drawing.Size(100, 23);
            this.m_weeklyRange.TabIndex = 14;
            this.m_weeklyRange.Text = "Weekly Range";
            this.m_weeklyRange.UseVisualStyleBackColor = true;
            this.m_weeklyRange.Click += new System.EventHandler(this.OnButtonWeeklyRangeClick);
            // 
            // m_specificRange
            // 
            this.m_specificRange.Enabled = false;
            this.m_specificRange.Location = new System.Drawing.Point(45, 139);
            this.m_specificRange.Name = "m_specificRange";
            this.m_specificRange.Size = new System.Drawing.Size(100, 23);
            this.m_specificRange.TabIndex = 15;
            this.m_specificRange.Text = "Specific Range";
            this.m_specificRange.UseVisualStyleBackColor = true;
            this.m_specificRange.Click += new System.EventHandler(this.OnButtonSpecificRangeClick);
            // 
            // m_lastDayOfMonth
            // 
            this.m_lastDayOfMonth.Enabled = false;
            this.m_lastDayOfMonth.Location = new System.Drawing.Point(671, 53);
            this.m_lastDayOfMonth.Name = "m_lastDayOfMonth";
            this.m_lastDayOfMonth.Size = new System.Drawing.Size(117, 23);
            this.m_lastDayOfMonth.TabIndex = 16;
            this.m_lastDayOfMonth.Text = "Last Day Of Month";
            this.m_lastDayOfMonth.UseVisualStyleBackColor = true;
            this.m_lastDayOfMonth.Click += new System.EventHandler(this.OnButtonLastDayOfMonthClick);
            // 
            // m_secondNonWeekdayOfMonth
            // 
            this.m_secondNonWeekdayOfMonth.Enabled = false;
            this.m_secondNonWeekdayOfMonth.Location = new System.Drawing.Point(647, 95);
            this.m_secondNonWeekdayOfMonth.Name = "m_secondNonWeekdayOfMonth";
            this.m_secondNonWeekdayOfMonth.Size = new System.Drawing.Size(171, 23);
            this.m_secondNonWeekdayOfMonth.TabIndex = 17;
            this.m_secondNonWeekdayOfMonth.Text = "Second Non Weekday Of Month";
            this.m_secondNonWeekdayOfMonth.UseVisualStyleBackColor = true;
            this.m_secondNonWeekdayOfMonth.Click += new System.EventHandler(this.OnButtonSecondNonWeekdayOfMonthClick);
            // 
            // m_21stOfJuly
            // 
            this.m_21stOfJuly.Enabled = false;
            this.m_21stOfJuly.Location = new System.Drawing.Point(671, 139);
            this.m_21stOfJuly.Name = "m_21stOfJuly";
            this.m_21stOfJuly.Size = new System.Drawing.Size(117, 23);
            this.m_21stOfJuly.TabIndex = 18;
            this.m_21stOfJuly.Text = "21st Of July";
            this.m_21stOfJuly.UseVisualStyleBackColor = true;
            this.m_21stOfJuly.Click += new System.EventHandler(this.OnButton21OfJulyClick);
            // 
            // m_schdeuleModificationOrDeletion
            // 
            this.m_schdeuleModificationOrDeletion.AutoSize = true;
            this.m_schdeuleModificationOrDeletion.Location = new System.Drawing.Point(322, 0);
            this.m_schdeuleModificationOrDeletion.Name = "m_schdeuleModificationOrDeletion";
            this.m_schdeuleModificationOrDeletion.Size = new System.Drawing.Size(168, 13);
            this.m_schdeuleModificationOrDeletion.TabIndex = 19;
            this.m_schdeuleModificationOrDeletion.Text = "Schedules modification or deletion";
            // 
            // m_modifyOrdinalSchedule
            // 
            this.m_modifyOrdinalSchedule.Enabled = false;
            this.m_modifyOrdinalSchedule.Location = new System.Drawing.Point(452, 40);
            this.m_modifyOrdinalSchedule.Name = "m_modifyOrdinalSchedule";
            this.m_modifyOrdinalSchedule.Size = new System.Drawing.Size(136, 23);
            this.m_modifyOrdinalSchedule.TabIndex = 20;
            this.m_modifyOrdinalSchedule.Text = "Modify Ordinal Schedule";
            this.m_modifyOrdinalSchedule.UseVisualStyleBackColor = true;
            this.m_modifyOrdinalSchedule.Click += new System.EventHandler(this.OnButtonModifyClick);
            // 
            // m_schedule
            // 
            this.m_schedule.AutoSize = true;
            this.m_schedule.Location = new System.Drawing.Point(67, 45);
            this.m_schedule.Name = "m_schedule";
            this.m_schedule.Size = new System.Drawing.Size(83, 13);
            this.m_schedule.TabIndex = 22;
            this.m_schedule.Text = "Schedule Name";
            // 
            // m_scheduleName
            // 
            this.m_scheduleName.Enabled = false;
            this.m_scheduleName.Location = new System.Drawing.Point(172, 42);
            this.m_scheduleName.Name = "m_scheduleName";
            this.m_scheduleName.Size = new System.Drawing.Size(235, 20);
            this.m_scheduleName.TabIndex = 21;
            // 
            // m_deleteSchedule
            // 
            this.m_deleteSchedule.Enabled = false;
            this.m_deleteSchedule.Location = new System.Drawing.Point(630, 40);
            this.m_deleteSchedule.Name = "m_deleteSchedule";
            this.m_deleteSchedule.Size = new System.Drawing.Size(127, 23);
            this.m_deleteSchedule.TabIndex = 24;
            this.m_deleteSchedule.Text = "Delete Schedule";
            this.m_deleteSchedule.UseVisualStyleBackColor = true;
            this.m_deleteSchedule.Click += new System.EventHandler(this.OnButtonDeleteCoverageClick);
            // 
            // m_attachAlarmSchedule
            // 
            this.m_attachAlarmSchedule.Enabled = false;
            this.m_attachAlarmSchedule.Location = new System.Drawing.Point(603, 22);
            this.m_attachAlarmSchedule.Name = "m_attachAlarmSchedule";
            this.m_attachAlarmSchedule.Size = new System.Drawing.Size(127, 23);
            this.m_attachAlarmSchedule.TabIndex = 28;
            this.m_attachAlarmSchedule.Text = "Attach Schedule";
            this.m_attachAlarmSchedule.UseVisualStyleBackColor = true;
            this.m_attachAlarmSchedule.Click += new System.EventHandler(this.OnButtonAttachCoverageClick);
            // 
            // m_attachScheduleToAlarm
            // 
            this.m_attachScheduleToAlarm.AutoSize = true;
            this.m_attachScheduleToAlarm.Location = new System.Drawing.Point(336, 0);
            this.m_attachScheduleToAlarm.Name = "m_attachScheduleToAlarm";
            this.m_attachScheduleToAlarm.Size = new System.Drawing.Size(139, 13);
            this.m_attachScheduleToAlarm.TabIndex = 27;
            this.m_attachScheduleToAlarm.Text = "Attach schedule to an alarm";
            // 
            // m_alarmName
            // 
            this.m_alarmName.Enabled = false;
            this.m_alarmName.Location = new System.Drawing.Point(286, 24);
            this.m_alarmName.Name = "m_alarmName";
            this.m_alarmName.Size = new System.Drawing.Size(235, 20);
            this.m_alarmName.TabIndex = 29;
            // 
            // m_alarm
            // 
            this.m_alarm.AutoSize = true;
            this.m_alarm.Location = new System.Drawing.Point(140, 27);
            this.m_alarm.Name = "m_alarm";
            this.m_alarm.Size = new System.Drawing.Size(64, 13);
            this.m_alarm.TabIndex = 30;
            this.m_alarm.Text = "Alarm Name";
            // 
            // m_acessRule
            // 
            this.m_acessRule.AutoSize = true;
            this.m_acessRule.Location = new System.Drawing.Point(126, 27);
            this.m_acessRule.Name = "m_acessRule";
            this.m_acessRule.Size = new System.Drawing.Size(98, 13);
            this.m_acessRule.TabIndex = 34;
            this.m_acessRule.Text = "Access Rule Name";
            // 
            // m_accessRuleName
            // 
            this.m_accessRuleName.Enabled = false;
            this.m_accessRuleName.Location = new System.Drawing.Point(286, 24);
            this.m_accessRuleName.Name = "m_accessRuleName";
            this.m_accessRuleName.Size = new System.Drawing.Size(235, 20);
            this.m_accessRuleName.TabIndex = 33;
            // 
            // m_attachAccessRuleSchedule
            // 
            this.m_attachAccessRuleSchedule.Enabled = false;
            this.m_attachAccessRuleSchedule.Location = new System.Drawing.Point(603, 22);
            this.m_attachAccessRuleSchedule.Name = "m_attachAccessRuleSchedule";
            this.m_attachAccessRuleSchedule.Size = new System.Drawing.Size(127, 23);
            this.m_attachAccessRuleSchedule.TabIndex = 32;
            this.m_attachAccessRuleSchedule.Text = "Attach Schedule";
            this.m_attachAccessRuleSchedule.UseVisualStyleBackColor = true;
            this.m_attachAccessRuleSchedule.Click += new System.EventHandler(this.OnButtonAttachAccessRuleClick);
            // 
            // m_attachScheduleToAccessRule
            // 
            this.m_attachScheduleToAccessRule.AutoSize = true;
            this.m_attachScheduleToAccessRule.Location = new System.Drawing.Point(319, 0);
            this.m_attachScheduleToAccessRule.Name = "m_attachScheduleToAccessRule";
            this.m_attachScheduleToAccessRule.Size = new System.Drawing.Size(168, 13);
            this.m_attachScheduleToAccessRule.TabIndex = 31;
            this.m_attachScheduleToAccessRule.Text = "Attach schedule to an access rule";
            // 
            // m_createSchedule
            // 
            this.m_createSchedule.Controls.Add(this.m_21stOfJuly);
            this.m_createSchedule.Controls.Add(this.m_secondNonWeekdayOfMonth);
            this.m_createSchedule.Controls.Add(this.m_lastDayOfMonth);
            this.m_createSchedule.Controls.Add(this.m_specificRange);
            this.m_createSchedule.Controls.Add(this.m_weeklyRange);
            this.m_createSchedule.Controls.Add(this.m_specificTwilight);
            this.m_createSchedule.Controls.Add(this.m_dailyTwilight);
            this.m_createSchedule.Controls.Add(this.m_schedulesCreation);
            this.m_createSchedule.Controls.Add(this.m_dailyRange);
            this.m_createSchedule.Location = new System.Drawing.Point(22, 46);
            this.m_createSchedule.Name = "m_createSchedule";
            this.m_createSchedule.Size = new System.Drawing.Size(843, 175);
            this.m_createSchedule.TabIndex = 35;
            this.m_createSchedule.TabStop = false;
            // 
            // m_modificationOrDeletion
            // 
            this.m_modificationOrDeletion.Controls.Add(this.m_deleteSchedule);
            this.m_modificationOrDeletion.Controls.Add(this.m_schedule);
            this.m_modificationOrDeletion.Controls.Add(this.m_scheduleName);
            this.m_modificationOrDeletion.Controls.Add(this.m_modifyOrdinalSchedule);
            this.m_modificationOrDeletion.Controls.Add(this.m_schdeuleModificationOrDeletion);
            this.m_modificationOrDeletion.Location = new System.Drawing.Point(22, 225);
            this.m_modificationOrDeletion.Name = "m_modificationOrDeletion";
            this.m_modificationOrDeletion.Size = new System.Drawing.Size(843, 82);
            this.m_modificationOrDeletion.TabIndex = 36;
            this.m_modificationOrDeletion.TabStop = false;
            // 
            // m_alarmSchedule
            // 
            this.m_alarmSchedule.Controls.Add(this.m_attachScheduleToAlarm);
            this.m_alarmSchedule.Controls.Add(this.m_alarm);
            this.m_alarmSchedule.Controls.Add(this.m_alarmName);
            this.m_alarmSchedule.Controls.Add(this.m_attachAlarmSchedule);
            this.m_alarmSchedule.Location = new System.Drawing.Point(22, 313);
            this.m_alarmSchedule.Name = "m_alarmSchedule";
            this.m_alarmSchedule.Size = new System.Drawing.Size(843, 70);
            this.m_alarmSchedule.TabIndex = 37;
            this.m_alarmSchedule.TabStop = false;
            // 
            // m_accessRuleSchedule
            // 
            this.m_accessRuleSchedule.Controls.Add(this.m_attachScheduleToAccessRule);
            this.m_accessRuleSchedule.Controls.Add(this.m_acessRule);
            this.m_accessRuleSchedule.Controls.Add(this.m_accessRuleName);
            this.m_accessRuleSchedule.Controls.Add(this.m_attachAccessRuleSchedule);
            this.m_accessRuleSchedule.Location = new System.Drawing.Point(22, 398);
            this.m_accessRuleSchedule.Name = "m_accessRuleSchedule";
            this.m_accessRuleSchedule.Size = new System.Drawing.Size(843, 58);
            this.m_accessRuleSchedule.TabIndex = 38;
            this.m_accessRuleSchedule.TabStop = false;
            // 
            // m_passwordInput
            // 
            this.m_passwordInput.Location = new System.Drawing.Point(474, 14);
            this.m_passwordInput.Name = "m_passwordInput";
            this.m_passwordInput.PasswordChar = '*';
            this.m_passwordInput.Size = new System.Drawing.Size(100, 20);
            this.m_passwordInput.TabIndex = 2;
            // 
            // Schedules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(888, 471);
            this.Controls.Add(this.m_accessRuleSchedule);
            this.Controls.Add(this.m_alarmSchedule);
            this.Controls.Add(this.m_modificationOrDeletion);
            this.Controls.Add(this.m_createSchedule);
            this.Controls.Add(this.m_status);
            this.Controls.Add(this.m_scheduleStatus);
            this.Controls.Add(this.m_serverName);
            this.Controls.Add(this.m_server);
            this.Controls.Add(this.m_password);
            this.Controls.Add(this.m_username);
            this.Controls.Add(this.m_passwordInput);
            this.Controls.Add(this.m_usernameInput);
            this.Controls.Add(this.m_login);
            this.Name = "Schedules";
            this.Text = "Schedules";
            this.m_createSchedule.ResumeLayout(false);
            this.m_createSchedule.PerformLayout();
            this.m_modificationOrDeletion.ResumeLayout(false);
            this.m_modificationOrDeletion.PerformLayout();
            this.m_alarmSchedule.ResumeLayout(false);
            this.m_alarmSchedule.PerformLayout();
            this.m_accessRuleSchedule.ResumeLayout(false);
            this.m_accessRuleSchedule.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

