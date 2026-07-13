<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_wucGridTicketsAgentsNew" Codebehind="wucGridTicketsAgentsNew.ascx.cs" %>

<fieldset>
    <legend>All Non-Closed Partner Tickets</legend>
    <table>
        <tr>
            <td>
                Page Size:</td>
            <td>
                <asp:DropDownList ID="cboPageSize2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize2_SelectedIndexChanged">
                    <asp:ListItem Selected="True">5</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>25</asp:ListItem>
                    <asp:ListItem>50</asp:ListItem>
                    <asp:ListItem>100</asp:ListItem>
                    <asp:ListItem>250</asp:ListItem>
                    <asp:ListItem>500</asp:ListItem>
                </asp:DropDownList></td>
            <td>
                Assigned To:</td>
            <td>
                <asp:DropDownList ID="UserID" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UserID_SelectedIndexChanged">
                </asp:DropDownList></td>
        </tr>
    </table>
    <asp:GridView ID="grdTickets" runat="server" AutoGenerateColumns="False" AllowPaging="True"
        OnDataBound="grdTickets_DataBound" AllowSorting="True" Font-Names="Verdana" Font-Size="X-Small"
        CssClass="mGrid" PageSize="5" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
        DataKeyNames="TicketUID" OnRowDataBound="grdTickets_RowDataBound" DataSourceID="odsTickets">
        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
        <Columns>
            <asp:TemplateField>
                <ItemStyle Width="10px" />
                <ItemTemplate>
                    <asp:Image runat="server" ID="img" ImageUrl="~/Images/msg.gif" AlternateText="Attention Required"
                        ToolTip="Attention Required" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                <ItemStyle Width="40px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                <ItemTemplate>
                    <asp:LinkButton ID="lbtnTicketID" runat="server" CommandName="View"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px" />
            </asp:TemplateField>
            <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                <ItemStyle Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="AgentFullName" HeaderText="Partner" SortExpression="AgentFullName">
                <ItemStyle Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="Problem" HeaderText="Problem" SortExpression="Problem">
                <ItemStyle Width="80px" />
            </asp:BoundField>
            <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="UserName" HeaderText="Assign To" SortExpression="UserName">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                SortExpression="DateCreated">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                <ItemStyle Width="60px" />
            </asp:BoundField>
            <%--<asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                SortExpression="DueDate">
                <ItemStyle Width="60px" />
            </asp:BoundField>--%>
            <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
        </Columns>
        <PagerStyle CssClass="pgr" />
        <AlternatingRowStyle CssClass="alt" />
    </asp:GridView>
    <asp:HiddenField ID="hidPageSize" Value="99" runat="server" />
    <asp:HiddenField ID="hidCurrentPage" Value="1" runat="server" />
    <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetTicketsPaging"
        SelectCountMethod="GetTicketsPagingCount" TypeName="PaymentXP.DataObjects.DataTicket"
        OldValuesParameterFormatString="original_{0}" OnSelecting="odsTickets_Selecting">
        <SelectParameters>
            <asp:Parameter Name="prms" Type="Object" />
            <asp:ControlParameter ControlID="hidPageSize" DefaultValue="5" Name="PageSize" PropertyName="Value"
                Type="Int32" />
            <asp:ControlParameter ControlID="hidCurrentPage" DefaultValue="1" Name="CurrentPage"
                PropertyName="Value" Type="Int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:Panel runat="server" ID="pnlNoTickets">
        &nbsp; No Pending Tickets..
    </asp:Panel>
</fieldset>
