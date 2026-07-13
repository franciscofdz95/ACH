using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucReservePercentDialog : System.Web.UI.UserControl
    {
        public delegate void EventClickSaveSuccess();
        public event EventClickSaveSuccess event_click_savesuccess;

        public WebDialogWindow WinInstance
        {
            get { return dlgReservePercent; }
        }

        public int ZID
        {
            get { return (int)(ViewState["ZID"] ?? 0); }
            set { ViewState["ZID"] = value; }
        }

        public DateTime ReportDate
        {
            get { return (DateTime)(ViewState["ReportDate"] ?? DateTime.MinValue); }
            set { ViewState["ReportDate"] = value; }
        }

        public int ReserveID
        {
            get { return (int)(ViewState["ReserveID"] ?? 0); }
            set { ViewState["ReserveID"] = value; }
        }

        private string MID
        {
            get { return (string)(ViewState["MID"] ?? ""); }
            set { ViewState["MID"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void FormShow()
        {
            MerchantApp ma = DataMerchantApp.GetInstance().GetMerchantApp(this.ZID);

            RDBReserve objRes = DataReserve.GetRDBReserve(this.ReserveID, this.ZID);

            lblCurrent.Text = string.Format("{0:P2}", ma.ReservePercent / 100);
            lblZID.Text = ma.ID.ToString();
            lblReportDate.Text = this.ReportDate.ToShortDateString();

            this.MID = ma.SettlePlatformMid;

            wpeReservePercent.Value = objRes.ReservePct * 100;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.ZID > 0 && !string.IsNullOrWhiteSpace(this.MID) && this.ReportDate != DateTime.MinValue)
            {
                //Decimal decNewPerc = Convert.ToDecimal(wpeReservePercent.Value) / 100;

                Decimal decNewPerc = Convert.ToDecimal(wpeReservePercent.Value);

                DataReserve.UpdateRDBReserveAllocationManual(this.ReserveID, this.ZID, this.ReportDate, decNewPerc);

                this.dlgReservePercent.WindowState = DialogWindowState.Hidden;

                if (event_click_savesuccess != null)
                {
                    event_click_savesuccess();
                }
            }

        }
    }
}