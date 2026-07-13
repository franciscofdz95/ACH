<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="SecureLeadForms_frmLeadQueues" Title="Lead Queues" CodeBehind="frmLeadQueues.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucLeadsQueue.ascx" TagName="wucLeadsQueue" TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucLeadAppointments.ascx" TagName="wucLeadAppointments" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function CloseMCC() {
            oWebDialogWindowAccountgrps = $find('dlgUserFilter');
            oWebDialogWindowAccountgrps.set_windowState($IG.DialogWindowState.Hidden);
        }


    </script>

    <div id="contentpage">
        <fieldset>
            <legend>Status Queue Filters</legend>
            <table>
                <tr> <td style="vertical-align: top;">Select:</td>
                    <td><asp:LinkButton runat="server" ID="btnFilterUsers" OnClick="btnFilterUsers_Click">Filter Users</asp:LinkButton></td>
                    <td>&nbsp;&nbsp;</td>
                    <td style="vertical-align: top;">Source:</td>
                    <td style="vertical-align: top;">
                        <asp:DropDownList ID="LeadSources" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LeadSources_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
        <uc2:wucLeadAppointments ID="WucLeadAppointments" runat="server"></uc2:wucLeadAppointments>
        <uc1:wucLeadsQueue ID="WucSalesAssigned" runat="server" />
        <uc1:wucLeadsQueue ID="WucSalesInCommunication" runat="server" />
        <uc1:wucLeadsQueue ID="WucSalesAppOut" runat="server" />
        <uc1:wucLeadsQueue ID="WucSalesAppSubmitted" runat="server" />
        <uc1:wucLeadsQueue ID="WucSalesStatementsReceived" runat="server" />
        
        <ig:WebDialogWindow ID="dlgUserFilter" runat="server" Height="625px" InitialLocation="Centered" ClientIDMode="Static"
            Modal="True" Width="800px" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <div>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 50%">
                                    <fieldset style="height: 520px;">
                                        <legend>&nbsp;Non Filtered Lead Users</legend>
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
                                        <legend>&nbsp;Filtered Lead Users</legend>
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
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <center>
                        <asp:Button runat="server" Text="Apply" ID="Apply" OnClick="Apply_Click"></asp:Button>
                        <input type="button" value="Close" onclick="CloseMCC()" />
                    </center>
                </Template>
            </ContentPane>
        </ig:WebDialogWindow>
    </div>
</asp:Content>
