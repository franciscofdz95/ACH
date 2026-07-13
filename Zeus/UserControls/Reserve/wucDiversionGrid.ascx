<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucDiversionGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucDiversionGrid" %>
<asp:GridView ID="grdDivertMethod" runat="server" AutoGenerateColumns="False"
    Font-Names="Verdana" ShowHeaderWhenEmpty="True"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd" OnRowDataBound="grdDivertMethod_RowDataBound"
    OnRowCommand="grdDivertMethod_RowCommand">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
        <asp:TemplateField HeaderText="Date Diverted">
            <ItemTemplate>
                <asp:LinkButton ID="lnkDateDiverted" CommandName="ReportDate" CommandArgument='<%# Bind("DiversionID") %>'
                    runat="server"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:BoundField DataField="DateUndiverted" HeaderText="Date Undiverted" NullDisplayText="" />
        <asp:BoundField DataField="DivertedBy" HeaderText="Diverted By" />
        <asp:TemplateField HeaderText="Reserve Rate">
            <ItemTemplate>
                <asp:Label ID="lblReserveRate" runat="server" Text='<%# Bind("ReserveRate") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:BoundField DataField="DivertedReason" HeaderText="Reason" />
        <asp:BoundField DataField="Resolution" HeaderText="Resolution" />
        <asp:BoundField DataField="DiversionType" HeaderText="Type" />
        <%--   <asp:TemplateField>
            <ItemTemplate>
               <asp:Button runat="server" CommandName="RemoveDiversion" 
                    CommandArgument='<%# Bind("DiversionID") %>' ID="btnRemove" 
                    Text="Remove Diversion" onclick="btnRemove_Click" />
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon"></SelectedRowStyle>
</asp:GridView>
