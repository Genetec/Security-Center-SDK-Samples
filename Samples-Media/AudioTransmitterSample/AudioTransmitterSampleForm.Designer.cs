namespace AudioTransmitterSample
{
    partial class AudioTransmitterSampleForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartVideoSource = new System.Windows.Forms.Button();
            this.btnStopVideoSource = new System.Windows.Forms.Button();
            this.lblVideoOutput = new System.Windows.Forms.Label();
            this.lstFrames = new System.Windows.Forms.ListBox();
            this.m_grpConnectionInfo = new System.Windows.Forms.GroupBox();
            this.m_btnConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.m_txtDirectory = new System.Windows.Forms.TextBox();
            this.m_txtUserName = new System.Windows.Forms.TextBox();
            this.m_txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_grpCamInfo = new System.Windows.Forms.GroupBox();
            this.cameraGuidComboBox = new System.Windows.Forms.ComboBox();
            this.m_grpConnectionInfo.SuspendLayout();
            this.m_grpCamInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartVideoSource
            // 
            this.btnStartVideoSource.Enabled = false;
            this.btnStartVideoSource.Location = new System.Drawing.Point(6, 55);
            this.btnStartVideoSource.Name = "btnStartVideoSource";
            this.btnStartVideoSource.Size = new System.Drawing.Size(127, 23);
            this.btnStartVideoSource.TabIndex = 0;
            this.btnStartVideoSource.Text = "Start transmitting audio";
            this.btnStartVideoSource.UseVisualStyleBackColor = true;
            this.btnStartVideoSource.Click += new System.EventHandler(this.OnButtonStartVideoSourceClick);
            // 
            // btnStopVideoSource
            // 
            this.btnStopVideoSource.Enabled = false;
            this.btnStopVideoSource.Location = new System.Drawing.Point(139, 55);
            this.btnStopVideoSource.Name = "btnStopVideoSource";
            this.btnStopVideoSource.Size = new System.Drawing.Size(122, 23);
            this.btnStopVideoSource.TabIndex = 1;
            this.btnStopVideoSource.Text = "Stop transmitting audio";
            this.btnStopVideoSource.UseVisualStyleBackColor = true;
            this.btnStopVideoSource.Click += new System.EventHandler(this.OnButtonStopVideoSourceClick);
            // 
            // lblVideoOutput
            // 
            this.lblVideoOutput.AutoSize = true;
            this.lblVideoOutput.Location = new System.Drawing.Point(6, 85);
            this.lblVideoOutput.Name = "lblVideoOutput";
            this.lblVideoOutput.Size = new System.Drawing.Size(101, 13);
            this.lblVideoOutput.TabIndex = 2;
            this.lblVideoOutput.Text = "Frames information :";
            // 
            // lstFrames
            // 
            this.lstFrames.FormattingEnabled = true;
            this.lstFrames.Location = new System.Drawing.Point(9, 102);
            this.lstFrames.Name = "lstFrames";
            this.lstFrames.Size = new System.Drawing.Size(568, 381);
            this.lstFrames.TabIndex = 3;
            // 
            // m_grpConnectionInfo
            // 
            this.m_grpConnectionInfo.Controls.Add(this.m_btnConnect);
            this.m_grpConnectionInfo.Controls.Add(this.label4);
            this.m_grpConnectionInfo.Controls.Add(this.m_txtDirectory);
            this.m_grpConnectionInfo.Controls.Add(this.m_txtUserName);
            this.m_grpConnectionInfo.Controls.Add(this.m_txtPassword);
            this.m_grpConnectionInfo.Controls.Add(this.label2);
            this.m_grpConnectionInfo.Controls.Add(this.label3);
            this.m_grpConnectionInfo.Location = new System.Drawing.Point(12, 12);
            this.m_grpConnectionInfo.Name = "m_grpConnectionInfo";
            this.m_grpConnectionInfo.Size = new System.Drawing.Size(584, 131);
            this.m_grpConnectionInfo.TabIndex = 20;
            this.m_grpConnectionInfo.TabStop = false;
            this.m_grpConnectionInfo.Text = "Connection Information";
            // 
            // m_btnConnect
            // 
            this.m_btnConnect.Location = new System.Drawing.Point(499, 101);
            this.m_btnConnect.Name = "m_btnConnect";
            this.m_btnConnect.Size = new System.Drawing.Size(75, 23);
            this.m_btnConnect.TabIndex = 19;
            this.m_btnConnect.Text = "Connect";
            this.m_btnConnect.UseVisualStyleBackColor = true;
            this.m_btnConnect.Click += new System.EventHandler(this.OnButtonConnectClick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Directory:";
            // 
            // m_txtDirectory
            // 
            this.m_txtDirectory.Location = new System.Drawing.Point(98, 23);
            this.m_txtDirectory.Name = "m_txtDirectory";
            this.m_txtDirectory.Size = new System.Drawing.Size(479, 20);
            this.m_txtDirectory.TabIndex = 18;
            this.m_txtDirectory.Text = "127.0.0.1";
            // 
            // m_txtUserName
            // 
            this.m_txtUserName.Location = new System.Drawing.Point(98, 49);
            this.m_txtUserName.Name = "m_txtUserName";
            this.m_txtUserName.Size = new System.Drawing.Size(479, 20);
            this.m_txtUserName.TabIndex = 11;
            this.m_txtUserName.Text = "admin";
            // 
            // m_txtPassword
            // 
            this.m_txtPassword.Location = new System.Drawing.Point(98, 75);
            this.m_txtPassword.Name = "m_txtPassword";
            this.m_txtPassword.PasswordChar = '*';
            this.m_txtPassword.Size = new System.Drawing.Size(479, 20);
            this.m_txtPassword.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "User name:";
            // 
            // m_grpCamInfo
            // 
            this.m_grpCamInfo.Controls.Add(this.cameraGuidComboBox);
            this.m_grpCamInfo.Controls.Add(this.btnStartVideoSource);
            this.m_grpCamInfo.Controls.Add(this.btnStopVideoSource);
            this.m_grpCamInfo.Controls.Add(this.lblVideoOutput);
            this.m_grpCamInfo.Controls.Add(this.lstFrames);
            this.m_grpCamInfo.Enabled = false;
            this.m_grpCamInfo.Location = new System.Drawing.Point(12, 149);
            this.m_grpCamInfo.Name = "m_grpCamInfo";
            this.m_grpCamInfo.Size = new System.Drawing.Size(586, 495);
            this.m_grpCamInfo.TabIndex = 21;
            this.m_grpCamInfo.TabStop = false;
            this.m_grpCamInfo.Text = "Camera Information";
            // 
            // cameraGuidComboBox
            // 
            this.cameraGuidComboBox.DisplayMember = "Content";
            this.cameraGuidComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraGuidComboBox.FormattingEnabled = true;
            this.cameraGuidComboBox.Location = new System.Drawing.Point(6, 24);
            this.cameraGuidComboBox.Name = "cameraGuidComboBox";
            this.cameraGuidComboBox.Size = new System.Drawing.Size(345, 21);
            this.cameraGuidComboBox.TabIndex = 10;
            // 
            // AudioTransmitterSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 655);
            this.Controls.Add(this.m_grpCamInfo);
            this.Controls.Add(this.m_grpConnectionInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AudioTransmitterSampleForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AudioTransmitter Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.AudioTransmitterSampleForm_Load);
            this.m_grpConnectionInfo.ResumeLayout(false);
            this.m_grpConnectionInfo.PerformLayout();
            this.m_grpCamInfo.ResumeLayout(false);
            this.m_grpCamInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartVideoSource;
        private System.Windows.Forms.Button btnStopVideoSource;
        private System.Windows.Forms.Label lblVideoOutput;
        private System.Windows.Forms.ListBox lstFrames;
        private System.Windows.Forms.GroupBox m_grpConnectionInfo;
        private System.Windows.Forms.Button m_btnConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_txtDirectory;
        private System.Windows.Forms.TextBox m_txtUserName;
        private System.Windows.Forms.TextBox m_txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox m_grpCamInfo;
        private System.Windows.Forms.ComboBox cameraGuidComboBox;
    }
}