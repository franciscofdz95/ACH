<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantOwners" Title="Merchant Owners" CodeBehind="frmMerchantOwners.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc5" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucTradeReference.ascx" TagName="wucTradeReference"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucOwner.ascx" TagName="wucOwner" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucCorporateBusiness.ascx" TagName="wucCorporateBusiness" TagPrefix="ucCorpBuz" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
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
                        <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlOwners"></asp:Panel>
                    <asp:Panel runat="server" ID="pnlCorp">
                        <ucCorpBuz:wucCorporateBusiness ID="wucCorpBuz1" runat="server" />
                        <uc3:wucTradeReference ID="WucTradeReference0" runat="server" />
                        <uc3:wucTradeReference ID="WucTradeReference1" runat="server" />
                        <uc3:wucTradeReference ID="WucTradeReference2" runat="server" />
                    </asp:Panel>
                    <br />
                    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="250px" Width="400px"
                        Modal="true" InitialLocation="centered" WindowState="Hidden" Moveable="false">
                        <ContentPane>
                            <Template>
                                <uc5:wuConfirm runat="server" ID="confirm" />
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Confirm Values" CloseBox-Visible="false">
                        </Header>
                    </ig:WebDialogWindow>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
