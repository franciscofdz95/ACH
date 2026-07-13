<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucCorporateBusinessUW" CodeBehind="wucCorporateBusinessUW.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc7" %>

<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="Corporate Business"></asp:Label>
    </legend>
    <table cellspacing="2" width="100%">

        <tr>
            <td colspan="8"></td>
        </tr>

        <tr>
            <td class="lblRight">Business Name:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="FullName" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>
            <td class="lblRight">Business Phone:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="HomePhone" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>

            <td class="lblRight">Business Email:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="Email" runat="server" Text="" Width="150px"></asp:TextBox>

            </td>
            <td class="lblRight">Business Ownership %:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="PercentOwnership" runat="server" Text="" Width="150px"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="lblRight">Business TaxID:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="TaxID" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>
            <td class="lblRight">Business Address:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="Address1" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>

            <td class="lblRight">City:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="City" runat="server" Text="" Width="150px"></asp:TextBox>

            </td>
            <td class="lblRight">State:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="State" runat="server" Text="" Width="150px"></asp:TextBox></td>
        </tr>

        <tr>
            <td class="lblRight">Zip:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="Zip" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>
            <td class="lblRight" colspan="6"></td>
        </tr>

        <tr>
            <td colspan="8"></td>
        </tr>
    </table>
</fieldset>
<br />
