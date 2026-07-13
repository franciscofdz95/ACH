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
using Infragistics.Web.UI.EditorControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects;
using System.Collections.Generic;
using PaymentXP.BusinessObjects.Reponse;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using System.Text;
using PaymentXP.BusinessObjects.Request;
using System.Xml;
using iTextSharp.text.pdf;
using System.Xml.Xsl;
using System.Xml.XPath;
using Winnovative;
using System.Text.RegularExpressions;
using CommonUtility;
using ZeusWeb;
using System.Web.Configuration;

public partial class wucOwnerUW : System.Web.UI.UserControl
{

    public WebDialogWindow DialogWindow
    {
        get { return WebDialogWindow2; }
    }

    public Button btnHistory
    {
        get { return History; }
    }

    public Button SaveButton
    {
        get { return btnSave; }
    }

    public string SetFullName
    {
        get { return lblFullName.Text; }
        set { lblFullName.Text = value; }
    }

    public string SetTitle
    {
        get { return lblTitle.Text; }
        set { lblTitle.Text = value; }
    }

    //PXP-2891 Rohit Thakur
    public Panel panelBeneficialOwner
    {
        get { return pnlBeneficialOwner; }
    }

    public Panel panelAuthorizedSignature
    {
        get { return pnlAuthorizedSignature; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //WebUtil.SetUserSpecificDisplayMode(CreditDate);
            //WebUtil.SetUserSpecificDisplayMode(OldestTrade);
            //WebUtil.SetUserSpecificDisplayMode(TOldestTrade);
            //WebUtil.SetUserSpecificDisplayMode(TCreditDate);
            LoadCreditHistory();
            ClearControls();
        }

    }

    private void ClearControls()
    {
        this.ddlCreditTypeID.SelectedValue = "-1";
        this.CreditScore0.Text = string.Empty;
        this.OldestTrade.Text = DateTime.Today.ToShortDateString(); ;
        this.NoofTrades0.Text = string.Empty;
        this.txtNotes.Text = string.Empty;
        btnSave.Text = "Save & Close";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        bool perform = false;

        if (ddlCreditTypeID.SelectedValue == "-1")
        {
            lblError.Text = "Please select credit type.";
            return;
        }

        int creditscore = CommonUtility.Util.if_i(CreditScore0.ValueInt, 0);

        if (creditscore != 0)
        {

            prms.Add("@CreditTypeID", Convert.ToInt32(ddlCreditTypeID.SelectedValue));
            prms.Add("@OwnershipUID", btnSave.CommandArgument);
            prms.Add("@CreditScore", CreditScore0.ValueInt);
            prms.Add("@Notes", txtNotes.Text);
            prms.Add("@OldestTrade", DataLayer.Date2Field(OldestTrade.Value));
            prms.Add("@NoofTrades", NoofTrades0.ValueInt);
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
            if (btnSave.Text == "Save & Close")
            {
                DataAccess.DataUnderwritingDao.InsertCreditHistory(prms);
                perform = true;
            }
            else
            {
                if (btnSave.Text == "Update & Close" && !string.IsNullOrWhiteSpace(lblUID.Text))
                {
                    prms.Add("@UID", lblUID.Text);
                    DataAccess.DataUnderwritingDao.UpdateCreditHistory(prms);
                    perform = true;
                }
            }
        }
        else
        {
            if (creditscore > 1000)
                lblError.Text = "Credit score should be less than 1000";
            else
                lblError.Text = "Please enter credit score.";
        }



        if (perform && LoadCreditHistory())
        {
            LoadOwnerGrid();
            FormClose();


        }

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        FormClose();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        LoadCreditHistory();
    }

    private void FormClose()
    {
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

        lblError.Text = "";
    }
    protected void grdOwner_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;

