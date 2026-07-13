using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class frmAgentDocuments : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = true;

        if (UserSessions.CurrentAgent != null)
            base.UID = UserSessions.CurrentAgent.AgentUID;
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
        ((HyperLink)this.Master.FindControl("lnkAgentDocuments")).CssClass = "active";

        if (!this.IsPostBack)
        {
            this.VsIsRoleAdmin = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_ADMIN)
                    && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_ADMIN].Enabled == true);

            this.VsIsRoleCreditUnderWriting = (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING)
                    && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING].Enabled == true);
            this.FormShow("");
        }

        if (UserSessions.CurrentUser.IsBank)
        {
            pnlUpload.Visible = false;
            //pnlBarcode.Visible = false;
        }
    }


    #region Helper Functions

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
            m_Prms.Add("@SortOrder", "BusinessDBAName");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

        litRecordCount.Text = DataMerchantAppPaging.GetDocumentPagingCount(m_Prms, 0, 0).ToString();
        grdDocuments.DataBind();
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

    private void LoadDocuments()
    {
        Hashtable prms = new Hashtable();
        prms.Add("@AgentID", UserSessions.CurrentAgent.AgentID);

        //even though this is agent document library, per Wilson, this is only for Accounting Documents
        prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.SalesPartner);

        this.SetDataSource(prms, Convert.ToInt32(ddlPageSize.SelectedValue));
    }
    #endregion

    #region Properties



    #endregion

    #region frmBaseDataEntry Implementation

    public override void FormShow(string ID)
    {
        Aspose.Pdf.Kit.License licPdfKit = new Aspose.Pdf.Kit.License();
        licPdfKit.SetLicense(@"Aspose.Total.lic");

        Aspose.Pdf.License licPdf = new Aspose.Pdf.License();
        licPdf.SetLicense(@"Aspose.Total.lic");

        Aspose.BarCode.License lic = new Aspose.BarCode.License();
        lic.SetLicense(@"Aspose.Total.lic");

        /* Removed: LookupTableHandler.LoadMDocType(lstDocumentTypes2, true, MDoc.eMDocTypeGroup.Accounting); */
        LookupTableHandler.LoadMDocType(lstDocumentTypes, false, MDoc.eMDocTypeGroup.SalesPartner);

        //This page should only have the Accounting item in the list
        lstDocumentSources.Items.Add(new ListItem(MDoc.eMDocSourceID.SalesPartner.ToString(), ((int)MDoc.eMDocSourceID.SalesPartner).ToString()));

        //DataAgent data = DataAccess.DataAgentDao;

        ////Let's do away from reading the agent uid from session since it causes bugs where
        ////the user can have two browser windows opan sharing the same session
        ////Agent agent = data.GetAgent(UserSessions.CurrentAgent.AgentUID);
        //Agent agent = data.GetAgent(ID);

        ////if an invalid agent UID is provided in the url query string then redirect the
        ////user to the search agents form
        //if (agent == null)
        //    Response.Redirect("~/SecureAgentManagementForms/frmSearchAgents.aspx");

        this.LoadDocuments();

        DataAgent data = DataAccess.DataAgentDao;
        Agent agent = data.GetAgent(UserSessions.CurrentAgent.AgentUID);

        FormBinding.BindObjectToControls(agent, pnlDetail);

        bool has_access = false;

        UserRole role = null;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_CREDIT_UNDERWRITING, out role))
            has_access = role.Enabled;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_IT, out role))
            has_access = role.Enabled;

        if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_ADMIN, out role))
            has_access = role.Enabled;

        btnPrintCUWebites.Visible = has_access;
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

    #endregion

    #region Event Handlers

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (lstDocumentTypes.SelectedIndex < 1)
        {
            WucMessage1.AddMessageError("Please select a document type");
            return;
        }

        if (fupDocument.HasFile && UserSessions.CurrentAgent != null)
        {
            string file = fupDocument.PostedFile.FileName;
            string file_ext = file.Substring(file.LastIndexOf('.') + 1).ToUpper();

            if (!MDoc.GetWhiteListExtensions().Contains(file_ext))
            {
                WucMessage1.AddMessageError("Invalid file type. Only the following types are accepted: " + CommonUtility.Util.implode(MDoc.GetWhiteListExtensions(), ", "));
                return;
            }

            byte[] image = this.fupDocument.FileBytes;
            string docName = string.Empty;

            ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
            objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"]; //make sure to always add this line

            ZeusWeb.MDocWS.UploadResponse objR = objFU.UploadFileWithSourceAndUser(
                fupDocument.FileBytes
                , 0
                , ""
                , Convert.ToInt32(UserSessions.CurrentAgent.AgentID)
                , UserSessions.CurrentAgent.AgentUID
                , Convert.ToInt32(lstDocumentTypes.SelectedItem.Value)
                , fupDocument.FileName
                , "Zeus"
                , 0
                , Description.Text
                , ""
                , Convert.ToInt32(UserSessions.CurrentAgent.AgentID)
                , (int)MDoc.eMDocSourceID.SalesPartner
                , UserSessions.CurrentUser.UserName
            );

            ////eluxa: uploads using agent id's from hidden controles instead of session variables.
            ////this gets rid of a bug where the user can have two instances of zeus open either on
            ////two browser tabs or two windows and the application logic incorrectly assigns the
            ////uploaded document to the wrong agent id because the session got corrupted
            //ZeusWeb.MDocWS.UploadResponse objR = objFU.UploadFileWithSource(
            //    fupDocument.FileBytes
            //    , 0
            //    , ""
            //    , Convert.ToInt32(this.AgentID.Value)
            //    , this.AgentUID.Value
            //    , Convert.ToInt32(lstDocumentTypes.SelectedItem.Value)
            //    , fupDocument.FileName
            //    , "Zeus"
            //    , 0
            //    , Description.Text
            //    , ""
            //    , Convert.ToInt32(this.AgentID.Value)
            //    , (int)MDoc.eMDocSourceID.SalesPartner
            //);

            if (objR.DocID > 0)
            {
                WucMessage1.AddMessageStatus("File Uploaded: " + fupDocument.FileName);
                Response.Redirect("~/SecureAgentManagementForms/frmAgentDocuments.aspx?AgentUID=" + UserSessions.CurrentAgent.AgentUID);
            }
            else
            {
                WucMessage1.AddMessageError("Could not upload file: " + objR.StatusMessage);
            }

            this.LoadDocuments();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.CurrentPage = e.NewPageIndex + 1;
        this.BindGrid();
    }

    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = m_Prms;
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

    protected void lstDocumentSources_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocuments();
    }

    protected void grdDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                var row = ((DataRowView)e.Row.DataItem).Row;

                MDoc objM = DataMerchantAppPaging.getMDoc(row);

                //MDoc objM = (MDoc)e.Row.DataItem;
                LinkButton lnkDeletedoc = ((LinkButton)e.Row.FindControl("lnkDelete"));
                lnkDeletedoc.OnClientClick = "return confirm('Are you sure you want to delete this document?');";

                CheckBox chkPrivate = ((CheckBox)e.Row.FindControl("IsPrivate"));
                chkPrivate.Checked = CommonUtility.Util.if_b(DataBinder.Eval(e.Row.DataItem, "IsPrivate"), false);

                if (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING)
                    chkPrivate.Enabled = true;
                else
                    chkPrivate.Enabled = false;

                bool isAccessible = (UserSessions.CurrentUser.DefaultRoleUID.ToUpper() == PaymentXP.BusinessObjects.Constants.ROLE_CREDIT_UNDERWRITING || (UserSessions.CurrentUser.UserRoles.ContainsKey(PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS) && UserSessions.CurrentUser.UserRoles[PaymentXP.BusinessObjects.Constants.ROLE_SPECIALACCESS].Enabled == true));

                if (!objM.OrigName.ToUpper().EndsWith(".PDF"))
                {
                    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
                    img1.ImageUrl = "~/images/document_view.png";
                }

                HyperLink lnk = (HyperLink)e.Row.FindControl("hypOrigName");
                Label lbl = (Label)e.Row.FindControl("lblOrigName");

                lnk.NavigateUrl = string.Format("~/SecureAgentManagementForms/frmAgentDocumentPreview.aspx?DocID={0}&AgentUID={1}"
                        , DataBinder.Eval(e.Row.DataItem, "DocID").ToString()
                        , UserSessions.CurrentAgent.AgentUID
                        );

                if (chkPrivate.Checked && !isAccessible)
                {
                    lbl.Visible = true;
                    lnk.Visible = false;
                }

                if (chkPrivate.Checked)
                    lnk.ToolTip = "Private Document";


                // strips off the directory and just puts the filename.
                string[] arr = lnk.Text.Split(new char[] { '\\' });
                lnk.Text = arr[arr.Length - 1];

                if (grdDocuments.EditIndex != e.Row.RowIndex || grdDocuments.EditIndex == -1)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Attributes.Add("style", "display: inline;");

                    //((Label)e.Row.Cells[2].FindControl("lblUnitCost")).Text = DataLayer.Field2Dec(DataBinder.Eval(e.Row.DataItem, "UnitCost")).ToString("c2");
                    //((Label)e.Row.Cells[3].FindControl("lblQuantity")).Text = DataLayer.Field2Int(DataBinder.Eval(e.Row.DataItem, "Quantity")).ToString();
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Visible = (this.VsIsRoleAdmin || this.VsIsRoleCreditUnderWriting);
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Attributes.Add("style", "display: none;");

                    DropDownList ddp = ((DropDownList)e.Row.Cells[3].FindControl("ddpType"));


                    LookupTableHandler.LoadMDocType(ddp, false, (MDoc.eMDocTypeGroup)objM.DocTypeGroupID);
                    ddp.Items.RemoveAt(0);
                    ListHandler.ListFindItem(ddp, DataBinder.Eval(e.Row.DataItem, "DocTypeID").ToString());
                }

                if (UserSessions.CurrentUser.IsBank)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkDelete")).Attributes.Add("style", "display: none;");
                    ((CheckBox)e.Row.FindControl("IsPrivate")).Attributes.Add("style", "display: none;");
                }

                e.Row.Cells[8].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[8].Text);
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
            DropDownList ddp = (DropDownList)grdDocuments.Rows[e.RowIndex].Cells[3].FindControl("ddpType");
            obj.DocTypeID = DataLayer.Field2Int(ddp.SelectedValue);
            obj.Description = ((TextBox)grdDocuments.Rows[e.RowIndex].Cells[4].FindControl("txtDescription")).Text;

            DataAccess.DataDocumentsDao.UpdateMDocument(obj);

            grdDocuments.EditIndex = -1;
            LoadDocuments();
        }
        e.Cancel = true;
    }

    protected void grdDocuments_RowEditing(object sender, GridViewEditEventArgs e)
    {
        grdDocuments.EditIndex = e.NewEditIndex + (grdDocuments.PageIndex * grdDocuments.PageSize);
        //LoadDocuments();
    }

    protected void grdDocuments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdDocuments.EditIndex = -1;
        //LoadDocuments();
    }

    //PXP-10767 >> RThakur Start
    // delete a file
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();

        objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

        if (objFU != null)
        {
            int doc_id = Convert.ToInt32(lb.CommandArgument);

            if (objFU.DeleteFileWithUser(doc_id, 0, Convert.ToInt32(UserSessions.CurrentAgent.AgentID), 0, UserSessions.CurrentUser.UserName))
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
                DataAccess.DataDocumentsDao.UpdateAgentPrivateMDoc(DocID, chkPrivate.Checked);
                hdnDocuments.Value = string.Empty;
                LoadDocuments();
            }
        }

    }

    //PXP-10767 >> RThakur End

    #endregion

    #region Code Removed
    /* Removed */
    //protected void btnPrint_Click(object sender, EventArgs e)
    //{
    //    PrintDocumentBarcode();
    //    //PrintBarcode
    //    //foreach (ListItem lst in lstDocumentTypes2.Items)
    //    //{
    //    //    if (lst.Selected)
    //    //        this.PrintBarcode(lst.Text, lst.Value, true);
    //    //}

    //    //WucMessage1.AddMessageStatus("Barcodes printed successfully!");
    //}

    /* Removed */
    //protected void btnPreview_Click(object sender, EventArgs e)
    //{
    //    PrintDocumentBarcode();
    //    //foreach (ListItem lst in lstDocumentTypes2.Items)
    //    //{
    //    //    if (lst.Selected)
    //    //        this.PrintBarcode(lst.Text, lst.Value, false);
    //    //}
    //}

    /* Removed */
    //protected void btnPreviewZID_Click(object sender, EventArgs e)
    //{
    //    PrintZIDBarcode();
    //}

    /* Removed */
    //private void PrintZIDBarcode()
    //{
    //    //this.PrintBarcode(lst.Text, lst.Value, false);
    //    bool Print = false;
    //    Aspose.Pdf.Pdf pdf1 = pdf1 = new Aspose.Pdf.Pdf();
    //    MemoryStream imgStream = null;

    //    // Generate the barcode image
    //    BarCodeBuilder builder = new BarCodeBuilder();
    //    builder.SymbologyType = Symbology.Code39Standard;

    //    builder.CodeText = UserSessions.CurrentMerchantApp.ID;


    //    builder.xDimension = 0.1f;
    //    builder.yDimension = 0.1f;

    //    // save the barcode image in memory stream
    //    imgStream = new MemoryStream();
    //    builder.Save(imgStream, ImageFormat.Jpeg);
    //    imgStream.Position = 0;


    //    // Create the pdf file and add the barcode image to it


    //    // add the new section for image
    //    Aspose.Pdf.Section secImage = pdf1.Sections.Add();
    //    Aspose.Pdf.Image image1 = new Aspose.Pdf.Image(secImage);
    //    image1.ImageInfo.ImageFileType = ImageFileType.Jpeg;
    //    image1.ImageInfo.OpenType = ImageOpenType.Memory;
    //    image1.ImageScale = 2.5F;
    //    System.IO.BinaryReader reader = new System.IO.BinaryReader(imgStream);
    //    imgStream.Position = 0;
    //    image1.ImageInfo.ImageStream = imgStream;
    //    secImage.Paragraphs.Add(image1);

    //    string s = string.Empty;
    //    s = "\n\nMeritus Payment Solutions\n";
    //    s += "DBA: " + UserSessions.CurrentMerchantApp.BusinessDBAName + "\n";
    //    s += "ZID: " + UserSessions.CurrentMerchantApp.ID + "\n";
    //    s += "MID: " + UserSessions.CurrentMerchantApp.SettlePlatformMid + "\n";


    //    Text dba = new Text(s);

    //    secImage.Paragraphs.Add(dba);
    //    if (Print)
    //    {
    //        PdfViewer viewer = new PdfViewer();
    //        viewer.PrintPageDialog = false;
    //        viewer.AutoResize = true;         // print the file with adjusted size
    //        viewer.AutoRotate = true;         // print the file with adjusted rotation		

    //        MemoryStream[] ms = new MemoryStream[1];
    //        pdf1.Save(ms[0]);
    //        viewer.OpenPdfFile(ms[0]);
    //        //viewer.OpenPdfFile(@"c:/temp/test.pdf");
    //        viewer.PrintDocument();
    //        viewer.ClosePdfFile();
    //    }
    //    else
    //    {

    //        pdf1.Save("barcode.pdf", SaveType.OpenInAcrobat, base.Response);
    //        Response.End();
    //    }

    //    imgStream.Close();

    //}

    /* Removed */
    //private void PrintDocumentBarcode()
    //{
    //    //this.PrintBarcode(lst.Text, lst.Value, false);
    //    bool Print = false;
    //    Aspose.Pdf.Pdf pdf1 = pdf1 = new Aspose.Pdf.Pdf();
    //    MemoryStream imgStream = null;

    //    foreach (ListItem item in lstDocumentTypes2.Items)
    //    {
    //        if (item.Selected)
    //        {
    //            string DocumentType = item.Text;
    //            string DocumentID = item.Value;
    //            // Generate the barcode image
    //            BarCodeBuilder builder = new BarCodeBuilder();
    //            builder.SymbologyType = Symbology.Code39Standard;

    //            builder.CodeText = DocumentID;


    //            builder.xDimension = 0.1f;
    //            builder.yDimension = 0.1f;

    //            // save the barcode image in memory stream
    //            imgStream = new MemoryStream();
    //            builder.Save(imgStream, ImageFormat.Jpeg);
    //            imgStream.Position = 0;


    //            // Create the pdf file and add the barcode image to it


    //            // add the new section for image
    //            Aspose.Pdf.Section secImage = pdf1.Sections.Add();
    //            Aspose.Pdf.Image image1 = new Aspose.Pdf.Image(secImage);
    //            image1.ImageInfo.ImageFileType = ImageFileType.Jpeg;
    //            image1.ImageInfo.OpenType = ImageOpenType.Memory;
    //            image1.ImageScale = 2.5F;
    //            System.IO.BinaryReader reader = new System.IO.BinaryReader(imgStream);
    //            imgStream.Position = 0;
    //            image1.ImageInfo.ImageStream = imgStream;
    //            secImage.Paragraphs.Add(image1);

    //            string s = "\n\nDocument Type: " + DocumentType;

    //            Text dba = new Text(s);

    //            secImage.Paragraphs.Add(dba);
    //        }
    //    }

    //    if (Print)
    //    {
    //        PdfViewer viewer = new PdfViewer();
    //        viewer.PrintPageDialog = false;
    //        viewer.AutoResize = true;         // print the file with adjusted size
    //        viewer.AutoRotate = true;         // print the file with adjusted rotation		

    //        MemoryStream[] ms = new MemoryStream[1];
    //        pdf1.Save(ms[0]);
    //        viewer.OpenPdfFile(ms[0]);
    //        //viewer.OpenPdfFile(@"c:/temp/test.pdf");
    //        viewer.PrintDocument();
    //        viewer.ClosePdfFile();
    //    }
    //    else
    //    {

    //        pdf1.Save("barcode.pdf", SaveType.OpenInAcrobat, base.Response);
    //        Response.End();
    //    }

    //    imgStream.Close();
    //}

    /* Removed */
    //private Winnovative.WnvHtmlConvert.PdfConverter GetPdfConverter()
    //{
    //    Winnovative.WnvHtmlConvert.PdfConverter pdfConverter = new Winnovative.WnvHtmlConvert.PdfConverter();
    //    pdfConverter.LicenseKey = "/NfN3M7P3MTF3MrSzNzPzdLNztLFxcXF";

    //    //pdfConverter.LicenseKey = "put your license key here";

    //    // set the HTML page width in pixels
    //    // the default value is 1024 pixels
    //    pdfConverter.PageWidth = 1024; // autodetect the HTML page width


    //    // set if the generated PDF contains selectable text or an embedded image - default value is true
    //    pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;

    //    //set the PDF page size 
    //    pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
    //    // set the PDF compression level
    //    pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
    //    // set the PDF page orientation (portrait or landscape)
    //    pdfConverter.PdfDocumentOptions.PdfPageOrientation = PDFPageOrientation.Portrait;
    //    //set the PDF standard used to generate the PDF document
    //    pdfConverter.PdfStandardSubset = PdfStandardSubset.Full;
    //    // show or hide header and footer
    //    pdfConverter.PdfDocumentOptions.ShowHeader = false;
    //    pdfConverter.PdfDocumentOptions.ShowFooter = false;
    //    //set the PDF document margins
    //    pdfConverter.PdfDocumentOptions.LeftMargin = 0;
    //    pdfConverter.PdfDocumentOptions.RightMargin = 0;
    //    pdfConverter.PdfDocumentOptions.TopMargin = 0;
    //    pdfConverter.PdfDocumentOptions.BottomMargin = 0;
    //    // set if the HTTP links are enabled in the generated PDF
    //    pdfConverter.PdfDocumentOptions.LiveUrlsEnabled = true;
    //    // set if the HTML content is resized if necessary to fit the PDF page width - default is true
    //    pdfConverter.PdfDocumentOptions.FitWidth = true;
    //    // set if the PDF page should be automatically resized to the size of the HTML content when FitWidth is false
    //    pdfConverter.PdfDocumentOptions.AutoSizePdfPage = true;
    //    // embed the true type fonts in the generated PDF document
    //    pdfConverter.PdfDocumentOptions.EmbedFonts = false;
    //    // compress the images in PDF with JPEG to reduce the PDF document size - default is true
    //    pdfConverter.PdfDocumentOptions.JpegCompressionEnabled = true;
    //    // set if the JavaScript is enabled during conversion 
    //    pdfConverter.ScriptsEnabled = pdfConverter.ScriptsEnabledInImage = false;

    //    // set if the converter should try to avoid breaking the images between PDF pages
    //    pdfConverter.AvoidImageBreak = false;

    //    //pdfConverter.PdfHeaderOptions.HeaderText = textBoxHeaderText.Text;
    //    //pdfConverter.PdfHeaderOptions.HeaderTextColor = Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor), ddlHeaderColor.SelectedValue));
    //    //pdfConverter.PdfHeaderOptions.HeaderSubtitleText = textBoxHeaderSubtitle.Text;
    //    //pdfConverter.PdfHeaderOptions.DrawHeaderLine = cbDrawHeaderLine.Checked;
    //    //pdfConverter.PdfHeaderOptions.HeaderHeight = 50;

    //    //pdfConverter.PdfFooterOptions.FooterText = textBoxFooterText.Text;
    //    //pdfConverter.PdfFooterOptions.FooterTextColor = Color.FromKnownColor((KnownColor)Enum.Parse(typeof(KnownColor), ddlFooterTextColor.SelectedValue));
    //    //pdfConverter.PdfFooterOptions.DrawFooterLine = cbDrawFooterLine.Checked;
    //    //pdfConverter.PdfFooterOptions.PageNumberText = textBoxPageNmberText.Text;
    //    //pdfConverter.PdfFooterOptions.ShowPageNumber = cbShowPageNumber.Checked;
    //    //pdfConverter.PdfFooterOptions.FooterHeight = 50;

    //    pdfConverter.PdfBookmarkOptions.TagNames = false ? new string[] { "h1", "h2" } : null;

    //    return pdfConverter;
    //}

    /* Removed */
    //protected void btnPrintCUWebites_Click(object sender, EventArgs e)
    //{
    //    if (!Page.IsValid)
    //        return;

    //    /* Customer Service Phone */
    //    string urlToConvert = "http://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.BusinessDBAPhone;

    //    string downloadName = "Report";
    //    //byte[] downloadBytes = null;

    //    downloadName += ".pdf";
    //    Winnovative.WnvHtmlConvert.PdfConverter pdfConverter = GetPdfConverter();

    //    // call the converter and get a Document object from URL
    //    Document pdfDocument = pdfConverter.GetPdfDocumentObjectFromUrl(urlToConvert);

    //    // get the conversion summary object from the event arguments
    //    ConversionSummary conversionSummary = pdfConverter.ConversionSummary;

    //    // the PDF page where the previous conversion ended
    //    Winnovative.WnvHtmlConvert.PdfDocument.PdfPage lastPage = pdfDocument.Pages[conversionSummary.LastPageIndex];
    //    // the last rectangle in the last PDF page where the previous conversion ended
    //    RectangleF lastRectangle = conversionSummary.LastPageRectangle;

    //    // the result of adding an element to a PDF page
    //    // ofers the index of the PDF page where the rendering ended 
    //    // and the bounding rectangle of the rendered content in the last page
    //    AddElementResult addResult = null;

    //    // the converter for the second URL
    //    HtmlToPdfElement htmlToPdfURL2 = null;

    //    Winnovative.WnvHtmlConvert.PdfDocument.PdfPage newPage = pdfDocument.Pages.AddNewPage();

    //    try
    //    {
    //        /* Business Phone */
    //        if (UserSessions.CurrentMerchantApp.BusinessPhone != string.Empty)
    //        {
    //            urlToConvert = "http://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.BusinessPhone;
    //            htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
    //            addResult = newPage.AddElement(htmlToPdfURL2);
    //            lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
    //        }
    //    }
    //    catch { }


    //    try
    //    {
    //        /* home Phone */
    //        if (UserSessions.CurrentMerchantApp.Owners[0].HomePhone != string.Empty)
    //        {
    //            urlToConvert = "http://www.phonevalidator.com/results.aspx?p=" + UserSessions.CurrentMerchantApp.Owners[0].HomePhone;
    //            newPage = pdfDocument.Pages.AddNewPage();
    //            htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
    //            addResult = newPage.AddElement(htmlToPdfURL2);
    //            lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
    //        }
    //    }
    //    catch { }

    //    try
    //    {
    //        /* Domain */
    //        if (UserSessions.CurrentMerchantApp.BusinessWebsite != string.Empty)
    //        {
    //            urlToConvert = "http://centralops.net/co/DomainDossier.aspx?addr=" + UserSessions.CurrentMerchantApp.BusinessWebsite + "&dom_whois=true&dom_dns=true&net_whois=true";
    //            newPage = pdfDocument.Pages.AddNewPage();
    //            htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
    //            addResult = newPage.AddElement(htmlToPdfURL2);
    //            lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
    //        }
    //    }
    //    catch { }

    //    try
    //    {
    //        /* Ripoff report */
    //        if (UserSessions.CurrentMerchantApp.BusinessDBAName != string.Empty)
    //        {
    //            urlToConvert = "http://www.ripoffreport.com/Search/" + UserSessions.CurrentMerchantApp.BusinessDBAName + ".aspx";
    //            newPage = pdfDocument.Pages.AddNewPage();
    //            htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
    //            addResult = newPage.AddElement(htmlToPdfURL2);
    //            lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
    //        }
    //    }
    //    catch { }

    //    try
    //    {
    //        /* Ripoff report */
    //        if (UserSessions.CurrentMerchantApp.BusinessDBAName != string.Empty)
    //        {
    //            urlToConvert = "http://www.complaintsboard.com/?search=" + UserSessions.CurrentMerchantApp.BusinessDBAName + "&everything=Everything";
    //            newPage = pdfDocument.Pages.AddNewPage();
    //            htmlToPdfURL2 = new HtmlToPdfElement(0, 0, urlToConvert);
    //            addResult = newPage.AddElement(htmlToPdfURL2);
    //            lastPage = pdfDocument.Pages[addResult.EndPageIndex]; // the PDF page where the previous conversion ended
    //        }
    //    }
    //    catch { }

    //    // add a HTML string after all the rendered content
    //    HtmlToPdfElement htmlStringToPdf = new HtmlToPdfElement(addResult.EndPageBounds.Left, addResult.EndPageBounds.Bottom,
    //        "<b><i>The rendered content ends here</i></b>", null);

    //    lastPage.AddElement(htmlStringToPdf);

    //    string origfname = string.Format("CU-Website-{0}.pdf", CommonUtility.Util.GetDateTimeStamp());
    //    string tempdir = ConfigurationManager.AppSettings["TempUploadDir"];

    //    if (!Directory.Exists(tempdir))
    //    {
    //        Directory.CreateDirectory(tempdir);
    //    }

    //    string filename = string.Format(@"{0}\{1}"
    //        , tempdir
    //        , origfname
    //        );


    //    pdfDocument.Save(filename);

    //    pdfDocument.Close();

    //    byte[] byteArr = CommonUtility.Util.FileToByteArray(filename);

    //    MDocWS.FileUpload objFU = new MDocWS.FileUpload();

    //    MDocWS.UploadResponse objR = objFU.UploadFile(
    //        byteArr
    //        , 0
    //        , ""
    //        , Convert.ToInt32(UserSessions.CurrentAgent.AgentID)
    //        , UserSessions.CurrentAgent.AgentUID
    //        , (int)MDoc.eMDocType.CU_Websites
    //        , origfname
    //        , "Zeus"
    //        , 0
    //        , ""
    //        , ""
    //        , 0
    //        );

    //    pdfDocument = null;

    //    if (objR.DocID > 0)
    //    {
    //        WucMessage1.AddMessageStatus("CU-Website PDF Generated");
    //    }
    //    else
    //    {
    //        WucMessage1.AddMessageError(objR.StatusMessage);
    //    }

    //    Response.Redirect("~/SecureAgentManagementForms/frmAgentDocuments.aspx");

    //    this.LoadDocuments();

    //    /* open pdf */
    //    //System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
    //    //response.Clear();
    //    //response.AddHeader("Content-Type", "binary/octet-stream");
    //    //response.AddHeader("Content-Disposition",
    //    //    "attachment; filename=" + downloadName + "; size=" + downloadBytes.Length.ToString());
    //    //response.Flush();
    //    //response.BinaryWrite(downloadBytes);
    //    //response.Flush();
    //    //response.End();
    //}

    #endregion
}
