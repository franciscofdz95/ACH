<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucSalesDetailedWeekly" Codebehind="wucSalesDetailedWeeklyGrid.ascx.cs" %>
<asp:Panel runat="server" ID="pnl" Visible="false" ScrollBars="horizontal" Width="960px">
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
        ShowFooter="true" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
        Width="98%" AlternatingRowStyle-CssClass="alt" OnRowDataBound="grd_RowDataBound" onRowCreated="grd_RowCreated"
        FooterStyle-CssClass="footer">
        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
        <Columns>
            <asp:BoundField HeaderText="Sales Rep" DataField="Name" SortExpression="Name">
                <ItemStyle Width="150px" />
            </asp:BoundField>
            <asp:BoundField HeaderText="Ext" DataField="Ext" SortExpression="Ext">
                <ItemStyle Width="30px" />
            </asp:BoundField>
            <asp:BoundField DataField="Monday Sub" HeaderText="Mon" SortExpression="Monday Sub">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday Sub" HeaderText="Tues" SortExpression="Tuesday Sub">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday Sub" HeaderText="Wed" SortExpression="Wednesday Sub">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday Sub" HeaderText="Thur" SortExpression="Thursday Sub">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday Sub" HeaderText="Fri" SortExpression="Friday Sub">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Sub WTD" HeaderText="WTD" SortExpression="Total Sub WTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Sub MTD" HeaderText="MTD" SortExpression="Total Sub MTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Sub YTD" HeaderText="YTD" SortExpression="Total Sub YTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Monday App" HeaderText="Mon" SortExpression="Monday App">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday App" HeaderText="Tues" SortExpression="Tuesday App">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday App" HeaderText="Wed" SortExpression="Wednesday App">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday App" HeaderText="Thur" SortExpression="Thursday App">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday App" HeaderText="Fri" SortExpression="Friday App">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total App WTD" HeaderText="WTD" SortExpression="Total App WTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total App MTD" HeaderText="MTD" SortExpression="Total App MTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total App YTD" HeaderText="YTD" SortExpression="Total App YTD">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <%--<asp:BoundField DataField="Monday AppVol" HeaderText="Mon" SortExpression="Monday AppVol"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday AppVol" HeaderText="Tues" SortExpression="Tuesday AppVol"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday AppVol" HeaderText="Wed" SortExpression="Wednesday AppVol"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday AppVol" HeaderText="Thur" SortExpression="Thursday AppVol"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday AppVol" HeaderText="Fri" SortExpression="Friday AppVol"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total AppVol WTD" HeaderText="WTD" SortExpression="Total AppVol WTD"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total AppVol MTD" HeaderText="MTD" SortExpression="Total AppVol MTD"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total AppVol YTD" HeaderText="YTD" SortExpression="Total AppVol YTD"
                DataFormatString="{0:c}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>--%>
            <asp:BoundField DataField="Monday Vol" HeaderText="Mon" SortExpression="Monday Vol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday Vol" HeaderText="Tues" SortExpression="Tuesday Vol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday Vol" HeaderText="Wed" SortExpression="Wednesday Vol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday Vol" HeaderText="Thur" SortExpression="Thursday Vol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday Vol" HeaderText="Fri" SortExpression="Friday Vol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Vol WTD" HeaderText="WTD" SortExpression="Total Vol WTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Vol MTD" HeaderText="MTD" SortExpression="Total Vol MTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total Vol YTD" HeaderText="YTD" SortExpression="Total Vol YTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Monday MonVol" HeaderText="Mon" SortExpression="Monday MonVol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Tuesday MonVol" HeaderText="Tues" SortExpression="Tuesday MonVol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Wednesday MonVol" HeaderText="Wed" SortExpression="Wednesday MonVol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Thursday MonVol" HeaderText="Thur" SortExpression="Thursday MonVol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Friday MonVol" HeaderText="Fri" SortExpression="Friday MonVol"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total MonVol WTD" HeaderText="WTD" SortExpression="Total MonVol WTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total MonVol MTD" HeaderText="MTD" SortExpression="Total MonVol MTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Total MonVol YTD" HeaderText="YTD" SortExpression="Total MonVol YTD"
                DataFormatString="{0:0.00}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Avg Statements Per App Approved" HeaderText="Avg Statements Per App Approved"
                SortExpression="Avg Statements Per App Approved" DataFormatString="{0:0.0}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
            <asp:BoundField DataField="Avg Calls Per App Approved" HeaderText="Avg Calls Per App Approved"
                SortExpression="Avg Calls Per App Approved" DataFormatString="{0:0.0}">
                <ItemStyle Width="30px" HorizontalAlign="right" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <br />
    <div class="bucketfooter" id="div" style="width: 1025px;">
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
