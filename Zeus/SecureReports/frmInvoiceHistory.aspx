<%@ Page Language="C#" MasterPageFile="~/MasterPageAccounting.master" AutoEventWireup="true" CodeBehind="frmInvoiceHistory.aspx.cs" Title="Billing History Report" Inherits="ZeusWeb.SecureReports.frmInvoiceHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../css/demo_table_jui.css" />
    <script type="text/javascript" src="../js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script language="javascript" type="text/jscript">

        //used to store search parametetrs
        var ddT;
        var _startDate = "";
        var _endDate = "";
        var _merchantId = 0;
        var _mid = "";
        var _amtOperator = 0;
        var _amount = 0;
        var _productId = -1;
        var _achDescriptor = "";
        var _achStatus = -1;
        var _sortDir = "";
        var _sortExpr = "";

        $(document).ready(function () {

            $("#lnkInvoiceHistory").addClass("active");

            //init date pickers
            $("#txtStartDate").datepicker();
            $("#txtEndDate").datepicker();

            //init datatable
            ddT = $('#tblInvoiceHistory').dataTable({
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
                    {
                        "mData": "MerchantID",
                        "sWidth": "65px"
                    },
                    { "mData": "SettlePlatformMID", "sWidth": "90px" },
                    { "mData": "BusinessDBAName", "sWidth": "175px" },
                    { "mData": "SalesPartnerDBA", "sWidth": "150px" },
                    { "mData": "SalesPartnerID", "sWidth": "60px" },
                    { "mData": "BankName", "sWidth": "125px" },
                    { "mData": "FeeDescription", "sWidth": "175px" },
                    { "mData": "AchTransID", "sWidth": "75px" },
                    { "mData": "ACHDescriptor", "sWidth": "75px" },
                    { "mData": "ACHStatus", "sWidth": "75px" },
                    { "mData": "BillDate", "sWidth": "65px" },
                    { "mData": "InvAmt", "sWidth": "75px" },
                    { "mData": "TotalRecordCount", "bSearchable": false, "bVisible": false }
                ],
                "sAjaxSource": "../ajax/InvoiceFees.aspx",
                "fnServerParams": function (aoData) {
                    aoData.push(
                                    { "name": "command", "value": "invoicesearch" },
                                    { "name": "startDate", "value": _startDate },
                                    { "name": "endDate", "value": _endDate },
                                    { "name": "merchantId", "value": _merchantId },
                                    { "name": "mid", "value": _mid },
                                    { "name": "amountOperator", "value": _amtOperator },
                                    { "name": "amount", "value": _amount },
                                    { "name": "productId", "value": _productId },
                                    { "name": "achDescriptor", "value": _achDescriptor },
                                    { "name": "achStatus", "value": _achStatus }
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

        function clearSearch() {
            $('.txtclr').val("");
            $('.lstclr').prop('selectedIndex', 0);
        }

        function searchInvoice() {
            _startDate = $("#txtStartDate").val();
            _endDate = $("#txtEndDate").val();
            _merchantId = $("#txtZID").val();
            _mid = $("#txtBankMID").val();
            _amtOperator = $("#slctAmtOperator option:selected").val();
            _amount = $("#txtInvAmt").val();
            _productId = $("#<%=lstInvoiceCategory.ClientID %> option:selected").val();
            _achDescriptor = $("#txtACHDescriptor").val();
            _achStatus = $("#<%=lstACHStatus.ClientID %> option:selected").val();

            $.blockUI({ message: '<h1>Retrieving, please wait......</h1>' });

            ddT.fnClearTable(false);
            ddT.fnDraw();
        }

        function exportExcel() {
            var exportUrl = '<%= WebUtil.GetBaseUrl() + "ajax/InvoiceFees.aspx?command=exporthistory" %>';
            exportUrl += "&startDate=" + encodeURIComponent(_startDate) + "&endDate=" + encodeURIComponent(_endDate) + "&merchantId=" + encodeURIComponent(_merchantId);
            exportUrl += "&mid=" + encodeURIComponent(_mid) + "&amountOperator=" + _amtOperator + "&amount=" + encodeURIComponent(_amount);
            exportUrl += "&productId=" + _productId + "&achDescriptor=" + encodeURIComponent(_achDescriptor) + "&achStatus=" + _achStatus;
            exportUrl += "&sortDir=" + encodeURIComponent(_sortDir) + "&sortExpr=" + encodeURIComponent(_sortExpr);

            $("#ifrmExcel").attr("src", exportUrl);
        }

    </script>
    <div class="title">
        &nbsp;&nbsp;Billing History Report
                <hr class="line" />
    </div>
    <div style="padding-top: 10px;">
        <table style="width: 700px; table-layout: fixed;">
            <tr>
                <td style="width: 300px; vertical-align: top;">
                    <table style="padding: 1px; table-layout: fixed;">
                        <tr>
                            <td style="width: 100px; text-align: right;">ZID:</td>
                            <td style="width: 200px">
                                <input type="text" class="txtclr" id="txtZID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">MID:</td>
                            <td>
                                <input type="text" class="txtclr" id="txtBankMID" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">Product Name:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="lstInvoiceCategory" CssClass="lstclr" Style="width: 150px;" Visible="true" EnableViewState="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">Descriptor:</td>
                            <td>
                                <input type="text" class="txtclr" id="txtACHDescriptor" />
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <input type="button" value="Search" onclick="searchInvoice()" />
                                &nbsp;<input type="button" value="Clear" onclick="clearSearch()" />
                                &nbsp;<input type="button" value="Excel" id="btnExcelExport" onclick="exportExcel()" style="visibility: hidden;" />
                                <iframe id="ifrmExcel" width="0" height="0" style="visibility:hidden;"></iframe>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 400px; vertical-align: top;">
                    <table style="padding: 1px; table-layout: fixed;">
                        <tr>
                            <td style="width: 100px; text-align: right;">Bill Start Date:</td>
                            <td style="width: 300px">
                                <input type="text" id="txtStartDate" class="txtclr" style="width: 90px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">Bill End Date:</td>
                            <td>
                                <input type="text" id="txtEndDate" class="txtclr" style="width: 90px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">Amount:</td>
                            <td>
                                <select id="slctAmtOperator" class="lstclr">
                                    <option value="0" selected="selected">=</option>
                                    <option value="1">>=</option>
                                    <option value="2">></option>
                                    <option value="3"><=</option>
                                    <option value="4"><</option>
                                </select>
                                &nbsp;
                                <input type="text" id="txtInvAmt" class="txtclr" style="width: 90px;" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100px; text-align: right;">Status:</td>
                            <td>
                                <asp:DropDownList runat="server" ID="lstACHStatus" Width="175px" CssClass="lstclr" Style="width: 150px;" EnableViewState="false"></asp:DropDownList></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2"></td>
            </tr>
        </table>
    </div>

    <div style="padding-top: 15px;">
        <table id="tblInvoiceHistory" class="display" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <th style="width: 65px; text-align: left;">ZID</th>
                    <th style="width: 90px; text-align: left;">MID</th>
                    <th style="width: 175px; text-align: left;">DBA</th>
                    <th style="width: 150px; text-align: left;">Agent</th>
                    <th style="width: 60px; text-align: left;">Agent ID</th>
                    <th style="width: 125px; text-align: left;">Bank Name</th>
                    <th style="width: 175px; text-align: left;">Fee Description</th>
                    <th style="width: 75px; text-align: left;">ACH TransID</th>
                    <th style="width: 75px; text-align: left;">ACH Descriptor</th>
                    <th style="width: 75px; text-align: left;">ACHStatus</th>
                    <th style="width: 65px; text-align: left;">ACH Date</th>
                    <th style="width: 75px; text-align: left;">Subtotal</th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
    </div>

</asp:Content>
