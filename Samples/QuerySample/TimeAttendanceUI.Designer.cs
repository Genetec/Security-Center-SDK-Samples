// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace QuerySample
{
    #region Classes

    partial class TimeAttendanceUI
    {
        #region Fields

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ColumnHeader Entities;

        private System.Windows.Forms.Button m_addEntity;

        private System.Windows.Forms.Button m_deleteEntity;

        private System.Windows.Forms.ListView m_entityList;

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
            this.Entities = new System.Windows.Forms.ColumnHeader();
            this.m_query = new System.Windows.Forms.Button();
            this.m_deleteEntity = new System.Windows.Forms.Button();
            this.m_addEntity = new System.Windows.Forms.Button();
            this.m_entityList = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // Entities
            // 
            this.Entities.Text = "Entities";
            this.Entities.Width = 203;
            // 
            // m_query
            // 
            this.m_query.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_query.Location = new System.Drawing.Point(188, 421);
            this.m_query.Name = "m_query";
            this.m_query.Size = new System.Drawing.Size(58, 23);
            this.m_query.TabIndex = 11;
            this.m_query.Text = "Query";
            this.m_query.UseVisualStyleBackColor = true;
            this.m_query.Click += new System.EventHandler(this.OnButtonQueryClick);
            // 
            // m_deleteEntity
            // 
            this.m_deleteEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_deleteEntity.Location = new System.Drawing.Point(36, 421);
            this.m_deleteEntity.Name = "m_deleteEntity";
            this.m_deleteEntity.Size = new System.Drawing.Size(26, 23);
            this.m_deleteEntity.TabIndex = 10;
            this.m_deleteEntity.Text = "-";
            this.m_deleteEntity.UseVisualStyleBackColor = true;
            this.m_deleteEntity.Click += new System.EventHandler(this.OnButtonDeleteClick);
            // 
            // m_addEntity
            // 
            this.m_addEntity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.m_addEntity.Location = new System.Drawing.Point(4, 421);
            this.m_addEntity.Name = "m_addEntity";
            this.m_addEntity.Size = new System.Drawing.Size(26, 23);
            this.m_addEntity.TabIndex = 9;
            this.m_addEntity.Text = "+";
            this.m_addEntity.UseVisualStyleBackColor = true;
            this.m_addEntity.Click += new System.EventHandler(this.OnButtonAddClick);
            // 
            // m_entityList
            // 
            this.m_entityList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                              | System.Windows.Forms.AnchorStyles.Left)
                                                                             | System.Windows.Forms.AnchorStyles.Right)));
            this.m_entityList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[]
            {
                this.Entities
            });
            this.m_entityList.Location = new System.Drawing.Point(3, 3);
            this.m_entityList.Name = "m_entityList";
            this.m_entityList.Size = new System.Drawing.Size(247, 412);
            this.m_entityList.TabIndex = 8;
            this.m_entityList.UseCompatibleStateImageBehavior = false;
            this.m_entityList.View = System.Windows.Forms.View.Details;
            // 
            // TimeAttendanceUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_query);
            this.Controls.Add(this.m_deleteEntity);
            this.Controls.Add(this.m_addEntity);
            this.Controls.Add(this.m_entityList);
            this.Name = "TimeAttendanceUI";
            this.Size = new System.Drawing.Size(253, 447);
            this.ResumeLayout(false);

        }

        #endregion
    }

    #endregion
}

