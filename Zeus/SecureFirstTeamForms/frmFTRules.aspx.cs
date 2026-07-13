using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects;
using System.Collections;

namespace ZeusWeb.SecureFirstTeamForms
{
    public partial class frmFTRules : frmBaseDataEntry
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
            if (!Page.IsPostBack)
            {
                this.FormShow("");
            }
        }


      

        public override void FormShow(string ID)
        {
            this.Page.Init += new EventHandler(Page_Init);

            wucMRule1.RoleUID = Constants.ROLE_FIRSTTEAM;
            LookupTableHandler.LoadUsersByRole(ddlReps, true, Constants.ROLE_FIRSTTEAM);
            LookupTableHandler.LoadFirstTeamRules(ddlRuleName, true);
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            wucMRule1.GlobalEnabledClick += new UserControls.wucMRule.GlobalEnabledClickHandler(wucMRule1_GlobalEnabledClick);

            wucMRule1.RuleNameClick += new UserControls.wucMRule.RuleNameClickHandler(wucMRule1_RuleNameClick);
        }

        protected void wucMRule1_RuleNameClick(object sender, GridViewCommandEventArgs e)
        {
            string rule_name = e.CommandName;
            int rule_id = CommonUtility.Util.if_i(e.CommandArgument, 0);

            switch (CommonUtility.Util.if_s(e.CommandName).ToLower())
            {
                case "rulename":

                    if (rule_id > 0)
                    {

                        ddlRuleName.SelectedValue = rule_id.ToString();

                        this.btnSearch_Click(null, null);
                    }
                    break;
            }
        }

        protected void wucMRule1_GlobalEnabledClick(object sender, bool e)
        {
            wucFTRuleFilter1.RefreshGrid();
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            wucFTRuleFilter1.Visible = true;
            Hashtable prms = new Hashtable();

            if (ddlMerchantEnabled.SelectedValue != "-1")
            {
                prms.Add("@MerchantRuleEnabled", ddlMerchantEnabled.SelectedValue);
            }

            if (ddlRuleName.SelectedValue!= "-1")
            {
                prms.Add("@MRuleID", ddlRuleName.SelectedValue);
            }

            //if (SearchBeginDate.Value != null &&  Convert.ToDateTime(SearchBeginDate.Value).Year != DateTime.MinValue.Year)
            //{
            //    prms.Add("@FromDate", SearchBeginDate.Value);
            //}

            //if (SearchEndDate.Value != null && Convert.ToDateTime(SearchEndDate.Value).Year != DateTime.MinValue.Year)
            //{
            //    prms.Add("@EndDate", SearchEndDate.Value);
            //}

            if (ddlReps.SelectedValue != "-1" && CommonUtility.Util.IsValidGuid(ddlReps.SelectedValue))
            {
                prms.Add("@FTRepUserUID", ddlReps.SelectedValue);
            }

            if (tbBusinessDBAName.Text.Trim() != "")
            {
                prms.Add("@BusinessDBAName", tbBusinessDBAName.Text.Trim());
            }

            if (tbBusinessLegalName.Text.Trim() != "")
            {
                prms.Add("@BusinessLegalName", tbBusinessLegalName.Text.Trim());
            }

            if (tbZID.Text.Trim() != "" && CommonUtility.Util.IsValidInt32(tbZID.Text))
            {
                prms.Add("@MerchantID", tbZID.Text);
            }

            wucFTRuleFilter1.SetDataSource(prms);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

            ddlMerchantEnabled.SelectedValue = "-1";
            ddlRuleName.SelectedValue = "-1";
            tbZID.Text = "";
            tbBusinessDBAName.Text = "";
            ddlReps.SelectedValue = "-1";
            //SearchBeginDate.Value = null;
            //SearchEndDate.Value = null;

            wucFTRuleFilter1.SetDataSource(new Hashtable());
        }
    }
}