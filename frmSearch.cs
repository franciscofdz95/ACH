using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win.UltraWinToolbars;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public enum ACH_Search
    {
        Account_Block,
        Ach_Recurring,
        Bad_Return,
        Bank,
        Batch,
        Batch_Detail,
        Batch_File_Log,
        ExceedAverageTicket,
        ExceedMonthlyVolume,
        Group_Merchant,
        Hold,
        Holiday,
        Journal,
        Log,     
        Merchant,
        Merchant_Balance,
        Monthly_Activity_Totals,
        Negative_Balance_Return,
        Origin,
        OT_Monthly_Activity_Totals,
        Over_Ticket_Items,
        Pending,
        Process_Log,
        Purge_Upload_Batch,
        Reason_Code,
        Refcode,
        Return,
        ReturnRates,        
        ReturnSummary,
        ReturnTotals,
        Secc,
        SettlementForecast,
        Source,
        Transaction,
        TransStatus,
        TransType,
        Unauthorized_Return_Rates,
        User
    }

    public partial class frmSearch : Form
    {
        public UserControl m_Ctrl = null;
        public iFormData m_Frm = null;
        SearchBase m_Data = null;
        DataSet m_Ds = null;
        UltraGridRow m_Row = null;
        bool m_Show = false;
        ACH_Search m_Search;
        bool m_DisplayPicker = false;

        public frmSearch()
        {
            InitializeComponent();
        }

        //private void CreateToolBarButton(string Key,int ImageIndex )
        //{
        //    ButtonTool btn = new ButtonTool(Key);
        //    btn.SharedProps.DisplayStyle = ToolDisplayStyle.ImageAndText;
        //    btn.SharedProps.Caption = Key;
        //    btn.SharedProps.AppearancesSmall.Appearance.Image = ImageIndex;
        //    this.tbrSearch.Tools.Add(btn);
        //    this.tbrSearch.Toolbars[0].Tools.AddTool(Key);
        //    //this.tbrSearch.Tools[Key].InstanceProps.IsFirstInGroup = true;
        //    this.tbrSearch.Toolbars[0].Tools[Key].InstanceProps.IsFirstInGroup = true;

        //}



        public frmSearch(ACH_Search search,iFormData frm)
        {
            InitializeComponent();

            m_Search = search;
            m_Frm = frm;

            if (m_Frm != null)
                m_Frm.SearchForm = this;


            FormHandler.CreateToolBarButton(tbrSearch,"Search");
            FormHandler.CreateToolBarButton(tbrSearch, "Clear");
            FormHandler.CreateToolBarButton(tbrSearch, "Export to Excel");

            switch (search)
            {
                case ACH_Search.Ach_Recurring:
                    m_Data = new SearchAchRecurring();
                    break;
                case ACH_Search.Transaction:
                    m_Data = new SearchTransaction();
                    //CreateToolBarButton("Mass Update Trans Status");
                    //CreateToolBarButton("Mass Update Next Process Date");
                    break;
                case ACH_Search.Over_Ticket_Items:
                    m_Data = new SearchOverTicketItems();
                    FormHandler.CreateToolBarButton(tbrSearch,"Release Selected OverTicket Items");
                    break;
                case ACH_Search.Account_Block:
                    m_Data = new SearchAccountBlock();
                    //tbrTop.Items.Add("Release Account Block", imgList.Images[8]);
                    //tbrTop.Items.Add(new ToolStripSeparator());
                    //tbrTop.Items.Add("Add Account Block", imgList.Images[2]);
                    //tbrTop.Items.Add(new ToolStripSeparator());
                    break;
                case ACH_Search.Purge_Upload_Batch:
                    m_Data = new SearchPurgeBatch();
                    FormHandler.CreateToolBarButton(tbrSearch,"Delete Selected Upload Batch");
                    //pnlMessage.Visible = true;
                    break;
                case ACH_Search.Log:
                    m_Data = new SearchLog();
                    break;
                case ACH_Search.Return:
                    m_Data = new SearchReturn();
                    break;
                case ACH_Search.Merchant:
                    m_Data = new SearchMerchant();
                    break;
                case ACH_Search.Merchant_Balance:
                    m_Data = new SearchMerchantBalance();
                    break;
                case ACH_Search.Bank:
                    m_Data = new SearchBank();
                    break;
                case ACH_Search.Journal:
                    m_Data = new SearchJournal();
                    break;
                case ACH_Search.Hold:
                    m_Data = new SearchHold();
                    break;
                case ACH_Search.Pending:
                    m_Data = new SearchPending();
                    break;
                case ACH_Search.User:
                    m_Data = new SearchUser();
                    break;
                case ACH_Search.TransStatus:
                    m_Data = new SearchTransStatus();
                    break;
                case ACH_Search.Reason_Code:
                    m_Data = new SearchReasonCode();
                    break;
                case ACH_Search.Holiday:
                    m_Data = new SearchHoliday();
                    break;
                case ACH_Search.Group_Merchant:
                    m_Data = new SearchGroupMerchant();
                    break;
                case ACH_Search.Batch:
                    m_Data = new SearchBatch();
                    break;
                case ACH_Search.Batch_Detail:
                    m_Data = new SearchBatchDetail();
                    break;
                case ACH_Search.Refcode:
                    m_Data = new SearchRefcode();
                    break;
                case ACH_Search.Origin:
                    m_Data = new SearchOrigin();
                    break;
                case ACH_Search.Secc:
                    m_Data = new SearchSecc();
                    break;
                case ACH_Search.Source :
                    m_Data = new SearchSource();
                    break;
                case ACH_Search.TransType:
                    m_Data = new SearchTransType ();
                    break;
                case ACH_Search.Process_Log:
                    m_Data = new SearchProcessLog();
                    break;
                case ACH_Search.Batch_File_Log:
                    m_Data = new SearchBatchFileLog(); 
                    break;
                case ACH_Search.SettlementForecast:
                    m_Data = new SearchSettlementForecast();
                    break;
                case ACH_Search.Bad_Return:
                    m_Data = new SearchBadReturn();
                    break;
                case ACH_Search.ReturnRates:
                    m_Data = new SearchReturnRates();
                    break;
                case ACH_Search.ReturnSummary:
                    m_Data = new SearchReturnSummary();
                    break;
                case ACH_Search.ReturnTotals:
                    m_Data = new SearchReturnTotals();
                    break;
                case ACH_Search.OT_Monthly_Activity_Totals:
                    m_Data = new SearchOTMonthlyActivityTotals();
                    break;
                case ACH_Search.Monthly_Activity_Totals:
                    m_Data = new SearchMonthlyActivityTotals();
                    break;
                case ACH_Search.Negative_Balance_Return:
                    m_Data = new SearchNegativeBalanceReport();
                    break;
                case ACH_Search.Unauthorized_Return_Rates:
                    m_Data = new SearchUnauthorizedReturnRates();
                    break;
                case ACH_Search.ExceedAverageTicket:
                    m_Data = new SearchExceedAverageTicket();
                    break;
                case ACH_Search.ExceedMonthlyVolume:
                    m_Data = new SearchExceedMonthlyVolume();
                    break;
            }


            if (frm != null || m_Search == ACH_Search.Batch_Detail)
                FormHandler.CreateToolBarButton(tbrSearch,"Edit");

            if ((main.g_User.IsAdmin  && frm != null && search != ACH_Search.Log) || search == ACH_Search.Account_Block)
            {
                FormHandler.CreateToolBarButton(tbrSearch,"New");
                FormHandler.CreateToolBarButton(tbrSearch,"Delete");
            }

            if (frm != null && m_Search == ACH_Search.Transaction)
                FormHandler.CreateToolBarButton(tbrSearch, "Mass Update");

            FormHandler.CreateToolBarButton(tbrSearch,"Close");

            m_Ctrl = m_Data.m_Ctrl;
            this.Text = search.ToString().Replace("_", " ");
            pnlSearch.Height = m_Data.m_Ctrl.Height;
            pnlSearch.Controls.Add(m_Ctrl);
            m_Ctrl.Dock = DockStyle.Fill;

            TextBoxTool txt = (TextBoxTool)main.g_frmMain.tbrMain.Toolbars[0].Tools["txtDefaultMerchant"];

            if (txt.Tag != null)
                if (txt.Tag.ToString().Trim() != string.Empty)
                    this.SetDefaultAchID(txt.Tag.ToString().Trim());

            m_Show = true;
            this.FormSearchLoad();
            m_Show = false;


            //if (main.g_frmMain.cboMerchantIDs.SelectedIndex > 0)
            //{
            //    m_Show = true;
            //    this.FormSearch();
            //    m_Show = false;
            //}
            //else
            //{
            //    this.SetDefaultMerchantID();
            //    this.FormSearch();
            //}

            if (!main.g_User.IsAdmin)
            {
                //if (tbrSearch.Toolbars[0].Tools.Exists("Export to Excel"))
                //    tbrSearch.Toolbars[0].Tools["Export to Excel"].SharedProps.Visible = false;

                if (tbrSearch.Toolbars[0].Tools.Exists("Edit"))
                    tbrSearch.Toolbars[0].Tools["Edit"].SharedProps.Visible = false;

                if (tbrSearch.Toolbars[0].Tools.Exists("Mass Update"))
                    tbrSearch.Toolbars[0].Tools["Mass Update"].SharedProps.Visible = false;
            }

        }

        public frmSearch(ACH_Search search,iFormData frm, bool DisplayPicker)
        {
            InitializeComponent();

            m_DisplayPicker = DisplayPicker;
            m_Search = search;
            m_Frm = frm;

            if (m_Frm != null)
                m_Frm.SearchForm = this;

            this.MinimizeBox = false;
            this.MaximizeBox = false;

            FormHandler.CreateToolBarButton(tbrSearch,"Search");
            FormHandler.CreateToolBarButton(tbrSearch,"Clear");
            FormHandler.CreateToolBarButton(tbrSearch,"Export to Excel");

            pnlMessage.Visible = false;
            switch (search)
            {
                case ACH_Search.Ach_Recurring:
                    m_Data = new SearchAchRecurring();
                    break;
                case ACH_Search.Transaction:
                    m_Data = new SearchTransaction();
                    break;
                case ACH_Search.Account_Block:
                    m_Data = new SearchAccountBlock();
                    break;
                case ACH_Search.Purge_Upload_Batch:
                    m_Data = new SearchPurgeBatch();
                    break;
                case ACH_Search.Log:
                    m_Data = new SearchLog();
                    break;
                case ACH_Search.Return:
                    m_Data = new SearchReturn();
                    break;
                case ACH_Search.Merchant:
                    m_Data = new SearchMerchant();
                    break;
                case ACH_Search.Bank:
                    m_Data = new SearchBank();
                    break;
                case ACH_Search.Journal:
                    m_Data = new SearchJournal();
                    break;
                case ACH_Search.Hold:
                    m_Data = new SearchHold();
                    break;
                case ACH_Search.Pending:
                    m_Data = new SearchPending();
                    break;
                case ACH_Search.User:
                    m_Data = new SearchUser();
                    break;
                case ACH_Search.TransStatus:
                    m_Data = new SearchTransStatus();
                    break;
                case ACH_Search.Reason_Code:
                    m_Data = new SearchReasonCode();
                    break;
                case ACH_Search.Batch:
                    m_Data = new SearchBatch();
                    break;
                case ACH_Search.Batch_Detail:
                    m_Data = new SearchBatchDetail();
                    break;
                case ACH_Search.Bad_Return:
                    m_Data = new SearchBadReturn();
                    break;
            }

            FormHandler.CreateToolBarButton(tbrSearch,"Select Record");
            FormHandler.CreateToolBarButton(tbrSearch,"Close");

            m_Ctrl = m_Data.m_Ctrl;
            this.Text = search.ToString().Replace("_", " ");
            m_Ctrl.Dock = DockStyle.Fill;
            pnlSearch.Height = m_Data.m_Ctrl.Height;
            pnlSearch.Controls.Add(m_Ctrl);

            //TextBoxTool txt = (TextBoxTool)main.g_frmMain.tbrMain.Toolbars[0].Tools["txtDefaultMerchant"];

            //if (txt.Tag != null)
            //    if (txt.Tag.ToString().Trim() != string.Empty)
            //        this.SetDefaultAchID(txt.Tag.ToString().Trim());

            m_Show = true;
            this.FormSearchLoad();
            m_Show = false;

            if (!main.g_User.IsAdmin)
            {
                //if (tbrSearch.Toolbars[0].Tools.Exists("Export to Excel"))
                //    tbrSearch.Toolbars[0].Tools["Export to Excel"].SharedProps.Visible = false;

                if (tbrSearch.Toolbars[0].Tools.Exists("Edit"))
                    tbrSearch.Toolbars[0].Tools["Edit"].SharedProps.Visible = false;

                if (tbrSearch.Toolbars[0].Tools.Exists("Mass Update"))
                    tbrSearch.Toolbars[0].Tools["Mass Update"].SharedProps.Visible = false;
            }


        }

        public void SetDefaultAchID(string AchID)
        {

            foreach (Control ctrl in m_Ctrl.Controls)
            {
                if (ctrl.Controls.Count > 0)
                {
                    foreach (Control ctrl2 in ctrl.Controls)
                    {
                        if (ctrl2.Name == "txtAchID")
                        {
                            ctrl2.Text = AchID;
                        }
                    }
                }
            }

        }

        private void tbrTop_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            DialogResult result;
            Form frm;

            switch (e.ClickedItem.Text)
            {
                case "Release Selected OverTicket Items":
                    result = FormHandler.DispalyQuestionMessage("Are you sure you want to release the selected over ticket items?");

                    if (result == DialogResult.No)
                        return;

                    DataTransaction data = new DataTransaction();
                    ArrayList prms = new ArrayList();

                    foreach (UltraGridRow dr in grdSearch.Selected.Rows)
                    {
                        prms.Clear();
                        prms.Add(new SqlParameter("@TransID", dr.Cells["Trans ID"].Value));
                        prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));
                        data.UpdateReleaseOverTicketItem(prms);
                    }

                    data = null;
                    if (this.FormDataCheck())
                        this.FormSearch();
                    break;

                //case "Mass Update":
                //    frm = new frmUpdateTransaction(e.ClickedItem.Text, m_Ds);
                //    frm.ShowDialog();
                //    if (this.FormDataCheck())
                //        this.FormSearch();

                //    frm = null;
                //    break;
                //case "Mass Update Next Process Date":
                //    frm = new frmUpdateTransaction(e.ClickedItem.Text, m_Ds);
                //    frm.ShowDialog();
                //    if (this.FormDataCheck())
                //        this.FormSearch();

                //    frm = null;
                //    break;
                case "Delete Selected Upload Batch":
                    if (this.PurgeBatch())
                    {
                        if (this.FormDataCheck())
                            this.FormSearch();
                    }
                    
                    break;
                case "&Search":
                    if (this.FormDataCheck())
                        this.FormSearch();

                    break;
                case "Clea&r":
                    this.FormClear();
                    break;
                case "&Edit":
                    this.SelectCurrentRecord();
                    break;
                case "&Export to Excel":
                    FormHandler.ExportGridToExcel(grdSearch);

                    break;
                case "&New":
                    m_Frm.FormNew();

                    if (m_Frm.IsDirty)
                        if (this.FormDataCheck())
                            this.FormSearch();
                    break;
                case "&Delete":
                    this.FormDelete();
                    break;
                case "&Close":
                    this.Close();
                    break;
                case "Select Record":
                    this.SelectPicker();
                    break;
            }
        }

        private void SelectPicker()
        {
            if (grdSearch.Rows.Count == 0)
                return;

            if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count > 1)
            {
                FormHandler.DispalyInformationMessage("Please select a row");
                return;
            }

            if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count == 1)
                grdSearch.Rows[0].Selected = true;

            m_Row = grdSearch.ActiveRow;
            this.Close();
        }

        private void tbrSearch_ToolClick(object sender, ToolClickEventArgs e)
        {
            DialogResult result;
            Form frm;

            switch (e.Tool.Key)
            {
                case "Release Selected OverTicket Items":
                    result = FormHandler.DispalyQuestionMessage("Are you sure you want to release the selected over ticket items?");

                    if (result == DialogResult.No)
                        return;

                    DataTransaction data = new DataTransaction();
                    ArrayList prms = new ArrayList();

                    foreach (UltraGridRow dr in grdSearch.Selected.Rows)
                    {
                        prms.Clear();
                        prms.Add(new SqlParameter("@TransID", dr.Cells["TransID"].Value));
                        prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));
                        data.UpdateReleaseOverTicketItem(prms);
                    }

                    data = null;
                    if (this.FormDataCheck())
                        this.FormSearch();
                    break;

                case "Mass Update":
                    frm = new frmUpdateTransaction(grdSearch);
                    frm.ShowDialog();
                    if (this.FormDataCheck())
                        this.FormSearch();

                    frm = null;
                    break;
                //case "Mass Update Next Process Date":
                //    frm = new frmUpdateTransaction(e.Tool.Key, m_Ds);
                //    frm.ShowDialog();
                //    if (this.FormDataCheck())
                //        this.FormSearch();

                //    frm = null;
                //    break;
                case "Delete Selected Upload Batch":
                    if (this.PurgeBatch())
                    {
                        if (this.FormDataCheck())
                            this.FormSearch();
                    }

                    break;
                case "Search":
                    if (this.FormDataCheck())
                        this.FormSearch();

                    break;
                case "Clear":
                    this.FormClear();
                    break;
                case "Edit":
                    this.SelectCurrentRecord();
                    break;
                case "Export to Excel":
                    FormHandler.ExportGridToExcel(grdSearch);

                    break;
                case "New":
                    m_Frm.FormNew();

                    if (m_Frm.IsDirty)
                        if (this.FormDataCheck())
                            this.FormSearch();
                    break;
                case "Delete":
                    this.FormDelete();
                    break;
                case "Close":
                    this.Close();
                    break;
                case "Select Record":
                    if (grdSearch.Rows.Count == 0)
                        return;

                    if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count > 1)
                    {
                        FormHandler.DispalyInformationMessage("Please select a row");
                        return;
                    }

                    if (grdSearch.Selected.Rows.Count == 0 && grdSearch.Rows.Count == 1)
                        grdSearch.Rows[0].Selected = true;

                    m_Row = grdSearch.ActiveRow;
                    this.Close();
                    break;
            }
        }
        
        private void FormDelete()
        {
            if (grdSearch.Selected.Rows.Count == 0)
                return;

            DialogResult result = FormHandler.DispalyQuestionMessage("Do you want to delete the selected records?");
            if (result == DialogResult.No)
                return;

            foreach (UltraGridRow dr in grdSearch.Selected.Rows)
            {
                m_Frm.FormDelete(dr, false);

            }

            if (this.FormDataCheck())
                this.FormSearch();

        }

        private bool PurgeBatch()
        {
            if (grdSearch.Selected.Rows.Count == 0)
            {
                FormHandler.DispalyInformationMessage("Please select a batch.");
                return false;
            }

            ArrayList prms = new ArrayList();
            DataTransaction data = new DataTransaction();

            UltraGridRow dr = grdSearch.Selected.Rows[0];

            if (Convert.ToInt32(dr.Cells["Total Record Count"].Value) != Convert.ToInt32(dr.Cells["Total Open Record"].Value))
            {
                FormHandler.DispalyErrorMessage("Unable to delete batch.  Total Record Count and Total Open Record must be equal.");
                return false;
            }

            DialogResult result;
            result = FormHandler.DispalyQuestionMessage("Are you sure you want to delete Upload ID " + dr.Cells["Upload ID"].Value.ToString() + ".");

            if (result == DialogResult.No)
            {
                return false;
            }

            prms.Add(new SqlParameter("@AchID", Convert.ToInt32(dr.Cells["ACH ID"].Value)));
            prms.Add(new SqlParameter("@UploadID", Convert.ToInt32(dr.Cells["Upload ID"].Value)));
            prms.Add(new SqlParameter("@UserID", main.Current_User));

            if (data.DeleteUploadBatch(prms) == 0)
                FormHandler.DispalyErrorMessage("Failed to delete Upload ID " + dr.Cells["Upload ID"].Value.ToString() + ".");

            return true;
        }

        public UltraGridRow PickRecord(iFormData data)
        {
            m_Frm = data;
            this.ShowDialog();
            return m_Row;
        }

        public void FormClear()
        {
            ArrayList prms = new ArrayList();

            BindingSource bs = (BindingSource)grdSearch.DataSource;
            DataTable dt = (DataTable)bs.DataSource;
            dt.Rows.Clear();

            this.ClearParamters(m_Ctrl);
        }

        private void ClearParamters(Control ctrls)
        {
            ComboBox cbo = null;
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Tag != null)
                {
                    SearchFieldInfo pfi = (SearchFieldInfo)ctrl.Tag;

                    switch (pfi.FieldType)
                    {
                        case Search_Field_Type.Int:
                        case Search_Field_Type.Number:
                        case Search_Field_Type.String:
                            ctrl.Text = string.Empty;
                            break;
                        case Search_Field_Type.Date:
                            if (ctrl is Infragistics.Win.UltraWinSchedule.UltraCalendarCombo)
                                ctrl.Text = string.Empty;
                            else
                                ctrl.Text = DateTime.Today.ToString();
                            break;
                        case Search_Field_Type.Bool:
                            CheckBox chk = (CheckBox)ctrl;
                            chk.Checked = false;
                            break;
                    }

                    if (ctrl.GetType().ToString() == "System.Windows.Forms.ComboBox")
                    {
                        cbo = (ComboBox)ctrl;
                        cbo.SelectedIndex = -1;
                    }

                }

                if (ctrl.Controls.Count > 0)
                {
                    this.ClearParamters(ctrl);
                }
            }
        }
        public bool FormDataCheck()
        {
            string strError = string.Empty;
            strError = m_Data.FormCheck(m_Ctrl);
            if (strError != string.Empty)
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void GetParameters(ArrayList prms, Control ctrls)
        {
            string strValue = string.Empty;
            AchListItem item = null;
            ComboBox cbo = null;
            foreach (Control ctrl in ctrls.Controls)
            {
                if (ctrl.Tag != null)
                {

                    strValue = ctrl.Text;

                    if (strValue != string.Empty)
                    {

                        if (ctrl.GetType().ToString() == "System.Windows.Forms.ComboBox")
                        {
                            cbo = (ComboBox)ctrl;
                            item = (AchListItem)cbo.SelectedItem;
                            strValue = item.ItemValue;
                        }
                    }

                    if (strValue != string.Empty)
                    {
                        SearchFieldInfo pfi = (SearchFieldInfo)ctrl.Tag;
                        switch (pfi.FieldType)
                        {
                            case Search_Field_Type.Number:
                                prms.Add(new SqlParameter(pfi.ParamterName, Convert.ToDecimal(strValue)));
                                break;
                            case Search_Field_Type.Bool:
                                CheckBox chk = (CheckBox)ctrl;
                                prms.Add(new SqlParameter(pfi.ParamterName, Convert.ToBoolean(chk.Checked)));
                                break;
                            default:
                                prms.Add(new SqlParameter(pfi.ParamterName, strValue));
                                break;
                        }
                    }
                }

                if (ctrl.Controls.Count > 0)
                {
                    this.GetParameters(prms,ctrl);
                }
            }
        }
        public void FormSearch()
        {
            this.Cursor = Cursors.WaitCursor ;

            if (!m_Show)
                main.g_frmStatus.Text  = "Searching " + this.Text + " table.";

            ArrayList prms = new ArrayList();
            //string strValue = string.Empty;
            //AchListItem item = null;
            //ComboBox cbo = null;

            this.GetParameters(prms, m_Ctrl);



            //foreach (Control ctrl in m_Ctrl.Controls)
            //{
            //    if (ctrl.Controls.Count > 0)
            //    {
            //        foreach (Control ctrl2 in ctrl.Controls)
            //        {
            //            if (ctrl2.Tag != null)
            //            {
                            
            //                strValue = ctrl2.Text;

            //                if (strValue != string.Empty)
            //                {

            //                    if (ctrl2.GetType().ToString() == "System.Windows.Forms.ComboBox")
            //                    {                            
            //                           cbo = (ComboBox) ctrl2;
            //                           item = (AchListItem) cbo.SelectedItem; 
            //                           strValue = item.ItemValue; 
            //                    }
            //                }

            //                if (strValue != string.Empty)
            //                {
            //                    SearchFieldInfo pfi = (SearchFieldInfo)ctrl2.Tag;
            //                    switch (pfi.FieldType)
            //                    {
            //                        case Search_Field_Type.Number:
            //                            prms.Add(new SqlParameter(pfi.ParamterName, Convert.ToDecimal(strValue)));
            //                            break;
            //                        case Search_Field_Type.Bool:
            //                            CheckBox chk = (CheckBox)ctrl2;
            //                            prms.Add(new SqlParameter(pfi.ParamterName, Convert.ToBoolean(chk.Checked)));
            //                            break;
            //                        default:
            //                            prms.Add(new SqlParameter(pfi.ParamterName, strValue));
            //                            break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            m_Ds = m_Data.FormSearch(prms);
            BindingSource bs = new BindingSource();
            bs.DataSource = m_Ds.Tables[0];
            //grdSearch.DataSource = null;
            //grdSearch.ResetDisplayLayout();
            //grdSearch.Layouts.Clear();
            grdSearch.DataSource = bs;

            foreach (DataColumn col in m_Ds.Tables[0].Columns)
            {
                switch (col.DataType.ToString())
                {                        
                    case "System.Int32":
                    case "System.Decimal":
                    case "System.Byte":
                    case "System.Double":
                        grdSearch.DisplayLayout.Bands[0].Columns[col.Ordinal].CellAppearance.TextHAlign = Infragistics.Win.HAlign.Right;
                        break;
                }
            }

            if (grdSearch.Rows.Count > 0)
                grdSearch.Rows[0].Selected = true;


            //DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            //buttonColumn.Name = "Details";
            //buttonColumn.HeaderText = "Details";
            //buttonColumn.Text = "View Details";
            //buttonColumn.UseColumnTextForButtonValue = true;
            //grdSearch.Columns.Insert(grdSearch.Columns.Count, buttonColumn);

            lblRecordCount.Text = "Records Found: " + m_Ds.Tables[0].Rows.Count;


            if (!m_Show)
                main.g_frmStatus.Text = string.Empty;

            this.Cursor = Cursors.Default;
        }

        public void FormSearchLoad()
        {
            this.Cursor = Cursors.WaitCursor;

            if (!m_Show)
                main.g_frmStatus.Text = "Searching " + this.Text + " table.";

            ArrayList prms = new ArrayList();

            if (m_Frm != null)
                prms.Add(new SqlParameter("@" + m_Frm.KeyColumnName, -1));

            m_Ds = m_Data.FormSearch(prms);
            BindingSource bs = new BindingSource();
            bs.DataSource = m_Ds.Tables[0];
            grdSearch.DataSource = bs;

            lblRecordCount.Text = "Records Found: " + m_Ds.Tables[0].Rows.Count;

            if (!m_Show)
                main.g_frmStatus.Text = string.Empty;

            this.Cursor = Cursors.Default;
        }

        //private void grdSearch_RowPrePaint(object sender, UltraGridRowPrePaintEventArgs e)
        //{
        //    m_Data.FormFormatList(grdSearch, e);
        //}

        private void frmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.FormDataCheck())
                    this.FormSearch();
            }
        }

        public UltraGridRow GetSelectedGridRow()
        {
            if (grdSearch.Selected.Rows.Count == 1)
                return m_Row;
            else
                return null;
        }

        private void SelectCurrentRecord()
        {
            if (m_Frm == null && m_Search != ACH_Search.Batch_Detail)
                return;


            if (grdSearch.Selected.Rows.Count == 1)
            {
                if (m_Search == ACH_Search.Batch_Detail)
                {
                    UltraGridRow dr = grdSearch.ActiveRow;
                    if (dr.Cells["Source"].Value.ToString() == "C")
                        FormHandler.OpenDataForm(new frmTransaction(), DataLayer.Int2Field(dr.Cells["Trans ID"].Value));
                    else
                        FormHandler.OpenDataForm(new frmEFT(), DataLayer.Int2Field(dr.Cells["Trans ID"].Value));
                }
                else
                {
                    m_Frm.FormOpen(grdSearch.Selected.Rows[0]);

                    if (m_Frm.IsDirty)
                    {
                        if (this.FormDataCheck())
                        {
                            this.FormSearch();
                        }
                        m_Frm.IsDirty = false;
                    }
                }

            }
            else if (grdSearch.Selected.Rows.Count > 1 )
            {
                FormHandler.DispalyInformationMessage("Can only view 1 record at a time.  Please select 1 record.");
            }
        }

        private void frmSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            m_Data = null;
            m_Ctrl = null;
            m_Ds = null;
            m_Frm = null;
        }

        private void grdSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.FormDelete();
            }
        }

        private void grdSearch_DoubleClickRow(object sender, DoubleClickRowEventArgs e)
        {
            if (m_DisplayPicker)
                this.SelectPicker();
            else
                this.SelectCurrentRecord();
        }

        private void grdSearch_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            e.Layout.Override.CellClickAction = CellClickAction.RowSelect;
            e.Layout.Override.SelectTypeRow = SelectType.Extended;
            e.Layout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Override.AllowAddNew = AllowAddNew.No;
            e.Layout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
            e.Layout.Bands[0].Override.RowAlternateAppearance.BackColor = Color.LemonChiffon;

            m_Data.GridInitializeLayout(grdSearch);
        }

        private void grdSearch_ClickCellButton(object sender, CellEventArgs e)
        {
            m_Data.GridClickCellButton(sender, e);
            this.FormSearch();
        }

        private void grdSearch_InitializeRow(object sender, InitializeRowEventArgs e)
        {
            m_Data.GridInitializeRow(sender, e);
        }

      

     

   
    

    

   
    }
}