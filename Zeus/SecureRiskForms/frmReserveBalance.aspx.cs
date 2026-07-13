using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace ZeusWeb
{
    public partial class frmReserveBalance : frmBaseSearch
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
            if (!Page.IsPostBack)
            {
                this.AsOfDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                Hashtable prm = new Hashtable();
                prm.Add("@LastDate", this.AsOfDate.Text);
                wucSummaryGrid1.SetDataSource(prm);
            }
        }

        protected void cbRHAM_CheckedChanged(object sender, EventArgs e)
        {
            ApplySearch();
        }

        protected void ApplyAsOfDate_Click(object sender, EventArgs e)
        {
            ApplySearch();
        }

        private void ApplySearch()
        {
            wucSummaryGrid1.IncludeReservesHeldAtMeritus = cbRHAM.Checked;

            Hashtable prm = new Hashtable();
            DateTime res;

            //if an invalid date is provided then provide today's as the default
            if(DateTime.TryParse(this.AsOfDate.Text, out res))
                prm.Add("@LastDate", this.AsOfDate.Text);
            else
                prm.Add("@LastDate", DateTime.Now.ToString("MM/dd/yyyy"));

            wucSummaryGrid1.SetDataSource(prm);
        }
    }
}