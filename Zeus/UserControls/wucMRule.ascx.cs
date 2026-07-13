using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PaymentXP.DataObjects;
using System.Collections;
using PaymentXP.BusinessObjects;

namespace ZeusWeb.UserControls
{
    public partial class wucMRule : wucBaseDataEntry
    {

        public delegate void RuleNameClickHandler(object sender, GridViewCommandEventArgs e);
        public event RuleNameClickHandler RuleNameClick;

        public delegate void GlobalEnabledClickHandler(object sender, bool e);
        public event GlobalEnabledClickHandler GlobalEnabledClick;


        // default to global rule.
        private eControlRuleType _ControlRuleType = eControlRuleType.Global;

        public eControlRuleType ControlRuleType
        {
            get { return _ControlRuleType; }
            set { _ControlRuleType = value; }
        }

        private bool _ShowGlobalEnable = true;

        public bool ShowGlobalEnable
        {
            get { return _ShowGlobalEnable; }
            set { _ShowGlobalEnable = value; }
        }

        public int MerchantID
        {
            get
            {
                return CommonUtility.Util.if_i(ViewState["MerchantID"], 0);
            }
            set
            {
                ViewState["MerchantID"] = value;
            }
        }

        public string RoleUID
        {
            get
            {
                return CommonUtility.Util.if_s(ViewState["RoleUID"], null);
            }
            set
            {
                ViewState["RoleUID"] = value;
            }
        }



        public Dictionary<int, MRule> VSMRule
        {
            get
            {
                if (ViewState["VSMRule"] == null)
                {
                    return null;
                }
                else
                {
                    return (Dictionary<int, MRule>)ViewState["VSMRule"];
                }
            }
            set { ViewState["VSMRule"] = value; }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FormShow("");
            }
        }

