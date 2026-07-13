<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucCreateUser" CodeBehind="wucCreateUser.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <contenttemplate>
        <fieldset>
            <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="Navy"></asp:Label>
            <br />
            <table cellpadding="2" cellspacing="2" align="center">
                <tr>
                    <td align="right">DBA:</td>
                    <td>
                        <asp:Label ID="lblDBA" runat="server" Text="lblUserName"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">User Name:</td>
                    <td>
                        <asp:Label ID="lblUserName" runat="server" Text="lblUserName"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">Email:</td>
                    <td>
                        <asp:Label ID="lblEmail" runat="server"></asp:Label></td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblKey" runat="server">Gateway Key:</asp:Label></td>
                    <td>
                        <asp:Label ID="GatewayKey" runat="server"></asp:Label></td>
                </tr>
               <%--code changes for PXP-12673 by koshlendra start--%>
                 <tr>
                    <td align="right">
                        <asp:Label ID="lblAltKey" runat="server" Visible="false">Alt. Gateway Key:</asp:Label></td>
                    <td>
                        <asp:Label ID="AlternativeGatewayKey" runat="server" Visible="false"></asp:Label></td>
                </tr>
                <%--code changes for PXP-12673 by koshlendra end--%>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblHostedPaymentKey" runat="server" Visible="false">Hosted Payment Key:</asp:Label></td>
                    <td>
                        <asp:Label ID="HostedPaymentPageKey" runat="server" Visible="false"></asp:Label></td>
                </tr>
                <tr id="trPin" runat="server">
                    <td align="right">Mobile Password:</td>
                    <td>
                        <asp:Label ID="tbMerchantPin" runat="server"></asp:Label></td>
                </tr>
                   <tr id="trLockdownKey" runat="server" Visible="false">
                    <td align="right">
                        <asp:Label ID="lblLockdownKey" runat="server">NMI Lockdown API Key:</asp:Label></td>
                    <td>
                        <asp:Label ID="NMILockdownAPIKey" runat="server"></asp:Label></td>
                </tr>
            </table>
                        <br />
            <table align="center">
                <tr>
                    <td>
                        <asp:Button ID="btnCreateAccount" OnClick="btnCreateAccount_Click" runat="server" CausesValidation="False"></asp:Button>
                    </td>
                    <td>
                        <asp:Button ID="btnResetPassword" OnClick="btnResetPassword_Click" runat="server"
                            Text="Reset Password" CausesValidation="False"></asp:Button>
                    </td>
                </tr>
            </table>

            <br />
            <asp:Panel ID="pnlLogin" runat="server" Height="" Width="">
                <table border="1" cellpadding="0" cellspacing="0" align="center">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkActive" runat="server" AutoPostBack="True" OnCheckedChanged="chkActive_CheckedChanged"
                                Text="Active" /></td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkMerchantWebsite" runat="server" Text="Merchant Website Access"
                                AutoPostBack="True" OnCheckedChanged="chkMerchantWebsite_CheckedChanged" /></td>
                        <td>
                            <asp:Button ID="btnWelcomeEmailMerchantWebsite" OnClick="btnWelcomeEmailMerchantWebsite_Click"
                                runat="server" Text="Email Merchant Website" Width="250px" CausesValidation="False">
                            </asp:Button></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkAgentWebsite" runat="server" Text="Partner Website Access" AutoPostBack="True"
                                OnCheckedChanged="chkAgentWebsite_CheckedChanged" /></td>
                        <td>
                            <asp:Button ID="btnWelcomeEmailAgentWebsite" OnClick="btnWelcomeEmailAgentWebsite_Click"
                                runat="server" Text="Email Partner Website" Width="250px" CausesValidation="False">
                            </asp:Button></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkPaymentXP" runat="server" AutoPostBack="True" Text="PaymentXP Access"
                                OnCheckedChanged="chkPaymentXP_CheckedChanged" /></td>
                        <td>
                            <asp:Button ID="btnWelcomeEmailPaymentXP" runat="server" CausesValidation="False"
                                OnClick="btnWelcomeEmailPaymentXP_Click" Text="Email PaymentXP" Width="250px" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkHostedPaymentPage" runat="server" AutoPostBack="True" Text="Hosted Payment Page Access"
                                OnCheckedChanged="chkHostedPaymentPage_CheckedChanged" /></td>
                        <td>
                            <asp:Button ID="btnWelcomeEmailHostedPaymentPage" runat="server" CausesValidation="False"
                                OnClick="btnWelcomeEmailHostedPaymentPage_Click" Text="Email Hosted Payment Page" Width="250px" /></td>
                    </tr>
                    <%--code changes for PXP-12673 by koshlendra start --%>
                     <tr>
                        <td>
                            <asp:CheckBox ID="chkAlternativeGatewayKey" runat="server" AutoPostBack="True" Text="Alternative Gateway Key Access"
                                OnCheckedChanged="chkAlternativeGatewayKey_CheckedChanged" /></td>
                      
                    </tr> 
                    <%--code changes for PXP-12673 by koshlendra End  --%>                                  
                     <tr>
                        <td>
                            <asp:CheckBox ID="chkMobilePaymentPage" runat="server" AutoPostBack="True" Text="Mobile Payment Page Access" 
                                OnCheckedChanged="chMobilePaymentPage_CheckedChanged"   /></td>                                                    
                    </tr>
                    <%-- DM-4897 alamadrid --%>                                  
                     <tr>
                        <td>
                            <asp:CheckBox ID="chkIsMFAEnabled" runat="server" AutoPostBack="True" Text="MFA Enabled" 
                               Enabled="false"    /></td>                                                    
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
    </contenttemplate>
</asp:UpdatePanel>
