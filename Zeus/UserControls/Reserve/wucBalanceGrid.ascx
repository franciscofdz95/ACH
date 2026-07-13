<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucBalanceGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucBalanceGrid" %>
<table width="100%">
    <tr>
        <td style="text-align: left;">Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="250">250</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="text-align: right;">Total Record Count:
            <asp:Label runat="server" ID="lblRowCount">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True"
    OnPageIndexChanging="GridView1_PageIndexChanging" Font-Names="Verdana" Font-Size="X-Small"
    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd" OnRowDataBound="GridView1_RowDataBound"
    OnRowCommand="grd_RowCommand">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
        <asp:TemplateField HeaderText="ZID">
            <ItemTemplate>
                <asp:LinkButton ID="lnkZID" CommandName="ZID" CommandArgument='<%# Bind("ZID") %>'
                    Text='<%# Bind("ZID") %>' runat="server"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="MID">

            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypMID" NavigateUrl='<%# WebUtil.GetBaseUrl() + "SecureRiskForms/frmReserveSearch.aspx?MID=" + Eval("MID").ToString() %>' Text='<%# Bind("MID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBAName">
            <ItemTemplate>
                <asp:Label ID="lblDBAName" runat="server" Text='<%# Bind("DBAName") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Divert">
            <ItemTemplate>
                <asp:Label runat="server" ID="lbDivert" Text='<%# String.Format("{0:0.00}", Eval("Divert") ) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Reserve">
            <ItemTemplate>
                <asp:Label runat="server" ID="lbReserve" Text='<%# String.Format("{0:0.00}", Eval("Reserve") ) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Balance">
            
            <ItemTemplate>
                <asp:Label runat="server" ForeColor="Red" ID="lblPending">(PENDING)</asp:Label>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Balance", "{0:0.00}") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
    </Columns>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon"></SelectedRowStyle>
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetReserveBalancePaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetReserveBalancePagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<%--      <hr class="line" />
            <asp:LinkButton ID="lnkExportSummary" runat="server">
                <span style="height: 25px; vertical-align: middle;">
                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span> <span style="margin-left: 5px;">
                        Save Excel</span></asp:LinkButton>--%>