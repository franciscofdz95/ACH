<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucDivertGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucDivertGrid" %>
<asp:GridView ID="grdMD050" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
    ShowHeaderWhenEmpty="True" OnPreRender="grdMD050_PreRender"
    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" DataKeyNames="ReportDate,ZID"
    SelectedRowStyle-BackColor="#fffacd" OnRowDataBound="grdMD050_RowDataBound" OnRowCommand="grdMD050_RowCommand">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
        <asp:TemplateField HeaderText="">
            <HeaderTemplate>
                <input type="checkbox" id="DivertCBColumn" style="text-align:center;padding-left:3px;padding-right:3px;" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox runat="server" CssClass="cbcolumn" ID="cbSelect" style="text-align:center;padding-left:3px;padding-right:3px;" />
                <asp:HiddenField runat="server" ID="hidDivertIDs" Value='<%# Eval("DivertIDs") %>' />
            </ItemTemplate>
            <HeaderStyle Width="30px" />
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Status" Visible="false">
            <ItemTemplate>
                <asp:Literal runat="server" ID="litStatus"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Report Date">
            <ItemTemplate>
                <asp:LinkButton ID="lnkReportDate" CommandName="ReportDate" CommandArgument='<%# String.Format("{0}_{1}", Eval("ReportDate"), Eval("ZID"))  %>'
                    runat="server"></asp:LinkButton>
                <asp:Label runat="server" ID="lblReportDate" Text='<%# String.Format("{0:M/d/yyyy}", Eval("ReportDate")) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <asp:BoundField DataField="ZID" HeaderText="ZID" />
        <asp:TemplateField HeaderText="MID">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypMID" NavigateUrl='<%# WebUtil.GetBaseUrl() + "SecureRiskForms/frmReserveSearch.aspx?MID=" + Eval("settleplatformmid").ToString() %>' Text='<%# Bind("settleplatformmid") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" />
        <asp:BoundField DataField="BankName" HeaderText="Bank" Visible="false" />
        <asp:BoundField DataField="Amount" HeaderText="Amount Withheld" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="BatchWithHeldAmount" HeaderText="Sent to Rsrv Acct" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Reserve" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Reserve">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="DivertClear" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Divert">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="DivertReject" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Paysafe">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <asp:BoundField DataField="PostMerchant" DataFormatString="{0:0.00}" ItemStyle-HorizontalAlign="Right"
            HeaderText="Merchant">

            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>

        <%--
            <asp:TemplateField HeaderText="Download">
            <ItemTemplate>
                <asp:LinkButton ID="lnkDownload" CommandName="Download" CommandArgument='<%# String.Format("{0}_{1}", Eval("ReportDate"), Eval("ZID"))  %>'
                    runat="server"></asp:LinkButton>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>--%>
    </Columns>
    <EmptyDataTemplate>
        No Records
    </EmptyDataTemplate>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon" />
    <RowStyle CssClass="realrow" />
</asp:GridView>

<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
    <span style="height: 25px; vertical-align: middle;">
        <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
</asp:LinkButton>

<script type="text/javascript" language="javascript">


    $(document).ready(function () {
        $("#DivertCBColumn").change(function () {
            var outercb = this.checked;
            $.each($('#<%= grdMD050.ClientID %> .cbcolumn input'), function () {

                if ($(this).attr("disabled") != "disabled") {
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
