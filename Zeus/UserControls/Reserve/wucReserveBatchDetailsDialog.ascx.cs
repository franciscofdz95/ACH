using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using System.Collections;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucReserveBatchDetailsDialog : System.Web.UI.UserControl
    {


        public decimal RunningTotal
        {
            get { return (decimal)(ViewState["RunningTotal"] ?? 0); }
            set { ViewState["RunningTotal"] = value; }
        }

        public decimal RunningSales
        {
            get { return (decimal)(ViewState["RunningSales"] ?? 0); }
            set { ViewState["RunningSales"] = value; }
        }

        public decimal RunningReturns
        {
            get { return (decimal)(ViewState["RunningReturns"] ?? 0); }
            set { ViewState["RunningReturns"] = value; }
        }

        public int ZID { get; set; }
        public string MID { get; set; }
        public string DBAName { get; set; }

        private DateTime _ReportDate;

        public DateTime ReportDate
        {
            get { return _ReportDate; }
            set { _ReportDate = value; }
        }

        public WebDialogWindow WinInstance
        {
            get { return dlgReserveBatchDetails; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.RunningTotal = 0;
                this.RunningReturns = 0;
                this.RunningSales = 0;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbAmount = (Label)e.Row.FindControl("Label1");
                this.RunningTotal += CommonUtility.Util.ConvertCurrencyToDecimal(lbAmount.Text);

                Label lbSales = (Label)e.Row.FindControl("lblSales");
                this.RunningSales += CommonUtility.Util.ConvertCurrencyToDecimal(lbSales.Text);

                Label lbReturns = (Label)e.Row.FindControl("lblReturns");
                this.RunningReturns += CommonUtility.Util.ConvertCurrencyToDecimal(lbReturns.Text);

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbFoot = (Label)e.Row.FindControl("lblFootAmountTotal");
                lbFoot.Text = string.Format("{0:C2}", this.RunningTotal);

                Label lbFootSales = (Label)e.Row.FindControl("lblFootSalesTotal");
                lbFootSales.Text = string.Format("{0:C2}", this.RunningSales);

                Label lbFootReturns = (Label)e.Row.FindControl("lblFootReturnsTotal");
                lbFootReturns.Text = string.Format("{0:C2}", this.RunningReturns);
            }


        }

        public void FormShow(string ID)
        {
            if (this.ZID > 0 && this.ReportDate != DateTime.MinValue)
            {
                Hashtable prms = new Hashtable();
                prms.Add("@ZID", this.ZID);
                prms.Add("@ReportDate", this.ReportDate);

                GridView1.DataSource = DataReserve.GetBatchDetails(prms);
                GridView1.DataBind();

                MerchantApp ma = DataMerchantApp.GetInstance().GetMerchantApp(this.ZID);

                lblZID.Text = this.ZID.ToString();

                if (ma != null)
                {
                    lblMID.Text = ma.SettlePlatformMid;
                    lblDBA.Text = ma.BusinessDBAName;
                }

            }
        }
    }
}