<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true" Inherits="frmMerchantCollections" Title="Collections" CodeBehind="frmMerchantCollections.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <table width="100%">
            <tr>
                <td>                    
                    
                    <uc1:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                    <fieldset>
                        <legend>Collections Email</legend>
                        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">                           
                            <div class="indentedcontent20">
                                <table cellpadding="0" cellspacing="5">
                                    <tr>
                                        <td class="lblRight"><asp:CheckBox ID="chkPrimaryContactEmail" runat="server" Text="Primary Contact Email:"/></td>
                                        <td>                                            
                                            <asp:Label runat="server" ID="Email" Width="200px"></asp:Label>
                                        </td>
                                        <td colspan="2"></td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight"><asp:CheckBox ID="chkSecondaryEmail" runat="server" Text="Secondary Email:"/></td>
                                        <td>
                                           <asp:TextBox runat="server" ID="SecondEmail" Width="200px"></asp:TextBox>
                                        </td>
                                        <td class="lblRight">
                                            Due Amount:
                                        </td>
                                        <td>
                                            $<asp:TextBox runat="server" ID="Amount" Width="200px"></asp:TextBox>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="lblRight">&nbsp;
                                        </td>
                                        <td colspan="3"> <asp:Button runat="server" ID="btnSend" OnClick="btnSend_Click" Text="Send Email" /> </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>