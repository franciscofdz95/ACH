using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using PaymentXP.Facade;


    public partial class MasterPageCompliance : frmBaseMaster
    {
       
        public enum eMasterSideMenu : int
        {
            NotSet,
            SearchCRM,
            CRMDetails
        }

        public void SideMenuSelect(eMasterSideMenu eM)
        {

            switch (eM)
            {
                case eMasterSideMenu.SearchCRM:
                    wucTopMenu1.StatusBarText = "Search TPP"; //Replace CRM to TPP for PXP-8417 by Ali Khan
                    lnkSearchCRM.CssClass = "active";
                    break;

                case eMasterSideMenu.CRMDetails:
                    wucTopMenu1.StatusBarText = "TPP Details"; //Replace CRM to TPP for PXP-8417 by Ali Khan
                    lnkCRMDetails.CssClass = "active";
                    break;
            }

        }
        public void SetStatusBarText(string msg)
        {
            wucTopMenu1.StatusBarText = msg;
        }
        public int ErrorCount()
        {
            return this.WucMessage1.ErrorCount();
        }

        public void AddMessageError(string msg)
        {
            this.WucMessage1.AddMessageError(msg);
        }

        public void AddMessageStatus(string msg)
        {
            this.WucMessage1.AddMessageStatus(msg);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            
            if (UserSessions.CurrentUser == null)
                Response.Redirect("~/frmLogin.aspx");

            this.HandleSideMenu();
            if (!this.IsPostBack)
            {
                FormHandler.SetSecurity(this.Page.Master);
            }
        }
        protected void HandleSideMenu()
        {
            foreach (Control c in pnlSideMenu.Controls)
            {
                if (c is HyperLink)
                {
                    HyperLink h = (HyperLink)c;

                    // the search link should always be visible no matter what.
                    if (h.ID != "lnkSearchCRM")
                    {
                        if (UserSessions.CurrentCRM != null)
                        {
                            h.Enabled = true;
                            h.NavigateUrl = WebUtil.InjectParam(h.NavigateUrl, "CRMUID", Request.QueryString["CRMUID"]);
                        }
                        else
                        {
                            h.NavigateUrl = string.Empty;
                            h.CssClass = "disabledText";
                            h.Enabled = false;
                        }
                    }

                }
            }
        }


    }