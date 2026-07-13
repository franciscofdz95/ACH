<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucTopMenu.ascx.cs"
    Inherits="wucTopMenu" %>
<%@ Register Src="wucDBInfo.ascx" TagName="wucDBInfo" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucUserProfile.ascx" TagName="wucUserProfile" TagPrefix="uc2" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
 <script type="text/javascript">
     $(document).ready(function () {


         //If user presses enter.
         $('#quickTicketSearch').on('keydown', function (e) {

             var key = e.which;
             if (key == 13) {
                 ShowTicket();
             }
         });
     });

     var requestRunning = false;
     function ShowTicket() {
         var ticketid = $('#quickTicketSearch').val();

         if (requestRunning || ticketid == '') { // don't do anything if an AJAX request is pending or ticketid is empty
             return;
         }
         $.ajax({
             type: "POST",
             url: "../ajax/GenericAjaxControl.aspx",
             data: { command: 'GetTicketUID', TicketID: ticketid },
             success: function (msg) {
                 if (msg.response == 'No Ticket UID') {
                     alert("Ticket ID:" + ticketid + " does not exist.");
                 }
                 else if (msg.response == 'InvalidTicketID')
                 {
                     alert("Ticket ID:" + ticketid + " is not a valid input.");
                 }
                 else {
                     OpenTicket(msg.response);
                 }
             },
             complete: function () {
                 requestRunning = false;
             }
         });
     }
 </script>
<uc1:wucDBInfo ID="wucDBInfo1" runat="server" />
<div id="header">
    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="menutabscontainer">
        <tr>
            <td style="text-align: left; color: #0a94d6; font-size: 15px; font-weight: bold; width: 125px;">
                <asp:Image runat="server" ID="imgBeta" Height="25px" ImageUrl="~/Images/beta_small.png" />
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/logo.png" Height="25px" />
            </td>
            <td>
                <div id="menutabs">
                    <asp:Panel runat="server" ID="pnlMenu">
                        <ul runat="server" id="ulMenu">
                            <li>
                                <asp:HyperLink ID="lnkHome" CssClass="enum_home" NavigateUrl="~/SecureHomeForms/frmHome.aspx"
                                    runat="server">Home</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkMerchants" CssClass="enum_merchants" NavigateUrl="~/SecureMerchantManagementForms/frmMerchantSearch.aspx"
                                    runat="server">Merchants</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkRiskTab" CssClass="enum_risk" NavigateUrl="~/SecureRiskForms/frmRiskMain.aspx"
                                    runat="server">Risk</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkReports" CssClass="enum_reports" NavigateUrl="~/SecureReports/frmReports.aspx"
                                    runat="server">Reports</asp:HyperLink></li>
                            <%--<li>
                                <asp:HyperLink ID="lnkForms" CssClass="enum_forms" NavigateUrl="~/SecureForms/frmForms.aspx"
                                    runat="server">Forms</asp:HyperLink></li>--%>
                            <li>
                                <asp:HyperLink ID="lnkSales" CssClass="enum_sales" NavigateUrl="~/SecureLeadForms/frmLeadQueues.aspx"
                                    runat="server">Sales</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkAgents" CssClass="enum_partners" NavigateUrl="~/SecureAgentManagementForms/frmSearchAgents.aspx"
                                    runat="server">Agents</asp:HyperLink></li>
                           <%-- <li>
                                <asp:HyperLink ID="lnkFirstTeam" CssClass="enum_firstteam" NavigateUrl="~/SecureFirstTeamForms/frmFTDashboard.aspx"
                                    runat="server">Premier Services</asp:HyperLink></li>
                            <li>--%>
                                <asp:HyperLink ID="lnkAdmin" CssClass="enum_admin" NavigateUrl="~/SecureAdminForms/frmAddUser.aspx"
                                    runat="server">Admin</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkTickets" CssClass="enum_tickets" NavigateUrl="~/SecureTicketForms/frmTicketSearch.aspx"
                                    runat="server">Tickets</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkAccounting" CssClass="enum_accounting" NavigateUrl="~/SecureReports/frmInvoiceReport.aspx" Visible="false"
                                    runat="server">Accounting</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkCompliance" CssClass="enum_Compliance" NavigateUrl="~/SecureComplianceForms/frmCRMVendorSetupSearch.aspx"
                                    runat="server">Compliance</asp:HyperLink></li>
                             <li>
                                <asp:HyperLink ID="lnkQuality" CssClass="enum_Quality" NavigateUrl="~/SecureQualityForms/frmQAAppErrorSearch.aspx"
                                    runat="server">Quality</asp:HyperLink></li>
                            <li>
                                <asp:HyperLink ID="lnkAgentAllocation" CssClass="enum_Allocations" NavigateUrl="~/SecureQualityForms/frmAgentAllocationSearch.aspx"
                                    runat="server">Allocations</asp:HyperLink></li>
                        </ul>
                    </asp:Panel>
                </div>
            </td>
            <td align="right" id="topright" class="topright" runat="server">
            <%--Added by Koshlendra for PXP-2206: In Zeus if two or more users are editing a ZID at the same time , display a notification message--%>     
                 <asp:Label ID="lblUserEditingNotice" runat="server" ></asp:Label>&nbsp;
             <%--******** End of PXP-2206 **************--%>
                Welcome <asp:Label ID="lblWelcome" runat="server"></asp:Label>&nbsp;
                <asp:HyperLink ID="lnkUsers" runat="server" NavigateUrl="~/SecureHomeForms/frmProfileSettings.aspx">Profile</asp:HyperLink>
                <asp:HyperLink ID="lnkLogOut" runat="server" NavigateUrl="~/frmLogout.aspx">Log Out</asp:HyperLink>
            </td>
        </tr>
    </table>
    <div class="clear">
    </div>
    <div class="activelabel">
         <asp:Label ID="lblZIDText" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblStatusBarText" runat="server"></asp:Label>
        <span style="float:right;height:12px"><input type="text" id="quickTicketSearch" placeholder="Search Ticket ID" maxlength="15" style="width:120px">&nbsp;<i class="glyphicon glyphicon-search" title="Search" style="cursor:default" onclick = ShowTicket()></i></span>
    </div>
    <ig:WebDialogWindow ID="WebDialogWindow4" runat="server" Height="350px" Width="500px"
        Modal="True" InitialLocation="Centered" WindowState="hidden">
        <ContentPane>
            <Template>
                <br />
                <uc2:wucUserProfile runat="server" id="wucUserProfile1" />
            </Template>
        </ContentPane>
        <Header CaptionText="User Profile Management">
        </Header>
    </ig:WebDialogWindow>
</div>
