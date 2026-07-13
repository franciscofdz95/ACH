using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

namespace ZeusWeb.SecureFirstTeamForms
{
    public partial class frmFTHistory : frmBaseSearch
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

            // history grid
            wucFTGridHistory1.Visible = true;
            Hashtable prmsHistory = new Hashtable();
            //prmsHistory.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
            wucFTGridHistory1.SetDataSource(prmsHistory);
            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            wucFTGridHistory1.Visible = true;
            Hashtable prmsLog = new Hashtable();


                if (Convert.ToDateTime(SearchBeginDate.Value).Year != DateTime.MinValue.Year)
                {

                    prmsLog.Add("@StartDate", SearchBeginDate.Value);
                }

                if (Convert.ToDateTime(SearchEndDate.Value).Year != DateTime.MinValue.Year)
                {

                    prmsLog.Add("@EndDate", SearchEndDate.Value);
                }

            



            wucFTGridHistory1.SetDataSource(prmsLog);
        }

        

    }
}