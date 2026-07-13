using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using System.Collections;

namespace ZeusWeb.SecureFirstTeamForms
{
    public partial class frmFTDashboard : frmBaseDataEntry
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            // Set Property to Store ViewState on Server
            base.StoreViewStateOnServer = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
            if (!Page.IsPostBack)
            {
                this.FormShow("");
            }

            UpdatePanel1.PreRender += new EventHandler(UpdatePanel1_PreRender);


            wucFTGridManager1.nameclick += new UserControls.wucFTGridManager.MyEventHandler(wucFTGridManager1_nameclick);

            wucFTGridManager1.ticketclick += new UserControls.wucFTGridManager.MyTicketHandler(wucFTGridManager1_ticketclick);

            wucFTGridRep1.ticketclick += new UserControls.wucFTGridRep.MyTicketHandler(wucFTGridRep1_ticketclick);
        }

        protected void UpdatePanel1_PreRender(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>force_min();</script>", false);
        }

        public override void FormShow(string ID)
        {
            LookupTableHandler.LoadUsersByRole(ddlReps, true, Constants.ROLE_FIRSTTEAM);
            this.ddlReps_SelectedIndexChanged(ddlReps, null);
        }

        protected void wucFTGridRep1_ticketclick(object sender, eTicketType eT, string user_uid, string merchantappuid, string businessdbaname)
        {
            ddlReps.SelectedValue = user_uid;

            //this.setup_tab_1_rep_specific(user_uid, eT, merchantappuid);

            this.setup_tab_5_rep_merchant(user_uid, eT, merchantappuid, businessdbaname);
            WebTab1.SelectedIndex = 4;


        }




        protected void wucFTGridManager1_ticketclick(object sender, eTicketType eT, string user_uid, string fullname)
        {
            ddlReps.SelectedValue = user_uid;

            this.ddlReps_SelectedIndexChanged(ddlReps, null, Convert.ToString( (int)eT));
        }

        protected void wucFTGridManager1_nameclick(object sender, string user_uid, string fullname)
        {
            ddlReps.SelectedValue = user_uid;
            this.ddlReps_SelectedIndexChanged(ddlReps, null);

        }


   


        protected void ddlReps_SelectedIndexChanged(object sender, EventArgs e, string tt)
        {
            DropDownList ddl = (DropDownList)sender;

            string user_uid = ddl.SelectedValue;

            if (CommonUtility.Util.IsValidGuid(user_uid))
            {

                // a FT rep was selected.

                // handle the top. show his merchants in the first pane.
                MultiView1.ActiveViewIndex = 2;
                wucFTGridRep1.LoadRepView(user_uid);

                // handle the bottom.
                WebTab1.SelectedIndex = 0;
                this.setup_tab_1_rep_sla(user_uid, ddl.SelectedItem.Text);
                this.setup_tab_2_rep_alerts(user_uid, ddl.SelectedItem.Text);
                this.setup_tab_3_rep_tasks(user_uid, ddl.SelectedItem.Text);
                this.setup_tab_4_rep_tickets(user_uid, ddl.SelectedItem.Text);
                this.setup_tab_5_remove();

                if (!string.IsNullOrEmpty(tt))
                {
                    int ttint = Convert.ToInt32(tt);
                    if (ttint == (int)eTicketType.Alert)
                    {
                        WebTab1.SelectedIndex = 1;
                    }
                    else if (ttint == (int)eTicketType.Task)
                    {
                        WebTab1.SelectedIndex = 2;
                    }
                    else if (ttint == (int)eTicketType.Ticket)
                    {
                        WebTab1.SelectedIndex = 3;
                    }
                }
            }
            else
            {
                // all was selected

                // handle the top
                MultiView1.ActiveViewIndex = 1;
                wucFTGridManager1.LoadManagerView();

                // handle the bottom
                WebTab1.SelectedIndex = 0;
                this.setup_tab_1_all_sla();
                this.setup_tab_2_all_alerts();
                this.setup_tab_3_all_tasks();
                this.setup_tab_4_all_tickets();
                this.setup_tab_5_all_unassigned();

            }
        }
        protected void ddlReps_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddlReps_SelectedIndexChanged(sender, e, "");


        }

        public void setup_tab_1_all_sla()
        {
            Hashtable prms = new Hashtable();
            prms["@UserRoleUID"] = Constants.ROLE_FIRSTTEAM;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@IsPastDueDate"] = 1;
            wucFTGridTicket1.SetDataSource(prms);

            //WebTab1.Tabs[1].Text = string.Format("Tickets out of SLA ({0})", wucFTGridTicket2.TotalRowCount.ToString());
            WebTab1.Tabs[0].Text = "Tickets out of SLA";
        }

        public void setup_tab_2_all_alerts()
        {
            Hashtable prms = new Hashtable();
            prms["@UserRoleUID"] = Constants.ROLE_FIRSTTEAM;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Alert;

            wucFTGridTicket2.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets ({0})", wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[1].Text = "All Alerts";
        }

        public void setup_tab_3_all_tasks()
        {
            Hashtable prms = new Hashtable();
            prms["@UserRoleUID"] = Constants.ROLE_FIRSTTEAM;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Task;

            wucFTGridTicket3.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets ({0})", wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[2].Text = "All Tasks";
        }

        public void setup_tab_4_all_tickets()
        {
            Hashtable prms = new Hashtable();
            prms["@UserRoleUID"] = Constants.ROLE_FIRSTTEAM;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Ticket;

            wucFTGridTicket4.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets ({0})", wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[3].Text = "All Tickets";
        }

        public void setup_tab_5_all_unassigned()
        {

            //WebTab1.Tabs[4].Enabled = true;
            //WebTab1.Tabs[4].Hidden = false;

            Hashtable prms = new Hashtable();
            prms["@StatusUIDList"] = string.Format("{0}", Ticket.TICKET_OPEN);
            prms["@DepartmentID"] = 12; // ticket department id for FIRST TEAM
            wucFTGridTicket5.SetDataSource(prms);

            //WebTab1.Tabs[2].Text = string.Format("Unassigned for FT ({0})", wucFTGridTicket3.TotalRowCount.ToString());
            WebTab1.Tabs[4].Text = "Unassigned for PS";
        }


        public void setup_tab_1_rep_sla(string user_uid, string fullname)
        {
            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;
            prms["@IsPastDueDate"] = 1;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);

            wucFTGridTicket1.SetDataSource(prms);

            //WebTab1.Tabs[1].Text = string.Format("Tickets out of SLA for {0} ({1})", fullname, wucFTGridTicket2.TotalRowCount.ToString());
            WebTab1.Tabs[0].Text = string.Format("Tickets out of SLA for {0}", fullname);
        }

        public void setup_tab_2_rep_alerts(string user_uid, string fullname)
        {
            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Alert;

            wucFTGridTicket2.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets for {0} ({1})", fullname, wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[1].Text = "Alerts";


        }

        public void setup_tab_3_rep_tasks(string user_uid, string fullname)
        {
            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Task;

            wucFTGridTicket3.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets for {0} ({1})", fullname, wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[2].Text = "Tasks";
        }

        public void setup_tab_4_rep_tickets(string user_uid, string fullname)
        {
            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);
            prms["@TicketType"] = (int)eTicketType.Ticket;

            wucFTGridTicket4.SetDataSource(prms);

            //WebTab1.Tabs[0].Text = string.Format("All tickets for {0} ({1})", fullname, wucFTGridTicket1.TotalRowCount.ToString());
            WebTab1.Tabs[3].Text = "Tickets";
        }

        public void setup_tab_5_rep_merchant(string user_uid, eTicketType eT, string merchantapp_uid, string businessdbaname)
        {


            WebTab1.Tabs[4].Enabled = true;
            WebTab1.Tabs[4].Hidden = false;

            WebTab1.SelectedIndex = 0;

            Hashtable prms = new Hashtable();
            prms["@UserUID"] = user_uid;
            prms["@TicketType"] = (int)eT;
            prms["@MerchantAppUID"] = merchantapp_uid;
            prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);

            wucFTGridTicket5.SetDataSource(prms);


            switch (eT)
            {
                case eTicketType.Ticket:
                    //WebTab1.Tabs[4].Text = string.Format("Tickets ({0})", wucFTGridTicket1.TotalRowCount.ToString());
                    WebTab1.Tabs[4].Text = "Tickets for " + businessdbaname;
                    break;
                case eTicketType.Alert:
                    //WebTab1.Tabs[4].Text = string.Format("Alerts ({0})", wucFTGridTicket1.TotalRowCount.ToString());
                    WebTab1.Tabs[4].Text = "Alerts for " + businessdbaname;
                    break;
                case eTicketType.Task:
                    //WebTab1.Tabs[4].Text = string.Format("Tasks ({0})", wucFTGridTicket1.TotalRowCount.ToString());
                    WebTab1.Tabs[4].Text = "Tasks for " + businessdbaname;
                    break;
            }
        }

        //public void setup_tab_1_rep_specific(string user_uid, eTicketType eT, string merchantappuid)
        //{
        //    WebTab1.SelectedIndex = 0;



        //    Hashtable prms = new Hashtable();
        //    prms["@UserUID"] = user_uid;
        //    prms["@TicketType"] = (int)eT;
        //    prms["@MerchantAppUID"] = merchantappuid;
        //    prms["@StatusUIDList"] = string.Format("{0},{1},{2}", Ticket.TICKET_ASSIGNED, Ticket.TICKET_OPEN, Ticket.TICKET_PENDING);

        //    wucFTGridTicket1.SetDataSource(prms);


        //    switch (eT)
        //    {
        //        case eTicketType.Ticket:
        //            //WebTab1.Tabs[0].Text = string.Format("Tickets ({0})", wucFTGridTicket1.TotalRowCount.ToString());
        //            WebTab1.Tabs[0].Text = "Tickets";
        //            break;
        //        case eTicketType.Alert:
        //            //WebTab1.Tabs[0].Text = string.Format("Alerts ({0})", wucFTGridTicket1.TotalRowCount.ToString());
        //            WebTab1.Tabs[0].Text = "Alerts";
        //            break;
        //        case eTicketType.Task:
        //            //WebTab1.Tabs[0].Text = string.Format("Tasks ({0})", wucFTGridTicket1.TotalRowCount.ToString());
        //            WebTab1.Tabs[0].Text = "Tasks";
        //            break;
        //    }
        //}

        public void setup_tab_5_remove()
        {
            WebTab1.SelectedIndex = 0;
            WebTab1.Tabs[4].Enabled = false;
            WebTab1.Tabs[4].Hidden = true;

        }

        public override void FormClear()
        {
            throw new NotImplementedException();
        }

        public override bool FormSave()
        {
            throw new NotImplementedException();
        }

        public override void FormNew()
        {
            throw new NotImplementedException();
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            throw new NotImplementedException();
        }

        public override void FormCancel()
        {
            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }
    }
}