using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Text;

using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using iTextSharp.text;
using System.IO;
using PaymentXP.BusinessObjects.Notify;

public partial class wucConditions : System.Web.UI.UserControl
{
    #region Members
    public bool isACHOnly
    {
        get { return (bool)ViewState["isACHOnly"]; }
        set { ViewState["isACHOnly"] = value; }
    }

    /// <summary>
    /// Given the conditionid, returns the associated doctypeid. it's a 1 to 1 relationship.
    /// </summary>
    private Dictionary<int, int> diCondDocAssoc
    {
        set
        {
            ViewState["diCondDocAssoc"] = value;
        }
        get
        {
            if (ViewState["diCondDocAssoc"] != null)
                return (Dictionary<int, int>)ViewState["diCondDocAssoc"];
            else
                return new Dictionary<int, int>();
        }
    }

    /// <summary>
    /// primary key is the conditiondetailid, the list is all the uwconditiondocuments associated with that conditionid
    /// </summary>
    public Dictionary<int, List<UWConditionDocument>> diDocGrid
    {
        set
        {
            ViewState["diDocGrid"] = value;
        }
        get
        {
            if (ViewState["diDocGrid"] != null)
                return (Dictionary<int, List<UWConditionDocument>>)ViewState["diDocGrid"];
            else
                return null;
        }
    }

    public List<UWConditions> lstConditionsClone
    {
        set
        {
            ViewState["lstConditionsClone"] = value;
        }
        get
        {
            if (ViewState["lstConditionsClone"] != null)
                return (List<UWConditions>)ViewState["lstConditionsClone"];
            else
                return null;
        }
    }

    public string StatusName
    {
        set
        {
            ViewState["StatusName"] = value;
        }
        get
        {
            if (ViewState["StatusName"] != null)
                return ViewState["StatusName"].ToString();
            else
                return string.Empty;
        }
    }

    public string errorMessage
    {
        set { ViewState["errorMessage"] = value; }
        get { if (ViewState["errorMessage"] != null) return ViewState["errorMessage"].ToString(); else return string.Empty; }

    }

    public bool isEdit
    {
        set
        {
            ViewState["Editmode"] = value;
        }
        get
        {
            if (ViewState["Editmode"] != null)
                return DataLayer.Field2Bool(ViewState["Editmode"]);
            else
                return false;
        }
    }

    public int frmPage
    {
        set
        {
            ViewState["m_page"] = value;
        }
        get
        {
            if (ViewState["m_page"] != null)
                return Convert.ToInt32(ViewState["m_page"]);
            else
                return 0;
        }
    }

    public string MerchantAppUID
    {
        set
        {
            ViewState["MerchantAppUID"] = value;
        }
        get
        {
            if (ViewState["MerchantAppUID"] != null)
                return ViewState["MerchantAppUID"].ToString();
            else
                return string.Empty;
        }
    }

