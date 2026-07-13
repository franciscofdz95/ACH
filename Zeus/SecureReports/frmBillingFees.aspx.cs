using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ZeusWeb.Class;
using System.Text;
using System.Reflection;

namespace ZeusWeb.SecureReports
{
    public partial class frmBillingFees : System.Web.UI.Page
    {
        private BillingTypeEnum BillingType
        {
            get { return ViewState["BillingType"] == null ? BillingTypeEnum.Select : (BillingTypeEnum)ViewState["BillingType"]; }
            set { ViewState["BillingType"] = value; }
        }

        private int BankColumn
        {
            get { return ViewState["BankColumn"] == null ? -1 : (int)ViewState["BankColumn"]; }
            set { ViewState["BankColumn"] = value; }
        }

        private int ActionColumn
        {
            get { return ViewState["ActionColumn"] == null ? -1 : (int)ViewState["ActionColumn"]; }
            set { ViewState["ActionColumn"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormHandler.SetSecurity(this.Page);
                BillingType = BillingTypeEnum.Select;
                //TableIsEmpty();
            }
            BillingType = GetBillingTypeEnum();
            if (BillingType.Equals(BillingTypeEnum.Select))
            {
                TableIsEmpty();
                pnlMonth.Visible = false;
                pnlButtons.Visible = false;
            }                
        }

        private void FillDataTable()
        {
            TableIsEmpty();
            switch (BillingType)
            {
                case BillingTypeEnum.Select:
                    TableIsEmpty();
                    break;
                case BillingTypeEnum.Annual:
                    if (btnAch.Visible)
                    {
                        List<AnnualFeesRow> data = DataTablePaging.GetAnnualFees();
                        cblBanks.Items.Clear();
                        data.Select(x => x.Bank).Distinct().ToList().ForEach(x => cblBanks.Items.Add(x));
                        foreach (ListItem item in cblBanks.Items) item.Selected = true;
                        DataBind(data);
                    }
                    break;
                case BillingTypeEnum.PCI_NAF:
                    if (btnAch.Visible)
                    {
                        List<PCINAFRow> data = DataTablePaging.GetPCINAFBilling();
                        cblBanks.Items.Clear();
                        data.Select(x => x.Bank).Distinct().ToList().ForEach(x => cblBanks.Items.Add(x));
                        foreach (ListItem item in cblBanks.Items) item.Selected = true;
                        DataBind(data);
                    }
                    break;
                case BillingTypeEnum.Annual_PCI:
                    if (btnAch.Visible)
                    {
                        List<PCIAnnualRow> data = DataTablePaging.GetPCIAnnualBilling();
                        cblBanks.Items.Clear();
                        data.Select(x => x.Bank).Distinct().ToList().ForEach(x => cblBanks.Items.Add(x));
                        foreach (ListItem item in cblBanks.Items) item.Selected = true;
                        DataBind(data);
                    }
                    break;
            }
        }

        private void TableIsEmpty()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[9] {
                        new DataColumn("DBA", typeof(string)),
                        new DataColumn("ZID", typeof(string)),
                        new DataColumn("AnnualFee", typeof(string)),
                        new DataColumn("PartnerID", typeof(string)),
                        new DataColumn("PartnerName", typeof(string)),
                        new DataColumn("Status", typeof(string)),
                        new DataColumn("Bank", typeof(string)),
                        new DataColumn("Month", typeof(string)),
                        new DataColumn("Action", typeof(string))
                    });
            DataBind(dt);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillDataTable();
        }

