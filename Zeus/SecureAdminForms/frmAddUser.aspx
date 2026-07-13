<%@ Page Language="C#" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="true" Inherits="frmAddUser" Title="User Profile" CodeBehind="frmAddUser.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.NavigationControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucSelectBank.ascx" TagName="wucSelectBank" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucSelectAgent.ascx" TagName="wucSelectAgent" TagPrefix="uc2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucSelectMerchant.ascx" TagName="wucSelectMerchant"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageAdmin.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
    <asp:TextBox ID="txtCustomError" Width="1145px" Height="15px" Font-Bold="true" ForeColor="Red" runat="server" MaxLength="500" Visible="false"></asp:TextBox>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td valign="top" style="width: 50%">
                    <fieldset style="height: 220px">
                        <legend>User List</legend>                      
                        <ig:WebDataTree ID="uwtUsers" runat="server" Height="200px" Width="100%"
                            SelectionType="Single" OnNodeClick="uwtUsers_NodeClick">
                            <AutoPostBackFlags NodeClick="On" />
                        </ig:WebDataTree>
                    </fieldset>
                </td>
                <td valign="top">
                    <fieldset style="height: 220px">
                        <legend>Search Parameters</legend>
                        <asp:Panel ID="pnlSearchParameters" runat="server" Height="" Width="">
                            <center>
                                <br />
                                <table border="0" cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td class="lblRight">User Type:</td>
                                        <td>
                                            <asp:DropDownList ID="cboHookTables" runat="server" Width="180px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">User Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtUserName" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">DBA:</td>
                                        <td>
                                            <asp:TextBox ID="txtDBA" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">First Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="txtLastName" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight"></td>
                                        <td></td>
                                    </tr>
                                </table>
                                <div>
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <igtxt:WebImageButton ID="btnSearch" runat="server" AccessKey="h" OnClick="btnSearch_Click"
                                                        Text="Search">
                                                        <Appearance>
                                                            <Image Url="~/Images/Check.png" />
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                </td>
                                                <td>
                                                    <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                                        AccessKey="l">
                                                        <Appearance>
                                                            <Image Url="~/Images/delete.png" />
                                                        </Appearance>
                                                    </igtxt:WebImageButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </div>
                            </center>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
        <br />
        <center>
            <asp:Panel ID="pnlTools" runat="server" Height="" Width="">
                <div class="tbrtools">
                    <table>
                        <tr>
                            <td>
                                <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/edit.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/add2.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                    CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue_error.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/refresh.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnSendAccount" runat="server" Text="Email Account" CommandName="EmailAccount"
                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="false">
                                    <Appearance>
                                        <Image Url="~/Images/mail2.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            
                            <%--<td>
                                <igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete"
                                    AccessKey="d" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/delete2.png" />
                                    </Appearance>
                                    <ClientSideEvents Click="DeleteConfirmation_Click" />
                                </igtxt:WebImageButton>
                            </td>--%>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
        </center>
        <asp:Panel ID="pnlDetail" runat="server" Width="100%" Enabled="false">
            <%--            <asp:UpdatePanel runat="server" ID="RefreshPanel1">
                <ContentTemplate>--%>
            <table width="100%">
                <tr>
                    <td valign="top" style="width: 35%">
                        <fieldset style="height: 400px">
                            <legend>User Information</legend>
                            <table cellspacing="2">
                                <tbody>
                                    <tr>
                                        <td class="lblRight">Parent User:</td>
                                        <td>
                                            <asp:Label ID="ParentUserFullName" runat="server"></asp:Label>
                                            <asp:Label ID="ParentUID" runat="server" Visible="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Active:</td>
                                        <td>
                                            <asp:CheckBox ID="HasDBAccess" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight"></td>
                                        <td>
                                            <asp:HiddenField ID="HookTableKeyUID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Type Name:</td>
                                        <td>
                                            <asp:DropDownList ID="HookTableUID" runat="server" Width="180px" AutoPostBack="true" OnSelectedIndexChanged="HookTableUID_SelectedIndexChanged">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">
                                            <asp:LinkButton ID="lnkLookup" runat="server" OnClientClick="ShowHookTable();" CausesValidation="False">Type Lookup:</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Panel ID="Panel2" runat="server" Height="" Width="">
                                                <asp:TextBox ID="HookTableKeyID" runat="server" Width="65px" Enabled ="false"></asp:TextBox>
                                                <asp:TextBox ID="DBA" runat="server" Width="101px" Enabled ="false"></asp:TextBox>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">User Name:</td>
                                        <td>
                                            <asp:TextBox ID="UserName" runat="server" Width="175px" Enabled="false"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Password:</td>
                                        <td>
                                            <asp:Label ID="Password" runat="server" Text=""></asp:Label>
                                            <asp:TextBox runat="server" ID="txtPassword" Visible="false" ReadOnly="true" Width="175px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">First Name:</td>
                                        <td>
                                            <asp:TextBox ID="FirstName" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Last Name:</td>
                                        <td>
                                            <asp:TextBox ID="LastName" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight" colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Email:</td>
                                        <td>
                                            <asp:TextBox ID="Email" runat="server" Width="175px"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Main Dept:</td>
                                        <td>
                                            <asp:DropDownList ID="DefaultRoleUID" runat="server" Width="180px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Access Level:</td>
                                        <td>
                                            <asp:DropDownList ID="AccessLevelUID" runat="server" Width="180px">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Primary Office:</td>
                                        <td>
                                            <asp:DropDownList ID="OfficeID" runat="server" Width="180px">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr runat="server" visible="false">
                                        <td class="lblRight">Internal Account:</td>
                                        <td>
                                            <asp:CheckBox ID="InternalAccountAccess" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Next Pwd Change Date:</td>
                                        <td>
                                           <ig:WebDatePicker ID="ChangePwdDate" runat="server" NullText="" Width="205px" ReadOnly="true">
                                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                SlideOpenDuration="1" />
                                                        </ig:WebDatePicker>
                                            <%--<asp:Label ID="ChangePwdDate" runat="server" Width="175px"></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Last Pwd Change Date :</td>
                                        <td>
                                            <asp:Label ID="LastPwdChangeDate" runat="server" Width="175px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">Failed Login Attempts:</td>
                                        <td>
                                            <%--<asp:Label ID="LoginAttempts" runat="server" Width="175px"></asp:Label></td>--%>
                                         <asp:TextBox runat="server" ID="LoginAttempts"  Width="175px" ReadOnly="true"></asp:TextBox>
                                    </tr>
                                    <tr runat="server" id="rowCopyUser">
                                        <td class="lblRight">Copy From User</td>
                                        <td>
                                             <asp:DropDownList ID="UserID" runat="server" Width="175px" TabIndex="113"                                               >
                                            </asp:DropDownList>
                                           <%-- <asp:TextBox ID="CopyUser" runat="server" Width="100px"></asp:TextBox><asp:LinkButton runat="server" Text="Select" ID="selectuser" OnClick="selectuser_Click"></asp:LinkButton>--%>
                                        </td>
                                    </tr>
                                    
                                </tbody>
                            </table>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 34%">
                        <fieldset style="height: 420px">
                            <legend>Roles</legend>
                            <asp:Panel ID="pnlRoles" runat="server" Width="100%" Enabled="false">
                                <asp:CheckBoxList ID="lstUserRoles" runat="server" RepeatColumns="2" Width="100%">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </fieldset>
                    </td>
                    <td valign="top" style="width: 27%">

                        <fieldset style="height: 100px">
                            <legend>Portals</legend>
                            <asp:Panel ID="pnlPortals" runat="server" Width="100%" Enabled="false">
                                <asp:CheckBoxList ID="lstUserPortals" runat="server" RepeatColumns="2" Width="100%">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </fieldset>

                        <asp:Panel runat="server" ID="pnlPartnerChannel" Width="100%" Visible="false" Enabled="false">
                            <fieldset style="height: 185px">
                                <legend>Agent Channels</legend>
                                <asp:CheckBoxList ID="lstChannelPartners" runat="server" RepeatColumns="1">
                                </asp:CheckBoxList>
                            </fieldset>
                        </asp:Panel>

                        <fieldset style="height: 100px">
                            <legend>Office Access</legend>
                            <asp:Panel ID="pnlOfficeAccess" runat="server" Width="100%" Enabled="false">
                                <asp:CheckBoxList ID="lstOfficeAccess" runat="server" RepeatColumns="2" Width="100%">
                                </asp:CheckBoxList>
                            </asp:Panel>
                        </fieldset>
                    </td>
                </tr>
            </table>
            <%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
            <br />
            <div class="tabcontent">
                <b>&nbsp;&nbsp;Portals</b>
                <asp:DropDownList ID="lstPortals" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstPortals_SelectedIndexChanged"></asp:DropDownList>
                    <b>&nbsp;&nbsp;Roles</b>
                <asp:DropDownList ID="lstRoles" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstRoles_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:UpdatePanel ID="upnlForms" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Form Permissions</legend>
                            <asp:GridView ID="grdForms" Enabled="False" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                AllowSorting="True" PageSize="500" DataKeyNames="UserRoleFormUID,UserUID,FormUID,RoleUID" OnRowDataBound="grdForms_RowDataBound" OnRowCommand="grdForms_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="PortalName" HeaderText="Portal" SortExpression="PortalName" />
                                    <asp:BoundField DataField="RoleName" HeaderText="Role" SortExpression="RoleName" />
                                    <asp:TemplateField HeaderText="Form ID" SortExpression="FormID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFormID" runat="server">LinkButton</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="FormID" HeaderText="Form ID" SortExpression="FormID" />--%>
                                    <asp:BoundField DataField="FormDesc" HeaderText="Form Name" SortExpression="FormDesc" />
                                    <asp:BoundField DataField="ObjectCount" HeaderText="Object Cnt" />
                                    <asp:TemplateField HeaderText="Enabled">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="chkForms_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserRoleFormUID" HeaderText="UserRoleFormUID" Visible="False" />
                                    <asp:BoundField DataField="UserUID" HeaderText="UserUID" Visible="False" />
                                    <asp:BoundField DataField="RoleUID" HeaderText="RoleUID" Visible="False" />
                                    <asp:BoundField DataField="FormUID" HeaderText="FormUID" Visible="False" />
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <FooterStyle CssClass="footer" />
                                <AlternatingRowStyle CssClass="alt" />
                            </asp:GridView>
                            <asp:LinkButton ID="lnkFrmEnableAll" runat="server" OnClick="lnkFrmEnableAll_Click"
                                Enabled="False">Enable All</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkFrmDisableAll" runat="server" OnClick="lnkFrmDisableAll_Click"
                                Enabled="False">Disable All</asp:LinkButton>
                        </fieldset>
                        <ig:WebDialogWindow ID="dlgObjects" runat="server" Height="500px" Width="600px"
                            InitialLocation="Centered" Modal="True" WindowState="Hidden">
                            <ContentPane>
                                <Template>
                                    <div class="tabcontent">
                                        <fieldset>
                                            <legend>Control Permissions</legend>
                                            <asp:GridView ID="grdObjects" Enabled="False" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                AllowSorting="True" PageSize="500" DataKeyNames="UserRoleObjectUID,UserUID,FormRefUID,RoleUID,ObjectUID"
                                                OnRowDataBound="grdObjects2_RowDataBound">
                                                <Columns>
                                                    <asp:BoundField DataField="PortalName" HeaderText="Portal" SortExpression="PortalName" />
                                                    <asp:BoundField DataField="RoleName" HeaderText="Role" SortExpression="RoleName" />


                                                    <asp:BoundField DataField="FormID" HeaderText="Form ID" SortExpression="FormID" />
                                                    <asp:BoundField DataField="FormDesc" HeaderText="Form Name" SortExpression="FormDesc" />
                                                    <asp:BoundField DataField="FormRef" HeaderText="Form Ref" SortExpression="FormRef" />
                                                    <asp:BoundField DataField="ControlDesc" HeaderText="Control Name" SortExpression="ControlDesc" />
                                                    <asp:TemplateField HeaderText="Visible?">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkVisible" runat="server" AutoPostBack="true" OnCheckedChanged="chkObjects_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Enable?">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkEnabled" runat="server" AutoPostBack="true" OnCheckedChanged="chkObjects_CheckedChanged" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="UserRoleObjectUID" HeaderText="UserRoleObjectUID" Visible="False" />
                                                    <asp:BoundField DataField="UserUID" HeaderText="UserUID" Visible="False" />
                                                    <asp:BoundField DataField="RoleUID" HeaderText="RoleUID" Visible="False" />
                                                    <asp:BoundField DataField="FormUID" HeaderText="FormUID" Visible="False" />
                                                    <asp:BoundField DataField="ObjectUID" HeaderText="ObjectUID" Visible="False" />
                                                    <asp:BoundField DataField="ParentUID" HeaderText="ParentUID" Visible="False" />
                                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                                                    <asp:BoundField DataField="FormRefUID" HeaderText="FormRefUID" Visible="False" />
                                                    <asp:BoundField DataField="ItemKey" HeaderText="ItemKey" Visible="False" />
                                                </Columns>
                                                <PagerStyle CssClass="pgr" />
                                                <FooterStyle CssClass="footer" />
                                                <AlternatingRowStyle CssClass="alt" />
                                            </asp:GridView>
                                            <asp:LinkButton ID="lnkObjEnableAll" runat="server" OnClick="lnkObjEnableAll_Click"
                                                Enabled="False">Enable All</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="lnkObjDisableAll" runat="server" OnClick="lnkObjDisableAll_Click"
                                Enabled="False">Disable All</asp:LinkButton>
                                        </fieldset>
                                    </div>
                                </Template>
                            </ContentPane>
                            <Header CaptionText="Form Objects">
                            </Header>
                        </ig:WebDialogWindow>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upnlObjects" runat="server">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:Panel>
        <br />
        <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="375px" Width="500px"
            Modal="True" InitialLocation="Centered" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <uc1:wucSelectMerchant ID="WucSelectMerchant1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Template>
            </ContentPane>
            <Header CaptionText="Merchant Search">
            </Header>
        </ig:WebDialogWindow>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="375px" Width="500px"
            Modal="True" InitialLocation="Centered" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <uc2:wucSelectAgent ID="WucSelectAgent1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Template>
            </ContentPane>
            <Header CaptionText="Agent Search">
            </Header>
        </ig:WebDialogWindow>
        <ig:WebDialogWindow ID="WebDialogWindow3" runat="server" Height="375px" Width="500px"
            Modal="True" InitialLocation="Centered" WindowState="hidden">
            <ContentPane>
                <Template>
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <uc3:wucSelectBank ID="WucSelectBank1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Template>
            </ContentPane>
            <Header CaptionText="Bank Search">
            </Header>
        </ig:WebDialogWindow>
    </div>


    <script type="text/javascript">
        function Field2Str(fieldvalue) {
            if (fieldvalue == null)
                return '';
            else
                return fieldvalue;
        }

        function ShowHookTable() {
            switch (document.getElementById('<% =HookTableUID.ClientID %>').value.toUpperCase()) {
                case '4CB95A71-7DD1-43F3-8F97-9BD15BB04834': //Agent
                    oWebDialogWindow2 = $find('<% =WebDialogWindow1.ClientID %>');
                    oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
                    break;
                case 'E65F8292-7091-4EDD-A96D-378F8EA8567C': //Internal

                    break;
                case '904683F4-094B-4BDA-AEF2-1BD7931C77D0': //Merchant
                    oWebDialogWindow2 = $find('<% =WebDialogWindow2.ClientID %>');
                    oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
                    break;
                case 'C20FD654-4EE5-4B35-9F35-62628287195D': //Bank
                    oWebDialogWindow2 = $find('<% =WebDialogWindow3.ClientID %>');
                    oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
                    break;
                default:
            }
            return false;
        }
    </script>

</asp:Content>
