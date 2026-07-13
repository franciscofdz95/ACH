using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using OfficeOpenXml;
using System.Data;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucRejectGrid : wucBaseSearch
    {
        
        private bool _ShowMerchantColumns = false;

        public bool ShowMerchantColumns
        {
            get { return _ShowMerchantColumns; }
            set { _ShowMerchantColumns = value; }
        }

        
        public const int COLUMN_ZID = 1;
        
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

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.BindGrid();
            }

            this.PreRender += new EventHandler(wucRejectGrid_PreRender);

            ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnExpExcel);
        }

        public void BindGrid()
        {
            Hashtable prms = new Hashtable();

            if (this.ZID != 0)
            {
                prms.Add("@ZID", this.ZID);
            }

            this.m_Prms = prms;

            List<RDBReject> li = DataReserve.GetRDBReject(this.m_Prms);

            if (li == null)
            {
                li = new List<RDBReject>();
            }

            grdReject.DataSource = li;
            grdReject.DataBind();

        }

        protected void grdReject_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                RDBReject objRR = (RDBReject)e.Row.DataItem;

            }
        }


        protected void grdReject_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {

                
            }

        }

        protected void wucRejectGrid_PreRender(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "temp", "<script type='text/javascript'>force_min();</script>", false);

            grdReject.Columns[COLUMN_ZID].Visible = this.ShowMerchantColumns;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Rejects");

                List<RDBReject> li = DataReserve.GetRDBReject(this.m_Prms);


                string[,] liHeaders = new string[6, 3]
                    {
                        {"ReportDate", "Report Date", "datetime"},
                        {"ZID", "ZID", "int"},
                        {"Amount", "Amount", "currency"},
                        {"Code", "Code", "string"},
                        {"Memo", "Memo", "string"},
                        {"Bank", "Bank", "string"},
                        
                    };

                DataTable dt = GridViewExportUtil.GetExportableDataTable<RDBReject>(li, liHeaders);

                GridViewExportUtil.PrepareWorksheetFromDataTable(ws, dt, liHeaders);

                ws.Cells.LoadFromDataTable(dt, true);

                string filename = String.Format("RDBRejects_ZID-{0}_{1}.xlsx", this.ZID.ToString(), CommonUtility.Util.GetDateTimeStamp());


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
