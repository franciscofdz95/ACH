namespace AchSystem
{
    partial class frmPicture
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("UltraToolbar1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Print");
            this.tbrTop = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmPicture_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmPicture_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmPicture_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmPicture_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.pic = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pdMain = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbrTop
            // 
            this.tbrTop.DesignerFlags = 1;
            this.tbrTop.DockWithinContainer = this;
            this.tbrTop.ShowFullMenusDelay = 500;
            this.tbrTop.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.VisualStudio2005;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.Text = "tbrTools";
            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1});
            this.tbrTop.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            buttonTool2.SharedProps.Caption = "Print";
            buttonTool2.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.tbrTop.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2});
            this.tbrTop.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.tbrTop_ToolClick);
            // 
            // _frmPicture_Toolbars_Dock_Area_Left
            // 
            this._frmPicture_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPicture_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._frmPicture_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmPicture_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPicture_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 52);
            this._frmPicture_Toolbars_Dock_Area_Left.Name = "_frmPicture_Toolbars_Dock_Area_Left";
            this._frmPicture_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 493);
            this._frmPicture_Toolbars_Dock_Area_Left.ToolbarsManager = this.tbrTop;
            // 
            // _frmPicture_Toolbars_Dock_Area_Right
            // 
            this._frmPicture_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPicture_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._frmPicture_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmPicture_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPicture_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(871, 52);
            this._frmPicture_Toolbars_Dock_Area_Right.Name = "_frmPicture_Toolbars_Dock_Area_Right";
            this._frmPicture_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 493);
            this._frmPicture_Toolbars_Dock_Area_Right.ToolbarsManager = this.tbrTop;
            // 
            // _frmPicture_Toolbars_Dock_Area_Top
            // 
            this._frmPicture_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPicture_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._frmPicture_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmPicture_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPicture_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmPicture_Toolbars_Dock_Area_Top.Name = "_frmPicture_Toolbars_Dock_Area_Top";
            this._frmPicture_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(871, 52);
            this._frmPicture_Toolbars_Dock_Area_Top.ToolbarsManager = this.tbrTop;
            // 
            // _frmPicture_Toolbars_Dock_Area_Bottom
            // 
            this._frmPicture_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmPicture_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._frmPicture_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmPicture_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmPicture_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 545);
            this._frmPicture_Toolbars_Dock_Area_Bottom.Name = "_frmPicture_Toolbars_Dock_Area_Bottom";
            this._frmPicture_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(871, 0);
            this._frmPicture_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tbrTop;
            // 
            // pic
            // 
            this.pic.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pic.Location = new System.Drawing.Point(0, 0);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(850, 1100);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pic.TabIndex = 4;
            this.pic.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(871, 493);
            this.panel1.TabIndex = 9;
            // 
            // pdMain
            // 
            this.pdMain.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pdMain_PrintPage);
            // 
            // frmPicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(871, 545);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._frmPicture_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmPicture_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmPicture_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._frmPicture_Toolbars_Dock_Area_Bottom);
            this.Name = "frmPicture";
            this.Text = "Image";
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tbrTop;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmPicture_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmPicture_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmPicture_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmPicture_Toolbars_Dock_Area_Bottom;
        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Panel panel1;
        private System.Drawing.Printing.PrintDocument pdMain;

    }
}