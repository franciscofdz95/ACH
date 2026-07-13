<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMerchantAlerts.aspx.cs"
    Inherits="frmMerchantAlerts" Title="Merchant Alerts" MasterPageFile="~/MasterPageMerchant.master" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="~/UserControls/wucAlertsnCategories.ascx" TagName="Alerts" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>            
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlTools" runat="server">
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
        </asp:Panel>
        <uc1:Alerts runat="server" ID="wucAlerts1" />
    </div>
</asp:Content>
