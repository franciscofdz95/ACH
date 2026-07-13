using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucLeadAppointments : wucBaseSearch
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                RefreshGrid();
            }
        }

        public void RefreshGrid()
        {
            grdAppointments.DataSourceID = "odsGetUserLeadAppointments";

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
            grdAppointments.DataSourceID = "odsGetUserLeadAppointments";
            this.CurrentPage = 1;
            grdAppointments.PageIndex = 0;
            grdAppointments.PageSize = this.PageSize;
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

            if (!m_Prms.ContainsKey("@UserName"))
                m_Prms.Add("@UserName", UserSessions.CurrentUser.UserName);
            else
                m_Prms["@UserName"] = UserSessions.CurrentUser.UserName;

            if (!m_Prms.ContainsKey("@Active"))
                m_Prms.Add("@Active", 1);
            else
                m_Prms["@Active"] = 1;

            int rowcount =  DataMerchantAppPaging.GetUserLeadAppointmentsCount(m_Prms, this.PageSize, this.CurrentPage);

            lblRecordCount.Text = "Total Records Found: " + rowcount.ToString();

            grdAppointments.PageSize = this.PageSize;
            grdAppointments.DataBind();

            pnlRecords.Visible = grdAppointments.Rows.Count != 0;
            pnlNoRecords.Visible = grdAppointments.Rows.Count == 0;
        }

        public int CurrentPage
        {
            get
            {
                if (ViewState[this.ClientID + "_CurrentPage"] == null)
                    return 1;
                else
                    return Convert.ToInt32(ViewState[this.ClientID + "_CurrentPage"]);
            }
            set { ViewState[this.ClientID + "_CurrentPage"] = value; }
        }

        public int PageSize
        {
            get
            {

                if (ViewState[this.ClientID + "_PageSize"] == null)
                    return 10;
                else
                    return Convert.ToInt32(ViewState[this.ClientID + "_PageSize"]);
            }
            set { ViewState[this.ClientID + "_PageSize"] = value; }
        }

        public Hashtable m_Prms
        {
            get
            {
                if (ViewState["m_Prms"] == null)
                    return null;
                else
                    return (Hashtable)ViewState["m_Prms"];
            }
            set { ViewState["m_Prms"] = value; }
        }

        protected void grdAppointments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                     if (e.Row.Cells[5].Text.Equals("&nbsp;"))
                        e.Row.Cells[5].ToolTip = "";
                    else
                        e.Row.Cells[5].ToolTip = e.Row.Cells[5].Text;
                    e.Row.Cells[5].Text = "<div style='width:395px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;'>" + e.Row.Cells[5].Text + "</div>";

                    LinkButton btn = (LinkButton)e.Row.FindControl("lbtnLeadID");
                    btn.Text = DataBinder.Eval(e.Row.DataItem, "LeadID").ToString();
                    btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "LeadID").ToString();

                    e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);
                    e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text);

                    break;
                default:
                    break;
            }
        }

        protected void grdAppointments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string url = string.Empty;
            GridViewRow grdRow = null;

            if (e.CommandSource is LinkButton)
                grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            else if (e.CommandSource is ImageButton)
                grdRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            else
                return;

            switch (e.CommandName)
            {
                case "ID":
                    url = "frmLeadsDetail.aspx?Adding=false";
                    url += "&PostBackURL=~/SecureLeadForms/frmLeads.aspx";
                    url += "&LeadID=" + grdAppointments.DataKeys[grdRow.RowIndex].Values["LeadID"].ToString();
                    url += "&LeadUID=" + grdAppointments.DataKeys[grdRow.RowIndex].Values["LeadUID"].ToString();
                    Response.Redirect(url);
                    break;

                case "Active":
                    string AppointmentUID = grdAppointments.DataKeys[grdRow.RowIndex].Values["AppointmentID"].ToString();
                    DataAppointment data = DataAccess.DataAppointmentDao;
                    Appointment appntmnt = data.GetAppointment(AppointmentUID);

                    appntmnt.Active = false;
                    appntmnt.UserUpdated = UserSessions.CurrentUser.UserName;
                    data.UpdateAppointment(appntmnt);

                    this.BindGrid();
                    break;
            }
        }

        protected void grdAppointments_PageIndexChanging(object sender, GridViewPageEventArgs e)
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