<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageSales.master"
    Inherits="frmSalesSummaryReport" Title="Inside Sales Summary Report" CodeBehind="frmSalesSummaryReport.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPageSales.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <fieldset>
                <legend>Inside Sales Summary Report</legend>
                <asp:Panel runat="server" ID="pnlSearch">
                    <table>
                        <tr>
                            <td class="lblRight">
                                Begin Date:
                            </td>
                            <td align="left">                                
                                <ig:WebDatePicker ID="SearchBeginDate" runat="server" Width="150px" BackColor="#EFF3FF"
                                    BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                End Date:
                            </td>
                            <td align="left">
                                <ig:WebDatePicker ID="SearchEndDate" runat="server" Width="150px" BackColor="#EFF3FF"
                                    BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td class="lblRight">
                                Period:</td>
                            <td>
                                <asp:DropDownList ID="Date" runat="server" Width="150px">
                                </asp:DropDownList>
                            </td>
                        </tr>--%>
                        <tr>
                            <%--<td class="lblRight">
                                Agent:</td>
                            <td align="left">
                                <asp:DropDownList ID="AgentAgentID" runat="server" Width="300px">
                                </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server"
                                    TargetControlID="AgentAgentID" PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt"
                                    PromptPosition="Top" IsSorted="true" QueryPattern="Contains">
                                </cc1:ListSearchExtender>
                            </td>--%>
                            <td colspan="2">
                                <uc3:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="1" IDWidth="150"
                                    DBAWidth="150" lblDBAWidth="74" lblIDWidth="74" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <br />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                    ValidationGroup="SearchLead"></asp:Button>
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false"
                                    ValidationGroup="SearchLead"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel runat="server" ID="pnlGrd" Width="99%">
                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                        ShowFooter="true" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                        AllowSorting="true" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grd_RowDataBound"
                        FooterStyle-CssClass="footer" OnSorting="grd_Sorting">
                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                        <Columns>
                            <asp:BoundField HeaderText="Sales Representative" DataField="Name" SortExpression="Name">
                                <ItemStyle Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AverageCallsPerDay" HeaderText="Average Calls Per Day"
                                DataFormatString="{0:N1}" SortExpression="AverageCallsPerDay">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AverageCallsPerHour" HeaderText=" Average Calls Per Hour"
                                DataFormatString="{0:N1}" SortExpression=" AverageCallsPerHour">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="TotalCalls" HeaderText="Total Calls for Period" SortExpression="TotalCalls">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Avg#ofCallsperStatementReceived" HeaderText="Avg # of Calls per Statement Received"
                                DataFormatString="{0:N1}" SortExpression="Avg#ofCallsperStatementReceived">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AVG#ofCallsperApplicationSubmitted" HeaderText="AVG # of Calls per Application Submitted"
                                DataFormatString="{0:N1}" SortExpression="AVG#ofCallsperApplicationSubmitted">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="AVG#ofCallsperApplicationApproved" HeaderText="AVG # of Calls per Application Approved"
                                DataFormatString="{0:N1}" SortExpression="AVG#ofCallsperApplicationApproved">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="StatementsReceived" HeaderText="Statements Received" SortExpression="StatementsReceived">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApplicationsSubmitted" HeaderText="Apps Submitted" SortExpression="ApplicationsSubmitted">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApplicationsApproved" HeaderText="Apps Approved" SortExpression="ApplicationsApproved">
                                <ItemStyle Width="30px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ApprovedVolume" HeaderText="Approved Volume" SortExpression="ApprovedVolume"
                                DataFormatString="{0:0.00}">
                                <ItemStyle Width="40px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="VolumeProcessed" HeaderText="Volume Processed" SortExpression="VolumeProcessed"
                                DataFormatString="{0:0.00}">
                                <ItemStyle Width="40px" HorizontalAlign="right" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MonthlyProfit" HeaderText="Monthly Profit" SortExpression="MonthlyProfit"
                                DataFormatString="{0:0.00}">
                                <ItemStyle Width="40px" HorizontalAlign="right" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                    <asp:Panel runat="server" ID="pnlBucketFooter" Visible="false" CssClass="bucketfooter">
                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click" CommandArgument="grd"><span style="height: 25px; vertical-align: middle;"><asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span></asp:LinkButton>
                    </asp:Panel>
                </asp:Panel>
             
            </fieldset>
        </div>
    </div>
</asp:Content>
