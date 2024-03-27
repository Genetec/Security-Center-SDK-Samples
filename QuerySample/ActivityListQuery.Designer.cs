// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class ActivityListQuery
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Button m_add;

        private System.Windows.Forms.Button m_delete;

        private System.Windows.Forms.Button m_doorActivityQuery;

        private System.Windows.Forms.ColumnHeader m_entities;

        private System.Windows.Forms.ListView m_entityList;

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
            this.m_doorActivityQuery = new System.Windows.Forms.Button();
            this.m_delete = new System.Windows.Forms.Button();
            this.m_add = new System.Windows.Forms.Button();
            this.m_entityList = new System.Windows.Forms.ListView();
            this.m_entities = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // m_doorActivityQuery
            // 
            this.m_doorActivityQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_doorActivityQuery.Location = new System.Drawing.Point(208, 384);
            this.m_doorActivityQuery.Name = "m_doorActivityQuery";
            this.m_doorActivityQuery.Size = new System.Drawing.Size(58, 23);
            this.m_doorActivityQuery.TabIndex = 7;
            this.m_doorActivityQuery.Text = "Query";
            this.m_doorActivityQuery.UseVisualStyleBackColor = true;
            this.m_doorActivityQuery.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_delete
            // 
            this.m_delete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_delete.Location = new System.Drawing.Point(36, 384);
            this.m_delete.Name = "m_delete";
            this.m_delete.Size = new System.Drawing.Size(26, 23);
            this.m_delete.TabIndex = 6;
            this.m_delete.Text = "-";
            this.m_delete.UseVisualStyleBackColor = true;
            this.m_delete.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // m_add
            // 
            this.m_add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_add.Location = new System.Drawing.Point(4, 384);
            this.m_add.Name = "m_add";
            this.m_add.Size = new System.Drawing.Size(26, 23);
            this.m_add.TabIndex = 5;
            this.m_add.Text = "+";
            this.m_add.UseVisualStyleBackColor = true;
            this.m_add.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // m_entityList
            // 
            this.m_entityList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                              | System.Windows.Forms.AnchorStyles.Left)
                                                                             | System.Windows.Forms.AnchorStyles.Right)));
            this.m_entityList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.m_entities
            });
            this.m_entityList.Location = new System.Drawing.Point(3, 0);
            this.m_entityList.Name = "m_entityList";
            this.m_entityList.Size = new System.Drawing.Size(262, 378);
            this.m_entityList.TabIndex = 4;
            this.m_entityList.UseCompatibleStateImageBehavior = false;
            this.m_entityList.View = System.Windows.Forms.View.Details;
            // 
            // m_entities
            // 
            this.m_entities.Text = "m_entities";
            this.m_entities.Width = 256;
            // 
            // ActivityListQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_doorActivityQuery);
            this.Controls.Add(this.m_delete);
            this.Controls.Add(this.m_add);
            this.Controls.Add(this.m_entityList);
            this.Name = "ActivityListQuery";
            this.Size = new System.Drawing.Size(270, 410);
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion
}

