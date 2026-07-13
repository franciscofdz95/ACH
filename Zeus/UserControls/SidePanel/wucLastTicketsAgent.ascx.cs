using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace ZeusWeb.UserControls.SidePanel
{
    public partial class wucLastTicketsAgent : wucBaseSearch
    {
        
        private void LoadTickets()
        {
            
            grd.DataBind();

            grd.Sort("DateCreated", SortDirection.Descending);
            pnlNoRecords.Visible = !(grd.Rows.Count > 0);
            pnlRecords.Visible = grd.Rows.Count > 0;

            if (UserSessions.CurrentAgent != null)
            {
                btnTicket.Enabled = true;
                btnTicket.Attributes.Add("onclick", string.Format("NewTicketAgent('{0}')", UserSessions.CurrentAgent.AgentUID));
                this.Visible = true;
            }
            else
            {
                btnTicket.Enabled = false;
                this.Visible = false;
            }

        }

        protected void odsTickets_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {

            if (UserSessions.CurrentAgent != null)
            {
                Hashtable prms = new Hashtable();

                prms.Add("@PageSize", 4);
                prms.Add("@CurrentPage", 1);
                prms.Add("@SortOrder", "TicketID");
                prms.Add("@SortDirection", "1");
                prms.Add("@AgentUID", UserSessions.CurrentAgent.AgentUID);
                e.InputParameters[0] = prms;
                e.InputParameters[3] = this.grd.ID;
            }
            else
            {
                e.Cancel = true;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.FormShow();
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

        private void clearAll()
        {

            pnlNoRecords.Visible = true;
            pnlRecords.Visible = false;

            //btnTicket.Enabled = false;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTickets();
            //pnl.Update();
        }

        public void FormShow()
        {
            if (UserSessions.CurrentAgent != null && CommonUtility.Util.IsValidGuid(UserSessions.CurrentAgent.AgentUID))
            {
                hypMore.NavigateUrl = "~/SecureTicketForms/frmTicketSearch.aspx?AgentUID=" + UserSessions.CurrentAgent.AgentUID;
            }

            this.LoadTickets();
           
        }
    }
}