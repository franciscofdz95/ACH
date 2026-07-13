<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantCategories" Title="Categories" Codebehind="frmMerchantCategories.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucMerchantCategories.ascx" TagName="wucMerchantCategories"
    TagPrefix="uc2" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Font-Size="10pt" ForeColor="Green"></asp:Label><asp:Label
                        ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="10pt" ForeColor="Red" />
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
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="Server" ID="pnl">
                        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <uc2:wucMerchantCategories ID="wucMerchantCategories1" runat="server" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
