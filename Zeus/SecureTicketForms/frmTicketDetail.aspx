<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false"
    Inherits="frmTicketDetail" MasterPageFile="~/MasterPageTicket.master" Codebehind="frmTicketDetail.aspx.cs" %>

<%@ Register Src="~/UserControls/wucTicket.ascx" TagName="wucTicket" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageTicket.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <uc1:wucTicket ID="ticket1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
