using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using System.Text;

namespace ZeusWeb.UserControls.FirstTeam
{
    public partial class wucFTGridChangelog : wucBaseSearch
    {
        private bool _ShowMerchantID = true;

        public bool ShowMerchantID
        {
            get { return _ShowMerchantID; }
            set { _ShowMerchantID = value; }
        }

        private bool _ShowFTRep = true;

        public bool ShowFTRep
        {
            get { return _ShowFTRep; }
            set { _ShowFTRep = value; }
        }

        private bool _ShowBusinessDBAName = true;

        public bool ShowBusinessDBAName
        {
            get { return _ShowBusinessDBAName; }
            set { _ShowBusinessDBAName = value; }
        }

        public Dictionary<int, MRule> m_VSMRule;

        protected void Page_Load(object sender, EventArgs e)
        {

            this.GridView1.PreRender += new EventHandler(GridView1_PreRender);

        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {

            GridView1.Columns[3].Visible = this.ShowMerchantID;
            GridView1.Columns[4].Visible = this.ShowFTRep;
            GridView1.Columns[5].Visible = this.ShowBusinessDBAName;
            
        }

        /// <summary>
        /// to be called from parent controls to refresh the grid.
        /// </summary>
        public void RefreshGrid()
        {
            GridView1.DataSourceID = "ObjectDataSource1";

            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
            }

            this.BindGrid();
        }

        protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters[0] = this.m_Prms;
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

            //if (!string.IsNullOrEmpty(this._ObjectName))
            //{
            //    m_Prms["@ObjectName"] = this._ObjectName;
            //}

            //if (!string.IsNullOrEmpty(this._MerchantAppUID))
            //{
            //    m_Prms["@UID"] = this._MerchantAppUID;
            //}

            //m_Prms["@PortalUID"] = UserSessions.PortalUID;

            int rowcount = 0;

            List<ChangeLogs> li = DataChangeLogs.SearchChangeLogsFT_Paging(m_Prms);

            if (li != null && li.Count > 0)
            {
                rowcount = li[0].TotalRecordCount;
            }

            lblRowCount.Text = rowcount.ToString();


            GridView1.DataBind();
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
                ChangeLogs item = (ChangeLogs)e.Row.DataItem;

                if (this.m_VSMRule == null)
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@RoleUID", Constants.ROLE_FIRSTTEAM);
                    this.m_VSMRule = DataMRule.SearchMRule(prms);
                }

                bool is_param = false;

                ///////////////////// handle rule name column

                if (this.m_VSMRule != null)
                {

                    Literal litRN = (Literal)e.Row.FindControl("litRuleName");

                    switch (item.ObjectName.ToLower())
                    {
                        case "mruleparam":

                            foreach (KeyValuePair<int, MRule> kvp in this.m_VSMRule)
                            {
                                int mruleparamid = Convert.ToInt32(item.ID);
                                if (kvp.Value.MRuleParamDict != null && kvp.Value.MRuleParamDict.Count > 0 && kvp.Value.MRuleParamDict.ContainsKey(mruleparamid))
                                {
                                    litRN.Text = kvp.Value.RuleNameNice;
                                    break;
                                }
                            }

                            is_param = true;

                            break;

                        case "mrule":

                            litRN.Text = this.m_VSMRule[Convert.ToInt32(item.ID)].RuleNameNice;
                            break;

                    }
                }


                ///////////////////// handle change column

                Literal lit = (Literal)e.Row.FindControl("litChanges");

                Dictionary<string, string[]> di = CommonUtility.Formatting.ParseLogItem(item.Change);

                StringBuilder sb = new StringBuilder();

                if (di != null && di.Count > 0)
                {
                    sb.Append(@"<table class='changeloginner' >
                    <tr>
                        <th>Field</th>
                        <th>Old Value</th>
                        <th>New Value</th>
                    </tr>
                    ");

                    foreach (KeyValuePair<string, string[]> kvp in di)
                    {

                        string pval = kvp.Key;

                        int my_mruleparamid = CommonUtility.Util.if_i(item.ID, 0);

                        if (is_param)
                        {
                            foreach (KeyValuePair<int, MRule> x in this.m_VSMRule)
                            {
                                if (x.Value.MRuleParamDict != null && x.Value.MRuleParamDict.Count > 0)
                                {
                                    if (x.Value.MRuleParamDict.ContainsKey(my_mruleparamid))
                                    {
                                        pval = x.Value.MRuleParamDict[my_mruleparamid].ParamName;
                                        break;
                                    }

                                }

                            }
                        }

                        if (pval.ToLower().Equals("enddate") || pval.ToLower().Trim().Equals("startdate"))
                        {
                            DateTime oldVal;
                            DateTime newVal;
                            if (DateTime.TryParse(kvp.Value[0], out oldVal) && DateTime.TryParse(kvp.Value[0], out newVal))
                            {
                                sb.AppendFormat(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", pval, WebUtil.ConvertToUserDatePattern(kvp.Value[0]), WebUtil.ConvertToUserDatePattern(kvp.Value[1]));
                            }
                            else
                            {
                                sb.AppendFormat(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", pval, kvp.Value[0], kvp.Value[1]);
                            }
                        }
                        else
                        {
                            sb.AppendFormat(@"<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>", pval, kvp.Value[0], kvp.Value[1]);
                        }
                    }

                    sb.Append("</table>");
                }

                lit.Text = sb.ToString();


                e.Row.Cells[0].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[0].Text);


            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.BindGrid();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(ddlPageSize.SelectedItem.Value);
            this.m_Prms["@PageSize"] = this.PageSize;
            this.SetDataSource(this.m_Prms);
        }


    }
}