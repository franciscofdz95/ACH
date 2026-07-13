using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using PaymentXP.BusinessObjects;
using PaymentXP.DataObjects;

public partial class UserControls_wucWSComplianceGrid : wucBaseSearch
{

    public delegate void GridViewRowCommandHandler(object sender, string TicketUID, string WSComplianceID);
    public event GridViewRowCommandHandler GridViewCommand;


    private int _MerchantID;

    public int MerchantID
    {
        get { return _MerchantID; }
        set { _MerchantID = value; }
    }

    public void SetDataSource(Hashtable prms, int pagesize)
    {
        GridView1.DataSourceID = "ods";
        this.CurrentPage = 1;
        this.PageSize = pagesize;
        GridView1.PageIndex = 0;
        GridView1.PageSize = pagesize;
        this.m_Prms = prms;
        BindGrid();



    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "view")
        {

            string[] arr = CommonUtility.Util.if_s(e.CommandArgument, "").Split(new char[] { '_' });
            string ticket_uid = arr[0];
            string wscompliance_id = arr[1];

            if (this.GridViewCommand != null)
            {
                this.GridViewCommand(GridView1, ticket_uid, wscompliance_id);
            }
        }


    }

    public void ForceRefresh()
    {
        this.BindGrid();
    }

    private void BindGrid()
    {

        // force to biggest. no paging. we get redirect endless loops.
        this.PageSize = 999;

        if (!m_Prms.ContainsKey("@PageSize"))
            m_Prms.Add("@PageSize", this.PageSize);
        else
            m_Prms["@PageSize"] = this.PageSize;

        if (!m_Prms.ContainsKey("@CurrentPage"))
            m_Prms.Add("@CurrentPage", this.CurrentPage);
        else
            m_Prms["@CurrentPage"] = this.CurrentPage;

        if (!m_Prms.ContainsKey("@SortOrder"))
            m_Prms.Add("@SortOrder", "DateCreated");
        else
            m_Prms["@SortOrder"] = this.SortOrder;

        m_Prms["@SortDirection"] = SqlUtil.ConvertSortDirectionToSql(this.SortDirectionSearch);

        //if (!m_Prms.ContainsKey("@MerchantID"))
        //    m_Prms.Add("@MerchantID", this.MerchantID);
        //else
        //    m_Prms["@MerchantID"] = this.MerchantID;






        int rowcount = DataMerchantAppPaging.GetWSCompliancePagingCount(m_Prms, this.PageSize, this.CurrentPage);

        lblRowCount.Text = rowcount.ToString();

        GridView1.DataBind();
    }




    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {

            case DataControlRowType.DataRow:

                //e.Row.Cells[2].Text = WebUtil.ConvertDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                //e.Row.Cells[6].Text = WebUtil.ConvertDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCompletedBy")));

                Label lbReqType = (Label)e.Row.FindControl("lblRequestType");

                if (lbReqType.Text == "0" || lbReqType.Text == "2")
                {
                    lbReqType.Text = "Requested";
                }
                else if (lbReqType.Text == "1")
                {
                    lbReqType.Text = "Scheduled";
                }
                else if (lbReqType.Text == "3")
                {
                    lbReqType.Text = "User Initiated";
                }


                Label lbCompStatus = (Label)e.Row.FindControl("lblComplianceStatus");

                switch (lbCompStatus.Text.ToUpper().Trim())
                {
                    case Constants.COMPLIANCE_NONCOMPLIANT:
                        {
                            lbCompStatus.Text = "Complete (Non-Compliant)";
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");
                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            lbRiskScore.Text = "N/A";
                            lbScore.Text = "N/A";

                        }
                        break;

                    case Constants.COMPLIANCE_COMPLIANT:
                        {
                            lbCompStatus.Text = "Complete (Compliant)";
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");
                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            lbRiskScore.Text = "N/A";
                            lbScore.Text = "N/A";
                        }
                        break;

                    case Constants.COMPLIANCE_TBD:
                        lbCompStatus.Text = "TBD";
                        break;

                    case Constants.COMPLIANCE_WEBSITEINACTIVE:
                        {
                            lbCompStatus.Text = "Complete (Website Inactive)";
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");
                            lbRiskScore.Text = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "RiskIndex"), 0).ToString();

                            if (lbRiskScore.Text == "0")
                            {
                                lbRiskScore.Text = "N/A";
                            }
                            
                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            lbScore.Text = "N/A";
                        }
                        break;

                    case Constants.COMPLIANCE_NOECOMMERCE:
                        {
                            lbCompStatus.Text = "Complete (No E-Commerce)";
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");
                            lbRiskScore.Text = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "RiskIndex"), 0).ToString();
                            if (lbRiskScore.Text == "0")
                            {
                                lbRiskScore.Text = "N/A";
                            }
                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            lbScore.Text = "N/A";
                        }
                        break;

                    case Constants.COMPLIANCE_CANCELLED:
                        {
                            lbCompStatus.Text = "Cancelled";
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");
                            lbRiskScore.Text = "N/A";
                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            lbScore.Text = "N/A";
                        }
                        break;

                    case Constants.COMPLIANCE_REVIEWCOMPLETE:
                        {
                            lbCompStatus.Text = "Complete";

                            Label lbScore = (Label)e.Row.FindControl("lblScore");
                            Label lbRiskScore = (Label)e.Row.FindControl("lblRiskScore");

                            int risk_index = CommonUtility.Util.if_i(DataBinder.Eval(e.Row.DataItem, "RiskIndex"), 0);
                            decimal points_possible = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "PointsPossible"), 0);
                            decimal points_earned = CommonUtility.Util.if_dec(DataBinder.Eval(e.Row.DataItem, "PointsEarned"), 0);

                            if (points_possible != 0)
                            {
                                lbRiskScore.Text = risk_index.ToString();
                                if (lbRiskScore.Text == "0")
                                {
                                    lbRiskScore.Text = "N/A";
                                }
                                lbScore.Text = string.Format("{0:f0}%", 100 * (points_earned / points_possible));
                            }
                            else
                            {
                                lbRiskScore.Text = "N/A";
                                lbScore.Text = "N/A";
                            }
                        }
                        break;
                }

                //if (lbCompStatus.Text.ToUpper().Trim() == Constants.COMPLIANCE_NA)
                //{
                //    lbCompStatus.Text = "Requested";
                //}
                //else if (lbCompStatus.Text == "1")
                //{
                //    lbCompStatus.Text = "Scheduled";
                //}
                string datecreated = WebUtil.ConvertToUserShortDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCreated")));
                string datecompletedby = WebUtil.ConvertToUserShortDateTimeFormat(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "DateCompletedBy")));
                e.Row.Cells[1].Text = string.IsNullOrEmpty(datecreated) ? "" : datecreated;
                e.Row.Cells[5].Text = string.IsNullOrEmpty(datecompletedby) ? "" :datecompletedby;


                break;
        }
    }


    protected void ods_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        e.InputParameters[0] = this.m_Prms;
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.CurrentPage = 1;
        this.SortOrder = e.SortExpression;
        this.SortDirectionSearch = e.SortDirection;
        this.BindGrid();
    }


    //protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    this.CurrentPage = e.NewPageIndex + 1;
    //    this.BindGrid();
    //}


}