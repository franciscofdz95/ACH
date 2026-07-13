    <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCRMCustomerGrid.ascx.cs" Inherits="ZeusWeb.UserControls.CRM.wucCRMCustomerGrid" %>

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
</script>




<asp:UpdatePanel runat="server" ID="UpdatePanel1" >
    <ContentTemplate>
        <asp:GridView ID="grdConsumerAction" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" ShowHeaderWhenEmpty="True"
            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
           SelectedRowStyle-BackColor="#fffacd">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
            <Columns>
                      
                               
                <asp:BoundField DataField="ActionDateTime" HeaderText="Action Date/Time">
                </asp:BoundField>  
                
                <asp:BoundField DataField="ActionType" HeaderText="Action Type">
                </asp:BoundField>        
                               
                <asp:BoundField DataField="ActionMethod" HeaderText="Action Method">
                </asp:BoundField> 
                
                 <asp:BoundField DataField="ActionOutcome" HeaderText="Outcome">
                </asp:BoundField>        
                               
                <asp:BoundField DataField="ActionSource" HeaderText="Action Source">
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