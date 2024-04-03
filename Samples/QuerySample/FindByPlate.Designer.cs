namespace QuerySample
{
    partial class FindByPlate
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
            this.m_results = new System.Windows.Forms.ListView();
            this.m_searchDialog = new System.Windows.Forms.SplitContainer();
            this.m_search = new System.Windows.Forms.Button();
            this.m_plate = new System.Windows.Forms.TextBox();
            this.m_plateLbl = new System.Windows.Forms.Label();
            this.m_okayAndCancelButtons = new System.Windows.Forms.Panel();
            this.m_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.m_searchDialog)).BeginInit();
            this.m_searchDialog.Panel1.SuspendLayout();
            this.m_searchDialog.Panel2.SuspendLayout();
            this.m_searchDialog.SuspendLayout();
            this.m_okayAndCancelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_results
            // 
            this.m_results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_results.FullRowSelect = true;
            this.m_results.GridLines = true;
            this.m_results.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.m_results.HideSelection = false;
            this.m_results.Location = new System.Drawing.Point(0, 0);
            this.m_results.Name = "m_results";
            this.m_results.Size = new System.Drawing.Size(505, 344);
            this.m_results.TabIndex = 1;
            this.m_results.UseCompatibleStateImageBehavior = false;
            this.m_results.View = System.Windows.Forms.View.Details;
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
            this.m_searchDialog.Panel1.Controls.Add(this.m_plate);
            this.m_searchDialog.Panel1.Controls.Add(this.m_plateLbl);
            // 
            // m_searchDialog.Panel2
            // 
            this.m_searchDialog.Panel2.Controls.Add(this.m_results);
            this.m_searchDialog.Panel2.Controls.Add(this.m_okayAndCancelButtons);
            this.m_searchDialog.Size = new System.Drawing.Size(735, 395);
            this.m_searchDialog.SplitterDistance = 226;
            this.m_searchDialog.TabIndex = 1;
            // 
            // m_search
            // 
            this.m_search.Location = new System.Drawing.Point(12, 78);
            this.m_search.Name = "m_search";
            this.m_search.Size = new System.Drawing.Size(128, 40);
            this.m_search.TabIndex = 14;
            this.m_search.Text = "Find";
            this.m_search.UseVisualStyleBackColor = true;
            this.m_search.Click += new System.EventHandler(this.m_search_Click);
            // 
            // m_plate
            // 
            this.m_plate.Location = new System.Drawing.Point(11, 39);
            this.m_plate.Name = "m_plate";
            this.m_plate.Size = new System.Drawing.Size(136, 20);
            this.m_plate.TabIndex = 9;
            // 
            // m_plateLbl
            // 
            this.m_plateLbl.Location = new System.Drawing.Point(8, 16);
            this.m_plateLbl.Name = "m_plateLbl";
            this.m_plateLbl.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.m_plateLbl.Size = new System.Drawing.Size(139, 20);
            this.m_plateLbl.TabIndex = 8;
            this.m_plateLbl.Text = "Plate (full or partial)";
            this.m_plateLbl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_okayAndCancelButtons
            // 
            this.m_okayAndCancelButtons.Controls.Add(this.m_close);
            this.m_okayAndCancelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_okayAndCancelButtons.Location = new System.Drawing.Point(0, 344);
            this.m_okayAndCancelButtons.Name = "m_okayAndCancelButtons";
            this.m_okayAndCancelButtons.Size = new System.Drawing.Size(505, 51);
            this.m_okayAndCancelButtons.TabIndex = 0;
            // 
            // m_close
            // 
            this.m_close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_close.Location = new System.Drawing.Point(400, 8);
            this.m_close.Name = "m_close";
            this.m_close.Size = new System.Drawing.Size(98, 32);
            this.m_close.TabIndex = 15;
            this.m_close.Text = "&Close";
            this.m_close.UseVisualStyleBackColor = true;
            // 
            // FindByPlate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(735, 395);
            this.Controls.Add(this.m_searchDialog);
            this.Name = "FindByPlate";
            this.Text = "FindByPlate";
            this.m_searchDialog.Panel1.ResumeLayout(false);
            this.m_searchDialog.Panel1.PerformLayout();
            this.m_searchDialog.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_searchDialog)).EndInit();
            this.m_searchDialog.ResumeLayout(false);
            this.m_okayAndCancelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView m_results;
        private System.Windows.Forms.SplitContainer m_searchDialog;
        private System.Windows.Forms.Button m_search;
        private System.Windows.Forms.TextBox m_plate;
        private System.Windows.Forms.Label m_plateLbl;
        private System.Windows.Forms.Panel m_okayAndCancelButtons;
        private System.Windows.Forms.Button m_close;
    }
}