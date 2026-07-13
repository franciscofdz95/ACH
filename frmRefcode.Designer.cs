namespace AchSystem
{
    partial class frmRefcode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRefcode));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtRefcode = new System.Windows.Forms.TextBox();
            this.txtRefcodeID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.chkIncludeInBalance = new System.Windows.Forms.CheckBox();
            this.chkIncludeInAvailableBalance = new System.Windows.Forms.CheckBox();
            this.chkIsFeeCode = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            this.ultraExpandableGroupBoxPanel1.SuspendLayout();
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 76;
            this.label2.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(104, 61);
            this.txtDescription.MaxLength = 50;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(366, 20);
            this.txtDescription.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(8, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(51, 13);
            this.label10.TabIndex = 94;
            this.label10.Text = "Refcode:";
            // 
            // txtRefcode
            // 
            this.txtRefcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefcode.Location = new System.Drawing.Point(104, 35);
            this.txtRefcode.MaxLength = 9;
            this.txtRefcode.Name = "txtRefcode";
            this.txtRefcode.ReadOnly = true;
            this.txtRefcode.Size = new System.Drawing.Size(100, 20);
            this.txtRefcode.TabIndex = 20;
            this.txtRefcode.TabStop = false;
            // 
            // txtRefcodeID
            // 
            this.txtRefcodeID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefcodeID.Location = new System.Drawing.Point(104, 9);
            this.txtRefcodeID.MaxLength = 9;
            this.txtRefcodeID.Name = "txtRefcodeID";
            this.txtRefcodeID.ReadOnly = true;
            this.txtRefcodeID.Size = new System.Drawing.Size(100, 20);
            this.txtRefcodeID.TabIndex = 95;
            this.txtRefcodeID.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(8, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 96;
            this.label1.Text = "Refcode ID:";
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(380, 90);
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraExpandableGroupBox1.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 25);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(484, 217);
            this.ultraExpandableGroupBox1.TabIndex = 16;
            this.ultraExpandableGroupBox1.Text = "Journal Refcode";
            this.ultraExpandableGroupBox1.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkIsFeeCode);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkIncludeInAvailableBalance);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.chkIncludeInBalance);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtRefcodeID);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtDescription);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label1);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label10);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.txtRefcode);
            this.ultraExpandableGroupBoxPanel1.Controls.Add(this.label2);
            this.ultraExpandableGroupBoxPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(480, 195);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // chkIncludeInBalance
            // 
            this.chkIncludeInBalance.AutoSize = true;
            this.chkIncludeInBalance.BackColor = System.Drawing.Color.Transparent;
            this.chkIncludeInBalance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeInBalance.Location = new System.Drawing.Point(349, 122);
            this.chkIncludeInBalance.Name = "chkIncludeInBalance";
            this.chkIncludeInBalance.Size = new System.Drawing.Size(121, 17);
            this.chkIncludeInBalance.TabIndex = 97;
            this.chkIncludeInBalance.Text = "Include In Balance?";
            this.chkIncludeInBalance.UseVisualStyleBackColor = false;
            // 
            // chkIncludeInAvailableBalance
            // 
            this.chkIncludeInAvailableBalance.AutoSize = true;
            this.chkIncludeInAvailableBalance.BackColor = System.Drawing.Color.Transparent;
            this.chkIncludeInAvailableBalance.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIncludeInAvailableBalance.Location = new System.Drawing.Point(303, 145);
            this.chkIncludeInAvailableBalance.Name = "chkIncludeInAvailableBalance";
            this.chkIncludeInAvailableBalance.Size = new System.Drawing.Size(167, 17);
            this.chkIncludeInAvailableBalance.TabIndex = 98;
            this.chkIncludeInAvailableBalance.Text = "Include In Available Balance?";
            this.chkIncludeInAvailableBalance.UseVisualStyleBackColor = false;
            // 
            // chkIsFeeCode
            // 
            this.chkIsFeeCode.AutoSize = true;
            this.chkIsFeeCode.BackColor = System.Drawing.Color.Transparent;
            this.chkIsFeeCode.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsFeeCode.Location = new System.Drawing.Point(381, 168);
            this.chkIsFeeCode.Name = "chkIsFeeCode";
            this.chkIsFeeCode.Size = new System.Drawing.Size(89, 17);
            this.chkIsFeeCode.TabIndex = 99;
            this.chkIsFeeCode.Text = "Is Fee Code?";
            this.chkIsFeeCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkIsFeeCode.UseVisualStyleBackColor = false;
            // 
            // frmRefcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 242);
            this.Controls.Add(this.ultraExpandableGroupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRefcode";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Journal Refcode";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRefcode_FormClosed);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtRefcode;
        private System.Windows.Forms.TextBox txtRefcodeID;
        private System.Windows.Forms.Label label1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private System.Windows.Forms.CheckBox chkIncludeInAvailableBalance;
        private System.Windows.Forms.CheckBox chkIncludeInBalance;
        private System.Windows.Forms.CheckBox chkIsFeeCode;
    }
}