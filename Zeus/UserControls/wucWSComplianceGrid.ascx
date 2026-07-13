<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_wucWSComplianceGrid"
    CodeBehind="wucWSComplianceGrid.ascx.cs" %>
<div style="text-align: right;">
    Total Records Found:
    <asp:Label runat="server" ID="lblRowCount">0</asp:Label>
</div>
<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
    OnRowCommand="GridView1_RowCommand"
    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer"
    Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="WSComplianceID">
    <Columns>
        <asp:BoundField DataField="TicketID" HeaderText="TicketID" />
        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" />
        <asp:TemplateField HeaderText="Type">
            <ItemTemplate>
                <asp:Label ID="lblRequestType" runat="server" Text='<%# Bind("RequestType") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="CompletedBy" HeaderText="Completed By" />
        <asp:TemplateField HeaderText="Status">
            <ItemTemplate>
                <asp:Label ID="lblComplianceStatus" runat="server" Text='<%# Bind("WSComplianceStatusUID") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="DateCompletedBy" DataFormatString="{0:MM/dd/yyy}" HeaderText="Date Completed" />
        <asp:TemplateField HeaderText="Rating">
            <ItemTemplate>
                <asp:Label ID="lblScore" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Risk Level">
            <ItemTemplate>
                <asp:Label ID="lblRiskScore" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Letter Sent">
            <ItemTemplate>
                <asp:Label ID="lblLetterSent" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Checklist">
            <ItemTemplate>
                <asp:LinkButton runat="server" ID="lbView" CommandName="view" CommandArgument='<%# Eval("TicketUID") + "_" + Eval("WSComplianceID") %>'>View</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        None
    </EmptyDataTemplate>
    <FooterStyle CssClass="footer" />
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetWSCompliancePaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetWSCompliancePagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
