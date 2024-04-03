
namespace QuerySample
{
    partial class SequenceQueryUI
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
            this.m_cameraList.Location = new System.Drawing.Point(11, 8);
            this.m_cameraList.Name = "m_cameraList";
            this.m_cameraList.Size = new System.Drawing.Size(262, 158);
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
            this.m_end.Location = new System.Drawing.Point(64, 227);
            this.m_end.Name = "m_end";
            this.m_end.Size = new System.Drawing.Size(203, 20);
            this.m_end.TabIndex = 14;
            // 
            // m_to
            // 
            this.m_to.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_to.AutoSize = true;
            this.m_to.Location = new System.Drawing.Point(15, 227);
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
            this.m_start.Location = new System.Drawing.Point(64, 201);
            this.m_start.Name = "m_start";
            this.m_start.Size = new System.Drawing.Size(203, 20);
            this.m_start.TabIndex = 12;
            // 
            // m_from
            // 
            this.m_from.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_from.AutoSize = true;
            this.m_from.Location = new System.Drawing.Point(15, 201);
            this.m_from.Name = "m_from";
            this.m_from.Size = new System.Drawing.Size(30, 13);
            this.m_from.TabIndex = 11;
            this.m_from.Text = "From";
            // 
            // m_browseQuery
            // 
            this.m_browseQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseQuery.Location = new System.Drawing.Point(207, 267);
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
            this.m_removeCamera.Location = new System.Drawing.Point(50, 172);
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
            this.m_addCamera.Location = new System.Drawing.Point(18, 172);
            this.m_addCamera.Name = "m_addCamera";
            this.m_addCamera.Size = new System.Drawing.Size(26, 23);
            this.m_addCamera.TabIndex = 9;
            this.m_addCamera.Text = "+";
            this.m_addCamera.UseVisualStyleBackColor = true;
            this.m_addCamera.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // SequenceQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cameraList);
            this.Controls.Add(this.m_end);
            this.Controls.Add(this.m_to);
            this.Controls.Add(this.m_start);
            this.Controls.Add(this.m_from);
            this.Controls.Add(this.m_browseQuery);
            this.Controls.Add(this.m_removeCamera);
            this.Controls.Add(this.m_addCamera);
            this.Name = "SequenceQueryUI";
            this.Size = new System.Drawing.Size(285, 295);
            this.Load += new System.EventHandler(this.SequenceQueryUI_Load);
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
    }
}
