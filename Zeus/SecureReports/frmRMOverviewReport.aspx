<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="SecureReports_frmRMOverviewReport"
    Title="SS Overview" Codebehind="frmRMOverviewReport.aspx.cs" %>

<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <uc1:wucMessage ID="WucMessage1" runat="server" />
    <asp:Panel runat="server" CssClass="SOSR" ID="pnlSearch">
        <fieldset>
            <legend>SS Overview</legend>
            <table>
                <tr>
                    <td class="lblRight">
                        Time Frame:</td>
                    <td>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTimeFrame" OnSelectedIndexChanged="ddlTimeFrame_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Select Time Frame</asp:ListItem>
                            <asp:ListItem Value="1">Last 30 days</asp:ListItem>
                            <asp:ListItem Value="2">Last 60 days</asp:ListItem>
                            <asp:ListItem Value="3">Last 90 days</asp:ListItem>
                            <asp:ListItem Value="4">Last Month</asp:ListItem>
                            <asp:ListItem Value="6">Last 6 Months</asp:ListItem>
                            <asp:ListItem Value="7">Last 12 Months</asp:ListItem>
                            <asp:ListItem Value="5">Month to Date</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Begin Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="DateStart" runat="server" EnableAppStyling="False" NullDateLabel=""
                            Width="130px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        End Date:</td>
                    <td>
                        <ig:WebDatePicker ID="DateEnd" runat="server" EnableAppStyling="False" NullDateLabel=""
                            Width="130px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Date Grouping:</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlDateGrouping">
                            <asp:ListItem Value="DateDaily">Daily</asp:ListItem>
                            <asp:ListItem Value="DateMonthly">Monthly</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td>
                        <asp:CheckBox runat="server" Text="Show Charts" ID="cbChart" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <br />
                        <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                            AccessKey="h">
                            <Appearance>
                                <Image Url="~/Images/Check.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                        &nbsp;
                        <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                            CausesValidation="False" AccessKey="l">
                            <Appearance>
                                <Image Url="~/Images/delete.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                        &nbsp;&nbsp;
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <fieldset>
        <legend>Search Results</legend>
        <asp:Panel runat="server" ID="pnl" Width="98%" ScrollBars="horizontal">
            <asp:GridView ID="GridView1" OnPreRender="GridView1_PreRender" OnRowDataBound="GridView1_RowDataBound"
                CssClass="mGrid" runat="server">
            </asp:GridView>
            <asp:LinkButton runat="server" ID="lbSentToExcel" OnClick="lbSentToExcel_Click">
                <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" />Send To Excel</asp:LinkButton>
        </asp:Panel>
    </fieldset>
</asp:Content>
