<%@ Page Title="Products" Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    CodeBehind="frmProducts.aspx.cs" Inherits="frmProducts" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" CausesValidation="False"
            AccessKey="a" OnClick="btnAdd_Click">
            <Appearance>
                <Image Url="~/Images/add2.png" />
            </Appearance>
        </igtxt:WebImageButton>
    </div>
    <div>
        <fieldset>
            <legend>Products</legend>
            <asp:GridView Width="800" ID="ProductGrid" CssClass="mGrid" AutoGenerateColumns="false"
                Font-Size="X-Small" Font-Names="Verdana" runat="server">
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:HyperLinkField Text="Details" DataNavigateUrlFormatString="~/SecureReports/frmProductManagement.aspx?ID={0}"
                        DataNavigateUrlFields="ProductID" ItemStyle-HorizontalAlign="Center" HeaderText="" />
                    <asp:BoundField DataField="PortalList" HeaderText="Portals"></asp:BoundField>
                    <asp:BoundField DataField="ProductID" HeaderText="Product ID"></asp:BoundField>
                    <asp:BoundField DataField="ProductName" HeaderText="Title"></asp:BoundField>
                    <asp:BoundField DataFormatString="{0:d}" DataField="DateCreated" HeaderText="Created Date" />
                    <asp:BoundField DataField="IsVisibleOnPrivateLabel" ItemStyle-HorizontalAlign="Center" HeaderText="Exclude Private Label" />
                    <asp:BoundField DataField="IsActive" ItemStyle-HorizontalAlign="Center" HeaderText="Active" />
                    <asp:BoundField DataField="CreateTicket" HeaderText="Create Ticket?"></asp:BoundField>

                </Columns>
            </asp:GridView>
        </fieldset>
    </div>
</asp:Content>
