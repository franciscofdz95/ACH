namespace ACH2005
{
    partial class frmPurgeBatch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPurgeBatch));
            this.tbrTop = new System.Windows.Forms.ToolStrip();
            this.tbrSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tbrDeleteBatch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBatchDate = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grdSearch = new System.Windows.Forms.DataGridView();
            this.tbrTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrTop
            // 
            this.tbrTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbrSearch,
            this.toolStripSeparator2,
            this.tbrClear,
            this.toolStripSeparator1,
            this.tbrDeleteBatch,
            this.toolStripSeparator3});
            this.tbrTop.Location = new System.Drawing.Point(0, 0);
            this.tbrTop.Name = "tbrTop";
            this.tbrTop.Size = new System.Drawing.Size(811, 25);
            this.tbrTop.TabIndex = 0;
            this.tbrTop.Text = "toolStrip1";
            this.tbrTop.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tbrTop_ItemClicked);
            // 
            // tbrSearch
            // 
            this.tbrSearch.Image = ((System.Drawing.Image)(resources.GetObject("tbrSearch.Image")));
            this.tbrSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrSearch.Name = "tbrSearch";
            this.tbrSearch.Size = new System.Drawing.Size(60, 22);
            this.tbrSearch.Text = "Search";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrClear
            // 
            this.tbrClear.Image = ((System.Drawing.Image)(resources.GetObject("tbrClear.Image")));
            this.tbrClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrClear.Name = "tbrClear";
            this.tbrClear.Size = new System.Drawing.Size(52, 22);
            this.tbrClear.Text = "Clear";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tbrDeleteBatch
            // 
            this.tbrDeleteBatch.Image = ((System.Drawing.Image)(resources.GetObject("tbrDeleteBatch.Image")));
            this.tbrDeleteBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbrDeleteBatch.Name = "tbrDeleteBatch";
            this.tbrDeleteBatch.Size = new System.Drawing.Size(88, 22);
            this.tbrDeleteBatch.Text = "Delete Batch";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // txtAchID
            // 
            this.txtAchID.BackColor = System.Drawing.SystemColors.Window;
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(299, 27);
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(247, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "ACH ID:";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtBatchDate);
            this.groupBox1.Controls.Add(this.txtAchID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtMerchantID);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(496, 85);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Required Search Fields";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(26, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Merchant ID:";
            // 
            // txtBatchDate
            // 
            this.txtBatchDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatchDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtBatchDate.Location = new System.Drawing.Point(127, 52);
            this.txtBatchDate.Name = "txtBatchDate";
            this.txtBatchDate.Size = new System.Drawing.Size(100, 20);
            this.txtBatchDate.TabIndex = 40;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Batch Date:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.BackColor = System.Drawing.SystemColors.Window;
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(127, 28);
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 20;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 125);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(811, 3);
            this.splitter1.TabIndex = 14;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(811, 100);
            this.panel1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.panel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 633);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(811, 31);
            this.panel4.TabIndex = 113;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Unable to delete";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(122, 7);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(15, 15);
            this.panel3.TabIndex = 46;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 45;
            this.label1.Text = "Able to delete";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LemonChiffon;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(5, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(15, 15);
            this.panel2.TabIndex = 44;
            // 
            // grdSearch
            // 
            this.grdSearch.AllowUserToAddRows = false;
            this.grdSearch.AllowUserToDeleteRows = false;
            this.grdSearch.AllowUserToResizeRows = false;
            this.grdSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.grdSearch.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grdSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdSearch.Location = new System.Drawing.Point(0, 128);
            this.grdSearch.MultiSelect = false;
            this.grdSearch.Name = "grdSearch";
            this.grdSearch.ReadOnly = true;
            this.grdSearch.RowHeadersVisible = false;
            this.grdSearch.RowTemplate.Height = 19;
            this.grdSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSearch.Size = new System.Drawing.Size(811, 505);
            this.grdSearch.TabIndex = 114;
            this.grdSearch.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.grdSearch_RowPrePaint);
            // 
            // frmPurgeBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 664);
            this.Controls.Add(this.grdSearch);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tbrTop);
            this.KeyPreview = true;
            this.Name = "frmPurgeBatch";
            this.Text = "Purge Batch";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmPurgeBatch_KeyDown);
            this.Load += new System.EventHandler(this.frmChangeTransStatus_Load);
            this.tbrTop.ResumeLayout(false);
            this.tbrTop.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tbrTop;
        private System.Windows.Forms.ToolStripButton tbrSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tbrClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tbrDeleteBatch;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker txtBatchDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView grdSearch;

    }
}