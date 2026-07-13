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
    public partial class wucManualDialog : wucBaseDataEntry
    {
        public delegate void EventClickSaveSuccess(int zid);
        public event EventClickSaveSuccess event_click_savesuccess;

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
            get { return dlgManual; }
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
            LookupTableHandler.LoadRDBBank(BankID, false, this.ZID);
            LookupTableHandler.LoadRDBTransactionMethod(MethodID, false, PaymentXP.BusinessObjects.eRDBTransactionMethodType.Manual);
            LookupTableHandler.LoadRDBReserveType(ReserveTypeID, false);

            if (this.ReserveID > 0)
            {

                // edit mode
                this.Adding = false;

                RDBReserve objRM = DataReserve.GetRDBReserve(this.ReserveID, this.ZID);

                if (objRM != null)
                {
                    FormBinding.BindObjectToControls(objRM, pnlDetails);

                    if (objRM.JournalIDDivert > 0 || objRM.JournalIDReserve > 0)
                    {
                        btnSave.Enabled = false;
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

                // adding a new one
                objRM = new RDBReserve();

                FormBinding.BindControlsToObject(objRM, pnlDetails);

                // if ACH Debit, then it's going to be a negative amount.
                if (TransactionType.SelectedValue == "1") // ACH Debit = 5
                {
                    objRM.Amount = objRM.Amount * -1m;
                }

                objRM.ZID = this.ZID;
                objRM.ReserveSourceID = eRDBReserveSourceID.ManualReserve;
                
                if (objRM.ReserveTypeID == eRDBReserveType.Reserve)
                {
                    objRM.Reserve = objRM.Amount; // this must be added, otherwise it won't insert into the journal.
                }
                else if (objRM.ReserveTypeID == eRDBReserveType.Divert)
                {
                    objRM.Divert = objRM.Amount;
                }

                if (DataReserve.InsertRDBReserve(objRM) > 0)
                {
                    // insert into journal
                    DataReserve.InsertRDBJournalReserveTrans(objRM.ReserveID, objRM.ZID, CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0), DateTime.MinValue);

                    // post journal entry
                    DataReserve.InsertRDBJournalReserveTrans(objRM.ReserveID, objRM.ZID, CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0), DateTime.Now);
                }

                ret = true;
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

            if (ReportDate.Text.Trim() == "")
            {
                cvDate.IsValid = false;
                yield return cvDate.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_dec(Amount.Value, 0) > 0))
            {
                cvAmount.IsValid = false;
                yield return cvAmount.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(ReserveTypeID.SelectedValue, 0) > 0))
            {
                cvReserveTypeID.IsValid = false;
                yield return cvReserveTypeID.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(BankID.SelectedValue, 0) > 0))
            {
                cvBankID.IsValid = false;
                yield return cvBankID.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(MethodID.SelectedValue, 0) > 0))
            {
                cvMethodID.IsValid = false;
                yield return cvMethodID.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(TransactionType.SelectedValue, 0) > 0))
            {
                cvTransactionType.IsValid = false;
                yield return cvTransactionType.ErrorMessage;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.FormSave())
            {
                if (event_click_savesuccess != null)
                {
                    event_click_savesuccess(this.ZID);
                }

                dlgManual.WindowState = DialogWindowState.Hidden;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlgManual.WindowState = DialogWindowState.Hidden;
        }


        protected void MethodID_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get previous note
            string previous_note = "";
            string[] arr = Notes.Text.Split(new char[] { ':' });
            if (arr.Length > 1)
            {
                previous_note = Notes.Text.Substring(arr[0].Length + 1).Trim(); // +1 for the colon.
            }

            // popuplate note field by combining method with previous note
            DropDownList ddl = (DropDownList)sender;
            if (!(ddl.Text == "" || ddl.Text == "-1"))
            {
                eRDBTransactionMethod eTM = (eRDBTransactionMethod)Convert.ToInt32(ddl.SelectedValue);
                Notes.Text = string.Format("{0}: {1} ", eTM.ToString(), previous_note);
            }
        }


        protected void ReserveTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////am i handling notes for reserve types?
            //this.HandleBankNotes();
        }
    }
}