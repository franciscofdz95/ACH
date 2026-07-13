<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucFTGridTicket.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucFTGridTicket" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>


            
<table width="100%">
    <tr>
        <td class="lblLeft" style="vertical-align: bottom">
            Page Size:
            <asp:DropDownList ID="cboPageSize2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize2_SelectedIndexChanged">
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
                <asp:ListItem>250</asp:ListItem>
                <asp:ListItem>500</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="lblRight">
            Ticket count:
            <asp:Label ID="lblRecordCount" runat="server" Text="">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="grdTickets" runat="server" AutoGenerateColumns="False" AllowPaging="True"
    OnRowCommand="grdTickets_RowCommand" OnPageIndexChanging="grdTickets_PageIndexChanging"
    OnDataBound="grdTickets_DataBound" AllowSorting="True" Font-Names="Verdana" Font-Size="X-Small"
    CssClass="mGrid" PageSize="5" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    DataKeyNames="TicketUID" OnRowDataBound="grdTickets_RowDataBound" DataSourceID="ods"
    EnableModelValidation="True">
    <EmptyDataTemplate>
        No tickets match that criteria.
    </EmptyDataTemplate>
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
    
        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
            <ItemStyle Width="40px" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="TicketID" SortExpression="TicketID">
            <ItemTemplate>
                <asp:LinkButton ID="lbtnTicketID" runat="server" CommandName="View"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Width="40px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBA Name" SortExpression="DBAName">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypDBAName" Target="_blank" Text='<%# Bind("DBAName") %>'
                    NavigateUrl='<%# "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantUID")  %>'></asp:HyperLink>
            </ItemTemplate>
            <ItemStyle Width="80px" />
        </asp:TemplateField>
        <asp:BoundField DataField="AgentFullName" HeaderText="Agent DBA" SortExpression="AgentFullName">
            <ItemStyle Width="80px" />
        </asp:BoundField>
        <asp:TemplateField ItemStyle-Width="200px" HeaderText="Subject" SortExpression="Problem">
            <HeaderTemplate>
                Subject
                <span class="headmoreless fakea" onclick="ToggleHeadMoreLess(this, event, '<%= grdTickets.ClientID %>')">More</span>
            </HeaderTemplate>
            <ItemTemplate>
                <p style="padding:0;margin:0;" class="minimize"><asp:Literal ID="litProb" runat="server" Text='<%# Bind("Problem") %>'></asp:Literal></p>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department">
            <ItemStyle Width="60px" />
        </asp:BoundField>
        <asp:BoundField DataField="UserName" HeaderText="Assign To" SortExpression="UserName">
            <ItemStyle Width="60px" />
        </asp:BoundField>
        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority">
            <ItemStyle Width="60px" />
        </asp:BoundField>
        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy HH:mm tt}"
            SortExpression="DateCreated">
            <ItemStyle Width="60px" />
        </asp:BoundField>
        <asp:BoundField DataField="DateModified" HeaderText="Date Updated" DataFormatString="{0:MM-dd-yy HH:mm tt}"
            SortExpression="DateModified">
            <ItemStyle Width="60px" />
        </asp:BoundField>
         <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
            SortExpression="DueDate">
            <ItemStyle Width="60px" />
        </asp:BoundField>
         <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
            <ItemStyle Width="60px" />
        </asp:BoundField>
        <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
    </Columns>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
</asp:GridView>
<asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetTicketsPaging" EnablePaging="True"
    MaximumRowsParameterName="PageSize" SelectCountMethod="GetTicketsPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="odsTickets_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
        <asp:Parameter Name="ControlID" Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
