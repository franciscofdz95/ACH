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

public partial class frmStatementsPreview : frmBaseDataEntry
{
    private string MSG_ERROR_ACCESS_DENIED = "<br /><br />Access Denied [" + "<a alt='back' href='" + "javascript:window.close();" + "'>close</a>" + "]";
    private string MSG_ERROR_STATEMENT_PREPARE = "<br /><br />Your statement is being prepared. Please try again. [" + "<a alt='back' href='" + "javascript:window.close();" + "'>close</a>" + "]";
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
                string authPlatformMID = Request.QueryString["a"];
                string month = Request.QueryString["m"];
                string year = Request.QueryString["y"];
                string dba = Request.QueryString["d"] ?? "";

                if (!(string.IsNullOrEmpty(authPlatformMID) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(year)))
                {
                    string path = ConfigurationManager.AppSettings.Get("StatementsPath");

                    authPlatformMID = crypto.Decrypt(authPlatformMID);
                    month = crypto.Decrypt(month);
                    year = crypto.Decrypt(year);

                    string file = path + "//" + year + "//" + month + "//" + authPlatformMID + ".pdf";

                    if (File.Exists(file))
                    {
                        string fileName = GenerateFileName(authPlatformMID, month, year, dba);

                        Response.ContentType = "Application/pdf";
                        Response.AddHeader("Content-Disposition", $"inline; filename=\"{fileName}\"");
                        Response.WriteFile(file);
                        Response.End();

                        //Page.ClientScript.RegisterStartupScript(GetType(), "test", "<script>javascript:var x=window.open('" + file + "','_blank','Height=300,Width=700,menubar=No,toolbar=no,scrollbars=yes');</script>");
                    }
                    else
                    {
                        this.lblMessage.Text = MSG_ERROR_STATEMENT_PREPARE;
                    }                  
                }
                else
                {
                    this.lblMessage.Text = MSG_ERROR_ACCESS_DENIED;
                }
            }
            catch
            {
                this.lblMessage.Text = MSG_ERROR_STATEMENT_ERROR;
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

    private string GenerateFileName(string merchantId, string month, string year, string dba = "")
    {
        try
        {
            if (!string.IsNullOrEmpty(dba))
            {
                return $"{crypto.Decrypt(dba)}_{merchantId}_{GetMonthName(month)}_{year}.pdf";
            }
            else
            {
                return $"{merchantId}_{GetMonthName(month)}_{year}.pdf";
            }
        }
        catch
        {
            return $"{merchantId}_{month}_{year}.pdf";
        }
    }

    private string GetMonthName(string month)
    {
        if (int.TryParse(month, out int monthNumber) && monthNumber >= 1 && monthNumber <= 12)
        {
            return new DateTime(2000, monthNumber, 1).ToString("MMMM");
        }

        return month;
    }
}