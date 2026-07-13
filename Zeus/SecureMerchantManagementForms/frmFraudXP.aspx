<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmFraudXP" Title="FraudXP" Codebehind="frmFraudXP.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Src="../UserControls/wucTermsConditions.ascx" TagName="wucTermsConditions"
    TagPrefix="uc6" %>
<%@ Register Src="../UserControls/wucWebsiteMonitoring.ascx" TagName="wucWebsiteMonitoring"
    TagPrefix="uc3" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage"><asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td>
                    
                    <asp:Panel ID="pnlTools" runat="server">
                        <div class="tbrtools">
                            <div class="tbrtoolsleft">
                            
<table>
<tr>
        <td><igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/edit.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                                    AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/disk_blue_error.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
    <td><igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                    <Appearance>
                                        <Image Url="~/Images/refresh.png" />
                                    </Appearance>
                                </igtxt:WebImageButton></td>
   
</tr>
</table>
                                
       
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
                    <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    <br />
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                        <div>
                            <div class="indentedcontent20">
                                <a href="#FraudXPStatus">Fraud XP Status</a> | <a href="#DeviceReputation">Device Reputation</a>
                                | <a href="#IPGeolocation">IP Geolocation</a> | <a href="#TermsConditions">Terms & Conditions</a>
                                | <a href="#WebsiteMonitoring">Website Content Monitoring</a>
                            </div>
                            <br />
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;<a name="FraudXPStatus">Fraud XP Status</a>
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <br />
                                <table width="100%" cellpadding="5">
                                    <tr>
                                        <td style="width: 400px">
                                            Fraud XP Enabled
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="FraudXPOn" runat="server" /></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 400px">
                                            Fraud XP was subscribed by
                                        </td>
                                        <td>
                                            <asp:Label ID="FraudXPSubscribedBy" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 400px">
                                            Fraud XP was subscribed on
                                        </td>
                                        <td>
                                            <asp:Label ID="FraudXPSubscriberDate" runat="server" Text=""></asp:Label></td>
                                    </tr>
                                </table>
                            </div>
                            <br />
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;<a name="DeviceReputation">Device Reputation</a>
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <br />
                                                    <b>Note: </b>Device string is required with this option.<br />
                                                    <b>&nbsp;&nbsp;Options:</b><br />
                                                    <asp:RadioButtonList ID="DeviceCheckOptions" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DeviceCheckOptions_SelectedIndexChanged">
                                                        <asp:ListItem Value="N">Disable Device Check</asp:ListItem>
                                                        <asp:ListItem Value="Y">Enable Device Check For ALL Transactions</asp:ListItem>
                                                        <asp:ListItem Value="H">Enable Device Check For High Ticket Items</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:Panel ID="pnlDeviceHighTicketThreshold" runat="server" Visible="false">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Check on Transactions over
                                                                        <asp:TextBox ID="HighTicketDeviceThreshold" runat="server" Width="75px"></asp:TextBox>
                                                                        (US)<asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="High Ticket must be between 0 and 99999"
                                                                            Display="None" ControlToValidate="HighTicketDeviceThreshold" Type="Integer" MinimumValue="0"
                                                                            MaximumValue="99999"></asp:RangeValidator></b></td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </div>
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;<a name="IPGeolocation">IP Geolocation</a>
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <br />
                                <b>Note: </b>This option requires the following fields to be passed in the credit
                                card<br />
                                transaction: Customer IP Address, Billing City/State/ZipCode/Country.
                                <br />
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <b>&nbsp;&nbsp;Options:</b><br />
                                        <asp:RadioButtonList ID="IPGeoCheckOptions" runat="server" AutoPostBack="True" OnSelectedIndexChanged="IPGeoCheckOptions_SelectedIndexChanged">
                                            <asp:ListItem Value="N">Disable IP/Geolocation Check</asp:ListItem>
                                            <asp:ListItem Value="Y">Enable IP/Geolocation Check For ALL Transactions</asp:ListItem>
                                            <asp:ListItem Value="H">Enable IP/Geolocation Check For High Ticket Items</asp:ListItem>
                                        </asp:RadioButtonList>
                                        <asp:Panel ID="pnlIPGeoHighTicketThreshold" runat="server" Visible="false">
                                            <table>
                                                <tr>
                                                    <td>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Check on Transactions over
                                                            <asp:TextBox ID="HighTicketIPGeoThreshold" runat="server" Width="75px"></asp:TextBox>
                                                            (US)<asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="High Ticket must be between 0 and 99999"
                                                                Display="None" ControlToValidate="HighTicketIPGeoThreshold" Type="Integer" MinimumValue="0"
                                                                MaximumValue="99999"></asp:RangeValidator></b></td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                            </div>
                            <br />
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;<a name="TermsConditions">TERMS & CONDITIONS</a>
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                    <ContentTemplate>
                                        <uc6:wucTermsConditions ID="WucTermsConditions1" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <br />
                            <br />
                            <div class="title">
                                &nbsp;&nbsp;<a name="WebsiteMonitoring">WEBSITE CONTENT MONITORING</a>
                                <hr class="line" />
                            </div>
                            <div class="indentedcontent20">
                                <br />
                                <!--PXP-7231(Meritus word replacement with paysafe) By Sanidhya kumar -->
                                <b>Note:</b> "Approved URL" validation is ONLY available when merchant is processing
                                through Paysafe' hosted payment page
                                <br />
                                <uc3:wucWebsiteMonitoring ID="WucWebsiteMonitoring1" runat="server" />
                                <br />
                            </div>
                            <br />
                            <br />
                        </div>
                    </asp:Panel>
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
