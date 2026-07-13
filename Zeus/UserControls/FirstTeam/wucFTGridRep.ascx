<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTGridRep.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucFTGridRep" %>
<table width="100%">
    <tr>
        <td class="lblLeft" style="vertical-align: bottom">
            Page Size:
            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                <asp:ListItem Selected="true">10</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="lblRight">
            Ticket count:
            <asp:Label ID="lblTicketCount" runat="server" Text="">0</asp:Label><br />
            Unique merchants with tickets:
            <asp:Label ID="lblRecordCount" runat="server" Text="">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="grd" runat="server" Font-Names="Verdana" Font-Size="X-Small" OnSorting="grd_Sorting"
    CssClass="mGrid" OnRowCommand="grd_RowCommand" OnPageIndexChanging="grdTickets_PageIndexChanging"
    PagerStyle-CssClass="pgr" AllowPaging="True" AllowSorting="true" AlternatingRowStyle-CssClass="alt"
    EnableModelValidation="True" OnRowDataBound="grdTick_RowDataBound" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField HeaderText="Alert Level" Visible="false" />
        <asp:TemplateField HeaderText="ZID" SortExpression="MerchantID">
            <ItemTemplate>
                <asp:HiddenField runat="server" ID="hidMerchantUID" Value='<%# Bind("MerchantUID") %>' />
                <asp:HiddenField runat="server" ID="hidUserUID" Value='<%# Bind("UserUID") %>' />
                <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantUID") + "&Adding=false"  %>'
                    runat="server" ID="hypZID" Text='<%# Bind("MerchantID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBA" SortExpression="BusinessDBAName">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblDBA" Text='<%# Bind("BusinessDBAName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="BankName" HeaderText="Bank" SortExpression="BankName" />
        <asp:BoundField HeaderText="Multiple" Visible="false" />
        <asp:TemplateField HeaderText="PS Merchant?" SortExpression="ISFTMERCHANT">
            <ItemTemplate>
                <asp:Label runat="server" ID="lblIsFTMerchant"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Alerts" SortExpression="ALERTS">
            <ItemTemplate>
                <asp:LinkButton ID="lblAlerts" runat="server" CommandName="caAlerts" Text='<%# Bind("Alerts") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tasks" SortExpression="TASKS">
            <ItemTemplate>
                <asp:LinkButton ID="lblTasks" runat="server" CommandName="caTasks" Text='<%# Bind("Tasks") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tickets" SortExpression="TICKETS">
            <ItemTemplate>
                <asp:LinkButton ID="lblTickets" runat="server" CommandName="caTickets" Text='<%# Bind("Tickets") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        No Data...
    </EmptyDataTemplate>
    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
        LastPageText="&raquo;" />
    <AlternatingRowStyle CssClass="alt" />
    <PagerStyle CssClass="pgr" />
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetFTRepViewPaging" EnablePaging="True"
    MaximumRowsParameterName="PageSize" SelectCountMethod="GetFTRepViewPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="odsTickets_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
