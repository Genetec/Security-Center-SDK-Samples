// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace Genetec.Sdk.Samples.SamplesLibrary
{
    #region Classes

    partial class SearchDlg
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_cancel;

        private System.Windows.Forms.Label m_description;

        private System.Windows.Forms.TextBox m_descriptionInput;

        private System.Windows.Forms.ListView m_entitiesList;

        private System.Windows.Forms.Label m_firstName;

        private System.Windows.Forms.TextBox m_name;

        private System.Windows.Forms.ColumnHeader m_nameColumnHeader;

        private System.Windows.Forms.Button m_ok;

        private System.Windows.Forms.Panel m_okayAndCancelButtons;

        private System.Windows.Forms.Button m_search;

        private System.Windows.Forms.SplitContainer m_searchDialog;

        private System.Windows.Forms.ColumnHeader m_typeColumnHeader;

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
            this.m_searchDialog = new System.Windows.Forms.SplitContainer();
            this.m_search = new System.Windows.Forms.Button();
            this.m_description = new System.Windows.Forms.Label();
            this.m_descriptionInput = new System.Windows.Forms.TextBox();
            this.m_name = new System.Windows.Forms.TextBox();
            this.m_firstName = new System.Windows.Forms.Label();
            this.m_entitiesList = new System.Windows.Forms.ListView();
            this.m_nameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.m_typeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.m_okayAndCancelButtons = new System.Windows.Forms.Panel();
            this.m_ok = new System.Windows.Forms.Button();
            this.m_cancel = new System.Windows.Forms.Button();
            this.m_searchDialog.Panel1.SuspendLayout();
            this.m_searchDialog.Panel2.SuspendLayout();
            this.m_searchDialog.SuspendLayout();
            this.m_okayAndCancelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_searchDialog
            // 
            this.m_searchDialog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_searchDialog.IsSplitterFixed = true;
            this.m_searchDialog.Location = new System.Drawing.Point(0, 0);
            this.m_searchDialog.Name = "m_searchDialog";
            // 
            // m_searchDialog.Panel1
            // 
            this.m_searchDialog.Panel1.Controls.Add(this.m_search);
            this.m_searchDialog.Panel1.Controls.Add(this.m_description);
            this.m_searchDialog.Panel1.Controls.Add(this.m_descriptionInput);
            this.m_searchDialog.Panel1.Controls.Add(this.m_name);
            this.m_searchDialog.Panel1.Controls.Add(this.m_firstName);
            // 
            // m_searchDialog.Panel2
            // 
            this.m_searchDialog.Panel2.Controls.Add(this.m_entitiesList);
            this.m_searchDialog.Panel2.Controls.Add(this.m_okayAndCancelButtons);
            this.m_searchDialog.Size = new System.Drawing.Size(735, 395);
            this.m_searchDialog.SplitterDistance = 226;
            this.m_searchDialog.TabIndex = 0;
            // 
            // m_search
            // 
            this.m_search.Location = new System.Drawing.Point(48, 80);
            this.m_search.Name = "m_search";
            this.m_search.Size = new System.Drawing.Size(128, 40);
            this.m_search.TabIndex = 14;
            this.m_search.Text = "Search";
            this.m_search.UseVisualStyleBackColor = true;
            this.m_search.Click += new System.EventHandler(this.OnButtonSearchClick);
            // 
            // m_description
            // 
            this.m_description.Location = new System.Drawing.Point(8, 48);
            this.m_description.Name = "m_description";
            this.m_description.Size = new System.Drawing.Size(64, 20);
            this.m_description.TabIndex = 13;
            this.m_description.Text = "Description";
            this.m_description.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_descriptionInput
            // 
            this.m_descriptionInput.Location = new System.Drawing.Point(80, 48);
            this.m_descriptionInput.Name = "m_descriptionInput";
            this.m_descriptionInput.Size = new System.Drawing.Size(136, 20);
            this.m_descriptionInput.TabIndex = 11;
            this.m_descriptionInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextFieldKeyPress);
            // 
            // m_name
            // 
            this.m_name.Location = new System.Drawing.Point(80, 16);
            this.m_name.Name = "m_name";
            this.m_name.Size = new System.Drawing.Size(136, 20);
            this.m_name.TabIndex = 9;
            this.m_name.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnTextFieldKeyPress);
            // 
            // m_firstName
            // 
            this.m_firstName.Location = new System.Drawing.Point(8, 16);
            this.m_firstName.Name = "m_firstName";
            this.m_firstName.Size = new System.Drawing.Size(64, 20);
            this.m_firstName.TabIndex = 8;
            this.m_firstName.Text = "Name";
            this.m_firstName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_entitiesList
            // 
            this.m_entitiesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.m_nameColumnHeader,
                this.m_typeColumnHeader
            });
            this.m_entitiesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_entitiesList.FullRowSelect = true;
            this.m_entitiesList.GridLines = true;
            this.m_entitiesList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_entitiesList.HideSelection = false;
            this.m_entitiesList.Location = new System.Drawing.Point(0, 0);
            this.m_entitiesList.Name = "m_entitiesList";
            this.m_entitiesList.Size = new System.Drawing.Size(505, 344);
            this.m_entitiesList.TabIndex = 1;
            this.m_entitiesList.UseCompatibleStateImageBehavior = false;
            this.m_entitiesList.View = System.Windows.Forms.View.Details;
            this.m_entitiesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnListEntitiesMouseDoubleClick);
            // 
            // m_nameColumnHeader
            // 
            this.m_nameColumnHeader.Text = "Name";
            this.m_nameColumnHeader.Width = 132;
            // 
            // m_typeColumnHeader
            // 
            this.m_typeColumnHeader.Text = "Type";
            this.m_typeColumnHeader.Width = 230;
            // 
            // m_okayAndCancelButtons
            // 
            this.m_okayAndCancelButtons.Controls.Add(this.m_ok);
            this.m_okayAndCancelButtons.Controls.Add(this.m_cancel);
            this.m_okayAndCancelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_okayAndCancelButtons.Location = new System.Drawing.Point(0, 344);
            this.m_okayAndCancelButtons.Name = "m_okayAndCancelButtons";
            this.m_okayAndCancelButtons.Size = new System.Drawing.Size(505, 51);
            this.m_okayAndCancelButtons.TabIndex = 0;
            // 
            // m_ok
            // 
            this.m_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_ok.Location = new System.Drawing.Point(296, 8);
            this.m_ok.Name = "m_ok";
            this.m_ok.Size = new System.Drawing.Size(98, 32);
            this.m_ok.TabIndex = 16;
            this.m_ok.Text = "&OK";
            this.m_ok.UseVisualStyleBackColor = true;
            // 
            // m_cancel
            // 
            this.m_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_cancel.Location = new System.Drawing.Point(400, 8);
            this.m_cancel.Name = "m_cancel";
            this.m_cancel.Size = new System.Drawing.Size(98, 32);
            this.m_cancel.TabIndex = 15;
            this.m_cancel.Text = "&Cancel";
            this.m_cancel.UseVisualStyleBackColor = true;
            // 
            // SearchDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 395);
            this.Controls.Add(this.m_searchDialog);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Search dialog";
            this.m_searchDialog.Panel1.ResumeLayout(false);
            this.m_searchDialog.Panel1.PerformLayout();
            this.m_searchDialog.Panel2.ResumeLayout(false);
            this.m_searchDialog.ResumeLayout(false);
            this.m_okayAndCancelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion
}

