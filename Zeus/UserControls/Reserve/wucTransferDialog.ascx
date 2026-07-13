<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucTransferDialog.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucTransferDialog" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<ig:WebDialogWindow ID="dlg" runat="server" Width="300px" Modal="True"
    InitialLocation="Centered" WindowState="Hidden">
    <ContentPane>
        <Template>
        <asp:ValidationSummary ID="ValidationSummary1"  ValidationGroup="TransferDialog"  CssClass="errorlist" runat="server" />
            <asp:Panel runat="server" CssClass="dialog" ID="pnlDetails">
                <asp:GridView ID="gvBankBalance" runat="server" CssClass="mGrid" OnRowDataBound="gvBankBalance_RowDataBound" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                        <asp:BoundField DataField="ReserveType" HeaderText="Reserve Type" />
                        <asp:BoundField DataField="Balance" DataFormatString="{0:C2}" ItemStyle-HorizontalAlign="Right"
                            HeaderText="Balance" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="errorlist">No reserve balance</div>
                    </EmptyDataTemplate>
                </asp:GridView>
                <table>
                      <%-- <tr>
                        <td>
                            Posted Date:
                        </td>
                        <td style="text-align:right;">
                           <ig:WebDatePicker ID="PostedDate" runat="server" EnableAppStyling="False" HorizontalAlign="Right" NullDateLabel=""
                                DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                            <asp:CustomValidator ID="cvDate" runat="server" ValidationGroup="TransferDialog"
                                Display="Dynamic" ControlToValidate="PostedDate" ErrorMessage="Date Required">*</asp:CustomValidator>
                            <asp:Label runat="server" ID="ReportDate"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr>
                        
                        <td>
                            Divert Amount:
                        </td>
                        <td style="text-align:right;">
                            <asp:Label runat="server" ID="lblDivertAmount">0.00</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reserve Amount:
                        </td>
                        <td style="text-align:right;">
                          <asp:Label runat="server" ID="lblReserveAmount">0.00</asp:Label>

                        </td>
                    </tr>
                    <tr>
                        <td>
                            Transfer Amount:
                        </td>
                        <td style="text-align:right;">
                              <ig:WebNumericEditor ID="TransferAmount" CausesValidation="true"  ValidationGroup="TransferDialog"  runat="server" ValueText="0" Width="80px" NullText="0.00" MinValue="0" Nullable="False"
                                DataMode="Decimal">
                            </ig:WebNumericEditor>

                        <asp:CustomValidator ID="cvAmount" runat="server" Display="Dynamic"  ValidationGroup="TransferDialog"  ErrorMessage="Transfer Amount must not exceed Divert Amount">*</asp:CustomValidator>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Bank:
                        </td>
                        <td style="text-align:right;">
                              <asp:DropDownList  ValidationGroup="TransferDialog"  runat="server" ID="BankID"></asp:DropDownList>
                                    <asp:CustomValidator ID="cbBank" runat="server" ValidationGroup="TransferDialog"  Display="Dynamic" ControlToValidate="BankID" ErrorMessage="Bank Required">*</asp:CustomValidator>
                  
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" CausesValidation="true"  ValidationGroup="TransferDialog"  Text="Transfer" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Close" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="Transfer Funds From Divert to Reserve">
    </Header>
</ig:WebDialogWindow>
