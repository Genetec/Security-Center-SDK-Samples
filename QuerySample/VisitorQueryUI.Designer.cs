// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class VisitorQueryUI
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox m_company;

        private System.Windows.Forms.Label m_companyName;

        private System.Windows.Forms.Label m_familyName;

        private System.Windows.Forms.TextBox m_firstName;

        private System.Windows.Forms.TextBox m_lastName;

        private System.Windows.Forms.Label m_name;

        private System.Windows.Forms.Button m_query;

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
            this.m_name = new System.Windows.Forms.Label();
            this.m_firstName = new System.Windows.Forms.TextBox();
            this.m_query = new System.Windows.Forms.Button();
            this.m_lastName = new System.Windows.Forms.TextBox();
            this.m_familyName = new System.Windows.Forms.Label();
            this.m_company = new System.Windows.Forms.TextBox();
            this.m_companyName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_name
            // 
            this.m_name.AutoSize = true;
            this.m_name.Location = new System.Drawing.Point(14, 21);
            this.m_name.Name = "m_name";
            this.m_name.Size = new System.Drawing.Size(57, 13);
            this.m_name.TabIndex = 0;
            this.m_name.Text = "First Name";
            // 
            // m_firstName
            // 
            this.m_firstName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_firstName.Location = new System.Drawing.Point(77, 18);
            this.m_firstName.Name = "m_firstName";
            this.m_firstName.Size = new System.Drawing.Size(179, 20);
            this.m_firstName.TabIndex = 1;
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_query.Location = new System.Drawing.Point(196, 262);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(60, 20);
            this.m_query.TabIndex = 5;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_lastName
            // 
            this.m_lastName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                           | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lastName.Location = new System.Drawing.Point(77, 44);
            this.m_lastName.Name = "m_lastName";
            this.m_lastName.Size = new System.Drawing.Size(179, 20);
            this.m_lastName.TabIndex = 7;
            // 
            // m_familyName
            // 
            this.m_familyName.AutoSize = true;
            this.m_familyName.Location = new System.Drawing.Point(14, 47);
            this.m_familyName.Name = "m_familyName";
            this.m_familyName.Size = new System.Drawing.Size(58, 13);
            this.m_familyName.TabIndex = 6;
            this.m_familyName.Text = "Last Name";
            // 
            // m_company
            // 
            this.m_company.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                          | System.Windows.Forms.AnchorStyles.Right)));
            this.m_company.Location = new System.Drawing.Point(77, 70);
            this.m_company.Name = "m_company";
            this.m_company.Size = new System.Drawing.Size(179, 20);
            this.m_company.TabIndex = 9;
            // 
            // m_companyName
            // 
            this.m_companyName.AutoSize = true;
            this.m_companyName.Location = new System.Drawing.Point(14, 73);
            this.m_companyName.Name = "m_companyName";
            this.m_companyName.Size = new System.Drawing.Size(51, 13);
            this.m_companyName.TabIndex = 8;
            this.m_companyName.Text = "Company";
            // 
            // VisitorQueryUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_company);
            this.Controls.Add(this.m_companyName);
            this.Controls.Add(this.m_lastName);
            this.Controls.Add(this.m_familyName);
            this.Controls.Add(this.m_query);
            this.Controls.Add(this.m_firstName);
            this.Controls.Add(this.m_name);
            this.Name = "VisitorQueryUI";
            this.Size = new System.Drawing.Size(265, 294);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

