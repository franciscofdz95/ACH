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

public partial class frmLeadDocuments : frmBaseDataEntry
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkLeadDocuments")).CssClass = "active";

        if (!this.IsPostBack)
        {
            this.FormShow("");
        }

        if (UserSessions.CurrentUser.IsBank)
        {
            pnlUpload.Visible = false;
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
        prms.Add("@PrimaryKeyID", UserSessions.CurrentLead.LeadID);
        prms.Add("@DocTypeGroupID", (int)MDoc.eMDocTypeGroup.Lead);

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

        LookupTableHandler.LoadMDocType(lstDocumentTypes, false, MDoc.eMDocTypeGroup.Lead);

        lstDocumentSources.Items.Add(new ListItem(MDoc.eMDocTypeGroup.Lead.ToString(), ((int)MDoc.eMDocTypeGroup.Lead).ToString()));

        //Lead lead = DataLead.GetInstance().GetLead(ID);
        
        ////if we can't find the lead then we'll redirect the user to the search leads
        ////because if the user uploads a document, the doc will fail to be associated
        ////with a lead and thus causing a bug
        //if (lead == null)
        //    Response.Redirect("~/SecureLeadForms/frmLeads.aspx");

        FormBinding.BindObjectToControls(UserSessions.CurrentLead, pnlDetail);

        this.LoadDocuments();

        ////////////DataAgent data = DataAccess.DataAgentDao;
        ////////////Agent agent = data.GetAgent(UserSessions.CurrentLead.AgentUID);

        FormBinding.BindObjectToControls(UserSessions.CurrentLead, pnlDetail);

        
        
        
        
        ////bool has_access = false;

        ////UserRole role = null;

        ////if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_CREDIT_UNDERWRITING, out role))
        ////    has_access = role.Enabled;

        ////if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_IT, out role))
        ////    has_access = role.Enabled;

        ////if (UserSessions.CurrentUser.UserRoles.TryGetValue(Constants.ROLE_ADMIN, out role))
        ////    has_access = role.Enabled;
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

        if (fupDocument.HasFile && UserSessions.CurrentLead != null)
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
                , 0
                , ""
                , Convert.ToInt32(lstDocumentTypes.SelectedItem.Value)
                , fupDocument.FileName
                , "Zeus"
                , 0
                , Description.Text
                , ""
                , Convert.ToInt32(UserSessions.CurrentLead.LeadID)
                , (int)MDoc.eMDocSourceID.Lead
                , UserSessions.CurrentUser.UserName
            );

            if (objR.DocID > 0)
            {
                WucMessage1.AddMessageStatus("File Uploaded: " + fupDocument.FileName);
                Response.Redirect("~/SecureLeadForms/frmLeadDocuments.aspx?LeadUID=" + UserSessions.CurrentLead.LeadUID);
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

                if (!objM.OrigName.ToUpper().EndsWith(".PDF"))
                {
                    System.Web.UI.WebControls.Image img1 = (System.Web.UI.WebControls.Image)e.Row.FindControl("Image1");
                    img1.ImageUrl = "~/images/document_view.png";
                }

                HyperLink lnk = (HyperLink)e.Row.FindControl("hypOrigName");

                lnk.NavigateUrl = "~/SecureLeadForms/frmLeadDocumentPreview.aspx?"
                                                        + "DocID=" + DataBinder.Eval(e.Row.DataItem, "DocID").ToString() 
                                                        + "&PrimaryKeyID=" + DataBinder.Eval(e.Row.DataItem, "PrimaryKeyID");

                // strips off the directory and just puts the filename.
                string[] arr = lnk.Text.Split(new char[] { '\\' });
                lnk.Text = arr[arr.Length - 1];

                if (grdDocuments.EditIndex != e.Row.RowIndex || grdDocuments.EditIndex == -1)
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                }
                else
                {
                    ((LinkButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((LinkButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((LinkButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");

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
    }

    protected void grdDocuments_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        grdDocuments.EditIndex = -1;
    }

    #endregion
}
