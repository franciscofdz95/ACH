<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucReleaseDialog.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucReleaseDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlgRelease" runat="server" Width="350px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">
                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="VGReleaseDialog"
                    CssClass="errorlist" runat="server" />
                <asp:Label runat="server" ID="lblApproval"></asp:Label>
                <asp:GridView ID="gvBankBalance" runat="server" CssClass="mGrid" OnRowDataBound="gvBankBalance_RowDataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                        <asp:BoundField DataField="ReserveType" HeaderText="Reserve Type" />
                        <asp:BoundField DataField="Balance" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Balance" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="errorlist">No reserve balance</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <table>
                    <tr>
                        <td>Request Date:
                        </td>
                        <td>
                            <%--  <ig:WebDatePicker ID="ReportDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                            <asp:CustomValidator ID="cvDate" runat="server" ValidationGroup="VGReleaseDialog"
                                Display="Dynamic" ControlToValidate="ReportDate" ErrorMessage="Date Required">*</asp:CustomValidator>--%>
                            <asp:Label runat="server" ID="ReportDate"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Amount:
                        </td>
                        <td>
                            <ig:WebNumericEditor ID="Amount" MaxDecimalPlaces="2" MinDecimalPlaces="2" CausesValidation="true"
                                MinValue="0" MaxValue="99999999" ValidationGroup="VGReleaseDialog" runat="server"
                                ValueText="0" Width="95px" NullText="0.00" DataMode="Decimal" Nullable="False">
                            </ig:WebNumericEditor>
                            <asp:CustomValidator ID="cvAmount" runat="server" ValidationGroup="VGReleaseDialog"
                                ErrorMessage="Amount must be greater than 0.00">*</asp:CustomValidator>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>Release Type:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="TransTypeID" Width="100px" OnSelectedIndexChanged="TransactionTypeID_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvReleaseTypeID" runat="server" ValidationGroup="VGReleaseDialog"
                                ControlToValidate="TransTypeID" ErrorMessage="Select Release Type">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Reserve Type:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" AutoPostBack="true" ID="ReserveTypeID" Width="100px" OnSelectedIndexChanged="ReserveTypeID_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvReserveTypeID" runat="server" ValidationGroup="VGReleaseDialog"
                                ControlToValidate="ReserveTypeID" ErrorMessage="Select Reserve Type">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Method:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="MethodID" Width="100px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvTransactionMethod" runat="server" ValidationGroup="VGReleaseDialog"
                                ControlToValidate="MethodID" ErrorMessage="Select Method">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Bank:
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="BankID" Width="100px">
                            </asp:DropDownList>
                            <asp:CustomValidator ID="cvBankID" runat="server" ValidationGroup="VGReleaseDialog"
                                ControlToValidate="BankID" ErrorMessage="Select Bank">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Bank Notes:
                        </td>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="BankNotes" Width="200px" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvBankNotes" runat="server" ControlToValidate="BankNotes"
                                ValidationGroup="VGReleaseDialog" ErrorMessage="Bank notes required">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>Internal Notes:
                        </td>
                        <td>
                            <asp:TextBox TextMode="MultiLine" ID="InternalNotes" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlButtons" Style="text-align: center;">
                <asp:Button ID="btnSave" runat="server" CausesValidation="true" ValidationGroup="VGReleaseDialog"
                    Text="Save" OnClick="btnSave_Click" />
                <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnClose_Click" />
            </asp:Panel>
            <br />



        </Template>
    </ContentPane>
    <Header CaptionText="Reserve Release">
    </Header>
</ig:WebDialogWindow>

