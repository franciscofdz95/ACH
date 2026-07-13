<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmAgentAlerts"
    MasterPageFile="~/MasterPageAgent.master" Codebehind="frmAgentAlerts.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAlertsnCategories.ascx" TagName="Alerts" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="tbrtools">
            <div class="tbrtoolsleft">
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
                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" CommandName="Save"
                                AccessKey="s" OnClick="tbrTools_ButtonClicked">
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
                    </tr>
                </table>
            </div>
           
        </div>
        <uc1:Alerts runat="server" ID="wucAlerts1" />
    </div>
</asp:Content>
