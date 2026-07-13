<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCompass.ascx.cs" Inherits="ZeusWeb.UserControls.wucCompass" %>

<fieldset>
    <legend>Compass Parameters</legend>
    <table>
        <tr>
            <td class="lblRight" style="width: 150px">Division Number:
            </td>
            <td>
                <asp:TextBox ID="txtDivisionNumber" runat="server" MaxLength="10"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td><asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Size="10pt" ForeColor="Red"></asp:Label></td>
        </tr>
    </table>
</fieldset>
