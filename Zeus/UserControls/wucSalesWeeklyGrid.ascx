<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucSalesWeekly" Codebehind="wucSalesWeeklyGrid.ascx.cs" %>
<asp:Panel runat="server" ID="pnl" Visible="false">
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" OnRowCreated="grd_RowCreated"
        ShowFooter="true" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
        AlternatingRowStyle-CssClass="alt" OnRowDataBound="grd_RowDataBound" FooterStyle-CssClass="footer">
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
        <Columns>
            <asp:BoundField HeaderText="Sales Rep" DataField="Name" SortExpression="Name">
                <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Ext" DataField="Ext" SortExpression="Ext">
                <ItemStyle Width="40px" />
            </asp:BoundField>
            <asp:BoundField DataField="Monday Calls" HeaderText="Mon" SortExpression="Monday Calls">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday Calls" HeaderText="Tues" SortExpression="Tuesday Calls">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday Calls" HeaderText="Wed" SortExpression="Wednesday Calls">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday Calls" HeaderText="Thur" SortExpression="Thursday Calls">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday Calls" HeaderText="Fri" SortExpression="Friday Calls">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Calls WTD" HeaderText="WTD" SortExpression="Total Calls WTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Calls MTD" HeaderText="MTD" SortExpression="Total Calls MTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Calls YTD" HeaderText="YTD" SortExpression="Total Calls YTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Monday Statements" HeaderText="Mon" SortExpression="Monday Statements">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday Statements" HeaderText="Tues" SortExpression="Tuesday Statements">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday Statements" HeaderText="Wed" SortExpression="Wednesday Statements">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday Statements" HeaderText="Thur" SortExpression="Thursday Statements">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday Statements" HeaderText="Fri" SortExpression="Friday Statements">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Statements WTD" HeaderText="WTD" SortExpression="Total Statements WTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Statements MTD" HeaderText="MTD" SortExpression="Total Statements MTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Statements YTD" HeaderText="YTD" SortExpression="Total Statements YTD">
                <ItemStyle Width="40px" HorizontalAlign="right" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <br />
    <div class="bucketfooter" id="div">
        <table width="100%">
            <tr>
                <td align="left" style="width: 33%;">
                    <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click" CommandArgument="grd">
                        <span style="height: 25px; vertical-align: middle;">
                            <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                Excel</span></asp:LinkButton>&nbsp;&nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
<asp:Label runat="server" ID="lblData" Text="No Records..." Visible="true"></asp:Label>
