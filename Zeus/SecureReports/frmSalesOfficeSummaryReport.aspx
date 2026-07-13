<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmSalesOfficeSummaryReport"
    Title="Sales Office Summary" CodeBehind="frmSalesOfficeSummaryReport.aspx.cs" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc3" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <asp:BulletedList runat="server" ID="blError" CssClass="errorlist">
    </asp:BulletedList>
    <asp:Panel runat="server" CssClass="SOSR" ID="pnlSearch">
        <fieldset>
            <legend>Sales Office Summary Report</legend>
            <table>
                <tr>
                    <td class="lblRight">Time Frame:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTimeFrame" OnSelectedIndexChanged="ddlTimeFrame_SelectedIndexChanged" Width="130px">
                            <asp:ListItem Value="-1">Select Time Frame</asp:ListItem>
                            <asp:ListItem Value="1">Last 30 days</asp:ListItem>
                            <asp:ListItem Value="2">Last 60 days</asp:ListItem>
                            <asp:ListItem Value="3">Last 90 days</asp:ListItem>
                            <asp:ListItem Value="4">Last Month</asp:ListItem>
                            <asp:ListItem Value="5">Month to Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Begin Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="DateStart" runat="server" EnableAppStyling="False" NullDateLabel=""
                            Width="130px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">SS Rep:
                    </td>
                    <td>
                        <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="130px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">End Date:
                    </td>
                    <td>
                        <ig:WebDatePicker ID="DateEnd" runat="server" EnableAppStyling="False" NullDateLabel=""
                            Width="130px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                SlideOpenDuration="1" />
                        </ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc3:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="1" IDWidth="125"
                            DBAWidth="125" lblDBAWidth="102" lblIDWidth="102" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Include Sub-Agents:
                    </td>
                    <td align="left">
                        <asp:CheckBox ID="chkSubAgent" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Agent Category:
                    </td>
                    <td align="left">
                        <asp:CheckBoxList runat="server" ID="AgentCategoryUID" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Agent Channel:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="PartnerChannel" runat="server" Width="130px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight"></td>
                    <td align="left">
                        <div>
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
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <fieldset>
        <legend>Search Results</legend>
        <asp:Panel runat="server" ID="pnlRecords" Visible="false">
            <table>
                <tr>
                    <td class="lblLeft">Page Size:
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
                        <asp:GridView ID="GridView1" runat="server" CssClass="mGrid" OnPageIndexChanging="GridView1_PageIndexChanging"
                            OnRowDataBound="GridView1_RowDataBound" ShowFooter="True" AllowPaging="true"
                            AutoGenerateColumns="false">
                            <FooterStyle CssClass="footer gNumber" />
                            <PagerStyle CssClass="pgr" />
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                <asp:BoundField DataField="Office Name" HeaderText="Office Name" />
                                <asp:BoundField DataField="Received" HeaderText="Received" ItemStyle-CssClass="gNumber"
                                    SortExpression="Received" />
                                <asp:BoundField DataField="Approved" HeaderText="Approved" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Declined" HeaderText="Declined" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Withdrawn" HeaderText="Withdrawn" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Closed" HeaderText="Closed" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Pended" HeaderText="Pended" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Approval %" HeaderText="Approval %" DataFormatString="{0:f2}%"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Withdrawn %" HeaderText="Withdrawn %" DataFormatString="{0:f2}%"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Application Pending %" HeaderText="Application Pending %"
                                    DataFormatString="{0:f2}%" ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Credit Pending %" HeaderText="Credit Pending %" DataFormatString="{0:f2}%"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Card Present %" HeaderText="Card Present %" DataFormatString="{0:f2}%"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="High Risk %" HeaderText="High Risk %" DataFormatString="{0:f2}%"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Portfolio Count" HeaderText="Portfolio Count" DataFormatString="{0:n0}"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Processing Volume" HeaderText="Processing Volume" DataFormatString="{0:0.00}"
                                    ItemStyle-CssClass="gNumber" />
                                <asp:BoundField DataField="Portfolio Capacity Processing %" HeaderText="Portfolio Capacity Processing %"
                                    DataFormatString="{0:f2}%" ItemStyle-CssClass="gNumber" />
                            </Columns>
                        </asp:GridView>
                        &nbsp;
            <hr />
                        <asp:LinkButton runat="server" ID="lbSentToExcel" OnClick="lbSentToExcel_Click">
                            <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" />Send To Excel
                        </asp:LinkButton>&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlNoRecords">
            No Records Found
        </asp:Panel>
    </fieldset>
</asp:Content>
