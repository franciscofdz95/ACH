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
using System.IO;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;

using Infragistics.WebUI.WebDataInput;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using PaymentXP.Facade;
using iTextSharp.text.pdf;


using Aspose.BarCode;
using Aspose.Pdf;
using Aspose.Pdf.Kit;

using Winnovative;


public partial class frmMerchantDocuments : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentMerchantApp != null)
            base.UID = UserSessions.CurrentMerchantApp.MerchantAppUID;

    }

    public bool IsPrivateEnable
    {
        get { return (bool)(ViewState["IsPrivateEnable"] ?? false); }
        set { ViewState["IsPrivateEnable"] = value; }
    }

    public bool VsIsRoleAdmin
    {
        get { return (bool)(ViewState["VsIsRoleAdmin"] ?? false); }
        set { ViewState["VsIsRoleAdmin"] = value; }
    }

    public bool VsIsRoleCreditUnderWriting
    {
        get { return (bool)(ViewState["VsIsRoleCreditUnderWriting"] ?? false); }
        set { ViewState["VsIsRoleCreditUnderWriting"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        if (!this.IsPostBack)
        {
            this.VsIsRoleAdmin = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_ADMIN)
                    && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_ADMIN].Enabled == true);

            this.VsIsRoleCreditUnderWriting = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING)
                    && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled == true);
            
            this.Master.SideMenuSelect(MasterPageMerchant.eMasterSideMenu.Documents);

            if (UserSessions.CurrentMerchantApp != null)
            {
                this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Documents");
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

            this.SortOrder = "DOCDATE";
            this.SortDirectionSearch = SortDirection.Descending;

            this.FormShow("");
        }

        if (UserSessions.CurrentUser.IsBank)
        {
            pnlUpload.Visible = false;
            pnlBarcode.Visible = false;
        }

    }

    public void SetDataSource(Hashtable prms, int pagesize)
    {
        grdDocuments.DataSourceID = "ods";

        this.CurrentPage = 1;
        this.PageSize = pagesize;

        grdDocuments.PageIndex = 0;
        grdDocuments.PageSize = pagesize;

        this.m_Prms = prms;

        BindGrid();
    }

    private void BindGrid()
    {
        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", this.SortOrder);
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

        if (UserSessions.CurrentUser.IsBank)
            m_Prms["@IsBank"] = 1;

        litRecordCount.Text = DataMerchantAppPaging.GetDocumentPagingCount(m_Prms, 0, 0).ToString();
        grdDocuments.DataBind();
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.BindGrid();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        long file_size_max = Convert.ToInt64(ConfigurationManager.AppSettings["MAX_FILE_UPLOAD_BYTES"]);

        bool is_upload_success = false;

        try
        {
            if (lstDocumentTypes.SelectedIndex < 1)
            {
                WucMessage1.AddMessageError("Please select a document type");
                return;
            }
        }
        catch (Exception ex)
        {
            WucMessage1.AddMessageError("File Error: " + ex.Message);
        }

        if (is_upload_success)
        {
            Response.Redirect("~/SecureMerchantManagementForms/frmMerchantDocuments.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
        }
    }

    public bool DocUploadUpdateUserID(int DocumentID, string UserName)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@DocumentID", DocumentID);
        prms.Add("@UserName", UserName);

        return DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);
    }

    private void CreatePDF(object obj, string PDF, string filename)
    {
        PdfReader pdfReader = new PdfReader(Server.MapPath(string.Concat(ConfigurationManager.AppSettings["DOCINPUT"].ToString(), PDF)));
        using (FileStream stream = new FileStream(Server.MapPath(string.Concat(ConfigurationManager.AppSettings["DOCOUTPUT"].ToString(), string.Concat(filename, ".pdf"))), FileMode.Create))
        {
            PdfStamper pdfStamper = new PdfStamper(pdfReader, stream);
            AcroFields fields = pdfStamper.AcroFields;

            Type objType = obj.GetType();
            PropertyInfo[] objPropertiesArray = objType.GetProperties();

            foreach (PropertyInfo prop in objPropertiesArray)
            {
                if (prop.GetValue(obj, null) != null)
                {
                    string propertyValue = prop.GetValue(obj, null).ToString();

                    fields.SetField(prop.Name, propertyValue);
                }
            }

            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
        }
    }

    protected void lstDocGroupTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocuments();
    }

    protected void lstDocumentSources_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocuments();
    }

    private void LoadDocuments()
    {
        Hashtable prms = new Hashtable();

        prms.Add("@MerchantID", UserSessions.CurrentMerchantApp.ID);

        if (lstDocGroupTypes.SelectedValue != "-1")
        {
            prms.Add("@DocTypeGroupID", DataLayer.Field2IntSafe(lstDocGroupTypes.SelectedValue));
        }
        else
        {
            string grouptypes = string.Empty;

            for (int i = 1; i < lstDocGroupTypes.Items.Count; i++)
                grouptypes += lstDocGroupTypes.Items[i].Value + ",";

            prms.Add("@DocTypeGroupIDList", grouptypes);
        }

        prms.Add("@UserUID", UserSessions.CurrentUser.UID);

        this.SetDataSource(prms, Convert.ToInt32(ddlPageSize.SelectedValue));

    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = m_Prms;
    }

    public override void FormShow(string ID)
    {
        Aspose.Pdf.Kit.License licPdfKit = new Aspose.Pdf.Kit.License();
        licPdfKit.SetLicense(@"Aspose.Total.lic");

        Aspose.Pdf.License licPdf = new Aspose.Pdf.License();
        licPdf.SetLicense(@"Aspose.Total.lic");

        Aspose.BarCode.License lic = new Aspose.BarCode.License();
        lic.SetLicense(@"Aspose.Total.lic");

        LookupTableHandler.LoadMDocType(lstDocumentTypes2, true, MDoc.eMDocTypeGroup.Merchant);
        LookupTableHandler.LoadMDocType(lstDocumentTypes, false, MDoc.eMDocTypeGroup.Merchant);

        LookupTableHandler.LoadMDocGroupTypes(lstDocGroupTypes, true);


        //remove Sales Partner. business rule: we want to hide this from the merchant document view page
        //remove Lead. business rule, we want to hide this from the merchant document review page

        List<ListItem> liRem = new List<ListItem>();

        foreach (ListItem item in lstDocGroupTypes.Items)
        {
            if (item.Value == Convert.ToString((int)MDoc.eMDocTypeGroup.SalesPartner)
                || item.Value == Convert.ToString((int)MDoc.eMDocTypeGroup.Chargeback) || item.Value == Convert.ToString((int)MDoc.eMDocTypeGroup.CBeResponse)
               )
            {
                liRem.Add(item);
            }
        }

        // remove the list
        if (liRem != null && liRem.Count > 0)
        {
            foreach (ListItem obj in liRem)
            {
                lstDocGroupTypes.Items.Remove(obj);
            }
        }


        User user = UserSessions.CurrentUser;

        UserForm frm = null;

        if (user.UserForms.TryGetValue("FRMMERCHANTDOCUMENTS", out frm) && frm.HasAccess)
        {
            if (frm.ControlObjects == null)
                DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

            foreach (ControlObject obj in frm.ControlObjects)
            {
                IsPrivateEnable = (obj.Type.ToUpper() == "CHECKBOX" && obj.ID.ToUpper() == "PRIVATEDOCUMENT" && obj.IsVisible && obj.IsEnabled);
            }
        }

        this.LoadDocuments();

        MerchantFacade facade = new MerchantFacade();
        MerchantApp agreement = facade.GetMerchantAppZeus(UserSessions.CurrentMerchantApp.MerchantAppUID);

        FormBinding.BindObjectToControls(agreement, pnlDetail);

        //check to see if the account is ACH only and get the ach status in case if it is or else the cc status
        string m_StatusUID = agreement.StatusUID.ToUpper();

        if (WucBusinessInfo1.isACHonly && UserSessions.ActiveAchMerchant != null)
        {
            m_StatusUID = UserSessions.ActiveAchMerchant.MerchantStatusUID.ToUpper();
        }

        FormHandler.ShowClosureCodes(WucBusinessInfo1, m_StatusUID);

        WucBusinessInfo1.pnlInfo.Enabled = false;

        bool has_access = false;

        UserRole role = null;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_CREDIT_UNDERWRITING, out role))
            has_access = role.Enabled;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_IT, out role))
            has_access = role.Enabled;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_ADMIN, out role))
            has_access = role.Enabled;

        btnPrintCUWebites.Visible = has_access;
        
       
        WucBusinessInfo1.LoadOffice(agreement);
        
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
        return true;
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    protected void grdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                var row = ((DataRowView)e.Row.DataItem).Row;

                MDoc objM = DataMerchantAppPaging.getMDoc(row);

                LinkButton lnkconditiondoc = ((LinkButton)e.Row.FindControl("lnkDelete"));

                int primaryKeyID = 0;
                if (DataBinder.Eval(e.Row.DataItem, "PrimaryKeyID") != null)
                    primaryKeyID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "PrimaryKeyID"));
                    
                if (primaryKeyID != 0)
                    lnkconditiondoc.OnClientClick = "return confirm('Are you sure you want to delete document linked to condition?')";
                else
                    lnkconditiondoc.OnClientClick = "return confirm('Are you sure you want to delete this document?');";

                CheckBox chkPrivate = ((CheckBox)e.Row.FindControl("IsPrivate"));
                chkPrivate.Checked = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsPrivate"), false);
                chkPrivate.Enabled = IsPrivateEnable;

                bool isAccessible = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsAccessible"), false);
                // code changes for PXP-6927 by koshlendra start
                string file_ext = objM.OrigName.ToUpper().Substring(objM.OrigName.ToUpper().LastIndexOf('.') + 1).ToUpper();
                System.Web.UI.WebControls.Image img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
                img.ImageUrl = (WebUtil.GetDocIconUrl(file_ext));
                // code changes for PXP-6927 by koshlendra end
                HyperLink lnk = (HyperLink)e.Row.FindControl("hypOrigName");
                Label lbl = (Label)e.Row.FindControl("lblOrigName");

                Dictionary<string, string> di = new Dictionary<string, string>();

                di["DocID"] = DataBinder.Eval(e.Row.DataItem, "DocID").ToString();
                di["MerchantAppUID"] = UserSessions.CurrentMerchantApp.MerchantAppUID;
                di["MerchantID"] = UserSessions.CurrentMerchantApp.ID;

                lbl.Visible = false;
                lnk.Visible = true;
                lnk.NavigateUrl = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di)));

                if (chkPrivate.Checked && !isAccessible)
                {                    
                    lbl.Visible = true;
                    lnk.Visible = false;
                }

                if(chkPrivate.Checked)
                    lnk.ToolTip = "Private Document";

                // strips off the directory and just puts the filename.
                string[] arr = lnk.Text.Split(new char[] { '\\' });
                lnk.Text = arr[arr.Length - 1];

                if (grdDocuments.EditIndex != e.Row.RowIndex || grdDocuments.EditIndex == -1)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Visible = false;
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Visible = true;
                }
                else
                {   
                    //PXP-3171
                    bool IsDeleteVisibForSalesSupp = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SALES_SUPPORT)
                    && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SALES_SUPPORT].Enabled == true);
                    if (UserSessions.CurrentMerchantApp != null)
                    {
                        string merchantBank = UserSessions.CurrentMerchantApp.MerchantAppTypeUID.Replace("-1", string.Empty).ToUpper();
                        if (merchantBank.Equals(Constants.BANK_ACH_ONLY) && UserSessions.CurrentMerchantApp.AchID > 0)
                        {
                            IsDeleteVisibForSalesSupp = (IsDeleteVisibForSalesSupp && UserSessions.CurrentMerchantApp.ACHStatus.ToUpper().StartsWith("SS")) ? true : false;
                        }
                        else
                        {
                            IsDeleteVisibForSalesSupp = (IsDeleteVisibForSalesSupp && UserSessions.CurrentMerchantApp.StatusName.ToUpper().StartsWith("SS")) ? true : false;
                        }
                    }

                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Visible = true;
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Visible = (this.VsIsRoleAdmin || this.VsIsRoleCreditUnderWriting || IsDeleteVisibForSalesSupp);
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Visible = false;
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Visible = false;
                    // Niranjan:- PXP-4852 Restrict SS group to delete documents when Applications moved back from CU queue to SS queue.
                    if (((LinkButton)e.Row.FindControl("lnkDelete")).Visible)
                    {
                        bool DeleteDocEnable = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "DeleteDocEnable"));
                        if (!DeleteDocEnable)
                        {
                            LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                            lnkDelete.Enabled = false;
                            lnkDelete.OnClientClick = null;
                            lnkDelete.Style.Add("opacity", "0.5");
                        }
                    }

                    DropDownList ddp = ((DropDownList)e.Row.Cells[4].FindControl("ddpType"));
                    LookupTableHandler.LoadMDocType(ddp, false, (MDoc.eMDocTypeGroup)objM.DocTypeGroupID);
                    ddp.Items.RemoveAt(0);
                    ListHandler.ListFindItem(ddp, DataBinder.Eval(e.Row.DataItem, "DocTypeID").ToString());
                }

                if (UserSessions.CurrentUser.IsBank)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Visible = false;
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Visible = false;
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Visible = false;
                }

                string contentSize = DataBinder.Eval(e.Row.DataItem, "ContentSize").ToString();
                decimal size;

                if (decimal.TryParse(contentSize, out size))
                {
                    decimal percent = size / 1024;

                    e.Row.Cells[8].Text = percent.ToString("N") + "KB";
                }

                e.Row.Cells[9].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[9].Text);

                break;

            default:
                break;
        }
    }

    protected void grdDocuments_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        MDoc obj = DataAccess.DataDocumentsDao.GetMDocument(Convert.ToInt32(grdDocuments.DataKeys[e.RowIndex].Values["DocID"].ToString()));

        if (obj != null)
        {
            DropDownList ddp = (DropDownList)grdDocuments.Rows[e.RowIndex].Cells[4].FindControl("ddpType");
            obj.DocTypeID = DataLayer.Field2Int(ddp.SelectedValue);
            obj.Description = ((TextBox)grdDocuments.Rows[e.RowIndex].Cells[5].FindControl("txtDescription")).Text;
            obj.UserCreated = UserSessions.CurrentUser.UserName;
            DataAccess.DataDocumentsDao.UpdateMDocument(obj);

            grdDocuments.EditIndex = -1;
            LoadDocuments();
        }

        e.Cancel = true;
    }

    protected void grdDocuments_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdDocuments.EditIndex = e.NewEditIndex + (grdDocuments.PageIndex * grdDocuments.PageSize);
    }

    protected void grdDocuments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdDocuments.EditIndex = -1;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintDocumentBarcode();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        PrintDocumentBarcode();
    }

    protected void btnPreviewZID_Click(object sender, EventArgs e)
    {
        PrintZIDBarcode();
    }

    private void PrintZIDBarcode()
    {
        bool Print = false;
        Aspose.Pdf.Pdf pdf1 = pdf1 = new Aspose.Pdf.Pdf();
        MemoryStream imgStream = null;

        // Generate the barcode image
        BarCodeBuilder builder = new BarCodeBuilder();
        builder.SymbologyType = Symbology.Code39Standard;

        builder.CodeText = UserSessions.CurrentMerchantApp.ID;


        builder.xDimension = 0.1f;
        builder.yDimension = 0.1f;

        // save the barcode image in memory stream
        imgStream = new MemoryStream();
        builder.Save(imgStream, ImageFormat.Jpeg);
        imgStream.Position = 0;


        // Create the pdf file and add the barcode image to it
        // add the new section for image
        Aspose.Pdf.Section secImage = pdf1.Sections.Add();
        Aspose.Pdf.Image image1 = new Aspose.Pdf.Image(secImage);
        image1.ImageInfo.ImageFileType = ImageFileType.Jpeg;
        image1.ImageInfo.OpenType = ImageOpenType.Memory;
        image1.ImageScale = 2.5F;
        System.IO.BinaryReader reader = new System.IO.BinaryReader(imgStream);
        imgStream.Position = 0;
        image1.ImageInfo.ImageStream = imgStream;
        secImage.Paragraphs.Add(image1);

        string s = string.Empty;
        s = "\n\nPaysafe Solutions\n";
        s += "DBA: " + UserSessions.CurrentMerchantApp.BusinessDBAName + "\n";
        s += "ZID: " + UserSessions.CurrentMerchantApp.ID + "\n";
        s += "MID: " + UserSessions.CurrentMerchantApp.SettlePlatformMid + "\n";

        Text dba = new Text(s);

        secImage.Paragraphs.Add(dba);
        if (Print)
        {
            PdfViewer viewer = new PdfViewer();
            viewer.PrintPageDialog = false;
            viewer.AutoResize = true;         // print the file with adjusted size
            viewer.AutoRotate = true;         // print the file with adjusted rotation		

            MemoryStream[] ms = new MemoryStream[1];
            pdf1.Save(ms[0]);
            viewer.OpenPdfFile(ms[0]);
            //viewer.OpenPdfFile(@"c:/temp/test.pdf");
            viewer.PrintDocument();
            viewer.ClosePdfFile();
        }
        else
        {

            pdf1.Save("barcode.pdf", SaveType.OpenInAcrobat, base.Response);
            Response.End();
        }

        imgStream.Close();

    }

    private void PrintDocumentBarcode()
    {
        //this.PrintBarcode(lst.Text, lst.Value, false);
        bool Print = false;
        Aspose.Pdf.Pdf pdf1 = pdf1 = new Aspose.Pdf.Pdf();
        MemoryStream imgStream = null;

        foreach (ListItem item in lstDocumentTypes2.Items)
        {
            if (item.Selected)
            {
                string DocumentType = item.Text;
                string DocumentID = item.Value;
                // Generate the barcode image
                BarCodeBuilder builder = new BarCodeBuilder();
                builder.SymbologyType = Symbology.Code39Standard;

                builder.CodeText = DocumentID;


                builder.xDimension = 0.1f;
                builder.yDimension = 0.1f;

                // save the barcode image in memory stream
                imgStream = new MemoryStream();
                builder.Save(imgStream, ImageFormat.Jpeg);
                imgStream.Position = 0;


                // Create the pdf file and add the barcode image to it
                // add the new section for image
                Aspose.Pdf.Section secImage = pdf1.Sections.Add();
                Aspose.Pdf.Image image1 = new Aspose.Pdf.Image(secImage);
                image1.ImageInfo.ImageFileType = ImageFileType.Jpeg;
                image1.ImageInfo.OpenType = ImageOpenType.Memory;
                image1.ImageScale = 2.5F;
                System.IO.BinaryReader reader = new System.IO.BinaryReader(imgStream);
                imgStream.Position = 0;
                image1.ImageInfo.ImageStream = imgStream;
                secImage.Paragraphs.Add(image1);

                string s = "\n\nDocument Type: " + DocumentType;


                Text dba = new Text(s);

                secImage.Paragraphs.Add(dba);
            }
        }

        if (Print)
        {
            PdfViewer viewer = new PdfViewer();
            viewer.PrintPageDialog = false;
            viewer.AutoResize = true;         // print the file with adjusted size
            viewer.AutoRotate = true;         // print the file with adjusted rotation		

            MemoryStream[] ms = new MemoryStream[1];
            pdf1.Save(ms[0]);
            viewer.OpenPdfFile(ms[0]);
            //viewer.OpenPdfFile(@"c:/temp/test.pdf");
            viewer.PrintDocument();
            viewer.ClosePdfFile();
        }
        else
        {

            pdf1.Save("barcode.pdf", SaveType.OpenInAcrobat, base.Response);
            Response.End();
        }

        imgStream.Close();

    }

    private Winnovative.HtmlToPdfConverter GetPdfConverter()
    {
        Winnovative.HtmlToPdfConverter pdfConverter = new Winnovative.HtmlToPdfConverter();
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

        // Set the maximum time in seconds to wait for HTML page to be loaded 
        // Leave it not set for a default 60 seconds maximum wait time
        pdfConverter.NavigationTimeout = 120;

        // set if the converter should try to avoid breaking the images between PDF pages
        pdfConverter.PdfDocumentOptions.AvoidImageBreak = false;


        pdfConverter.PdfBookmarkOptions.HtmlElementSelectors = false ? new string[] { "h1", "h2" } : null;

        return pdfConverter;
    }


    protected void btnPrintCUWebites_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
            return;

        /* Customer Service Phone */
        string urlToConvert = "https://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.BusinessDBAPhone;

        string downloadName = "Report";
        //byte[] downloadBytes = null;

        downloadName += ".pdf";
        Winnovative.HtmlToPdfConverter pdfConverter = GetPdfConverter();

        // call the converter and get a Document object from URL
        Document pdfDocument = pdfConverter.ConvertUrlToPdfDocumentObject(urlToConvert);

        // get the conversion summary object from the event arguments
        ConversionSummary conversionSummary = pdfConverter.ConversionSummary;

        // the PDF page where the previous conversion ended
        Winnovative.PdfPage lastPage = pdfDocument.Pages[conversionSummary.LastPageIndex];
        // the last rectangle in the last PDF page where the previous conversion ended
        RectangleF lastRectangle = conversionSummary.LastPageRectangle;

        // the result of adding an element to a PDF page
        // ofers the index of the PDF page where the rendering ended 
        // and the bounding rectangle of the rendered content in the last page
        AddElementResult addResult = null;

        // the converter for the second URL
        HtmlToPdfElement htmlToPdfURL2 = null;

        Winnovative.PdfPage newPage = pdfDocument.Pages.AddNewPage();

        try
        {
            /* Business Phone */
            if (UserSessions.CurrentMerchantApp.BusinessPhone != string.Empty)
            {
                urlToConvert = "https://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.BusinessPhone;
                htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
                addResult = newPage.AddElement(htmlToPdfURL2);
                lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
            }
        }
        catch { }


        try
        {
            /* home Phone */
            if (UserSessions.CurrentMerchantApp.Owners[0].HomePhone != string.Empty)
            {
                urlToConvert = "https://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.Owners[0].HomePhone;
                newPage = pdfDocument.Pages.AddNewPage();
                htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
                addResult = newPage.AddElement(htmlToPdfURL2);
                lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
            }
        }
        catch { }



        try
        {
            /* Domain */
            if (UserSessions.CurrentMerchantApp.BusinessWebsite != string.Empty)
            {
                urlToConvert = "https://centralops.net/co/DomainDossier.aspx?addr=" + UserSessions.CurrentMerchantApp.BusinessWebsite + "&dom_whois=true&dom_dns=true&net_whois=true";
                newPage = pdfDocument.Pages.AddNewPage();
                htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
                addResult = newPage.AddElement(htmlToPdfURL2);
                lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
            }
        }
        catch { }

        try
        {
            /* Ripoff report */
            if (UserSessions.CurrentMerchantApp.BusinessDBAName != string.Empty)
            {
                urlToConvert = "https://www.ripoffreport.com/Search/" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".aspx";
                newPage = pdfDocument.Pages.AddNewPage();
                htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
                addResult = newPage.AddElement(htmlToPdfURL2);
                lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
            }
        }
        catch { }


        try
        {
            /* Ripoff report */
            if (UserSessions.CurrentMerchantApp.BusinessDBAName != string.Empty)
            {
                urlToConvert = "https://www.complaintsboard.com/?search=" + UserSessions.CurrentMerchantApp.BusinessDBAName + "&everything=Everything";
                newPage = pdfDocument.Pages.AddNewPage();
                htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
                addResult = newPage.AddElement(htmlToPdfURL2);
                lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
            }
        }
        catch { }


        // add a HTML string after all the rendered content
        HtmlToPdfElement htmlStringToPdf = new HtmlToPdfElement(addResult.EndPageBounds.Left, addResult.EndPageBounds.Bottom,
            "<b><i>The rendered content ends here</i></b>", null);

        lastPage.AddElement(htmlStringToPdf);

        string origfname = string.Format("CU-Website-{0}.pdf", CommonUtility.Util.GetDateTimeStamp());
        string tempdir = ConfigurationManager.AppSettings["TempUploadDir"];

        if (!Directory.Exists(tempdir))
        {
            Directory.CreateDirectory(tempdir);
        }

        string filename = string.Format(@"{0}\{1}"
            , tempdir
            , origfname
            );


        pdfDocument.Save(filename);

        pdfDocument.Close();

        byte[] byteArr = CommonUtility.Util.FileToByteArray(filename);

        ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
        objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

        ZeusWeb.MDocWS.UploadResponse objR = objFU.UploadFileWithSourceAndUser(
                byteArr
                , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                , UserSessions.CurrentMerchantApp.MerchantAppUID
                , 0
                , ""
                , (int)MDoc.eMDocType.CU_Websites
                , origfname
                , "Zeus"
                , 0
                , ""
                , ""
                , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                , (int)MDoc.eMDocSourceID.Merchant
                , UserSessions.CurrentUser.UserName
            );

        pdfDocument = null;

        if (objR.DocID > 0)
        {
            WucMessage1.AddMessageStatus("CU-Website PDF Generated");
        }
        else
        {
            WucMessage1.AddMessageError(objR.StatusMessage);
        }

        LoadDocuments();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        this.LoadDocuments();
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.CurrentPage = 1;
        this.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

        this.LoadDocuments();
    }

    // delete a file
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();

        objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

        if (objFU != null)
        {
            int doc_id = Convert.ToInt32(lb.CommandArgument);

            if (objFU.DeleteFileWithUser(doc_id, Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), 0, 0, UserSessions.CurrentUser.UserName))
            {
                WucMessage1.AddMessageStatus("File has been deleted");
            }

            grdDocuments.EditIndex = -1;
            LoadDocuments();
        }
    }

    protected void IsPrivate_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkPrivate = (CheckBox)sender;        
        GridViewRow grdRow = (GridViewRow)chkPrivate.NamingContainer;

        if (!string.IsNullOrWhiteSpace(hdnDocuments.Value))
        {
            int DocID = CommonUtility.Util.if_i(grdDocuments.DataKeys[grdRow.RowIndex].Values[0].ToString(), 0);

            if (DocID > 0)
            {
                DataAccess.DataDocumentsDao.UpdatePrivateMDoc(DocID, chkPrivate.Checked);
                hdnDocuments.Value = string.Empty;
                LoadDocuments();
            }
        }

    }

    protected void grdDocuments_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        LoadDocuments();
    }
}
