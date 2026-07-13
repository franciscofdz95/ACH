using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects.Reserve;
using System.Collections;
using PaymentXP.BusinessObjects;
using System.Data;
using Infragistics.Web.UI.EditorControls;
using System.Drawing;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucDivertDialog : wucBaseDataEntry
    {
        public delegate void EventClickSaveSuccess(int zid);
        public event EventClickSaveSuccess event_click_savesuccess;

        private const int index_Amount = 3;
        private const int index_BatchWithHeld = 4;
        private const int index_Reserve = 5;
        private const int index_Divert = 6;
        private const int index_MeritusOps = 7;
        private const int index_Merchant = 8;
        private const int index_RowTotal = 9;

        private decimal m_Amount = 0.00M;

        private decimal m_BatchWithHeld = 0.00M;

        private decimal m_Reserve = 0.00M;
        private decimal m_DivertClear = 0.00M;
        private decimal m_Merchant = 0.00M;
        private decimal m_DivertReject = 0.00M;

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

        //public int DivertID
        //{
        //    get
        //    {
        //        if (ViewState["DivertID"] == null)
        //        {
        //            return 0;
        //        }
        //        else
        //        {
        //            return (int)ViewState["DivertID"];
        //        }
        //    }
        //    set { ViewState["DivertID"] = value; }
        //}

        public DateTime DivertDate
        {
            get
            {
                if (ViewState["DivertDate"] == null)
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return (DateTime)ViewState["DivertDate"];
                }
            }
            set { ViewState["DivertDate"] = value; }
        }

        public WebDialogWindow WinInstance
        {
            get { return dlg; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {


                if (this.ZID > 0)
                {
                    this.FormShow("");
                }
            }
        }

        public override void FormShow(string ID)
        {

            Hashtable prms = new Hashtable();


            this.Adding = false;

            prms = new Hashtable();
            prms.Add("@ZID", this.ZID);
            prms.Add("@ViewPendingRecords", 1);
            prms.Add("@QAReportDate", this.DivertDate);

            ReportDate.Text = this.DivertDate.ToShortDateString();


            List<RDBDivert> li = DataReserve.GetRDBDivert(prms);

            grdDivertDetails.DataSource = li;
            grdDivertDetails.DataBind();

            if (li != null && li.Count > 0 && this.ZID > 0)
            {

                lblMID.Text = li[0].SettlePlatformMID;
                lblZID.Text = this.ZID.ToString();
                lblDBA.Text = li[0].BusinessDBAName;

                prms = new Hashtable();
                prms.Add("@ZID", this.ZID);
                prms.Add("@PageSize", 1);
                prms.Add("@CurrentPage", 1);
                prms.Add("@SortDirection", 1);
                prms.Add("@LastDate", this.DivertDate);

                List<RDBSummary> liSum = DataReserve.GetRDBSummary(prms);

                // NOTE: because we supply a divert date, the rowcount might be zero.
                if (liSum != null && liSum.Count > 0)
                {
                    lblDivertAmount.Text = String.Format("{0:0.00}", liSum[0].Divert_Net);
                    lblReserveAmount.Text = String.Format("{0:0.00}", liSum[0].Reserve_Net);
                }

                this.UpdateGridTotals();
            }



        }

        public override void FormClear()
        {
            FormHandler.ClearAllControls(pnlDetails);
            this.ZID = 0;
            this.DivertDate = DateTime.MinValue;

        }

        public override bool FormSave()
        {

            bool ret = false;

            // update grid also performs a datacheck.
            if (this.UpdateGridTotals() && this.Page.IsValid)
            {

                foreach (GridViewRow gvr in grdDivertDetails.Rows)
                {
                    //grdMD050.DataKeys[msgRow.RowIndex].Value.ToString()

                    int divert_id = CommonUtility.Util.if_i(grdDivertDetails.DataKeys[gvr.RowIndex].Value, 0);

                    RDBDivert item = DataReserve.GetRDBDivert(divert_id);

                    WebNumericEditor wceReserve = (WebNumericEditor)gvr.FindControl("Reserve");
                    WebNumericEditor wceDivertClear = (WebNumericEditor)gvr.FindControl("DivertClear");
                    WebNumericEditor wcePostMerchant = (WebNumericEditor)gvr.FindControl("PostMerchant");
                    WebNumericEditor wceDivertReject = (WebNumericEditor)gvr.FindControl("DivertReject");

                    item.Reserve = Convert.ToDecimal(wceReserve.Value);
                    item.DivertClear = Convert.ToDecimal(wceDivertClear.Value);
                    item.PostMerchant = Convert.ToDecimal(wcePostMerchant.Value);
                    item.DivertReject = Convert.ToDecimal(wceDivertReject.Value);

                    ret = true;

                    DataReserve.UpdateRDBDivert(item);

                    //if (DataReserve.UpdateRDBDivert(item) > 0)
                    //{
                    //    //// insert into journal
                    //    //DataReserve.InsertRDBJournalDivertTrans(item);

                    //    //// post into journal.
                    //    //item.PostedDate = DateTime.Now;
                    //    //DataReserve.InsertRDBJournalDivertTrans(item);
                    //}
                }
            }

            return ret;

            //bool ret = false;

            //if (this.ZID > 0 && this.FormDataCheck() && Page.IsValid)
            //{
            //    RDBDivert objRM = null;

            //    if (this.DivertID > 0)
            //    {
            //        // editing an existing
            //        objRM = DataReserve.GetRDBDivert(this.DivertID);

            //        objRM.Clone();

            //        FormBinding.BindControlsToObject(objRM, pnlDetails);

            //        if (DataReserve.UpdateRDBDivert(objRM) > 0)
            //        {

            //            this.UpdateChangelog(objRM);

            //            ret = true;
            //        }
            //    }
            //    else
            //    {
            //        // adding a new one
            //        objRM = new RDBDivert();

            //        objRM.ZID = this.ZID;

            //        FormBinding.BindControlsToObject(objRM, pnlDetails);

            //        if (DataReserve.InsertRDBDivert(objRM) > 0)
            //        {
            //            ret = true;
            //        }
            //    }
            //}

            //return ret;
        }

        //protected void value_TextChanged(object sender, EventArgs e)
        //{
        //    this.UpdateGridTotals();
        //}

        private bool UpdateGridTotals()
        {
            bool ret = true;

            decimal d_total_TotalAmount = 0;
            decimal d_total_BatchWithHeld = 0;
            decimal d_total_Reserve = 0;
            decimal d_total_DivertClear = 0;
            decimal d_total_DivertReject = 0;
            decimal d_total_PostMerchant = 0;
            decimal d_total_Total = 0;



            foreach (GridViewRow gvr in grdDivertDetails.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    decimal d_Amount = CommonUtility.Util.ConvertCurrencyToDecimal(((Label)gvr.FindControl("Amount")).Text);
                    decimal d_BatchWithHeld = CommonUtility.Util.ConvertCurrencyToDecimal(((Label)gvr.FindControl("BatchWithHeldAmount")).Text);
                    decimal d_Reserve = Convert.ToDecimal(((WebNumericEditor)gvr.FindControl("Reserve")).Value);
                    decimal d_DivertClear = Convert.ToDecimal(((WebNumericEditor)gvr.FindControl("DivertClear")).Value);
                    decimal d_DivertReject = Convert.ToDecimal(((WebNumericEditor)gvr.FindControl("DivertReject")).Value);
                    decimal d_PostMerchant = Convert.ToDecimal(((WebNumericEditor)gvr.FindControl("PostMerchant")).Value);

                    decimal d_Positive = CommonUtility.Util.ConvertCurrencyToDecimal(((Label)gvr.FindControl("lblPositive")).Text);
                    decimal d_Negative = CommonUtility.Util.ConvertCurrencyToDecimal(((Label)gvr.FindControl("lblNegative")).Text);

                    decimal d_original_row_amount = CommonUtility.Util.ConvertCurrencyToDecimal(((Label)gvr.FindControl("Amount")).Text);

                    d_total_BatchWithHeld += d_BatchWithHeld;
                    d_total_Reserve += d_Reserve;
                    d_total_DivertClear += d_DivertClear;
                    d_total_DivertReject += d_DivertReject;
                    d_total_PostMerchant += d_PostMerchant;
                    d_total_TotalAmount += d_original_row_amount;


                    d_total_Total += d_BatchWithHeld + d_Reserve + d_DivertClear + d_DivertReject + d_PostMerchant;



                    Label lbSum = (Label)gvr.FindControl("lblSum");

                    lbSum.Text = string.Format("{0:0.00}", d_Amount - (d_BatchWithHeld + d_Reserve + d_DivertClear + d_DivertReject + d_PostMerchant));

                    if (cvCustomRow.IsValid == true)
                    {
                        // check 1
                        if (d_BatchWithHeld + d_Reserve + d_DivertClear + d_DivertReject + d_PostMerchant != d_original_row_amount)
                        {
                            cvCustomRow.ErrorMessage = string.Format("Row {0} must sum up to equal {1:0.00}", Convert.ToString((gvr.RowIndex + 1)), d_original_row_amount);
                            cvCustomRow.IsValid = false;
                            cvCustomRow.BackColor = ColorTranslator.FromHtml("#fcc");
                            ret = false;
                        }
                        else
                        {
                            cvCustomRow.ErrorMessage = "";
                            cvCustomRow.IsValid = true;

                            if (gvr.RowState == DataControlRowState.Normal)
                            {
                                cvCustomRow.BackColor = System.Drawing.Color.White;
                            }
                            else
                            {
                                cvCustomRow.BackColor = ColorTranslator.FromHtml("#c1c1c1");
                            }

                            ret = true;

                        }
                    }


                    // check 4. Maintain record integrity; ensure users (analysts) cannot allocate more than came in, even though the allocations may net out to a correct amount
                    // applies only to Woodforest and NCAL. because for wells, Meritus and Merchant are locked.
                    // strategy:
                    /*
                     * 
                     * if zero column count == 4
                     *      all columns have a zero in them. this is okay sometimes because its all sent to reserve.
                     *      
                     * if zero column count == 3 
                     *      sum of all 4 columns must equal amount. (this just means only 1 amount allocated.)
                     *     
                     * if zero column count == 2
                     *      sum of all 4 columns must equal amount and
                     *      sum of absolute values of columns must equal sum of absolute value of positive and negative.
                     *      
                     * if zero column count == 1
                     *      throw error, at max we can only allocate 2 fields. this is just to make it easy and manageable. do a manual transfer if you need.
                     *      
                     * if zero column count == 0
                     *      throw error, at max we can only allocate 2 fields. this is just to make it easy and manageable. do a manual transfer if you need.
                     * */

                    //// TOL: commented out to overcome divert bug. 

                    //int zero_count = 0;

                    //zero_count += (d_Reserve == 0m) ? 1 : 0;
                    //zero_count += (d_DivertClear == 0m) ? 1 : 0;
                    //zero_count += (d_DivertReject == 0m) ? 1 : 0;
                    //zero_count += (d_PostMerchant == 0m) ? 1 : 0;


                    //if (zero_count == 4)
                    //{
                    //    // all zero's, all good.
                    //}
                    //else if (zero_count == 3)
                    //{
                    //    decimal mysum = d_Reserve + d_DivertClear + d_DivertReject + d_PostMerchant + d_BatchWithHeld;

                    //    if (mysum != d_Amount)
                    //    {
                    //        cvCustomRow.ErrorMessage = string.Format("Allocation + Sent to Rsrv must equal Amount");
                    //        cvCustomRow.IsValid = false;
                    //        cvCustomRow.BackColor = ColorTranslator.FromHtml("#fcc");
                    //        ret = false;
                    //    }
                    //}
                    //else if (zero_count == 2)
                    //{
                    //    decimal mysum = d_Reserve + d_DivertClear + d_DivertReject + d_PostMerchant + d_BatchWithHeld;

                    //    if (mysum != d_Amount)
                    //    {
                    //        cvCustomRow.ErrorMessage = string.Format("Allocation + Sent to Rsrv must equal Amount");
                    //        cvCustomRow.IsValid = false;
                    //        cvCustomRow.BackColor = ColorTranslator.FromHtml("#fcc");

                    //    }
                    //    else
                    //    {
                    //        // mysum and d_Amount are equal, so it passes the first check.
                    //        // lets do the absolute value test.

                    //        decimal sum_left = Math.Abs(d_BatchWithHeld) +
                    //                            Math.Abs(d_Reserve) +
                    //                            Math.Abs(d_DivertClear) +
                    //                            Math.Abs(d_DivertReject) +
                    //                            Math.Abs(d_PostMerchant);

                    //        decimal sum_right = Math.Abs(d_Positive) +
                    //                            Math.Abs(d_Negative);

                    //        if (sum_left != sum_right)
                    //        {
                    //            cvCustomRow.ErrorMessage = string.Format("Sum of Absolute Values Error!");
                    //            cvCustomRow.IsValid = false;
                    //            cvCustomRow.BackColor = ColorTranslator.FromHtml("#fcc");
                    //            ret = false;
                    //        }


                    //    }
                    //}
                    //else if (zero_count == 1 || zero_count == 0)
                    //{
                    //    cvCustomRow.ErrorMessage = string.Format("You can only allocate at max 2 columns");
                    //    cvCustomRow.IsValid = false;
                    //    cvCustomRow.BackColor = ColorTranslator.FromHtml("#fcc");
                    //    ret = false;
                    //}




                }
            }

            grdDivertDetails.FooterRow.Cells[index_Reserve].Text = d_total_Reserve.ToString("0.00");
            grdDivertDetails.FooterRow.Cells[index_Divert].Text = d_total_DivertClear.ToString("0.00");
            grdDivertDetails.FooterRow.Cells[index_MeritusOps].Text = d_total_DivertReject.ToString("0.00");
            grdDivertDetails.FooterRow.Cells[index_Merchant].Text = d_total_PostMerchant.ToString("0.00");
            grdDivertDetails.FooterRow.Cells[index_RowTotal].Text = (d_total_TotalAmount - (d_total_BatchWithHeld + d_total_Reserve + d_total_DivertClear + d_total_DivertReject + d_total_PostMerchant)).ToString("0.00");

            afterDivert.Text = String.Format("{0:0.00}", d_total_DivertClear + CommonUtility.Util.ConvertCurrencyToDecimal(lblDivertAmount.Text));
            afterReserve.Text = string.Format("{0:0.00}", d_total_Reserve + CommonUtility.Util.ConvertCurrencyToDecimal(lblReserveAmount.Text) + d_total_BatchWithHeld);


            // check 2

            decimal new_divertclear = CommonUtility.Util.ConvertCurrencyToDecimal(grdDivertDetails.FooterRow.Cells[index_Divert].Text);
            decimal old_divertclear = CommonUtility.Util.ConvertCurrencyToDecimal(lblDivertAmount.Text);
            //row sum check
            // original divert amount + new divert amount >= 0

            if (cvCustomDivertZero.IsValid == true)
            {
                if (new_divertclear + old_divertclear < 0)
                {
                    cvCustomDivertZero.IsValid = false;
                    cvCustomDivertZero.ErrorMessage = "Sum of Divert Clear + original Divert Amount must never be less than 0.00";
                    ret = false;
                }
                else
                {
                    cvCustomDivertZero.IsValid = true;
                    cvCustomDivertZero.ErrorMessage = "";
                    ret = true;
                }
            }

            // check 3. basically, you don't want to perform allocations that will result into a negative reserve balance.
            // but there is a special case, if it starts out as negative (which can happen when we import data), then bypass this check.
            decimal old_reserveclear = CommonUtility.Util.ConvertCurrencyToDecimal(lblReserveAmount.Text);

            if (old_reserveclear >= 0)
            {
                decimal new_reserveclear = CommonUtility.Util.ConvertCurrencyToDecimal(grdDivertDetails.FooterRow.Cells[index_Reserve].Text);

                if (cvCustomReserveZero.IsValid == true)
                {
                    if (new_reserveclear + old_reserveclear < 0)
                    {
                        cvCustomReserveZero.IsValid = false;
                        cvCustomReserveZero.ErrorMessage = "Sum of reserve + original reserve Amount must never be less than 0.00";
                        ret = false;
                    }
                    else
                    {
                        cvCustomReserveZero.IsValid = true;
                        cvCustomReserveZero.ErrorMessage = "";
                        ret = true;
                    }
                }
            }





            return ret;
        }

        public override void FormNew()
        {
            this.FormClear();
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            //int count = 0;

            //foreach (string str in this.ValidateForm())
            //{
            //    count++;
            //}

            //// no errors, so all good.
            //return (count == 0);

            return true;
        }

        //protected IEnumerable<string> ValidateForm()
        //{

        //    // row match




        //}

        public override void FormCancel()
        {

            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.FormSave())
            {
                if (event_click_savesuccess != null)
                {
                    event_click_savesuccess(this.ZID);
                }

                dlg.WindowState = DialogWindowState.Hidden;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlg.WindowState = DialogWindowState.Hidden;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            int zid = this.ZID;
            DateTime diverteddate = this.DivertDate;
            this.FormClear();

            this.ZID = zid;
            this.DivertDate = diverteddate;
            this.FormShow("");
        }


        protected void grdDivertDetails_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                RDBDivert obj = (RDBDivert)e.Row.DataItem;

                decimal d_amount = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Amount"));
                decimal d_batchwithheld = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "BatchWithHeldAmount"));
                decimal d_reserve = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Reserve"));
                decimal d_divertclear = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DivertClear"));
                decimal d_divertreject = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "DivertReject"));
                decimal d_merchant = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "PostMerchant"));


                this.m_Amount += d_amount;
                this.m_BatchWithHeld += d_batchwithheld;
                this.m_Reserve += d_reserve;
                this.m_DivertClear += d_divertclear;
                this.m_DivertReject += d_divertreject;
                this.m_Merchant += d_merchant;

                int zid = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "ZID"), 0);

                Label lbSum = (Label)e.Row.FindControl("lblSum");
                lbSum.Text = string.Format("{0:0.00}", d_amount - (d_batchwithheld + d_reserve + d_divertclear + d_divertreject + d_merchant));

                Label lbAmount = (Label)e.Row.FindControl("Amount");
                lbAmount.Text = String.Format("{0:0.00}", d_amount);

                Label lbBatchWithHeld = (Label)e.Row.FindControl("BatchWithHeldAmount");
                lbBatchWithHeld.Text = String.Format("{0:0.00}", d_batchwithheld);



                if (obj.BankID == eRDBBank.Wells)
                {
                    WebNumericEditor wceMeritus = (WebNumericEditor)e.Row.FindControl("DivertReject");
                    wceMeritus.ReadOnly = true;

                    WebNumericEditor wceMerchant = (WebNumericEditor)e.Row.FindControl("PostMerchant");
                    wceMerchant.ReadOnly = true;
                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {



                e.Row.Cells[1].Text = "Totals:";

                e.Row.Cells[index_Amount].Text = this.m_Amount.ToString("0.00");
                e.Row.Cells[index_Amount].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[index_BatchWithHeld].Text = this.m_BatchWithHeld.ToString("0.00");
                e.Row.Cells[index_BatchWithHeld].HorizontalAlign = HorizontalAlign.Right;

                e.Row.Cells[index_Reserve].Text = this.m_Reserve.ToString("0.00");
                e.Row.Cells[index_Reserve].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[index_Reserve].BackColor = System.Drawing.Color.LemonChiffon;

                e.Row.Cells[index_Divert].Text = this.m_DivertClear.ToString("0.00");
                e.Row.Cells[index_Divert].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[index_Divert].BackColor = System.Drawing.Color.LemonChiffon;

                e.Row.Cells[index_MeritusOps].Text = this.m_DivertReject.ToString("0.00");
                e.Row.Cells[index_MeritusOps].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[index_MeritusOps].BackColor = System.Drawing.Color.LemonChiffon;

                e.Row.Cells[index_Merchant].Text = this.m_Merchant.ToString("0.00");
                e.Row.Cells[index_Merchant].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[index_Merchant].BackColor = System.Drawing.Color.LemonChiffon;

                e.Row.Cells[index_RowTotal].Text = string.Format("{0:0.00}", this.m_Amount - (this.m_BatchWithHeld + this.m_Reserve + this.m_DivertClear + this.m_DivertReject + this.m_Merchant));
                e.Row.Cells[index_RowTotal].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[index_RowTotal].BackColor = System.Drawing.Color.LemonChiffon;




                Hashtable prms = new Hashtable();
                prms = new Hashtable();
                prms.Add("@ZID", this.ZID);
                prms.Add("@PageSize", 1);
                prms.Add("@CurrentPage", 1);
                prms.Add("@SortDirection", 1);
                prms.Add("@LastDate", this.DivertDate);

                // note: this gets called again in formshow(). this one gets called first. but it's cached so it's okay.
                List<RDBSummary> liSum = DataReserve.GetRDBSummary(prms);

                if (liSum != null && liSum.Count > 0)
                {
                    afterDivert.Text = String.Format("{0:0.00}", this.m_DivertClear + liSum[0].Divert_Net);
                    afterReserve.Text = string.Format("{0:0.00}", this.m_BatchWithHeld + this.m_Reserve + liSum[0].Reserve_Net);
                }


            }
        }

        protected void btnReCalcuate_Click(object sender, EventArgs e)
        {
            this.UpdateGridTotals();
        }
    }
}