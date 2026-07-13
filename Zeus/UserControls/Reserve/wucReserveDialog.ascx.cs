using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.BusinessObjects.Reserve;
using PaymentXP.DataObjects;
using System.Collections;
using System.Data;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucReserveDialog : wucBaseDataEntry
    {
        public delegate void EventClickSaveSuccess(int zid);
        public event EventClickSaveSuccess event_click_savesuccess;

        public decimal RunningTotal
        {
            get
            {

                if (ViewState["RunningTotal"] == null)
                {
                    return 0;
                }
                else
                {
                    return (decimal)ViewState["RunningTotal"];
                }
            }
            set { ViewState["RunningTotal"] = value; }
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

        public int ReserveID
        {
            get
            {
                if (ViewState["ReserveID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["ReserveID"];
                }
            }
            set { ViewState["ReserveID"] = value; }
        }

        public WebDialogWindow WinInstance
        {
            get { return dlgReserve; }
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.RunningTotal = 0;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbAmount = (Label)e.Row.FindControl("Label1");
                this.RunningTotal += CommonUtility.Util.ConvertCurrencyToDecimal(lbAmount.Text);
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbFoot = (Label)e.Row.FindControl("lblFootAmountTotal");
                lbFoot.Text = string.Format("{0:C2}", this.RunningTotal);
            }


        }

   
        public override void FormShow(string ID)
        {
            if (this.ReserveID > 0)
            {
                // edit mode
                this.Adding = false;


                RDBReserve objRM = DataReserve.GetRDBReserve(this.ReserveID, this.ZID);

                if (objRM != null)
                {
                    FormBinding.BindObjectToControls(objRM, pnlDetails);

                    Amount.Text = String.Format("{0:C2}", CommonUtility.Util.if_dec(Amount.Text, 0));

                    if (objRM.PostedDate != DateTime.MinValue)
                    {
                        btnSave.Enabled = false;
                    }

                    if (this.ZID > 0 && objRM.ReportDate != DateTime.MinValue)
                    {
                        Hashtable prms = new Hashtable();
                        prms.Add("@ZID", this.ZID);
                        prms.Add("@ReportDate", objRM.ReportDate);

                        GridView1.DataSource = DataReserve.GetBatchDetails(prms);
                        GridView1.DataBind();
                    }
                }

            }


        }

        public override void FormClear()
        {
            FormHandler.ClearAllControls(pnlDetails);

            this.ZID = 0;
            this.ReserveID = 0;
        }


        public override bool FormSave()
        {
            bool ret = false;

            if (this.ZID > 0 && this.FormDataCheck() && Page.IsValid)
            {
                RDBReserve objRM = null;

                if (this.ReserveID > 0)
                {


                    // editing an existing
                    objRM = DataReserve.GetRDBReserve(this.ReserveID, this.ZID);

                    // we never insert, only update


                    FormBinding.BindControlsToObject(objRM, pnlDetails);

                    if (DataReserve.UpdateRDBReserve(objRM) > 0)
                    {
                        //// check this.. are you sure this will work? you don't want to write to the journal. 
                        //// you just want to update the RDBReserve table. 

                        //// call first to update journal.
                        //DataReserve.InsertRDBJournalReserveTrans(objRM.ReserveID, objRM.ZID, Convert.ToInt32(UserSessions.CurrentUser.UserID), DateTime.MinValue);

                        //this.UpdateChangelog(objRM);

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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.FormSave())
            {
                if (event_click_savesuccess != null)
                {
                    event_click_savesuccess(this.ZID);
                }

                dlgReserve.WindowState = DialogWindowState.Hidden;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlgReserve.WindowState = DialogWindowState.Hidden;
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            int count = 0;

            foreach (string str in this.ValidateForm())
            {
                count++;
            }

            // no errors, so all good.
            return (count == 0);
        }
        protected IEnumerable<string> ValidateForm()
        {


            // we don't check the sign of the amount. what you see is what you get in the table from the dialog screen.
            //if (!( CommonUtility.Util.if_dec( Amount.Value, 0) > 0))
            //{
            //    cvAmount.IsValid = false;
            //    yield return cvAmount.ErrorMessage;
            //}


            decimal withheld_amount = CommonUtility.Util.ConvertCurrencyToDecimal(Amount.Text);
            decimal reserve = Convert.ToDecimal(Reserve.Value);
            decimal divert = Convert.ToDecimal(Divert.Value);

            if (withheld_amount != reserve + divert)
            {
                cvAmount.IsValid = false;
                yield return cvAmount.ErrorMessage;
            }

        }
        public override void FormCancel()
        {

            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }


    }
}