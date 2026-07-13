<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmDownGradeSummary" Title="Downgrade Summary" Codebehind="frmDownGradeSummary.aspx.cs" %>

<%@ Register Src="~/UserControls/wucDowngrade.ascx" TagName="wucDownGrade" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <uc1:wucDownGrade runat="server" ID="DownGrade" isSales="false" />
    </div>
</asp:Content>
