using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinToolbars;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;


namespace AchSystem
{
    public enum Ach_Form_Type
    {
        Standard,
        Wizard,
        Modal
    }
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmMain : System.Windows.Forms.Form
    {
        private Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar uebLeft;
        private Splitter splitter1;
        public ImageList imgMain;
        public Infragistics.Win.UltraWinToolbars.UltraToolbarsManager tbrMain;
        private UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Left;
        private UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Right;
        private UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Top;
        private UltraToolbarsDockArea _frmMain_Toolbars_Dock_Area_Bottom;
        private System.ComponentModel.IContainer components;


        public frmMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            string[] datasource = DataLayer.ConnectStringBuild().Split(';');
            this.Text = this.Text + " (" + datasource[0] + ")";

            //LookUpTableHandler.LoadMerchantIDs(cboMerchantIDs);

            main.g_frmMain = this;

            //ComboBoxTool cbo = (ComboBoxTool) tbrMain.Toolbars["MainMenu"].Tools["cboMerchants"];
            //LookUpTableHandler.LoadMerchantIDs(cbo);
        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem46 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance72 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem1 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance1 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance2 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance3 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance4 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem5 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance5 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem6 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance6 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup2 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem7 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance7 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem8 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance8 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem9 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance9 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem47 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance73 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem10 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance10 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem11 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance11 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem12 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance12 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem48 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance74 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup3 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem13 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance13 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem14 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem15 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem16 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem17 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem18 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem19 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem20 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem21 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup4 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem22 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance14 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem23 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance15 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem24 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance16 = new Infragistics.Win.Appearance();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            Infragistics.Win.Appearance appearance17 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem25 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance18 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem26 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance19 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem27 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance20 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem28 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance21 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem29 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance22 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem30 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance23 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem31 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance24 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem32 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance25 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup5 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem33 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance26 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup6 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem34 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance27 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem35 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance28 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem36 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance29 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem37 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance30 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem38 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance31 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem39 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance32 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem40 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance33 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem41 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance34 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem42 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance35 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup7 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem43 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance36 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem44 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance37 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup ultraExplorerBarGroup8 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup();
            Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem ultraExplorerBarItem45 = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem();
            Infragistics.Win.Appearance appearance38 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.UltraToolbar ultraToolbar1 = new Infragistics.Win.UltraWinToolbars.UltraToolbar("MainMenu");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool1 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("File");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool2 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Report");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool3 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Search");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool4 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Process");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool5 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Lookup");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool6 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Admin");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool7 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Utility");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool8 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Windows");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool1 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("txtDefaultMerchant");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool1 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Select Default Merchant");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool2 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch File Log");
            Infragistics.Win.Appearance appearance39 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool9 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("File");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool3 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool10 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Process");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool4 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch File Log");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool5 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Close Month End");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool6 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Load Returns");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool7 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Process Log");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool8 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Processing Engine");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool9 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Scrub Data");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool11 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Report");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool10 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Hold Releases");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool11 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Over Ticket Items");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool12 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Statement");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool13 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Merchant Balance");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool12 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Search");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool14 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Account Block");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool15 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Bank");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool16 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool17 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch Detail");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool18 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EFT");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool19 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Hold");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool20 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Journal");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool21 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Merchant");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool22 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Return");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool23 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Transaction");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool13 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Admin");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool24 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Activity Log");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool25 = new Infragistics.Win.UltraWinToolbars.ButtonTool("User");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool26 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Close Month End");
            Infragistics.Win.Appearance appearance40 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool27 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Load Returns");
            Infragistics.Win.Appearance appearance41 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool28 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Process Log");
            Infragistics.Win.Appearance appearance42 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool29 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Processing Engine");
            Infragistics.Win.Appearance appearance43 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool30 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Scrub Data");
            Infragistics.Win.Appearance appearance44 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool31 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Hold Releases");
            Infragistics.Win.Appearance appearance45 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool32 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Over Ticket Items");
            Infragistics.Win.Appearance appearance46 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool33 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Statement");
            Infragistics.Win.Appearance appearance47 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool34 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Merchant Balance");
            Infragistics.Win.Appearance appearance48 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool35 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Account Block");
            Infragistics.Win.Appearance appearance49 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool36 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Bank");
            Infragistics.Win.Appearance appearance50 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool37 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch");
            Infragistics.Win.Appearance appearance51 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool38 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Batch Detail");
            Infragistics.Win.Appearance appearance52 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool39 = new Infragistics.Win.UltraWinToolbars.ButtonTool("EFT");
            Infragistics.Win.Appearance appearance53 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool40 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Hold");
            Infragistics.Win.Appearance appearance54 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool41 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Journal");
            Infragistics.Win.Appearance appearance55 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool42 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Merchant");
            Infragistics.Win.Appearance appearance56 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool43 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Return");
            Infragistics.Win.Appearance appearance57 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool44 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Transaction");
            Infragistics.Win.Appearance appearance58 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool45 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Group Merchant");
            Infragistics.Win.Appearance appearance59 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool46 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Journal Refcode");
            Infragistics.Win.Appearance appearance60 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool47 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Origin");
            Infragistics.Win.Appearance appearance61 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool48 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Return Reason Code");
            Infragistics.Win.Appearance appearance62 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool49 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SEC Codes");
            Infragistics.Win.Appearance appearance63 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool50 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Source");
            Infragistics.Win.Appearance appearance64 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool51 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trans Status Code");
            Infragistics.Win.Appearance appearance65 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool52 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trans Type");
            Infragistics.Win.Appearance appearance66 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool14 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Lookup");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool53 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Group Merchant");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool54 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Journal Refcode");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool55 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Origin");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool56 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Return Reason Code");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool57 = new Infragistics.Win.UltraWinToolbars.ButtonTool("SEC Codes");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool58 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Source");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool59 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trans Status Code");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool60 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Trans Type");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool61 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Exit");
            Infragistics.Win.Appearance appearance67 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.MdiWindowListTool mdiWindowListTool1 = new Infragistics.Win.UltraWinToolbars.MdiWindowListTool("MDIWindowListTool1");
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool15 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Windows");
            Infragistics.Win.UltraWinToolbars.MdiWindowListTool mdiWindowListTool2 = new Infragistics.Win.UltraWinToolbars.MdiWindowListTool("MDIWindowListTool1");
            Infragistics.Win.UltraWinToolbars.ComboBoxTool comboBoxTool1 = new Infragistics.Win.UltraWinToolbars.ComboBoxTool("cboMerchants");
            Infragistics.Win.ValueList valueList1 = new Infragistics.Win.ValueList(0);
            Infragistics.Win.UltraWinToolbars.LabelTool labelTool1 = new Infragistics.Win.UltraWinToolbars.LabelTool("Default Merchant:");
            Infragistics.Win.UltraWinToolbars.TextBoxTool textBoxTool2 = new Infragistics.Win.UltraWinToolbars.TextBoxTool("txtDefaultMerchant");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool62 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Select Default Merchant");
            Infragistics.Win.Appearance appearance68 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool63 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Activity Log");
            Infragistics.Win.Appearance appearance69 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool64 = new Infragistics.Win.UltraWinToolbars.ButtonTool("User");
            Infragistics.Win.Appearance appearance70 = new Infragistics.Win.Appearance();
            Infragistics.Win.UltraWinToolbars.PopupMenuTool popupMenuTool16 = new Infragistics.Win.UltraWinToolbars.PopupMenuTool("Utility");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool65 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Purge Batch");
            Infragistics.Win.UltraWinToolbars.ButtonTool buttonTool66 = new Infragistics.Win.UltraWinToolbars.ButtonTool("Purge Batch");
            Infragistics.Win.Appearance appearance71 = new Infragistics.Win.Appearance();
            this.uebLeft = new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBar();
            this.imgMain = new System.Windows.Forms.ImageList(this.components);
            this.splitter1 = new System.Windows.Forms.Splitter();
            this._frmMain_Toolbars_Dock_Area_Left = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this.tbrMain = new Infragistics.Win.UltraWinToolbars.UltraToolbarsManager(this.components);
            this._frmMain_Toolbars_Dock_Area_Right = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmMain_Toolbars_Dock_Area_Top = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            this._frmMain_Toolbars_Dock_Area_Bottom = new Infragistics.Win.UltraWinToolbars.UltraToolbarsDockArea();
            ((System.ComponentModel.ISupportInitialize)(this.uebLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).BeginInit();
            this.SuspendLayout();
            // 
            // uebLeft
            // 
            this.uebLeft.Dock = System.Windows.Forms.DockStyle.Left;
            appearance72.Image = "gear_find.ico";
            ultraExplorerBarItem46.Settings.AppearancesSmall.Appearance = appearance72;
            ultraExplorerBarItem46.Text = "NCAL Split Funding";
            ultraExplorerBarItem46.ToolTipText = "Use this screen to split NCAL\'s Bankcard file";
            ultraExplorerBarItem1.Key = "Batch File Log";
            appearance1.Image = "gear_find.ico";
            ultraExplorerBarItem1.Settings.AppearancesSmall.Appearance = appearance1;
            ultraExplorerBarItem1.Text = "Batch File Log";
            ultraExplorerBarItem1.ToolTipText = "This screen tracks the batches that are uploaded by merchants.";
            ultraExplorerBarItem2.Key = "Close Month End";
            appearance2.Image = "gear_time.ico";
            ultraExplorerBarItem2.Settings.AppearancesSmall.Appearance = appearance2;
            ultraExplorerBarItem2.Text = "Close Month End";
            ultraExplorerBarItem2.ToolTipText = "This function should be performed at the end of every month.";
            ultraExplorerBarItem3.Key = "Load Returns";
            appearance3.Image = "gear_forbidden.ico";
            ultraExplorerBarItem3.Settings.AppearancesSmall.Appearance = appearance3;
            ultraExplorerBarItem3.Text = "Load Returns";
            ultraExplorerBarItem3.ToolTipText = "This screen allows you to load returns from the bank.";
            ultraExplorerBarItem4.Key = "Process Log";
            appearance4.Image = "gear_view.ico";
            ultraExplorerBarItem4.Settings.AppearancesSmall.Appearance = appearance4;
            ultraExplorerBarItem4.Text = "Process Log";
            ultraExplorerBarItem4.ToolTipText = "This screen displays the activities in the ACH process";
            ultraExplorerBarItem5.Key = "Processing Engine";
            appearance5.Image = "gear_run.ico";
            ultraExplorerBarItem5.Settings.AppearancesSmall.Appearance = appearance5;
            ultraExplorerBarItem5.Text = "Processing Engine";
            appearance6.Image = "gear_replace.ico";
            ultraExplorerBarItem6.Settings.AppearancesSmall.Appearance = appearance6;
            ultraExplorerBarItem6.Text = "Scrub Data";
            ultraExplorerBarItem6.ToolTipText = "This screen validate the ACH data.";
            ultraExplorerBarGroup1.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem46,
            ultraExplorerBarItem1,
            ultraExplorerBarItem2,
            ultraExplorerBarItem3,
            ultraExplorerBarItem4,
            ultraExplorerBarItem5,
            ultraExplorerBarItem6});
            ultraExplorerBarGroup1.Key = "Process Module";
            ultraExplorerBarGroup1.Text = "Process Module";
            ultraExplorerBarItem7.Key = "Bad Returns";
            appearance7.Image = "history_delete.ico";
            ultraExplorerBarItem7.Settings.AppearancesSmall.Appearance = appearance7;
            ultraExplorerBarItem7.Text = "Bad Returns";
            ultraExplorerBarItem7.ToolTipText = "This screen displays returns for transactions that did not orignate from this sys" +
    "tem.";
            ultraExplorerBarItem8.Key = "Hold Releases";
            appearance8.Image = "table_preferences.ico";
            ultraExplorerBarItem8.Settings.AppearancesSmall.Appearance = appearance8;
            ultraExplorerBarItem8.Text = "Hold Releases";
            ultraExplorerBarItem8.ToolTipText = "This screen shows the holds that will be released.";
            ultraExplorerBarItem9.Key = "Merchant Balance";
            appearance9.Image = "chart.ico";
            ultraExplorerBarItem9.Settings.AppearancesSmall.Appearance = appearance9;
            ultraExplorerBarItem9.Text = "Merchant Balance";
            ultraExplorerBarItem9.ToolTipText = "This screen shows that balances for all merchants.";
            appearance73.Image = "chart.ico";
            ultraExplorerBarItem47.Settings.AppearancesSmall.Appearance = appearance73;
            ultraExplorerBarItem47.Text = "Balance and Fee";
            ultraExplorerBarItem47.ToolTipText = "This screen shows that ending balance and total fees";
            ultraExplorerBarItem10.Key = "Settlement File Summary";
            appearance10.Image = "document_out.png";
            ultraExplorerBarItem10.Settings.AppearancesSmall.Appearance = appearance10;
            ultraExplorerBarItem10.Text = "Settlement File Summary";
            ultraExplorerBarItem11.Key = "Return Totals";
            appearance11.Image = "document_out.png";
            ultraExplorerBarItem11.Settings.AppearancesSmall.Appearance = appearance11;
            ultraExplorerBarItem11.Text = "Return Totals";
            ultraExplorerBarItem12.Key = "Statement";
            appearance12.Image = "line-chart.ico";
            ultraExplorerBarItem12.Settings.AppearancesSmall.Appearance = appearance12;
            ultraExplorerBarItem12.Text = "Statement";
            ultraExplorerBarItem12.ToolTipText = "The Statement report shows the detail ACH activities.";
            ultraExplorerBarItem48.Key = "Settlement Forecast";
            appearance74.Image = "document_out.png";
            ultraExplorerBarItem48.Settings.AppearancesSmall.Appearance = appearance74;
            ultraExplorerBarItem48.Text = "Settlement Forecast";
            ultraExplorerBarGroup2.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem7,
            ultraExplorerBarItem8,
            ultraExplorerBarItem9,
            ultraExplorerBarItem47,
            ultraExplorerBarItem10,
            ultraExplorerBarItem11,
            ultraExplorerBarItem12,
            ultraExplorerBarItem48});
            ultraExplorerBarGroup2.Key = "Report Module";
            ultraExplorerBarGroup2.Text = "Report Module";
            ultraExplorerBarItem13.Key = "Over Ticket Items";
            appearance13.Image = "table_delete.ico";
            ultraExplorerBarItem13.Settings.AppearancesSmall.Appearance = appearance13;
            ultraExplorerBarItem13.Text = "Over Ticket Items";
            ultraExplorerBarItem14.Key = "Return Rates";
            ultraExplorerBarItem14.Text = "Return Rates";
            ultraExplorerBarItem14.Visible = false;
            ultraExplorerBarItem15.Key = "Return Summary";
            ultraExplorerBarItem15.Text = "Return Summary";
            ultraExplorerBarItem16.Key = "Monthly Activity Totals";
            ultraExplorerBarItem16.Text = "Monthly Activity Totals";
            ultraExplorerBarItem17.Key = "OT-Monthly Activity Totals";
            ultraExplorerBarItem17.Text = "OT-Monthly Activity Totals";
            ultraExplorerBarItem18.Key = "Unauthorized Return Rates";
            ultraExplorerBarItem18.Text = "Unauthorized Return Rates";
            ultraExplorerBarItem19.Key = "Exceed Average Ticket";
            ultraExplorerBarItem19.Text = "Exceed Average Ticket";
            ultraExplorerBarItem20.Key = "Exceed Monthly Volume";
            ultraExplorerBarItem20.Text = "Exceed Monthly Volume";
            ultraExplorerBarItem21.Key = "Neg Balance Return";
            ultraExplorerBarItem21.Text = "Neg Balance Return";
            ultraExplorerBarGroup3.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem13,
            ultraExplorerBarItem14,
            ultraExplorerBarItem15,
            ultraExplorerBarItem16,
            ultraExplorerBarItem17,
            ultraExplorerBarItem18,
            ultraExplorerBarItem19,
            ultraExplorerBarItem20,
            ultraExplorerBarItem21});
            ultraExplorerBarGroup3.Key = "Risk Module";
            ultraExplorerBarGroup3.Text = "Risk Module";
            appearance14.Image = "server_forbidden.ico";
            ultraExplorerBarItem22.Settings.AppearancesSmall.Appearance = appearance14;
            ultraExplorerBarItem22.Text = "Account Block";
            ultraExplorerBarItem22.ToolTipText = "This screen tracks all the account blocks.";
            ultraExplorerBarItem23.Key = "Ach Recurring";
            appearance15.Image = "document_exchange.png";
            ultraExplorerBarItem23.Settings.AppearancesSmall.Appearance = appearance15;
            ultraExplorerBarItem23.Text = "Ach Recurring";
            ultraExplorerBarItem24.Key = "Bank";
            appearance16.Image = ((object)(resources.GetObject("appearance16.Image")));
            ultraExplorerBarItem24.Settings.AppearancesLarge.Appearance = appearance16;
            appearance17.Image = "goldbars.ico";
            ultraExplorerBarItem24.Settings.AppearancesSmall.Appearance = appearance17;
            ultraExplorerBarItem24.Text = "Bank";
            ultraExplorerBarItem24.ToolTipText = "This screen shows all ACH participating banks.";
            ultraExplorerBarItem25.Key = "Batch";
            appearance18.Image = "server.ico";
            ultraExplorerBarItem25.Settings.AppearancesSmall.Appearance = appearance18;
            ultraExplorerBarItem25.Text = "Batch";
            ultraExplorerBarItem25.ToolTipText = "This screen shows all batches created by the system.";
            ultraExplorerBarItem26.Key = "Batch Detail";
            appearance19.Image = "server_client.ico";
            ultraExplorerBarItem26.Settings.AppearancesSmall.Appearance = appearance19;
            ultraExplorerBarItem26.Text = "Batch Detail";
            ultraExplorerBarItem26.ToolTipText = "This screen displays all the transactions that went out to the bank.";
            ultraExplorerBarItem27.Key = "EFT";
            appearance20.Image = "server_client_exchange.ico";
            ultraExplorerBarItem27.Settings.AppearancesSmall.Appearance = appearance20;
            ultraExplorerBarItem27.Text = "EFT";
            ultraExplorerBarItem27.ToolTipText = "This screen tracks all payments and collections to and from merchants.";
            ultraExplorerBarItem28.Checked = true;
            ultraExplorerBarItem28.Key = "Hold";
            appearance21.Image = "server_time.ico";
            ultraExplorerBarItem28.Settings.AppearancesSmall.Appearance = appearance21;
            ultraExplorerBarItem28.Text = "Hold";
            ultraExplorerBarItem28.ToolTipText = "This screen shows all the hold releases payout to the merchants.";
            ultraExplorerBarItem29.Key = "Journal";
            appearance22.Image = "server_document.ico";
            ultraExplorerBarItem29.Settings.AppearancesSmall.Appearance = appearance22;
            ultraExplorerBarItem29.Text = "Journal";
            ultraExplorerBarItem29.ToolTipText = "This screen shows the detail ACH activities.";
            ultraExplorerBarItem30.Key = "Merchant";
            appearance23.Image = "User.ico";
            ultraExplorerBarItem30.Settings.AppearancesSmall.Appearance = appearance23;
            ultraExplorerBarItem30.Text = "Merchant";
            ultraExplorerBarItem30.ToolTipText = "This screen displays all ACH merchants.";
            ultraExplorerBarItem31.Key = "Return";
            appearance24.Image = "data_previous.ico";
            ultraExplorerBarItem31.Settings.AppearancesSmall.Appearance = appearance24;
            ultraExplorerBarItem31.Text = "Return";
            ultraExplorerBarItem31.ToolTipText = "This screen shows scrubbed returns and also returns that are initiated by the RDF" +
    "I.";
            appearance25.Image = "server_preferences.ico";
            ultraExplorerBarItem32.Settings.AppearancesSmall.Appearance = appearance25;
            ultraExplorerBarItem32.Text = "Transaction";
            ultraExplorerBarItem32.ToolTipText = "This screen shows all ACH transactions.";
            ultraExplorerBarGroup4.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem22,
            ultraExplorerBarItem23,
            ultraExplorerBarItem24,
            ultraExplorerBarItem25,
            ultraExplorerBarItem26,
            ultraExplorerBarItem27,
            ultraExplorerBarItem28,
            ultraExplorerBarItem29,
            ultraExplorerBarItem30,
            ultraExplorerBarItem31,
            ultraExplorerBarItem32});
            ultraExplorerBarGroup4.Key = "Search Module";
            ultraExplorerBarGroup4.Text = "Search Module";
            ultraExplorerBarItem33.Key = "Purge Batch";
            appearance26.Image = ((object)(resources.GetObject("appearance26.Image")));
            ultraExplorerBarItem33.Settings.AppearancesSmall.Appearance = appearance26;
            ultraExplorerBarItem33.Text = "Purge Batch";
            ultraExplorerBarGroup5.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem33});
            ultraExplorerBarGroup5.Key = "Utility Module";
            ultraExplorerBarGroup5.Text = "Utility Module";
            appearance27.Image = ((object)(resources.GetObject("appearance27.Image")));
            ultraExplorerBarItem34.Settings.AppearancesSmall.Appearance = appearance27;
            ultraExplorerBarItem34.Text = "Group Merchant";
            ultraExplorerBarItem34.ToolTipText = "Group Merchant allows you to logically group merchants.";
            ultraExplorerBarItem35.Key = "Holiday";
            appearance28.Image = "clock.png";
            ultraExplorerBarItem35.Settings.AppearancesSmall.Appearance = appearance28;
            ultraExplorerBarItem35.Text = "Holiday";
            appearance29.Image = ((object)(resources.GetObject("appearance29.Image")));
            ultraExplorerBarItem36.Settings.AppearancesSmall.Appearance = appearance29;
            ultraExplorerBarItem36.Text = "Journal Refcode";
            ultraExplorerBarItem36.ToolTipText = "Refcode shows the purpose of the journal entry.";
            appearance30.Image = ((object)(resources.GetObject("appearance30.Image")));
            ultraExplorerBarItem37.Settings.AppearancesSmall.Appearance = appearance30;
            ultraExplorerBarItem37.Text = "Origin";
            ultraExplorerBarItem37.ToolTipText = "Origin shows how a transaction was integrated into the system.";
            appearance31.Image = ((object)(resources.GetObject("appearance31.Image")));
            ultraExplorerBarItem38.Settings.AppearancesSmall.Appearance = appearance31;
            ultraExplorerBarItem38.Text = "Return Reason Code";
            ultraExplorerBarItem38.ToolTipText = "Reason Code shows why a transaction was returned from the bank.";
            appearance32.Image = ((object)(resources.GetObject("appearance32.Image")));
            ultraExplorerBarItem39.Settings.AppearancesSmall.Appearance = appearance32;
            ultraExplorerBarItem39.Text = "SECC Codes";
            ultraExplorerBarItem39.ToolTipText = "SECC Code show the type of the ACH transaction.";
            appearance33.Image = ((object)(resources.GetObject("appearance33.Image")));
            ultraExplorerBarItem40.Settings.AppearancesSmall.Appearance = appearance33;
            ultraExplorerBarItem40.Text = "Source";
            ultraExplorerBarItem40.ToolTipText = "Source identifies whether a transaction is created by a merchant or by the system" +
    ".";
            appearance34.Image = ((object)(resources.GetObject("appearance34.Image")));
            ultraExplorerBarItem41.Settings.AppearancesSmall.Appearance = appearance34;
            ultraExplorerBarItem41.Text = "Trans Status Code";
            ultraExplorerBarItem41.ToolTipText = "Status Code describes the state of a transaction.";
            appearance35.Image = ((object)(resources.GetObject("appearance35.Image")));
            ultraExplorerBarItem42.Settings.AppearancesSmall.Appearance = appearance35;
            ultraExplorerBarItem42.Text = "Trans Type";
            ultraExplorerBarItem42.ToolTipText = "Trans Type shows whether a transaction is a debit or a credit and the bank accoun" +
    "t type.";
            ultraExplorerBarGroup6.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem34,
            ultraExplorerBarItem35,
            ultraExplorerBarItem36,
            ultraExplorerBarItem37,
            ultraExplorerBarItem38,
            ultraExplorerBarItem39,
            ultraExplorerBarItem40,
            ultraExplorerBarItem41,
            ultraExplorerBarItem42});
            ultraExplorerBarGroup6.Key = "Lookup Module";
            ultraExplorerBarGroup6.Text = "Lookup Module";
            ultraExplorerBarItem43.Key = "Activity Log";
            appearance36.Image = "dot-chart.ico";
            ultraExplorerBarItem43.Settings.AppearancesSmall.Appearance = appearance36;
            ultraExplorerBarItem43.Text = "Activity Log";
            ultraExplorerBarItem44.Key = "User";
            appearance37.Image = "lock.ico";
            ultraExplorerBarItem44.Settings.AppearancesSmall.Appearance = appearance37;
            ultraExplorerBarItem44.Text = "User";
            ultraExplorerBarGroup7.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem43,
            ultraExplorerBarItem44});
            ultraExplorerBarGroup7.Key = "Admin Module";
            ultraExplorerBarGroup7.Text = "Admin Module";
            ultraExplorerBarItem45.Key = "Change Password";
            appearance38.Image = "replace2.png";
            ultraExplorerBarItem45.Settings.AppearancesSmall.Appearance = appearance38;
            ultraExplorerBarItem45.Text = "Change Password";
            ultraExplorerBarGroup8.Items.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarItem[] {
            ultraExplorerBarItem45});
            ultraExplorerBarGroup8.Key = "User";
            ultraExplorerBarGroup8.Text = "User";
            this.uebLeft.Groups.AddRange(new Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarGroup[] {
            ultraExplorerBarGroup1,
            ultraExplorerBarGroup2,
            ultraExplorerBarGroup3,
            ultraExplorerBarGroup4,
            ultraExplorerBarGroup5,
            ultraExplorerBarGroup6,
            ultraExplorerBarGroup7,
            ultraExplorerBarGroup8});
            this.uebLeft.GroupSpacing = 7;
            this.uebLeft.ImageListSmall = this.imgMain;
            this.uebLeft.Location = new System.Drawing.Point(0, 26);
            this.uebLeft.Name = "uebLeft";
            this.uebLeft.Size = new System.Drawing.Size(194, 567);
            this.uebLeft.TabIndex = 19;
            this.uebLeft.ViewStyle = Infragistics.Win.UltraWinExplorerBar.UltraExplorerBarViewStyle.Office2003;
            this.uebLeft.ItemClick += new Infragistics.Win.UltraWinExplorerBar.ItemClickEventHandler(this.uebLeft_ItemClick);
            // 
            // imgMain
            // 
            this.imgMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMain.ImageStream")));
            this.imgMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMain.Images.SetKeyName(0, "gear_find.ico");
            this.imgMain.Images.SetKeyName(1, "gear_time.ico");
            this.imgMain.Images.SetKeyName(2, "gear_forbidden.ico");
            this.imgMain.Images.SetKeyName(3, "gear_view.ico");
            this.imgMain.Images.SetKeyName(4, "gear_run.ico");
            this.imgMain.Images.SetKeyName(5, "gear_replace.ico");
            this.imgMain.Images.SetKeyName(6, "table_preferences.ico");
            this.imgMain.Images.SetKeyName(7, "table_delete.ico");
            this.imgMain.Images.SetKeyName(8, "line-chart.ico");
            this.imgMain.Images.SetKeyName(9, "chart.ico");
            this.imgMain.Images.SetKeyName(10, "server_forbidden.ico");
            this.imgMain.Images.SetKeyName(11, "server.ico");
            this.imgMain.Images.SetKeyName(12, "server_client.ico");
            this.imgMain.Images.SetKeyName(13, "server_client_exchange.ico");
            this.imgMain.Images.SetKeyName(14, "server_time.ico");
            this.imgMain.Images.SetKeyName(15, "server_document.ico");
            this.imgMain.Images.SetKeyName(16, "data_previous.ico");
            this.imgMain.Images.SetKeyName(17, "server_preferences.ico");
            this.imgMain.Images.SetKeyName(18, "goldbars.ico");
            this.imgMain.Images.SetKeyName(19, "data_scroll.ico");
            this.imgMain.Images.SetKeyName(20, "data_new.ico");
            this.imgMain.Images.SetKeyName(21, "data_error.ico");
            this.imgMain.Images.SetKeyName(22, "data_lock.ico");
            this.imgMain.Images.SetKeyName(23, "data_disk.ico");
            this.imgMain.Images.SetKeyName(24, "data_ok.ico");
            this.imgMain.Images.SetKeyName(25, "data_unknown.ico");
            this.imgMain.Images.SetKeyName(26, "dot-chart.ico");
            this.imgMain.Images.SetKeyName(27, "lock.ico");
            this.imgMain.Images.SetKeyName(28, "key1.ico");
            this.imgMain.Images.SetKeyName(29, "User.ico");
            this.imgMain.Images.SetKeyName(30, "Users.ico");
            this.imgMain.Images.SetKeyName(31, "view.ico");
            this.imgMain.Images.SetKeyName(32, "Recycle Bin Empty.ico");
            this.imgMain.Images.SetKeyName(33, "exit.ico");
            this.imgMain.Images.SetKeyName(34, "history_delete.ico");
            this.imgMain.Images.SetKeyName(35, "document_out.png");
            this.imgMain.Images.SetKeyName(36, "clock.png");
            this.imgMain.Images.SetKeyName(37, "replace2.png");
            this.imgMain.Images.SetKeyName(38, "document_exchange.png");
            this.imgMain.Images.SetKeyName(39, "money2.png");
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(194, 26);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 567);
            this.splitter1.TabIndex = 21;
            this.splitter1.TabStop = false;
            // 
            // _frmMain_Toolbars_Dock_Area_Left
            // 
            this._frmMain_Toolbars_Dock_Area_Left.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Left.BackColor = System.Drawing.SystemColors.Control;
            this._frmMain_Toolbars_Dock_Area_Left.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Left;
            this._frmMain_Toolbars_Dock_Area_Left.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMain_Toolbars_Dock_Area_Left.Location = new System.Drawing.Point(0, 26);
            this._frmMain_Toolbars_Dock_Area_Left.Name = "_frmMain_Toolbars_Dock_Area_Left";
            this._frmMain_Toolbars_Dock_Area_Left.Size = new System.Drawing.Size(0, 567);
            this._frmMain_Toolbars_Dock_Area_Left.ToolbarsManager = this.tbrMain;
            // 
            // tbrMain
            // 
            this.tbrMain.DesignerFlags = 1;
            this.tbrMain.DockWithinContainer = this;
            this.tbrMain.DockWithinContainerBaseType = typeof(System.Windows.Forms.Form);
            this.tbrMain.ImageListSmall = this.imgMain;
            this.tbrMain.ShowFullMenusDelay = 500;
            this.tbrMain.Style = Infragistics.Win.UltraWinToolbars.ToolbarStyle.VisualStudio2005;
            ultraToolbar1.DockedColumn = 0;
            ultraToolbar1.DockedRow = 0;
            ultraToolbar1.IsMainMenuBar = true;
            textBoxTool1.InstanceProps.IsFirstInGroup = true;
            ultraToolbar1.NonInheritedTools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            popupMenuTool1,
            popupMenuTool2,
            popupMenuTool3,
            popupMenuTool4,
            popupMenuTool5,
            popupMenuTool6,
            popupMenuTool7,
            popupMenuTool8,
            textBoxTool1,
            buttonTool1});
            ultraToolbar1.Text = "MainMenu";
            this.tbrMain.Toolbars.AddRange(new Infragistics.Win.UltraWinToolbars.UltraToolbar[] {
            ultraToolbar1});
            appearance39.Image = "gear_find.ico";
            buttonTool2.SharedPropsInternal.AppearancesSmall.Appearance = appearance39;
            buttonTool2.SharedPropsInternal.Caption = "Batch File Log";
            buttonTool2.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            popupMenuTool9.SharedPropsInternal.Caption = "&File";
            popupMenuTool9.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool3});
            popupMenuTool10.SharedPropsInternal.Caption = "&Process";
            popupMenuTool10.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool4,
            buttonTool5,
            buttonTool6,
            buttonTool7,
            buttonTool8,
            buttonTool9});
            popupMenuTool11.SharedPropsInternal.Caption = "&Report";
            popupMenuTool11.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool10,
            buttonTool11,
            buttonTool12,
            buttonTool13});
            popupMenuTool12.SharedPropsInternal.Caption = "&Search";
            popupMenuTool12.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool14,
            buttonTool15,
            buttonTool16,
            buttonTool17,
            buttonTool18,
            buttonTool19,
            buttonTool20,
            buttonTool21,
            buttonTool22,
            buttonTool23});
            popupMenuTool13.SharedPropsInternal.Caption = "&Admin";
            popupMenuTool13.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool24,
            buttonTool25});
            appearance40.Image = 1;
            buttonTool26.SharedPropsInternal.AppearancesSmall.Appearance = appearance40;
            buttonTool26.SharedPropsInternal.Caption = "Close Month End";
            appearance41.Image = 2;
            buttonTool27.SharedPropsInternal.AppearancesSmall.Appearance = appearance41;
            buttonTool27.SharedPropsInternal.Caption = "Load Returns";
            appearance42.Image = 3;
            buttonTool28.SharedPropsInternal.AppearancesSmall.Appearance = appearance42;
            buttonTool28.SharedPropsInternal.Caption = "Process Log";
            appearance43.Image = 4;
            buttonTool29.SharedPropsInternal.AppearancesSmall.Appearance = appearance43;
            buttonTool29.SharedPropsInternal.Caption = "Processing Engine";
            appearance44.Image = 5;
            buttonTool30.SharedPropsInternal.AppearancesSmall.Appearance = appearance44;
            buttonTool30.SharedPropsInternal.Caption = "Scrub Data";
            appearance45.Image = 6;
            buttonTool31.SharedPropsInternal.AppearancesSmall.Appearance = appearance45;
            buttonTool31.SharedPropsInternal.Caption = "Hold Releases";
            appearance46.Image = 7;
            buttonTool32.SharedPropsInternal.AppearancesSmall.Appearance = appearance46;
            buttonTool32.SharedPropsInternal.Caption = "Over Ticket Items";
            appearance47.Image = 8;
            buttonTool33.SharedPropsInternal.AppearancesSmall.Appearance = appearance47;
            buttonTool33.SharedPropsInternal.Caption = "Statement";
            appearance48.Image = 9;
            buttonTool34.SharedPropsInternal.AppearancesSmall.Appearance = appearance48;
            buttonTool34.SharedPropsInternal.Caption = "Merchant Balance";
            appearance49.Image = 10;
            buttonTool35.SharedPropsInternal.AppearancesSmall.Appearance = appearance49;
            buttonTool35.SharedPropsInternal.Caption = "Account Block";
            appearance50.Image = 18;
            buttonTool36.SharedPropsInternal.AppearancesSmall.Appearance = appearance50;
            buttonTool36.SharedPropsInternal.Caption = "Bank";
            appearance51.Image = 11;
            buttonTool37.SharedPropsInternal.AppearancesSmall.Appearance = appearance51;
            buttonTool37.SharedPropsInternal.Caption = "Batch";
            appearance52.Image = 12;
            buttonTool38.SharedPropsInternal.AppearancesSmall.Appearance = appearance52;
            buttonTool38.SharedPropsInternal.Caption = "Batch Detail";
            appearance53.Image = 13;
            buttonTool39.SharedPropsInternal.AppearancesSmall.Appearance = appearance53;
            buttonTool39.SharedPropsInternal.Caption = "EFT";
            appearance54.Image = 14;
            buttonTool40.SharedPropsInternal.AppearancesSmall.Appearance = appearance54;
            buttonTool40.SharedPropsInternal.Caption = "Hold";
            appearance55.Image = 15;
            buttonTool41.SharedPropsInternal.AppearancesSmall.Appearance = appearance55;
            buttonTool41.SharedPropsInternal.Caption = "Journal";
            appearance56.Image = 29;
            buttonTool42.SharedPropsInternal.AppearancesSmall.Appearance = appearance56;
            buttonTool42.SharedPropsInternal.Caption = "Merchant";
            appearance57.Image = 16;
            buttonTool43.SharedPropsInternal.AppearancesSmall.Appearance = appearance57;
            buttonTool43.SharedPropsInternal.Caption = "Return";
            appearance58.Image = 17;
            buttonTool44.SharedPropsInternal.AppearancesSmall.Appearance = appearance58;
            buttonTool44.SharedPropsInternal.Caption = "Transaction";
            appearance59.Image = 30;
            buttonTool45.SharedPropsInternal.AppearancesSmall.Appearance = appearance59;
            buttonTool45.SharedPropsInternal.Caption = "Group Merchant";
            appearance60.Image = 19;
            buttonTool46.SharedPropsInternal.AppearancesSmall.Appearance = appearance60;
            buttonTool46.SharedPropsInternal.Caption = "Journal Refcode";
            appearance61.Image = 20;
            buttonTool47.SharedPropsInternal.AppearancesSmall.Appearance = appearance61;
            buttonTool47.SharedPropsInternal.Caption = "Origin";
            appearance62.Image = 21;
            buttonTool48.SharedPropsInternal.AppearancesSmall.Appearance = appearance62;
            buttonTool48.SharedPropsInternal.Caption = "Return Reason Code";
            appearance63.Image = 22;
            buttonTool49.SharedPropsInternal.AppearancesSmall.Appearance = appearance63;
            buttonTool49.SharedPropsInternal.Caption = "SEC Codes";
            appearance64.Image = 23;
            buttonTool50.SharedPropsInternal.AppearancesSmall.Appearance = appearance64;
            buttonTool50.SharedPropsInternal.Caption = "Source";
            appearance65.Image = 24;
            buttonTool51.SharedPropsInternal.AppearancesSmall.Appearance = appearance65;
            buttonTool51.SharedPropsInternal.Caption = "Trans Status Code";
            appearance66.Image = 25;
            buttonTool52.SharedPropsInternal.AppearancesSmall.Appearance = appearance66;
            buttonTool52.SharedPropsInternal.Caption = "Trans Type";
            popupMenuTool14.SharedPropsInternal.Caption = "&Lookup";
            popupMenuTool14.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool53,
            buttonTool54,
            buttonTool55,
            buttonTool56,
            buttonTool57,
            buttonTool58,
            buttonTool59,
            buttonTool60});
            appearance67.Image = 33;
            buttonTool61.SharedPropsInternal.AppearancesSmall.Appearance = appearance67;
            buttonTool61.SharedPropsInternal.Caption = "&Exit";
            buttonTool61.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            mdiWindowListTool1.SharedPropsInternal.Caption = "MDIWindowListTool1";
            popupMenuTool15.SharedPropsInternal.Caption = "&Windows";
            popupMenuTool15.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            mdiWindowListTool2});
            comboBoxTool1.SharedPropsInternal.Caption = "cboMerchants";
            comboBoxTool1.ValueList = valueList1;
            labelTool1.SharedPropsInternal.Caption = "Default Merchant:";
            textBoxTool2.Locked = true;
            textBoxTool2.SharedPropsInternal.Caption = "txtDefaultMerchant";
            textBoxTool2.SharedPropsInternal.Width = 200;
            appearance68.Image = 31;
            buttonTool62.SharedPropsInternal.AppearancesSmall.Appearance = appearance68;
            buttonTool62.SharedPropsInternal.Caption = "Select Default Merchant";
            buttonTool62.SharedPropsInternal.DisplayStyle = Infragistics.Win.UltraWinToolbars.ToolDisplayStyle.ImageAndText;
            appearance69.Image = 26;
            buttonTool63.SharedPropsInternal.AppearancesSmall.Appearance = appearance69;
            buttonTool63.SharedPropsInternal.Caption = "Activity Log";
            appearance70.Image = 27;
            buttonTool64.SharedPropsInternal.AppearancesSmall.Appearance = appearance70;
            buttonTool64.SharedPropsInternal.Caption = "User";
            popupMenuTool16.SharedPropsInternal.Caption = "&Utility";
            popupMenuTool16.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool65});
            appearance71.Image = 32;
            buttonTool66.SharedPropsInternal.AppearancesSmall.Appearance = appearance71;
            buttonTool66.SharedPropsInternal.Caption = "Purge Batch";
            this.tbrMain.Tools.AddRange(new Infragistics.Win.UltraWinToolbars.ToolBase[] {
            buttonTool2,
            popupMenuTool9,
            popupMenuTool10,
            popupMenuTool11,
            popupMenuTool12,
            popupMenuTool13,
            buttonTool26,
            buttonTool27,
            buttonTool28,
            buttonTool29,
            buttonTool30,
            buttonTool31,
            buttonTool32,
            buttonTool33,
            buttonTool34,
            buttonTool35,
            buttonTool36,
            buttonTool37,
            buttonTool38,
            buttonTool39,
            buttonTool40,
            buttonTool41,
            buttonTool42,
            buttonTool43,
            buttonTool44,
            buttonTool45,
            buttonTool46,
            buttonTool47,
            buttonTool48,
            buttonTool49,
            buttonTool50,
            buttonTool51,
            buttonTool52,
            popupMenuTool14,
            buttonTool61,
            mdiWindowListTool1,
            popupMenuTool15,
            comboBoxTool1,
            labelTool1,
            textBoxTool2,
            buttonTool62,
            buttonTool63,
            buttonTool64,
            popupMenuTool16,
            buttonTool66});
            this.tbrMain.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.tbrMain_ToolClick);
            // 
            // _frmMain_Toolbars_Dock_Area_Right
            // 
            this._frmMain_Toolbars_Dock_Area_Right.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Right.BackColor = System.Drawing.SystemColors.Control;
            this._frmMain_Toolbars_Dock_Area_Right.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Right;
            this._frmMain_Toolbars_Dock_Area_Right.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMain_Toolbars_Dock_Area_Right.Location = new System.Drawing.Point(776, 26);
            this._frmMain_Toolbars_Dock_Area_Right.Name = "_frmMain_Toolbars_Dock_Area_Right";
            this._frmMain_Toolbars_Dock_Area_Right.Size = new System.Drawing.Size(0, 567);
            this._frmMain_Toolbars_Dock_Area_Right.ToolbarsManager = this.tbrMain;
            // 
            // _frmMain_Toolbars_Dock_Area_Top
            // 
            this._frmMain_Toolbars_Dock_Area_Top.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Top.BackColor = System.Drawing.SystemColors.Control;
            this._frmMain_Toolbars_Dock_Area_Top.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Top;
            this._frmMain_Toolbars_Dock_Area_Top.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMain_Toolbars_Dock_Area_Top.Location = new System.Drawing.Point(0, 0);
            this._frmMain_Toolbars_Dock_Area_Top.Name = "_frmMain_Toolbars_Dock_Area_Top";
            this._frmMain_Toolbars_Dock_Area_Top.Size = new System.Drawing.Size(776, 26);
            this._frmMain_Toolbars_Dock_Area_Top.ToolbarsManager = this.tbrMain;
            // 
            // _frmMain_Toolbars_Dock_Area_Bottom
            // 
            this._frmMain_Toolbars_Dock_Area_Bottom.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this._frmMain_Toolbars_Dock_Area_Bottom.BackColor = System.Drawing.SystemColors.Control;
            this._frmMain_Toolbars_Dock_Area_Bottom.DockedPosition = Infragistics.Win.UltraWinToolbars.DockedPosition.Bottom;
            this._frmMain_Toolbars_Dock_Area_Bottom.ForeColor = System.Drawing.SystemColors.ControlText;
            this._frmMain_Toolbars_Dock_Area_Bottom.Location = new System.Drawing.Point(0, 593);
            this._frmMain_Toolbars_Dock_Area_Bottom.Name = "_frmMain_Toolbars_Dock_Area_Bottom";
            this._frmMain_Toolbars_Dock_Area_Bottom.Size = new System.Drawing.Size(776, 0);
            this._frmMain_Toolbars_Dock_Area_Bottom.ToolbarsManager = this.tbrMain;
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(776, 593);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.uebLeft);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Left);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Right);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Bottom);
            this.Controls.Add(this._frmMain_Toolbars_Dock_Area_Top);
            this.IsMdiContainer = true;
            this.Name = "frmMain";
            this.Text = "ACH Systm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Shown += new System.EventHandler(this.frmMain_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.uebLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbrMain)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        public bool GetUser(string UserName, string Password)
        {
            SqlDataReader dr = null;
            DataUser data = new DataUser();
            ArrayList prms = new ArrayList();


            try
            {
                prms.Add(new SqlParameter("@LoginID", UserName));
                prms.Add(new SqlParameter("@Password", Password));
                dr = data.SelectUserLogin(prms);
                if (dr.Read())
                {
                    main.g_User.UserID = Convert.ToInt32(dr["UserID"]);
                    main.g_User.LoginID = dr["LoginID"].ToString();
                    main.g_User.FirstName = dr["Firstname"].ToString();
                    main.g_User.LastName = dr["Lastname"].ToString();
                    main.g_User.IsAdmin = DataLayer.Field2Bool(dr["IsAdmin"]);
                    main.g_User.ProcessModule = DataLayer.Field2Bool(dr["ProcessModule"]);
                    main.g_User.ReportModule = DataLayer.Field2Bool(dr["ReportModule"]);
                    main.g_User.UtilityModule = DataLayer.Field2Bool(dr["UtilityModule"]);
                    main.g_User.LookUpModule = DataLayer.Field2Bool(dr["LookUpModule"]);
                    main.g_User.AdminModule = DataLayer.Field2Bool(dr["AdminModule"]);
                    main.g_User.SearchModule = DataLayer.Field2Bool(dr["SearchModule"]);
                    main.g_User.RiskModule = DataLayer.Field2Bool(dr["RiskModule"]);

                    main.Current_User = UserName;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                dr.Close();
                dr = null;
            }


        }


        public bool ChangeUserPassword(string UserName, string OldPassword, string NewPassword)
        {
            DataUser data = new DataUser();
            ArrayList prms = new ArrayList();


            try
            {
                prms.Add(new SqlParameter("@LoginID", UserName));
                prms.Add(new SqlParameter("@OldPassword", OldPassword));
                prms.Add(new SqlParameter("@NewPassword", NewPassword));
                int rows = data.UpdateUserPassword(prms);

                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }

        }

        private void OpenForm(string key)
        {
            Form frm = null;
            Ach_Form_Type type = Ach_Form_Type.Standard;

            foreach (Form form in this.MdiChildren)
            {
                if (form.Text == key)
                {
                    form.Activate();
                    return;
                }
            }

            switch (key)
            {
                case "Account Block":
                    frm = new frmSearch(ACH_Search.Account_Block, new frmAccountBlock());
                    break;
                case "Ach Recurring":
                    frm = new frmSearch(ACH_Search.Ach_Recurring, new frmAchRecurring());
                    break;
                case "Activity Log":
                    frm = new frmSearch(ACH_Search.Log, new frmLog());
                    break;
                case "Bad Returns":
                    frm = new frmSearch(ACH_Search.Bad_Return, null);
                    break;
                case "Bank":
                    frm = new frmSearch(ACH_Search.Bank, new frmBank());
                    break;
                case "Batch":
                    frm = new frmSearch(ACH_Search.Batch, new frmBatch());
                    break;
                case "Batch Detail":
                    frm = new frmSearch(ACH_Search.Batch_Detail, null);
                    break;
                case "Batch File Log":
                    frm = new frmSearch(ACH_Search.Batch_File_Log, null);
                    break;
                case "Settlement Forecast":
                    frm = new frmSearch(ACH_Search.SettlementForecast, null);
                    break;
                case "Close Month End":
                    frm = new frmMonthEnd();
                    type = Ach_Form_Type.Wizard;
                    break;
                case "EFT":
                    frm = new frmSearch(ACH_Search.Pending, new frmEFT());
                    break;
                case "Group Merchant":
                    frm = new frmSearch(ACH_Search.Group_Merchant, new frmGroupMerchant());
                    break;
                case "Hold":
                    frm = new frmSearch(ACH_Search.Hold, new frmHold());
                    break;
                case "Hold Releases":
                    frm = new frmHoldRelease();
                    break;
                case "NCAL Split Funding":
                    frm = new frmSplitFile();
                    break;
                case "Holiday":
                    frm = new frmSearch(ACH_Search.Holiday, null);
                    break;
                case "Journal":
                    frm = new frmSearch(ACH_Search.Journal, new frmJournal());
                    break;
                case "Journal Refcode":
                    frm = new frmSearch(ACH_Search.Refcode, new frmRefcode());
                    break;
                case "Load Returns":
                    frm = new frmLoadReturns();
                    type = Ach_Form_Type.Wizard;
                    break;
                case "Merchant":
                    frm = new frmSearch(ACH_Search.Merchant, new frmMerchant());
                    break;
                case "Merchant Balance":
                    frm = new frmSearch(ACH_Search.Merchant_Balance, null);
                    break;
                case "Balance and Fee":
                    frm = new frmBalanceFee();
                    break;
                case "Origin":
                    frm = new frmSearch(ACH_Search.Origin, new frmOrigin());
                    break;
                case "Over Ticket Items":
                    frm = new frmSearch(ACH_Search.Over_Ticket_Items, new frmTransaction());
                    break;
                case "Return Rates":
                    frm = new frmSearch(ACH_Search.ReturnRates, null);
                    break;
                case "Return Summary":
                    frm = new frmSearch(ACH_Search.ReturnSummary, null);
                    break;
                case "OT-Monthly Activity Totals":
                    frm = new frmSearch(ACH_Search.OT_Monthly_Activity_Totals, null);
                    break;
                case "Monthly Activity Totals":
                    frm = new frmSearch(ACH_Search.Monthly_Activity_Totals, null);
                    break;
                case "Neg Balance Return":
                    frm = new frmSearch(ACH_Search.Negative_Balance_Return, null);
                    break;
                case "Unauthorized Return Rates":
                    frm = new frmSearch(ACH_Search.Unauthorized_Return_Rates, null);
                    break;
                case "Exceed Average Ticket":
                    frm = new frmSearch(ACH_Search.ExceedAverageTicket, null);
                    break;
                case "Exceed Monthly Volume":
                    frm = new frmSearch(ACH_Search.ExceedMonthlyVolume, null);
                    break;
                case "Process Log":
                    frm = new frmSearch(ACH_Search.Process_Log, null);
                    break;
                case "Processing Engine":
                    frm = new frmAchProcessEngine();
                    type = Ach_Form_Type.Wizard;
                    break;
                case "Purge Batch":
                    frm = new frmSearch(ACH_Search.Purge_Upload_Batch, null);
                    break;
                case "Return":
                    frm = new frmSearch(ACH_Search.Return, new frmReturn());
                    break;
                case "Return Reason Code":
                    frm = new frmSearch(ACH_Search.Reason_Code, new frmReturnReasonCode());
                    break;
                case "Scrub Data":
                    frm = new frmScrubData();
                    break;
                case "SECC Codes":
                    frm = new frmSearch(ACH_Search.Secc, new frmSecc());
                    break;
                case "Settlement File Summary":
                    frm = new frmSettleFileSummaryReport();
                    break;
                case "Return Totals":
                    frm = new frmSearch(ACH_Search.ReturnTotals, null);
                    break;
                case "Select Default Merchant":
                    UltraGridRow row = PickerHandler.PickMerchant();
                    if (row != null)
                    {
                        TextBoxTool txt = (TextBoxTool)tbrMain.Toolbars[0].Tools["txtDefaultMerchant"];
                        txt.Tag = row.Cells["AchID"].Value.ToString();
                        txt.Text = row.Cells["AchID"].Value.ToString() + " - " + row.Cells["Merchant Name"].Value.ToString();

                        foreach (Form frm1 in this.MdiChildren)
                        {
                            if (frm1.Name == "frmSearch")
                            {
                                frmSearch frm2 = (frmSearch)frm1;
                                frm2.FormClear();
                                frm2.SetDefaultAchID(txt.Tag.ToString());
                            }
                        }

                    }

                    frm = null;
                    break;
                case "Source":
                    frm = new frmSearch(ACH_Search.Source, new frmSource());
                    break;
                case "Statement":
                    frm = new frmStatement();
                    break;
                case "Trans Status Code":
                    frm = new frmSearch(ACH_Search.TransStatus, new frmTransStatus());
                    break;
                case "Trans Type":
                    frm = new frmSearch(ACH_Search.TransType, new frmTransType());
                    break;
                case "Transaction":
                    frm = new frmSearch(ACH_Search.Transaction, new frmTransaction());
                    break;
                case "User":
                    frm = new frmSearch(ACH_Search.User, new frmUser());
                    break;
                case "Change Password":
                    this.ChangePassword();
                    return;
                    break;
                case "Exit":
                    this.Close();
                    break;

            }

            if (frm == null)
                return;

            frm.Text = key;
            frm.MdiParent = this;

            switch (type)
            {
                case Ach_Form_Type.Wizard:
                    frm.MaximizeBox = false;
                    frm.FormBorderStyle = FormBorderStyle.FixedSingle;
                    frm.WindowState = FormWindowState.Normal;
                    frm.StartPosition = FormStartPosition.CenterScreen;
                    break;
                case Ach_Form_Type.Modal:
                    break;
                case Ach_Form_Type.Standard:
                    frm.WindowState = FormWindowState.Maximized;
                    break;
            }

            frm.Show();




        }

        private void ChangePassword()
        {
            frmChangePassword frm = new frmChangePassword();
            int tries = 3;
            bool perform = false;

            while (tries > 0)
            {
                if (frm.ShowDialog() == DialogResult.Cancel)
                    break;

                if (!this.ChangeUserPassword(main.Current_User, frm.txtOldPassword.Text, frm.txtNewPassword.Text))
                {
                    tries--;
                    switch (tries)
                    {
                        case 0:
                            FormHandler.DispalyWarningMessage("Password change failed. You're out of luck. Bye! Bye!");
                            break;
                        case 1:
                            FormHandler.DispalyWarningMessage("Password change failed. Last try.");
                            break;
                        default:
                            FormHandler.DispalyWarningMessage("Password change failed. You have " + tries.ToString() + " tries remaining.");
                            break;
                    }
                }
                else
                {
                    perform = true;
                    break;
                }
            }
            frm = null;

            if (perform)
                FormHandler.DispalyInformationMessage("Password changed successfully!");

        }
        private void uebLeft_ItemClick(object sender, Infragistics.Win.UltraWinExplorerBar.ItemEventArgs e)
        {
            this.OpenForm(e.Item.Text);
        }

        private void tbrMain_ToolClick(object sender, ToolClickEventArgs e)
        {
            this.OpenForm(e.Tool.Key);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            frmLogin frm = new frmLogin();
            int tries = 3;
            bool perform = false;

            while (tries > 0)
            {
                if (frm.ShowDialog() == DialogResult.Cancel)
                    break;

                if (!this.GetUser(frm.txtUserName.Text, frm.txtPassword.Text))
                {
                    tries--;
                    switch (tries)
                    {
                        case 0:
                            FormHandler.DispalyWarningMessage("Authentication failed. You're out of luck. Bye! Bye!");
                            break;
                        case 1:
                            FormHandler.DispalyWarningMessage("Authentication failed. Last try.");
                            frm.txtPassword.Text = "";
                            break;
                        default:
                            FormHandler.DispalyWarningMessage("Authentication failed. You have " + tries.ToString() + " tries remaining.");
                            frm.txtPassword.Text = "";
                            break;
                    }
                }
                else
                {
                    perform = true;
                    break;
                }
            }
            frm = null;

            if (perform)
                this.SetFormSecurity();
            else
                this.Close();


        }

        public void SetFormSecurity()
        {
            uebLeft.Groups["Search Module"].Visible = main.g_User.SearchModule;
            uebLeft.Groups["Process Module"].Visible = main.g_User.ProcessModule;
            uebLeft.Groups["Report Module"].Visible = main.g_User.ReportModule;
            uebLeft.Groups["Utility Module"].Visible = main.g_User.UtilityModule;
            uebLeft.Groups["Lookup Module"].Visible = main.g_User.LookUpModule;
            uebLeft.Groups["Admin Module"].Visible = main.g_User.AdminModule;
            uebLeft.Groups["Risk Module"].Visible = main.g_User.RiskModule;

            tbrMain.Toolbars[0].Tools["Search"].SharedProps.Visible = main.g_User.SearchModule;
            tbrMain.Toolbars[0].Tools["Process"].SharedProps.Visible = main.g_User.ProcessModule;
            tbrMain.Toolbars[0].Tools["Report"].SharedProps.Visible = main.g_User.ReportModule;
            tbrMain.Toolbars[0].Tools["Utility"].SharedProps.Visible = main.g_User.UtilityModule;
            tbrMain.Toolbars[0].Tools["Lookup"].SharedProps.Visible = main.g_User.LookUpModule;
            tbrMain.Toolbars[0].Tools["Admin"].SharedProps.Visible = main.g_User.AdminModule;

            this.Refresh();
        }
        private void mnuTop_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                frmAdmin frm = new frmAdmin();
                DialogResult dr = frm.ShowDialog();
                if (dr == DialogResult.OK)
                    this.SetFormSecurity();

                frm = null;
            }
        }

        private void CloseAllWindow()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
                frm.Dispose();
            }
        }

        //private void cboMerchantIDs_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cboMerchantIDs.Tag.ToString() != cboMerchantIDs.Text)
        //    {
        //        cboMerchantIDs.Tag = cboMerchantIDs.Text;
        //        foreach (Form frm in this.MdiChildren)
        //        {
        //            if (frm.Name == "frmSearch")
        //            {
        //                frmSearch frm2 = (frmSearch)frm;
        //                frm2.FormClear();

        //                if (cboMerchantIDs.SelectedIndex != 0)
        //                    frm2.SetDefaultMerchantID();
        //            }
        //        }
        //    }

        //}

        private void mnuCloseAllWindow_Click(object sender, EventArgs e)
        {
            this.CloseAllWindow();
        }







    }
}
