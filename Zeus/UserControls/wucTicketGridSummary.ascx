<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucTicketGridSummary.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucTicketGridSummary" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script type="text/javascript">
    function CloseMCC() {
        oWebDialogWindowAccountgrps = $find('dlgUserFilter');
        oWebDialogWindowAccountgrps.set_windowState($IG.DialogWindowState.Hidden);
    }


</script>

<fieldset>
    <legend>Department Ticket Summary</legend>
    <asp:LinkButton runat="server" ID="btnFilterUsers" OnClick="btnFilterUsers_Click">Filter Users</asp:LinkButton>
    <asp:GridView ID="grdSummary" runat="server" OnRowCommand="grdSummary_RowCommand"
        OnRowDataBound="grdSummary_RowDataBound" AutoGenerateColumns="False" Font-Names="Verdana"
        Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsSummary" AlternatingRowStyle-CssClass="alt"
        AllowSorting="true" AllowPaging="false" DataKeyNames="UserName">
        <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField DataField="FullName" HeaderText="Name" SortExpression="FullName">
                <ItemStyle Width="40px" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="Working\Assigned" SortExpression="TicketCount" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="TicketCount" CommandName="TicketCount" runat="server" Text='<%#Eval("TicketCount") %>'
                        CommandArgument='<%#Eval("UserUID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Near Due Date" SortExpression="NearSLA" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="NearSLA" CommandName="NearSLA" runat="server" Text='<%#Eval("NearSLA") %>'
                        CommandArgument='<%#Eval("UserUID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Past Due Date" SortExpression="PastSLA" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="PastSLA" CommandName="PastSLA" runat="server" Text='<%#Eval("PastSLA") %>'
                        CommandArgument='<%#Eval("UserUID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Assigned Out" SortExpression="AssignedOut" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="AssignedOut" CommandName="AssignedOut" runat="server" Text='<%#Eval("AssignedOut") %>'
                        CommandArgument='<%#Eval("UserUID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px"></ItemStyle>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Recently Closed" SortExpression="RecentlyClosed" ItemStyle-Width="40px">
                <ItemTemplate>
                    <asp:LinkButton ID="RecentlyClosed" CommandName="RecentlyClosed" runat="server" Text='<%#Eval("RecentlyClosed") %>'
                        CommandArgument='<%#Eval("UserUID") %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Width="40px"></ItemStyle>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Longest Aged" SortExpression="LongestAged" DataField="LongestAged">
                <ItemStyle Width="40px"></ItemStyle>
            </asp:BoundField>
        </Columns>
        <EmptyDataTemplate>
            Nobody is in your department.
        </EmptyDataTemplate>
        <PagerStyle CssClass="pgr"></PagerStyle>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsSummary" runat="server" SelectMethod="GetTicketSummary"
        TypeName="PaymentXP.Dataobjects.DataTicket" OldValuesParameterFormatString="original_{0}"
        OnSelecting="odsSummary_Selecting">
        <SelectParameters>
            <asp:Parameter Name="prms" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
    
</fieldset>
<br />
<asp:Panel runat="server" ID="pnlTickets" Visible="false">
    <fieldset>
        <legend>
            <asp:Label runat="server" ID="lblText"></asp:Label></legend>
        <table style="width: 100%">
            <tr>
                <td class="lblLeft" style="vertical-align: bottom">Page Size:
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
                <td class="lblRight">Ticket count:
                    <asp:Label ID="lblRecordCount" runat="server" Text="">0</asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="grdTickets" runat="server" AutoGenerateColumns="False" AllowPaging="True"
            OnPageIndexChanging="grdTickets_PageIndexChanging"
            AllowSorting="True" Font-Names="Verdana" Font-Size="X-Small"
            CssClass="mGrid" PageSize="5" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="TicketUID" OnRowDataBound="grdTickets_RowDataBound" OnSorting="grdTickets_Sorting"
            EnableModelValidation="True">
            <EmptyDataTemplate>
                No tickets match that criteria.
            </EmptyDataTemplate>
            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
            <Columns>
                <asp:TemplateField SortExpression="PriorityID">
                    <ItemTemplate></ItemTemplate>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkSort" runat="server" CommandName="Sort" CommandArgument="PriorityID">
                            <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/sortboth.png" BorderStyle="None" Width="7px" />
                        </asp:LinkButton>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnTicketID" runat="server" CssClass="zeustooltip" CommandName="View"></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Width="30px" />
                </asp:TemplateField>
                <%--DM-1759--%>
                <asp:BoundField DataField="MLE" HeaderText="MLE" SortExpression="MLE" ItemStyle-Width="30px"/>
                <%--DM-1759--%>
                <asp:TemplateField HeaderText="DBA Name" SortExpression="DBAName">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hypDBAName" Target="_blank" Text='<%# Bind("DBAName") %>'
                            NavigateUrl='<%# "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantUID")  %>'></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width="80px" />
                </asp:TemplateField>
                <asp:BoundField DataField="AgentFullName" HeaderText="Agent Name" SortExpression="AgentFullName">
                    <ItemStyle Width="80px" />
                </asp:BoundField>

                <asp:TemplateField ItemStyle-Width="200px" HeaderText="Issue" SortExpression="Problem">
                    <HeaderTemplate>
                        Issue
                        <span class="headmoreless fakea" onclick="ToggleHeadMoreLess(this, event, '<%= grdTickets.ClientID %>')">More</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <p style="margin: 0; padding: 0;" class="minimize">
                            <asp:Literal ID="lblProblem" runat="server" Text='<%# Bind("Problem") %>' Mode="Encode"></asp:Literal>
                        </p>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="Category" HeaderText="Sub-Category" SortExpression="Category">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
