<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" CodeBehind="frmAccountGroupsReport.aspx.cs" Inherits="ZeusWeb.SecureReports.AccountGroupsReport" Title="Account Groups Maintenance" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function CloseMCC() {
            oWebDialogWindowAccountgrps = $find('<% =dlgEditAccountGroups.ClientID %>');
        oWebDialogWindowAccountgrps.set_windowState($IG.DialogWindowState.Hidden);

    }
</script>

    <div>
        <div class="title">
            &nbsp;&nbsp;Account Groups Report
        <hr class="line" />
        </div>
         <uc1:wucMessage ID="WucMessage" runat="server" />
        <div class="indentedcontent20">
            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                <div>
                    <table>
                        <tr>
                            <td class="lblRight">
                                <asp:Label ID="lblAccountGroup" runat="server" Text="Account Group Name:" Width="150px"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAccountGroup" runat="server" Width="400px" MaxLength="40"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">
                                <asp:Label ID="lblAccountGroupDesc" runat="server" Text="Description:" Width="150px"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtAccountGroupDesc" runat="server" Width="400px" TextMode="MultiLine" Height="40px" Rows="3" EnableViewState="False"></asp:TextBox>

                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight"></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <igtxt:WebImageButton ID="btnSearch" runat="server" AccessKey="h" OnClick="btnSearch_Click" Text="Search">
                                                <Appearance>
                                                    <Image Url="~/Images/Check.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnClear" runat="server" AccessKey="l" OnClick="btnClear_Click" Text="Clear">
                                                <Appearance>
                                                    <Image Url="~/Images/delete.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnAdd" runat="server" AccessKey="a"  Text="Add" OnClick="btnAdd_Click" >
                                                <Appearance>
                                                    <Image Url="~/Images/Check.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </div>
            </asp:Panel>
        </div>
    </div>
    <asp:Panel ID="pnlAccountGroups" runat="server" ScrollBars="Auto" Height="700px">
        <div>
            <table width="100%">
                <tr>
                    <td width="50%">
                        <fieldset style="height: 550px;">
                            <legend>&nbsp;<asp:Label runat="server" ID="lblAccountGroups" Text="Account Groups"></asp:Label></legend>
                            <%-- <span>Please select a Account Group to assign to the merchant. Please select and add to the Account groups.</span>--%>
                            <div style="height: 495px; overflow: scroll;overflow-x: hidden">
                                <asp:GridView ID="grdAccountGroupsSearch" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                                    AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                    DataKeyNames="AccountGroupID, AccountGroup,Description,IsDisabled,UserUpdated,DateUpdated" CssClass="mGrid" ShowHeader="true" EmptyDataText="No Account Groups Searched..."
                                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%"
                                    ShowFooter="false" AllowPaging="false" OnRowCommand="grdAccountGroupsSearch_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="5px" HeaderText="Account Group">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="AccountGroup" runat="server" CausesValidation="false" CommandName="View"
                                                    Text='<%#Eval("AccountGroup") %>'></asp:LinkButton>
                                                <asp:HiddenField ID="AccountGroupID" Value='<%# Eval("AccountGroupID") %>' runat="server" />

                                            </ItemTemplate>

                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="IsDisabled" HeaderText="Hide" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="UserUpdated" HeaderText="User Updated" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="DateUpdated" HeaderText="Date Updated" HeaderStyle-Width="150px" />

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
    </asp:Panel>  
    <ig:WebDialogWindow ID="dlgEditAccountGroups" runat="server" Height="250px" InitialLocation="Centered"
        Modal="True" Width="800px" WindowState="Hidden">
        <ContentPane>
            <Template>
                <asp:BulletedList runat="server" ID="blError" CssClass="errorlist"  ForeColor="Red"></asp:BulletedList>
                <asp:Panel ID="pnlEditAccountGroup" runat="server" Height="" Width="">
                    <fieldset>
                        <legend>Account Group</legend>
                    <div>
                        <table>
                            <tr>
                                <td class="lblRight">
                                    <asp:Label ID="lblAccountGroupID" runat="server" Text="Account Group ID:" Width="200px"></asp:Label></td>
                                <td>
                                    <asp:Label ID="txtAccountGroupID" runat="server" Text=""></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    <asp:Label ID="lblEditAccountGroup" runat="server" Text="Account Group Name:" Width="200px"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEditAccountGroup" runat="server" Width="400px" MaxLength="40"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    <asp:Label ID="lblEditAccountGroupDesc" runat="server" Text="Description:" Width="150px"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtEditAccountGroupDesc" runat="server" Width="400px" TextMode="MultiLine" Height="50px" Rows="3" EnableViewState="False" MaxLength="200"></asp:TextBox>

                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">
                                    Hide:
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkEditAccountGroupHide" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight"></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnEditSave" runat="server" Text="Save" OnClick="btnEditSave_Click">
                                                    <Appearance>
                                                        <Image Url="~/Images/Check.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" >
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>

                        </table>
                    </div>
                        </fieldset>
                </asp:Panel>
              
            </Template>
              </ContentPane>
    </ig:WebDialogWindow>
</asp:Content>
