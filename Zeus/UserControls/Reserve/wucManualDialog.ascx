<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucManualDialog.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucManualDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlgManual" runat="server" Width="400px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" ID="pnlDetails" CssClass="dialog">
                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="VGManualDialog" CssClass="errorlist" runat="server" />
                <table style="table-layout:fixed;">
                    <tr>
                        <td style="width:100px;">Report Date
                        </td>
                        <td style="width:300px;">
                            <ig:WebDatePicker ID="ReportDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                            <asp:CustomValidator ID="cvDate" runat="server" ValidationGroup="VGManualDialog" Display="Dynamic" ControlToValidate="ReportDate" ErrorMessage="Date Required">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Amount
                        </td>
                        <td>
                            <ig:WebNumericEditor ID="Amount" CausesValidation="true" ValidationGroup="VGManualDialog" runat="server" ValueText="0" Width="80px" NullText="0.00" MinValue="0" Nullable="False"
                                DataMode="Decimal">
                            </ig:WebNumericEditor>
                            <asp:CustomValidator ID="cvAmount" runat="server" ValidationGroup="VGManualDialog" ControlToValidate="Amount" ErrorMessage="Amount must be greater than 0.00">*</asp:CustomValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Bank
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="BankID" Width="100px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvBankID" runat="server" ValidationGroup="VGManualDialog" ControlToValidate="BankID" ErrorMessage="Select Bank">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reserve Type
                        </td>
                        <td>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ReserveTypeID" Width="100px" OnSelectedIndexChanged="ReserveTypeID_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvReserveTypeID" runat="server" ValidationGroup="VGManualDialog"
                                ControlToValidate="ReserveTypeID" ErrorMessage="Select Reserve Type">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Transaction
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="TransactionType" Width="100px">
                                <asp:ListItem Value="0">Select Transaction Type</asp:ListItem>
                                <asp:ListItem Value="1">Debit</asp:ListItem>
                                <asp:ListItem Value="2">Credit</asp:ListItem>
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvTransactionType" runat="server" ValidationGroup="VGManualDialog"
                                ControlToValidate="TransactionType" ErrorMessage="Select Transaction Type">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Method
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="MethodID" Width="100px"  OnSelectedIndexChanged="MethodID_SelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvMethodID" runat="server" ValidationGroup="VGManualDialog" ControlToValidate="MethodID" ErrorMessage="Select Transaction Method">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Notes
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="Notes" Width="255px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="VGManualDialog" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Manual Reserve">
    </Header>
</ig:WebDialogWindow>
