<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmAllQueues"
    MasterPageFile="~/MasterPageReports.master" Codebehind="frmAllQueues.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div class="dialog">
                    <fieldset>
                        <legend>Hourly Call Summary Report (All Queues and Agents)</legend>
                        <br />
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <th class="lblRight">
                                    Agents:
                                </th>
                                <td>
                                    All Agents
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <th class="lblRight">
                                    Start Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="StartDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="80px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td>
                                    <asp:DropDownList ID="StartTime" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <th class="lblRight">
                                    Queues:
                                </th>
                                <td>
                                    <asp:DropDownList ID="Queues" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <th class="lblRight">
                                    End Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="EndDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="80px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td>
                                    <asp:DropDownList ID="EndTime" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="chkHour" Text="Half Hour" Checked="false" OnCheckedChanged="chkHour_CheckedChanged"
                                        AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <br />
                                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Panel ID="pnlgrd" runat="server">
                            <asp:GridView ID="grdQueue" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                CssClass="mGrid" OnRowDataBound="grdQueue_RowDataBound" Height="100%">
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <FooterStyle HorizontalAlign="Right" CssClass="footer" />
                                <Columns>
                                    <asp:BoundField HeaderText="Time" DataField="Time" ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="IncomingCalls" HeaderText="Incoming Calls" ItemStyle-HorizontalAlign="right" />
                                    <%--  <asp:BoundField DataField="OutgoingCalls" HeaderText="Outgoing Calls" />--%>
                                    <asp:BoundField DataField="AvgLenofInboundCalls" HeaderText="Average Length of Call (sec)"
                                        ItemStyle-HorizontalAlign="right" />
                                    <%--<asp:BoundField DataField="AvgLenofOutboundCalls" HeaderText="Average Length of Call" />--%>
                                    <asp:BoundField DataField="AvgInboundHoldTime" HeaderText="Average Hold Time (sec)"
                                        ItemStyle-HorizontalAlign="right" />
                                    <%-- <asp:BoundField DataField="AvgOutboundHoldTime" HeaderText="Average Hold Time" />--%>
                                    <asp:BoundField DataField="ActualInboundTalkTime" HeaderText="Total Call Time (sec)"
                                        ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="AbandonedCalls" HeaderText="Abandoned Calls" ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="AvgHoldTimeBeforeAbandonement" HeaderText="Avg Hold Time Before Abandonement (sec)"
                                        ItemStyle-HorizontalAlign="right" />
                                    <%-- <asp:BoundField DataField="ActualOutboundTalkTime" HeaderText="Actual Talk Time (Mins)" />
                   <asp:BoundField DataField="AbandonedCalls" HeaderText="Abandoned Calls" />
                    <asp:BoundField DataField="AvgHoldTimebeforeAbandonement" HeaderText="Average Hold Time before Abandonement" />--%>
                                    <asp:BoundField DataField="CallsfromMerchant" HeaderText="Merchant Queue Calls" ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="CallsfromAgent" HeaderText="Agent Queue Calls" ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="DCACalls" HeaderText="DCA Queue Calls" ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="DCABillingCalls" HeaderText="DCA Billing Queue Calls"
                                        ItemStyle-HorizontalAlign="right" />
                                    <asp:BoundField DataField="DCACancelCalls" HeaderText="DCA Cancel Queue Calls" ItemStyle-HorizontalAlign="right" />
                                </Columns>
                            </asp:GridView>
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                        </td>
                                        <%--                        <td align="right">
                            Export:&nbsp;</td>
                        <td align="left">
                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                            </asp:RadioButtonList></td>
                        <td align="right" style="width: 33%;">
                        </td>--%>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
