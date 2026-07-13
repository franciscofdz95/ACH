<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true"
    Inherits="frmRiskBatchReleased" Title="Batch Released" CodeBehind="frmRiskBatchReleased.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <fieldset>
        <div class="buckethdright">
            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>&nbsp;</div>
        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="true" OnRowCommand="grd_RowCommand"
            DataKeyNames="MerchantAppUID,BatchID,ZID" OnRowDataBound="grd_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Hold">
                    <ItemTemplate>
                        <asp:Button ID="btnHold" runat="server" Text="Hold" CommandName="Hold" OnClientClick="return confirm('Do you want to HOLD this batch?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Void">
                    <ItemTemplate>
                        <asp:Button ID="btnVoid" runat="server" Text="Void" CommandName="Void" OnClientClick="return confirm('Do you want to VOID this batch?');" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="MID">
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%#  "~/SecureRiskForms/frmRiskBatchDetails.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&BatchID=" +  Eval("BatchID") %>'
                            runat="server" ID="hypZID" Text='<%# Eval("MID") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DBA" HeaderText="DBA" />
                <asp:BoundField DataField="Legal" HeaderText="Legal" />
                <asp:BoundField DataField="ReleasedBy" HeaderText="Released By" />
                <asp:BoundField DataField="SalesCount" HeaderText="Sales Count" ItemStyle-HorizontalAlign="right" />
                <asp:BoundField DataField="SalesAmount" HeaderText="Sales Amount" DataFormatString="{0:0.00}"
                    ItemStyle-HorizontalAlign="right" />
                <asp:BoundField DataField="CreditCount" HeaderText="Credit Count" ItemStyle-HorizontalAlign="right" />
                <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amount" DataFormatString="{0:0.00}"
                    ItemStyle-HorizontalAlign="right" />
            </Columns>
        </asp:GridView>
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
    </fieldset>
</asp:Content>
