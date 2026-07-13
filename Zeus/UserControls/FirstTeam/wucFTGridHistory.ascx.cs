using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using System.Text;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.FirstTeam
{
    public partial class wucFTGridHistory : wucBaseSearch
    {



        protected void Page_Load(object sender, EventArgs e)
        {

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

            int rowcount = DataMerchantAppPaging.GetMRuleRunLogPagingCount(m_Prms, this.PageSize, this.CurrentPage);

            lblRowCount.Text = rowcount.ToString();

            GridView1.PageSize = this.PageSize;
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
            this.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            this.BindGrid();
        }


    }
}