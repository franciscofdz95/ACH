namespace AchSystem
{
    partial class frmBatchDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBatchDetail));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.txtPostedDate = new System.Windows.Forms.MaskedTextBox();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBatchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBox4 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel4 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cboSource = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cboBankID = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtTransID = new System.Windows.Forms.TextBox();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox4)).BeginInit();
            this.ultraExpandableGroupBox4.SuspendLayout();
            this.ultraExpandableGroupBoxPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.Images.SetKeyName(0, "filesave.gif");
            this.imgList.Images.SetKeyName(1, "delete.gif");
            this.imgList.Images.SetKeyName(2, "new.gif");
            this.imgList.Images.SetKeyName(3, "search.gif");
            this.imgList.Images.SetKeyName(4, "undo.gif");
            this.imgList.Images.SetKeyName(5, "greencheck3.gif");
            this.imgList.Images.SetKeyName(6, "Exit.gif");
            this.imgList.Images.SetKeyName(7, "close.gif");
            this.imgList.Images.SetKeyName(8, "ckbx_grn.gif");
            this.imgList.Images.SetKeyName(9, "ig_tbarHistory2.gif");
            this.imgList.Images.SetKeyName(10, "status.gif");
            this.imgList.Images.SetKeyName(11, "Credit_.GIF");
            this.imgList.Images.SetKeyName(12, "Road2.gif");
            // 
            // txtPostedDate
            // 
            this.txtPostedDate.Enabled = false;
            this.txtPostedDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPostedDate.Location = new System.Drawing.Point(103, 224);
            this.txtPostedDate.Mask = "00/00/0000 90:00:00";
            this.txtPostedDate.Name = "txtPostedDate";
            this.txtPostedDate.Size = new System.Drawing.Size(161, 20);
            this.txtPostedDate.TabIndex = 50;
            // 
            // txtAchID
            // 
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(104, 10);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.ReadOnly = true;
            this.txtAchID.Size = new System.Drawing.Size(133, 20);
            this.txtAchID.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Batch ID:";
            // 
            // txtBatchID
            // 
            this.txtBatchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBatchID.Location = new System.Drawing.Point(103, 198);
            this.txtBatchID.MaxLength = 9;
            this.txtBatchID.Name = "txtBatchID";
            this.txtBatchID.ReadOnly = true;
            this.txtBatchID.Size = new System.Drawing.Size(113, 20);
            this.txtBatchID.TabIndex = 40;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(10, 227);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Posted Date:";
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(103, 36);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.ReadOnly = true;
            this.txtMerchantID.Size = new System.Drawing.Size(134, 20);
            this.txtMerchantID.TabIndex = 30;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(10, 42);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(69, 13);
            this.label18.TabIndex = 128;
            this.label18.Text = "Merchant ID:";
            // 
            // ultraExpandableGroupBox4
            // 
            this.ultraExpandableGroupBox4.Controls.Add(this.ultraExpandableGroupBoxPanel4);
            this.ultraExpandableGroupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox4.ExpandedSize = new System.Drawing.Size(383, 192);
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraExpandableGroupBox4.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox4.Location = new System.Drawing.Point(0, 25);
            this.ultraExpandableGroupBox4.Name = "ultraExpandableGroupBox4";
            this.ultraExpandableGroupBox4.Size = new System.Drawing.Size(401, 296);
            this.ultraExpandableGroupBox4.TabIndex = 20;
            this.ultraExpandableGroupBox4.Text = "Required Fields";
            this.ultraExpandableGroupBox4.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel4
            // 
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.linkLabel2);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label10);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.textBox1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.checkBox1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.cboSource);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label9);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.cboBankID);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label8);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtTransID);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtMerchantName);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label6);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.linkLabel1);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtBatchID);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtAchID);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtMerchantID);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label18);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label3);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.label2);
            this.ultraExpandableGroupBoxPanel4.Controls.Add(this.txtPostedDate);
            this.ultraExpandableGroupBoxPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel4.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel4.Name = "ultraExpandableGroupBoxPanel4";
            this.ultraExpandableGroupBoxPanel4.Size = new System.Drawing.Size(397, 274);
            this.ultraExpandableGroupBoxPanel4.TabIndex = 0;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(11, 94);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(60, 13);
            this.linkLabel2.TabIndex = 202;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Trans ID:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(11, 175);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 13);
            this.label10.TabIndex = 200;
            this.label10.Text = "Job ID:";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(103, 168);
            this.textBox1.MaxLength = 2;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(113, 20);
            this.textBox1.TabIndex = 201;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(266, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(70, 17);
            this.checkBox1.TabIndex = 199;
            this.checkBox1.Text = "Process?";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // cboSource
            // 
            this.cboSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSource.Enabled = false;
            this.cboSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSource.FormattingEnabled = true;
            this.cboSource.Location = new System.Drawing.Point(103, 141);
            this.cboSource.Name = "cboSource";
            this.cboSource.Size = new System.Drawing.Size(161, 21);
            this.cboSource.TabIndex = 197;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(10, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(44, 13);
            this.label9.TabIndex = 198;
            this.label9.Text = "Source:";
            // 
            // cboBankID
            // 
            this.cboBankID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBankID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboBankID.FormattingEnabled = true;
            this.cboBankID.Location = new System.Drawing.Point(103, 114);
            this.cboBankID.Name = "cboBankID";
            this.cboBankID.Size = new System.Drawing.Size(161, 21);
            this.cboBankID.TabIndex = 195;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(10, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 13);
            this.label8.TabIndex = 196;
            this.label8.Text = "Bank ID:";
            // 
            // txtTransID
            // 
            this.txtTransID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTransID.Location = new System.Drawing.Point(103, 88);
            this.txtTransID.MaxLength = 10;
            this.txtTransID.Name = "txtTransID";
            this.txtTransID.ReadOnly = true;
            this.txtTransID.Size = new System.Drawing.Size(161, 20);
            this.txtTransID.TabIndex = 193;
            this.txtTransID.TabStop = false;
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(103, 62);
            this.txtMerchantName.MaxLength = 16;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.ReadOnly = true;
            this.txtMerchantName.Size = new System.Drawing.Size(233, 20);
            this.txtMerchantName.TabIndex = 191;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(11, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 192;
            this.label6.Text = "Merchant Name:";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(11, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(50, 13);
            this.linkLabel1.TabIndex = 5;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Ach ID:";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // frmBatchDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 321);
            this.Controls.Add(this.ultraExpandableGroupBox4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBatchDetail";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Batch Detail";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBatch_FormClosed);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox4, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox4)).EndInit();
            this.ultraExpandableGroupBox4.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel4.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox txtPostedDate;
        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBatchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.Label label18;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox4;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel4;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTransID;
        private System.Windows.Forms.ComboBox cboBankID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.LinkLabel linkLabel2;
    }
}