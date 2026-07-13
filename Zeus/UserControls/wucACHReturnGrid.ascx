<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucACHReturnGrid" Codebehind="wucACHReturnGrid.ascx.cs" %>
<script id="igClientScript" type="text/javascript">
   
    function Field2Str(fieldvalue)
    {
        if (fieldvalue == null)
            return '';
        else
            return fieldvalue;
    }
    
    function ShowReturn(sGridID, sControlID)
    {
        var grid = igtbl_getGridById(sGridID);        
        var row = grid.getActiveRow();
        
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_TransID').innerHTML = Field2Str(row.getCellFromKey('TransID').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_RoutingNumber').innerHTML = Field2Str(row.getCellFromKey('RoutingNumber').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_MaskedAccountNumber').innerHTML = Field2Str(row.getCellFromKey('MaskedAccountNumber').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_AccountName').innerHTML = Field2Str(row.getCellFromKey('AccountName').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_Amount').innerHTML = Field2Str(row.getCellFromKey('Amount').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_ReferenceNumber').innerHTML = Field2Str(row.getCellFromKey('ReferenceNumber').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_Description').innerHTML = Field2Str(row.getCellFromKey('Description').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_MerchantName').innerHTML = Field2Str(row.getCellFromKey('MerchantName').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_ReturnReason').innerHTML = Field2Str(row.getCellFromKey('Reason Desc').getValue()); 

        if (row.getCellFromKey('DateProcessed').getValue() != null)
            document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_DateProcessed').innerHTML = Field2Str(row.getCellFromKey('DateProcessed').getValue().format("MM/dd/yy HH:mm"));
                
        if (row.getCellFromKey('PostedDate').getValue() != null)
            document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_ReturnDate').innerHTML = Field2Str(row.getCellFromKey('PostedDate').getValue().format("MM/dd/yy HH:mm"));
        
        if (row.getCellFromKey('TransDate').getValue() != null)
            document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_PostedDate').innerHTML = Field2Str(row.getCellFromKey('TransDate').getValue().format("MM/dd/yy HH:mm"));

        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_PaymentType').innerHTML = Field2Str(row.getCellFromKey('PaymentType').getValue());
        document.getElementById('ctl00_ContentPlaceHolder1_dlgReturn_tmpl_pnlReturn_BankAccountType').innerHTML = Field2Str(row.getCellFromKey('BankAccountType').getValue());
        
        dlg2 = $find('ctl00_ContentPlaceHolder1_dlgReturn');
        dlg2.set_windowState($IG.DialogWindowState.Normal);         
    }

</script>

<fieldset>
    <legend>
        <asp:Label ID="lblGridTitle" runat="server" Text="Label"></asp:Label></legend>
    <asp:Panel runat="server" ID="pnlRows" Width="90%">
        <div class="buckethdrright" align="right">
            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>&nbsp;</div>
        <asp:Panel runat="server" ID="pnl" ScrollBars="vertical" Height="200px">
            <asp:GridView ID="grd" runat="server" OnRowCommand="grdTran_RowCommand" OnRowDataBound="grdTran_RowDataBound"
                AutoGenerateColumns="false" ShowFooter="true" CssClass="mGrid" Font-Size="X-Small"
                Width="99.8%" Font-Names="verdana" DataKeyNames="ReturnID,Trans ID">
                <AlternatingRowStyle CssClass="alt" />
                <PagerStyle CssClass="pgr" />
                <FooterStyle Font-Bold="true" CssClass="footer" />
                <Columns>
                    
                    <asp:TemplateField HeaderText="TransID">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" ID="lnkTransID" Text='<%#Eval("Trans ID")%>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Posted Date" DataFormatString="{0:MM/dd/yy HH:mm}" HeaderText="Posted Date">
                    </asp:BoundField>
                    <asp:BoundField DataField="Masked Account No" HeaderText="Acct. #"></asp:BoundField>
                    <asp:BoundField DataField="Trans Route" HeaderText="Routing #"></asp:BoundField>
                    <asp:BoundField DataField="Account Name" HeaderText="Account Name"></asp:BoundField>
                    <asp:BoundField DataField="Ref ID" HeaderText="Ref #"></asp:BoundField>
                    <asp:BoundField DataField="Reason Desc" HeaderText="Reason"></asp:BoundField>
                    <asp:BoundField DataField="Amount" DataFormatString="{0:0.00}" HeaderText="Amount"></asp:BoundField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnAction" runat="server" Text="Button" Font-Size="8pt" Width="75px" />
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
    <asp:Label runat="server" ID="lblData" Text="No Data" Visible="false"></asp:Label>
</fieldset>
