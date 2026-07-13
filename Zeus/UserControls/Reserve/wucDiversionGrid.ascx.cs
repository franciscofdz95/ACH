using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucDiversionGrid : wucBaseSearch
    {
        public delegate void EventClickReportDate(int diversionid);
        public event EventClickReportDate event_click_reportdate;

        public delegate void EventClickUnDivertedSuccess(int zid, int diversionid);
        public event EventClickUnDivertedSuccess event_click_undivertedsuccess;

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
            if (!Page.IsPostBack)
            {
                this.LoadDiverted(this.ZID);
            }
        }

        public void LoadDiverted(int zid)
        {
            if (zid > 0)
            {
                Hashtable prms = new Hashtable();
                prms.Add("@ZID", zid);

                List<RDBDiversion> li = DataReserve.GetReserveDiversion(prms);

                grdDivertMethod.DataSource = li;
                grdDivertMethod.DataBind();
            }
        }

        protected void grdDivertMethod_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:

                    RDBDiversion obj = (RDBDiversion)e.Row.DataItem;

                    LinkButton lnkDateDiverted = (LinkButton)e.Row.FindControl("lnkDateDiverted");
                    lnkDateDiverted.Text = WebUtil.ConvertToUserShortDateTimeFormat(DataBinder.Eval(e.Row.DataItem, "DateDiverted").ToString());
                    //lnkZID.CommandArgument = DataBinder.Eval(e.Row.DataItem, "ZID").ToString

                    string dt = WebUtil.ConvertToUserShortDateTimeFormat(DataBinder.Eval(e.Row.DataItem, "DateUnDiverted").ToString());
                    e.Row.Cells[1].Text = (Convert.ToDateTime(dt).Date != DateTime.MinValue) ? dt : "";

                    Label lbRR = (Label)e.Row.FindControl("lblReserveRate");
                    lbRR.Text = (obj.DiversionTypeID == (int)eRDBDiversionTypeID.ReservePlan) ?  String.Format("{0:P2}", obj.ReserveRate):"N/A" ;


                    break;

                case DataControlRowType.Footer:

                    break;

                default:
                    break;
            }
        }

        protected void grdDivertMethod_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource is LinkButton)
            {

                LinkButton lb = (LinkButton)e.CommandSource;

                switch (lb.CommandName)
                {
                    case "ReportDate":

                        int diversion_id = Convert.ToInt32(lb.CommandArgument);

                        if (event_click_reportdate != null)
                        {
                            event_click_reportdate(diversion_id);
                        }

                        break;
                }
            }
        }

        public void BindGrid()
        {
            if (this.ZID > 0)
            {
                this.LoadDiverted(this.ZID);
            }
        }

        //protected void btnRemove_Click(object sender, EventArgs e)
        //{
        //    Button b = (Button)sender;

        //    if (b != null && b.CommandName.ToLower().Trim() == "removediversion")
        //    {
        //        int diversion_id = Convert.ToInt32(b.CommandArgument);

        //        Hashtable prms = new Hashtable();
        //        prms.Add("@DiversionID", diversion_id);

        //        List<RDBDiversion> li = DataReserve.GetReserveDiversion(prms);

        //        if (li != null && li.Count == 1)
        //        {
        //            RDBDiversion obj = li[0];
        //            obj.DateUndiverted = DateTime.Now;
        //            int result = DataReserve.UpdateReserveDiversion(obj);

        //            if (event_click_undivertedsuccess != null)
        //            {
        //                event_click_undivertedsuccess(this.ZID, obj.DiversionID);
        //            }
        //        }

        //    }
        //}

    }
}