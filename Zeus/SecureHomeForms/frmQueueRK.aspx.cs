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

public partial class frmQueueRK : frmBaseDataEntry
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!this.IsPostBack)
        {
            //UserSessions.CurrentPage = "> " + UserSessions.CurrentModule + " > Credit Underwriting Queue";

            LookupTableHandler.LoadOfficeQueueAccess(lstOfficeAccess); 
            LoadQueues();
        }
    }

    public void LoadQueues()
    {
        this.LoadMerchantAppByStatus("35c0e149-d8e2-4b59-a223-eee410e80821", pnlAB2Risk, "SENT FROM APPLICATION BOARDING");
        this.LoadMerchantAppByStatus("b3c5096e-36d0-4db8-82c7-8c4ec401fbe0", pnlReceived, "RECEIVED");
        this.LoadMerchantAppByStatus("85cc2592-74e0-461d-b089-dc96f59f7589", pnlInReview, "IN REVIEW");
        this.LoadMerchantAppByStatus("04eb3c35-2274-48a9-8752-c471a41abb39", pnlSentToDeployment, "SENT TO DEPLOYMENT");          

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
