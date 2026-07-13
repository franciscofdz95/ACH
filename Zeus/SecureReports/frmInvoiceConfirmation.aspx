<%@ Page Language="C#" MasterPageFile="~/MasterPageAccounting.master" AutoEventWireup="true" CodeBehind="frmInvoiceConfirmation.aspx.cs" Title="Billing Confirmation Report" Inherits="ZeusWeb.SecureReports.frmInvoiceConfirmation" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../css/demo_table_jui.css" />
    <script type="text/javascript" src="../js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script language="javascript" type="text/jscript">

        var ddT;
        var _startDate = "";
        var _endDate = "";
        var _sortDir = "";
        var _sortExpr = "";

        $(document).ready(function () {

            $("#lnkInvoiceConfirmation").addClass("active");

            //init date pickers
            $("#txtStartDate").datepicker();
            $("#txtEndDate").datepicker();

            //init datatable
            ddT = $('#tblConfirmation').dataTable({
                "bJQueryUI": true,
                "bServerSide": true,
                "bProcessing": false,
                "sScrollY": 400,
                "sAjaxDataProp": "aaData",
                "bPaginate": true,
                "sPaginationType": "full_numbers",
                "aLengthMenu": [25, 50, 100, 250],
                "iDisplayLength": 25,
                "bAutoWidth": false,
                "bFilter": false,
                "aoColumns": [
                    { "mData": "DateCreated", "sWidth": "75px", "asSorting": ["desc", "asc", "desc"] },
                    { "mData": "MonthCreated", "sWidth": "75px" },
                    { "mData": "Product", "sWidth": "150px" },
                    { "mData": "CreatedBy", "sWidth": "75px" },
                    { "mData": "Amount", "sWidth": "75px" },
                    { "mData": "AmountClosed", "sWidth": "85px" },
                    { "mData": "AmountRejected", "sWidth": "85px" },
                    { "mData": "AmountCollected", "sWidth": "85px" },
                    { "mData": "TotalRecordCount", "bSearchable": false, "bVisible": false }
                ],
                "sAjaxSource": "../ajax/InvoiceFees.aspx",
                "fnServerParams": function (aoData) {
                    aoData.push(
                                    { "name": "command", "value": "invoiceconfirm" },
                                    { "name": "startDate", "value": _startDate },
                                    { "name": "endDate", "value": _endDate }
                    );
                },
                "fnServerData": function (sSource, aoData, fnCallback) {
                    
                    $.getJSON(sSource, aoData, function (json) {
                        if (json.iTotalRecords > 0) {
                            $("#btnExcelExport").css("visibility", "visible");
                        } else {
                            $("#btnExcelExport").css("visibility", "hidden");
                        }

                        fnCallback(json);

                        _sortDir = json.sortDir;
                        _sortExpr = json.sortExpr;

                        $.unblockUI();
                    });
                },
                "oLanguage": {
                    "sEmptyTable": "No invoice history.",
                    "sProcessing": "Retrieving invoices..."
                }
            });
        });

        function searchConfirmation() {
            _startDate = $("#txtStartDate").val();
            _endDate = $("#txtEndDate").val();

            $.blockUI({ message: '<h1>Retrieving, please wait......</h1>' });

            ddT.fnClearTable(false);
            ddT.fnDraw();
        }

        function clearSearch() {
            $('.txtclr').val("");
        }

        function exportExcel() {
            var exportUrl = '<%= WebUtil.GetBaseUrl() + "ajax/InvoiceFees.aspx?command=exportconfirmation" %>';
            exportUrl += "&startDate=" + encodeURIComponent(_startDate) + "&endDate=" + encodeURIComponent(_endDate) + "&sortDir=" + encodeURIComponent(_sortDir) + "&sortExpr=" + encodeURIComponent(_sortExpr);

            $("#ifrmExcel").attr("src", exportUrl);
        }

    </script>
    <div class="title">
        &nbsp;&nbsp;Confirmation Report
                <hr class="line" />
    </div>
    <div>
        <table style="table-layout: fixed; width: 400px;">
            <tr>
                <td style="width: 120px; text-align: right;">Date Created Start:</td>
                <td style="width: 280px;">
                    <input type="text" id="txtStartDate" class="txtclr" style="width: 90px;" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">Date Created End:</td>
                <td>
                    <input type="text" id="txtEndDate" class="txtclr" style="width: 90px;" />
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <input type="button" value="Search" onclick="searchConfirmation()" />&nbsp;
                    <input type="button" value="Clear" onclick="clearSearch()" />&nbsp;<input type="button" value="Excel" id="btnExcelExport" onclick="exportExcel()" style="visibility: hidden;" />
                    <iframe id="ifrmExcel" width="0" height="0" style="visibility:hidden;"></iframe>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 15px;">
        <table id="tblConfirmation" class="display" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <th style="width: 75px; text-align: left;">Date Created</th>
                    <th style="width: 75px; text-align: left;">Month Created</th>
                    <th style="width: 150px; text-align: left;">Product</th>
                    <th style="width: 75px; text-align: left;">Created By</th>
                    <th style="width: 75px; text-align: left;">Amount</th>
                    <th style="width: 85px; text-align: left;">Amount Closed</th>
                    <th style="width: 85px; text-align: left;">Amount Rejected</th>
                    <th style="width: 85px; text-align: left;">Amount Collected %</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>
</asp:Content>
