﻿namespace QuerySample
{
    partial class CameraIntegrityQueryUI
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListView m_cameraList;

        private System.Windows.Forms.ColumnHeader m_cameras;

        private System.Windows.Forms.Button m_removeCamera;

        private System.Windows.Forms.Button m_addCamera;

        private System.Windows.Forms.Button m_browseQuery;

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
            this.m_cameraList = new System.Windows.Forms.ListView();
            this.m_cameras = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_removeCamera = new System.Windows.Forms.Button();
            this.m_addCamera = new System.Windows.Forms.Button();
            this.m_browseQuery = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_cameraList
            // 
            this.m_cameraList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cameraList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.m_cameras});
            this.m_cameraList.HideSelection = false;
            this.m_cameraList.Location = new System.Drawing.Point(-5, 0);
            this.m_cameraList.Name = "m_cameraList";
            this.m_cameraList.Size = new System.Drawing.Size(293, 70);
            this.m_cameraList.TabIndex = 1;
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
            this.m_removeCamera.Location = new System.Drawing.Point(48, 76);
            this.m_removeCamera.Name = "m_removeCamera";
            this.m_removeCamera.Size = new System.Drawing.Size(42, 34);
            this.m_removeCamera.TabIndex = 5;
            this.m_removeCamera.Text = "-";
            this.m_removeCamera.UseVisualStyleBackColor = true;
            this.m_removeCamera.Click += new System.EventHandler(this.m_removeCamera_Click);
            // 
            // m_addCamera
            // 
            this.m_addCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_addCamera.Location = new System.Drawing.Point(3, 76);
            this.m_addCamera.Name = "m_addCamera";
            this.m_addCamera.Size = new System.Drawing.Size(39, 34);
            this.m_addCamera.TabIndex = 4;
            this.m_addCamera.Text = "+";
            this.m_addCamera.UseVisualStyleBackColor = true;
            this.m_addCamera.Click += new System.EventHandler(this.m_addCamera_Click);
            // 
            // m_browseQuery
            // 
            this.m_browseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseQuery.Location = new System.Drawing.Point(211, 120);
            this.m_browseQuery.Name = "m_browseQuery";
            this.m_browseQuery.Size = new System.Drawing.Size(60, 20);
            this.m_browseQuery.TabIndex = 8;
            this.m_browseQuery.Text = "Query";
            this.m_browseQuery.UseVisualStyleBackColor = true;
            this.m_browseQuery.Click += new System.EventHandler(this.m_browseQuery_Click);
            // 
            // CameraIntegrityQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_browseQuery);
            this.Controls.Add(this.m_removeCamera);
            this.Controls.Add(this.m_addCamera);
            this.Controls.Add(this.m_cameraList);
            this.Name = "CameraIntegrityQueryUI";
            this.Size = new System.Drawing.Size(283, 153);
            this.ResumeLayout(false);

        }

        #endregion

    }

}
