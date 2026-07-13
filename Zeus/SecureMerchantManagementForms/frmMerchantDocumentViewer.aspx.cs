using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
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

public partial class frmMerchantDocumentViewer : frmBaseDataEntry
{

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
            if (Request.QueryString["duid"] != null)
            {
                string docuid = Request.QueryString["duid"];

                Hashtable prms = new Hashtable();
                prms.Add("@DocumentImagesUID", docuid);
                prms.Add("@IncludeImage", true);
                
                SqlCommand cmd = DataLayer.CreateCommand("sp_SearchDocumentImages", prms);

                SqlDataReader reader = DataLayer.GetDataReader(cmd, DataLayer.ConnectStringImageDBBuild());

                if (reader.Read())
                {
                    byte[] buffer = new byte[reader.GetInt64(reader.GetOrdinal("ContentLength"))];
                    string contenttype = reader.GetString(reader.GetOrdinal("ContentType"));
                    long image = reader.GetBytes(reader.GetOrdinal("Image"), 0, buffer, 0, buffer.Length);
                    
                    try
                    {
                        Response.Clear();
                        Response.ContentType = contenttype;
                        Response.AddHeader("content-disposition", "inline;");
                        Response.AddHeader("content-length", image.ToString());
                        Response.BinaryWrite(buffer);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("<html><title>error</title><body>" + ex.Message + "</body></html>");
                    }
                    finally
                    {
                        Response.Flush();
                        Response.End();
                        reader.Close();
                    }
                }
            }
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
