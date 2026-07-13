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


public partial class LeadCategories : System.Web.UI.UserControl
{
    public LinkButton btnAdd
    {
        get { return lnkAdd; }
    }

    public Label labelName
    {
        get { return lblName; }
    }

    public Panel pnlGrid
    {
        get { return pnlGrd; }
    }

    public Panel pnl
    {
        get { return Panel1; }
    }

    public Panel pnlCat
    {
        get { return pnlCategories; }
    }

    public string LeadUID
    {
        get { if (ViewState["LeadUID"] != null) return ViewState["LeadUID"].ToString(); else return string.Empty; }
        set { ViewState["LeadUID"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LookupTableHandler.LoadLeadCategories(Category, false);
        if (grdBusiness.Rows.Count == 0)
            LoadEmptyGrid();

        lnkAdd.Enabled = (LeadUID != string.Empty);
    }

    protected void odsLeads1_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@LeadID", DataLayer.UID2Field(LeadUID));
        prms.Add("@Category", "1");
        e.InputParameters[0] = prms;
    }

    protected void odsLeads2_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        Hashtable prms = new Hashtable();
        prms.Add("@LeadID", DataLayer.UID2Field(LeadUID));
        prms.Add("@Category", "2,3");
        e.InputParameters[0] = prms;
    }

    protected void grdBusiness_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string url = string.Empty;
        string status = string.Empty;
        string leaduid = string.Empty;
        string assignto = string.Empty;
        GridViewRow grdRow = null;

        // basically, we just want to have a whitelist of valid commands.
        Dictionary<string, string> diValidCommands = new Dictionary<string, string>();

        diValidCommands.Add("EditLead", "ImageButton");
        diValidCommands.Add("UpdateLead", "ImageButton");
        diValidCommands.Add("CancelLead", "ImageButton");
        diValidCommands.Add("DeleteLead", "ImageButton");

