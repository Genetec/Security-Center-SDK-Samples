// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    partial class LogonDlg
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_connect;

        private System.Windows.Forms.GroupBox m_connectionInfo;

        private System.Windows.Forms.Label m_directory;

        private System.Windows.Forms.TextBox m_directoryInput;

        private System.Windows.Forms.Label m_password;

        private System.Windows.Forms.TextBox m_passwordInput;

        private System.Windows.Forms.Label m_state;

        private System.Windows.Forms.Label m_username;

        private System.Windows.Forms.TextBox m_usernameInput;

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
            this.m_connectionInfo = new System.Windows.Forms.GroupBox();
            this.m_connect = new System.Windows.Forms.Button();
            this.m_password = new System.Windows.Forms.Label();
            this.m_username = new System.Windows.Forms.Label();
            this.m_passwordInput = new System.Windows.Forms.TextBox();
            this.m_usernameInput = new System.Windows.Forms.TextBox();
            this.m_directoryInput = new System.Windows.Forms.TextBox();
            this.m_directory = new System.Windows.Forms.Label();
            this.m_state = new System.Windows.Forms.Label();
            this.m_connectionInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_connectionInfo
            // 
            this.m_connectionInfo.Controls.Add(this.m_connect);
            this.m_connectionInfo.Controls.Add(this.m_password);
            this.m_connectionInfo.Controls.Add(this.m_username);
            this.m_connectionInfo.Controls.Add(this.m_passwordInput);
            this.m_connectionInfo.Controls.Add(this.m_usernameInput);
            this.m_connectionInfo.Controls.Add(this.m_directoryInput);
            this.m_connectionInfo.Controls.Add(this.m_directory);
            this.m_connectionInfo.Location = new System.Drawing.Point(8, 8);
            this.m_connectionInfo.Name = "m_connectionInfo";
            this.m_connectionInfo.Size = new System.Drawing.Size(272, 176);
            this.m_connectionInfo.TabIndex = 1;
            this.m_connectionInfo.TabStop = false;
            this.m_connectionInfo.Text = "Connection info";
            // 
            // m_connect
            // 
            this.m_connect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                          | System.Windows.Forms.AnchorStyles.Right)));
            this.m_connect.Location = new System.Drawing.Point(16, 120);
            this.m_connect.Name = "m_connect";
            this.m_connect.Size = new System.Drawing.Size(240, 40);
            this.m_connect.TabIndex = 6;
            this.m_connect.Text = "&Connect";
            this.m_connect.UseVisualStyleBackColor = true;
            this.m_connect.Click += new System.EventHandler(this.OnButtonConnectClick);
            // 
            // m_password
            // 
            this.m_password.Location = new System.Drawing.Point(8, 88);
            this.m_password.Name = "m_password";
            this.m_password.Size = new System.Drawing.Size(80, 20);
            this.m_password.TabIndex = 5;
            this.m_password.Text = "Password";
            this.m_password.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_username
            // 
            this.m_username.Location = new System.Drawing.Point(8, 56);
            this.m_username.Name = "m_username";
            this.m_username.Size = new System.Drawing.Size(80, 20);
            this.m_username.TabIndex = 4;
            this.m_username.Text = "Username";
            this.m_username.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_passwordInput
            // 
            this.m_passwordInput.Location = new System.Drawing.Point(96, 88);
            this.m_passwordInput.Name = "m_passwordInput";
            this.m_passwordInput.PasswordChar = '*';
            this.m_passwordInput.Size = new System.Drawing.Size(160, 20);
            this.m_passwordInput.TabIndex = 3;
            // 
            // m_usernameInput
            // 
            this.m_usernameInput.Location = new System.Drawing.Point(96, 56);
            this.m_usernameInput.Name = "m_usernameInput";
            this.m_usernameInput.Size = new System.Drawing.Size(160, 20);
            this.m_usernameInput.TabIndex = 2;
            this.m_usernameInput.Text = "Admin";
            // 
            // m_directoryInput
            // 
            this.m_directoryInput.Location = new System.Drawing.Point(96, 24);
            this.m_directoryInput.Name = "m_directoryInput";
            this.m_directoryInput.Size = new System.Drawing.Size(160, 20);
            this.m_directoryInput.TabIndex = 1;
            // 
            // m_directory
            // 
            this.m_directory.Location = new System.Drawing.Point(8, 24);
            this.m_directory.Name = "m_directory";
            this.m_directory.Size = new System.Drawing.Size(80, 20);
            this.m_directory.TabIndex = 0;
            this.m_directory.Text = "Directory";
            this.m_directory.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_state
            // 
            this.m_state.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                                                                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_state.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_state.Location = new System.Drawing.Point(8, 192);
            this.m_state.Name = "m_state";
            this.m_state.Size = new System.Drawing.Size(272, 24);
            this.m_state.TabIndex = 2;
            this.m_state.Text = "n/a";
            this.m_state.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LogonDlg
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(289, 229);
            this.Controls.Add(this.m_state);
            this.Controls.Add(this.m_connectionInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LogonDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log on to Security Center";
            this.m_connectionInfo.ResumeLayout(false);
            this.m_connectionInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion
}

