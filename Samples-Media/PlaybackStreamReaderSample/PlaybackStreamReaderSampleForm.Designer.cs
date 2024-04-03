using System;
using System.Windows.Forms;

namespace PlaybackStreamReaderSample
{
    partial class PlaybackStreamReaderSampleForm
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
            this.btnConnectReader = new System.Windows.Forms.Button();
            this.btnSeek = new System.Windows.Forms.Button();
            this.lblVideoOutput = new System.Windows.Forms.Label();
            this.lstFrames = new System.Windows.Forms.ListBox();
            this.btnStartRead = new System.Windows.Forms.Button();
            this.m_grpConnectionInfo = new System.Windows.Forms.GroupBox();
            this.m_btnConnect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.m_txtDirectory = new System.Windows.Forms.TextBox();
            this.m_txtUserName = new System.Windows.Forms.TextBox();
            this.m_txtPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_grpCamInfo = new System.Windows.Forms.GroupBox();
            this.comboBoxSource = new System.Windows.Forms.ComboBox();
            this.lblSequence = new System.Windows.Forms.Label();
            this.btnStopRead = new System.Windows.Forms.Button();
            this.cameraGuidComboBox = new System.Windows.Forms.ComboBox();
            this.comboBoxStream = new System.Windows.Forms.ComboBox();
            this.btnDisconnectReader = new System.Windows.Forms.Button();
            this.dtpSeekDate = new System.Windows.Forms.DateTimePicker();
            this.btnQuerySequences = new System.Windows.Forms.Button();
            this.btnQueryBySource = new System.Windows.Forms.Button();
            this.lstSequences = new System.Windows.Forms.ListBox();
            this.m_grpConnectionInfo.SuspendLayout();
            this.m_grpCamInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnectReader
            // 
            this.btnConnectReader.Enabled = false;
            this.btnConnectReader.Location = new System.Drawing.Point(6, 55);
            this.btnConnectReader.Name = "btnConnectReader";
            this.btnConnectReader.Size = new System.Drawing.Size(114, 23);
            this.btnConnectReader.TabIndex = 0;
            this.btnConnectReader.Text = "Connect reader";
            this.btnConnectReader.UseVisualStyleBackColor = true;
            this.btnConnectReader.Click += new System.EventHandler(this.OnButtonConnectReaderClick);
            // 
            // btnSeek
            // 
            this.btnSeek.Enabled = false;
            this.btnSeek.Location = new System.Drawing.Point(246, 55);
            this.btnSeek.Name = "btnSeek";
            this.btnSeek.Size = new System.Drawing.Size(114, 23);
            this.btnSeek.TabIndex = 1;
            this.btnSeek.Text = "Seek";
            this.btnSeek.UseVisualStyleBackColor = true;
            this.btnSeek.Click += new System.EventHandler(this.OnButtonSeekClick);
            // 
            // lblVideoOutput
            // 
            this.lblVideoOutput.AutoSize = true;
            this.lblVideoOutput.Location = new System.Drawing.Point(6, 112);
            this.lblVideoOutput.Name = "lblVideoOutput";
            this.lblVideoOutput.Size = new System.Drawing.Size(101, 13);
            this.lblVideoOutput.TabIndex = 2;
            this.lblVideoOutput.Text = "Frames information :";
            // 
            // lstFrames
            // 
            this.lstFrames.FormattingEnabled = true;
            this.lstFrames.Location = new System.Drawing.Point(9, 128);
            this.lstFrames.Name = "lstFrames";
            this.lstFrames.Size = new System.Drawing.Size(568, 186);
            this.lstFrames.TabIndex = 3;
            this.lstFrames.MouseDoubleClick += new MouseEventHandler(this.OnFrameItemDoubleClick);
            // 
            // btnStartRead
            // 
            this.btnStartRead.Enabled = false;
            this.btnStartRead.Location = new System.Drawing.Point(6, 86);
            this.btnStartRead.Name = "btnStartRead";
            this.btnStartRead.Size = new System.Drawing.Size(114, 23);
            this.btnStartRead.TabIndex = 6;
            this.btnStartRead.Text = "Start read";
            this.btnStartRead.UseVisualStyleBackColor = true;
            this.btnStartRead.Click += new System.EventHandler(this.OnButtonStartReadClick);
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
            this.m_grpCamInfo.Controls.Add(this.lblSequence);
            this.m_grpCamInfo.Controls.Add(this.btnStopRead);
            this.m_grpCamInfo.Controls.Add(this.cameraGuidComboBox);
            this.m_grpCamInfo.Controls.Add(this.comboBoxStream);
            this.m_grpCamInfo.Controls.Add(this.comboBoxSource);
            this.m_grpCamInfo.Controls.Add(this.btnConnectReader);
            this.m_grpCamInfo.Controls.Add(this.btnDisconnectReader);
            this.m_grpCamInfo.Controls.Add(this.btnStartRead);
            this.m_grpCamInfo.Controls.Add(this.btnSeek);
            this.m_grpCamInfo.Controls.Add(this.lblVideoOutput);
            this.m_grpCamInfo.Controls.Add(this.lstFrames);
            this.m_grpCamInfo.Controls.Add(this.dtpSeekDate);
            this.m_grpCamInfo.Controls.Add(this.btnQuerySequences);
            this.m_grpCamInfo.Controls.Add(this.btnQueryBySource);
            this.m_grpCamInfo.Controls.Add(this.lstSequences);
            this.m_grpCamInfo.Enabled = false;
            this.m_grpCamInfo.Location = new System.Drawing.Point(12, 149);
            this.m_grpCamInfo.Name = "m_grpCamInfo";
            this.m_grpCamInfo.Size = new System.Drawing.Size(586, 570);
            this.m_grpCamInfo.TabIndex = 21;
            this.m_grpCamInfo.TabStop = false;
            this.m_grpCamInfo.Text = "Camera Information";
            // 
            // comboBoxSource
            // 
            this.comboBoxSource.DisplayMember = "Content";
            this.comboBoxSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSource.FormattingEnabled = true;
            this.comboBoxSource.Location = new System.Drawing.Point(389, 23);
            this.comboBoxSource.Name = "comboBoxSource";
            this.comboBoxSource.Size = new System.Drawing.Size(185, 21);
            this.comboBoxSource.TabIndex = 18;
            // 
            // lblSequence
            // 
            this.lblSequence.AutoSize = true;
            this.lblSequence.Location = new System.Drawing.Point(6, 357);
            this.lblSequence.Name = "lblSequence";
            this.lblSequence.Size = new System.Drawing.Size(70, 13);
            this.lblSequence.TabIndex = 16;
            this.lblSequence.Text = "Sequences : ";
            // 
            // btnStopRead
            // 
            this.btnStopRead.Enabled = false;
            this.btnStopRead.Location = new System.Drawing.Point(126, 86);
            this.btnStopRead.Name = "btnStopRead";
            this.btnStopRead.Size = new System.Drawing.Size(114, 23);
            this.btnStopRead.TabIndex = 12;
            this.btnStopRead.Text = "Stop read";
            this.btnStopRead.UseVisualStyleBackColor = true;
            this.btnStopRead.Click += new System.EventHandler(this.OnButtonStopReadClick);
            // 
            // cameraGuidComboBox
            // 
            this.cameraGuidComboBox.DisplayMember = "Content";
            this.cameraGuidComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cameraGuidComboBox.FormattingEnabled = true;
            this.cameraGuidComboBox.Location = new System.Drawing.Point(6, 23);
            this.cameraGuidComboBox.Name = "cameraGuidComboBox";
            this.cameraGuidComboBox.Size = new System.Drawing.Size(234, 21);
            this.cameraGuidComboBox.TabIndex = 10;
            this.cameraGuidComboBox.SelectedIndexChanged += new System.EventHandler(this.OnComboBoxCameraSelectedIndexChanged);
            // 
            // comboBoxStream
            // 
            this.comboBoxStream.DisplayMember = "Content";
            this.comboBoxStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxStream.FormattingEnabled = true;
            this.comboBoxStream.Location = new System.Drawing.Point(246, 23);
            this.comboBoxStream.Name = "comboBoxStream";
            this.comboBoxStream.Size = new System.Drawing.Size(137, 21);
            this.comboBoxStream.TabIndex = 8;
            // 
            // btnDisconnectReader
            // 
            this.btnDisconnectReader.Enabled = false;
            this.btnDisconnectReader.Location = new System.Drawing.Point(126, 55);
            this.btnDisconnectReader.Name = "btnDisconnectReader";
            this.btnDisconnectReader.Size = new System.Drawing.Size(114, 23);
            this.btnDisconnectReader.TabIndex = 13;
            this.btnDisconnectReader.Text = "Disconnect reader";
            this.btnDisconnectReader.UseVisualStyleBackColor = true;
            this.btnDisconnectReader.Click += new System.EventHandler(this.btnDisconnectReader_Click);
            // 
            // dtpSeekDate
            // 
            this.dtpSeekDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpSeekDate.Enabled = false;
            this.dtpSeekDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSeekDate.Location = new System.Drawing.Point(366, 58);
            this.dtpSeekDate.Name = "dtpSeekDate";
            this.dtpSeekDate.Size = new System.Drawing.Size(137, 20);
            this.dtpSeekDate.TabIndex = 11;
            // 
            // btnQuerySequences
            // 
            this.btnQuerySequences.Location = new System.Drawing.Point(6, 331);
            this.btnQuerySequences.Name = "btnQuerySequences";
            this.btnQuerySequences.Size = new System.Drawing.Size(114, 23);
            this.btnQuerySequences.TabIndex = 14;
            this.btnQuerySequences.Text = "Query sequences";
            this.btnQuerySequences.UseVisualStyleBackColor = true;
            this.btnQuerySequences.Click += new System.EventHandler(this.btnQuerySequences_Click);
            // 
            // btnQueryBySource
            // 
            this.btnQueryBySource.Location = new System.Drawing.Point(126, 331);
            this.btnQueryBySource.Name = "btnQueryBySource";
            this.btnQueryBySource.Size = new System.Drawing.Size(114, 23);
            this.btnQueryBySource.TabIndex = 15;
            this.btnQueryBySource.Text = "Query by sources";
            this.btnQueryBySource.UseVisualStyleBackColor = true;
            this.btnQueryBySource.Click += new System.EventHandler(this.btnQueryBySource_Click);
            // 
            // lstSequences
            // 
            this.lstSequences.FormattingEnabled = true;
            this.lstSequences.Location = new System.Drawing.Point(9, 373);
            this.lstSequences.Name = "lstSequences";
            this.lstSequences.Size = new System.Drawing.Size(568, 186);
            this.lstSequences.TabIndex = 17;
            // 
            // PlaybackStreamReaderSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 731);
            this.Controls.Add(this.m_grpCamInfo);
            this.Controls.Add(this.m_grpConnectionInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PlaybackStreamReaderSampleForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "PlaybackStreamReader Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.m_grpConnectionInfo.ResumeLayout(false);
            this.m_grpConnectionInfo.PerformLayout();
            this.m_grpCamInfo.ResumeLayout(false);
            this.m_grpCamInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnConnectReader;
        private System.Windows.Forms.Button btnSeek;
        private System.Windows.Forms.Label lblVideoOutput;
        private System.Windows.Forms.ListBox lstFrames;
        private System.Windows.Forms.Button btnStartRead;
        private System.Windows.Forms.GroupBox m_grpConnectionInfo;
        private System.Windows.Forms.Button m_btnConnect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_txtDirectory;
        private System.Windows.Forms.TextBox m_txtUserName;
        private System.Windows.Forms.TextBox m_txtPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox m_grpCamInfo;
        private System.Windows.Forms.ComboBox comboBoxStream;
        private System.Windows.Forms.ComboBox cameraGuidComboBox;
        private System.Windows.Forms.Button btnStopRead;
        private System.Windows.Forms.DateTimePicker dtpSeekDate;
        private System.Windows.Forms.Button btnDisconnectReader;
        private System.Windows.Forms.Label lblSequence;
        private System.Windows.Forms.Button btnQueryBySource;
        private System.Windows.Forms.Button btnQuerySequences;
        private System.Windows.Forms.ListBox lstSequences;
        private System.Windows.Forms.ComboBox comboBoxSource;
    }
}