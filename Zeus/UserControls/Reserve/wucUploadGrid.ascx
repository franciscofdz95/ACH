<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucUploadGrid.ascx.cs" Inherits="ZeusWeb.UserControls.Reserve.wucUploadGrid" %>


<asp:GridView ID="grdUpload" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" OnRowDataBound="grdUpload_RowDataBound"
    ShowHeaderWhenEmpty="True"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd">
    <Columns>
        <asp:BoundField DataField="DateCreated" HeaderText="Date Uploaded" />
        <asp:BoundField DataField="Filename" HeaderText="Filename" />
        <asp:BoundField DataField="TotalImported" HeaderText="Rows Imported" />
        <asp:BoundField DataField="ReportDates" HeaderText="Report Dates" />
        <asp:BoundField DataField="UploadedBy" HeaderText="Uploaded By" />
        <asp:TemplateField HeaderText="Date Completed">
            <ItemTemplate>
                <asp:Label ID="lblDateCompleted" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        Nothing uploaded yet.
    </EmptyDataTemplate>
    <AlternatingRowStyle CssClass="alt" />
    <PagerStyle CssClass="pgr"></PagerStyle>
    <SelectedRowStyle BackColor="LemonChiffon" />
    <RowStyle CssClass="realrow" />
</asp:GridView>
