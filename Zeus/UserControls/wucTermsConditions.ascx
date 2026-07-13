<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucTermsConditions" Codebehind="wucTermsConditions.ascx.cs" %>
<%@ Register Src="wucShedule.ascx" TagName="wucShedule" TagPrefix="uc1" %>
<div style="width: 98%">
    <table width="100%">
        <tr>
            <td style="width: 97px">
                Name:</td>
            <td>
                <asp:TextBox ID="txtName" runat="server" MaxLength="255" Width="100%"></asp:TextBox>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 85px">
                <br />
                Short T&amp;C:</td>
            <td>
                <asp:TextBox ID="txtShortTerms" runat="server" Height="100px" MaxLength="4000" TextMode="MultiLine"
                    Width="100%"></asp:TextBox></td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 85px">
                Long T&amp;C:</td>
            <td>
                <asp:TextBox ID="txtTerms" runat="server" Height="100px" MaxLength="4000" TextMode="MultiLine"
                    Width="100%"></asp:TextBox></td>
            <td>
            </td>
        </tr>
        <tr>
            <td style="width: 85px">
            </td>
            <td>
                <asp:Button ID="btnAddTerms" runat="server" Text="Add Terms & Conditions" OnClick="btnAddTerms_Click"
                    ValidationGroup="TermsAndConditions" />&nbsp;<asp:Button ID="btnClearFields" runat="server"
                        Text="Clear Fields" /></td>
            <td>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
        Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsTerms" Width="100%" DataKeyNames="TCID"
        OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand">
        <PagerStyle CssClass="pgr" />
        <AlternatingRowStyle CssClass="alt" />
        <Columns>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkName" runat="server">LinkButton</asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="ShortDesc" HeaderText="Short Terms &amp; Conditions" />
            <asp:BoundField DataField="Description" HeaderText="Long Terms &amp; Conditions"
                Visible="false" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:TemplateField HeaderText="Schedule">
                <ItemTemplate>
                    <asp:Button ID="btnRecurring" runat="server" Text="Add" CausesValidation="false"
                        Width="75px" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Button ID="btnEnbaled" runat="server" Text="Button" CausesValidation="false"
                        Width="75px" />
                    <asp:Button ID="btnEnbaled2" runat="server" Text="Button" CausesValidation="false"
                        Width="75px" />
                </ItemTemplate>
                <ItemStyle Width="75px" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="odsTerms" runat="server" SelectMethod="GetTermsConditionsDS"
        TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
        OnSelecting="odsTerms_Selecting">
        <SelectParameters>
            <asp:Parameter Name="prms" Type="Object" />
        </SelectParameters>
    </asp:ObjectDataSource>
</div>
<uc1:wucShedule ID="WucShedule1" runat="server" />
