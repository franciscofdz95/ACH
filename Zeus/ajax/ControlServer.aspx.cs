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
using System.Text;
using System.IO;
using PaymentXP.DataObjects;

public partial class ControlServer : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter tw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(tw);

        bool assume_success = true;

        if (Request["control"] != null)
        {

            switch (Request["control"])
            {
                case "testcontrol":

                    wucCreateUser mycontrol = (wucCreateUser)Page.LoadControl("~/UserControls/wucCreateUser.ascx");
                    mycontrol.SetParams(Request); // interface
                    mycontrol.ForcePreRender(); // interface

                    mycontrol.RenderControl(hw);
                    break;

                case "uploadfile":


                    ZeusWeb.MDocWS.FileUpload fu = new ZeusWeb.MDocWS.FileUpload();


                    fu.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];


                    string orig_filename = Request.Files["newlyuploadedfile"].FileName;

                    byte[] b = null;
                    using (var memoryStream = new MemoryStream())
                    {
                        Request.Files["newlyuploadedfile"].InputStream.CopyTo(memoryStream);
                        b = memoryStream.ToArray();
                    }


                    int primary_key_id = CommonUtility.Util.if_i(Request.QueryString["PrimaryKeyID"], 0);
                    string primary_key_uid = CommonUtility.Util.if_s(Request.QueryString["PrimaryKeyUID"], "");

                    int merchantapp_id = CommonUtility.Util.if_i(Request.QueryString["MerchantAppID"], 0);
                    string merchantapp_uid = CommonUtility.Util.if_s(Request.QueryString["MerchantAppUID"], "");

                    int mdoc_sourceid = CommonUtility.Util.if_i(Request.QueryString["MDocSourceID"], 0);

                    int default_doctypeid = CommonUtility.Util.if_i(Request.QueryString["DefaultDocTypeID"], 0);// default to other if nothing is specified.
                    int doctypeid = CommonUtility.Util.if_i(Request.QueryString["DocTypeID"], default_doctypeid);
                    string descr = CommonUtility.Util.if_s(Request.QueryString["Description"]);
                    string username = CommonUtility.Util.if_s(Request.QueryString["Username"]);
                    //PXP-8889:START Fix Attachment Download By Ali Khan
                    int agentId = CommonUtility.Util.if_i(Request.QueryString["AgentId"], 0);
                    //PXP-8889:END
                    
                    bool isPrivate = CommonUtility.Util.if_b(Request.QueryString["IsPrivate"], false); // DM-794 by Jorge
                    ZeusWeb.MDocWS.UploadResponse resp = fu.UploadFileWithSourceAndUser(b, merchantapp_id, merchantapp_uid, agentId, null, doctypeid, orig_filename, "Merchant", doctypeid, descr, primary_key_uid, primary_key_id, mdoc_sourceid, username);

                    try
                    {
                        if (resp != null && resp.DocID > 0)
                        {
                            Hashtable prms = new Hashtable();

                            prms.Add("@DocumentID", resp.DocID);
                            prms.Add("@UserName", username);

                            DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);
                            // DM-794 by Jorge: Added update file as private if checkbox private is checked 
                            if (isPrivate)
                            {
                                DataAccess.DataDocumentsDao.UpdatePrivateMDoc(resp.DocID, true);
                            }
                        }
                    }
                    catch
                    {
                        //suppress any exceptions
                    }

                    break;


                default:
                    assume_success = false;
                    break;
            }
        }

        HttpContext.Current.Response.Clear();
        if (assume_success == true)
        {
            HttpContext.Current.Response.Charset = "UTF-8";
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.Write(sb.ToString());
        }
        HttpContext.Current.Response.End();
    }

    // NOTE: VERY NECESSARY. tricks .net into letting you run controls without it being placed in  form runat=server tag.
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
}
