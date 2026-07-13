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
using System.Collections.Generic;

public partial class frmQueueAR : frmBaseDataEntry
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
            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Relationship Management Queue";
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus("2223E9E3-970D-435F-A2E2-C5247F43AA7A", pnlReceived, "Received");
        this.LoadMerchantAppByStatus("226D8CA8-6F11-4323-BD45-6EBA9A65CA4E", pnlPendingInternal, "Pending Internal");
        this.LoadMerchantAppByStatus("A9204C55-4802-4D5C-85B7-24AEF07E818A", pnlSent2DataEntry, "Sent to Data Entry");
    }

    protected void lstOfficeAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadQueues();
    }

    public void LoadMerchantAppByStatus(string status, wucApplicationQueue pnl, string Title)
    {
        List<string> officeAccess = LookupTableHandler.GetSelectedOffices(lstOfficeAccess);
        pnl.SetDataSource(status, Title, officeAccess);
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
