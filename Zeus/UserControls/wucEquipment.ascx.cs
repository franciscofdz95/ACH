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
using System.Collections.Generic;

public partial class wucEquipment : System.Web.UI.UserControl
{

    public bool EquipmentDocs
    {
        get { if (ViewState["EquipmentDocs"] != null) return (bool)ViewState["EquipmentDocs"]; else return false; }
        set { ViewState["EquipmentDocs"] = value; }
    }

    public Panel EquipmentDoc
    {
        get { return pnlEquipmentDocs; }
    }

    //PXP-3427
    public bool IsEMVMerchantValueChanged
    {
        get { if (ViewState["IsEMVMerchantValueChanged"] != null) return (bool)ViewState["IsEMVMerchantValueChanged"]; else return false; }
        set { ViewState["IsEMVMerchantValueChanged"] = value; }
    }
    public string EMVMerchantSelectedValue
    {
        get { if (ViewState["EMVMerchantDDLSelectedValue"] != null) return (string)ViewState["EMVMerchantDDLSelectedValue"]; else return string.Empty; }
        set { ViewState["EMVMerchantDDLSelectedValue"] = value; }
    }



    protected string SortOrder1
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"] == null)
                return string.Empty;
            else
                return HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"].ToString();
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_SortOrder"] = value; }
    }

    protected SortDirection SortDirectionSearch1
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"] == null)
                return SortDirection.Descending;
            else
                return (SortDirection)HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"];
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_SortDirectionSearch"] = value; }
    }

    protected int CurrentPage1
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"] == null)
                return 1;
            else
                return Convert.ToInt32(HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"]);
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_CurrentPage"] = value; }
    }

    protected int PageSize1
    {
        get
        {
            if (HttpContext.Current.Session[this.Page.ToString() + "_PageSize"] == null)
                return 5;
            else
                return Convert.ToInt32(HttpContext.Current.Session[this.Page.ToString() + "_PageSize"]);
        }
        set { HttpContext.Current.Session[this.Page.ToString() + "_PageSize"] = value; }
    }


    // the selected equipment from the gridview
    public string SelectedUID
    {
        get { return (string)(ViewState["SelectedUID"] ?? string.Empty); }
        set { ViewState["SelectedUID"] = value; }
    }

    public int grdCount
    {
        get { return grd.Rows.Count; }
    }

    override protected void OnInit(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LookupTableHandler.MerchantAppStatus(TerminalStatusUID, false, "Deployment");
            LookupTableHandler.LoadEquipmentTypes(cboType, false);
            LookupTableHandler.LoadEquipmentMaker(cboMaker, false);
            LookupTableHandler.LoadTerminalShippings(ShippingUID, false);
            //Niranjan PXP-8045
            LookupTableHandler.LoadDeployType(DeployTypeID, false);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (EquipmentDocs)
            {
                SetDataSource();
            }
            //PXP-3014
            if (UserSessions.CurrentMerchantApp != null)
            {
                EMVComplianceMerchant.SelectedValue = UserSessions.CurrentMerchantApp.EMVComplianceMerchant;
            }
        }
    }

    public void SetDataSource()
    {

        grdDocuments.DataSourceID = "ods";

        this.CurrentPage1 = 1;
        this.PageSize1 = 5;

        grdDocuments.PageIndex = CurrentPage1 - 1;
        grdDocuments.PageSize = PageSize1;

        LoadDocuments();

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
            grdSearch.DataBind();
            grdSearch.Columns[5].Visible = false;
        }
        else
        {
            grdSearch.DataSource = null;
            grdSearch.DataBind();
            grdSearch.Columns[5].Visible= false;
        }
        pnl1.Visible = (grdSearch.Rows.Count > 0);
        nosearchrecords.Visible = !(grdSearch.Rows.Count > 0);
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
        pnl1.Visible = false;
        nosearchrecords.Visible = true;
        lblError.Text = "";
        this.IsNewItem.Value = "0";
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grd.PageIndex = e.NewPageIndex;
        LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandSource is LinkButton)
        {

            EquipmentFacade facade = new EquipmentFacade();
            Equipment equip = new Equipment();

            equip = facade.GetMerchantAppItem(e.CommandArgument.ToString(), UserSessions.CurrentUser.TimeZone);
            TextBox txtQIRID = (TextBox)QIUlookup.FindControl("txtQIRCompany");
            Button btnQIRLookUp = (Button)QIUlookup.FindControl("btnLookup");
            GridViewRow grdRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
            switch (e.CommandName)
            {
                case "select":

                    this.SelectedUID = equip.UID;
                    FormBinding.BindObjectToControls(equip, pnlEquipment);
                    txtQIRID.Text = equip.QIRCompany;
                    this.IsNewItem.Value = "0";
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
                    btnQIRLookUp.Enabled = true;
                    //Niranjan PXP-8045
                    DeployTypeID.SelectedValue = Convert.ToString(equip.DeployTypeID);


                    break;

                case "erase":

                    equip.IsEnabled = false;
                    equip.RecordID = Convert.ToInt32( grd.DataKeys[grdRow.RowIndex].Values["EquipmentRecordID"]);
                    equip.UserUpdated = UserSessions.CurrentUser.UserName;//Code changes for PXP-8191 by koshlendra
                    facade.UpdateEquipment(equip);
                    LoadEquipments(UserSessions.CurrentMerchantApp.MerchantAppUID);

                    MerchantFacade facade2 = new MerchantFacade();

                    //PXP-3433
                    MerchantApp app = (UserSessions.CurrentMerchantApp != null) ? UserSessions.CurrentMerchantApp : new MerchantApp();
                    CheckMerchantEMVbyEquipments(ref app, facade, app.MerchantAppUID);

                    if (grd.Rows.Count == 0)
                    {
                        app.NoEquipment = true;
                    }
                    facade2.UpdateMerchantApp(app);

                    Maker.Text = "";
                    Model.Text = "";
                    Type.Text = "";
                    txtQIRID.Text = "";
                    EMVCompliance.SelectedIndex = 0;
                    btnQIRLookUp.Enabled = false;
                    //Niranjan PXP-8045
                    DeployTypeID.SelectedValue = "0";
                    FormHandler.DisplayMessage(Page.ClientScript, "Item deleted successfully.");
                    break;

                //Code added by amit for PXP-7621
                case "ViewVarSheet":

                    IDictionary<string, string> custom = new Dictionary<string, string>();
                    DataEquipment data = DataAccess.DataEquipmentDao;
                    MerchantApp mapp = (UserSessions.CurrentMerchantApp != null) ? UserSessions.CurrentMerchantApp : new MerchantApp();
                   
                    string citystatezip = mapp.BusinessCity + "," + mapp.BusinessState + " " + mapp.BusinessZip;
                    custom.Add("CityStateZip", citystatezip);

                    string maker = string.Empty, model = string.Empty, appID = string.Empty, DownloadNo = string.Empty, TerminalNo = string.Empty, serialNumber = string.Empty, deployType = string.Empty;

                    string tId = equip.TerminalNumber;
                    string terminalNumberVAR = equip.TerminalNumberVAR;
                    string storeNumberVAR = equip.StoreNumberVAR;

                    //string rowIndex = (sender as LinkButton).CommandArgument;
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    int index = row.RowIndex;
                    if (grd.Rows.Count > 0)
                    {
                        if (index > -1) //Code modified for PXP-7797
                        {
                            // NOTE!!! these indices are directly tied to the order of the grid in wucEquipment. please keep them syncronized!!
                            maker = grd.Rows[index].Cells[4].Text;
                            model = grd.Rows[index].Cells[5].Text;
                            //PXP-2319
                            serialNumber = grd.Rows[index].Cells[7].Text;
                            TerminalNo = grd.Rows[index].Cells[8].Text;
                            appID = grd.Rows[index].Cells[9].Text;
                            deployType = grd.Rows[index].Cells[10].Text;
                            //Niranjan :- PXP-8285
                            DownloadNo = grd.Rows[index].Cells[11].Text;
                        }
                    }
                    if (maker != "&nbsp;")
                        custom.Add("Maker", maker);

                    if (model != "&nbsp;")
                        custom.Add("Model", model);

                    if (appID != "&nbsp;")
                        custom.Add("ApplicationID", appID);
                    else
                        appID = string.Empty;

                    if (DownloadNo != "&nbsp;")
                        custom.Add("DownloadNumber", DownloadNo);

                    if (TerminalNo != "&nbsp;")
                        custom.Add("TerminalNumber", TerminalNo);
                    else
                        TerminalNo = string.Empty;

                    if (serialNumber != "&nbsp;")
                        custom.Add("SerialNumber", serialNumber);
                    else
                        serialNumber = string.Empty;

                    if (deployType != "&nbsp;")
                        custom.Add("DeployType", deployType);
                    else
                        deployType = string.Empty;

                    if (mapp.SettlePlatformUID.ToUpper().Equals("2980F0E1-A1BE-4169-AF75-43CA0C41B450")) // Global Payments East platform
                        custom.Add("BIN", "033500");

                    if (!string.IsNullOrWhiteSpace(mapp.AuthPlatformUID))
                        custom.Add(mapp.AuthPlatformUID, "Yes");

                    //if (app.AuthPlatformUID.ToLower() == "04d4c5de-fa4d-4fab-b744-d2569234f104")
                    if (!string.IsNullOrWhiteSpace(mapp.AuthPlatformUID))
                        custom.Add("FrontMID", mapp.AuthPlatformMid);

                    if (!string.IsNullOrWhiteSpace(mapp.AuthPlatformUID))
                        custom.Add("FrontEnd", LookupTableHandler.m_FrontEnds.Find(item => item.ItemValue.ToUpper() == mapp.AuthPlatformUID.ToUpper()).ItemText);

                    if (!string.IsNullOrWhiteSpace(mapp.SettlePlatformUID))
                        custom.Add("BackEnd", LookupTableHandler.m_BackEnds.Find(item => item.ItemValue.ToUpper() == mapp.SettlePlatformUID.ToUpper()).ItemText);

                    string txtAppType = getAppType(mapp.ApplicationTypeUID);
                    custom.Add("ApplicationType", txtAppType);

                    string txtRelType = getReleaseType(mapp.ReleaseMethodUID);
                    custom.Add("ReleaseMethod", txtRelType);

                    string etc = getETCType(mapp.ETCTypeUID);
                    custom.Add("ETC", etc);

                    if (mapp.MonthendApproved)
                        custom.Add("DiscountMethod", "Monthly");
                    else
                        custom.Add("DiscountMethod", "Daily");

                    string timeZone = getTimeZone(mapp.BusinessState);
                    custom.Add("Timezone", timeZone);

                    if (mapp.MerchantAppTypeUID.ToUpper() == "0AF5CDBA-D720-4040-80D8-F1C68CC236D8") //WellsFargo bank
                        custom.Add("Chargeback#", ConfigurationManager.AppSettings["CBMSFaxOmahaWells"].ToString());

                    else if (mapp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST) //Woodforest bank
                        custom.Add("Chargeback#", ConfigurationManager.AppSettings["CBMSFaxWoodforest"].ToString());
                    else if (mapp.MerchantAppTypeUID.ToUpper() == Constants.BANK_WOODFOREST_SB) //Woodforest bank SB
                        custom.Add("Chargeback#", ConfigurationManager.AppSettings["CBMSFaxWoodforestSB"].ToString());

                    else if (mapp.MerchantAppTypeUID.ToUpper() == "5176D910-3317-4528-8C43-626228F6CBB4") //Ncal bank
                        custom.Add("Chargeback#", ConfigurationManager.AppSettings["CBMSFaxOmahaNcal"].ToString());

                    if (!string.IsNullOrEmpty(mapp.AuthPlatformUID))
                        DataAccess.DataRiskDao.LoadAvailableCurrencies(mapp);
                    System.Text.StringBuilder Currency = new System.Text.StringBuilder();
                    Currency.Append("USD");

                    if (mapp.AvailableCardCurrencies != null && mapp.AvailableCardCurrencies.Count > 0)
                    {
                        Currency.Clear();
                        foreach (MerchantCardCurrency curr in mapp.AvailableCardCurrencies)
                        { Currency.Append(curr + ", "); }
                    }

                    custom.Add("Currency", Currency.ToString().Trim().TrimEnd(','));

                    string CSemail = Constants.CLIENT_SERVICE_EMAIL;

                    if (mapp.PrivateLabelUID != null)
                    {
                        PrivateLabel PL = DataAccess.DataMerchantAppDao.GetPrivateLabel(mapp.PrivateLabelUID);
                        CSemail = PL.PLEmail;
                    }
                    custom.Add("ClientServicesEmail", CSemail);
                    



                    custom["Descriptor"] = MerchantFacade.GetPrioritizedMerchantDescriptor(mapp.ID, mapp.Descriptor);

                    string varSheetPrefix = string.Empty;
                    string storeNumber = mapp.Tsys_StoreNumber ?? "NA";
                    string settlementID = mapp.SettlePlatformMid ?? "NA";                    

                    switch (mapp.SettlePlatformUID.ToUpper())
                    {
                        case SettlementPlatforms.Omaha:
                            varSheetPrefix = "FISERV_";
                            custom["VISAMCCCode"] = string.IsNullOrEmpty(mapp.VisaSicCode) ? "NA" : mapp.VisaSicCode;
                            custom["CAID"] = settlementID.Length >= 15 ? settlementID.Substring(0, 15) : "NA";
                            custom["PinDebit"] = mapp.PinDebit ? "Enabled" : "NA";
                            custom["ETC"] = string.IsNullOrEmpty(etc) ? "NA" : etc;
                            custom["AMEXMid"] = string.IsNullOrEmpty(mapp.AMEXMid) ? "NA" : mapp.AMEXMid;
                            custom["DiscoverMid"] = string.IsNullOrEmpty(mapp.DiscoverMid) ? "NA" : mapp.DiscoverMid;
                            custom["GiftCardMID"] = string.IsNullOrEmpty(mapp.GiftCardMID) ? "NA" : mapp.GiftCardMID;
                            custom["EBTFCSMid"] = string.IsNullOrEmpty(mapp.EBTFCSMid) ? "NA" : mapp.EBTFCSMid;
                            custom["ID"] = string.IsNullOrEmpty(mapp.ID) ? "NA" : mapp.ID;
                            custom["MerchantKey"] = string.IsNullOrEmpty(mapp.MerchantKey) ? "NA" : mapp.MerchantKey;

                            break;
                        case SettlementPlatforms.Tsys:
                            varSheetPrefix = "TSYS_";
                            custom["BIN"] = string.IsNullOrEmpty(mapp.Tsys_AcquirerBin) ? "NA" : mapp.Tsys_AcquirerBin;
                            custom["VNumber"] = tId;
                            custom["TerminalNumber"] = string.IsNullOrEmpty(terminalNumberVAR) ? "NA" : terminalNumberVAR;
                            custom["Agent"] = string.IsNullOrEmpty(mapp.Tsys_AgentBank) ? "NA" : mapp.Tsys_AgentBank;
                            custom["Chain"] = string.IsNullOrEmpty(mapp.Tsys_AgentChain) ? "NA" : mapp.Tsys_AgentChain;
                            custom["StoreNumber"] = string.IsNullOrEmpty(storeNumberVAR) ? "NA" : storeNumberVAR;
                            custom["VISAMCCCode"] = string.IsNullOrEmpty(mapp.VisaSicCode) ? "NA" : mapp.VisaSicCode;
                            custom["MerchantLocNumber"] = string.IsNullOrEmpty(mapp.Tsys_LocationNumber) ? "NA" : $"0{mapp.Tsys_LocationNumber}";
                            /// <summary>
                            /// AhmerBashir 16-03-2026
                            /// DM-7315
                            /// </summary>
                            custom["CAID"] =
                               settlementID.Length >= 12 && storeNumberVAR.Length >= 3
                                ? string.Concat(settlementID.Substring(0, 12), storeNumberVAR.Substring(0, 3)) : "NA";
                            custom["ApplicationID"] = string.IsNullOrEmpty(appID) ? "NA" : appID;
                            custom["PinDebit"] = mapp.PinDebit ? "Enabled" : "NA";
                            custom["ETC"] = string.IsNullOrEmpty(etc) ? "NA" : etc;
                            custom["AMEXMid"] = string.IsNullOrEmpty(mapp.AMEXMid) ? "NA" : mapp.AMEXMid;
                            custom["DiscoverMid"] = string.IsNullOrEmpty(mapp.DiscoverMid) ? "NA" : mapp.DiscoverMid;
                            custom["GiftCardMID"] = string.IsNullOrEmpty(mapp.GiftCardMID) ? "NA" : mapp.GiftCardMID;
                            custom["EBTFCSMid"] = string.IsNullOrEmpty(mapp.EBTFCSMid) ? "NA" : mapp.EBTFCSMid;
                            custom["ID"] = string.IsNullOrEmpty(mapp.ID) ? "NA" : mapp.ID;
                            custom["MerchantKey"] = string.IsNullOrEmpty(mapp.MerchantKey) ? "NA" : mapp.MerchantKey;
                            custom["SerialNumber"] = string.IsNullOrEmpty(serialNumber) ? "NA" : serialNumber;
                            custom["DeployType"] = string.IsNullOrEmpty(deployType) ? "NA" : deployType;
                            break;
                        default:                            
                            break;
                    }

                    // if pxp is not enabled, then don't pass in the merchant id/key into the var sheet.
                    if (!IsPXPEnabled())
                    {
                        custom["ID"] = "NA";
                        custom["MerchantKey"] = "NA";
                    }
                    
                    FormBinding.BindObjectToPDF(UserSessions.CurrentMerchantApp, $"~/PDF/{varSheetPrefix}VAR_SHEET.pdf", "Var_Sheet_" + UserSessions.CurrentMerchantApp.ID.ToString(), custom);
                    break;

                    //End Code added by amit for PXP-7621
            }
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //PXP-2319 set EMV value as empty string
            if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EMVCompliance")).Equals("True"))
            {
                //Code changes done to fix PXP-11435[In Equipment grid Model is showing EMv complaince value]
                e.Row.Cells[6].Text = "Yes";
            }
            else if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EMVCompliance")).Equals("False"))
            {
                //Code changes done to fix PXP-11435[In Equipment grid Model is showing EMv complaince value]
                e.Row.Cells[6].Text = "No";
            }
            e.Row.ToolTip = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TerminalStatusName"));


            LinkButton lnk = (LinkButton)e.Row.FindControl("Delete");
            if (lnk != null)
            {
                string role = UserSessions.CurrentUser.DefaultRoleUID.ToUpper();
                lnk.Visible = ((role == Constants.ROLE_IT || role == Constants.ROLE_DEPLOYMENT
                    || role == Constants.ROLE_ADMIN || role == Constants.ROLE_ACCOUNTING)
                    || (UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_ACCOUNTING) ||
                    UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_IT) ||
                    UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_DEPLOYMENT) ||
                    UserSessions.CurrentUser.UserRoles.ContainsKey(Constants.ROLE_ADMIN)));
            }
        }
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
                    Maker.Text = (grdRow.Cells[3].Text.Equals("&nbsp;")) ? string.Empty : grdRow.Cells[3].Text;
                    Model.Text = grdRow.Cells[4].Text;
                    ItemUID.Value = grdSearch.DataKeys[grdRow.RowIndex].Values["UID"].ToString();
                    //PXP-2319
                    if (grdRow.Cells[5].Text.ToLower().Equals("true"))
                    {
                        EMVCompliance.SelectedValue = "1";
                    }
                    else if (grdRow.Cells[5].Text.ToLower().Equals("false"))
                    {
                        EMVCompliance.SelectedValue = "0";
                    }
                    Button btnQIRLookUp = (Button)QIUlookup.FindControl("btnLookup");
                    btnQIRLookUp.Enabled = true;
                    WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
                    break;
            }
        }
        else
            return;
    }

    public void LoadEquipments(string merchantappuid)
    {
        if (CommonUtility.Util.IsValidGuid(merchantappuid))
        {
            EquipmentFacade facade = new EquipmentFacade();
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", merchantappuid);
            prms.Add("@IsEnabled", true);
            DataSet ds = facade.GetMerchantAppItem(prms);

            DataView dv = ds.Tables[0].DefaultView;
            grd.DataSource = dv;
            grd.DataBind();

            if (grd.Rows.Count > 0)
            {
                pnlGrid.Visible = true;
            }
            else
            {
                pnlGrid.Visible = false;
                this.FormShow("");
            }

            FormHandler.SetControlEditMode(pnlEquipmentDocs,true);
        }
    }

    public void FormShow(string ID)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            this.SelectedUID = ID;
            EquipmentFacade facade2 = new EquipmentFacade();
            Equipment equip = facade2.GetMerchantAppItem(ID, UserSessions.CurrentUser.TimeZone);
            FormBinding.BindObjectToControls(equip, pnlEquipment);
            //PXP-3014
            if (UserSessions.CurrentMerchantApp != null)
            {
                EMVComplianceMerchant.SelectedValue = UserSessions.CurrentMerchantApp.EMVComplianceMerchant;
            }
        }
    }

    public bool FormSave(string merchantappuid)
    {
        //save equipment
        EquipmentFacade facade = new EquipmentFacade();

        Equipment equip = null;
        Equipment clone2 = null;
        bool perform = false;

        bool IsNewEquipment = CommonUtility.Util.if_b(this.IsNewItem.Value, false);

        MerchantFacade merchantFacade = new MerchantFacade();
        MerchantApp merchantAppCurrent = new MerchantApp();
        if (UserSessions.CurrentMerchantApp != null)
        {
            merchantAppCurrent = UserSessions.CurrentMerchantApp;
        }
        else
        {
            merchantAppCurrent = UserSessions.diCurrentMerchantApp[merchantappuid];
        }

        if (FormDataCheck())
        {
            if (!string.IsNullOrWhiteSpace(this.ItemUID.Value) && IsNewEquipment)
            {
                equip = new Equipment();
            }
            else
            {
                equip = facade.GetMerchantAppItem(this.SelectedUID, UserSessions.CurrentUser.TimeZone);

                if (equip != null)
                {
                    clone2 = (Equipment)equip.Clone();
                }
            }

            FormBinding.BindControlsToObject(equip, pnlEquipment);

            equip.MerchantAppsUID = merchantappuid;
            int QIRId = 0;
            if (!string.IsNullOrEmpty(QIUlookup.m_QIRId))
            {
                QIRId = Convert.ToInt32(QIUlookup.m_QIRId);
            }

            equip.QIRVendorId = QIRId;
            //PXP-3014
            equip.EMVComplianceMerchant = EMVComplianceMerchant.SelectedValue;
            //PXP:5768: Ani: Zeus: Rename 'Cambridge' office as 'London'
            if (merchantAppCurrent.Office.Equals(CommonUtility.Util.Offices.London) || merchantAppCurrent.Office.Equals(CommonUtility.Util.Offices.Montreal))
            {
                if (CommonUtility.Util.IsValidGuid(merchantappuid))
                {
                    Hashtable prms = new Hashtable();
                    prms.Add("@MerchantAppUID", merchantappuid);
                    prms.Add("@IsEnabled", true);
                    DataSet ds = facade.GetMerchantAppItem(prms);
                    int totalRows = ds.Tables[0].Rows.Count;
                    int rowsHandpoint = ds.Tables[0].Select("EquipmentMakerUID = " + "'" + Constants.ITEMS_HANDPOINT + "'").Length;
                    if (totalRows.Equals(rowsHandpoint) && totalRows > 0)
                    {
                        equip.EMVComplianceMerchant = "1";//Merchant EMV Compliant
                    }
                    else
                    {
                        equip.EMVComplianceMerchant = string.Empty;
                    }
                }

                merchantAppCurrent.EMVComplianceMerchant = equip.EMVComplianceMerchant;
            }

            if (IsNewEquipment)
            {
                equip.UserCreated = UserSessions.CurrentUser.UserName;
                perform = facade.InsertEquipment(equip);
            }
            else
            {
                equip.UserUpdated = UserSessions.CurrentUser.UserName;
                perform = facade.UpdateEquipment(equip);
                FormHandler.LogFormChanges("", equip.UID, 0, clone2, equip);
            }
            CheckMerchantEMVbyEquipments(ref merchantAppCurrent, facade, merchantappuid);


            if (perform)
            {
                this.LoadEquipments(merchantappuid);
                this.FormShow(equip.UID);
            }
            grd.SelectedIndex = -1;

        }
        else if (grd.Rows.Count > 0)
            perform = true;
        //PXP-3433
        if (this.IsEMVMerchantValueChanged)
        { //Set value from the drop down directly if user Selected a value manually
            this.IsEMVMerchantValueChanged = false;
            merchantAppCurrent.EMVComplianceMerchant = EMVMerchantSelectedValue;
        }
        merchantFacade.UpdateMerchantApp(merchantAppCurrent);

        return perform;
    }

    public bool FormDataCheck()
    {
        lblError.Text = string.Empty;
        string message = string.Empty;
        if (this.ItemUID.Value == string.Empty)
            message += "Please select an Equipment.<br/>";
        else
        {
            if (TerminalStatusUID.SelectedIndex == 0)
                message += "Please select Terminal Status.<br/>";
            if (DeployTypeID != null && DeployTypeID.SelectedIndex == 0)
                message += "Please select a Deploy Type.<br/>";
        }
        lblError.Text = message;
        return message == string.Empty;
    }

    protected void btnLookup_Click(object sender, EventArgs e)
    {
        FormBinding.BindObjectToControls(new Equipment(), pnlEquipment);
        ListHandler.ListFindItem(TerminalStatusUID, "7839B1A8-6EC7-46D3-807E-1C192F8EF65A");
        this.ItemUID.Value = string.Empty;
        this.IsNewItem.Value = "1";
        tbEquipmentSearch.Text = "";
        TextBox txtQIRCompany = (TextBox)QIUlookup.FindControl("txtQIRCompany");
        txtQIRCompany.Text = string.Empty;
        TextBox txtQIRID = (TextBox)QIUlookup.FindControl("txtQIRVID");
        txtQIRID.Text = string.Empty;
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    public void FormClear()
    {
        FormHandler.ClearAllControls(pnlEquipment);
        grd.DataSource = null;
        grd.SelectedIndex = -1;
        grd.DataBind();
        ListHandler.ListFindItem(TerminalStatusUID, "7839B1A8-6EC7-46D3-807E-1C192F8EF65A");
    }

    protected void grdDocuments_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage1 = e.NewPageIndex + 1;
        this.LoadDocuments();
    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();

        if (UserSessions.CurrentMerchantApp != null)
        {
            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            prms.Add("@DocTypeGroupID", MDoc.eMDocTypeGroup.Merchant);
            prms.Add("@DocTypeID", (int)MDoc.eMDocType.GatewayHardwareInformation);

            if (!prms.ContainsKey("@PageSize"))
                prms.Add("@PageSize", this.PageSize1);
            else
                prms["@PageSize"] = this.PageSize1;

            if (!prms.ContainsKey("@CurrentPage"))
                prms.Add("@CurrentPage", this.CurrentPage1);
            else
                prms["@CurrentPage"] = this.CurrentPage1;

            if (!prms.ContainsKey("@SortOrder"))
                prms.Add("@SortOrder", "DocID");
            else
                prms["@SortOrder"] = this.SortOrder1;

            prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch1);

            if (UserSessions.CurrentUser.IsBank)
                prms["@IsBank"] = 1;

            e.InputParameters[0] = prms;

            int cnt = DataMerchantAppPaging.GetDocumentPagingCount(prms, 0, 0);

            if (cnt > 0)
            {
                pnlEquipmentDocs.Visible = true;
                litRecordCount.Text = cnt.ToString();
            }
            else
            {
                pnlEquipmentDocs.Visible = false;
            }
        }

    }

    protected void grdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                var row = ((DataRowView)e.Row.DataItem).Row;

                MDoc objM = DataMerchantAppPaging.getMDoc(row);


                // code changes for PXP-6927 by koshlendra start
                string file_ext = objM.OrigName.ToUpper().Substring(objM.OrigName.ToUpper().LastIndexOf('.') + 1).ToUpper();
                System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
                img1.ImageUrl = (WebUtil.GetDocIconUrl(file_ext));
                // code changes for PXP-6927 by koshlendra end

                HyperLink lnk = (HyperLink)e.Row.FindControl("hypOrigName");

                Dictionary<string, string> di = new Dictionary<string, string>();

                di["DocID"] = DataBinder.Eval(e.Row.DataItem, "DocID").ToString();
                di["MerchantAppUID"] = UserSessions.CurrentMerchantApp.MerchantAppUID;
                di["MerchantID"] = UserSessions.CurrentMerchantApp.ID;

                lnk.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di)));

                // strips off the directory and just puts the filename.
                string[] arr = lnk.Text.Split(new char[] { '\\' });
                lnk.Text = arr[arr.Length - 1];

                e.Row.Cells[8].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[8].Text);
                break;

            default:
                break;
        }
    }

    private void LoadDocuments()
    {
        grdDocuments.PageIndex = this.CurrentPage1 - 1;
        grdDocuments.PageSize = this.PageSize1;

        if(UserSessions.CurrentMerchantApp != null)
            grdDocuments.DataBind();
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage1 = 1;
        this.PageSize1 = Convert.ToInt32(ddlPageSize.SelectedValue);

        this.LoadDocuments();
    }
    //PXP-3427
    protected void EMVComplianceMerchant_SelectedIndexChanged(object sender, EventArgs e)
    {
        //PXP-3433
        this.IsEMVMerchantValueChanged = true;
        this.EMVMerchantSelectedValue = EMVComplianceMerchant.SelectedValue;
    }

    //PXP-3433
    public void CheckMerchantEMVbyEquipments(ref MerchantApp merchantAppCurrent, EquipmentFacade facade, string merchantappuid) //, Equipment equip) //Sets MerchantEMVCompliance based on the equipments present for that merchant.
    {
        int totalRows;
        int rowsHandpoint;
        if (CommonUtility.Util.IsValidGuid(merchantappuid))
        {
            if (facade == null)
            {
                facade = new EquipmentFacade();
            }
            Hashtable prms = new Hashtable();
            prms.Add("@MerchantAppUID", merchantappuid);
            prms.Add("@IsEnabled", true);
            DataSet ds = facade.GetMerchantAppItem(prms);
            totalRows = ds.Tables[0].Rows.Count;
            rowsHandpoint = ds.Tables[0].Select("EquipmentMakerUID = " + "'" + Constants.ITEMS_HANDPOINT + "'").Length;
            if (totalRows.Equals(0))
            {
                merchantAppCurrent.EMVComplianceMerchant = string.Empty;
            }
            //PXP:5768: Ani: Zeus: Rename 'Cambridge' office as 'London'
            if (merchantAppCurrent.Office.Equals(CommonUtility.Util.Offices.London) || merchantAppCurrent.Office.Equals(CommonUtility.Util.Offices.Montreal))
            {
                if (totalRows.Equals(rowsHandpoint) && totalRows > 0)
                {
                    merchantAppCurrent.EMVComplianceMerchant = "1";
                }
                else
                {
                    merchantAppCurrent.EMVComplianceMerchant = string.Empty;
                }
            }
            //If drop down changed manually then set that value.
            if (this.IsEMVMerchantValueChanged)
            { //Set value from the drop down directly if user Selected a value manually
                this.IsEMVMerchantValueChanged = false;
                merchantAppCurrent.EMVComplianceMerchant = EMVMerchantSelectedValue;
            }
        }
    }
    //Code added by amit for PXP-7621
    private string getTimeZone(string State)
    {
        switch (State)
        {
            case "TN":
            case "TX":
            case "IL":
            case "KS":
            case "AL":
            case "WI":
            case "MN":
            case "MO":
            case "AR":
            case "OK":
            case "NE":
            case "ND":
            case "IA":
            case "MS":
            case "LA":
            case "SD":
                return "Central";

            case "PW":
            case "FM":
            case "MH":
                return string.Empty;

            case "VT":
            case "VA":
            case "NC":
            case "NJ":
            case "PA":
            case "NY":
            case "FL":
            case "MA":
            case "RI":
            case "OH":
            case "GA":
            case "NH":
            case "DE":
            case "CT":
            case "KY":
            case "MD":
            case "SC":
            case "IN":
            case "MI":
            case "ME":
            case "DC":
            case "WV":
                return "Eastern";

            case "MT":
            case "CO":
            case "WY":
            case "AZ":
            case "ID":
            case "NM":
            case "UT":
                return "Mountain";

            case "GU":
            case "AS":
            case "MP":
            case "PR":
            case "VI":
                return "Atlantic";

            case "AK":
                return "Alaska";

            case "NV":
            case "OR":
            case "WA":
            case "CA":
                return "Pacific";

            case "HI":
                return "Hawaii-Aleutian";

            default:
                return string.Empty;
        }
    }
    private string getETCType(string ETCTypeUID)
    {
        switch (ETCTypeUID.ToLower())
        {
            case "130d20eb-6223-4e2b-aa51-31ebd5bdb400":
                return "Tape";
            case "5e11ee47-1952-4886-a75b-7955a753f4f8":
                return "ETC7";
            case "281d82c5-fa35-472f-985a-feec0cb0f223":
                return "ETC7";
            case "d7159b45-a1eb-469c-9c3c-e148b94d3c6c":
                return "ETCB";
            default:
                return string.Empty;
        }
    }

    private string getReleaseType(string releaseUID)
    {
        switch (releaseUID.ToLower())
        {
            case "0da44562-cd00-4d46-8d93-1235304efc9c":
                return "12-Month Rolling";
            case "2ec149a7-6ddb-4676-a875-2c0a4b63d573":
                return "Set Schedule";
            case "f9d4ef85-ce82-4405-bf27-b8d8054f3d71":
                return "6-Month Rolling";
            case "b4fd0f67-896e-4e70-84a0-d023d5a65fd9":
                return "Periodic Review";
            case "158e8075-f0ae-4984-9854-e6a572948521":
                return "Other";
            case "2e347a16-b2f2-4f4b-8e2d-efec034c61e4":
                return "9-Month Rolling";
            default:
                return string.Empty;
        }
    }

    private string getAppType(string typeUID)
    {
        switch (typeUID.ToLower())
        {
            case "7033f360-69ec-450a-9ac9-2342d2663809":
                return "Petroleum";
            case "eef505e5-cd91-4b35-af56-471cece2bf5e":
                return "Retail";
            case "8e8c5f97-565f-4fe4-aa53-615345aefb46":
                return "Restaurant w/ Tips";
            case "16797c45-85f2-4bbf-a6af-659a260997fd":
                return "Mail Order / Internet";
            case "007874b2-635a-4c55-876b-6b16c3b885a7":
                return "Check";
            case "b44d8e49-fead-42d3-ad86-7a1da15fcc60":
                return "Hospitality / Lodging";
            case "2f7efee9-c9bf-4b48-9dd7-a9729ffbca51":
                return "Restaurant w/o Tips";
            case "fdf490ad-7aaf-49ab-b7ed-d8e476f1a9f7":
                return "Stored Value";
            default:
                return string.Empty;
        }
    }
    public static bool IsPXPEnabled()
    {
        bool ret = false;

        Hashtable prms = new Hashtable();
        prms.Add("@HookTableKeyUID", UserSessions.CurrentMerchantApp.MerchantAppUID);
        prms.Add("@UserName", UserSessions.CurrentMerchantApp.ID);
        UserFacade facade3 = new UserFacade();
        User user = facade3.GetUser(prms);

        if (user != null && user.UserPortals != null)
        {
            foreach (UserPortal item in user.UserPortals)
            {
                if (item.PortalID.ToUpper() == PaymentXP.BusinessObjects.Constants.PORTAL_PAYMENTXP.ToUpper() && item.Enabled)
                {
                    ret = true;
                    break;
                }
            }

        }

        return ret;
    }
    public void EnableDisableEquipmentGrid(bool editMode)
    {
        grd.Enabled = true;
        for (int i = 0; i < grd.Rows.Count; i++)
        {
            LinkButton lbSelect = (LinkButton)grd.Rows[i].FindControl("Select");
            LinkButton lbDelete = (LinkButton)grd.Rows[i].FindControl("Delete");
            if (editMode)
            {
                lbDelete.Enabled = true;
                lbSelect.Enabled = true;
            }
            else
            {
                lbDelete.Enabled = false;
                lbSelect.Enabled = false;
            }
        }
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>disableEquipmentPaging('" + editMode + "');</script>", false);
    }
    //End Code added by amit for PXP-7621
}
