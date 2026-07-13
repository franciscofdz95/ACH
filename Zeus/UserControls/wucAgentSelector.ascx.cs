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
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

public partial class Control_wucAgentSelector : System.Web.UI.UserControl
{
    public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
    public event GridRowCommandHandler GridRowCommand;


    public LinkButton btnSelect
    {
        get { return lbSelectAgent; }
    }

    public string m_AgentUID
    {
        get { return AgentUID.Text; }
        set { AgentUID.Text = value; }
    }

    public string m_AgentID
    {
        get { return AgentID.Text; }
        set { AgentID.Text = value; }
    }

    public string m_AgentDBA
    {
        get { return AgentDBA.Text; }
        set { AgentDBA.Text = value; }
    }

    public enum eASLayoutStyle
    {
        vertical = 1,
        horizontal = 2
    }

    public Unit IDWidth
    {
        set { ViewState["IDWidth"] = value; }
        get
        {
            if (ViewState["IDWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["IDWidth"];
        }
    }

    public short IDTabIndex
    {
        set { ViewState["IDTabIndex"] = value; }
        get
        {
            if (ViewState["IDTabIndex"] == null)
                return 0;
            else
                return (short)ViewState["IDTabIndex"];
        }
    }

    public Unit DBAWidth
    {
        set { ViewState["DBAWidth"] = value; }
        get
        {
            if (ViewState["DBAWidth"] == null)
                return Unit.Pixel(90);
            else
                return (Unit)ViewState["DBAWidth"];
        }
    }

    public short DBATabIndex
    {
        set { ViewState["DBATabIndex"] = value; }
        get
        {
            if (ViewState["DBATabIndex"] == null)
                return 0;
            else
                return (short)ViewState["DBATabIndex"];
        }
    }

    public Unit lblIDWidth
    {
        set { ViewState["lblIDWidth"] = value; }
        get
        {
            if (ViewState["lblIDWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblIDWidth"];
        }
    }

    public Unit lblDBAWidth
    {
        set { ViewState["lblDBAWidth"] = value; }
        get
        {
            if (ViewState["lblDBAWidth"] == null)
                return Unit.Pixel(60);
            else
                return (Unit)ViewState["lblDBAWidth"];
        }
    }

    public eASLayoutStyle LayoutStyle
    {
        get
        {
            if (ViewState["LayoutStyle"] == null)
                return (eASLayoutStyle.vertical);
            else
                return (eASLayoutStyle)ViewState["LayoutStyle"];
        }
        set { ViewState["LayoutStyle"] = value; }
    }

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
        grdAgent.GridRowCommand += new wucAgent.GridRowCommandHandler(grdAgents_GridRowCommand);
        this.PreRender += new EventHandler(wucAgentSelector_PreRender);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(AgentDBA.Text))
           // AgentDBA.Text = Request[AgentDBA.UniqueID];
            AgentDBA.Text = AgentDBA1.Value;

        if (string.IsNullOrEmpty(AgentSelectorMasterAgentUID.Text))
            AgentSelectorMasterAgentUID.Text = Request[AgentSelectorMasterAgentUID.UniqueID];

        if (string.IsNullOrEmpty(AgentUID.Text))
            AgentUID.Text = Request[AgentUID.UniqueID];

        dlgAgent.Attributes.Add("onKeyDown", "KeyDownHandler('" + ((Button)grdAgent.FindControl("btnSearch")).ClientID + "')");
        grdAgent.DataSourceSelectCountMethod = "GetAgentsPagingRowCount";
        grdAgent.DataSourceSelectMethod = "GetAgentsPaging";

        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsBank)
        {
            pnlDetails.Visible = false;
        }

        if (UserSessions.CurrentUser != null && UserSessions.CurrentUser.IsAgent)
            AgentSelectorMasterAgentUID.Text = UserSessions.CurrentUser.HookTableKeyUID;

        if (!IsPostBack)
        {
            dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
            
        }

    }

    void wucAgentSelector_PreRender(object sender, EventArgs e)
    {
        switch (this.LayoutStyle)
        {
            case eASLayoutStyle.horizontal:
                pnlAgent.CssClass = "AgentSelectorHorizontal";
                break;

            default:
                pnlAgent.CssClass = "AgentSelectorVertical";
                break;
        }

        AgentID.Width = IDWidth;
        AgentDBA.Width = DBAWidth;
        lblAgentID.Width = lblIDWidth;
        lblAgentDBA.Width = lblDBAWidth;

        if (this.IDTabIndex > 0)
            AgentID.TabIndex = this.IDTabIndex;

        if (this.DBATabIndex > 0)
            AgentDBA.TabIndex = this.DBATabIndex;
    }

    private void grdAgents_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        LinkButton lnk = null;
        if (e.CommandSource is LinkButton)
        {
            lnk = (LinkButton)e.CommandSource;
        }
        else
        {
            return;
        }

        string[] str = e.CommandArgument.ToString().Split(new char[] { ',' });
        string uid = str[0];

        AgentUID.Text = str[0];
        DataAgent da = new DataAgent();
        Agent app = da.GetAgent(uid);
        AgentDBA.Text = app.AgentDBA;
        AgentID.Text = app.AgentID.ToString();

        grdAgent.ClearGrid();
        this.dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;

        if (this.GridRowCommand != null)
        {
            this.GridRowCommand(sender, e);
        }
    }

    protected void btnAgentSelect_Click(object sender, EventArgs e)
    {
        Hashtable prms = new Hashtable();
        grdAgent.SetDataSource(prms, 10);
        dlgAgent.Modal = false;
        dlgAgent.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }

    public void FormClear()
    {
        grdAgent.ClearGrid();
        AgentID.Text = string.Empty;
        AgentDBA.Text = string.Empty;
        AgentUID.Text = string.Empty;
        AgentSelectorMasterAgentUID.Text = string.Empty; 
    }

    public void EnableAgentDBA()
    {
        AgentDBA.ReadOnly = false;
    }

    public void SetEditMode(bool editmode)
    {
        btnSelect.Visible = editmode;
        AgentDBA.Enabled = editmode;
        AgentID.Enabled = editmode;
    }
}
