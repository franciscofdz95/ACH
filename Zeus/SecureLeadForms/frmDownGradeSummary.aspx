<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="SecureLeadForsm_frmDownGradeSummary" Title="Downgrade Summary" Codebehind="frmDownGradeSummary.aspx.cs" %>

<%@ Register Src="~/UserControls/wucDowngrade.ascx" TagName="wucDownGrade" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <uc1:wucDownGrade runat="server" ID="DownGrade" isSales="true" />
        </div>
    </div>
</asp:Content>
