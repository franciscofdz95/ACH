<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" 
    Inherits="frmAgentAllocationDetail" MasterPageFile="~/MasterPageAllocations.Master" CodeBehind="frmAgentAllocationDetail.aspx.cs" Title="Agent Allocation Details"  %>

<%@ Register Src="~/UserControls/wucAgentAllocationDetail.ascx" TagName="wucAgentAllocationDetail" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageAllocations.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <uc1:wucAgentAllocationDetail ID="AgentAllocationDetail1" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
