<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCRMNotificationGrid.ascx.cs" Inherits="ZeusWeb.UserControls.CRM.wucCRMNotificationGrid" %>

<script type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

    });

    //        this is a really stupid bug. basically, when you're using jquery calls mixed with microsoft ajax (ie update panel stuff), the jquery calls dont work
    //        anymore after and update panel is triggered. because the jquery does not know when the request ended. so the fix is to manually call the jquery code
    //        when the end request event is fired by the update panel.
    //        
    //        http://zeemalik.wordpress.com/2007/11/27/how-to-call-client-side-javascript-function-after-an-updatepanel-asychronous-ajax-request-is-over/


    function EndRequestHandler() {

        $('.ttfield').tooltip({
            "placement": "right",
            "trigger": "hover"
        });
    }
    //PXP-13240 : by Satyajit on 03/11/2020
    function viewHTMLData(type, htmlContent)
    {
        var w = window.open();
        var html = htmlContent;
        w.document.title = type;
        $(w.document.body).html(html);
    }
</script>




<asp:UpdatePanel runat="server" ID="UpdatePanel1" >
    <ContentTemplate>
        <asp:GridView ID="grdNotificationHistory" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" ShowHeaderWhenEmpty="True"
            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" 
            OnRowDataBound="grdNotificationHistory_RowDataBound"
            SelectedRowStyle-BackColor="#fffacd">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
            <Columns>  
                <asp:BoundField DataField="NotificationDateTime" HeaderText="Notification Date/Time">
                </asp:BoundField>    
                
                <asp:BoundField DataField="NotificationType" HeaderText="Notification Type">
                </asp:BoundField>  
                
                 <asp:BoundField DataField="NotificationMethod" HeaderText="Notification Method">
                </asp:BoundField> 
                           
                <asp:BoundField DataField="OrderID" HeaderText="Order ID">
                </asp:BoundField>        
                                     
                <asp:BoundField DataField="NMITransactionID" HeaderText="NMI Transaction ID">
                </asp:BoundField>  
                               
                <asp:BoundField DataField="CustomerName" HeaderText="Customer Name">
                </asp:BoundField>   

                <asp:BoundField DataField="CustomerEmail" HeaderText="Customer Email">
                </asp:BoundField>        
                               
                <asp:BoundField DataField="CustomerPhone" HeaderText="Customer Phone">
                </asp:BoundField> 
                <%--PXP-12164 by Sanidhya Kumar--%>
                <asp:BoundField DataField="NotificationContent" HeaderText="Notification Content">
                </asp:BoundField>

            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
            <SelectedRowStyle BackColor="LemonChiffon"></SelectedRowStyle>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>


<div style="clear: both;">
    <!-- -->
</div>

<script type="text/javascript">

    $('.ttfield').tooltip({
        "placement": "right",
        "trigger": "hover"
    });

</script>
