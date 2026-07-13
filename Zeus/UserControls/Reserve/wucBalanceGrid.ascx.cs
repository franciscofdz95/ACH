using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucBalanceGrid : wucBaseSearch
    {
        public delegate void EventClickZID(int zid);
        public event EventClickZID event_click_zid;

        public bool HasPending
        {
            get { return (bool)(this.ViewState["HasPending"] ?? false); }
            set { this.ViewState["HasPending"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.m_Prms = this.m_Prms ?? new Hashtable();
        }


        /// <summary>
        /// to be called from parent controls to refresh the grid.
        /// </summary>
        public void RefreshGrid()
        {
            GridView1.DataSourceID = "ods";

            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
            }

            this.BindGrid();
        }

        protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.m_Prms == null)
            {
                e.Cancel = true;
            }
            else
            {
                e.InputParameters[0] = this.m_Prms;
            }
        }


        /// <summary>
        /// set the datasource from the data stored in mprms and pagesize. returns the zid if only 1 record was found.
        /// </summary>
        /// <param name="prms"></param>
        /// <param name="pagesize"></param>
        public int SetDataSource(Hashtable prms)
        {

            int single_zid = 0;

            GridView1.DataSourceID = "ods";
            this.CurrentPage = 1;
            GridView1.PageIndex = 0;
            GridView1.PageSize = this.PageSize;
            this.m_Prms = prms;
            single_zid = BindGrid();


            return single_zid;
        }


        /// <summary>
        /// returns zid if only 1 row found.
        /// </summary>
        /// <returns></returns>
        private int BindGrid()
        {
            int single_zid = 0;
            if (this.m_Prms != null)
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

                GridView1.PageSize = this.PageSize;
                GridView1.PageIndex = this.CurrentPage - 1;

                int rowcount = DataMerchantAppPaging.GetReserveBalancePagingCount(m_Prms, this.PageSize, this.CurrentPage);

                lblRowCount.Text = rowcount.ToString();

                GridView1.DataBind();

                if (GridView1.Rows != null && GridView1.Rows.Count == 1)
                {
                    LinkButton lbZID = (LinkButton)GridView1.Rows[0].FindControl("lnkZID");
                    single_zid = CommonUtility.Util.if_i(lbZID.Text, 0);
                }

            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            return single_zid;
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
                this.HasPending = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "HasPending"), false);

                Label lbPending = (Label)e.Row.FindControl("lblPending");

                lbPending.Visible = this.HasPending;
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.BindGrid();
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            LinkButton lnk = null;


            if (e.CommandSource is LinkButton)
            {
                lnk = (LinkButton)e.CommandSource;

                switch (lnk.CommandName)
                {
                    case "ZID":

                        int zid = CommonUtility.Util.if_i(e.CommandArgument, 0);

                        if (zid > 0 && event_click_zid != null)
                        {

                            event_click_zid(zid);
                        }

                        break;
                }
            }




        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.m_Prms["@CurrentPage"] = 1;

            this.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            this.m_Prms["@PageSize"] = this.PageSize;

            this.BindGrid();
        }


    }
}