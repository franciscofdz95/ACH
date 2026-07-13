<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLastTicketsMerchant.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucLastTicketsMerchant" %>

<script type="text/javascript">
    //$(document).ready(function () {
    //    loadTicketLastMerchant();

    //});


    //function loadTicketLastMerchant() {
    //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ticketLastMerchantRequestHandler);
    //}

    //function ticketLastMerchantRequestHandler() {
    //    $('.zeustooltip').tooltip({
    //        content: function () {
    //            return $(this).prop('title');
    //        }
    //    });
    //}
</script>

<fieldset style="width: 320px;">
    <legend>Last Tickets</legend>
    <asp:UpdatePanel ID="pnl" runat="server" OnPreRender="pnl_PreRender" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" ShowHeaderWhenEmpty="true"
                Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                DataKeyNames="TicketUID" AllowPaging="True" OnPageIndexChanging="grd_PageIndexChanging"
                OnRowDataBound="grd_RowDataBound" PageSize="4" DataSourceID="odsTickets">
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnTicket" runat="server" CausesValidation="false" CommandName="View" CssClass="zeustooltip"
                                ></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Width="30px" />
                    </asp:TemplateField>
                    <%-- <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                            <ItemStyle Width="90px" />
                                        </asp:BoundField>--%>
                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Status") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderTemplate>
                            <asp:DropDownList runat="server" ID="ddlHeaderStatus" AutoPostBack="true"  ViewStateMode="Enabled"
                                onselectedindexchanged="ddlHeaderStatus_SelectedIndexChanged">
                                <asp:ListItem Value="">All Statuses</asp:ListItem>
                                <asp:ListItem Value="CDCD0A20-6603-4B07-94DC-F65D01290F6B">Open</asp:ListItem>
                                <asp:ListItem Value="0AEE2CAB-CEC4-476B-9598-918DBABD43CF">Assigned</asp:ListItem>
                                <asp:ListItem Value="DFF04FF8-3C47-45E1-B0BB-30C22C7CAF17">Pending</asp:ListItem>
                                <asp:ListItem Value="F6433994-587E-46B1-9FE6-4FD85E6A7520">Closed</asp:ListItem>
                            </asp:DropDownList>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="40px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                        <ItemStyle Width="100px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                        <ItemStyle Width="60px" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    No tickets found.
                </EmptyDataTemplate>
                <PagerStyle CssClass="pgr" />
            </asp:GridView>
            <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetRecentTicketsPaging"
                TypeName="DataMerchantAppPaging" EnablePaging="true" MaximumRowsParameterName="PageSize"
                SelectCountMethod="GetRecentTicketsPagingCount" StartRowIndexParameterName="CurrentPage"
                OldValuesParameterFormatString="original_{0}" OnSelecting="odsTickets_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                    <asp:Parameter Name="PageSize" Type="Int32" />
                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                    <asp:Parameter Name="ControlID" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            
            <asp:HyperLink runat="server" style="float:right"  ID="hypMore">More</asp:HyperLink>
            <asp:HyperLink ID="btnTicket" CssClass="fakea" runat="server">New Ticket</asp:HyperLink>&nbsp;
            <asp:LinkButton ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" CausesValidation="false">Refresh</asp:LinkButton>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>

<script type="text/javascript">

    // note: bootstrap.js conflicts with the jquery tooltip.
   // there is a bug here. comment it out for now.
   //  ticketLastMerchantRequestHandler();
</script>
