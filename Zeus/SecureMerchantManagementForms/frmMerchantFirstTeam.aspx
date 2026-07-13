<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="True"
    Inherits="frmMerchantFirstTeam" Title="Premier Services" CodeBehind="frmMerchantFirstTeam.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucServices.ascx" TagName="wucServices" TagPrefix="uc2" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTGridChangelog.ascx" TagName="wucFTGridChangelog"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTGridHistory.ascx" TagName="wucFTGridHistory"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControls/FirstTeam/wucFTRuleFilter.ascx" TagName="wucFTRuleFilter"
    TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <script src="../js/encoder.js" type="text/javascript"></script>
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
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
                        <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Settings
                            <hr class="line" />
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="FirstTeam" runat="server" />
                                    </td>
                                    <td>
                                        Premier Services Merchant
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PS Rep:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="FirstTeamRepUID" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Additional Options
                            <hr class="line" />
                        </div>
                        <uc2:wucServices ID="WucServices1" runat="server" />
                        <br />
                        <br />
                        <div class="title">
                            &nbsp;&nbsp;Premier Services Rules
                            <hr class="line" />
                        </div>
                         <ig:WebTab ID="WebTab1" runat="server" Width="970px">
                            <Tabs>
                                <ig:ContentTabItem runat="server" Enabled="true" Text="Rules">
                                    <Template>
                                        <div style="padding: 10px;">
                                            <uc3:wucFTRuleFilter ID="wucFTRuleFilter1" runat="server" ShowBusinessDBAName="false"
                                                ShowBusinessLegalName="false" ShowFTRep="false" ShowGlobalEnable="false" ShowZID="false" />
                                        </div>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                            <Tabs>
                                <ig:ContentTabItem runat="server" Text="Changelog">
                                    <Template>
                                        <div style="padding: 10px;">
                                            <uc4:wucFTGridChangelog ID="wucFTGridChangelog1" runat="server" ShowBusinessDBAName="false"
                                                ShowFTRep="false" ShowMerchantID="false" />
                                        </div>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                            <Tabs>
                                <ig:ContentTabItem runat="server" Text="Run History">
                                    <Template>
                                        <div style="padding: 10px;">
                                            <uc5:wucFTGridHistory ID="wucFTGridHistory1" runat="server" />
                                        </div>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                        </ig:WebTab>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="160px" Width="400px"
        Modal="True" InitialLocation="Centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <asp:HiddenField runat="server" ID="hidMerchantID" />
                <asp:HiddenField runat="server" ID="hidMRuleID" />
                <p style="text-align: center">
                    The new Start Date will be
                    <asp:TextBox runat="server" Width="20px" Style="text-align: right; padding: 5px;"
                        ID="tbNewStartDate">7</asp:TextBox>
                    Days from today.
                </p>
                <p style="text-align: center">
                    <asp:Button ID="btnSNOK" runat="server" Text="Ok" OnClick="ButtonOK_Click"></asp:Button>
                    <asp:Button ID="btnSNCancel" runat="server" Text="Cancel" OnClick="ButtonCancel_Click">
                    </asp:Button>
                </p>
            </Template>
        </ContentPane>
        <Header CaptionText="Set Start Date">
        </Header>
    </ig:WebDialogWindow>
    <br />
    <br />
</asp:Content>
