
namespace QuerySample
{
    partial class BlockingQueryUI
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
            this.m_cameraList = new System.Windows.Forms.ListView();
            this.m_cameras = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.m_end = new System.Windows.Forms.DateTimePicker();
            this.m_to = new System.Windows.Forms.Label();
            this.m_start = new System.Windows.Forms.DateTimePicker();
            this.m_from = new System.Windows.Forms.Label();
            this.m_browseQuery = new System.Windows.Forms.Button();
            this.m_removeCamera = new System.Windows.Forms.Button();
            this.m_addCamera = new System.Windows.Forms.Button();
            this.m_stopBlock = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.m_startBlock = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.blockBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.blockLevel = new System.Windows.Forms.NumericUpDown();
            this.unblockBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.blockLevel)).BeginInit();
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
            this.m_cameraList.Location = new System.Drawing.Point(1, 8);
            this.m_cameraList.Name = "m_cameraList";
            this.m_cameraList.Size = new System.Drawing.Size(262, 63);
            this.m_cameraList.TabIndex = 8;
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
            this.m_end.Location = new System.Drawing.Point(54, 132);
            this.m_end.Name = "m_end";
            this.m_end.Size = new System.Drawing.Size(203, 20);
            this.m_end.TabIndex = 14;
            // 
            // m_to
            // 
            this.m_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_to.AutoSize = true;
            this.m_to.Location = new System.Drawing.Point(5, 132);
            this.m_to.Name = "m_to";
            this.m_to.Size = new System.Drawing.Size(20, 13);
            this.m_to.TabIndex = 13;
            this.m_to.Text = "To";
            // 
            // m_start
            // 
            this.m_start.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_start.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_start.Location = new System.Drawing.Point(54, 106);
            this.m_start.Name = "m_start";
            this.m_start.Size = new System.Drawing.Size(203, 20);
            this.m_start.TabIndex = 12;
            // 
            // m_from
            // 
            this.m_from.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_from.AutoSize = true;
            this.m_from.Location = new System.Drawing.Point(5, 106);
            this.m_from.Name = "m_from";
            this.m_from.Size = new System.Drawing.Size(30, 13);
            this.m_from.TabIndex = 11;
            this.m_from.Text = "From";
            // 
            // m_browseQuery
            // 
            this.m_browseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseQuery.Location = new System.Drawing.Point(197, 158);
            this.m_browseQuery.Name = "m_browseQuery";
            this.m_browseQuery.Size = new System.Drawing.Size(60, 20);
            this.m_browseQuery.TabIndex = 15;
            this.m_browseQuery.Text = "Query";
            this.m_browseQuery.UseVisualStyleBackColor = true;
            this.m_browseQuery.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_removeCamera
            // 
            this.m_removeCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_removeCamera.Location = new System.Drawing.Point(40, 77);
            this.m_removeCamera.Name = "m_removeCamera";
            this.m_removeCamera.Size = new System.Drawing.Size(26, 23);
            this.m_removeCamera.TabIndex = 10;
            this.m_removeCamera.Text = "-";
            this.m_removeCamera.UseVisualStyleBackColor = true;
            this.m_removeCamera.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // m_addCamera
            // 
            this.m_addCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_addCamera.Location = new System.Drawing.Point(8, 77);
            this.m_addCamera.Name = "m_addCamera";
            this.m_addCamera.Size = new System.Drawing.Size(26, 23);
            this.m_addCamera.TabIndex = 9;
            this.m_addCamera.Text = "+";
            this.m_addCamera.UseVisualStyleBackColor = true;
            this.m_addCamera.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // m_stopBlock
            // 
            this.m_stopBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_stopBlock.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_stopBlock.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_stopBlock.Location = new System.Drawing.Point(54, 210);
            this.m_stopBlock.Name = "m_stopBlock";
            this.m_stopBlock.Size = new System.Drawing.Size(203, 20);
            this.m_stopBlock.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "To";
            // 
            // m_startBlock
            // 
            this.m_startBlock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_startBlock.CustomFormat = "  yyyy-MM-dd    hh:mm:ss tt";
            this.m_startBlock.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.m_startBlock.Location = new System.Drawing.Point(54, 184);
            this.m_startBlock.Name = "m_startBlock";
            this.m_startBlock.Size = new System.Drawing.Size(203, 20);
            this.m_startBlock.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "From";
            // 
            // blockBtn
            // 
            this.blockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blockBtn.Location = new System.Drawing.Point(197, 265);
            this.blockBtn.Name = "blockBtn";
            this.blockBtn.Size = new System.Drawing.Size(60, 20);
            this.blockBtn.TabIndex = 20;
            this.blockBtn.Text = "Block";
            this.blockBtn.UseVisualStyleBackColor = true;
            this.blockBtn.Click += new System.EventHandler(this.OnBlockBtn_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Level";
            // 
            // blockLevel
            // 
            this.blockLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.blockLevel.Location = new System.Drawing.Point(54, 239);
            this.blockLevel.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.blockLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.blockLevel.Name = "blockLevel";
            this.blockLevel.Size = new System.Drawing.Size(120, 20);
            this.blockLevel.TabIndex = 22;
            this.blockLevel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // unblockBtn
            // 
            this.unblockBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.unblockBtn.Location = new System.Drawing.Point(54, 265);
            this.unblockBtn.Name = "unblockBtn";
            this.unblockBtn.Size = new System.Drawing.Size(60, 20);
            this.unblockBtn.TabIndex = 23;
            this.unblockBtn.Text = "Unblock";
            this.unblockBtn.UseVisualStyleBackColor = true;
            this.unblockBtn.Click += new System.EventHandler(this.OnUnblockBtnClick);
            // 
            // BlockingQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.unblockBtn);
            this.Controls.Add(this.blockLevel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_stopBlock);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_startBlock);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.blockBtn);
            this.Controls.Add(this.m_cameraList);
            this.Controls.Add(this.m_end);
            this.Controls.Add(this.m_to);
            this.Controls.Add(this.m_start);
            this.Controls.Add(this.m_from);
            this.Controls.Add(this.m_browseQuery);
            this.Controls.Add(this.m_removeCamera);
            this.Controls.Add(this.m_addCamera);
            this.Name = "BlockingQueryUI";
            this.Size = new System.Drawing.Size(265, 294);
            ((System.ComponentModel.ISupportInitialize)(this.blockLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView m_cameraList;
        private System.Windows.Forms.ColumnHeader m_cameras;
        private System.Windows.Forms.DateTimePicker m_end;
        private System.Windows.Forms.Label m_to;
        private System.Windows.Forms.DateTimePicker m_start;
        private System.Windows.Forms.Label m_from;
        private System.Windows.Forms.Button m_browseQuery;
        private System.Windows.Forms.Button m_removeCamera;
        private System.Windows.Forms.Button m_addCamera;
        private System.Windows.Forms.DateTimePicker m_stopBlock;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker m_startBlock;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button blockBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown blockLevel;
        private System.Windows.Forms.Button unblockBtn;
    }
}
