<%@ Page Language="C#" MasterPageFile="~/MasterPageAccounting.master" AutoEventWireup="true" CodeBehind="frmInvoiceReport.aspx.cs" Title="Billing Report" Inherits="ZeusWeb.SecureReports.frmInvoiceReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../css/demo_table_jui.css" />
    <link rel="Stylesheet" type="text/css" href="../css/modalPopLite.css" />
    <script type="text/javascript" src="../js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../js/jquery.dataTables.fnReloadAjax.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../js/modalPopLite.js"></script>
    <script language="javascript" type="text/jscript">

        var ddT;
        var curBillDate;
        var categoryId;
        var _modal;
        var _curInvoiceDetailId;

        $(document).ready(function () {

            $("#lnkInvoiceReport").addClass("active");
            $("#txtDatepicker").datepicker();

            _modal = $('#popup-wrapper').modalPopLite();
            ddT = $('#tblInvoiceReprt').dataTable({
                "bJQueryUI": true,
                "bServerSide": false,
                "bProcessing": false,
                "sScrollY": 400,
                "sAjaxDataProp": "aaData",
                "bPaginate": false,
                "bAutoWidth": false,
                "aoColumns": [
                    {
                        "mData": "MerchantID",
                        "sWidth": "65px",
                        "mRender": function (data, type, row) {
                            var colAction = '<a href="javascript:displayMerchantFee(' + row.InvoiceDetailID + ')">' + row.MerchantID + '</a>';
                            return colAction;
                        }
                    },
                    { "mData": "SettlePlatformMID", "sWidth": "90px" },
                    { "mData": "BusinessDBAName", "sWidth": "175px" },
                    { "mData": "SalesPartnerDBA", "sWidth": "150px" },
                    { "mData": "SalesPartnerID", "sWidth": "60px" },
                    { "mData": "BankName", "sWidth": "125px" },
                    { "mData": "MerchantStatus", "sWidth": "80px" },
                    { "mData": "DateEnrolled", "sWidth": "65px" },
                    { "mData": "LastBillDate", "sWidth": "65px" },
                    { "mData": "FeeDescription", "sWidth": "175px" },
                    { "mData": "ACHDescriptor", "sWidth": "75px" },
                    { "mData": "BillDate", "sWidth": "65px" },
                    { "mData": "InvoiceAmt", "sWidth": "75px" },
                    { "mData": "InvoiceQuantity", "sWidth": "50px" },
                    { "mData": "InvAmt", "sWidth": "75px" },
                    {
                        "mData": "InvoiceDetailID",
                        "bSearchable": false,
                        "bSortable": false,
                        "sWidth": "75px",
                        "mRender": function (data, type, row) {
                            var colAction = '<select><option value="202">Bill</option><option value="201">Waive</option></select>';
                            colAction += '<input type="hidden" class="clsRun" value="' + row.RunID + '"/><input type="hidden" class="clsInvoiceDetailID" value="' + row.InvoiceDetailID + '"/>';
                            return colAction;
                        }
                    },
                    { "mData": "TotalRecordCount", "bSearchable": false, "bVisible": false },
                    { "mData": "RunID", "bSearchable": false, "bVisible": false }
                ],
                "fnServerParams": function (aoData) {
                    aoData.push({ "name": "command", "value": "invoicereport" }, { "name": "categoryId", "value": categoryId });
                },
                "oLanguage": {
                    "sSearch": "Filter records:",
                    "sEmptyTable": "No invoices to bill.",
                    "sProcessing": "Retrieving invoices..."
                }
            });
            $('.clsEdit').focusout(function () {
                var fee = parseFloat($('#txtProductFee').val()) * parseFloat($('#txtQuantity').val());
                if (isNaN(fee))
                    $('#txtCurrentFee').val('0');
                else
                    $('#txtCurrentFee').val(fee);
            });
        });

        function getProductInvoice() {
            if (!timedOut && (count < sessionTimeOut)) {
                clearInterval(interval);
                count = 0;
                categoryId = $("#<%=lstInvoiceCategory.ClientID %> option:selected").val();

                $.blockUI({ message: '<h1>Retrieving, please wait......</h1>' });

                ddT.fnReloadAjax("../ajax/InvoiceFees.aspx", function (json) {
                    $.unblockUI();

                    var ret = $.parseJSON(json.jqXHR.responseText);

                    if (ret.iTotalRecords > 0) {
                        curBillDate = new Date(ret.billDate);
                        $(".clsVis").css('visibility', 'visible');
                    } else {
                        curBillDate = "";
                        $(".clsVis").css('visibility', 'hidden');
                    }
                    interval = setInterval(checkSession, 1000);
                });
            } else {
                alert("Your session has expired, please login again.");
                location.href = "../frmLogin.aspx";
            }
        }

        function updateBillDate() {
            var billDate = $('#txtDatepicker').val().trim();

            if (billDate == "") {
                alert("Please select a bill date.");
                return;
            }

            billDate = new Date(billDate);

            if (billDate < curBillDate) {
                alert("Bill date can't be earlier than current bill date.");
                return;
            }

            $("#tblInvoiceReprt tbody tr td:nth-child(12)").each(function () {
                $(this).text($('#txtDatepicker').val().trim()).css('color', 'red');
            });
        }

        function displayMerchantFee(invoiceDetailId) {

            $.ajax({
                type: "POST",
                url: "../ajax/InvoiceFees.aspx",
                data: { command: 'getinvoicedetail', invoicdetailid: invoiceDetailId },
                success: function (msg) {
                    if (msg.success == 'true') {

                        $("#txtZID").val(msg.MerchantID);
                        $("#txtDBA").val(msg.BusinessDBAName);
                        $("#txtFeeType").val(msg.FeeDescription);
                        $("#txtProductFee").val(msg.MerchantFee);
                        $("#txtCurrentFee").val(msg.InvAmt);
                        $("#txtQuantity").val(msg.Quantity);
                        $('#spnMessage').html("");
                        $('#btnEditFee').val("Edit");
                        $('.clsEdit').attr("disabled", true);

                        if (msg.Waived == 1)
                            $("#chkWaived").prop('checked', true);
                        else
                            $("#chkWaived").prop('checked', false);

                        _curInvoiceDetailId = invoiceDetailId;
                        _modal[0].openModal();
                    } else {

                        $("#txtZID").val("");
                        $("#txtDBA").val("");
                        $("#txtFeeType").val("");
                        $("#txtProductFee").val("");
                        $("#txtCurrentFee").val("");
                        $("#txtQuantity").val("");
                        $('#spnMessage').html(msg.error).css("color", "red");
                    }
                }
            });
        }

        function closeModal() {
            _modal[0].closeModal();
        }

        function achInvoice() {

            if (confirm('Are you sure you want to process ACH transactions for invoices in this report?')) {

                var rows = ddT.fnGetNodes();
                var invoices = [];

                for (var i = 0; i < rows.length; i++) {
                    var invoiceObj = new Object();

                    invoiceObj.InvoiceDetailID = $(rows[i]).find("td:eq(15) .clsInvoiceDetailID").val();
                    invoiceObj.RunID = $(rows[i]).find("td:eq(15) .clsRun").val();
                    invoiceObj.InvoiceStatusID = $(rows[i]).find("td:eq(15) select").val();
                    invoiceObj.BillDate = $(rows[i]).find("td:eq(11)").text();
                    invoiceObj.InvoiceAmount = $(rows[i]).find("td:eq(14)").text();

                    invoices.push(invoiceObj);
                }

                $.blockUI({ message: '<h1>Processing ACH for invoices, please wait......</h1>' });

                $.ajax({
                    type: "POST",
                    url: "../ajax/InvoiceFees.aspx",
                    data: { command: 'processach', invoiceach: JSON.stringify(invoices) },
                    success: function (msg) {
                        if (msg.success == 'true') {
                            //refresh table
                            getProductInvoice();
                        }

                        $.blockUI({ message: '<h1>' + msg.message + '</h1>' });
                        setTimeout($.unblockUI, 2000);
                    }
                });
            }

        }

        function exportExcel() {
            var exportUrl = '<%= WebUtil.GetBaseUrl() + "ajax/InvoiceFees.aspx?command=exportreport" %>';
            exportUrl += "&categoryId=" + encodeURIComponent(categoryId);

            $("#ifrmExcel").attr("src", exportUrl);
        }

        function editFee(btn) {
            if ($(btn).val() == "Edit") {
                $(btn).val("Save");
                $('.clsEdit').prop("disabled", false);
            } else {

                $.ajax({
                    type: "POST",
                    url: "../ajax/InvoiceFees.aspx",
                    data: { command: 'updateinvoicefee', invoicdetailid: _curInvoiceDetailId, merchantid: $('#txtZID').val(), billAmount: $('#txtCurrentFee').val(), feeAmount: $('#txtProductFee').val(), quantity: $('#txtQuantity').val() },
                    success: function (msg) {
                        if (msg.success == 'true') {
                            $(btn).val("Edit");
                            $('.clsEdit').attr("disabled", true);
                            $('#spnMessage').html(msg.message).css("color", "green");

                            //redraw table
                            getProductInvoice();
                        } else {
                            $('#spnMessage').html(msg.message).css("color", "red");
                        }
                    }
                });
            }
        }

        //function execInvoice() {
        //    if (confirm('Are you sure you want to execute invoice action?')) {
        //        $.blockUI({ message: '<h1>Executing invoicing action, please wait......</h1>' });

        //        $.ajax({
        //            type: "POST",
        //            url: "../ajax/InvoiceFees.aspx",
        //            data: { command: 'execinvoice', actionId: $("#slctInvoiceAction option:selected").val() },
        //            success: function (msg) {

        //                $.blockUI({ message: '<h1>' + msg.message + '</h1>' });
        //                setTimeout($.unblockUI, 1000);
        //            }
        //        });
        //    }
        //})

        //DM-291
        var sessionTimeOut = <%=HttpContext.Current.Session.Timeout%> * 60 * 1000;
        var count = 0;
        var timedOut = false;
        var interval;
        $(() => { interval = setInterval(checkSession, 1000); });

        function checkSession() {
            count += 1000;
            if (count == sessionTimeOut) {
                timedOut = true;
                clearInterval(interval);
            } else {
                timedout = false;
            }
        }
        //DM-291
    </script>
    <div class="title">
        &nbsp;&nbsp;Billing Report
                <hr class="line" />
    </div>
    <div class="pagefooter" style="padding-top: 10px;">
        <table width="450px">
            <tr>
                <td align="right" style="width: 110px;">Invoice Product:</td>
                <td style="width: 100px;">
                    <asp:DropDownList runat="server" ID="lstInvoiceCategory" Width="95px" EnableViewState="false" /></td>
                <td>
                    <input type="button" onclick="getProductInvoice()" value="Search" />&nbsp;<input type="button" onclick="achInvoice()" value="ACH" class="clsVis" style="visibility: hidden;" />&nbsp;<input type="button" value="Excel" class="clsVis" onclick="exportExcel()" style="visibility: hidden;" />
                    <iframe id="ifrmExcel" width="0" height="0" style="visibility: hidden;"></iframe>
                </td>
            </tr>
            <tr class="clsVis" style="visibility: hidden;">
                <td align="right">Invoice Bill Date:</td>
                <td>
                    <input type="text" id="txtDatepicker" style="width: 90px;" />

                </td>
                <td>
                    <input type="button" onclick="updateBillDate()" value="Apply" />
                </td>
            </tr>
        </table>
    </div>
    <%--    <div class="title">
        &nbsp;&nbsp;Test Invoicing
                <hr class="line" />
        </div>
    <div class="pagefooter" style="padding-top: 10px;">
        <table width="450px">
            <tr>
                <td align="right" style="width: 110px;">Invoice Action:</td>
                <td style="width: 100px;">
                    <select id="slctInvoiceAction" style="background-color:#F9F9F9;border-color:#ADC3DE;border-width:1px;border-style:Solid;font-family:Tahoma;font-size:8.5pt;width:95px;">
                        <option value="-1">Select</option>
                        <option value="1">CBMS</option>
	                    <option value="2">CBMS Plus</option>
	                    <option value="3">FraudXP</option>
	                    <option value="5">Advanced Reporting</option>
	                    <option value="6">Quick Books Plugin</option>
                        <option value="0">Delete Invoices</option>
                    </select>
                </td>
                <td>
                    <input type="button" value="Execute" onclick="execInvoice()" />
                </td>
            </tr>
        </table>
    </div>--%>
    <div style="padding-top: 20px;">

        <div>
            <table id="tblInvoiceReprt" class="display" cellpadding="0" cellspacing="0">
                <thead>
                    <tr>
                        <th style="width: 65px; text-align: left;">ZID</th>
                        <th style="width: 90px; text-align: left;">MID</th>
                        <th style="width: 175px; text-align: left;">DBA</th>
                        <th style="width: 150px; text-align: left;">Agent</th>
                        <th style="width: 60px; text-align: left;">Agent ID</th>
                        <th style="width: 125px; text-align: left;">Bank Name</th>
                        <th style="width: 80px; text-align: left;">Current MS Status</th>
                        <th style="width: 65px; text-align: left;">Date Enrolled</th>
                        <th style="width: 65px; text-align: left;">Last Billed</th>
                        <th style="width: 175px; text-align: left;">Fee Description</th>
                        <th style="width: 75px; text-align: left;">ACH Descriptor</th>
                        <th style="width: 65px; text-align: left;">Bill Date</th>
                        <th style="width: 75px; text-align: left;">Amount</th>
                        <th style="width: 50px; text-align: left;">Quantity</th>
                        <th style="width: 75px; text-align: left;">Subtotal</th>
                        <th style="width: 75px; text-align: left;">Action</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>
    <div id="popup-wrapper" style="width: 400px;">
        <div class="title" style="padding-top: 5px;">
            &nbsp;&nbsp;Merchant Product Fee
                <hr class="line" />
        </div>
        <div style="padding-top: 10px; padding-bottom: 5px;">
            <table>
                <tr>
                    <td>ZID:</td>
                    <td>
                        <input type="text" id="txtZID" style="width: 250px;" readonly="readonly" disabled="disabled" /></td>
                </tr>
                <tr>
                    <td>Merchant DBA:</td>
                    <td>
                        <input type="text" id="txtDBA" style="width: 250px;" readonly="readonly" disabled="disabled" /></td>
                </tr>
                <tr>
                    <td>Fee Type:</td>
                    <td>
                        <input type="text" id="txtFeeType" style="width: 250px;" readonly="readonly" disabled="disabled" /></td>
                </tr>
                <tr>
                    <td>Product Rate:</td>
                    <td>
                        <input type="text" id="txtProductFee" class="clsEdit" disabled="disabled" maxlength="9" /></td>
                </tr>
                <tr>
                    <td>Quantity:</td>
                    <td>
                        <input type="text" id="txtQuantity" class="clsEdit" disabled="disabled" maxlength="7" /></td>
                </tr>
                <tr>
                    <td>Total Fee:</td>
                    <td>
                        <input type="text" id="txtCurrentFee" readonly="readonly" disabled="disabled" maxlength="9" /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <input type="checkbox" id="chkWaived" disabled="disabled" />&nbsp;Waived</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td><span id="spnMessage"></span></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <input type="button" value="Edit" id="btnEditFee" onclick="editFee(this)" />&nbsp<input type="button" value="Cancel" id="btnCancel" onclick="closeModal()" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
