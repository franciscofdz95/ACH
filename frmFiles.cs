using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Data;

namespace ACH2005
{
	/// <summary>
	/// Summary description for frmFiles.
	/// </summary>
	public class frmFiles : System.Windows.Forms.Form
	{
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tbrTop;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmProcessingEngine_Toolbars_Dock_Area_Top;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmProcessingEngine_Toolbars_Dock_Area_Bottom;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmProcessingEngine_Toolbars_Dock_Area_Left;
		private Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea _frmProcessingEngine_Toolbars_Dock_Area_Right;
		private Infragistics.Win.UltraWinGrid.UltraGrid grdFiles;
		private System.ComponentModel.IContainer components;

		private DataTable m_tbl;
		private string m_SearchDirectory;

		public frmFiles(string SearchDirectory)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			m_SearchDirectory = SearchDirectory;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
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
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("View Files");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear List");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export List to Excel");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Export List to Excel");
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFiles));
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("View Files");
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Clear List");
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            this.tbrTop = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmProcessingEngine_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmProcessingEngine_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmProcessingEngine_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.grdFiles = new Infragistics.Win.UltraWinGrid.UltraGrid();
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // tbrTop
            // 
            this.tbrTop.DockWithinContainer = this;
            this.tbrTop.MdiMergeable = false;
            this.tbrTop.ShowFullMenusDelay = 500;
            this.tbrTop.ShowQuickCustomizeButton = false;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.Text = "tbrTools";
            buttonTool1.InstanceProps.IsFirstInGroup = true;
            buttonTool2.InstanceProps.IsFirstInGroup = true;
            buttonTool3.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool1,
            buttonTool2,
            buttonTool3});
            this.tbrTop.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance1.Image = ((object)(resources.GetObject("appearance1.Image")));
            buttonTool4.SharedProps.AppearancesSmall.Appearance = appearance1;
            buttonTool4.SharedProps.Caption = "Export Report to Excel";
            buttonTool4.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance2.Image = ((object)(resources.GetObject("appearance2.Image")));
            buttonTool5.SharedProps.AppearancesSmall.Appearance = appearance2;
            buttonTool5.SharedProps.Caption = "View Files";
            buttonTool5.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance3.Image = ((object)(resources.GetObject("appearance3.Image")));
            buttonTool6.SharedProps.AppearancesSmall.Appearance = appearance3;
            buttonTool6.SharedProps.Caption = "Clear List";
            buttonTool6.SharedProps.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            this.tbrTop.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool5,
            buttonTool6});
            this.tbrTop.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.tbrTop_ToolClick);
            // 
            // _frmProcessingEngine_Toolbars_Dock_Area_Top
            // 
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.Name = "_frmProcessingEngine_Toolbars_Dock_Area_Top";
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(588, 24);
            this._frmProcessingEngine_Toolbars_Dock_Area_Top.ToolbarsManager = this.tbrTop;
            // 
            // _frmProcessingEngine_Toolbars_Dock_Area_Bottom
            // 
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 402);
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.Name = "_frmProcessingEngine_Toolbars_Dock_Area_Bottom";
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(588, 0);
            this._frmProcessingEngine_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tbrTop;
            // 
            // _frmProcessingEngine_Toolbars_Dock_Area_Left
            // 
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 24);
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.Name = "_frmProcessingEngine_Toolbars_Dock_Area_Left";
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 378);
            this._frmProcessingEngine_Toolbars_Dock_Area_Left.ToolbarsManager = this.tbrTop;
            // 
            // _frmProcessingEngine_Toolbars_Dock_Area_Right
            // 
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(588, 24);
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.Name = "_frmProcessingEngine_Toolbars_Dock_Area_Right";
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 378);
            this._frmProcessingEngine_Toolbars_Dock_Area_Right.ToolbarsManager = this.tbrTop;
            // 
            // grdFiles
            // 
            this.grdFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFiles.Location = new System.Drawing.Point(0, 24);
            this.grdFiles.Name = "grdFiles";
            this.grdFiles.Size = new System.Drawing.Size(588, 378);
            this.grdFiles.TabIndex = 9;
            this.grdFiles.InitializeLayout += new Infragistics.Win.UltraWinGrid.InitializeLayoutEventHandler(this.grdFiles_InitializeLayout);
            // 
            // frmFiles
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(588, 402);
            this.Controls.Add(this.grdFiles);
            this.Controls.Add(this._frmProcessingEngine_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmProcessingEngine_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmProcessingEngine_Toolbars_Dock_Area_Top);
            this.Controls.Add(this._frmProcessingEngine_Toolbars_Dock_Area_Bottom);
            this.Name = "frmFiles";
            this.Text = "Files To Process";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmFiles_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tbrTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdFiles)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion
		
		private void FormClear()
		{	
			m_tbl.Rows.Clear();
		}

		private void ViewFiles()
		{
			this.Cursor = Cursors.WaitCursor;
			
			try
			{

				DataRow row;

//				string[] dirs1 = Directory.GetDirectories (@"C:\ACH\TransCentral\", "1*");
//				string[] dirs2 = Directory.GetDirectories (@"C:\ACH\TransCentral\", "2*");

				//string[] dirs1 = Directory.GetDirectories (@"\\nbs-brown\TransCentral\", "1*");
				//string[] dirs2 = Directory.GetDirectories (@"\\nbs-brown\TransCentral\", "2*");

				string[] dirs1 = Directory.GetDirectories(m_SearchDirectory, "1*");
				string[] dirs2 = Directory.GetDirectories(m_SearchDirectory, "2*");

				
				string[] dirs = new string[dirs1.Length + dirs2.Length];
				
				dirs1.CopyTo(dirs, 0);
 
				for (int i = 0;i < dirs2.Length;i++)
				{
					dirs[dirs1.Length + i] = dirs2[i]; 
				}
				

				//string[] dirs = Directory.GetDirectories (@"C:\ACH\TransCentral\", "1*");
			
				foreach (string dir in dirs) 
				{
					string[] files = Directory.GetFiles(@dir, "*.*");
					foreach (string file in files) 
					{
						//string strFile = file.Substring(file.Length - 3,3);
						//if (file.Substring(file.Length - 3, 3).ToUpper() != "RSP" && file.Substring(file.Length - 3, 3).ToUpper() != "RTN" && file.Substring(file.Length - 7, 7).ToUpper() != "RSP.PGP" && file.Substring(file.Length - 7, 7).ToUpper() != "RTN.PGP")
						if (file.Substring(file.Length - 3, 3).ToUpper() == "CCK" || file.Substring(file.Length - 3, 3).ToUpper() == "XLS" || file.Substring(file.Length - 7, 7).ToUpper() == "CCK.PGP" || file.Substring(file.Length - 3, 3).ToUpper() == "INC" || file.Substring(file.Length - 7, 7).ToUpper() == "INC.PGP" || file.Substring(file.Length - 3, 3).ToUpper() == "PRI" || file.Substring(file.Length - 7, 7).ToUpper() == "PRI.PGP" || file.Substring(file.Length - 3, 3).ToUpper() == "PPS" || file.Substring(file.Length - 7, 7).ToUpper() == "PPS.PGP")
						{
							row = m_tbl.NewRow();
							row["MerchantID"] = dir.Substring(dir.Length-5,5);
							row["File"] = file.Substring(file.LastIndexOf(@"\") + 1 ,file.Length - file.LastIndexOf(@"\") - 1);
							m_tbl.Rows.Add(row);
						}
					}
				}

			}
			catch (Exception exc)
			{
				throw exc;
			}
			
			this.Cursor = Cursors.Default;
		}
		private void grdFiles_InitializeLayout(object sender, Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs e)
		{
			e.Layout.Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;
		}

		private void tbrTop_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
		{
			switch(e.Tool.Key)
			{
				case "View Files":
					this.FormClear();
					Application.DoEvents();
					this.ViewFiles();
					break;
			}
		}

		private void frmFiles_Load(object sender, System.EventArgs e)
		{
			m_tbl = new DataTable();
			DataColumn col;

			col = new DataColumn("MerchantID");
			m_tbl.Columns.Add(col);
			col = new DataColumn("File");
			m_tbl.Columns.Add(col);
			
			grdFiles.DataSource = m_tbl;
			grdFiles.DisplayLayout.Bands[0].Columns["File"].Width = 400;
		}

	}
}
