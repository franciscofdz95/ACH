<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCRMSubscription.ascx.cs" Inherits="ZeusWeb.UserControls.CRM.wucCRMSubscription" %>
<style type="text/css">
    .rdbbusiness .lblRight {
        font-weight: bold;
        white-space: nowrap;
    }

    .rdbbusiness table {
        width: 100%;
    }

    .rdbbusiness td {
        vertical-align: top;
    }
</style>
<fieldset>
    <legend>Merchant Info</legend>
    <asp:Panel ID="pnlMerchantInfo" CssClass="rdbbusiness" runat="server">
        <table>
            <tr>
                <td class="lblRight">CRM Identifier:
                </td>
                <td>
                    <asp:Label ID="CRMID" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Merchant Identifier (NMI Logon):
                </td>
                <td>
                    <asp:Label ID="NMILogon" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Merchant Name (DBA or MLE):
                </td>
                <td>
                    <asp:Label ID="MerchantName" runat="server">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Merchant Website URL:
                </td>
                <td>
                    <asp:Label ID="MerchantWebsiteUrl" runat="server">
                    </asp:Label>
                </td>
                <td class="lblRight">Customer Service Phone Number:
                </td>
                <td>
                    <asp:Label ID="CustomerServicePhone" runat="server"></asp:Label>
                </td>
                 <%--PXP-12164 by Sanidhya Kumar--%>
                <td class="lblRight">Billing Descriptor:
                </td>
                <td>
                    <asp:Label ID="BillingDescriptor" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Cancellation URL:
                </td>
                <td>
                    <asp:Label ID="CancelSubscriptionURL" runat="server"></asp:Label>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
</fieldset>
<fieldset>
    <legend>Subscription Info</legend>
    <asp:Panel ID="pnlSubscriptionInfo" CssClass="rdbbusiness" runat="server">
        <table>
            <tr>
                <td class="lblRight">Subscription Plan ID:
                </td>
                <td>
                    <asp:Label ID="SubscriptionPlanID" runat="server"></asp:Label>


                </td>
                <td class="lblRight">Trial Amount:
                </td>
                <td>
                    <asp:Label ID="TrialAmount" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Subscription Amount:
                </td>
                <td>
                    <asp:Label ID="SubscriptionAmount" runat="server">
                    </asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Token Value:
                </td>
                <td>
                    <asp:Label ID="TokenValue" runat="server">
                    </asp:Label>
                </td>
                <td class="lblRight">Product Description:
                </td>
                <td>
                    <asp:Label ID="ProductDescription" runat="server"></asp:Label>
                </td>

                <td class="lblRight">Start Trial Date:
                </td>
                <td>
                    <asp:Label ID="StartTrialDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lblRight">End Trial Date:
                </td>
                <td>
                    <asp:Label ID="EndTrialDate" runat="server">
                    </asp:Label>
                </td>

                <td class="lblRight">Start Subscription Date:
                </td>
                <td>
                    <asp:Label ID="StartSubscriptionDate" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Subscription duration (number of rebills):</td>
                <td>
                    <asp:Label ID="SubscriptionDuration" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="lblRight">Subscription Frequency: </td>
                <td>
                    <asp:Label ID="SubscriptionFrequency" runat="server"></asp:Label>
                </td>

                <td class="lblRight">Proposed Primary rebill day of month:
                </td>
                <td>
                    <asp:Label ID="ProposedPrimaryRebillDate" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Proposed Secondary rebill day of month if DECLINE:
                </td>
                <td>
                    <asp:Label ID="ProposedSecondaryRebillDate" runat="server">
                    </asp:Label>
                </td>

            </tr>
            <tr>
                <td class="lblRight">NMI Transaction ID (Trial): </td>
                <td>
                    <asp:Label ID="NMITransactionIDTrial" runat="server"></asp:Label>
                </td>
                <td class="lblRight">Order ID (Trial):</td>
                <td>
                    <asp:Label ID="OrderIDTrial" runat="server"></asp:Label>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </asp:Panel>
</fieldset>
