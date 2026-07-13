<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucCrawlerHistory" Codebehind="wucCrawlerHistory.ascx.cs" %>
<div style="width: 98%">
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
        Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsCrawlers" Width="100%"
        DataKeyNames="CrawlerID" OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand">
        <PagerStyle CssClass="pgr" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnEnbaled" runat="server" Text="Button" CausesValidation="false"
                        Width="75px" />
                </ItemTemplate>
                <ItemStyle Width="75px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="URL">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkURL" runat="server" Target="_blank"></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Snap Shot">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkScreenShot" runat="server" Target="_blank">View</asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Width="75px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Report">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkReport" runat="server" Target="_blank">View</asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Width="75px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="HTML">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkHTML" runat="server" Target="_blank">View</asp:HyperLink>
                </ItemTemplate>
                <ItemStyle Width="75px" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:BoundField DataField="StatusName" HeaderText="Status">
                <ItemStyle Width="85px" />
            </asp:BoundField>
            <asp:BoundField DataField="ContentSize" HeaderText="Content Size">
                <ItemStyle Width="85px" />
            </asp:BoundField>
            <asp:BoundField DataField="DateRequested" HeaderText="Date Requested">
                <ItemStyle Width="85px" />
            </asp:BoundField>
            <asp:BoundField DataField="DateProcessed" HeaderText="Date Processed">
                <ItemStyle Width="85px" />
            </asp:BoundField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsCrawlers" runat="server" SelectMethod="GetCrawlers"
        TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
        OnSelecting="odsCrawlers_Selecting">
        <SelectParameters>
            <asp:Parameter Name="prms" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
