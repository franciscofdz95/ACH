<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLastTicketsAgent.ascx.cs"
    Inherits="ZeusWeb.UserControls.SidePanel.wucLastTicketsAgent" %>

    <fieldset style="width: 320px;">
        <legend>Last Tickets</legend>
        <asp:UpdatePanel ID="pnl" runat="server" UpdateMode="conditional">
            <ContentTemplate>
                <asp:Panel ID="pnlDetail" runat="server">
                    <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                                    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                                        Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                        DataKeyNames="TicketUID" OnRowDataBound="grd_RowDataBound" PageSize="4" DataSourceID="odsTickets">
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnTicket" runat="server" CausesValidation="false" CommandName="View"
                                                        Text=""></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="30px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                <ItemStyle Width="40px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                                <ItemStyle Width="60px" />
                                            </asp:BoundField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetTicketsPaging"
                                        TypeName="DataMerchantAppPaging" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTicketsPagingCount"
                                        StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                                        OnSelecting="odsTickets_Selecting">
                                        <SelectParameters>
                                            <asp:Parameter Name="prms" Type="Object" />
                                            <asp:Parameter Name="PageSize" Type="Int32" />
                                            <asp:Parameter Name="CurrentPage" Type="Int32" />
                                            <asp:Parameter Name="ControlID" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <br />
                                </asp:Panel>
                                <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                                    No data...
                                </asp:Panel>
                                  <asp:HyperLink runat="server" style="float:right;"   ID="hypMore">More</asp:HyperLink>
                                <asp:HyperLink ID="btnTicket" CssClass="fakea" runat="server">New Ticket</asp:HyperLink>&nbsp;
                                <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click">Refresh</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>

