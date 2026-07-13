<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmPaysafeStatement" Title="Untitled Page" Codebehind="frmPaysafeStatement.aspx.cs" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Statements View</title>
</head>
<body>
    <form id="form1" runat="server"> 
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>   
    <div style="text-align:center">
    
    <rsweb:ReportViewer ID="rptMeritusStatement1" runat="server" Font-Names="Verdana" Font-Size="8pt" Height="400px" Width="100%">
        <LocalReport ReportPath="web\Reports\PaysafeStatement.rdlc">
            <DataSources>
                <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="dsMerchantMonthlyInvoice" />
            </DataSources>
        </LocalReport>
    </rsweb:ReportViewer>

    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" 
        TypeName="ZeusWeb.ds_sp_SelectMerchantMonthlyInvoiceTableAdapters.sp_SelectMerchantMonthlyInvoiceTableAdapter" OldValuesParameterFormatString="original_{0}">
        <SelectParameters>
            <asp:Parameter Name="MerchantID" Type="Int32" />
            <asp:Parameter Name="BillDate" Type="DateTime" />
        </SelectParameters>
        </asp:ObjectDataSource>
    </div> 
    </form>
</body>
</html>