    public Dictionary<string, string> PendingNote
    {
        set
        {
            ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "Note"] = value;
        }
        get
        {
            if (ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "Note"] != null)
            {
                return (Dictionary<string, string>)ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "Note"];
            }
            else
            {
                ViewState[UserSessions.CurrentMerchantApp.MerchantAppUID + "Note"] = new Dictionary<string, string>();
                return new Dictionary<string, string>();
            }
        }
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Text = "";
        string status = string.Empty;

        if (!IsPostBack)
        {
            string m_StatusName = string.Empty;

            if (isACHOnly && UserSessions.ActiveAchMerchant != null)
            {
                m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2);
            }
            else if (UserSessions.CurrentMerchantApp != null)
            {
                m_StatusName = UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2);
            }

            if (UserSessions.CurrentMerchantApp != null)
            {
                if (m_StatusName.ToUpper() != "SS")// AP Queue is now changed to RM
                {
                    ListHandler.ListFindItem(cboType, "CU");
                }
                else
                {
                    ListHandler.ListFindItem(cboType, "SS");
                }

                this.diCondDocAssoc = this.FillCondDocAssoc();

                this.FillDiDocGrid();
            }

            LoadConditions();
        }

        if (grdConditions.Rows.Count == 1 && grdConditions.Rows[0].Cells[0].Text.Contains("No Conditions"))
        {
            LoadConditions();
        }

        errorMessage = "";
    }

    public void FillDiDocGrid()
    {
        List<UWConditionDocument> li = DataConditions.SelectUWConditionDocument(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));

        this.diDocGrid = new Dictionary<int, List<UWConditionDocument>>();

        if (li != null)
        {

            foreach (UWConditionDocument item in li)
            {
                if (!this.diDocGrid.ContainsKey(item.ConditionDetailID))
                {
                    this.diDocGrid.Add(item.ConditionDetailID, new List<UWConditionDocument>());
                }

                this.diDocGrid[item.ConditionDetailID].Add(item);

            }
        }
    }

    public List<UWConditionDocument> GetConditionDocObj(int ConditionDetailID)
    {
        List<UWConditionDocument> li = null;

        if (this.diDocGrid != null && this.diDocGrid.ContainsKey(ConditionDetailID))
        {
            li = this.diDocGrid[ConditionDetailID];
        }

        return li;
    }

    protected void grdConditions_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                break;

            case DataControlRowType.DataRow:

                int condition_detail_id = 0;

                if (e.Row.DataItem.GetType().FullName.EndsWith("DataRowView"))
                {
                    DataRowView drv = (DataRowView)e.Row.DataItem;

                    condition_detail_id = CommonUtility.Util.if_i(drv["ConditionDetailID"], 0);
                }
                else
                {
                    UWConditions objUWC = (UWConditions)e.Row.DataItem;

                    condition_detail_id = CommonUtility.Util.if_i(objUWC.ConditionDetailID, 0);
                }

                //if (condition_detail_id == 0)
                //{
                //    throw new Exception("condition_detail_id is 0 when it shouldnt be. could not match the datarowview type.");
                //}


                //if (grdConditions.DataSource.GetType().Name == "DataSet")
                //{
                ((Label)e.Row.Cells[0].FindControl("lblID")).Text = DataBinder.Eval(e.Row.DataItem, "ConditionDetailID").ToString();
                ((Label)e.Row.Cells[1].FindControl("lblName")).Text = "<div style='width: 190px; overflow: hidden; white-space: nowrap; text-overflow: ellipsis;'><nobr>" + DataBinder.Eval(e.Row.DataItem, "ConditionName").ToString() + "</nobr></div>";
                ((Label)e.Row.Cells[1].FindControl("lblName")).ToolTip = DataBinder.Eval(e.Row.DataItem, "ConditionName").ToString();
                ((CheckBox)e.Row.Cells[2].FindControl("chkNeedMore")).Checked = true;
                ((CheckBox)e.Row.Cells[3].FindControl("chkReceived")).Checked = DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "ReceivedInfo"));
                ((TextBox)e.Row.Cells[4].FindControl("txtEmail")).Text = HttpUtility.HtmlDecode(DataBinder.Eval(e.Row.DataItem, "EmailText").ToString());
                ((TextBox)e.Row.Cells[4].FindControl("txtEmail")).ToolTip = HttpUtility.HtmlDecode(DataBinder.Eval(e.Row.DataItem, "EmailText").ToString());
                ((TextBox)e.Row.Cells[5].FindControl("txtComments")).Text = HttpUtility.HtmlDecode(DataBinder.Eval(e.Row.DataItem, "Comments").ToString());
                ((TextBox)e.Row.Cells[5].FindControl("txtComments")).ToolTip = HttpUtility.HtmlDecode(DataBinder.Eval(e.Row.DataItem, "Comments").ToString());

                ((TextBox)e.Row.Cells[6].FindControl("txtAgentNote")).Text = HttpUtility.HtmlDecode(DataBinder.Eval(e.Row.DataItem, "AgentNote").ToString());


                ((CheckBox)e.Row.Cells[3].FindControl("chkReceived")).Enabled = this.isEdit;

                TextBox txt1 = ((TextBox)e.Row.Cells[4].FindControl("txtEmail"));
                //Niranjan:- PXP-4256 Condition Description should not be editable.
                txt1.ReadOnly = true;

                TextBox txt2 = ((TextBox)e.Row.Cells[5].FindControl("txtComments"));
                txt2.ReadOnly = !this.isEdit;

                //if ((CONDITIONS)frmPage == CONDITIONS.RELATIONSHIPMANAGEMENT || UserSessions.CurrentUser.UserRoles.ContainsKey("A094E9C6-E5BF-4536-B534-532DAE90F687"))
                //    txt1.ReadOnly = true;
                //else
                //    txt2.ReadOnly = true;

                // business rule: comment box can be modified by anybody. basically this page has restricted access already.
                //if (this.isEdit)
                //{
                //    txt2.ReadOnly = false;
                //}

                if (DataLayer.Field2Bool(DataBinder.Eval(e.Row.DataItem, "ReceivedInfo")))
                    ((CheckBox)e.Row.Cells[3].FindControl("chkReceived")).Enabled = !chkHistory.Checked;

                ((CheckBox)e.Row.Cells[3].FindControl("chkReceived")).Attributes.Add("onclick", "checkInfo('" + chkReceivedStatus.ClientID + "','" + ((CheckBox)e.Row.Cells[3].FindControl("chkReceived")).ClientID + "')");


                Literal lit = (Literal)e.Row.FindControl("litDocs");
                //GridView gvDoc = (GridView)e.Row.FindControl("gvDocuments");

                UserControls_wucDocumentGrid gvDoc = (UserControls_wucDocumentGrid)e.Row.FindControl("WucDocumentGrid1");


                gvDoc.EditMode = this.isEdit;

                //Check if current logged-in internal user's office is LosAngeles then increase colspan attribute value of HTML table column to 8 to allow that user to view Agent Note in Pending Conditions grid with proper grid design.
                if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
                {
                    // this is the trick that creates a new row.
                    lit.Text = @"</td><tr><td colspan='1'></td><td colspan='8'>";
                }
                else
                {
                    // this is the trick that creates a new row.
                    lit.Text = @"</td><tr><td colspan='1'></td><td colspan='7'>";
                }



                lit.Visible = true;
                gvDoc.Visible = true;

                List<UWConditionDocument> li = this.GetConditionDocObj(condition_detail_id);
                if (li != null && li.Count > 0)
                {

                    if (li.Count == 1 && CommonUtility.Util.if_s(li[0].OrigName) == "")
                    {
                        // this documents are required, but no files have been uploaded yet.
                        gvDoc.MyDataSource = null;
                    }
                    else
                    {
                        gvDoc.MyDataSource = li;
                        gvDoc.MyDataBind();
                        gvDoc.EditMode = this.isEdit;

                    }
                }
                else
                {
                    gvDoc.MyDataSource = null;
                }
                e.Row.Cells[7].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[7].Text);

                DateTime dt = DataLayer.Field2Date(DataBinder.Eval(e.Row.DataItem, "ARDate"));
                e.Row.Cells[8].Text = dt == DateTime.MinValue ? string.Empty : WebUtil.ConvertToUserDateTimeSettings(dt.ToString());

                // }
                break;

            case DataControlRowType.Footer:

                DropDownList ddConditions = ((DropDownList)e.Row.FindControl("ConditionName"));
                if (ddConditions != null)
                {
                    if (UserSessions.CurrentMerchantApp != null)
                    {
                        string m_StatusName = UserSessions.CurrentMerchantApp.StatusName;

                        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
                        {
                            m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
                        }

                        string statname = "CU";

                        if (m_StatusName.Substring(0, 2).ToUpper() == "SS")
                        {
                            statname = "SS";// AP Queue is now changed to RM
                        }

                        LookupTableHandler.LoadUWConditions(ddConditions, false, statname);
                    }
                }

                LinkButton btn = (LinkButton)e.Row.FindControl("lnkID");

                if (btn != null)
                {
                    btn.Attributes.Add("onclick", "callfunc();return false;");
                }


                break;
        }
    }

    protected void grdConditions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        UWConditions condition = new UWConditions();
        DataMerchantApp data = DataAccess.DataMerchantAppDao;

        switch (e.CommandName)
        {
            case "Insert":

                GridViewRow grdRow = grdConditions.FooterRow;

                DropDownList ddlCond = (DropDownList)grdRow.Cells[1].FindControl("ConditionName");

                if (ddlCond != null && ddlCond.SelectedIndex > 0)
                {

                    // INSERT into UWConditionDetail
                    LinkButton lnk = ((LinkButton)grdRow.Cells[0].FindControl("lnkID"));
                    if (lnk != null)// && hdn.Value == "0")
                    {
                        condition.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                        condition.NeedInfo = true;
                        condition.EmailText = ((TextBox)grdRow.Cells[4].FindControl("Email")).Text.Replace("\r\n", "<br />");
                        //Niranjan:- PXP-4256 Condition Description should not be editable.
                        condition.Comments = condition.EmailText + Environment.NewLine + ((TextBox)grdRow.Cells[5].FindControl("Comments")).Text.Replace("\r\n", "<br />"); 
                        condition.ReceivedInfo = ((CheckBox)grdRow.Cells[3].FindControl("Received")).Checked;
                        condition.ConditionName = ((DropDownList)grdRow.Cells[1].FindControl("ConditionName")).SelectedItem.Text;
                        condition.ConditionID = ((DropDownList)grdRow.Cells[1].FindControl("ConditionName")).SelectedValue;

                        int condition_detail_id = DataConditions.InsertConditions(condition, UserSessions.CurrentUser.UserName);

                        UserSessions.CurrentMerchantApp.HasConditions = true;
                        MerchantFacade facade = new MerchantFacade();
                        facade.UpdateMerchantApp(UserSessions.CurrentMerchantApp);

                        string statname = "CU";

                        /// when merchant bank is achonly and has a ach account associated with it then check the ach status
                        /// if ach status queue is "SS" then we update the conditions associated with SS.
                        /// for all other ach status we default the status queue as CU for conditions
                        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
                        {
                            if (UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper() == "SS")
                            {
                                statname = "SS";
                            }
                        }
                        else if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper() == "SS")// AP Queue is now changed to RM
                        {
                            statname = "SS";// AP Queue is now changed to RM
                        }

                        chkReceivedStatus.Checked = false;
                        Hashtable prms = new Hashtable();
                        prms.Add("@MerchantAppUID", DataLayer.UID2Field(UserSessions.CurrentMerchantApp.MerchantAppUID));
                        prms.Add("@AllInfoReceived", false);
                        prms.Add("@Type", statname);
                        data.UpdateMerchantUWNotes(prms);


                        //Do not insert Ticket when adding a condition, US 10173
                        //Rollback US 10173

                        if (!PendingNote.ContainsKey(condition_detail_id.ToString()))
                        {
                            string str = "New Pending condition added: \n<ol><li>" + condition.ConditionName + " | " + condition.EmailText + (string.IsNullOrWhiteSpace(condition.Comments) ? string.Empty : " : " + condition.Comments) + "</li></ol>";
                            FormHandler.AddTicket(UserSessions.CurrentMerchantApp, str);
                        }

                        LoadConditions();
                    }

                }
                else
                {
                    errorMessage = "Please select a condition.";
                }
                break;

            case "CancelCom":

                LoadConditions();
                break;
        }
    }

    protected void btnResend_Click(object sender, EventArgs e)
    {
        if (Pendingconditions(UserSessions.CurrentMerchantApp, true))
        {
            txtEmailAdd.Text = "";
            errorMessage = "Email sent successfully";
            FormHandler.DisplayMessage(Page.ClientScript, errorMessage);
        }
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        if (grdConditions.Rows[0].Cells[0].Text.Contains("No Conditions"))
        {
            grdConditions.Rows[0].Visible = false;
            grdConditions.Rows[0].Cells.Clear();
        }

        grdConditions.FooterRow.Visible = true;

    }

    protected void chkHistory_CheckedChanged(object sender, EventArgs e)
    {
        if (!chkHistory.Checked)
        {
            string statname = "CU";


            /// when merchant bank is achonly and has a ach account associated with it then check the ach status
            /// if ach status queue is "SS" then we update the conditions associated with SS.
            /// for all other ach status we default the status queue as CU for conditions
            if (isACHOnly && UserSessions.ActiveAchMerchant != null)
            {
                if (UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper() == "SS")
                {
                    statname = "SS";
                }
            }
            else if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper() == "SS")
            {
                statname = "SS";// AP Queue is now changed to RM
            }

            ListHandler.ListFindItem(cboType, statname);
        }

        LoadConditions();
    }

    protected void chkReceived_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow grdRow = (GridViewRow)chk.NamingContainer;
        UWConditions condition = new UWConditions();

        condition.ConditionDetailID = ((Label)grdRow.Cells[0].FindControl("lblID")).Text;
        condition.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
        condition.NeedInfo = ((CheckBox)grdRow.Cells[2].FindControl("chkNeedMore")).Checked;
        condition.EmailText = ((TextBox)grdRow.Cells[4].FindControl("txtEmail")).Text;
        condition.Comments = ((TextBox)grdRow.Cells[5].FindControl("txtComments")).Text;
        condition.ReceivedInfo = ((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Checked;
        condition.ConditionName = ((Label)grdRow.Cells[1].FindControl("lblName")).ToolTip;

        if (chk.Checked)
        {
            foreach (UWConditions con in lstConditionsClone)
            {
                if (con.ConditionDetailID == condition.ConditionDetailID)
                {
                    if (PendingNote.ContainsKey(condition.ConditionDetailID))
                        PendingNote.Remove(condition.ConditionDetailID);

                    PendingNote.Add(condition.ConditionDetailID, condition.ConditionName + " | " + condition.EmailText + " : " + condition.Comments);
                }
            }
        }
        else
        {
            if (PendingNote.ContainsKey(condition.ConditionDetailID))
                PendingNote.Remove(condition.ConditionDetailID);
        }
    }

    protected void chkReceivedStatus_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        UWConditions condition = new UWConditions();
        GridViewRow grdRow;

        if (grdConditions.Rows.Count > 0 && grdConditions.Rows[0].Cells.Count > 1)
        {
            if (chk.Checked)
            {
                for (int i = 0; i < grdConditions.Rows.Count; i++)
                {
                    grdRow = grdConditions.Rows[i];
                    condition.ConditionDetailID = ((Label)grdRow.Cells[0].FindControl("lblID")).Text;
                    condition.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                    condition.NeedInfo = ((CheckBox)grdRow.Cells[2].FindControl("chkNeedMore")).Checked;
                    condition.EmailText = ((TextBox)grdRow.Cells[4].FindControl("txtEmail")).Text;
                    condition.Comments = ((TextBox)grdRow.Cells[5].FindControl("txtComments")).Text;
                    condition.ReceivedInfo = ((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Checked;
                    condition.ConditionName = ((Label)grdRow.Cells[1].FindControl("lblName")).ToolTip;

                    if (!PendingNote.ContainsKey(condition.ConditionDetailID))
                        PendingNote.Add(condition.ConditionDetailID, condition.ConditionName + " | " + condition.EmailText + " : " + condition.Comments);
                }
            }
            else
            {
                for (int i = 0; i < grdConditions.Rows.Count; i++)
                {
                    grdRow = grdConditions.Rows[i];
                    condition.ReceivedInfo = ((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Checked;
                    condition.ConditionName = ((Label)grdRow.Cells[1].FindControl("lblName")).ToolTip;

                    if (PendingNote.ContainsKey(condition.ConditionDetailID) && condition.ReceivedInfo == false)
                        PendingNote.Remove(condition.ConditionDetailID);
                }
            }
        }
    }

    protected void cboType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadConditions();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        string fileName = "PendingConditions_" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".xls";
        FormHandler.Export2Excel(fileName, grd);
    }

    public void FormSave()
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        AchMerchant achMerchant = null;

        if (isACHOnly)
        {
            achMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(app.ID));
        }

        UserSessions.CurrentMerchantApp = app;
        //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
        UserSessions.ActiveAchMerchant = achMerchant;

        bool result = false;
        StringBuilder sb = new StringBuilder();


        //eluxa 2013-01-18: this is a temp fix to allow all users that have access to frmUnderwritingPending.aspx to be able
        //to update conditions just like an RM would be able. the bug was that creditunderwriting couldn't
        //update the the received status for the condition when checkboxing received. we need to explore the 
        //cause of this bug later
        //if ((CONDITIONS)frmPage == CONDITIONS.RELATIONSHIPMANAGEMENT)
        //{
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        Hashtable prms = new Hashtable();
        string statName = "CU";

        /// when merchant bank is achonly and has a ach account associated with it then check the ach status
        /// if ach status queue is "SS" then we update the conditions associated with SS.
        /// for all other ach status we default the status queue as CU for conditions
        if (isACHOnly && achMerchant != null)
        {
            if (achMerchant.MerchantStatusName.Substring(0, 2).ToUpper() == "SS")
                statName = "SS";
        }
        else if (app.StatusName.Substring(0, 2).ToUpper() == "SS")
        {
            statName = "SS";
        }

        if (UserSessions.CurrentMerchantApp != null)
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);

        prms.Add("@Needmore", true);
        prms.Add("@type", statName);

        DataSet ds = data.GetUWConditions(prms);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (UpdtateConditions())
            {
                if (chkReceivedStatus.Checked)
                {
                    string to = Constants.CREDIT_UNDERWRITING_EMAIL;
                    sb.Append("<br><table border=1 cellspacing=5 cellpadding=5 style='border-collapse:collapse;border:none'><tr style='border:solid 1px;'><td><b>Condition(s)</b></td><td><b>Email Text</b></td><td><b>Comments</b></td><td><b>Request on</b></td><td><b>Received on</b></td><td><b>Type</b></td></tr>");

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        sb.Append("<tr><td>" + dr["ConditionName"].ToString() + "</td>");
                        sb.Append("<td>" + dr["EmailText"].ToString() + "</td>");
                        sb.Append("<td>" + dr["Comments"].ToString() + "</td>");
                        sb.Append("<td>" + dr["CUDate"].ToString() + "</td>");
                        sb.Append("<td>" + dr["ARDate"].ToString() + "</td>");
                        sb.Append("<td>" + dr["Type"].ToString() + "</td></tr>");
                    }

                    sb.Append("</table><br>");

                    if (!isACHOnly)
                    {
                        if (app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_PENDING || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED)
                            app.StatusUID = Constants.QUEUESTATUS_CU_RECEIVED_PD;

                        if (app.StatusUID.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN)
                        {
                            app.StatusUID = Constants.QUEUESTATUS_SS_RECEIVED;
                            to = Constants.APPLICATION_EMAIL;
                        }
                    }
                    else
                    {
                        bool istrue = false;

                        if (achMerchant != null)
                        {
                            if (achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_PENDING || achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED)
                            {
                                achMerchant.MerchantStatusUID = Constants.QUEUESTATUS_CU_RECEIVED_PD;
                                istrue = true;
                            }

                            if (achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE || achMerchant.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN)
                            {
                                achMerchant.MerchantStatusUID = Constants.QUEUESTATUS_SS_RECEIVED;
                                istrue = true;
                                to = Constants.APPLICATION_EMAIL;
                            }

                            if (istrue)
                                DataAccess.DataAchMerchantDao.UpdateAchMerchant(achMerchant);
                        }
                    }

                    AlertNotification.SendAlertNotification(true, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, app.MerchantAppUID, app.BusinessDBAName, app.BusinessLegalName, "Conditions", to, Constants.CREDIT_UNDERWRITING_EMAIL + "," + Constants.RELATIONSHIP_MANAGEMENT_EMAIL, "All conditions are received for this account." + sb.ToString(), " - Application Complete", null, UserSessions.CurrentUser.UserName, app.PrivateLabelUID, UserSessions.CurrentMerchantApp.AgentDBA, app.Bank);
                    ZeusWeb.Logging.EmailLog.InfoFormat("For DBAName : {0} All conditions are received for this account. Email sent to: {1}", app.BusinessDBAName, to);

                }

                result = Pendingconditions(app, false);
            }
        }

        app = UserSessions.CurrentMerchantApp;

        if (result)
        {

            IList<GenericListItem> list = DataAccess.DataMerchantAppDao.GetMerchantUWEmailBody(app.MerchantAppUID, statName);

            if (list.Count > 0)
            {
                app.HasConditions = !chkReceivedStatus.Checked;
            }
            else
            {
                app.HasConditions = false;
            }

            facade.UpdateMerchantApp(app);
        }
        else if (app.HasConditions)
        {
            app.HasConditions = false;
            facade.UpdateMerchantApp(app);
        }
        //PXP-7620 RThakur
        else
        {
            string statusUIDToCheck = string.Empty;
            string statusNameToCheck = string.Empty;
            if (achMerchant != null)
            {
                statusUIDToCheck = achMerchant.MerchantStatusUID.ToUpper();
                statusNameToCheck = achMerchant.MerchantStatusName;
            }
            else
            {
                statusUIDToCheck = app.StatusUID.ToUpper();
                statusNameToCheck = app.StatusName;
            }
            if (PendingNote.Count > 0)
            {
                if ((statusNameToCheck.StartsWith("CU") && !(statusUIDToCheck.ToUpper().Equals(Constants.QUEUESTATUS_CU_DECLINED) || statusUIDToCheck.ToUpper().Equals(Constants.QUEUESTATUS_CU_WITHDRAWN)))
                    || !(statusUIDToCheck.ToUpper().Equals(Constants.QUEUESTATUS_SS_WITHDRAWN) || statusNameToCheck.Equals("SS - Withdrawn"))
                   )
                {
                    FormHandler.CloseApplicationTicket(app, UserSessions.CurrentUser);
                }
            }
        }

        LoadConditions();

    }

    public void FormCancel()
    {
        LoadConditions();
    }

    public void Enable(bool istrue)
    {
        chkHistory.Enabled = true;
        cboType.Enabled = true;
    }

    public void setValues(int val)
    {
        frmPage = val;

        string stat = string.Empty;


        /// when merchant bank is achonly and has a ach account associated with it then check the ach status
        /// if ach status queue is "SS" then we update the conditions associated with SS.
        /// for all other ach status we default the status queue as CU for conditions
        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
        {
            stat = UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper();
        }
        else
        {
            stat = UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper();
        }

        if ((CONDITIONS)frmPage == CONDITIONS.RELATIONSHIPMANAGEMENT && stat != "SS")
        {
            lnkAdd.Visible = false;
        }
        else
        {
            lnkAdd.Visible = true;
        }
    }

    public void LoadConditions()
    {
        grdConditions.Enabled = true;

        Hashtable prms = new Hashtable();

        if (UserSessions.CurrentMerchantApp != null)
            prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);

        if ((CONDITIONS)frmPage == CONDITIONS.RELATIONSHIPMANAGEMENT)
        {
            prms.Add("@Needmore", true);
            prms.Add("@frmPage", 2);
        }

        if (!chkHistory.Checked)
            prms.Add("@history", false);

        if (cboType.SelectedIndex > 0)
            prms.Add("@type", cboType.SelectedValue);

        IList<UWConditions> condition = new List<UWConditions>();
        condition = DataConditions.GetUWConditionsList(prms);

        if (grdConditions.FooterRow != null)
            grdConditions.FooterRow.Cells.Clear();

        if (condition.Count == 0)
        {
            condition.Add(new UWConditions());
            grdConditions.DataSource = condition;
            grdConditions.DataBind();

            // Get the total number of columns in the GridView to know what the Column Span should be       
            int columnsCount = grdConditions.Columns.Count;
            grdConditions.Rows[0].Cells.Clear();

            // clear all the cells in the row      
            grdConditions.Rows[0].Cells.Add(new TableCell());

            //add a new blank cell      
            grdConditions.Rows[0].Cells[0].ColumnSpan = columnsCount;
            grdConditions.Rows[0].Cells[0].CssClass = "EmptyDataRowStyle";

            //set No Results found to the new added cell       
            grdConditions.Rows[0].Cells[0].Text = ".....No Conditions....";
            grdConditions.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;

            btnExpExcel.Enabled = false;
        }
        else
        {
            grdConditions.DataSource = condition;
            grdConditions.DataBind();

            grd.DataSource = condition;
            grd.DataBind();

            lstConditionsClone = (List<UWConditions>)condition;
            btnExpExcel.Enabled = true;
        }

        //Check if current logged-in internal user's office is LosAngeles then allow that user to view Agent Note in Pending Conditions grid.
        if (UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
        {
            grdConditions.Columns[6].Visible = true;
        }
        else
        {
            grdConditions.Columns[6].Visible = false;
        }

        if (grdConditions.FooterRow != null)
            grdConditions.FooterRow.Visible = false;
        //grdConditions.FooterRow.Attributes.Add("style", "display:none;");

        if (UserSessions.CurrentMerchantApp != null)
        {
            string statname = "CU";

            /// when merchant bank is achonly and has a ach account associated with it then check the ach status
            /// if ach status queue is "SS" then we update the conditions associated with SS.
            /// for all other ach status we default the status queue as CU for conditions
            if (isACHOnly && UserSessions.ActiveAchMerchant != null)
            {
                if (UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper() == "SS")
                {
                    statname = "SS";
                }
            }
            else
            {
                if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper() == "SS")
                {
                    statname = "SS";// RM Queue is now changed to SS
                }
            }

            IList<GenericListItem> list = DataAccess.DataMerchantAppDao.GetMerchantUWEmailBody(UserSessions.CurrentMerchantApp.MerchantAppUID, statname);
            if (list.Count > 0)
            {
                txtEmail.Text = list[0].ItemText;
                chkReceivedStatus.Checked = Convert.ToBoolean(list[0].ItemValue);
            }
            else
                chkReceivedStatus.Checked = false;
        }

        if (UserSessions.CurrentMerchantApp != null)
            WucNotes1.LoadNotes(UserSessions.CurrentMerchantApp.MerchantAppUID, "");
    }

    private bool UpdtateConditions()
    {
        UWConditions condition = new UWConditions();
        DataMerchantApp data = DataAccess.DataMerchantAppDao;
        bool istrue = true;

        string statusName = "CU";


        /// when merchant bank is achonly and has a ach account associated with it then check the ach status
        /// if ach status queue is "SS" then we update the conditions associated with SS.
        /// for all other ach status we default the status queue as CU for conditions
        if (isACHOnly && UserSessions.ActiveAchMerchant != null)
        {
            if (UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2).ToUpper() == "SS")
            {
                statusName = "SS";
            }
        }
        else if (UserSessions.CurrentMerchantApp.StatusName.Substring(0, 2).ToUpper() == "SS")// AP Queue is now changed to RM
        {
            statusName = "SS";// AP Queue is now changed to RM
        }

        if (grdConditions.Rows.Count > 0 && !(grdConditions.Rows[0].Cells[0].Text.Contains("No Conditions")))
        {
            foreach (GridViewRow grdRow in grdConditions.Rows)
            {
                condition.ConditionDetailID = ((Label)grdRow.Cells[0].FindControl("lblID")).Text;
                condition.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                condition.NeedInfo = true;
                condition.EmailText = ((TextBox)grdRow.Cells[4].FindControl("txtEmail")).Text;
                condition.Comments = ((TextBox)grdRow.Cells[5].FindControl("txtComments")).Text;
                condition.ReceivedInfo = ((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Checked || chkReceivedStatus.Checked;
                condition.ConditionName = ((Label)grdRow.Cells[1].FindControl("lblName")).ToolTip;

                if (!condition.ReceivedInfo)
                {
                    istrue = false;
                }
                else
                {
                    if (PendingNote.ContainsKey(condition.ConditionDetailID))
                    {
                        PendingNote[condition.ConditionDetailID] = condition.ConditionName + " | " + condition.EmailText + " : " + condition.Comments;
                    }
                }

                DataConditions.UpdtateConditions(condition, UserSessions.CurrentUser.UserName);

                UserControls_wucDocumentGrid objG = (UserControls_wucDocumentGrid)grdRow.FindControl("WucDocumentGrid1");
                objG.EditMode = this.isEdit;
                objG.UpdateControl();

                if (chkReceivedStatus.Checked == true)
                {
                    istrue = true;

                    if (PendingNote.ContainsKey(condition.ConditionDetailID))
                        PendingNote[condition.ConditionDetailID] = condition.ConditionName + " | " + condition.EmailText + " : " + condition.Comments;
                    else if (((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Enabled)
                        PendingNote.Add(condition.ConditionDetailID, condition.ConditionName + " | " + condition.EmailText + " : " + condition.Comments);
                }
            }

            if (istrue)
            {
                chkReceivedStatus.Checked = true;
            }

            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", DataLayer.UID2Field(UserSessions.CurrentMerchantApp.MerchantAppUID));
            prms.Add("@ConditionsEmailBody", txtEmail.Text);
            prms.Add("@AllInfoReceived", chkReceivedStatus.Checked);
            prms.Add("@Type", statusName);
            data.UpdateMerchantUWNotes(prms);

            MerchantFacade facade = new MerchantFacade();
            MerchantApp app = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
            AchMerchant ach = null;

            if (PendingNote.Count > 0)
            {

                StringBuilder str = new StringBuilder();
                StringBuilder str1 = new StringBuilder();

                str.Append("Pending condition(s) received: \n<ol>");

                foreach (string note in PendingNote.Keys)
                {
                    if (!string.IsNullOrEmpty(PendingNote[note]))
                        str1.Append("<li>" + PendingNote[note] + ".</li>");
                }

                if (str1.Capacity > 0)
                {
                    str.Append(str1.ToString());
                    str.Append("</ol>");

                    //Rollback US 10173
                    FormHandler.AddTicket(app, str.ToString());
                }

                PendingNote.Clear();
            }

            if (istrue)
            {
                app.HasConditions = !chkReceivedStatus.Checked;

                if (!isACHOnly)
                {
                    if (app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_PENDING || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED)
                        app.StatusUID = Constants.QUEUESTATUS_CU_RECEIVED_PD;

                    if (app.StatusUID.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN)
                        app.StatusUID = Constants.QUEUESTATUS_SS_RECEIVED;
                }
                else
                {
                    ach = DataAccess.DataAchMerchantDao.GetAchMerchant(Convert.ToInt32(app.ID));

                    if (ach != null)
                    {
                        if (ach.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_PENDING || ach.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_CU_WITHDRAWN || app.StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_BANK_REQUESTED)
                        {
                            ach.MerchantStatusUID = Constants.QUEUESTATUS_CU_RECEIVED_PD;
                            DataAccess.DataAchMerchantDao.UpdateAchMerchant(ach);
                        }

                        if (ach.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_SS_APP_INCOMPLETE || ach.MerchantStatusUID.ToUpper() == Constants.QUEUESTATUS_SS_WITHDRAWN)
                        {
                            ach.MerchantStatusUID = Constants.QUEUESTATUS_SS_RECEIVED;
                            DataAccess.DataAchMerchantDao.UpdateAchMerchant(ach);
                        }
                    }
                }

                facade.UpdateMerchantApp(app);

                //Rollback US 10173
                FormHandler.AddTicket(app, string.Empty);

            }
            UserSessions.CurrentMerchantApp = app;
            //FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
            UserSessions.ActiveAchMerchant = ach;

        }

        return !istrue;
    }

    private bool Pendingconditions(MerchantApp app, bool isResend)
    {
        if (grdConditions.Rows.Count > 0 && !(grdConditions.Rows[0].Cells[0].Text.Contains("No Conditions")))
        {
            StringBuilder sb = new StringBuilder();
            bool sendEmail = false;
            string AgentEmail = string.Empty;

            sb.Append("<br><ol>");

            foreach (GridViewRow grdRow in grdConditions.Rows)
            {
                if (((CheckBox)grdRow.Cells[3].FindControl("chkReceived")).Checked == false && chkReceivedStatus.Checked == false)
                {
                    sb.Append("<li><span style='font-size:10.0pt;font-family:Verdana;'><b>" + ((Label)grdRow.Cells[1].FindControl("lblName")).Text + "</b></span>");
                    //Niranjan:- PXP-4256 Condition Description should not be editable.
                    TextBox txt = (TextBox)grdRow.Cells[5].FindControl("txtComments");

                    if (!string.IsNullOrWhiteSpace(txt.Text))
                        sb.Append("<ul><li><span style='font-size:10.0pt;font-family:Verdana;'>" + txt.Text.Replace("\r\n", "<br />") + "</span></li></ul>");

                    sb.Append("</li>");
                    sendEmail = true;
                }
            }

            sb.Append("</ol><br><p><span style='font-size:10.0pt;font-family:Verdana;'>" + txtEmail.Text.Replace("\r\n", "<br />") + "</span></p>");

            if (sendEmail)
            {
                AgentNotification notification = null;
                string cc = "", m_StatusName = string.Empty;

                /// when merchant bank is achonly and has a ach account associated with it then check the ach status
                /// if ach status queue is "SS" then we update the conditions associated with SS.
                /// for all other ach status we default the status queue as CU for conditions
                if (isACHOnly && UserSessions.ActiveAchMerchant != null)
                {
                    m_StatusName = UserSessions.ActiveAchMerchant.MerchantStatusName;
                }
                else
                {
                    m_StatusName = app.StatusName;
                }

                if (m_StatusName.Substring(0, 2).ToUpper() == "SS")
                {
                    notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.ApplicationIncomplete, app.PrivateLabelUID);
                }
                else
                {
                    notification = NotificationService.GetAgentNotification(app.AgentID, NotificationType.ApplicationPending, app.PrivateLabelUID);
                   
                    //Chandra: for PXP-3293, add credit team email as BCC
                    notification.AddBccRecipient(Constants.CREDIT_UNDERWRITING_EMAIL);
                }

                if (isResend)
                {
                    AgentEmail = txtEmailAdd.Text.Replace("\r\n", "<br />");

                    if (!string.IsNullOrEmpty(AgentEmail))
                    {
                        notification.RemoveAllRecipients();
                        notification.AddRecipient(AgentEmail);
                    }
                }

                notification.UserName = UserSessions.CurrentUser.UserName;
                AlertNotification.SendAgentAlertNotification(notification, app, Constants.RELATIONSHIP_MANAGEMENT_EMAIL, cc, notification.Name, sb.ToString(), " - " + notification.Name, Portal: ePortals.ZEUS);
            }

            return true;
        }

        return false;
    }

    public Dictionary<int, int> FillCondDocAssoc()
    {
        Dictionary<int, int> di = null;

        DataTable dt = DataConditions.GetUWConditionsOnly();

        if (dt != null && dt.Rows.Count > 0)
        {
            di = new Dictionary<int, int>();

            foreach (DataRow dr in dt.Rows)
            {
                int ConditionID = Convert.ToInt32(dr["ConditionID"]);
                int DocTypeID = CommonUtility.Util.if_i(dr["DocTypeID"], 0);

                if (DocTypeID > 0)
                {

                    if (!di.ContainsKey(ConditionID))
                    {
                        di.Add(ConditionID, DocTypeID);
                    }
                    else
                    {
                        di[ConditionID] = (DocTypeID);
                    }
                }
            }
        }

        return di;
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text);
        e.Row.Cells[5].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[5].Text);
    }

}
