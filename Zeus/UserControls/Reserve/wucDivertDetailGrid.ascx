<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDivertDetailGrid.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucDivertDetailGrid" %>
<asp:GridView ID="grdMD050" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" AllowSorting="true" OnSorting="grdMD050_Sorting"
     ShowHeaderWhenEmpty="true" OnPreRender="grdMD050_PreRender"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd" OnRowDataBound="grdMD050_RowDataBound" OnRowCommand="grdMD050_RowCommand">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
   
        <asp:BoundField DataField="ReportDate" SortExpression="ReportDate"  HeaderText="Report Date" DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="ZID" Visible="false"  HeaderText="ZID" />
        <asp:BoundField DataField="settleplatformmid" Visible="false" HeaderText="MID" />
        <asp:BoundField DataField="BusinessDBAName"  Visible="false" HeaderText="DBAName" />
        <asp:BoundField DataField="BankName" SortExpression="BankName" HeaderText="Bank"  Visible="false"/>
        <asp:BoundField DataField="DivertCategory" SortExpression="DivertCategory" HeaderText="Category"  Visible="true"/>
        <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" />
        <asp:BoundField DataField="BatchWithHeldAmount" SortExpression="BatchWithHeldAmount" HeaderText="Batch WithHeld" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" />
        <asp:BoundField DataField="Reserve" SortExpression="Reserve" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Reserve" />
        <asp:BoundField DataField="DivertClear" SortExpression="DivertClear" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Divert" />
        <asp:BoundField DataField="DivertReject" SortExpression="DivertReject" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Paysafe" />
        <asp:BoundField DataField="PostMerchant" SortExpression="PostMerchant" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Merchant" />
    </Columns>
    <EmptyDataTemplate>
        No Records
    </EmptyDataTemplate>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon" />
    <RowStyle CssClass="realrow" />
</asp:GridView>

<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click"><span style="height: 25px; vertical-align: middle;"><asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span></asp:LinkButton>



<div style="clear:both;"><!-- --></div>