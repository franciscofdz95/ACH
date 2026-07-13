using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.FirstTeam
{
    public partial class wucFTAssignMerchants : wucBaseSearch
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LookupTableHandler.LoadUsersByRole(ddlReps, false, Constants.ROLE_FIRSTTEAM);
                this.RefreshGrid();
            }
            
        }

        /// <summary>
        /// to be called from parent controls to refresh the grid.
        /// </summary>
        public void RefreshGrid()
        {
            GridView1.DataSourceID = "odsPotentialFTMerchants";

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
            GridView1.DataSourceID = "odsPotentialFTMerchants";
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


            int rowcount = DataMerchantAppPaging.GetPotentialFTMerchantsCount(m_Prms, this.PageSize, this.CurrentPage);

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
                ((Label)e.Row.FindControl("lblOffice")).Text = ((CommonUtility.Util.Offices)CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "OfficeID").ToString(), -1)).ToString();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int i = 0; string LeadId = string.Empty;

            string Zids = string.Empty;

            bool isChecked = false;

            foreach (GridViewRow row in GridView1.Rows)
            {
                if (((CheckBox)row.FindControl("chkAssign")).Checked)
                {
                    isChecked = true;
                    break;
                }
            }

            if (!isChecked)
            {
                lblError1.Text = "Please select at least one Merchant to continue.";
                WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

            }
            else
            {
                if (ddlReps.SelectedValue == "-1" && ddlAction.SelectedValue == "Assign")
                {
                    lblError1.Text = "Please select a PS rep to Assign";
                    WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                }

                else
                {
                    for (i = 0; i < this.GridView1.Rows.Count; i++)
                    {
                        CheckBox chk = ((CheckBox)GridView1.Rows[i].Cells[0].FindControl("chkAssign"));

                        if (chk.Checked)
                        {
                            Zids += GridView1.DataKeys[i].Values["ZID"].ToString() + ",";
                        }
                    }

                    if (!string.IsNullOrEmpty(Zids))
                    {
                        DataMerchantApp.GetInstance().UpdateMerchantFTRepAssignment(this.ddlReps.SelectedValue, Zids, ddlAction.SelectedValue, UserSessions.CurrentUser.UserName);
                        RefreshGrid();
                        lblError1.Text = "Action Completed successfully";
                        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
                    }

                }
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        }

        protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlAction.SelectedValue == "Remove")
            {
                this.lblPSRep.Visible = false;
                this.ddlReps.Visible = false;
            }

            else
            {
                this.lblPSRep.Visible = true;
                this.ddlReps.Visible = true;
            }
        }
    }
}