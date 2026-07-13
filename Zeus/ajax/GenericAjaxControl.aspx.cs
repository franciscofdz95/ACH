using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

namespace ZeusWeb.ajax
{
    public partial class GenericAjaxControl : System.Web.UI.Page
    {
        //max json length for serializing data (64MB)
        private const int MAX_JSON_LEN = 67108864;

        protected void Page_Load(object sender, EventArgs e)
        {
            string response = "";

            try
            {
                string command = Request["command"].ToString();

                switch (command)
                {
                    case "GetTicketUID":
                        this.GetTicketUID();

                        break;

                    default:
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException)
            {
                //suppress ThreadAbortException because it will be raised for export to excel features where
                //response.end gets called. we suppress the exception because it's really not an error
                //and is expected to get thrown when Response.End gets called
            }
            catch (Exception ex)
            {
                response = @"{""error"": ""Your request cannot be executed: A server error has occurred, please contact IT.""}";

                Response.ContentType = "application/json";
                Response.Write(response);
            }

            Response.End();
        }

        //This method is to fetch the UID for Quick Ticket Search. We need to UID as our 
        //entire ticket logic and sessions is driven by TicketUID
        public void GetTicketUID()
        {
            StringBuilder sb = new StringBuilder();
            DataTicket data = DataAccess.DataTicketDao;
            Ticket ticket = new Ticket();
            Hashtable prms = new Hashtable();
            string ticketid = Request["TicketID"].ToString();
            Regex rgx = new Regex("[^0-9]");
            ticketid = rgx.Replace(ticketid, "");

            if (!string.IsNullOrEmpty(ticketid.Trim()))
            {
                prms.Add("@TicketID", ticketid.Trim());
                ticket = data.GetTicket(prms, UserSessions.CurrentUser.TimeZone);
                if (ticket != null)
                {
                    sb.AppendFormat(@"{{""response"":""{0}""}}", ticket.TicketUID);
                }
                else
                {
                    sb.AppendFormat(@"{{""response"":""No Ticket UID""}}");
                }
            }
            else
            {
                sb.AppendFormat(@"{{""response"":""InvalidTicketID""}}");
            }

            Response.ContentType = "application/json";
            Response.Write(sb.ToString());

        }
    }
}