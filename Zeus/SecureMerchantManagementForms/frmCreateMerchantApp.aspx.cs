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

public partial class frmCreateMerchantApp : frmBaseDataEntry
{
    protected override void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);
        // Set Property to Store ViewState on Server
        base.StoreViewStateOnServer = false;
    }
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (UserSessions.CurrentMerchantApp != null)
        {
            this.Page.Title = string.Format("DBA: {0} - {1}", UserSessions.CurrentMerchantApp.BusinessDBAName, "Create Merchant App");
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
