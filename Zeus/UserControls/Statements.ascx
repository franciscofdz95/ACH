<%@ Control Language="C#" AutoEventWireup="true" Inherits="ucontrol_Statements" Codebehind="Statements.ascx.cs" %>
<asp:GridView ID="grdStatements" runat="server" AutoGenerateColumns="False" GridLines="Vertical"
    ShowFooter="True" CssClass="mGrid" OnRowDataBound="grdStatements_RowDataBound" Width="96%">
    <FooterStyle CssClass="footer" HorizontalAlign="Right" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
    <Columns>
        <asp:BoundField DataField="Year" HeaderText="Year" />
        <asp:BoundField DataField="Month" HeaderText="Month" />
        <asp:HyperLinkField DataNavigateUrlFields="View" DataTextField="View" Target="_blank" />
    </Columns>
</asp:GridView>
