<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="true" Inherits="frmRoles" Title="Manage Roles" Codebehind="frmRoles.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                <table width="100%">
                    <tr>
                        <td>
                            <b>Portal:</b>&nbsp;
                            <asp:DropDownList ID="lstPortals" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstPortals_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td>
                            <div class="tbrtools" style="text-align: center; display: none">
                                <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/edit.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp;<igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add"
                                    AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="False">
                                    <Appearance>
                                        <Image Url="~/Images/add2.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp;<igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                    AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp;<igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue_error.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp;<igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/refresh.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                                &nbsp;<igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete"
                                    AccessKey="d" OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="False">
                                    <Appearance>
                                        <Image Url="~/Images/delete2.png" />
                                    </Appearance>
                                    <ClientSideEvents Click="DeleteConfirmation_Click" />
                                </igtxt:WebImageButton>
                            </div>
                        </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td style="width: 150px; text-align: left; vertical-align: top;">
                            <fieldset style="height: 425px; width: 150px">
                                <legend>Roles List</legend>
                                <asp:ListBox ID="lstRoles" runat="server" Height="400px" Width="150px" AutoPostBack="True"
                                    OnSelectedIndexChanged="lstRoles_SelectedIndexChanged"></asp:ListBox>
                            </fieldset>
                        </td>
                        <td style="text-align: left; vertical-align: top;">
                            <fieldset>
                                <legend>Form Permissions</legend>
                                <asp:GridView ID="grdForm2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    AllowSorting="True" PageSize="500" DataKeyNames="RoleFormUID,HasAccess,FormUID"
                                    OnRowDataBound="grdForm2_RowDataBound" OnRowCommand="grdForm2_RowCommand" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Form">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFormID" runat="server">LinkButton</asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                        <asp:TemplateField HeaderText="HasAccess">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkHasAccess" runat="server" AutoPostBack="true" OnCheckedChanged="chkForms_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="RoleFormUID" HeaderText="RoleFormUID" Visible="False" />
                                        <asp:BoundField DataField="FormUID" HeaderText="FormUID" Visible="False" />
                                        <asp:BoundField DataField="ObjectCount" HeaderText="Object Count" SortExpression="ObjectCount" />
                                    </Columns>
                                    <PagerStyle CssClass="pgr" />
                                    <FooterStyle CssClass="footer" />
                                    <AlternatingRowStyle CssClass="alt" />
                                </asp:GridView>
                            </fieldset>
                            <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="500px" Width="600px"
                                InitialLocation="Centered" Modal="True" WindowState="Hidden">
                                <ContentPane>
                                    <Template>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <div class="tabcontent">
                                                    <fieldset>
                                                        <legend>Control Permissions</legend>
                                                        <asp:GridView ID="grdObjects2" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                            AllowSorting="True" PageSize="500" DataKeyNames="RoleObjectUID,Visible,Enabled,FormUID"
                                                            OnRowDataBound="grdObjects2_RowDataBound" Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="Control ID" SortExpression="ID" />
                                                                <asp:BoundField DataField="ItemKey" HeaderText="Item Key" SortExpression="ItemKey" />
                                                                <asp:TemplateField HeaderText="Visible">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkVisible" runat="server" AutoPostBack="true" OnCheckedChanged="chkObjects_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Enabled">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="chkObjects_CheckedChanged" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="RoleObjectUID" HeaderText="RoleObjectUID" Visible="False" />
                                                                <asp:BoundField DataField="ObjectUID" HeaderText="ObjectUID" Visible="False" />
                                                                <asp:BoundField DataField="FormUID" HeaderText="FormUID" Visible="False" />
                                                                <asp:BoundField DataField="ParentUID" HeaderText="ParentUID" Visible="False" />
                                                            </Columns>
                                                            <PagerStyle CssClass="pgr" />
                                                            <FooterStyle CssClass="footer" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                        </asp:GridView>
                                                    </fieldset>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Form Objects">
                                </Header>
                            </ig:WebDialogWindow>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
