namespace AchSystem
{
    partial class frmStatement
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStatement));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            this.txtAchID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cboPeriod = new System.Windows.Forms.ComboBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.label22 = new System.Windows.Forms.Label();
            this.txtMerchantName = new System.Windows.Forms.TextBox();
            this.txtMerchantID = new System.Windows.Forms.TextBox();
            this.btnMerchant = new System.Windows.Forms.Button();
            this.btnExportToExcel = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.pnlMerchantInfo = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grdStatement = new Infragistics.Win.UltraWinGrid.UltraGrid();
            this.ultraGridExcelExporter1 = new Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter(this.components);
            this.pnlMain.SuspendLayout();
            this.pnlMerchantInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatement)).BeginInit();
            this.SuspendLayout();
            // 
            // txtAchID
            // 
            this.txtAchID.BackColor = System.Drawing.SystemColors.Window;
            this.txtAchID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAchID.Location = new System.Drawing.Point(83, 123);
            this.txtAchID.MaxLength = 9;
            this.txtAchID.Name = "txtAchID";
            this.txtAchID.Size = new System.Drawing.Size(100, 20);
            this.txtAchID.TabIndex = 10;
            this.txtAchID.TextChanged += new System.EventHandler(this.txtAchID_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(9, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 122;
            this.label3.Text = "ACH ID:";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(426, 121);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 40;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 126;
            this.label2.Text = "Period:";
            // 
            // cboPeriod
            // 
            this.cboPeriod.FormattingEnabled = true;
            this.cboPeriod.Location = new System.Drawing.Point(281, 123);
            this.cboPeriod.Name = "cboPeriod";
            this.cboPeriod.Size = new System.Drawing.Size(139, 21);
            this.cboPeriod.TabIndex = 30;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.label22);
            this.pnlMain.Controls.Add(this.txtMerchantName);
            this.pnlMain.Controls.Add(this.txtMerchantID);
            this.pnlMain.Controls.Add(this.btnMerchant);
            this.pnlMain.Controls.Add(this.btnExportToExcel);
            this.pnlMain.Controls.Add(this.label18);
            this.pnlMain.Controls.Add(this.pnlMerchantInfo);
            this.pnlMain.Controls.Add(this.cboPeriod);
            this.pnlMain.Controls.Add(this.label3);
            this.pnlMain.Controls.Add(this.label2);
            this.pnlMain.Controls.Add(this.txtAchID);
            this.pnlMain.Controls.Add(this.btnSearch);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(770, 168);
            this.pnlMain.TabIndex = 130;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label22.Location = new System.Drawing.Point(8, 100);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(69, 13);
            this.label22.TabIndex = 180;
            this.label22.Text = "Merchant ID:";
            // 
            // txtMerchantName
            // 
            this.txtMerchantName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantName.Location = new System.Drawing.Point(281, 97);
            this.txtMerchantName.MaxLength = 16;
            this.txtMerchantName.Name = "txtMerchantName";
            this.txtMerchantName.ReadOnly = true;
            this.txtMerchantName.Size = new System.Drawing.Size(329, 20);
            this.txtMerchantName.TabIndex = 178;
            // 
            // txtMerchantID
            // 
            this.txtMerchantID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMerchantID.Location = new System.Drawing.Point(83, 97);
            this.txtMerchantID.MaxLength = 9;
            this.txtMerchantID.Name = "txtMerchantID";
            this.txtMerchantID.ReadOnly = true;
            this.txtMerchantID.Size = new System.Drawing.Size(100, 20);
            this.txtMerchantID.TabIndex = 177;
            // 
            // btnMerchant
            // 
            this.btnMerchant.Image = global::AchSystem.Properties.Resources.search;
            this.btnMerchant.Location = new System.Drawing.Point(189, 123);
            this.btnMerchant.Name = "btnMerchant";
            this.btnMerchant.Size = new System.Drawing.Size(29, 21);
            this.btnMerchant.TabIndex = 20;
            this.btnMerchant.UseVisualStyleBackColor = true;
            this.btnMerchant.Click += new System.EventHandler(this.btnMerchant_Click);
            // 
            // btnExportToExcel
            // 
            this.btnExportToExcel.Location = new System.Drawing.Point(507, 121);
            this.btnExportToExcel.Name = "btnExportToExcel";
            this.btnExportToExcel.Size = new System.Drawing.Size(103, 23);
            this.btnExportToExcel.TabIndex = 50;
            this.btnExportToExcel.Text = "Export to Excel";
            this.btnExportToExcel.UseVisualStyleBackColor = true;
            this.btnExportToExcel.Click += new System.EventHandler(this.button1_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.Transparent;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label18.Location = new System.Drawing.Point(189, 100);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(86, 13);
            this.label18.TabIndex = 179;
            this.label18.Text = "Merchant Name:";
            // 
            // pnlMerchantInfo
            // 
            this.pnlMerchantInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlMerchantInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMerchantInfo.Controls.Add(this.panel2);
            this.pnlMerchantInfo.Controls.Add(this.pictureBox1);
            this.pnlMerchantInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMerchantInfo.Location = new System.Drawing.Point(0, 0);
            this.pnlMerchantInfo.Name = "pnlMerchantInfo";
            this.pnlMerchantInfo.Size = new System.Drawing.Size(770, 91);
            this.pnlMerchantInfo.TabIndex = 130;
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::AchSystem.Properties.Resources.OrangeExplorer;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(768, 25);
            this.panel2.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "Merchant Statement";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(10, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(63, 54);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // grdStatement
            // 
            appearance1.BackColor = System.Drawing.SystemColors.Window;
            appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption;
            this.grdStatement.DisplayLayout.Appearance = appearance1;
            this.grdStatement.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            this.grdStatement.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.False;
            appearance2.BackColor = System.Drawing.SystemColors.ActiveBorder;
            appearance2.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical;
            appearance2.BorderColor = System.Drawing.SystemColors.Window;
            this.grdStatement.DisplayLayout.GroupByBox.Appearance = appearance2;
            appearance3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdStatement.DisplayLayout.GroupByBox.BandLabelAppearance = appearance3;
            this.grdStatement.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid;
            appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight;
            appearance4.BackColor2 = System.Drawing.SystemColors.Control;
            appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.grdStatement.DisplayLayout.GroupByBox.PromptAppearance = appearance4;
            this.grdStatement.DisplayLayout.MaxColScrollRegions = 1;
            this.grdStatement.DisplayLayout.MaxRowScrollRegions = 1;
            appearance5.BackColor = System.Drawing.SystemColors.Window;
            appearance5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.grdStatement.DisplayLayout.Override.ActiveCellAppearance = appearance5;
            appearance6.BackColor = System.Drawing.SystemColors.Highlight;
            appearance6.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.grdStatement.DisplayLayout.Override.ActiveRowAppearance = appearance6;
            this.grdStatement.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted;
            this.grdStatement.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted;
            appearance7.BackColor = System.Drawing.SystemColors.Window;
            this.grdStatement.DisplayLayout.Override.CardAreaAppearance = appearance7;
            appearance8.BorderColor = System.Drawing.Color.Silver;
            appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter;
            this.grdStatement.DisplayLayout.Override.CellAppearance = appearance8;
            this.grdStatement.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            this.grdStatement.DisplayLayout.Override.CellPadding = 0;
            appearance9.BackColor = System.Drawing.SystemColors.Control;
            appearance9.BackColor2 = System.Drawing.SystemColors.ControlDark;
            appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element;
            appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal;
            appearance9.BorderColor = System.Drawing.SystemColors.Window;
            this.grdStatement.DisplayLayout.Override.GroupByRowAppearance = appearance9;
            appearance10.TextHAlignAsString = "Left";
            this.grdStatement.DisplayLayout.Override.HeaderAppearance = appearance10;
            this.grdStatement.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti;
            this.grdStatement.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand;
            appearance11.BackColor = System.Drawing.SystemColors.Window;
            appearance11.BorderColor = System.Drawing.Color.Silver;
            this.grdStatement.DisplayLayout.Override.RowAppearance = appearance11;
            this.grdStatement.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.False;
            appearance12.BackColor = System.Drawing.SystemColors.ControlLight;
            this.grdStatement.DisplayLayout.Override.TemplateAddRowAppearance = appearance12;
            this.grdStatement.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill;
            this.grdStatement.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate;
            this.grdStatement.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy;
            this.grdStatement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdStatement.Location = new System.Drawing.Point(0, 168);
            this.grdStatement.Name = "grdStatement";
            this.grdStatement.Size = new System.Drawing.Size(770, 360);
            this.grdStatement.TabIndex = 60;
            this.grdStatement.InitializeRow += new Infragistics.Win.UltraWinGrid.InitializeRowEventHandler(this.grdStatement_InitializeRow);
            this.grdStatement.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdStatement_InitializeLayout);
            // 
            // frmStatement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 528);
            this.Controls.Add(this.grdStatement);
            this.Controls.Add(this.pnlMain);
            this.KeyPreview = true;
            this.Name = "frmStatement";
            this.Text = "Statement";
            this.Load += new System.EventHandler(this.frmStatement_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStatement_KeyDown);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlMerchantInfo.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdStatement)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtAchID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPeriod;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlMerchantInfo;
        private System.Windows.Forms.Panel panel2;
        private Infragistics.Win.UltraWinGrid.UltraGrid grdStatement;
        private System.Windows.Forms.Button btnExportToExcel;
        private Infragistics.Win.UltraWinGrid.ExcelExport.UltraGridExcelExporter ultraGridExcelExporter1;
        private System.Windows.Forms.Button btnMerchant;
        private System.Windows.Forms.TextBox txtMerchantName;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtMerchantID;
        private System.Windows.Forms.Label label18;
    }
}