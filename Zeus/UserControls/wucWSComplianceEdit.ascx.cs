using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.DataObjects;
using PaymentXP.BusinessObjects;
using Infragistics.WebUI.WebDataInput;
using PaymentXP.Facade;
using System.Text;
using System.Data;
using System.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using CommonUtility;



public partial class UserControls_wucWSComplianceEdit : wucBaseDataEntry
{
    public TimeZones TimeZoneID
    {
        get { return (TimeZones)ViewState["TimeZoneID"]; }
        set { ViewState["TimeZoneID"] = value; }

    }

    public delegate void MyEventHandler(object sender, string e);
    public event MyEventHandler coolevent;

    public delegate void MyEmailClick(string pTo, string pFrom, string pCC, string pSubject, List<string> liFiles, string pBody, string pdf_location, string pdf_clean_name);
    public event MyEmailClick emailclick;

    public decimal VSRunningPossible
    {
        get { return (decimal)(ViewState["VSRunningPossible"] ?? 0m); }
        set { ViewState["VSRunningPossible"] = value; }
    }

    public decimal VSRunningMissed
    {
        get { return (decimal)(ViewState["VSRunningMissed"] ?? 0); }
        set { ViewState["VSRunningMissed"] = value; }
    }

    public decimal VSRunningEarned
    {
        get { return (decimal)(ViewState["VSRunningEarned"] ?? 0); }
        set { ViewState["VSRunningEarned"] = value; }
    }


    public string Answer_HowLongIsTheTrialPeriod = "";

    // these question_* variables are special in that there are IF conditions in the row binding that assign css classes.
    // for instance, if you land on a question and your id is not question_website_must_be_active, then it will assign a "mydependant" class to you.
    // another instance, if your sort order is numerically greater than question_is_ecommerce, then your visibility will be controlled by whether it's ON or OFF.




    //used when looping thru the sorted gridview rows.
    public bool show_is_active = true;
    public bool show_is_ecommerce = true;
    public bool show_is_offer = true;

    /// <summary>
    /// when a website is non-compliant, put the description list here.
    /// </summary>
    private List<string> liBadList = null;

    public string vsTicketUID
    {
        get
        {
            return CommonUtility.Util.if_s(ViewState["vsTicketUID"], null);
        }
        set
        {
            ViewState["vsTicketUID"] = value;
        }
    }

    public int vsWSComplianceID
    {
        get
        {
            return CommonUtility.Util.if_i(ViewState["vsWSComplianceID"], 0);
        }
        set
        {
            ViewState["vsWSComplianceID"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            this.TimeZoneID = UserSessions.CurrentUser.TimeZone;
            this.vsWSComplianceID = CommonUtility.Util.if_i(Request.QueryString["WSComplianceID"], 0);

            this.vsTicketUID = CommonUtility.Util.if_s(Request.QueryString["TicketUID"], null);

            this.FormShow(null);
        }
    }

    public override void FormShow(string ID)
    {

        FormHandler.SetControlEditMode(pnlDetails, this.EditMode);

        Hashtable prmsWS = new Hashtable();
        if (this.vsWSComplianceID > 0)
        {
            prmsWS.Add("@WSComplianceID", this.vsWSComplianceID);
        }

        if (!string.IsNullOrEmpty(this.vsTicketUID))
        {
            prmsWS.Add("@TicketUID", this.vsTicketUID);
        }

        // setup our WSCompliance object
        WSCompliance obj = DataWSCompliance.SelectWSCompliance(prmsWS, UserSessions.CurrentUser.TimeZone);

        if (obj == null)
        {
            //wucMessage2.AddMessageError("Could not get website compliance object!");
            return;
        }


        // get our current ticket associated with this Website Compliance Review
        Hashtable prms3 = new Hashtable();
        prms3.Add("@TicketID", obj.TicketID);
        Ticket CurrTicket = DataTicket.GetInstance().GetTicket(prms3,UserSessions.CurrentUser.TimeZone);

        if (CurrTicket == null && !string.IsNullOrEmpty(CurrTicket.TicketUID))
        {
            wucMessage2.AddMessageError("Could not get correct ticket!");
            return;
        }

        if (CurrTicket.StatusID.ToUpper() == Ticket.TICKET_CLOSE)
        {
            // turn off buttons. tickets are closed already
            btnCancel.Enabled = false;
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            btnCancelReview.Enabled = false;
        }


        // on form load, we ALWAYS overwrite this object so that we always have a fresh copy.
        //UserSessions.diCurrentWSCompliance[obj.ID] = obj;

        UserSessions.CurrentWSCompliance = obj;


        this.load_compliance_checklist(obj);

        this.load_documents(CurrTicket);

        this.refresh_displays(obj, CurrTicket);

        switch (obj.StatusUID.ToUpper())
        {
            case Constants.COMPLIANCE_COMPLIANT:
            case Constants.COMPLIANCE_NONCOMPLIANT:
            case Constants.COMPLIANCE_REVIEWCOMPLETE:
                pnlMore.Visible = true;

                // only display the email link if risk level in 3 or 4.
                btnInitiateEmail.Visible = (obj.RiskIndex == 3 || obj.RiskIndex == 4) ? true : false;

                if (btnInitiateEmail.Visible)
                {
                    // note: the initiate email button should only be used by a select group of people with the compliance department.
                    // we want to whitelist the users in the web.config

                    // all usernames who have access to the button. default to kelvin.
                    //string email_button_whitelist = CommonUtility.Util.if_s(ConfigurationManager.AppSettings["ComplianceEmailButton"], "kcho");

                    User user = UserSessions.CurrentUser;

                    UserForm frm = null;

                    bool isVisible = false;

                    if (user.UserForms.TryGetValue("FRMCOMPLIANCE", out frm) && frm.HasAccess)
                    {
                        if (frm.ControlObjects == null)
                            DataAccess.DataUserDao.GetUserObjectPermissions(frm, user, UserSessions.PortalUID);

                        foreach (ControlObject obj1 in frm.ControlObjects)
                        {
                            if (obj1.Type.ToUpper() == "BUTTON" && obj1.ID.ToUpper() == "INITIATEEMAIL")
                            {
                                isVisible = (obj1.IsVisible && obj1.IsEnabled);
                                break;
                            }
                        }
                    }

                    //// convert the string into a list.
                    //List<string> liWL_username = CommonUtility.Util.Explode(email_button_whitelist, new char[] { ',' });


                    //// set visbility if the current user is in the white list.
                    //btnInitiateEmail.Visible = liWL_username.Contains(UserSessions.CurrentUser.UserName.ToLower().Trim());

                    btnInitiateEmail.Visible = isVisible;

                }

                break;

            default:
                pnlMore.Visible = false;
                break;
        }

      
    }

    protected void load_compliance_checklist(WSCompliance objWSC)
    {
        WSCompliance obj = objWSC;

        // load checklist grid
        if (obj != null && obj.diComplianceItem != null && obj.diComplianceItem.Count > 0)
        {
            GridView1.DataSource = WSComplianceFacade.GetItemsNested(obj);
            GridView1.DataBind();
        }
    }

    protected void load_documents(Ticket t)
    {
        // load document grid
        Hashtable prms = new Hashtable();


        //prms.Add("@PrimaryKeyID", UserSessions.CurrentWSCompliance.TicketID);
        prms.Add("@PrimaryKeyID", t.TicketID);
        prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.Tickets);
        List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);
        if (li != null && li.Count > 0)
        {
            GridView2.DataSource = li;
            GridView2.DataBind();
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            this.VSRunningPossible = 0;
            this.VSRunningMissed = 0;
            this.VSRunningEarned = 0;
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("WSComplianceItemAnswerUID");

            WSComplianceItem obj = (WSComplianceItem)e.Row.DataItem;

            if (ddl != null)
            {
                ddl.Items.Clear();

                TextBox tb = (TextBox)e.Row.FindControl("tbComment");
                tb.Text = obj.Comment;

                switch ((eWSComplianceQuestionType)obj.WSComplianceQuestionTypeID)
                {
                    case eWSComplianceQuestionType.ComplianceDropdown:
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Compliant", Constants.COMPLIANCE_ANSWER_COMPLIANT));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Non-Compliant", Constants.COMPLIANCE_ANSWER_NONCOMPLIANT));
                        //ddl.Items.Add(new ListItem("Unclear", Constants.COMPLIANCE_ANSWER_UNCLEAR));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", Constants.COMPLIANCE_ANSWER_NA));

                        break;

                    case eWSComplianceQuestionType.RequiredText:
                        ddl.Visible = false;
                        break;

                    case eWSComplianceQuestionType.CustomDropdown:

                        // fetch the selected value from the first part of the comment, delimited by the pipe.
                        string[] arr = obj.Comment.Split(new char[] { '|' });


                        // special case
                        if (obj.WSComplianceItemID == WSComplianceFacade.question_how_long_is_the_trial_period)
                        {
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));

                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", Constants.COMPLIANCE_ANSWER_NA));

                            for (int i = 1; i <= 30; i++)
                            {
                                if (i == 1)
                                {
                                    ddl.Items.Add(new System.Web.UI.WebControls.ListItem(string.Format("{0} day", i.ToString()), i.ToString()));
                                }
                                else
                                {
                                    ddl.Items.Add(new System.Web.UI.WebControls.ListItem(string.Format("{0} days", i.ToString()), i.ToString()));
                                }

                            }

                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Over 30 Days", "999"));

