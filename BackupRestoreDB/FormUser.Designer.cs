namespace BackupRestoreDB
{
    partial class BackupRestoreDB
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
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dgvDatabases = new System.Windows.Forms.DataGridView();
            this.dgvBackups = new System.Windows.Forms.DataGridView();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDBName = new System.Windows.Forms.Label();
            this.lblBackupCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatabases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(1082, 25);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvDatabases);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvBackups);
            this.splitContainer1.Panel2.Controls.Add(this.pnlBottom);
            this.splitContainer1.Panel2.Controls.Add(this.pnlTop);
            this.splitContainer1.Size = new System.Drawing.Size(1082, 478);
            this.splitContainer1.SplitterDistance = 252;
            this.splitContainer1.TabIndex = 1;
            // 
            // dgvDatabases
            // 
            this.dgvDatabases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatabases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDatabases.Location = new System.Drawing.Point(0, 0);
            this.dgvDatabases.Name = "dgvDatabases";
            this.dgvDatabases.RowTemplate.Height = 24;
            this.dgvDatabases.Size = new System.Drawing.Size(252, 478);
            this.dgvDatabases.TabIndex = 0;
            this.dgvDatabases.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDatabases_CellContentClick);
            // 
            // dgvBackups
            // 
            this.dgvBackups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBackups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBackups.Location = new System.Drawing.Point(0, 51);
            this.dgvBackups.Name = "dgvBackups";
            this.dgvBackups.RowTemplate.Height = 24;
            this.dgvBackups.Size = new System.Drawing.Size(826, 327);
            this.dgvBackups.TabIndex = 2;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 378);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(826, 100);
            this.pnlBottom.TabIndex = 1;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblDBName);
            this.pnlTop.Controls.Add(this.lblBackupCount);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(826, 51);
            this.pnlTop.TabIndex = 0;
            // 
            // lblDBName
            // 
            this.lblDBName.AutoSize = true;
            this.lblDBName.Location = new System.Drawing.Point(146, 16);
            this.lblDBName.Name = "lblDBName";
            this.lblDBName.Size = new System.Drawing.Size(20, 17);
            this.lblDBName.TabIndex = 3;
            this.lblDBName.Text = "...";
            // 
            // lblBackupCount
            // 
            this.lblBackupCount.AutoSize = true;
            this.lblBackupCount.Location = new System.Drawing.Point(312, 19);
            this.lblBackupCount.Name = "lblBackupCount";
            this.lblBackupCount.Size = new System.Drawing.Size(20, 17);
            this.lblBackupCount.TabIndex = 2;
            this.lblBackupCount.Text = "...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(229, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 17);
            this.label2.TabIndex = 1;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tên cơ sở dữ liệu";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // BackupRestoreDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 503);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripMain);
            this.Name = "BackupRestoreDB";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sao lưu - Phục hồi cơ sở dữ liệu";
            // this.Load += new System.EventHandler(this.BackupRestoreDB_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatabases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBackups)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dgvDatabases;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvBackups;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblDBName;
        private System.Windows.Forms.Label lblBackupCount;
    }
}

