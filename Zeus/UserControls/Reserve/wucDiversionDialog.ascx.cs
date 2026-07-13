using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Infragistics.Web.UI.LayoutControls;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls.Reserve
{

    public partial class wucDiversionDialog : wucBaseDataEntry
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

        public int DiversionID
        {
            get
            {
                if (ViewState["DiversionID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (int)ViewState["DiversionID"];
                }
            }
            set { ViewState["DiversionID"] = value; }
        }

        public WebDialogWindow WinInstance
        {
            get { return dlgDivertedMethod; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (this.ZID > 0)
                {
                    this.FormShow("");
                }

                LoadDropDowns();
            }
        }

        private void LoadDropDowns()
        {
            //LookupTableHandler.LoadRDBDiversionType(DiversionTypeID, false);

            LookupTableHandler.LoadDiversionType(DiversionTypeID);



        }

        public override void FormShow(string ID)
        {

            WebUtil.SetUserSpecificDisplayMode(DateDiverted);
            WebUtil.SetUserSpecificDisplayMode(DateUndiverted);
            if (this.DiversionID > 0)
            {
                // edit mode
                this.Adding = false;
                this.EditMode = true;
                FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

                RDBDiversion objRM = DataReserve.GetDiversion(this.DiversionID);

                trReserveRate.Visible = objRM.DiversionTypeID == 2;
                trResolution.Visible = true;
                trDateUndiverted.Visible = true;

                if (objRM != null)
                {
                    FormBinding.BindObjectToControls(objRM, pnlDetail);

                    ReserveRate.Value = objRM.ReserveRate * 100;

                    if (objRM.DateUndiverted == DateTime.MinValue)
                    {
                        // by default, it shows the datetimemin value, its ugly, so just show an empty string instead.
                        DateUndiverted.Text = "";
                        btnSave.Enabled = true;


                    }
                    else
                    {
                        // disable the save button if it's already been diverted.

                        btnSave.Enabled = false;
                        this.EditMode = false;
                        // if it's been undiverted, then make the whole thing just readonly.
                        FormHandler.SetControlEditMode(pnlDetail, this.EditMode);
                    }

                }

            }
            else
            {
                this.Adding = true;
                this.EditMode = true;
                DateDiverted.Text = DateTime.Now.ToString();
                DateUndiverted.Text = "";

                FormHandler.SetControlEditMode(pnlDetail, this.EditMode);

                trResolution.Visible = false;
                trDateUndiverted.Visible = false;
            }
        }

        public override void FormClear()
        {

            FormHandler.ClearAllControls(pnlDetail);


        }

        public override bool FormSave()
        {
            bool ret = false;

            if (this.ZID > 0 && this.FormDataCheck() && Page.IsValid)
            {
                RDBDiversion objRM = null;

                if (!this.FormDataCheck())
                    return false;

                if (this.DiversionID > 0)
                {
                    // editing an existing
                    objRM = DataReserve.GetDiversion(this.DiversionID);

                    FormBinding.BindControlsToObject(objRM, pnlDetail);

                    objRM.ReserveRate = ReserveRate.ValueDecimal / 100;

                    if (DataReserve.UpdateReserveDiversion(objRM) > 0)
                    {
                        ret = true;
                    }
                }
                else
                {
                    // adding a new one
                    objRM = new RDBDiversion();

                    FormBinding.BindControlsToObject(objRM, pnlDetail);
                    objRM.DivertedBy = UserSessions.CurrentUser.UserName;
                    objRM.ZID = this.ZID;
                    objRM.ReserveRate = ReserveRate.ValueDecimal / 100;
                    objRM.Bank = ((int)(LookupTableHandler.GetBankByMid(UserSessions.CurrentMerchantApp.SettlePlatformMid))).ToString();

                    DataReserve.InsertReserveDiversion(objRM);

                    if (objRM.DiversionID > 0)
                    {
                        ret = true;
                    }
                }
            }

            return ret;
        }

        public override void FormNew()
        {
            this.Adding = true;
            this.EditMode = true;
            FormHandler.ClearAllControls(pnlDetail);
            FormHandler.SetControlEditMode(pnlDetail, true);
            this.ZID = 0;
            this.DiversionID = 0;
            this.DivertedBy.Text = UserSessions.CurrentUser.UserName;
            DateDiverted.Text = DateTime.Now.ToString();
            DateUndiverted.Text = "";

            trResolution.Visible = false;
            trDateUndiverted.Visible = false;
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            bool perform = true;
            string error = string.Empty;

            //if (Convert.ToDateTime(DateUndiverted.Value) != DateTime.MinValue && Convert.ToDateTime(DateUndiverted.Value) < Convert.ToDateTime(DateDiverted.Value))
            //    error += "Undiverted Date has to be greater than diverted date.<br>";

            //if (Convert.ToDateTime(DateDiverted.Value) == DateTime.MinValue)
            //    error += "Please select a Diverted Date.<br>";

            //if (DiversionReasonID.SelectedIndex == 0)
            //    error += "Please select a Reason.<br>";

            if (DiversionTypeID.SelectedIndex == 0)
                error += "Please select a Type.<br>";


            if (!string.IsNullOrEmpty(error))
            {

                perform = false;
            }
            lblError.Text = error;
            return perform;
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

                dlgDivertedMethod.WindowState = DialogWindowState.Hidden;
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            this.FormClear();
            dlgDivertedMethod.WindowState = DialogWindowState.Hidden;
        }

        protected void DivertedTypeID_SelectedIndexChanged(object sender, EventArgs e)
        {
            trReserveRate.Visible = DiversionTypeID.SelectedItem.Value == "2";
        }


    }
}