<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoiceFees.aspx.cs" Inherits="ZeusWeb.ajax.InvoiceFees" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="grdInvoiceHistory" AutoGenerateColumns="false" runat="server" AllowSorting="false" AllowPaging="false"
            HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small" BorderColor="white" BorderStyle="None"
            Visible="false" GridLines="none" BorderWidth="0px" Width="100%" CssClass="mGrid">
            <HeaderStyle HorizontalAlign="Left" />
            <RowStyle HorizontalAlign="Left" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="MerchantID" HeaderText="ZID" />
                <asp:BoundField DataField="SettlePlatformMID" HeaderText="MID" DataFormatString="&nbsp;{0}" />
                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA" />
                <asp:BoundField DataField="SalesPartnerDBA" HeaderText="Agent" />
                <asp:BoundField DataField="SalesPartnerID" HeaderText="Agent ID" />
                <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                <asp:BoundField DataField="FeeDescription" HeaderText="Fee Description" />
                <asp:BoundField DataField="AchTransID" HeaderText="ACH Trans ID" />
                <asp:BoundField DataField="ACHDescriptor" HeaderText="ACH Descriptor" />
                <asp:BoundField DataField="ACHStatus" HeaderText="ACHStatus" />
                <asp:BoundField DataField="ACHBillDate" HeaderText="ACH Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="InvoiceAmount" HeaderText="Subtotal" DataFormatString="{0:0.00}" />
            </Columns>
        </asp:GridView>

        <asp:GridView ID="grdInvoiceConfirm" AutoGenerateColumns="false" runat="server" AllowSorting="false" AllowPaging="false"
            HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small" BorderColor="white" BorderStyle="None" Visible="false"
            GridLines="none" BorderWidth="0px" Width="100%" CssClass="mGrid">
            <HeaderStyle HorizontalAlign="Left" />
            <RowStyle HorizontalAlign="Left" />
            <AlternatingRowStyle CssClass="alt" />
            <Columns>
                <asp:BoundField DataField="ACHDate" DataFormatString="{0:d}" HeaderText="Date Created" />
                <asp:BoundField DataField="ACHMonthDate" HeaderText="Month Created" />
                <asp:BoundField DataField="Product" HeaderText="Product" />
                <asp:BoundField DataField="BillBy" HeaderText="Created By" />
                <asp:BoundField DataField="TotalAmount" DataFormatString="{0:0.00}" HeaderText="Amount" />
                <asp:BoundField DataField="TotalClosed" DataFormatString="{0:0.00}" HeaderText="Amount Closed" />
                <asp:BoundField DataField="TotalRejected" DataFormatString="{0:0.00}" HeaderText="Amount Rejected" />
                <asp:BoundField DataField="PercentClosed" HeaderText="Amount Collected %" />
            </Columns>
        </asp:GridView>

        <asp:GridView ID="grdInvoice" AutoGenerateColumns="false" runat="server" AllowSorting="false" AllowPaging="false"
                HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small" BorderColor="white" BorderStyle="None"
                GridLines="none" BorderWidth="0px" Width="100%" CssClass="mGrid" EmptyDataText="No invoices to bill.">
                <HeaderStyle HorizontalAlign="Left" />
                <RowStyle HorizontalAlign="Left" />
                
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="MerchantID" HeaderText="ZID" />
                    <asp:BoundField DataField="SettlePlatformMID" HeaderText="MID" DataFormatString="&nbsp;{0}" />
                    <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA" />
                    <asp:BoundField DataField="SalesPartnerDBA" HeaderText="Agent" />
                    <asp:BoundField DataField="SalesPartnerID" HeaderText="Agent ID" />
                    <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
                    <asp:BoundField DataField="MerchantStatus" HeaderText="Current Status" />
                    <asp:BoundField DataField="DateEnrolled" HeaderText="Date Enrolled" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="LastBillDate" HeaderText="Last Billed" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="FeeDescription" HeaderText="Fee Description" />
                    <asp:BoundField DataField="ACHDescriptor" HeaderText="ACH Descriptor" />
                    <asp:BoundField DataField="BillDate" HeaderText="Bill Date" DataFormatString="{0:d}" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                    <asp:BoundField DataField="InvoiceAmount" HeaderText="Subtotal" DataFormatString="{0:0.00}" />
                </Columns>
            </asp:GridView>
    </div>
    </form>
</body>
</html>
