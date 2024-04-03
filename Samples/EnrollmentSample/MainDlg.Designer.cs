using System;

namespace Genetec.Sdk.Samples
{
    partial class MainDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ColumnHeader m_chTimestamp;
            System.Windows.Forms.ColumnHeader m_chCredential;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDlg));
            this.m_tsMain = new System.Windows.Forms.ToolStrip();
            this.m_btnConnect = new System.Windows.Forms.ToolStripButton();
            this.m_btnDisconnect = new System.Windows.Forms.ToolStripButton();
            this.m_lblFilter = new System.Windows.Forms.Label();
            this.m_btnFilter = new System.Windows.Forms.Button();
            this.m_lvCredentials = new System.Windows.Forms.ListView();
            this.m_objTabControl = new System.Windows.Forms.TabControl();
            this.m_objInformationPage = new System.Windows.Forms.TabPage();
            this.m_pnlVisitor = new System.Windows.Forms.Panel();
            this.m_lblDeparture = new System.Windows.Forms.Label();
            this.m_lblArrival = new System.Windows.Forms.Label();
            this.m_dtDeparture = new System.Windows.Forms.DateTimePicker();
            this.m_dtArrival = new System.Windows.Forms.DateTimePicker();
            this.m_rbCardholder = new System.Windows.Forms.RadioButton();
            this.m_rbVisitor = new System.Windows.Forms.RadioButton();
            this.m_lblInformationDescription = new System.Windows.Forms.Label();
            this.m_lblLastName = new System.Windows.Forms.Label();
            this.m_lblFirstName = new System.Windows.Forms.Label();
            this.m_tbLastName = new System.Windows.Forms.TextBox();
            this.m_tbFirstName = new System.Windows.Forms.TextBox();
            this.m_objCredentialPage = new System.Windows.Forms.TabPage();
            this.m_nupCardID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nupFacility = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cbFormat = new System.Windows.Forms.ComboBox();
            this.m_rbAutomatic = new System.Windows.Forms.RadioButton();
            this.m_rbManual = new System.Windows.Forms.RadioButton();
            this.m_lblCredentialDescription = new System.Windows.Forms.Label();
            this.m_objSummaryPage = new System.Windows.Forms.TabPage();
            this.m_lblResult = new System.Windows.Forms.Label();
            this.m_pnlBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.m_btnCreate = new System.Windows.Forms.Button();
            this.m_btnNext = new System.Windows.Forms.Button();
            this.m_btnPrevious = new System.Windows.Forms.Button();
            m_chTimestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            m_chCredential = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_tsMain.SuspendLayout();
            this.m_objTabControl.SuspendLayout();
            this.m_objInformationPage.SuspendLayout();
            this.m_pnlVisitor.SuspendLayout();
            this.m_objCredentialPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nupCardID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nupFacility)).BeginInit();
            this.m_objSummaryPage.SuspendLayout();
            this.m_pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_chTimestamp
            // 
            m_chTimestamp.Text = "Timestamp";
            m_chTimestamp.Width = 150;
            // 
            // m_chCredential
            // 
            m_chCredential.Text = "Credential";
            m_chCredential.Width = 350;
            // 
            // m_tsMain
            // 
            this.m_tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_btnConnect,
            this.m_btnDisconnect});
            this.m_tsMain.Location = new System.Drawing.Point(0, 0);
            this.m_tsMain.Name = "m_tsMain";
            this.m_tsMain.Size = new System.Drawing.Size(744, 25);
            this.m_tsMain.TabIndex = 5;
            this.m_tsMain.Text = "toolStrip1";
            // 
            // m_btnConnect
            // 
            this.m_btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("m_btnConnect.Image")));
            this.m_btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnConnect.Name = "m_btnConnect";
            this.m_btnConnect.Size = new System.Drawing.Size(72, 22);
            this.m_btnConnect.Text = "Connect";
            this.m_btnConnect.Click += new System.EventHandler(this.OnButtonConnect_Click);
            // 
            // m_btnDisconnect
            // 
            this.m_btnDisconnect.Enabled = false;
            this.m_btnDisconnect.Image = ((System.Drawing.Image)(resources.GetObject("m_btnDisconnect.Image")));
            this.m_btnDisconnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.m_btnDisconnect.Name = "m_btnDisconnect";
            this.m_btnDisconnect.Size = new System.Drawing.Size(86, 22);
            this.m_btnDisconnect.Text = "Disconnect";
            this.m_btnDisconnect.Click += new System.EventHandler(this.OnButtonDisconnect_Click);
            // 
            // m_lblFilter
            // 
            this.m_lblFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lblFilter.Location = new System.Drawing.Point(52, 60);
            this.m_lblFilter.Name = "m_lblFilter";
            this.m_lblFilter.Size = new System.Drawing.Size(660, 24);
            this.m_lblFilter.TabIndex = 3;
            this.m_lblFilter.Text = "n/a";
            this.m_lblFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_btnFilter
            // 
            this.m_btnFilter.Image = global::Genetec.Sdk.Samples.Properties.Resources.edit;
            this.m_btnFilter.Location = new System.Drawing.Point(12, 56);
            this.m_btnFilter.Name = "m_btnFilter";
            this.m_btnFilter.Size = new System.Drawing.Size(32, 32);
            this.m_btnFilter.TabIndex = 1;
            this.m_btnFilter.UseVisualStyleBackColor = true;
            this.m_btnFilter.Click += new System.EventHandler(this.OnButtonFilter_Click);
            // 
            // m_lvCredentials
            // 
            this.m_lvCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lvCredentials.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            m_chTimestamp,
            m_chCredential});
            this.m_lvCredentials.FullRowSelect = true;
            this.m_lvCredentials.Location = new System.Drawing.Point(12, 92);
            this.m_lvCredentials.Name = "m_lvCredentials";
            this.m_lvCredentials.Size = new System.Drawing.Size(716, 192);
            this.m_lvCredentials.TabIndex = 0;
            this.m_lvCredentials.UseCompatibleStateImageBehavior = false;
            this.m_lvCredentials.View = System.Windows.Forms.View.Details;
            this.m_lvCredentials.SelectedIndexChanged += new System.EventHandler(this.OnUserInterfaceChanged);
            // 
            // m_objTabControl
            // 
            this.m_objTabControl.Controls.Add(this.m_objInformationPage);
            this.m_objTabControl.Controls.Add(this.m_objCredentialPage);
            this.m_objTabControl.Controls.Add(this.m_objSummaryPage);
            this.m_objTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_objTabControl.Enabled = false;
            this.m_objTabControl.Location = new System.Drawing.Point(0, 25);
            this.m_objTabControl.Name = "m_objTabControl";
            this.m_objTabControl.SelectedIndex = 0;
            this.m_objTabControl.Size = new System.Drawing.Size(744, 431);
            this.m_objTabControl.TabIndex = 10;
            this.m_objTabControl.SelectedIndexChanged += new System.EventHandler(this.OnUserInterfaceChanged);
            // 
            // m_objInformationPage
            // 
            this.m_objInformationPage.Controls.Add(this.m_pnlVisitor);
            this.m_objInformationPage.Controls.Add(this.m_rbCardholder);
            this.m_objInformationPage.Controls.Add(this.m_rbVisitor);
            this.m_objInformationPage.Controls.Add(this.m_lblInformationDescription);
            this.m_objInformationPage.Controls.Add(this.m_lblLastName);
            this.m_objInformationPage.Controls.Add(this.m_lblFirstName);
            this.m_objInformationPage.Controls.Add(this.m_tbLastName);
            this.m_objInformationPage.Controls.Add(this.m_tbFirstName);
            this.m_objInformationPage.Location = new System.Drawing.Point(4, 22);
            this.m_objInformationPage.Name = "m_objInformationPage";
            this.m_objInformationPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_objInformationPage.Size = new System.Drawing.Size(736, 405);
            this.m_objInformationPage.TabIndex = 0;
            this.m_objInformationPage.Text = "Information";
            this.m_objInformationPage.UseVisualStyleBackColor = true;
            // 
            // m_pnlVisitor
            // 
            this.m_pnlVisitor.Controls.Add(this.m_lblDeparture);
            this.m_pnlVisitor.Controls.Add(this.m_lblArrival);
            this.m_pnlVisitor.Controls.Add(this.m_dtDeparture);
            this.m_pnlVisitor.Controls.Add(this.m_dtArrival);
            this.m_pnlVisitor.Location = new System.Drawing.Point(0, 120);
            this.m_pnlVisitor.Name = "m_pnlVisitor";
            this.m_pnlVisitor.Size = new System.Drawing.Size(560, 168);
            this.m_pnlVisitor.TabIndex = 23;
            this.m_pnlVisitor.Visible = false;
            // 
            // m_lblDeparture
            // 
            this.m_lblDeparture.Location = new System.Drawing.Point(8, 26);
            this.m_lblDeparture.Name = "m_lblDeparture";
            this.m_lblDeparture.Size = new System.Drawing.Size(64, 16);
            this.m_lblDeparture.TabIndex = 22;
            this.m_lblDeparture.Text = "Departure";
            this.m_lblDeparture.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblArrival
            // 
            this.m_lblArrival.Location = new System.Drawing.Point(8, 2);
            this.m_lblArrival.Name = "m_lblArrival";
            this.m_lblArrival.Size = new System.Drawing.Size(64, 16);
            this.m_lblArrival.TabIndex = 21;
            this.m_lblArrival.Text = "Arrival";
            this.m_lblArrival.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_dtDeparture
            // 
            this.m_dtDeparture.Location = new System.Drawing.Point(80, 26);
            this.m_dtDeparture.Name = "m_dtDeparture";
            this.m_dtDeparture.Size = new System.Drawing.Size(200, 20);
            this.m_dtDeparture.TabIndex = 20;
            this.m_dtDeparture.Value = new System.DateTime(2016, 12, 12, 15, 1, 14, 237);
            // 
            // m_dtArrival
            // 
            this.m_dtArrival.Location = new System.Drawing.Point(80, 2);
            this.m_dtArrival.Name = "m_dtArrival";
            this.m_dtArrival.Size = new System.Drawing.Size(200, 20);
            this.m_dtArrival.TabIndex = 19;
            this.m_dtArrival.Value = new System.DateTime(2016, 12, 12, 15, 1, 14, 239);
            // 
            // m_rbCardholder
            // 
            this.m_rbCardholder.AutoSize = true;
            this.m_rbCardholder.Checked = true;
            this.m_rbCardholder.Location = new System.Drawing.Point(16, 40);
            this.m_rbCardholder.Name = "m_rbCardholder";
            this.m_rbCardholder.Size = new System.Drawing.Size(76, 17);
            this.m_rbCardholder.TabIndex = 18;
            this.m_rbCardholder.TabStop = true;
            this.m_rbCardholder.Text = "Cardholder";
            this.m_rbCardholder.UseVisualStyleBackColor = true;
            this.m_rbCardholder.Click += new System.EventHandler(this.OnRadioEntityType_Click);
            // 
            // m_rbVisitor
            // 
            this.m_rbVisitor.AutoSize = true;
            this.m_rbVisitor.Location = new System.Drawing.Point(104, 40);
            this.m_rbVisitor.Name = "m_rbVisitor";
            this.m_rbVisitor.Size = new System.Drawing.Size(53, 17);
            this.m_rbVisitor.TabIndex = 17;
            this.m_rbVisitor.Text = "Visitor";
            this.m_rbVisitor.UseVisualStyleBackColor = true;
            this.m_rbVisitor.Click += new System.EventHandler(this.OnRadioEntityType_Click);
            // 
            // m_lblInformationDescription
            // 
            this.m_lblInformationDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lblInformationDescription.ForeColor = System.Drawing.Color.Blue;
            this.m_lblInformationDescription.Location = new System.Drawing.Point(8, 16);
            this.m_lblInformationDescription.Name = "m_lblInformationDescription";
            this.m_lblInformationDescription.Size = new System.Drawing.Size(720, 16);
            this.m_lblInformationDescription.TabIndex = 16;
            this.m_lblInformationDescription.Text = "Please enter the information about the entity to enroll:";
            this.m_lblInformationDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_lblLastName
            // 
            this.m_lblLastName.Location = new System.Drawing.Point(8, 96);
            this.m_lblLastName.Name = "m_lblLastName";
            this.m_lblLastName.Size = new System.Drawing.Size(64, 16);
            this.m_lblLastName.TabIndex = 11;
            this.m_lblLastName.Text = "Last name";
            this.m_lblLastName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_lblFirstName
            // 
            this.m_lblFirstName.Location = new System.Drawing.Point(8, 72);
            this.m_lblFirstName.Name = "m_lblFirstName";
            this.m_lblFirstName.Size = new System.Drawing.Size(64, 16);
            this.m_lblFirstName.TabIndex = 10;
            this.m_lblFirstName.Text = "First name";
            this.m_lblFirstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_tbLastName
            // 
            this.m_tbLastName.Location = new System.Drawing.Point(80, 96);
            this.m_tbLastName.Name = "m_tbLastName";
            this.m_tbLastName.Size = new System.Drawing.Size(200, 20);
            this.m_tbLastName.TabIndex = 9;
            this.m_tbLastName.TextChanged += new System.EventHandler(this.OnUserInterfaceChanged);
            // 
            // m_tbFirstName
            // 
            this.m_tbFirstName.Location = new System.Drawing.Point(80, 72);
            this.m_tbFirstName.Name = "m_tbFirstName";
            this.m_tbFirstName.Size = new System.Drawing.Size(200, 20);
            this.m_tbFirstName.TabIndex = 8;
            this.m_tbFirstName.TextChanged += new System.EventHandler(this.OnUserInterfaceChanged);
            // 
            // m_objCredentialPage
            // 
            this.m_objCredentialPage.Controls.Add(this.m_nupCardID);
            this.m_objCredentialPage.Controls.Add(this.label2);
            this.m_objCredentialPage.Controls.Add(this.m_nupFacility);
            this.m_objCredentialPage.Controls.Add(this.label1);
            this.m_objCredentialPage.Controls.Add(this.m_cbFormat);
            this.m_objCredentialPage.Controls.Add(this.m_rbAutomatic);
            this.m_objCredentialPage.Controls.Add(this.m_rbManual);
            this.m_objCredentialPage.Controls.Add(this.m_lblCredentialDescription);
            this.m_objCredentialPage.Controls.Add(this.m_lblFilter);
            this.m_objCredentialPage.Controls.Add(this.m_lvCredentials);
            this.m_objCredentialPage.Controls.Add(this.m_btnFilter);
            this.m_objCredentialPage.Location = new System.Drawing.Point(4, 22);
            this.m_objCredentialPage.Name = "m_objCredentialPage";
            this.m_objCredentialPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_objCredentialPage.Size = new System.Drawing.Size(736, 405);
            this.m_objCredentialPage.TabIndex = 1;
            this.m_objCredentialPage.Text = "Credential";
            this.m_objCredentialPage.UseVisualStyleBackColor = true;
            // 
            // m_nupCardID
            // 
            this.m_nupCardID.Location = new System.Drawing.Point(55, 352);
            this.m_nupCardID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nupCardID.Name = "m_nupCardID";
            this.m_nupCardID.Size = new System.Drawing.Size(157, 20);
            this.m_nupCardID.TabIndex = 26;
            this.m_nupCardID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 25;
            this.label2.Text = "Card ID";
            // 
            // m_nupFacility
            // 
            this.m_nupFacility.Location = new System.Drawing.Point(54, 326);
            this.m_nupFacility.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nupFacility.Name = "m_nupFacility";
            this.m_nupFacility.Size = new System.Drawing.Size(158, 20);
            this.m_nupFacility.TabIndex = 24;
            this.m_nupFacility.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Facility";
            // 
            // m_cbFormat
            // 
            this.m_cbFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbFormat.FormattingEnabled = true;
            this.m_cbFormat.Items.AddRange(new object[] {
            "Standard 26 bits",
            "H10302",
            "H10304",
            "H10306",
            "HID Corporate 1000"});
            this.m_cbFormat.Location = new System.Drawing.Point(12, 299);
            this.m_cbFormat.Name = "m_cbFormat";
            this.m_cbFormat.Size = new System.Drawing.Size(200, 21);
            this.m_cbFormat.TabIndex = 21;
            this.m_cbFormat.SelectedIndexChanged += new System.EventHandler(this.OnComboFormat_SelectedIndexChanged);
            // 
            // m_rbAutomatic
            // 
            this.m_rbAutomatic.AutoSize = true;
            this.m_rbAutomatic.Checked = true;
            this.m_rbAutomatic.Location = new System.Drawing.Point(12, 6);
            this.m_rbAutomatic.Name = "m_rbAutomatic";
            this.m_rbAutomatic.Size = new System.Drawing.Size(72, 17);
            this.m_rbAutomatic.TabIndex = 20;
            this.m_rbAutomatic.TabStop = true;
            this.m_rbAutomatic.Text = "Automatic";
            this.m_rbAutomatic.UseVisualStyleBackColor = true;
            this.m_rbAutomatic.CheckedChanged += new System.EventHandler(this.OnRadioAutomatic_CheckedChanged);
            // 
            // m_rbManual
            // 
            this.m_rbManual.AutoSize = true;
            this.m_rbManual.Location = new System.Drawing.Point(100, 6);
            this.m_rbManual.Name = "m_rbManual";
            this.m_rbManual.Size = new System.Drawing.Size(60, 17);
            this.m_rbManual.TabIndex = 19;
            this.m_rbManual.Text = "Manual";
            this.m_rbManual.UseVisualStyleBackColor = true;
            // 
            // m_lblCredentialDescription
            // 
            this.m_lblCredentialDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lblCredentialDescription.ForeColor = System.Drawing.Color.Blue;
            this.m_lblCredentialDescription.Location = new System.Drawing.Point(8, 30);
            this.m_lblCredentialDescription.Name = "m_lblCredentialDescription";
            this.m_lblCredentialDescription.Size = new System.Drawing.Size(720, 16);
            this.m_lblCredentialDescription.TabIndex = 17;
            this.m_lblCredentialDescription.Text = "Please select a door filter and swipe a card you want to assign to the enrolled e" +
    "ntity";
            this.m_lblCredentialDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // m_objSummaryPage
            // 
            this.m_objSummaryPage.Controls.Add(this.m_lblResult);
            this.m_objSummaryPage.Location = new System.Drawing.Point(4, 22);
            this.m_objSummaryPage.Name = "m_objSummaryPage";
            this.m_objSummaryPage.Padding = new System.Windows.Forms.Padding(3);
            this.m_objSummaryPage.Size = new System.Drawing.Size(736, 405);
            this.m_objSummaryPage.TabIndex = 2;
            this.m_objSummaryPage.Text = "Summary";
            this.m_objSummaryPage.UseVisualStyleBackColor = true;
            // 
            // m_lblResult
            // 
            this.m_lblResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_lblResult.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_lblResult.Location = new System.Drawing.Point(3, 3);
            this.m_lblResult.Name = "m_lblResult";
            this.m_lblResult.Size = new System.Drawing.Size(730, 399);
            this.m_lblResult.TabIndex = 0;
            this.m_lblResult.Text = "n/a";
            this.m_lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // m_pnlBottom
            // 
            this.m_pnlBottom.Controls.Add(this.m_btnCreate);
            this.m_pnlBottom.Controls.Add(this.m_btnNext);
            this.m_pnlBottom.Controls.Add(this.m_btnPrevious);
            this.m_pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_pnlBottom.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.m_pnlBottom.Location = new System.Drawing.Point(0, 456);
            this.m_pnlBottom.Name = "m_pnlBottom";
            this.m_pnlBottom.Size = new System.Drawing.Size(744, 40);
            this.m_pnlBottom.TabIndex = 11;
            this.m_pnlBottom.WrapContents = false;
            // 
            // m_btnCreate
            // 
            this.m_btnCreate.Location = new System.Drawing.Point(653, 3);
            this.m_btnCreate.Name = "m_btnCreate";
            this.m_btnCreate.Size = new System.Drawing.Size(88, 32);
            this.m_btnCreate.TabIndex = 2;
            this.m_btnCreate.Text = "&Create";
            this.m_btnCreate.UseVisualStyleBackColor = true;
            this.m_btnCreate.Visible = false;
            this.m_btnCreate.Click += new System.EventHandler(this.OnButtonCreate_Click);
            // 
            // m_btnNext
            // 
            this.m_btnNext.Location = new System.Drawing.Point(559, 3);
            this.m_btnNext.Name = "m_btnNext";
            this.m_btnNext.Size = new System.Drawing.Size(88, 32);
            this.m_btnNext.TabIndex = 0;
            this.m_btnNext.Text = "&Next";
            this.m_btnNext.UseVisualStyleBackColor = true;
            this.m_btnNext.Click += new System.EventHandler(this.OnButtonNext_Click);
            // 
            // m_btnPrevious
            // 
            this.m_btnPrevious.Location = new System.Drawing.Point(465, 3);
            this.m_btnPrevious.Name = "m_btnPrevious";
            this.m_btnPrevious.Size = new System.Drawing.Size(88, 32);
            this.m_btnPrevious.TabIndex = 1;
            this.m_btnPrevious.Text = "&Previous";
            this.m_btnPrevious.UseVisualStyleBackColor = true;
            this.m_btnPrevious.Visible = false;
            this.m_btnPrevious.Click += new System.EventHandler(this.OnButtonPrevious_Click);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 496);
            this.Controls.Add(this.m_objTabControl);
            this.Controls.Add(this.m_pnlBottom);
            this.Controls.Add(this.m_tsMain);
            this.DoubleBuffered = true;
            this.Name = "MainDlg";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enrollment sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.m_tsMain.ResumeLayout(false);
            this.m_tsMain.PerformLayout();
            this.m_objTabControl.ResumeLayout(false);
            this.m_objInformationPage.ResumeLayout(false);
            this.m_objInformationPage.PerformLayout();
            this.m_pnlVisitor.ResumeLayout(false);
            this.m_objCredentialPage.ResumeLayout(false);
            this.m_objCredentialPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nupCardID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nupFacility)).EndInit();
            this.m_objSummaryPage.ResumeLayout(false);
            this.m_pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip m_tsMain;
        private System.Windows.Forms.ToolStripButton m_btnConnect;
        private System.Windows.Forms.ToolStripButton m_btnDisconnect;
        private System.Windows.Forms.ListView m_lvCredentials;
        private System.Windows.Forms.Button m_btnFilter;
        private System.Windows.Forms.Label m_lblFilter;
        private System.Windows.Forms.TabControl m_objTabControl;
        private System.Windows.Forms.TabPage m_objInformationPage;
        private System.Windows.Forms.Label m_lblLastName;
        private System.Windows.Forms.Label m_lblFirstName;
        private System.Windows.Forms.TextBox m_tbLastName;
        private System.Windows.Forms.TextBox m_tbFirstName;
        private System.Windows.Forms.TabPage m_objCredentialPage;
        private System.Windows.Forms.Label m_lblInformationDescription;
        private System.Windows.Forms.RadioButton m_rbCardholder;
        private System.Windows.Forms.RadioButton m_rbVisitor;
        private System.Windows.Forms.FlowLayoutPanel m_pnlBottom;
        private System.Windows.Forms.Button m_btnPrevious;
        private System.Windows.Forms.Button m_btnNext;
        private System.Windows.Forms.TabPage m_objSummaryPage;
        private System.Windows.Forms.Label m_lblCredentialDescription;
        private System.Windows.Forms.Button m_btnCreate;
        private System.Windows.Forms.Label m_lblResult;
        private System.Windows.Forms.DateTimePicker m_dtArrival;
        private System.Windows.Forms.Label m_lblDeparture;
        private System.Windows.Forms.Label m_lblArrival;
        private System.Windows.Forms.DateTimePicker m_dtDeparture;
        private System.Windows.Forms.Panel m_pnlVisitor;
        private System.Windows.Forms.ComboBox m_cbFormat;
        private System.Windows.Forms.RadioButton m_rbAutomatic;
        private System.Windows.Forms.RadioButton m_rbManual;
        private System.Windows.Forms.NumericUpDown m_nupCardID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nupFacility;
        private System.Windows.Forms.Label label1;
    }
}

