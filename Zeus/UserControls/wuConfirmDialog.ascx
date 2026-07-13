<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wuConfirmDialog" Codebehind="wuConfirmDialog.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Panel runat="server" DefaultButton="Button1" ID="pnlConfirm">
    <table align="center">
        <tr>
            <td colspan="2">
                <div class="title">
                    <h6>
                        Confirm the values.</h6>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSSN" runat="server" Height="13px" Text="Owner1 SSN:" Visible="false"></asp:Label>
            </td>
            <td>
                <ig:WebMaskEditor ID="txtSSN" runat="server" InputMask="###-##-####" Visible="false">
                </ig:WebMaskEditor>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTaxID" runat="server" Text="Tax Reg#:" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTaxID" runat="server" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDDA" runat="server" Text="Account Number:" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDDA" runat="server" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMID" runat="server" Text="Front MID:" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMID" runat="server" Visible="false"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBMID" runat="server" Text="Back MID:" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBMID" runat="server" Visible="false"></asp:TextBox></td>
        </tr>
          <tr>
            <td>
                <asp:Label ID="lblRoute" runat="server" Text="Routing Number:" Visible="false"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRoute" runat="server" Visible="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" CausesValidation="False" />
            </td>
        </tr>
    </table>
</asp:Panel>
