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
using PaymentXP.Facade;
using PaymentXP.BusinessObjects;
using Infragistics.WebUI.WebDataInput;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.DataObjects;
using System.Globalization;
using Paysafe.Zeus3DE.Model;
using System.Web.Services.Description;

public partial class wucMerchantTicketClone : System.Web.UI.UserControl
{
	public Func<string, string, string, bool, bool, DateTime, string> CloneTicketCommand;
	public Action<string> DoneCommand;

	private string TicketUID
	{
		get
		{
			if (ViewState["ctd2401"] == null)
			{
				ViewState["ctd2401"] = string.Empty;
			}
			return (string)ViewState["ctd2401"];
		}
		set { ViewState["ctd2401"] = value; }
	}

	private string TicketIssue
	{
		get
		{
			if (ViewState["cti2401"] == null)
			{
				ViewState["cti2401"] = string.Empty;
			}
			return (string)ViewState["cti2401"];
		}
		set { ViewState["cti2401"] = value; }
	}

	private DateTime TicketDueDate
	{
		get
		{
			if (ViewState["tdd2401"] == null)
			{
				ViewState["tdd2401"] = DateTime.Now;
			}
			return (DateTime)ViewState["tdd2401"];
		}
		set { ViewState["tdd2401"] = value; }
	}

	private bool CloneTicketEnabled
	{
		get
		{
			if (ViewState["cte2401"] == null)
			{
				ViewState["cte2401"] = false;
			}
			return (bool)ViewState["cte2401"];
		}
		set { ViewState["cte2401"] = value; }
	}

	private DataTable CloneTicketsDataTable
	{
		get
		{
			if (ViewState["ctdt2401"] == null)
			{
				ViewState["cte2401"] = new DataTable();
			}
			return (DataTable)ViewState["ctdt2401"];
		}
		set { ViewState["ctdt2401"] = value; }
	}

	public delegate void GridRowCommandHandler(object sender, GridViewCommandEventArgs e);
	public event GridRowCommandHandler GridRowCommand;

	public Hashtable m_Prms
	{
		get
		{
			if (ViewState["m_Prms"] == null)
				return (new Hashtable());
			else
				return (Hashtable)ViewState["m_Prms"];
		}
		set { ViewState["m_Prms"] = value; }
	}

	public int CurrentPage
	{
		get
		{
			if (ViewState["CurrentPage"] == null)
				return 1;
			else
				return (int)ViewState["CurrentPage"];
		}
		set { ViewState["CurrentPage"] = value; }
	}

	public int PageSize
	{
		get
		{
			if (ViewState["PageSize"] == null)
				return 10;
			else
				return (int)ViewState["PageSize"];
		}
		set { ViewState["PageSize"] = value; }
	}

	public SortDirection SortDirectionSearch
	{
		get
		{
			if (ViewState["SortDirectionSearch"] == null)
				return SortDirection.Descending;
			else
				return (SortDirection)ViewState["SortDirectionSearch"];
		}
		set { ViewState["SortDirectionSearch"] = value; }

	}

	public string SortOrder
	{
		get
		{
			if (ViewState["SortOrder"] == null)
				return string.Empty;
			else
				return ViewState["SortOrder"].ToString();
		}
		set { ViewState["SortOrder"] = value; }
	}

	public string PostBackURL
	{
		get
		{
			if (ViewState["PostBackURL"] == null)
				return string.Empty;
			else
				return ViewState["PostBackURL"].ToString();
		}
		set { ViewState["PostBackURL"] = value; }
	}

	public string DataSourceSelectMethod
	{
		get { return odsTransactions.SelectMethod; }
		set { odsTransactions.SelectMethod = value; }
	}

	public string DataSourceSelectCountMethod
	{
		get { return odsTransactions.SelectCountMethod; }
		set { odsTransactions.SelectCountMethod = value; }
	}

	public Unit GridHeight
	{
		set { grd.Height = value; }
	}

	public GridView Grid
	{
		get { return grd; }
	}

	public void SetDataSource(Hashtable prms, int pagesize)
	{
		grd.DataSourceID = "odsTransactions";
		this.CurrentPage = 1;
		this.PageSize = pagesize;
		grd.PageSize = pagesize;
		this.m_Prms = prms;
		BindGrid();
	}

