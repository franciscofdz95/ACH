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
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using Infragistics.WebUI.WebDataInput;
using System.Text.RegularExpressions;

public partial class SecureReports_frmMeritusNews : frmBaseSearch
{

    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
            {
                return false;
            }
            else
            {
                return Convert.ToBoolean(ViewState["EditMode"]);
            }
        }
        set { ViewState["EditMode"] = value; }

    }

    public bool Adding
    {
        get
        {
            if (ViewState["Adding"] != null)
            {
                return Convert.ToBoolean(ViewState["Adding"]);
            }
            else
            {
                return true;
            }
        }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        this.always_init();

        if (!IsPostBack)
        {
            this.initialize();
        }
    }

    protected void always_init()
    {

    }

    protected void initialize()
    {
        //Apply security settings
        FormHandler.SetSecurity(this.Page);

        //load all dropdownlists
        LookupTableHandler.LoadPortals(PortalUID, true);

        this.Search(true);

        // on pageload, we always show the search.
        pnlEdit.Visible = false;
        pnlSearch.Visible = true;

        //pnlNoRecords.Visible = false;
        //pnlRecords.Visible = false;

        // handle buttons
        btnEdit.Enabled = false;
        btnAdd.Enabled = true;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;
        btnRefresh.Enabled = false;

        //gvMeritusNews.PageSize = this.PageSize;
        //gvMeritusNews.AllowPaging = true;





    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.Search(false);

    }

    protected void gvMeritusNews_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lbHC = (Label)e.Row.FindControl("lblHtmlContent");
            DataRowView drv = (DataRowView)e.Row.DataItem;

            string fulltext =  Regex.Replace(drv["HTMLContent"].ToString(), @"<(.|\n)*?>", string.Empty);

            lbHC.Text = CommonUtility.Util.TruncateText(fulltext, 50);
            lbHC.ToolTip = CommonUtility.Util.TruncateText(fulltext, 500);

        }
    }

    protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
        gvMeritusNews.PageSize = this.PageSize;
        gvMeritusNews.PageIndex = this.CurrentPage - 1;
        this.Search(false);
    }

    public override void Search(bool IsOnLoad)
    {
        //Populate search fields        
        if (IsOnLoad && this.SearchParameters != null)
        {
            SearchParameter spMA = (SearchParameter)this.SearchParameters;
            FormBinding.BindObjectToControls(spMA, pnlSearch);
        }
        gvMeritusNews.DataBind();
        pnlRecords.Visible = gvMeritusNews.Rows.Count > 0;
        pnlNoRecords.Visible = gvMeritusNews.Rows.Count == 0;


    }

    protected void gvMeritusNews_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.Search(false);
    }

    protected void btnClear_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.FormClear();
    }

    protected void btnSearch_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        this.CurrentPage = 1;
        gvMeritusNews.PageIndex = this.CurrentPage - 1;
        this.SortOrder = string.Empty;
        //this.IsKeywordSearch = false;
        this.SearchParameters = null;
        this.Search(false);
    }

    private void FormClear()
    {
        this.SearchParameters = null;
        FormHandler.ClearAllControls(this);

        gvMeritusNews.Columns.Clear();
        ListHandler.ListFindItem(cboPageSize, this.PageSize.ToString());
        this.Search(false);

        pnlRecords.Visible = false;
        pnlNoRecords.Visible = true;
    }

    protected void gvMeritusNews_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow grdRow = null;
        if (e.CommandSource is LinkButton)
        {
            switch (e.CommandName.ToLower())
            {
                case "edit":

                    grdRow = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer);

                    HiddenField hfPortalUID = (HiddenField)grdRow.FindControl("hidPortalUID");
                    HiddenField hfNewsUID = (HiddenField)grdRow.FindControl("hidNewsUID");

                    Hashtable prms = new Hashtable();
                    prms.Add("@UID", hfNewsUID.Value.Trim());
                    prms.Add("@PortalUID", hfPortalUID.Value.Trim());
                    prms.Add("DisregardDisabledDate", "1");

                    IList<MeritusNews> liMA = DataCCBatchDeposits.GetNews(prms);

                    if (liMA.Count == 1)
                    {
                        pnlEdit.Visible = true;
                        WucMeritusNewsDetail1.FillMeritusNews(liMA[0]);
                        WucMeritusNewsDetail1.SetControlEditMode(false);
                        pnlSearch.Visible = false;
                        this.SetButtons(eButtonSet.ReadOnly);
                    }

                    break;


            }
        }
    }

    protected void odsMertiusNews_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@PageSize", this.PageSize);
        prms.Add("@CurrentPage", this.CurrentPage);
        prms.Add("@SortOrder", this.SortOrder);

        prms.Add("@SortDirection", SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch));

        if (CommonUtility.Util.if_s(NewsID.Text) != "")
        {
            prms.Add("@NewsID", NewsID.Text.ToString());
        }

        if (PortalUID.SelectedValue != "-1")
        {
            prms.Add("@PortalUID", PortalUID.SelectedValue);
        }

        if (CommonUtility.Util.if_s(Subject.Text) != "")
        {
            prms.Add("@Subject", Subject.Text.Trim());
        }

        if (CommonUtility.Util.if_s(HTMLContent.Text) != "")
        {
            prms.Add("@HTMLContent", HTMLContent.Text.Trim());
        }

        switch (Active.SelectedValue)
        {
            case "active":
                prms.Add("@OnlyShowActive", 1);
                break;

            case "inactive":
                prms.Add("@OnlyShowInActive", 1);
                break;
        }


        e.InputParameters[0] = prms;

        if (prms.Count > 0)
        {
            //Save search fields in session variable
            SearchParameter objMN = new SearchParameter();
            FormBinding.BindControlsToObject(objMN, pnlSearch);
            this.SearchParameters = objMN;
        }

        lblRecordCount.Text = DataMerchantAppPaging.SelectMeritusNews_PagingCount(prms, this.PageSize, this.CurrentPage).ToString();


    }

    protected IEnumerable<string> ValidateMeritusNews()
    {
        if (string.IsNullOrEmpty(WucMeritusNewsDetail1.CSubject))
        {
            yield return "Subject Required";
        }

        if (string.IsNullOrEmpty(WucMeritusNewsDetail1.CUrl))
        {
            yield return "URL Required";
        }

        if (string.IsNullOrEmpty(WucMeritusNewsDetail1.CPortalUID))
        {
            yield return "Portal Required";
        }

        if (string.IsNullOrEmpty(WucMeritusNewsDetail1.CHTMLContent))
        {
            yield return "Html Body Required";
        }

    }

    protected bool DataCheck()
    {
        foreach (string str in this.ValidateMeritusNews())
        {
            WucMessage1.AddMessageError(str);
        }

        return (WucMessage1.ErrorCount() == 0) ? true : false;
    }

    protected bool FormSave()
    {
        bool blnRet = false;

        if (this.DataCheck())
        {
            WucMeritusNewsDetail1.BindControlsToObject();
            //FormBinding.BindControlsToObject(WucMeritusNewsDetail1.ObjMN, pnlEdit);
            DataMeritusNews dataMA = DataAccess.DataMeritusNewsDao;
            if (WucMeritusNewsDetail1.ObjMN.NewsID == 0)
            {
                // add
                if (dataMA.InsertMeritusNews(WucMeritusNewsDetail1.ObjMN) == 1)
                {
                    blnRet = true;
                }
            }
            else if (WucMeritusNewsDetail1.ObjMN.NewsID > 0)
            {
                // edit
                if (dataMA.UpdateMeritusNews(WucMeritusNewsDetail1.ObjMN) == 1)
                {
                    blnRet = true;
                }
            }

        }

        return blnRet;
    }

    protected void FormNew()
    {
        pnlEdit.Visible = true;
        pnlSearch.Visible = false;
        WucMeritusNewsDetail1.Clear();
        WucMeritusNewsDetail1.FillMeritusNews(new MeritusNews());
    }

    protected void FormShow()
    {

    }

    protected void FormCancel()
    {

    }

    protected void FormEdit()
    {

    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Add":
                this.FormNew();
                WucMeritusNewsDetail1.SetControlEditMode(true);
                this.SetButtons(eButtonSet.Add);

                break;

            case "Save":

                bool isAdding = (WucMeritusNewsDetail1.ObjMN.NewsID == 0) ? true : false;

                if (this.FormSave())
                {

                    //this.Adding = false;
                    //this.EditMode = false;
                    //pnlSearch.Visible = true;
                    //pnlEdit.Visible = false;

                    if (isAdding)
                    {
                        WucMessage1.AddMessageStatus("New News Article Successfully Added. NewsID = " + WucMeritusNewsDetail1.ObjMN.NewsID.ToString());
                        Response.Redirect("~/SecureReports/frmMeritusNews.aspx");
                    }
                    else
                    {
                        WucMessage1.AddMessageStatus("News Article Successfully Edited");
                        this.Adding = false;
                        this.EditMode = false;
                        pnlSearch.Visible = false;
                        pnlEdit.Visible = true;
                        SetButtons(eButtonSet.ReadOnly);
                    }
                }
                break;

            case "Refresh":

                this.FormShow();
                break;

            case "Cancel":

                if (this.Adding)
                {
                    this.SetButtons(eButtonSet.Search);
                    pnlSearch.Visible = true;
                    pnlEdit.Visible = false;
                }
                else
                {
                    if (this.EditMode)
                    {
                        this.EditMode = false;
                        this.SetButtons(eButtonSet.ReadOnly);
                    }
                    else
                    {
                        // go back to search mode.
                        pnlSearch.Visible = true;
                        pnlEdit.Visible = false;
                        this.SetButtons(eButtonSet.Search);
                    }
                }

                gvMeritusNews.DataBind();

                break;

            case "Edit":
                this.EditMode = true;
                this.FormShow();
                WucMeritusNewsDetail1.SetControlEditMode(true);
                this.SetButtons(eButtonSet.Edit);

                break;
        }
    }

    private void SetButtons(eButtonSet eBS)
    {
        // clear all first.
        btnEdit.Enabled = false;
        btnAdd.Enabled = false;
        btnSave.Enabled = false;
        btnCancel.Enabled = false;
        btnRefresh.Enabled = false;
        hypBack.Visible = false;

        pnlToolbar.Visible = false;
        switch (eBS)
        {
            case eButtonSet.Add:
                pnlToolbar.Visible = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                hypBack.Visible = true;

                break;

            case eButtonSet.Edit:
                pnlToolbar.Visible = true;
                btnSave.Enabled = true;
                btnCancel.Enabled = true;
                hypBack.Visible = true;
                break;

            case eButtonSet.ReadOnly:
                pnlToolbar.Visible = true;
                btnEdit.Enabled = true;
                btnCancel.Enabled = true;
                btnRefresh.Enabled = true;
                hypBack.Visible = true;
                break;

            case eButtonSet.Search:
                btnAdd.Enabled = true;
                break;

        }
    }

}

