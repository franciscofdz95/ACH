<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucTradeVerification" Codebehind="wucTradeVerification.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></legend>
    <table cellspacing="5">
        <tbody>
            <tr>
                <td class="lblRight">
                    Owner Name:</td>
                <td>
                    <asp:TextBox ID="FullName" runat="server" Text="" Width="125px" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
                <td class="lblRight">
                    Credit Score:</td>
                <td>
                    <asp:TextBox ID="CreditScore" runat="server" Width="125px" ReadOnly="true" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="lblRight">
                    # of Trades:</td>
                <td>
                    <asp:TextBox ID="NoofTrades" runat="server" Width="125px"></asp:TextBox>
                </td>
                <td class="lblRight">
                    Oldest Trade:</td>
                <td>
                    
                    <ig:WebDatePicker ID="OldestTrade" runat="server" NullText="" Width="130px" DisplayModeFormat="MM/dd/yyyy">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                            SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    Notes:<br />
                    <asp:TextBox runat="server" ID="TradeStatus" TextMode="MultiLine" Rows="4" Width="99%"></asp:TextBox>
                </td>
            </tr>
        </tbody>
    </table>
</fieldset>
<br />
