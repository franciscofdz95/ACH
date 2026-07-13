<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucRejectGrid.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucRejectGrid" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>


<asp:GridView ID="grdReject" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
    DataKeyNames="RejectID" ShowHeaderWhenEmpty="True" OnRowDataBound="grdReject_RowDataBound"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
    AlternatingRowStyle-CssClass="alt" SelectedRowStyle-BackColor="#fffacd" 
    OnRowCommand="grdReject_RowCommand">
    <Columns>
      
      
        <asp:BoundField DataField="ReportDate" DataFormatString="{0:d}" HeaderText="Report Date" />
        <asp:BoundField DataField="ZID" HeaderText="ZID" />
        <asp:BoundField DataField="DDA" Visible="false" HeaderText="DDA" />
        <asp:BoundField DataField="ABA" Visible="false" HeaderText="ABA" />
        <asp:BoundField DataField="Amount"  DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right" HeaderText="Amount" />
        <asp:BoundField DataField="Code" HeaderText="Code" />
        <asp:BoundField DataField="Memo" HeaderText="Memo" />
        <%--<asp:BoundField DataField="JournalID" HeaderText="JournalID" />
        <asp:BoundField DataField="DateCreated" DataFormatString="{0:d}"  HeaderText="Date Created" />--%>
        <%--<asp:BoundField DataField="Reserve"  DataFormatString="{0:0.00}"  HeaderText="Reserve" />
        <asp:BoundField DataField="Divert"  DataFormatString="{0:0.00}"  HeaderText="Divert" />--%>
        <asp:BoundField DataField="BankID" HeaderText="Bank" />
      
      
    </Columns>
    <EmptyDataTemplate>
        No Records
    </EmptyDataTemplate>
    <AlternatingRowStyle CssClass="alt" />

<PagerStyle CssClass="pgr"></PagerStyle>

    <SelectedRowStyle BackColor="LemonChiffon" />
</asp:GridView>

<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click"><span style="height: 25px; vertical-align: middle;"><asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span></asp:LinkButton>



<div style="clear:both;"><!-- --></div>
