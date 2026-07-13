<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="SecureReports_frmReports" Title="Reports" CodeBehind="frmReports.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language='JavaScript' type="text/javascript">

        $(document).ready(function () {

            $("#tabReport").tabs();
        });

    </script>

    <div id="contentpage">
        <fieldset>
            <ig:WebTab ID="WebTab1" runat="server" Height="600px" Width="800px">
                <Tabs>
                    <ig:ContentTabItem runat="server" Text="Applications">
                        <Template>
                            <fieldset>
                                <table cellpadding="10">
                                    <tr>
                                        <td valign="top">
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportDealCountSummary" NavigateUrl="~/SecureReports/frmDealCountSummaryReport.aspx"
                                                    runat="server">Deal Count Summary Report</asp:HyperLink><br />
                                                This report provides a summary of deals received, approved, and declined, and broken
                                                down by Agent.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportMerchantStatusHistory" NavigateUrl="~/SecureReports/frmApplicationStatusHistory.aspx"
                                                    runat="server">Merchant Status History Report</asp:HyperLink><br />
                                                Use this report to search the status history of applications.
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportMerchantStatus" NavigateUrl="~/SecureReports/frmApplicationStatus.aspx"
                                                    runat="server">Merchant Status Report</asp:HyperLink><br />
                                                Every application you've submitted is here.
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportMerchantNotesByStatus" NavigateUrl="~/SecureReports/frmMerchantNotesReport.aspx"
                                                    runat="server">Merchant Notes By Status</asp:HyperLink><br />
                                                Allow users to generate and pull all notes related to a merchant(s).
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportMerchantNotesSearch" NavigateUrl="~/SecureReports/frmMerchantNotesSearch.aspx"
                                                    runat="server">Merchant Notes Search</asp:HyperLink><br />
                                                Search all merchant notes
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportUnderwritingDetails" NavigateUrl="~/SecureReports/frmUWDetailsReport.aspx"
                                                    runat="server">Underwriting Details</asp:HyperLink><br />
                                                This report shows all approved and declined applications.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportUnderwritingMTD" NavigateUrl="~/SecureReports/frmUWMTDReport.aspx"
                                                    runat="server">Underwriting MTD</asp:HyperLink><br />
                                                This report shows MTD statistics for Underwriting.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkAccountGroups" NavigateUrl="~/SecureReports/frmAccountGroupsReport.aspx"
                                                    runat="server">Account Groups</asp:HyperLink><br />
                                                This report shows Account Group statistics.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkMultipleMIDsNote"  
                                                    runat="server" NavigateUrl="~/SecureReports/frmUploadMIDNotes.aspx">Add Notes</asp:HyperLink><br />
                                                Add Notes to multiple ZIDs
                                            </div>
                                        </td>
                                        </td>
                                        <td valign="top"></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" Text="Phone" Hidden="true">
                        <Template>
                            <fieldset>
                                <table cellpadding="10">
                                    <tr>
                                        <td valign="top">
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportExecutiveTeamView" NavigateUrl="~/SecureReports/frmAllAgentsSummary.aspx"
                                                    Enabled="true" runat="server">Executive Team's View Report</asp:HyperLink><br />
                                                This report provides a detailed call summary breakdown.This report can not show
                                                today’s data as the call data is updated at the end of the day.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportSummarAllQueues" NavigateUrl="~/SecureReports/frmAllQueues.aspx"
                                                    Enabled="true" runat="server">Summary Report for All Queues</asp:HyperLink><br />
                                                This report provides a call summary breakdown by hour for all queues.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportSummaryByQueue" NavigateUrl="~/SecureReports/frmAllAgentsByQueue.aspx"
                                                    Enabled="true" runat="server">Summary Report By Queue</asp:HyperLink><br />
                                                This report provides a call summary breakdown by hour for all queues.
                                            </div>
                                        </td>
                                        <td valign="top"></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" Text="Sales Support">
                        <Template>
                            <fieldset>
                                <table cellpadding="10">
                                    <tr>
                                        <td valign="top">
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportAgentNotesSearch" NavigateUrl="~/SecureReports/frmAgentNotesSearch.aspx"
                                                    runat="server">Agent Notes Search</asp:HyperLink><br />
                                                Search all Agent notes
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkCurrentPending" NavigateUrl="~/SecureReports/frmCurrentPendingReport.aspx"
                                                    Enabled="true" runat="server">Current Pending Report</asp:HyperLink><br />
                                                This report shows a list of current pending applications with items required.
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkEmailBlaster" NavigateUrl="~/SecureReports/frmEmailBlaster.aspx"
                                                    runat="server">Email Blaster</asp:HyperLink><br />
                                                This is a feature to create personalized emails to all Agents or merchants.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkChainIDs" NavigateUrl="~/SecureReports/frmMerchantChainIDManagement.aspx"
                                                    runat="server">Chain ID Management</asp:HyperLink><br />
                                                This is a feature to manage Merchant Chain IDs.
                                            </div>
                                             <div class="reportbucket">
                                                <asp:HyperLink ID="HyperLink1" NavigateUrl="~/SecureReports/frmScheduleAFeesMaster.aspx"
                                                    runat="server">Schedule A Type</asp:HyperLink><br />
                                                This is a feature to manage Schedule A Fees Master.
                                            </div>
                                        </td>
                                        <td valign="top">
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportPortfolioVolume" NavigateUrl="#" runat="server" Enabled="false">Portfolio Volume</asp:HyperLink><br />
                                                Add Comments
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkSalesOfficeActivity" NavigateUrl="~/SecureReports/frmSalesOfficeActivityReport.aspx"
                                                    Enabled="true" runat="server">Sales Office Activity Report</asp:HyperLink><br />
                                                This report provides a summary of deals received, broken down by Agent.
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkHistoryPending" NavigateUrl="~/SecureReports/frmSalesPendHistoryReport.aspx"
                                                    Enabled="true" runat="server">Sales Office Pend History Report</asp:HyperLink><br />
                                                This report shows a list of applications that were previously pended.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkSalesOfficeSummary" NavigateUrl="~/SecureReports/frmSalesOfficeSummaryReport.aspx"
                                                    Enabled="true" runat="server">Sales Office Summary Report</asp:HyperLink><br />
                                                This report provides a summary of deals received, broken down by Sales Office.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkRMOverviewReport" NavigateUrl="~/SecureReports/frmRMOverviewReport.aspx"
                                                    runat="server">SS Overview Report</asp:HyperLink><br />
                                                Overview of Agents and Merchant Applications.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkRMReportsByPortfolio" NavigateUrl="~/SecureReports/frmRMReportsByPortfolio.aspx"
                                                    runat="server">SS Reports by Portfolio</asp:HyperLink><br />
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" Text="Merchant Support">
                        <Template>
                            <fieldset>
                                <table cellpadding="10">
                                    <tr>
                                        <td valign="top">
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportCashAdvance" NavigateUrl="~/SecureReports/frmCashAdvance.aspx"
                                                    Enabled="true" runat="server">Cash Advance</asp:HyperLink><br />
                                                This is a feature to set up and manage Paysafe Cash Advances.
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportDeploymentStatus" NavigateUrl="#" runat="server" Enabled="false">Deployment Status</asp:HyperLink><br />
                                                Track all shipments to merchants, with status, tracking numbers, and a direct link
                                                to shippers.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportDowngradeSummary" NavigateUrl="~/SecureReports/frmDownGradeSummary.aspx"
                                                    runat="server">Downgrade Summary</asp:HyperLink><br />
                                                Add Comments
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkETF" NavigateUrl="~/SecureReports/frmETF.aspx" runat="server">ETF</asp:HyperLink><br />
                                                Add Comments
                                            </div>
                                            <div class="reportbucket" style="display: none;">
                                                <asp:HyperLink ID="lnkReportLockedUsers" NavigateUrl="#" runat="server" Enabled="false">Locked Users</asp:HyperLink><br />
                                                Add Comments
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkReportMerchantPCI" NavigateUrl="~/SecureReports/frmMerchantPCIReport.aspx"
                                                    Enabled="true" runat="server">Merchant PCI</asp:HyperLink><br />
                                                This is a feature to manage Merchant PCI compliance.
                                            </div>
                                        </td>
                                        <td valign="top"></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </Template>
                    </ig:ContentTabItem>
                    <ig:ContentTabItem runat="server" Text="Marketing" Hidden="false">
                        <Template>
                            <fieldset>
                                <table cellpadding="10">
                                    <tr>
                                        <td valign="top">
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkMeritusAlerts" NavigateUrl="~/SecureReports/frmMeritusAlerts.aspx"
                                                    runat="server">Paysafe Alerts</asp:HyperLink><br />
                                                This is a feature to create alert messages on each of the Paysafe portals.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkMeritusNews" NavigateUrl="~/SecureReports/frmMeritusNews.aspx"
                                                    runat="server">Paysafe News</asp:HyperLink><br />
                                                This is a feature to create news postings on each of the Paysafe portals.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkProducts" NavigateUrl="~/SecureReports/frmProducts.aspx"
                                                    runat="server">Product Management</asp:HyperLink><br />
                                                This is a feature to manage Paysafe Products.
                                            </div>
                                            <div class="reportbucket">
                                                <asp:HyperLink ID="lnkTemplates" NavigateUrl="~/SecureReports/frmTicketTemplates.aspx"
                                                    runat="server">Ticket Template Management</asp:HyperLink><br />
                                                This is a feature to manage Ticket Templates.
                                            </div>
                                        </td>
                                        <td valign="top"></td>
                                    </tr>
                                </table>
                            </fieldset>
                        </Template>
                    </ig:ContentTabItem>
                </Tabs>
            </ig:WebTab>
        </fieldset>
    </div>
</asp:Content>
