<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucSummaryGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucSummaryGrid" %>
<table width="100%" id="gridhead" runat="server">
    <tr>
        <td style="text-align: left;">Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize"
                OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
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



<asp:Panel runat="server" ID="pnlDate">
    <asp:DropDownList runat="server" ID="ddlReportDate" AutoPostBack="true" OnSelectedIndexChanged="lstPeriods_SelectedIndexChanged">
    </asp:DropDownList>
</asp:Panel>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowDataBound="GridView1_RowDataBound" AllowPaging="True" ShowHeaderWhenEmpty="True"
    OnRowCreated="GridView1_RowCreated" OnRowCommand="GridView1_RowCommand" Font-Names="Verdana"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" OnPageIndexChanging="GridView1_PageIndexChanging"
    SelectedRowStyle-BackColor="#fffacd">
    <Columns>
        <asp:TemplateField HeaderText="MID">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypMID" NavigateUrl='<%# WebUtil.GetBaseUrl() + "SecureRiskForms/frmReserveSearch.aspx?MID=" + Eval("merchantid").ToString() %>' Text='<%# Bind("merchantid") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ZID">
            <ItemTemplate>
                <asp:Label ID="Label2" runat="server" Text='<%# Bind("zid") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="dbaname" HeaderText="DBAName" />
        <asp:BoundField DataField="Divert_Deposit" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Deposit">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Divert_Release" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Release">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Divert_ToReserve" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="To Reserve">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Divert_Net" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Net">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reserve_Deposit" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Deposit">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reserve_Release" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Release">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reserve_FromDivert" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="From Divert">
            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reserve_Net" DataFormatString="{0:C2}" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right"
            HeaderText="Net">

            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>

        <asp:TemplateField HeaderText="Balance">
            <ItemTemplate>

                <asp:Label runat="server" ForeColor="Red" ID="lblPending">(PENDING)</asp:Label>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Balance", "{0:C2}") %>'></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        No Reserves
    </EmptyDataTemplate>
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon"></SelectedRowStyle>
</asp:GridView>

<table runat="server" id="tblExport" visible="true">
    <tr>
        <td>
            <asp:RadioButtonList ID="lstExportPageSize" runat="server" RepeatColumns="2">
                <asp:ListItem Selected="true" Value="Current Page">Current Page</asp:ListItem>
                <asp:ListItem Value="All Pages">All Pages</asp:ListItem>
            </asp:RadioButtonList></td>
        <td>
            <asp:LinkButton ID="btnExpExcel" runat="server" CausesValidation="false" OnClick="btnExport_Click">
                <span style="height: 25px; vertical-align: middle;">
                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
            </asp:LinkButton>
        </td>
    </tr>
</table>






<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetReserveSummaryPaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetReserveSummaryPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
