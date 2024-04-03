// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class PatrollerPositionQueryUI
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_browseSources;

        private System.Windows.Forms.DateTimePicker m_endDateAndTime;

        private System.Windows.Forms.Label m_from;

        private System.Windows.Forms.Button m_query;

        private System.Windows.Forms.TextBox m_source;

        private System.Windows.Forms.Label m_sourceName;

        private System.Windows.Forms.DateTimePicker m_startDateAndTime;

        private System.Windows.Forms.Label m_to;

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
            this.m_browseSources = new System.Windows.Forms.Button();
            this.m_source = new System.Windows.Forms.TextBox();
            this.m_sourceName = new System.Windows.Forms.Label();
            this.m_endDateAndTime = new System.Windows.Forms.DateTimePicker();
            this.m_to = new System.Windows.Forms.Label();
            this.m_startDateAndTime = new System.Windows.Forms.DateTimePicker();
            this.m_from = new System.Windows.Forms.Label();
            this.m_query = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_browseSources
            // 
            this.m_browseSources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseSources.Location = new System.Drawing.Point(232, 14);
            this.m_browseSources.Name = "m_browseSources";
            this.m_browseSources.Size = new System.Drawing.Size(30, 20);
            this.m_browseSources.TabIndex = 23;
            this.m_browseSources.Text = "...";
            this.m_browseSources.UseVisualStyleBackColor = true;
            this.m_browseSources.Click += new System.EventHandler(this.OnButtonBrowsePatrollerPositionSourceClick);
            // 
            // m_source
            // 
            this.m_source.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                         | System.Windows.Forms.AnchorStyles.Right)));
            this.m_source.Location = new System.Drawing.Point(65, 14);
            this.m_source.Name = "m_source";
            this.m_source.ReadOnly = true;
            this.m_source.Size = new System.Drawing.Size(167, 20);
            this.m_source.TabIndex = 22;
            // 
            // m_sourceName
            // 
            this.m_sourceName.AutoSize = true;
            this.m_sourceName.Location = new System.Drawing.Point(16, 14);
            this.m_sourceName.Name = "m_sourceName";
            this.m_sourceName.Size = new System.Drawing.Size(41, 13);
            this.m_sourceName.TabIndex = 21;
            this.m_sourceName.Text = "Source";
            // 
            // m_endDateAndTime
            // 
            this.m_endDateAndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                 | System.Windows.Forms.AnchorStyles.Right)));
            this.m_endDateAndTime.CustomFormat = "  MM-dd-yyyy    hh:mm:ss tt";
            this.m_endDateAndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_endDateAndTime.Location = new System.Drawing.Point(65, 94);
            this.m_endDateAndTime.Name = "m_endDateAndTime";
            this.m_endDateAndTime.Size = new System.Drawing.Size(167, 20);
            this.m_endDateAndTime.TabIndex = 19;
            // 
            // m_to
            // 
            this.m_to.AutoSize = true;
            this.m_to.Location = new System.Drawing.Point(16, 94);
            this.m_to.Name = "m_to";
            this.m_to.Size = new System.Drawing.Size(20, 13);
            this.m_to.TabIndex = 18;
            this.m_to.Text = "To";
            // 
            // m_startDateAndTime
            // 
            this.m_startDateAndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                   | System.Windows.Forms.AnchorStyles.Right)));
            this.m_startDateAndTime.CustomFormat = "  MM-dd-yyyy    hh:mm:ss tt";
            this.m_startDateAndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_startDateAndTime.Location = new System.Drawing.Point(65, 68);
            this.m_startDateAndTime.Name = "m_startDateAndTime";
            this.m_startDateAndTime.Size = new System.Drawing.Size(167, 20);
            this.m_startDateAndTime.TabIndex = 17;
            // 
            // m_from
            // 
            this.m_from.AutoSize = true;
            this.m_from.Location = new System.Drawing.Point(16, 68);
            this.m_from.Name = "m_from";
            this.m_from.Size = new System.Drawing.Size(30, 13);
            this.m_from.TabIndex = 16;
            this.m_from.Text = "From";
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_query.Location = new System.Drawing.Point(192, 260);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(60, 20);
            this.m_query.TabIndex = 20;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // PatrollerPositionQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.m_browseSources);
            this.Controls.Add(this.m_source);
            this.Controls.Add(this.m_sourceName);
            this.Controls.Add(this.m_endDateAndTime);
            this.Controls.Add(this.m_to);
            this.Controls.Add(this.m_startDateAndTime);
            this.Controls.Add(this.m_from);
            this.Controls.Add(this.m_query);
            this.Name = "PatrollerPositionQueryUI";
            this.Size = new System.Drawing.Size(265, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

