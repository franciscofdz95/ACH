namespace AchSystem
{
    partial class frmTest
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
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("tbrTools");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool2");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool3");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool4");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool1");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool2");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool3");
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("ButtonTool4");
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTest));
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            this.tbrTop = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this._frmTest_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmTest_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmTest_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmTest_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.ultraExpandableGroupBox2 = new Infragistics.Win.Misc.UltraExpandableGroupBox();
            this.ultraExpandableGroupBoxPanel2 = new Infragistics.Win.Misc.UltraExpandableGroupBoxPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).BeginInit();
            this.ultraExpandableGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbrTop
            // 
            this.tbrTop.DesignerFlags = 1;
            this.tbrTop.DockWithinContainer = this;
            this.tbrTop.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.tbrTop.ImageListSmall = this.imgList;
            this.tbrTop.ShowFullMenusDelay = 500;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3,
            buttonTool4});
            ultraToolbar1.Text = "tbrTools";
            this.tbrTop.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance2.Image = 2;
            buttonTool5.SharedPropsInternal.AppearancesSmall.Appearance = appearance2;
            buttonTool5.SharedPropsInternal.Caption = "ButtonTool1";
            buttonTool5.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool6.SharedPropsInternal.AppearancesSmall.Appearance = appearance3;
            buttonTool6.SharedPropsInternal.Caption = "ButtonTool2";
            appearance4.Image = ((object)(resources.GetObject("appearance4.Image")));
            buttonTool7.SharedPropsInternal.AppearancesSmall.Appearance = appearance4;
            buttonTool7.SharedPropsInternal.Caption = "ButtonTool3";
            appearance5.Image = ((object)(resources.GetObject("appearance5.Image")));
            buttonTool8.SharedPropsInternal.AppearancesSmall.Appearance = appearance5;
            buttonTool8.SharedPropsInternal.Caption = "ButtonTool4";
            this.tbrTop.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8});
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
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
            // _frmTest_Toolbars_Dock_Area_Left
            // 
            this._frmTest_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmTest_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._frmTest_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmTest_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmTest_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 27);
            this._frmTest_Toolbars_Dock_Area_Left.Name = "_frmTest_Toolbars_Dock_Area_Left";
            this._frmTest_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 434);
            this._frmTest_Toolbars_Dock_Area_Left.ToolbarsManager = this.tbrTop;
            // 
            // _frmTest_Toolbars_Dock_Area_Right
            // 
            this._frmTest_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmTest_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._frmTest_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmTest_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmTest_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(719, 27);
            this._frmTest_Toolbars_Dock_Area_Right.Name = "_frmTest_Toolbars_Dock_Area_Right";
            this._frmTest_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 434);
            this._frmTest_Toolbars_Dock_Area_Right.ToolbarsManager = this.tbrTop;
            // 
            // _frmTest_Toolbars_Dock_Area_Top
            // 
            this._frmTest_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmTest_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._frmTest_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmTest_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmTest_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmTest_Toolbars_Dock_Area_Top.Name = "_frmTest_Toolbars_Dock_Area_Top";
            this._frmTest_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(719, 27);
            this._frmTest_Toolbars_Dock_Area_Top.ToolbarsManager = this.tbrTop;
            // 
            // _frmTest_Toolbars_Dock_Area_Bottom
            // 
            this._frmTest_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmTest_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._frmTest_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmTest_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmTest_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 461);
            this._frmTest_Toolbars_Dock_Area_Bottom.Name = "_frmTest_Toolbars_Dock_Area_Bottom";
            this._frmTest_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(719, 0);
            this._frmTest_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tbrTop;
            // 
            // ultraExpandableGroupBox2
            // 
            this.ultraExpandableGroupBox2.Controls.Add(this.ultraExpandableGroupBoxPanel2);
            this.ultraExpandableGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ultraExpandableGroupBox2.ExpandedSize = new System.Drawing.Size(470, 173);
            appearance1.FontData.BoldAsString = "True";
            this.ultraExpandableGroupBox2.HeaderAppearance = appearance1;
            this.ultraExpandableGroupBox2.Location = new System.Drawing.Point(0, 27);
            this.ultraExpandableGroupBox2.Name = "ultraExpandableGroupBox2";
            this.ultraExpandableGroupBox2.Size = new System.Drawing.Size(719, 220);
            this.ultraExpandableGroupBox2.TabIndex = 22;
            this.ultraExpandableGroupBox2.Text = "Parameters";
            this.ultraExpandableGroupBox2.ViewStyle = Infragistics.Win.Misc.GroupBoxViewStyle.VisualStudio2005;
            // 
            // ultraExpandableGroupBoxPanel2
            // 
            this.ultraExpandableGroupBoxPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraExpandableGroupBoxPanel2.Location = new System.Drawing.Point(2, 20);
            this.ultraExpandableGroupBoxPanel2.Name = "ultraExpandableGroupBoxPanel2";
            this.ultraExpandableGroupBoxPanel2.Size = new System.Drawing.Size(715, 198);
            this.ultraExpandableGroupBoxPanel2.TabIndex = 0;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 461);
            this.Controls.Add(this.ultraExpandableGroupBox2);
            this.Controls.Add(this._frmTest_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmTest_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmTest_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._frmTest_Toolbars_Dock_Area_Bottom);
            this.Name = "frmTest";
            this.Text = "frmTest";
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ultraExpandableGroupBox2)).EndInit();
            this.ultraExpandableGroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tbrTop;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmTest_Toolbars_Dock_Area_Left;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmTest_Toolbars_Dock_Area_Right;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmTest_Toolbars_Dock_Area_Top;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmTest_Toolbars_Dock_Area_Bottom;
        public System.Windows.Forms.ImageList imgList;
        private Infragistics.Win.Misc.UltraExpandableGroupBox ultraExpandableGroupBox2;
        private Infragistics.Win.Misc.UltraExpandableGroupBoxPanel ultraExpandableGroupBoxPanel2;
    }
}