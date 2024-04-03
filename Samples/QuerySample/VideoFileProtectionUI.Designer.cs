
using System;
using System.EnterpriseServices;
using System.Linq;
using Genetec.Sdk;

namespace QuerySample
{
    partial class VideoFileProtectionUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.protectBtn = new System.Windows.Forms.Button();
            this.m_cameraList = new System.Windows.Forms.ListView();
            this.m_cameras = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_end = new System.Windows.Forms.DateTimePicker();
            this.m_to = new System.Windows.Forms.Label();
            this.m_start = new System.Windows.Forms.DateTimePicker();
            this.m_from = new System.Windows.Forms.Label();
            this.m_browseQuery = new System.Windows.Forms.Button();
            this.m_removeCamera = new System.Windows.Forms.Button();
            this.m_addCamera = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_startProtect = new System.Windows.Forms.DateTimePicker();
            this.m_stopProtect = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.Unprotect = new System.Windows.Forms.Button();
            this.m_infiniteProtection = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // protectBtn
            // 
            this.protectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.protectBtn.Location = new System.Drawing.Point(197, 266);
            this.protectBtn.Name = "protectBtn";
            this.protectBtn.Size = new System.Drawing.Size(60, 20);
            this.protectBtn.TabIndex = 36;
            this.protectBtn.Text = "Protect";
            this.protectBtn.UseVisualStyleBackColor = true;
            this.protectBtn.Click += new System.EventHandler(this.protectBtn_Click);
            // 
            // m_cameraList
            // 
            this.m_cameraList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cameraList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_cameras});
            this.m_cameraList.HideSelection = false;
            this.m_cameraList.Location = new System.Drawing.Point(1, 9);
            this.m_cameraList.Name = "m_cameraList";
            this.m_cameraList.Size = new System.Drawing.Size(262, 63);
            this.m_cameraList.TabIndex = 24;
            this.m_cameraList.UseCompatibleStateImageBehavior = false;
            this.m_cameraList.View = System.Windows.Forms.View.Details;
            // 
            // m_cameras
            // 
            this.m_cameras.Text = "m_cameras";
            this.m_cameras.Width = 256;
            // 
            // m_end
            // 
            this.m_end.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_end.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_end.Location = new System.Drawing.Point(54, 133);
            this.m_end.Name = "m_end";
            this.m_end.Size = new System.Drawing.Size(203, 20);
            this.m_end.TabIndex = 30;
            // 
            // m_to
            // 
            this.m_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_to.AutoSize = true;
            this.m_to.Location = new System.Drawing.Point(5, 133);
            this.m_to.Name = "m_to";
            this.m_to.Size = new System.Drawing.Size(20, 13);
            this.m_to.TabIndex = 29;
            this.m_to.Text = "To";
            // 
            // m_start
            // 
            this.m_start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_start.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_start.Location = new System.Drawing.Point(54, 107);
            this.m_start.Name = "m_start";
            this.m_start.Size = new System.Drawing.Size(203, 20);
            this.m_start.TabIndex = 28;
            // 
            // m_from
            // 
            this.m_from.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_from.AutoSize = true;
            this.m_from.Location = new System.Drawing.Point(5, 107);
            this.m_from.Name = "m_from";
            this.m_from.Size = new System.Drawing.Size(30, 13);
            this.m_from.TabIndex = 27;
            this.m_from.Text = "From";
            // 
            // m_browseQuery
            // 
            this.m_browseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseQuery.Location = new System.Drawing.Point(197, 159);
            this.m_browseQuery.Name = "m_browseQuery";
            this.m_browseQuery.Size = new System.Drawing.Size(60, 20);
            this.m_browseQuery.TabIndex = 31;
            this.m_browseQuery.Text = "Query files";
            this.m_browseQuery.UseVisualStyleBackColor = true;
            this.m_browseQuery.Click += new System.EventHandler(this.m_browseQuery_Click);
            // 
            // m_removeCamera
            // 
            this.m_removeCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_removeCamera.Location = new System.Drawing.Point(40, 78);
            this.m_removeCamera.Name = "m_removeCamera";
            this.m_removeCamera.Size = new System.Drawing.Size(26, 23);
            this.m_removeCamera.TabIndex = 26;
            this.m_removeCamera.Text = "-";
            this.m_removeCamera.UseVisualStyleBackColor = true;
            this.m_removeCamera.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // m_addCamera
            // 
            this.m_addCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_addCamera.Location = new System.Drawing.Point(8, 78);
            this.m_addCamera.Name = "m_addCamera";
            this.m_addCamera.Size = new System.Drawing.Size(26, 23);
            this.m_addCamera.TabIndex = 25;
            this.m_addCamera.Text = "+";
            this.m_addCamera.UseVisualStyleBackColor = true;
            this.m_addCamera.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 209);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 32;
            this.label2.Text = "Protect from";
            // 
            // m_startProtect
            // 
            this.m_startProtect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_startProtect.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_startProtect.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_startProtect.Location = new System.Drawing.Point(75, 209);
            this.m_startProtect.Name = "m_startProtect";
            this.m_startProtect.Size = new System.Drawing.Size(182, 20);
            this.m_startProtect.TabIndex = 33;
            // 
            // m_stopProtect
            // 
            this.m_stopProtect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_stopProtect.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_stopProtect.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_stopProtect.Location = new System.Drawing.Point(75, 235);
            this.m_stopProtect.Name = "m_stopProtect";
            this.m_stopProtect.Size = new System.Drawing.Size(182, 20);
            this.m_stopProtect.TabIndex = 35;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "to";
            // 
            // Unprotect
            // 
            this.Unprotect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Unprotect.Location = new System.Drawing.Point(8, 266);
            this.Unprotect.Name = "Unprotect";
            this.Unprotect.Size = new System.Drawing.Size(69, 20);
            this.Unprotect.TabIndex = 37;
            this.Unprotect.Text = "Unprotect";
            this.Unprotect.UseVisualStyleBackColor = true;
            this.Unprotect.Click += new System.EventHandler(this.Unprotect_Click);
            // 
            // m_infiniteProtection
            // 
            this.m_infiniteProtection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_infiniteProtection.AutoSize = true;
            this.m_infiniteProtection.Location = new System.Drawing.Point(84, 266);
            this.m_infiniteProtection.Name = "m_infiniteProtection";
            this.m_infiniteProtection.Size = new System.Drawing.Size(107, 17);
            this.m_infiniteProtection.TabIndex = 38;
            this.m_infiniteProtection.Text = "Infinite protection";
            this.m_infiniteProtection.UseVisualStyleBackColor = true;
            // 
            // VideoFileProtectionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_infiniteProtection);
            this.Controls.Add(this.Unprotect);
            this.Controls.Add(this.m_stopProtect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_startProtect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.protectBtn);
            this.Controls.Add(this.m_cameraList);
            this.Controls.Add(this.m_end);
            this.Controls.Add(this.m_to);
            this.Controls.Add(this.m_start);
            this.Controls.Add(this.m_from);
            this.Controls.Add(this.m_browseQuery);
            this.Controls.Add(this.m_removeCamera);
            this.Controls.Add(this.m_addCamera);
            this.Name = "VideoFileProtectionUI";
            this.Size = new System.Drawing.Size(265, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button protectBtn;
        private System.Windows.Forms.ListView m_cameraList;
        private System.Windows.Forms.ColumnHeader m_cameras;
        private System.Windows.Forms.DateTimePicker m_end;
        private System.Windows.Forms.Label m_to;
        private System.Windows.Forms.DateTimePicker m_start;
        private System.Windows.Forms.Label m_from;
        private System.Windows.Forms.Button m_browseQuery;
        private System.Windows.Forms.Button m_removeCamera;
        private System.Windows.Forms.Button m_addCamera;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker m_startProtect;
        private System.Windows.Forms.DateTimePicker m_stopProtect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Unprotect;
        private System.Windows.Forms.CheckBox m_infiniteProtection;
    }
}