        if (diValidCommands.ContainsKey(e.CommandName))
        {
            if (diValidCommands[e.CommandName] == "ImageButton")
            {
                ImageButton btn = (ImageButton)e.CommandSource;
                grdRow = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            }
            else
                return;

            DataLeadServices objdataLead = new DataLeadServices();
            Hashtable prms = new Hashtable();

            switch (e.CommandName)
            {
                case "UpdateLead":

                    prms = new Hashtable();

                    prms.Add("@LeadID", LeadUID);
                    prms.Add("@LeadServiceID", grdBusiness.DataKeys[grdRow.RowIndex].Values["LeadsServicesUID"].ToString());
                    prms.Add("@ServiceID", grdBusiness.DataKeys[grdRow.RowIndex].Values["ServiceID"].ToString());
                    prms.Add("@Checked", true);
                    prms.Add("@Description", ((TextBox)grdBusiness.Rows[grdRow.RowIndex].FindControl("txtContactName")).Text);
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

                    objdataLead.UpdateLeadServices(prms);
                    grdBusiness.EditIndex = -1;

                    BindCategories();
                    break;

                case "EditLead":

                    grdBusiness.EditIndex = grdRow.RowIndex;
                    BindCategories();
                    break;

                case "CancelLead":

                    grdBusiness.EditIndex = -1;
                    BindCategories();
                    break;

                case "DeleteLead":

                    prms = new Hashtable();

                    prms.Add("@LeadID", LeadUID);
                    prms.Add("@LeadServiceID", grdBusiness.DataKeys[grdRow.RowIndex].Values["LeadsServicesUID"].ToString());
                    prms.Add("@ServiceID", grdBusiness.DataKeys[grdRow.RowIndex].Values["ServiceID"].ToString());
                    prms.Add("@Checked", false);
                    prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

                    objdataLead.UpdateLeadServices(prms);
                    grdBusiness.EditIndex = -1;

                    BindCategories();
                    break;
            }
        }
    }

    protected void grdBusiness_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                break;

            case DataControlRowType.DataRow:

                if (grdBusiness.EditIndex != e.Row.RowIndex || grdBusiness.EditIndex == -1)
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: none;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: inline;");
                }
                else
                {
                    ((ImageButton)e.Row.FindControl("lnkUpdate")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkCancel")).Attributes.Add("style", "display: inline;");
                    ((ImageButton)e.Row.FindControl("lnkEdit")).Attributes.Add("style", "display: none;");
                }

                e.Row.Cells[2].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[2].Text);
                e.Row.Cells[4].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[4].Text);
                break;

            case DataControlRowType.Footer:
                break;

            default:
                break;
        }
    }

    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                CheckBox chk = (CheckBox)e.Row.FindControl("chkChecked");
                chk.Checked = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Checked"));

                TextBox txt = (TextBox)e.Row.FindControl("text");
                txt.Text = DataBinder.Eval(e.Row.DataItem, "Description").ToString();

                break;
            default:
                break;
        }
    }

    protected void btnSaveB_Click(object sender, EventArgs e)
    {
        Hashtable prms;
        DataLeadServices leadServices = new DataLeadServices();

        prms = new Hashtable();
        prms.Add("@LeadServiceID", Category.SelectedValue);
        prms.Add("@LeadID", DataLayer.UID2Field(LeadUID));
        prms.Add("@Description", Notes.Text);
        prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);
        leadServices.InsertLeadServices(prms);

        ListHandler.ListFindItem(Category, "-1");
        Notes.Text = "";
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        FormServices();
    }

    protected void btnCancelB_Click(object sender, EventArgs e)
    {
        ListHandler.ListFindItem(Category, "-1");
        Notes.Text = "";
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        Notes.Text = string.Empty;
        ListHandler.ListFindItem(Category, "-1");
        WebDialogWindow4.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    private void LoadEmptyGrid()
    {
        IList<LeadServices> lstLeadServices = new List<LeadServices>();

        lstLeadServices.Add(new LeadServices());
        grd.Visible = true;
        grd.DataSource = lstLeadServices;
        grd.DataBind();

        grdBusiness.Visible = false;
        grdBusiness.DataSource = null;
        grdBusiness.DataBind();

        int columnsCount = grdBusiness.Columns.Count;
        grd.Rows[0].Cells.Clear();

        // clear all the cells in the row      
        grd.Rows[0].Cells.Add(new TableCell());

        //add a new blank cell      
        grd.Rows[0].Cells[0].ColumnSpan = columnsCount;
        grd.Rows[0].Cells[0].CssClass = "EmptyDataRowStyle";

        //set No Results found to the new added cell       
        grd.Rows[0].Cells[0].Text = ".....No categories for the lead....";
        grd.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    }

    public void BindCategories()
    {
        if (LeadUID != string.Empty)
        {
            DataLeadServices leadser = new DataLeadServices();
            Hashtable prms = new Hashtable();
            prms.Add("@LeadID", LeadUID);
            prms.Add("@Category", "5");
            prms.Add("@Checked", true);

            DataSet ds = leadser.GetLeadServicesDetails(prms);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                grdBusiness.Visible = true;
                grdBusiness.DataSource = ds;
                grdBusiness.DataBind();

                grd.Visible = false;
                grd.DataSource = null;
                grd.DataBind();
            }
            else
            {
                LoadEmptyGrid();
            }
        }
        else
        {
            LoadEmptyGrid();
        }

    }

    public void UpdtateServices()
    {
        Hashtable prms;
        DataLeadServices leadServices = new DataLeadServices();

        foreach (GridViewRow grdRow in grd1.Rows)
        {
            prms = new Hashtable();
            prms.Add("@LeadServiceID", DataLayer.UID2Field(grd1.DataKeys[grdRow.RowIndex].Values["LeadServiceID"].ToString()));
            prms.Add("@LeadID", DataLayer.UID2Field(LeadUID));
            prms.Add("@Checked", ((CheckBox)grdRow.Cells[0].FindControl("chkChecked")).Checked);
            prms.Add("@Description", ((TextBox)grdRow.Cells[2].FindControl("text")).Text);
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

            leadServices.UpdateLeadServices(prms);
        }

        foreach (GridViewRow grdRow in grd2.Rows)
        {
            prms = new Hashtable();
            prms.Add("@LeadServiceID", DataLayer.UID2Field(grd2.DataKeys[grdRow.RowIndex].Values["LeadServiceID"].ToString()));
            prms.Add("@LeadID", DataLayer.UID2Field(LeadUID));
            prms.Add("@Checked", ((CheckBox)grdRow.Cells[0].FindControl("chkChecked")).Checked);
            prms.Add("@Description", ((TextBox)grdRow.Cells[2].FindControl("text")).Text);
            prms.Add("@UserCreated", UserSessions.CurrentUser.UserName);

            leadServices.UpdateLeadServices(prms);
        }
    }

    public void FormServices()
    {
        grd1.DataBind();
        grd2.DataBind();

        BindCategories();
    }
}
