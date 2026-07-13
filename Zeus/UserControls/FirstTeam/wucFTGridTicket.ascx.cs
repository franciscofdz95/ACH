using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Web.UI.HtmlControls;

namespace ZeusWeb.UserControls
{
    public partial class wucFTGridTicket : wucBaseSearch
    {
        

        public void SetDataSource()
        {
            this.SetDataSource(this.m_Prms);
        }

        public void SetDataSource(Hashtable prms)
        {
            grdTickets.DataSourceID = "ods";
            grdTickets.PageIndex = this.CurrentPage - 1;
            grdTickets.PageSize = this.PageSize;
            this.m_Prms = prms;
        }

        protected void grdTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //string url = string.Empty;



            //switch (e.CommandName)
            //{
            //    case "View":

            //        LinkButton btn = (LinkButton)e.CommandSource;

            //        if (btn.Text == "0")
            //            return;
            //        else
            //        {
            //            //if (this.PerformRedirect)
            //            //{
            //            //    Hashtable prms = new Hashtable();
            //            //    prms.Add("@UID", e.CommandArgument.ToString());
            //            //    Ticket ticket = DataAccess.DataTicketDao.GetTicket(prms);
            //            //    UserSessions.CurrentTicket = ticket;

            //            //    url = "~/SecureTicketForms/frmTicketPopup.aspx?Adding=false";
            //            //    url += "&PostBackURL=~/SecureHomeForms/frmHome.aspx";
            //            //    url += "&TicketUID=" + ticket.TicketUID.Trim();

            //            //    Response.Redirect(url);
            //            //}
            //        }

            //        break;

            //    case "BusinessDBAName":

            //        string str_merchant_uid = Convert.ToString(e.CommandArgument);

            //        UserSessions.CurrentMerchantApp = DataAccess.DataMerchantAppDao.GetMerchantApp(str_merchant_uid);

            //        Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx");
            //        break;
            //}
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
            this.SetDataSource(this.m_Prms);
        }

        protected void grdTickets_DataBound(object sender, EventArgs e)
        {

        }

        protected void cboPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(cboPageSize2.SelectedItem.Value);
            this.m_Prms["@PageSize"] = this.PageSize;
            this.SetDataSource(this.m_Prms);
        }

        protected void grdTickets_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.Header:

                   
                    break;

                case DataControlRowType.DataRow:

                    // ticket button
                    LinkButton btn = (LinkButton)e.Row.FindControl("lbtnTicketID");
                    btn.Text = DataBinder.Eval(e.Row.DataItem, "TicketID").ToString();
                    btn.CommandArgument = DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString();
                    btn.Attributes.Add("onclick", "javascript:return OpenTicket('" + DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString() + "');");

                 
                    break;

                case DataControlRowType.Footer:
                    break;

                default:
                    break;
            }
        }

        protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //hash table is use to store parameters which will be passed to the stored procedure

            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
            }

            if (this.m_Prms.Count > 0)
            {
                if (!m_Prms.ContainsKey("@PageSize"))
                    m_Prms.Add("@PageSize", "10");
                else
                    m_Prms["@PageSize"] = this.PageSize;


                if (!m_Prms.ContainsKey("@CurrentPage"))
                    m_Prms.Add("@CurrentPage", "1");
                else
                    m_Prms["@CurrentPage"] = this.CurrentPage;


                if (!m_Prms.ContainsKey("@SortOrder"))
                    m_Prms.Add("@SortOrder", "TicketID");
                else
                    m_Prms["@SortOrder"] = this.SortOrder;


                m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);


                lblRecordCount.Text = DataMerchantAppPaging.GetTicketsPagingCount(m_Prms, this.PageSize, this.CurrentPage, "my control").ToString();
                
                e.InputParameters[0] = this.m_Prms;

            }
        }

        protected void grdTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.SetDataSource(this.m_Prms);
        }

    }
}
