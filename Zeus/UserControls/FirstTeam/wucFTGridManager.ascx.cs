using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucFTGridManager : System.Web.UI.UserControl
    {
        public delegate void MyEventHandler(object sender, string user_uid, string fullname);
        public event MyEventHandler nameclick;

        public delegate void MyTicketHandler(object sender, PaymentXP.BusinessObjects.eTicketType eT, string user_uid, string fullname);
        public event MyTicketHandler ticketclick;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadManagerView();

        }

        protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string user_uid = "";

            // for the given linkbutton that was clicked, track down the gridview and the gridviewrow that it was clicked from!
            GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

            LinkButton lbRep = (LinkButton)gvr.FindControl("lnkRepID");

            string full_name = lbRep.Text;

            switch (e.CommandName)
            {
                case "caAlerts":

                    user_uid = e.CommandArgument.ToString();

                    if (this.ticketclick != null)
                    {
                        this.ticketclick(sender, eTicketType.Alert, user_uid, full_name);
                    }
                    
                    break;

                case "caTasks":

                    user_uid = e.CommandArgument.ToString();

                    if (this.ticketclick != null)
                    {
                        this.ticketclick(sender, eTicketType.Task, user_uid, full_name);
                    }


                    break;

                case "caTickets":

                    user_uid = e.CommandArgument.ToString();

                    if (this.ticketclick != null)
                    {
                        this.ticketclick(sender, eTicketType.Ticket, user_uid, full_name);
                    }
                   
                    break;
            }
        }

        public void LoadManagerView()
        {
            DataTicket data = DataAccess.DataTicketDao;

            Hashtable prms = new Hashtable();


            prms.Add("@PageSize", 999);
            prms.Add("@CurrentPage", 1);
            prms.Add("@SortOrder", null);
            prms.Add("@SortDirection", 1);


            grd.DataSource = data.GetFTManagerView(prms);
            grd.DataBind();
        }

        protected void cboPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.grd.PageSize = Convert.ToInt32(cboPageSize.SelectedItem.Value);
            LoadManagerView();
        }

        protected void grdTick_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    LinkButton lbRep = (LinkButton)e.Row.FindControl("lnkRepID");
                    string repFirstName = CommonUtility.Util.if_s(DataBinder.Eval(e.Row.DataItem, "FirstName"));
                    string repLastName = CommonUtility.Util.if_s(DataBinder.Eval(e.Row.DataItem, "LastName"));
                    lbRep.Text = string.Format("{0} {1}", repFirstName, repLastName);

                    string user_uid = lbRep.CommandArgument;

                    Dictionary<string, string> di = new Dictionary<string, string>();

                    //di.Clear();
                    //LinkButton lbAlerts = (LinkButton)e.Row.FindControl("lblAlerts");
                    //di.Add("UserUID", user_uid);
                    //di.Add("TicketType", lbAlerts.CommandArgument);
                    //lbAlerts.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");

                    //di.Clear();
                    //LinkButton lbTasks = (LinkButton)e.Row.FindControl("lblTasks");
                    //di.Add("UserUID", user_uid);
                    //di.Add("TicketType", lbTasks.CommandArgument);
                    //lbTasks.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");

                    //di.Clear();
                    //LinkButton lbTickets = (LinkButton)e.Row.FindControl("lblTickets");
                    //di.Add("UserUID", user_uid);
                    //di.Add("TicketType", lbTickets.CommandArgument);
                    //lbTickets.Attributes.Add("onclick", "javascript:return OpenTicket('" + CommonUtility.Util.DictToUrl(di, false) + "')");

                    break;

                case DataControlRowType.Footer:
                    break;
                default:
                    break;
            }
        }

        protected void lnkRepID_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            string agent_uid = lb.CommandArgument;

            LinkButton lbFN = (LinkButton)lb.FindControl("lnkRepID");

            string fullname = lbFN.Text;

            if (this.nameclick != null)
            {
                this.nameclick(sender, agent_uid, fullname);
            }
        }



    }
}