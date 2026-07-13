<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="wucReserveGrid.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucReserveGrid" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<asp:GridView ID="grdReserve" runat="server" AutoGenerateColumns="False" Font-Names="Verdana" OnSorting="grdReserve_Sorting"
    DataKeyNames="ReserveID,ZID" ShowHeaderWhenEmpty="True" Font-Size="X-Small" CssClass="mGrid"
    PagerStyle-CssClass="pgr" OnPreRender="grdReserve_PreRender" AlternatingRowStyle-CssClass="alt"
    SelectedRowStyle-BackColor="#fffacd" OnRowDataBound="grdReserve_RowDataBound" AllowSorting="True"
    OnRowCommand="grdReserve_RowCommand">
    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
    <Columns>
        <%--0--%>
        <asp:TemplateField HeaderText="">
            <HeaderTemplate>
                <input type="checkbox" id="ReserveCBColumn" style="text-align:center;padding-left:3px;padding-right:3px;"/>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox runat="server" CssClass="cbcolumn" ID="cbSelect" style="text-align:center;padding-left:3px;padding-right:3px;" />
            </ItemTemplate>
            <HeaderStyle Width="30px" />
        </asp:TemplateField>
        <%--1--%>
        <asp:TemplateField HeaderText="Report Date" SortExpression="ReportDate">
            <ItemTemplate>
                <asp:LinkButton ID="lnkReportDate" CommandArgument='<%# String.Format("{0}_{1}", Eval("ReserveID"), Eval("ZID"))  %>'
                    Text='<%# String.Format("{0:M/d/yyyy}", Eval("ReportDate")) %>' CommandName="ReserveEntry" Visible="false"
                    runat="server"></asp:LinkButton>
                <asp:Label runat="server" ID="lblReportDate" Text='<%# String.Format("{0:M/d/yyyy}", Eval("ReportDate")) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="left" />
            <HeaderStyle HorizontalAlign="Center" />
        </asp:TemplateField>
        <%--2--%>
        <asp:TemplateField HeaderText="ZID">
            <ItemTemplate>
                <asp:HyperLink runat="server" Text='<%# Bind("ZID") %>' NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?Adding=false&MerchantAppUID=" + Eval("MerchantAppUID") %>' ID="hypZID"></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <%--3--%>
        <asp:TemplateField HeaderText="MID">
            <ItemTemplate>
                <asp:HyperLink runat="server" ID="hypMID" NavigateUrl='<%# WebUtil.GetBaseUrl() + "SecureRiskForms/frmReserveSearch.aspx?MID=" + Eval("SettlePlatformMID").ToString() %>' Text='<%# Bind("SettlePlatformMID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <%--4--%>
        <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" />
        
        <%--5--%>
        <asp:TemplateField SortExpression="BatchAmount" HeaderText="Sale Amount">
            <ItemTemplate>
                <asp:LinkButton runat="server" CommandArgument='<%# String.Format("{0}_{1}", Eval("ZID"), Eval("ReportDate"))  %>'
                    CommandName="Amount" ID="lbBatchAmount" Text='<%# Bind("BatchAmount", "{0:c2}") %>'></asp:LinkButton>
                <%--
                    <asp:Label runat="server" ID="LabelBatchAmount" Text='<%# Bind("BatchAmount", "{0:c2}") %>'></asp:Label>--%>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <%--6--%>
        <asp:BoundField DataField="BatchNetAmount" SortExpression="BatchNetAmount" HeaderText="Net Amount" ItemStyle-HorizontalAlign="Right"
            DataFormatString="{0:c2}">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <%--7--%>
        <asp:BoundField DataField="Amount" HeaderText="Amt Withheld" SortExpression="Amount" ItemStyle-HorizontalAlign="Right"
            DataFormatString="{0:c2}">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <%--8--%>
        <asp:BoundField DataField="CalcReserve" SortExpression="CalcReserve" HeaderText="Amt Withheld (Expected)" ItemStyle-HorizontalAlign="Right"
            DataFormatString="{0:c2}">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <%--9--%>
        <asp:TemplateField HeaderText="Reserve % (Eff) (Gross Sales)" SortExpression="CalcReservePct">
            <ItemTemplate>
                <asp:Label ID="lblRPC" runat="server" Text='<%# Bind("CalcReservePct") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <%--10--%>
        <asp:TemplateField HeaderText="Reserve %" SortExpression="Reservepct">
            <ItemTemplate>
                <asp:LinkButton ID="linkRP" runat="server" Text='<%# Bind("Reservepct") %>' CommandName="ReservePercent" CommandArgument='<%# String.Format("{0}_{1}_{2}", Eval("ZID"),Eval("ReserveID"), Eval("ReportDate"))  %>'></asp:LinkButton>
                <asp:Label ID="lblRP" runat="server" Text='<%# Bind("Reservepct") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <%--11--%>
        <asp:TemplateField HeaderText="Reserve" SortExpression="Reserve">

            <ItemTemplate>

                <asp:LinkButton ID="lnkReserveAmount" CommandArgument='<%# String.Format("{0}_{1}", Eval("ZID"), Eval("ReserveID"))  %>'
                    Text='<%# Bind("Reserve", "{0:c2}") %>' CommandName="ReserveAmount"
                    runat="server"></asp:LinkButton>

                <asp:Label ID="lblReserveAmount" runat="server" Text='<%# Bind("Reserve", "{0:c2}") %>'></asp:Label>
            </ItemTemplate>


            <ItemStyle HorizontalAlign="Right" />
        </asp:TemplateField>
        <%--12--%>
        <asp:BoundField DataField="Divert" HeaderText="Divert" SortExpression="Divert" ItemStyle-HorizontalAlign="Right"
            DataFormatString="{0:c2}">
            <ItemStyle HorizontalAlign="Right"></ItemStyle>
        </asp:BoundField>
        <%--13--%>
        <asp:BoundField DataField="ReserveSource" HeaderText="Source" SortExpression="ReserveSource" ItemStyle-HorizontalAlign="Left">
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <%--14--%>
        <asp:BoundField DataField="Diversions" HeaderText="Diversions"  ItemStyle-HorizontalAlign="Left">
            <ItemStyle HorizontalAlign="Left"></ItemStyle>
        </asp:BoundField>
        <%--15--%>
        <asp:BoundField DataField="Bank" HeaderText="Bank" />
    </Columns>
    <EmptyDataTemplate>
        No Records
    </EmptyDataTemplate>
    <PagerStyle CssClass="pgr" />
    <AlternatingRowStyle CssClass="alt" />
    <SelectedRowStyle BackColor="LemonChiffon" />


</asp:GridView>
<asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
    <span style="height: 25px; vertical-align: middle;">
        <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
            Excel</span>
</asp:LinkButton>
<script type="text/javascript" language="javascript">



    $(document).ready(function () {
        $("#ReserveCBColumn").change(function () {
            var outercb = this.checked;
            $.each($('#<%= grdReserve.ClientID %> .cbcolumn input'), function () {
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
