<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCompliance.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="frmCRMVendorSetupDetail.aspx.cs" Inherits="frmCRMVendorSetupDetail" %>

<%@ Register Src="~/UserControls/wucCompliance.ascx" TagName="wucCompliance" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageCompliance.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <uc1:wucCompliance ID="Compliance1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