                            if (arr.Length > 1)
                            {
                                ddl.SelectedValue = arr[0];
                                tb.Text = arr[1];
                            }

                        }



                        if (obj.WSComplianceItemID == WSComplianceFacade.question_website_must_be_active)
                        {
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Yes", Constants.COMPLIANCE_ANSWER_YES));
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("No", Constants.COMPLIANCE_ANSWER_NO));

                            if (arr.Length > 1)
                            {
                                ddl.SelectedValue = arr[0];
                                tb.Text = arr[1];
                            }

                        }


                        break;

                    case eWSComplianceQuestionType.YesNoDropdown:
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("No", Constants.COMPLIANCE_ANSWER_NO));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Yes", Constants.COMPLIANCE_ANSWER_YES));
                        break;

                    case eWSComplianceQuestionType.PassNoPassDropdown:
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Pass", Constants.COMPLIANCE_ANSWER_PNP_PASS));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("No Pass", Constants.COMPLIANCE_ANSWER_PNP_NOPASS));

                        if (obj.HasNA)
                        {
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", Constants.COMPLIANCE_ANSWER_PNP_NA));
                        }

                        break;

                    case eWSComplianceQuestionType.AcceptabilityDropdown:
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select --", ""));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Acceptable", Constants.COMPLIANCE_ANSWER_ACCEPT_ACCEPTABLE));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Needs Work", Constants.COMPLIANCE_ANSWER_ACCEPT_NEEDSWORK));
                        ddl.Items.Add(new System.Web.UI.WebControls.ListItem("Unacceptable", Constants.COMPLIANCE_ANSWER_ACCEPT_UNACCEPTABLE));

                        if (obj.HasNA)
                        {
                            ddl.Items.Add(new System.Web.UI.WebControls.ListItem("N/A", Constants.COMPLIANCE_ANSWER_ACCEPT_NA));
                        }

                        break;
                }

                //eluxa: this is a quick fix for a bug where the trial period days drop down list does not
                //populate the correct value saved by the user. this line of code is the cause for the
                //bug because it looks like the question does not have a WSComplianceItemAnswerUID assigned
                //if (obj.WSComplianceItemID != WSComplianceFacade.question_how_long_is_the_trial_period)
                //{
                //    if (string.IsNullOrEmpty(obj.WSComplianceItemAnswerUID))
                //    {
                //        ddl.SelectedValue = "";
                //    }
                //    else if (obj.WSComplianceItemAnswerUID.ToUpper() == Constants.COMPLIANCE_ANSWER_NA)
                //    {
                //       // ddl.
                //    }
                //    else
                //    {
                //        ddl.SelectedValue = obj.WSComplianceItemAnswerUID.ToUpper();
                //    }
                //}

                /////////////////////////////////////////////

                // assign the answer to the dropdownlist here. 
                // there are special case where we don't want to do that. for instance, with the "How long is the trial period" question, this is done in
                // the area above that creates the values for the DDL
                switch (obj.WSComplianceItemID)
                {
                    case WSComplianceFacade.question_how_long_is_the_trial_period:
                        // ready set above.
                        break;

                    default:
                        ddl.SelectedValue = obj.WSComplianceItemAnswerUID.ToUpper();
                        break;
                }




                if (obj.WSComplianceItemID == WSComplianceFacade.question_how_long_is_the_trial_period)
                {

                    this.Answer_HowLongIsTheTrialPeriod = obj.Comment;
                }

                if (WSComplianceFacade.question_website_must_be_active == obj.WSComplianceItemID)
                {
                    if (ddl.SelectedValue == Constants.COMPLIANCE_ANSWER_YES)
                    {
                        this.show_is_active = true;
                    }
                    else if (ddl.SelectedValue == Constants.COMPLIANCE_ANSWER_NO)
                    {
                        this.show_is_active = false;
                    }

                    e.Row.Visible = true;
                }
                else
                {
                    e.Row.Visible = (this.show_is_active && this.show_is_ecommerce && this.show_is_offer);

                    if (e.Row.Visible)
                    {
                        if (WSComplianceFacade.question_is_ecommerce == obj.WSComplianceItemID)
                        {
                            if (ddl.SelectedValue == Constants.COMPLIANCE_ANSWER_YES)
                            {
                                this.show_is_ecommerce = true;
                            }
                            else if (ddl.SelectedValue == Constants.COMPLIANCE_ANSWER_NO)
                            {
                                this.show_is_ecommerce = false;
                            }

                            e.Row.Visible = true;
                        }
                    }


                }


                if (e.Row.Visible)
                {
                    // i'm still visible, but only if my parent says i'm able to be.

                    if (obj.ParentID != 0)
                    {
                        string myanswer = UserSessions.CurrentWSCompliance.diComplianceItem[obj.ParentID].WSComplianceItemAnswerUID.ToUpper();

                        if (myanswer == Constants.COMPLIANCE_ANSWER_NO)
                        {
                            e.Row.Visible = false;
                        }
                        else if (myanswer == Constants.COMPLIANCE_ANSWER_YES)
                        {
                            e.Row.Visible = true;
                        }

                    }
                }

                // special case, we want to make this ddl read only. the value of this is controlled by the question "How long is the trial period"
                if (WSComplianceFacade.question_trial_within == obj.WSComplianceItemID)
                {
                    ddl.Enabled = false;

                    string answer_howlong = this.Answer_HowLongIsTheTrialPeriod.Split(new char[] { '|' })[0].Trim().ToUpper();

                    ddl.SelectedValue = answer_howlong;

                }

                // special case, the comments are always visible no matter waht.
                if (WSComplianceFacade.question_comments == obj.WSComplianceItemID)
                {
                    e.Row.Visible = true;
                }

                switch (ddl.SelectedValue.ToUpper())
                {
                    case Constants.COMPLIANCE_ANSWER_COMPLIANT:
                    case Constants.COMPLIANCE_ANSWER_ACCEPT_ACCEPTABLE:
                    //case Constants.COMPLIANCE_ANSWER_YES:
                    case Constants.COMPLIANCE_ANSWER_PNP_PASS:
                        e.Row.CssClass = "RowCompliant";
                        break;

                    case Constants.COMPLIANCE_ANSWER_NONCOMPLIANT:
                    case Constants.COMPLIANCE_ANSWER_PNP_NOPASS:
                        e.Row.CssClass = "RowNonCompliant";
                        break;


                    case Constants.COMPLIANCE_ANSWER_ACCEPT_NEEDSWORK:
                        e.Row.CssClass = "RowNeedsWork";
                        break;

                    case Constants.COMPLIANCE_ANSWER_ACCEPT_UNACCEPTABLE:
                        e.Row.CssClass = "RowPinkShort";
                        break;

                    case Constants.COMPLIANCE_ANSWER_NO:
                        //e.Row.CssClass = "RowNo";
                        break;

                    case Constants.COMPLIANCE_ANSWER_NA:
                    case Constants.COMPLIANCE_ANSWER_PNP_NA:
                    case Constants.COMPLIANCE_ANSWER_ACCEPT_NA:
                        e.Row.CssClass = "RowNa";
                        break;
                }

                if ((obj.WSComplianceWeightID == eWSComplianceWeight.Medium || obj.WSComplianceWeightID == eWSComplianceWeight.High)
                        && WSComplianceFacade.IsLowScoreAnswer(obj.WSComplianceItemAnswerUID))
                {
                    e.Row.CssClass = "RowPink";
                }


                Label lbPointsPossible = (Label)e.Row.FindControl("lblPointsPossible");
                Label lbPointsMissed = (Label)ddl.Parent.FindControl("lblPointsMissed");
                Label lbPointsEarned = (Label)ddl.Parent.FindControl("lblPointsEarned");

                int maxpoints = CommonUtility.Util.if_i(obj.MaxPoints, 0);

                if (maxpoints == 0 || WSComplianceFacade.IsAnswerNA(obj.WSComplianceItemAnswerUID))
                {
                    lbPointsPossible.Text = "-";
                    lbPointsEarned.Text = "-";
                    lbPointsMissed.Text = "-";
                }
                else
                {
                    if (e.Row.Visible)
                    {

                        lbPointsPossible.Text = string.Format("{0:f1}", maxpoints);

                        decimal dpointsearned = WSComplianceFacade.GetPointsEarned(ddl.SelectedValue, obj.MaxPoints);
                        decimal dpointsmissed = (string.IsNullOrWhiteSpace(ddl.SelectedValue)) ? 0 : obj.MaxPoints - dpointsearned;

                        lbPointsEarned.Text = string.Format("{0:f1}", dpointsearned);
                        lbPointsMissed.Text = string.Format("{0:f1}", dpointsmissed);

                        this.VSRunningPossible += maxpoints;
                        this.VSRunningEarned += dpointsearned;
                        this.VSRunningMissed += dpointsmissed;

                        if (dpointsmissed > 0)
                        {
                            lbPointsMissed.ForeColor = System.Drawing.Color.Red;
                            lbPointsMissed.Text = string.Format("({0})", lbPointsMissed.Text);
                        }
                        else
                        {
                            lbPointsMissed.ForeColor = System.Drawing.Color.Black;
                        }
                    }
                }
            }

        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lbPointsPossibleFooter = (Label)e.Row.FindControl("lblPointsPossibleFooter");
            Label lbPointsMissedFooter = (Label)e.Row.FindControl("lblPointsMissedFooter");
            Label lbPointsEarnedFooter = (Label)e.Row.FindControl("lblPointsEarnedFooter");

            lbPointsPossibleFooter.Text = string.Format("{0:f1}", this.VSRunningPossible);
            lbPointsMissedFooter.Text = string.Format("{0:f1}", this.VSRunningMissed);
            lbPointsEarnedFooter.Text = string.Format("{0:f1}", this.VSRunningEarned);
        }
    }

    public override void FormClear()
    {
        throw new NotImplementedException();
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
        //UserSessions.diCurrentWSCompliance.Remove(this.vsWSComplianceID);
        Response.Redirect(WebUtil.GetMyUrl());
    }

    public override void ToggleButtons()
    {
        btnEdit.Enabled = !btnEdit.Enabled;
        btnSave.Enabled = !btnSave.Enabled;
        btnCancel.Enabled = !btnCancel.Enabled;
        btnSubmit.Enabled = !btnSubmit.Enabled;

        //apply permissions here for users that initiated the review or have global or manager access levels
        //the ability to close close reviews
        if (UserSessions.CurrentUser.AccessLevelUID.ToUpper().Equals(Constants.ACCESS_LEVEL_GLOBAL)
            || UserSessions.CurrentUser.AccessLevelUID.ToUpper().Equals(Constants.ACCESS_LEVEL_MANAGER)
            || UserSessions.CurrentTicket.UserCreatedUserUID.Equals(UserSessions.CurrentUser.UID))
            btnCancelReview.Enabled = !btnCancelReview.Enabled;
    }

    protected void tbrTools_ButtonClicked(object sender, Infragistics.WebUI.WebDataInput.ButtonEventArgs e)
    {
        WebImageButton btn = (WebImageButton)sender;

        string url = string.Empty;
        switch (btn.Text)
        {
            case "Edit":
                this.EditMode = true;
                FormShow(null);
                this.ToggleButtons();
                break;

            case "Cancel":
                this.EditMode = false;
                this.FormCancel();
                break;

            case "Save":

                if (this.FormSave())  // valid input values.
                {
                    this.EditMode = false;
                    FormShow(null);
                    wucMessage2.AddMessageStatus("Checklist Saved.");
                    this.ToggleButtons();
                }
                else
                {
                    wucMessage2.AddMessageError("ERROR: Could not update");
                }
                break;


            case "Submit":

                this.FormSave();

                string result = null;

                int risk_level = 0;

                decimal points_possible = 0;
                decimal points_earned = 0;

                List<string> liError = this.CheckCompliance(UserSessions.CurrentWSCompliance, ref result, ref risk_level, ref points_possible, ref points_earned);

                if (CommonUtility.Util.IsValidGuid(result))
                {

                    string template = "{0}. <br />Risk Level: {1} <br />Rating: {2}";

                    if (result == Constants.COMPLIANCE_COMPLIANT)
                    {
                        this.coolevent(null, string.Format(template, "Compliant", risk_level, "N/A"));
                    }
                    else if (result == Constants.COMPLIANCE_NONCOMPLIANT)
                    {
                        this.coolevent(null, string.Format(template, "Non-Compliant", risk_level, "N/A"));
                    }
                    else if (result == Constants.COMPLIANCE_WEBSITEINACTIVE)
                    {
                        this.coolevent(null, string.Format(template, "Complete (Website Inactive)", risk_level, "N/A"));
                    }
                    else if (result == Constants.COMPLIANCE_NOECOMMERCE)
                    {
                        this.coolevent(null, string.Format(template, "Complete (No E-Commerce)", risk_level, "N/A"));
                    }
                    else if (result == Constants.COMPLIANCE_REVIEWCOMPLETE)
                    {
                        this.coolevent(null, string.Format(template, "Complete", risk_level.ToString(), string.Format("{0:f0}%", (points_earned / points_possible) * 100)));
                    }
                    else
                    {
                        wucMessage2.AddMessageError("ERROR: could not submit website review");
                    }
                }
                else
                {
                    if (liError != null)
                    {
                        foreach (string error in liError)
                        {
                            wucMessage2.AddMessageError(error);
                        }
                    }
                }

                break;

            case "Cancel Review":

                CancelReview();

                Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?ComplianceCancelled=true&MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);

                break;

        }
    }

    public override bool FormSave()
    {
        bool perform = true;

        // update the checklist items.
        foreach (GridViewRow gvr in GridView1.Rows)
        {
            perform = false;

            if (gvr.RowType == DataControlRowType.DataRow)
            {
                HiddenField hfWSCAID = (HiddenField)gvr.FindControl("hidWSComplianceAssocID");
                TextBox tbComm = (TextBox)gvr.FindControl("tbComment");
                DropDownList ddl = (DropDownList)gvr.FindControl("WSComplianceItemAnswerUID");

                WSComplianceItem objItem = DataWSCompliance.SelectWSComplianceAssoc(CommonUtility.Util.if_i(hfWSCAID.Value, 0));

                if (objItem != null && objItem.WSComplianceAssocID > 0)
                {
                    // WE NEVER SAVE PIPE. the pipe is used a delimiter for custom fields.
                    objItem.Comment = tbComm.Text.Trim().Replace('|', ' ');


                    objItem.WSComplianceItemAnswerUID = ddl.SelectedValue;

                    objItem.UserUpdated = UserSessions.CurrentUser.UserName;
                    perform = DataWSCompliance.UpdateWSComplianceAssoc(objItem);
                }
            }

            if (perform == false)
            {
                break;
            }
        }


        // update the status of the ticket too.
        if (perform
            && UserSessions.CurrentTicket != null
            && (
                UserSessions.CurrentTicket.StatusID.ToUpper() == Ticket.TICKET_OPEN
                || UserSessions.CurrentTicket.StatusID.ToUpper() == Ticket.TICKET_PENDING
                || UserSessions.CurrentTicket.StatusID.ToUpper() == Ticket.TICKET_ASSIGNED
                )
            )
        {
            UserSessions.CurrentTicket.StatusID = Ticket.TICKET_PENDING;
            UserSessions.CurrentTicket.UserModified = UserSessions.CurrentUser.UserName;
            UserSessions.CurrentTicket.UserID = UserSessions.CurrentUser.UID;

            DataTicket.GetInstance().UpdateTicket(UserSessions.CurrentTicket,this.TimeZoneID);
        }

        if (perform)
        {
            // update our compliance object.
            Hashtable ht = new Hashtable();
            ht.Add("@WSComplianceID", this.vsWSComplianceID);
            UserSessions.CurrentWSCompliance = DataWSCompliance.SelectWSCompliance(ht, UserSessions.CurrentUser.TimeZone);
        }

        return perform;
    }

    // this is called externally from the parent page.
    public bool FormSubmit()
    {

        bool ret = false;

        // NOTE: for performance, we dont wnat to save the form here, because we're under the assumption that it will be saved at compliance time.
        // double check to make sure everything is good when we have this commented out.
        this.FormSave();

        string retval = null;
        int risk_level = 0;
        decimal points_possible = 0;
        decimal points_earned = 0;

        // check if what is saved is compliant
        List<string> liError = this.CheckCompliance(UserSessions.CurrentWSCompliance, ref retval, ref risk_level, ref points_possible, ref points_earned);


        if (CommonUtility.Util.IsValidGuid(retval))
        {
            if (retval == Constants.COMPLIANCE_COMPLIANT)
            {
                UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_COMPLIANT;
            }
            else if (retval == Constants.COMPLIANCE_NONCOMPLIANT)
            {
                UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_NONCOMPLIANT;
            }
            else if (retval == Constants.COMPLIANCE_WEBSITEINACTIVE)
            {
                UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_WEBSITEINACTIVE;
                UserSessions.CurrentWSCompliance.RiskIndex = 5;
            }
            else if (retval == Constants.COMPLIANCE_NOECOMMERCE)
            {
                UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_NOECOMMERCE;
                UserSessions.CurrentWSCompliance.RiskIndex = 5;
            }
            else if (retval == Constants.COMPLIANCE_REVIEWCOMPLETE)
            {
                UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_REVIEWCOMPLETE;
                UserSessions.CurrentWSCompliance.PointsEarned = points_earned;
                UserSessions.CurrentWSCompliance.PointsPossible = points_possible;
                UserSessions.CurrentWSCompliance.RiskIndex = risk_level;

            }
            else
            {
                wucMessage2.AddMessageError("Error: Cannot submit");
                return false;
            }

            UserSessions.CurrentWSCompliance.CompletedBy = UserSessions.CurrentUser.UserName;
            UserSessions.CurrentWSCompliance.UserUpdated = UserSessions.CurrentUser.UserName;

            DataWSCompliance.UpdateWSCompliance(UserSessions.CurrentWSCompliance);

            ret = true;

            // close ticket.
            UserSessions.CurrentTicket.StatusID = Ticket.TICKET_CLOSE;
            DataTicket.GetInstance().UpdateTicket(UserSessions.CurrentTicket,this.TimeZoneID);

            bool HasBeenCUApproved = DataMerchantApp.GetInstance().HasStatus(
                UserSessions.CurrentMerchantApp.MerchantAppUID,
                PaymentXP.BusinessObjects.Constants.QUEUESTATUS_CU_APPROVED);



            if (HasBeenCUApproved)
            {

                // i refresh the displays to update them. also, upon saving, we use the labels to populate the ticket.
                this.refresh_displays(UserSessions.CurrentWSCompliance, UserSessions.CurrentTicket);


                DateTime dt = DateTime.Today.AddDays(1);

                if (UserSessions.CurrentWSCompliance.StatusUID == Constants.COMPLIANCE_NONCOMPLIANT)
                {

                    // if website was non-compliant, then we create risk a new ticket to invesitage.

                    Ticket ticket = new Ticket();

                    ticket.MerchantAppUID = UserSessions.CurrentTicket.MerchantAppUID;
                    ticket.AgentUID = UserSessions.CurrentTicket.AgentUID;
                    ticket.StatusID = Ticket.TICKET_OPEN;
                    ticket.UserID = null;
                    ticket.DepartmentID = "7"; // RISK. let's not hard code this.
                    ticket.CategoryID = "961"; // "non-compliance" lets not hard code this
                    ticket.ParentID = "960"; // "website review"
                    ticket.Problem = string.Format(@"TicketID: {0}
Completed By: {1}
Status: {2}
Rating: {3}
Risk Level: {4}"
    , UserSessions.CurrentTicket.TicketID
    , UserSessions.CurrentWSCompliance.CompletedBy
    , lblResult.Text
    , lblComplianceRating.Text
    , lblRiskIndex.Text);

                    ticket.DateCreated = DateTime.Now;
                    ticket.DateModified = DateTime.Now;
                    ticket.UserCreated = UserSessions.CurrentUser.UserName;
                    ticket.UserModified = UserSessions.CurrentUser.UserName;
                    ticket.Priority = "Low";
                    ticket.TicketSource = "i"; // "i" is for internal
                    ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                    ticket.AttentionReq = true;
                    ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
                    ticket.TimeZone = "6";
                    ticket.OfficeID = UserSessions.CurrentMerchantApp.Office.GetHashCode();
                    //ticket.Origin = 4; // Internal

                    DataTicket.GetInstance().InsertTicket(ticket,this.TimeZoneID);

                    if (ticket.TicketUID != "-1")
                    {
                        // good ticket, save it in the session.
                        UserSessions.CurrentTicket = ticket;

                        // send out apppriate emails!!
                        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false,false);
                        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
                        //PXP-2955
                        FormHandler.InsertMerchantNotesForTicket(ticket.MerchantAppUID, ticket.UserCreated, ticket);
                    }
                    else
                    {
                        wucMessage2.AddMessageError("ERROR: Could not create website compliance ticket for Risk.");
                        Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
                    }

                }
                else if (UserSessions.CurrentWSCompliance.StatusUID == Constants.COMPLIANCE_WEBSITEINACTIVE)
                {

                    // if website was inactive, then we create risk a new ticket to invesitage.

                    Ticket ticket = new Ticket();

                    ticket.MerchantAppUID = UserSessions.CurrentTicket.MerchantAppUID;
                    ticket.AgentUID = UserSessions.CurrentTicket.AgentUID;
                    ticket.StatusID = Ticket.TICKET_OPEN;
                    ticket.UserID = null;
                    ticket.DepartmentID = "7"; // RISK. let's not hard code this.
                    ticket.CategoryID = "964"; // "website inactive" lets not hard code this
                    ticket.ParentID = "960"; // "website review"
                    ticket.Problem = string.Format(@"TicketID: {0}
Completed By: {1}
Status: {2}
Rating: {3}
Risk Level: {4}"
    , UserSessions.CurrentTicket.TicketID
    , UserSessions.CurrentWSCompliance.CompletedBy
    , lblResult.Text
    , lblComplianceRating.Text
    , lblRiskIndex.Text);

                    ticket.DateCreated = DateTime.Now;
                    ticket.DateModified = DateTime.Now;
                    ticket.UserCreated = UserSessions.CurrentUser.UserName;
                    ticket.UserModified = UserSessions.CurrentUser.UserName;
                    ticket.Priority = "Low";
                    ticket.TicketSource = "i"; // "i" is for internal
                    ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                    ticket.AttentionReq = true;
                    ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
                    ticket.TimeZone = "6";
                    ticket.OfficeID = UserSessions.CurrentMerchantApp.Office.GetHashCode();
                    //ticket.Origin = 4; // Internal

                    DataTicket.GetInstance().InsertTicket(ticket,this.TimeZoneID);

                    if (ticket.TicketUID != "-1")
                    {
                        // good ticket, save it in the session.
                        UserSessions.CurrentTicket = ticket;

                        // send out apppriate emails!!
                        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false,false);
                        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
                        //PXP-2955
                        FormHandler.InsertMerchantNotesForTicket(ticket.MerchantAppUID, ticket.UserCreated, ticket);
                    }
                    else
                    {
                        wucMessage2.AddMessageError("ERROR: Could not create website compliance ticket for Risk.");
                        Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
                    }
                }
                else if (UserSessions.CurrentWSCompliance.StatusUID == Constants.COMPLIANCE_NOECOMMERCE)
                {

                    // if website has no ecommerce, then we create risk a new ticket to invesitage.

                    Ticket ticket = new Ticket();

                    ticket.MerchantAppUID = UserSessions.CurrentTicket.MerchantAppUID;
                    ticket.AgentUID = UserSessions.CurrentTicket.AgentUID;
                    ticket.StatusID = Ticket.TICKET_OPEN;
                    ticket.UserID = null;
                    ticket.DepartmentID = "7"; // RISK. let's not hard code this.
                    ticket.CategoryID = "1049"; // "no e-commerce" lets not hard code this
                    ticket.ParentID = "960"; // "website review"
                    ticket.Problem = string.Format(@"TicketID: {0}
Completed By: {1}
Status: {2}
Rating: {3}
Risk Level: {4}"
    , UserSessions.CurrentTicket.TicketID
    , UserSessions.CurrentWSCompliance.CompletedBy
    , lblResult.Text
    , lblComplianceRating.Text
    , lblRiskIndex.Text);

                    ticket.DateCreated = DateTime.Now;
                    ticket.DateModified = DateTime.Now;
                    ticket.UserCreated = UserSessions.CurrentUser.UserName;
                    ticket.UserModified = UserSessions.CurrentUser.UserName;
                    ticket.Priority = "Low";
                    ticket.TicketSource = "i"; // "i" is for internal
                    ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                    ticket.AttentionReq = true;
                    ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
                    ticket.TimeZone = "6";
                    ticket.OfficeID = UserSessions.CurrentMerchantApp.Office.GetHashCode();
                    //ticket.Origin = 4; // Internal

                    DataTicket.GetInstance().InsertTicket(ticket,this.TimeZoneID);

                    if (ticket.TicketUID != "-1")
                    {
                        // good ticket, save it in the session.
                        UserSessions.CurrentTicket = ticket;

                        // send out apppriate emails!!
                        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false,false);
                        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
                        //PXP-2955
                        FormHandler.InsertMerchantNotesForTicket(ticket.MerchantAppUID, ticket.UserCreated, ticket);
                    }
                    else
                    {
                        wucMessage2.AddMessageError("ERROR: Could not create website compliance ticket for Risk.");
                        Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
                    }

                }
                else if (retval == Constants.COMPLIANCE_REVIEWCOMPLETE && (risk_level == 4 || risk_level == 5))
                {

                    // if website review conducted and complete.

                    Ticket ticket = new Ticket();

                    ticket.MerchantAppUID = UserSessions.CurrentTicket.MerchantAppUID;
                    ticket.AgentUID = UserSessions.CurrentTicket.AgentUID;
                    ticket.StatusID = Ticket.TICKET_OPEN;
                    ticket.UserID = null;
                    ticket.DepartmentID = "7"; // RISK. let's not hard code this.
                    ticket.CategoryID = "961"; // "non-compliance" lets not hard code this
                    ticket.ParentID = "960"; // "website review"
                    ticket.Problem = string.Format(@"TicketID: {0}
Completed By: {1}
Status: {2}
Rating: {3}
Risk Level: {4}"
                        , UserSessions.CurrentTicket.TicketID
                        , UserSessions.CurrentWSCompliance.CompletedBy
                        , lblResult.Text
                        , lblComplianceRating.Text
                        , lblRiskIndex.Text);



                    ticket.DateCreated = DateTime.Now;
                    ticket.DateModified = DateTime.Now;
                    ticket.UserCreated = UserSessions.CurrentUser.UserName;
                    ticket.UserModified = UserSessions.CurrentUser.UserName;
                    ticket.Priority = "Low";
                    ticket.TicketSource = "i"; // "i" is for internal
                    ticket.UserCreatedUserUID = UserSessions.CurrentUser.UID;
                    ticket.AttentionReq = true;
                    ticket.DueDate = new DateTime(dt.Year, dt.Month, dt.Day, 18, 0, 0);
                    ticket.TimeZone = "6";
                    ticket.OfficeID = UserSessions.CurrentMerchantApp.Office.GetHashCode();
                    //ticket.Origin = 4; // Internal

                    DataTicket.GetInstance().InsertTicket(ticket,this.TimeZoneID);

                    if (ticket.TicketUID != "-1")
                    {
                        // good ticket, save it in the session.
                        UserSessions.CurrentTicket = ticket;

                        // send out apppriate emails!!
                        PaymentXP.Facade.TicketNotification.NewTicketCreated(ticket.TicketUID, false,false);
                        ZeusWeb.Logging.EmailLog.InfoFormat("Creating new ticket for ID : {0} .", ticket.TicketUID);
                        //PXP-2955
                        FormHandler.InsertMerchantNotesForTicket(ticket.MerchantAppUID, ticket.UserCreated, ticket);
                    }
                    else
                    {
                        wucMessage2.AddMessageError("ERROR: Could not create website compliance ticket for Risk.");
                        Response.Redirect("~/SecureMerchantManagementForms/frmCompliance.aspx?MerchantAppUID=" + UserSessions.CurrentMerchantApp.MerchantAppUID);
                    }

                }
            }
            else
            {
                if (retval == Constants.COMPLIANCE_NONCOMPLIANT)
                {
                    switch (UserSessions.CurrentMerchantApp.StatusUID.ToUpper())
                    {
                        case Constants.QUEUESTATUS_CU_RECEIVED:
                        case Constants.QUEUESTATUS_CU_PENDING:
                        case Constants.QUEUESTATUS_CU_IN_REVIEW:

                            UWConditions condition = new UWConditions();

                            string bad_list = "";

                            if (this.liBadList != null && this.liBadList.Count > 0)
                            {
                                bad_list = CommonUtility.Util.implode(this.liBadList, ",");
                            }

                            // INSERT into UWConditionDetail
                            condition.MerchantAppsUID = UserSessions.CurrentMerchantApp.MerchantAppUID;
                            condition.NeedInfo = true;
                            condition.EmailText = "Non compliant conditions: " + bad_list;
                            condition.Comments = "";
                            condition.ReceivedInfo = false;
                            condition.ConditionName = PaymentXP.BusinessObjects.Constants.COMPLIANCE_PENIDNG_NAME;
                            condition.ConditionID = PaymentXP.BusinessObjects.Constants.COMPLIANCE_PENDING_ID;

                            int condition_detail_id = DataConditions.InsertConditions(condition,UserSessions.CurrentUser.UserName);
                            break;
                    }
                }
            }
        }
        else
        {
            ret = false;

            foreach (string error in liError)
            {
                wucMessage2.AddMessageError(error);
            }
        }


        return ret;
    }

    /// <summary>
    /// given my WSCompliance object, check if it's compliant based on our rules.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>will return either a GUID or an error string</returns>
    protected List<string> CheckCompliance(WSCompliance obj, ref string ret, ref int risk_level, ref decimal points_possible, ref decimal points_earned)
    {
        // log all errors here.
        List<string> liError = new List<string>();

        // start at 5 on default.
        risk_level = 5;

        // how many choices were made as "Compliant"
        int compliance_count = 0;

        // total available compliance choices (everything that wasn't NA or anything that we can count towards compliance)
        int choice_available_count = 0;

        // yay or nay, we record it here.
        int choice_made_count = 0;

        bool perform = true;


        int HighFailCount = 0;
        int MediumFailCount = 0;

        points_possible = 0;
        points_earned = 0;




        try
        {

            List<WSComplianceItem> li = WSComplianceFacade.GetItemsNested(obj);

            bool parent_is_valid = false;


            foreach (WSComplianceItem item in li)
            {

                int maxpoints = 0;

                if (item.WSComplianceItemID == WSComplianceFacade.question_website_must_be_active)
                {
                    if (item.WSComplianceItemAnswerUID.ToUpper() == Constants.COMPLIANCE_ANSWER_NO)
                    {
                        ret = Constants.COMPLIANCE_WEBSITEINACTIVE;
                        liError.Add(string.Format("Required: {0}", item.ComplianceDescription));
                        break;
                    }
                }


                if (item.WSComplianceItemID == WSComplianceFacade.question_is_ecommerce)
                {
                    if (item.WSComplianceItemAnswerUID.ToUpper() == Constants.COMPLIANCE_ANSWER_NO)
                    {
                        ret = Constants.COMPLIANCE_NOECOMMERCE;
                        liError.Add(string.Format("Required: {0}", item.ComplianceDescription));
                        break;
                    }
                }

                bool is_parent = (item.ParentID == 0) ? true : false;

                bool has_kids = (is_parent && obj.diComplianceItem[item.WSComplianceAssocID].diComplianceItem.Count > 0) ? true : false;

                bool am_i_valid = (is_parent || parent_is_valid); // true if you are a parent, or you're a child that has a valid parent.

                if (is_parent)
                {
                    parent_is_valid = false;
                }




                if (am_i_valid)
                {

                    switch ((eWSComplianceQuestionType)item.WSComplianceQuestionTypeID)
                    {
                        case eWSComplianceQuestionType.ComplianceDropdown:

                            if (item.WSComplianceWeightID == eWSComplianceWeight.High && WSComplianceFacade.IsLowScoreAnswer(item.WSComplianceItemAnswerUID))
                            {
                                HighFailCount++;
                            }

                            if (item.WSComplianceWeightID == eWSComplianceWeight.Medium && WSComplianceFacade.IsLowScoreAnswer(item.WSComplianceItemAnswerUID))
                            {
                                MediumFailCount++;
                            }

                            maxpoints = CommonUtility.Util.if_i(item.MaxPoints, 0);

                            if (!(maxpoints == 0 || WSComplianceFacade.IsAnswerNA(item.WSComplianceItemAnswerUID)))
                            {
                                decimal dpointsearned = WSComplianceFacade.GetPointsEarned(item.WSComplianceItemAnswerUID, item.MaxPoints);
                                decimal dpointsmissed = item.MaxPoints - dpointsearned;

                                points_possible += maxpoints;
                                points_earned += dpointsearned;

                            }

                            // standard behavior for standard dropdowns.

                            switch (item.WSComplianceItemAnswerUID.ToUpper())
                            {
                                case Constants.COMPLIANCE_ANSWER_NONCOMPLIANT:

                                    ++choice_available_count;
                                    ++choice_made_count;

                                    liError.Add(string.Format("Required: {0}", item.ComplianceDescription));

                                    break;
                                case Constants.COMPLIANCE_ANSWER_COMPLIANT:

                                    ++compliance_count;

                                    ++choice_available_count;
                                    ++choice_made_count;

                                    break;
                                case Constants.COMPLIANCE_ANSWER_NA:
                                    break;

                                default:
                                    ++choice_available_count;
                                    liError.Add(string.Format("Required: {0}", item.ComplianceDescription));
                                    break;
                            }

                            break;

                        case eWSComplianceQuestionType.CustomDropdown:

                            // on a per question basis, DEFINE if it is compliant, or not.
                            if (item.WSComplianceItemID == WSComplianceFacade.question_how_long_is_the_trial_period)
                            {

                                // we dont care about the result. because it sets the question below.

                                //string mycomment = CommonUtility.Util.if_s(item.Comment);

                                //string[] arr = mycomment.Split(new char[] { '|' });

                                //if (arr.Length > 1 && CommonUtility.Util.IsValidInt32(arr[0]))
                                //{
                                //    ++choice_made_count;
                                //    ++compliance_count;

                                //}
                                //else
                                //{
                                //    liBadList.Add(string.Format(" {0}:{1}", item.ComplianceDescription, item.Comment));
                                //}

                                //++choice_available_count;
                            }

                            // add more questions as needed.


                            break;

                        case eWSComplianceQuestionType.RequiredText:

                            // for comments, they're a special case, theyre optional, and don't count towards anything.
                            if (item.WSComplianceItemID != WSComplianceFacade.question_comments)
                            {
                                if (item.Comment.Trim() != "")
                                {
                                    ++choice_made_count;
                                    ++compliance_count;
                                }
                                else
                                {
                                    liError.Add(string.Format("Required: {0}", item.ComplianceDescription));
                                }

                                ++choice_available_count;
                            }

                            break;

                        case eWSComplianceQuestionType.YesNoDropdown:
                            switch (item.WSComplianceItemAnswerUID.ToUpper())
                            {
                                case Constants.COMPLIANCE_ANSWER_YES:
                                    parent_is_valid = true;
                                    break;
                                default:
                                    parent_is_valid = false;

                                    break;
                            }



                            break;


                        case eWSComplianceQuestionType.PassNoPassDropdown:
                        case eWSComplianceQuestionType.AcceptabilityDropdown:

                            if (!WSComplianceFacade.IsAnswerNA(item.WSComplianceItemAnswerUID))
                            {
                                choice_available_count++;

                                switch (item.WSComplianceItemAnswerUID.ToUpper())
                                {
                                    case Constants.COMPLIANCE_ANSWER_ACCEPT_ACCEPTABLE:
                                    case Constants.COMPLIANCE_ANSWER_ACCEPT_NEEDSWORK:
                                    case Constants.COMPLIANCE_ANSWER_ACCEPT_UNACCEPTABLE:
                                    case Constants.COMPLIANCE_ANSWER_PNP_PASS:
                                    case Constants.COMPLIANCE_ANSWER_PNP_NOPASS:
                                        choice_made_count++;
                                        break;

                                    default:
                                        liError.Add(string.Format("Required: {0}", item.ComplianceDescription));
                                        break;
                                }
                            }



                            if (item.WSComplianceWeightID == eWSComplianceWeight.High && WSComplianceFacade.IsLowScoreAnswer(item.WSComplianceItemAnswerUID))
                            {
                                HighFailCount++;
                            }

                            if (item.WSComplianceWeightID == eWSComplianceWeight.Medium && WSComplianceFacade.IsLowScoreAnswer(item.WSComplianceItemAnswerUID))
                            {
                                MediumFailCount++;
                            }

                            maxpoints = CommonUtility.Util.if_i(item.MaxPoints, 0);

                            if (!(maxpoints == 0 || WSComplianceFacade.IsAnswerNA(item.WSComplianceItemAnswerUID)))
                            {
                                decimal dpointsearned = WSComplianceFacade.GetPointsEarned(item.WSComplianceItemAnswerUID, item.MaxPoints);
                                decimal dpointsmissed = item.MaxPoints - dpointsearned;

                                points_possible += maxpoints;
                                points_earned += dpointsearned;

                            }

                            break;
                    }

                }
            }

            // document check
            if (perform && GridView2.Rows.Count == 0)
            {
                perform = false;
                ret = "At least one document must be uploaded";
                liError.Add("At least one document must be uploaded");
            }

            // make sure this check is after the documents check. we want documents to be uploaded no matter what!!!
            if (perform && (ret == Constants.COMPLIANCE_WEBSITEINACTIVE || ret == Constants.COMPLIANCE_NOECOMMERCE))
            {
                // website inactive, so we just quit
                perform = false;
            }



            //if (perform && !(compliance_count > 0))
            //{
            //    perform = false;
            //    ret = "At least one item must be marked as compliant";
            //}

            if (perform && choice_made_count != choice_available_count)
            {
                perform = false;
                ret = "All checklist items must be completed before you can submit";
                liError.Add("All checklist items must be completed before you can submit");
            }

            if (perform)
            {
                //if (compliance_count == choice_available_count)
                //{
                //    ret = Constants.COMPLIANCE_COMPLIANT;
                //}
                //else
                //{
                //    ret = Constants.COMPLIANCE_NONCOMPLIANT;
                //}

                ret = Constants.COMPLIANCE_REVIEWCOMPLETE;

                // calculate risk level:

                if (HighFailCount > 0)
                {
                    risk_level = 4;
                }
                else if (MediumFailCount > 0)
                {
                    risk_level = 3;
                }
                else
                {
                    if (points_possible > 0)
                    {
                        decimal percent = (points_earned / points_possible);

                        // note: we want to round before comparing it to the threshold. this is a business rule.
                        percent = Convert.ToDecimal(string.Format("{0:f1}", percent));

                        decimal magic_threshold = .92m;

                        if (percent < magic_threshold)
                        {
                            risk_level = 2;
                        }
                        else
                        {
                            risk_level = 1;
                        }
                    }
                }

            }

        }
        catch
        {
            ret = "";
        }

        return liError;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        WSCompliance objWSC = DataWSCompliance.SelectWSCompliance(this.vsWSComplianceID);

        Hashtable prms = new Hashtable();
        prms["TicketUID"] = this.vsTicketUID;
        Ticket t = DataTicket.GetInstance().GetTicket(prms,UserSessions.CurrentUser.TimeZone);

        if (fuFile.HasFile && !string.IsNullOrEmpty(fuFile.FileName) && objWSC != null && t != null)
        {

            ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();

            objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];

            ZeusWeb.MDocWS.UploadResponse objResp = objFU.UploadFileWithSourceAndUser(fuFile.FileBytes
                , Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)
                , t.MerchantAppUID
                , CommonUtility.Util.if_i(t.AgentID, 0)
                , CommonUtility.Util.if_s(t.AgentUID)
                , (int)MDoc.eMDocType.Ticket
                , fuFile.FileName
                , "Website Compliance"
                , 0
                , Comment.Text.Trim()
                , t.TicketUID
                , CommonUtility.Util.if_i(t.TicketID, 0)
                , (int)MDoc.eMDocSourceID.Tickets
                , UserSessions.CurrentUser.UserName
                );

            if (objResp != null && objResp.DocID > 0)
            {
                // uploaded good.     
                this.load_documents(t);
                bool IsuserNameUpdated = DocUploadUpdateUserID(objResp.DocID, UserSessions.CurrentUser.UserName);

                if (IsuserNameUpdated == false)
                {
                    wucMessage2.AddMessageError("Username of uploader not updated");
                }        

                this.refresh_displays(objWSC, t);

                // if the ticket is currently open, then we mark it as assigned and set it to the current user.
                if (t.Status == Ticket.TICKET_OPEN)
                {
                    t.Status = Ticket.TICKET_ASSIGNED;
                    t.UserID = UserSessions.CurrentUser.UID;
                }

                wucMessage2.AddMessageStatus("New Document Uploaded. Filename: " + fuFile.FileName);

            }
            else
            {
                wucMessage2.AddMessageError("ERROR: Could not upload the file: " + fuFile.FileName);
            }

            // save form just to make sure no values are lost.
            this.FormSave();

        }
    }


    public bool DocUploadUpdateUserID(int DocumentID, string UserName)
    {
        Hashtable prms = new Hashtable();

        prms.Add("@DocumentID", DocumentID);
        prms.Add("@UserName", UserName);

        return DataDocuments.GetInstance().DocUploadUpdateUserIDMdocument(prms);

    }

    private void refresh_displays(WSCompliance objWSC, Ticket t)
    {
        // set title fields
        lblTicketID.Text = objWSC.TicketID.ToString();





        lblDBA.Text = t.BusinessDBAName;
        lblZID.Text = objWSC.MerchantID.ToString();
        DateTime dt;
        DateTime.TryParse(objWSC.DateCompletedBy.ToString("MM/dd/yyyy"),out dt);
        lblDateOfReview.Text = (dt == DateTime.MinValue) ? "TBD" : WebUtil.CovertToUserDateTimePattern(objWSC.DateCompletedBy);
        lblWebsiteURL.Text = UserSessions.CurrentMerchantApp.BusinessWebsite;
        lblMCC.Text = UserSessions.CurrentMerchantApp.SicCode;

        lblFacetoFace.Text = string.Format("{0:F0}", UserSessions.CurrentMerchantApp.TinfoStoreFrontSwipedPercent);
        lblInternet.Text = string.Format("{0:F0}", UserSessions.CurrentMerchantApp.TinfoInterntPercent);
        lblMailOrder.Text = string.Format("{0:F0}", UserSessions.CurrentMerchantApp.TinfoMailOrderPercent);
        lblTelephoneOrder.Text = string.Format("{0:F0}", UserSessions.CurrentMerchantApp.TinfoTelephoneOrderPercent);

        lblDateCUApproved.Text = WebUtil.ConvertToUserDatePattern(this.GetFirstDateMerchantStatusHistory(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_CU_APPROVED));
        lblDateCSActive.Text = WebUtil.ConvertToUserDatePattern(this.GetFirstDateMerchantStatusHistory(UserSessions.CurrentMerchantApp.MerchantAppUID, Constants.QUEUESTATUS_MS_ACTIVE));

        Underwriting objUW = DataAccess.DataUnderwritingDao.LoadMerchantUWNotes(UserSessions.CurrentMerchantApp.MerchantAppUID);
        if (objUW != null)
        {
            lblUnderwritingNotes.Text = objUW.NotesUW;
        }

        //hypCUWebsite
        Hashtable prms = new Hashtable();
        prms.Add("@MerchantID", Convert.ToInt32(UserSessions.CurrentMerchantApp.ID));
        prms.Add("@DocTypeID", 50);  // "CU Websites"
        prms.Add("@IsDeleted", 0);
        List<MDoc> liDoc = DataDocuments.GetInstance().GetMDocuments(prms);

        Literal litCU = new Literal();
        StringBuilder sb = new StringBuilder();

        if (liDoc != null)
        {

            sb.Append("<ol>");

            foreach (MDoc obj in liDoc)
            {
                Dictionary<string, string> di = new Dictionary<string, string>();

                di["DocID"] = obj.DocID.ToString();
                di["MerchantAppUID"] = obj.MerchantAppUID;
                di["MerchantID"] = obj.MerchantID.ToString();

                //string url = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di)));

                string url = string.Format("~/SecureMerchantManagementForms/frmMerchantDocumentPreview.aspx?x={0}", Server.UrlEncode(CommonUtility.Crypto.EncryptUrl(di)));


                HyperLink hl = new HyperLink();

                hl.NavigateUrl = url;
                hl.Text = (obj.OrigName.Trim() == "") ? obj.DocName.Trim() : obj.OrigName.Trim();



                //render control to string
                StringBuilder b = new StringBuilder();
                HtmlTextWriter h = new HtmlTextWriter(new StringWriter(b));
                hl.RenderControl(h);
                string controlAsString = b.ToString();

                sb.AppendFormat("<li>{0}</li>", controlAsString);
            }
            sb.Append("</ol>");

            litCU.Text = sb.ToString();

            phLink.Controls.Add(litCU);
        }



        // if the merchant on the ticket is different than the merchant that's selected, then display a warning.

        if (UserSessions.CurrentMerchantApp != null)
        {
            lblNoteMerch.Visible = (objWSC.MerchantID != Convert.ToInt32(UserSessions.CurrentMerchantApp.ID)) ? true : false;
        }


        if (this.VSRunningPossible == 0)
        {
            lblEarnedOverPossible.Text = "0.0 / 0.0";
            lblComplianceRating.Text = "N/A";
        }
        else
        {
            lblEarnedOverPossible.Text = string.Format("{0:f1} / {1:f1}", this.VSRunningEarned, this.VSRunningPossible);
            lblComplianceRating.Text = string.Format("{0:f0}%", (this.VSRunningEarned / this.VSRunningPossible) * 100);
        }

        int risk_index = CommonUtility.Util.if_i(UserSessions.CurrentWSCompliance.RiskIndex, 0);

        lblRiskIndex.Text = (risk_index > 0) ? risk_index.ToString() : "N/A";

        switch (objWSC.StatusUID.ToUpper())
        {

            case Constants.COMPLIANCE_NONCOMPLIANT:
                lblResult.Text = "Complete (Non-Compliant)";
                lblRiskIndex.Text = "N/A";  // override
                break;

            case Constants.COMPLIANCE_COMPLIANT:
                lblResult.Text = "Complete (Compliant)";
                lblRiskIndex.Text = "N/A";  // override
                break;

            case Constants.COMPLIANCE_TBD:
                lblResult.Text = "TBD";
                break;

            case Constants.COMPLIANCE_WEBSITEINACTIVE:
                lblResult.Text = "Complete (Website Inactive)";
                break;

            case Constants.COMPLIANCE_NOECOMMERCE:
                lblResult.Text = "Complete (No E-Commerce)";
                break;

            case Constants.COMPLIANCE_CANCELLED:
                lblResult.Text = "Cancelled";
                break;

            case Constants.COMPLIANCE_REVIEWCOMPLETE:
                lblResult.Text = "Complete";
                break;

            default:
                lblResult.Text = objWSC.StatusName;
                break;
        }

        if (lblComplianceRating.Text == "0%")
        {
            lblComplianceRating.Text = "N/A"; // override
        }
    }

    private string GetFirstDateMerchantStatusHistory(string merchantappuid, string statusuid)
    {
        string ret = "";

        Hashtable prms = new Hashtable();
        prms.Add("@MerchantAppUID", merchantappuid);
        prms.Add("@StatusUID", statusuid);
        DataSet ds = DataMerchantApp.GetInstance().GetMerchantStatusHistory(prms);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {

            DateTime dt = CommonUtility.Util.if_d(ds.Tables[0].Rows[0]["Changed Date"], DateTime.MinValue);

            if (dt == DateTime.MinValue)
            {
                ret = "";
            }
            else
            {
                ret = dt.ToShortDateString();
            }
        }

        return ret;
    }

    protected void lbFilename_Click(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        if (lb.CommandName == "ViewDoc")
        {


            int doc_id = Convert.ToInt32(lb.CommandArgument);
            ZeusWeb.MDocWS.FileUpload objFU = new ZeusWeb.MDocWS.FileUpload();
            objFU.Url = ConfigurationManager.AppSettings["MDocWS.fileupload"];
            ZeusWeb.MDocWS.MDoc objMD = objFU.GetFile(doc_id, UserSessions.CurrentWSCompliance.MerchantID, 0, 0);

            if (objMD != null && objMD.FileBinary != null)
            {
                string filenamepre = objMD.OrigName.Replace(" ", "_");

                // Add the file name and attachment, which will force the open/cance/save dialog to show, to the header
                Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filenamepre));
                Response.AddHeader("Content-Length", objMD.ContentSize.ToString());

                string tempfile = filenamepre.ToLower();

                if (tempfile.EndsWith(".tif") || tempfile.EndsWith(".tiff"))
                {
                    Response.ContentType = "image/tiff";
                }
                else if (tempfile.EndsWith(".jpeg") || tempfile.EndsWith(".jpg"))
                {
                    Response.ContentType = "image/jpeg";
                }
                else if (tempfile.EndsWith(".gif"))
                {
                    Response.ContentType = "image/gif";
                }
                else if (tempfile.EndsWith(".png"))
                {
                    Response.ContentType = "image/png";
                }
                else
                {
                    Response.ContentType = "application/pdf";
                }

                Response.BinaryWrite(objMD.FileBinary);
                Response.End();
            }
        }
    }

    /// <summary>
    /// Added by Eric Luxa to allow global users and users that initiated the compliance
    /// review to be able to cancel a review. Cancelling a review will update the compliance
    /// status and close the ticket associated with the compliance review
    /// </summary>
    private void CancelReview()
    {
        //update compliance
        UserSessions.CurrentWSCompliance.CompletedBy = UserSessions.CurrentUser.UserName;
        UserSessions.CurrentWSCompliance.StatusUID = Constants.COMPLIANCE_CANCELLED;
        UserSessions.CurrentWSCompliance.UserUpdated = UserSessions.CurrentUser.UserName;
        DataWSCompliance.UpdateWSCompliance(UserSessions.CurrentWSCompliance);

        //close ticket
        UserSessions.CurrentTicket.StatusID = Ticket.TICKET_CLOSE;
        DataTicket.GetInstance().UpdateTicket(UserSessions.CurrentTicket,this.TimeZoneID);
    }



    protected void WSComplianceItemAnswerUID_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;

        HiddenField hf_item_id = (HiddenField)ddl.Parent.FindControl("hidWSComplianceItemID");
        HiddenField hf_answer_uid = (HiddenField)ddl.Parent.FindControl("hidWSComplianceItemAnswerUID");
        HiddenField hf_assoc_id = (HiddenField)ddl.Parent.FindControl("hidWSComplianceAssocID");

        TextBox tb_comment = (TextBox)ddl.Parent.FindControl("tbComment");

        int item_id = CommonUtility.Util.if_i(hf_item_id.Value, 0);
        string answer_uid = CommonUtility.Util.if_s(hf_answer_uid.Value);
        int assoc_id = CommonUtility.Util.if_i(hf_assoc_id.Value, 0);

        WSComplianceItem item = DataWSCompliance.SelectWSComplianceAssoc(assoc_id);
        item.WSComplianceItemAnswerUID = ddl.SelectedValue;
        DataWSCompliance.UpdateWSComplianceAssoc(item);

        foreach (KeyValuePair<int, WSComplianceItem> kvp in UserSessions.CurrentWSCompliance.diComplianceItem)
        {
            if (kvp.Value.WSComplianceItemID == item_id)
            {
                // reflect it in our session.
                kvp.Value.WSComplianceItemAnswerUID = ddl.SelectedValue;
                kvp.Value.Comment = tb_comment.Text;

                // save to db
                DataWSCompliance.UpdateWSComplianceAssoc(kvp.Value);

                GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                GridView1.DataBind();
                break;
            }
            else
            {

                // if its not in the main, then its in the nested.

                if (kvp.Value.diComplianceItem != null)
                {
                    foreach (KeyValuePair<int, WSComplianceItem> inner in kvp.Value.diComplianceItem)
                    {
                        if (inner.Value.WSComplianceItemID == item_id)
                        {
                            inner.Value.WSComplianceItemAnswerUID = ddl.SelectedValue;
                            inner.Value.Comment = tb_comment.Text;

                            DataWSCompliance.UpdateWSComplianceAssoc(inner.Value);

                            //  rebind to grid.
                            GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                            GridView1.DataBind();
                            break;
                        }
                    }

                }

            }
        }

        if (WSComplianceFacade.question_how_long_is_the_trial_period == item_id)
        {
            string myanswer = "";

            if (ddl.SelectedValue == Constants.COMPLIANCE_ANSWER_NA) //note: one is NA, the other is PNP_NA
            {
                myanswer = Constants.COMPLIANCE_ANSWER_PNP_NA;
            }
            else if (CommonUtility.Util.IsValidInt32(ddl.SelectedValue))
            {

                int days = Convert.ToInt32(ddl.SelectedValue);

                if (days >= 7 && days <= 30)
                {
                    myanswer = Constants.COMPLIANCE_ANSWER_PNP_PASS;
                }
                else
                {
                    myanswer = Constants.COMPLIANCE_ANSWER_PNP_NOPASS;
                }
            }

            foreach (KeyValuePair<int, WSComplianceItem> kvp in UserSessions.CurrentWSCompliance.diComplianceItem)
            {
                if (kvp.Value.WSComplianceItemID == WSComplianceFacade.question_offer_trial)
                {
                    foreach (KeyValuePair<int, WSComplianceItem> innerkvp in kvp.Value.diComplianceItem)
                    {
                        if (innerkvp.Value.WSComplianceItemID == WSComplianceFacade.question_trial_within)
                        {
                            innerkvp.Value.WSComplianceItemAnswerUID = myanswer;
                            //innerkvp.Value.Comment =
                            DataWSCompliance.UpdateWSComplianceAssoc(innerkvp.Value);

                            // rebind to grid.
                            GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                            GridView1.DataBind();
                            break;
                        }
                    }
                }
            }

        }
    }



    protected void tbComment_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;

        HiddenField hf_assoc_id = (HiddenField)tb.Parent.FindControl("hidWSComplianceAssocID");

        int assoc_id = CommonUtility.Util.if_i(hf_assoc_id.Value, 0);

        if (assoc_id > 0)
        {
            foreach (KeyValuePair<int, WSComplianceItem> kvp in UserSessions.CurrentWSCompliance.diComplianceItem)
            {

                if (kvp.Value.WSComplianceAssocID == assoc_id)
                {
                    kvp.Value.Comment = tb.Text.Trim();
                    DataWSCompliance.UpdateWSComplianceAssoc(kvp.Value);

                    GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                    GridView1.DataBind();
                    break;
                }
                else
                {
                    if (kvp.Value.diComplianceItem != null)
                    {
                        foreach (KeyValuePair<int, WSComplianceItem> inner in kvp.Value.diComplianceItem)
                        {
                            if (inner.Value.WSComplianceAssocID == assoc_id)
                            {
                                inner.Value.Comment = tb.Text.Trim();
                                DataWSCompliance.UpdateWSComplianceAssoc(inner.Value);

                                GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                                GridView1.DataBind();
                                break;
                            }
                        }
                    }
                }
            }

        }
    }


    protected void tbInternalComment_TextChanged(object sender, EventArgs e)
    {
        TextBox tb = (TextBox)sender;

        HiddenField hf_assoc_id = (HiddenField)tb.Parent.FindControl("hidWSComplianceAssocID");

        int assoc_id = CommonUtility.Util.if_i(hf_assoc_id.Value, 0);

        if (assoc_id > 0)
        {
            foreach (KeyValuePair<int, WSComplianceItem> kvp in UserSessions.CurrentWSCompliance.diComplianceItem)
            {

                if (kvp.Value.WSComplianceAssocID == assoc_id)
                {
                    kvp.Value.InternalComment = tb.Text.Trim();
                    DataWSCompliance.UpdateWSComplianceAssoc(kvp.Value);

                    GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                    GridView1.DataBind();
                    break;
                }
                else
                {
                    if (kvp.Value.diComplianceItem != null)
                    {
                        foreach (KeyValuePair<int, WSComplianceItem> inner in kvp.Value.diComplianceItem)
                        {
                            if (inner.Value.WSComplianceAssocID == assoc_id)
                            {
                                inner.Value.InternalComment = tb.Text.Trim();
                                DataWSCompliance.UpdateWSComplianceAssoc(inner.Value);

                                GridView1.DataSource = WSComplianceFacade.GetItemsNested(UserSessions.CurrentWSCompliance);
                                GridView1.DataBind();
                                break;
                            }
                        }
                    }
                }
            }

        }
    }


    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        //DataTable tbl = null;

        //Hashtable prms = new Hashtable();
        //prms.Add("@BusinessDBAName", "test");
        //DataSet ds = DataMerchantApp.GetInstance().GetMerchantApps(prms);

        //tbl = ds.Tables[0];



        using (ExcelPackage pck = new ExcelPackage())
        {

            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Compliance");

            List<WSCompXL> li = WSComplianceFacade.GetWSCompXL(UserSessions.CurrentWSCompliance, WSComplianceFacade.liHide);

            ws.Cells.Style.WrapText = true;
            ws.Cells.Style.Font.Size = 10;

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/PaysafeNotificationLogo.PNG");
            System.Drawing.Image logo = System.Drawing.Image.FromFile(path);

            ws.HeaderFooter.FirstHeader.InsertPicture(logo, PictureAlignment.Left);

            ws.Row(0).Height = 60;

            ws.Cells["A2"].Value = "Website Compliance Review";
            using (ExcelRange rng = ws.Cells["A2:F2"])
            {
                rng.Merge = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Font.Bold = true;
            }

            ws.Column(1).Width = 90.00;
            ws.Column(2).Width = 20.00;
            ws.Column(3).Width = 10.00;
            ws.Column(4).Width = 10.00;
            ws.Column(5).Width = 10.00;
            ws.Column(6).Width = 40.00;

            ws.Cells["A4"].Value = "Date of Review: " + UserSessions.CurrentWSCompliance.DateCompletedBy.ToShortDateString().Trim();
            ws.Cells["A5"].Value = "Review ID: " + UserSessions.CurrentWSCompliance.TicketID.ToString();
            ws.Cells["A6"].Value = "Website URL: " + UserSessions.CurrentMerchantApp.BusinessWebsite;
            ws.Cells["A7"].Value = "DBA Name: " + UserSessions.CurrentMerchantApp.BusinessDBAName;
            ws.Cells["A8"].Value = "Merchant ID: " + UserSessions.CurrentMerchantApp.SettlePlatformMid;




            ws.Cells["A10:F10"].Style.Font.Bold = true;
            ws.Cells["A10:F10"].Style.Fill.PatternType = ExcelFillStyle.Solid;
            ws.Cells["A10:F10"].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);

            //ws.Cells["A10"].LoadFromCollection(li, true);

            int initial_row = 10;

            //header
            ws.Cells[string.Format("A{0}", initial_row.ToString())].Value = "Description";
            ws.Cells[string.Format("B{0}", initial_row.ToString())].Value = "Status";
            ws.Cells[string.Format("C{0}", initial_row.ToString())].Value = "Points Possible";
            ws.Cells[string.Format("D{0}", initial_row.ToString())].Value = "Points Missed";
            ws.Cells[string.Format("E{0}", initial_row.ToString())].Value = "Points Earned";
            ws.Cells[string.Format("F{0}", initial_row.ToString())].Value = "Comments";
            ++initial_row;

            decimal points_possible = 0m;
            decimal points_missed = 0m;
            decimal points_earned = 0m;

            string additional_comments = "";

            // rows
            foreach (WSCompXL item in li)
            {
                points_possible += item.PointsPossible;
                points_missed += item.PointsMissed;
                points_earned += item.PointsEarned;

                if (item.Description.Trim().ToLower() == "additional comments")
                {
                    additional_comments = item.Comments;

                    ws.Cells[string.Format("A{0}", initial_row.ToString())].Value = "";
                    ws.Cells[string.Format("B{0}", initial_row.ToString())].Value = "Total:";
                    ws.Cells[string.Format("C{0}", initial_row.ToString())].Value = string.Format("{0:f1}", points_possible);
                    ws.Cells[string.Format("C{0}", initial_row.ToString())].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    if (points_missed > 0)
                    {
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Value = string.Format("({0:f1})", points_missed);
                    }
                    else
                    {
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Value = string.Format("{0:f1}", points_missed);
                    }
                    ws.Cells[string.Format("D{0}", initial_row.ToString())].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;


                    ws.Cells[string.Format("E{0}", initial_row.ToString())].Value = string.Format("{0:f1}", points_earned);
                    ws.Cells[string.Format("E{0}", initial_row.ToString())].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    ws.Cells[string.Format("F{0}", initial_row.ToString())].Value = "";
                }
                else
                {
                    ws.Cells[string.Format("A{0}", initial_row.ToString())].Value = item.Description;
                    ws.Cells[string.Format("B{0}", initial_row.ToString())].Value = item.Status;
                    ws.Cells[string.Format("C{0}", initial_row.ToString())].Value = item.PointsPossible;

                    if (item.PointsMissed > 0)
                    {
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Value = string.Format("({0:f1})", item.PointsMissed);
                    }
                    else
                    {
                        ws.Cells[string.Format("D{0}", initial_row.ToString())].Value = item.PointsMissed;
                    }
                    ws.Cells[string.Format("D{0}", initial_row.ToString())].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    ws.Cells[string.Format("E{0}", initial_row.ToString())].Value = item.PointsEarned;
                    ws.Cells[string.Format("F{0}", initial_row.ToString())].Value = item.Comments;
                }


                if (item.IsPink)
                {
                    ws.Cells[string.Format("A{0}:F{0}", initial_row.ToString())].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[string.Format("A{0}:F{0}", initial_row.ToString())].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Pink);
                }

                ++initial_row;
            }



            int last_grid_index = 6;
            using (ExcelRange col = ws.Cells[10, 1, 10 + li.Count, last_grid_index])
            {
                col.Style.Border.BorderAround(ExcelBorderStyle.Thin, System.Drawing.Color.Black);
            }

            ws.Cells[10, 1, 10 + li.Count, last_grid_index].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            ws.Cells[10, 1, 10 + li.Count, last_grid_index].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);

            ws.Cells[10, 1, 10 + li.Count, last_grid_index].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            ws.Cells[10, 1, 10 + li.Count, last_grid_index].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);


            ws.Cells[11 + li.Count + 1, 1].Value = "Additional Comments: " + additional_comments;

            using (ExcelRange rng = ws.Cells[11 + li.Count + 1, 1, 11 + li.Count + 1, last_grid_index])
            {
                rng.Merge = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }

            //if (!CommonUtility.Util.IsValidGuid(UserSessions.CurrentMerchantApp.PrivateLabelUID))
            //{
            //    ws.Cells[11 + li.Count + 3, 1].Value = "1901 E. Alton Avenue, Suite 220, Santa Ana, CA 92705";
            //    ws.Cells[12 + li.Count + 3, 1].Value = "Phone: (949) 788-1010 | Fax: (949) 315-3216";
            //    ws.Cells[13 + li.Count + 3, 1].Value = "www.merituspayment.com";
            //}

            using (ExcelRange rng = ws.Cells[11 + li.Count + 3, 1, 11 + li.Count + 3, last_grid_index])
            {
                rng.Merge = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Font.Bold = true;
            }

            using (ExcelRange rng = ws.Cells[12 + li.Count + 3, 1, 12 + li.Count + 3, last_grid_index])
            {
                rng.Merge = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Font.Bold = true;
            }

            using (ExcelRange rng = ws.Cells[13 + li.Count + 3, 1, 13 + li.Count + 3, last_grid_index])
            {
                rng.Merge = true;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                rng.Style.Font.Bold = true;
            }


            ws.Cells["B4"].Value = "Compliance Rating";
            ws.Cells["B5"].Value = string.Format("{0:f0}%", (points_earned / points_possible) * 100);
            ws.Cells["B6"].Value = string.Format("{0} / {1} Points", points_earned, points_possible);

            string filename = String.Format("{0}_{1}_Website-Compliance-Review.xlsx", CommonUtility.Util.GenerateSlug(UserSessions.CurrentMerchantApp.BusinessDBAName), UserSessions.CurrentMerchantApp.SettlePlatformMid);

            //Write it back to the client
            Response.Clear();   // necessary!!!
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=" + filename);
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();     // necessary!!! without the clear/end pair, it will display it as corrupt!!


        }
    }

    //protected void btnPreview_Click(object sender, EventArgs e)
    //{

    //    String myoutput = GetPreviewText();

    //    if (!string.IsNullOrEmpty(myoutput))
    //    {

    //        string filename = String.Format("{0}_{1}_Website-Compliance-Review", CommonUtility.Util.GenerateSlug(UserSessions.CurrentMerchantApp.BusinessDBAName), UserSessions.CurrentMerchantApp.SettlePlatformMid);

    //        Response.AddHeader("Content-Disposition", String.Format("attachment; filename={0}", filename));
    //        Response.AddHeader("Content-Length", myoutput.Length.ToString());
    //        Response.ContentType = "text/html";
    //        Response.Write(myoutput);
    //        Response.End();
    //    }


    //}


    //**PXP-7231(Meritus word replacement with paysafe) By Sanidhya kumar
    public static string EMAIL_NONCOMPLIANT = @"
