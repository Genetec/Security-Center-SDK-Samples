// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    partial class CustomFieldValueEditorDlg
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label m_fullName;

        private System.Windows.Forms.Panel m_name;

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
            this.m_name = new System.Windows.Forms.Panel();
            this.m_fullName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_name
            // 
            this.m_name.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                        | System.Windows.Forms.AnchorStyles.Left)
                                                                       | System.Windows.Forms.AnchorStyles.Right)));
            this.m_name.Location = new System.Drawing.Point(8, 36);
            this.m_name.MinimumSize = new System.Drawing.Size(50, 24);
            this.m_name.Name = "m_name";
            this.m_name.Size = new System.Drawing.Size(278, 24);
            this.m_name.TabIndex = 2;
            // 
            // m_fullName
            // 
            this.m_fullName.Location = new System.Drawing.Point(8, 8);
            this.m_fullName.Name = "m_fullName";
            this.m_fullName.Size = new System.Drawing.Size(280, 24);
            this.m_fullName.TabIndex = 3;
            this.m_fullName.Text = "Name";
            this.m_fullName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CustomFieldValueEditorDlg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(295, 73);
            this.Controls.Add(this.m_fullName);
            this.Controls.Add(this.m_name);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(250, 70);
            this.Name = "CustomFieldValueEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit custom field value";
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion
}

