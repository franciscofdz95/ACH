<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucReleaseGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucReleaseGrid" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:GridView ID="grdRelease" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" AllowSorting="True" OnSorting="grdRelease_Sorting"
    DataKeyNames="ReleaseID" ShowHeaderWhenEmpty="True" OnRowDataBound="grdRelease_RowDataBound"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd" OnRowCommand="grdRelease_RowCommand">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <input type="checkbox" id="ReleaseCBColumn" style="text-align:center;padding-left:3px;padding-right:3px;" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox runat="server" CssClass="cbcolumn" ID="cbSelect" style="text-align:center;padding-left:3px;padding-right:3px;"/>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status" Visible="false">
            <ItemTemplate>
                <asp:Literal runat="server" ID="litStatus"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Posted Date" SortExpression="PostedDate">
            <ItemTemplate>
                <asp:LinkButton ID="lnkPostedDate" CommandName="PostedDate" CommandArgument='<%# String.Format("{0}_{1}", Eval("ReleaseID"), Eval("ZID"))  %>'
                    Text='<%# String.Format("{0:M/d/yyyy}", Eval("PostedDate")) %>' runat="server"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:BoundField DataField="ZID" HeaderText="ZID" />
        <asp:TemplateField HeaderText="MID">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypMID" NavigateUrl='<%# WebUtil.GetBaseUrl() + "SecureRiskForms/frmReserveSearch.aspx?MID=" + Eval("SettlePlatformMID").ToString() %>' Text='<%# Bind("SettlePlatformMID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" />
        <asp:BoundField DataField="Amount" SortExpression="Amount" HeaderText="Amount" DataFormatString="{0:0.00}">
            <ItemStyle HorizontalAlign="Right" />
        </asp:BoundField>
        <asp:BoundField DataField="TransType" SortExpression="TransType" HeaderText="Release Type" />
        <asp:BoundField DataField="ReserveType" SortExpression="ReserveType" HeaderText="Reserve Type" />
        <asp:BoundField DataField="Method" SortExpression="Method" HeaderText="Method" />
        <asp:BoundField DataField="Bank" SortExpression="Bank" HeaderText="Bank" />
        <asp:TemplateField>
            <HeaderTemplate>
                Bank Notes (<span class="headmoreless fakea" onclick="ToggleHeadMoreLess(this, event, '<%= grdRelease.ClientID %>')">More</span>)
            </HeaderTemplate>
            <ItemTemplate>
                <p style="margin: 0; padding: 0;" class="minimize">
                    <asp:Literal ID="lblNotes" runat="server" Text='<%# Bind("BankNotes") %>'></asp:Literal>
                </p>
            </ItemTemplate>
            <ItemStyle Width="200px" />
        </asp:TemplateField>
        <asp:BoundField DataField="UserName" HeaderText="Initiated By" />
        <asp:BoundField DataField="ApprovedBy" HeaderText="Approved By" />
        <%--<asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="lbApprove" CommandArgument='<%# Bind("ReleaseID") %>' runat="server" OnClientClick="return confirm('Are you sure you want approve this record?');"
                    CommandName="Approve" Text="Approve"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
    <EmptyDataTemplate>
        No Records
    </EmptyDataTemplate>
    <AlternatingRowStyle CssClass="alt" />
    <PagerStyle CssClass="pgr"></PagerStyle>
    <SelectedRowStyle BackColor="LemonChiffon" />
    <RowStyle CssClass="realrow" />
</asp:GridView>
<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
    <span style="height: 25px; vertical-align: middle;">
        <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
            Excel</span>
</asp:LinkButton>
<script type="text/javascript" language="javascript">

    /* we use the change() and the prop() functions because it was correctly writing to the DOM, but was not updated on screen */
    $(document).ready(function () {
        $("#ReleaseCBColumn").change(function () {
            var outercb = this.checked;
            $.each($('#<%= grdRelease.ClientID %> .cbcolumn input'), function () {
                /* we only want to modify the ones that are enabled */
                if ($(this).prop("disabled") == false) {
                    if (outercb) {
                        $(this).prop("checked", true);
                    } else {
                        $(this).prop("checked", false);
                    }
                }
            });
        });
    });

</script>
<div style="clear: both;">
    <!-- -->
</div>
