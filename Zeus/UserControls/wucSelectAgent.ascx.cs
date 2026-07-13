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
using PaymentXP.Facade;
using PaymentXP.DataObjects;

public partial class wucSelectAgent : System.Web.UI.UserControl
{

    public string WebDialogWindowClientID
    {
        get { return ViewState["WebDialogWindowClientID"].ToString(); }
        set { ViewState["WebDialogWindowClientID"] = value; }
    }


    public string HookTableDBAClientID
    {
        get { return ViewState["HookTableDBAClientID"].ToString(); }
        set { ViewState["HookTableDBAClientID"] = value; }
    }

    public string HookTableIDClientID
    {
        get { return ViewState["HookTableIDClientID"].ToString(); }
        set { ViewState["HookTableIDClientID"] = value; }
    }

    public string HookTableUIDClientID
    {
        get { return ViewState["HookTableUIDClientID"].ToString(); }
        set { ViewState["HookTableUIDClientID"] = value; }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Search(true);
        }
    } 

    public void Search(bool IsOnLoad)
    {
        int RecordCount = 0;
        //Populate search fields

        //hash table is use to store parameters which will be passed to the stored procedure
        Hashtable prms = new Hashtable();

        //If procedure is called for the first time pass a dummy parameter to initial the grid
        if (ID.Text != string.Empty)
            prms.Add("@ID", ID.Text);

        if (DBA.Text != string.Empty)
            prms.Add("@DBA", DBA.Text);

        if (FirstName.Text != string.Empty)
            prms.Add("@FirstName", FirstName.Text);

        if (LastName.Text != string.Empty)
            prms.Add("@LastName", LastName.Text);        

        if (prms.Count > 0)
        {
            DataAgent data = DataAccess.DataAgentDao;
            DataSet ds = null;

            ds = data.GetAgentsDS(prms);
            DataView dv = ds.Tables[0].DefaultView;
            RecordCount = dv.Table.Rows.Count;

            grd.DataSource = ds;
            grd.DataBind();
        }
        lblRecordCount.Text = "Total Records Found: " + RecordCount.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.Search(false);
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        FormHandler.ClearAllControls(pnlSearch);
        grd.DataSource = null;
        grd.DataBind();
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                string UID = DataBinder.Eval(e.Row.DataItem, "AgentID").ToString();
                LinkButton btn = ((LinkButton)e.Row.FindControl("btnSelect"));
                btn.Attributes.Add("onClick", "ShowHookTableSelectAgent('" + e.Row.Cells[2].Text + "','" + e.Row.Cells[3].Text + "','" + UID + "');");

                break;
            default:
                break;
        }
    }
}
