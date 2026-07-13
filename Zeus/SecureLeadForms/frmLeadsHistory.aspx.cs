using System;
using System.Data;
using System.Data.SqlClient;
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

public partial class frmLeadHistory : frmBaseDataEntry
{

   

    protected void Page_Load(object sender, EventArgs e)
    {
        PaymentXP.BusinessObjects.Logging.XPLogger.ZeusBalancingLog.InfoFormat("Last accessed in {0} on {1} in {2} by {3}. BIGip cookie name: {4} : value: {5}",
            this.GetType().Name, DateTime.Now.ToString(), HttpContext.Current.Server.MachineName, UserSessions.CurrentUser == null ? string.Empty : UserSessions.CurrentUser.UserName, CommonUtility.WebUtil.BIGipName, CommonUtility.WebUtil.GetFromCookie(CommonUtility.WebUtil.BIGipName, string.Empty));
        ((HyperLink)this.Master.FindControl("lnkHistory")).CssClass = "active";

        if (!this.IsPostBack)
        {
            //this.UID = UserSessions.CurrentLeadUID;
            this.UID = UserSessions.CurrentLead.LeadUID;
            this.FormShow(this.UID);
        }
    }

  

    public override void FormShow(string ID)
    {
        DataLead data = DataAccess.DataLeadDao;
        Lead objLead = data.GetLead(ID);

        FormClear();

        this.LeadInfo1.FormShow(objLead, false);

        Hashtable prms = new Hashtable();
        prms.Add("@LeadsUID", this.UID);
        DataSet ds = data.GetLeadsStatusHistory(prms);
        grd.DataSource = ds;
        grd.DataBind();
        lblStatus.Visible = !(grd.Rows.Count > 0);
    }

    public override void FormClear()
    {
        this.LeadInfo1.FormClear();
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

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                e.Row.Cells[1].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[1].Text);
                break;
            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

}
