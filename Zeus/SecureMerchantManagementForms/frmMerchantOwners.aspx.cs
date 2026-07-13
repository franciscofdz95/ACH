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
using System.Reflection;
using System.IO;
using System.Collections.Generic;

using Infragistics.WebUI.WebControls;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.EditorControls;

using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using PaymentXP.Facade;
using System.Linq;

public partial class frmMerchantOwners : frmBaseDataEntry
{
	bool isValid = true;
	private bool isOfficeIrvine;

	protected override void OnPreInit(EventArgs e)
	{
		base.OnPreInit(e);
		// Set Property to Store ViewState on Server
		base.StoreViewStateOnServer = true;

		if (UserSessions.CurrentMerchantApp != null)
			base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

	}

	bool IsWoodForestOnlyApp
	{
		get
		{
			//code changes done for PXP-10232 by koshlendra
			if (UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST)
				|| UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST_SB)
                || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_BBVACOMPASS)
				|| UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_CITIZENS) )
				return true;
			else
				return false;
		}
	}
	override protected void OnInit(EventArgs e)
	{
		//wucOwner0.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		//wucOwner1.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		//wucOwner2.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		//wucOwner3.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		////PXP-2883
		//wucOwner4.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		//wucOwner5.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
		WucBusinessInfo1.TextChange += new wucBusinessInfo.TextChangeHandler(WucBusinessInfo1_TextChange);
		confirm.ButtonClick += new wuConfirmDialog.ButtonClickHandler(confirm_ButtonClick);

		//PXP-4979 by koshlendra start
		//code changes done for PXP-10232 by koshlendra
		if (UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine && 
			(UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST 
			|| UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB
            || UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_CITIZENS
			|| UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper() == Constants.BANK_BBVACOMPASS))
		{
			isOfficeIrvine = true;
			//((CheckBox)wucOwner0.FindControl("AuthorizedSignature")).Text = "Controller";
			//((CheckBox)wucOwner1.FindControl("AuthorizedSignature")).Text = "Controller";
			//((CheckBox)wucOwner2.FindControl("AuthorizedSignature")).Text = "Controller";
			//((CheckBox)wucOwner3.FindControl("AuthorizedSignature")).Text = "Controller";
			//((CheckBox)wucOwner4.FindControl("AuthorizedSignature")).Text = "Controller";

			//// PXP-6467 Fady Massoud 08/04/2018
			//for (int i = 0; i < 6; i++)
			//{
			//    ((CheckBox)pnlOwners.FindControl(("wucOwner" + i)).FindControl("CBRWaived")).Text = "T&C Signed (Run CBR)";
			//}
		}
		else
		{
			isOfficeIrvine = false;
			//((CheckBox)wucOwner0.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner1.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner2.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner3.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner4.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner0.FindControl("CBRWaived")).Checked = false;
			//((CheckBox)wucOwner1.FindControl("CBRWaived")).Checked = false;
			//((CheckBox)wucOwner2.FindControl("CBRWaived")).Checked = false;
			//((CheckBox)wucOwner3.FindControl("CBRWaived")).Checked = false;
			//((CheckBox)wucOwner4.FindControl("CBRWaived")).Checked = false;
			//((CheckBox)wucOwner5.FindControl("CBRWaived")).Visible = false;
			//((CheckBox)wucOwner5.FindControl("CBRWaived")).Checked = false;
		}
		//PXP-4979 by koshlendra end

		//DM-5037 ini
		foreach (var control in pnlOwners.Controls)
		{
			if (control is wucOwner)
			{
				var _wucOwner = control as wucOwner;
				_wucOwner.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
				if (isOfficeIrvine)
				{
					((CheckBox)_wucOwner.FindControl("AuthorizedSignature")).Text = "Controller";
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Text = "T&C Signed (Run CBR)";
				}
				else
				{
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Checked = false;
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Visible = false;
				}
			}
		}
		//DM-5037 end

		base.OnInit(e);
	}

	void confirm_ButtonClick(object sender, EventArgs e)
	{
		MerchantApp app = UserSessions.CurrentMerchantApp;
		string url = string.Empty, message = string.Empty;
		//bool count = true;

		if (((TextBox)confirm.FindControl("txtTaxID")).Visible && ((TextBox)confirm.FindControl("txtTaxID")).Text != ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text)
		{
			//count = false;
			message += " Please enter valid TaxID.<br />";
			((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text = (app != null) ? app.BusinessTaxID : string.Empty;
		}
		if (((TextBox)confirm.FindControl("txtMID")).Visible && ((TextBox)confirm.FindControl("txtMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text)
		{
			//count = false;
			message += " Please enter valid Front MID.<br />";
			((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text = (app != null) ? app.AuthPlatformMid : string.Empty;
		}
		if (((TextBox)confirm.FindControl("txtBMID")).Visible && ((TextBox)confirm.FindControl("txtBMID")).Text != ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text)
		{
			//count = false;
			message += " Please enter valid Back MID.<br />";
			((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text = (app != null) ? app.SettlePlatformMid : string.Empty;
		}
		if (((WebMaskEditor)confirm.FindControl("txtSSN")).Visible)
		{
			var _ownerID = ((HiddenField)confirm.FindControl("hiddenOwnerId")).Value ;
			var parentID = ((HiddenField)confirm.FindControl("hiddenPanelID")).Value;

			wucOwner _wucOwner = (wucOwner)pnlOwners.FindControl(parentID);
			if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)_wucOwner.FindControl("SSN")).Value.ToString())
			{
				//count = false;
				this.Master.AddMessageError("Please enter valid SSN for " + _wucOwner.ClientID + ".");
				if (app != null && app.Owners.Count > 0)
				{
					var _owner = app.Owners.Where(p => p.OwnerID == _ownerID).FirstOrDefault();
					if (_owner != null)
					{
						((WebMaskEditor)_wucOwner.FindControl("SSN")).Text = _owner.SSN;
					}
					else
					{
						((WebMaskEditor)_wucOwner.FindControl("SSN")).Text = "";
					}
				}
				else
					((WebMaskEditor)_wucOwner.FindControl("SSN")).Text = "";
			}

			//switch (((Label)confirm.FindControl("lblSSN")).Text)
			//{
			//    case "Owner1 SSN:":
			//    default:
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner0.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner1.");
			//            if (app != null && app.Owners.Count > 0)
			//                ((WebMaskEditor)wucOwner0.FindControl("SSN")).Text = app.Owners[0].SSN;
			//            else
			//                ((WebMaskEditor)wucOwner0.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//    case "Owner2 SSN:":
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner1.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner2.");
			//            if (app != null && app.Owners.Count > 1)
			//                ((WebMaskEditor)wucOwner1.FindControl("SSN")).Text = app.Owners[1].SSN;
			//            else
			//                ((WebMaskEditor)wucOwner1.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//    case "Owner3 SSN:":
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner2.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner3.");
			//            if (app != null && app.Owners.Count > 2)
			//                ((WebMaskEditor)wucOwner2.FindControl("SSN")).Text = app.Owners[2].SSN;
			//            else
			//                ((WebMaskEditor)wucOwner2.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//    case "Owner4 SSN:":
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner3.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner4.");
			//            if (app != null && app.Owners.Count > 3)
			//                ((WebMaskEditor)wucOwner3.FindControl("SSN")).Text = app.Owners[3].SSN;
			//            else
			//                ((WebMaskEditor)wucOwner3.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//    //PXP-2883
			//    case "Owner5 SSN:":
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner4.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner5.");
			//            if (app != null && app.Owners.Count > 4)
			//                ((WebMaskEditor)wucOwner4.FindControl("SSN")).Text = app.Owners[4].SSN;
			//            else
			//                ((WebMaskEditor)wucOwner4.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//    case "Owner6 SSN:":
			//        if (((WebMaskEditor)confirm.FindControl("txtSSN")).Value.ToString() != ((WebMaskEditor)wucOwner5.FindControl("SSN")).Value.ToString())
			//        {
			//            //count = false;
			//            this.Master.AddMessageError("Please enter valid SSN for Owner6.");
			//            if (app != null && app.Owners.Count > 5)
			//                ((WebMaskEditor)wucOwner5.FindControl("SSN")).Text = app.Owners[5].SSN;
			//            else

			//                ((WebMaskEditor)wucOwner5.FindControl("SSN")).Text = "";
			//        }
			//        break;
			//}
		}


		confirm.SetValue(false);
		WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
	}

	void wucOwner0_ValueChange(object sender, Infragistics.Web.UI.EditorControls.TextEditorValueChangedEventArgs e, string ownerID, string ssn, string parentID)
	{
		//lblError.Text = string.Empty;
		if (UserSessions.CurrentMerchantApp != null)
		{
			if (((WebMaskEditor)sender).ClientID.ToUpper().StartsWith("OWNER") )
			{
				var _strIndex = ((WebMaskEditor)sender).ClientID.ToUpper().Replace("OWNER", "").Trim();
				int _ownerIndex = 0;
				if(int.TryParse(_strIndex, out _ownerIndex))
				{
					var _owner = UserSessions.CurrentMerchantApp.Owners.Where(p => p.OwnerID == ownerID).FirstOrDefault();
					if ((_owner != null && ssn != UserSessions.CurrentMerchantApp.Owners[0].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 0)
					{
						((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
						((Label)confirm.FindControl("lblSSN")).Visible = true;
						((Label)confirm.FindControl("lblSSN")).Text = "Owner" + _strIndex + " SSN: ";
						((HiddenField)confirm.FindControl("hiddenOwnerId")).Value = ownerID;
						((HiddenField)confirm.FindControl("hiddenPanelID")).Value = parentID;
						((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
						isValid = false;
						WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
					}
				}
			}

			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER0"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner0.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 0 && SSN != UserSessions.CurrentMerchantApp.Owners[0].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 0)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner1 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER1"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner1.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 1 && SSN != UserSessions.CurrentMerchantApp.Owners[1].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 1)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner2 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER2"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner2.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 2 && SSN != UserSessions.CurrentMerchantApp.Owners[2].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 2)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner3 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER3"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner3.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 3 && SSN != UserSessions.CurrentMerchantApp.Owners[3].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 3)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner4 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
			////PXP-2883
			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER4"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner3.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 4 && SSN != UserSessions.CurrentMerchantApp.Owners[4].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 4)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner5 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
			//if (((WebMaskEditor)sender).ClientID.ToUpper().Contains("OWNER5"))
			//{
			//    SSN = ((WebMaskEditor)wucOwner3.FindControl("SSN")).Value.ToString();
			//    if ((UserSessions.CurrentMerchantApp.Owners.Count > 5 && SSN != UserSessions.CurrentMerchantApp.Owners[5].SSN) || UserSessions.CurrentMerchantApp.Owners.Count == 5)
			//    {
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Visible = true;
			//        ((Label)confirm.FindControl("lblSSN")).Text = "Owner6 SSN:";
			//        ((WebMaskEditor)confirm.FindControl("txtSSN")).Focus();
			//        isValid = false;
			//        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
			//    }
			//}
		}
	}

	void WucBusinessInfo1_TextChange(object sender, EventArgs e)
	{
		//lblError.Text = string.Empty;
		if (UserSessions.CurrentMerchantApp != null)
		{
			switch (((TextBox)sender).ID)
			{
				case "BusinessTaxID":
					string BusinessTaxID = ((TextBox)WucBusinessInfo1.FindControl("BusinessTaxID")).Text;
					if (BusinessTaxID != UserSessions.CurrentMerchantApp.BusinessTaxID)
					{
						((TextBox)confirm.FindControl("txtTaxID")).Visible = true;
						((Label)confirm.FindControl("lblTaxID")).Visible = true;
						((TextBox)confirm.FindControl("txtTaxID")).Focus();
						isValid = false;
						WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
					}
					break;
				case "AuthPlatformMid":
					string AuthPlatformMid = ((TextBox)WucBusinessInfo1.FindControl("AuthPlatformMid")).Text;
					if (AuthPlatformMid != UserSessions.CurrentMerchantApp.AuthPlatformMid)
					{
						((TextBox)confirm.FindControl("txtMID")).Visible = true;
						((Label)confirm.FindControl("lblMID")).Visible = true;
						((TextBox)confirm.FindControl("txtMID")).Focus();
						isValid = false;
						WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
					}
					break;
				case "SettlePlatformMid":
					string SettlePlatformMid = ((TextBox)WucBusinessInfo1.FindControl("SettlePlatformMid")).Text;
					if (SettlePlatformMid != UserSessions.CurrentMerchantApp.SettlePlatformMid)
					{
						((TextBox)confirm.FindControl("txtBMID")).Visible = true;
						((Label)confirm.FindControl("lblBMID")).Visible = true;
						((TextBox)confirm.FindControl("txtBMID")).Focus();
						isValid = false;
						WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
					}
					break;
				default:
					break;
			}
		}
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
			this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
		WucBusinessInfo1.pnlInfo.Enabled = false;
		// if(UserSessions.CurrentMerchantApp.MerchantAppTypeUID.ToUpper().Equals(Constants.BANK_WOODFOREST))
		// if (UserSessions.CurrentMerchantApp.Bank.Equals("Woodforest"))


		if (!this.IsPostBack)
		{
			this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Owners);

			if (UserSessions.CurrentMerchantApp != null)
			{
				this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Owners");
			}

			//wucOwner0.SetTitle = "Owner 1";
			//wucOwner1.SetTitle = "Owner 2";
			//wucOwner2.SetTitle = "Owner 3";
			//wucOwner3.SetTitle = "Owner 4";
			////PXP-2883
			//wucOwner4.SetTitle = "Owner 5";
			//wucOwner5.SetTitle = "Owner 6";

			WucTradeReference0.SetTitle = "Trade Reference 1";
			WucTradeReference1.SetTitle = "Trade Reference 2";
			WucTradeReference2.SetTitle = "Trade Reference 3";

			//PXP-2883
			//Code commented for PXP-3199:Owner 5 and Owner 6 should be display for all office and merchant by Koshlendra on 28/12/2017 start 
			//if (!UserSessions.CurrentMerchantApp.Office.Equals(CommonUtility.Util.Offices.Cambridge))
			//{
			//    wucOwner4.Visible = false;
			//    wucOwner5.Visible = false;
			//}
			//Code commented for PXP-3199:Owner 5 and Owner 6 should be display for all office and merchant by Koshlendra on 28/12/2017 end 

			this.Adding = Convert.ToBoolean(Request["Adding"]);
			if (this.Adding)
			{
				this.FormNew();
			}
			else
			{
				if (Request["MerchantAppUID"] != null)
					UID = Request["MerchantAppUID"].ToString();
				else
					UID = UserSessions.CurrentMerchantApp.MerchantAppUID;
				this.FormShow(UID);
			}
		}
		else
		{
			if (UserSessions.CurrentMerchantApp != null)
			{
				if (!Request.Form["__EVENTTARGET"].EndsWith("$btnCancel"))
				{
					MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
					ExtractCorporateBusinessFromOwners(agreement);
					Owners_RenderUI(agreement.Owners, agreement.Office);
				}
			}
		}
	}

	public override void FormShow(string ID)
	{
		MerchantFacade facade = new MerchantFacade();
		MerchantApp agreement = facade.GetMerchantAppZeus(ID);

		UserSessions.CurrentMerchantApp = agreement;

		DropDownList status = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
		LookupTableHandler.MerchantAppStatus(status, false, "Merchant Management", agreement.StatusName.Substring(0, 2));

		bool isAch = agreement.AchID > 0 && agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY;

		if (agreement != null)
		{
			agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);
			ExtractCorporateBusinessFromOwners(agreement);
			agreement.TradeReferences = DataAccess.DataMerchantAppDao.GetTradeReferences(agreement.MerchantAppUID);

			if (isAch && UserSessions.ActiveAchMerchant != null)
			{
				//FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
				UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));
				UserSessions.ActiveAchMerchant.CloneAchMerchant();


				DropDownList achstatus = (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID");
				LookupTableHandler.MerchantAppStatus(achstatus, false, "Merchant Management", UserSessions.ActiveAchMerchant.MerchantStatusName.Substring(0, 2));
				ListHandler.ListFindItem(achstatus, UserSessions.ActiveAchMerchant.MerchantStatusUID);

			}
		}

		FormBinding.BindObjectToControls(agreement, pnlDetail);
		FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
		WucBusinessInfo1.SelectButton.Enabled = this.EditMode;

		//wucOwner0.pnlOwner.Enabled = this.EditMode;
		//wucOwner1.pnlOwner.Enabled = this.EditMode;
		//wucOwner2.pnlOwner.Enabled = this.EditMode;
		//wucOwner3.pnlOwner.Enabled = this.EditMode;
		////PXP-2883
		//wucOwner4.pnlOwner.Enabled = this.EditMode;
		//wucOwner5.pnlOwner.Enabled = this.EditMode;
		wucCorpBuz1.pnlCorporateBusiness.Enabled = this.EditMode;

		WucTradeReference0.pnlTR.Enabled = this.EditMode;
		WucTradeReference1.pnlTR.Enabled = this.EditMode;
		WucTradeReference2.pnlTR.Enabled = this.EditMode;

		//check to see if the account is ACH only and get the ach status in case if it is or else the cc status
		string m_StatusUID = agreement.StatusUID.ToUpper();

		if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
		{
			m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
		}

		FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

		//List<Control> lst = new List<Control>();
		//lst.Insert(0, wucOwner0);
		//lst.Insert(1, wucOwner1);
		//lst.Insert(2, wucOwner2);
		//lst.Insert(3, wucOwner3);
		////PXP-2883
		//lst.Insert(4, wucOwner4);
		//lst.Insert(5, wucOwner5);

		//code changes done for Fixing default formating of home phone number by koshlendra
		//for (int j = 0; j < lst.Count; j++)
		//{
		//    DropDownList CountryDisplayCodeddl0 = ((DropDownList)((wucOwner)lst[j]).FindControl("HomePhoneCountryCode"));
		//    TextBox CountryDisplayCode0 = ((TextBox)((wucOwner)lst[j]).FindControl("HomeCountryCodeDisplay"));
		//    CountryDisplayCode0.Text = CountryDisplayCodeddl0.SelectedValue;

		//    WebMaskEditor HomePhoneNumbertxt = (WebMaskEditor)((wucOwner)lst[j]).FindControl("HomePhone");
		//    if (agreement.Office == CommonUtility.Util.Offices.Irvine)
		//    {
		//        HomePhoneNumbertxt.InputMask = "000-000-0000";
		//    }
		//    else
		//    {
		//        HomePhoneNumbertxt.InputMask = "############";
		//    }
		//}

		//for (int i = 0; i < agreement.Owners.Count; i++)
		//{
		//    FormBinding.BindObjectToControls(agreement.Owners[i], lst[i]);

		//    ((CheckBox)((wucOwner)lst[i]).FindControl("PersonalGuarantor")).Enabled = true;
		//    //Added for phone Country Code and Extention 
		//    DropDownList CountryDisplayCodeddl0 = ((DropDownList)((wucOwner)lst[i]).FindControl("HomePhoneCountryCode"));
		//    TextBox CountryDisplayCode0 = ((TextBox)((wucOwner)lst[i]).FindControl("HomeCountryCodeDisplay"));
		//    CountryDisplayCode0.Text = CountryDisplayCodeddl0.SelectedValue;

		//    WebMaskEditor HomePhoneNumbertxt = (WebMaskEditor)((wucOwner)lst[i]).FindControl("HomePhone");
		//    if (agreement.Office == CommonUtility.Util.Offices.Irvine)
		//    {
		//        HomePhoneNumbertxt.InputMask = "000-000-0000";
		//        string strHomePhoneNumbertxt = CommonUtility.Util.GetNumbersFromString(agreement.Owners[i].HomePhone);
		//        HomePhoneNumbertxt.Value = strHomePhoneNumbertxt.ToString();
		//    }
		//    else
		//    {
		//        HomePhoneNumbertxt.InputMask = "############";
		//        HomePhoneNumbertxt.Value = agreement.Owners[i].HomePhone;
		//    }

		//}

		Owners_RenderUI(agreement.Owners, agreement.Office);

		DropDownList CorpBuz_DDLCountryDisplayCode = ((DropDownList)(wucCorpBuz1).FindControl("HomePhoneCountryCode"));
		TextBox CorpBuz_TxtCountryDisplayCode = ((TextBox)(wucCorpBuz1).FindControl("HomeCountryCodeDisplay"));
		CorpBuz_TxtCountryDisplayCode.Text = CorpBuz_DDLCountryDisplayCode.SelectedValue;

		WebMaskEditor CorpBuz_TxtHomePhoneNumber = (WebMaskEditor)(wucCorpBuz1).FindControl("HomePhone");
		if (agreement.Office == CommonUtility.Util.Offices.Irvine)
		{
			CorpBuz_TxtHomePhoneNumber.InputMask = "000-000-0000";
		}
		else
		{
			CorpBuz_TxtHomePhoneNumber.InputMask = "############";
		}

		FormBinding.BindObjectToControls(agreement.CorpBuzOwners, wucCorpBuz1);
		if (agreement.Office == CommonUtility.Util.Offices.Irvine)
		{
			CorpBuz_TxtHomePhoneNumber.InputMask = "000-000-0000";
			string strHomePhoneNumbertxt = CommonUtility.Util.GetNumbersFromString(agreement.CorpBuzOwners.HomePhone);
			CorpBuz_TxtHomePhoneNumber.Value = strHomePhoneNumbertxt;
			// CorpBuz_TxtHomePhoneNumber.Text = agreement.CorpBuzOwners.HomePhone;
		}
		else
		{
			CorpBuz_TxtHomePhoneNumber.InputMask = "############";
			CorpBuz_TxtHomePhoneNumber.Value = agreement.CorpBuzOwners.HomePhone;
		}

		List<Control> lst1 = new List<Control>();
		lst1.Insert(0, WucTradeReference0);
		lst1.Insert(1, WucTradeReference1);
		lst1.Insert(2, WucTradeReference2);

		//Added for phone Country Code and Extention 
		for (int j = 0; j < 3; j++)
		{
			DropDownList PhoneNumberCountryCodeddl0 = ((DropDownList)((wucTradeReference)lst1[j]).FindControl("PhoneNumberCountryCode"));
			TextBox PhoneNumberCountryCodeDisplay0 = ((TextBox)((wucTradeReference)lst1[j]).FindControl("PhoneNumberCountryCodeDisplay"));
			PhoneNumberCountryCodeDisplay0.Text = PhoneNumberCountryCodeddl0.SelectedValue;

			WebMaskEditor PhoneNumbertxt = (WebMaskEditor)((wucTradeReference)lst1[j]).FindControl("PhoneNumber");
			if (agreement.Office == CommonUtility.Util.Offices.Irvine)
			{
				PhoneNumbertxt.InputMask = "000-000-0000";
			}
			else
			{
				PhoneNumbertxt.InputMask = "############";
			}
		}

		for (int i = 0; i < agreement.TradeReferences.Count && i < 3; i++)
		{
			FormBinding.BindObjectToControls(agreement.TradeReferences[i], lst1[i]);
			TextBox txt = ((TextBox)((wucTradeReference)lst1[i]).FindControl("Address"));

			if (txt != null)
				txt.Text = agreement.TradeReferences[i].Address;

			DropDownList PhoneNumberCountryCodeddl0 = ((DropDownList)((wucTradeReference)lst1[i]).FindControl("PhoneNumberCountryCode"));
			TextBox PhoneNumberCountryCodeDisplay0 = ((TextBox)((wucTradeReference)lst1[i]).FindControl("PhoneNumberCountryCodeDisplay"));
			PhoneNumberCountryCodeDisplay0.Text = PhoneNumberCountryCodeddl0.SelectedValue;

			WebMaskEditor PhoneNumbertxt = (WebMaskEditor)((wucTradeReference)lst1[i]).FindControl("PhoneNumber");
			if (agreement.Office == CommonUtility.Util.Offices.Irvine)
			{
				PhoneNumbertxt.InputMask = "000-000-0000";
				string strPhoneNumbertxt = CommonUtility.Util.GetNumbersFromString(agreement.TradeReferences[i].PhoneNumber);
				PhoneNumbertxt.Value = strPhoneNumbertxt.ToString();
			}
			else
			{
				PhoneNumbertxt.InputMask = "############";
				PhoneNumbertxt.Value = agreement.TradeReferences[i].PhoneNumber;
			}
		}

		DropDownList ReleaseMethodUID = (DropDownList)WucBusinessInfo1.FindControl("ReleaseMethodUID");
		ReleaseMethodUID.Enabled = false;

		DropDownList Discount = (DropDownList)WucBusinessInfo1.FindControl("DiscountMethod");
		Discount.Enabled = false;

		MasterPageMerchant master = (MasterPageMerchant)this.Master;
		master.ShowNotes(agreement.UWNotes, agreement.AgentMemo, agreement.FirstTeamNotes, this.EditMode);

		bool CCApproved = MerchantFacade.ExistsStatus(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED) && !isAch;
		bool ACHApproved = MerchantFacade.ExistsACHStatus(UserSessions.CurrentMerchantApp.ID, Constants.QUEUESTATUS_CU_APPROVED) && isAch;


		WucBusinessInfo1.LoadOffice(agreement);


		// CU-Approved Lockdown! only members of role "Special Access" can edit these fields once an account has been approved.
		if (this.EditMode == true && (CCApproved || ACHApproved))
		{
			bool has_special_access_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS)
				&& UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS].Enabled == true);
			//code Added by Koshlendra for PXP-8490           
			bool has_credit_underwriting_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING)
			   && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled == true);
			bool has_sale_support_role = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SALES_SUPPORT)
			   && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SALES_SUPPORT].Enabled == true);
			//Code added by koshlendra for PXP-9251[Allow OP user to add/update Owner information after application is approved] start
			bool has_Operation_role = (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == PaymentXP.BusinessObjects.Constants.ROLE_OPERATIONS);
			//Code added by koshlendra for PXP-9251[Allow OP user to add/update Owner information after application is approved] end
			//Condition midified by Koshlendra for PXP-9251[Allow OP user to add/update Owner information after application is approved]
			if (!(has_credit_underwriting_role || has_Operation_role || (has_special_access_role && !has_sale_support_role)))
			{
				FormHandler.SetControlEditMode(pnlOwners, false);
			}
		}
		//Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message   
		if ((this.EditMode && !this.btnEdit.Enabled && this.btnSave.Enabled) || (this.btnSave.Enabled && !this.EditMode) || (!this.btnCancel.Enabled && !this.EditMode))
		{
			master.UpdateNotification("");
			MerchantFacade.RemoveUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
		}
		/******** End of PXP-2206 **************/
	}

	private void Owners_RenderUI(IList<Owner> owners, CommonUtility.Util.Offices office)
	{
		//DM-5037 ini
		if (pnlOwners.Controls.Count > 0)
		{
			foreach (var control in pnlOwners.Controls)
			{
				if (control is wucOwner)
				{
					var _wucOwner = control as wucOwner;
					_wucOwner.pnlOwner.Enabled = this.EditMode;
				}
			}
		}
		else
		{
			for (int i = 0; i < owners.Count; i++)
			{
				var _owner = owners[i];
				if (_owner.OwnerCatagoryID == Constants.CORPORATEBUSINESS_OWNERCATAGORYID)
				{
					continue;
				}

				wucOwner _wucOwner = LoadControl("../UserControls/wucOwner.ascx") as wucOwner;
				pnlOwners.Controls.Add(_wucOwner);

				_wucOwner.pnlOwner.Enabled = this.EditMode;
				_wucOwner.SetTitle = "Owner " + (i + 1);

				if (isOfficeIrvine)
				{
					((CheckBox)_wucOwner.FindControl("AuthorizedSignature")).Text = "Controller";
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Text = "T&C Signed (Run CBR)";
				}
				else
				{
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Checked = false;
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Visible = false;
				}

				((CheckBox)(_wucOwner).FindControl("PersonalGuarantor")).Enabled = true;

				//Added for phone Country Code and Extention 
				DropDownList CountryDisplayCodeddl0 = ((DropDownList)(_wucOwner).FindControl("HomePhoneCountryCode"));
				((TextBox)((_wucOwner).FindControl("HomeCountryCodeDisplay"))).Text = CountryDisplayCodeddl0.SelectedValue;

				WebMaskEditor HomePhoneNumbertxt = (WebMaskEditor)(_wucOwner).FindControl("HomePhone");
				if (office == CommonUtility.Util.Offices.Irvine)
				{
					HomePhoneNumbertxt.InputMask = "000-000-0000";
					string strHomePhoneNumbertxt = CommonUtility.Util.GetNumbersFromString(_owner.HomePhone);
					HomePhoneNumbertxt.Value = strHomePhoneNumbertxt.ToString();
				}
				else
				{
					HomePhoneNumbertxt.InputMask = "############";
					HomePhoneNumbertxt.Value = _owner.HomePhone;
				}

				FormBinding.BindObjectToControls(_owner, _wucOwner);

				_wucOwner.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
			}

			for (int ownerCount = owners.Count; ownerCount < 6; ownerCount++)
			{
				wucOwner _wucOwner = LoadControl("../UserControls/wucOwner.ascx") as wucOwner;
				pnlOwners.Controls.Add(_wucOwner);

				_wucOwner.pnlOwner.Enabled = this.EditMode;
				_wucOwner.SetTitle = "Owner " + (ownerCount + 1);

				if (isOfficeIrvine)
				{
					((CheckBox)_wucOwner.FindControl("AuthorizedSignature")).Text = "Controller";
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Text = "T&C Signed (Run CBR)";
				}
				else
				{
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Checked = false;
					((CheckBox)_wucOwner.FindControl("CBRWaived")).Visible = false;
				}

				((CheckBox)(_wucOwner).FindControl("PersonalGuarantor")).Enabled = true;

				DropDownList CountryDisplayCodeddl0 = ((DropDownList)(_wucOwner).FindControl("HomePhoneCountryCode"));
				TextBox CountryDisplayCode0 = ((TextBox)(_wucOwner).FindControl("HomeCountryCodeDisplay"));
				CountryDisplayCode0.Text = CountryDisplayCodeddl0.SelectedValue;

				WebMaskEditor HomePhoneNumbertxt = (WebMaskEditor)(_wucOwner).FindControl("HomePhone");
				if (office == CommonUtility.Util.Offices.Irvine)
				{
					HomePhoneNumbertxt.InputMask = "000-000-0000";
				}
				else
				{
					HomePhoneNumbertxt.InputMask = "############";
				}
				_wucOwner.ValueChange += new wucOwner.ValueChangeHandler(wucOwner0_ValueChange);
			}
		}
		//DM-5037 end
	}

	public void ExtractCorporateBusinessFromOwners(MerchantApp agreement)
	{
		wucCorpBuz1.EnableDisableCorporateBusinessSection(agreement.BusinessStructureUID.ToUpper());
		agreement.CorpBuzOwners.IsBusinessOwnedByCorporate = false;
		foreach (Owner item in agreement.Owners)
		{
			if (item.OwnerCatagoryID == Constants.CORPORATEBUSINESS_OWNERCATAGORYID)
			{
				agreement.CorpBuzOwners = item;
				agreement.CorpBuzOwners.IsBusinessOwnedByCorporate =!string.IsNullOrEmpty(agreement.CorpBuzOwners.FullName);
				agreement.Owners.Remove(item);
				break;
			}
		}

	}

	public override void FormClear()
	{
		FormHandler.ClearAllControls(pnlDetail);
	}

	public override bool FormSave()
	{
		bool perform = false;
		MerchantFacade facade = new MerchantFacade();

		try
		{
			if (isValid)
			{
				MerchantApp agreement = null;
				AchMerchant achMerchant = null;
				string OrigStatus = string.Empty;

				if (this.Adding)
					agreement = new MerchantApp();
				else
				{
					agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
				}

				DataMerchantApp data = DataAccess.DataMerchantAppDao;

				//Loading the Owners and Tradereferences again as agreement is a new object here.
				agreement.Owners = data.GetOwners(agreement.MerchantAppUID);
				ExtractCorporateBusinessFromOwners(agreement);
				agreement.TradeReferences = data.GetTradeReferences(agreement.MerchantAppUID);
				agreement.CloneMerchantApp();
				OrigStatus = agreement.StatusUID;

				if (agreement.AchID > 0 && agreement.MerchantAppTypeUID.ToUpper() == Constants.BANK_ACH_ONLY)
				{
					//FMassoud 08-25-2017 : Auto Setting it in get{} Check UserSessions.cs
					UserSessions.ActiveAchMerchant = DataAccess.DataAchMerchantDao.GetAchMerchant(int.Parse(agreement.ID));

					if (UserSessions.ActiveAchMerchant != null)
					{
						UserSessions.ActiveAchMerchant.CloneAchMerchant();
						OrigStatus = UserSessions.ActiveAchMerchant.MerchantStatusUID;
						achMerchant = UserSessions.ActiveAchMerchant;
					}
				}

				List<Owner> ownerc = new List<Owner>();
				for (int i = 0; i < agreement.Owners.Count; i++)
				{
					ownerc.Add((Owner)agreement.Owners[i].Clone());
				}

				Owner corpB = new Owner();
				corpB = agreement.CorpBuzOwners.Clone();

				List<TradeReference> tradeReferencec = new List<TradeReference>();
				for (int i = 0; i < agreement.TradeReferences.Count; i++)
				{
					tradeReferencec.Add((TradeReference)agreement.TradeReferences[i].Clone());
				}

				FormBinding.BindControlsToObject(agreement, pnlDetail);

				UserSessions.CurrentMerchantApp = agreement;

				if (!this.FormDataCheck())
					return false;

				User user = UserSessions.CurrentUser;
				agreement.UserUpdated = user.UserName;

				DropDownList statusuid = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");

				if (!this.Adding)
				{
					//Owner owner1 = new Owner();
					//FormBinding.BindControlsToObject(owner1, wucOwner0);
					//Owner owner2 = new Owner();
					//FormBinding.BindControlsToObject(owner2, wucOwner1);
					//Owner owner3 = new Owner();
					//FormBinding.BindControlsToObject(owner3, wucOwner2);
					//Owner owner4 = new Owner();
					//FormBinding.BindControlsToObject(owner4, wucOwner3);
					////PXP-2883
					//Owner owner5 = new Owner();
					//FormBinding.BindControlsToObject(owner5, wucOwner4);

					//Owner owner6 = new Owner();
					//FormBinding.BindControlsToObject(owner6, wucOwner5);

					Owner CorpBusiness = new Owner();
					FormBinding.BindControlsToObject(CorpBusiness, wucCorpBuz1);

					TradeReference tradereference1 = new TradeReference();
					FormBinding.BindControlsToObject(tradereference1, WucTradeReference0);
					TradeReference tradereference2 = new TradeReference();
					FormBinding.BindControlsToObject(tradereference2, WucTradeReference1);
					TradeReference tradereference3 = new TradeReference();
					FormBinding.BindControlsToObject(tradereference3, WucTradeReference2);

					agreement.UWNotes = ((TextBox)this.Master.FindControl("UWNotesEdit")).Text;
					agreement.FirstTeamNotes = ((TextBox)this.Master.FindControl("FirstTeamNotesEdit")).Text;

					int rows = facade.UpdateMerchantApp(agreement);

					if (rows > 0)
					{
						this.SaveOwners();
						this.SaveTradeReferences();

						for (int i = 0, j = 0; i < ownerc.Count; i++, j++)
						{
							if (j < agreement.MerchantAppClone.Owners.Count)
								agreement.MerchantAppClone.Owners[j] = ownerc[i];
						}

						agreement.MerchantAppClone.CorpBuzOwners = corpB;

						for (int i = 0, j = 0; i < tradeReferencec.Count; i++, j++)
						{
							if (j < agreement.MerchantAppClone.TradeReferences.Count)
								agreement.MerchantAppClone.TradeReferences[j] = tradeReferencec[i];
						}

					}

					FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), agreement.MerchantAppClone, agreement);
				}


				UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(agreement.MerchantAppUID);
				FormHandler.CompleteApplication(agreement, achMerchant, OrigStatus, user.UserName);
				perform = true;
			}
			else
				isValid = true;
		}
		catch (Exception exc)
		{
			throw exc;
		}

		return perform;
	}

	public override void ToggleButtons()
	{
		btnEdit.Enabled = !this.EditMode;
		btnSave.Enabled = this.EditMode;
		btnCancel.Enabled = this.EditMode;
		btnRefresh.Enabled = !this.EditMode;

		this.Master.ToggleMenu(!this.EditMode);

	}

	public override bool FormDataCheck()
	{
		MerchantApp app = UserSessions.CurrentMerchantApp;

		//DropDownList CountryDisplayCodeddl0 = (DropDownList)wucOwner0.FindControl("HomePhoneCountryCode");
		//TextBox CountryDisplayCode0 = (TextBox)wucOwner0.FindControl("HomeCountryCodeDisplay");
		//CountryDisplayCode0.Text = CountryDisplayCodeddl0.SelectedValue;

		//DropDownList CountryDisplayCodeddl1 = (DropDownList)wucOwner1.FindControl("HomePhoneCountryCode");
		//TextBox CountryDisplayCode1 = (TextBox)wucOwner1.FindControl("HomeCountryCodeDisplay");
		//CountryDisplayCode1.Text = CountryDisplayCodeddl1.SelectedValue;

		//DropDownList CountryDisplayCodeddl2 = (DropDownList)wucOwner2.FindControl("HomePhoneCountryCode");
		//TextBox CountryDisplayCode2 = (TextBox)wucOwner2.FindControl("HomeCountryCodeDisplay");
		//CountryDisplayCode2.Text = CountryDisplayCodeddl2.SelectedValue;

		//DropDownList CountryDisplayCodeddl3 = (DropDownList)wucOwner3.FindControl("HomePhoneCountryCode");
		//TextBox CountryDisplayCode3 = (TextBox)wucOwner3.FindControl("HomeCountryCodeDisplay");
		//CountryDisplayCode3.Text = CountryDisplayCodeddl3.SelectedValue;

		//DM-5037 ini
		foreach (var control in pnlOwners.Controls)
		{
			if (control is wucOwner)
			{
				var _wucOwner = control as wucOwner;
				DropDownList CountryDisplayCodeddl = (DropDownList)_wucOwner.FindControl("HomePhoneCountryCode");
				TextBox CountryDisplayCode = (TextBox)_wucOwner.FindControl("HomeCountryCodeDisplay");
				CountryDisplayCode.Text = CountryDisplayCodeddl.SelectedValue;
			}
		}
		//DM-5037 end

		DropDownList PhoneNumberCountryCodeddl0 = (DropDownList)WucTradeReference0.FindControl("PhoneNumberCountryCode");
		TextBox PhoneNumberCountryCodeDisplay0 = (TextBox)WucTradeReference0.FindControl("PhoneNumberCountryCodeDisplay");
		PhoneNumberCountryCodeDisplay0.Text = PhoneNumberCountryCodeddl0.SelectedValue;

		DropDownList PhoneNumberCountryCodeddl1 = (DropDownList)WucTradeReference1.FindControl("PhoneNumberCountryCode");
		TextBox PhoneNumberCountryCodeDisplay1 = (TextBox)WucTradeReference1.FindControl("PhoneNumberCountryCodeDisplay");
		PhoneNumberCountryCodeDisplay1.Text = PhoneNumberCountryCodeddl1.SelectedValue;

		DropDownList PhoneNumberCountryCodeddl2 = (DropDownList)WucTradeReference2.FindControl("PhoneNumberCountryCode");
		TextBox PhoneNumberCountryCodeDisplay2 = (TextBox)WucTradeReference2.FindControl("PhoneNumberCountryCodeDisplay");
		PhoneNumberCountryCodeDisplay2.Text = PhoneNumberCountryCodeddl2.SelectedValue;

		//PXP-4113 TODO Update MerchantApp Owners
		GetOwnersDataFromUI(ref app);           //set values from UI to the MerchantApp object

		//Fmassoud 2017.08.28 Sending New Status to Formhandler        
		DropDownList _statusUID = WucBusinessInfo1.isACHonly ? (DropDownList)WucBusinessInfo1.FindControl("ACHStatusUID") : (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
		IList<string> message = FormHandler.MerchantDataCheck(app, true, this.Adding, _statusUID.SelectedValue.ToUpper(), UserSessions.ActiveAchMerchant);

		//PXP-4113 
		app = UserSessions.CurrentMerchantApp;  //Set the original values in the DB back to the object

		//Added for phone Country Code and Extention 
		//WebMaskEditor HomePhoneExttxt = (WebMaskEditor)wucOwner0.FindControl("HomePhoneExt");
		//if (HomePhoneExttxt.Text.Trim().Length > 6)
		//{
		//    this.Master.AddMessageError("Please enter 6 digit Owners 1 extention.");
		//}

		//WebMaskEditor HomePhonetxt = (WebMaskEditor)wucOwner0.FindControl("HomePhone");
		//if (app.Office == CommonUtility.Util.Offices.Irvine)
		//{
		//    string strHomePhonetxt = CommonUtility.Util.GetNumbersFromString(HomePhonetxt.Text); //HomePhonetxt.Text.Replace("-", "").ToString();
		//    if ((strHomePhonetxt.Trim().Length >= 1 || HomePhoneExttxt.Text.Trim().Length > 0) && strHomePhonetxt.Trim().Length < 10)
		//    {
		//        this.Master.AddMessageError("Please enter 10 digit Owners 1 phone number.");
		//    }
		//}
		//if (HomePhonetxt.Text.Trim().Length > 12)
		//{
		//    this.Master.AddMessageError("Please enter at the max 12 digit Owners 1 phone number.");
		//}

		//WebMaskEditor HomePhoneExttxt1 = (WebMaskEditor)wucOwner1.FindControl("HomePhoneExt");
		//if (HomePhoneExttxt1.Text.Trim().Length > 6)
		//{
		//    this.Master.AddMessageError("Please enter 6 digit Owners 2 extention.");
		//}

		//WebMaskEditor HomePhonetxt1 = (WebMaskEditor)wucOwner1.FindControl("HomePhone");
		//if (app.Office == CommonUtility.Util.Offices.Irvine)
		//{
		//    string strHomePhonetxt1 = CommonUtility.Util.GetNumbersFromString(HomePhonetxt1.Text); //HomePhonetxt1.Text.Replace("-", "").ToString();
		//    if ((strHomePhonetxt1.Trim().Length >= 1 || HomePhoneExttxt1.Text.Trim().Length > 0) && strHomePhonetxt1.Trim().Length < 10)
		//    {
		//        this.Master.AddMessageError("Please enter 10 digit Owners 2 phone number.");
		//    }
		//}
		//if (HomePhonetxt1.Text.Trim().Length > 12)
		//{
		//    this.Master.AddMessageError("Please enter at the max 12 digit Owners 2 phone number.");
		//}

		//WebMaskEditor HomePhoneExttxt2 = (WebMaskEditor)wucOwner2.FindControl("HomePhoneExt");
		//if (HomePhoneExttxt2.Text.Trim().Length > 6)
		//{
		//    this.Master.AddMessageError("Please enter 6 digit Owners 3 extention.");
		//}

		//WebMaskEditor HomePhonetxt2 = (WebMaskEditor)wucOwner2.FindControl("HomePhone");
		//if (app.Office == CommonUtility.Util.Offices.Irvine)
		//{
		//    string strHomePhonetxt2 = CommonUtility.Util.GetNumbersFromString(HomePhonetxt2.Text); //HomePhonetxt2.Text.Replace("-", "").ToString();
		//    if ((strHomePhonetxt2.Trim().Length >= 1 || HomePhoneExttxt2.Text.Trim().Length > 0) && strHomePhonetxt2.Trim().Length < 10)
		//    {
		//        this.Master.AddMessageError("Please enter 10 digit Owners 3 phone number.");
		//    }
		//}
		//if (HomePhonetxt2.Text.Trim().Length > 12)
		//{
		//    this.Master.AddMessageError("Please enter at the max 12 digit Owners 3 phone number.");
		//}

		//WebMaskEditor HomePhoneExttxt3 = (WebMaskEditor)wucOwner3.FindControl("HomePhoneExt");
		//if (HomePhoneExttxt3.Text.Trim().Length > 6)
		//{
		//    this.Master.AddMessageError("Please enter 6 digit Owners 4 extention.");
		//}

		//WebMaskEditor HomePhonetxt3 = (WebMaskEditor)wucOwner3.FindControl("HomePhone");
		//if (app.Office == CommonUtility.Util.Offices.Irvine)
		//{
		//    string strHomePhonetxt3 = CommonUtility.Util.GetNumbersFromString(HomePhonetxt3.Text); //HomePhonetxt3.Text.Replace("-", "").ToString();
		//    if ((strHomePhonetxt3.Trim().Length >= 1 || HomePhoneExttxt3.Text.Trim().Length > 0) && strHomePhonetxt3.Trim().Length < 10)
		//    {
		//        this.Master.AddMessageError("Please enter 10 digit Owners 4 phone number.");
		//    }
		//}
		//if (HomePhonetxt3.Text.Trim().Length > 12)
		//{
		//    this.Master.AddMessageError("Please enter at the max 12 digit Owners 4 phone number.");
		//}

		//DM-5037 ini
		var count = 1;
		foreach (var control in pnlOwners.Controls)
		{
			if (control is wucOwner)
			{
				var _wucOwner = control as wucOwner;

				WebMaskEditor HomePhoneExttxt = (WebMaskEditor)_wucOwner.FindControl("HomePhoneExt");
				if (HomePhoneExttxt.Text.Trim().Length > 6)
				{
					this.Master.AddMessageError("Please enter 6 digit Owners " + count + " extention.");
				}

				WebMaskEditor HomePhonetxt = (WebMaskEditor)_wucOwner.FindControl("HomePhone");
				if (app.Office == CommonUtility.Util.Offices.Irvine)
				{
					string strHomePhonetxt = CommonUtility.Util.GetNumbersFromString(HomePhonetxt.Text); //HomePhonetxt2.Text.Replace("-", "").ToString();
					if ((strHomePhonetxt.Trim().Length >= 1 || HomePhoneExttxt.Text.Trim().Length > 0) && strHomePhonetxt.Trim().Length < 10)
					{
						this.Master.AddMessageError("Please enter 10 digit Owners " + count + " phone number.");
					}
				}
				if (HomePhonetxt.Text.Trim().Length > 12)
				{
					this.Master.AddMessageError("Please enter at the max 12 digit Owners " + count + " phone number.");
				}
				count++;
			}
		}
		//DM-5037 end


		WebMaskEditor PhoneExttxt = (WebMaskEditor)WucTradeReference0.FindControl("PhoneNumberExt");
		if (PhoneExttxt.Text.Trim().Length > 6)
		{
			this.Master.AddMessageError("Please enter 6 digit Trade 1 extention.");
		}

		WebMaskEditor Phonetxt = (WebMaskEditor)WucTradeReference0.FindControl("PhoneNumber");
		if (app.Office == CommonUtility.Util.Offices.Irvine)
		{
			string strPhonetxt = CommonUtility.Util.GetNumbersFromString(Phonetxt.Text); //Phonetxt.Text.Replace("-", "").ToString();
			if ((strPhonetxt.Trim().Length >= 1 || PhoneExttxt.Text.Trim().Length > 0) && strPhonetxt.Trim().Length < 10)
			{
				this.Master.AddMessageError("Please enter 10 digit Trade 1 phone number.");
			}
		}

		if (Phonetxt.Text.Trim().Length > 12)
		{
			this.Master.AddMessageError("Please enter at the max 12 digit Trade 1 phone number.");
		}

		WebMaskEditor PhoneExttxt1 = (WebMaskEditor)WucTradeReference1.FindControl("PhoneNumberExt");
		if (PhoneExttxt1.Text.Trim().Length > 6)
		{
			this.Master.AddMessageError("Please enter 6 digit Trade 2 extention.");
		}

		WebMaskEditor Phonetxt1 = (WebMaskEditor)WucTradeReference1.FindControl("PhoneNumber");
		if (app.Office == CommonUtility.Util.Offices.Irvine)
		{
			string strPhonetxt1 = CommonUtility.Util.GetNumbersFromString(Phonetxt1.Text); //Phonetxt1.Text.Replace("-", "").ToString();
			if ((strPhonetxt1.Trim().Length >= 1 || PhoneExttxt1.Text.Trim().Length > 0) && strPhonetxt1.Trim().Length < 10)
			{
				this.Master.AddMessageError("Please enter 10 digit Trade 2 phone number.");
			}
		}

		if (Phonetxt1.Text.Trim().Length > 12)
		{
			this.Master.AddMessageError("Please enter at the max 12 digit Trade  phone number.");
		}

		WebMaskEditor PhoneExttxt2 = (WebMaskEditor)WucTradeReference2.FindControl("PhoneNumberExt");
		if (PhoneExttxt2.Text.Trim().Length > 6)
		{
			this.Master.AddMessageError("Please enter 6 digit Trade 3 extention.");
		}

		WebMaskEditor Phonetxt2 = (WebMaskEditor)WucTradeReference2.FindControl("PhoneNumber");
		if (app.Office == CommonUtility.Util.Offices.Irvine)
		{
			string strPhonetxt2 = CommonUtility.Util.GetNumbersFromString(Phonetxt2.Text); //Phonetxt2.Text.Replace("-", "").ToString();
			if ((strPhonetxt2.Trim().Length >= 1 || PhoneExttxt2.Text.Trim().Length > 0) && strPhonetxt2.Trim().Length < 10)
			{
				this.Master.AddMessageError("Please enter 10 digit Trade 3 phone number.");
			}
		}

		if (Phonetxt2.Text.Trim().Length > 12)
		{
			this.Master.AddMessageError("Please enter at the max 12 digit Trade 3 phone number.");
		}
		WebMaskEditor CorpBuz_TxtHomePhoneExt = (WebMaskEditor)wucCorpBuz1.FindControl("HomePhoneExt");
		WebMaskEditor CorpBuz_TxtHomePhone = (WebMaskEditor)wucCorpBuz1.FindControl("HomePhone");
		CheckBox CorpBuz_CbxIsBusinessOwnedByCorporate = (CheckBox)wucCorpBuz1.FindControl("IsBusinessOwnedByCorporate");
		TextBox CorpBuz_TxtFullName = (TextBox)wucCorpBuz1.FindControl("FullName");
		if (app.Office == CommonUtility.Util.Offices.Irvine)
		{
			string strCorpBuz_TxtHomePhone = CommonUtility.Util.GetNumbersFromString(CorpBuz_TxtHomePhone.Text);
			if ((strCorpBuz_TxtHomePhone.Trim().Length >= 1 || CorpBuz_TxtHomePhoneExt.Text.Trim().Length > 0) && strCorpBuz_TxtHomePhone.Trim().Length < 10)
			{
				this.Master.AddMessageError("Please enter 10 digit Corporate Business phone number.");
			}
		}
		//if (HomePhonetxt.Text.Trim().Length > 12)
		//{
		//    this.Master.AddMessageError("Please enter at the max 12 digit Corporate Business phone number.");
		//}
		if (CorpBuz_CbxIsBusinessOwnedByCorporate.Checked && string.IsNullOrEmpty(CorpBuz_TxtFullName.Text))
		{
			this.Master.AddMessageError("All the data for Corporate Business section is required. Please enter data for all the fields..");
		}

		//Niranjan Bug fix PXP-7330
		decimal percentageOwnerValue = decimal.Zero;
		//alamadrid DM-4646
		int pgcount = 0;
		foreach (var control in pnlOwners.Controls)
		{
			if (control is wucOwner)
			{
				//Niranjan Bug fix PXP-7330
				var _wucOwner = control as wucOwner;
				WebPercentEditor PercentOwnership = (WebPercentEditor)_wucOwner.FindControl("PercentOwnership");
				percentageOwnerValue += Convert.ToDecimal(PercentOwnership.Value);

				//alamadrid DM-4646
				CheckBox PG_checkbox = (CheckBox)_wucOwner.FindControl("PersonalGuarantor");
				pgcount += Convert.ToInt32(PG_checkbox.Checked);
			}
		}
		WebPercentEditor CorpBuz_PercentOwnership = (WebPercentEditor)wucCorpBuz1.FindControl("PercentOwnership");
		percentageOwnerValue = percentageOwnerValue + Convert.ToDecimal(CorpBuz_PercentOwnership.Value);
		if (percentageOwnerValue > 100)
		{
			this.Master.AddMessageError("Total of Ownership% should not be greater than 100%");

		}

		////alamadrid DM-4646
		//int pgcount = 0;
		//for (int i = 0; i < 6; i++)
		//{
		//    CheckBox PG_checkbox = ((CheckBox)pnlOwners.FindControl(("wucOwner" + i)).FindControl("PersonalGuarantor"));
		//    pgcount += Convert.ToInt32(PG_checkbox.Checked);
		//}

		if (pgcount > 1)
			this.Master.AddMessageError("Only one Owner can be designated as the Personal Guarantor");

		if (message.Count > 0)
		{
			foreach (string mess in message)
				this.Master.AddMessageError(mess);
		}

		if (this.Master.ErrorCount() == 0)
			return true;
		else
		{
			if (app.MerchantAppClone != null)
			{
				ListHandler.ListFindItem(((DropDownList)WucBusinessInfo1.FindControl("StatusUID")), app.MerchantAppClone.StatusUID);
				app.StatusUID = app.MerchantAppClone.StatusUID;
			}

			return false;
		}
	}

	public override void FormNew()
	{
		this.FormClear();
		this.Adding = true;
		this.EditMode = true;
		FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
		this.ToggleButtons();

		DropDownList cbo = (DropDownList)WucBusinessInfo1.FindControl("StatusUID");
		if (cbo != null)
			ListHandler.ListFindItem(cbo, "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409"); //Application Processing Received Status
	}

	public override bool FormDelete()
	{
		return false;
	}

	public override void FormCancel()
	{
		this.EditMode = false;
		MerchantFacade facade = new MerchantFacade();
		UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
		FormShow(this.UID);
		this.Adding = false;
		this.ToggleButtons();
	}

	public bool SaveTradeReferences()
	{
		try
		{
			MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
			DataMerchantApp data = DataAccess.DataMerchantAppDao;

			//Save reference 1
			TradeReference trade = null;
			TradeReference clone = null;
			string TradeReferenceID = null;

			if (agreement.TradeReferences.Count > 0)
			{
				trade = (TradeReference)agreement.TradeReferences[0];
				clone = (TradeReference)trade.Clone();
				TradeReferenceID = trade.TradeReferenceID;
			}
			else
			{
				trade = new TradeReference();
				clone = new TradeReference();
			}

			FormBinding.BindControlsToObject(trade, WucTradeReference0);

			trade.Position = 1;
			trade.MerchantAppsUID = this.UID;
			trade.PhoneNumber = ((WebMaskEditor)this.WucTradeReference0.FindControl("PhoneNumber")).Value.ToString().Trim();

			if (string.IsNullOrWhiteSpace(TradeReferenceID))
			{
				trade.UserCreated = UserSessions.CurrentUser.UserName;
				data.InsertTradeReference(trade);
			}
			else
			{
				trade.UserUpdated = UserSessions.CurrentUser.UserName;
				if (string.IsNullOrEmpty(trade.TradeReferenceID))
					trade.TradeReferenceID = TradeReferenceID;
				data.UpdateTradeReference(trade);
			}

			//Save reference 2
			trade = null;
			clone = null;
			TradeReferenceID = null;

			if (agreement.TradeReferences.Count > 1)
			{
				trade = (TradeReference)agreement.TradeReferences[1];
				clone = (TradeReference)trade.Clone();
				TradeReferenceID = trade.TradeReferenceID;
			}
			else
			{
				trade = new TradeReference();
				clone = new TradeReference();
			}

			FormBinding.BindControlsToObject(trade, WucTradeReference1);

			trade.Position = 2;
			trade.MerchantAppsUID = this.UID;
			trade.PhoneNumber = ((WebMaskEditor)this.WucTradeReference1.FindControl("PhoneNumber")).Value.ToString().Trim();

			if (string.IsNullOrWhiteSpace(TradeReferenceID))
			{
				trade.UserCreated = UserSessions.CurrentUser.UserName;
				data.InsertTradeReference(trade);
			}
			else
			{
				trade.UserUpdated = UserSessions.CurrentUser.UserName;
				if (string.IsNullOrEmpty(trade.TradeReferenceID))
					trade.TradeReferenceID = TradeReferenceID;
				data.UpdateTradeReference(trade);
			}

			//Save reference 3
			trade = null;
			clone = null;
			TradeReferenceID = null;

			if (agreement.TradeReferences.Count > 2)
			{
				trade = (TradeReference)agreement.TradeReferences[2];
				clone = (TradeReference)trade.Clone();
				TradeReferenceID = trade.TradeReferenceID;
			}
			else
			{
				trade = new TradeReference();
				clone = new TradeReference();
			}

			FormBinding.BindControlsToObject(trade, WucTradeReference2);

			trade.Position = 3;
			trade.MerchantAppsUID = this.UID;
			trade.PhoneNumber = ((WebMaskEditor)this.WucTradeReference2.FindControl("PhoneNumber")).Value.ToString().Trim();

			if (string.IsNullOrWhiteSpace(TradeReferenceID))
			{
				trade.UserCreated = UserSessions.CurrentUser.UserName;
				data.InsertTradeReference(trade);
			}
			else
			{
				trade.UserUpdated = UserSessions.CurrentUser.UserName;
				if (string.IsNullOrEmpty(trade.TradeReferenceID))
					trade.TradeReferenceID = TradeReferenceID;
				data.UpdateTradeReference(trade);
			}


			return true;
		}
		catch (Exception exc)
		{
			throw exc;
		}
	}

	public bool SaveOwners()
	{
		try
		{
			MerchantApp agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
			ExtractCorporateBusinessFromOwners(agreement);

			DataMerchantApp data = DataAccess.DataMerchantAppDao;

			//DM-5037 ini
			Owner owner = null;
			Owner clone = null;
			string ownerID = string.Empty;
			string Corporate_Province = string.Empty;
			var Position = 0;
			var LastPositon = pnlOwners.Controls.Count;
			foreach (var control in pnlOwners.Controls)
			{
				if (control is wucOwner)
				{
					var _wucOwner = control as wucOwner;
					Position++;

					//Save owner 
					owner = null;
					clone = null;
					ownerID = ((TextBox)_wucOwner.FindControl("OwnerID")).Text;

					if (agreement.Owners.Count > 0 && !string.IsNullOrWhiteSpace(ownerID))
					{
						owner = (Owner)agreement.Owners.Where(p => p.OwnerID == ownerID).FirstOrDefault();
						clone = (Owner)owner.Clone();
						ownerID = owner.OwnerID;
					}
					else
					{
						owner = new Owner();
						clone = new Owner();
					}

					FormBinding.BindControlsToObject(owner, _wucOwner);

					owner.Position = Position;
					owner.OwnerCatagoryID = Constants.REGULAROWNER_OWNERCATAGORYID;
					owner.UserUpdated = UserSessions.CurrentUser.UserName;
					owner.MerchantAppsUID = this.UID;
					if (!owner.Country.Trim().ToUpper().Equals("US"))
					{
						TextBox txtProvince = (TextBox)_wucOwner.FindControl("Province");
						owner.State = txtProvince.Text;
						Corporate_Province = owner.State;
					}
					if (string.IsNullOrWhiteSpace(ownerID))
					{
						owner.UserCreated = UserSessions.CurrentUser.UserName;
						data.InsertOwner(owner);
					}
					else
					{
						if (string.IsNullOrWhiteSpace(owner.OwnerID))
							owner.OwnerID = ownerID;
						data.UpdateOwner(owner);
						FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, Convert.ToInt32(agreement.ID), clone, owner);
					}
				}
			}


			//Save Corporate Owner
				if (agreement.CorpBuzOwners.OwnerCatagoryID == Constants.CORPORATEBUSINESS_OWNERCATAGORYID)
				{
					owner = (Owner)agreement.CorpBuzOwners;
					clone = (Owner)owner.Clone();
					ownerID = owner.OwnerID;
				}
				else
				{
					owner = new Owner();
					clone = new Owner();
					owner.Position = ++Position;
					ownerID = null;
				}

				FormBinding.BindControlsToObject(owner, wucCorpBuz1);
				
				owner.OwnerCatagoryID = Constants.CORPORATEBUSINESS_OWNERCATAGORYID;
				owner.HomePhone = ((WebMaskEditor)this.wucCorpBuz1.FindControl("HomePhone")).Value.ToString().Trim();

				owner.UserUpdated = UserSessions.CurrentUser.UserName;
				owner.MerchantAppsUID = this.UID;
				if (!owner.Country.Trim().ToUpper().Equals("US"))
				{
					owner.State = Corporate_Province;
				}
				if (string.IsNullOrWhiteSpace(ownerID))
				{
					owner.UserCreated = UserSessions.CurrentUser.UserName;
					data.InsertOwner(owner);
				}
				else
				{
					if (string.IsNullOrWhiteSpace(owner.OwnerID))
						owner.OwnerID = ownerID;
					if (!string.IsNullOrEmpty(owner.FullName))
						data.UpdateOwner(owner);
					else
						data.DeleteOwner(owner);

					FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, Convert.ToInt32(agreement.ID), clone, owner);
				}
			//DM-5037 end
			return true;
		}
		catch (Exception exc)
		{
			throw exc;
		}
	}

	private void CloseForm()
	{
		string url = string.Empty;

		if (Request.QueryString["PostBackURL"] != null)
			url = Convert.ToString(Request.QueryString["PostBackURL"]);

		if (!string.IsNullOrWhiteSpace(url))
			Response.Redirect(url);
		else
			Response.Redirect("frmMerchantSearch.aspx");
	}

	protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
	{
		WebImageButton btn = (WebImageButton)sender;

		string url = string.Empty;
		switch (btn.Text)
		{
			case "Add":
				this.FormNew();
				break;

			case "Save":
				if (this.FormSave())
				{
					this.EditMode = false;
					this.ToggleButtons();
					this.Adding = false;
					url = "~/SecureMerchantManagementForms/frmMerchantOwners.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID + "&Adding=false";
					Response.Redirect(url);
				}
				break;

			case "Refresh":
				MerchantFacade facade = new MerchantFacade();
				UserSessions.CurrentMerchantApp = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);
				this.FormShow(this.UID);
				break;

			case "Cancel":
				if (string.IsNullOrWhiteSpace(this.UID))
					this.CloseForm();
				else
					this.FormCancel();
				break;

			case "Close":
				this.CloseForm();
				break;

			case "Delete":
				if (this.FormDelete())
					Response.Redirect("frmLeads.aspx");
				break;

			case "Upload Document":
				Response.Redirect("~/DocumentUpload/frmDocumentUpload.aspx?MerchantAppUID=" + this.UID);
				break;

			case "PDF":
				break;

			case "Edit":
				{
					this.EditMode = true;
					this.FormShow(this.UID);
					//Modified by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message   
					string firstUserdetail = MerchantFacade.GetFirstUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID);
					if (!string.IsNullOrWhiteSpace(firstUserdetail))
					{
						string notification = firstUserdetail + " is currently editing this ZID. Please ensure you will not overwrite each other's work.";
						Page.ClientScript.RegisterStartupScript(this.GetType(), "Notification", "alert(" + '"' + notification + '"' + ");", true);
						MasterPageMerchant master = (MasterPageMerchant)this.Master;
						master.UpdateNotification(firstUserdetail + " is editing this ZID.");
					}
					MerchantFacade.AddNewUserEditingForZID(UserSessions.CurrentMerchantApp.MerchantAppUID, UserSessions.CurrentUser.FirstLastName + ": " + UserSessions.CurrentUser.OfficeName);
					/******** End of PXP-2206 **************/
					this.ToggleButtons();
				}
				break;
		}
	}


	/// <summary>
	/// This method creates an Owner object from the wucOwner control to send to 3DE service to validate
	/// </summary>
	/// <param name="listWucOwner"></param>
	/// <param name="ownerCount"></param>
	public void GetOwnersDataFromUI(ref MerchantApp app)
	{
		//List<wucOwner> listWucOwner = new List<wucOwner>();
		//listWucOwner.Add(wucOwner0);
		//listWucOwner.Add(wucOwner1);
		//listWucOwner.Add(wucOwner2);
		//listWucOwner.Add(wucOwner3);
		//listWucOwner.Add(wucOwner4);
		//listWucOwner.Add(wucOwner5);
		//int counter = 0;
		if (app.Owners == null) app.Owners = new List<Owner>();
		if (app.Owners.Count > 0)
		{
			//foreach (Owner ownr in app.Owners)
			//{
			//    if (counter < 5)
			//    {
			//        TextBox txtFname = (TextBox)listWucOwner[counter].FindControl("FirstName");
			//        TextBox txtLname = (TextBox)listWucOwner[counter].FindControl("LastName");
			//        TextBox txtMname = (TextBox)listWucOwner[counter].FindControl("MiddleName");

			//        //string strlname = txtLname.Text;
			//        WebMaskEditor wmeHomePhone = (WebMaskEditor)listWucOwner[counter].FindControl("HomePhone");
			//        //string strhomePhone = Convert.ToString(wmeHomePhone.Value);
			//        TextBox txtCity = (TextBox)listWucOwner[counter].FindControl("City");
			//        //string strcity = txtCity.Text;
			//        DropDownList ddlState = (DropDownList)listWucOwner[counter].FindControl("State");
			//        //string strState = ddlState.SelectedValue;
			//        TextBox txtZip = (TextBox)listWucOwner[counter].FindControl("Zip");
			//        //string strZip = txtZip.Text;


			//        //Add values to UserSessions.MerchantApp
			//        ownr.FirstName = txtFname.Text;
			//        ownr.LastName = txtLname.Text;
			//        ownr.MiddleName = txtMname.Text;
			//        ownr.HomePhone = Convert.ToString(wmeHomePhone.Value);
			//        ownr.City = txtCity.Text;
			//        ownr.State = ddlState.SelectedIndex.Equals(0) ? string.Empty : ddlState.SelectedValue;
			//        ownr.Zip = txtZip.Text;
			//    }
			//    counter++;
			//}

			//for (int ownerCount = counter; ownerCount < listWucOwner.Count; ownerCount++)
			//{
			//    Owner ownr = new Owner();
			//    TextBox txtFname = (TextBox)listWucOwner[ownerCount].FindControl("FirstName");
			//    TextBox txtLname = (TextBox)listWucOwner[ownerCount].FindControl("LastName");
			//    TextBox txtMname = (TextBox)listWucOwner[ownerCount].FindControl("MiddleName");
			//    WebMaskEditor wmeHomePhone = (WebMaskEditor)listWucOwner[ownerCount].FindControl("HomePhone");
			//    TextBox txtCity = (TextBox)listWucOwner[ownerCount].FindControl("City");
			//    DropDownList ddlState = (DropDownList)listWucOwner[ownerCount].FindControl("State");
			//    TextBox txtZip = (TextBox)listWucOwner[ownerCount].FindControl("Zip");

			//    ownr.FirstName = txtFname.Text;
			//    ownr.LastName = txtLname.Text;
			//    ownr.MiddleName = txtMname.Text;
			//    ownr.HomePhone = Convert.ToString(wmeHomePhone.Value);
			//    ownr.City = txtCity.Text;
			//    ownr.State = ddlState.SelectedIndex.Equals(0) ? string.Empty : ddlState.SelectedValue;
			//    ownr.Zip = txtZip.Text;

			//    app.Owners.Add(ownr);

			//}

			//DM-5037 ini
			foreach (var control in pnlOwners.Controls)
			{
				if (control is wucOwner)
				{
					var _wucOwner = control as wucOwner;
					string ownerID = ((TextBox)_wucOwner.FindControl("OwnerID")).Text;
					if (app.Owners.Count > 0 && !string.IsNullOrWhiteSpace(ownerID))
					{
						Owner owner = app.Owners.Where(p => p.OwnerID == ownerID).FirstOrDefault();
						if (owner != null)
						{
							FormBinding.BindControlsToObject(owner, _wucOwner);
						}
					}
					else
					{
						Owner owner = new Owner();
						FormBinding.BindControlsToObject(owner, _wucOwner);
						app.Owners.Add(owner);
					}
				}
			}
			//DM-5037 end
		}
	}
}

