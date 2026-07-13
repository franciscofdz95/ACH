namespace AchSystem
{
    partial class frmUpdateTransaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdateTransaction));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtNextProcessTime = new System.Windows.Forms.DateTimePicker();
            this.txtNextProcessDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.imgList.Images.SetKeyName(13, "money2.png");
            this.imgList.Images.SetKeyName(14, "copy.png");
            this.imgList.Images.SetKeyName(15, "data_into.png");
            this.imgList.Images.SetKeyName(16, "transform.png");
            // 
            // tbrTop
            // 
            this.tbrTop.MenuSettings.ForceSerialization = true;
            this.tbrTop.ToolbarSettings.ForceSerialization = true;
            this.tbrTop.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.tbrTop_ToolClick);
            // 
            // tabMain
            // 
            this.tabMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMain.Controls.Add(this.tabPage1);
            this.tabMain.Controls.Add(this.tabPage2);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 26);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(372, 195);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.cboStatus);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(364, 166);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Change Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "New Status:";
            // 
            // cboStatus
            // 
            this.cboStatus.FormattingEnabled = true;
            this.cboStatus.Location = new System.Drawing.Point(95, 70);
            this.cboStatus.Name = "cboStatus";
            this.cboStatus.Size = new System.Drawing.Size(136, 21);
            this.cboStatus.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.tabPage2.Controls.Add(this.txtNextProcessTime);
            this.tabPage2.Controls.Add(this.txtNextProcessDate);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(364, 166);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Change Process Date";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtNextProcessTime
            // 
            this.txtNextProcessTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.txtNextProcessTime.Location = new System.Drawing.Point(229, 73);
            this.txtNextProcessTime.Name = "txtNextProcessTime";
            this.txtNextProcessTime.ShowUpDown = true;
            this.txtNextProcessTime.Size = new System.Drawing.Size(100, 20);
            this.txtNextProcessTime.TabIndex = 22;
            // 
            // txtNextProcessDate
            // 
            this.txtNextProcessDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtNextProcessDate.Location = new System.Drawing.Point(123, 73);
            this.txtNextProcessDate.Name = "txtNextProcessDate";
            this.txtNextProcessDate.Size = new System.Drawing.Size(100, 20);
            this.txtNextProcessDate.TabIndex = 21;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(99, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "New Process Date:";
            // 
            // frmUpdateTransaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 221);
            this.Controls.Add(this.tabMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateTransaction";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Transaction(s)";
            this.Load += new System.EventHandler(this.frmUpdateTransaction_Load);
            this.Controls.SetChildIndex(this.tabMain, 0);
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboStatus;
        private System.Windows.Forms.DateTimePicker txtNextProcessDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker txtNextProcessTime;
    }
}