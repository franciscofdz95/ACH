<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMultiLinkMerchant.ascx.cs"
    Inherits="wucMultiLinkMerchant" %>
<asp:GridView ID="grdLinks" OnPageIndexChanging="grd_PageIndexChanging" DataSourceID="ObjectDataSource1"
    runat="server" AutoGenerateColumns="False" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
    PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" Font-Names="verdana"
    Font-Size="X-Small" OnRowDataBound="grdLinks_RowDataBound" OnRowCancelingEdit="grdLinks_RowCancelingEdit"
    OnRowEditing="grdLinks_RowEditing" OnRowUpdating="grdLinks_RowUpdating" DataKeyNames="DocID"
    AllowPaging="true">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
        <asp:TemplateField HeaderText="ZID">
            <ItemTemplate>
                <asp:HyperLink ID="hypZID" runat="server" Text='<%# Eval("ID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Legal Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="LegalName"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBA">
            <ItemTemplate>
                <asp:Label ID="DBAName" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Contact #">
            <ItemTemplate>
                <asp:Label runat="server" ID="ContactNo"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBA #">
            <ItemTemplate>
                <asp:Label runat="server" ID="DBANo"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Mailing Address">
            <ItemTemplate>
                <asp:Label runat="server" ID="MailingAddress"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Business Address">
            <ItemTemplate>
                <asp:Label runat="server" ID="BusinessAddress"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Website">
            <ItemTemplate>
                <asp:Label ID="Website" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Tax ID">
            <ItemTemplate>
                <asp:Label runat="server" ID="TaxID"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Owner Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="Name"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Owner SSN">
            <ItemTemplate>
                <asp:Label runat="server" ID="SSN"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Phone">
            <ItemTemplate>
                <asp:Label runat="server" ID="Phone"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Contact Name">
            <ItemTemplate>
                <asp:Label runat="server" ID="ContactName"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Bank Contact #">
            <ItemTemplate>
                <asp:Label runat="server" ID="BankContact" Text='#Eval("BankAccount")'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle CssClass="footer" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetMultilinkPaging" EnablePaging="True"
    MaximumRowsParameterName="PageSize" SelectCountMethod="GetMultilinkPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DatamerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
