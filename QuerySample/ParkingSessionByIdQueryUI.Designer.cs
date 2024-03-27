using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuerySample
{
    partial class ParkingSessionByIdQueryUI
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
            this.m_parkingZoneTextBox = new System.Windows.Forms.TextBox();
            this.m_parkingZoneLabel = new System.Windows.Forms.Label();
            this.m_parkingSessionLabel = new System.Windows.Forms.Label();
            this.m_browseSourcesParkingZone = new System.Windows.Forms.Button();
            this.m_query = new System.Windows.Forms.Button();
            this.m_parkingSessionsComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // m_parkingZoneTextBox
            // 
            this.m_parkingZoneTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.m_parkingZoneTextBox.Location = new System.Drawing.Point(8, 48);
            this.m_parkingZoneTextBox.Name = "m_parkingZoneTextBox";
            this.m_parkingZoneTextBox.Size = new System.Drawing.Size(224, 20);
            this.m_parkingZoneTextBox.TabIndex = 0;
            // 
            // m_parkingZoneLabel
            // 
            this.m_parkingZoneLabel.AutoSize = true;
            this.m_parkingZoneLabel.Location = new System.Drawing.Point(6, 32);
            this.m_parkingZoneLabel.Name = "m_parkingZoneLabel";
            this.m_parkingZoneLabel.Size = new System.Drawing.Size(71, 13);
            this.m_parkingZoneLabel.TabIndex = 3;
            this.m_parkingZoneLabel.Text = "Parking Zone";
            // 
            // m_parkingSessionLabel
            // 
            this.m_parkingSessionLabel.AutoSize = true;
            this.m_parkingSessionLabel.Location = new System.Drawing.Point(6, 86);
            this.m_parkingSessionLabel.Name = "m_parkingSessionLabel";
            this.m_parkingSessionLabel.Size = new System.Drawing.Size(83, 13);
            this.m_parkingSessionLabel.TabIndex = 4;
            this.m_parkingSessionLabel.Text = "Parking Session";
            // 
            // m_browseSourcesParkingZone
            // 
            this.m_browseSourcesParkingZone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseSourcesParkingZone.Location = new System.Drawing.Point(232, 48);
            this.m_browseSourcesParkingZone.Name = "m_browseSourcesParkingZone";
            this.m_browseSourcesParkingZone.Size = new System.Drawing.Size(30, 20);
            this.m_browseSourcesParkingZone.TabIndex = 22;
            this.m_browseSourcesParkingZone.Text = "...";
            this.m_browseSourcesParkingZone.UseVisualStyleBackColor = true;
            this.m_browseSourcesParkingZone.Click += new System.EventHandler(this.m_browseSourcesParkingZone_Click);
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_query.Location = new System.Drawing.Point(44, 211);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(188, 35);
            this.m_query.TabIndex = 23;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.m_query_Click);
            // 
            // m_parkingSessionsComboBox
            // 
            this.m_parkingSessionsComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_parkingSessionsComboBox.FormattingEnabled = true;
            this.m_parkingSessionsComboBox.Location = new System.Drawing.Point(8, 102);
            this.m_parkingSessionsComboBox.Name = "m_parkingSessionsComboBox";
            this.m_parkingSessionsComboBox.Size = new System.Drawing.Size(254, 21);
            this.m_parkingSessionsComboBox.TabIndex = 24;
            this.m_parkingSessionsComboBox.Text = "Select a Parking Zone first";
            this.m_parkingSessionsComboBox.SelectedIndexChanged += new System.EventHandler(this.m_parkingZoneComboBox_SelectedIndexChanged);
            // 
            // ParkingSessionByIdQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_parkingSessionsComboBox);
            this.Controls.Add(this.m_query);
            this.Controls.Add(this.m_browseSourcesParkingZone);
            this.Controls.Add(this.m_parkingSessionLabel);
            this.Controls.Add(this.m_parkingZoneLabel);
            this.Controls.Add(this.m_parkingZoneTextBox);
            this.Name = "ParkingSessionByIdQueryUI";
            this.Size = new System.Drawing.Size(265, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_parkingZoneTextBox;
        private System.Windows.Forms.Label m_parkingZoneLabel;
        private System.Windows.Forms.Label m_parkingSessionLabel;
        private System.Windows.Forms.Button m_browseSourcesParkingZone;
        private System.Windows.Forms.Button m_query;
        private System.Windows.Forms.ComboBox m_parkingSessionsComboBox;
    }
}
