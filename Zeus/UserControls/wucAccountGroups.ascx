<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAccountGroups.ascx.cs" Inherits="ZeusWeb.UserControls.wucAccountGroups" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script type="text/javascript">
    function CloseMCC() {
        $('#txtAccountGroupName').val('');
        oWebDialogWindowAccountgrps = $find('<% =dlgAccountGroups.ClientID %>');
        oWebDialogWindowAccountgrps.set_windowState($IG.DialogWindowState.Hidden);
       
}
</script>
<div style="display:inline">
            <asp:BulletedList ID="AccountGroups1" runat="server" Visible="false"></asp:BulletedList>
            <asp:TextBox ID="txtAccountGroups" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
    
    <asp:LinkButton ID="btnSelect" runat="server" Text="Select" OnClick="btnAddAccGroups_Click"></asp:LinkButton>
    
    <ig:WebDialogWindow ID="dlgAccountGroups" runat="server" Height="700px" InitialLocation="Centered"
        Modal="True" Width="800px" WindowState="Hidden">
        <ContentPane>
            <Template>
                <asp:Panel ID="pnlAddContact" runat="server" ScrollBars="None" DefaultButton="btnSearch">

                    <div>
                        <div>
                            <fieldset>
                                <legend>&nbsp;<span>Search Account Groups</span></legend>
                                <table>
                                    <tr>
                                        <td align="right">Account Group Name:
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtAccountGroupName" Width="250px" ClientIDMode="Static"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:Label runat="server" ID="lblMessage" EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </asp:Panel>
               <%-- <asp:UpdatePanel ID="UpdatePanelAccGroups" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <asp:Panel ID="pnlAccountGroups" runat="server"  Height="580px">
                    <div>
                        <table width="100%">
                            <tr>
                                <td width="50%">
                                    <fieldset style="height: 520px;">
                                        <legend>&nbsp;<asp:Label runat="server" ID="lblAccountGroups" Text="Available Account Groups"></asp:Label></legend>
                                        <%-- <span>Please select a Account Group to assign to the merchant. Please select and add to the Account groups.</span>--%>
                                        <div style="height: 495px; overflow-y: scroll;">
                                            <asp:GridView ID="grdAccountGroupsSearch" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                                                AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                                DataKeyNames="ItemValue, ItemText" CssClass="mGrid" ShowHeader="true" EmptyDataText="No Account Groups Searched..."
                                                BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%"
                                                ShowFooter="false" AllowPaging="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderStyle-Width="5px">
                                                        <ItemTemplate>
                                                            <asp:CheckBox runat="server" ID="chkEnabled" />
                                                        <asp:TextBox ID ="AccountGroupID" Visible="false" Text='<%# Eval("ItemValue") %>' runat="server"></asp:TextBox>

                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ItemText" SortExpression="ItemText" HeaderText="Account Group Name" HeaderStyle-Width="150px" />
                                                </Columns>
                                                <PagerStyle CssClass="pgr" />
                                                <FooterStyle CssClass="footer" />
                                                <PagerSettings Mode="NumericFirstLast" />
                                            </asp:GridView>
                                        </div>
                                    </fieldset>
                                </td>
                                <td>
                                    <asp:Button ID="AddAccountGroups" CssClass="button" runat="server" Text=">" OnClick="AddAccountGroups_Click" />
                                    <asp:Button ID="DeleteAccountGroups" CssClass="button" runat="server" Text="<" OnClick="DeleteAccountGroups_Click" />
                                    <br />
                                    <%--<input type="button" value="Cancel" onclick="CloseAGC()" />--%>
                                </td>
                                <td width="50%">
                                    <fieldset style="height: 520px;">
                                        <legend>&nbsp;<asp:Label runat="server" ID="lblSelectedAccGroups" Text="Assigned Account Groups"></asp:Label></legend>
                                        <%--<span>These Account Groups are added</span>--%>
                                        <div style="height: 495px; overflow-y: scroll">
                                        <asp:GridView ID="grdAccountGroupsSelected" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                                            AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                            DataKeyNames="ItemValue, ItemText" CssClass="mGrid" ShowHeader="true" EmptyDataText="No Account Groups Assigned..."
                                            BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%"
                                            ShowFooter="false" >

                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="5px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox runat="server" ID="chkEnabled" />
                                                        <asp:TextBox ID ="AccountGroupID" Visible="false" Text='<%# Eval("ItemValue") %>' runat="server"></asp:TextBox>

                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemText" SortExpression="ItemText" HeaderText="Account Group Name" HeaderStyle-Width="150px" />
                                            </Columns>
                                            <PagerStyle CssClass="pgr" />
                                            <FooterStyle CssClass="footer" />
                                            <PagerSettings Mode="NumericFirstLast" />
                                        </asp:GridView>
                                            </div>

                                    </fieldset>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <center>
            <asp:Button runat="server" Text="Ok" ID ="Ok" OnClick="Ok_Click" ></asp:Button>
            <input type="button" value="Close" onclick="CloseMCC()" />
        </center>
                </asp:Panel>
                       <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                <header captiontext="Account Groups"></header>
            </Template>
        </ContentPane>
    </ig:WebDialogWindow>
</div>



