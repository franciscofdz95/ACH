using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Infragistics.Web.UI.EditorControls;

namespace ZeusWeb.UserControls
{
    public partial class wucFTRuleFilter : wucBaseSearch
    {
        private bool _ShowZID = true;

        public bool ShowZID
        {
            get { return _ShowZID; }
            set { _ShowZID = value; }
        }

        private bool _ShowBusinessDBAName = true;

        public bool ShowBusinessDBAName
        {
            get { return _ShowBusinessDBAName; }
            set { _ShowBusinessDBAName = value; }
        }

        private bool _ShowBusinessLegalName = true;

        public bool ShowBusinessLegalName
        {
            get { return _ShowBusinessLegalName; }
            set { _ShowBusinessLegalName = value; }
        }

        private bool _ShowFTRep = true;

        public bool ShowFTRep
        {
            get { return _ShowFTRep; }
            set { _ShowFTRep = value; }
        }

        //public delegate void MerchantEnabledClickHandler(object sender, bool e);
        //public event MerchantEnabledClickHandler MerchantEnabledClick;

        public delegate void RefreshClickHandler(object sender, string str);
        public event RefreshClickHandler RefreshClick;

        public delegate void SnoozeClickHandler(object sender, int merchantid, int mruleid);
        public event SnoozeClickHandler SnoozeClick;

        //public Hashtable m_Prms
        //{
        //    get
        //    {
        //        if (ViewState["m_Prms"] == null)
        //            return null;
        //        else
        //            return (Hashtable)ViewState["m_Prms"];
        //    }
        //    set { ViewState["m_Prms"] = value; }
        //}

        private bool _ShowGlobalEnable = true;

        public bool ShowGlobalEnable
        {
            get { return _ShowGlobalEnable; }
            set { _ShowGlobalEnable = value; }
        }

        private Dictionary<int, List<MRuleParam>> VSMRuleParam
        {
            get
            {
                if (ViewState["VSMRuleParam"] == null)
                    return null;
                else
                    return (Dictionary<int, List<MRuleParam>>)ViewState["VSMRuleParam"];
            }
            set { ViewState["VSMRuleParam"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                // build the VSMRuleParam structure
                this.VSMRuleParam = DataMRule.SearchMRuleParam(Constants.ROLE_FIRSTTEAM);
            }

            GridView1.PreRender += new EventHandler(GridView1_PreRender);


        }



        /// <summary>
        /// to be called from parent controls to refresh the grid.
        /// </summary>
        public void RefreshGrid()
        {

            this.BindGrid();
        }


        /// <summary>
        /// set the datasource from the data we current have.
        /// </summary>
        public void SetDataSource()
        {
            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
                this.m_Prms.Add("@PageSize", this.PageSize);
                this.m_Prms.Add("@CurrentPage", this.CurrentPage);
                this.m_Prms.Add("@SortDirection", SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch));
            }

            this.SetDataSource(this.m_Prms);
        }

        /// <summary>
        /// set the datasource from the data stored in mprms and pagesize.
        /// </summary>
        /// <param name="prms"></param>
        /// <param name="pagesize"></param>
        public void SetDataSource(Hashtable prms)
        {
            GridView1.DataSourceID = "ObjectDataSource1";
            this.CurrentPage = 1;
            GridView1.PageIndex = 0;
            GridView1.PageSize = this.PageSize;
            this.m_Prms = prms;
            BindGrid();
        }

        private void BindGrid()
        {

            if (this.VSMRuleParam == null)
            {
                this.VSMRuleParam = DataMRule.SearchMRuleParam(Constants.ROLE_FIRSTTEAM);
            }

            if (!m_Prms.ContainsKey("@PageSize"))
                m_Prms.Add("@PageSize", this.PageSize);
            else
                m_Prms["@PageSize"] = this.PageSize;

            if (!m_Prms.ContainsKey("@CurrentPage"))
                m_Prms.Add("@CurrentPage", this.CurrentPage);
            else
                m_Prms["@CurrentPage"] = this.CurrentPage;

            if (!m_Prms.ContainsKey("@SortOrder"))
                m_Prms.Add("@SortOrder", "DateCreated");
            else
                m_Prms["@SortOrder"] = this.SortOrder;

            m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

            int rowcount = DataMerchantAppPaging.GetMRuleMerchantPagingCount(m_Prms, this.PageSize, this.CurrentPage);

            lblRowCount.Text = rowcount.ToString();

            GridView1.DataBind();
        }

        #region gridview_callbacks

        protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = this.m_Prms;
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.CurrentPage = 1;
            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;
            this.BindGrid();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MRule objMR = (MRule)e.Row.DataItem;

                LinkButton lsn = (LinkButton)e.Row.FindControl("lbSnooze");

