<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTGridHistory.ascx.cs" Inherits="ZeusWeb.UserControls.FirstTeam.wucFTGridHistory" %>
<table width="100%">
    <tr>
        <td style="text-align: left;">
            Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="250">250</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="text-align: right;">
            Total Record Count:
            <asp:Label runat="server" ID="lblRowCount">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="GridView1" CssClass="mGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" OnPageIndexChanging="GridView1_PageIndexChanging"
    OnRowDataBound="GridView1_RowDataBound" EnableModelValidation="True">
    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
    <Columns>
        <asp:BoundField HeaderText="Date Run" DataField="DateCreated" 
            ItemStyle-Width="130px">
        </asp:BoundField>
        <asp:BoundField DataField="MerchantID" ItemStyle-Width="100px" HeaderText="ZID">
        </asp:BoundField>
        <asp:BoundField DataField="BusinessDBAName" HeaderText="DBAName" />
        <asp:BoundField DataField="TicketID" HeaderText="TicketID" />
        <asp:BoundField DataField="RuleNameNice" HeaderText="Rule Name" />
    </Columns>
    <PagerStyle CssClass="pgr"></PagerStyle>
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMRuleRunLogPaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetMRuleRunLogPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>