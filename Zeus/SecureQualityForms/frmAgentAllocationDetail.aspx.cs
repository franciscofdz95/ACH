using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAgentAllocationDetail : frmBaseDataEntry
    {

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            // Set Property to Store ViewState on Server
            base.StoreViewStateOnServer = true;

            //this.UID = CommonUtility.Util.if_s(Request.QueryString["AgentKeyID"], null);
            this.UID = CommonUtility.Util.if_s(Request.QueryString["AgentID"], null) + "&" + CommonUtility.Util.if_s(Request.QueryString["SourceName"], null);
            //this.UID = CommonUtility.Util.if_s(Request.QueryString["SourceName"], null);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ((HyperLink)this.Master.FindControl("lnkAgentAllocationDetails")).CssClass = "active";

            string queryStrAgentKeyID  = CommonUtility.Util.if_s(Request.QueryString["AgentKeyID"], null);
            if (!IsPostBack)
            {
                if (Request.QueryString["Adding"] != null)
                    this.AgentAllocationDetail1.Adding = Convert.ToBoolean(Request.QueryString["Adding"].ToString());

                if (Request.QueryString["statusmess"] != null && Request.QueryString["statusmess"].ToString() != string.Empty)
                    this.Master.AddMessageStatus(Request.QueryString["statusmess"].ToString());

                if (UserSessions.CurrentAgentAllocation != null)
                {
                    this.Page.Title = string.Format("AgentKeyID: {0} - AgentID: {1} - SourceName: {2}", queryStrAgentKeyID, UserSessions.CurrentAgentAllocation.AgentID, UserSessions.CurrentAgentAllocation.SourceName);
                    this.Master.SetStatusBarText(string.Format("<b>AgentKeyID:</b> {0}  <b>AgentID:</b> {1}  <b>SourceName:</b> {2}", queryStrAgentKeyID, UserSessions.CurrentAgentAllocation.AgentID, UserSessions.CurrentAgentAllocation.SourceName));
                }
            }
        }

        public override void FormShow(string ID)
        {
            throw new NotImplementedException();
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