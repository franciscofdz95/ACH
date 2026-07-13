<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucACHGrid" Codebehind="wucACHGrid.ascx.cs" %>
<fieldset>
    <legend>
        <asp:Label ID="lblGridTitle" runat="server" Text="Label"></asp:Label></legend>
    <asp:Panel runat="server" ID="pnlRows" Width="90%">
        <div class="buckethdrright" align="right">
            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>&nbsp;</div>
        <asp:Panel runat="server" ID="pnl" Height="200px" ScrollBars="vertical">
            <asp:GridView ID="grdTran" runat="server" OnRowCommand="grdTran_RowCommand" OnRowDataBound="grdTran_RowDataBound"
                AutoGenerateColumns="false" ShowFooter="true" CssClass="mGrid" Font-Size="X-Small"
                Width="99.8%" Font-Names="verdana">
                <AlternatingRowStyle CssClass="alt" />
                <PagerStyle CssClass="pgr" />
                <FooterStyle Font-Bold="true" CssClass="footer" />
                <Columns>
                    <asp:BoundField DataField="TransID" HeaderText="TransID"></asp:BoundField>
                    <asp:BoundField DataField="Trans Date" DataFormatString="{0:MM/dd/yy HH:mm}" HeaderText="Posted Date">
                    </asp:BoundField>
                    <asp:BoundField DataField="Masked Account Number" HeaderText="Acct. #"></asp:BoundField>
                    <asp:BoundField DataField="TransRoute" HeaderText="Routing #"></asp:BoundField>
                    <asp:BoundField DataField="Account Name" HeaderText="Account Name"></asp:BoundField>
                    <asp:BoundField DataField="Origin" HeaderText="Type"></asp:BoundField>
                    <asp:BoundField DataField="Ref ID" HeaderText="Ref No"></asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status"></asp:BoundField>
                    <asp:BoundField DataField="Amount" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="right"
                        HeaderText="Amount" FooterStyle-HorizontalAlign="right"></asp:BoundField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnAction" runat="server" Text="Button" Width="75px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <div class="bucketfooter">
            <div class="bucketfooterleft">
                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                            PDF</span></asp:LinkButton>
            </div>
        </div>
    </asp:Panel>
    <asp:Label ID="lblData" Text="No Data" runat="server" Visible="false"></asp:Label>
</fieldset>
