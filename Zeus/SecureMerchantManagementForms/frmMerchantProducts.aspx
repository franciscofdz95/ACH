<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    CodeBehind="frmMerchantProducts.aspx.cs" Inherits="frmMerchantProducts" %>

<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucProductSubscription.ascx" TagName="wucProductSubscription" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>

<asp:Content ID="Head1" ContentPlaceHolderID="HeadPlaceHolder" runat="server">
    <script type="text/javascript" language="javascript">
        function isNumber(n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        function validateNumericEntry(n) {
            if (!isNumber(n.value)) {
                n.value = "";
                n.style.border = "1px solid #ff0000";
            }
            else {
                n.style.border = "1px solid #ADAEB1";
            }
        }

        function expandProdDesc() {
            $('.prodDesc').show();
        }

        function collapseProdDesc() {
            $('.prodDesc').hide();
        }
        // **** Functions for DM-3435 ******************* //

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                        <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <fieldset>
                            <legend>Products</legend>
                            <table width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <uc1:wucProductSubscription ID="wucProductSubscription1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

