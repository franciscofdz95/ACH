using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.DataObjects;
using System.Collections;
using System.Data;
using System.Globalization;
using PaymentXP.BusinessObjects;
using PaymentXP.BusinessObjects.Reserve;

namespace ZeusWeb.UserControls.Reserve
{
    public partial class wucTransferDialog : wucBaseDataEntry
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



            if (this.ZID > 0)
            {
                LookupTableHandler.LoadRDBBank(BankID, false, this.ZID);

                Hashtable prms = new Hashtable();
                prms.Add("@ZID", this.ZID);
                prms.Add("@PageSize", 1);
                prms.Add("@CurrentPage", 1);
                prms.Add("@SortDirection", 1);

                List<RDBSummary> li = DataReserve.GetRDBSummary(prms);

                if (li != null && li.Count > 0)
                {
                    lblDivertAmount.Text = String.Format("{0:0.00}", li[0].Divert_Net);
                    lblReserveAmount.Text = String.Format("{0:0.00}", li[0].Reserve_Net);
                }

                gvBankBalance.DataSource = DataReserve.GetBalanceByBank(this.ZID);
                gvBankBalance.DataBind();
            }
        }

        public override void FormClear()
        {
            FormHandler.ClearAllControls(pnlDetails);
            this.ZID = 0;


        }

        public override bool FormSave()
        {
            bool ret = false;

            if (this.ZID > 0 && this.FormDataCheck() && Page.IsValid)
            {
                DataReserve.InsertTransferDivert2Reserve(
                    this.ZID,
                    Convert.ToDecimal(TransferAmount.Value),
                    (eRDBBank)Convert.ToInt32(BankID.SelectedValue),
                    CommonUtility.Util.if_i(UserSessions.CurrentUser.UserID, 0));

                ret = true;
            }

            return ret;
        }

        public override void FormNew()
        {
            this.FormClear();

            //PostedDate.Value = DateTime.Now;
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            bool ret = false;

            //if (PostedDate.Text.Trim() == "")
            //{
            //    ret = false;
            //    cvDate.IsValid = false;
            //}

            decimal divAmount = decimal.Parse(lblDivertAmount.Text, NumberStyles.Currency);
            decimal trxAmount = decimal.Parse(TransferAmount.Text, NumberStyles.Currency);

            if (trxAmount <= divAmount)
            {
                ret = true;
                cvAmount.IsValid = true;
            }
            else
            {
                ret = false;
                cvAmount.IsValid = false;
            }

            if (!(CommonUtility.Util.if_i(BankID.SelectedValue, 0) > 0))
            {
                ret = false;
                cbBank.IsValid = false;
            }

            return ret;
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

                dlg.WindowState = DialogWindowState.Hidden;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlg.WindowState = DialogWindowState.Hidden;
        }

        protected void gvBankBalance_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            //if (e.Row.RowType == DataControlRowType.Header)
            //{
            //    this.LiAvailBanks = new List<int>();
            //    this.LiAvailReserveTypes = new List<int>();
            //}

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{

            //    int bank_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "BankID"));

            //    if (!this.LiAvailBanks.Contains(bank_id))
            //    {
            //        this.LiAvailBanks.Add(bank_id);
            //    }

            //    int reserve_type_id = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ReserveTypeID"));

            //    if (!this.LiAvailReserveTypes.Contains(reserve_type_id))
            //    {
            //        this.LiAvailReserveTypes.Add(reserve_type_id);
            //    }
            //}
        }


    }
}