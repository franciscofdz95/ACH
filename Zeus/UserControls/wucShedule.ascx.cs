using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class wucShedule : System.Web.UI.UserControl
{


    public void OpenWindow(string tcid)
    {
        TCID.Value = tcid;

        Hashtable prms = new Hashtable();
        prms.Add("@TCID", tcid);
        IList<TermsCondition> list = DataAccess.DataRiskDao.GetTermsConditions(prms);


        Schedule schedule = list[0].TCSchedule;
        IsEnabled.Checked = schedule.IsEnabled;
        switch (schedule.OccurenceOption)
        {
            case OccurenceOptions.Daily:
                ListHandler.ListFindItem(cboOccurenceOption, Convert.ToInt32(schedule.OccurenceOption).ToString()); 
                ListHandler.ListFindItem(cboDayly, Convert.ToInt32(schedule.DayOfMonthOption).ToString());

                break;
            case OccurenceOptions.Monthly:
                ListHandler.ListFindItem(cboOccurenceOption, Convert.ToInt32(schedule.OccurenceOption).ToString()); 
                ListHandler.ListFindItem(cboMontylyDay, Convert.ToInt32(schedule.DayOfMonthOption).ToString());
                ListHandler.ListFindItem(cboMontylyMonth, Convert.ToInt32(schedule.MonthOfYearOption).ToString());
                
                break;
        }

        SetOccurrencePanels();
        SetEnableColor();

        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Normal;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            cboOccurenceOption.SelectedIndex = 1;
            SetOccurrencePanels();
            //this.LoadYears();
        }
    }


    public string ValidateData()
    {
        string error = string.Empty;
        

        return error;
    }

    //private void LoadYears()
    //{

    //    cboMonthlyDurYear.Items.Clear();
    //    cboMonthlyDurYearEnd.Items.Clear();
    //    for (int i = 0; i < 20; i++)
    //    {
    //        string year = DateTime.Today.AddYears(i - 2).ToString("yyyy");
    //        cboMonthlyDurYear.Items.Add(new ListItem(year, year));
    //        cboMonthlyDurYearEnd.Items.Add(new ListItem(year, year));
    //    }


    //}

    public void FormNew()
    {

    }
    protected void Occurrence_Selected(object sender, EventArgs e)
    {
        SetOccurrencePanels();
    }

    

    public void SetOccurrencePanels()
    {
        switch (cboOccurenceOption.SelectedValue)
        {
            case "1":
                pnOptDayly.Visible = true;
                //mark 05-27-2010 pnDurationDayly.Visible = true;
                pnOptMonthly.Visible = false;
                break;
            case "2":
                pnOptMonthly.Visible = true;
                //mark 05-27-2010 pnDurationMonthly.Visible = true;
                pnOptDayly.Visible = false;
                break;
            default:
                pnOptMonthly.Visible = false;
                pnOptDayly.Visible = false;
                break;
        }
    }

    //protected void btnAddRecurring_Click(object sender, EventArgs e)
    //{
    //    UserSessions.CurrentGroup = "Transaction";
    //    UserSessions.CurrentPage = "PostRecurringACH";
    //    UserSessions.CurrentURL = "./FormTransaction/frmAchRecurringPost.aspx?TransID=" + TransID.Text.ToString();
    //    Response.Redirect("~/frmMain.aspx");

    //}
    //protected void txtEnd_ValueChanged(object sender, Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs e)
    //{
    //    optdDurationEnd.Checked = true;
    //    rdDurationNoEnd.Checked = false;
    //}

    

    public void SetEnableColor()
    {
        if (IsEnabled.Checked)
            lblEnableSchedule.ForeColor = System.Drawing.Color.Green;
        else
            lblEnableSchedule.ForeColor = System.Drawing.Color.Red;
    }

    protected void IsEnabled_CheckedChanged(object sender, EventArgs e)
    {
        SetEnableColor();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Schedule schedule = new Schedule();
        schedule.IsEnabled = IsEnabled.Checked;
        switch (cboOccurenceOption.SelectedItem.Text)
        {
            case "Daily":
                schedule.OccurenceOption = OccurenceOptions.Daily;
                schedule.DayOfMonthOption = (DayOfMonthOptions)Convert.ToInt32(cboDayly.SelectedItem.Value);
                break;
            case "Monthly":
                schedule.OccurenceOption = OccurenceOptions.Monthly;

                schedule.MontlyOption = MontlyOptions.Day;
                schedule.DayOfMonthOption = (DayOfMonthOptions)Convert.ToInt32(cboMontylyDay.SelectedItem.Value);
                schedule.MonthOfYearOption = (MonthOfYearOptions)Convert.ToInt32(cboMontylyMonth.SelectedItem.Value);

                break;
        }

        schedule.IsEndDate = false;

        DataAccess.DataRiskDao.UpdateTermsConditionsSchedule(TCID.Value, UserSessions.CurrentMerchantApp.ID, schedule);

        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
        //schedule.StartDate= DateTime.Today.Add(1)

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        WebDialogWindow1.WindowState = Infragistics.Web.UI.LayoutControls.DialogWindowState.Hidden;
    }
    
}
