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
using PaymentXP.Facade;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Infragistics.WebUI.WebDataInput;
using Infragistics.WebUI.WebSchedule;
using System.IO;
using System.Collections.Generic;

public partial class UserControls_NewApp : System.Web.UI.UserControl
{
    public bool EditMode
    {
        get
        {
            if (ViewState["EditMode"] == null)
                return false;
            else
                return Convert.ToBoolean(ViewState["EditMode"]);
        }
        set { ViewState["EditMode"] = value; }
    }

    public bool Adding
    {
        get { return Convert.ToBoolean(ViewState["Adding"]); }
        set { ViewState["Adding"] = Convert.ToBoolean(value); }
    }

    public string UID
    {
        get { return Convert.ToString(ViewState["UID"]); }
        set { ViewState["UID"] = Convert.ToString(value); }
    }

    public bool AllFields
    {
        get { return Convert.ToBoolean(ViewState["fields"]); }
        set { ViewState["fields"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        FormHandler.SetSecurity(this.Page);

        if (!this.IsPostBack)
        {
            WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

            LookupTableHandler.LoadEquipmentTypes(cboType, false);
            LookupTableHandler.LoadEquipmentMaker(cboMaker, false);
            LookupTableHandler.LoadFrontEndPlatforms(AuthPlatformUID, false);
            LookupTableHandler.LoadBackEndPlatforms(SettlePlatformUID, false);


            //LookupTableHandler.LoadAgentsNew(AgentUID, false);

            //if (UserSessions.CurrentUser.IsAgent)
            //    LookupTableHandler.LoadAgentnSubAgents(AgentUID, false, UserSessions.CurrentUser.AgentID);
            //else
            //    LookupTableHandler.LoadAgents(AgentUID, false);

            LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, false);
            LookupTableHandler.LoadBusinessStructures(BusinessStructureUID, false);
            LookupTableHandler.LoadReturnPolicies(ReturnPoliciesUID, false);
            LookupTableHandler.LoadApplicationTypes(ApplicationTypeUID, false);
            btnLookup.Attributes.Add("onclick", "return ShowEquipment()");
            if (!String.IsNullOrEmpty(this.Request.QueryString["MerchantAppUID"]))
            {
                this.EditMode = false;
                this.FormShow(this.Request.QueryString["MerchantAppUID"]);
                if (!String.IsNullOrEmpty(this.Request.QueryString["pdf"]))
                    viewPdf();
            }
            else
            {
                UserSessions.CurrentMerchantApp = null;
                btnPDF.Visible = false;
                if (!String.IsNullOrEmpty(this.Request.QueryString["LeadID"]))
                {
                    this.EditMode = true;
                    this.ShowForm(this.Request.QueryString["LeadID"]);
                }
            }

        }
    }

    private void ShowForm(string ID)
    {
        DataLead data = DataAccess.DataLeadDao;
        Lead lead = data.GetLead(ID);

        FormBinding.BindObjectToControls(lead, pnlDetail);
        //UserSessions.CurrentLeadUID = ID;

        BusinessDBAName.Text = lead.DBAName;
        //AgentUID.SelectedValue = lead.AgentID;

        BusinessStructureUID.SelectedItem.Text = lead.BusinessType;
        BusinessContact.Text = lead.ContactName;
        BusinessContactTitle.Text = lead.ContactTitle;
        BusinessPhone.Text = lead.PhoneNumber;
        BusinessFax.Text = lead.FaxNumber;
        BusinessEmailAddress.Text = lead.Email;
        BusinessAddress.Text = lead.Address1;
        BusinessCity.Text = lead.City;
        BusinessZip.Text = lead.ZipCode;
        BusinessState.SelectedItem.Text = lead.State;
        Referral.Text = lead.Referral;
        BusinessWebsite.Text = lead.Url;
        BusinessDBAPhone.Text = lead.CellNumber;
    }

    protected void btnSave_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {

        this.Adding = !(UserSessions.CurrentMerchantApp != null && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.MerchantAppUID));

        this.EditMode = true;

        MerchantApp ma = FormSave();

        if (ma != null && CommonUtility.Util.IsValidGuid(ma.MerchantAppUID))
        {
            FormShow(this.UID);
            lblError.Visible = true;
            lblError.Text = "  Application is successfully saved.";

            Dictionary<string, string> diG = new Dictionary<string, string>();
            diG["Adding"] = "false";
            diG["MerchantAppUID"] = ma.MerchantAppUID;

            string url = WebUtil.GetMyUrl(diG, false);
            Response.Redirect(url);

        }
        else
        {
            lblError.Visible = true;
            lblError.Text = " Could not save application.";
        }
    }

