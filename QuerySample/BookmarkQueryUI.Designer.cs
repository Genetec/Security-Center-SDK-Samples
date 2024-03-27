// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;

namespace QuerySample
{
    #region Classes

    partial class BookmarkQueryUI
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_addCamera;

        private System.Windows.Forms.Button m_browseQuery;

        private System.Windows.Forms.ListView m_cameraList;

        private System.Windows.Forms.ColumnHeader m_cameras;

        private System.Windows.Forms.DateTimePicker m_endDateAndTime;

        private System.Windows.Forms.Label m_from;

        private System.Windows.Forms.Button m_removeCamera;

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
            this.m_browseQuery = new System.Windows.Forms.Button();
            this.m_from = new System.Windows.Forms.Label();
            this.m_startDateAndTime = new System.Windows.Forms.DateTimePicker();
            this.m_endDateAndTime = new System.Windows.Forms.DateTimePicker();
            this.m_to = new System.Windows.Forms.Label();
            this.m_cameraList = new System.Windows.Forms.ListView();
            this.m_cameras = new System.Windows.Forms.ColumnHeader();
            this.m_removeCamera = new System.Windows.Forms.Button();
            this.m_addCamera = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_browseQuery
            // 
            this.m_browseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseQuery.Location = new System.Drawing.Point(196, 262);
            this.m_browseQuery.Name = "m_btnQuery";
            this.m_browseQuery.Size = new System.Drawing.Size(60, 20);
            this.m_browseQuery.TabIndex = 7;
            this.m_browseQuery.Text = "Query";
            this.m_browseQuery.UseVisualStyleBackColor = true;
            this.m_browseQuery.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_from
            // 
            this.m_from.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_from.AutoSize = true;
            this.m_from.Location = new System.Drawing.Point(4, 196);
            this.m_from.Name = "m_from";
            this.m_from.Size = new System.Drawing.Size(30, 13);
            this.m_from.TabIndex = 3;
            this.m_from.Text = "From";
            // 
            // m_startDateAndTime
            // 
            this.m_startDateAndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                                                                   | System.Windows.Forms.AnchorStyles.Right)));
            this.m_startDateAndTime.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_startDateAndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_startDateAndTime.Location = new System.Drawing.Point(53, 196);
            this.m_startDateAndTime.Name = "m_startDateAndTime";
            this.m_startDateAndTime.Size = new System.Drawing.Size(203, 20);
            this.m_startDateAndTime.TabIndex = 4;
            //this.m_startDateAndTime.Value = DateTime.UtcNow - TimeSpan.FromDays(1);
            // 
            // m_endDateAndTime
            // 
            this.m_endDateAndTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                                                                 | System.Windows.Forms.AnchorStyles.Right)));
            this.m_endDateAndTime.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_endDateAndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_endDateAndTime.Location = new System.Drawing.Point(53, 222);
            this.m_endDateAndTime.Name = "m_endDateAndTime";
            this.m_endDateAndTime.Size = new System.Drawing.Size(203, 20);
            this.m_endDateAndTime.TabIndex = 6;
            //this.m_endDateAndTime.Value = DateTime.UtcNow;
            // 
            // m_to
            // 
            this.m_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_to.AutoSize = true;
            this.m_to.Location = new System.Drawing.Point(4, 222);
            this.m_to.Name = "m_to";
            this.m_to.Size = new System.Drawing.Size(20, 13);
            this.m_to.TabIndex = 5;
            this.m_to.Text = "To";
            // 
            // m_cameraList
            // 
            this.m_cameraList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                              | System.Windows.Forms.AnchorStyles.Left)
                                                                             | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cameraList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.m_cameras
            });
            this.m_cameraList.Location = new System.Drawing.Point(0, 3);
            this.m_cameraList.Name = "m_cameraList";
            this.m_cameraList.Size = new System.Drawing.Size(262, 158);
            this.m_cameraList.TabIndex = 0;
            this.m_cameraList.UseCompatibleStateImageBehavior = false;
            this.m_cameraList.View = System.Windows.Forms.View.Details;
            // 
            // m_cameras
            // 
            this.m_cameras.Text = "m_cameras";
            this.m_cameras.Width = 256;
            // 
            // m_removeCamera
            // 
            this.m_removeCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_removeCamera.Location = new System.Drawing.Point(39, 167);
            this.m_removeCamera.Name = "m_removeCamera";
            this.m_removeCamera.Size = new System.Drawing.Size(26, 23);
            this.m_removeCamera.TabIndex = 2;
            this.m_removeCamera.Text = "-";
            this.m_removeCamera.UseVisualStyleBackColor = true;
            this.m_removeCamera.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // m_addCamera
            // 
            this.m_addCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_addCamera.Location = new System.Drawing.Point(7, 167);
            this.m_addCamera.Name = "m_addCamera";
            this.m_addCamera.Size = new System.Drawing.Size(26, 23);
            this.m_addCamera.TabIndex = 1;
            this.m_addCamera.Text = "+";
            this.m_addCamera.UseVisualStyleBackColor = true;
            this.m_addCamera.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // BookmarkQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_removeCamera);
            this.Controls.Add(this.m_addCamera);
            this.Controls.Add(this.m_cameraList);
            this.Controls.Add(this.m_endDateAndTime);
            this.Controls.Add(this.m_to);
            this.Controls.Add(this.m_startDateAndTime);
            this.Controls.Add(this.m_from);
            this.Controls.Add(this.m_browseQuery);
            this.Name = "BookmarkQueryUI";
            this.Size = new System.Drawing.Size(265, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

