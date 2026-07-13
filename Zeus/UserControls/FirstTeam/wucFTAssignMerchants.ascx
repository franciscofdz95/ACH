<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTAssignMerchants.ascx.cs" Inherits="ZeusWeb.UserControls.FirstTeam.wucFTAssignMerchants" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<script type="text/javascript">
    function SelectAll(chk) {

        $('#<%=GridView1.ClientID %>').find("input:checkbox").each(function () {
        if (this != chk) {
            this.checked = chk.checked;
        }

    });
}

</script>
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></td>
        </tr>
    <tr>
        <td style="text-align: left;">
            Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="250">250</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="lblRight">Bulk Action:
        </td>
        <td class="lblLeft">
            <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                <asp:ListItem Selected="True" Value="Assign">Assign</asp:ListItem>
                <asp:ListItem Value="Remove">Remove</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td class="lblRight">
             <asp:Label ID="lblPSRep" runat="server" Text="PS Rep:"></asp:Label>
        </td>
        <td class="lblLeft"><asp:DropDownList runat="server" ID="ddlReps">
            </asp:DropDownList></td>
        <td> <asp:Button ID="Save" runat="server" Text="Save" OnClick="btnSave_Click"/></td>
        <td style="text-align: right;">
            Total Record Count:
            <asp:Label runat="server" ID="lblRowCount">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="GridView1" CssClass="mGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" OnPageIndexChanging="GridView1_PageIndexChanging"
    OnRowDataBound="GridView1_RowDataBound" EnableModelValidation="True" DataKeyNames="ZID,OfficeID,MerchantAppUID"   ClientIDMode="Static" EmptyDataText ="No records found.">
    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:CheckBox runat="server" ID="chkSelectAll" onclick="javascript:SelectAll(this);" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chkAssign" />
            </ItemTemplate>
            <ItemStyle Width="15px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ZID">
            <ItemTemplate>
                <asp:HyperLink NavigateUrl='<%#"~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                    runat="server" ID="hypZID" Text='<%# Eval("ZID") %>'></asp:HyperLink>
            </ItemTemplate>
            <ItemStyle Width="35px" />
        </asp:TemplateField>
        <asp:BoundField DataField="BusinessDBAName" HeaderText="DBAName" />
        <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" />
        <asp:BoundField DataField="MonthlyVolume" HeaderText="CC Volume" />
        <asp:BoundField DataField="CCStatus" HeaderText="CC Status" />
        <asp:TemplateField HeaderText="Office">
            <ItemTemplate>
                <asp:Label ID="lblOffice" runat="server"></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="50px" />
        </asp:TemplateField>
        <asp:BoundField DataField="BankName" HeaderText="Bank Name" />
        <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" />
        <asp:BoundField DataField="AgentChannel" HeaderText="Agent Channel" />
    </Columns>
    <PagerStyle CssClass="pgr"></PagerStyle>
</asp:GridView>
<asp:ObjectDataSource ID="odsPotentialFTMerchants" runat="server" SelectMethod="GetPotentialFTMerchants"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetPotentialFTMerchantsCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
<ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="150px" InitialLocation="Centered" ClientIDMode="Static"
    Modal="True" Width="400px" WindowState="Hidden">
    <ContentPane EnableRelativeLayout="true">
        <Template>
            <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom:25px; margin-top:25px">
                <table cellspacing="5" width="100%" align="center">
                    <tr>
                        <td align="center">
                            <asp:Label runat="server" ID="lblError1" Text="" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" Text="Ok" ID="btnOk" OnClick="btnOk_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </Template>
    </ContentPane>
    <Header CaptionText="Alert" CloseBox-Visible="false"></Header>
</ig:WebDialogWindow>