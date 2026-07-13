using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using System.Collections;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucFTGridRep : wucBaseSearch
    {
        public delegate void MyTicketHandler(object sender, PaymentXP.BusinessObjects.eTicketType eT, string user_uid, string merchantappuid, string businessdbaname);
        public event MyTicketHandler ticketclick;

       

        string _UserUID = null;

        public string UserUID
        {
            get { return _UserUID; }
            set { _UserUID = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SetDataSource();
            }
        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {




            switch (e.CommandName)
            {
                case "BusinessDBAName":

                    LinkButton btn = (LinkButton)e.CommandSource;

                    string str_merchant_uid = Convert.ToString(e.CommandArgument);

                    //UserSessions.CurrentMerchantApp = DataAccess.DataMerchantAppDao.GetMerchantApp(str_merchant_uid);

                    Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=False&MerchantAppUID=" + str_merchant_uid);

                    break;

                case "caAlerts":
                    {
                        // for the given linkbutton that was clicked, track down the gridview and the gridviewrow that it was clicked from!
                        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                        LinkButton lbRep = (LinkButton)gvr.FindControl("lnkRepID");

                        HiddenField hfMUID = (HiddenField)gvr.FindControl("hidMerchantUID");

                        string merchantapp_uid = hfMUID.Value;

                        HiddenField hfUserUID = (HiddenField)gvr.FindControl("hidUserUID");

                        Label lbDBA = (Label)gvr.FindControl("lblDBA");

                        string user_uid = hfUserUID.Value;

                        string full_name = (lbRep != null) ? lbRep.Text : "";
                        if (this.ticketclick != null)
                        {
                            this.ticketclick(sender, eTicketType.Alert, user_uid, merchantapp_uid, lbDBA.Text);
                        }

                        break;
                    }

                case "caTasks":
                    {
                        // for the given linkbutton that was clicked, track down the gridview and the gridviewrow that it was clicked from!
                        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                        LinkButton lbRep = (LinkButton)gvr.FindControl("lnkRepID");

                        HiddenField hfMUID = (HiddenField)gvr.FindControl("hidMerchantUID");

                        string merchantapp_uid = hfMUID.Value;

                        HiddenField hfUserUID = (HiddenField)gvr.FindControl("hidUserUID");

                        Label lbDBA = (Label)gvr.FindControl("lblDBA");

                        string user_uid = hfUserUID.Value;

                        string full_name = (lbRep != null) ? lbRep.Text : "";

                        if (this.ticketclick != null)
                        {
                            this.ticketclick(sender, eTicketType.Task, user_uid, merchantapp_uid, lbDBA.Text);
                        }

                        break;
                    }

                case "caTickets":
                    {
                        // for the given linkbutton that was clicked, track down the gridview and the gridviewrow that it was clicked from!
                        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                        LinkButton lbRep = (LinkButton)gvr.FindControl("lnkRepID");

                        HiddenField hfMUID = (HiddenField)gvr.FindControl("hidMerchantUID");

                        string merchantapp_uid = hfMUID.Value;

                        HiddenField hfUserUID = (HiddenField)gvr.FindControl("hidUserUID");

                        Label lbDBA = (Label)gvr.FindControl("lblDBA");

                        string user_uid = hfUserUID.Value;

                        string full_name = (lbRep != null) ? lbRep.Text : "";

                        if (this.ticketclick != null)
                        {
                            this.ticketclick(sender, eTicketType.Ticket, user_uid, merchantapp_uid, lbDBA.Text);
                        }

                        break;
                    }
            }

        }

        protected void grdTick_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:


                    HiddenField hfMUID = (HiddenField)(e.Row.FindControl("hidMerchantUID"));
                    string merchantuid = hfMUID.Value;

                    Dictionary<string, string> di = new Dictionary<string, string>();

                    LinkButton lbAlerts = (LinkButton)e.Row.FindControl("lblAlerts");
                    LinkButton lbTasks = (LinkButton)e.Row.FindControl("lblTasks");
                    LinkButton lbTickets = (LinkButton)e.Row.FindControl("lblTickets");

                    if (CommonUtility.Util.if_s(merchantuid) != "")
                    {
                        //di.Clear();
                        //di.Add("UserUID", this.UserUID);
                        //di.Add("TicketType", lbAlerts.CommandArgument);
                        //di.Add("MerchantUID", merchantuid);
                        //lbAlerts.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");

                        //di.Clear();
                        //di.Add("UserUID", this.UserUID);
                        //di.Add("TicketType", lbTasks.CommandArgument);
                        //di.Add("MerchantUID", merchantuid);
                        //lbTasks.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");

                        //di.Clear();
                        //di.Add("UserUID", this.UserUID);
                        //di.Add("TicketType", lbTickets.CommandArgument);
                        //di.Add("MerchantUID", merchantuid);
                        //lbTickets.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");
                    }
                    else
                    {
                        lbAlerts.Enabled = false;
                        lbTickets.Enabled = false;
                        lbTasks.Enabled = false;
                    }


                    Label lbIsFTM = (Label)e.Row.FindControl("lblIsFTMerchant");
                    lbIsFTM.Text = (CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsFTMerchant"), false)) ? "Yes" : "No";


                    string my_username = CommonUtility.Util.if_s(DataBinder.Eval(e.Row.DataItem, "Username"));
                    string rep_w_merch_ticket = CommonUtility.Util.if_s(DataBinder.Eval(e.Row.DataItem, "MerchantFTRepUsername"), my_username);

                    if (my_username != rep_w_merch_ticket)
                    {
                        lbIsFTM.Text += " (" + rep_w_merch_ticket + ")";
                    }


                    //this.TotalTicketCount = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalTicketCount"));
                    lblRecordCount.Text = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalRecordCount")).ToString();
                    lblTicketCount.Text = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TotalTicketCount")).ToString();

                    break;

                case DataControlRowType.Footer:
                    break;
                default:
                    break;
            }
        }


        public void SetDataSource()
        {
            this.SetDataSource(this.m_Prms);
        }

        public void SetDataSource(Hashtable prms)
        {
            grd.DataSourceID = "ods";
            grd.PageIndex = this.CurrentPage - 1;
            grd.PageSize = this.PageSize;

            this.m_Prms = prms;
        }

        protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            //hash table is use to store parameters which will be passed to the stored procedure

            if (this.m_Prms == null)
            {
                this.m_Prms = new Hashtable();
            }


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

            m_Prms["@OnlyShowIfHasTickets"] = 1;

            m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

            lblRecordCount.Text = DataMerchantAppPaging.GetFTRepViewPagingCount(m_Prms, this.PageSize, this.CurrentPage).ToString();

            e.InputParameters[0] = this.m_Prms;
        }

        protected void grdTickets_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.SetDataSource(this.m_Prms);
        }

        protected void grd_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;
            this.SetDataSource(this.m_Prms);

        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            this.CurrentPage = 1;
            this.SetDataSource();
        }

        public void LoadRepView(string user_uid)
        {
            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;

            this.SetDataSource(prms);
        }

    }
}