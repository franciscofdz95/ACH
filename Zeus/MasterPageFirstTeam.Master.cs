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
using PaymentXP.DataObjects;
using System.Collections.Generic;
using CommonUtility;

public partial class MasterPageFirstTeam : frmBaseMaster
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (UserSessions.CurrentUser == null)
			Response.Redirect("~/frmLogin.aspx");

		if (HttpContext.Current.UrlContains("SecureFirstTeamForms"))
		{
			HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
		}

		if (!this.IsPostBack)
		{
			FormHandler.SetSecurity(this.Page.Master);
		}
	}
}