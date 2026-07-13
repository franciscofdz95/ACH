<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmRMReportsByPortfolio"
    Title="SS Reports by Portfolio" CodeBehind="frmRMReportsByPortfolio.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.DataSourceControls" TagPrefix="ig" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="title">
                    &nbsp;&nbsp;SS Reports by Portfolio
                    <hr class="line" />
                </div>
                <table>
                    <tr>
                        <td style="width: 17%; vertical-align: top;">
                            <table width="100%" class="tablecellborder">
                                <tr>
                                    <td>Begin Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>End Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td>ARM:
                                    </td>
                                    <td>
                                        <b>
                                            <asp:Label ID="lblArm" runat="server" Text=""></asp:Label></b>
                                    </td>
                                </tr>
                            </table>
                            <fieldset>
                                <legend>Prior Month</legend>
                                <table width="100%" class="tablecellborder">
                                    <tr>
                                        <td>Approved Cnt:
                                        </td>
                                        <td>
                                            <asp:Label ID="LastMonthCnt" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Approved Vol:
                                        </td>
                                        <td>
                                            <asp:Label ID="LastMonthVol" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>%Delta:
                                        </td>
                                        <td>
                                            <asp:Label ID="ApprovedVolDelta" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Actual Vol:
                                        </td>
                                        <td>
                                            <asp:Label ID="LastMonthActualVol" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>%Delta:
                                        </td>
                                        <td>
                                            <asp:Label ID="ActualVolDelta" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top">                            
                            <ig:WebTab ID="tabReport" runat="server" Width="1000px" OnSelectedIndexChanged="tabReport_SelectedIndexChanged">
                                <Tabs>
                                    <ig:ContentTabItem runat="server" Text="Summary">
                                        <Template>
                                            <fieldset>
                                                <asp:Label ID="lblNoDataSummary" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlSummary" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" DataKeyNames="UserUID" OnRowDataBound="grd_RowDataBound"
                                                        OnRowCommand="grd_RowCommand" SelectedRowStyle-BackColor="#fffacd">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                                        <Columns>
                                                            <asp:BoundField DataField="UserUID" HeaderText="UserUID" Visible="False" />
                                                            <asp:TemplateField HeaderText="Rep">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkUserName" runat="server"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="left" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="SS">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAP" runat="server">SS</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CU">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCU" runat="server">CU</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="OP">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkAB" runat="server">OP</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="DP">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkDP" runat="server">DP</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Tickets">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkTickets" runat="server">Tickets</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Top SPs">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkTopSP" runat="server">Top SP</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cancelled">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCancelled" runat="server">Cancelled</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportSummary" runat="server" OnClick="lnkExportSummary_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Details">
                                        <Template>
                                            <fieldset>
                                                <asp:Label ID="lblNoDataDetails" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlDetails" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCountDetails" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grdDetails" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="True" OnRowDataBound="grdDetails_RowDataBound"
                                                        OnRowCommand="grdDetails_RowCommand">
                                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ZID" SortExpression="ZID">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkZID" runat="server" CommandName="ZID"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="AID" HeaderText="AID" />
                                                            <%--<asp:BoundField DataField="DBA" HeaderText="DBA" />--%>
                                                            <asp:TemplateField HeaderText="DBA" SortExpression="BusinessDBAName">
                                                                <ItemTemplate>
                                                                    <asp:Label runat="server" Text='<%#Eval("DBA") %>' ID="lblDBA"></asp:Label>
                                                                    <asp:LinkButton runat="server" ID="lnkDBA" CommandName="DBAName"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="183px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Legal" HeaderText="Legal" />
                                                            <asp:BoundField DataField="Status" HeaderText="Status" />
                                                            <asp:BoundField DataField="APReceivedDate" HeaderText="SS Recd">
                                                                <ItemStyle Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CUReceivedDate" HeaderText="CU Recd">
                                                                <ItemStyle Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ABReceivedDate" HeaderText="OP Recd">
                                                                <ItemStyle Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DPReceivedDate" HeaderText="DP Recd">
                                                                <ItemStyle Width="75px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Agent ID" HeaderText="Agent ID" />
                                                            <asp:BoundField DataField="Agent Name" HeaderText="Agent Name" />
                                                            <asp:BoundField DataField="Bank" HeaderText="Bank" />
                                                            <asp:BoundField DataField="Approved Vol" HeaderText="Approved Vol" DataFormatString="{0:N0}">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:LinkButton ID="lnkExportDetails" runat="server" OnClick="lnkExportDetails_Click">
                                                                    <span style="height: 25px; vertical-align: middle;">
                                                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                                </asp:LinkButton></td>
                                                            <td align="right">
                                                                <span style="color: Tomato;">Red</span> = Missed SLA<br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </fieldset>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Tickets">
                                        <Template>
                                            <fieldset>
                                                <legend>Assigned</legend>
                                                <asp:Label ID="lblNoDataAssigned" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlAssigned" Height="" Visible="false">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Button runat="server" ID="btnRefresh" Text="Refresh" OnClick="btnRefresh_Click" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label ID="lblRecordCountAssigned" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <asp:GridView ID="grdAssigned" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="true" OnRowDataBound="grdTickets_RowDataBound">
                                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="100" FirstPageText="&laquo;"
                                                            LastPageText="&raquo;" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkTicketID" runat="server" CommandName="ID"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ZID" HeaderText="ZID" Visible="false" ItemStyle-CssClass="togle">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Category" HeaderText="Sub-Category" SortExpression="Category">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Origin" HeaderText="Origin" SortExpression="Origin">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Problem" HeaderText="Problem" Visible="false" ItemStyle-CssClass="togle">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateCreated" HeaderText="Date Opened" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                                                                SortExpression="DateCreated">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LastChanged" HeaderText="Date Udated" SortExpression="LastChanged"
                                                                DataFormatString="{0:MM-dd-yy HH:mm tt}">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Priority" HeaderText="Severity" SortExpression="Priority">
                                                                <ItemStyle Width="40px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportAssigned" runat="server" OnClick="lnkExportAssigned_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                            <fieldset>
                                                <legend>Assigned Out</legend>
                                                <asp:Label ID="lblNoDataAssignedOut" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlAssignedOut" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCountAssignedOut" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grdAssignedOut" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="true" OnRowDataBound="grdTickets_RowDataBound">
                                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="100" FirstPageText="&laquo;"
                                                            LastPageText="&raquo;" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkTicketID" runat="server" CommandName="ID"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="ZID" HeaderText="ZID" Visible="false" ItemStyle-CssClass="togle">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="AssignedTo" HeaderText="Assigned To" SortExpression="AssignedTo">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Category" HeaderText="Sub-Category" SortExpression="Category">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Origin" HeaderText="Origin" SortExpression="Origin">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Problem" HeaderText="Problem" Visible="false" ItemStyle-CssClass="togle">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DateCreated" HeaderText="Date Opened" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                                                                SortExpression="DateCreated">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LastChanged" HeaderText="Date Updated" SortExpression="LastChanged"
                                                                DataFormatString="{0:MM-dd-yy HH:mm tt}">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Priority" HeaderText="Severity" SortExpression="Priority">
                                                                <ItemStyle Width="40px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                                                <ItemStyle Width="80px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportAssignedOut" runat="server" OnClick="lnkExportAssignedOut_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image4" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Top SPs">
                                        <Template>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnApply2" runat="server" Text="Refresh" CausesValidation="false"
                                                            OnClick="btnApply2_Click" /></td>
                                                    <td>
                                                        <asp:RadioButtonList ID="lstSPOptions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lstSPOptions_OnSelectedIndexChanged"
                                                            RepeatColumns="2">
                                                            <asp:ListItem Selected="true">View Top 5</asp:ListItem>
                                                            <asp:ListItem>View All</asp:ListItem>
                                                        </asp:RadioButtonList></td>
                                                </tr>
                                            </table>
                                            <fieldset>
                                                <legend>Top Approval %</legend>
                                                <asp:Label ID="lblNoDataSPApprovals" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlSPApprovals" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCountTopApproval" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grdTopSPPct" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="false">
                                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="100" FirstPageText="&laquo;"
                                                            LastPageText="&raquo;" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SP" HeaderText="SP">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Received" HeaderText="Received" DataFormatString="{0:N0}"
                                                                ItemStyle-HorizontalAlign="right">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Approvals" HeaderText="Approvals" DataFormatString="{0:N0}"
                                                                ItemStyle-HorizontalAlign="right">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="ApprovalPct" HeaderText="Approval %" DataFormatString="{0:P0}"
                                                                ItemStyle-HorizontalAlign="right">
                                                                <ItemStyle Width="60px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportTopSPApprovals" runat="server" OnClick="lnkExportTopSPApprovals_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image5" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                            <fieldset>
                                                <legend>Top Volume</legend>
                                                <asp:Label ID="lblNoDataSPVolume" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlSPVolume" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCountTopVolume" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grdTopSPVol" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="false">
                                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="100" FirstPageText="&laquo;"
                                                            LastPageText="&raquo;" />
                                                        <Columns>
                                                            <asp:BoundField DataField="SP" HeaderText="SP">
                                                                <ItemStyle Width="150px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TransCount" HeaderText="Count" DataFormatString="{0:N0}"
                                                                ItemStyle-HorizontalAlign="right">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TransSales" HeaderText="Volume" DataFormatString="{0:0.00}"
                                                                ItemStyle-HorizontalAlign="right">
                                                                <ItemStyle Width="100px" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportTopSPVolume" runat="server" OnClick="lnkExportTopSPVolume_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image6" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                        </Template>
                                    </ig:ContentTabItem>
                                    <ig:ContentTabItem runat="server" Text="Cancelled">
                                        <Template>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnApply" runat="server" Text="Refresh" CausesValidation="false"
                                                            OnClick="btnApply_Click" /></td>
                                                </tr>
                                            </table>
                                            <fieldset>
                                                <asp:Label ID="lblNoDataCancelled" Text="No Data..." runat="server"></asp:Label>
                                                <asp:Panel runat="server" ID="pnlCancelled" Height="" Visible="false">
                                                    <div class="buckethdright">
                                                        <asp:Label ID="lblRecordCountCancellations" SkinID="RecordCount" runat="server" Text=""></asp:Label>
                                                    </div>
                                                    <br />
                                                    <asp:GridView ID="grdCancellations" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                        AlternatingRowStyle-CssClass="alt" PageSize="100" ShowFooter="true" OnRowDataBound="grdCancellations_RowDataBound"
                                                        OnRowCommand="grdCancellations_RowCommand">
                                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                                            LastPageText="&raquo;" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ZID" SortExpression="ZID">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkZID" runat="server" CommandName="ZID"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="40px" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="AID" HeaderText="AID" />
                                                            <asp:BoundField DataField="DBA" HeaderText="DBA" />
                                                            <asp:BoundField DataField="Legal" HeaderText="Legal" />
                                                            <asp:BoundField DataField="Agent ID" HeaderText="Agent ID" />
                                                            <asp:BoundField DataField="Agent Name" HeaderText="Agent Name>" />
                                                            <asp:BoundField DataField="Bank" HeaderText="Bank" />
                                                            <asp:BoundField DataField="Cancelled Date" HeaderText="Cancelled Date" />
                                                            <asp:BoundField DataField="Closure Reason" HeaderText="Closure Reason" />
                                                            <asp:BoundField DataField="ETF Status" HeaderText="ETF Status" />
                                                            <asp:BoundField DataField="Approved Vol" HeaderText="Approved Vol" ItemStyle-HorizontalAlign="right"
                                                                DataFormatString="{0:N0}" />
                                                        </Columns>
                                                    </asp:GridView>
                                                    <hr class="line" />
                                                    <asp:LinkButton ID="lnkExportCancellations" runat="server" OnClick="lnkExportCancellations_Click">
                                                        <span style="height: 25px; vertical-align: middle;">
                                                            <asp:Image ID="Image7" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">Save Excel</span>
                                                    </asp:LinkButton>
                                                </asp:Panel>
                                            </fieldset>
                                        </Template>
                                    </ig:ContentTabItem>
                                </Tabs>
                                <AutoPostBackFlags SelectedIndexChanged="On" />
                            </ig:WebTab>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
    </table>

    <%--    <script language="javascript" type="text/javascript">
    function newticket(ticketid)
    { 

        if(ticketid == '') 
        {
            window.open('../SecureTicketForms/frmTicketPopup.aspx?Adding=true&RequestOrigin=Agent', 'ticketwindow','width=900,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');
        } 
        else 
        {
            // assign a random number to the window ID so that it does not replace itself.
            window.open('../SecureTicketForms/frmTicketPopup.aspx?Adding=false&TicketUID='+ticketid, 'ticketwindow','width=900,height=650, toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no');    
        }

    }
    </script>
    --%>
</asp:Content>
