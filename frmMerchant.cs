using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using Infragistics.Win.UltraWinGrid;
using Nmc.Ach.Dal;

namespace AchSystem
{
    public partial class frmMerchant : AchSystem.frmBase 
    {

        public frmMerchant()
        {
            InitializeComponent();


            FormHandler.CreateToolBarButton(tbrTop, "Wallet");

            tbrTop.Toolbars[0].Tools["Wallet"].ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.Wallet_ToolClick);

            FormHandler.AddControlChangedEvent(this);

            txtAchID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMerchantID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtWithdrawTransRoute.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtWithdrawAccountNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtReservePct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtReservePeriod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtHoldPeriod.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtFileLoadFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtAdditionalFiles.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtStatementFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtItemFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtMonthlyMin.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtInquiryFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtReturnFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtOverdraftFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtProcessFee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtItemLimit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtFeeID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtFundID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            cboGroupMerchant.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboSecc.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboMerchantType.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);
            cboBankID.KeyDown += new System.Windows.Forms.KeyEventHandler(EventHandler.Delete_KeyDown);


            txtSameDDANumber_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtNextReviewDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtNewMerchantDays_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtItemLimitTicket_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtItemLimitTicket_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchCredit_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtBatchCredit_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMonthlyLimit_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtMonthlyLimit_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMonthlyTransactionLimit_Count.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtMonthlyTransactionLimit_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchNetDollarWeekly_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtBatchNetDollarWeekly_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchNetDollar_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtBatchNetDollar_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchTotalTicket_Count.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtBatchTotalTicket_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);
            txtNegativeBatch_Amount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.CurrencyOnly_KeyPress);
            txtNegativeBatch_Times.KeyPress += new System.Windows.Forms.KeyPressEventHandler(EventHandler.NumericOnly_KeyPress);

            LookUpTableHandler.LoadBankID(cboBankID);
            LookUpTableHandler.LoadMerchantType(cboMerchantType);
            LookUpTableHandler.LoadSecc(cboSecc);
            LookUpTableHandler.LoadGroupMerchants (cboGroupMerchant);
            LookUpTableHandler.LoadMerchantTest(cboTest);

            this.Data = new DataMerchant();
            this.KeyColumnName = "AchID";
            FormHandler.SetSecurity(this);
            if (!main.g_User.IsAdmin)
            {
                this.tbrTop.Toolbars[0].Tools["Wallet"].SharedProps.Visible = false;
            }
        }

        private void Wallet_ToolClick(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs e)
        {
            frmWallet frm = new frmWallet(Convert.ToInt32(txtAchID.Text), Convert.ToInt32(txtMerchantID.Text), txtMerchantName.Text );

            frm.ShowDialog();

        }
        public override bool FormFind()
        {
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@" + this.KeyColumnName, this.ID));
            this.Dr = this.Data.Select (prms);

            if (this.Dr.Read())
                return true;
            else
                return false;
        }

        public override void FormShow()
        {
            this.Showing = true;

            txtAchID.Text = this.Dr["AchID"].ToString().Trim();
            txtMerchantID.Text = this.Dr["Merchant ID"].ToString().Trim();
            chkActive.Checked = DataLayer.Field2Bool(this.Dr["Active"]);
            txtMerchantName.Text = this.Dr["Merchant Name"].ToString().Trim();
            txtDescription.Text = this.Dr["Ach Description"].ToString().Trim();
            txtDiscretionary.Text = this.Dr["Ach Discretionary Data"].ToString().Trim();
            ListHandler.ListFindItem(cboSecc, this.Dr["Secc"].ToString().Trim());
            ListHandler.ListFindItem(cboBankID, this.Dr["BankID"].ToString().Trim());
            txtTransRoute.Text = this.Dr["Trans Route"].ToString().Trim();
            txtAccountNo.Text = this.Dr["Account No"].ToString().Trim();
            txtAccountName.Text = this.Dr["Account Name"].ToString().Trim();
            txtWithdrawTransRoute.Text = this.Dr["Withdraw TR"].ToString().Trim();
            txtWithdrawAccountNo.Text = this.Dr["Withdraw Acct No"].ToString().Trim();
            txtReservePct.Text = DataLayer.Field2Dec(this.Dr["Reserve %"]).ToString().Trim();
            txtReservePeriod.Text = DataLayer.Field2Int(this.Dr["Reserve Period"]).ToString().Trim();
            txtHoldPeriod.Text = DataLayer.Field2Int(this.Dr["Hold Period"]).ToString().Trim();
            ListHandler.ListFindItem(cboMerchantType, this.Dr["Merchant Type"].ToString().Trim());
            txtFileLoadFee.Text = DataLayer.Field2Dec(this.Dr["File Load Fee"]).ToString().Trim();
            txtAdditionalFiles.Text = DataLayer.Field2Dec(this.Dr["Additional Files"]).ToString().Trim();
            txtStatementFee.Text = DataLayer.Field2Dec(this.Dr["Statement Fee"]).ToString().Trim();
            txtItemFee.Text = DataLayer.Field2Dec(this.Dr["Item Fee"]).ToString().Trim();
            txtMonthlyMin.Text = DataLayer.Field2Dec(this.Dr["Monthly Min"]).ToString().Trim();
            txtInquiryFee.Text = DataLayer.Field2Dec(this.Dr["Inquiry Fee"]).ToString().Trim();
            txtReturnFee.Text = DataLayer.Field2Dec(this.Dr["Return Fee"]).ToString().Trim();
            txtOverdraftFee.Text = DataLayer.Field2Dec(this.Dr["Over Draft Fee"]).ToString().Trim();
            txtProcessFee.Text = DataLayer.Field2Dec(this.Dr["Process Fee %"]).ToString().Trim();
            txtItemLimit.Text = DataLayer.Field2Dec(this.Dr["Item Limit Amount"]).ToString().Trim();
            txtFeeID.Text = DataLayer.Field2Int(this.Dr["Withdraw Fees AchID"]).ToString().Trim();
            txtFundID.Text = DataLayer.Field2Int(this.Dr["Withdraw Funds AchID"]).ToString().Trim();            
            chkResponseFile.Checked = DataLayer.Field2Bool( this.Dr["Produce Rsp File"]);
            chkReturnFile.Checked = DataLayer.Field2Bool(this.Dr["Produce Rtn File"]);
            chkOverrideSECC.Checked = DataLayer.Field2Bool(this.Dr["OverrideSECC"]);
            chkCheckDuplicateTrans.Checked = DataLayer.Field2Bool(this.Dr["CheckDuplicateTrans"]);

            if (DataLayer.Field2Str(this.Dr["Stop EFT From"]) == "Y")
                chkStopEFT.Checked = true;
            else
                chkStopEFT.Checked = false;

            chkCreateHold.Checked = DataLayer.Field2Bool(this.Dr["Create Hold"]);
            chkMonthlyBilling.Checked = DataLayer.Field2Bool(this.Dr["Monthly Billing"]);
            chkAllowCredits.Checked = DataLayer.Field2Bool(this.Dr["Allow Credits"]);
            ListHandler.ListFindItem(cboGroupMerchant, this.Dr["Group ID"].ToString().Trim());
            ListHandler.ListFindItem(cboTest, this.Dr["Test"].ToString().Trim());

            chkWatchMerchant.Checked = DataLayer.Field2Bool(this.Dr["Risk Watch Merchant Ind"]);
            chkSameDDANumber.Checked = DataLayer.Field2Bool(this.Dr["Risk Same DDA Number Ind"]);
            txtSameDDANumber_Times.Text = DataLayer.Field2Int(this.Dr["Risk Same DDA Number Times"]).ToString();
            chkNextReviewDate.Checked = DataLayer.Field2Bool (this.Dr["Risk Next Review Date Ind"]);
            txtNextReviewDate.Text = this.Dr["Risk Next Review Date"].ToString().Trim();
            chkNewMerchantDays.Checked = DataLayer.Field2Bool (this.Dr["Risk New Merchant Days Ind"]);
            txtNewMerchantDays_Times.Text = DataLayer.Field2Int(this.Dr["Risk New Merchant Days Times"]).ToString();
            chkItemLimitTicket.Checked = DataLayer.Field2Bool (this.Dr["Risk Item Limit Ticket Ind"]);
            txtItemLimitTicket_Amount.Text = DataLayer.Field2Dec(this.Dr["Risk Item Limit Ticket Amount"]).ToString();
            txtItemLimitTicket_Times.Text = DataLayer.Field2Int(this.Dr["Risk Item Limit Ticket Times"]).ToString();
            chkBatchCreditAmount.Checked = DataLayer.Field2Bool (this.Dr["Risk Batch Net Credit Amount Ind"]);
            txtBatchCredit_Amount.Text = DataLayer.Field2Dec(this.Dr["Risk Batch Net Credit Amount"]).ToString();
            txtBatchCredit_Times.Text = DataLayer.Field2Int(this.Dr["Risk Batch Net Credit Amount Times"]).ToString();
            chkMonthlyLimit.Checked = DataLayer.Field2Bool (this.Dr["Risk Monthly Limit Ind"]); 
            txtMonthlyLimit_Amount.Text  = DataLayer.Field2Dec (this.Dr["Risk Monthly Limit Amount"]).ToString();
            txtMonthlyLimit_Times.Text = DataLayer.Field2Int(this.Dr["Risk Monthly Limit Times"]).ToString();
            chkMonthlyTransactionLimit.Checked = DataLayer.Field2Bool (this.Dr["Risk Monthly Transaction Limit Ind"]);
            txtMonthlyTransactionLimit_Count.Text = DataLayer.Field2Int(this.Dr["Risk Monthly Transaction Limit Count"]).ToString();
            txtMonthlyTransactionLimit_Times.Text = DataLayer.Field2Int(this.Dr["Risk Monthly Transaction Limit Times"]).ToString();
            chkBatchNetDollarWeekly.Checked = DataLayer.Field2Bool (this.Dr["Risk Batch Net Dollar Weekly Ind"]); 
            txtBatchNetDollarWeekly_Amount.Text  = DataLayer.Field2Dec (this.Dr["Risk Batch Net Dollar Weekly Amount"]).ToString();
            txtBatchNetDollarWeekly_Times.Text = DataLayer.Field2Int(this.Dr["Risk Batch Net Dollar Weekly Times"]).ToString();
            chkBatchNetDollar.Checked = DataLayer.Field2Bool (this.Dr["Risk Batch Net Dollar Ind"]); 
            txtBatchNetDollar_Amount.Text  = DataLayer.Field2Dec (this.Dr["Risk Batch Net Dollar Amount"]).ToString();
            txtBatchNetDollar_Times.Text = DataLayer.Field2Int(this.Dr["Risk Batch Net Dollar Times"]).ToString();
            chkBatchTotalTicket.Checked = DataLayer.Field2Bool (this.Dr["Risk Batch Total Ticket Ind"]); 
            txtBatchTotalTicket_Count.Text  = DataLayer.Field2Int (this.Dr["Risk Batch Total Ticket Count"]).ToString();
            txtBatchTotalTicket_Times.Text = DataLayer.Field2Int(this.Dr["Risk Batch Total Ticket Times"]).ToString();
            chkNegativeBatch.Checked = DataLayer.Field2Bool (this.Dr["Risk Negative Batch Ind"]); 
            txtNegativeBatch_Amount.Text  = DataLayer.Field2Dec (this.Dr["Risk Negative Batch Amount"]).ToString();
            txtNegativeBatch_Times.Text = DataLayer.Field2Int(this.Dr["Risk Negative Batch Times"]).ToString();
            chkSeasonalMonths.Checked = DataLayer.Field2Bool (this.Dr["Risk Seasonal Months Ind"]); 
            chkSpecialHandling.Checked = DataLayer.Field2Bool (this.Dr["Risk Special Handling Ind"]);
            txtEmail.Text = this.Dr["Email"].ToString().Trim();

            FormHandler.PopulateControlTag(this);

            this.Showing = false;
        }

        public override void FormOpen(UltraGridRow dr)
        {
            if (dr != null)
            {
                this.ID = Convert.ToInt64(dr.Cells[this.KeyColumnName].Value);
                if (this.FormFind())
                    this.FormShow();

                this.ShowDialog();
            }
            else
            {
                if (this.ID == -1)
                    if (!tbrTop.Toolbars[0].Tools["Save"].SharedProps.Enabled)
                        this.Close();
            }
        }

        public override void FormNew()
        {
            this.Adding = true;            
            this.FormClear();
            FormHandler.ClearControlTag(this);
            this.FormToggleButtons();

            ListHandler.ListFindItem(cboTest, "N");
            ListHandler.ListFindItem(cboMerchantType , "H");

            if (this.Visible == false)
                this.ShowDialog();
        }

        public override void FormClear()
        {
            this.Showing = true;

            txtAchID.Text = string.Empty;
            txtMerchantID.Text = string.Empty;
            chkActive.Checked = true;
            txtMerchantName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtDiscretionary.Text = string.Empty;
            cboSecc.SelectedIndex = -1;
            cboBankID.SelectedIndex = -1;
            txtTransRoute.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtAccountName.Text = string.Empty;
            txtWithdrawTransRoute.Text = string.Empty;
            txtWithdrawAccountNo.Text = string.Empty;
            txtReservePct.Text = "0";
            txtReservePeriod.Text = "0";
            txtHoldPeriod.Text = "0";
            cboMerchantType.SelectedIndex = -1;
            txtFileLoadFee.Text = "0";
            txtAdditionalFiles.Text = "0";
            txtStatementFee.Text = "0";
            txtItemFee.Text = "0";
            txtMonthlyMin.Text = "0";
            txtInquiryFee.Text = "0";
            txtReturnFee.Text = "0";
            txtOverdraftFee.Text = "0";
            txtProcessFee.Text = "0";
            txtItemLimit.Text = "0";
            txtFeeID.Text = "0";
            txtFundID.Text = "0";
            chkResponseFile.Checked = false;
            chkReturnFile.Checked = false;
            chkStopEFT.Checked = false;
            chkStopEFT.Checked = false;
            chkMonthlyBilling.Checked = false;
            chkCreateHold.Checked = false;
            cboGroupMerchant.SelectedIndex = -1;
            cboTest.SelectedIndex = -1;
            txtEmail.Text = string.Empty;
            chkOverrideSECC.Checked = false;
            chkCheckDuplicateTrans.Checked = false;
    
            this.Risk_Merchant_Defaults(true);

            this.Showing = false;
        }

        private void Risk_Merchant_Defaults(bool IsStandMerchant)
        {
            chkWatchMerchant.Checked = true;

            chkSameDDANumber.Checked = true;
            if (IsStandMerchant)
                txtSameDDANumber_Times.Text = "3";
            else
                txtSameDDANumber_Times.Text = "2";

            chkNextReviewDate.Checked = false;
            txtNextReviewDate.Text = string.Empty;

            chkNewMerchantDays.Checked = true;
            if (IsStandMerchant)
                txtNewMerchantDays_Times.Text = "15";
            else
                txtNewMerchantDays_Times.Text = "30";

            chkItemLimitTicket.Checked = false;
            txtItemLimitTicket_Amount.Text = "0";
            txtItemLimitTicket_Times.Text = "1";
            chkBatchCreditAmount.Checked = false;
            txtBatchCredit_Amount.Text = "0";
            txtBatchCredit_Times.Text = "1";
            chkMonthlyLimit.Checked = false;
            txtMonthlyLimit_Amount.Text = "0";
            txtMonthlyLimit_Times.Text = "1";
            chkMonthlyTransactionLimit.Checked = false;
            txtMonthlyTransactionLimit_Count.Text = "0";
            txtMonthlyTransactionLimit_Times.Text = "1";
            chkBatchNetDollarWeekly.Checked = false;
            txtBatchNetDollarWeekly_Amount.Text = "0";
            txtBatchNetDollarWeekly_Times.Text = "1";
            chkBatchNetDollar.Checked = false;
            txtBatchNetDollar_Amount.Text = "0";
            txtBatchNetDollar_Times.Text = "1";
            chkBatchTotalTicket.Checked = false;
            txtBatchTotalTicket_Count.Text = "0";
            txtBatchTotalTicket_Times.Text = "1";
            chkNegativeBatch.Checked = false;
            txtNegativeBatch_Amount.Text = "0";
            txtNegativeBatch_Times.Text = "1";
            chkSeasonalMonths.Checked = false;
            chkSpecialHandling.Checked = false; 
        }

        public override bool FormAdd()
        {
            ArrayList prms = new ArrayList();
            AchListItem item = null;

            SqlParameter prm = new SqlParameter("@AchID", -1);
            prm.Direction = ParameterDirection.Output;
            prms.Add(prm);
            prms.Add(new SqlParameter("@MerchantID", txtMerchantID.Text));
            prms.Add(new SqlParameter("@Active", DataLayer.Bool2Field(chkActive.Checked) ));
            prms.Add(new SqlParameter("@AchCoName", txtMerchantName.Text));
            prms.Add(new SqlParameter("@AchDescrp", txtDescription.Text));
            prms.Add(new SqlParameter("@AchDiscrtn",txtDiscretionary.Text ));
            item = (AchListItem) cboSecc.SelectedItem;
            prms.Add(new SqlParameter("@Secc", item.ItemValue ));
            item = (AchListItem) cboBankID.SelectedItem;
            prms.Add(new SqlParameter("@BankID",item.ItemValue ));
            prms.Add(new SqlParameter("@TransRoute",txtTransRoute.Text ));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            prms.Add(new SqlParameter("@dfiAcctName", txtAccountName.Text));
            prms.Add(new SqlParameter("@WithdrawTR", txtWithdrawTransRoute.Text));
            prms.Add(new SqlParameter("@WithdrawAcctNo",  txtWithdrawAccountNo.Text));
            prms.Add(new SqlParameter("@Reservepct", DataLayer.Decimal2Field(txtReservePct.Text)));
            prms.Add(new SqlParameter("@ReservePeriod", DataLayer.Int2Field(txtReservePeriod.Text)));
            prms.Add(new SqlParameter("@HoldPeriod", DataLayer.Int2Field(txtHoldPeriod.Text)));
            item = (AchListItem) cboMerchantType.SelectedItem;
            prms.Add(new SqlParameter("@MerchantTypeCode", item.ItemValue ));
            prms.Add(new SqlParameter("@FileLoadFee", DataLayer.Decimal2Field(txtFileLoadFee.Text)));
            prms.Add(new SqlParameter("@AdditionalFiles", DataLayer.Decimal2Field(txtAdditionalFiles.Text)));
            prms.Add(new SqlParameter("@StatementFee", DataLayer.Decimal2Field(txtStatementFee.Text)));
            prms.Add(new SqlParameter("@ItemFee", DataLayer.Decimal2Field(txtItemFee.Text)));
            prms.Add(new SqlParameter("@MonthlyMin", DataLayer.Decimal2Field(txtMonthlyMin.Text)));
            prms.Add(new SqlParameter("@InquiryFee",DataLayer.Decimal2Field(txtInquiryFee.Text)));
            prms.Add(new SqlParameter("@ReturnFee", DataLayer.Decimal2Field(txtReturnFee.Text)));
            prms.Add(new SqlParameter("@OverDraftFee",DataLayer.Decimal2Field(txtOverdraftFee.Text)));
            prms.Add(new SqlParameter("@ProcessFee",DataLayer.Decimal2Field(txtProcessFee.Text)));
            prms.Add(new SqlParameter("@ItemLimitAmount", DataLayer.Decimal2Field(txtItemLimit.Text)));
            prms.Add(new SqlParameter("@WithdrawFees_AchID", txtFeeID.Text));
            prms.Add(new SqlParameter("@WithdrawFunds_AchID", txtFundID.Text));
            prms.Add(new SqlParameter("@ProduceRspFile",DataLayer.Bool2Field(chkResponseFile.Checked ) ));
            prms.Add(new SqlParameter("@ProduceRtnFile",DataLayer.Bool2Field(chkReturnFile.Checked) ));
            if (chkStopEFT.Checked)
                prms.Add(new SqlParameter("@StopEFTFrom", "Y"));
            else
                prms.Add(new SqlParameter("@StopEFTFrom", "N"));

            prms.Add(new SqlParameter("@CreateHold", DataLayer.Bool2Field(chkCreateHold.Checked)));
            prms.Add(new SqlParameter("@MonthlyBase", DataLayer.Bool2Field(chkMonthlyBilling.Checked)));
            prms.Add(new SqlParameter("@AllowBlindCredits", DataLayer.Bool2Field(chkAllowCredits.Checked)));
            prms.Add(new SqlParameter("@AddedBy", main.g_User.UserID));

            if (cboGroupMerchant.SelectedIndex != -1)
            {
                item = (AchListItem)cboGroupMerchant.SelectedItem;
                prms.Add(new SqlParameter("@GroupID", item.ItemValue));
            }
            else
                prms.Add(new SqlParameter("@GroupID", DBNull.Value));

            item = (AchListItem)cboTest.SelectedItem;
            prms.Add(new SqlParameter("@Test", item.ItemValue));

            prms.Add(new SqlParameter("@Risk_WatchMerchant_Ind", DataLayer.Bool2Field(chkWatchMerchant.Checked)));
            prms.Add(new SqlParameter("@Risk_SameDDANumber_Ind", DataLayer.Bool2Field(chkSameDDANumber.Checked)));
            prms.Add(new SqlParameter("@Risk_SameDDANumber_Times", DataLayer.Int2Field(txtSameDDANumber_Times.Text)));
            prms.Add(new SqlParameter("@Risk_NextReviewDate_Ind", DataLayer.Bool2Field(chkNextReviewDate.Checked)));
            prms.Add(new SqlParameter("@Risk_NextReviewDate_Date", DataLayer.Date2Field(txtNextReviewDate.Text)));
            prms.Add(new SqlParameter("@Risk_NewMerchantDays_Ind", DataLayer.Bool2Field(chkNewMerchantDays.Checked)));
            prms.Add(new SqlParameter("@Risk_NewMerchantDays_Times", DataLayer.Int2Field(txtNewMerchantDays_Times.Text)));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Ind", DataLayer.Bool2Field(chkItemLimitTicket.Checked)));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Amount", DataLayer.Decimal2Field(txtItemLimitTicket_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Times", DataLayer.Int2Field(txtItemLimitTicket_Times.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Ind", DataLayer.Bool2Field(chkBatchCreditAmount.Checked)));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Amount", DataLayer.Decimal2Field(txtBatchCredit_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Times", DataLayer.Int2Field(txtBatchCredit_Times.Text)));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Ind", DataLayer.Bool2Field(chkMonthlyLimit.Checked)));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Amount", DataLayer.Decimal2Field(txtMonthlyLimit_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Times", txtMonthlyLimit_Times.Text));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Ind", DataLayer.Bool2Field(chkMonthlyTransactionLimit.Checked)));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Count", DataLayer.Int2Field(txtMonthlyTransactionLimit_Count.Text)));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Times", DataLayer.Int2Field(txtMonthlyTransactionLimit_Times.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Ind", DataLayer.Bool2Field(chkBatchNetDollarWeekly.Checked)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Amount", DataLayer.Decimal2Field(txtBatchNetDollarWeekly_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Times", DataLayer.Int2Field(txtBatchNetDollarWeekly_Times.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Ind", DataLayer.Bool2Field(chkBatchNetDollar.Checked)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Amount", DataLayer.Decimal2Field(txtBatchNetDollar_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Times", DataLayer.Int2Field(txtBatchNetDollar_Times.Text)));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Ind", DataLayer.Bool2Field(chkBatchTotalTicket.Checked)));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Count", DataLayer.Int2Field(txtBatchTotalTicket_Count.Text)));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Times", DataLayer.Int2Field(txtBatchTotalTicket_Times.Text)));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Ind", DataLayer.Bool2Field(chkNegativeBatch.Checked)));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Amount", DataLayer.Decimal2Field(txtNegativeBatch_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Times", DataLayer.Int2Field(txtNegativeBatch_Times.Text)));
            prms.Add(new SqlParameter("@Risk_SeasonalMonths_Ind", DataLayer.Bool2Field(chkSeasonalMonths.Checked)));
            prms.Add(new SqlParameter("@Risk_SpecialHandling_Ind", DataLayer.Bool2Field(chkSpecialHandling.Checked)));
            prms.Add(new SqlParameter("@Email", txtEmail.Text));
            prms.Add(new SqlParameter("@OverrideSECC", DataLayer.Bool2Field(chkOverrideSECC.Checked)));
            prms.Add(new SqlParameter("@CheckDuplicateTrans", DataLayer.Bool2Field(chkCheckDuplicateTrans.Checked)));

            long lngID = this.Data.Insert(prms);

            if (lngID != -1)
            {
                this.Adding = false;
                this.IsDirty = true;
                txtAchID.ReadOnly = true;
                this.ID = lngID;
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool FormUpdate()
        {
            ArrayList prms = new ArrayList();
            AchListItem item = null;

            prms.Add(new SqlParameter("@MerchantID",txtMerchantID.Text ));
            prms.Add(new SqlParameter("@AchID", txtAchID.Text));
            prms.Add(new SqlParameter("@Active", DataLayer.Bool2Field(chkActive.Checked) ));
            prms.Add(new SqlParameter("@AchCoName", txtMerchantName.Text));
            prms.Add(new SqlParameter("@AchDescrp", txtDescription.Text));
            prms.Add(new SqlParameter("@AchDiscrtn",txtDiscretionary.Text ));
            item = (AchListItem) cboSecc.SelectedItem;
            prms.Add(new SqlParameter("@Secc", item.ItemValue ));
            item = (AchListItem) cboBankID.SelectedItem;
            prms.Add(new SqlParameter("@BankID",item.ItemValue ));
            prms.Add(new SqlParameter("@TransRoute",txtTransRoute.Text ));
            prms.Add(new SqlParameter("@AccountNo", txtAccountNo.Text));
            prms.Add(new SqlParameter("@dfiAcctName", txtAccountName.Text));
            prms.Add(new SqlParameter("@WithdrawTR", txtWithdrawTransRoute.Text));
            prms.Add(new SqlParameter("@WithdrawAcctNo",  txtWithdrawAccountNo.Text));
            prms.Add(new SqlParameter("@Reservepct", DataLayer.Decimal2Field(txtReservePct.Text)));
            prms.Add(new SqlParameter("@ReservePeriod", DataLayer.Int2Field(txtReservePeriod.Text)));
            prms.Add(new SqlParameter("@HoldPeriod", DataLayer.Int2Field(txtHoldPeriod.Text)));
            item = (AchListItem) cboMerchantType.SelectedItem;
            prms.Add(new SqlParameter("@MerchantTypeCode", item.ItemValue ));
            prms.Add(new SqlParameter("@FileLoadFee", DataLayer.Decimal2Field(txtFileLoadFee.Text)));
            prms.Add(new SqlParameter("@AdditionalFiles", DataLayer.Decimal2Field(txtAdditionalFiles.Text)));
            prms.Add(new SqlParameter("@StatementFee", DataLayer.Decimal2Field(txtStatementFee.Text)));
            prms.Add(new SqlParameter("@ItemFee", DataLayer.Decimal2Field(txtItemFee.Text)));
            prms.Add(new SqlParameter("@MonthlyMin", DataLayer.Decimal2Field(txtMonthlyMin.Text)));
            prms.Add(new SqlParameter("@InquiryFee",DataLayer.Decimal2Field(txtInquiryFee.Text)));
            prms.Add(new SqlParameter("@ReturnFee", DataLayer.Decimal2Field(txtReturnFee.Text)));
            prms.Add(new SqlParameter("@OverDraftFee",DataLayer.Decimal2Field(txtOverdraftFee.Text)));
            prms.Add(new SqlParameter("@ProcessFee",DataLayer.Decimal2Field(txtProcessFee.Text)));
            prms.Add(new SqlParameter("@ItemLimitAmount", DataLayer.Decimal2Field(txtItemLimit.Text)));
            prms.Add(new SqlParameter("@WithdrawFees_AchID", txtFeeID.Text));
            prms.Add(new SqlParameter("@WithdrawFunds_AchID", txtFundID.Text));
            prms.Add(new SqlParameter("@ProduceRspFile",DataLayer.Bool2Field(chkResponseFile.Checked ) ));
            prms.Add(new SqlParameter("@ProduceRtnFile",DataLayer.Bool2Field(chkReturnFile.Checked) ));
            if (chkStopEFT.Checked)
                prms.Add(new SqlParameter("@StopEFTFrom","Y"));
            else
                prms.Add(new SqlParameter("@StopEFTFrom","N"));

            prms.Add(new SqlParameter("@CreateHold", DataLayer.Bool2Field(chkCreateHold.Checked)));
            prms.Add(new SqlParameter("@MonthlyBase", DataLayer.Bool2Field(chkMonthlyBilling.Checked)));
            prms.Add(new SqlParameter("@AllowBlindCredits", DataLayer.Bool2Field(chkAllowCredits.Checked)));
            prms.Add(new SqlParameter("@UpdatedBy", main.g_User.UserID));

            if (cboGroupMerchant.SelectedIndex != -1)
            {
                item = (AchListItem)cboGroupMerchant.SelectedItem;
                prms.Add(new SqlParameter("@GroupID", item.ItemValue));
            }
            else
                prms.Add(new SqlParameter("@GroupID", DBNull.Value));

            item = (AchListItem)cboTest.SelectedItem;
            prms.Add(new SqlParameter("@Test", item.ItemValue));

            prms.Add(new SqlParameter("@Risk_WatchMerchant_Ind", DataLayer.Bool2Field(chkWatchMerchant.Checked )));
            prms.Add(new SqlParameter("@Risk_SameDDANumber_Ind", DataLayer.Bool2Field(chkSameDDANumber.Checked )));
            prms.Add(new SqlParameter("@Risk_SameDDANumber_Times", DataLayer.Int2Field(txtSameDDANumber_Times.Text )));
            prms.Add(new SqlParameter("@Risk_NextReviewDate_Ind", DataLayer.Bool2Field(chkNextReviewDate.Checked )));
            prms.Add(new SqlParameter("@Risk_NextReviewDate_Date", DataLayer.Date2Field(txtNextReviewDate.Text)));
            prms.Add(new SqlParameter("@Risk_NewMerchantDays_Ind", DataLayer.Bool2Field(chkNewMerchantDays.Checked )));
            prms.Add(new SqlParameter("@Risk_NewMerchantDays_Times", DataLayer.Int2Field(txtNewMerchantDays_Times.Text )));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Ind", DataLayer.Bool2Field(chkItemLimitTicket.Checked )));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Amount", DataLayer.Decimal2Field(txtItemLimitTicket_Amount.Text )));
            prms.Add(new SqlParameter("@Risk_ItemLimitTicket_Times", DataLayer.Int2Field(txtItemLimitTicket_Times.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Ind", DataLayer.Bool2Field(chkBatchCreditAmount.Checked )));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Amount", DataLayer.Decimal2Field(txtBatchCredit_Amount.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetCreditAmount_Times", DataLayer.Int2Field(txtBatchCredit_Times.Text)));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Ind", DataLayer.Bool2Field(chkMonthlyLimit.Checked )));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Amount", DataLayer.Decimal2Field(txtMonthlyLimit_Amount.Text )));
            prms.Add(new SqlParameter("@Risk_MonthlyLimit_Times", txtMonthlyLimit_Times.Text ));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Ind", DataLayer.Bool2Field(chkMonthlyTransactionLimit.Checked )));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Count", DataLayer.Int2Field(txtMonthlyTransactionLimit_Count.Text )));
            prms.Add(new SqlParameter("@Risk_MonthlyTransactionLimit_Times", DataLayer.Int2Field(txtMonthlyTransactionLimit_Times.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Ind", DataLayer.Bool2Field(chkBatchNetDollarWeekly.Checked )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Amount", DataLayer.Decimal2Field(txtBatchNetDollarWeekly_Amount.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollarWeekly_Times", DataLayer.Int2Field(txtBatchNetDollarWeekly_Times.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Ind", DataLayer.Bool2Field(chkBatchNetDollar.Checked )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Amount", DataLayer.Decimal2Field(txtBatchNetDollar_Amount.Text )));
            prms.Add(new SqlParameter("@Risk_BatchNetDollar_Times", DataLayer.Int2Field(txtBatchNetDollar_Times.Text )));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Ind", DataLayer.Bool2Field(chkBatchTotalTicket.Checked )));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Count", DataLayer.Int2Field(txtBatchTotalTicket_Count.Text )));
            prms.Add(new SqlParameter("@Risk_BatchTotalTicket_Times", DataLayer.Int2Field(txtBatchTotalTicket_Times.Text )));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Ind", DataLayer.Bool2Field(chkNegativeBatch.Checked )));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Amount", DataLayer.Decimal2Field(txtNegativeBatch_Amount.Text)));
            prms.Add(new SqlParameter("@Risk_NegativeBatch_Times", DataLayer.Int2Field(txtNegativeBatch_Times.Text )));
            prms.Add(new SqlParameter("@Risk_SeasonalMonths_Ind", DataLayer.Bool2Field(chkSeasonalMonths.Checked )));
            prms.Add(new SqlParameter("@Risk_SpecialHandling_Ind", DataLayer.Bool2Field(chkSpecialHandling.Checked)));
            prms.Add(new SqlParameter("@Email", txtEmail.Text));
            prms.Add(new SqlParameter("@OverrideSECC", DataLayer.Bool2Field(chkOverrideSECC.Checked)));
            prms.Add(new SqlParameter("@CheckDuplicateTrans", DataLayer.Bool2Field(chkCheckDuplicateTrans.Checked)));

            int intRows = this.Data.Update(prms);

            if (intRows > 0)
            {
                if (this.FormFind())
                    this.FormShow();

                this.FormToggleButtons();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void FormUndo()
        {
            this.Adding = false;

            this.FormToggleButtons();
            txtAchID.ReadOnly = true;

            if (this.ID != -1)
            {
                if (this.FormFind())
                    this.FormShow();
            }
            else
            {
                this.Close();
            }
        }

        public override bool FormDataCheck()
        {
            string strError = string.Empty;

            if (txtMerchantID.Text.Trim() == string.Empty)
                strError += "Please enter a Merchant ID.\n";

            if (txtMerchantName.Text.Trim() == string.Empty)
                strError += "Please enter a Merchant Name.\n";

            if (txtDescription.Text.Trim() == string.Empty)
                strError += "Please enter an ACH Description.\n";

            if (txtDiscretionary.Text.Trim() == string.Empty)
                strError += "Please enter an ACH Discretionary.\n";

            if (cboSecc.SelectedIndex == -1)
                strError += "Please select an SECC.\n";

            if (cboBankID.SelectedIndex == -1)
                strError += "Please select a Bank ID.\n";

            if (cboMerchantType.SelectedIndex == -1)
                strError += "Please select a Merchant Type.\n";

            if (cboTest.SelectedIndex == -1)
                strError += "Please select a Test flag.\n";

            if (txtTransRoute.Text.Trim() == string.Empty)
                strError += "Please enter a Trans Route.\n";
            else
            {
                DataBank data = new DataBank();
                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@TransRoute", txtTransRoute.Text));
                DataSet ds = data.Search(prms);
                
                if (ds.Tables[0].Rows.Count == 0)
                    strError += "Invalid Trans Route.\n";
            }

            if (txtAccountNo.Text.Trim() == string.Empty)
                strError += "Please enter an Account No.\n";

            if (txtAccountName .Text.Trim() == string.Empty)
                strError += "Please enter an Account Name.\n";

            if (txtWithdrawTransRoute.Text.Trim() != string.Empty)
            {
                DataBank data = new DataBank();
                ArrayList prms = new ArrayList();
                prms.Add(new SqlParameter("@TransRoute", txtWithdrawTransRoute.Text));
                DataSet ds = data.Search(prms);

                if (ds.Tables[0].Rows.Count == 0)
                    strError += "Invalid Withdraw Trans Route.\n";

            }


            if (strError == string.Empty)
                return true;
            else
            {
                FormHandler.DispalyErrorMessage(strError);
                return false;
            }
        }

        private void frmMerchant_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.Dr != null)
            {
                this.Dr.Close();
                this.Dr = null;
            }
            
        }

     

        public void PopulateMerchantInfo(UltraGridRow row)
        {
            if (row != null)
            {
                txtAchID.Text = row.Cells["AchID"].Value.ToString();
                txtMerchantID.Text = row.Cells["Merchant ID"].Value.ToString();
                txtMerchantName.Text = row.Cells["Merchant Name"].Value.ToString();
                txtTransRoute.Text = row.Cells["Trans Route"].Value.ToString().Trim();
                txtAccountNo.Text = row.Cells["Account No"].Value.ToString().Trim();
                txtAccountName.Text = row.Cells["Account Name"].Value.ToString().Trim();
            }
        }

        private void txtAchID_Leave(object sender, EventArgs e)
        {
            if (txtAchID.Text == string.Empty)
                return;

            UltraGridRow row = PickerHandler.PickMerchant(Convert.ToInt32(txtAchID.Text));
            this.PopulateMerchantInfo(row);

        }

        private void btnAchID_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            this.PopulateMerchantInfo(row);
        }

        private void btnMerchant_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            if (row != null)
                txtFeeID.Text = row.Cells["AchID"].Value.ToString();
        }

        private void btnMerchant2_Click(object sender, EventArgs e)
        {
            UltraGridRow row = PickerHandler.PickMerchant();
            if (row != null)
                txtFundID.Text = row.Cells["AchID"].Value.ToString();
        }

        private void btnStandardMerchant_Click(object sender, EventArgs e)
        {
            this.Risk_Merchant_Defaults(true);
        }

        private void btnHighRiskMerchant_Click(object sender, EventArgs e)
        {
            this.Risk_Merchant_Defaults(false);
        }



        public bool ManuallyWriteReturnFile(int AchID, string MerchantID, DateTime date)
        {
            string strFileSeqNumber = string.Empty;
            string strFile = string.Empty;
            string strLine = string.Empty;

            string FTPCentral = ConfigurationManager.AppSettings["FTPCentral"];

            DataAchProcess data = new DataAchProcess();
            ArrayList prms = new ArrayList();

            prms.Add(new SqlParameter("@AchID", AchID));
            prms.Add(new SqlParameter("@Date", date));

            SqlDataReader dr = data.SelectReturnFileProcessRecreate(prms);

            strFile = FTPCentral + MerchantID + @"\" + date.ToString("yyMMdd") + "01" + ".rtn";
            StreamWriter sw = File.CreateText(strFile);

            while (dr.Read())
            {
                strLine = (char)34 + DataLayer.Field2Str(dr["RefID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransID"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Dec(dr["Amount"]).ToString() + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonCode"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["ReasonDesc"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["Type"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransType"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["AccountNo"]) + (char)34 + ",";
                strLine += (char)34 + DataLayer.Field2Str(dr["TransRoute"]) + (char)34;

                sw.WriteLine(strLine);
            }

            data = null;
            sw.Close();

            return true;
        }

        private void btnRecreateReturnFile_Click(object sender, EventArgs e)
        {
            this.ManuallyWriteReturnFile(Convert.ToInt32(this.txtAchID.Text), txtMerchantID.Text, txtReturnPostedDate.Value);

            MessageBox.Show("Done!");
        }
   

    }
}