<p>
  Dear [[merchantprimarycontact]]:
</p>
<p>
  In an ongoing effort to minimize cardholder disputes and maintain compliance with bankcard industry standards, the Paysafe Compliance Department conducts regularly scheduled website reviews for our clients that process credit card transactions for online orders.  Upon completion of a review on [[reviewdate]], your company's website <span style='color:blue; text-decoration:underline'>[[merchantwebsite]]</span> was identified as non-compliant according to Paysafe' Website Policy.
</p>
<p>
  Your website was non-compliant on the following issue(s):
</p>
<ul>
[[noncompliantissuelist]]
</ul>
<p>
Attached to this email are the results of Paysafe' Website Compliance review that was conducted on your merchant account, which may contain notes that refer to specific non-compliant sections of the website.  Also included are screenshots of the website to help identify the non-compliant issues.
</p>
<p>
Please address all issues found on the checklist.  Failure to address these non-compliant issues may lead to additional fees, as well as restrictions such as withheld deposits or account de-activation.  If you have any questions, please contact the Paysafe Compliance Department via e-mail at <a href='mailto:compliance_pp@paysafe.com'>compliance_pp@paysafe.com</a>.
</p>

";

    public static string EMAIL_COMPLIANT = @"
<p>
Dear [[merchantprimarycontact]]:
</p>
<p>
Upon completion of another review on [[reviewdate]], your company's website <span style='color:blue; text-decoration:underline'>[[merchantwebsite]]</span> was identified as compliant according to Paysafe' Website Policy.  Thank you for taking the time to make the appropriate compliance changes to your website.  If you have any questions, please contact the Paysafe Compliance Department via e-mail at <a href='mailto:compliance_pp@paysafe.com'>compliance_pp@paysafe.com</a>.
</p>

