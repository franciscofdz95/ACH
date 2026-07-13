using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


    public partial class frmCRMVendorSetupDetail : frmBaseDataEntry
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            // Set Property to Store ViewState on Server
            base.StoreViewStateOnServer = true;

            this.UID = CommonUtility.Util.if_s(Request.QueryString["CRMUID"], null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((HyperLink)this.Master.FindControl("lnkCRMDetails")).CssClass = "active";

            if (!IsPostBack)
            {
                if (Request.QueryString["Adding"] != null)
                    this.Compliance1.Adding = Convert.ToBoolean(Request.QueryString["Adding"].ToString());

                if (Request.QueryString["statusmess"] != null && Request.QueryString["statusmess"].ToString() != string.Empty)
                    this.Master.AddMessageStatus(Request.QueryString["statusmess"].ToString());

                if (UserSessions.CurrentCRM != null)
                {
                    this.Page.Title = string.Format("CRMID: {0} - CRM Name: {1}", UserSessions.CurrentCRM.CRMID, UserSessions.CurrentCRM.CRMName);
                    this.Master.SetStatusBarText(string.Format("<b>TPP ID:</b> {0}  <b>TPP Name:</b> {1}", UserSessions.CurrentCRM.CRMID, UserSessions.CurrentCRM.CRMName)); //Replace CRM to TPP for PXP-8417 by Ali Khan
                }
            }
        }


        public override void FormShow(string ID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void FormClear()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool FormSave()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void FormNew()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool FormDelete()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool FormDataCheck()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void FormCancel()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ToggleButtons()
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }