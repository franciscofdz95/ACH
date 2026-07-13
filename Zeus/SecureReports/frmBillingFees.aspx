<%@ Page Language="C#" MasterPageFile="~/MasterPageAccounting.master" AutoEventWireup="true" CodeBehind="frmBillingFees.aspx.cs" Title="Billing Fees" Inherits="ZeusWeb.SecureReports.frmBillingFees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 1000px">
        <link href="../css/datatable-1.13/jquery-ui.css" rel="stylesheet" type="text/css" />
        <link href="../css/datatable-1.13/dataTables.jqueryui.min.css" rel="stylesheet" type="text/css" />
        <link href="../css/datatable-1.13/scroller.jqueryui.min.css" rel="stylesheet" type="text/css" />
        <link href="../css/datatable-1.13/fixedColumns.jqueryui.min.css" rel="stylesheet" type="text/css" />

        <script src="../js/jquery-3.7.1.min.js" type="text/javascript"></script>
        <script language='JavaScript' src="../js/jquery-migrate-3.4.0.min.js" type="text/javascript"></script>
        <script src="../js/jquery-ui.1.12.0.custom.min.js" type="text/javascript"></script>
        <script src="../js/datatable-1.13/jquery.dataTables.min.js" type="text/javascript"></script>
        <script src="../js/datatable-1.13/dataTables.jqueryui.min.js" type="text/javascript"></script>
        <script src="../js/datatable-1.13/dataTables.scroller.min.js" type="text/javascript"></script>
        <script src="../js/datatable-1.13/dataTables.fixedColumns.min.js" type="text/javascript"></script>

        <style type="text/css">
            .modalpop {
                position: fixed;
                top: 0;
                left: 0;
                background-color: black;
                z-index: 99;
                opacity: 0.8;
                filter: alpha(opacity=80);
                -moz-opacity: 0.8;
                min-height: 100%;
                width: 100%;
                border: 5px solid #808080;
            }

            .containerDiv {
                display: flex;
            }

            .div1, .div2 {
                flex: 1;
            }

            .controlcontainer {
                position: relative;
            }

            .item {
            }

            .item1 {
            }

            .item2 {
                position: relative;
            }

            #list_checkboxs, .list_collapse {
                background-color: White;
                width: 150px;
                height: 90px;
                overflow-x: hidden;
                overflow-y: scroll;
                border: 1px solid #666;
                z-index: 10;
                padding: 5px;
                text-align: left;
                display: block;
            }

            .chkList {
                width: 10px;
                height: 10px;
                background-color: transparent;
                padding: 0 5px 0 0;
                border: 1px solid #ccc
            }

            .dataTables_wrapper {
                overflow-x: auto;
                width: 1500px;
            }

            th, td {
                white-space: nowrap;
            }

            div.dataTables_wrapper {
                margin: 0 auto;
            }

            table.dataTable tbody tr > .dtfc-fixed-left,
            table.dataTable tbody tr > .dtfc-fixed-right {
                z-index: 1;
                background-color: white;
            }

            .center {
                position: fixed;
                top: 50%;
                left: 50%;
                /* bring your own prefixes */
                transform: translate(-50%, -50%);
            }

            .overlay {
                background: lightgrey;
                position: absolute;
                top: 0;
                right: 0;
                bottom: 0;
                left: 0;
                opacity: 0.5;
            }

            .dataTables_wrapper {
                overflow-x: auto;
                width: 1000px;
            }
        </style>
        <script language="javascript" type="text/jscript">

            $(function () {

                $(".ddlBillingOptions").selectmenu({
                    change: function (event, data) {
                        var firstVal = $("#ContentPlaceHolder1_ddlBillingOptions-button .ui-selectmenu-text").text();
                        //alert(firstVal);
                        $(".ddlBillingOptions").val(firstVal);
                        $(".ddlBillingOptions").change();
                        return true;
                    }
                });
                $(".SearchButton").button({
                    icon: "ui-icon-search"
                });
                $(".AchButton").button({
                    icon: "ui-icon-check"
                });
                $(".ExportButton").button({
                    icon: "ui-icon-arrowthick-1-s"
                });
                $("[id*=dgvData]").removeAttr("Hidden");

                $("[id*=dgvData]").DataTable(
                    {
                        fixedColumns: {
                            rightColumns: 1
                        },
                        "bSort": true,
                        "bJQueryUI": true,
                        scrollCollapse: true,
                        scrollY: 300,
                        scrollX: true,
                        paging: true
                    });
                $(".overlay").hide();
                $('table.checkboxList>tbody>tr>td>:checkbox').addClass('chkList');
                $('table.checkboxList>tbody>tr>td>:checkbox').on("click",
                    function () {
                        var bankName = this.value;
                        var checkedCheckbox = this.checked;
                        var counterChanges = 0;
                        var table = new DataTable('#ContentPlaceHolder1_dgvData');

                        table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                            var rowNode = this.node();
                            $(rowNode).find("td").each(function () {
                                var cellData = $(this).html();
                                if (cellData == bankName) {
                                    counterChanges++;
                                    if (checkedCheckbox)
                                        $(rowNode).find("select").val("Bill");
                                    else
                                        $(rowNode).find("select").val("Waive");
                                }
                            });
                        });
                        if (checkedCheckbox)
                            alert(counterChanges + " rows from " + bankName + " bank were selected");
                        else
                            alert(counterChanges + " rows from " + bankName + " bank were unselected");
                    }
                );
            });

            $('form').on("submit", null, function () {
                setTimeout(function () {
                    $(".overlay").show();
                }, 200);
            });

            function ConfirmACH() {
                var rowCount = $('table.dataTable tr').length;
                if (rowCount <= 3) {
                    alert("There are no rows");
                    return false;
                }
                $("[id*=hdnZids]").val("");
                var zids = '';
                var table = new DataTable('#ContentPlaceHolder1_dgvData');
                table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    var rowNode = this.node();
                    if ($(rowNode).find("select").val() == "Bill") {
                        zids = zids + ',' + $(rowNode).find("td:first").html();
                    }
                });
                $("[id*=hdnZids]").val(zids);
                if (confirm("Are you sure you want to process ACH transactions for invoices in this report?"))
                    $(".overlay").show();
                else
                    return false;
            }

            function decodeHTMLEntities(str) {
                var element = document.createElement('div');
                if (str && typeof str === 'string') {
                    // strip script/html tags
                    str = str.replace(/<script[^>]*>([\S\s]*?)<\/script>/gmi, '');
                    str = str.replace(/<\/?\w(?:[^"'>]|"[^"]*"|'[^']*')*>/gmi, '');
                    element.innerHTML = str;
                    str = element.textContent;
                    element.textContent = '';
                }

                return str;
            }

            function fnExcelReport() {
                var Results = [];
                var columns = [];
                var row = [];
                var table = new DataTable('#ContentPlaceHolder1_dgvData');
                $('#ContentPlaceHolder1_dgvData tr:first th').each(function () {
                    var value = $(this).text();
                    columns.push(value);
                });
                Results.push(columns);
                table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                    var rowNode = this.node();
                    row = [];
                    $(rowNode).find("td").each(function () {
                        var cellData = $(this).html();
                        if (cellData.indexOf("<select") >= 0)
                            cellData = $(rowNode).find("select").val();
                        
                        if (cellData != '') {
                            cellData = decodeHTMLEntities(cellData);
                            cellData = cellData.split(",").join("").replace(/[\n\r]+/g, '');
                        }
                        row.push(cellData);
                    });
                    Results.push(row);
                });
                var CsvString = "";
                Results.forEach(function (RowItem, RowIndex) {
                    RowItem.forEach(function (ColItem, ColIndex) {
                        CsvString += ColItem + ',';
                    });
                    CsvString += "\r\n";
                });
                CsvString = "data:text/csv;charset=utf-8,%EF%BB%BF" + encodeURIComponent(CsvString);
                var x = document.createElement("A");
                x.setAttribute("href", CsvString);
                x.setAttribute("target", "_blank");
                var fileName = $('#ContentPlaceHolder1_ddlBillingOptions').val();
                var date = new Date();
                var current_date = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                var current_time = date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
                var date_time = current_date + " " + current_time;
                x.setAttribute("download", fileName + "_" + date_time + ".csv");
                document.body.appendChild(x);
                x.click();

                return false;
            }

        </script>


        <div class="title">
            &nbsp;&nbsp;Billing Fees
                <hr class="line" />
        </div>
        <div class="pagefooter" style="padding-top: 10px;">
            <div class="container"></div>
            <div class="containerDiv">
                <div class="div1" style="width: 100px">
                    <asp:Panel ID="pnlBillingType" GroupingText="Report Options:" runat="server">
                        <div style="height: 130px">
                            <asp:Panel ID="pnlBillingReport" runat="server">
                                <div class="item item1">
                                    <asp:Label ID="lblBillingType" runat="server" Text="Billing Type:" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="item item2">
                                    <asp:DropDownList runat="server" ID="ddlBillingOptions" CssClass="ddlBillingOptions" Width="95px" EnableViewState="false" AutoPostBack="true" OnSelectedIndexChanged="ddlBillingOptions_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="Select" />
                                        <asp:ListItem Text="Annual" Value="Annual" />
                                        <asp:ListItem Text="Annual PCI" Value="Annual PCI" />
                                        <asp:ListItem Text="PCI NAF" Value="PCI NAF" />
                                    </asp:DropDownList>
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlMonth" runat="server" CssClass="controlcontainer" Visible="false">
                                <div class="item item1">
                                    <asp:Label ID="lblMonthDescription" runat="server" Text="Month:" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="item item2">
                                    <asp:Label ID="lblMonth" runat="server" Text=""></asp:Label>
                                </div>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlButtons" runat="server" Visible="false">

                                <asp:LinkButton ID="btnSearch" runat="server" CssClass="SearchButton" OnClick="btnSearch_Click">Search</asp:LinkButton>&nbsp;
                            <asp:LinkButton ID="btnAch" runat="server" CssClass="AchButton" OnClientClick="return ConfirmACH();" OnClick="btnAch_Click">ACH</asp:LinkButton>&nbsp;
                            <button id="btnExport" class="ExportButton" onclick="return fnExcelReport();">Export </button>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </div>
                <div class="div2">
                    <asp:Panel ID="pnlOptions" GroupingText="Parameter Options:" runat="server">
                        <div style="height: 130px">
                            <div class="item item1">
                                <asp:Label ID="lblBanks" runat="server" Font-Bold="true" Text="Banks selected:"></asp:Label>
                            </div>
                            <div class="item item2">
                                <div id="list_checkboxs">
                                    <asp:CheckBoxList ID="cblBanks" CssClass="checkboxList" Style="overflow: scroll;"
                                        BorderStyle="None" runat="server">
                                    </asp:CheckBoxList>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
        <asp:Panel ID="pnlLoader" CssClass="overlay" runat="server">
            <asp:Image ID="imgLoader" runat="server" CssClass="center" ImageUrl="../Images/loaderarrows.gif" />
        </asp:Panel>
        <div id="divAnnualProcess" style="padding-top: 20px;">
            <div style="width: 1000px">
                <asp:GridView ID="dgvData" runat="server" class="display stripe row-border order-column nowrap" Hidden="Hidden" Style="width: 100%" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="dgvData_RowDataBound">
                </asp:GridView>
            </div>
        </div>
        <asp:HiddenField ID="hdnZids" runat="server" />
    </div>
</asp:Content>
