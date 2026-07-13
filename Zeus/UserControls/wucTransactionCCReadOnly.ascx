<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucTransactionCCReadOnly" Codebehind="wucTransactionCCReadOnly.ascx.cs" %>

<script type="text/javascript">

    
    function PrintCCReceipt()
    {   
        var TransID = document.getElementById('ctl00_ContentPlaceHolder1_pnlCC_TransID');
        
        //If null try to get master page transid       
        if (TransID == null)
        {
            var TransID = document.getElementById('ctl00_dlgCCTransaction_tmpl_pnlCCTransaction_TransID');
        }
        
        if (TransID != null)                                   
            OpenNewWindow('../FormTransaction/frmCCReceipt.aspx?TransID=' + TransID.value);
    }
    
    
</script>

<div style="text-align: left;">
    <br />
    <div class="title1">
        &nbsp;Credit Card Transaction Results
        <hr class="line" />
    </div>
    <table width="97%" border="0" cellpadding="0" cellspacing="2">
        <tr>
            <td class="lblRight" style="width: 12%;" valign="top">
                Merchant Name:
            </td>
            <td style="width: 35%;">
                <asp:Label ID="MerchantName" runat="server"></asp:Label>
            </td>
            <td class="lblRight" style="width: 12%;" valign="top">
                &nbsp;Posted Date:</td>
            <td style="width: 35%;">
                <asp:Label ID="PostedDate" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Trans ID:</td>
            <td>
                <asp:Label ID="TransID" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                &nbsp;Status:</td>
            <td>
                <asp:Label ID="StatusName" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Trans Type:</td>
            <td>
                <asp:Label ID="TransTypeName" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                CVV Response:</td>
            <td>
                <asp:Label ID="ResponseCVV2Desc" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Response Msg:</td>
            <td>
                <asp:Label ID="ResponseMsg" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                AVS Response:</td>
            <td>
                <asp:Label ID="ResponseAVSDesc" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Recur &nbsp;ID:</td>
            <td>
                <asp:Label ID="RecurID" runat="server"></asp:Label>&nbsp;
            </td>
            <td class="lblRight" valign="top">
                &nbsp;</td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <br />
    <div class="title1">
        &nbsp;Payment/Authorization Information
        <hr class="line" />
    </div>
    <table width="97%" border="0" cellpadding="0" cellspacing="2">
        <tr>
            <td class="lblRight" style="width: 12%;" valign="top">
                Card Number:</td>
            <td style="width: 35%;">
                <asp:Label ID="CardNumberMask" runat="server"></asp:Label>&nbsp;
            </td>
            <td class="lblRight" style="width: 12%;" valign="top">
                &nbsp;Expiration Date:</td>
            <td style="width: 35%;">
                <asp:Label ID="CardExpDateMMYY" runat="server"></asp:Label>&nbsp;
            </td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Amount:</td>
            <td>
                <asp:Label ID="Amount" runat="server"></asp:Label>
            </td>
            <td class="lblRight" valign="top">
                &nbsp;Reference Number:</td>
            <td>
                <asp:Label ID="ReferenceNumber" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <div class="title1">
        &nbsp;Customer Information
        <hr class="line" />
    </div>
    <table width="97%" border="0" cellpadding="0" cellspacing="2">
        <tr>
            <td class="lblRight" style="width: 12%;" valign="top">
                Customer ID:
            </td>
            <td style="width: 35%;">
                <asp:Label ID="CustomerID" runat="server"></asp:Label>
            </td>
            <td class="lblRight" style="width: 12%;" valign="top">
                &nbsp;Customer Name:</td>
            <td style="width: 35%;">
                <asp:Label ID="CustomerName" runat="server"></asp:Label></td>
        </tr>
        <%--<tr>
            <td class="lblRight">
                First Name:</td>
            <td>
                <asp:Label ID="BillingFirstName" runat="server" ></asp:Label></td>
            <td class="lblRight">
                &nbsp;Last Name:</td>
            <td>
                <asp:Label ID="BillingLastName" runat="server" ></asp:Label></td>
        </tr>--%>
        <tr>
            <td class="lblRight" valign="top">
                Address:</td>
            <td>
                <asp:Label ID="BillingAddress" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                &nbsp;City:</td>
            <td>
                <asp:Label ID="BillingCity" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                State:</td>
            <td>
                <asp:Label ID="BillingState" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                &nbsp;Zip:</td>
            <td>
                <asp:Label ID="BillingZip" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
                Phone:</td>
            <td>
                <asp:Label ID="BillingPhone" runat="server"></asp:Label></td>
            <td class="lblRight" valign="top">
                &nbsp;Email:</td>
            <td>
                <asp:Label ID="BillingEmail" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td class="lblRight" valign="top">
            </td>
            <td>
                &nbsp;</td>
            <td class="lblRight" valign="top">
            </td>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <div class="title1">
        &nbsp;Transaction Details
        <hr class="line" />
    </div>
    <br />
    <asp:Panel runat="server" ID="pnl" Width="94%" Height="150px" BorderColor="activeborder"
        BorderWidth="1px" ScrollBars="both" Visible="false">
        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="true" CssClass="mGrid"
            AllowSorting="true" AllowPaging="true" Width="98%">
            <AlternatingRowStyle CssClass="alt" />
            <PagerStyle CssClass="pgr" />
            <FooterStyle CssClass="footer" HorizontalAlign="Right" />
            <RowStyle HorizontalAlign="left" VerticalAlign="top" />
        </asp:GridView>
    </asp:Panel>
    &nbsp;&nbsp;<asp:Label ID="noRecords" Text="No Data.." runat="server"></asp:Label>
</div>
