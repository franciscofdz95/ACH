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

using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using PaymentXP.DataObjects;
using Infragistics.WebUI.WebSchedule;
using System.Text.RegularExpressions;
using Infragistics.Web.UI.EditorControls;
using System.Collections.Generic;
using static PaymentXP.Facade.ConstantFacade;

public partial class wucApplicationQueue : wucBaseSearch
{
    public delegate void ValueChangedHandler(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e);
    public event ValueChangedHandler ValueChanged;

    public delegate void CheckChangedHandler(object sender, EventArgs e);
    public event CheckChangedHandler CheckChanged;

    private string _PostBackURL;

    public string PostBackURL
    {
        get { return _PostBackURL; }
        set { _PostBackURL = value; }
    }
    
    public string VolumeLevel
    {
        get
        {
            if (ViewState["VolumeLevel"] == null)
                return string.Empty;
            else
                return ViewState["VolumeLevel"].ToString();
        }
        set { ViewState["VolumeLevel"] = value; }

    }

    public string StatusUID
    {
        get
        {
            if (ViewState["StatusUID"] == null)
                return string.Empty;
            else
                return ViewState["StatusUID"].ToString();
        }
        set { ViewState["StatusUID"] = value; }

    }
    //PXP-9308 by Sanidhya
    public bool IsPendingRegistration
    {
        get
        {
            if (ViewState["IsPendingRegistration"] == null)
                return false;
            else
                return (bool)ViewState["IsPendingRegistration"];
        }
        set { ViewState["IsPendingRegistration"] = value; }

    }

