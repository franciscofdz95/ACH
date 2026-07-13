<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucTradeReference" Codebehind="wucTradeReference.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="" Enabled="true"></asp:Label></legend>
    <asp:Panel runat="server" ID="pnl">
        <table width="100%">
            <tr>
                <td class="lblRight">
                    Vendor Name:</td>
                <td>
                    <asp:TextBox ID="VendorName" runat="server" MaxLength="50"></asp:TextBox>
                    <asp:TextBox ID="TradeReferenceID" runat="server" Visible="False"></asp:TextBox></td>
                <%--<td class="lblRight">
                    Account Number:</td>
                <td>
                    <asp:TextBox ID="AccountNumber" runat="server" MaxLength="50"></asp:TextBox></td>--%>
                <td class="lblRight">
                    Address:</td>
                <td>
                    <asp:TextBox ID="Address" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                <td class="lblRight">
                    Contact:</td>
                <td>
                    <asp:TextBox ID="ContactName" runat="server" MaxLength="50"></asp:TextBox></td>
                <td class="lblRight">
                    Phone:</td>
                <td>
                    <asp:DropDownList ID="PhoneNumberCountryCode" runat="server" Width="45px">
                    </asp:DropDownList>
                    <asp:TextBox ID="PhoneNumberCountryCodeDisplay" runat="server" Width="40px"></asp:TextBox>
                    <ig:WebMaskEditor ID="PhoneNumber" runat="server" InputMask="############" PromptChar=' ' ShowMaskOnFocus="False" Width="78px">
                    </ig:WebMaskEditor>
                    <ig:WebMaskEditor ID="PhoneNumberExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="42px" ShowMaskOnFocus="False"></ig:WebMaskEditor>                                        
                </td>
            </tr>
        </table>
    </asp:Panel>
</fieldset>

<script type="text/javascript">
    $(document).ready(function () {

        $("#ContentPlaceHolder1_WucTradeReference0_PhoneNumberCountryCode").change(function () {
            $("#ContentPlaceHolder1_WucTradeReference0_PhoneNumberCountryCodeDisplay").val($(this).val());
        });

        $("#ContentPlaceHolder1_WucTradeReference1_PhoneNumberCountryCode").change(function () {
            $("#ContentPlaceHolder1_WucTradeReference1_PhoneNumberCountryCodeDisplay").val($(this).val());
        });


        $("#ContentPlaceHolder1_WucTradeReference2_PhoneNumberCountryCode").change(function () {
            $("#ContentPlaceHolder1_WucTradeReference2_PhoneNumberCountryCodeDisplay").val($(this).val());
        });


    });

</script>
