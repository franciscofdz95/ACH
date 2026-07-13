<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/MasterPageReports.master" Inherits="frmSalesOfficeActivityReport"
    Title="Sales Office Activity" Codebehind="frmSalesOfficeActivityReport.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:BulletedList runat="server" ID="blError" CssClass="errorlist">
    </asp:BulletedList>
    <div>
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div class="title">
                &nbsp;&nbsp;Sales Office Activity Report
                <hr class="line" />
            </div>
            <asp:ValidationSummary runat="server" ID="ValidSummary" ShowSummary="true" DisplayMode="BulletList"
                Visible="true" />
            <table>
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" Text="Begin Date:" ID="lblText" Width="120px"></asp:Label>
                    </td>
                    <td align="left">
                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" BackColor="#EFF3FF" BorderStyle="Solid"
                            BorderWidth="1px" Width="150px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        SS Rep:
                    </td>
                    <td>
                        <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="vertical" IDWidth="100px"
                                DBAWidth="145px" lblDBAWidth="120px" lblIDWidth="120px" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Include Sub-Agents:
                    </td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkSubAgent" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Agent Category:
                    </td>
                    <td align="left">
                        <asp:CheckBoxList runat="server" ID="AgentCategoryUID" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Agent Channel:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="PartnerChannel" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td align="left">
                        <div>
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
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Label runat="server" ID="lblData" Text="No Data" Visible="false"></asp:Label>
        <asp:Panel runat="server" ID="pnl1" Width="99%" ScrollBars="Horizontal">
            <table width="100%">
                <tr>
                    <td class="lblLeft">
                        Page Size:
                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                            <asp:ListItem Selected="True">10</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                            <asp:ListItem>250</asp:ListItem>
                            <asp:ListItem>500</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:GridView runat="server" ID="grdSalesActivity" AutoGenerateColumns="true" CssClass="mGrid"
                            OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                            OnRowDataBound="grdSalesActivity_RowDataBound" AllowSorting="true">
                            <RowStyle VerticalAlign="Top" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <FooterStyle CssClass="footer" HorizontalAlign="right" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                        </asp:GridView>
                        <br />
                        <div style="width: 100%">
                            <div class="buckethdrleft">
                                <asp:Panel runat="server" ID="Panel1">
                                    <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                        <span style="height: 25px; vertical-align: middle;">
                                            <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                Excel</span></asp:LinkButton>
                                </asp:Panel>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
</asp:Content>