";
    //Modify for PXP-8058 by Anuj kumar
    public static string EMAIL_FOOTER = @"<p><br></p><p><p class='MsoNormal'>Sincerely,<o:p></o:p></p> <p class='MsoNormal'><b><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:#115794'>&nbsp;</span></b></p> <p class='MsoNormal'><b><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:#115794'>Paysafe Payment Solutions – Compliance Department</span></b><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;;color:#115794'><o:p></o:p></span></p> <p class='MsoNormal'><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:#1F497D'>Office:&nbsp; 949.788.1010<o:p></o:p></span></p> <p class='MsoNormal'><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:#1F497D'>Fax:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 949.861.9240<o:p></o:p></span></p> <p class='MsoNormal'><u><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:blue'><a href='mailto:compliance_pp@paysafe.com'>compliance_pp@paysafe.com</a></span></u><span style='font-family: 'Century Gothic', sans-serif;'><o:p></o:p></span></p> <p class='MsoNormal'><a href='" + ResourceService.AppResources["MeritusWebsite"] + "'><span style='font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;'>https://www.paysafe.com</span></a><span style='font-family: 'Century Gothic', sans-serif;'><o:p></o:p></span></p> <p class='MsoNormal'><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;; color:#1F497D'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span><span style='font-size:10.0pt;font-family:&quot;Arial&quot;,&quot;sans-serif&quot;'><o:p></o:p></span></p> <p class='MsoNormal'><span style='color:#1F497D'>&nbsp;</span><a href='https://www.paymentxp.com/'><span style='text-decoration:none; text-underline:none'><!--[if gte vml 1]><v:shapetype id='_x0000_t75' coordsize='21600,21600' o:spt='75' o:preferrelative='t' path='m@4@5l@4@11@9@11@9@5xe' filled='f' stroked='f'> <v:stroke joinstyle='miter'/> <v:formulas> <v:f eqn='if lineDrawn pixelLineWidth 0'/> <v:f eqn='sum @0 1 0'/> <v:f eqn='sum 0 0 @1'/> <v:f eqn='prod @2 1 2'/> <v:f eqn='prod @3 21600 pixelWidth'/> <v:f eqn='prod @3 21600 pixelHeight'/> <v:f eqn='sum @0 0 1'/> <v:f eqn='prod @6 1 2'/> <v:f eqn='prod @7 21600 pixelWidth'/> <v:f eqn='sum @8 21600 0'/> <v:f eqn='prod @7 21600 pixelHeight'/> <v:f eqn='sum @10 21600 0'/> </v:formulas> <v:path o:extrusionok='f' gradientshapeok='t' o:connecttype='rect'/> <o:lock v:ext='edit' aspectratio='t'/> </v:shapetype><v:shape id='Picture_x0020_5' o:spid='_x0000_i1025' type='#_x0000_t75' alt='paysafe-full-logo' style='width:168pt;height:68.25pt'> <v:imagedata src='https://www.paymentXP.com/merchants/images/logo.jpg' o:href='cid:image001.gif@01CE3AD1.F54886F0'/> </v:shape><![endif]--><!--[if !vml]--><img border='0' width='224' height='91' src='https://www.paymentXP.com/merchants++/images/logo.jpg' alt='paysafe-full-logo' v:shapes='Picture_x0020_5'><!--[endif]--></span></a><span style='color:#1F497D'><o:p></o:p></span></p> <p class='MsoNormal'><b><span style='font-size:12.0pt;font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:#595959'>Follow us on:</span></b><span style='color:#1F497D'>&nbsp; </span><a href='https://www.linkedin.com/company/paysafegroup/'><span style='color:#1F497D;text-decoration:none;text-underline:none'><!--[if gte vml 1]><v:shape id='Picture_x0020_4' o:spid='_x0000_i1026' type='#_x0000_t75' alt='LinkedInIcon' style='width:22.5pt;height:22.5pt'> <v:imagedata src='https://www.paymentxp.com/images/templates/meritus/LinkedInIcon.jpg' o:href='cid:image002.jpg@01CE3AD1.F54886F0'/> </v:shape><![endif]--><!--[if !vml]--><img border='0' width='30' height='30' src='https://www.paymentxp.com/images/templates/meritus/LinkedInIcon.jpg' alt='LinkedInIcon' v:shapes='Picture_x0020_4'><!--[endif]--></span></a><span style='color:#1F497D'>&nbsp;&nbsp;</span><a href='https://www.facebook.com/PlugIntoPaysafe/'><span style='color:#1F497D;text-decoration:none;text-underline:none'><!--[if gte vml 1]><v:shape id='Picture_x0020_3' o:spid='_x0000_i1027' type='#_x0000_t75' alt='FacebookIcon' style='width:22.5pt;height:22.5pt'> <v:imagedata src='https://www.paymentxp.com/images/templates/meritus/FacebookIcon.jpg' o:href='cid:image003.jpg@01CE3AD1.F54886F0'/> </v:shape><![endif]--><!--[if !vml]--><img border='0' width='30' height='30' src='https://www.paymentxp.com/images/templates/meritus/FacebookIcon.jpg' alt='FacebookIcon' v:shapes='Picture_x0020_3'><!--[endif]--></span></a><span style='color:#1F497D'>&nbsp;&nbsp;</span><a href='https://plus.google.com/101028556866176407277'><span style='color:#1F497D; text-decoration:none;text-underline:none'><!--[if gte vml 1]><v:shape id='Picture_x0020_2' o:spid='_x0000_i1028' type='#_x0000_t75' alt='GooglePlusIcon' style='width:22.5pt; height:22.5pt'> <v:imagedata src='https://www.paymentxp.com/images/templates/meritus/GooglePlusIcon.jpg' o:href='cid:image004.jpg@01CE3AD1.F54886F0'/> </v:shape><![endif]--><!--[if !vml]--><img border='0' width='30' height='30' src='https://www.paymentxp.com/images/templates/meritus/GooglePlusIcon.jpg' alt='GooglePlusIcon' v:shapes='Picture_x0020_2'><!--[endif]--></span></a><span style='color:#1F497D'>&nbsp;&nbsp;</span><a href='https://twitter.com/PlugIntoPaysafe'><span style='color:#1F497D; text-decoration:none;text-underline:none'><!--[if gte vml 1]><v:shape id='Picture_x0020_1' o:spid='_x0000_i1029' type='#_x0000_t75' alt='TwitterIcon' style='width:22.5pt; height:22.5pt'> <v:imagedata src='https://www.paymentxp.com/images/templates/meritus/TwitterIcon.jpg' o:href='cid:image005.jpg@01CE3AD1.F54886F0'/> </v:shape><![endif]--><!--[if !vml]--><img border='0' width='30' height='30' src='https://www.paymentxp.com/images/templates/meritus/TwitterIcon.jpg' alt='TwitterIcon' v:shapes='Picture_x0020_1'><!--[endif]--></span></a><span style='color:#1F497D'><o:p></o:p></span></p> <p class='MsoNormal'><o:p>&nbsp;</o:p></p> <p class='MsoNormal'><b><span style='font-size:7.5pt;font-family:&quot;Century Gothic&quot;,&quot;sans-serif&quot;; color:navy'>The information contained in this transmission may contain privileged and confidential information. It is intended only for the use of the addressee. If the person actually receiving this communication or any other reader of the communication is not the named recipient, or the employee or agent responsible to deliver it to the recipient, any use, dissemination, distribution or copying of this communication is strictly prohibited. If you have received this communication in error, please immediately notify us by return e-mail, and destroy this communication and all copies thereof, including all attachments.</span></b><o:p></o:p></p></p>";


    public byte[] GetPDFDocument()
    {
        byte[] mybytearr = null;

        try
        {
            MemoryStream ms = new MemoryStream();

            Document document = new Document();

            WSCompliance obj = UserSessions.CurrentWSCompliance;

            MerchantApp objMA = UserSessions.CurrentMerchantApp;

            List<WSCompXL> li = WSComplianceFacade.GetWSCompXL(obj, WSComplianceFacade.liHide);

            decimal p_Earned = 0;
            decimal p_Possible = 0;

            foreach (WSCompXL i in li)
            {
                p_Earned += i.PointsEarned;
                p_Possible += i.PointsPossible;
            }

            document = new Document(PageSize.LETTER.Rotate(), 50, 50, 10, 20);

            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            document.Open();

            BaseFont bffont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            iTextSharp.text.Font myfontheading = new iTextSharp.text.Font(bffont, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
            iTextSharp.text.Font myfontbold = new iTextSharp.text.Font(bffont, 7, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);
            iTextSharp.text.Font myfontnormal = new iTextSharp.text.Font(bffont, 7);
            iTextSharp.text.Font myfontsmall = new iTextSharp.text.Font(bffont, 6);
            iTextSharp.text.Font myfontsmallred = new iTextSharp.text.Font(bffont, 6, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.RED);
            iTextSharp.text.Font myfontboldred = new iTextSharp.text.Font(bffont, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.RED);
            iTextSharp.text.Font myfontheadingul = new iTextSharp.text.Font(bffont, 14, iTextSharp.text.Font.BOLD, iTextSharp.text.Color.BLACK);




            // to put the logo and the title on the same line, i created a table, with 4 cells: image, blank, title, blank
            // the image seems to expand to fit the container, so i made the table cell just perfect.
            PdfPTable table0 = new PdfPTable(4);

            table0.SetWidths(new int[4] { 91, 138, 229, 229 });
            table0.TotalWidth = 687f;
            table0.LockedWidth = true;
            table0.DefaultCell.Border = PdfPCell.NO_BORDER;

            if (!CommonUtility.Util.IsValidGuid(objMA.PrivateLabelUID))
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Images/PaysafeNotificationLogo.PNG");
                iTextSharp.text.Image logo1 = iTextSharp.text.Image.GetInstance(path);
                logo1.ScalePercent(75);
                table0.AddCell(logo1);
            }
            else
            {
                table0.AddCell(new Paragraph());
            }

            table0.AddCell(new Paragraph());
            //Paragraph phead = new Paragraph("Website Compliance Review", myfontheadingul);

            Chunk chkHeader = new Chunk("Website Compliance Review", myfontheading);
            chkHeader.SetUnderline(1f, -2f);
            Paragraph phead = new Paragraph(chkHeader);
            phead.Alignment = 1; // center;
            table0.AddCell(phead);

            table0.AddCell(new Paragraph());

            document.Add(table0);







            PdfPTable table1 = new PdfPTable(4);
            table1.DefaultCell.Border = PdfPCell.NO_BORDER;

            table1.AddCell(new Paragraph("Date of Review:", myfontbold));
            table1.AddCell(new Paragraph(obj.DateCompletedBy.ToShortDateString().Trim(), myfontnormal));
            table1.AddCell(new Paragraph());
            table1.AddCell(new Paragraph());


            table1.AddCell(new Paragraph("Review ID:", myfontbold));
            table1.AddCell(new Paragraph(obj.TicketID.ToString(), myfontnormal));
            PdfPCell pCell1 = new PdfPCell(new Phrase("Compliance Rating", myfontbold));
            pCell1.BorderColorTop = iTextSharp.text.Color.BLACK;
            pCell1.BorderColorRight = iTextSharp.text.Color.BLACK;
            pCell1.BorderColorLeft = iTextSharp.text.Color.BLACK;
            pCell1.BorderColorBottom = iTextSharp.text.Color.BLACK;
            pCell1.BorderWidthTop = 1f;
            pCell1.BorderWidthRight = 1f;
            pCell1.BorderWidthLeft = 1f;
            pCell1.BorderWidthBottom = 1f;
            pCell1.HorizontalAlignment = 1; // center;
            pCell1.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;
            table1.AddCell(pCell1);
            table1.AddCell(new Paragraph());


            string score_percentage = "0.00 %";
            string score_numdom = "N/A";

            if (UserSessions.CurrentWSCompliance != null && UserSessions.CurrentWSCompliance.PointsPossible > 0)
            {
                score_percentage = string.Format("{0:f0}%", (UserSessions.CurrentWSCompliance.PointsEarned / UserSessions.CurrentWSCompliance.PointsPossible) * 100);
                score_numdom = string.Format("{0:f1} / {1:f1} Points", UserSessions.CurrentWSCompliance.PointsEarned, UserSessions.CurrentWSCompliance.PointsPossible);
            }


            table1.AddCell(new Paragraph("Website URL:", myfontbold));
            table1.AddCell(new Paragraph(objMA.BusinessWebsite, myfontnormal));
            PdfPCell pCell2 = new PdfPCell(new Phrase(score_percentage, myfontbold));
            pCell2.BorderColorTop = iTextSharp.text.Color.WHITE;
            pCell2.BorderColorRight = iTextSharp.text.Color.BLACK;
            pCell2.BorderColorLeft = iTextSharp.text.Color.BLACK;
            pCell2.BorderColorBottom = iTextSharp.text.Color.BLACK;
            pCell2.BorderWidthTop = 0f;
            pCell2.BorderWidthRight = 1f;
            pCell2.BorderWidthLeft = 1f;
            pCell2.BorderWidthBottom = 0.5f;
            pCell2.HorizontalAlignment = 1; // center;
            table1.AddCell(pCell2);
            table1.AddCell(new Paragraph());


            table1.AddCell(new Paragraph("DBA Name:", myfontbold));
            table1.AddCell(new Paragraph(objMA.BusinessDBAName, myfontnormal));
            PdfPCell pCell3 = new PdfPCell(new Phrase(score_numdom, myfontbold));
            pCell3.BorderColorTop = iTextSharp.text.Color.WHITE;
            pCell3.BorderColorRight = iTextSharp.text.Color.BLACK;
            pCell3.BorderColorLeft = iTextSharp.text.Color.BLACK;
            pCell3.BorderColorBottom = iTextSharp.text.Color.BLACK;
            pCell3.BorderWidthTop = 0f;
            pCell3.BorderWidthRight = 1f;
            pCell3.BorderWidthLeft = 1f;
            pCell3.BorderWidthBottom = 1f;
            pCell3.HorizontalAlignment = 1; // center;
            table1.AddCell(pCell3);
            table1.AddCell(new Paragraph());


            table1.AddCell(new Paragraph("Merchant ID:", myfontbold));
            table1.AddCell(new Paragraph(objMA.SettlePlatformMid, myfontnormal));
            table1.AddCell(new Paragraph());
            table1.AddCell(new Paragraph());



            //table1.AddCell(new Paragraph("Result:", myfontbold));
            //table1.AddCell(new Paragraph(obj.StatusName, myfontnormal));


            //table1.AddCell(new Paragraph("Compliance Rating:", myfontbold));
            //table1.AddCell(new Paragraph(string.Format("{0:f1}%", 100 * (p_Earned / p_Possible)), myfontnormal));

            //table1.AddCell(new Paragraph("Earned / Possible:", myfontbold));
            //table1.AddCell(new Paragraph(string.Format("{0} / {1}", p_Earned, p_Possible), myfontnormal));

            table1.HorizontalAlignment = Element.ALIGN_LEFT;
            table1.SetWidths(new int[4] { 10, 41, 19, 30 });
            table1.TotalWidth = 687f;
            table1.LockedWidth = true;

            document.Add(table1);


            PdfPTable table2 = new PdfPTable(6);
            //table2.SetWidths(new int[3] { 9, 51, 40 });
            table2.SetWidths(new int[6] { 51, 9, 6, 6, 6, 22 });
            table2.TotalWidth = 687f;
            table2.LockedWidth = true;



            PdfPCell pDescription = new PdfPCell(new Paragraph("Description", myfontbold));
            pDescription.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;

            PdfPCell pStatus = new PdfPCell(new Paragraph("Status", myfontbold));
            pStatus.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;

            PdfPCell pPossible = new PdfPCell(new Paragraph("Possible", myfontbold));
            pPossible.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;
            PdfPCell pMissed = new PdfPCell(new Paragraph("Missed", myfontbold));
            pMissed.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;
            PdfPCell pEarned = new PdfPCell(new Paragraph("Earned", myfontbold));
            pEarned.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;

            PdfPCell pComments = new PdfPCell(new Paragraph("Comments", myfontbold));
            pComments.BackgroundColor = iTextSharp.text.Color.LIGHT_GRAY;


            table2.AddCell(pDescription);
            table2.AddCell(pStatus);
            table2.AddCell(pPossible);
            table2.AddCell(pMissed);
            table2.AddCell(pEarned);
            table2.AddCell(pComments);

            // 
            WSCompXL itemComments = null;

            decimal running_possible = 0;
            decimal running_missed = 0;
            decimal running_earned = 0;

            // add meat
            foreach (WSCompXL item in li)
            {

                if (item.Description.Trim().ToLower() == "additional comments")
                {
                    itemComments = item;
                    continue;
                }

                PdfPCell paraDescriptionCell = new PdfPCell(new Paragraph(item.Description, myfontsmall));
                paraDescriptionCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                table2.AddCell(paraDescriptionCell);

                PdfPCell paraStatusCell = new PdfPCell(new Paragraph(item.Status, myfontsmall));
                paraStatusCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                table2.AddCell(paraStatusCell);

                PdfPCell paraPointsPossibleCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", item.PointsPossible), myfontsmall));
                paraPointsPossibleCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                paraPointsPossibleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(paraPointsPossibleCell);
                running_possible += item.PointsPossible;


                if (item.PointsMissed > 0)
                {
                    PdfPCell paraPointsMissedCell = new PdfPCell(new Paragraph(string.Format("({0:f1})", item.PointsMissed), myfontsmallred));
                    paraPointsMissedCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                    paraPointsMissedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(paraPointsMissedCell);
                }
                else
                {
                    PdfPCell paraPointsMissedCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", item.PointsMissed), myfontsmall));
                    paraPointsMissedCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                    paraPointsMissedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    table2.AddCell(paraPointsMissedCell);
                }

                running_missed += item.PointsMissed;

                PdfPCell paraPointsEarnedCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", item.PointsEarned), myfontsmall));
                paraPointsEarnedCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                paraPointsEarnedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(paraPointsEarnedCell);
                running_earned += item.PointsEarned;

                PdfPCell paraCommentsCell = new PdfPCell(new Paragraph(item.Comments, myfontsmall));
                paraCommentsCell.BackgroundColor = (item.IsPink) ? iTextSharp.text.Color.PINK : iTextSharp.text.Color.WHITE;
                table2.AddCell(paraCommentsCell);
            }

            // add footer
            table2.AddCell(new PdfPCell(new Paragraph("", myfontsmall)));

            PdfPCell paraTotalTotalCell = new PdfPCell(new Paragraph("Totals", myfontbold));
            paraTotalTotalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(paraTotalTotalCell);

            PdfPCell paraTotalPointsPossibleCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", running_possible), myfontbold));
            paraTotalPointsPossibleCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(paraTotalPointsPossibleCell);

            if (running_missed > 0)
            {
                PdfPCell paraTotalPointsMissedCell = new PdfPCell(new Paragraph(string.Format("({0:f1})", running_missed), myfontboldred));
                paraTotalPointsMissedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(paraTotalPointsMissedCell);
            }
            else
            {
                PdfPCell paraTotalPointsMissedCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", running_missed), myfontbold));
                paraTotalPointsMissedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table2.AddCell(paraTotalPointsMissedCell);
            }


            PdfPCell paraTotalPointsEarnedCell = new PdfPCell(new Paragraph(string.Format("{0:f1}", running_earned), myfontbold));
            paraTotalPointsEarnedCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            table2.AddCell(paraTotalPointsEarnedCell);

            table2.AddCell(new PdfPCell(new Paragraph("", myfontsmall)));


            table2.SpacingBefore = 10f;
            table2.SpacingAfter = 10f;

            document.Add(table2);



            PdfPTable tablepara = new PdfPTable(1);
            PdfPCell pPara = new PdfPCell(new Paragraph(string.Format("Additional Comments: {0}", itemComments.Comments), myfontnormal));
            pPara.BorderWidth = 1f;
            pPara.BorderColor = iTextSharp.text.Color.BLACK;
            tablepara.SetWidths(new int[1] { 687 });
            tablepara.AddCell(pPara);

            document.Add(tablepara);





            //Paragraph pfooter1 = new Paragraph("1901 E. Alton Avenue, Suite 220, Santa Ana, CA 92705", myfontsmall);
            //pfooter1.Alignment = 1;
            //Paragraph pfooter2 = new Paragraph("Phone: (949) 788-1010 | Fax: (949) 315-3216", myfontsmall);
            //pfooter2.Alignment = 1;
            //Paragraph pfooter3 = new Paragraph("merituspayment.com", myfontsmall);
            //pfooter3.Alignment = 1;

            //document.Add(pfooter1);
            //document.Add(pfooter2);
            //document.Add(pfooter3);

            document.Close();

            mybytearr = ms.ToArray();



        }
        catch(Exception ex)
        {

        }

        return mybytearr;

    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {

        Document document = new Document();



        byte[] arr = this.GetPDFDocument();

        if (document != null)
        {

            string filename = String.Format("{0}_{1}_Website-Compliance-Review.pdf", CommonUtility.Util.GenerateSlug(UserSessions.CurrentMerchantApp.BusinessDBAName), UserSessions.CurrentMerchantApp.SettlePlatformMid);

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(arr, 0, arr.Length);
            Response.OutputStream.Flush();
            Response.End();


        }

    }

    protected string WritePDFToFile()
    {
        string full_path = "";

        byte[] arr = this.GetPDFDocument();

        if (arr != null && arr.Length > 0)
        {
            if (!Directory.Exists(ConfigurationManager.AppSettings["TempUploadDir"]))
            {
                Directory.CreateDirectory(ConfigurationManager.AppSettings["TempUploadDir"]);
            }

            string temp_dir = string.Format(@"{0}\", ConfigurationManager.AppSettings["TempUploadDir"]);
            string prefix = string.Format("{0}__", Guid.NewGuid().ToString());
            string filename = String.Format("{0}_{1}_Website-Compliance-Review.pdf", CommonUtility.Util.GenerateSlug(UserSessions.CurrentMerchantApp.BusinessDBAName), UserSessions.CurrentMerchantApp.SettlePlatformMid);

            full_path = temp_dir + prefix + filename;

            FileStream file = new FileStream(full_path, FileMode.Create, System.IO.FileAccess.Write);
            file.Write(arr, 0, arr.Length);
            file.Close();
        }

        return full_path;
    }

    protected void btnInitiateEmail_Click(object sender, EventArgs e)
    {

        // normally the contact list is not pulled automatically. so we pull it manually on load.
        if (UserSessions.CurrentMerchantApp.ContactList == null)
        {
            UserSessions.CurrentMerchantApp.ContactList = DataContact.SearchContact(Convert.ToInt32(UserSessions.CurrentMerchantApp.ID), eControlContactType.Merchant);
        }

        List<string> liTo = new List<string>();
        List<string> liCC = new List<string>();


        string bTo = "";
        string bFrom = Constants.COMPLIANCE_PP_EMAIL;// "compliance_pp@paysafe.com";
        string bCC = "";
        string bSubject = "";
        List<string> liFiles = new List<string>();
        string bBody = "";

        if (UserSessions.CurrentMerchantApp.ContactList != null && UserSessions.CurrentMerchantApp.ContactList.Count > 0)
        {
            foreach (Contact c in UserSessions.CurrentMerchantApp.ContactList)
            {
                if (c.IsActive)
                {
                    if (c.IsPrimary)
                    {
                        if (c.GetFirstEmail() != null)
                        {
                            liTo.Add(c.GetFirstEmail().Address);
                        }
                    }
                    else
                    {
                        if (c.GetFirstEmail() != null)
                        {
                            liCC.Add(c.GetFirstEmail().Address);
                        }

                    }
                }
            }

            if (UserSessions.CurrentMerchantApp.FirstTeam && CommonUtility.Util.IsValidGuid(UserSessions.CurrentMerchantApp.FirstTeamRepUID))
            {
                User FTU = DataUser.GetInstance().GetUser(UserSessions.CurrentMerchantApp.FirstTeamRepUID);

                if (FTU != null)
                {
                    liCC.Add(FTU.Email);
                }
            }

            bTo = CommonUtility.Util.implode(liTo, ";");
            bCC = CommonUtility.Util.implode(liCC, ";");



            // if for some reason, there is no primary contact in the merchants contacts, then fall back to the notification emails
            if (string.IsNullOrEmpty(bTo))
            {
                bTo = UserSessions.CurrentMerchantApp.NotificationEmails;
            }
            else if (!string.IsNullOrWhiteSpace(UserSessions.CurrentMerchantApp.NotificationEmails))
            {
                // otherwise, tack on the notification email to the CC.
                bCC += ";" + UserSessions.CurrentMerchantApp.NotificationEmails.Trim();
            }
        }

        bSubject = string.Format("Website Compliance Alert - {0} (MID: {1})", UserSessions.CurrentMerchantApp.BusinessDBAName, UserSessions.CurrentMerchantApp.SettlePlatformMid);

        bBody = WSComplianceFacade.GetPreviewText(UserSessions.CurrentWSCompliance, UserSessions.CurrentMerchantApp);


        string pdf_compliance_file = this.WritePDFToFile();

        string pdf_clean_name = this.remove_guid_prefix(pdf_compliance_file);

        liFiles.Add(pdf_clean_name);

        Hashtable prms = new Hashtable();
        prms.Add("@PrimaryKeyID", UserSessions.CurrentTicket.TicketID);
        prms.Add("@MDocSourceID", (int)MDoc.eMDocSourceID.Tickets);
        List<MDoc> li = DataDocuments.GetInstance().GetMDocuments(prms);

        if (li != null)
        {
            foreach (MDoc m in li)
            {
                liFiles.Add(m.OrigName);
            }
        }


        this.emailclick(bTo, bFrom, bCC, bSubject, liFiles, bBody, pdf_compliance_file, pdf_clean_name);

    }
    /// <summary>
    /// 2240595b-4d9a-4c6d-bec8-96597c1859bc__my-clean-colon_5482984000803355_Website-Compliance-Review -> my-clean-colon_5482984000803355_Website-Compliance-Review
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public string remove_guid_prefix(string str)
    {
        string ret = "";

        if (!string.IsNullOrEmpty(str))
        {

            string myfile = Path.GetFileName(str);

            string[] arr = myfile.Split(new char[] { '_' });

            if (arr.Length > 0)
            {
                if (CommonUtility.Util.IsValidGuid(arr[0]))
                {
                    ret = myfile.Replace(arr[0] + "__", "");
                }
            }
        }

        return ret;
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[3].Text = WebUtil.ConvertToUserDateTimeSettings(e.Row.Cells[3].Text);
    }

}
