<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReserveAmountDialog.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucReserveAmountDialog" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" 
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<ig:WebDialogWindow ID="dlgReserveAmount" runat="server"
    Width="250px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">

                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="VGReserveAmountDialog"
                    CssClass="errorlist" runat="server" />

                <table border="0" style="width:100%">

                    <tr>
                        <td>ZID:</td>
                        <td style="text-align: right; width: 60px;">
                            <asp:Label runat="server" ID="lblZID"></asp:Label>

                        </td>
                    </tr>
                      <tr>
                        <td>DBA:</td>
                        <td style="text-align: right; width: 60px;">
                            <asp:Label runat="server" ID="lblDBA"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Amount Withheld:</td>
                        <td style="text-align: right; width: 60px;">
                            <asp:Label runat="server" ID="lblAmountWithheld"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Reserve:</td>
                        <td style="text-align: right; width: 60px;">
                            <ig:WebNumericEditor OnTextChanged="wceReserveAmount_TextChanged" AutoPostBackFlags-ValueChanged="On" ID="wceReserveAmount" runat="server" BorderStyle="None" MinValue="0" Font-Names="Verdana" Font-Size="8pt" Width="60px" HorizontalAlign="Right"></ig:WebNumericEditor>

                            <asp:CustomValidator ID="cvReserveAmount" runat="server" Display="Dynamic" ValidationGroup="VGReserveAmountDialog" ErrorMessage="Invalid Amount">*</asp:CustomValidator>

                        </td>
                    </tr>
                    <tr>
                        <td>Divert:</td>
                        <td style="text-align: right; width: 60px;">
                            <asp:Label runat="server" ID="lblDivertAmount"></asp:Label>
                        </td>
                    </tr>


                </table>
                <br />
                <div style="text-align: center;">
                    <asp:Button runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click" />
                </div>

            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Reserve/Divert Re-allocation">
    </Header>
</ig:WebDialogWindow>