	private void BindGrid()
	{
		if (!txtDBA.Text.Equals(string.Empty))
		{
			if (!m_Prms.ContainsKey("@BusinessDBAName"))
				m_Prms.Add("@BusinessDBAName", txtDBA.Text);
			else
				m_Prms["@BusinessDBAName"] = txtDBA.Text;
		}

		if (!BusinessLegalName.Text.Equals(string.Empty))
		{
			if (!m_Prms.ContainsKey("@BusinessLegalName"))
				m_Prms.Add("@BusinessLegalName", BusinessLegalName.Text);
			else
				m_Prms["@BusinessLegalName"] = BusinessLegalName.Text;
		}

		if (!string.IsNullOrEmpty(AgentID.Text))
		{
			if (!m_Prms.ContainsKey("@AgentID"))
				m_Prms.Add("@AgentID", AgentID.Text);
			else
				m_Prms["@AgentID"] = AgentID.Text;

		}

		if (!string.IsNullOrEmpty(AgentDBA.Text))
		{

			if (!m_Prms.ContainsKey("@AgentDBA"))
				m_Prms.Add("@AgentDBA", AgentDBA.Text);
			else
				m_Prms["@AgentDBA"] = AgentDBA.Text;
		}

		if (UserSessions.CurrentUser.IsAgent)
		{
			if (!m_Prms.ContainsKey("@AgentUIDSub"))
				m_Prms.Add("@AgentUIDSub", UserSessions.CurrentUser.HookTableKeyUID);
			else
				m_Prms["@AgentUIDSub"] = UserSessions.CurrentUser.HookTableKeyUID;
		}

		if (MerchantAppTypeUID.SelectedIndex > 0)
		{
			if (!m_Prms.ContainsKey("@MerchantAppTypeUID"))
				m_Prms.Add("@MerchantAppTypeUID", MerchantAppTypeUID.SelectedItem.Value);
			else
				m_Prms["@MerchantAppTypeUID"] = MerchantAppTypeUID.SelectedItem.Value;
		}

		if (!MerchantID.Text.Equals(string.Empty))
		{
			if (!m_Prms.ContainsKey("@ID"))
				m_Prms.Add("@ID", MerchantID.Text);
			else
				m_Prms["@ID"] = MerchantID.Text;
		}

		if (!AchID.Text.Equals(string.Empty))
		{
			if (!m_Prms.ContainsKey("@AchID"))
				m_Prms.Add("@AchID", AchID.Text);
			else
				m_Prms["@AchID"] = AchID.Text;
		}

		if (!string.IsNullOrWhiteSpace(FMAID.Text))
		{
			m_Prms["@FMAID"] = FMAID.Text;
		}

		if (!string.IsNullOrWhiteSpace(MLEName.Text))
		{
			m_Prms["@BusinessLegalName"] = MLEName.Text.Trim();
		}

		if (!SettlePlatformMid.Text.Equals(string.Empty))
		{
			if (!m_Prms.ContainsKey("@SettlePlatformMid"))
				m_Prms.Add("@SettlePlatformMid", SettlePlatformMid.Text);
			else
				m_Prms["@SettlePlatformMid"] = SettlePlatformMid.Text;
		}

		if (m_Prms != null && m_Prms.Count > 0)
		{

			if (!m_Prms.ContainsKey("@PageSize"))
				m_Prms.Add("@PageSize", this.PageSize);
			else
				m_Prms["@PageSize"] = this.PageSize;

			if (!m_Prms.ContainsKey("@CurrentPage"))
				m_Prms.Add("@CurrentPage", this.CurrentPage);
			else
				m_Prms["@CurrentPage"] = this.CurrentPage;

			if (!m_Prms.ContainsKey("@SortOrder"))
				m_Prms.Add("@SortOrder", "BusinessDBAName");
			else
				m_Prms["@SortOrder"] = this.SortOrder;

			m_Prms["@SortDirection"] = this.ConvertSortDirectionToSql(this.SortDirectionSearch);

			grd.DataBind();

			//lblRecordCount.Text = "Total Records Found: " + DataMerchantAppPaging.GetMerchantAppsPagingRowCount(m_Prms, 0, 0,this.ID).ToString();
			int rowcount = DataMerchantAppPaging.GetMerchantAppsPagingRowCount(m_Prms, 0, 0, this.ID);

			lblRecordCount.Text = "Total Records Found: " + rowcount.ToString();

			pnlRecords.Visible = (rowcount > 0);
			pnlNoRecords.Visible = !(rowcount > 0);
		}
	}