                lsn.CommandArgument = string.Format("{0}_{1}", objMR.MerchantID.ToString(), objMR.MRuleID.ToString());

                CheckBox cbGE = (CheckBox)e.Row.FindControl("cbGlobalEnabled");
                cbGE.Checked = objMR.GlobalEnabled;
                cbGE.Enabled = false; // force it to false.

                CheckBox cbME = (CheckBox)e.Row.FindControl("cbMerchantEnabled");
                cbME.Checked = objMR.MerchantRuleEnabled;

                WebDatePicker tbSt = (WebDatePicker)e.Row.FindControl("tbStartDate");
                tbSt = WebUtil.SetUserSpecificDisplayMode(tbSt);
                tbSt.Value = objMR.StartDate;

                WebDatePicker tbEn = (WebDatePicker)e.Row.FindControl("tbEndDate");
                tbEn = WebUtil.SetUserSpecificDisplayMode(tbEn);
                tbEn.Value = objMR.EndDate;


                // first, we build all the possible param values for this rule.

                if (this.VSMRuleParam.ContainsKey(objMR.MRuleID))
                {
                    List<MRuleParam> templi = this.VSMRuleParam[objMR.MRuleID];

                    foreach (MRuleParam obj in templi)
                    {
                        obj.MerchantID = objMR.MerchantID;
                    }

                    List<MRuleParam> myclone = DataMRule.CloneMRuleParamList(templi);


                    if (objMR.MRuleParamDict != null && objMR.MRuleParamDict.Count > 0)
                    {
                        // then we overlay the custom values onto the possible.
                        foreach (KeyValuePair<int, MRuleParam> kvp in objMR.MRuleParamDict)
                        {
                            foreach (MRuleParam mrp in myclone)
                            {
                                if (mrp.MRuleParamID == kvp.Key)
                                {
                                    mrp.ParamValue = kvp.Value.ParamValue;
                                }
                            }
                        }
                    }

                    GridView gv = (GridView)e.Row.FindControl("grdParams");

                    gv.DataSource = myclone;
                    gv.DataBind();

                    Panel pGrid = (Panel)e.Row.FindControl("pnlGrid");

                    // enable or disable the grid depending on if use defaults is checked.
                    FormHandler.SetControlEditMode(pGrid, !objMR.UseDefaultParams);
                }
                

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.BindGrid();
        }


        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            //switch (e.CommandName)
            //{
            //    case "BusinessDBAName":

            //        LinkButton btn = (LinkButton)e.CommandSource;

            //        string str_merchant_uid = Convert.ToString(e.CommandArgument);

            //        //UserSessions.CurrentMerchantApp = DataAccess.DataMerchantAppDao.GetMerchantApp(str_merchant_uid);

            //        Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=False&MerchantAppUID=" + str_merchant_uid);

            //        break;

            //}

        }



        protected void GridView1_PreRender(object sender, EventArgs e)
        {

            GridView1.Columns[0].Visible = this.ShowGlobalEnable;
            GridView1.Columns[3].Visible = this.ShowZID;
            GridView1.Columns[4].Visible = this.ShowBusinessDBAName;
            GridView1.Columns[5].Visible = this.ShowBusinessLegalName;
            GridView1.Columns[6].Visible = this.ShowFTRep;

            if (GridView1.HeaderRow != null)
            {
                DropDownList dFTRep = (DropDownList)GridView1.HeaderRow.FindControl("ddlFTRep");

                if (dFTRep != null && dFTRep.Items.Count == 0)
                {
                    dFTRep.Items.Clear();

                    IList<GenericListItem> li = DataUser.GetInstance().GetMerchantUsers(Constants.ROLE_FIRSTTEAM);

                    dFTRep.Items.Add(new ListItem("All", ""));

                    foreach (GenericListItem i in li)
                    {
                        dFTRep.Items.Add(new ListItem(i.ItemText, i.ItemValue));
                    }

                    if (this.m_Prms.ContainsKey("@FTRepUserUID") && CommonUtility.Util.IsValidGuid(this.m_Prms["@FTRepUserUID"].ToString()))
                    {
                        dFTRep.SelectedValue = this.m_Prms["@FTRepUserUID"].ToString();
                    }
                }
            }
        }

        #endregion

        #region gridcommands

        protected void cbMerchantEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            HiddenField hf = (HiddenField)cb.Parent.FindControl("hidMRuleID");
            int rule_id = Convert.ToInt32(hf.Value);

            hf = (HiddenField)cb.Parent.FindControl("hidMerchantID");
            int merchant_id = Convert.ToInt32(hf.Value);

            MRule objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);

            objMR.Clone();

            if (objMR != null)
            {
                objMR.MerchantRuleEnabled = cb.Checked;

                if (cb.Checked)
                {
                    // if enabling merchant, we want the default params to be true. (business rule)
                    objMR.UseDefaultParams = true;

                    // check the checkbox to be true too
                    CheckBox cbP = (CheckBox)cb.Parent.FindControl("cbUseDefaultParams");
                    cbP.Checked = true;

                    // disable the param settings
                    Panel pGrid = (Panel)cb.Parent.FindControl("pnlGrid");
                    FormHandler.SetControlEditMode(pGrid, false);
                }

                DataMRule.UpdateMRuleMerchant(objMR);

                if (string.IsNullOrEmpty(objMR.MerchantAppUID))
                {
                    // this means we did an insert for the first time. so just refresh our object. ie, merchantappuid is missing after 1st insert
                    objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);
                }

                // log here
                this.UpdateChangelog(objMR);

                if (this.RefreshClick != null)
                {
                    this.RefreshClick(sender, "batman");
                }
            }

        }

        protected void cbUseDefaultParams_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;

            HiddenField hf = (HiddenField)cb.Parent.FindControl("hidMRuleID");
            int rule_id = Convert.ToInt32(hf.Value);

            hf = (HiddenField)cb.Parent.FindControl("hidMerchantID");
            int merchant_id = Convert.ToInt32(hf.Value);

            MRule objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);

            objMR.Clone();

            if (objMR != null)
            {
                objMR.UseDefaultParams = cb.Checked;

                Panel pgrid = (Panel)cb.Parent.FindControl("pnlGrid");

                // enable/disable the grid depending on if use default is checked.
                FormHandler.SetControlEditMode(pgrid, !cb.Checked);

                DataMRule.UpdateMRuleMerchant(objMR);

                if (string.IsNullOrEmpty(objMR.MerchantAppUID))
                {
                    // this means we did an insert for the first time. so just refresh our object. ie, merchantappuid is missing after 1st insert
                    objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);
                }

                // log here
                this.UpdateChangelog(objMR);

                if (this.RefreshClick != null)
                {
                    this.RefreshClick(sender, "batman");
                }
            }

        }

        protected void tbStartDate_TextChanged(object sender, EventArgs e)
        {

            WebDatePicker tbSt = (WebDatePicker)sender;

            HiddenField hf = (HiddenField)tbSt.Parent.FindControl("hidMRuleID");
            int rule_id = Convert.ToInt32(hf.Value);

            hf = (HiddenField)tbSt.Parent.FindControl("hidMerchantID");
            int merchant_id = Convert.ToInt32(hf.Value);

            MRule objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);

            objMR.Clone();

            DateTime dtS = (DateTime)tbSt.Value;

            if (dtS != DateTime.MinValue)
            {
                // valid date, save
                if (objMR != null)
                {
                    objMR.StartDate = dtS;
                    DataMRule.UpdateMRuleMerchant(objMR);

                    if (string.IsNullOrEmpty(objMR.MerchantAppUID))
                    {
                        // this means we did an insert for the first time. so just refresh our object. ie, merchantappuid is missing after 1st insert
                        objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);
                    }

                    // log here.
                    this.UpdateChangelog(objMR);

                    if (this.RefreshClick != null)
                    {
                        this.RefreshClick(sender, "batman");
                    }
                }
            }

        }

        protected void tbEndDate_TextChanged(object sender, EventArgs e)
        {

            WebDatePicker tbEn = (WebDatePicker)sender;

            HiddenField hf = (HiddenField)tbEn.Parent.FindControl("hidMRuleID");
            int rule_id = Convert.ToInt32(hf.Value);

            hf = (HiddenField)tbEn.Parent.FindControl("hidMerchantID");
            int merchant_id = Convert.ToInt32(hf.Value);


            MRule objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);

            objMR.Clone();

            DateTime dtE;

            dtE = (DateTime)tbEn.Value;


            if (dtE != DateTime.MinValue)
            {
                // valid date, save
                if (objMR != null)
                {
                    objMR.EndDate = dtE;
                    DataMRule.UpdateMRuleMerchant(objMR);

                    if (string.IsNullOrEmpty(objMR.MerchantAppUID))
                    {
                        // this means we did an insert for the first time. so just refresh our object. ie, merchantappuid is missing after 1st insert
                        objMR = DataMRule.GetMRuleMerchant(merchant_id, rule_id, Constants.ROLE_FIRSTTEAM);
                    }

                    this.UpdateChangelog(objMR);


                    if (this.RefreshClick != null)
                    {
                        this.RefreshClick(sender, "batman");
                    }
                }
            }


        }

        protected void tbParamValue_TextChanged(object sender, EventArgs e)
        {

            TextBox tbPV = (TextBox)sender;

            if (CommonUtility.Util.IsValidInt32(tbPV.Text))
            {
                HiddenField hfMRuleID = (HiddenField)tbPV.Parent.FindControl("hidMRuleID");
                HiddenField hfMRuleParamID = (HiddenField)tbPV.Parent.FindControl("hidMRuleParamID");
                HiddenField hfMerchantID = (HiddenField)tbPV.Parent.FindControl("hidMerchantID");

                int rule_id = CommonUtility.Util.if_i(hfMRuleID.Value, 0);
                int ruleparam_id = CommonUtility.Util.if_i(hfMRuleParamID.Value, 0);
                int merchant_id = CommonUtility.Util.if_i(hfMerchantID.Value, 0);

                //if (merchant_id == 0)
                //{
                //    // this means the mruleparammerchant row does not exists, so lets pull the merchant from this row.
                //    HiddenField hfpMerchantID = (HiddenField)tbPV.Parent.Parent.FindControl("hidMerchantID");
                //    merchant_id = CommonUtility.Util.if_i(hfpMerchantID.Value, 0);
                //}

                MRuleParam mpr = DataMRule.GetMRuleParamMerchant(merchant_id, ruleparam_id, rule_id);

                mpr.Clone();

                if (mpr != null)
                {
                    mpr.ParamValue = tbPV.Text;
                    DataMRule.UpdateMRuleParamMerchant(mpr);

                    if (string.IsNullOrEmpty(mpr.MerchantAppUID))
                    {
                        // this means we did an insert for the first time. so just refresh our object. ie, merchantappuid is missing after 1st insert
                        MRuleParam mpr_new = DataMRule.GetMRuleParamMerchant(merchant_id, ruleparam_id, rule_id);
                    }

                    this.UpdateChangelog(mpr);

                    if (this.RefreshClick != null)
                    {
                        this.RefreshClick(sender, "batman");
                    }
                }
            }

        }

        #endregion

        #region changelog

        private void UpdateChangelog(MRule objMR)
        {

            string DifferenceLog = "";

            // get differences before you save.
            if (objMR.MRuleClone != null)
            {
                DifferenceLog = objMR.GetDifferences();
            }

            // no exception, then save if there's any differences
            if (!string.IsNullOrEmpty(DifferenceLog))
            {
                DataUser.GetInstance().InsertChangeLog(
                    objMR.BusinessDBAName
                    , UserSessions.CurrentUser.UserName
                    , CommonUtility.Util.if_s(objMR.MerchantAppUID, null)
                    , objMR.MRuleID.ToString()
                    , "MRule"
                    , DifferenceLog
                    , UserSessions.PortalUID
                    );

            }
        }

        private void UpdateChangelog(MRuleParam objMRP)
        {

            string DifferenceLog = "";

            // get differences before you save.
            if (objMRP.MRuleParamClone != null)
            {
                DifferenceLog = objMRP.GetDifferences();
            }

            // no exception, then save if there's any differences
            if (!string.IsNullOrEmpty(DifferenceLog))
            {
                DataUser.GetInstance().InsertChangeLog(
                    objMRP.BusinessDBAName
                    , UserSessions.CurrentUser.UserName
                    , CommonUtility.Util.if_s(objMRP.MerchantAppUID, null)
                    , objMRP.MRuleParamID.ToString()
                    , "MRuleParam"
                    , DifferenceLog
                    , UserSessions.PortalUID
                    );

            }
        }

        #endregion

        #region header_delegates

        protected void ddlFTRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            if (ddl.SelectedValue == "")
            {
                if (this.m_Prms.ContainsKey("@FTRepUserUID"))
                {
                    this.m_Prms.Remove("@FTRepUserUID");
                    this.BindGrid();
                }
            }
            else if (CommonUtility.Util.IsValidGuid(ddl.SelectedValue))
            {
                this.m_Prms["@FTRepUserUID"] = ddl.SelectedValue;
                this.BindGrid();
            }



        }

        #endregion


        protected void lbSnooze_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            if (lb.CommandName == "snooze" && lb.CommandArgument.Contains("_"))
            {
                string[] arr = lb.CommandArgument.Split(new char[] { '_' });

                int merchantid = CommonUtility.Util.if_i(arr[0], 0);

                int mruleid = CommonUtility.Util.if_i(arr[1], 0);

                if (this.SnoozeClick != null)
                {
                    this.SnoozeClick(sender, merchantid, mruleid);
                }
            }
        }


        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(ddlPageSize.SelectedItem.Value);
            this.m_Prms["@PageSize"] = this.PageSize;
            this.SetDataSource(this.m_Prms);

        }


        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {

            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;

            this.SetDataSource(this.m_Prms);

        }



    }
}