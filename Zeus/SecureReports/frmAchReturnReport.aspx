<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmAchReturnReport" Title="ACH Returns Reports" Codebehind="frmAchReturnReport.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucACHReturnGrid.ascx" TagName="wucACHReturnGrid"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
    <table width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>ACH Returns - Search Criteria</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <table>
                            <tr>
                                <td class="lblRight">
                                    From:</td>
                                <td>
                                    <ig:WebDatePicker ID="SearchBeginDate" runat="server" NullDateLabel="Select a date"
                                        AllowNull="False" Width="100px" EnableAppStyling="False">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td class="lblRight">
                                    To:</td>
                                <td>
                                    <ig:WebDatePicker ID="SearchEndDate" runat="server" NullDateLabel="Select a date"
                                        AllowNull="False" Width="100px" EnableAppStyling="False">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td class="lblRight">
                                    Trans ID:</td>
                                <td>
                                    <asp:TextBox ID="TransID" runat="server" Width="100px"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TransID"
                                        Display="None" ErrorMessage="Invalid TransID" ValidationExpression="^\d+$"></asp:RegularExpressionValidator></td>
                                <td class="lblRight">
                                    Account No:</td>
                                <td>
                                    <asp:TextBox ID="AccountNumber" runat="server" Width="100px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    Account Name:</td>
                                <td>
                                    <asp:TextBox ID="AccountName" runat="server" MaxLength="22" Width="100px"></asp:TextBox></td>
                                <td class="lblRight">
                                    Ref. #:</td>
                                <td class="fieldnote">
                                    <asp:TextBox ID="ReferenceNumber" runat="server" MaxLength="22" Width="100px"></asp:TextBox></td>
                                <td class="lblRight">
                                    Return Type:</td>
                                <td class="fieldnote">
                                    <asp:DropDownList ID="ReturnType" runat="server" Width="105px">
                                        <asp:ListItem Value="A">All</asp:ListItem>
                                        <asp:ListItem Value="R">Return</asp:ListItem>
                                        <asp:ListItem Value="C">Correction</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    Amount:</td>
                                <td>
                                    <asp:TextBox ID="Amount" runat="server" Width="100px"></asp:TextBox>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="Amount"
                                        Display="None" ErrorMessage="Invalid Amount" MaximumValue="1000000" MinimumValue="0"
                                        Type="Currency"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    Reason:</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ReasonCode" runat="server" Width="85%">
                                    </asp:DropDownList></td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="Reviewed" runat="server" Text="Reviewed?" TextAlign="Left" Visible="False" /></td>
                            </tr>
                            <tr>
                                <td align="center" colspan="8">
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />&nbsp;<asp:Button
                                        ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CausesValidation="False" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <br />
                </fieldset>
                <uc1:wucACHReturnGrid ID="grdACHSales" runat="server" />
                <br />
                <uc1:wucACHReturnGrid ID="grdACHCredit" runat="server" />
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