        private void DataBind(object data)
        {
            dgvData.Columns.Clear();
            BoundField column = null;

            switch (BillingType)
            {
                case BillingTypeEnum.Select:
                    PropertyInfo[] lstSelect = typeof(AnnualFeesRow).GetProperties();
                    foreach (PropertyInfo oProperty in lstSelect)
                    {
                        column = new BoundField() { HeaderText = oProperty.Name, DataField = oProperty.Name };
                        dgvData.Columns.Add(column);
                    }
                    column = new BoundField() { HeaderText = "Action" };
                    dgvData.Columns.Add(column);
                    break;
                case BillingTypeEnum.Annual:
                    PropertyInfo[] lstAnnual = typeof(AnnualFeesRow).GetProperties();
                    foreach (PropertyInfo oProperty in lstAnnual)
                    {
                        column = new BoundField() { HeaderText = oProperty.Name, DataField = oProperty.Name };
                        dgvData.Columns.Add(column);
                    }
                    column = new BoundField() { HeaderText = "Action" };
                    dgvData.Columns.Add(column);
                    break;
                case BillingTypeEnum.PCI_NAF:
                    PropertyInfo[] lstPCI_NAF = typeof(PCINAFRow).GetProperties();
                    foreach (PropertyInfo oProperty in lstPCI_NAF)
                    {
                        column = new BoundField() { HeaderText = oProperty.Name, DataField = oProperty.Name };
                        dgvData.Columns.Add(column);
                    }
                    column = new BoundField() { HeaderText = "Action" };
                    dgvData.Columns.Add(column);
                    break;
                case BillingTypeEnum.Annual_PCI:
                    PropertyInfo[] lstPCI_Annual = typeof(PCIAnnualRow).GetProperties();
                    foreach (PropertyInfo oProperty in lstPCI_Annual)
                    {
                        column = new BoundField() { HeaderText = oProperty.Name, DataField = oProperty.Name };
                        dgvData.Columns.Add(column);
                    }
                    column = new BoundField() { HeaderText = "Action" };
                    dgvData.Columns.Add(column);
                    break;
            }


            dgvData.DataSource = data;
            dgvData.DataBind();
            dgvData.UseAccessibleHeader = true;
            dgvData.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        private BillingTypeEnum GetBillingTypeEnum()
        {
            BillingTypeEnum BillingSelected = BillingTypeEnum.Select;
            switch (ddlBillingOptions.SelectedValue)
            {
                case "Select":
                    BillingSelected = BillingTypeEnum.Select;
                    BankColumn = -1;
                    ActionColumn = -1;
                    break;
                case "Annual":
                    BillingSelected = BillingTypeEnum.Annual;
                    BankColumn = 6;
                    ActionColumn = 8;
                    break;
                case "PCI NAF":
                    BillingSelected = BillingTypeEnum.PCI_NAF;
                    BankColumn = 10;
                    ActionColumn = 15;
                    break;
                case "Annual PCI":
                    BillingSelected = BillingTypeEnum.Annual_PCI;
                    BankColumn = 22;
                    ActionColumn = 25;
                    break;                
            }
            return BillingSelected;
        }

        protected void dgvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAction = new DropDownList();
                ddlAction.Items.Add("Bill");
                ddlAction.Items.Add("Waive");
                e.Row.Cells[dgvData.Columns.Count - 1].Controls.Add(ddlAction);
                switch (BillingType)
                {
                    case BillingTypeEnum.Annual:
                        e.Row.Cells[BankColumn].CssClass = "bankColumn";
                        e.Row.Cells[ActionColumn].CssClass = "actionColumn";
                        break;
                    case BillingTypeEnum.PCI_NAF:
                        e.Row.Cells[BankColumn].CssClass = "bankColumn";
                        e.Row.Cells[ActionColumn].CssClass = "actionColumn";
                        break;
                    case BillingTypeEnum.Annual_PCI:
                        e.Row.Cells[BankColumn].CssClass = "bankColumn";
                        e.Row.Cells[ActionColumn].CssClass = "actionColumn";
                        break;
                }
            }
        }

        protected void btnAch_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.AddRange(new DataColumn[1] {
                        new DataColumn("ZID", typeof(string))
                    });
            List<string> dtList = hdnZids.Value.Split(',').ToList();
            dtList.RemoveAll(x => x == "");
            dtList.ForEach(x => dt.Rows.Add(dt.NewRow()[0] = x));

            switch (BillingType)
            {
                case BillingTypeEnum.Annual:
                        DataTablePaging.InsertACHAnnualFees(dt, UserSessions.CurrentUser.UserName);
                        ddlBillingOptions.SelectedIndex = 0;
                        ddlBillingOptions_SelectedIndexChanged(ddlBillingOptions, EventArgs.Empty);
                        TableIsEmpty();
                    break;
                case BillingTypeEnum.PCI_NAF:
                        DataTablePaging.InsertACHPCINAFFees(dt, UserSessions.CurrentUser.UserName);
                        ddlBillingOptions.SelectedIndex = 0;
                        ddlBillingOptions_SelectedIndexChanged(ddlBillingOptions, EventArgs.Empty);
                        TableIsEmpty();
                    break;
                case BillingTypeEnum.Annual_PCI:
                    DataTablePaging.InsertACHPCIAnnualFees(dt, UserSessions.CurrentUser.UserName);
                    ddlBillingOptions.SelectedIndex = 0;
                    ddlBillingOptions_SelectedIndexChanged(ddlBillingOptions, EventArgs.Empty);
                    TableIsEmpty();
                    break;
            }
        }

        protected void ddlBillingOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            BillingType = GetBillingTypeEnum();
            TableIsEmpty();
            switch (BillingType)
            {
                case BillingTypeEnum.Select:
                    pnlMonth.Visible = false;
                    lblMonth.Text = string.Empty;
                    pnlButtons.Visible = false;
                    cblBanks.Items.Clear();                    
                    break;
                case BillingTypeEnum.Annual:
                    pnlMonth.Visible = true;
                    lblMonthDescription.Text = "Month";
                    lblMonth.Text = DateTime.Now.ToString("MMMM");
                    pnlButtons.Visible = true;
                    break;
                case BillingTypeEnum.PCI_NAF:
                    pnlMonth.Visible = true;
                    lblMonthDescription.Text = "Month";
                    lblMonth.Text = DateTime.Now.ToString("MMMM");
                    pnlButtons.Visible = true;
                    break;
                case BillingTypeEnum.Annual_PCI:
                    pnlMonth.Visible = true;
                    DateTime firstDay, lastDay, today = DateTime.Today;
                    if (today.Month <= 6)
                    {
                        firstDay = new DateTime(today.Year, 1, 1);
                        lastDay = new DateTime(today.Year,6, DateTime.DaysInMonth(today.Year, 06));
                    }
                    else
                    {
                        firstDay = new DateTime(today.Year, 7, 1);
                        lastDay = new DateTime(today.Year, 12, DateTime.DaysInMonth(today.Year, 12));
                    }
                    lblMonthDescription.Text = "Period";
                    lblMonth.Text = string.Concat(firstDay.ToString("MM/dd/yyyy")," - ", lastDay.ToString("MM/dd/yyyy"));
                    pnlButtons.Visible = true;
                    break;
            }
        }
    }
}