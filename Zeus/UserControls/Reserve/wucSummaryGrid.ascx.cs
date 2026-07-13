using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using PaymentXP.DataObjects;
using OfficeOpenXml;
using System.IO;
using PaymentXP.BusinessObjects.Reserve;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucSummaryGrid : wucBaseSearch
    {


        public delegate void EventClickExport(List<RDBSummary> li, int zid);
        public event EventClickExport event_click_export;

        public bool HasPending
        {
            get { return (bool)(this.ViewState["HasPending"] ?? false); }
            set { this.ViewState["HasPending"] = value; }
        }

        public bool HasJournal
        {
            get { return (bool)(ViewState["HasJournal"] ?? false); }
            set { ViewState["HasJournal"] = value; }
        }

        public int ZID
        {
            get { return (int)(ViewState["ZID"] ?? 0); }
            set { ViewState["ZID"] = value; }
        }

        public bool ShowFilter
        {
            get { return (bool)(ViewState["_ShowFilter"] ?? true); }
            set { ViewState["_ShowFilter"] = value; }
        }

        public bool ShowExport
        {
            get { return (bool)(ViewState["_ShowExport"] ?? true); }
            set { ViewState["_ShowExport"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    this.BindGrid();
            //}

          

            // thank you!!! http://forums.asp.net/t/1090634.aspx
            // when this control is rendered in an update panel, the "Response.End()" returns an error. add this to allow the script manager to recognize it.
            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);

            this.PreRender += wucSummaryGrid_PreRender;
        }

        protected void wucSummaryGrid_PreRender(object sender, EventArgs e)
        {
            //gridhead.Visible = this.ShowFilter;
            tblExport.Visible = this.ShowExport;

            if (this.ShowFilter == false)
            {
                pnlDate.Style.Add("display", "none");
            }
        }

        protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            if (this.m_Prms == null)
            {
                e.Cancel = true;
            }
            else
            {
                e.InputParameters[0] = this.m_Prms;
            }
        }


        /// <summary>
        /// set the datasource from the data stored in mprms and pagesize. this.zid will be used if not passed in.
        /// </summary>
        /// <param name="prms"></param>
        /// <param name="pagesize"></param>
        public List<RDBSummary> SetDataSource(Hashtable prms)
        {
            GridView1.DataSourceID = "ods"; 
            this.CurrentPage = 1;
            GridView1.PageIndex = 0;
            GridView1.PageSize = this.PageSize;
            this.m_Prms = prms;
            return BindGrid();


        }

        /// <summary>
        /// acts similar to formshow, in that this fucntion is called by whatever page is using it.
        /// </summary>
        /// <returns></returns>
        private List<RDBSummary> BindGrid()
        {

            List<RDBSummary> liSum = null;
            

            if (this.ZID > 0)
            {
                // we only want to display the date ddl if a zid is set
                pnlDate.Visible = true;

                if (ddlReportDate.Items.Count == 0)
                {
                    this.LoadPeriods(this.ZID, ddlReportDate);
                }
            }
            else
            {
                pnlDate.Visible = false;
            }

            if (this.m_Prms != null)
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
                    m_Prms.Add("@SortOrder", "DateCreated");
                else
                    m_Prms["@SortOrder"] = this.SortOrder;

                m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

                GridView1.PageSize = this.PageSize;
                GridView1.PageIndex = this.CurrentPage - 1;

                if (this.ZID > 0)
                {
                    this.m_Prms["@ZID"] = this.ZID;

                    if (!string.IsNullOrEmpty(ddlReportDate.SelectedValue) && ddlReportDate.SelectedValue != "All")
                    {
                        DateTime dtP = DateTime.Parse(ddlReportDate.SelectedValue);
                        m_Prms["@Period"] = dtP;
                    }
                    else
                    {
                        if (m_Prms.ContainsKey("@Period"))
                        {
                            m_Prms.Remove("@Period");
                        }
                    }
                }

                if (this.HasJournal)
                {
                    m_Prms["@HasJournal"] = this.HasJournal;
                }

                if (this.IncludeReservesHeldAtMeritus)
                {
                    m_Prms["@IncludeReservesHeldAtMeritus"] = this.IncludeReservesHeldAtMeritus;
                }

                liSum = DataReserve.GetRDBSummary(m_Prms);

                if (liSum != null)
                {
                    
                    lblRowCount.Text = liSum[0].TotalRecordCount.ToString();

                    // hide header if it's just one row.
                    gridhead.Visible = (liSum.Count > 1) ? true : false;

                }

                

                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }

            return liSum;
        }


        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.CurrentPage = 1;
            this.SortOrder = e.SortExpression;
            this.SortDirectionSearch = e.SortDirection;
            this.BindGrid();


        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.Header)
            {
                // put all the cells here that you want to span 2 rows
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[2].Visible = false;
                e.Row.Cells[11].Visible = false;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                RDBSummary obj = (RDBSummary)e.Row.DataItem;

                Label lbPending = (Label)e.Row.FindControl("lblPending");

                lbPending.Visible = obj.HasPending;

                this.HasPending = obj.HasPending;
                

            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.CurrentPage = e.NewPageIndex + 1;
            this.BindGrid();
        }


        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
            // thank you: http://www.dotnettwitter.com/2010/12/how-to-create-multiple-row-header-and.html

            // Adding a column manually once the header created
            if (e.Row.RowType == DataControlRowType.Header) // If header created
            {
                GridView ProductGrid = (GridView)sender;

                // Creating a Row
                GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                TableCell HeaderCell = new TableCell();
                HeaderCell.Text = "MerchantID";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Year Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "ZID";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2; // For merging first, second row cells to one
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Period Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "DBAName";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Revenue Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Divert";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding Revenue Column
                HeaderCell = new TableCell();
                HeaderCell.Text = "Reserve";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.ColumnSpan = 4;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                HeaderCell = new TableCell();
                HeaderCell.Text = "Balance";
                HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                HeaderCell.RowSpan = 2;
                HeaderCell.CssClass = "HeaderStyle";
                HeaderRow.Cells.Add(HeaderCell);

                //Adding the Row at the 0th position (first row) in the Grid
                ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandSource is LinkButton)
            //{

            //    LinkButton lb = (LinkButton)e.CommandSource;

            //    switch (lb.CommandName)
            //    {
            //        case "ReportDate":

            //            int release_id = Convert.ToInt32(lb.CommandArgument);

            //            if (event_click_reportdate != null)
            //            {
            //                event_click_reportdate(release_id);
            //            }

            //            break;
            //    }
            //}

        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CurrentPage = 1;
            this.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);

            this.BindGrid();
        }

        protected void lstPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BindGrid();
            
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

            if (lstExportPageSize.SelectedValue == "All Pages")
            {

                this.m_Prms["@CurrentPage"] = 1;
                this.m_Prms["@PageSize"] = 999999;
            }
            else
            {
                this.m_Prms["@CurrentPage"] = this.CurrentPage;
                this.m_Prms["@PageSize"] = this.PageSize;
            }

            if (this.event_click_export != null)
            {
                this.event_click_export(DataReserve.GetRDBSummary(this.m_Prms), this.ZID);
            }
            else
            {

                using (ExcelPackage pck = new ExcelPackage())
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Reserve Summary");

                    List<RDBSummary> li = DataReserve.GetRDBSummary(this.m_Prms);


                    string[,] liHeaders = new string[12, 3]
                    {
                        {"MerchantID", "MerchantID", "string"},
                        {"ZID", "ZID", "int"},
                        {"DBAName", "DBAName", "string"},
                        {"Divert_Deposit", "Divert-Deposit", "currency"},
                        {"Divert_Release", "Divert-Release", "currency"},
                        {"Divert_ToReserve", "Divert-ToReserve", "currency"},
                        {"Divert_Net", "Divert-Net", "currency"},
                        {"Reserve_Deposit", "Reserve-Deposit", "currency"},
                        {"Reserve_Release", "Reserve-Release", "currency"},
                        {"Reserve_FromDivert", "Reserve-FromDivert", "currency"},
                        {"Reserve_Net", "Reserve-Net", "currency"},
                        {"Balance", "Balance", "currency"},
                        
                    };

                    DataTable dt = GridViewExportUtil.GetExportableDataTable<RDBSummary>(li, liHeaders);

                    GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                    ws.Cells.LoadFromDataTable(dt, true);

                    string filename = "";

                    if (this.ZID > 0)
                    {
                        filename = String.Format("RDBReserveSummary_ZID-{0}_{1}.xlsx", this.ZID.ToString(), CommonUtility.Util.GetDateTimeStamp());
                    }
                    else
                    {
                        filename = String.Format("RDBReserveSummary_{0}.xlsx", CommonUtility.Util.GetDateTimeStamp());
                    }

                    //Write it back to the client
                    Response.Clear();   // necessary!!!
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
                    Response.BinaryWrite(pck.GetAsByteArray());
                    Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!

                }
            }

        
            }




        // reference? http://stackoverflow.com/questions/12085767/write-excel-using-excel-package-with-formatting



    }
}