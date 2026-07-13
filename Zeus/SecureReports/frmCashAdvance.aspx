<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmCashAdvance"
    Title="Merchant Cash Advance" MasterPageFile="~/MasterPageReports.master" Codebehind="frmCashAdvance.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/wucMerchants.ascx" TagName="wucMerchants" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucCashAdvance.ascx" TagName="CashAdvance" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="contentpage">
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        <asp:UpdatePanel ID="upnlMerchants" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Panel ID="pnlTools" runat="server">
                                <div class="tbrtools">
                                    <div class="tbrtoolsleft">
                                        &nbsp;<igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                            AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                            <Appearance>
                                                <Image Url="~/Images/edit.png" />
                                            </Appearance>
                                        </igtxt:WebImageButton>
                                        &nbsp;<igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add"
                                            AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                            <Appearance>
                                                <Image Url="~/Images/add2.png" />
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
                                    </div>
                                   
                                </div>
                            </asp:Panel>
                            <table>
                                <tr>
                                    <td valign="top">
                                        <fieldset class="dialog" style="height: 400px; width: 250px;">
                                            <legend>Select Merchant</legend>
                                            <asp:ListBox runat="server" ID="lstMerchants" AutoPostBack="true" SelectionMode="single"
                                                OnSelectedIndexChanged="lstMerchants_SelectedIndexChanged" Width="100%" Height="95%">
                                            </asp:ListBox>
                                        </fieldset>
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                                            <fieldset class="dialog">
                                                <legend>Merchant Information</legend>
                                                <table border="0" cellspacing="2">
                                                    <tr>
                                                        <td class="lblRight">
                                                            ZID:</td>
                                                        <td>
                                                            <asp:TextBox ID="ID" runat="server" Enabled="False" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10px;">
                                                        </td>
                                                        <td class="lblRight">
                                                            DBA:</td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessDBAName" runat="server" Width="150px" Enabled="False"></asp:TextBox>
                                                            &nbsp;<asp:LinkButton ID="btnMerSelect" runat="server" Text="Select" Visible="false"
                                                                CausesValidation="false" TabIndex="1" OnClick="btnMerSelect_Click" Style="vertical-align: bottom;" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">
                                                            MID:</td>
                                                        <td>
                                                            <asp:TextBox ID="SettlePlatformMid" runat="server" Enabled="False" Width="150px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10px;">
                                                        </td>
                                                        <td class="lblRight">
                                                            Legal Name:</td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessLegalName" runat="server" Enabled="False" Width="150px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <ig:WebDialogWindow ID="dlgcontrol" runat="server" Height="550px" Width="700px" Modal="false"
                                                    InitialLocation="Centered" WindowState="Hidden">
                                                    <ContentPane>
                                                        <Template>
                                                            <uc2:wucMerchants ID="grdMerchants" runat="server" />
                                                        </Template>
                                                    </ContentPane>
                                                    <Header CaptionText="Merchants">
                                                    </Header>
                                                </ig:WebDialogWindow>
                                            </fieldset>
                                            <asp:Panel runat="server" ID="pnlAdvances">
                                                <uc1:CashAdvance runat="server" ID="CashAdvance1" />
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