            case DataControlRowType.DataRow:
                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);
                e.Row.Cells[5].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[5].Text);




                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
       
    }

    protected void grdOwner_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton btn = null;
        if (e.CommandSource is LinkButton)
        {
            btn = (LinkButton)e.CommandSource;
            switch (btn.CommandName)
            {
                case "Select":
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);

                    lblCreditDate.Text = row.Cells[1].Text;
                    ddlCreditTypeID.ClearSelection();
                    string crTypeID = HttpUtility.HtmlDecode(row.Cells[2].Text);
                    ddlCreditTypeID.Items.FindByText(crTypeID).Selected = true;
                    CreditScore0.Text = row.Cells[3].Text.Trim();
                    NoofTrades0.Text = row.Cells[4].Text.Trim();
                    OldestTrade.Value = row.Cells[5].Text;
                    txtNotes.Text = row.Cells[6].Text;
                    lblUID.Text = row.Cells[7].Text;

                    btnSave.Text = "Update & Close";


                    break;
            }

        }
        else
            return;
    }

    private void LoadOwnerGrid()
    {
        DataSet ds = DataAccess.DataUnderwritingDao.GetCreditHistory(UserSessions.CurrentMerchantApp.MerchantAppUID, btnSave.CommandArgument);
        grdOwner.DataSource = ds;
        grdOwner.DataBind();
    }

    private bool LoadCreditHistory()
    {
        DataSet ds = DataAccess.DataUnderwritingDao.GetCreditHistory(UserSessions.CurrentMerchantApp.MerchantAppUID, btnSave.CommandArgument);
        grdCreditHistory.DataSource = ds;
        grdCreditHistory.DataBind();

        return (ds.Tables[0].Rows.Count > 0);
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        LoadOwnerGrid();
        lblOwerUID.Text = btnSave.CommandArgument.ToString();
        WebDialogWindow2.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;

        lblCreditDate.Text = DateTime.Now.ToString();
        ClearControls();
    }
    protected void btnGet_Click(object sender, EventArgs e)
    {
        btnGetOld.Visible = false; 

        if (UserSessions.CurrentMerchantApp.Owners == null || UserSessions.CurrentMerchantApp.Owners.Count == 0)
        {
            FormHandler.DisplayMessage(Page.ClientScript, "Owner information for this merchant is missing."); return;
        }
        else
        {

            if(string.IsNullOrWhiteSpace(lblFullName.Text))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Owner Name is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(SSN.Text.Replace("-", "")))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Owner SSN is missing."); return;
            }
            else if (string.IsNullOrWhiteSpace(hidAddress.Value))
            {
                FormHandler.DisplayMessage(Page.ClientScript, "Owner address is missing."); return;
            }
            else
            {
                btnGetOld.Visible = true; //btnNo.Visible = true; Button1.Visible = true;

                Hashtable prms = new Hashtable();
                prms.Add("@ReqOwnerSSN", SSN.Text.Replace("-", ""));

                DataNetConnect objNC = new DataNetConnect();
                ZeusBusinessProfile objPP = new ZeusBusinessProfile();
                objPP = objNC.GetBusinessProfile(prms);

                if (objPP != null)
                {
                    lbl.Text = "You are about to pull credit for " + lblFullName.Text + ". The last credit report was pulled on " + objPP.LastCreditDate.ToShortDateString() + " for ZID: " + objPP.MerchantID + ". Press Ok to proceed.";
                }
                else
                {
                    lbl.Text = "You are about to pull credit for " + lblFullName.Text + ". Press Ok to proceed.";
                    btnGetOld.Visible = false;
                }
            }

        }

        WebDialogWindow1.WindowState = DialogWindowState.Normal;

    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        WebDialogWindow1.WindowState = DialogWindowState.Hidden;
    }

    protected void btnGetOld_Click(object sender, EventArgs e)
    {
        string m_CreditScore = null;
        DateTime m_CreditDate = DateTime.MinValue;
        string m_NoOfTrades = null;
        string m_OldestTrade = null;
        string m_OwnerShipUID = null;

        Hashtable prms = new Hashtable();
        prms.Add("@ReqOwnerSSN", SSN.Text.Replace("-","").Trim());

        DataNetConnect objNC = new DataNetConnect();
        ZeusBusinessProfile objPP = new ZeusBusinessProfile();
        objPP = objNC.GetBusinessProfile(prms);

        m_OwnerShipUID = btnSave.CommandArgument.ToUpper();

        if (objPP != null)
        {
            m_CreditScore = objPP.ResCreditScore.ToString();
            m_NoOfTrades = objPP.ResNumOfTrade.ToString();
            if(objPP.ResLastTradeDate != null)
                m_OldestTrade = objPP.ResLastTradeDate.Value.ToString();
            if(objPP.ResCreditReportDate != null)
                m_CreditDate = objPP.ResCreditReportDate.Value;
            btnView.Visible = true;
            btnAdd.Visible = (objPP.DocID <= 0);
            btnClear.Visible = false;
            //BusinessProfileID.Value = objPP.BusinessProfileID.ToString();
            if (objPP.LastCreditDate != null)
                CreditReportDate.Value = objPP.LastCreditDate.ToShortDateString();
            if (objPP.ResRealEstateLoanBalance > 0)
                ListHandler.ListFindItem(NameAddressSSNSummary, "1");

            btnView.Enabled = true;

            objPP.UserCreated = UserSessions.CurrentUser.UserName;
            objPP.UserModified = string.Empty;
            objPP.DateModified = null;
            objPP.DateCreated = DateTime.Now;
            objPP.IsCurrent = false;

            if (objPP.MerchantID.ToString() != UserSessions.CurrentMerchantApp.ID)
            {
                objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);

                if (objPP.MerchantID > 0)
                    BusinessProfileID.Value = objNC.InsertBusinessProfile(objPP);
            }
        }
        else
        {
            //CreditScore.Text = string.Empty;
            //NoofTrades.Text = string.Empty;
            //OldestTrade.Value = null;
            //CreditDate.Value = null;
            btnView.Visible = false;
            btnAdd.Visible = false;
            btnClear.Visible = false;
            BusinessProfileID.Value = "";
            CreditReportDate.Value = "";
            ListHandler.ListFindItem(NameAddressSSNSummary, "-1");
        }



        Hashtable para = new Hashtable();
        if (!string.IsNullOrWhiteSpace(m_CreditScore) && m_CreditScore.Trim() != "0")
        {
            para.Add("@OwnershipUID", m_OwnerShipUID);
            para.Add("@CreditScore", m_CreditScore);
            para.Add("@OldestTrade", m_OldestTrade);
            para.Add("@Notes", string.Empty);
            para.Add("@NoofTrades", m_NoOfTrades);
            para.Add("@UserCreated", "System");
            para.Add("@CreditTypeID", Owner.CreditType.Experian);

            DataAccess.DataUnderwritingDao.InsertCreditHistory(para);

            AddPDF();
        }

        LoadCreditHistory();
        WebDialogWindow1.WindowState = DialogWindowState.Hidden;


    }

    protected void btnGet1_Click(object sender, EventArgs e)
    {
        try
        {
            // in response check fro the object ProfileSummary and in that we have a  field called RealEstateBalance,which determines the Property field
            Logging.ExperianLog.InfoFormat("[ZID={0}] Retrieving credit report for owner......", UserSessions.CurrentMerchantApp.ID);
            NetConnectRequest objNCR = new NetConnectRequest();
            DataNetConnect objNC = new DataNetConnect();

            objNCR.EAI = "FL1QEPJZ";

            if (ConfigurationManager.AppSettings["ExperianUserID"].ToString().ToUpper() == "MERITUS_DEMO")
            {
                objNCR.DBHost = NetConnectRequestDBHost.BISTEST;
                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian environment: TEST", UserSessions.CurrentMerchantApp.ID);
            }
            else
            {
                objNCR.DBHost = NetConnectRequestDBHost.BISPROD;
                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian environment: PRODUCTION", UserSessions.CurrentMerchantApp.ID);
            }

            objNCR.Request = new RequestType();

            objNCR.Request.Products = new RequestTypeProducts();

            BusinessProfileType bpt = new BusinessProfileType();

            bpt = FillBOPRequest();

            objNCR.Request.Products.Item = bpt;

            string errCertificate = objNC.GetExperianCertificationTest();

            bool isPassed = (errCertificate == string.Empty);

            if (isPassed)
            {
                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian Certificate Test: Passed", UserSessions.CurrentMerchantApp.ID);

                // convert object to XML
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(NetConnectRequest));
                StringWriter textWriter = new StringWriter();
                xmlSerializer.Serialize(textWriter, objNCR);

                string request_body_xml = textWriter.ToString().TrimStart('{').TrimEnd('}');
                string postData = "NETCONNECT_TRANSACTION=" + System.Web.HttpUtility.UrlEncode(request_body_xml);

                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian Request: {1}", UserSessions.CurrentMerchantApp.ID, MaskSSN(request_body_xml, bpt.BusinessOwner.SSN));

                //Enforce the call to use TLS 1.2,
                //By Default Framework 4.5 will resolve to SSL3 and TLS 1.0 is they are present on the server.
                string TLSVersion = ConfigurationManager.AppSettings["TLSVersion"].ToString();

                // Chandra: PXP-2982 TLS1.2: Zeus
                //System.Net.ServicePointManager.SecurityProtocol = TLSVersion.Equals("TLS12") ? System.Net.SecurityProtocolType.Tls12 : System.Net.SecurityProtocolType.Tls11;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;


                Crypto crypto = new Crypto();
                HttpWebRequest experianRequest = (HttpWebRequest)WebRequest.Create(objNC.GetExperianServerName());
                experianRequest.Method = "POST";
                experianRequest.ContentType = "application/x-www-form-urlencoded";
                string UserIDFormated = ConfigurationManager.AppSettings["ExperianUserID"].ToString() + ":" + crypto.Decrypt(ConfigurationManager.AppSettings["ExperianPassword"].ToString());
                experianRequest.Headers.Add("Authorization", "BASIC " + objNC.ConvertToBase64String(UserIDFormated));
                experianRequest.Timeout = 100000;
                experianRequest.KeepAlive = false;
                experianRequest.Credentials = System.Net.CredentialCache.DefaultCredentials;

                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian credentials set.", UserSessions.CurrentMerchantApp.ID);

                System.Text.ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] byteData;
                byteData = encoding.GetBytes(postData);

                experianRequest.AllowAutoRedirect = true;
                experianRequest.ContentLength = byteData.Length;
                Stream newStream = experianRequest.GetRequestStream();

                Logging.ExperianLog.InfoFormat("[ZID={0}] Writing request byte [len={1}]......", UserSessions.CurrentMerchantApp.ID, byteData.Length);

                newStream.Write(byteData, 0, byteData.Length);
                newStream.Close();

                Logging.ExperianLog.InfoFormat("[ZID={0}] Request byte [len={1}] sent.", UserSessions.CurrentMerchantApp.ID, byteData.Length);

                GetReponse(experianRequest);
            }
            else
            {
                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian Certificate error: {1}", UserSessions.CurrentMerchantApp.ID, errCertificate);
                WucMessage1.AddMessageError(errCertificate);
            }
        }
        catch (Exception ex)
        {
            Logging.ExperianLog.ErrorFormat("[ZID={0}] Failed to send request to Experian: {1}", UserSessions.CurrentMerchantApp.ID, ex.Message);
            Logging.ExperianLog.ErrorFormat("[ZID={0}] Stack Trace: {1}", UserSessions.CurrentMerchantApp.ID, ex.StackTrace);

            WucMessage1.AddMessageError("An error has occurred while processing your Experian Credit Report request. Please contact IT, if problems persist.");
        }

        WebDialogWindow1.WindowState = DialogWindowState.Hidden;
    }

    private void GetReponse(HttpWebRequest experianRequest)
    {
        string m_CreditScore = null;
        DateTime m_CreditDate;
        string m_NoOfTrades = null;
        string m_OldestTrade = null;
        string m_OwnerShipUID = null;
        try
        {
            NetConnectResponse objAFR = new NetConnectResponse();
            XmlSerializer xmlserial = new XmlSerializer(typeof(NetConnectResponse));
            HttpWebRequest experianRequest1 = experianRequest;

            Logging.ExperianLog.InfoFormat("[ZID={0}] Retrieving Experian response...", UserSessions.CurrentMerchantApp.ID);

            HttpWebResponse experianResponse = (HttpWebResponse)experianRequest.GetResponse();

            Logging.ExperianLog.InfoFormat("[ZID={0}] Experian response status: {1} - {2}", UserSessions.CurrentMerchantApp.ID, experianResponse.StatusCode.GetHashCode(), experianResponse.StatusCode.ToString());

            if (experianResponse.StatusCode.ToString() == "OK")
            {
                Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian response...", UserSessions.CurrentMerchantApp.ID);

                using (var reader = new StreamReader(experianResponse.GetResponseStream()))
                {
                    string xml = reader.ReadToEnd();
                    Logging.ExperianLog.InfoFormat("[ZID={0}] Experian response: {1}", UserSessions.CurrentMerchantApp.ID, xml);

                    var doc = new XmlDocument();
                    doc.LoadXml(xml);

                    Logging.ExperianLog.InfoFormat("[ZID={0}] Deserializing Experian response......", UserSessions.CurrentMerchantApp.ID);

                    MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(doc.InnerXml));
                    NetConnectResponse resultingMessage = (NetConnectResponse)xmlserial.Deserialize(memStream);
                    string realestatebal = string.Empty;

                    Logging.ExperianLog.InfoFormat("[ZID={0}] Experian response deserialized, completion code: {1}", UserSessions.CurrentMerchantApp.ID, resultingMessage.CompletionCode);

                    if (resultingMessage.CompletionCode == "4000")
                    {
                        Logging.ExperianLog.InfoFormat("[ZID={0}] Experian error: {1}", UserSessions.CurrentMerchantApp.ID, resultingMessage.ErrorMessage);
                        WucMessage1.AddMessageError(resultingMessage.ErrorMessage);
                        return;
                    }
                    else if (resultingMessage.CompletionCode == "2000")
                    {
                        Logging.ExperianLog.InfoFormat("[ZID={0}] Experian error: {1}", UserSessions.CurrentMerchantApp.ID, resultingMessage.ErrorMessage);
                        WucMessage1.AddMessageError("Authorization failed. Please update the password.");
                        return;
                    }
                    else if (resultingMessage.CompletionCode != "0000")
                    {
                        Logging.ExperianLog.InfoFormat("[ZID={0}] Experian error: {1}", UserSessions.CurrentMerchantApp.ID, resultingMessage.ErrorMessage);
                        WucMessage1.AddMessageError("System error. Call Experian Technical Support at 1 800-854-7201.");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(resultingMessage.ErrorMessage))
                    {
                        ProductsType objBOP = (ProductsType)resultingMessage.Item;

                        Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian Credit report......", UserSessions.CurrentMerchantApp.ID);

                        for (int k = 0; k < objBOP.Items.Length; k++)
                        {
                            if (objBOP.Items[k].GetType() == typeof(BusinessProfile))
                            {
                                BusinessProfile objBP = (BusinessProfile)objBOP.Items[k];

                                Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian Business profile......", UserSessions.CurrentMerchantApp.ID);

                                for (int i = 0; i < objBP.Items.Length; i++)
                                {
                                    if (objBP.Items[i].GetType() == typeof(BIS_ProcessingMessage))
                                    {
                                        BIS_ProcessingMessage procMsg = (BIS_ProcessingMessage)objBP.Items[i];
                                        if (procMsg.ProcessingAction != null && procMsg.ProcessingAction.code != null)
                                        {
                                            Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian Business profile processing code: {1}", UserSessions.CurrentMerchantApp.ID, procMsg.ProcessingAction.code);
                                            Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian Business profile processing value: {1}", UserSessions.CurrentMerchantApp.ID, procMsg.ProcessingAction.Value);

                                            if (procMsg.ProcessingAction.code == "031")
                                            {
                                                //try again request?
                                                GetReponse(experianRequest1);
                                                break;
                                            }
                                            else if (!string.IsNullOrWhiteSpace(procMsg.ProcessingAction.Value))
                                            {
                                                WucMessage1.AddMessageError(procMsg.ProcessingAction.Value.Trim());
                                            }
                                            else if (!string.IsNullOrWhiteSpace(procMsg.ProcessingAction.code))
                                            {
                                                WucMessage1.AddMessageError("System Error. Contact Experian.");
                                            }
                                        }
                                    }
                                }

                                Logging.ExperianLog.InfoFormat("[ZID={0}] Experian Business profile read complete.", UserSessions.CurrentMerchantApp.ID);
                            }
                            else if (objBOP.Items[k].GetType() == typeof(PaymentXP.BusinessObjects.Reponse.CreditProfileType))
                            {
                                PaymentXP.BusinessObjects.Reponse.CreditProfileType objBP = (PaymentXP.BusinessObjects.Reponse.CreditProfileType)objBOP.Items[k];

                                Logging.ExperianLog.InfoFormat("[ZID={0}] Reading Experian Credit profile......", UserSessions.CurrentMerchantApp.ID);

                                if (objBP.RiskModel != null && objBP.RiskModel.Length > 0)
                                // CreditScore.Text = CommonUtility.Util.if_s(CommonUtility.Util.if_i(objBP.RiskModel[0].Score, 0));
                                {
                                    m_CreditScore = objBP.RiskModel[0].Score;
                                    Logging.ExperianLog.InfoFormat("[ZID={0}] Experian Credit score: {1}", UserSessions.CurrentMerchantApp.ID, m_CreditScore);
                                }

                                m_CreditDate = DateTime.Today;

                                if (objBP.TradeLine != null)
                                    m_NoOfTrades = objBP.TradeLine.Length.ToString();

                                if (objBP.ProfileSummary != null && objBP.ProfileSummary.Length > 0)
                                {
                                    if (objBP.ProfileSummary[0].OldestTradeOpenDate != null)
                                    {
                                        int dt = 1;
                                        int yr = 1;
                                        int mn = 1;

                                        if (objBP.ProfileSummary[0].OldestTradeOpenDate.Length >= 2)
                                            mn = CommonUtility.Util.if_i(objBP.ProfileSummary[0].OldestTradeOpenDate.Substring(0, 2), 0);
                                        if (objBP.ProfileSummary[0].OldestTradeOpenDate.Length >= 4)
                                            yr = CommonUtility.Util.if_i(objBP.ProfileSummary[0].OldestTradeOpenDate.Substring(2), 0);

                                        m_OldestTrade = DateTime.Parse(mn.ToString() + "/" + dt.ToString() + "/" + yr.ToString()).ToShortDateString();
                                    }

                                    realestatebal = objBP.ProfileSummary[0].RealEstateBalance;
                                }
                                else if (objBP.InformationalMessage != null && objBP.InformationalMessage.Length > 0)
                                {
                                    for (int m = 0; m < objBP.InformationalMessage.Length; m++)
                                    {
                                        Logging.ExperianLog.ErrorFormat("[ZID={0}] Experian Credit Profile error: {1}", UserSessions.CurrentMerchantApp.ID, objBP.InformationalMessage[m].MessageText);
                                        WucMessage1.AddMessageError(objBP.InformationalMessage[m].MessageText);
                                    }
                                }
                            }
                        }

                        if (WucMessage1.ErrorCount() == 0)
                        {
                            #region Values to database

                            Logging.ExperianLog.InfoFormat("[ZID={0}] Saving Owner [UID={1}] Business credit profile......", UserSessions.CurrentMerchantApp.ID, btnSave.CommandArgument);

                            ZeusBusinessProfile objZBP = new ZeusBusinessProfile();

                            objZBP.DateCreated = DateTime.Now;
                            objZBP.UserCreated = UserSessions.CurrentUser.UserName;
                            objZBP.ResponseXML = doc.InnerXml;
                            objZBP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                            objZBP.ReqOpInitials = "MG";
                            objZBP.ReqSubCode = ConfigurationManager.AppSettings["SubCode"].ToString();// "688500";
                            objZBP.ReqBusinessName = UserSessions.CurrentMerchantApp.BusinessDBAName;
                            objZBP.ReqCurrentAddress = UserSessions.CurrentMerchantApp.BusinessAddress;
                            objZBP.ReqCurrentAddressCity = UserSessions.CurrentMerchantApp.BusinessCity;
                            objZBP.ReqCurrentAddressState = UserSessions.CurrentMerchantApp.BusinessState;
                            objZBP.ReqCurrentAddressZip = UserSessions.CurrentMerchantApp.BusinessZip;
                            objZBP.ReqVendorNumber = "BIS045";
                            objZBP.ReqCustomerName = "TESTING";

                            for (int i = 0; i < UserSessions.CurrentMerchantApp.Owners.Count; i++)
                            {
                                if (UserSessions.CurrentMerchantApp.Owners[i].OwnerID.ToUpper() == btnSave.CommandArgument.ToUpper())
                                {

                                    objZBP.ReqOwnerFirstName = UserSessions.CurrentMerchantApp.Owners[i].FirstName;
                                    objZBP.ReqOwnerLastName = UserSessions.CurrentMerchantApp.Owners[i].LastName;
                                    objZBP.ReqOwnerSSN = UserSessions.CurrentMerchantApp.Owners[i].SSN.Replace("-", "").Trim();
                                    objZBP.ReqOwnerAddressStreet = UserSessions.CurrentMerchantApp.Owners[i].Address1;
                                    objZBP.ReqOwnerAddressCity = UserSessions.CurrentMerchantApp.Owners[i].City;
                                    objZBP.ReqOwnerAddressState = UserSessions.CurrentMerchantApp.Owners[i].State;
                                    objZBP.ReqOwnerAddressZip = UserSessions.CurrentMerchantApp.Owners[i].Zip;
                                    m_OwnerShipUID = btnSave.CommandArgument.ToUpper();
                                }
                            }

                            objZBP.ResCreditReportDate = DateTime.Now;
                            objZBP.ResCreditScore = CommonUtility.Util.if_i(m_CreditScore, 0);

                            if (!string.IsNullOrWhiteSpace(m_OldestTrade))
                                objZBP.ResLastTradeDate = DataLayer.Field2Date(m_OldestTrade);

                            objZBP.ResNumOfTrade = CommonUtility.Util.if_i(m_NoOfTrades, 0);
                            objZBP.ResRealEstateLoanBalance = CommonUtility.Util.if_dec(realestatebal, 0.00M);
                            objZBP.IsCurrent = false;

                            DataNetConnect objNetCon = new DataNetConnect();
                            BusinessProfileID.Value = objNetCon.InsertBusinessProfile(objZBP);

                            Hashtable prms = new Hashtable();
                            if (!string.IsNullOrWhiteSpace(m_CreditScore) && m_CreditScore.Trim() != "0")
                            {
                                prms.Add("@OwnershipUID", btnSave.CommandArgument);
                                prms.Add("@CreditScore", m_CreditScore);
                                prms.Add("@OldestTrade", m_OldestTrade);
                                prms.Add("@Notes", string.Empty);
                                prms.Add("@NoofTrades", m_NoOfTrades);
                                prms.Add("@UserCreated", "System");
                                prms.Add("@CreditTypeID", Owner.CreditType.Experian);

                                DataAccess.DataUnderwritingDao.InsertCreditHistory(prms);

                                AddPDF();
                            }


                            Logging.ExperianLog.InfoFormat("[ZID={0}] Owner [UID={1}] '{2}, {3}' Business credit profile saved.", UserSessions.CurrentMerchantApp.ID, btnSave.CommandArgument, objZBP.ReqOwnerLastName, objZBP.ReqOwnerFirstName);

                            #endregion

                            btnView.Visible = true;
                            btnClear.Visible = true;
                            btnView.Enabled = true;

                            if (objZBP.ResRealEstateLoanBalance > 0)
                                ListHandler.ListFindItem(NameAddressSSNSummary, "1");

                            WucMessage1.AddMessageSuccess("Owner Credit report is generated successfully.");

                            Logging.ExperianLog.InfoFormat("[ZID={0}] Owner Credit report is generated successfully.", UserSessions.CurrentMerchantApp.ID);
                        }
                        else
                        {
                            Logging.ExperianLog.ErrorFormat("[ZID={0}] Cannot save Owner credit report, errors were reported on the credit pull.", UserSessions.CurrentMerchantApp.ID);
                        }
                    }
                    else
                    {
                        WucMessage1.AddMessageError(resultingMessage.ErrorMessage);
                        Logging.ExperianLog.ErrorFormat("[ZID={0}] Errors occurred while pull Owner Credit Profile: {1}", UserSessions.CurrentMerchantApp.ID, resultingMessage.ErrorMessage);
                    }
                }
            }

            LoadCreditHistory();
        }
        catch (Exception ex)
        {
            Logging.ExperianLog.ErrorFormat("[ZID={0}] An Exception [Type={1}] has occurred while pull Experian credit report: {2}", UserSessions.CurrentMerchantApp.ID, ex.GetType().Name, ex.Message);
            Logging.ExperianLog.ErrorFormat("[ZID={0}] Stack Trace: {1}", UserSessions.CurrentMerchantApp.ID, ex.StackTrace);

            WucMessage1.AddMessageError("System Error. No Record Found. Contact Experian.");
        }
    }

    private BusinessProfileType FillBOPRequest()
    {
        BusinessProfileType bpt = new BusinessProfileType();

        bpt.Subscriber = new BIS_SubscriberType();
        bpt.Subscriber.OpInitials = "MG";
        bpt.Subscriber.SubCode = ConfigurationManager.AppSettings["SubCode"].ToString();// "688500";

        bpt.BusinessApplicant = new BusinessApplicantType();
        bpt.BusinessApplicant.BusinessName = UserSessions.CurrentMerchantApp.BusinessDBAName;
        bpt.BusinessApplicant.CurrentAddress = new BusinessApplicant_AddressType();
        bpt.BusinessApplicant.CurrentAddress.Street = UserSessions.CurrentMerchantApp.BusinessAddress;
        bpt.BusinessApplicant.CurrentAddress.City = UserSessions.CurrentMerchantApp.BusinessCity;
        bpt.BusinessApplicant.CurrentAddress.State = UserSessions.CurrentMerchantApp.BusinessState;
        bpt.BusinessApplicant.CurrentAddress.Zip = UserSessions.CurrentMerchantApp.BusinessZip;

        Logging.ExperianLog.InfoFormat("[ZID={0}] Retrieving owner for '{1}'.....", UserSessions.CurrentMerchantApp.ID, UserSessions.CurrentMerchantApp.BusinessDBAName);

        Owner uwOwner = null;
        if (btnSave.CommandArgument != string.Empty)
        {
            Logging.ExperianLog.InfoFormat("[ZID={0}] Populating owner [UID={1}].....", UserSessions.CurrentMerchantApp.ID, btnSave.CommandArgument);

            uwOwner = DataAccess.DataMerchantAppDao.GetOwner(btnSave.CommandArgument);

            if (uwOwner.LastName != string.Empty && uwOwner.FirstName != string.Empty)
            {

                Logging.ExperianLog.InfoFormat("[ZID={0}] Pulling Experian credit report for owner [UID={1}]: {2}, {3}", UserSessions.CurrentMerchantApp.ID, btnSave.CommandArgument, uwOwner.LastName, uwOwner.FirstName);

                bpt.BusinessOwner = new BusinessOwnerType();
                bpt.BusinessOwner.OwnerName = new BIS_NameType();
                bpt.BusinessOwner.OwnerName.Surname = uwOwner.LastName;
                bpt.BusinessOwner.OwnerName.First = uwOwner.FirstName;
                bpt.BusinessOwner.SSN = uwOwner.SSN.Replace("-", "");
                bpt.BusinessOwner.CurrentAddress = new AddressType();
                bpt.BusinessOwner.CurrentAddress.Street = uwOwner.Address1;
                bpt.BusinessOwner.CurrentAddress.City = uwOwner.City;
                bpt.BusinessOwner.CurrentAddress.State = uwOwner.State;
                bpt.BusinessOwner.CurrentAddress.Zip = uwOwner.Zip;
            }
        }

        bpt.AddOns = new BusinessProfile_AddOnsType();
        bpt.AddOns.StandAlone = new ChoiceType();
        bpt.AddOns.StandAlone = ChoiceType.Y;
        bpt.AddOns.StandAloneSpecified = true;

        bpt.OutputType = new BusinessProfile_OutputType();
        bpt.OutputType.ItemElementName = new ItemChoiceType3();
        bpt.OutputType.ItemElementName = ItemChoiceType3.XML;

        BIS_XMLType xmlt = new BIS_XMLType();
        xmlt.Verbose = new ChoiceType();
        xmlt.Verbose = ChoiceType.Y;

        bpt.OutputType.Item = xmlt;

        bpt.Vendor = new Business_VendorType();
        bpt.Vendor.VendorNumber = "BIS045";

        bpt.Options = new BISOptions3_OptionsType();
        bpt.Options.CustomerName = "TESTING";

        return bpt;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        DataNetConnect objNC = new DataNetConnect();

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@BusinessProfileID", BusinessProfileID.Value);
        string xmlResponse = objNC.GetBusinessProfileXmlResponse(prms);

        if (!string.IsNullOrWhiteSpace(xmlResponse))
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(HttpContext.Current.Server.MapPath("~\\Styles\\ownerprofile\\BOP.xslt"));

            XPathDocument mydata = new XPathDocument(new XmlTextReader(xmlResponse, XmlNodeType.Document, null));

            StringWriter sw = new StringWriter();

            xslt.Transform(mydata, new XsltArgumentList(), sw);

            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

            //Try adding source strings for each image in content
            string tempPostContent = getcssImages(sw.ToString());

            TextReader reader1 = new StringReader(tempPostContent);

            Winnovative.PdfConverter pdfConverter = GetPdfConverter();
            MemoryStream memstr = new MemoryStream();

            pdfConverter.SavePdfFromHtmlStringToStream(tempPostContent, memstr);

            byte[] byteArr = memstr.ToArray();

            string filename = "CreditReport_" + lblFullName.Text +".pdf";

            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader(
                                    "Content-Disposition",
            "attachment; filename=" + filename + "; size="
                            + byteArr.Length.ToString());

            response.Flush();
            response.BinaryWrite(byteArr);
            response.End();

        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(btnSave.CommandArgument))
        {
            Owner owner1 = DataAccess.DataMerchantAppDao.GetOwner(btnSave.CommandArgument);
            //CreditScore.Text = owner1.CreditScore;
            //CreditDate.Value = owner1.CreditDate;
            //TradeStatus.Text = owner1.TradeStatus;
            //OldestTrade.Value = owner1.OldestTrade;
            //NoofTrades.Text = owner1.NoofTrades.ToString();              
        }
        else
            FillBusinessProfileReponse();

        btnClear.Visible = false;
    }

    protected void AddPDF()
    {
        string tempPostContent = string.Empty;

        try
        {

            DataNetConnect objNC = new DataNetConnect();
            ZeusBusinessProfile objPP = new ZeusBusinessProfile();
            Hashtable prms = new Hashtable();

            prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
            prms.Add("@BusinessProfileID", CommonUtility.Util.if_i(BusinessProfileID.Value,0));

            objPP = objNC.GetBusinessProfile(prms);

            if (objPP != null)
            {
                string xmlResponse = objPP.ResponseXML;//objNC.GetBusinessProfileXmlResponse(prms);


                if (!string.IsNullOrWhiteSpace(xmlResponse) && objPP.DocID <= 0)
                {
                    XslCompiledTransform xslt = new XslCompiledTransform();
                    xslt.Load(HttpContext.Current.Server.MapPath("~\\Styles\\ownerprofile\\BOP.xslt"));

                    XPathDocument mydata = new XPathDocument(new XmlTextReader(xmlResponse, XmlNodeType.Document, null));

                    StringWriter sw = new StringWriter();

                    xslt.Transform(mydata, new XsltArgumentList(), sw);

                    System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(sw);

                    //Try adding source strings for each image in content
                    tempPostContent = getcssImages(sw.ToString());

                    TextReader reader1 = new StringReader(tempPostContent);

                    Winnovative.PdfConverter pdfConverter = GetPdfConverter();

                    MemoryStream memstr = new MemoryStream();

                    pdfConverter.SavePdfFromHtmlStringToStream(tempPostContent, memstr);

                    byte[] byteArr = memstr.ToArray();

                    ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
                    objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

                    ZeusWeb.MDocWS.UploadResponse objR = objFU.UploadFileWithSourceAndUser(
                            byteArr
                            , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                            , UserSessions.CurrentMerchantApp.MerchantAppUID
                            , 0
                            , ""
                            , (int)MDoc.eMDocType.CreditReport
                            , "OwnerCreditReport_" + lblFullName.Text + ".pdf"
                            , "Zeus"
                            , 0
                            , ""
                            , ""
                            , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                            , (int)MDoc.eMDocSourceID.Merchant
                            , UserSessions.CurrentUser.UserName
                        );

                    if (objR.DocID > 0)
                    {
                        objPP.DocID = objR.DocID;
                        objPP.MerchantID = CommonUtility.Util.if_i(UserSessions.CurrentMerchantApp.ID, 0);
                        objPP.BusinessProfileID = CommonUtility.Util.if_i(BusinessProfileID.Value, 0);

                        objNC.UpdateBusinessProfile(objPP);

                        WucMessage1.AddMessageSuccess("Personal Credit Report PDF Generated.");
                    }
                    else
                    {
                        WucMessage1.AddMessageError(objR.StatusMessage);
                    }

                    btnAdd.Visible = false;
                    btnClear.Visible = false;
                }
            }
        }

        catch (Exception ex)
        {
            //Display parser errors in PDF. 
            WucMessage1.AddMessageError("PDF Error.");
        }

    }

    public void FillBusinessProfileReponse()
    {
        DataNetConnect objNC = new DataNetConnect();

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);
        prms.Add("@IsCurrent", true);
        prms.Add("@ReqOwnerSSN", SSN.Text.Replace("-", "").Trim());

        ZeusBusinessProfile objPP = new ZeusBusinessProfile();
        objPP = objNC.GetBusinessProfile(prms);

        if (objPP != null)
        {
            //CreditScore.Text = objPP.ResCreditScore.ToString();
            //NoofTrades.Text = objPP.ResNumOfTrade.ToString();
            if(objPP.ResLastTradeDate != null)
                // OldestTrade.Value = objPP.ResLastTradeDate.Value;
                if (objPP.ResCreditReportDate != null)
                    //CreditDate.Value = objPP.ResCreditReportDate.Value;
                    btnView.Visible = true;
            btnAdd.Visible = (objPP.DocID <= 0) && objPP.IsCurrent;
            btnClear.Visible = false;
            BusinessProfileID.Value = objPP.BusinessProfileID.ToString();
            if(objPP.LastCreditDate != null)
                CreditReportDate.Value = objPP.LastCreditDate.ToShortDateString();
            if (objPP.ResRealEstateLoanBalance > 0)
                ListHandler.ListFindItem(NameAddressSSNSummary, "1");
            btnView.Enabled = true;
        }
        else
        {
            //CreditScore.Text = string.Empty;
            //NoofTrades.Text = string.Empty;
            //OldestTrade.Value = null;
            //CreditDate.Value = null;
            btnView.Visible = false;
            btnAdd.Visible = false;
            btnClear.Visible = false;
            BusinessProfileID.Value = "";
            //CreditReportDate.Value = "";            
            //ListHandler.ListFindItem(NameAddressSSNSummary, "-1");
        }

    }

    private Winnovative.PdfConverter GetPdfConverter()
    {
        Winnovative.PdfConverter pdfConverter = new Winnovative.PdfConverter();
        pdfConverter.LicenseKey = "2FZFV0ZXQUZDQ1dGQllHV0RGWUZFWU5OTk5XRw==";// "/NfN3M7P3MTF3MrSzNzPzdLNztLFxcXF";

        //pdfConverter.LicenseKey = "put your license key here";

        // set the HTML page width in pixels
        // the default value is 1024 pixels
        pdfConverter.HtmlViewerWidth = 1024; // autodetect the HTML page width

        //set the PDF page size 
        pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
        // set the PDF compression level
        pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
        // set the PDF page orientation (portrait or landscape)
        pdfConverter.PdfDocumentOptions.PdfPageOrientation = PdfPageOrientation.Portrait;
        //set the PDF standard used to generate the PDF document
        pdfConverter.PdfDocumentOptions.PdfStandardSubset = PdfStandardSubset.Full;
        // show or hide header and footer
        pdfConverter.PdfDocumentOptions.ShowHeader = false;
        pdfConverter.PdfDocumentOptions.ShowFooter = false;
        //set the PDF document margins
        pdfConverter.PdfDocumentOptions.LeftMargin = 0;
        pdfConverter.PdfDocumentOptions.RightMargin = 0;
        pdfConverter.PdfDocumentOptions.TopMargin = 0;
        pdfConverter.PdfDocumentOptions.BottomMargin = 0;
        // set if the HTTP links are enabled in the generated PDF
        pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
        // set if the HTML content is resized if necessary to fit the PDF page width - default is true
        pdfConverter.PdfDocumentOptions.FitWidth = true;
        // set if the PDF page should be automatically resized to the size of the HTML content when FitWidth is false
        pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
        // embed the true type fonts in the generated PDF document
        pdfConverter.PdfDocumentOptions.EmbedFonts = false;
        // compress the images in PDF with JPEG to reduce the PDF document size - default is true
        pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
        // set if the JavaScript is enabled during conversion 
        pdfConverter.JavaScriptEnabled = false;

        // set if the converter should try to avoid breaking the images between PDF pages
        pdfConverter.PdfDocumentOptions.AvoidImageBreak = false;
        
        pdfConverter.PdfBookmarkOptions.HtmlElementSelectors = false ? new string[] { "h1", "h2" } : null;

        return pdfConverter;

    }

    public string getcssImages(string input)
    {
        if (input == null)
            return string.Empty;
        string tempInput = input;
        string pattern = @"../images";
        string src = string.Empty;
        HttpContext context = HttpContext.Current;
        string ImagePath = WebConfigurationManager.AppSettings["Experian.ZeusURL"];
        string IsNewDataCenter = WebConfigurationManager.AppSettings["IsNewDataCenter"];
        //Change the relative URL's to absolute URL's for an image, if any in the HTML code.
        foreach (Match m in Regex.Matches(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.RightToLeft))
        {
            if (m.Success)
            {
                string tempM = m.Value;
                src = m.Value.Replace("\"", "");

                if (src.ToLower().Contains("http://") == false || src.ToLower().Contains("https://") == false)
                {
                    //Insert new URL in img tag
                    string path;

                    if (IsNewDataCenter.Equals("TRUE"))
                    {
                        path = new Uri(ImagePath).GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath;
                    }

                    //Works in Irvine this way
                    else
                    {
                        path = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpRuntime.AppDomainAppVirtualPath;
                    }

                    src = path + src.Replace("..", "");
                    src = src.Replace("//images","/images");
                    try
                    {
                        //insert new url img tag in whole html code
                        tempInput = tempInput.Remove(m.Index, m.Length);
                        tempInput = tempInput.Insert(m.Index, src);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        return tempInput;
    }

    public void SetReadOnly()
    {
        lblFullName.ReadOnly = true;
        PercentOwnership.ReadOnly = true;
        SSN.ReadOnly = true;
    }

    public void FormSave()
    {
        Hashtable prms = new Hashtable();

        //if (!string.IsNullOrWhiteSpace(CreditScore.Text) && CreditScore.Text.Trim() != "0")
        //{
        //    prms.Add("@OwnershipUID", btnSave.CommandArgument);
        //    prms.Add("@CreditScore", CreditScore.Text);
        //    prms.Add("@OldestTrade", OldestTrade.Text);
        //    prms.Add("@Notes", TradeStatus.Text);
        //    prms.Add("@NoofTrades", NoofTrades.Text);
        //    prms.Add("@UserCreated", "System");
        //    prms.Add("@CreditTypeID", Owner.CreditType.Experian);

        //    DataAccess.DataUnderwritingDao.InsertCreditHistory(prms);

        //    AddPDF();
        //}
    }

    protected string GetSubmitPostBack()
    {
        return Page.ClientScript.GetPostBackEventReference(btnOk, string.Empty);
    }

    protected void grdCreditHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;

            case DataControlRowType.DataRow:

                e.Row.Cells[0].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[0].Text);
                e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text);

                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    private string MaskSSN(string str, string ssn)
    {
        if (!string.IsNullOrWhiteSpace(str) && !string.IsNullOrWhiteSpace(ssn))
        {
            //mask SSN in the logs
            str = str.Replace(ssn, "***-**-****");
        }

        return str;
    }


    public void SetEditmode(bool isTrue)
    {
        btnSave.Visible = isTrue;
        btnCancel.Visible = isTrue;

        //PXP-2891 and PXP-3933 Rohit Thakur
        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING))
        {
            if (UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled.Equals(true))
            {
                panelBeneficialOwner.Enabled = true;
                panelAuthorizedSignature.Enabled = true;
            }

            else
            {
                panelBeneficialOwner.Enabled = false;
                panelAuthorizedSignature.Enabled = false;
            }
        }
    }
}
