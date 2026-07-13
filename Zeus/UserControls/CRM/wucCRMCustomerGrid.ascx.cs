using PaymentXP.BusinessObjects.Zeus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ZeusWeb.UserControls.CRM
{
    public partial class wucCRMCustomerGrid : wucBaseSearch
    {

        public List<ConsumerAction> ConsumerActions
        {
            get { return (List<ConsumerAction>)(this.ViewState["ConsumerActions"] ?? new List<NotificationHistory>()); }
            set { this.ViewState["ConsumerActions"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindGrid()
        {
            grdConsumerAction.DataSource = ConsumerActions;
            grdConsumerAction.DataBind();
        }       
    }
}