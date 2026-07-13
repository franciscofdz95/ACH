<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTTicketList.ascx.cs"
    Inherits="wucFTTicketList" %>
<div id="contentpage">
    <table width="100%">
        <tr>
            <td>
                <fieldset>
                    <legend>Ticket Search</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <table width="100%">
                            <tr>
                                <td class="lblRight">
                                    DBA Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="BusinessDBAName" runat="server" Width="75px" EnableViewState="False"
                                        TabIndex="1"></asp:TextBox>
                                </td>
                                <td class="lblRight">
                                    Category:
                                </td>
                                <td>
                                    <asp:TextBox ID="TicketType" runat="server" Width="100px" EnableViewState="False"
                                        TabIndex="5"></asp:TextBox>
                                </td>
                                <td class="lblRight">
                                    Status:
                                </td>
                                <td>
                                    <asp:DropDownList ID="StatusID" runat="server" Width="90px" TabIndex="4">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </fieldset>
                <br />
                <fieldset>
                    <legend>Search Results</legend>
                    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">
                                    Page Size:
                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" DataKeyNames="TicketUID" OnRowDataBound="grd_RowDataBound"
                            OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                            OnSorting="grd_Sorting" DataSourceID="odsTickets">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                LastPageText="&raquo;" />
                            <Columns>
                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnTicketID" runat="server" CommandName="ID"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="ZID" HeaderText="ZID" Visible="false" ItemStyle-CssClass="togle">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Category" HeaderText="Sub-Category" SortExpression="Category">
                                    <ItemStyle Width="100px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Problem" HeaderText="Subject" Visible="false" ItemStyle-CssClass="togle">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Solution">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSolution" runat="server" Text='<%# Bind("Solution") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="DateCreated" HeaderText="Date Opened" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                                    SortExpression="DateCreated">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="DueDate" HeaderText="Due Date" SortExpression="DueDate"
                                    ItemStyle-CssClass="togle" DataFormatString="{0:MM-dd-yy HH:mm tt}">
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Days" HeaderText="Days Aged" SortExpression="Days">
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Priority" HeaderText="Severity" SortExpression="Priority">
                                    <ItemStyle Width="40px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="LastChangedDate" HeaderText="Last Modified" SortExpression="LastChangedDate"
                                    DataFormatString="{0:MM-dd-yy HH:mm tt}">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>
                                <%--  <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Tags" HeaderText="Tags" SortExpression="Tags" ItemStyle-CssClass="togle">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="RepeatCallIndicator" Visible="false" HeaderText="Repeat Call Indicator"
                                    SortExpression="RepeatCallIndicator" ItemStyle-CssClass="togle">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
                                 <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                                    <ItemStyle Width="150px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="MerchantID" HeaderText="MID" Visible="false" ItemStyle-CssClass="togle">
                                    <ItemStyle Width="80px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Origin" HeaderText="Origin" SortExpression="Origin">
                                    <ItemStyle Width="60px" />
                                </asp:BoundField>--%>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetTicketsPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetTicketsPagingCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsTickets_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <div class="bucketfooter">
                            <table width="100%">
                                <tr>
                                    <td align="left" style="width: 33%;">
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                    Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                        <%-- <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image1" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                                                    PDF</span></asp:LinkButton>--%>
                                    </td>
                                    <td align="right">
                                        Export:&nbsp;
                                    </td>
                                    <td align="left">
                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td align="right" style="width: 33%;">
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                        No data...
                    </asp:Panel>
                </fieldset>
                <br />
            </td>
        </tr>
    </table>
</div>
