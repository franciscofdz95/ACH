<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTGridManager.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucFTGridManager" %>

<table width="100%">
    <tr>
        <td class="lblLeft">
            <%--Page Size:
                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                        <asp:ListItem Selected="true">10</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                    </asp:DropDownList>--%>
        </td>
        <td class="lblRight">
            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" OnRowCommand="grd_RowCommand"
    OnRowDataBound="grdTick_RowDataBound" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
    AlternatingRowStyle-CssClass="alt" EnableModelValidation="True">
    <EmptyDataTemplate>
        No Data....
    </EmptyDataTemplate>
    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
        LastPageText="&raquo;" />
    <AlternatingRowStyle CssClass="alt" />
    <Columns>
        <asp:TemplateField HeaderText="Premier Services Manager" SortExpression="Username">
            <ItemTemplate>
                <asp:LinkButton ID="lnkRepID" runat="server" CommandName="FTRep" CommandArgument='<%#Eval("UserUID") %>'
                    OnClick="lnkRepID_Click"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle />
        </asp:TemplateField>
        <asp:BoundField DataField="NumAccounts" HeaderText="# of Accounts" SortExpression="NumAccounts">
            <ItemStyle Width="100px" HorizontalAlign="Right"  />
            <HeaderStyle  HorizontalAlign="Right"/>
        </asp:BoundField>
        <asp:TemplateField HeaderText="Alerts" SortExpression="Alerts">
            <ItemTemplate>
                <asp:LinkButton ID="lblAlerts" runat="server" CommandName="caAlerts" CommandArgument='<%#Eval("UserUID") %>' Text='<%# Bind("Alerts") %>'></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Width="100px" HorizontalAlign="Right"  />
            <HeaderStyle  HorizontalAlign="Right"/>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tasks" SortExpression="Tasks">
            <ItemTemplate>
                <asp:LinkButton ID="lblTasks" runat="server" CommandName="caTasks" CommandArgument='<%#Eval("UserUID") %>' Text='<%# Bind("Tasks") %>'></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Width="100px" HorizontalAlign="Right" />
            <HeaderStyle  HorizontalAlign="Right"/>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tickets" SortExpression="Tickets">
            <ItemTemplate>
                <asp:LinkButton ID="lblTickets" runat="server" CommandName="caTickets" CommandArgument='<%#Eval("UserUID") %>' Text='<%# Bind("Tickets") %>'></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Width="100px" HorizontalAlign="Right" />
            <HeaderStyle  HorizontalAlign="Right"/>
        </asp:TemplateField>
    </Columns>
    <PagerStyle CssClass="pgr" />
</asp:GridView>
