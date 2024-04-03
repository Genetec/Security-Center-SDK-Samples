// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Entity101
{
    #region Classes

    partial class Entity101
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_buttonAsyncQuery;

        private System.Windows.Forms.Button m_buttonCreate;

        private System.Windows.Forms.Button m_buttonDelete;

        private System.Windows.Forms.Button m_buttonLogOff;

        private System.Windows.Forms.Button m_buttonLogOn;

        private System.Windows.Forms.Button m_buttonUpdate;

        private System.Windows.Forms.TextBox m_textboxConsole;

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
            this.m_buttonAsyncQuery = new System.Windows.Forms.Button();
            this.m_textboxConsole = new System.Windows.Forms.TextBox();
            this.m_buttonLogOff = new System.Windows.Forms.Button();
            this.m_buttonLogOn = new System.Windows.Forms.Button();
            this.m_buttonUpdate = new System.Windows.Forms.Button();
            this.m_buttonCreate = new System.Windows.Forms.Button();
            this.m_buttonDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _buttonAsyncQuery
            // 
            this.m_buttonAsyncQuery.Enabled = false;
            this.m_buttonAsyncQuery.Location = new System.Drawing.Point(3, 12);
            this.m_buttonAsyncQuery.Name = "m_buttonAsyncQuery";
            this.m_buttonAsyncQuery.Size = new System.Drawing.Size(89, 23);
            this.m_buttonAsyncQuery.TabIndex = 10;
            this.m_buttonAsyncQuery.Text = "Query";
            this.m_buttonAsyncQuery.UseVisualStyleBackColor = true;
            this.m_buttonAsyncQuery.Click += new System.EventHandler(this.OnButtonAsyncQueryClick);
            // 
            // _textboxConsole
            // 
            this.m_textboxConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                  | System.Windows.Forms.AnchorStyles.Left)
                                                                                 | System.Windows.Forms.AnchorStyles.Right)));
            this.m_textboxConsole.Location = new System.Drawing.Point(3, 43);
            this.m_textboxConsole.Multiline = true;
            this.m_textboxConsole.Name = "m_textboxConsole";
            this.m_textboxConsole.ReadOnly = true;
            this.m_textboxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_textboxConsole.Size = new System.Drawing.Size(391, 195);
            this.m_textboxConsole.TabIndex = 8;
            // 
            // _buttonLogOff
            // 
            this.m_buttonLogOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonLogOff.Enabled = false;
            this.m_buttonLogOff.Location = new System.Drawing.Point(319, 246);
            this.m_buttonLogOff.Name = "m_buttonLogOff";
            this.m_buttonLogOff.Size = new System.Drawing.Size(75, 23);
            this.m_buttonLogOff.TabIndex = 7;
            this.m_buttonLogOff.Text = "LogOff";
            this.m_buttonLogOff.UseVisualStyleBackColor = true;
            this.m_buttonLogOff.Click += new System.EventHandler(this.OnButtonLogOffClick);
            // 
            // _buttonLogOn
            // 
            this.m_buttonLogOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_buttonLogOn.Location = new System.Drawing.Point(238, 246);
            this.m_buttonLogOn.Name = "m_buttonLogOn";
            this.m_buttonLogOn.Size = new System.Drawing.Size(75, 23);
            this.m_buttonLogOn.TabIndex = 6;
            this.m_buttonLogOn.Text = "LogOn";
            this.m_buttonLogOn.UseVisualStyleBackColor = true;
            this.m_buttonLogOn.Click += new System.EventHandler(this.OnButtonLogOnClick);
            // 
            // _buttonUpdate
            // 
            this.m_buttonUpdate.Enabled = false;
            this.m_buttonUpdate.Location = new System.Drawing.Point(98, 12);
            this.m_buttonUpdate.Name = "m_buttonUpdate";
            this.m_buttonUpdate.Size = new System.Drawing.Size(89, 23);
            this.m_buttonUpdate.TabIndex = 11;
            this.m_buttonUpdate.Text = "Update";
            this.m_buttonUpdate.UseVisualStyleBackColor = true;
            this.m_buttonUpdate.Click += new System.EventHandler(this.OnButtonUpdateClick);
            // 
            // _buttonCreate
            // 
            this.m_buttonCreate.Enabled = false;
            this.m_buttonCreate.Location = new System.Drawing.Point(193, 12);
            this.m_buttonCreate.Name = "m_buttonCreate";
            this.m_buttonCreate.Size = new System.Drawing.Size(89, 23);
            this.m_buttonCreate.TabIndex = 12;
            this.m_buttonCreate.Text = "Create";
            this.m_buttonCreate.UseVisualStyleBackColor = true;
            this.m_buttonCreate.Click += new System.EventHandler(this.OnButtonCreateClick);
            // 
            // _buttonDelete
            // 
            this.m_buttonDelete.Enabled = false;
            this.m_buttonDelete.Location = new System.Drawing.Point(288, 12);
            this.m_buttonDelete.Name = "m_buttonDelete";
            this.m_buttonDelete.Size = new System.Drawing.Size(89, 23);
            this.m_buttonDelete.TabIndex = 13;
            this.m_buttonDelete.Text = "Delete";
            this.m_buttonDelete.UseVisualStyleBackColor = true;
            this.m_buttonDelete.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 281);
            this.Controls.Add(this.m_buttonDelete);
            this.Controls.Add(this.m_buttonCreate);
            this.Controls.Add(this.m_buttonUpdate);
            this.Controls.Add(this.m_buttonAsyncQuery);
            this.Controls.Add(this.m_textboxConsole);
            this.Controls.Add(this.m_buttonLogOff);
            this.Controls.Add(this.m_buttonLogOn);
            this.Name = "Entity101";
            this.Text = "Entity101";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }

    #endregion
}

