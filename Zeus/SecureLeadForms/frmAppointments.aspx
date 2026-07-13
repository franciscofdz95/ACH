<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="frmAppointments" Title="Appointments Search" Codebehind="frmAppointments.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAppointments.ascx" TagName="Appointments" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <uc1:Appointments runat="server" ID="wucAppointments1" />
        </div>
    </div>
</asp:Content>
