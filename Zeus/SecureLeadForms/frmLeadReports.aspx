<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmLeadReports" MasterPageFile="~/MasterPageSales.master"
    CodeBehind="frmLeadReports.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPageSales.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <table width="100%">
                <tr>
                    <td align="center">
                        <div>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkMonthly" NavigateUrl="~/SecureLeadForms/frmSalesSummaryReport.aspx"
                                    runat="server">Inside Sales Summary Report</asp:HyperLink><br />
                                Use this Report to search monthly summary for Inside Sales.
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkDetailedWeekly" NavigateUrl="~/SecureLeadForms/frmDetailedWeeklyReport.aspx"
                                    runat="server">Inside Sales Weekly Detailed Report (Applications)</asp:HyperLink><br />
                                Use this report to search the weekly summary of Applications for Inside Sales.
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkWeekly" NavigateUrl="~/SecureLeadForms/frmWeeklySummaryReport.aspx"
                                    runat="server">Inside Sales Weekly Report (Calls, Statements, Attendance)</asp:HyperLink><br />
                                Use this report to search the weekly summary of Calls,Statements for Inside Sales.
                            </p>
                        </div>
                    </td>
                    <td align="center">
                        <div>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkReportDowngradeSummary" NavigateUrl="~/SecureLeadForms/frmDownGradeSummary.aspx"
                                    runat="server">Downgrade Summary Report</asp:HyperLink><br />
                                Add Comments
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkPipelineStatus" NavigateUrl="~/SecureReports/frmPipelineStatusReport.aspx"
                                    Enabled="true" runat="server">Pipeline Status Report</asp:HyperLink><br />
                                This report will show the current count for each Agents's lead's statuses and manage
                                pipeline/ sales cycle
                            </p>
                            <p class="reportbucket">
                                <asp:HyperLink ID="lnkLeadActivity" NavigateUrl="~/SecureReports/frmLeadActivityReport.aspx"
                                    Enabled="true" runat="server">Lead Activity Report</asp:HyperLink><br />
                                This report will show current leads status and help prioritize which leads to work
                                on/ follow up
                            </p>
                        </div>
                        <%-- <p class="reportbucket">
                            <asp:HyperLink ID="lnkActiveClient" NavigateUrl="~/SecureLeadForms/frmSalesAciveClientReport.aspx"
                                runat="server" Enabled="true" Style="display: none;">
                        Active Client Report
                            </asp:HyperLink><br />
                            <br />
                        </p>--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
