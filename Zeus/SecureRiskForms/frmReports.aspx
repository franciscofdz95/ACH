<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="SecureRiskForms_frmReports" Title="Reports" Codebehind="frmReports.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
        <fieldset class="dialog" style="width: 40%;">
            <legend>Reports</legend>
            <table width="100%" border="0">
                <tr>
                    <td align="center" valign="top">
                        <div>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkBatchSuspense" NavigateUrl="~/SecureRiskForms/frmBatchSuspenseRate.aspx"
                                    runat="server">Batch Suspense Rates Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkSuspenseReason" NavigateUrl="~/SecureRiskForms/frmSuspenseReasons.aspx"
                                    runat="server">Suspense Reasons Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkDeclineRatios" NavigateUrl="~/SecureRiskForms/frmDeclineRatios.aspx"
                                    runat="server">Decline Ratios Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkDeclineReasons" NavigateUrl="~/SecureRiskForms/frmDeclineReasons.aspx"
                                    runat="server">Decline Reasons Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkAVSSummary" NavigateUrl="~/SecureRiskForms/frmAVSSummary.aspx"
                                    runat="server">AVS Summary Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnk45DayInactivity" NavigateUrl="~/SecureRiskForms/frm45DaysInactivity.aspx"
                                    runat="server">45 Days Inactivity</asp:HyperLink><br />
                            </p>
                        </div>
                    </td>
                    <td valign="top">
                        <div>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkCVV2Summary" NavigateUrl="~/SecureRiskForms/frmCVV2Summary.aspx"
                                    runat="server">CVV2 Summary Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkRiskRatios" NavigateUrl="~/SecureRiskForms/frmDeclineRiskRatios.aspx"
                                    runat="server" Enabled="false">Decline Risk Ratios</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkManagerDaily" NavigateUrl="~/SecureRiskForms/frmManagerDaily.aspx"
                                    runat="server" Enabled="false">Manager Daily Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkManagerWeekly" NavigateUrl="~/SecureRiskForms/frmManagerWeekly.aspx"
                                    runat="server" Enabled="false">Manager Weekly Report</asp:HyperLink><br />
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkManagerWeeklyTrend" NavigateUrl="~/SecureRiskForms/frmManagerWeeklyTrend.aspx"
                                    runat="server" Enabled="false">Manager Weekly Trend Report</asp:HyperLink><br />
                            </p>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    
</asp:Content>
