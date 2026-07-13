<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucReserveDialog.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucReserveDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlgReserve" runat="server" Width="350px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">
                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="VGReserveDialog"
                    CssClass="errorlist" runat="server" />
                <asp:CustomValidator ID="cvAmount" runat="server" ValidationGroup="VGReserveDialog"
                    ErrorMessage="Reserve + Divert must equal Amount Withheld"></asp:CustomValidator>
                <fieldset>
                    <legend>Amounts</legend>
                    <table>
                        <tr>
                            <td>
                                Amount Withheld:
                            </td>
                            <td>
                                <div style="width: 82px; text-align: right;">
                                    <asp:Label runat="server" ID="Amount"></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table>
                    <tr>
                        <td>
                            Reserve:
                        </td>
                        <td>
                            <ig:WebNumericEditor ID="Reserve" MaxDecimalPlaces="2" MinDecimalPlaces="2" CausesValidation="true"
                                ValidationGroup="VGReserveDialog" runat="server" ValueText="0" Width="80px" NullText="0.00"
                                DataMode="Decimal">
                            </ig:WebNumericEditor>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Divert:
                        </td>
                        <td>
                            <ig:WebNumericEditor ID="Divert" MaxDecimalPlaces="2" MinDecimalPlaces="2" CausesValidation="true"
                                ValidationGroup="VGReserveDialog" runat="server" ValueText="0" Width="80px" NullText="0.00"
                                DataMode="Decimal">
                            </ig:WebNumericEditor>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="VGReserveDialog"
                                Text="Save" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                </table>
                <asp:GridView runat="server" OnRowDataBound="GridView1_RowDataBound" ID="GridView1"
                    AutoGenerateColumns="False" ShowFooter="True" CssClass="mGrid">
                    <Columns>
                        <asp:TemplateField HeaderText="Report Date">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                Total:
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BatchNumber" HeaderText="Batch Number" />
                        <asp:BoundField DataField="Card Type" HeaderText="Card Type" />
                        <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Amount", "{0:C2}") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblFootAmountTotal" runat="server"></asp:Label>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Reserve Allocations">
    </Header>
</ig:WebDialogWindow>