    protected void btnSubmit_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        if (UserSessions.CurrentMerchantApp != null && !string.IsNullOrEmpty(UserSessions.CurrentMerchantApp.MerchantAppUID))
            this.Adding = false;
        else
            this.Adding = true;
        this.EditMode = false;

        if (Page.IsValid)
        {
            MerchantApp ma = FormSave();

            if (ma != null && CommonUtility.Util.IsValidGuid(ma.MerchantAppUID))
            {
                Dictionary<string, string> diG = new Dictionary<string, string>();
                diG["Adding"] = "false";
                diG["MerchantAppUID"] = ma.MerchantAppUID;

                string url = WebUtil.GetMyUrl(diG, false);
                Response.Redirect(url);
            }

        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "  New Application is unsuccessful";
        }
    }

    protected void btnCancel_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        FormCancel();
    }

    public void FormShow(string ID)
    {
        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(ID);
        this.UID = agreement.MerchantAppUID;

        if (agreement != null)
        {
            agreement.Owners = DataAccess.DataMerchantAppDao.GetOwners(agreement.MerchantAppUID);
            agreement.TradeReferences = DataAccess.DataMerchantAppDao.GetTradeReferences(agreement.MerchantAppUID);
        }

        btnPDF.Visible = true;
        FormBinding.BindObjectToControls(agreement, pnlDetail);
        UserSessions.CurrentMerchantApp = agreement;

        if (agreement.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_DRAFT))//"AW - Pending"))
        {
            FormHandler.SetControlEditMode(pnlDetail, true);
            btnSubmit.Enabled = btnCancel.Enabled = btnSave.Enabled = true;
            pnlBank.Visible = pnlOwners.Visible = pnlEquip.Visible = true;
        }
        else
        {
            this.AccountNumber.Text = agreement.AccountNumberMask;
            FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
            btnSubmit.Enabled = btnCancel.Enabled = btnSave.Enabled = false;
            if (!AllFields)
                pnlBank.Visible = pnlOwners.Visible = pnlEquip.Visible = false;
        }

        //fees
        txtTotalSalesType.Value = Convert.ToDouble(TinfoStoreFrontSwipedPercent.Value) + Convert.ToDouble(TinfoInterntPercent.Value) + Convert.ToDouble(TinfoMailOrderPercent.Value) + Convert.ToDouble(TinfoTelephoneOrderPercent.Value);
        txtTotalTransCompleted.Value = Convert.ToDouble(TinfoElectronicDataCaptureSwipedPercent.Value) + Convert.ToDouble(TinfoManualEntryWithImprintPercent.Value) + Convert.ToDouble(TinfoManualEntryNoCardNoImprintPercent.Value) + Convert.ToDouble(TinfoVoiceAuthPercent.Value);

        //owners
        if (agreement.Owners.Count > 0)
            FormBinding.BindObjectToControls(agreement.Owners[0], wucOwner0);

        if (agreement.Owners.Count > 1)
            FormBinding.BindObjectToControls(agreement.Owners[1], wucOwner1);

        if (agreement.Owners.Count > 2)
            FormBinding.BindObjectToControls(agreement.Owners[2], wucOwner2);

        if (agreement.Owners.Count > 3)
            FormBinding.BindObjectToControls(agreement.Owners[3], wucOwner3);

        //trade references
        if (agreement.TradeReferences.Count > 0)
            FormBinding.BindObjectToControls(agreement.TradeReferences[0], WucTradeReference0);

        if (agreement.TradeReferences.Count > 1)
            FormBinding.BindObjectToControls(agreement.TradeReferences[1], WucTradeReference1);

        if (agreement.TradeReferences.Count > 2)
            FormBinding.BindObjectToControls(agreement.TradeReferences[2], WucTradeReference2);

        LoadEquipments(ID);
        lblError.Text = string.Empty;
    }

    public MerchantApp FormSave()
    {
        MerchantApp agreement = null;

        try
        {
            if (!FormDataCheck())
                return null;


            MerchantApp clone = null;
            if (this.Adding)
            {
                agreement = new MerchantApp();
            }
            else
            {
                agreement = (MerchantApp)UserSessions.CurrentMerchantApp;
            }

            clone = (MerchantApp)agreement.Clone();

            string OrigAgentUID = agreement.AgentUID.ToUpper();

            FormBinding.BindControlsToObject(agreement, pnlDetail);

            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            Lead lead;
            DataLead datalead = DataAccess.DataLeadDao;

            User user = UserSessions.CurrentUser;
            agreement.UserUpdated = user.UserName;

            agreement.LeadsUID = UserSessions.CurrentLead.LeadUID;
            agreement.LeadsID = UserSessions.CurrentLead.LeadID.ToString();

            if (!this.Adding)
            {
                agreement.UserUpdated = user.UserName;

                if (OrigAgentUID != agreement.AgentUID.ToUpper())
                    data.DeleteMerchantAgentContract(agreement.MerchantAppUID);

                if (!this.EditMode)
                {
                    agreement.StatusUID = "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409";
                    agreement.StatusName = "SS - Received";//RM Queue is now changed to SS
                }

                MerchantFacade facade = new MerchantFacade();
                facade.UpdateMerchantApp(agreement);
                if (agreement.Brand == MerchantBrand.Optimal)
                {
                    data.UpdateFMAMerchant(agreement, user.UserName);
                }

                
                agreement = facade.GetMerchantAppZeus(agreement.MerchantAppUID);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, agreement.MerchantAppUID, Convert.ToInt32(agreement.ID), clone, agreement);
            }
            else
            {
                agreement.UserCreated = user.UserName;

                if (!this.EditMode)
                {
                    agreement.StatusUID = "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409";
                    agreement.StatusName = "SS - Received";// RM Queue is now changed to SS
                }

                agreement.DateCreated = DateTime.Now;
                data.InsertMerchantApp(agreement);

                if (agreement.MerchantAppUID != "-1")
                {
                    long fmaId = agreement.FMAID;

                    MerchantFacade facade = new MerchantFacade();
                    agreement = facade.GetMerchantAppZeus(agreement.MerchantAppUID);
                    agreement.FMAID = fmaId;
                    if (agreement.Brand == MerchantBrand.Optimal)
                    {
                        data.UpdateFMAMerchant(agreement, user.UserName);
                    }
                }
            }

            this.UID = agreement.MerchantAppUID;

            lead = datalead.GetLead(UserSessions.CurrentLead.LeadUID);

            if (agreement.StatusUID.ToUpper() == "D96EC87C-CCB0-4C88-B9B8-2B497BA6E409")
            {
                lead.StatusID = "71371612-989D-4E88-B8F8-2EDEB1511CEB";
                lead.Status = Constants.LEADSTATUS_FILEINREVIEW;
                //lead.MerchantAppUID = agreement.MerchantAppUID;
                datalead.UpdateLead(lead);
            }
            else
            {
                if (lead.Status != "Application Received" || lead.StatusID.ToUpper() != "359A038C-B86F-47C2-8990-EC5DFC800211")
                {
                    lead.StatusID = Constants.LEADSTATUS_APPLICATIONSENT;
                    lead.Status = "Application Sent";
                    datalead.UpdateLead(lead);
                }
            }


            this.SaveOwners(agreement);
            this.SaveTradeReferences();
            this.SaveEquipment(agreement);
        }
        catch (Exception exc)
        {
            throw exc;
        }

        return agreement;
    }

    private void SaveEquipment(MerchantApp ma)
    {
        bool perform = false;
        Equipment equip = new Equipment();
        EquipmentFacade facade = new EquipmentFacade();
        if (ma != null)
        {
            if (Type.Text != string.Empty && Maker.Text != string.Empty && Model.Text != string.Empty)
            {
                FormBinding.BindControlsToObject(equip, pnlEquip);
                equip.MerchantAppsUID = ma.MerchantAppUID;
                equip.UserUpdated = UserSessions.CurrentUser.UserName;
                equip.UserCreated = UserSessions.CurrentUser.UserName;
                equip.TerminalStatusUID = "7839b1a8-6ec7-46d3-807e-1c192f8ef65a";

                perform = facade.InsertEquipment(equip);
                if (perform)
                    LoadEquipments(equip.MerchantAppsUID);
                Type.Text = Maker.Text = Model.Text = string.Empty;
            }
        }
    }

    public bool FormDataCheck()
    {
        string message = string.Empty;
        //decimal perOwner = 0.0M;

        //perOwner = Convert.ToDecimal(((WebPercentEditor)wucOwner0.FindControl("PercentOwnership")).Text.Replace('%', ' ').Trim());
        //perOwner += Convert.ToDecimal(((WebPercentEditor)wucOwner1.FindControl("PercentOwnership")).Text.Replace('%', ' ').Trim());
        //perOwner += Convert.ToDecimal(((WebPercentEditor)wucOwner2.FindControl("PercentOwnership")).Text.Replace('%', ' ').Trim());
        //perOwner += Convert.ToDecimal(((WebPercentEditor)wucOwner3.FindControl("PercentOwnership")).Text.Replace('%', ' ').Trim());

        //if ((Convert.ToDecimal(InterchangePlus.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(InterchangePerItem.Text.Replace('$', ' ').Trim()) == 0.0M) && (Convert.ToDecimal(VisaDiscountPercent.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(VisaTransFee.Text.Replace('$', ' ').Trim()) == 0.0M || Convert.ToDecimal(MidQualifyingRatePercent.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(MCDiscountPercent.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(MCTransFee.Text.Replace('$', ' ').Trim()) == 0.0M || Convert.ToDecimal(NonQualifyingRatePercent.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(DiscoverDiscountRate.Text.Replace('%', ' ').Trim()) == 0.0M || Convert.ToDecimal(DiscoverTransFee.Text.Replace('$', ' ').Trim()) == 0.0M))
        //    message = "Please enter Discount rates or Interchange pass through";

        //else if (Convert.ToDecimal(TinfoInterntPercent.Text.Replace('%', ' ').Trim()) > 0.0M && BusinessWebsite.Text.Equals(string.Empty))
        //    message = "Website is required";

        //else if (Convert.ToDecimal(txtTotalTransCompleted.Text.Replace('%', ' ').Trim()) != 100.0M)
        //    message = "Transaction completed % must be 100.";

        //else if (Convert.ToDecimal(txtTotalSalesType.Text.Replace('%', ' ').Trim()) != 100.0M)
        //    message = "Transaction type % must be 100.";

        //else if ((Convert.ToDecimal(TinfoTelephoneOrderPercent.Text.Replace('%', ' ').Trim()) + Convert.ToDecimal(TinfoMailOrderPercent.Text.Replace('%', ' ').Trim()) + Convert.ToDecimal(TinfoInterntPercent.Text.Replace('%', ' ').Trim())) >= 50.0M && Descriptor.Text.Equals(string.Empty))
        //    message = "Descriptor is required.";

        //else if (((TextBox)wucOwner0.FindControl("LastName")).Text.Equals(string.Empty) || ((TextBox)wucOwner0.FindControl("FirstName")).Text.Equals(string.Empty) || ((WebDateChooser)wucOwner0.FindControl("DOB")).Text.Equals(string.Empty)
        //     || ((TextBox)wucOwner0.FindControl("Address1")).Text.Equals(string.Empty) || ((TextBox)wucOwner0.FindControl("City")).Text.Equals(string.Empty) || ((TextBox)wucOwner0.FindControl("Zip")).Text.Equals(string.Empty) ||
        //     ((WebMaskEditor)wucOwner0.FindControl("HomePhone")).Text.Equals(string.Empty) || ((DropDownList)wucOwner0.FindControl("State")).SelectedValue.Equals("-1"))
        //    message = "Please enter atleast one owner information";

        //else if (perOwner < 52.0M)
        //    message = "A total of 52% or more of the Ownership is required";

        //else if (wucOwner0.ValidAdd || wucOwner1.ValidAdd || wucOwner2.ValidAdd || wucOwner3.ValidAdd)
        //    message = "No P.O box numbers for address";

        //else if (grdEquipments.Rows.Count == 0 && (Type.Text.Equals(string.Empty) && Maker.Text.Equals(string.Empty) && Model.Text.Equals(string.Empty)))
        //    message = "Please select atleast one equipment";

        if (message == string.Empty)
            return true;
        else
        {
            lblError.Text = message;
            return false;
        }
    }

    public void FormCancel()
    {
        if (Request["PostBackURL"] != null)
            Response.Redirect(Request["PostBackURL"]);
        else
        {
            FormHandler.ClearAllControls(pnlDetail);
            Session.Clear();
        }
    }

    public void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public void FormClear()
    {
        FormHandler.ClearAllControls(pnlDetail);
    }

    protected void btnPDF_Click(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        viewPdf();
    }

    public void viewPdf()
    {
        MerchantApp app = UserSessions.CurrentMerchantApp;
        if (string.IsNullOrEmpty(this.BusinessDBAName.Text.Trim()))
            return;
        IDictionary<string, string> custom = new Dictionary<string, string>();

        string billingAddress = app.BusinessMailingAddress;
        custom.Add("BillingAddress", billingAddress);

        string state = string.Empty;
        if (app.BusinessState != "-1" && app.BusinessState != "--")
            state = app.BusinessState;
        string citystatezip = app.BusinessCity + "  " + state + "  " + app.BusinessZip;
        custom.Add("LocationCityStateZip", citystatezip);

        string Mstate = string.Empty;
        if (app.BusinessMailingState != "-1")
            Mstate = app.BusinessMailingState;

        string billingcitystatezip = app.BusinessMailingCity + "  " + Mstate + "  " + app.BusinessMailingZip;
        custom.Add("BillingCityStateZip", billingcitystatezip);

        string SwipedPercent = app.TinfoElectronicDataCaptureSwipedPercent.ToString();
        custom.Add("Electronic Data Capture Swiped", SwipedPercent);

        string ImprintPercent = app.TinfoManualEntryWithImprintPercent.ToString();
        custom.Add("Manual Entry with Imprint 1", ImprintPercent);

        string NoImprintPercent = app.TinfoManualEntryNoCardNoImprintPercent.ToString();
        custom.Add("Manual entry, no card present", NoImprintPercent);

        string AuthPercent = app.TinfoVoiceAuthPercent.ToString();
        custom.Add("Manual Entry with Imprint 2", AuthPercent);

        string WirelessServiceFee = app.WirelessServiceFee.ToString();
        custom.Add("WirelessDataMonthlyFee", WirelessServiceFee);

        string vdp = "0";
        if (!(app.InterchangePlus.ToString().Equals("0.00")) && !(app.InterchangePlus.ToString().Equals(string.Empty)))
        {
            vdp = app.InterchangePlus.ToString();
            int ind = vdp.IndexOf(".");
            if (ind != -1)
            {
                vdp = vdp.Remove(ind, 1);
                vdp = Convert.ToInt32(vdp).ToString();
            }
            if (Convert.ToInt32(vdp) > 100)
                vdp = Convert.ToString(Convert.ToInt32(vdp) / 100);
            custom.Add("NonQualifyingRatePercent", " ");
            custom.Add("MidQualifyingRatePercent", " ");
        }

        string VisaDiscountPercent = "IC + " + vdp + " BP";
        if (vdp == "0")
            VisaDiscountPercent = app.VisaDiscountPercent.ToString();
        custom.Add("VisaQualifiedRate", VisaDiscountPercent);

        string MCDiscountPercent = "IC + " + vdp + " BP";
        if (vdp == "0")
            MCDiscountPercent = app.MCDiscountPercent.ToString();
        custom.Add("MCQualifiedRate", MCDiscountPercent);

        string DiscoverDiscountRate = "IC + " + vdp + " BP";
        if (vdp == "0")
            DiscoverDiscountRate = app.DiscoverDiscountRate.ToString();
        custom.Add("DiscoverQualifiedRate", DiscoverDiscountRate);


        string VisaTransFee = app.VisaTransFee.ToString();
        if (VisaTransFee.Equals(string.Empty) || VisaTransFee.Equals("0.00"))
            VisaTransFee = app.InterchangePerItem.ToString();
        custom.Add("VisaQualifiedFee", VisaTransFee);

        string MCTransFee = app.MCTransFee.ToString();
        if (MCTransFee.Equals(string.Empty) || MCTransFee.Equals("0.00"))
            MCTransFee = app.InterchangePerItem.ToString();
        custom.Add("MCQualifiedFee", MCTransFee);

        string DiscoverTransFee = app.DiscoverTransFee.ToString();
        if (DiscoverTransFee.Equals(string.Empty) || DiscoverTransFee.Equals("0.00"))
            DiscoverTransFee = app.InterchangePerItem.ToString();
        custom.Add("DiscoverQualifiedFee", DiscoverTransFee);

        string AMEXTransFee = app.AMEXTransFee.ToString();
        custom.Add("undefined_11", AMEXTransFee);

        string JCBTransFee = app.JCBTransFee.ToString();
        custom.Add("undefined_13", JCBTransFee);

        string EBTTransFee = app.EBTTransFee.ToString();
        custom.Add("Trans Fee", EBTTransFee);

        custom.Add(app.BusinessStructureUID, "Yes");

        string BusinessStartDate = (app.BusinessStartDate.Equals(DateTime.MinValue) ? " " : app.BusinessStartDate.ToShortDateString());
        custom.Add("BusinessStartDate", BusinessStartDate);

        custom.Add("SPECIAL REQUESTS", Notes.Text);

        int Qty = 0, Qty_2 = 0, Qty_3 = 0, Qty_4 = 0, Qty_5 = 0, Qty_6 = 0;
        string str1 = string.Empty, str2 = string.Empty, str3 = string.Empty, str4 = string.Empty, str5 = string.Empty, str6 = string.Empty;
        //Equipment
        foreach (GridViewRow grdRow in grdEquipments.Rows)
        {
            string maker = grdRow.Cells[2].Text;
            if (grdRow.Cells[1].Text.Equals("Check Reader"))
            {
                Qty_4 += 1;
                str1 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str1.Contains(maker)) ? string.Empty : (maker) + ",");
            }
            else if (grdRow.Cells[1].Text.Equals("PC Software"))
            {
                Qty_5 += 1;
                str2 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str2.Contains(maker)) ? string.Empty : (maker) + ",");
            }
            else if (grdRow.Cells[1].Text.Equals("Printer"))
            {
                Qty_3 += 1;
                str3 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str3.Contains(maker)) ? string.Empty : (maker) + ",");
            }
            else if (grdRow.Cells[1].Text.Equals("Terminal"))
            {
                Qty += 1;
                str4 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str4.Contains(maker)) ? string.Empty : (maker) + ",");
            }
            else if (grdRow.Cells[1].Text.Equals("Pin-Pad"))
            {
                Qty_2 += 1;
                str5 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str5.Contains(maker)) ? string.Empty : (maker) + ",");
            }
            else
            {
                Qty_6 += 1;
                str6 += ((maker.Equals("&nbsp;") || maker.Equals(string.Empty) || str6.Contains(maker)) ? string.Empty : (maker) + ",");
            }
        }
        custom.Add("Terminal Model", (str4.Length > 0) ? str4.Substring(0, str4.Length - 1) : string.Empty);
        custom.Add("Printer Model", (str3.Length > 0) ? str3.Substring(0, str3.Length - 1) : string.Empty);
        custom.Add("Software Model", (str2.Length > 0) ? str2.Substring(0, str2.Length - 1) : string.Empty);
        custom.Add("PIN Pad", (str5.Length > 0) ? str5.Substring(0, str5.Length - 1) : string.Empty);
        custom.Add("Check Reader", (str1.Length > 0) ? str1.Substring(0, str1.Length - 1) : string.Empty);
        custom.Add("Misc", (str6.Length > 0) ? str6.Substring(0, str6.Length - 1) : string.Empty);

        custom.Add("Qty", Qty.ToString());
        custom.Add("Qty_2", Qty_2.ToString());
        custom.Add("Qty_3", Qty_3.ToString());
        custom.Add("Qty_4", Qty_4.ToString());
        custom.Add("Qty_5", Qty_5.ToString());
        custom.Add("Qty_6", Qty_6.ToString());

        if (AllFields || app.StatusUID.ToUpper().Equals(Constants.QUEUESTATUS_SS_DRAFT))//"AW - Pending"))
        {
            //owners
            for (int i = 1, j = 0; i < 3; i++)
            {
                j = i - 1;
                if (app.Owners.Count > j)
                {
                    custom.Add("Owner" + i.ToString() + "FullName", app.Owners[j].FirstName + " " + app.Owners[j].MiddleName + " " + app.Owners[j].LastName);
                    custom.Add("Owner" + i.ToString() + "Title", app.Owners[j].Title);
                    custom.Add("Owner" + i.ToString() + "Percent", app.Owners[j].PercentOwnership.ToString() + "%");
                    custom.Add("Owner" + i.ToString() + "DOB", app.Owners[j].DOB.Equals(DateTime.MinValue) ? " " : app.Owners[j].DOB.ToShortDateString());
                    custom.Add("Owner" + i.ToString() + "SSN", app.Owners[j].SSN);
                    custom.Add("Owner" + i.ToString() + "Phone", app.Owners[j].HomePhone);
                    custom.Add("Owner" + i.ToString() + "Address", app.Owners[j].Address1);
                    custom.Add("Owner" + i.ToString() + "CityStateZip", app.Owners[j].City + " " + app.Owners[j].State + "  " + app.Owners[j].Zip);
                    custom.Add("Owner" + i.ToString() + "City", app.Owners[j].City);
                    custom.Add("Owner" + i.ToString() + "State", app.Owners[j].State);
                    custom.Add("Owner" + i.ToString() + "Zip", app.Owners[j].Zip);
                    custom.Add("Owner" + i.ToString() + "DLNumber", app.Owners[j].DriversLicense);
                    custom.Add("Owner" + i.ToString() + "DLState", app.Owners[j].DriversLicenseState);
                    custom.Add("Owner" + i.ToString() + "DLNumberState", app.Owners[j].DriversLicense + "  " + app.Owners[j].DriversLicenseState);
                }
                else
                {
                    custom.Add("Owner" + i.ToString() + "FullName", string.Empty);
                    custom.Add("Owner" + i.ToString() + "Title", string.Empty);
                    custom.Add("Owner" + i.ToString() + "Percent", "0");
                    custom.Add("Owner" + i.ToString() + "DOB", string.Empty);
                    custom.Add("Owner" + i.ToString() + "SSN", string.Empty);
                    custom.Add("Owner" + i.ToString() + "Phone", string.Empty);
                    custom.Add("Owner" + i.ToString() + "Address", string.Empty);
                    custom.Add("Owner" + i.ToString() + "CityStateZip", string.Empty);
                    custom.Add("Owner" + i.ToString() + "City", string.Empty);
                    custom.Add("Owner" + i.ToString() + "State", string.Empty);
                    custom.Add("Owner" + i.ToString() + "Zip", string.Empty);
                    custom.Add("Owner" + i.ToString() + "DLNumber", string.Empty);
                    custom.Add("Owner" + i.ToString() + "DLState", string.Empty);
                    custom.Add("Owner" + i.ToString() + "DLNumberState", string.Empty);
                }

            }

            //trade references
            if (app.TradeReferences.Count > 0)
            {
                custom.Add("Trade Name", app.TradeReferences[0].VendorName);
                custom.Add("Contact Name_2", app.TradeReferences[0].ContactName);
                custom.Add("Phone", app.TradeReferences[0].PhoneNumber);
            }
            if (app.TradeReferences.Count > 1)
            {
                custom.Add("Trade Name2", app.TradeReferences[1].VendorName);
                custom.Add("Contact Name_3", app.TradeReferences[1].ContactName);
                custom.Add("Phone_2", app.TradeReferences[1].PhoneNumber);
            }
        }
        else
        {
            custom.Add("BankName", string.Empty);
            custom.Add("AccountNumber", string.Empty);
            custom.Add("RoutingNumber", string.Empty);
        }

        FormBinding.BindObjectToPDF(app, "~/forms/Wells_Fargo_Meritus_Application_1212.pdf", "Wells Fargo Paysafe Application 1212" + app.ID.ToString(), custom);
    }

    public bool SaveTradeReferences()
    {
        try
        {
            TradeReference trade = null;
            DataMerchantApp data = DataAccess.DataMerchantAppDao;
            int i = 0;

            //Save reference 1
            trade = new TradeReference();
            FormBinding.BindControlsToObject(trade, WucTradeReference0);

            trade.Position = i;
            trade.UserUpdated = UserSessions.CurrentUser.UserName;
            trade.UserCreated = UserSessions.CurrentUser.UserName;
            trade.MerchantAppsUID = this.UID;

            if (trade.TradeReferenceID == null || trade.TradeReferenceID == string.Empty)
                data.InsertTradeReference(trade);
            else
                data.UpdateTradeReference(trade);

            //Save reference 2
            trade = new TradeReference();
            FormBinding.BindControlsToObject(trade, WucTradeReference1);

            trade.Position = i++;
            trade.UserUpdated = UserSessions.CurrentUser.UserName;
            trade.UserCreated = UserSessions.CurrentUser.UserName;
            trade.MerchantAppsUID = this.UID;

            if (trade.TradeReferenceID == null || trade.TradeReferenceID == string.Empty)
                data.InsertTradeReference(trade);
            else
                data.UpdateTradeReference(trade);

            //Save reference 3
            trade = new TradeReference();
            FormBinding.BindControlsToObject(trade, WucTradeReference2);

            trade.Position = i++;
            trade.UserUpdated = UserSessions.CurrentUser.UserName;
            trade.UserCreated = UserSessions.CurrentUser.UserName;
            trade.MerchantAppsUID = this.UID;

            if (trade.TradeReferenceID == null || trade.TradeReferenceID == string.Empty)
                data.InsertTradeReference(trade);
            else
                data.UpdateTradeReference(trade);

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    public bool SaveOwners(MerchantApp ma)
    {
        try
        {
            MerchantApp agreement = ma;
            DataMerchantApp data = DataAccess.DataMerchantAppDao;

            //Save owner 1
            Owner owner = null;
            Owner clone = null;
            if (agreement.Owners.Count > 0)
            {
                owner = (Owner)agreement.Owners[0];
                clone = (Owner)owner.Clone();
            }
            else
                owner = new Owner();

            FormBinding.BindControlsToObject(owner, wucOwner0);

            owner.Position = 1;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.UserCreated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;

            if (owner.OwnerID == null || owner.OwnerID == string.Empty)
                data.InsertOwner(owner);
            else
            {
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }
            //Save owner 2
            if (agreement.Owners.Count > 1)
            {
                owner = (Owner)agreement.Owners[1];
                clone = (Owner)owner.Clone();
            }
            else
                owner = new Owner();


            FormBinding.BindControlsToObject(owner, wucOwner1);

            owner.Position = 2;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.UserCreated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;

            if (owner.OwnerID == null || owner.OwnerID == string.Empty)
                data.InsertOwner(owner);
            else
            {
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            //Save owner 3
            if (agreement.Owners.Count > 2)
            {
                owner = (Owner)agreement.Owners[2];
                clone = (Owner)owner.Clone();
            }
            else
                owner = new Owner();

            FormBinding.BindControlsToObject(owner, wucOwner2);

            owner.Position = 3;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.UserCreated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;

            if (owner.OwnerID == null || owner.OwnerID == string.Empty)
                data.InsertOwner(owner);
            else
            {
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            //Save owner 4
            if (agreement.Owners.Count > 3)
            {
                owner = (Owner)agreement.Owners[3];
                clone = (Owner)owner.Clone();
            }
            else
                owner = new Owner();

            FormBinding.BindControlsToObject(owner, wucOwner3);

            owner.Position = 4;
            owner.UserUpdated = UserSessions.CurrentUser.UserName;
            owner.UserCreated = UserSessions.CurrentUser.UserName;
            owner.MerchantAppsUID = this.UID;

            if (owner.OwnerID == null || owner.OwnerID == string.Empty)
                data.InsertOwner(owner);
            else
            {
                data.UpdateOwner(owner);
                FormHandler.LogFormChanges(agreement.BusinessDBAName, owner.OwnerID, 0, clone, owner);
            }

            return true;
        }
        catch (Exception exc)
        {
            throw exc;
        }
    }

    private void LoadEquipments(string MerchantAppUID)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", MerchantAppUID);
        DataEquipment data = DataAccess.DataEquipmentDao;
        DataSet ds = data.GetMerchantAppItem(prms);

        this.grdEquipments.DataSource = ds;
        this.grdEquipments.DataBind();

        pnlEquipment.Visible = (grdEquipments.Rows.Count > 0);
        pnlNoEquipment.Visible = !(grdEquipments.Rows.Count > 0);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.SearchEquipments();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        cboMaker.SelectedIndex = 0;
        cboType.SelectedIndex = 0;
        txtModel.Text = string.Empty;
        SearchEquipments();
    }

    private void SearchEquipments()
    {
        EquipmentFacade facade = new EquipmentFacade();
        Hashtable prms = new Hashtable();

        if (cboType.SelectedIndex != 0)
            prms.Add("@EquipmentTypeUID", cboType.SelectedItem.Value);

        if (cboMaker.SelectedIndex != 0)
            prms.Add("@EquipmentMakerUID", cboMaker.SelectedItem.Value);

        if (txtModel.Text.Trim() != string.Empty)
            prms.Add("@Model", txtModel.Text);

        if (prms.Count > 0)
        {
            DataSet ds = facade.GetEquipmentItems(prms);
            DataView dv = ds.Tables[0].DefaultView;
            grdSearch.DataSource = dv;
        }
        else
            grdSearch.DataSource = null;
        grdSearch.DataBind();
        pnlNoRecords.Visible = !(grdSearch.Rows.Count > 0);
        pnlgrdSearch.Visible = (grdSearch.Rows.Count > 0);
    }

    protected void grdSearch_Rowcommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is LinkButton)
        {
            switch (e.CommandName)
            {
                case "Select":
                    GridViewRow grdRow = (GridViewRow)((LinkButton)e.CommandSource).NamingContainer;
                    Type.Text = grdRow.Cells[2].Text;
                    Maker.Text = grdRow.Cells[3].Text;
                    Model.Text = grdRow.Cells[4].Text;
                    ItemUID.Value = grdSearch.DataKeys[grdRow.RowIndex].Values["UID"].ToString();
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
                    break;
            }
        }
        else
            return;
    }

    //protected void btnAdd_Click(object sender, EventArgs e)
    //{
    //    SaveEquipment();
    //}

    private string FormatPhoneNumber(string value)
    {
        return value.Replace("(", "").Replace(")", "").Replace("-", "");
    }
}
