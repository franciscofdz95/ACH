using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.SecureFirstTeamForms
{
    public partial class frmFTChangelog : frmBaseSearch
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));

            if (!Page.IsPostBack)
            {
                this.FormShow();
            }

        }

        public void FormShow()
        {
            SearchBeginDate.Value = DateTime.Now.AddDays(-14);
            SearchEndDate.Value = DateTime.Now;

            wucFTGridChangelog1.Visible = true;
            Hashtable prmsLog = new Hashtable();
            prmsLog.Add("@ObjectNameList", "MRuleParam,MRule");
            prmsLog.Add("@PortalUID", UserSessions.PortalUID);
            wucFTGridChangelog1.SetDataSource(prmsLog);

            LookupTableHandler.LoadUsersByRole(ddlReps, true, Constants.ROLE_FIRSTTEAM);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            wucFTGridChangelog1.Visible = true;
            Hashtable prmsLog = new Hashtable();

            prmsLog.Add("@ObjectNameList", "MRuleParam,MRule");
            prmsLog.Add("@PortalUID", UserSessions.PortalUID);

            if (Convert.ToDateTime(SearchBeginDate.Value).Year != DateTime.MinValue.Year)
            {
                prmsLog.Add("@FromDate", SearchBeginDate.Value);
            }

            if (Convert.ToDateTime(SearchEndDate.Value).Year != DateTime.MinValue.Year)
            {
                prmsLog.Add("@EndDate", SearchEndDate.Value);
            }

            if (ddlReps.SelectedValue != "-1" && CommonUtility.Util.IsValidGuid(ddlReps.SelectedValue))
            {
                prmsLog.Add("@UserUID", ddlReps.SelectedValue);
            }

            if (tbDBA.Text.Trim() != "")
            {
                prmsLog.Add("@BusinessDBAName", tbDBA.Text.Trim());
            }

            if (tbZID.Text.Trim() != "" && CommonUtility.Util.IsValidInt32(tbZID.Text))
            {
                prmsLog.Add("@MerchantID", tbZID.Text);
            }

            wucFTGridChangelog1.SetDataSource(prmsLog);
        }
    }
}