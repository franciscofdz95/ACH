<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="frmLeadHistory" Title="Lead History" Codebehind="frmLeadsHistory.aspx.cs" %>

<%@ Register Src="~/UserControls/wucLeadInfo.ascx" TagName="LeadInfo" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <uc1:LeadInfo runat="server" ID="LeadInfo1" />
            <fieldset>
                <legend>Leads Status History</legend>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text="no data.." Visible="false"></asp:Label>
                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="true" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" OnRowDataBound="grd_RowDataBound">
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </div>
    </div>
</asp:Content>
