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
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using CommonUtility;
using System.Globalization;
using PaymentXP.BusinessObjects.Tickets;
using System.Linq;
using HtmlAgilityPack;
using System.IO;//code added by koshlendra for PXP-7622
using ZeusWeb.Extensions;

public partial class wucTicket : wucBaseDataEntry
{
	public string vsChangeHistoryFieldID
	{
		get { return CommonUtility.Util.if_i(ViewState["vsChangeHistoryFieldID"], -1).ToString(); }
		set { ViewState["vsChangeHistoryFieldID"] = value; }
	}

	public string CopyTicketUID
	{
		get { return Convert.ToString(ViewState["CopyTicketUID"]); }
		set { ViewState["CopyTicketUID"] = Convert.ToString(value); }
	}

	public string CurrrentTicketUID
	{
		get { return Convert.ToString(ViewState["CurrrentTicketUID"]); }
		set { ViewState["CurrrentTicketUID"] = Convert.ToString(value); }
	}

	public Ticket OriginalTicket
	{
		get { return (Ticket)ViewState["OriginalTicket"]; }
		set { ViewState["OriginalTicket"] = value; }
	}

	public List<TicketNoteModel> list
	{
		get { return (List<TicketNoteModel>)ViewState["ListNotesExcel"]; }
		set { ViewState["ListNotesExcel"] = value; }
	}

	public int WEBSITE_REVIEW_CATEGORYID = 963;

	public bool AlertEnable
	{
		get
		{
			if (ViewState["AlertEnable"] == null)
			{
				return false;
			}
			else
			{
				return Convert.ToBoolean(ViewState["AlertEnable"]);
			}
		}
		set { ViewState["AlertEnable"] = value; }
	}

	public TimeZones TimeZoneID
	{
		get { return (TimeZones)ViewState["TimeZoneID"]; }
		set { ViewState["TimeZoneID"] = value; }

	}

	/// <summary>
	/// Used to figure out which button was clicked.. either "Save" (false) or "Save and Close" (true)
	/// </summary>
	bool _IsSaveClose = false;

	/// <summary>
	/// Used to figure out if we need to copy notes and documents also when copying a ticket.
	/// </summary>
	bool _IsCopyNotes = false;
	bool _IsCopyDocs = false;

	string _WindowCallID = "";

	/// <summary>
	/// the ticket control can either be called from a page, or a window. if its called from a window, then this value gets populated.
	/// </summary>
	public string WindowCallID
	{
		get { return _WindowCallID; }
		set { _WindowCallID = value; }
	}

	public bool IsPrivateEnable
	{
		get { return (bool)(ViewState["IsPrivateEnable"] ?? false); }
		set { ViewState["IsPrivateEnable"] = value; }
	}

	string m_RequestOrigin = string.Empty;

	public string RequestOrigin
	{
		get { return m_RequestOrigin; }
		set { m_RequestOrigin = value; }
	}
	public IList<GenericListItem> m_TicketApprovalManager;

	protected string GetSubmitPostBack()
	{
		return Page.ClientScript.GetPostBackEventReference(btnSave, string.Empty);
	}

