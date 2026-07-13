<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_wucDocumentUploads"
    CodeBehind="wucDocumentUploads.ascx.cs" %>
<fieldset>
    <legend>PENDING DOCUMENTS</legend>
    <asp:Panel runat="server" ID="pnlDocs" Visible="true">
        <table width="100%">
            <tr>
                <td class="lblLeft">
                    Page Size:
                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem>15</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem>100</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td class="lblRight">
                    <asp:Label runat="server" ID="lblRecordCount"></asp:Label>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
            DataSourceID="ods" AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr"
            OnPageIndexChanging="GridView1_PageIndexChanging" FooterStyle-CssClass="footer"
            Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="GridView1_RowDataBound"
            AllowPaging="True" AllowSorting="True">
            <Columns>
                <asp:TemplateField HeaderText="ZID">
                    <ItemTemplate>
                        <%--      <asp:LinkButton runat="server" ID="lbMerchantID" Text='<%# Bind("MerchantID") %>'
                            CommandArgument='<%# Bind("MerchantUID") %>' CommandName="SetMerchant" OnClick="lbMerchantID_Click"></asp:LinkButton>--%>
                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantUID") + "&Adding=false&PostBackURL=~/SecureHomeForms/frmHome.aspx"  %>'
                            runat="server" ID="hypZID" Text='<%# Eval("MerchantID") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DBA">
                    <ItemTemplate>
                        <%-- <asp:LinkButton runat="server" ID="lbMerchantDBA" Text='<%# Bind("BusinessDBAName") %>'
                            CommandArgument='<%# Bind("MerchantUID") %>' CommandName="SetMerchantDBA" OnClick="lbMerchantID_Click"></asp:LinkButton>--%>
                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmUnderwritingPending.aspx?MerchantAppUID=" + Eval("MerchantUID") + "&Adding=false&PostBackURL=~/SecureHomeForms/frmHome.aspx"  %>'
                            runat="server" ID="hypDBA" Text='<%# Eval("BusinessDBAName") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Uploaded">
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("DateUploaded") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="100px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Partner Name">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAgentName" Text='<%# Bind("AgentDBA") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Condition Name">
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("ConditionName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Document Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hypFilename" Target="_blank" runat="server" Text='<%# Bind("OrigName") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <%-- <asp:TemplateField HeaderText="Assigned To">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblAssignedTo" Text='<%# Bind("AssignToUsername") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:BoundField DataField="PendedBy" HeaderText="Previously Pended By" SortExpression="PendedBy">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="RMRep" HeaderText="SS Rep" SortExpression="RMRep">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
            </Columns>
            <FooterStyle CssClass="footer" />
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
        <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetUWConditionDocument_Paging"
            EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetUWConditionDocument_PagingCount"
            StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
            OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
            <SelectParameters>
                <asp:Parameter Name="prms" Type="Object" />
                <asp:Parameter Name="PageSize" Type="Int32" />
                <asp:Parameter Name="CurrentPage" Type="Int32" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlNone" Visible="false">
        No User-Uploaded Documents Found.
    </asp:Panel>
</fieldset>
<table width="100%" style="vertical-align: top;">
    <tr valign="top">
        <td align="left" style="width: 33%;" valign="top">
            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                <span style="height: 25px; vertical-align: middle;">
                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                                                    Excel</span>
            </asp:LinkButton>&nbsp;&nbsp;
        </td>
        <td align="right" valign="middle">Export:&nbsp;</td>
        <td align="left" valign="middle">
            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                <asp:ListItem  Value="0">Current Page</asp:ListItem>
                <asp:ListItem Selected="true" Value="1">All Pages</asp:ListItem>
            </asp:RadioButtonList></td>
        <td align="right" style="width: 33%;"></td>
    </tr>
</table>