        public override void FormShow(string ID)
        {
            if (this.Visible)
            {



                this.VSMRule = this.GetLatestMRule();

                GridView1.DataSource = this.VSMRule.Values;

                if (this.ControlRuleType == eControlRuleType.Global)
                {
                    GridView1.Columns[0].HeaderText = "Global Enabled";
                    GridView1.Columns[3].HeaderText = "Default Parameters";
                }
                else
                {
                    GridView1.Columns[0].HeaderText = "Enabled";
                    GridView1.Columns[3].HeaderText = "Parameters";
                }


                GridView1.DataBind();


            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MRule objMR = (MRule)e.Row.DataItem;


                CheckBox cb = (CheckBox)e.Row.FindControl("cbEnabled");
                cb.Checked = objMR.GlobalEnabled;


                // bind param gridview
                Dictionary<int, MRuleParam> diP = objMR.MRuleParamDict;
                if (diP != null && diP.Count > 0)
                {
                    GridView gvParams = (GridView)e.Row.FindControl("GridView2");
                    gvParams.DataSource = diP.Values;
                    gvParams.DataBind();
                }
            }
        }


        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MRuleParam objMRP = (MRuleParam)e.Row.DataItem;

            }
        }

        public override void FormClear()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// bind the values from the gridview to the VS object
        /// </summary>
        public void MatchGridToVS()
        {
            if (GridView1.Rows != null && GridView1.Rows.Count > 0)
            {
                foreach (GridViewRow gvr in GridView1.Rows)
                {
                    if ((gvr.RowState == DataControlRowState.Normal || gvr.RowState == DataControlRowState.Alternate) && gvr.RowType == DataControlRowType.DataRow)
                    {
                        HiddenField hMRuleID = (HiddenField)gvr.FindControl("hidMRuleID");
                        int rule_id = Convert.ToInt32(hMRuleID.Value);

                        if (this.VSMRule.ContainsKey(rule_id) && this.VSMRule[rule_id] != null)
                        {
                            CheckBox cbEn = (CheckBox)gvr.FindControl("cbEnabled");
                            this.VSMRule[rule_id].GlobalEnabled = cbEn.Checked;
                        }

                        GridView gv2 = (GridView)gvr.FindControl("GridView2");

                        if (gv2 != null && gv2.Rows != null && gv2.Rows.Count > 0)
                        {
                            foreach (GridViewRow gvr2 in gv2.Rows)
                            {
                                HiddenField hfRPM = (HiddenField)gvr2.FindControl("hidMRuleParamID");
                                int rule_param_id = Convert.ToInt32(hfRPM.Value);

                                if (this.VSMRule[rule_id].MRuleParamDict.ContainsKey(rule_param_id) && this.VSMRule[rule_id].MRuleParamDict[rule_param_id] != null)
                                {
                                    TextBox tbVal = (TextBox)gvr2.FindControl("tbValue");
                                    this.VSMRule[rule_id].MRuleParamDict[rule_param_id].ParamValue = tbVal.Text.Trim();
                                }
                            }
                        }
                    }
                }
            }
        }

        public override bool FormSave()
        {
            bool perform = true;
            // save to MRuleMerchant

            try
            {
                this.MatchGridToVS();

                foreach (KeyValuePair<int, MRule> kvp in this.VSMRule)
                {

                    MRule objMR = kvp.Value;

                    switch (this.ControlRuleType)
                    {
                        case eControlRuleType.Merchant:
                            DataMRule.UpdateMRuleMerchant(objMR);

                            // log
                            break;

                        case eControlRuleType.Global:
                            DataMRule.UpdateMRule(objMR);

                            // log
                            break;
                    }

                    if (objMR.MRuleParamDict != null && objMR.MRuleParamDict.Count > 0)
                    {
                        foreach (KeyValuePair<int, MRuleParam> kvp2 in objMR.MRuleParamDict)
                        {
                            switch (this.ControlRuleType)
                            {
                                case eControlRuleType.Merchant:

                                    DataMRule.UpdateMRuleParamMerchant(kvp2.Value);

                                    // log
                                    break;

                                case eControlRuleType.Global:
                                    DataMRule.UpdateMRuleParam(kvp2.Value);

                                    // log
                                    break;

                            }
                        }
                    }
                }
            }
            //catch (Exception ex)
            catch
            {
                perform = false;
            }


            this.VSMRule = this.GetLatestMRule();

            return perform;


        }


        protected void tbDefaultParamValue_TextChanged(object sender, EventArgs e)
        {

            TextBox tb = (TextBox)sender;

            if (CommonUtility.Util.IsValidInt32(tb.Text))
            {
                HiddenField hfMRPID = (HiddenField)tb.Parent.FindControl("hidMRuleParamID");
                HiddenField hfMRID = (HiddenField)tb.Parent.FindControl("hidMRuleID");

                int ruleparam_id = CommonUtility.Util.if_i(hfMRPID.Value, 0);
                int rule_id = CommonUtility.Util.if_i(hfMRID.Value, 0);

                MRuleParam mpr = DataMRule.GetMRuleParam(ruleparam_id, rule_id);

                mpr.Clone();

                if (mpr != null)
                {
                    mpr.DefaultValue = tb.Text;
                    DataMRule.UpdateMRuleParam(mpr);

                    this.UpdateChangelog(mpr);
                }
            }

        }

        private void UpdateChangelog(MRuleParam objMRP)
        {

            string DifferenceLog = "";

            // get differences before you save.
            if (objMRP.MRuleParamClone != null)
            {
                DifferenceLog = objMRP.GetDifferences();
            }

            // no exception, then save if there's any differences
            if (!string.IsNullOrEmpty(DifferenceLog))
            {
                DataUser.GetInstance().InsertChangeLog(
                    "" // not merchant specific, so 
                    , UserSessions.CurrentUser.UserName
                    , CommonUtility.Util.if_s(objMRP.MerchantAppUID, null)
                    , objMRP.MRuleParamID.ToString()
                    , "MRuleParam"
                    , DifferenceLog
                    , UserSessions.PortalUID
                    );

            }
        }


        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            switch (CommonUtility.Util.if_s(e.CommandName).ToLower())
            {
                case "rulename":

                    if (this.RuleNameClick != null)
                    {
                        this.RuleNameClick(sender, e);
                    }
                    break;

            }


        }

        public Dictionary<int, MRule> GetLatestMRule()
        {
            Dictionary<int, MRule> di = null;

            Hashtable prms = new Hashtable();

            if (this.ControlRuleType == eControlRuleType.Merchant)
            {
                // merchant specific
                prms.Add("@MerchantID", this.MerchantID);
                prms.Add("@RoleUID", this.RoleUID);
                di = DataMRule.GetMRuleMerchant(prms);
            }
            else
            {
                // global
                prms.Add("@RoleUID", this.RoleUID);
                di = DataMRule.SearchMRule(prms);
            }

            return di;
        }

        public override void FormNew()
        {
            throw new NotImplementedException();
        }

        public override bool FormDelete()
        {
            throw new NotImplementedException();
        }

        public override bool FormDataCheck()
        {
            throw new NotImplementedException();
        }

        public override void FormCancel()
        {
            throw new NotImplementedException();
        }

        public override void ToggleButtons()
        {
            throw new NotImplementedException();
        }

        protected void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            HiddenField hf = (HiddenField)cb.Parent.FindControl("hidMRuleID");
            int rule_id = CommonUtility.Util.if_i(hf.Value, 0);

            if (rule_id > 0)
            {

                Hashtable prms = new Hashtable();
                prms.Add("@MRuleID", rule_id);
                Dictionary<int, MRule> di = DataMRule.SearchMRule(prms);
                if (di != null && di.Count == 1 && di.ContainsKey(rule_id))
                {
                    di[rule_id].GlobalEnabled = cb.Checked;
                    DataMRule.UpdateMRule(di[rule_id]);
                    // log here
                }

                if (this.GlobalEnabledClick != null)
                {
                    this.GlobalEnabledClick(sender, cb.Checked);
                }
            }

            
        }

        protected void GridView1_PreRender(object sender, EventArgs e)
        {

            GridView1.Columns[0].Visible = this.ShowGlobalEnable;

        }

     

      
    }
}