    public bool Conditions
    {
        get
        {
            if (ViewState["Conditions"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["Conditions"]);
        }
        set { ViewState["Conditions"] = value; }

    }

    public string Type
    {
        get
        {
            if (ViewState["Type"] == null)
                return string.Empty;
            else
                return ViewState["Type"].ToString();
        }
        set { ViewState["Type"] = value; }
    }

    public bool ConditionDate
    {
        get
        {
            if (ViewState["ConditionDate"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["ConditionDate"]);
        }
        set { ViewState["ConditionDate"] = value; }
    }

    public bool Export
    {
        get
        {
            if (ViewState["Export"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["Export"]);
        }
        set { ViewState["Export"] = value; }
    }

    public bool ApprovalRequested
    {
        get
        {
            if (ViewState["ApprovalRequested"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["ApprovalRequested"]);
        }
        set { ViewState["ApprovalRequested"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Date.Visible = ConditionDate;
    }

    public void SetDataSource(string statusuid, string Title, List<string> officeIds)
    {
        SetDataSource(statusuid, Title, officeIds, DateTime.MinValue);
    }

    public void SetDataSource(string statusuid, string Title, List<string> officeIds, DateTime BeginQueueDate)
    {
        StatusUID = statusuid.ToUpper();
        lblTitle.Text = Title;
        Hashtable prms = new Hashtable();

        prms.Add("@UserID", UserSessions.CurrentUser.UserID);

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        
        if (officeIds.Count == 0)
        {
            officeIds.Add("0");
        }
        prms.Add("@OfficeIDList", string.Join(",", officeIds));

        if (BeginQueueDate != DateTime.MinValue)
        {
            prms.Add("@BeginQueueDate", BeginQueueDate);
        }

        if (statusuid == string.Empty && ConditionDate == true)
            prms.Add("@Conditions", true);
         
        else if (statusuid != string.Empty)
            prms.Add("@StatusUID", statusuid);

        if (!string.IsNullOrEmpty(Type))
            prms.Add("@type", Type);

        if (ConditionDate && !AllDates.Checked)
        {
            if (!string.IsNullOrEmpty(ConditionalDueDate.Text))
            {
                ConditionalDueDate.Value = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                prms.Add("@ConditionDate", ConditionalDueDate.Value);
            }
        }

        prms.Add("@Requested", ApprovalRequested);

        if (!string.IsNullOrWhiteSpace(VolumeLevel) && Type == Constants.QueueCU_Status && statusuid.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED)
            prms.Add("@VolumeLevel", VolumeLevel);

        if (ApprovalRequested)
        {
            prms.Add("@SortOrder", "StatusChangedDate");
            this.SortOrder = "StatusChangedDate";
            this.SortDirectionSearch = SortDirection.Descending;
            prms.Add("@SortDirection", 1);
        }

        this.m_Prms = prms;
        BindGrid();
    }

    //PXP-9308 by Sanidhya
    public void SetDataSource(string statusuid, string Title, List<string> officeIds, DateTime BeginQueueDate,bool _isPendingRegistration)
    {
        StatusUID = statusuid.ToUpper();
        lblTitle.Text = Title;
        Hashtable prms = new Hashtable();

        prms.Add("@UserID", UserSessions.CurrentUser.UserID);

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        //PXP-9308 By Sanidhya
        IsPendingRegistration = _isPendingRegistration;
        prms.Add("@IsPendingRegistration", _isPendingRegistration);
        if (officeIds.Count == 0)
        {
            officeIds.Add("0");
        }
        prms.Add("@OfficeIDList", string.Join(",", officeIds));

        if (BeginQueueDate != DateTime.MinValue)
        {
            prms.Add("@BeginQueueDate", BeginQueueDate);
        }

        if (statusuid == string.Empty && ConditionDate == true)
            prms.Add("@Conditions", true);

        else if (statusuid != string.Empty)
            prms.Add("@StatusUID", statusuid);

        if (!string.IsNullOrEmpty(Type))
            prms.Add("@type", Type);

        if (ConditionDate && !AllDates.Checked)
        {
            if (!string.IsNullOrEmpty(ConditionalDueDate.Text))
            {
                ConditionalDueDate.Value = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");
                prms.Add("@ConditionDate", ConditionalDueDate.Value);
            }
        }

        prms.Add("@Requested", ApprovalRequested);

        if (!string.IsNullOrWhiteSpace(VolumeLevel) && Type == Constants.QueueCU_Status && statusuid.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED)
            prms.Add("@VolumeLevel", VolumeLevel);

        if (ApprovalRequested)
        {
            prms.Add("@SortOrder", "StatusChangedDate");
            this.SortOrder = "StatusChangedDate";
            this.SortDirectionSearch = SortDirection.Descending;
            prms.Add("@SortDirection", 1);
        }

        this.m_Prms = prms;
        BindGrid();
    }

    private void BindGrid()
    {
        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "StatusChangedDate");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

        lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetQueuesPagingRowCount(m_Prms, 0, 0, this.ID).ToString();

        grd.DataBind();
        pnlRecords.Visible = grd.Rows.Count != 0;
        pnlNoRecords.Visible = grd.Rows.Count == 0;
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        bool isTrue = false;

        //DM-507 -Chandra
        if (StatusUID == Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_SOFTWARE || StatusUID == Constants.QUEUESTATUS_DP_REVIEW) //Constants.QUEUESTATUS_DP_SCHEDULE_DOWNLOAD_TRAINING_HARDWARE)
        {
            if (Export == false)
            {
                isTrue = true;
            }
        }
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:

                e.Row.Cells[0].Visible = isTrue;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = isTrue;
                //PXP-9308 By Sanidhya
                if (StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && IsPendingRegistration)
                {
                    e.Row.Cells[17].Visible = true;
                }
                else
                {
                    e.Row.Cells[17].Visible = ConditionDate;
                } 
                e.Row.Cells[18].Visible = (Type.ToUpper() == Constants.QueueCU_Status || Type.ToUpper() == Constants.QueueSS_Status);// RM Queue is now changed to SS
                e.Row.Cells[19].Visible = !isTrue;                
                e.Row.Cells[14].Visible = (Type.ToUpper() == Constants.QueueCU_Status && StatusUID.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED);
                break;

            case DataControlRowType.DataRow:

                HyperLink hypZ = (HyperLink)e.Row.FindControl("hypZID");

                switch (StatusUID.ToUpper())
                {
                    case Constants.QUEUESTATUS_CU_APPROVED:
                    case Constants.QUEUESTATUS_CU_DECLINED:
                    case Constants.QUEUESTATUS_CU_IN_REVIEW:
                    case Constants.QUEUESTATUS_CU_PENDING:
                    case Constants.QUEUESTATUS_CU_WITHDRAWN:
                    case Constants.QUEUESTATUS_CU_RECEIVED_PD:
                        hypZ.NavigateUrl = "~/SecureMerchantManagementForms/frmUnderwriting.aspx?MerchantAppUID=" + DataBinder.Eval(e.Row.DataItem, "UID") + "&Adding=false&PostBackURL=" + this.PostBackURL;

                        break;

                    case Constants.QUEUESTATUS_CU_RECEIVED:
                    case Constants.QUEUESTATUS_CU_3DEDECISION:
                        hypZ.NavigateUrl = "~/SecureMerchantManagementForms/frmUnderwriting.aspx?MerchantAppUID=" + DataBinder.Eval(e.Row.DataItem, "UID") + "&Adding=false&PostBackURL=" + this.PostBackURL;

                        grd.Columns[18].HeaderText = "3DE Status";  // overwriting the header text for CU Recived/CU 3DE queue.
                        BoundField field = (BoundField)grd.Columns[18];
                        field.DataField = "Status3DE";
                        field.SortExpression = "Status3DE";

                        /// DM-1097 by Jorge
                        //if (StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_3DEDECISION)
                        //{
                        //    grd.Columns[5].Visible = false;
                        //    grd.Columns[7].Visible = false;
                        //}

                        break;

                    default:
                        hypZ.NavigateUrl = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + DataBinder.Eval(e.Row.DataItem, "UID") + "&Adding=false&PostBackURL=" + this.PostBackURL;

                        break;
                }

                //PXP-9308 By sanidhya
                if (StatusUID.ToUpper()==Constants.QUEUESTATUS_CU_APPROVED && IsPendingRegistration)
                {
                    #region Master MRP
                    grd.Columns[5].HeaderText = "Master MRP";  // overwriting the header text for FMAID/MRP Status.
                    BoundField _masterMRPField = (BoundField)grd.Columns[5];
                    _masterMRPField.DataField = "MasterMRP";
                    _masterMRPField.SortExpression = "MasterMRP";
                    #endregion

                    /// DM-1097 by Jorge
                    //#region MLE
                    //grd.Columns[7].HeaderText = "MLE";  // overwriting the header text for MCP/MLE.
                    //BoundField _mleField = (BoundField)grd.Columns[7];
                    //_mleField.DataField = "MLE";
                    //_mleField.SortExpression = "MLE";
                    //#endregion

                    #region MRP Status
                    grd.Columns[14].HeaderText = "MRP Status";  // overwriting the header text for MCP/MLE.
                    BoundField _masterStatusField = (BoundField)grd.Columns[14];
                    _masterStatusField.DataField = "MRPStatus";
                    _masterStatusField.SortExpression = "MRPStatus";
                    #endregion

                    #region MRP Date Submitted
                    grd.Columns[17].Visible = true;
                    grd.Columns[17].HeaderText = "MRP Date Submitted";  // overwriting the header text for Assigned Date/MRP Date Submitted.
                    BoundField _mrpDateField = (BoundField)grd.Columns[17];
                    _mrpDateField.DataField = "MRPDateSubmitted";
                    _mrpDateField.SortExpression = "MRPDateSubmitted";

                    #endregion

                    #region Approved By
                    grd.Columns[18].HeaderText = "Approved By";  // overwriting the header text for Assigned Date/MRP Date Submitted.
                    BoundField _approvedByField = (BoundField)grd.Columns[18];
                    _approvedByField.DataField = "ApprovedBy";
                    _approvedByField.SortExpression = "ApprovedBy";
                    #endregion

                }
				
				switch (Type)
                {
                    case Constants.QueueCU_Status:
                    case Constants.QueueSS_Status: // RM Queue is now changed to SS

                        HyperLink hyDBAName = (HyperLink)e.Row.FindControl("hypDBAName");

                        bool has_conditions = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "HasConditions"));

                        if (!has_conditions)
                        {
                            hyDBAName.NavigateUrl = null;
                            hyDBAName.Style.Add("color", "black");
                            hyDBAName.Style.Add("text-decoration", "none");
                        }
                        if (Type?.ToUpper() == Constants.QueueSS_Status) // New Column SS QA Rep
                        {
                            grd.Columns[20].Visible = true;  
                            e.Row.Cells[20].Visible = true;
                        }
                       
                        grd.Columns[10].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        break;
                    case "DE":
                        grd.Columns[10].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        break;
                    case "RE":
                        grd.Columns[10].Visible = false;
                        e.Row.Cells[10].Visible = false;
                        break;
                    default:
                        break;
                }

                string Id = DataBinder.Eval(e.Row.DataItem, "ID").ToString();
                string UID = DataBinder.Eval(e.Row.DataItem, "UID").ToString();

                e.Row.Cells[0].Visible = isTrue;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = isTrue;
                //PXP-9308 By Sanidhya
                if (StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && IsPendingRegistration)
                {
                    e.Row.Cells[17].Visible = true;
                }
                else
                {
                    e.Row.Cells[17].Visible = ConditionDate;
                } 
                e.Row.Cells[18].Visible = (Type.ToUpper() == Constants.QueueCU_Status || Type.ToUpper() == Constants.QueueSS_Status);// AP Queue is now changed to RM
                e.Row.Cells[19].Visible = !isTrue;
                e.Row.Cells[14].Visible = (Type.ToUpper() == Constants.QueueCU_Status && StatusUID.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED);
                grd.Columns[14].Visible = (Type.ToUpper() == Constants.QueueCU_Status && StatusUID.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED);

                if (e.Row.Cells[16].Visible)
                {
                    ListHandler.ListFindItem(((DropDownList)e.Row.Cells[16].FindControl("ddpBy")), DataBinder.Eval(e.Row.DataItem, "TrainingBy").ToString());
                    if (DataBinder.Eval(e.Row.DataItem, "TrainingBy").ToString() == "Reschedule Training")
                        ((WebDatePicker)e.Row.Cells[16].FindControl("TrainingDate")).Value = DataBinder.Eval(e.Row.DataItem, "TrainingDate").ToString();

                    ((DropDownList)e.Row.Cells[16].FindControl("ddpBy")).Attributes.Add("onchange", "TogglePanel('" + ((DropDownList)e.Row.Cells[16].FindControl("ddpBy")).ClientID + "','" + UID + "')");
                }

                ((HtmlImage)e.Row.FindControl("img1")).Attributes.Add("onclick", "CollapseExpand('" + Id + "','" + DataBinder.Eval(e.Row.DataItem, "TrainingBy").ToString() + "','" + ((HtmlImage)e.Row.FindControl("img1")).ClientID + "','" + UID + "')");

                

                string receiveddate = WebUtil.ConvertToUserDateTimeSettings(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                string queuedate = WebUtil.ConvertToUserDateTimeSettings(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "StatusChangedDate")));
                string duedate = WebUtil.ConvertToUserShortDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ConditionalDueDate")));
                e.Row.Cells[8].Text = string.IsNullOrEmpty(receiveddate) ? "" : receiveddate;
                e.Row.Cells[9].Text = string.IsNullOrEmpty(queuedate) ? "" : queuedate;
                if (e.Row.Cells[17].Visible) //Code fix for PXP-13997
                    e.Row.Cells[17].Text = string.IsNullOrEmpty(duedate) ? "" : duedate;

                bool isRollover = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "IsRolloverAccount"));

                if (isRollover)
                {
                    e.Row.BackColor = System.Drawing.Color.Gainsboro;
                }

                //DM-4298 ini
				if (lblTitle.Text == QueueCU.CONDITIONAL_APPROVALS)
				{
					grd.Columns[18].HeaderText = "Approved By";  // overwriting the header text.
					BoundField _approvedByField = (BoundField)grd.Columns[18];
					_approvedByField.DataField = "ApprovedBy";
					_approvedByField.SortExpression = "ApprovedBy";
				}
                //DM-4298 end
				break;

            case DataControlRowType.Footer:

                e.Row.Cells[0].Visible = isTrue;
                e.Row.Cells[15].Visible = false;
                e.Row.Cells[16].Visible = isTrue;
                //PXP-9308 By Sanidhya
                if (StatusUID.ToUpper() == Constants.QUEUESTATUS_CU_APPROVED && IsPendingRegistration)
                {
                    e.Row.Cells[17].Visible = true;
                }
                else
                {
                    e.Row.Cells[17].Visible = ConditionDate;
                }
                e.Row.Cells[18].Visible = (Type.ToUpper() == Constants.QueueCU_Status || Type.ToUpper() == Constants.QueueSS_Status);
                e.Row.Cells[19].Visible = !isTrue;
                e.Row.Cells[14].Visible = (Type.ToUpper() == Constants.QueueCU_Status && StatusUID.ToUpper() != Constants.QUEUESTATUS_CU_RECEIVED); 

                break;

            default:
                break;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string merchantappuid = string.Empty;
        string assignto = string.Empty;

        MerchantApp app = null;

        if (e.CommandName == "ZID" || e.CommandName == "ACHID")
        {
            LinkButton btn = (LinkButton)e.CommandSource;
            if (btn.Text == "0")
                return;

            MerchantFacade facade = new MerchantFacade();
            app = facade.GetMerchantAppZeus(e.CommandArgument.ToString());
            UserSessions.CurrentMerchantApp = app;

            status = app.StatusName;
            merchantappuid = app.MerchantAppUID.ToUpper();
        }

        switch (e.CommandName)
        {
            case "ZID":

                if (app != null)
                {
                    if (status.ToUpper().Substring(0, 2) != Constants.QueueCU_Status)
                    {
                        url = "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID=" + app.MerchantAppUID + "&PostBackURL=" + this.PostBackURL;
                    }
                    else
                    {
                        url = "~/SecureMerchantManagementForms/frmUnderwriting.aspx?Adding=false&MerchantAppUID=" + app.MerchantAppUID + "&PostBackURL=" + this.PostBackURL;
                    }

                    Response.Redirect(url);
                }
                break;

        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.BindGrid();
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        this.grd.PageSize = this.PageSize;
        this.BindGrid();
    }

    private int ConvertSortDirectionToSql(SortDirection direction)
    {
        int newSortDirection;

        switch (direction)
        {
            case SortDirection.Descending:
                newSortDirection = 1;
                this.SortDirectionSearch = SortDirection.Descending;
                break;

            default:
                newSortDirection = 0;
                this.SortDirectionSearch = SortDirection.Ascending;
                break;
        }
        return newSortDirection;
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
    }

    protected void odsMerchants_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
        e.InputParameters[3] = this.ID;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        GridViewRow grdRow = (GridViewRow)((Button)sender).NamingContainer;
        MerchantFacade facade = new MerchantFacade();
        MerchantApp app = facade.GetMerchantAppZeus(grd.DataKeys[grdRow.RowIndex].Values["UID"].ToString());

        app.TrainingBy = ((DropDownList)grdRow.Cells[15].FindControl("ddpBy")).SelectedItem.Text;

        if (app.TrainingBy == "Reschedule Training")
            app.TrainingDate = DataLayer.Field2Date(((WebDatePicker)grdRow.Cells[15].FindControl("TrainingDate")).Value);
        else
            app.StatusUID = Constants.QUEUESTATUS_MS_SCHEDULED_WELCOME_CALL;

        facade.UpdateMerchantApp(app);
        this.BindGrid();
    }

