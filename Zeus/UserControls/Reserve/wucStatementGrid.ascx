<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucStatementGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.Reserve.wucStatementGrid" %>



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




<asp:UpdatePanel runat="server" ID="UpdatePanel1" OnPreRender="UpdatePanel1_PreRender">
    <ContentTemplate>
        <asp:GridView ID="grdStatement" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" ShowHeaderWhenEmpty="True"
            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
            OnRowDataBound="grdStatement_RowDataBound" SelectedRowStyle-BackColor="#fffacd">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("PostedDate", "{0:MM/dd/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        Dates<br />
                        <asp:DropDownList runat="server" ID="ddlReportDate" AutoPostBack="true" OnSelectedIndexChanged="lstPeriods_SelectedIndexChanged">
                        </asp:DropDownList>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        Reserve Type<br />
                        <asp:DropDownList runat="server" ID="ddlReserveType" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                        </asp:DropDownList>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Source">
                    <ItemTemplate>
                        
                            <asp:Label ID="Label3" runat="server" data-toggle="tooltip" class="ttfield" ToolTip='<%# Bind("Comments") %>' Text='<%# Bind("Source") %>'></asp:Label>
                        
                    </ItemTemplate>
                    <HeaderTemplate>
                        Description<br />
                        <asp:DropDownList runat="server" ID="ddlReserveSource" AutoPostBack="true" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                        </asp:DropDownList>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Bank">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("BankName") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderTemplate>
                        Bank<br />
                        <asp:DropDownList runat="server" ID="ddlBank" AutoPostBack="true" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged">
                        </asp:DropDownList>
                    </HeaderTemplate>

                </asp:TemplateField>
                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c2}">
                    <ItemStyle HorizontalAlign="Right" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Balance">
              
                    <ItemTemplate>
                        <asp:Label runat="server" ForeColor="Red" Visible="false" ID="lblPending">(PENDING)</asp:Label>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("RunningTotal", "{0:c2}") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
            <SelectedRowStyle BackColor="LemonChiffon"></SelectedRowStyle>
        </asp:GridView>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click"><span style="height: 25px; vertical-align: middle;">
    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span></asp:LinkButton>

<div style="clear: both;">
    <!-- -->
</div>

<script type="text/javascript">

    $('.ttfield').tooltip({
        "placement": "right",
        "trigger": "hover"
    });

</script>
