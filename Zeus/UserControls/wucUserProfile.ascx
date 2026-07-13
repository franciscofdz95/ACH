<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucUserProfile.ascx.cs" Inherits="wucUserProfile" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<script type="text/javascript">
    function btnSave_ClientClick(oButton, oEvent) {
        var x = confirm("This change will log you out of Zeus. Please save your changes on all pages before pressing OK.");
        if (!x)
            oEvent.cancel = true;
    }
    </script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" updatemode="Conditional">
    <contenttemplate>
        <asp:Label runat="server" ID="Error" SkinID="Error"></asp:Label>
<br />
        <div class="tbrtools">
            <div class="tbrtoolsleft">
                <table>
                    <tr>
                        <td>
                             <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                AccessKey="e" OnClick="btnEdit_Click" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/edit.png" />
                                </Appearance>
                             </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click"
                                CausesValidation="false" AccessKey="S">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                                <ClientSideEvents Click="btnSave_ClientClick" />
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                                CausesValidation="false" AccessKey="C">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="dialog">
            <asp:Panel runat="server" ID="pnlDetail">
                <fieldset>
                    <legend>User Profile</legend>
                    <table cellpadding="2" cellspacing="2">               
                        <tr>
                            <td align="right">
                                User Name:</td>
                            <td>
                                <asp:Label ID="UserName" runat="server" Text=""></asp:Label></td>
                        </tr>
                         <tr>
                            <td align="right">
                                Office Location:</td>
                            <td>
                                <asp:Label ID="Office" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right">
                                Email:</td>
                            <td>
                                <asp:Label ID="Email" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="right">
                                First Name:</td>
                            <td>

                                <asp:Label ID="FirstName" runat="server"></asp:Label></td>
                        </tr> 
                         <tr>
                            <td align="right">
                                Last Name:</td>
                            <td>
                                <asp:Label ID="LastName" runat="server"></asp:Label></td>
                        </tr> 
                        <tr>
                            <td align="right">
                               Date Pattern:</td>
                            <td>
                                <asp:DropDownList ID="DatePattern" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="right">
                                Time Pattern:</td>
                            <td>
                                <asp:DropDownList ID="TimePattern" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td align="right">
                                Time Zone:</td>
                            <td>
                                <asp:DropDownList ID="TimeZoneID" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="text-align:right;">
                                Office Access:
                            </td>
                            <td>
                                <asp:CheckBoxList ID="lstOfficeAccess" runat="server" RepeatColumns="4"></asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnChngPwd" runat="server" Text="Change Password" OnClick="btnChngPwd_Click" /></td>
                        </tr> 
                    </table>
                    <br />
                </fieldset>
            </asp:Panel>
        </div>
    </contenttemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger controlid="btnSave" eventname="Click" />
            <asp:AsyncPostBackTrigger controlid="btnEdit" eventname="Click" />
            <asp:AsyncPostBackTrigger controlid="btnCancel" eventname="Click" />
        </Triggers>
</asp:UpdatePanel>