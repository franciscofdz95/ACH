<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReservePercentDialog.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucReservePercentDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<ig:WebDialogWindow ID="dlgReservePercent" runat="server"
    Width="250px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">



                <table border="0" width="100%">

                    <tr>
                        <td>ZID:</td>
                        <td style="text-align:right; width:60px;">
                            <asp:Label runat="server" ID="lblZID"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Report Date:</td>
                        <td style="text-align:right; width:60px;">
                            <asp:Label runat="server" ID="lblReportDate"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Current Reserve %:</td>
                        <td style="text-align:right; width:60px;">
                            <asp:Label runat="server" ID="lblCurrent"></asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>Reserve %:</td>
                        <td>
                            <ig:WebPercentEditor ID="wpeReservePercent" runat="server" BorderStyle="None" MinDecimalPlaces="2" MinValue="0"
                                Font-Names="Verdana" Font-Size="8pt" Width="60px" HorizontalAlign="Right">
                            </ig:WebPercentEditor>

                        </td>
                    </tr>
                    

                </table>
                <br />
                <div style="text-align: center;">
                    <asp:Button runat="server" ID="btnUpdate" Text="Override Reserve Percentage" OnClick="btnUpdate_Click" />
                </div>

            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Reserve Percentage Override">
    </Header>
</ig:WebDialogWindow>
