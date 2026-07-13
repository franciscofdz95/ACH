<%@ Page Title="Reserve Queues" Language="C#" MasterPageFile="~/MasterPageRisk.master"
    AutoEventWireup="True" CodeBehind="frmReserveQueue.aspx.cs" Inherits="ZeusWeb.frmReserveQueue" %>

<%@ Register Src="../UserControls/Reserve/wucDivertGrid.ascx" TagName="wucDivertGrid"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/Reserve/wucReserveGrid.ascx" TagName="wucReserveGrid"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/Reserve/wucReleaseGrid.ascx" TagName="wucReleaseGrid"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/Reserve/wucDivertDialog.ascx" TagName="wucDivertDialog"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/Reserve/wucReleaseDialog.ascx" TagName="wucReleaseDialog"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControls/Reserve/wucReserveDialog.ascx" TagName="wucReserveDialog"
    TagPrefix="uc6" %>
<%@ Register Src="../UserControls/Reserve/wucReserveBatchDetailsDialog.ascx" TagName="wucReserveBatchDetailsDialog"
    TagPrefix="uc7" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/Reserve/wucReservePercentDialog.ascx" TagName="wucReservePercentDialog" TagPrefix="uc8" %>
<%@ Register Src="../UserControls/Reserve/wucReserveAmountDialog.ascx" TagName="wucReserveAmountDialog" TagPrefix="uc9" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc10" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/jscript">

        $(document).ready(function () {
            $("#<%=ReportDate.ClientID %>").datepicker();
        });

    </script>
    <asp:Panel runat="server" ID="pnlOverrideReportDate" Visible="false" Style="text-align: right">
        Override Report Date:
            <asp:TextBox runat="server" Width="90px" ID="ReportDate"></asp:TextBox><asp:Button runat="server" Text="Override" ID="btnOverride" OnClick="btnOverride_Click" />
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlImport">
        <div style="text-align: right">
            <asp:Button runat="server" ID="btnImport" OnClick="btnImport_Click" />
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlLastReports" Style="text-align: right">
        <asp:Label runat="server" ID="lblLastImport"></asp:Label>
        <br />
    </asp:Panel>

    <asp:Panel runat="server" ID="pnlImportError" Visible="false" CssClass="errorlist">
        <div style="text-align: left">
            <div>
                <asp:Label runat="server" ID="lblReportDate"></asp:Label>
            </div>
            <asp:Repeater ID="rReportDate" runat="server" OnItemDataBound="rReportDate_ItemDataBound">
                <ItemTemplate>
                    Import errors for report date <%# Eval("ReportDate", "{0:d}") %>:
                    <asp:BulletedList runat="server" ID="blErrorMessage">
                    </asp:BulletedList>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:Panel>

    &nbsp;
    <table>
        <tr>
            <td>
                <asp:CheckBox ID="chkShowHistory" runat="server" AutoPostBack="True" Text="Show History"
                    OnCheckedChanged="chkShowHistory_CheckedChanged" />
            </td>
            <td>
                <ig:WebDatePicker ID="QAReportDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                    Visible="false" DataMode="Date" Width="100px" BackColor="#EFF3FF" BorderStyle="Solid"
                    BorderWidth="1px">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />

                </ig:WebDatePicker>

            </td>
            <td>
                <asp:Button runat="server" Visible="false" ID="btnQASearch" Text="Search" OnClick="btnQASearch_Click" />
            </td>
        </tr>
    </table>

    <uc10:wucMessage ID="wucMessage1" runat="server" />

    <fieldset>
        <legend>QA of Funds Withheld</legend>

        <uc2:wucReserveGrid ID="wucReserveGrid1" ShowCheckbox="True" IsQueue="True" ShowMerchantColumns="True"
            runat="server" />
        <br />
        <div style="text-align: right;">

            <asp:Button ID="btnReCalc" runat="server" Text="Reset Default Allocations" OnClick="btnReCalc_Click" />
            <asp:Button ID="Button1" runat="server" Text="Post Selected Reserve Records" OnClientClick="return confirm('Are you sure you want post these reserve records?');" OnClick="btnReserveConfirm_Click" />

        </div>


    </fieldset>
    <br />
    <fieldset>
        <legend>Disposition of Diverted Funds</legend>

        <%--        <asp:Panel runat="server" ID="pnlADR060" Visible="false" CssClass="errorlist">
            <ul>
                <li>Please upload
                    <asp:HyperLink runat="server" NavigateUrl="~/SecureRiskForms/frmReserveUpload.aspx" ID="hypUpload">today's</asp:HyperLink>
                    ADR060 File.</li>
            </ul>
        </asp:Panel>--%>

        <%--        Import the file for MM/DD/YYYY.

        if no file exists, then just perform manual import.

        do you get adr060 files every day?--%>

        <uc1:wucDivertGrid ID="wucDivertGrid1" ShowMerchantColumns="True" ShowCheckbox="True"
            ViewPendingRecords="true" runat="server" />
        <div style="text-align: right;">
            <asp:Button ID="btnDivertPreviewOnly" runat="server" Text="Preview Excel" Visible="true"
                OnClick="btnDivertPreviewExcel_Click" />
            <asp:Button ID="btnConfirmDivert" runat="server" Text="Post Selected Divert Records" OnClientClick="return confirm('Are you sure you want confirm these divert records?');"
                OnClick="btnConfirmDivert_Click" />
        </div>

        <asp:GridView ID="grdDivDocs" runat="server" CssClass="mGrid" OnRowCommand="grdDivDocs_RowCommand" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="BatchID">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Bind("DivertBatchID") %>' CommandName="BatchFiles" CommandArgument='<%# Bind("DivertBatchID") %>' runat="server"
                            ID="lbBatchID"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DateCreated"
                    HeaderText="Date Released" />
                <asp:BoundField DataField="CreatedUsername"
                    HeaderText="Released By" />
                <asp:BoundField DataField="DivertCount"
                    HeaderText="Item Count" />
            </Columns>
        </asp:GridView>

    </fieldset>
    <br />
    <fieldset>
        <legend>Pending Releases</legend>

        <uc3:wucReleaseGrid ID="wucReleaseGrid1" ShowMerchantColumns="True" ShowCheckbox="True" IsQueue="True" ViewPendingRecords="true"
            runat="server" />
        <div style="text-align: right;">
            <asp:Button ID="btnPreviewPDFsOnly" runat="server" Text="Preview PDF's Only" Visible="false" OnClick="btnGenPDF_Click" />
            <asp:Button ID="btnReleaseFinal" runat="server" Text="Post Selected Release Records" OnClientClick="return confirm('Are you sure you want to approve and post these release records?');"
                OnClick="btnReleaseFinal_Click" />
        </div>


        <asp:Literal ID="litBatchList" runat="server"></asp:Literal>


        <asp:GridView ID="grdRelDocs" runat="server" CssClass="mGrid" OnRowCommand="grdRelDocs_RowCommand" AutoGenerateColumns="False">
            <Columns>
                <asp:TemplateField HeaderText="BatchID">
                    <ItemTemplate>
                        <asp:LinkButton Text='<%# Bind("ReleaseBatchID") %>' CommandName="BatchFiles" CommandArgument='<%# Bind("ReleaseBatchID") %>' runat="server"
                            ID="lbBatchID"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DateCreated"
                    HeaderText="Date Released" />
                <asp:BoundField DataField="CreatedUsername"
                    HeaderText="Released By" />
                <asp:BoundField DataField="ReleaseCount"
                    HeaderText="Item Count" />
            </Columns>
        </asp:GridView>

        <%--<asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">LinkButton</asp:LinkButton>--%>
    </fieldset>
    <uc4:wucDivertDialog ID="wucDivertDialog1" runat="server" />
    <uc5:wucReleaseDialog ID="wucReleaseDialog1" runat="server" />
    <uc6:wucReserveDialog ID="wucReserveDialog1" runat="server" />
    <uc7:wucReserveBatchDetailsDialog ID="wucReserveBatchDetailsDialog1" runat="server" />
    <uc9:wucReserveAmountDialog ID="wucReserveAmountDialog1" runat="server" />
    <uc8:wucReservePercentDialog ID="wucReservePercentDialog1" runat="server" />

    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>


    <script type="text/javascript">

        var myclick = function (gridname, buttonname) {
            if ($(gridname + " input:checked").length > 0) {
                $(buttonname).removeAttr('disabled');
            } else {
                $(buttonname).attr("disabled", "disabled");
            }
        }

        $(function () {

            // this will disable the button if there are no rows in the table.


            var reserve_grid = "#ContentPlaceHolder1_wucReserveGrid1_grdReserve";
            var reserve_gridbtn = "#ContentPlaceHolder1_Button1";

            var divert_grid = "#ContentPlaceHolder1_wucDivertGrid1_grdMD050";
            var divert_gridbtn = "#ContentPlaceHolder1_btnConfirmDivert";

            var release_grid = "#ContentPlaceHolder1_wucReleaseGrid1_grdRelease";
            var release_gridbtn = "#ContentPlaceHolder1_btnReleaseFinal";



            if ($(reserve_grid + " tr").length > 1 && $(reserve_grid + " tr td").length > 1) { // for "No Records"
                myclick(reserve_grid, reserve_gridbtn);
            } else {
                $(reserve_gridbtn).attr("disabled", "disabled");
            }

            $(reserve_grid + " input").on("click", function () {
                myclick(reserve_grid, reserve_gridbtn);
            });

            if ($(divert_grid + " tr").length > 1 && $(divert_grid + " tr td").length > 1) { // for "No Records"
                myclick(divert_grid, divert_gridbtn);
            } else {
                $(divert_gridbtn).attr("disabled", "disabled");
            }

            $(divert_grid + " input").on("click", function () {
                myclick(divert_grid, divert_gridbtn);
            });


            if ($(release_grid + " tr").length > 1 && $(release_grid + " tr td").length > 1) { // for "No Records"
                myclick(release_grid, release_gridbtn);
            } else {
                $(release_gridbtn).attr("disabled", "disabled");
            }

            $(release_grid + " input").on("click", function () {
                myclick(release_grid, release_gridbtn);
            });

        });



    </script>

</asp:Content>
