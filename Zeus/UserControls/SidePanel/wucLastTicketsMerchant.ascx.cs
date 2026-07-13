using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.Facade;
using System.Data;

namespace ZeusWeb.UserControls
{
    public partial class wucLastTicketsMerchant : wucBaseSearch
    {


        public string VSStatus
        {
            get { return (string)ViewState["VSStatus"]; }
            set { ViewState["VSStatus"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FormShow();
            }
        }

        public void FormShow()
        {

            if (UserSessions.CurrentMerchantApp != null)
            {
                hypMore.NavigateUrl = "~/SecureTicketForms/frmTicketSearch.aspx?ZID=" + UserSessions.CurrentMerchantApp.ID;

                btnTicket.Enabled = true;
                btnTicket.Attributes.Add("onclick", string.Format("NewTicketMerchant('{0}')", UserSessions.CurrentMerchantApp.MerchantAppUID));

                this.LoadTickets();
            }


        }

        private void LoadTickets()
        {
            //Load Tickets

            if (UserSessions.CurrentMerchantApp != null)
            {
                if (this.m_Prms == null)
                {
                    this.m_Prms = new Hashtable();
                }
                this.m_Prms["@PageSize"] = 4;
                this.m_Prms["@CurrentPage"] = this.CurrentPage;
                this.m_Prms["@SortOrder"] = "TicketID";
                this.m_Prms["@SortDirection"] = "1";
                this.m_Prms["@MerchantAppUID"] = UserSessions.CurrentMerchantApp.MerchantAppUID;

                grd.DataBind();
                //grd.Sort("DateCreated", SortDirection.Descending);
            }

            hypMore.Visible = grd.Rows.Count > 0;

        }



        protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

            if (UserSessions.CurrentMerchantApp != null)
            {
                //this.m_Prms.Add("@MerchantAppUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
                e.InputParameters[0] = this.m_Prms;
                e.InputParameters[3] = this.grd.ID;
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicket");
                    btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();

                    btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");
                    break;
            }
        }



        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.m_Prms = null;
            this.CurrentPage = 1;

            LoadTickets();
            pnl.Update();
        }


        protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            LoadTickets();
        }

        protected void ddlHeaderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;

            this.VSStatus = ddl.SelectedValue;

            if (ddl.SelectedValue == "")
            {
                if (this.m_Prms.ContainsKey("@StatusUID"))
                {
                    this.m_Prms.Remove("@StatusUID");
                    this.LoadTickets();
                }
            }
            else if (CommonUtility.Util.IsValidGuid(ddl.SelectedValue))
            {
                this.m_Prms["@StatusUID"] = ddl.SelectedValue;
                this.LoadTickets();
            }



        }

        protected void pnl_PreRender(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)grd.HeaderRow.FindControl("ddlHeaderStatus");
            if (this.VSStatus != null)
            {
                ddl.SelectedValue = this.VSStatus;
            }
        }


    }
}