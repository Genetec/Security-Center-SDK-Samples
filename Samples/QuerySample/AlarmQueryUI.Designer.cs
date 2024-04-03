// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class AlarmQueryUI
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label m_alarm;

        private System.Windows.Forms.TextBox m_alarmName;

        private System.Windows.Forms.ComboBox m_alarmState;

        private System.Windows.Forms.Button m_browseAlarms;

        private System.Windows.Forms.Label m_maximumResults;

        private System.Windows.Forms.TextBox m_maximumResultsInput;

        private System.Windows.Forms.Button m_query;

        private System.Windows.Forms.Label m_state;

        #endregion

        #region Destructors and Dispose Methods

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
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
            this.m_alarm = new System.Windows.Forms.Label();
            this.m_alarmName = new System.Windows.Forms.TextBox();
            this.m_browseAlarms = new System.Windows.Forms.Button();
            this.m_state = new System.Windows.Forms.Label();
            this.m_alarmState = new System.Windows.Forms.ComboBox();
            this.m_query = new System.Windows.Forms.Button();
            this.m_maximumResults = new System.Windows.Forms.Label();
            this.m_maximumResultsInput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_alarm
            // 
            this.m_alarm.AutoSize = true;
            this.m_alarm.Location = new System.Drawing.Point(51, 26);
            this.m_alarm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_alarm.Name = "m_alarm";
            this.m_alarm.Size = new System.Drawing.Size(39, 15);
            this.m_alarm.TabIndex = 0;
            this.m_alarm.Text = "Alarm";
            // 
            // m_alarmName
            // 
            this.m_alarmName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_alarmName.Location = new System.Drawing.Point(103, 22);
            this.m_alarmName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_alarmName.Name = "m_alarmName";
            this.m_alarmName.ReadOnly = true;
            this.m_alarmName.Size = new System.Drawing.Size(189, 20);
            this.m_alarmName.TabIndex = 1;
            // 
            // m_browseAlarms
            // 
            this.m_browseAlarms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseAlarms.Location = new System.Drawing.Point(301, 22);
            this.m_browseAlarms.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_browseAlarms.Name = "m_browseAlarms";
            this.m_browseAlarms.Size = new System.Drawing.Size(40, 25);
            this.m_browseAlarms.TabIndex = 2;
            this.m_browseAlarms.Text = "...";
            this.m_browseAlarms.UseVisualStyleBackColor = true;
            this.m_browseAlarms.Click += new System.EventHandler(this.OnButtonBrowseClick);
            // 
            // m_state
            // 
            this.m_state.AutoSize = true;
            this.m_state.Location = new System.Drawing.Point(52, 58);
            this.m_state.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_state.Name = "m_state";
            this.m_state.Size = new System.Drawing.Size(35, 15);
            this.m_state.TabIndex = 3;
            this.m_state.Text = "State";
            // 
            // m_alarmState
            // 
            this.m_alarmState.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_alarmState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_alarmState.FormattingEnabled = true;
            this.m_alarmState.Location = new System.Drawing.Point(103, 54);
            this.m_alarmState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_alarmState.Name = "m_alarmState";
            this.m_alarmState.Size = new System.Drawing.Size(189, 24);
            this.m_alarmState.TabIndex = 4;
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_query.Location = new System.Drawing.Point(261, 322);
            this.m_query.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(80, 25);
            this.m_query.TabIndex = 5;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_maximumResults
            // 
            this.m_maximumResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_maximumResults.AutoSize = true;
            this.m_maximumResults.Location = new System.Drawing.Point(4, 91);
            this.m_maximumResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.m_maximumResults.Name = "m_maximumResults";
            this.m_maximumResults.Size = new System.Drawing.Size(78, 15);
            this.m_maximumResults.TabIndex = 6;
            this.m_maximumResults.Text = "Max. Results";
            // 
            // m_maximumResultsInput
            // 
            this.m_maximumResultsInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_maximumResultsInput.Location = new System.Drawing.Point(103, 87);
            this.m_maximumResultsInput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.m_maximumResultsInput.MaxLength = 4;
            this.m_maximumResultsInput.Name = "m_maximumResultsInput";
            this.m_maximumResultsInput.Size = new System.Drawing.Size(132, 20);
            this.m_maximumResultsInput.TabIndex = 7;
            this.m_maximumResultsInput.Text = "255";
            // 
            // AlarmQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_maximumResultsInput);
            this.Controls.Add(this.m_maximumResults);
            this.Controls.Add(this.m_query);
            this.Controls.Add(this.m_alarmState);
            this.Controls.Add(this.m_state);
            this.Controls.Add(this.m_browseAlarms);
            this.Controls.Add(this.m_alarmName);
            this.Controls.Add(this.m_alarm);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AlarmQueryUI";
            this.Size = new System.Drawing.Size(353, 362);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

