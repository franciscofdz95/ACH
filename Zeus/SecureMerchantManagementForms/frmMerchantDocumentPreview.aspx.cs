using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;
using System.Collections.Generic;

public partial class frmMerchantDocumentPreview : frmBaseDataEntry
{
    private string MSG_ERROR_ACCESS_DENIED = "<br /><br />Access Denied [" + "<a alt='back' href='" + "javascript:window.close();" + "'>close</a>" + "]";
    //private string MSG_ERROR_STATEMENT_PREPARE = "<br /><br />Your document is being prepared. Please try again. [" + "<a alt='back' href='" + "javascript:window.close();" + "'>close</a>" + "]";
    private string MSG_ERROR_STATEMENT_ERROR = "<br /><br />There was an error in processing your request. [" + "<a alt='back' href='" + "javascript:window.close();" + "'>close</a>" + "]";
    private static CommonUtility.Crypto crypto = new CommonUtility.Crypto();

    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            try
            {
                int DocID = 0;
                int MerchantID = 0;
                int AgentID = 0;
                int PrimaryKeyID = 0;

                bool IsUsingEncrypted = false;

                if (Request.QueryString["x"] != null)
                {
                    Dictionary<string, string> di = CommonUtility.Crypto.DecryptUrl(Request.QueryString["x"]);

                    if (di != null)
                    {
                        DocID = Convert.ToInt32(CommonUtility.Util.if_di(di, "DocID", "0"));
                        MerchantID = Convert.ToInt32(CommonUtility.Util.if_di(di, "MerchantID", "0"));
                        AgentID = Convert.ToInt32(CommonUtility.Util.if_di(di, "AgentID", "0"));
                        PrimaryKeyID = Convert.ToInt32(CommonUtility.Util.if_di(di, "PrimaryKeyID", "0"));

                        IsUsingEncrypted = true;
                    }

                }

                if (DocID > 0 || Request["DocID"] != null)
                {
                    if (DocID == 0 && Request["DocID"] != null)
                    {
                        DocID = Convert.ToInt32(Request["DocID"]);
                    }


                    // TEMPORARY!!
                    if (CommonUtility.Util.if_s(Request.QueryString["xxMerchantID"], "") != "")
                    {
                        MerchantID = Convert.ToInt32(Request.QueryString["xxMerchantID"]);
                    }
                    else
                    {
                        if (UserSessions.CurrentMerchantApp != null)
                        {
                            MerchantID = Convert.ToInt32(UserSessions.CurrentMerchantApp.ID);
                        }
                    }

                    ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
                    objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

                    ZeusWeb.MDocWS.MDoc obj = objFU.GetFile(DocID, MerchantID, AgentID, PrimaryKeyID);
                    

                    if (obj != null && (MerchantID == obj.MerchantID || IsUsingEncrypted))
                    {

                        // Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", obj.OrigName.Replace(" ", "_").Replace(",", "_")));

                        Response.AddHeader("Content-Length", obj.ContentSize.ToString());


                        if (obj.MimeType.ToLower() == "application/unknown")
                        {
                            // unknown mime type, so we resort to the file extenstion.

                            if (obj.OrigName.EndsWith(".pdf"))
                            {
                                Response.ContentType = "application/pdf";
                            }
                            else if (obj.OrigName.EndsWith(".gif"))
                            {
                                Response.ContentType = "image/gif";
                            }
                            else if (obj.OrigName.EndsWith(".jpg") || obj.OrigName.EndsWith(".jpeg"))
                            {
                                Response.ContentType = "image/jpeg";
                            }
                            else if (obj.OrigName.EndsWith(".png"))
                            {
                                Response.ContentType = "image/png";
                            }
                            else if (obj.OrigName.EndsWith(".tif"))
                            {
                                Response.ContentType = "image/tiff";
                            }
                            else
                            {
                                //leave content type as unknown and let the client's system figure out which
                                //application to associate the file to
                                Response.ContentType = "application/unknown";
                            }
                        }
                        else
                        {
                            Response.ContentType = obj.MimeType;
                        }

                        Response.BinaryWrite(obj.FileBinary);




                        //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script>javascript:var x=window.open('" + file + "','_blank','Height=300,Width=700,menubar=No,toolbar=no,scrollbars=yes');</script>");

                    }
                    else
                    {
                        this.lblMessage.Text = MSG_ERROR_ACCESS_DENIED;
                    }
                }
                else
                {
                    this.lblMessage.Text = MSG_ERROR_STATEMENT_ERROR;
                }

            }
            catch (Exception ex)
            {
                this.lblMessage.Text = MSG_ERROR_STATEMENT_ERROR + ": " + ex.Message;
            }

            Response.End();
        }
    }


    public override void FormShow(string ID)
    {
        throw new Exception("The method or operation is not implemented.");
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
        throw new Exception("The method or operation is not implemented.");
    }

    public override void FormCancel()
    {
        throw new Exception("The method or operation is not implemented.");
    }

    public override void ToggleButtons()
    {
        throw new Exception("The method or operation is not implemented.");
    }
}