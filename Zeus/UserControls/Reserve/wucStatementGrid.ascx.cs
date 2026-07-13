using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using System.Data;
using OfficeOpenXml;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucStatementGrid : wucBaseSearch
    {

        public bool HasPending
        {
            get { return (bool)(this.ViewState["HasPending"] ?? false); }
            set { this.ViewState["HasPending"] = value; }
        }

        public int ZID
        {
            get
            {
                if (ViewState["ZID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["ZID"];
                }
            }
            set { ViewState["ZID"] = value; }
        }

        public string DatePeriod
        {
            get
            {
                if (ViewState["DatePeriod"] == null)
                {
                    return "";
                }
                else
                {
                    return (String)ViewState["DatePeriod"];
                }
            }
            set { ViewState["DatePeriod"] = value; }
        }

        public int VSReserveSourceID
        {
            get
            {
                if (ViewState["VSReserveSourceID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["VSReserveSourceID"];
                }
            }
            set { ViewState["VSReserveSourceID"] = value; }
        }



        public int VSBankID
        {
            get
            {
                if (ViewState["VSBankID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["VSBankID"];
                }
            }
            set { ViewState["VSBankID"] = value; }
        }


        public int VSReserveTypeID
        {
            get
            {
                if (ViewState["VSReserveTypeID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["VSReserveTypeID"];
                }
            }
            set { ViewState["VSReserveTypeID"] = value; }
        }


        // used to keep track of the ddl status. for some reason, using the control in the updatepanel + gridivew loses the state for the ddl. so we manually mind later.
        public string VSReportDate
        {
            get { return (string)ViewState["VSReportDate"]; }
            set { ViewState["VSReportDate"] = value; }
        }

        public string VSSource
        {
            get { return (string)ViewState["VSSource"]; }
            set { ViewState["VSSource"] = value; }
        }


        public string VSBank
        {
            get { return (string)ViewState["VSBank"]; }
            set { ViewState["VSBank"] = value; }
        }

        public string VSType
        {
            get { return (string)ViewState["VSType"]; }
            set { ViewState["VSType"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);

            this.PreRender += new EventHandler(wucReserveStatement_PreRender);

            if (!Page.IsPostBack)
            {
                this.FormShow();
            }
        }

        public void BindGrid()
        {
            if (this.ZID > 0)
            {
                Hashtable prms2 = new Hashtable();

                prms2.Add("@ZID", this.ZID);

                if (!string.IsNullOrEmpty(this.DatePeriod) && this.DatePeriod != "All")
                {
                    DateTime dt = DateTime.Parse(this.DatePeriod);
                    prms2.Add("@Period", dt);
                }

                if (this.VSReserveSourceID > 0)
                {
                    prms2.Add("@ReserveSourceID", this.VSReserveSourceID);
                }


                if (this.VSReserveTypeID > 0)
                {
                    prms2.Add("@ReserveTypeID", this.VSReserveTypeID);
                }

                if (this.VSBankID > 0)
                {
                    prms2.Add("@BankID", this.VSBankID);
                }

                if (this.IncludeReservesHeldAtMeritus)
                {
                    prms2.Add("@IncludeReservesHeldAtMeritus", this.IncludeReservesHeldAtMeritus);
                }


                this.m_Prms = prms2;
                DataSet ds2 = DataReserve.GetRDBStatement(prms2);

                grdStatement.DataSource = ds2.Tables[0].DefaultView;
                grdStatement.DataBind();
            }
        }

        protected void wucReserveStatement_PreRender(object sender, EventArgs e)
        {

            if (grdStatement.HeaderRow != null)
            {
                DropDownList dDate = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReportDate");
                if (dDate != null)
                {
                    this.LoadPeriods(this.ZID, dDate);
                }

                DropDownList dType = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReserveType");
                if (dType != null)
                {
                    LookupTableHandler.LoadRDBReserveType(dType, true);
                }

                DropDownList dSource = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReserveSource");
                if (dSource != null)
                {
                    LookupTableHandler.LoadRDBReserveSource(dSource, true);
                }

                DropDownList dBank = (DropDownList)grdStatement.HeaderRow.FindControl("ddlBank");
                if (dBank != null)
                {
                    LookupTableHandler.LoadRDBBank(dBank, true);
                }


            }
        }

        public void FormShow()
        {
            //this.BindGrid();
        }

        protected void lstPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.DatePeriod = ddl.SelectedValue;
            this.VSReportDate = ddl.SelectedValue;
            this.BindGrid();
        }

        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.VSReserveTypeID = CommonUtility.Util.if_i(ddl.SelectedValue, 0);
            this.VSType = ddl.SelectedValue;
            this.BindGrid();
        }

        protected void ddlSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.VSReserveSourceID = CommonUtility.Util.if_i(ddl.SelectedValue, 0);
            this.VSSource = ddl.SelectedValue;
            this.BindGrid();
        }

        protected void ddlBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)sender;
            this.VSBankID = CommonUtility.Util.if_i(ddl.SelectedValue, 0);
            this.VSBank = ddl.SelectedValue;
            this.BindGrid();
        }

        protected void UpdatePanel1_PreRender(object sender, EventArgs e)
        {
            DropDownList ddl = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReportDate");
            ddl.SelectedValue = this.VSReportDate;

            DropDownList ddlTy = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReserveType");
            ddlTy.SelectedValue = this.VSReserveTypeID.ToString();

            DropDownList ddlSo = (DropDownList)grdStatement.HeaderRow.FindControl("ddlReserveSource");
            ddlSo.SelectedValue = this.VSReserveSourceID.ToString();

            DropDownList ddlB = (DropDownList)grdStatement.HeaderRow.FindControl("ddlBank");
            ddlB.SelectedValue = this.VSBankID.ToString();
        }

        protected void grdStatement_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //// bold first and last rows
                //if (e.Row.RowIndex == 0 )
                //{
                //    e.Row.Style.Add("font-weight", "bold");
                //}

                Label lbRD = (Label)e.Row.FindControl("Label1");
                Label lbPend = (Label)e.Row.FindControl("lblPending");
                lbPend.Visible = false;

                if (lbRD.Text == "Starting Balance")
                {
                    e.Row.Style.Add("font-weight", "bold");
                }
                else if (lbRD.Text == "Ending Balance")
                {
                    e.Row.Style.Add("font-weight", "bold");
                    lbPend.Visible = this.HasPending;
                }
            }
        }

        private void LoadPeriods(int zid, DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem("All Dates", "All"));

            if (zid > 0)
            {
                Hashtable prms2 = new Hashtable();
                prms2.Add("@ZID", zid);
                DataSet ds2 = DataReserve.GetRDBStatementMonth(prms2);

                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    string nice = dr["Period"].ToString();

                    if (CommonUtility.Validation.IsNumeric(nice) && nice.Length == 6)
                    {
                        int year = Convert.ToInt32(nice.Substring(0, 4));
                        int month = Convert.ToInt32(nice.Substring(4, 2));

                        DateTime dt = new DateTime(year, month, 1);

                        nice = string.Format("{0} {1}", year.ToString(), dt.ToString("MMMM"));
                    }

                    ddl.Items.Add(new ListItem(nice, dr["StartDate"].ToString()));
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Statement");

                DataSet ds = DataReserve.GetRDBStatement(this.m_Prms);

                string[,] liHeaders = new string[6, 3] {
			            {"PostedDate", "PostedDate", "string"},	
                        {"Type", "Reserve Type", "string"},	
                        {"Source", "Description", "string"},	
                        {"BankName", "Bank", "string"},	
                        {"Amount", "Amount", "currency"},	
                        {"RunningTotal", "Balance", "currency"},	
                    };

                DataTable dt = GridViewExportUtil.GetExportableDataTable(ds.Tables[0], liHeaders);

                GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                ws.Cells.LoadFromDataTable(dt, true);


                string filename = String.Format("RDBStatement_ZID-{0}_{1}.xlsx", this.ZID.ToString(), CommonUtility.Util.GetDateTimeStamp());

                //pck.Workbook.Worksheets.Add(filename, ws);

                //Write it back to the client
                Response.Clear();   // necessary!!!
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!

            }
        }






    }
}