	protected string GetSavenClosePostBack()
	{
		return Page.ClientScript.GetPostBackEventReference(btnSaveClose, string.Empty);
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		EnsureChildControls();
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		var script = ScriptManager.GetCurrent(this.Page);
		script.RegisterPostBackControl(this.btnExpExcel);
		this.calDueDate.Format = UserSessions.CurrentUser.DatePattern;
		this.calCallbackDate.Format = UserSessions.CurrentUser.DatePattern;
		this._WindowCallID = CommonUtility.Util.if_s(Request.QueryString["WindowCallID"]);

		dlgClone.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdCloneMerchants.FindControl("btnSearch")).ClientID + "')");
		dlgcontrol.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdMerchants.FindControl("btnSearch")).ClientID + "')");
		dlgAgent.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdAgent.FindControl("btnSearch")).ClientID + "')");
		btnAddNotes.Attributes.Add("onClick", "Check('" + Description.ClientID + "')");
		Origin.Attributes.Add("onchange", "ToggleRepeat('" + Origin.ClientID + "')");

		this.UID = CommonUtility.Util.if_s(Request.QueryString["TicketUID"], null);

		//copyTicketUID
		this.CopyTicketUID = CommonUtility.Util.if_s(Request.QueryString["CopyTicketUID"], null);

		////Set Security on the page
		//FormHandler.SetSecurity(this.Page);
		grdCloneMerchants.CloneTicketCommand = CloneTicket;
		grdCloneMerchants.DoneCommand = RefreshTicket;

		grdMerchants.GridRowCommand += new wucMerchants.GridRowCommandHandler(grdMerchants_GridRowCommand);
		grdAgent.GridRowCommand += new wucAgent.GridRowCommandHandler(grdAgents_GridRowCommand);

		grdCloneMerchants.DataSourceSelectCountMethod = "GetMerchantAppsPagingRowCount";
		grdCloneMerchants.DataSourceSelectMethod = "GetMerchantAppsPaging";

		grdMerchants.DataSourceSelectCountMethod = "GetMerchantAppsPagingRowCount";
		grdMerchants.DataSourceSelectMethod = "GetMerchantAppsPaging";

		grdAgent.DataSourceSelectCountMethod = "GetAgentsPagingRowCount";
		grdAgent.DataSourceSelectMethod = "GetAgentsPaging";

		//Start Code added for PXP-14238
		m_TicketApprovalManager = LookupTableHandler.LoadTicketApprovalManager();
		//End Code added for PXP-14238

		if (UserSessions.CurrentUser == null)
		{
			Response.Redirect("~/frmLogin.aspx");
		}

		if (!IsPostBack)
		{
			//// default document sorting, and order
			//this.SortDirectionSearch = SortDirection.Ascending;
			//this.SortOrder = "ORIGNAME";

			this.TimeZoneID = UserSessions.CurrentUser.TimeZone;
			lblError.Text = "";

			LookupTableHandler.LoadMerchantAppTypes(MLEAcqBankUID, true);
			LookupTableHandler.LoadDropDownList(this.BankID, "Ach_Search_Bank_Info", "BankName", "BankID");
			LookupTableHandler.LoadUserTimeZones(TimeZone, false);
			LookupTableHandler.LoadTicketTemplates(this.ddlTicketTemplate, false);

			FillDropDown();

			dlgClone.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
			dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
			dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

			//set Adding flag
			if (this.Adding)
			{
				this.FormNew();
			}
			else
			{

				if (Request["AlertEnable"] != null)
					AlertEnable = Convert.ToBoolean(Request["AlertEnable"]);

				if (this.UID == string.Empty)
				{
					this.UID = (UserSessions.CurrentTicket == null) ? string.Empty : UserSessions.CurrentTicket.TicketUID;
				}

				if (!string.IsNullOrEmpty(this.UID))
				{
					this.FormShow(this.UID);
				}
				else if (!string.IsNullOrEmpty(this._WindowCallID))
				{
					this.FormNew();
				}
			}

			// START DM-794 by Jorge
			if (UserSessions.CurrentUser.UserRoles.Keys.Contains(Constants.ROLE_CREDIT_UNDERWRITING.ToUpper())
				|| UserSessions.CurrentUser.UserRoles.Keys.Contains(Constants.ROLE_RISK.ToUpper()))
			{
				IsPrivateFile.Checked = true;
				IsPrivateFile.Enabled = true;
				IsPrivateFileRow.Visible = true;
			}
			// END DM-794
		}

		// we ALWAYS SHOW the "Add Notes" section, if in edit mode
		//if (this.Adding == false)
		//{
		//    Panel pNE = (Panel)pnlTicketDetail.FindControl("pnlNoteEntry");
		//    if (pNE != null)
		//    {
		//        FormHandler.SetControlEditMode(pNE, true);
		//        TextBox tbDes = (TextBox)pNE.FindControl("Description");

		//        tbDes.ReadOnly = false;
		//        tbDes.Enabled = true;
		//    }
		//}
		//if (this.ddlTicketSource.SelectedValue == "e") // Scavenger requirements
		//{
		//    this.ChkSender.Visible = true;
		//    this.pnlScavenger.Visible = true;
		//    this.ContactEmail.Visible = true;
		//    this.Ticketcontact.Visible = false;
		//}

	}

	private void grdMerchants_GridRowCommand(object sender, GridViewCommandEventArgs e)
	{
		LinkButton lnk = null;
		if (e.CommandSource is LinkButton)
		{
			lnk = (LinkButton)e.CommandSource;
		}
		else
		{
			return;
		}
		string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
		string uid = str[0];

		MerchantFacade facade = new MerchantFacade();
		MerchantApp app = facade.GetMerchantAppZeus(uid);
		if (!this.IsMLETicket.Checked)
		{
			if (app != null && !string.IsNullOrEmpty(app.MerchantAppUID))
			{
				hypMerch.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={0}", app.MerchantAppUID);
				hypMerch.Enabled = true;
			}
			else
			{
				hypMerch.NavigateUrl = "";
				hypMerch.Enabled = false;
			}

			//FormBinding.BindObjectToControls(app, pnlTicketDetail);
			BusinessDBAName.Text = app.BusinessDBAName;
			MerchantID.Text = app.SettlePlatformMid;
			Bank.Text = app.BankName;
			ZID.Text = app.ID;
			AgentDBA.Text = app.AgentDBA;
			MerchantFMAID.Text = app.FMAID.ToString();
			AgentDBA.Text = app.AgentDBA;
			MerchantAppUID.Value = app.MerchantAppUID;
			AgentUID.Value = app.AgentUID;
			//PXP-8889 Attachment Download Error
			AgentID.Value = app.AgentID.ToString();

			//reset dropdown list for ach bank
			this.BankID.SelectedIndex = 0;
			ACHID.Text = app.AchID.ToString();
		}
		else
		{
			MLEName.Text = app.BusinessLegalName;
			MLEAcqBankUID.SelectedValue = app.MerchantAppTypeUID;
			ShowMLESpeceficFields();

		}

		DateCreated.Text = WebUtil.ConvertToUserDateTimeSettings(DateTime.Now.ToString());
		UserCreated.Text = UserSessions.CurrentUser.UserName;

		AchMerchant ach = DataAccess.DataAchMerchantDao.GetAchMerchant(CommonUtility.Util.if_i(app.ID, 0));

		if (ach != null)
			this.BankID.SelectedValue = ach.BankID.ToString();

		grdMerchants.ClearGrid();
		this.dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

		if (string.IsNullOrEmpty(BusinessDBAName.Text) || UserSessions.CurrentUser.IsAgent)
		{
			hypMerch.Enabled = false;
			hypMerch.NavigateUrl = "";
		}
		else
		{
			hypMerch.Enabled = true;
			hypMerch.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={0}", app.MerchantAppUID);
		}

		cbEmailFT.Visible = app.FirstTeam;
		cbEmailFT.Checked = app.FirstTeam;
		//chkAgent.Checked = (app.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");// directsales agent
		if (!this.IsMLETicket.Checked)
		{
			ChkMerchant.Visible = true;
			ChkMerchant.Enabled = true;
			chkAgent.Enabled = true;
		}


		wucContact1.ControlContactType = eControlContactType.Merchant;
		wucContact1.EditMode = false;
		wucContact1.ObjectID = CommonUtility.Util.if_i(app.ID, 0);
		wucContact1.FormShow("", false);

		if (!this.IsMLETicket.Checked)
		{
			Ticketcontact.Visible = false;
			OtherContact.Visible = true;
		}
		else
		{
			OtherContact.Visible = false;
			Ticketcontact.Visible = true;
		}
		//Fady Massoud 12-22-2020
		//PXP-15777 Scavenger Removal
		//tblEmailSender.Visible = ChkSender.Checked;

		if (this.Adding)
		{
			if (string.IsNullOrWhiteSpace(app.PrivateLabelUID))
			{
				this.PrivateLabelUID.Enabled = true;
				this.PrivateLabelUID.SelectedIndex = 0;
			}
			else
			{
				this.PrivateLabelUID.Enabled = false;
				this.PrivateLabelUID.SelectedValue = app.PrivateLabelUID;
			}
		}
		else if (string.IsNullOrWhiteSpace(UserSessions.CurrentTicket.PrivateLabelUID))
		{
			if (!string.IsNullOrWhiteSpace(app.PrivateLabelUID))
			{
				this.PrivateLabelUID.Enabled = false;
				this.PrivateLabelUID.SelectedValue = app.PrivateLabelUID;
			}
			else
			{
				this.PrivateLabelUID.Enabled = true;
				this.PrivateLabelUID.SelectedIndex = 0;
			}
		}

		//lbRemoveMerchant.Visible = true; //rollback US 7387
	}

	private void grdAgents_GridRowCommand(object sender, GridViewCommandEventArgs e)
	{
		LinkButton lnk = null;
		if (e.CommandSource is LinkButton)
		{
			lnk = (LinkButton)e.CommandSource;
		}
		else
		{
			return;
		}

		string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
		string uid = str[0];
		BusinessDBAName.Text = "";
		MerchantAppUID.Value = "";
		AgentUID.Value = str[0];
		DataAgent da = new DataAgent();
		Agent app = da.GetAgent(uid);
		FormBinding.BindObjectToControls(app, pnlTicketDetail);
		BusinessContact.Text = app.ContactFirstName + " " + app.ContactLastName;
		BusinessEmailAddress.Text = app.ContactEmail;
		BusinessPhone.Text = app.ContactPhone;
		BusinessFax.Text = app.Fax;
		AgentDBA.Text = app.AgentDBA;
		AgentID.Value = app.AgentID.ToString();//PXP-8889 Attachment Download Error
		if (ddlTicketSource.SelectedValue == "a")
		{
			chkAgent.Checked = true;//(app.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");// directsales agent
		}
		chkAgent.Enabled = true;
		DateCreated.Text = WebUtil.ConvertToUserDateTimeSettings(DateTime.Now.ToString());
		UserCreated.Text = UserSessions.CurrentUser.UserName;

		if (string.IsNullOrEmpty(BusinessDBAName.Text))
		{
			ChkMerchant.Enabled = false;
		}
		grdAgent.ClearGrid();
		this.dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

		wucContact1.ControlContactType = eControlContactType.Agent;
		wucContact1.EditMode = false;
		wucContact1.ObjectID = app.AgentID;
		wucContact1.FormShow("", false);

		Ticketcontact.Visible = false;
		OtherContact.Visible = true;
		//Fady Massoud 12-22-2020
		//PXP-15777 Scavenger Removal
		//tblEmailSender.Visible = ChkSender.Checked;

	}

	protected void grdTicketNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		grdTicketNotes.PageIndex = e.NewPageIndex;
		this.BindNotes(CommonUtility.Util.if_i(this.TicketID.Text, 0));
	}

	protected void grdTicketNotes_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		switch (e.Row.RowType)
		{
			case DataControlRowType.DataRow:

				TicketNotes tn = (TicketNotes)e.Row.DataItem;

				((Literal)e.Row.FindControl("litUserCreated")).Text = tn.UserCreated;
				((Label)e.Row.FindControl("labDateCreated")).Text = WebUtil.ConvertToUserDateTimeSettings(tn.DateCreated.ToString());
				((Label)e.Row.FindControl("labDateCreated")).ToolTip = WebUtil.ConvertToUserDateTimeSettings(tn.DateCreated.ToString());


				if (tn.Description.Contains("<html") && tn.Description.Contains("<body"))
				{
					string noteDesciption = tn.Description.Substring(tn.Description.IndexOf("<body")).ToString();

					HtmlDocument htmlDoc = new HtmlDocument();
					htmlDoc.LoadHtml(noteDesciption);

					if (htmlDoc.DocumentNode.InnerHtml.Contains("<base"))
						foreach (HtmlNode node in htmlDoc.DocumentNode.SelectNodes("//base"))
						{
							node.Remove();
						}

					noteDesciption = htmlDoc.DocumentNode.OuterHtml.ToString();

					((Literal)e.Row.FindControl("litBody")).Text = noteDesciption;

				}
				else
				{
					string formatted = Regex.Replace(tn.Description, @"\n", "<br />");
					((Literal)e.Row.FindControl("litBody")).Text = formatted;
				}

				// original
				//((Literal)e.Row.FindControl("litBody")).Text = Server.HtmlDecode(Server.HtmlDecode(formatted));

				Panel p = (Panel)e.Row.FindControl("pnlBlock");

				if (tn.IsInternal == false)
				{
					// external ticket
					p.CssClass = "ticketitem external";
				}
				else
				{
					p.CssClass = "ticketitem";
				}

				string temp = HttpUtility.HtmlDecode(tn.Description);
				//if (!string.IsNullOrEmpty(Solution.Text.Trim()) && (tn.Description.Trim()).Equals(Solution.Text.Trim()))
				string desctemp = HttpUtility.HtmlDecode(tn.Description.Trim());
				//Niranjan : PXP-3409 Scavenger #  "set as solution" flag is not visible on Re-opened tickets
				if (desctemp.Contains("<html") && desctemp.Contains("<body"))
				{
					desctemp = desctemp.Substring(desctemp.IndexOf("<body")).ToString();
				}
				string solution = HttpUtility.HtmlDecode(Solution.Text.Trim());
				if (!string.IsNullOrEmpty(Solution.Text.Trim()) && (desctemp.Equals(solution)) || (UserSessions.CurrentTicket != null && tn.Description.Trim() == UserSessions.CurrentTicket.Solution))
				{
					((CheckBox)e.Row.FindControl("chkIsSolution")).Checked = true;
					//fadfod
					//Fady Massoud 12-24-2020
					//PXP-15780 Notes
					((CheckBox)e.Row.FindControl("chkIsSolution")).Enabled = false;
					p.CssClass = "ticketitem ticketsolution";
				}
				else
				{
					p.CssClass = "ticketitem";
				}


				((CheckBox)e.Row.FindControl("ChkAccessToPartner")).Checked = tn.ViewAgent;
				((CheckBox)e.Row.FindControl("ChkAccessToMerchant")).Checked = tn.ViewMerchant;
				//Scavenger related stuff

				((CheckBox)e.Row.FindControl("ChkAccessToSender")).Checked = tn.ViewSender;
				if (UserSessions.CurrentTicket.TicketSource == "e")
				{
					((CheckBox)e.Row.FindControl("ChkAccessToSender")).Visible = true;
				}


				// show the requires feedback label if the note requires feedback.
				((Label)e.Row.FindControl("lblFeedBackRequired")).Visible = tn.IsFeedBackRequired;

				//*************** Got rid of the requires attention logic to couple the emal and access to ticket notes of the Merchant and Partner (TFS 5079 & TFS 6041) **********************
				// only show if ticket is external and isfeedbackreq is 0
				//if (ddlTicketSource.SelectedValue == "a" || ddlTicketSource.SelectedValue == "m" || ddlTicketSource.SelectedValue == "x")
				//{
				//    if (tn.IsInternal == "0")
				//    {
				//        // external tickets dont get the checkbox
				//        ((CheckBox)e.Row.FindControl("chkReqAtt")).Visible = false;
				//    }
				//    else
				//    {
				//        // show checkbox depending on if it is req or not.
				//        ((CheckBox)e.Row.FindControl("chkReqAtt")).Visible = (tn.IsFeedBackRequired == "0") ? true : false;
				//    }

				//    switch (ddlTicketSource.SelectedValue)
				//    {
				//        case "a":
				//            ((CheckBox)e.Row.FindControl("chkReqAtt")).Text = "Email Partner";
				//            break;

				//        case "m":
				//            ((CheckBox)e.Row.FindControl("chkReqAtt")).Text = "Email Merchant";
				//            break;

				//        case "x":
				//            ((CheckBox)e.Row.FindControl("chkReqAtt")).Text = "Email Merchant";
				//            break;
				//    }

				//}
				//else
				//{
				//    // if anything other than agent ticket, we hide the checkbox.
				//    ((CheckBox)e.Row.FindControl("chkReqAtt")).Visible = false;
				//}

				//Fady Massoud 12-24-2020
				//PXP-15780 Notes
				//((CheckBox)e.Row.FindControl("chkIsSolution")).Enabled = this.EditMode;//btnAddNotes.Enabled;
				((CheckBox)e.Row.FindControl("chkIsSolution")).Visible = (tn.UserCreated != "System");

				break;

			default:
				break;
		}
	}

	protected void gvFile_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			MDoc obj = (MDoc)e.Row.DataItem;

			CheckBox chkPrivate = ((CheckBox)e.Row.FindControl("IsPrivate"));
			chkPrivate.Checked = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsPrivate"), false);
			chkPrivate.Enabled = IsPrivateEnable;

			bool isAccessible = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsAccessible"), false);

			HyperLink lnk = (HyperLink)e.Row.FindControl("litHyp");
			Label lb = (Label)e.Row.FindControl("lblComment");
			HiddenField hON = (HiddenField)e.Row.FindControl("HidOrigName");
			Label lbl = (Label)e.Row.FindControl("lblOrigName");

			lbl.Visible = false;
			lnk.Visible = true;

			Dictionary<string, string> di = new Dictionary<string, string>();

			di.Add("DocID", obj.DocID.ToString());
			di.Add("PrimaryKeyID", obj.PrimaryKeyID.ToString());

			if (chkPrivate.Checked && !isAccessible)
			{
				lbl.Visible = true;
				lnk.Visible = false;
			}

			if (chkPrivate.Checked)
				lnk.ToolTip = "Private Document";

			// changed literal to hyperlink so that we can add tooltip on that
			string full_url = WebUtil.GetBaseUrl() + "SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x=" + Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di));
			lnk.NavigateUrl = full_url;

			if (!string.IsNullOrEmpty(obj.Description))
			{
				lb.Text = obj.Description;
				lb.Visible = true;
			}
			else
			{
				lb.Visible = false;
			}

			e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);


			//adding file size to the grid to show up in KB only.
			string contentSize = DataBinder.Eval(e.Row.DataItem, "ContentSize").ToString();
			decimal size;

			if (decimal.TryParse(contentSize, out size))
			{
				decimal percent = size / 1024;

				e.Row.Cells[5].Text = percent.ToString("N") + "KB";
			}


		}
	}

	protected void gvFile_PreRender(object sender, EventArgs e)
	{
		foreach (GridViewRow grv in gvFile.Rows)
		{

			HyperLink lit = (HyperLink)grv.FindControl("litHyp");

			HiddenField hDocID = (HiddenField)grv.FindControl("HidDocID");
			HiddenField hPKID = (HiddenField)grv.FindControl("HidPKID");
			HiddenField hON = (HiddenField)grv.FindControl("HidOrigName");


			if (lit != null && hDocID != null && hPKID != null && hON != null)
			{

				Dictionary<string, string> di = new Dictionary<string, string>();

				di.Add("DocID", hDocID.Value.ToString());
				di.Add("PrimaryKeyID", hPKID.Value.ToString());


				string full_url = WebUtil.GetBaseUrl() + "SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x=" + Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di));

				//lit.Text = string.Format("<a  target='_blank' href='{0}'>{1}</a>", full_url, hON.Value);
				lit.NavigateUrl = full_url;

			}

		}
	}

	protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
	{
		WebImageButton btn = (WebImageButton)sender;

		string url = string.Empty;
		switch (btn.CommandName)
		{
			case "Add":

				pnlAux.Visible = false;

				// //if (!(this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow))))
				// //{
				//     if (this.Page != null && this.Page.Master != null && (Label)this.Page.Master.FindControl("lblActiveTicket") != null)
				//     {
				//         // if here, then that means this control is called from a masterpage.
				//         //string myurl = string.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding={0}&TicketID={1}&AlertEnable={2}", "true", UserSessions.CurrentTicket.TicketID.ToString(), AlertEnable);
				//         string myurl = string.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding={0}&AlertEnable={1}", "true", AlertEnable);
				//         Response.Redirect(myurl);
				//     }
				//// }

				if (string.IsNullOrEmpty(this.WindowCallID))
				{
					Response.Redirect(WebUtil.GetMyUrl("Adding=true"));
				}
				else
				{
					Response.Redirect(WebUtil.GetMyUrl("Adding=true&WindowCallID=" + this.WindowCallID));
				}

				//this.FormNew();

				break;


			case "Save":
				{
					this._IsSaveClose = false;
					Ticket t = this.HandleSaveButton();

					if (t != null && !String.IsNullOrEmpty(t.TicketUID))
					{
						//PXP-16180 Copy Ticket feature 
						//07/09/2021 - Fady Massoud                        
						if (!string.IsNullOrWhiteSpace(this.CopyTicketUID))
						{
							chkNotes.Visible = CommonUtility.Util.if_i(NoteCount.Value, 0) > 0 ? true : false;
							chkDocuments.Visible = CommonUtility.Util.if_i(DocsCount.Value, 0) > 0 ? true : false;
							//bool isMLEEnabled = false;
							//if ((OriginalTicket.IsMLETicket && IsMLETicket.Checked && OriginalTicket.MLEName.ToUpper().Trim() == MLEName.Text.ToUpper().Trim())
							//    || (!OriginalTicket.IsMLETicket && !IsMLETicket.Checked))
							//{
							//    isMLEEnabled = true;
							//}

							//if (_DocsCount > 0 && isMLEEnabled)
							//    chkDocuments.Visible = true;
							//else
							//    chkDocuments.Visible = false;

							lblSuccess.Text = "Ticket Created Sucessfully";
							CurrrentTicketUID = t.TicketUID;

							if (chkNotes.Visible || chkDocuments.Visible)
							{
								dlgConfirm.WindowState = DialogWindowState.Normal;
							}
							else if (!string.IsNullOrEmpty(this._WindowCallID))
							{
								ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
							}
							else
							{
								Response.Redirect(String.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding=False&TicketUID={0}", t.TicketUID));
							}
						}
						else
						{
							if (!string.IsNullOrEmpty(this._WindowCallID))
							{
								ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
							}
							else
							{
								Response.Redirect(String.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding=False&TicketUID={0}", t.TicketUID));
							}
						}
					}
				}

				break;


			case "SaveClose":
				{
					this._IsSaveClose = true;
					Ticket t = this.HandleSaveButton();

					if (t != null && CommonUtility.Util.IsValidGuid(t.TicketUID))
					{
						if (!string.IsNullOrEmpty(this._WindowCallID))
						{
							ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
						}
						else
						{
							Response.Redirect("~/SecureTicketForms/frmTicketSearch.aspx");
						}
					}
				}

				break;

			case "Refresh":


				if (UserSessions.CurrentTicket != null)
				{
					Hashtable prms = new Hashtable();
					prms.Add("@UID", UserSessions.CurrentTicket.TicketUID);
					UserSessions.CurrentTicket = DataAccess.DataTicketDao.GetTicket(prms, UserSessions.CurrentUser.TimeZone);
					Response.Redirect(WebUtil.GetMyUrl("Adding=false&TicketUID=" + UserSessions.CurrentTicket.TicketUID));
				}

				break;

			case "Cancel":

				//if (!(this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow))))
				//{
				//    if (this.Page != null && this.Page.Master != null && (Label)this.Page.Master.FindControl("lblActiveTicket") != null)
				//    {
				//        // if here, then that means this control is called from a masterpage.

				//        //string myurl = string.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding={0}&TicketID={1}", "false", UserSessions.CurrentTicket.TicketID.ToString());
				//        string myurl = "~/SecureTicketForms/frmTicketSearch.aspx";
				//        Response.Redirect(myurl);
				//    }
				//}


				// if we're being cancelled from a window, then close the window.
				if (!string.IsNullOrEmpty(this._WindowCallID))
				{
					ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
				}
				else
				{

					if (this.UID == string.Empty)
					{
						this.FormClose(sender, e);
					}
					else
					{
						this.FormCancel();
					}
				}



				break;

			case "Edit":

				this.EditMode = true;
				this.FormShow(this.UID);
				this.ToggleButtons();


				if (UserSessions.CurrentTicket.StatusID.ToUpper().Trim() == Ticket.TICKET_CLOSE)
				{
					Solution.Enabled = true;
				}

				break;

			case "Close":

				this.FormClose(sender, e);

				break;

			case "Copy":
				{
					if (UserSessions.CurrentTicket != null)
						ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), "CopyTicketWindow", "CopyTicket('" + UserSessions.CurrentTicket.TicketUID + "')", true);

				}
				break;

			case "Clone":
				{
					if (UserSessions.CurrentTicket != null && this.FormDataCheck())
					{
						Hashtable prms = new Hashtable();
						grdCloneMerchants.ClearGrid();
						grdCloneMerchants.SetTicket(UserSessions.CurrentTicket.TicketUID, UserSessions.CurrentTicket.Problem, UserSessions.CurrentTicket.DueDate);
						grdCloneMerchants.SetDataSource(prms, 10);
						dlgClone.Modal = true;
						dlgClone.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
					}
				}
				break;
		}
	}
	//Upload Document
	//protected void btnSubmit_Click(object sender, EventArgs e)
	//{
	//    if (!string.IsNullOrEmpty(this.UID) && UserSessions.diCurrentTicket.ContainsKey(this.UID.ToUpper()))
	//    {
	//        Ticket t = UserSessions.diCurrentTicket[this.UID.ToUpper()];

	//        if (t.TicketClone == null)
	//            t.CloneTicket();
	//    }
	//}

	protected void UserID_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (UserID.SelectedIndex > 0 && StatusID.SelectedItem.Value.ToUpper() == "CDCD0A20-6603-4B07-94DC-F65D01290F6B") //open status
		{
			ListHandler.ListFindItem(StatusID, "0AEE2CAB-CEC4-476B-9598-918DBABD43CF"); //assign
		}
		else if (UserID.SelectedIndex == 0 && (StatusID.SelectedValue.ToUpper().Equals("0AEE2CAB-CEC4-476B-9598-918DBABD43CF") || StatusID.SelectedValue.ToUpper().Equals("DFF04FF8-3C47-45E1-B0BB-30C22C7CAF17"))) // Assign or pending
		{
			ListHandler.ListFindItem(StatusID, "CDCD0A20-6603-4B07-94DC-F65D01290F6B"); //open
		}
	}

	protected void StatusID_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (StatusID.SelectedIndex <= 0 || StatusID.SelectedItem.Value.ToUpper() == "CDCD0A20-6603-4B07-94DC-F65D01290F6B") // open status
		{
			UserID.SelectedIndex = -1;
		}
	}

	protected void Merch_Click(object sender, EventArgs e)
	{
		if (!MerchantAppUID.Value.Equals(string.Empty))
		{
			//if (UserSessions.CurrentMerchantApp == null || !MerchantAppUID.Value.Equals(UserSessions.CurrentMerchantApp.MerchantAppUID))
			//{
			//    MerchantFacade facade = new MerchantFacade();
			//    MerchantApp app = facade.GetMerchantApp(MerchantAppUID.Value);
			//    UserSessions.CurrentMerchantApp = app;
			//}
			Response.Redirect("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=False&MerchantAppUID=" + MerchantAppUID.Value);
		}
	}

	protected void btnMerSelect_Click(object sender, EventArgs e)
	{
		Hashtable prms = new Hashtable();
		grdMerchants.SetDataSource(prms, 10);
		dlgcontrol.Modal = false;
		dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
	}

	protected void btnAgentSelect_Click(object sender, EventArgs e)
	{
		Hashtable prms = new Hashtable();
		grdAgent.SetDataSource(prms, 10);
		dlgAgent.Modal = false;
		dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
	}

	protected void btnAddNotes_Click(object sender, EventArgs e)
	{
		//Fady Massoud 12-24-2020
		//PXP-15780 Notes
		//Adding this code to save the ticket when adding a note in edit mode.
		//if (this.EditMode)
		//{
		//    // EditAddNotes = true;
		//    FormSaveTicket();
		//}
		// only called on "EDIT", not "Add" ?
		string old_solution_value = Server.HtmlDecode(UserSessions.CurrentTicket.Solution);
		lblError.Text = "";

		bool NoteAdded = false;
		//Fady Massoud 12-22-2020
		//PXP-15777 Scavenger Removal
		//string fromemail = this.EmailOutFromEmail.Text;
		//string toemail = this.EmailOutToEmail.Text;
		//string ccemail = this.EmailOutCCEmail.Text;

		if (UserSessions.CurrentTicket != null)
		{
			Ticket t = UserSessions.CurrentTicket;

			if (t.TicketClone == null)
				t.CloneTicket();

			t.Problem = Server.HtmlEncode(this.Problem.Text);

			NoteAdded = this.AddNotes(t);
			if (NoteAdded)
			{
				Description.Text = string.Empty;
				chkAgent.Checked = false;

				FormHandler.LogTicketFormChanges(string.Empty, t.TicketUID, t.TicketID, t.TicketClone, t, "Notes Added");

				if (chkSolution.Checked && old_solution_value != t.Solution.Trim() && old_solution_value != string.Empty)
				{
					chkSolution.Checked = false;
					string solution_header = "Current Solution Changed. Old Solution: ";
					this.AddNotes(t, solution_header + old_solution_value, false, true);
				}
				else
					chkSolution.Checked = false;

				this.LoadNotes(CommonUtility.Util.if_i(t.TicketID, 0));
			}

		}

		FormShow(this.UID);
		//if (!NoteAdded && UserSessions.CurrentTicket != null)
		//{
		//    this.EmailOutFromEmail.Text = fromemail;
		//    this.EmailOutToEmail.Text = toemail;
		//    this.EmailOutCCEmail.Text = ccemail;
		//}
	}

	protected void btnClearNotes_Click(object sender, EventArgs e)
	{
		Description.Text = string.Empty;
	}

	protected void chkIsSolution_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox chk = ((CheckBox)sender);

		if (UserSessions.CurrentTicket.StatusID.ToUpper().Trim() == Ticket.TICKET_CLOSE)
		{
			// cannot set the solution if ticket is closed.            
			lblError.Text = "Solution cannot be changed for closed ticket";
			return;
		}

		GridViewRow Row = ((GridViewRow)((CheckBox)sender).NamingContainer);
		int rowIndex = Row.RowIndex;
		bool isChecked = chk.Checked;

		//Fady Massoud 12-24-2020
		//PXP-15780 Notes Tab
		//Saving Solution selected
		Ticket ticket = UserSessions.CurrentTicket;
		string old_solution_value = "";

		if (isChecked)
		{
			foreach (GridViewRow grdRow in grdTicketNotes.Rows)
			{
				if (grdRow.RowIndex == rowIndex)
				{
					Literal lit = (Literal)grdRow.FindControl("litBody");
					Solution.Text = HttpUtility.HtmlEncode(lit.Text);
				}
				else
				{
					((CheckBox)grdRow.FindControl("chkIsSolution")).Checked = false;
				}
			}

			//Fady Massoud 12-24-2020
			//PXP-15780 Notes Tab
			//Saving Solution selected 
			if (!ticket.Solution.Equals(Solution.Text.Trim()))
			{
				ticket.ResolutionDate = DateTime.Today;
				old_solution_value = ticket.Solution.Trim();
				ticket.Solution = Solution.Text;
				DataAccess.DataTicketDao.UpdateTicket(ticket, this.TimeZoneID);
				FormHandler.LogTicketFormChanges(string.Empty, ticket.TicketUID, ticket.TicketID, ticket.TicketClone, ticket, string.Empty);
			}
			if (Solution.Text.Trim().Equals(string.Empty))
			{
				ticket.ResolutionDate = DateTime.MinValue;
			}

			if (old_solution_value != Solution.Text.Trim() && old_solution_value != string.Empty)
			{
				string solution_header = "Current Solution Changed. Old Solution: ";
				this.AddNotes(ticket, solution_header + old_solution_value, false, true);
			}

			this.LoadNotes(CommonUtility.Util.if_i(ticket.TicketID, 0));
		}
		else
		{
			Solution.Text = "";
		}
		FormShow(this.UID);
	}

	public override void FormNew()
	{
		this.FormClear();
		this.Adding = true;
		this.EditMode = true;
		FormHandler.SetControlEditMode(pnlTicketDetail, this.EditMode);
		this.ToggleButtons();
		FillDropDown();
		StatusID.Enabled = false;
		btnAgentSelect.Enabled = true;
		btnMerSelect.Enabled = true;
		btnAddNotes.Enabled = false;
		btnClear.Enabled = false;
		//Description.Enabled = false;
		grdTicketNotes.DataSourceID = "";
		grdTicketNotes.DataBind();
		hypMerch.Enabled = false;
		cbEmailFT.Visible = false;
		MerchantFMAID.ReadOnly = MerchantID.ReadOnly = Bank.ReadOnly = ZID.ReadOnly = true;
		MLEAcqBankUID.Enabled = this.IsMLETicket.Checked;

		ACHID.Enabled = false;
		BankID.Enabled = false;
		clickClose.Visible = false;

		if (StatusID != null)
		{
			ListHandler.ListFindItem(StatusID, Ticket.TICKET_OPEN); // open status
		}

		//this.pnlFileAttachments.Visible = false;

		//btnAddNotes.Enabled = btnSubmit.Enabled = false;

		switch (this.RequestOrigin)
		{
			case "Merchant":
				btnAgentSelect.Enabled = false;
				btnMerSelect.Enabled = false;
				btnMLESelect.Enabled = false;

				MerchantFacade facade = new MerchantFacade();

				//MerchantApp app = facade.GetMerchantApp(UserSessions.CurrentMerchantApp.MerchantAppUID);
				MerchantApp app = facade.GetMerchantAppZeus(Request.QueryString["MerchantAppUID"]);

				if (app != null)
				{
					//UserSessions.CurrentMerchantApp = app;
					FormBinding.BindObjectToControls(app, pnlTicketDetail);
					MerchantID.Text = app.SettlePlatformMid;

					AgentDBA.Text = app.AgentDBA;
					AgentUID.Value = app.AgentUID;
					AgentID.Value = app.AgentID.ToString();//PXP-8889 Attachment Download Error
					ZID.Text = app.ID;

					IsFTMerchant.Visible = app.FirstTeam;
					cbEmailFT.Checked = app.FirstTeam;
					cbEmailFT.Visible = app.FirstTeam;
					IsFTMerchant.Visible = app.FirstTeam;
					chkAgent.Checked = (app.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");// directsales agent
					Ticketcontact.Visible = false;
					OtherContact.Visible = true;
					MLEName.Text = app.BusinessLegalName;
					MLEAcqBankUID.SelectedValue = app.MerchantAppTypeUID;

					if (CommonUtility.Util.if_s(app.PrivateLabelUID) != "")
					{
						PrivateLabelUID.Enabled = false;
						pnlPrivateLabel.Visible = true;
						PrivateLabel objPL = DataAccess.DataMerchantAppDao.GetPrivateLabel(app.PrivateLabelUID);
						lblPL.Text = objPL.PLCompanyName;
					}
				}
				break;

			case "Agent":
				btnAgentSelect.Enabled = false;
				btnMerSelect.Enabled = false;

				//FormBinding.BindObjectToControls(UserSessions.CurrentAgent, pnlTicketDetail);
				//AgentDBA.Text = UserSessions.CurrentAgent.AgentDBA;

				DataAgent da = new DataAgent();
				Agent agent = da.GetAgent(Request.QueryString["AgentUID"]);

				if (agent != null)
				{
					FormBinding.BindObjectToControls(agent, pnlTicketDetail);
					AgentDBA.Text = agent.AgentDBA;
					AgentUID.Value = agent.AgentUID;
					AgentID.Value = agent.AgentID.ToString();//PXP-8889 Attachment Download Error
					chkAgent.Checked = (agent.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9"); // directsales agent
				}

				Ticketcontact.Visible = false;
				OtherContact.Visible = true;

				break;

			default:

				Ticketcontact.Visible = true;
				OtherContact.Visible = false;

				break;
		}

		AlertEnable = false;
		DateCreated.Text = WebUtil.ConvertToUserDateTimeSettings(DateTime.Now.ToString());
		UserCreated.Text = UserSessions.CurrentUser.UserName;
		//Niranjan : PXP-6929
		CreatedBy.Text = UserSessions.CurrentUser.FirstLastName;
		pnlID.Visible = false;

		imgCallbackDate.Enabled = true;
		imgDueDate.Enabled = true;
		//Make this field read only
		ddlTicketSource.Enabled = false;

		// set office id to user's office id on new ticket creation.
		if (UserSessions.CurrentUser.Office != CommonUtility.Util.Offices.NoOffice)
		{
			OfficeID.SelectedValue = ((int)UserSessions.CurrentUser.Office).ToString();
		}
		//Fady Massoud 12-22-2020
		//PXP-15781 Document Upload
		//pnlFileAttachOnTicketcreation.Visible = true;
		//this.pnlFileAttachments.Visible = false;       
		this.pnlTickettemplate.Visible = this.Adding;

		chkAgent.Enabled = string.IsNullOrEmpty(this.AgentDBA.Text) ? false : true;
		ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;

		// user cannot copy a ticket while adding
		btnCopy.Visible = false;
		//PXP-13364
		chkManagerApproval.Checked = false;
		chkManagerApproval.Enabled = false;
		// check if the copy ticketuid is sent in the url
		if (!string.IsNullOrWhiteSpace(this.CopyTicketUID))
		{
			CopyTicket(this.CopyTicketUID);
		}

		//TabControl.Tabs[1].Enabled = false;

	}

	public override void FormShow(string UID)
	{
		DataTicket data = DataAccess.DataTicketDao;
		Ticket ticket = new Ticket();
		Hashtable prms = new Hashtable();


		prms.Add("@UID", UID);
		ticket = data.GetTicket(prms, UserSessions.CurrentUser.TimeZone);

		DataTicket objDTicket = new DataTicket();
		bool Validation = objDTicket.GMATicketValidationCheck(UserSessions.CurrentUser, ticket);
		if (!Validation)
		{
			HttpContext.Current.Response.Redirect("~/frmNoAccess.aspx");
		}


		// ticket.Solution = Server.HtmlDecode(ticket.Solution);
		ticket.Problem = Server.HtmlDecode(ticket.Problem);

		if (!string.IsNullOrEmpty(ticket.Solution))
		{
			this.clickClose.Enabled = true;
		}

		pnlID.Visible = true;
		UserSessions.CurrentTicket = ticket;
		clickClose.Visible = true;

		// if this ticket has a website review, then redirect to it!!
		int ws_compliance_id = WSComplianceFacade.TicketHasWSCompliance(CommonUtility.Util.if_i(ticket.TicketID, 0));
		if (ws_compliance_id > 0 && ticket.ZID != "")
		{
			Response.Redirect(string.Format("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID={0}&TicketUID={1}&WSComplianceID={2}"
					, ticket.MerchantAppUID
					, ticket.TicketUID
					, ws_compliance_id.ToString()
					));
		}



		FormBinding.BindObjectToControls(ticket, pnlTicketDetail);
		FormHandler.SetControlEditMode(pnlTicketDetail, this.EditMode);

		if (!string.IsNullOrWhiteSpace(ticket.DueDate.ToString()) && ticket.DueDate != DateTime.MinValue && ticket.DueDate.Year != 0001)
		{
			DueDate.Text = WebUtil.ConvertToUserDatePattern(ticket.DueDate.ToString());
			ListHandler.ListFindItem(DueDateTime, ticket.DueDate.ToString("HH:mm tt"));
		}

		if (!string.IsNullOrWhiteSpace(ticket.CallbackDate.ToString()) && ticket.CallbackDate != DateTime.MinValue && ticket.CallbackDate.Year != 0001)
		{
			CallbackDate.Text = WebUtil.ConvertToUserDatePattern(ticket.CallbackDate.ToString());
		}

		// why is this not set?
		AgentDBA.Text = ticket.AgentDBA;
		AgentUID.Value = ticket.AgentUID;
		//PXP-8889:START Attachment Download Error
		if (!string.IsNullOrEmpty(AgentUID.Value))
		{
			DataAgent dataAgent = DataAccess.DataAgentDao;
			Agent agent = dataAgent.GetAgent(AgentUID.Value);
			if (agent != null)
				ticket.AgentID = agent.AgentID.ToString();
			else
				ticket.AgentID = "0";
		}
		AgentID.Value = ticket.AgentID;
		//PXP-8889:END Attachment Download Error
		btnAgentSelect.Enabled = this.EditMode;
		btnMerSelect.Enabled = this.EditMode;
		Description.Enabled = this.EditMode;
		btnMLESelect.Enabled = this.EditMode;
		MLEAcqBankUID.Enabled = this.EditMode;
		IsMLETicket.Enabled = this.EditMode;


		cbEmailFT.Visible = ticket.IsFTMerchant;
		cbEmailFT.Checked = ticket.IsFTMerchant ? ticket.IsFTEmailReq : false;

		pnlAux.Visible = true;

		//Fady Massoud
		//PXP-15780 Ticket Tab
		//grdTicketNotes.DataSource = ticket.TicketNotes;
		//grdTicketNotes.DataBind();

		//LoadStatus(ticket);

		if (string.IsNullOrEmpty(BusinessDBAName.Text) || UserSessions.CurrentUser.IsAgent)
		{
			hypMerch.Enabled = false;
			hypMerch.NavigateUrl = "";
		}
		else
		{
			hypMerch.Enabled = true;
			hypMerch.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID={0}", ticket.MerchantAppUID);
		}

		if (ticket.TicketSource.Trim() == "")
		{
			ticket.TicketSource = "i";
		}

		ddlTicketSource.SelectedValue = ticket.TicketSource;
		ddlTicketSource.Enabled = false;
		pnlAgentSubmittedBy.Visible = false;





		switch (ddlTicketSource.SelectedValue)
		{

			case "a":
				// load departments and categories
				this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);

				// if its an agent ticket and submitted by is empty, we dont show the label.
				if (ticket.AgentSubmittedBy.Trim() != "")
				{
					pnlAgentSubmittedBy.Visible = true;
				}

				break;

			case "m":
				// load departments and categories
				this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);
				break;


			case "x":
				this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);
				break;

			case "e":
				this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);
				break;

			default:
				// on default, display as internal
				this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);
				break;
		}

		pnlPrivateLabel.Visible = false;

		//rules for private label uid:
		//1. if ticket alreay has a private label uid assigned to it, it can enver change even if the user selects a new merchant to assign ticket to
		//2. you can assign a private label uid on add new ticket. if a merchant is assigned to a private label uid when you're adding a ticket, u cant change the private label uid. 
		//3. for updates, you can assign a private label uid if no private label uid is assigned to the ticket. but once a private label uid is assigned it can never change becus if it does user will lose visibility of private label tickets
		//4. if the merchant is not assigned to a private label or no merchant is assigned to the ticket then u can assign a private label uid
		if (CommonUtility.Util.if_s(ticket.PrivateLabelUID) != "")
		{
			PrivateLabelUID.Enabled = false;
			pnlPrivateLabel.Visible = true;
			PrivateLabel objPL = DataAccess.DataMerchantAppDao.GetPrivateLabel(ticket.PrivateLabelUID);
			lblPL.Text = objPL.PLCompanyName;
		}

		MerchantID.ReadOnly = true;
		Bank.ReadOnly = true;
		ZID.ReadOnly = true;
		MerchantFMAID.ReadOnly = true;
		this.BankID.Enabled = false;
		this.ACHID.ReadOnly = true;

		OtherContact.Visible = false;
		Ticketcontact.Visible = true;

		//load ach information
		if (CommonUtility.Util.IsValidGuid(ticket.MerchantAppUID))
		{
			MerchantFacade facaca = new MerchantFacade();
			MerchantApp mApp = facaca.GetMerchantAppZeus(ticket.MerchantAppUID);

			if (mApp != null)
			{
				ACHID.Text = mApp.AchID.ToString();

				AchMerchant ach = DataAccess.DataAchMerchantDao.GetAchMerchant(CommonUtility.Util.if_i(mApp.ID, 0));

				if (ach != null)
					this.BankID.SelectedValue = ach.BankID.ToString();
			}

			OtherContact.Visible = true;
			Ticketcontact.Visible = false;

			wucContact1.ControlContactType = eControlContactType.Merchant;
			wucContact1.EditMode = false;
			wucContact1.ObjectID = CommonUtility.Util.if_i(mApp.ID, 0);
			wucContact1.FormShow("", false);
		}
		else if (string.IsNullOrWhiteSpace(ticket.MerchantAppUID) && CommonUtility.Util.IsValidGuid(ticket.AgentUID))
		{
			DataAgent da = new DataAgent();
			Agent agent = da.GetAgent(ticket.AgentUID);

			OtherContact.Visible = true;
			Ticketcontact.Visible = false;

			wucContact1.ControlContactType = eControlContactType.Agent;
			wucContact1.EditMode = false;
			wucContact1.ObjectID = agent.AgentID;
			wucContact1.FormShow("", false);
		}

		ListHandler.ListFindItem(ParentID, ticket.ParentID);
		ListHandler.ListFindItem(CategoryID, ticket.CategoryID);

		//btnAddNotes.Enabled = btnSubmit.Enabled = true;

		User user = UserSessions.CurrentUser;

		UserForm frm = null;

		if (user.UserForms.TryGetValue("FRMTICKETDETAIL", out frm) && frm.HasAccess)
		{
			if (frm.ControlObjects == null)
				DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

			foreach (ControlObject obj in frm.ControlObjects)
			{
				IsPrivateEnable = (obj.Type.ToUpper() == "CHECKBOX" && obj.ID.ToUpper() == "PRIVATEDOCUMENT" && obj.IsVisible && obj.IsEnabled);
			}

		}
		//Fady Massoud 12-22-2020
		//PXP-15781 Document Uploader
		//this.LoadDocuments(ticket);

		// we ALWAYS SHOW the "Add Notes" section. if in edit mode.

		if (this.Adding == false)
		{
			Panel pNE = (Panel)pnlTicketDetail.FindControl("pnlNoteEntry");
			if (pNE != null)
			{

				FormHandler.SetControlEditMode(pNE, true);
				// TextBox tbDes = (TextBox)pNE.FindControl("Description");
				Infragistics.WebUI.WebHtmlEditor.WebHtmlEditor tbDes = (Infragistics.WebUI.WebHtmlEditor.WebHtmlEditor)pNE.FindControl("Description");


				tbDes.ReadOnly = false;
				tbDes.Enabled = true;
			}
		}

		grdTicketNotes.Enabled = true;

		if (!EditMode)
		{
			pnlChange.Attributes.Add("style", "display:none;");
			pblChangeText.Attributes.Add("style", "display:none;");
		}

		if (ticket.DueDate == DateTime.MinValue || ticket.DueDate.Year == 0001)
		{
			DueDate.Text = "";
			ListHandler.ListFindItem(DueDateTime, "-1");
		}

		if (ticket.CallbackDate == DateTime.MinValue || ticket.CallbackDate.Year == 0001)
			CallbackDate.Text = "";

		//CheckAccessToBank();

		imgCallbackDate.Enabled = this.EditMode;
		imgDueDate.Enabled = this.EditMode;

		//*************** Now the "Access to Partner" & "Access to Merchant" will determine the access to ticket notes. Below code is for displaying based on ticket source. (TFS 5079 & TFS 6041) **********************
		switch (ticket.TicketSource)
		{
			case "i":

				if (!ticket.IsMLETicket)
				{
					chkAgent.Checked = false;
					chkAgent.Enabled = string.IsNullOrEmpty(ticket.AgentDBA) ? false : true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(ticket.BusinessDBAName) ? false : true;
				}
				break;

			case "e":

				//Fady Massoud 12-22-2020
				//PXP-15777 Scavenger Removal
				//this.ChkSender.Visible = true;
				//this.pnlScavenger.Visible = true;
				this.Ticketcontact.Visible = false;
				//ChkSender.Checked = false;
				if (!ticket.IsMLETicket)
				{
					chkAgent.Checked = false;
					ChkMerchant.Checked = false;

					chkAgent.Enabled = string.IsNullOrEmpty(ticket.AgentDBA) ? false : true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(ticket.BusinessDBAName) ? false : true;
				}
				//Fady Massoud 12-22-2020
				//PXP-15777 Scavenger Removal
				//if (string.IsNullOrEmpty(ticket.EmailOutFromEmail))
				//{
				//    this.EmailOutFromEmail.Text = ticket.ScavengerEmailTo;
				//}
				//if (string.IsNullOrEmpty(ticket.EmailOutToEmail))
				//{
				//    this.EmailOutToEmail.Text = ticket.ScavengerEmailFrom;
				//}
				//tblEmailSender.Visible = ChkSender.Checked;

				break;

			case "a":

				if (!ticket.IsMLETicket)
				{
					chkAgent.Checked = true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(ticket.BusinessDBAName) ? false : true;
				}

				break;

			case "m":

				if (!ticket.IsMLETicket)
				{
					ChkMerchant.Checked = true;
					chkAgent.Checked = (ticket.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");
				}
				break;

			case "x":
				if (!ticket.IsMLETicket)
				{
					ChkMerchant.Checked = true;
					chkAgent.Checked = (ticket.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");
				}
				break;
		}

		this.DateCreated.Text = WebUtil.CovertToUserDateTimePattern(ticket.DateCreated);
		//Niranjan : PXP-6929
		if (ticket.UserCreated.ToUpper() == "SYSTEM" || ticket.UserCreated.ToUpper() == "PRODUCT SUBSCRIPTION SERVICE")
		{
			this.CreatedBy.Text = ticket.UserCreated;
		}
		else
		{
			this.CreatedBy.Text = ticket.CreatedBy;
		}

		ShowMLESpeceficFields();
		this.TimeZone.ToolTip = TimeZone.SelectedItem.Text;

		//Fady Massoud 12-22-2020
		// History Default Load        
		//LoadTicketFieldHistory();

		chkAgentAction.Checked = ticket.IsAgentActionReq;
		if (PrivateLabelUID.SelectedValue.ToUpper().Equals(PrivateLabel.PRIVATELABEL_GMA) && UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
		{
			divAgentAction.Visible = true;
		}
		else
		{
			divAgentAction.Visible = false;
		}
		//PXP-13364
		chkManagerApproval.Checked = ticket.IsManagerApproval;
		if (this.EditMode)
		{
			if (UserSessions.CurrentUser != null && !string.IsNullOrEmpty(UserSessions.CurrentUser.UserID))
			{
				if (m_TicketApprovalManager != null)
				{
					bool isTicketApprovalManager = m_TicketApprovalManager.Any(ApprovalManagerUsers => ApprovalManagerUsers.ItemValue == UserSessions.CurrentUser.UserID);
					if (isTicketApprovalManager)
						chkManagerApproval.Enabled = true;
					else
						chkManagerApproval.Enabled = false;
				}
			}
		}
		else
		{ chkManagerApproval.Enabled = false; }
	}
	protected void tab_Click(object sender, EventArgs e)
	{
		switch (this.HiddenTabId.Value)
		{
			case "historytab":
				LookupTableHandler.LoadChangeHistoryFields(ddlChangeType, true, ChangeHistoryFields.ChangeHistoryFieldSource.Ticket);
				this.LoadTicketFieldHistory();
				break;
			case "uploadertab":
				LookupTableHandler.LoadMDocType(lstDocumentTypes, false, MDoc.eMDocTypeGroup.Ticket);
				this.LoadDocuments(UserSessions.CurrentTicket);
				break;
			case "notestab":
				//PXP-15947//Fady massoud 01-13-2020
				this.LoadNotes(CommonUtility.Util.if_i(UserSessions.CurrentTicket.TicketID, 0));
				break;
			default: break;
		}
	}


	private void LoadTicketFieldHistory()
	{
		DataTable dt = DataChangeLogs.SearchTicketChangeHistory(new Hashtable { { "@TicketID", Convert.ToInt32(UserSessions.CurrentTicket.TicketID) } });

		int ChangeHistoryFieldID = CommonUtility.Util.if_i(ddlChangeType.SelectedValue, 0);

		if (ChangeHistoryFieldID == 0)
		{
			this.grdChange.DataSource = dt;
			grdChange.Columns[0].Visible = true;
		}
		else
		{
			DataView dv = dt.DefaultView;
			dv.RowFilter = String.Format("ChangeHistoryFieldID='{0}'", ChangeHistoryFieldID);
			this.grdChange.DataSource = dv;
			grdChange.Columns[0].Visible = false;
		}

		this.grdChange.DataBind();
	}

	public override void FormClear()
	{
		grdMerchants.ClearGrid();
		grdAgent.ClearGrid();
		MerchantAppUID.Value = "";
		AgentUID.Value = "";
		AgentID.Value = "";//PXP-8889 Attachment Download Error
		FormHandler.ClearAllControls(pnlTicketDetail);

	}

	public override void FormCancel()
	{
		lblError.Text = string.Empty;
		this.EditMode = false;
		FormShow(this.UID);
		this.Adding = false;
		this.ToggleButtons();
	}

	public void FormClose(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
	{
		string url = string.Empty;

		if (Request.QueryString["PostBackURL"] != null)
		{
			url = Convert.ToString(Request.QueryString["PostBackURL"]);
		}
		else
		{
			url = "~/SecureTicketForms/frmTicketSearch.aspx";
		}

		if (!url.Equals(string.Empty))
		{
			Response.Redirect(url);
		}
		else
		{
			if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
			{
				FormClear();
				((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
			}


			if (!string.IsNullOrEmpty(this._WindowCallID))
			{
				ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
			}
		}
	}

	private Ticket FormSaveTicket()
	{
		//bool send = true;
		string sendTo = string.Empty;
		string userId = string.Empty;
		bool sendClosed = true;
		string ticketText = string.Empty;

		if (!this.FormDataCheck())
		{
			return null;
		}

		try
		{
			lblError.Text = "";
			Ticket ticket;
			Ticket cloneTicket;

			string OrigStatusID = string.Empty;
			string OrigUserID = string.Empty;
			string OrigDBAName = string.Empty;

			if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.BusinessDBAName != null)
			{
				OrigDBAName = UserSessions.CurrentTicket.BusinessDBAName;
			}

			if (this.Adding)
			{
				ticket = new Ticket();
			}
			else
			{
				ticket = (Ticket)UserSessions.CurrentTicket;
				OrigStatusID = ticket.StatusID;

				if (!string.IsNullOrEmpty(ticket.StatusID) && ticket.StatusID.Equals(StatusID.SelectedValue))
				{
					sendClosed = false;
				}

				OrigUserID = UserSessions.CurrentTicket.UserID;
			}

			ticket.CloneTicket();
			cloneTicket = ticket.TicketClone;

			if (!string.IsNullOrEmpty(MerchantAppUID.Value))
			{
				ticket.MerchantAppUID = MerchantAppUID.Value;
			}

			if (!string.IsNullOrEmpty(AgentUID.Value))
			{
				ticket.AgentUID = AgentUID.Value;
			}

			string old_solution_value = "";
			if (!ticket.Solution.Equals(Solution.Text.Trim()))
			{
				ticket.ResolutionDate = DateTime.Today;
				old_solution_value = ticket.Solution.Trim();
			}

			if (Solution.Text.Trim().Equals(string.Empty))
			{
				ticket.ResolutionDate = DateTime.MinValue;
			}

			FormBinding.BindControlsToObject(ticket, pnlTicketDetail);

			if (string.Equals(ticket.PrivateLabelUID, "-1"))
			{
				ticket.PrivateLabelUID = string.Empty;
			}

			DataTicket data = DataAccess.DataTicketDao;
			User user = UserSessions.CurrentUser;
			ticket.UserModified = user.UserName;
			ticket.CategoryID = CategoryID.SelectedValue;
			ticket.ParentID = ParentID.SelectedValue;
			ticket.IsFTEmailReq = ticket.IsFTMerchant ? cbEmailFT.Checked : false;
			ticket.AgentUID = ticket.IsMLETicket ? null : ticket.AgentUID;
			ticket.MerchantAppUID = ticket.IsMLETicket ? null : ticket.MerchantAppUID;
			if (PrivateLabelUID.SelectedValue.ToUpper().Equals(PrivateLabel.PRIVATELABEL_GMA) && UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
			{
				ticket.IsAgentEmailReq = true;
			}
			ticket.IsAgentActionReq = chkAgentAction.Checked;
			ticket.Problem = Server.HtmlEncode(this.Problem.Text);

			if (!string.IsNullOrEmpty(DueDate.Text))
			{
				DateTime dtDueDate;
				DateTime.TryParseExact(DueDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDueDate);
				ticket.DueDate = CommonUtility.Util.if_date(dtDueDate.ToString("MM/dd/yyyy") + " " + DueDateTime.SelectedItem.Text, DateTime.MinValue);
			}
			else if (string.IsNullOrEmpty(DueDate.Text) && (DueDateTime.SelectedItem.Text != "-- Select --"))
			{
				ticket.DueDate = CommonUtility.Util.if_date(DateTime.Now.ToString("MM/dd/yyyy") + " " + DueDateTime.SelectedItem.Text, DateTime.MinValue);

			}
			else if (string.IsNullOrEmpty(DueDate.Text) && (DueDateTime.SelectedItem.Text == "-- Select --"))
			{
				ticket.DueDate = DateTime.MinValue;
			}

			DateTime dtCallBackDate;
			DateTime.TryParseExact(CallbackDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallBackDate);
			ticket.CallbackDate = CommonUtility.Util.if_date(dtCallBackDate.ToString("MM/dd/yyyy"), DateTime.MinValue);
			ticket.ChangeReasonID = ticket.ChangeReasonID == "-1" ? string.Empty : ticket.ChangeReasonID;
			ticket.UserID = ticket.UserID == "-1" ? string.Empty : ticket.UserID;

			string body = string.Empty;
			int rows = 0;

			string FTEmail = cbEmailFT.Checked ? ticket.FTEmail : string.Empty;
			bool istrue = false;

			if (this.IsMLETicket.Checked)
			{
				ticket.BusinessDBAName = string.Empty;
			}

			if (!this.Adding)
			{
				// IN EDITING                
				istrue = UserSessions.CurrentTicket.IsDueDateHistory;

				if (istrue && (cloneTicket != null && ticket.DueDate != cloneTicket.DueDate))
				{
					if (ChangeReasonID.SelectedIndex == 0 || ChangeReason.Text.Trim() == string.Empty)
					{
						lblError.Text = "Please enter change reason.";
						pnlChange.Attributes.Add("style", "display:inline;");

						if (ChangeReasonID.SelectedItem.Text.ToUpper() == "OTHER")
						{
							pblChangeText.Attributes.Add("style", "display:inline;");
						}

						UserSessions.CurrentTicket = cloneTicket;
						return null;
					}
				}

				if (FormHandler.FormChanged(ticket, cloneTicket))
					ticket.AttentionReq = false;
				else if (ticket.AttentionReq)
					ticket.AttentionReq = true;

				if (ticket.StatusID != cloneTicket.StatusID && (ticket.StatusID == Ticket.TICKET_ASSIGNED || ticket.StatusID == Ticket.TICKET_OPEN))
					ticket.AttentionReq = true;
				else if (!string.IsNullOrWhiteSpace(cloneTicket.UserID) && ticket.UserID != cloneTicket.UserID)
					ticket.AttentionReq = true;

				if (ChangeReasonID.SelectedIndex != 0 || ChangeReason.Text.Trim() != string.Empty)
				{
					ticket.ChangeReason = string.IsNullOrEmpty(ChangeReason.Text.Trim()) ? ChangeReasonID.SelectedValue : ChangeReason.Text.Trim();
				}

				//PXP-13364
				ticket.IsManagerApproval = chkManagerApproval.Checked;

				rows = data.UpdateTicket(ticket, this.TimeZoneID);
				userId = ticket.UserID;

				UserSessions.CurrentTicket = ticket;

				FormHandler.LogTicketFormChanges(string.Empty, ticket.TicketUID, ticket.TicketID, ticket.TicketClone, ticket, string.Empty);
			}
			else
			{
				// IN ADDING

				int pending_review_count = WSComplianceFacade.GetPendingReviewCount(CommonUtility.Util.if_i(ZID.Text, 0));

				if (CommonUtility.Util.if_i(ticket.CategoryID, 0) == this.WEBSITE_REVIEW_CATEGORYID && pending_review_count > 0)
				{
					lblError.Text = "There is a review currently in progress. This ticket request cannot be completed.";
					return null;
				}

				ticket.DateCreated = DateTime.Now;
				ticket.UserCreated = user.UserName;
				ticket.UserCreatedUserUID = user.UID;

				DateTime dtCallbackDate;
				DateTime.TryParseExact(CallbackDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallbackDate);
				if (!string.IsNullOrEmpty(CallbackDate.Text) && CommonUtility.Util.if_date(dtCallbackDate.ToString("MM/dd/yyyy"), DateTime.MinValue) < DateTime.Today)
				{
					lblError.Text = "Please select a valid call back date.";
					return null;
				}

				ticket.TicketSource = ddlTicketSource.SelectedValue;

				if (ddlTicketSource.SelectedValue == "a" || ddlTicketSource.SelectedValue == "m" || ddlTicketSource.SelectedValue == "x")
				{
					ticket.AgentSubmittedBy = "Paysafe";
				}

				ticket.AttentionReq = true;
				//PXP-13364
				ticket.IsManagerApproval = chkManagerApproval.Checked;

				data.InsertTicket(ticket, this.TimeZoneID);


				if (ticket.TicketUID != "-1")
				{
					// were adding, so this will never be true.
					this.UID = ticket.TicketUID;
					DataTable dt = new DataTable();
					body = string.Empty;

					AgentAlerts objAlerts = null;
					IDictionary<int, AgentAlerts> agentAlerts = null;

					if (ticket.AgentUID != string.Empty || ticket.MerchantAppUID != string.Empty)
					{
						agentAlerts = DataAccess.DataAgentDao.LoadAgentAlerts(CommonUtility.Util.if_i(ticket.AgentID, 0));

						if (agentAlerts.TryGetValue(Alerts.MerchantTickets.GetHashCode(), out objAlerts) && objAlerts.Checked)
						{
							AlertEnable = true;
						}
					}

					// This Checkbox chkAgent is used to determine if an email should be sent on ticket creation. 
					// The chkAgent checkbox was only used to send notification when note is added, but this has been changed based on requirement in user Story 6571.
					// The ChkMerchant checkbox is now used to determine if the merchant is to be notified about the new ticket generation.
					PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, chkAgent.Checked || AlertEnable, ChkMerchant.Checked, FTEmail);

					ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} . Email sent to: {1}", ticket.TicketUID, FTEmail);
					// assigned or pending
					if (StatusID.SelectedValue.ToUpper().Equals(Ticket.TICKET_ASSIGNED) || StatusID.SelectedValue.ToUpper().Equals(Ticket.TICKET_PENDING))
					{
						userId = ticket.UserID;
					}

					// a new ticket is created at this point.
					// if it's up subcategory website review, and it has a merchant
					if (CommonUtility.Util.if_i(ticket.CategoryID, 0) == this.WEBSITE_REVIEW_CATEGORYID
						&& !string.IsNullOrEmpty(ticket.MerchantAppUID)
						)
					{
						if (pending_review_count == 0)
						{
							// no pending tickets, so we can create

							// insert into WSCompliance Table,
							Hashtable prms_ws = new Hashtable();
							prms_ws.Add("@TicketID", CommonUtility.Util.if_i(ticket.TicketID, 0));
							prms_ws.Add("@RequestType", (int)eWSComplianceRequestType.Requested);
							DataWSCompliance.InsertWSCompliance(prms_ws);
						}
					}
				}

			}

			//Adding notes and uploading documents
			if (ticket != null)
			{

				if (this.AddNotes(ticket))
				{
					Description.Text = string.Empty;

					chkSolution.Checked = false;
					chkAgent.Checked = false;

					FormHandler.LogTicketFormChanges(string.Empty, ticket.TicketUID, ticket.TicketID, ticket.TicketClone, ticket, "Notes Added");
				}
				else
				{
					if (lblError.Text.Trim() != string.Empty)
						return null;
				}

				if (!this.Adding && ticket.BusinessDBAName != OrigDBAName)
				{
					// dbaname was changed on ticket, so we add ticket notes to merchant notes.
					this.AddMerchantNotes(ticket);
				}

				if (!this.Adding)
				{
					// REMOVED. see task 7373
					//if (ticket.StatusID.ToUpper() == Ticket.TICKET_CLOSE)
					//{
					//    // close
					//    if (string.IsNullOrEmpty(ticket.Solution))
					//    {
					//        if (CommonUtility.Util.if_s(this.Description.Text.Trim()) == "" || chkSolution.Checked == false)
					//        {
					//            this.AddNotes(ticket, "Closed", false, true);
					//            Solution.Text = "Closed";
					//            ticket.Solution = "Closed";
					//        }
					//    }
					//}

					if (old_solution_value != Solution.Text.Trim() && old_solution_value != string.Empty)
					{
						string solution_header = "Current Solution Changed. Old Solution: ";

						this.AddNotes(ticket, solution_header + old_solution_value, false, true);
					}

					//REMOVED: As we added history for department,categroy,sub-category changes this part of adding notes is removed.
					//if ((ticket.DepartmentID != cloneTicket.DepartmentID) || (ticket.CategoryID != cloneTicket.CategoryID) || (ticket.ParentID != cloneTicket.ParentID))
					//{
					//    string parentcat = ParentID.Items.FindByValue(ticket.ParentID).Text;
					//    string cloneparentcat = LookupTableHandler.GetTicketCategoryByID(cloneTicket.ParentID);

					//    List<GenericListItem> ticketDepartments = (List<GenericListItem>)CachedLookupFacade.GetCachedTicketDepartmentListGeneric(new Hashtable { { "@ShowZeus", 1 } });
					//    List<GenericListItem> ticketCategories = (List<GenericListItem>)CachedLookupFacade.GetCachedTicketCategoryListGeneric(null, false);
					//    this.AddNotes(ticket, "Ticket has been reassigned by " + UserSessions.CurrentUser.UserName + " from " + ticketDepartments.Find(x => x.ItemValue == cloneTicket.DepartmentID).ItemText + " | " + cloneparentcat + " : " + ticketCategories.Find(x => x.ItemValue == cloneTicket.CategoryID).ItemText + " to " + ticketDepartments.Find(x => x.ItemValue == ticket.DepartmentID).ItemText + " | " + parentcat + " : " + ticketCategories.Find(x => x.ItemValue == ticket.CategoryID).ItemText + ".", false, true);
					//}

				}
			}

			User user1 = new User();
			DataUser dao = DataAccess.DataUserDao;

			if (!string.IsNullOrEmpty(userId) && !(userId.Equals("-1")) && StatusID.SelectedItem.Value.ToUpper() != Ticket.TICKET_CLOSE)
			{
				if (OrigUserID.ToUpper() != ticket.UserID.ToUpper() || OrigStatusID.ToUpper() != ticket.StatusID.ToUpper())
				{
					// only send emails if the userid is different and status in assign or pending.
					if (StatusID.SelectedItem.Value.ToUpper() == Ticket.TICKET_PENDING || StatusID.SelectedItem.Value.ToUpper() == Ticket.TICKET_ASSIGNED)
					{
						PaymentXP.Facade.TicketNotification.TicketAssignment(ticket.TicketUID, FTEmail);
						ZeusWeb.Logging.EmailLog.InfoFormat("Assigning ticket of : {0} . Email sent to: {1}", ticket.TicketUID, FTEmail);
					}
				}
			}

			if (StatusID.SelectedItem.Value.ToUpper() == Ticket.TICKET_CLOSE && sendClosed) //closed status
			{
				PaymentXP.Facade.TicketNotification.TicketClosed(ticket.TicketUID, chkAgent.Checked || AlertEnable, FTEmail);
				ZeusWeb.Logging.EmailLog.InfoFormat("Closing ticket of : {0} . Email sent to: {1}", ticket.TicketUID, FTEmail);

				// if we close a ticket, then attention is no longer required.
				DataTicket dataT = DataAccess.DataTicketDao;
				dataT.UpdateTicket_IsFeedBackRequired(UserSessions.CurrentTicket.TicketUID, false);
			}
			else if (this._IsSaveClose)
			{
				// REMOVED: see task: 7376
				//if (CommonUtility.Util.if_s(this.Solution.Text.Trim()) == string.Empty && CommonUtility.Util.if_s(ticket.Solution.Trim()) == string.Empty)
				//{
				//    Solution.Text = "Closed";
				//    ticket.Solution = "Closed";
				//}
				//this.AddNotes(ticket, "Closed", false, true);

				ticket.StatusID = Ticket.TICKET_CLOSE;
				DataTicket dataT = DataAccess.DataTicketDao;
				dataT.UpdateTicket(ticket, UserSessions.CurrentUser.TimeZone);

				PaymentXP.Facade.TicketNotification.TicketClosed(ticket.TicketUID, chkAgent.Checked || AlertEnable, FTEmail);
				ZeusWeb.Logging.EmailLog.InfoFormat("Closing ticket of : {0} . Email sent to: {1}", ticket.TicketUID, FTEmail);

			}
			else if (StatusID.SelectedItem.Value.ToUpper() == Ticket.TICKET_CLOSE)
			{
				PaymentXP.Facade.TicketNotification.TicketClosed(ticket.TicketUID, chkAgent.Checked || AlertEnable, FTEmail);
				ZeusWeb.Logging.EmailLog.InfoFormat("Closing ticket of : {0} . Email sent to: {1}", ticket.TicketUID, FTEmail);
			}

			//Start Code added for PXP-15331
			string currentTicketStatus = StatusID.SelectedItem.Value.ToUpper();
			if (this._IsSaveClose)
			{
				currentTicketStatus = ticket.StatusID;
			}
			if (OrigStatusID.ToUpper() != ticket.StatusID.ToUpper() && (currentTicketStatus == Ticket.TICKET_CLOSE && !string.IsNullOrEmpty(ticket.MerchantAppUID) && ticket.DepartmentID == "33" && ticket.ParentID == "2120" && ticket.CategoryID == "2128"))
			{
				DataAccess.DataMerchantAppDao.UpdateMerchantWithAgencyFlag(ticket.MerchantAppUID, true);
			}
			//End Code added for PXP-15331


			//REMOVED: As we added history for duedate, we are removign the feature to add notes
			//if (istrue && cloneTicket != null && ticket.DueDate != cloneTicket.DueDate)
			//{
			//    string clonetickDate = (cloneTicket != null && cloneTicket.DueDate != null && cloneTicket.DueDate != DateTime.MinValue && cloneTicket.DueDate.Year != 0001) ? cloneTicket.DueDate.ToString("MM/dd/yyy HH:mm tt") : "null";
			//    string tickDate = (ticket.DueDate != null && ticket.DueDate != DateTime.MinValue && ticket.DueDate.Year != 0001) ? ticket.DueDate.ToString("MM/dd/yyy HH:mm tt") : "null";
			//    DateTime dtClonetickDate;
			//    DateTime.TryParse(clonetickDate, out dtClonetickDate);

			//    DateTime dttickDate;
			//    DateTime.TryParse(tickDate, out dttickDate);

			//    if (clonetickDate != tickDate)
			//        AddNotes(ticket, "Due Date changed from " + CommonUtility.DateTimeUtil.ConvertUserTimeToPST(dtClonetickDate, this.TimeZoneID) + " PST to " + CommonUtility.DateTimeUtil.ConvertUserTimeToPST(dttickDate, this.TimeZoneID) + " PST. Change Reason: " + ChangeReason.Text + ".", false, false);

			//}


			pnlChange.Attributes.Add("style", "display:none;");
			pblChangeText.Attributes.Add("style", "display:none;");

			return ticket;
		}
		catch (Exception exc)
		{
			throw exc;
		}


	}

	public override bool FormDataCheck()
	{
		string message = string.Empty;


		if (DepartmentID.SelectedIndex == 0)
		{
			message += "Please select a department." + Environment.NewLine;
		}

		if (ParentID.SelectedIndex == 0)
		{
			message += "Please select a category." + Environment.NewLine;
		}

		if (CategoryID.SelectedIndex == 0)
		{
			message += "Please select a sub-category." + Environment.NewLine;
		}

		if (StatusID.SelectedIndex == 0)
		{
			message += "Please select a status." + Environment.NewLine;
		}

		if (StatusID.SelectedIndex > 0 && (StatusID.SelectedValue.ToUpper().Equals("0AEE2CAB-CEC4-476B-9598-918DBABD43CF") || StatusID.SelectedValue.ToUpper().Equals("DFF04FF8-3C47-45E1-B0BB-30C22C7CAF17")))
		{
			if (UserID.SelectedIndex == 0)
			{
				message += "Please select a user." + Environment.NewLine;
			}
		}


		if (ddlTicketSource.SelectedValue == "a" && AgentUID.Value == "")
		{
			message += "If Ticket Source is set to 'Partner', then you must select a partner." + Environment.NewLine;
		}

		if (ddlTicketSource.SelectedValue == "m")
		{
			if (String.IsNullOrEmpty(MerchantAppUID.Value))
			{
				message += "If Ticket Source is set to 'Merchant', then you must select a DBA Name." + Environment.NewLine;
			}
		}

		if (ddlTicketSource.SelectedValue == "x")
		{
			if (String.IsNullOrEmpty(MerchantAppUID.Value))
			{
				message += "If Ticket Source is set to 'PaymentXP', then you must select a DBA Name." + Environment.NewLine;
			}
		}

		if (String.IsNullOrEmpty(Problem.Text))
		{
			message += "Please enter an Issue." + Environment.NewLine;
		}

		if (String.IsNullOrWhiteSpace(MLEName.Text.Trim()) && IsMLETicket.Checked == true)
		{
			message += "Please select MLE." + Environment.NewLine;
		}

		if (string.IsNullOrWhiteSpace(DueDate.Text) && (StatusID.SelectedValue.ToUpper() != Ticket.TICKET_CLOSE && !this._IsSaveClose))
		{
			message += "Please enter a Due Date." + Environment.NewLine;
		}

		if (!string.IsNullOrWhiteSpace(DueDate.Text))
		{
			DateTime dtDueDate;
			DateTime.TryParseExact(DueDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDueDate);

			if (CommonUtility.Util.if_date(dtDueDate.ToString("MM/dd/yyyy"), DateTime.MinValue) != DateTime.MinValue)
			{
				if (DueDateTime.SelectedValue == "-1")
					message += "Time is missing for Due Date." + Environment.NewLine;

				DateTime dt = CommonUtility.Util.if_date(dtDueDate.ToString("MM/dd/yyyy") + " " + DueDateTime.SelectedValue.Replace("-1", ""), DateTime.MinValue);

				if (this.Adding && dt <= DateTime.Now)
				{
					message += "Due Date cannot be less than current date time." + Environment.NewLine;
				}
			}
			else
			{
				message += "Please enter a valid Due Date." + Environment.NewLine;
			}

		}

		if (!string.IsNullOrWhiteSpace(CallbackDate.Text))
		{
			DateTime dtCallbackDate;
			DateTime.TryParseExact(CallbackDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtCallbackDate);

			if (CommonUtility.Util.if_date(dtCallbackDate.ToString("MM/dd/yyyy"), DateTime.MinValue) == DateTime.MinValue)
				message += "Please enter a valid Callback Date." + Environment.NewLine;
		}

		if (String.IsNullOrWhiteSpace(OfficeID.SelectedValue))
		{
			message += "Please select an Office ID." + Environment.NewLine;
		}
		//Fady Massoud 12-22-2020
		//PXP-15777 Scavenger Removal
		//if (this.ddlTicketSource.SelectedValue == "e" && this.ChkSender.Checked && !string.IsNullOrEmpty(EmailOutFromEmail.Text))
		//{

		//    if (CommonUtility.Util.IsValidEmail(EmailOutFromEmail.Text))
		//    {
		//        string[] emails = EmailOutFromEmail.Text.Split(new char[] { ';', ',' });
		//        if (emails.Length > 1)
		//        {
		//            lblError.Text = "You can only have one email address in the Email From field.";
		//            return false;
		//        }
		//    }

		//    else
		//    {
		//        lblError.Text = "Email From is Invalid.";
		//        return false;
		//    }
		//}

		// in english, when trying to close a ticket, you:
		//   1) must have a solution or
		//   2) have a description and having it set as solution
		if (_IsSaveClose || StatusID.SelectedValue.ToUpper() == Ticket.TICKET_CLOSE)
		{
			//if (!string.IsNullOrWhiteSpace(Solution.Text) || (!string.IsNullOrWhiteSpace(this.Description.Text) && chkSolution.Checked == true))
			//{
			//    // all good!
			//    // note: i know we can just negate this logic, but it confuses me when i read it. i think this way is clearer to understand.
			//}
			//else
			//{
			//    lblError.Text = "Ticket must have a solution.";
			//    return false;
			//}
			//Fady Massoud 12-28-2020
			//PXP-15841 Bug Not able to close the ticket even solution note is already been added to the ticket
			if (string.IsNullOrEmpty(UserSessions.CurrentTicket.Solution))
			{
				message += "Ticket must have a solution.";
			}
		}

		if (message == string.Empty)
		{
			return true;
		}
		else
		{
			lblError.Text = message.Replace(Environment.NewLine, "<br/>"); ;
			return false;
		}
	}

	private void FillDropDown()
	{
		LookupTableHandler.MerchantAppStatus(StatusID, false, "Ticket");
		StatusID.Enabled = true;

		this.ddlTicketSource_SelectedIndexChanged(ddlTicketSource, null);

		LookupTableHandler.LoadActiveInternalUsers(UserID, false);
		LookupTableHandler.LoadChangeReason(ChangeReasonID, false);

		LookupTableHandler.LoadTime1(DueDateTime);
		DueDateTime.Items.Insert(0, new ListItem("-- Select --", "-1"));

		//Fady Massoud 12-22-2020
		//PXP-15781 Document Upload
		//LookupTableHandler.LoadMDocType(lstDocumentTypes, false, MDoc.eMDocTypeGroup.Ticket);

		//code added by koshlendra for PXP-7622 start
		//LookupTableHandler.LoadMDocType(ddlDocumentType, false, MDoc.eMDocTypeGroup.Ticket);
		//code added by koshlendra for PXP-7622 end
		LookupTableHandler.LoadPrivateLabels(PrivateLabelUID, false);

		//Fady Massoud 12-22-2020
		//History
		//LookupTableHandler.LoadChangeHistoryFields(ddlChangeType, true, ChangeHistoryFields.ChangeHistoryFieldSource.Ticket);
	}

	public void LoadDocuments(Ticket t)
	{
		Hashtable prms = new Hashtable();

		prms.Add("@PrimaryKeyID", t.TicketID);
		prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.Tickets);
		prms.Add("@UserUID", UserSessions.CurrentUser.UID);

		List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);

		if (li != null && li.Count > 0)
		{
			gvFile.DataSource = li;
			gvFile.DataBind();
		}
	}

	public bool DocUploadUpdateUserID(int DocumentID, string UserName)
	{
		Hashtable prms = new Hashtable();

		prms.Add("@DocumentID", DocumentID);
		prms.Add("@UserName", UserName);

		return DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);

	}

	//private void LoadStatus(Ticket t)
	//{
	//    this.LoadTicketStatus(t);
	//    statusHistory.Enabled = true;
	//}

	//private void LoadTicketStatus(Ticket t)
	//{

	//    List<TicketStatusHistory> liTSH = t.StatusHistory2;
	//    statusHistory.Items.Clear();

	//    foreach (TicketStatusHistory tsh in liTSH)
	//    {
	//        string text = string.Format("{0}: {3} - {1} ({2})", WebUtil.ConvertToUserDateTimeSettings(tsh.StatusChangedDate.ToString()), tsh.UserName, tsh.UserCreated, tsh.Status);
	//        statusHistory.Items.Add(new ListItem(text));
	//    }
	//}

	public void SendMail(string sendTo, string body, string ticketText)
	{
		bool perform = FormHandler.SendEmail(ticketText, body, body, UserSessions.CurrentUser.Email, sendTo, "", "", new Hashtable(), UserSessions.CurrentTicket.MerchantAppUID);
	}

	/// <summary>
	/// uses whats in the textbox as the description value
	/// </summary>
	/// <returns></returns>
	private bool AddNotes(Ticket t)
	{

		Ticket ticket = t;
		string desc = string.Empty;

		bool ret = false;

		//TextPlain is used only to check if the description is empty, do not use this to save the data to the DB.
		if (Server.HtmlEncode(Description.TextPlain.Trim()) != string.Empty)
		{
			//Replace the new line here with empty space because infragistics control is adding a line break and also a new line charechter.
			//This is duplicated in row data bounds when we replace a new line with line break, and due to which space gets doubled.
			//We are replacing new line char with line break for teh sake of scavenger text which sends only \n chars.
			ret = this.AddNotes(t, desc + Description.Text.Replace("\n", "").Replace("&nbsp;", " ").Trim(), false, false);
		}

		return ret;
	}

	private bool AddNotes(Ticket t, string my_description, bool IsReqAtt, bool isSystemGenerated)
	{

		Ticket ticket = t;
		Ticket ticketClone = t.TicketClone;
		if (my_description == string.Empty)
		{
			return false;
		}
		try
		{
			IList<TicketNotes> tNotes = ticket.TicketNotes;

			if (tNotes != null)
			{
				for (int i = 0; i < tNotes.Count; i++)
				{
					if (my_description.Trim().Equals(tNotes[i].Description.Trim()))
					{
						lblError.Text = "Same note already exists.";
						return false;
					}
				}
			}

			if (chkSolution.Checked)
			{
				if ((ticketClone != null && ticketClone.StatusID.ToUpper() == Ticket.TICKET_CLOSE.ToUpper()) || (ticketClone == null && ticket.StatusID.ToUpper() == Ticket.TICKET_CLOSE.ToUpper()))
				{
					lblError.Text = "Solution cannot be added for a closed ticket.";
					return false;
				}
			}
			//Fady Massoud 12-22-2020
			//PXP-15777 Scavenger Removal
			//if (ticket.TicketSource == "e" && this.ChkSender.Checked && !string.IsNullOrEmpty(EmailOutFromEmail.Text))
			//{
			//    if (CommonUtility.Util.IsValidEmail(EmailOutFromEmail.Text))
			//    {
			//        string[] emails = EmailOutFromEmail.Text.Split(new char[] { ';', ',' });
			//        if (emails.Length > 1)
			//        {
			//            lblError.Text = "You can only have one email address in the Email From field.";
			//            return false;
			//        }
			//    }
			//    else
			//    {
			//        lblError.Text = "Email From is Invalid.";
			//        return false;
			//    }
			//}

			// we set the column "IsFeedBackRequired" in the tickets table from the add notes functionality. reason why we do this is we want
			// this flagged in the ticket table, but we dont want to do a blank ticket updated because people might not be ready
			// to commit other fields yet. we just want to set this flag alone.
			DataTicket dataT = DataAccess.DataTicketDao;
			dataT.UpdateTicket_IsFeedBackRequired(t.TicketUID, IsReqAtt);
			ticket = t;
			ticket.AttentionReq = true;

			if (chkSolution.Checked)
			{
				ticket.Solution = my_description;
			}
			//Fady Massoud 12-22-2020
			//PXP-15777 Scavenger Removal
			//if (ChkSender.Checked)
			//{
			//    ticket.EmailOutToEmail = this.EmailOutToEmail.Text;
			//    ticket.EmailOutCCEmail = this.EmailOutCCEmail.Text;
			//}
			//ticket.EmailOutFromEmail = this.EmailOutFromEmail.Text;

			int rows = dataT.UpdateTicket(ticket, this.TimeZoneID);

			if (rows > 0)
			{
				FormHandler.LogTicketFormChanges(string.Empty, ticket.TicketUID, ticket.TicketID, ticket.TicketClone, ticket, string.Empty);
			}

			string subject = "";
			if (t != null && (!string.IsNullOrEmpty(t.MerchantAppUID) || !string.IsNullOrEmpty(t.MLEName)))
			{
				subject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nNote"
					, DepartmentID.SelectedItem.Text.ToString()
					, ParentID.SelectedItem.Text.Trim()
					, CategoryID.SelectedItem.Text.Trim()
					);
			}
			////PXP-14251 - Fix Zeus Ticket Notes displaying Blank HTML Page 
			my_description = Server.HtmlEncode(my_description.Trim());
			if (!this.IsMLETicket.Checked)
			{
				//Fady Massoud 12-22-2020
				//PXP-15777 Scavenger Removal
				//return TicketNotification.AddNotes(ticket, my_description, IsReqAtt, isSystemGenerated, chkAgent.Checked, ChkMerchant.Checked, ChkSender.Checked, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject);
				return TicketNotification.AddNotes(ticket, my_description, IsReqAtt, isSystemGenerated, chkAgent.Checked, ChkMerchant.Checked, false, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject);
			}

			else
			{
				//Fady Massoud 12-22-2020
				//PXP-15777 Scavenger Removal
				//return TicketNotification.AddMLENotes(ticket, my_description, ChkSender.Checked, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, true);
				return TicketNotification.AddMLENotes(ticket, my_description, false, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, true);
			}

		}
		catch (Exception exc)
		{
			throw exc;
		}
	}

	/// <summary>
	/// Collects the full ticket notes history and adds to merchant notes.
	/// </summary>
	private void AddMerchantNotes(Ticket t)
	{
		if (!MerchantAppUID.Value.Equals(string.Empty))
		{
			string subject = string.Empty;
			StringBuilder sb = new StringBuilder();

			foreach (GridViewRow grdRow in grdTicketNotes.Rows)
			{
				Literal lit = (Literal)grdRow.FindControl("litBody");
				Literal says = (Literal)grdRow.FindControl("litUserCreated");
				if (lit != null && says != null)
				{
					//I am encoding the text because our aim is to encode the value and save.
					//No matter where the note is coming from, we will have to encode the text and save in the DB.
					sb.Append(string.Format("{0}: {1};\n", says.Text.Trim(), Server.HtmlEncode(lit.Text.Trim())));
				}
			}

			//string mysolution = "";

			//if (Solution.Text.Trim() != "")
			//{
			//    mysolution = string.Format("Solution: {0}\n", Solution.Text.Trim());
			//}

			//ticketNotes = string.Format("TicketID: {0}\nProblem: {1}\n{2}Notes: \n{3}"
			//    , t.TicketID.ToString()
			//    , Problem.Text.Trim()
			//    , mysolution
			//    , sb.ToString()
			//    );

			subject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nNotes"
				, DepartmentID.SelectedItem.Text.ToString()
				, ParentID.SelectedItem.Text.Trim()
				, CategoryID.SelectedItem.Text.Trim()
				);

			TicketNotification.AddMerchantNoteNote(t.TicketID
				, MerchantAppUID.Value
				, UserSessions.CurrentUser.UserName
				, sb.ToString()
				, subject
				, chkAgent.Checked
				, true//chkBank.Checked
				, ChkMerchant.Checked
				);
		}
	}

	private void AddAgentNotes(Ticket t)
	{
		//Add agent note only when it is purely a agent ticket.
		if (!AgentUID.Value.Equals(string.Empty) && String.IsNullOrEmpty(MerchantAppUID.Value.ToString()))
		{
			StringBuilder sb = new StringBuilder();

			foreach (GridViewRow grdRow in grdTicketNotes.Rows)
			{
				Literal lit = (Literal)grdRow.FindControl("litBody");
				Literal says = (Literal)grdRow.FindControl("litUserCreated");
				if (lit != null && says != null)
				{
					//I am encoding the text because our aim is to encode the value and save.
					//No matter where the note is coming from, we will have to encode the text and save in the DB.
					sb.Append(string.Format("{0}: {1};\n", says.Text.Trim(), Server.HtmlEncode(lit.Text.Trim())));
				}
			}


		}
	}

	private void LoadNotes(int ticket_id)
	{
		if (ticket_id > 0)
		{
			DataTicketNotes data = DataAccess.DataTicketNotesDao;
			//DataSet ds = data.GetTicketNotes(null, UserSessions.CurrentTicket.TicketID, null, true);
			//grdTicketNotes.DataSource = ds;

			// we pass the params as a hash so that we can pull the notes as a List<> object.
			Hashtable htParms = new Hashtable();
			htParms.Add("@UID", null);
			htParms.Add("@TicketID", ticket_id);
			htParms.Add("@TicketNoteID", null);
			htParms.Add("@SortDirection", 0);

			IList<TicketNotes> li = data.GetTicketNotes(htParms);
			list = new List<TicketNoteModel>();
			foreach (TicketNotes tn in li)
			{
				//This value is being decoded here as we want to send to  the grid the right values which can be bound.
				//When note is added from the infragistics controls, the notes are added as encoded values 
				//and potentially dangerous scripts are encoded twice.
				//PXP-14251 - Fix Zeus Ticket Notes displaying Blank HTML Page 
				tn.Description = CommonUtility.Formatting.nl2br(System.Web.HttpUtility.HtmlDecode(tn.Description));

				list.Add(new TicketNoteModel (tn));
			}

			grdTicketNotes.DataSource = li;
			grdTicketNotes.DataBind();


		}
	}

	private void BindNotes(int TicketID)
	{

		this.LoadNotes(TicketID);
		//// we want to htmlencode the user input first before displaying on screen.
		//IList<TicketNotes> li = UserSessions.CurrentTicket.TicketNotes;
		//foreach (TicketNotes tn in li)
		//{
		//    tn.Description = HttpUtility.HtmlEncode(tn.Description);
		//}

		//grdTicketNotes.DataSource = li;
		//grdTicketNotes.DataBind();
	}

	public override void ToggleButtons()
	{
		btnEdit.Enabled = !btnEdit.Enabled;
		btnAdd.Enabled = !btnAdd.Enabled;
		myclick.Enabled = !myclick.Enabled;
		btnRefresh.Enabled = !btnRefresh.Enabled;
		btnCancel.Enabled = !btnCancel.Enabled;
		btnSaveClose.Enabled = !btnSaveClose.Enabled;
		clickClose.Enabled = this.EditMode;
		//TabControl.Tabs[1].Enabled = !this.EditMode;
		btnClone.Enabled = !this.EditMode;
	}

	private Ticket HandleSaveButton()
	{
		bool is_problem_changed = false;

		bool is_add_problem = false;

		if (this.Adding)
		{
			is_add_problem = true;
		}
		else if (UserSessions.CurrentTicket != null)// && this.Adding == false)
		{
			is_problem_changed = (Problem.Text.Trim() != UserSessions.CurrentTicket.Problem) ? true : false;
		}

		Ticket t = this.FormSaveTicket();

		if (t != null && CommonUtility.Util.if_i(t.TicketID, 0) > 0)
		{
			this.UID = t.TicketUID;

			if (CommonUtility.Util.IsValidGuid(t.MerchantAppUID) || !string.IsNullOrEmpty(t.MLEName))
			{

				string subject = string.Empty;

				if (is_add_problem)
				{
					subject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nProblem"
						  , DepartmentID.SelectedItem.Text.ToString()
						  , ParentID.SelectedItem.Text.Trim()
						  , CategoryID.SelectedItem.Text.Trim()
						  );
					if (!t.IsMLETicket)
					{
						TicketNotification.AddMerchantNoteProblem(t.TicketID
							, t.MerchantAppUID
							, UserSessions.CurrentUser.UserName
							, t.Problem
							, subject
							, chkAgent.Checked
							, true);
					}

					else
					{
						//Fady Massoud 12-22-2020
						//PXP-15777 Scavenger Removal
						//TicketNotification.AddMLENotes(t, "Issue: " + t.Problem, ChkSender.Checked, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
						TicketNotification.AddMLENotes(t, "Issue: " + t.Problem, false, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
					}
				}

				if (is_problem_changed)
				{
					subject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nProblem"
						  , DepartmentID.SelectedItem.Text.ToString()
						  , ParentID.SelectedItem.Text.Trim()
						  , CategoryID.SelectedItem.Text.Trim()
						  );

					if (!t.IsMLETicket)
					{
						TicketNotification.AddMerchantNoteProblem(t.TicketID
							, t.MerchantAppUID
							, UserSessions.CurrentUser.UserName
							, Problem.Text.Trim()
							, subject
							 , chkAgent.Checked
							, true);
					}

					else
					{
						//Fady Massoud 12-22-2020
						//PXP-15777 Scavenger Removal
						//TicketNotification.AddMLENotes(t, "Issue: " + t.Problem, ChkSender.Checked, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
						TicketNotification.AddMLENotes(t, "Issue: " + t.Problem, false, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
					}
				}

				if (t.StatusID.ToUpper() == Ticket.TICKET_CLOSE)
				{
					subject = string.Format("Department: {0}\nCategory: {1}\nSubcategory: {2}\nSolution"
						  , DepartmentID.SelectedItem.Text.ToString()
						  , ParentID.SelectedItem.Text.Trim()
						  , CategoryID.SelectedItem.Text.Trim()
						  );

					if (!t.IsMLETicket)
					{
						TicketNotification.AddMerchantNoteSolution(t.TicketID
							, t.MerchantAppUID
							, UserSessions.CurrentUser.UserName
							, t.Solution
							, subject
							, chkAgent.Checked
							, true);
					}
					else
					{
						//Fady Massoud 12-22-2020
						//PXP-15777 Scavenger Removal
						// TicketNotification.AddMLENotes(t, "Solution: " + t.Solution, ChkSender.Checked, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
						TicketNotification.AddMLENotes(t, "Solution: " + t.Solution, false, chkSolution.Checked, cbEmailFT.Checked, UserSessions.CurrentUser, subject, false);
					}
				}
			}

			//Add agent note only when it is purely a agent ticket.
			if (t != null && !string.IsNullOrEmpty(t.AgentUID) && string.IsNullOrEmpty(t.MerchantAppUID))
			{
				string subject = string.Empty;

				if (is_add_problem || is_problem_changed)
				{
					TicketNotification.AddAgentNote(t.TicketID
					, t.AgentUID
					, UserSessions.CurrentUser.UserName
					, "Issue: " + t.Problem //Do not encode as this was already encoded and saved.
					, Constants.AGENTNOTECODE_TICKET
					, true
					, ChkMerchant.Checked
					);

				}

				if (t.StatusID.ToUpper() == Ticket.TICKET_CLOSE)
				{

					TicketNotification.AddAgentNote(t.TicketID
				   , t.AgentUID
				   , UserSessions.CurrentUser.UserName
				   , "Solution: " + t.Solution //Do not encode as this was already encoded and saved.
				   , Constants.AGENTNOTECODE_TICKET
				   , true
				   , ChkMerchant.Checked
				   );
				}


			}
			//Fady Massoud 12-22-2020
			//PXP-15781 Document Upload
			//code added by koshlendra for PXP-7622 start
			//if (this.Adding == true)
			//    UploadDocuments(t);
			//code added by koshlendra for PXP-7622 end



			this.Adding = false;
			this.EditMode = false;
			this.ToggleButtons();
			string myurl = string.Empty;

		}
		else
		{
			if (_IsSaveClose)
			{
				clickClose.Enabled = true;
			}
			else
			{
				myclick.Enabled = true;
			}
		}

		return t;
	}

	//private string ConvertTicketSourceToString(string s)
	//{
	//    string ret = "i";
	//    switch (s)
	//    {
	//        case "a":
	//            ret = "Agent";
	//            break;

	//        case "m":
	//            ret = "Merchant";
	//            break;

	//        case "x":
	//            ret = "PaymentXP";
	//            break;

	//        default:
	//            ret = "Internal";
	//            break;
	//    }
	//    return ret;

	//}

	protected void CategoryID_SelectedIndexChanged(object sender, EventArgs e)
	{
		//PXP-13364 allow only when Deployment > Account Closure > Retention Closure/Closures
		//Bug fix PXP-15096
		if (UserSessions.CurrentUser != null && !string.IsNullOrEmpty(UserSessions.CurrentUser.UserID))
		{
			if (m_TicketApprovalManager != null)
			{
				bool isTicketApprovalManager = m_TicketApprovalManager.Any(ApprovalManagerUsers => ApprovalManagerUsers.ItemValue == UserSessions.CurrentUser.UserID);
				if (isTicketApprovalManager)
					chkManagerApproval.Enabled = true;
				else
					chkManagerApproval.Enabled = false;
			}
		}

	}

	protected void ddlTicketSource_SelectedIndexChanged(object sender, EventArgs e)
	{
		DropDownList ddl = (DropDownList)sender;

		switch (ddl.SelectedValue)
		{
			case "a": //apex

				//cbIsFeedBackRequired.Visible = true;
				//cbIsFeedBackRequired.Checked = true;
				//cbIsFeedBackRequired.Text = "Email Partner";

				// cbMailAgent.Visible = false;
				chkAgent.Checked = true;
				ChkMerchant.Checked = false;
				if (string.IsNullOrEmpty(BusinessDBAName.Text))
				{
					ChkMerchant.Enabled = false;
				}
				LookupTableHandler.LoadAgentTicketDepartments(DepartmentID, false);

				pnlCategory.Visible = false;
				pnlSubCategory.Visible = false;

				int departmentid = -1;
				if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.DepartmentID != "" && this.Adding == false)
				{
					departmentid = CommonUtility.Util.if_i(UserSessions.CurrentTicket.DepartmentID, -1);
					if (DepartmentID.Items.FindByValue(departmentid.ToString()) != null)
					{
						DepartmentID.SelectedValue = departmentid.ToString();
					}
				}

				if (departmentid == -1)
				{
					pnlCategory.Visible = false;
					pnlSubCategory.Visible = false;
				}
				else
				{
					LookupTableHandler.LoadTicketCategories(ParentID, false, departmentid, "a", "0");
					//LookupTableHandler.LoadCategories(ParentID, false, departmentid, "a");

					if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.ParentID != "" && this.Adding == false)
					{
						pnlCategory.Visible = true;
						pnlSubCategory.Visible = true;

						if (ParentID.Items.FindByValue(UserSessions.CurrentTicket.ParentID) != null)
						{
							ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;
							//LookupTableHandler.LoadSubCategories(CategoryID, false, departmentid, "a", ParentID.SelectedValue);
							LookupTableHandler.LoadTicketCategories(CategoryID, false, departmentid, "a", ParentID.SelectedValue);
						}
						else
						{
							ParentID.SelectedValue = "-1";
						}
					}

				}

				break;

			case "m": //insight

				//cbIsFeedBackRequired.Visible = true;
				//cbIsFeedBackRequired.Checked = false;
				//cbIsFeedBackRequired.Text = "Email Merchant";

				//cbMailAgent.Visible = true;
				ChkMerchant.Checked = true;
				ChkMerchant.Enabled = true;
				chkAgent.Checked = false;
				//  chkAgent.Checked = (ticket.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");

				LookupTableHandler.LoadMerchantTicketDepartments(DepartmentID, false);
				//DepartmentID.Enabled = true;

				pnlCategory.Visible = false;
				pnlSubCategory.Visible = false;

				departmentid = -1;
				if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.DepartmentID != "" && this.Adding == false)
				{
					departmentid = CommonUtility.Util.if_i(UserSessions.CurrentTicket.DepartmentID, -1);
					if (departmentid != -1)
					{
						if (DepartmentID.Items.FindByValue(departmentid.ToString()) != null)
						{
							DepartmentID.SelectedValue = departmentid.ToString();
						}
					}
				}

				if (departmentid == -1)
				{
					pnlCategory.Visible = false;
					pnlSubCategory.Visible = false;
				}
				else
				{
					LookupTableHandler.LoadTicketCategories(ParentID, false, departmentid, "m", "0");
					//LookupTableHandler.LoadCategories(ParentID, false, departmentid, "m");

					if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.ParentID != "" && this.Adding == false)
					{
						pnlCategory.Visible = true;
						pnlSubCategory.Visible = true;

						if (ParentID.Items.FindByValue(UserSessions.CurrentTicket.ParentID) != null)
						{
							ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;
							//LookupTableHandler.LoadSubCategories(CategoryID, false, departmentid, "m", ParentID.SelectedValue);
							LookupTableHandler.LoadTicketCategories(CategoryID, false, departmentid, "m", ParentID.SelectedValue);
						}
						else
						{
							ParentID.SelectedValue = "-1";
						}
					}

				}

				break;

			case "x": //payment xp

				//cbIsFeedBackRequired.Visible = true;
				//cbIsFeedBackRequired.Checked = false;
				//cbIsFeedBackRequired.Text = "Email Merchant";

				// cbMailAgent.Visible = true;
				ChkMerchant.Checked = true;
				ChkMerchant.Enabled = true;
				chkAgent.Checked = false;
				// chkAgent.Checked = (ticket.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");
				LookupTableHandler.LoadPaymentXPTicketDepartments(DepartmentID, false);
				//DepartmentID.Enabled = true;

				pnlCategory.Visible = false;
				pnlSubCategory.Visible = false;

				departmentid = -1;
				if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.DepartmentID != "" && this.Adding == false)
				{
					departmentid = CommonUtility.Util.if_i(UserSessions.CurrentTicket.DepartmentID, -1);
					if (departmentid != -1)
					{
						if (DepartmentID.Items.FindByValue(departmentid.ToString()) != null)
						{
							DepartmentID.SelectedValue = departmentid.ToString();
						}
					}
				}

				if (departmentid == -1)
				{
					pnlCategory.Visible = false;
					pnlSubCategory.Visible = false;
				}
				else
				{
					LookupTableHandler.LoadTicketCategories(ParentID, false, departmentid, "x", "0");
					//LookupTableHandler.LoadCategories(ParentID, false, departmentid, "x");

					if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.ParentID != "" && this.Adding == false)
					{
						pnlCategory.Visible = true;
						pnlSubCategory.Visible = true;

						if (ParentID.Items.FindByValue(UserSessions.CurrentTicket.ParentID) != null)
						{
							ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;
							//LookupTableHandler.LoadSubCategories(CategoryID, false, departmentid, "x", ParentID.SelectedValue);
							LookupTableHandler.LoadTicketCategories(CategoryID, false, departmentid, "x", ParentID.SelectedValue);
						}
						else
						{
							ParentID.SelectedValue = "-1";
						}
					}

				}

				break;

			case "i": //zeus

				//cbIsFeedBackRequired.Visible = true;
				//cbIsFeedBackRequired.Checked = false;
				//cbIsFeedBackRequired.Text = "Email Merchant";

				ChkMerchant.Checked = false;
				ChkMerchant.Enabled = true;
				chkAgent.Checked = false;

				//  cbMailAgent.Visible = true;
				//chkAgent.Checked = (ticket.AgentTypeUID.ToUpper() == "9CCD5194-464C-49B1-B4F3-47D4B26251C9");

				LookupTableHandler.LoadDepartments(DepartmentID, false);
				//DepartmentID.Enabled = true;
				pnlCategory.Visible = false;
				pnlSubCategory.Visible = false;

				if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.DepartmentID != "" && this.Adding == false)
				{
					DepartmentID.SelectedValue = UserSessions.CurrentTicket.DepartmentID;
				}


				//LookupTableHandler.LoadCategories(ParentID, false);

				if (DepartmentID.SelectedValue == "-1")
				{
					pnlCategory.Visible = false;
					pnlSubCategory.Visible = false;
				}
				else
				{
					LookupTableHandler.LoadTicketCategories(ParentID, false, CommonUtility.Util.if_i(DepartmentID.SelectedValue, -1), "i", "0");

					if (UserSessions.CurrentTicket != null && UserSessions.CurrentTicket.ParentID != "" && this.Adding == false)
					{
						pnlCategory.Visible = true;
						pnlSubCategory.Visible = true;

						if (ParentID.Items.FindByValue(UserSessions.CurrentTicket.ParentID) != null)
						{
							ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;
							//LookupTableHandler.LoadSubCategories(CategoryID, false, -1, "i", ParentID.SelectedValue);
							LookupTableHandler.LoadTicketCategories(CategoryID, false, -1, "i", ParentID.SelectedValue);
						}
						else
						{
							ParentID.SelectedValue = "-1";
						}
					}
				}

				break;

			case "e":

				break;
		}
	}

	protected void DepartmentID_SelectedIndexChanged(object sender, EventArgs e)
	{
		DropDownList ddl = (DropDownList)sender;

		int department_id = -1;

		if (ddl.SelectedValue != "-1" && ddl.SelectedValue != "")
		{
			department_id = CommonUtility.Util.if_i(ddl.SelectedValue, -1);
			//LookupTableHandler.LoadCategories(ParentID, false, department_id, ddlTicketSource.SelectedValue);
			//LookupTableHandler.LoadSubCategories(CategoryID, false, department_id, ddlTicketSource.SelectedValue, ParentID.SelectedValue);

			LookupTableHandler.LoadTicketCategories(ParentID, false, department_id, ddlTicketSource.SelectedValue, "0");
			LookupTableHandler.LoadTicketCategories(CategoryID, false, department_id, ddlTicketSource.SelectedValue, ParentID.SelectedValue);

			pnlCategory.Visible = true;
			pnlSubCategory.Visible = true;

		}
		else
		{
			pnlSubCategory.Visible = false;
			pnlCategory.Visible = false;
		}

		ParentID.SelectedValue = "-1";

		if (UserSessions.CurrentTicket != null
				&& CommonUtility.Util.if_i(UserSessions.CurrentTicket.DepartmentID, 0) == department_id
				&& CommonUtility.Util.if_i(UserSessions.CurrentTicket.ParentID, -1) != -1
				&& this.Adding == false
			)
		{
			//ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;

			ListHandler.ListFindItem(ParentID, UserSessions.CurrentTicket.ParentID);
			ListHandler.ListFindItem(CategoryID, UserSessions.CurrentTicket.CategoryID);

			//if (CommonUtility.Util.if_i(UserSessions.CurrentTicket.CategoryID, -1) != -1)
			//    CategoryID.SelectedValue = UserSessions.CurrentTicket.CategoryID;
		}

		switch (ddlTicketSource.SelectedValue)
		{

			case "i":
				if (!this.IsMLETicket.Checked)
				{
					chkAgent.Enabled = string.IsNullOrEmpty(this.AgentDBA.Text) ? false : true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;
				}
				break;

			case "e":
				if (!this.IsMLETicket.Checked)
				{
					chkAgent.Enabled = string.IsNullOrEmpty(this.AgentDBA.Text) ? false : true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;
				}
				//Fady Massoud 12-22-2020
				//PXP-15777 Scavenger Removal
				//tblEmailSender.Visible = ChkSender.Checked;

				break;

			case "a":
				if (!this.IsMLETicket.Checked)
				{
					chkAgent.Checked = true;
					ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;
				}
				break;

			case "m":
				if (!this.IsMLETicket.Checked)
				{
					ChkMerchant.Checked = true;
				}
				break;

			case "x":
				if (!this.IsMLETicket.Checked)
				{
					ChkMerchant.Checked = true;
				}
				break;
		}

		this.UserID.SelectedValue = "-1";
		this.StatusID.SelectedValue = "cdcd0a20-6603-4b07-94dc-f65d01290f6b"; //Open Status
	}

	protected void ParentID_SelectedIndexChanged(object sender, EventArgs e)
	{
		DropDownList ddl = (DropDownList)sender;

		int Category_id = -1;

		if (ddl.SelectedValue != "-1" && ddl.SelectedValue != "")
		{
			Category_id = CommonUtility.Util.if_i(ddl.SelectedValue, -1);
			LookupTableHandler.LoadTicketCategories(CategoryID, false, CommonUtility.Util.if_i(DepartmentID.SelectedValue, -1), ddlTicketSource.SelectedValue, Category_id.ToString());
			pnlSubCategory.Visible = true;

		}
		else
		{
			pnlSubCategory.Visible = false;
		}

		CategoryID.SelectedValue = "-1";

		if (UserSessions.CurrentTicket != null
				&& CommonUtility.Util.if_i(UserSessions.CurrentTicket.DepartmentID, 0) != -1
				&& CommonUtility.Util.if_i(UserSessions.CurrentTicket.ParentID, -1) == Category_id
				&& this.Adding == false
			)
		{
			ParentID.SelectedValue = UserSessions.CurrentTicket.ParentID;

			if (CommonUtility.Util.if_i(UserSessions.CurrentTicket.CategoryID, -1) != -1)
				CategoryID.SelectedValue = UserSessions.CurrentTicket.CategoryID;
		}

	}

	protected void DueDate_ValueChanged(object sender, EventArgs e)
	{
		if (!this.Adding)
		{
			bool istrue = UserSessions.CurrentTicket.IsDueDateHistory;
			DateTime dtOld = DataLayer.Field2Date(UserSessions.CurrentTicket.DueDate);

			// Bug fixed by Jorge: added validation when DueDateTime.SelectedItem is null
			string time = DueDateTime.SelectedItem != null && DueDateTime.SelectedItem.Value == "-1" ? string.Empty : " " + DueDateTime.SelectedItem.Text;

			DateTime dtDueDate;
			DateTime.TryParseExact(DueDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDueDate);

			DateTime dtNew = DataLayer.Field2Date(dtDueDate.ToString("MM/dd/yyyy") + " " + time);

			if (istrue && dtOld != dtNew)
			{
				//never display change reason ,once it is saved. it only appears on the note.                
				pnlChange.Attributes.Add("style", "display:inline;");
				ChangeReasonID.SelectedIndex = -1; ChangeReason.Text = "";
			}
			else
			{
				pnlChange.Attributes.Add("style", "display:none;");
			}

		}
	}

	protected void ChangeReasonID_SelectedIndexChanged(object sender, EventArgs e)
	{
		DropDownList ddl = (DropDownList)sender;

		if (ddl.SelectedItem.Text.ToUpper() == "OTHER")
		{
			pblChangeText.Attributes.Add("style", "display:inline;");
			ChangeReason.Text = "";
		}
		else
		{
			pblChangeText.Attributes.Add("style", "display:none;");
		}

		if (ddl.SelectedValue != "-1" && ddl.SelectedItem.Text.ToUpper() != "OTHER")
		{
			ChangeReason.Text = ddl.SelectedItem.Text;
		}

	}

	public override bool FormDelete()
	{
		throw new NotImplementedException();
	}

	public override bool FormSave()
	{
		throw new NotImplementedException();
	}
	//Fady Massoud 12-22-2020
	//PXP-15777 Scavenger Removal
	//protected void ChkSender_CheckedChanged(object sender, EventArgs e)
	//{
	//    tblEmailSender.Visible = ChkSender.Checked;
	//}
	protected void lbRefresh_Click(object sender, EventArgs e)
	{
		this.LoadDocuments(UserSessions.CurrentTicket);
	}

	protected void btnMLESelect_Click(object sender, EventArgs e)
	{
		Hashtable prms = new Hashtable();
		grdMerchants.SetDataSource(prms, 10);
		dlgcontrol.Modal = false;
		dlgcontrol.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
	}

	protected void IsMLETicket_CheckedChanged(object sender, EventArgs e)
	{
		ShowMLESpeceficFields();
		this.MLEAcqBankUID.Enabled = true;
	}

	private void ShowMLESpeceficFields()
	{
		this.pnlNonMLE.Visible = !this.IsMLETicket.Checked;
		this.pnlMLE.Visible = this.IsMLETicket.Checked;
		if (UserSessions.CurrentTicket != null)
		{
			this.pnlPrivateLabel.Visible = !this.IsMLETicket.Checked && CommonUtility.Util.if_s(UserSessions.CurrentTicket.PrivateLabelUID) != "";
		}
		if (this.IsMLETicket.Checked)
		{
			this.chkAgent.Enabled = false;
			this.ChkMerchant.Enabled = false;
			OtherContact.Visible = false;
			Ticketcontact.Visible = true;
		}
		else
		{
			chkAgent.Enabled = string.IsNullOrEmpty(this.AgentDBA.Text) ? false : true;
			ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;
			if (!string.IsNullOrWhiteSpace(this.BusinessDBAName.Text))
			{
				OtherContact.Visible = true;
				Ticketcontact.Visible = false;
			}
			else
			{
				OtherContact.Visible = false;
				Ticketcontact.Visible = true;
			}

		}
	}

	protected void chkSolution_CheckedChanged(object sender, EventArgs e)
	{
		this.clickClose.Enabled = this.chkSolution.Checked;
	}

	protected void lbRemoveMerchant_Click(object sender, EventArgs e)
	{
		//eluxa 2015/06/11: this method will never get called becuase the control will never visible.
		//we're doing this because we need to roll back "US 7387: Ability to remove Merchant from Ticket"
		//Refer to CS 5957 for original implementation

		hypMerch.NavigateUrl = "";
		hypMerch.Enabled = false;


		BusinessDBAName.Text = "";
		MerchantID.Text = "";
		Bank.Text = "";
		ZID.Text = "";
		AgentDBA.Text = "";
		MerchantFMAID.Text = "";
		AgentDBA.Text = "";
		MerchantAppUID.Value = "";
		AgentUID.Value = "";
		AgentID.Value = "";//PXP-8889 Attachment Download Error
						   //reset dropdown list for ach bank
		this.BankID.SelectedIndex = 0;
		ACHID.Text = "";

		MLEName.Text = "";
		MLEAcqBankUID.SelectedIndex = 0;
		ShowMLESpeceficFields();

		DateCreated.Text = WebUtil.ConvertToUserDateTimeSettings(DateTime.Now.ToString());
		UserCreated.Text = UserSessions.CurrentUser.UserName;

		cbEmailFT.Visible = false;
		cbEmailFT.Checked = false;

		if (!this.IsMLETicket.Checked)
		{
			ChkMerchant.Visible = false;
			ChkMerchant.Enabled = false;
			chkAgent.Enabled = false;
		}

		// reset contact
		wucContact1.FormNew();
		wucContact1.ControlContactType = eControlContactType.Merchant;
		wucContact1.EditMode = true;
		wucContact1.ObjectID = 0;
		wucContact1.FormShow("", false);
		OtherContact.Visible = false;
		Ticketcontact.Visible = true;
		//Fady Massoud 12-22-2020
		//PXP-15777 Scavenger Removal
		//tblEmailSender.Visible = ChkSender.Checked;
		this.PrivateLabelUID.Enabled = true;
		this.PrivateLabelUID.SelectedIndex = 0;

		lbRemoveMerchant.Visible = false;
	}

	protected void IsPrivate_CheckedChanged(object sender, EventArgs e)
	{
		CheckBox chkPrivate = (CheckBox)sender;
		GridViewRow grdRow = (GridViewRow)chkPrivate.NamingContainer;
		HiddenField hDocID = (HiddenField)grdRow.FindControl("HidDocID");
		if (!string.IsNullOrWhiteSpace(hdnDocuments.Value))
		{
			int DocID = CommonUtility.Util.if_i(hDocID.Value, 0);

			if (DocID > 0)
			{
				DataAccess.DataDocumentsDao.UpdatePrivateMDoc(DocID, chkPrivate.Checked);
				hdnDocuments.Value = string.Empty;
				LoadDocuments(UserSessions.CurrentTicket);
			}
		}
	}

	protected void ddlTicketTemplate_SelectedIndexChanged(object sender, EventArgs e)
	{
		List<TicketTemplate> _TicketTemplates = LookupTableHandler.GetTicketTemplates();

		TicketTemplate _CurrentTicketTemplate = _TicketTemplates.Where(tt => tt.TicketTemplateID == Convert.ToInt32(this.ddlTicketTemplate.SelectedValue)).FirstOrDefault();

		if (_CurrentTicketTemplate != null)
		{

			this.OfficeID.SelectedValue = _CurrentTicketTemplate.OfficeID.ToString();
			this.Problem.Text = _CurrentTicketTemplate.Issue;
			this.DepartmentID.SelectedValue = _CurrentTicketTemplate.DepartmentID.ToString();
			this.DueDate.Text = WebUtil.ConvertToUserDatePattern(DateTime.Today.Date.AddDays(_CurrentTicketTemplate.DueDays).ToString("d"));

			//As the time on Due date is a hardcoded drop down, the current time will be rounded to upcoming 30 Minutes.
			var time = DateTime.Now;
			var rounded = time.AddMinutes(
			time.Minute > 30 ? +(60 - time.Minute) : +(30 - time.Minute)).ToString("HH:mm tt");
			this.DueDateTime.SelectedValue = rounded;

			if (_CurrentTicketTemplate.DepartmentID > 0)
			{
				//Lod Categories and Subcategories from _CurrentTicketTemplate selected.
				LookupTableHandler.LoadTicketCategories(ParentID, false, _CurrentTicketTemplate.DepartmentID, ddlTicketSource.SelectedValue, "0");
				LookupTableHandler.LoadTicketCategories(CategoryID, false, _CurrentTicketTemplate.DepartmentID, ddlTicketSource.SelectedValue, _CurrentTicketTemplate.CategoryID.ToString());
				pnlCategory.Visible = true;
				pnlSubCategory.Visible = true;
				ListHandler.ListFindItem(this.ParentID, _CurrentTicketTemplate.CategoryID.ToString());
				ListHandler.ListFindItem(this.CategoryID, _CurrentTicketTemplate.SubCategoryID.ToString());
			}
		}

		//This is only for Internal tickets as this drop down event change applies only on new tickets.
		chkAgent.Enabled = string.IsNullOrEmpty(this.AgentDBA.Text) ? false : true;
		ChkMerchant.Enabled = string.IsNullOrEmpty(this.BusinessDBAName.Text) ? false : true;

	}


	private void CopyTicket(string ticketUID)
	{
		DataTicket data = DataAccess.DataTicketDao;
		Ticket ticket = new Ticket();
		Hashtable prms = new Hashtable();

		prms.Add("@UID", ticketUID);

		ticket = data.GetTicket(prms, UserSessions.CurrentUser.TimeZone);

		OriginalTicket = ticket;

		ListHandler.ListFindItem(ddlTicketSource, ticket.TicketSource);
		//IsMLETicket.Checked = ticket.IsMLETicket;
		//
		if (ticket.TicketSource != "i") // This condition check if this Tkicket is from Zeus
		{
			this.pnlTickettemplate.Visible = false;
		}
		ListHandler.ListFindItem(OfficeID, ticket.OfficeID.ToString());
		ListHandler.ListFindItem(Origin, ticket.Origin.ToString());
		ListHandler.ListFindItem(DepartmentID, ticket.DepartmentID);

		if (CommonUtility.Util.if_i(ticket.DepartmentID, -1) > 0)
		{
			//Lod Categories and Subcategories from the ticket we are tryign to copy.
			LookupTableHandler.LoadTicketCategories(ParentID, false, CommonUtility.Util.if_i(ticket.DepartmentID, -1), ddlTicketSource.SelectedValue, "0");
			LookupTableHandler.LoadTicketCategories(CategoryID, false, CommonUtility.Util.if_i(ticket.DepartmentID, -1), ddlTicketSource.SelectedValue, ticket.ParentID.ToString());
			pnlCategory.Visible = true;
			pnlSubCategory.Visible = true;
			ListHandler.ListFindItem(ParentID, ticket.ParentID.ToString());
			ListHandler.ListFindItem(CategoryID, ticket.CategoryID.ToString());
		}

		Problem.Text = Server.HtmlDecode(ticket.Problem);

		if (!string.IsNullOrWhiteSpace(ticket.DueDate.ToString()) && ticket.DueDate != DateTime.MinValue && ticket.DueDate.Year != 0001)
		{
			DueDate.Text = WebUtil.ConvertToUserDatePattern(ticket.DueDate.ToString());
			ListHandler.ListFindItem(DueDateTime, ticket.DueDate.ToString("HH:mm tt"));
		}

		if (!string.IsNullOrWhiteSpace(ticket.CallbackDate.ToString()) && ticket.CallbackDate != DateTime.MinValue && ticket.CallbackDate.Year != 0001)
		{
			CallbackDate.Text = WebUtil.ConvertToUserDatePattern(ticket.CallbackDate.ToString());
		}

		ListHandler.ListFindItem(TimeZone, ticket.TimeZone);
		ListHandler.ListFindItem(Priority, ticket.Priority);

		Tags.Text = ticket.Tags;


		NoteCount.Value = ticket.TicketNotes.Count.ToString();

		prms = new Hashtable();

		prms.Add("@PrimaryKeyID", ticket.TicketID);
		prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.Tickets);
		prms.Add("@UserUID", UserSessions.CurrentUser.UID);

		List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);

		if (li != null && li.Count > 0)
		{
			DocsCount.Value = li.Count.ToString();
		}
		//PXP-13364
		//Ani: DM-5724
		chkManagerApproval.Checked = false; 


		if (UserSessions.CurrentUser != null && !string.IsNullOrEmpty(UserSessions.CurrentUser.UserID))
		{
            if (m_TicketApprovalManager != null)
            {
                bool isTicketApprovalManager = m_TicketApprovalManager.Any(ApprovalManagerUsers => ApprovalManagerUsers.ItemValue == UserSessions.CurrentUser.UserID);
                if (isTicketApprovalManager)
                    chkManagerApproval.Enabled = true;
            }
        }

	}

	protected void ddlChangeType_SelectedIndexChanged(object sender, EventArgs e)
	{
		this.vsChangeHistoryFieldID = CommonUtility.Util.if_i(ddlChangeType.SelectedValue, -1).ToString();

		this.LoadTicketFieldHistory();
	}

	protected void ddlChangeType_PreRender(object sender, EventArgs e)
	{
		ddlChangeType.SelectedValue = this.vsChangeHistoryFieldID;
	}

	protected void grdChange_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.Header)
		{
			if (ddlChangeType.SelectedIndex == 0)
			{
				((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = "Field Value";
			}
			else
			{
				((Label)e.Row.Cells[1].FindControl("lblNameHeader")).Text = ddlChangeType.SelectedItem.Text;
			}
		}

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			string value = DataBinder.Eval(e.Row.DataItem, "NewValue").ToString();
			value = value.Replace("\\", ";<br>");

			string name = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

			name = name.Replace("\\", ";<br>");

			string[] duedate = value.Split(';');
			if (CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "ChangeHistoryFieldID"), -1) == 9)
			{
				if (!string.IsNullOrWhiteSpace(duedate[0]) && CommonUtility.Util.IsValidDateTime(duedate[0]))
				{
					duedate[0] = WebUtil.CovertToUserDateTimePattern(CommonUtility.Util.if_date(duedate[0], DateTime.MinValue));

					if (duedate.Length > 1)
					{
						value = duedate[0] + ";" + CommonUtility.Util.if_s(duedate[1], "");
					}
					else
					{
						value = duedate[0] + ";";
					}

				}
			}

			((Label)e.Row.Cells[1].FindControl("lblValue")).Text = value;
			((Label)e.Row.Cells[0].FindControl("lblName")).Text = name;
			e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);

		}
	}

	protected void btnOk_Click(object sender, EventArgs e)
	{
		this._IsCopyNotes = false;
		this._IsCopyDocs = false;

		lblSuccess.Text = "";
		lblErr.Text = "";

		if (((Button)sender).CommandArgument.ToUpper() == "YES")
		{
			if (chkDocuments.Checked || chkNotes.Checked)
			{
				this._IsCopyNotes = chkNotes.Checked;
				this._IsCopyDocs = chkDocuments.Checked;
			}
			else
			{
				lblErr.Text = "Select at least one item or press Cancel to close.";
				return;
			}
		}

		CopyTicketNotesandDocuments();

	}

	private void CopyTicketNotesandDocuments()
	{

		DataTicket data = new DataTicket();
		int copyItem = 0;

		//when the ticket is being copied from another ticket and user wants notes or  documents also then call this function
		if (this._IsCopyNotes || this._IsCopyDocs)
		{
			if (this._IsCopyNotes && this._IsCopyDocs)
			{
				copyItem = 3;
			}
			else if (this._IsCopyNotes)
			{
				copyItem = 1;
			}
			else
			{
				copyItem = 2;
			}

			if (copyItem == 1 || copyItem == 3)
				Solution.Text = OriginalTicket.Solution;

			data.CopyTicketNotesandDocs(CopyTicketUID, CurrrentTicketUID, copyItem);

		}

		dlgConfirm.WindowState = DialogWindowState.Hidden;

		if (!string.IsNullOrEmpty(this._WindowCallID))
		{
			ScriptManager.RegisterClientScriptBlock(pnl, pnl.GetType(), _WindowCallID, "javascript:window.close();", true);
		}
		else
		{
			Response.Redirect(String.Format("~/SecureTicketForms/frmTicketDetail.aspx?Adding=False&TicketUID={0}", this.UID));
		}

	}

	protected void PrivateLabelUID_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (PrivateLabelUID.SelectedValue.ToUpper().Equals(PrivateLabel.PRIVATELABEL_GMA) && UserSessions.CurrentUser.Office.Equals(CommonUtility.Util.Offices.LosAngeles))
		{
			divAgentAction.Visible = true;
		}
		else
		{
			divAgentAction.Visible = false;
		}
	}

	protected void btnExpExcel_Click(object sender, EventArgs e)
	{
		var grdTemp = new GridView();
		grdTemp.DataSource = list;
		grdTemp.DefaultWithPager();
		grdTemp.AddBoundColumn("TicketID", "TicketID");
		grdTemp.AddBoundColumn("NoteID", "NoteID");
		grdTemp.AddBoundColumn("Subject", "Subject");
		grdTemp.AddBoundColumn("Notes", "Notes");
		grdTemp.AddBoundColumn("Date Created", "DateCreated");
		grdTemp.AddBoundColumn("User Created", "UserCreated");
		grdTemp.DataBind();
		FormHandler.Export2Excel("MerchantTicketNotes.xls", grdTemp);
	}

	//DM-5093 ini
	public string CloneTicket(string copyTicketUID, string merchantAppUID, string issue, bool cloneNotes, bool cloneDocs, DateTime dueDate)
	{
		string _TicketID = "";
		Adding = true;
		bool oldIsCopyNotes = this._IsCopyNotes;
		bool oldIsCopyDocs = this._IsCopyDocs;
		string oldIssue = Problem.Text;
		try
		{
			CopyTicket(copyTicketUID);
			var lnk = new LinkButton();
			lnk.CommandArgument = merchantAppUID + ",";
			grdMerchants_GridRowCommand(null, new GridViewCommandEventArgs(lnk, new CommandEventArgs("", merchantAppUID + ",")));
			Problem.Text = issue;
			DueDate.Text = WebUtil.ConvertToUserDatePattern(dueDate.ToString());
			ListHandler.ListFindItem(DueDateTime, dueDate.ToString("HH:mm tt"));

			this.StatusID.SelectedValue = Ticket.TICKET_OPEN.ToLower();
			this.UserID.SelectedValue = "-1";

			Ticket _ticket = HandleSaveButton();

			this._IsCopyNotes = cloneNotes;
			this._IsCopyDocs = cloneDocs;
			if (cloneNotes)
			{
				DataTicket data = new DataTicket();
				int copyItem = 0;

				//when the ticket is being copied from another ticket and user wants notes or  documents also then call this function
				if (this._IsCopyNotes || this._IsCopyDocs)
				{
					if (this._IsCopyNotes && this._IsCopyDocs)
					{
						copyItem = 3;
					}
					else if (this._IsCopyNotes)
					{
						copyItem = 1;
					}
					else
					{
						copyItem = 2;
					}

					if (copyItem == 1 || copyItem == 3)
						Solution.Text = OriginalTicket.Solution;

					data.CopyTicketNotesandDocs(copyTicketUID, _ticket.TicketUID, copyItem);
				}
			}

			if (_ticket == null)
			{
				_TicketID = lblError.Text;
			}
			else
			{
				_TicketID = _ticket?.TicketID?.Replace("000000", "") + ";" + _ticket.TicketUID;
			}
		}
		catch (Exception ex)
		{
			ZeusWeb.Logging.ErrorLog.Error("Fail when clone ticket: " + copyTicketUID + " for merchant:" + merchantAppUID, ex);
		}
		Adding = false;
		this._IsCopyNotes = oldIsCopyNotes;
		this._IsCopyDocs = oldIsCopyDocs;
		Problem.Text = oldIssue;
		return _TicketID;
	}

	public void RefreshTicket(string copyTicketUID)
	{
		Response.Redirect(WebUtil.GetMyUrl("Adding=false&TicketUID=" + copyTicketUID));
	}
	//DM-5093  end

	//Fady Massoud 12-22-2020
	//PXP-15781 Document Uploader
	//code added by koshlendra for PXP-7622 start
	/// <summary>
	/// add multiple documents on listbox
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	//protected void btnUpload_Click(object sender, EventArgs e)
	//{
	//    try
	//    {
	//        {
	//            if (fupDocument.HasFile)
	//            {

	//                long f_size_max = Convert.ToInt64(ConfigurationManager.AppSettings["MAX_FILE_UPLOAD_BYTES"]);
	//                foreach (HttpPostedFile fileToUpload in fupDocument.PostedFiles)
	//                {
	//                    string filename = System.IO.Path.GetFileName(fileToUpload.FileName);
	//                    double size = fileToUpload.ContentLength / 1048576.0;
	//                    string file = fileToUpload.FileName;
	//                    if (fileToUpload.ContentLength > f_size_max)
	//                    {

	//                        continue;
	//                    }
	//                    string file_ext = file.Substring(file.LastIndexOf('.') + 1).ToUpper();

	//                    if (!MDoc.GetWhiteListExtensions().Contains(file_ext))
	//                    {
	//                        continue;

	//                    }

	//                    byte[] Bytes = null;

	//                    using (var binaryReader = new BinaryReader(fileToUpload.InputStream))
	//                    {
	//                        Bytes = binaryReader.ReadBytes(fileToUpload.ContentLength);
	//                    }

	//                    string ticketId = "Newticket";

	//                    if (UserSessions.GetTemporaryAttachmentsCount(Session, ticketId) == 0 || !UserSessions.GetTemporaryAttachments(Session, ticketId).ContainsKey(filename))
	//                    {
	//                        UserSessions.AddToTemporaryAttachments(Session, ticketId, filename, Bytes, CommonUtility.Util.if_i(int.Parse(this.ddlDocumentType.SelectedValue), 0));
	//                        System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
	//                        item.Text = Math.Round(size, 2).ToString() + " MB " + " " + filename;
	//                        item.Value = fileToUpload.FileName;
	//                        lstCustomAttachments.Items.Add(item);
	//                    }

	//                }



	//            }

	//        }


	//    }
	//    catch (Exception ex)
	//    {

	//    }



	//}

	//Fady Massoud 12-22-2020
	//PXP-15781 Document Uploader
	/// <summary>
	/// Uploading Documents while creating ticket
	/// </summary>
	/// <param name="ticket"></param>
	//private void UploadDocuments(Ticket ticket)
	//{
	//    try
	//    {
	//        string ticketId = "Newticket";

	//        Dictionary<string, byte[]> attachments = UserSessions.GetTemporaryAttachments(Session, ticketId);


	//        if (attachments != null)
	//        {

	//            foreach (string key in attachments.Keys)
	//            {
	//                UploadAttachments(attachments[key], key, Convert.ToInt32(ddlDocumentType.SelectedValue), ticket);
	//            }
	//        }
	//        UserSessions.ClearTemporaryAttachments(Session, ticketId);
	//        lstCustomAttachments.Items.Clear();
	//        ddlDocumentType.Items.Clear();

	//    }
	//    catch (Exception)
	//    {

	//    }
	//}
	//Fady Massoud 12-22-2020
	//PXP-15781 Document Uploader
	/// <summary>
	/// Function for Uploadin attachment on mdoc server.
	/// </summary>
	/// <param name="filebytes"> file byte </param>
	/// <param name="filename"> name of the files</param>
	/// <param name="docTypeID"> document type</param>
	/// <param name="t">New Ticket</param>
	/// <returns></returns>
	//public bool UploadAttachments(byte[] filebytes, string filename, int docTypeID, Ticket t)
	//{
	//    bool ret = false;
	//    try
	//    {

	//        ZeusWeb.MDocWS.FileUpload fu = new ZeusWeb.MDocWS.FileUpload();

	//        fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];
	//        int primary_key_id = CommonUtility.Util.if_i(t.TicketID, 0); ;
	//        string primary_key_uid = CommonUtility.Util.if_s(t.TicketUID, "");
	//        string orig_filename = filename;
	//        int merchantapp_id = CommonUtility.Util.if_i(ZID.Text, 0);
	//        string merchantapp_uid = CommonUtility.Util.if_s(MerchantAppUID.Value, "");
	//        int mdoc_sourceid = CommonUtility.Util.if_i((int)MDoc.eMDocSourceID.Tickets, 3);
	//        int doctypeid = CommonUtility.Util.if_i(ddlDocumentType.SelectedValue, 0);
	//        string descr = CommonUtility.Util.if_s(tbFilesDescription.Text);
	//        string username = CommonUtility.Util.if_s(UserSessions.CurrentUser.FirstName + " " + UserSessions.CurrentUser.LastName, "");
	//        //PXP-8889:START Attachment Download Error
	//        int agentId = CommonUtility.Util.if_i(AgentID.Value, 0);
	//        if (agentId == 0)
	//        {
	//            if (!string.IsNullOrEmpty(AgentUID.Value))
	//            {
	//                DataAgent dataAgent = DataAccess.DataAgentDao;
	//                Agent agent = dataAgent.GetAgent(AgentUID.Value);
	//                if (agent != null)
	//                {
	//                    agentId = agent.AgentID;
	//                }
	//            }
	//        }
	//        //PXP-8889:END Attachment Download Error
	//        if (filebytes != null)
	//        {
	//            // TODO: change merchantid and merchantuid.
	//            ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(filebytes, merchantapp_id, merchantapp_uid, agentId, null, doctypeid, orig_filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);//PXP-8889 Attachment Download Error

	//            if (resp != null && resp.DocID > 0)
	//            {

	//                ret = true;
	//            }

	//        }
	//    }
	//    catch (Exception ex)
	//    {

	//        throw;
	//    }

	//    return ret;
	//}




	//code added by koshlendra for PXP-7622 end
}
