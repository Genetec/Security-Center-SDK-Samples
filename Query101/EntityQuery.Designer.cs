// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace EntityQuery
{
    #region Classes

    partial class EntityQuery
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_asyncQuery1;

        private System.Windows.Forms.TextBox m_console;

        private System.Windows.Forms.Button m_logOff;

        private System.Windows.Forms.Button m_logOn;

        private System.Windows.Forms.Button m_querySync;

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
            this.m_logOn = new System.Windows.Forms.Button();
            this.m_logOff = new System.Windows.Forms.Button();
            this.m_console = new System.Windows.Forms.TextBox();
            this.m_querySync = new System.Windows.Forms.Button();
            this.m_asyncQuery1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _buttonLogOn
            // 
            this.m_logOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_logOn.Location = new System.Drawing.Point(156, 214);
            this.m_logOn.Name = "m_logOn";
            this.m_logOn.Size = new System.Drawing.Size(75, 23);
            this.m_logOn.TabIndex = 1;
            this.m_logOn.Text = "LogOn";
            this.m_logOn.UseVisualStyleBackColor = true;
            this.m_logOn.Click += new System.EventHandler(this.OnButtonLogOnClick);
            // 
            // _buttonLogOff
            // 
            this.m_logOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_logOff.Enabled = false;
            this.m_logOff.Location = new System.Drawing.Point(237, 214);
            this.m_logOff.Name = "m_logOff";
            this.m_logOff.Size = new System.Drawing.Size(75, 23);
            this.m_logOff.TabIndex = 2;
            this.m_logOff.Text = "LogOff";
            this.m_logOff.UseVisualStyleBackColor = true;
            this.m_logOff.Click += new System.EventHandler(this.OnButtonLogOffClick);
            // 
            // _textboxConsole
            // 
            this.m_console.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                                          | System.Windows.Forms.AnchorStyles.Right)));
            this.m_console.Location = new System.Drawing.Point(0, 32);
            this.m_console.Multiline = true;
            this.m_console.Name = "m_console";
            this.m_console.ReadOnly = true;
            this.m_console.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_console.Size = new System.Drawing.Size(312, 176);
            this.m_console.TabIndex = 3;
            // 
            // _buttonQuerySync
            // 
            this.m_querySync.Enabled = false;
            this.m_querySync.Location = new System.Drawing.Point(0, 3);
            this.m_querySync.Name = "m_querySync";
            this.m_querySync.Size = new System.Drawing.Size(75, 23);
            this.m_querySync.TabIndex = 4;
            this.m_querySync.Text = "QuerySync";
            this.m_querySync.UseVisualStyleBackColor = true;
            this.m_querySync.Click += new System.EventHandler(this.OnButtonQuerySyncClick);
            // 
            // _buttonAsyncQuery1
            // 
            this.m_asyncQuery1.Enabled = false;
            this.m_asyncQuery1.Location = new System.Drawing.Point(81, 3);
            this.m_asyncQuery1.Name = "m_asyncQuery1";
            this.m_asyncQuery1.Size = new System.Drawing.Size(89, 23);
            this.m_asyncQuery1.TabIndex = 5;
            this.m_asyncQuery1.Text = "QueryAsync1";
            this.m_asyncQuery1.UseVisualStyleBackColor = true;
            this.m_asyncQuery1.Click += new System.EventHandler(this.OnButtonAsyncQueryClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(318, 246);
            this.Controls.Add(this.m_asyncQuery1);
            this.Controls.Add(this.m_querySync);
            this.Controls.Add(this.m_console);
            this.Controls.Add(this.m_logOff);
            this.Controls.Add(this.m_logOn);
            this.Name = "EntityQuery";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