<%--                <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department">
                    <ItemStyle Width="60px" />
                </asp:BoundField>--%>
                <%--<asp:BoundField DataField="UserName" HeaderText="Assign To" SortExpression="UserName">
                    <ItemStyle Width="60px" />
                </asp:BoundField>--%>
                <asp:TemplateField HeaderText="Priority" Visible="false" SortExpression="Priority">
                    <ItemTemplate>
                        <asp:Label ID="lblPriority" Text='<%# Bind("Priority") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" />
                </asp:TemplateField>
                <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DateCreated">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DueDate">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="Days" HeaderText="Days Aged" SortExpression="Days">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="DateModified" HeaderText="Date Updated" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DateModified">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetTicketSummaryPaging" EnablePaging="True"
            MaximumRowsParameterName="PageSize" SelectCountMethod="GetTicketSummaryPagingCount"
            StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
            OnSelecting="odsTickets_Selecting" TypeName="DataMerchantAppPaging">
            <SelectParameters>
                <asp:Parameter Name="prms" Type="Object" />
                <asp:Parameter Name="PageSize" Type="Int32" />
                <asp:Parameter Name="CurrentPage" Type="Int32" />
                <asp:Parameter Name="ControlID" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </fieldset>
</asp:Panel>



<ig:WebDialogWindow ID="dlgUserFilter" runat="server" Height="625px" InitialLocation="Centered" ClientIDMode="Static"
    Modal="True" Width="800px" WindowState="Hidden">
    <ContentPane>
        <Template>
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <fieldset style="height: 520px;">
                                <legend>&nbsp;Non Filtered Ticket Users</legend>
                                <div style="height: 495px; overflow-y: scroll;">
                                    <asp:GridView ID="grdNonFilteredUser" AutoGenerateColumns="false" runat="server"
                                        AlternatingRowStyle-CssClass="alt" HorizontalAlign="left" 
                                        CssClass="mGrid" ShowHeader="true" EmptyDataText="No Users to Filter"
                                        BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="325px"
                                        Style="table-layout: fixed; word-wrap: break-word;">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkEnabled" />
                                                    <asp:HiddenField runat="server" ID="UserID" Value='<%#Eval("UserID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserName" HeaderText="Login" HeaderStyle-Width="75px" />
                                            <asp:BoundField DataField="FullName" HeaderText="Name" HeaderStyle-Width="75px" />
                                            <asp:BoundField DataField="AssignedRoles" HeaderText="Role(s)" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </td>
                        <td>
                            <asp:Button ID="AddUserTicketFilter" CssClass="button" runat="server" Text=">" OnClick="AddUserTicketFilter_Click" />
                            <br />
                            <br />
                            <asp:Button ID="DeleteUserTicketFilter" CssClass="button" runat="server" Text="<" OnClick="DeleteUserTicketFilter_Click" />
                            <br />
                        </td>
                        <td style="width: 50%">
                            <fieldset style="height: 520px;">
                                <legend>&nbsp;Filtered Ticket Users</legend>
                                <div style="height: 495px; overflow-y: scroll">
                                    <asp:GridView ID="grdFilteredUsers" AutoGenerateColumns="false" runat="server"
                                        AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                        CssClass="mGrid" ShowHeader="true" EmptyDataText="No User Filters applied"
                                        BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="325px"
                                        Style="table-layout: fixed; word-wrap: break-word;">

                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20px">
                                                <ItemTemplate>
                                                    <asp:CheckBox runat="server" ID="chkEnabled" />
                                                    <asp:HiddenField runat="server" ID="UserID" Value='<%#Eval("UserID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserName" HeaderText="Login" HeaderStyle-Width="75px" />
                                            <asp:BoundField DataField="FullName" HeaderText="Name" HeaderStyle-Width="75px" />
                                            <asp:BoundField DataField="AssignedRoles" HeaderText="Role(s)" />
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </fieldset>
                        </td>
                    </tr>
                </table>

            </div>
            <center>
            <asp:Button runat="server" Text="Apply" ID ="Apply" OnClick="Apply_Click" ></asp:Button>
            <input type="button" value="Close" onclick="CloseMCC()" />
        </center>
        </Template>
    </ContentPane>
</ig:WebDialogWindow>