	public void ClearGrid()
	{
		txtDBA.Text = string.Empty;
		BusinessLegalName.Text = string.Empty;
		SettlePlatformMid.Text = string.Empty;
		AchID.Text = string.Empty;
		MerchantID.Text = string.Empty;
		lblRecordCount.Text = "Total Record(s) Found: 0";

		pnlRecords.Visible = false;
		pnlNoRecords.Visible = true;
		MerchantAppTypeUID.SelectedIndex = -1;

		txtDBA.Enabled = true;
		BusinessLegalName.Enabled = true;
		SettlePlatformMid.Enabled = true;
		AchID.Enabled = true;
		MerchantID.Enabled = true;
		MerchantAppTypeUID.Enabled = true;

		btnSearch.Enabled = true;
		btnClose.Enabled = true;
		grd.Enabled = true;
		grd2.Enabled = true;
		grdResult.Enabled = true;

		CloneTickets_Initiate();
	}

	protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
	{
		this.CurrentPage = e.NewPageIndex + 1;
		this.BindGrid();
	}

	protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			//DM-5093 ini
			if (CloneTicketsDataTable != null && CloneTicketEnabled)
			{
				DataRow[] cloneRows = CloneTicketsDataTable.Select("ID = '" + DataBinder.Eval(e.Row.DataItem, "ID").ToString() + "'");
				if (cloneRows != null && cloneRows.Length > 0)
				{
					CheckBox checkBox = (CheckBox)e.Row.FindControl("chkCloneTicket");
					checkBox.Checked = true;
				}
			}
			//DM-5093 end
		}
	}

	protected void odsTransactions_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
	{
		if (this.m_Prms != null)
			e.InputParameters[0] = this.m_Prms;
		else
			e.InputParameters[0] = (new Hashtable());

		e.InputParameters[3] = this.ID;
	}

	protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		LinkButton lnk = null;

		if (e.CommandSource is LinkButton)
			lnk = (LinkButton)e.CommandSource;
		else
			return;

		if (this.GridRowCommand != null)
		{
			this.GridRowCommand(grd, e);
		}
	}

	protected void grd_Sorting(object sender, GridViewSortEventArgs e)
	{
		this.CurrentPage = 1;
		this.SortOrder = e.SortExpression;
		this.SortDirectionSearch = e.SortDirection;
		this.BindGrid();
	}

	private int ConvertSortDirectionToSql(SortDirection direction)
	{
		int newSortDirection;

		switch (direction)
		{
			case SortDirection.Descending:
				newSortDirection = 1;
				this.SortDirectionSearch = SortDirection.Descending;
				break;

			default:
				newSortDirection = 0;
				this.SortDirectionSearch = SortDirection.Ascending;
				break;
		}

		return newSortDirection;
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		this.CloneCalDueDate.Format = UserSessions.CurrentUser.DatePattern;

		AchID.Attributes.Add("onKeyPress", "CheckNumeric();");
		AgentID.Attributes.Add("onKeyPress", "CheckNumeric();");
		MerchantID.Attributes.Add("onKeyPress", "CheckNumeric();");
		btnSearch.Attributes.Add("onclick", "return validate('" + btnSearch.ClientID + "','" + AchID.ClientID + "','" + MerchantID.ClientID + "');");

		if (!this.IsPostBack)
		{
			this.CurrentPage = 1;
			//LookupTableHandler.LoadAgentsNew(AgentUID, true);
			LookupTableHandler.LoadMerchantAppTypes(MerchantAppTypeUID, true);

			//DM-5093 ini
			CloneTicketEnabled = false;
			//DM-5093 end
		}
		btnCloneTicket.Enabled = CloneTicketEnabled;
	}

	protected void btnSearch_Click(object sender, EventArgs e)
	{
		this.CurrentPage = 1;
		grd.PageIndex = 0;
		Hashtable prms = new Hashtable();
		ViewState["m_Prms"] = prms;
		BindGrid();
	}

	protected void btnClose_Click(object sender, EventArgs e)
	{
		if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
		{
			((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
		}
		//DM-5093 ini
		CloneTicketEnabled = false;
		CloneTickets_Initiate();
		btnCloneTicket.Enabled = CloneTicketEnabled;
		//DM-5093 end
	}

	//DM-5093 ini

	public void SetTicket(string ticketUID, string problem, DateTime dueDate)
	{
		TicketUID = ticketUID;
		TicketIssue = problem;
		TicketDueDate = dueDate;
	}

	protected void CloneTickets_Initiate()
	{
		CloneTicketsDataTable = new DataTable();
		CloneTicketsDataTable.Columns.Add("chkCloneTicket");
		CloneTicketsDataTable.Columns.Add("ID");
		CloneTicketsDataTable.Columns.Add("MerchantAppUID");
		CloneTicketsDataTable.Columns.Add("BusinessDBAName");
		CloneTicketsDataTable.Columns.Add("BusinessLegalName");
		CloneTicketsDataTable.Columns.Add("SettlePlatformMid");
		CloneTicketsDataTable.Columns.Add("AgentFirstLastName");
		CloneTicketsDataTable.Columns.Add("FMAID");
		CloneTicketsDataTable.Columns.Add("TicketID");
		CloneTicketsDataTable.Columns.Add("TicketUID");
		CloneTicketsDataTable.Columns.Add("Bank");
		CloneTicketsDataTable.Columns.Add("Status");
		CloneTicketsDataTable.Columns.Add("ACHStatus");

		grd2.DataSource = CloneTicketsDataTable;
		grd2.DataBind();
	}

	protected void chkCloneTicket_Click(object sender, EventArgs e)
	{
		if (!CloneTicketEnabled)
		{
			CloneTickets_Initiate();
			CloneTicketEnabled = true;
			btnCloneTicket.Enabled = CloneTicketEnabled;
		}

		var checkBox = (CheckBox)sender;
		if (checkBox.Checked && CloneTicketsDataTable.Rows.Count >= 10)
		{
			mdError.Visible = true;
			mdMsg.Text = "only 10 ZIDs are allowed to clone.";
			mdClose.Enabled = true;
			checkBox.Checked = false;
			return;
		}

		GridViewRow row = ((GridViewRow)checkBox.NamingContainer);

		HyperLink hypZID = (HyperLink)row.Cells[0].FindControl("hypZID");
		HiddenField hfMerchantAppUID = (HiddenField)row.Cells[0].FindControl("hfMerchantAppUID");

		DataRow[] rowsToDelete = CloneTicketsDataTable.Select("ID = '" + hypZID.Text + "'");

		if (checkBox.Checked && rowsToDelete.Length == 0)
		{
			DataRow newRow = CloneTicketsDataTable.NewRow();

			newRow["chkCloneTicket"] = checkBox.Checked;
			newRow["ID"] = HttpUtility.HtmlDecode(hypZID.Text);
			newRow["MerchantAppUID"] = HttpUtility.HtmlDecode(hfMerchantAppUID.Value);
			newRow["BusinessDBAName"] = HttpUtility.HtmlDecode(row.Cells[2].Text);
			newRow["BusinessLegalName"] = HttpUtility.HtmlDecode(row.Cells[3].Text);
			newRow["SettlePlatformMid"] = HttpUtility.HtmlDecode(row.Cells[4].Text);
			newRow["AgentFirstLastName"] = HttpUtility.HtmlDecode(row.Cells[5].Text);
			//newRow["FMAID"] = HttpUtility.HtmlDecode(row.Cells[6].Text);
			newRow["Bank"] = HttpUtility.HtmlDecode(row.Cells[6].Text);
			newRow["Status"] = HttpUtility.HtmlDecode(row.Cells[7].Text);
			newRow["ACHStatus"] = HttpUtility.HtmlDecode(row.Cells[8].Text);

			//// Add the new row to the DataTable
			CloneTicketsDataTable.Rows.Add(newRow);
		}
		else if (!checkBox.Checked && rowsToDelete.Length > 0)
		{
			foreach (DataRow irow in rowsToDelete)
			{
				CloneTicketsDataTable.Rows.Remove(irow);
			}
			CloneTicketsDataTable.AcceptChanges();
		}

		//// Rebind the GridView to reflect the changes
		grd2.DataSource = CloneTicketsDataTable;
		grd2.DataBind();
	}

	protected void grd2_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			CheckBox checkBox = (CheckBox)e.Row.FindControl("chkCloneTicket");
			checkBox.Checked = DataBinder.Eval(e.Row.DataItem, "chkCloneTicket").ToString() == "True" ? true : false;
		}
	}

	protected void btnCloneTicket_Click(object sender, EventArgs e)
	{
		LookupTableHandler.LoadTime1(ddlDueDateTime);
		ddlDueDateTime.Items.Insert(0, new ListItem("-- Select --", "-1"));

		txtIssue.Text = TicketIssue;
		txtDueDate.Text = WebUtil.ConvertToUserDatePattern(TicketDueDate.ToString());
		ListHandler.ListFindItem(ddlDueDateTime, TicketDueDate.ToString("HH:mm tt"));

		dlgOK.Visible = true;
		btnOK.Enabled = true;
		btnCancel.Enabled = true;
		txtIssue.Enabled = true;
		txtIssue.ReadOnly = false;
		txtDueDate.Enabled = true;
		txtDueDate.ReadOnly = false;
		ddlDueDateTime.Enabled = true;
		CloneCalDueDate.Enabled = true;
		chbNotesClone.Enabled = true;
		chbNotesClone.Checked = false;
		chbAttachmentsClone.Enabled = true;
		chbAttachmentsClone.Checked = false;

		pnlOK.Visible = true;
		pnlResult.Visible = false;
	}

	protected string DueDateValidate()
	{
		string message = string.Empty;
		DateTime dtDueDate;
		DateTime.TryParseExact(txtDueDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDueDate);

		if (CommonUtility.Util.if_date(dtDueDate.ToString("MM/dd/yyyy"), DateTime.MinValue) != DateTime.MinValue)
		{
			if (ddlDueDateTime.SelectedValue == "-1")
				message += "Time is missing for Due Date." + Environment.NewLine;

			DateTime dt = CommonUtility.Util.if_date(dtDueDate.ToString("MM/dd/yyyy") + " " + ddlDueDateTime.SelectedValue.Replace("-1", ""), DateTime.MinValue);

			if (dt <= DateTime.Now)
			{
				message += "Due Date cannot be less than current date time." + Environment.NewLine;
			}
		}
		else
		{
			message += "Please enter a valid Due Date." + Environment.NewLine;
		}
		return message;
	}

	protected void btnOk_Click(object sender, EventArgs e)
	{
		issueError.Visible = false;
		dueDateError.Visible = false;

		if (string.IsNullOrEmpty(txtIssue.Text))
		{
			issueError.Text = "The issue can't be empty";
			issueError.Visible = true;
			return;
		}
		//validate due date
		var msg = DueDateValidate();
		if (!string.IsNullOrEmpty(msg))
		{
			dueDateError.Text = msg;
			dueDateError.Visible = true;
			return;
		}

		pnlOK.Visible = false;
		pnlResult.Visible = true;

		//System.Threading.Thread.Sleep(10000);

		DataRow[] rows = CloneTicketsDataTable.Select("chkCloneTicket = 'True'");
		foreach (DataRow irow in rows)
		{
			if (CloneTicketCommand != null)
			{
				string rsTticket = CloneTicketCommand.Invoke(TicketUID, irow["MerchantAppUID"].ToString(), txtIssue.Text, chbNotesClone.Checked, chbAttachmentsClone.Checked, TicketDueDate);
				var str = rsTticket.Split(';');
				irow["TicketID"] = str[0];
				irow["TicketUID"] = str[1];
			}
		}
		grdResult.DataSource = CloneTicketsDataTable;
		grdResult.DataBind();
		btnDone.Enabled = true;
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		dlgOK.Visible = false;
	}

	protected void btnDone_Click(object sender, EventArgs e)
	{
		if (this.Parent.NamingContainer != null && this.Parent.NamingContainer.GetType().Equals(typeof(WebDialogWindow)))
		{
			((WebDialogWindow)this.Parent.NamingContainer).WindowState = DialogWindowState.Hidden;
		}
		//DM-5093 ini
		CloneTicketEnabled = false;
		CloneTickets_Initiate();
		btnCloneTicket.Enabled = CloneTicketEnabled;
		dlgOK.Visible = false;
		if (DoneCommand != null)
		{
			DoneCommand.Invoke(TicketUID);
		}
		//DM-5093 end
	}

	protected void grdResult_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			Label laID = (Label)e.Row.FindControl("labTicketID");
			laID.Attributes.Add("onclick", string.Format("OpenTicket('{0}')", DataBinder.Eval(e.Row.DataItem, "TicketUID").ToString()));
		}
	}

	protected void CloneDueDate_ValueChanged(object sender, EventArgs e)
	{
		// Bug fixed by Jorge: added validation when DueDateTime.SelectedItem is null
		string time = ddlDueDateTime.SelectedItem != null && ddlDueDateTime.SelectedItem.Value == "-1" ? string.Empty : " " + ddlDueDateTime.SelectedItem.Text;

		DateTime dtDueDate;
		DateTime.TryParseExact(txtDueDate.Text, UserSessions.CurrentUser.DatePattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtDueDate);

		TicketDueDate = DataLayer.Field2Date(dtDueDate.ToString("MM/dd/yyyy") + " " + time);
	}

	protected void mdClose_Click(object sender, EventArgs e)
	{
		mdError.Visible = false;
	}
	//DM-5093 end
}
