namespace AchSystem
{
    partial class frmSecc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSecc));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSECC = new System.Windows.Forms.TextBox();
            this.txtSeccID = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.ultraExpandableGroupBox1 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel1 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).BeginInit();
            this.ultraExpandableGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.ultraExpandableGroupBoxPanel2.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(16, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 98;
            this.label1.Text = "Secc Name:";
            // 
            // txtSECC
            // 
            this.txtSECC.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSECC.Location = new System.Drawing.Point(112, 41);
            this.txtSECC.MaxLength = 3;
            this.txtSECC.Name = "txtSECC";
            this.txtSECC.Size = new System.Drawing.Size(100, 20);
            this.txtSECC.TabIndex = 40;
            // 
            // txtSeccID
            // 
            this.txtSeccID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSeccID.Location = new System.Drawing.Point(112, 15);
            this.txtSeccID.MaxLength = 9;
            this.txtSeccID.Name = "txtSeccID";
            this.txtSeccID.ReadOnly = true;
            this.txtSeccID.Size = new System.Drawing.Size(100, 20);
            this.txtSeccID.TabIndex = 20;
            this.txtSeccID.TabStop = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(16, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(52, 13);
            this.label10.TabIndex = 94;
            this.label10.Text = "SECC ID:";
            // 
            // ultraExpandableGroupBox1
            // 
            this.ultraExpandableGroupBox1.Controls.Add(this.ultraExpandableGroupBoxPanel1);
            this.ultraExpandableGroupBox1.ExpandedSize = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBox1.Name = "ultraExpandableGroupBox1";
            this.ultraExpandableGroupBox1.Size = new System.Drawing.Size(200, 185);
            this.ultraExpandableGroupBox1.TabIndex = 0;
            // 
            // ultraExpandableGroupBoxPanel1
            // 
            this.ultraExpandableGroupBoxPanel1.Location = new System.Drawing.Point(0, 0);
            this.ultraExpandableGroupBoxPanel1.Name = "ultraExpandableGroupBoxPanel1";
            this.ultraExpandableGroupBoxPanel1.Size = new System.Drawing.Size(200, 100);
            this.ultraExpandableGroupBoxPanel1.TabIndex = 0;
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(380, 90);
            appearance1.FontData.BoldAsString = "True";
            appearance1.ForeColor = System.Drawing.Color.Red;
            this.ultraExpandableGroupBox2.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 25);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(511, 195);
            this.ultraExpandableGroupBox2.TabIndex = 18;
            this.ultraExpandableGroupBox2.Text = "SECC";
            this.ultraExpandableGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtSeccID);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label10);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.label1);
            this.ultraExpandableGroupBoxPanel2.Controls.Add(this.txtSECC);
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(507, 173);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // frmSecc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 220);
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Name = "frmSecc";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SECC";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSecc_FormClosed);
            this.Controls.SetChildIndex(this.ultraExpandableGroupBox2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox1)).EndInit();
            this.ultraExpandableGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.ResumeLayout(false);
            this.ultraExpandableGroupBoxPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSECC;
        private System.Windows.Forms.TextBox txtSeccID;
        private System.Windows.Forms.Label label10;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox1;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel1;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
    }
}