    protected void AllDates_CheckedChanged(object sender, EventArgs e)
    {
        ConditionalDueDate.Enabled = !AllDates.Checked;
        if (AllDates.Checked)
            ConditionalDueDate.Value = null;
        else
            ConditionalDueDate.Value = DateTime.Today.AddDays(1);
        if (this.CheckChanged != null)
        {
            this.CheckChanged(sender, e);
        }
    }

    protected void ConditionalDueDate_ValueChanged(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e)
    {
        if (this.ValueChanged != null)
        {
            this.ValueChanged(sender, e);
        }
    }


    protected void btnExport_Click(object sender, EventArgs e)
    {
        Export = true;

        if (rdExport.SelectedValue.Equals("1"))
        {
            this.PageSize = 5000;
            this.CurrentPage = 1;

        }
        else
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedValue);
            this.CurrentPage = grd.PageIndex + 1;
        }

        if (this.grd.SortExpression != "")
        {
            this.SortOrder = this.grd.SortExpression;
        }
        else
        {
            this.SortOrder = "StatusChangedDate";
        }
        this.grd.PageSize = this.PageSize;
        String newDataFormatString = "{0:MM/dd/yy H:mm:ss}";
        BoundField bfReceivedDate = grd.Columns[8] as BoundField;
        BoundField bfQueueDate = grd.Columns[9] as BoundField;
        bfReceivedDate.DataFormatString = newDataFormatString;
        bfQueueDate.DataFormatString = newDataFormatString;
        this.BindGrid();

        Export = false;
        string FileName = this.grd.Parent.ClientID.ToString();
        string Pattern = "_pnl";
        string[] Result = Regex.Split(FileName, Pattern);
        FormHandler.Export2Excel(Result[1] + ".xls", grd);

    }





}
