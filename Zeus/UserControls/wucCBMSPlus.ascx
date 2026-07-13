<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCBMSPlus.ascx.cs" Inherits="ZeusWeb.UserControls.wucCBMSPlus" %>
<fieldset>
    <legend>CBMS+</legend>
    <table>
        <tr>
            <td class="lblRight" style="width: 150px">Ethoca Member ID:
            </td>
            <td>
                <asp:TextBox ID="txtEthocaMemberId" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="lblRight" style="width: 150px">Verifi Member ID:
            </td>
            <td>
                <asp:TextBox ID="txtVerifiMemberId" runat="server" MaxLength="20"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Size="10pt" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</fieldset>