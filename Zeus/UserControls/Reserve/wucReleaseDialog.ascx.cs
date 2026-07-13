using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.BusinessObjects.Reserve;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.DataObjects;
using System.Collections;
using System.Data;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucReleaseDialog : wucBaseDataEntry
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

        public int ReleaseID
        {
            get
            {
                if (ViewState["ReleaseID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["ReleaseID"];
                }
            }
            set { ViewState["ReleaseID"] = value; }
        }

        public WebDialogWindow WinInstance
        {
            get { return dlgRelease; }
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

            this.PreRender += wucReleaseDialog_PreRender;

        }

        public override void FormShow(string ID)
        {
            LookupTableHandler.LoadRDBTransactionType(TransTypeID, false);
            LookupTableHandler.LoadRDBReserveType(ReserveTypeID, false);
            LookupTableHandler.LoadRDBBank(BankID, false, this.ZID);
            LookupTableHandler.LoadRDBTransactionMethod(MethodID, false, eRDBTransactionMethodType.Release);

            foreach (ListItem li in TransTypeID.Items)
            {
                // hide these. we dont ever want to manually release of this type.
                switch (CommonUtility.Util.if_i(li.Value, 0))
                {
                    case (int)eRDBTransactionType.Government:
                    case (int)eRDBTransactionType.Receiver:
                        li.Enabled = false;
                        break;
                }

                //if (li.Text == "Government" || li.Text == "Receiver")
                //{
                //    li.Enabled = false;
                //}
            }

            gvBankBalance.DataSource = DataReserve.GetBalanceByBank(this.ZID);
            gvBankBalance.DataBind();

            //if there's no balance, then the user cannot issue a release
            if(this.gvBankBalance.Rows.Count == 0)
                this.btnSave.Enabled = false;

            if (this.ReleaseID > 0)
            {
                // edit mode
                this.Adding = false;

                // once a release is entered into the system. it cannot be modified. to modify it, would be to delete it then recreate it.
                FormHandler.SetControlEditMode(pnlDetails, false);
                gvBankBalance.Visible = false;

                btnRemove.Enabled = true;
                btnRemove.Visible = true;

                RDBRelease objRM = DataReserve.GetRDBRelease(this.ReleaseID);

                if (objRM != null)
                {
                    FormBinding.BindObjectToControls(objRM, pnlDetails);


                    Amount.Value = Math.Abs(objRM.Amount);

                    btnSave.Enabled = objRM.JournalID == 0;
                    //btnDelete.Enabled = objRM.JournalID == 0;
                    //btnVoid.Enabled = objRM.AllowVoid();

                    if (objRM.PostedDate != DateTime.MinValue && objRM.AllowVoid())
                    {
                        // its been posted to the journal, so you must void.
                        btnRemove.Text = "Cancel/Void";
                        btnRemove.CommandName = "Cancel/Void";
                        btnRemove.OnClientClick = "return confirm('Are you sure you want to Cancel and Void this transaction?')";
                    }
                    else
                    {
                        // not posted to the journal yet, so you can just delete.
                        btnRemove.Text = "Delete";
                        btnRemove.CommandName = "Delete";
                        btnRemove.OnClientClick = "return confirm('Are you sure you want to Delete this transaction?')";
                    }

                    // display the amount as a positive number in the form. 
                    Amount.Value = Math.Abs(Convert.ToDecimal(Amount.Value));

                    if (objRM.JournalID > 0 && objRM.DateApproved != DateTime.MinValue)
                    {
                        btnSave.Enabled = false;
                        lblApproval.Text = string.Format("Release was approved on {0} by {1}", objRM.DateApproved.ToShortDateString(), objRM.ApprovedBy);

                    }
                    else
                    {
                        //btnSave.Enabled = true;
                        lblApproval.Text = "";

                    }


                }

            }

            //if (this.ZID > 0)
            //{
            //    Hashtable prms = new Hashtable();
            //    prms.Add("@ZID", this.ZID);
            //    prms.Add("@PageSize", 1);
            //    prms.Add("@CurrentPage", 1);
            //    prms.Add("@SortDirection", 1);

            //    DataTable dt = DataReserve.GetRDBSummary(prms);

            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        lblDivertAmount.Text = String.Format("{0:C2}", CommonUtility.Util.if_dec((dt.Rows[0]["Divert Net"]), 0));
            //        lblReserveAmount.Text = String.Format("{0:C2}", CommonUtility.Util.if_dec((dt.Rows[0]["Reserve Net"]), 0));

            //    }


            //}
        }

        public override void FormClear()
        {
            FormHandler.ClearAllControls(pnlDetails);

            this.ZID = 0;
            this.ReleaseID = 0;
        }

        public override bool FormSave()
        {
            bool ret = false;

            if (this.ZID > 0 && this.FormDataCheck() && Page.IsValid)
            {
                RDBRelease objRM = null;

                if (this.ReleaseID > 0)
                {
                    // editing an existing
                    objRM = DataReserve.GetRDBRelease(this.ReleaseID);


                    FormBinding.BindControlsToObject(objRM, pnlDetails);
                    objRM.TransTypeID = (eRDBTransactionType)Convert.ToInt32(TransTypeID.SelectedItem.Value);


                    //// at this point, amount is always positive on the screen.
                    //// we need to change the sign depending on the transtypeid
                    switch (CommonUtility.Util.if_i(TransTypeID.SelectedValue, 0))
                    {
                        case (int)eRDBTransactionType.CollectReserve:
                        case (int)eRDBTransactionType.Void:
                        case (int)eRDBTransactionType.VoidMeritusOps:
                        case (int)eRDBTransactionType.VoidGovernment:
                        case (int)eRDBTransactionType.VoidReceiver:
                            // we dont want to do change the sign
                            break;

                        default:
                            objRM.Amount = objRM.Amount * -1;
                            break;
                    }


                    //// replaced in favor of a refactor
                    //if (!(TransactionTypeID.SelectedValue == "5"  // Collect Reserve
                    //    || TransactionTypeID.SelectedValue == "6" // Void
                    //    || TransactionTypeID.SelectedValue == "7" // VoidMeritusOps
                    //    || TransactionTypeID.SelectedValue == "8" // VoidGovernment
                    //    || TransactionTypeID.SelectedValue == "9" // VoidReceiver
                    //    ))
                    //{
                    //    // anything other than "Collect Reserve" (5) or "Void" (6) we flip the sign.
                    //    objRM.Amount = objRM.Amount * -1;
                    //}

                    DataReserve.UpdateRDBRelease(objRM);
                    
                    ret = true;

                }
                else
                {
                    // adding a new one
                    objRM = new RDBRelease();

                    FormBinding.BindControlsToObject(objRM, pnlDetails);
                    objRM.ZID = this.ZID;
                    objRM.TransTypeID = (eRDBTransactionType)Convert.ToInt32(TransTypeID.SelectedItem.Value);

                    // at this point, amount is always positive on the screen.
                    // we need to change the sign depending on the transtypeid
                    switch (CommonUtility.Util.if_i(TransTypeID.SelectedValue, 0))
                    {
                        case (int)eRDBTransactionType.CollectReserve:
                        case (int)eRDBTransactionType.Void:
                        case (int)eRDBTransactionType.VoidMeritusOps:
                        case (int)eRDBTransactionType.VoidGovernment:
                        case (int)eRDBTransactionType.VoidReceiver:
                            // dont do anything
                            break;

                        default:
                            objRM.Amount = objRM.Amount * -1;
                            break;
                    }


                    //if (!(TransactionTypeID.SelectedValue == "5" // Collect Reserve
                    //    || TransactionTypeID.SelectedValue == "6" // Void
                    //    || TransactionTypeID.SelectedValue == "7" // VoidMeritusOps
                    //    || TransactionTypeID.SelectedValue == "8" // VoidGovernment
                    //    || TransactionTypeID.SelectedValue == "9" // VoidReceiver
                    //    ))
                    //{
                    //    // anything other than "Collect Reserve" (5) or "Void" (6) we flip the sign.
                    //    objRM.Amount = objRM.Amount * -1;
                    //}

                    // log who initiated it.
                    objRM.CreatedUserID = Convert.ToInt32(UserSessions.CurrentUser.UserID);

                    // insert it into the journal as well.
                    if (DataReserve.InsertRDBRelease(objRM) > 0 && objRM.ReleaseID > 0)
                    {
                        DataReserve.InsertRDBJournalReleaseTrans(objRM, DateTime.MinValue);

                        ret = true;
                    }
                }
            }

            return ret;
        }

        public override void FormNew()
        {
            this.FormClear();

            pnlDetails.Enabled = true;
            btnSave.Enabled = true;
            this.Adding = true;

            btnRemove.Enabled = false;
            btnRemove.Visible = false;

            ReportDate.Text = DateTime.Now.ToShortDateString();

            BankNotes.Text = "";

            btnSave.Enabled = true;
            FormHandler.SetControlEditMode(pnlDetails, true);
            gvBankBalance.Visible = true;
            lblApproval.Text = "";

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.FormSave())
            {
                if (event_click_savesuccess != null)
                {
                    event_click_savesuccess(this.ZID);
                }

                dlgRelease.WindowState = DialogWindowState.Hidden;
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlgRelease.WindowState = DialogWindowState.Hidden;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            int rows = 0;
            Button b = (Button)sender;

            switch (b.CommandName)
            {
                case "Cancel/Void":
                    rows = DataReserve.InsertRDBReleaseVoid(this.ZID, this.ReleaseID, Convert.ToInt32(UserSessions.CurrentUser.UserID));
                    break;


                case "Delete":
                    rows = DataReserve.DeleteRDBRelease(this.ReleaseID, this.ZID);

                    break;
            }


            dlgRelease.WindowState = DialogWindowState.Hidden;

            if (event_click_savesuccess != null)
            {
                event_click_savesuccess(this.ZID);
            }

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

            //if (ReportDate.Text.Trim() == "")
            //{
            //    cvDate.IsValid = false;
            //    yield return cvDate.ErrorMessage;
            //}

            // we don't check the sign of the amount. what you see is what you get in the table from the dialog screen.
            //if (!( CommonUtility.Util.if_dec( Amount.Value, 0) > 0))
            //{
            //    cvAmount.IsValid = false;
            //    yield return cvAmount.ErrorMessage;
            //}

            if (!(CommonUtility.Util.if_i(ReserveTypeID.SelectedValue, 0) > 0))
            {
                cvReserveTypeID.IsValid = false;
                yield return cvReserveTypeID.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(TransTypeID.SelectedValue, 0) > 0))
            {
                cvReleaseTypeID.IsValid = false;
                yield return cvReleaseTypeID.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(BankID.SelectedValue, 0) > 0))
            {
                cvBankID.IsValid = false;
                yield return cvBankID.ErrorMessage;
            }

            if (CommonUtility.Util.if_dec(Amount.Value, 0) <= 0)
            {
                cvAmount.IsValid = false;
                yield return cvAmount.ErrorMessage;
            }

            if (!(CommonUtility.Util.if_i(MethodID.SelectedValue, 0) > 0))
            {
                cvTransactionMethod.IsValid = false;
                cvTransactionMethod.ErrorMessage = "Method Required";
                yield return cvTransactionMethod.ErrorMessage;
            }

            // if wood forest
            if (CommonUtility.Util.if_i(BankID.SelectedValue, 0) == (int)eRDBBank.Woodforest)
            {
                if (CommonUtility.Util.if_i(MethodID.SelectedValue, 0) != (int)eRDBTransactionMethod.ACH) // anything other than ACH
                {
                    cvTransactionMethod.IsValid = false;
                    cvTransactionMethod.ErrorMessage = "For WoodForest, only Method ACH is available";
                    yield return cvTransactionMethod.ErrorMessage;
                }
            }

            // make sure reserve amount does not draw a negative balance in final reserve if you're releasing from reserve

            eRDBBank ebank = (eRDBBank)CommonUtility.Util.if_i(BankID.SelectedValue, 0);
            eRDBReserveType ereservetype = (eRDBReserveType)CommonUtility.Util.if_i(ReserveTypeID.SelectedValue, 0);
            decimal amount = Convert.ToDecimal(Amount.Value);


            if (ebank != eRDBBank.NotSet)
            {
                if (ereservetype == eRDBReserveType.Divert) // divert
                {
                    decimal banktypeamount = DataReserve.GetBalanceByBank(this.ZID, ebank, ereservetype);

                    if ((banktypeamount - amount) < 0)
                    {
                        cvAmount.IsValid = false;
                        cvAmount.ErrorMessage = "Release amount exceeds available divert balance";
                        yield return cvAmount.ErrorMessage;
                    }
                }
                else if (ereservetype == eRDBReserveType.Reserve) // reserve 
                {
                    decimal banktypeamount = DataReserve.GetBalanceByBank(this.ZID, ebank, ereservetype);

                    if ((banktypeamount - amount) < 0)
                    {
                        cvAmount.IsValid = false;
                        cvAmount.ErrorMessage = "Release amount exceeds available reserve balance";
                        yield return cvAmount.ErrorMessage;
                    }
                }
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

    
        public List<int> LiAvailBanks
        {
            get { return (List<int>)(ViewState["LiAvailBanks"] ?? null); }
            set { ViewState["LiAvailBanks"] = value; }
        }

        public List<int> LiAvailReserveTypes
        {
            get { return (List<int>)(ViewState["LiAvailReserveTypes"] ?? null); }
            set { ViewState["LiAvailReserveTypes"] = value; }
        }

        protected void gvBankBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                this.LiAvailBanks = new List<int>();
                this.LiAvailReserveTypes = new List<int>();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                int bank_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BankID"));

                if (!this.LiAvailBanks.Contains(bank_id))
                {
                    this.LiAvailBanks.Add(bank_id);
                }

                int reserve_type_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ReserveTypeID"));

                if (!this.LiAvailReserveTypes.Contains(reserve_type_id))
                {
                    this.LiAvailReserveTypes.Add(reserve_type_id);
                }


            }


        }



        protected void wucReleaseDialog_PreRender(object sender, EventArgs e)
        {

            //TransactionTypeID
            //ReserveTypeID
            //MethodID

            if (this.Adding)
            {
                foreach (ListItem li in BankID.Items)
                {
                    int val = CommonUtility.Util.if_i(li.Value, 0);

                    if (val > 0 && LiAvailBanks != null)
                    {
                        li.Enabled = LiAvailBanks.Contains(CommonUtility.Util.if_i(li.Value, 0));
                    }
                }

                foreach (ListItem li in ReserveTypeID.Items)
                {
                    int val = CommonUtility.Util.if_i(li.Value, 0);

                    if (val > 0 && LiAvailReserveTypes != null)
                    {
                        li.Enabled = LiAvailReserveTypes.Contains(val);
                    }
                }

            }


        }





        protected void TransactionTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            eRDBTransactionType eTT = (eRDBTransactionType)CommonUtility.Util.if_i(TransTypeID.SelectedValue, 0);

            if (eTT == eRDBTransactionType.CollectReserve)     // Collect Reserve
            {
                // when performing a collect reserve, we only put it into the reserve account.
                ReserveTypeID.SelectedValue = Convert.ToInt32(eRDBReserveType.Reserve).ToString();  // Reserve
                ReserveTypeID.Enabled = false;

                // also, when collecting, only viable method is ACH
                MethodID.SelectedValue = Convert.ToInt32(eRDBTransactionMethod.ACH).ToString(); // ACH
                MethodID.Enabled = false;
            }
            else if (eTT == eRDBTransactionType.Meritus)
            {
                // if releasing meritus ops account, only method avaiable is ACH
                MethodID.SelectedValue = Convert.ToInt32(eRDBTransactionMethod.ACH).ToString(); // ACH
                MethodID.Enabled = false;
            }
            else
            {
                ReserveTypeID.Enabled = true;
                MethodID.Enabled = true;
            }


            this.HandleBankNotes();
        }

        protected void ReserveTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HandleBankNotes();
        }

        private void HandleBankNotes()
        {
            //**PXP-7231(Meritus word replacement with paysafe) By Sanidhya kumar
            List<string> li = new List<string>() {
                "",
                "release divert to paysafe", 
                "release divert to merchant", 
                "release reserve to paysafe", 
                "release reserve to merchant", 
                "collect reserve for paysafe"
            };

            // the idea here is that we only want to change it if it's unmodified.
            if (li.Contains(BankNotes.Text.Trim().ToLower()))
            {

                if ((ReserveTypeID.SelectedValue == "1" || ReserveTypeID.SelectedValue == "2") // meritus, merchant 
                    && (TransTypeID.SelectedValue == "1" || TransTypeID.SelectedValue == "2")) // divert, reserve
                {
                    string _template = "Release {0} to {1}";
                    BankNotes.Text = string.Format(_template, ReserveTypeID.SelectedItem.Text, TransTypeID.SelectedItem.Text);
                }
                else if (TransTypeID.SelectedValue == "5" && ReserveTypeID.SelectedValue == "2") // ach and reserve
                {
                    //**PXP-7231(Meritus word replacement with paysafe) By Sanidhya kumar
                    BankNotes.Text = "Collect Reserve for Paysafe";
                }


            }

        }

    }
}
