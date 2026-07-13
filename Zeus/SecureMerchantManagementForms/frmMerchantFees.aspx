<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="True"
    Inherits="frmMerchantFees" Title="Merchant Fees" CodeBehind="frmMerchantFees.aspx.cs" %>

<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wuConfirmDialog.ascx" TagName="wuConfirm" TagPrefix="uc3" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src="../js/autoNumeric.js"></script>
    <link href="../css/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet">    
    <script type="text/javascript">

        $(document).ready(function () {

            //START:Added to autosum MOTO% and Internet% to CNP By Ali for PXP-6937
            // Code added for PXP-10225 by koshlendra
            <%if (EditMode && (HdnWoodforestUID.Value == HdnBank.Value || HdnBBVAUID.Value == HdnBank.Value)
     && UserSessions.CurrentMerchantApp.Office == CommonUtility.Util.Offices.Irvine)
        {%>

            var editmode = $('#ContentPlaceHolder1_HiddenEditMode').val();
            var Office = $('#ContentPlaceHolder1_HdnOffice').val();
            var Bank = $('#ContentPlaceHolder1_HdnBank').val();
            var WoodforestUID = $('#ContentPlaceHolder1_HdnWoodforestUID').val();
            var BBVAUID = $('#ContentPlaceHolder1_HdnBBVAUID').val();
            if (editmode == "True" && Office == "Irvine" && (Bank == WoodforestUID || Bank == BBVAUID)) {

                $('#<%=TinfoInterntPercent.ClientID%>,#<%=TinfoMailOrderPercent.ClientID %>').blur(function () {

                    var editTinfoInterntPercent = $('#<%=TinfoInterntPercent.ClientID%>');
                    var editTinfoMailOrderPercent = $('#<%=TinfoMailOrderPercent.ClientID %>');
                    var editTinfoManualEntryNoCardNoImprintPercent = $('#<%=TinfoManualEntryNoCardNoImprintPercent.ClientID %>');

                    var TinfoInterntPercent;
                    var TinfoManualEntryWithImprintPercent;

                    if (editTinfoInterntPercent.val() == '')
                        TinfoInterntPercent = 0;
                    else
                        TinfoInterntPercent = parseFloat(editTinfoInterntPercent.val().replace('%', ''));

                    if (editTinfoMailOrderPercent.val() == '')
                        TinfoMailOrderPercent = 0;
                    else
                        TinfoMailOrderPercent = parseFloat(editTinfoMailOrderPercent.val().replace('%', ''));


                    TinfoInterntPercent = TinfoInterntPercent + TinfoMailOrderPercent;
                    if (TinfoInterntPercent > 100)
                        TinfoInterntPercent = 100;
                    editTinfoManualEntryNoCardNoImprintPercent.val(TinfoInterntPercent.toFixed(2) + ' %');
                    editTinfoManualEntryNoCardNoImprintPercent.focus();
                    editTinfoManualEntryNoCardNoImprintPercent.blur();
                });
            }
            <%}%>
            //END:Added to autosum MOTO% and Internet% to CNP By Ali for PXP-6937


            //show rows based on brand of the merchant
            var brand = $('#ContentPlaceHolder1_Brand').val();
            if (brand == "Meritus") {
                $('#ProFee tr.optimal').remove();
            }

            //Code commented by koshlendra for removing issue of other bank fee
            //PXP-4983 ABARUA
            //var Office = $('#ContentPlaceHolder1_HdnOffice').val();
            //var Bank = $('#ContentPlaceHolder1_HdnBank').val();
            //var WoodforestUID = $('#ContentPlaceHolder1_HdnWoodforestUID').val();
            //if (!(Office == "Irvine" && Bank == WoodforestUID)) {
            //    $('#ProFee tr.Office').remove();
            //}


            //This code is to remove the merchant processing fee card type columns based on selection for this merchant.
            var disablecardlist = $('#ContentPlaceHolder1_DisableCardTypes').val();
            var substr = disablecardlist.split(',');
            if (substr != "") {
                for (var i = 0; i < substr.length; i++) {
                    $("#ProFee td." + substr[i]).remove();
                    $("#ProFee th." + substr[i]).remove();

                }
            }

            //Tool tip for the batchtype drop down.
            $('#ProFee tr.dropbatchtype').each(function (i, row) {
                var $row = $(row);
                $tds = $row.find("td");                   //Get all  td elements from the row
                $.each($tds, function () {
                    $(this).attr("title", $(this).find("select option:selected").text());
                });
            });

            //Finds the row with class dropdiscounttype and sets the edit mode of corresponding card type textboxes.
            $('#ProFee tr.dropdisctype').each(function (i, row) {
                var $row = $(row);
                $tds = $row.find("td");                   //Get all  td elements from the row
                $.each($tds, function () {
                    $(this).attr("title", $(this).find("select option:selected").text());
                    if ($(this).find("select").attr('disabled') != 'disabled') {
                        DisableDiscountType($(this).find("select"), true);  // Visits every single <td> element
                    }
                });
            });

            //Finds a row with class dynamic and sets the limit and will disable the textboxes for AMEX types.
            $('#ProFee tr.dynamic').each(function (i, row) {
                var $row = $(row);
                $(this).find("input").not('.DiscountQual input').autoNumeric('init', { vMax: '99.999' });
                var editmode = $('#ContentPlaceHolder1_HiddenEditMode').val();
                if (editmode == "True") {
                    DisableAMAO($row);
                }
            });

            //Highlight the row which is being edited.
            $("#ProFee tr").not(':first').hover(
                function () {
                    $(this).css("background", "gainsboro");
                },
                function () {
                    $(this).css("background", "");
                }
            );



            //This Method is for key down events handling
            $('#ProFee').on('keydown', 'tbody td', function (e) {
                var $input = $(this)
                var rownum = $input.parent().parent().children().index($(this).parent()) + 1;
                var colnum = $input.parent().children().index($(this)) + 1;
                if ($(this).closest("tr").is(":last-child")) {
                    colnum = colnum + 1;
                    var brand = $('#ContentPlaceHolder1_Brand').val();
                    if (brand == "Meritus") {
                        rownum = 3;
                    }
                    else {
                        rownum = 4;
                    }
                }

                var key = e.which;
                if (key == 8) //BackSpace
                {
                    $(this).find("input").val("");
                }

                if (key == 46) //Delete
                {
                    $(this).find("input").val("");
                }

                if (key == 9) //Tab will move the cursor to next row.
                {
                    e.preventDefault();
                    var $cell = $('#ProFee tr:eq(' + rownum + ') td:nth-child(' + colnum + ')')
                    var $tb = $cell.find("input:enabled");
                    $tb.select();

                }

                if (e.shiftKey && key == 9) // Shift + Tab will move to the upper row.
                {
                    e.preventDefault();
                    rownum = rownum - 2;
                    var $cell = $('#ProFee tr:eq(' + rownum + ') td:nth-child(' + colnum + ')')
                    var $tb = $cell.find("input:enabled");
                    $tb.select();
                }

            });


            //Copy data from current td across the row
            $('#ProFee').on('focus', 'tbody td:not(:first-child)', function (e) {
                var td = $(this);
                var rowIndex = $(this).parent().index('#ProFee  tr');
                td.find("input").attr('maxlength', '10');

                //Remove all Copy buttons first from the table.
                $('#ProFee').find("span").remove();

                //Append the span to the div element inside the td and copy if clicked.
                if ((td.find("select").attr("disabled") != "disabled") && (td.find("input").attr("readonly") != "readonly")) {
                    var $row = $(this).closest("tr");
                    $('<span class="fa fa-plus-square" title="Copy" style="cursor:default"></span>').mousedown(function () {
                        $tds = $row.find("td");                   //Get all td elements from the row
                        $.each($tds, function () {
                            var $thistd = $(this);

                            if ($thistd.find("input").attr("readonly") != "readonly") {
                                $thistd.find(":text").val(td.find("input").val());  // Visits every single <td> element
                            }

                            if ($thistd.find("select").attr("disabled") != "disabled") {
                                $thistd.find("select").val(td.find("select option:selected").val());  // Visits every single <td> element
                            }
                        });
                    }).appendTo(td.find("div"));
                }
            });

            applyDiscountQualifiedDecimals();
        });

        function DisableAMAO(tablerow) {
            var element = tablerow;
            element.find('td.AM').find('.DiscountQual :input').attr('readonly', 'readonly');
            element.find('td.AM').find('.DiscountQual :input').css("background-color", "gainsboro");
            element.find('td.AM').find('.DiscountQual :input').val("");

            element.find('td.AO').find('.DiscountQual :input').attr('readonly', 'readonly');
            element.find('td.AO').find('.DiscountQual :input').css("background-color", "gainsboro");
            element.find('td.AO').find('.DiscountQual :input').val("");

            element.find('td.AM').find('.DiscountMid :input').attr('readonly', 'readonly');
            element.find('td.AM').find('.DiscountMid :input').css("background-color", "gainsboro");
            element.find('td.AM').find('.DiscountMid :input').val("");

            element.find('td.AO').find('.DiscountMid :input').attr('readonly', 'readonly');
            element.find('td.AO').find('.DiscountMid :input').css("background-color", "gainsboro");
            element.find('td.AO').find('.DiscountMid :input').val("");

            element.find('td.AM').find('.DiscountNon :input').attr('readonly', 'readonly');
            element.find('td.AM').find('.DiscountNon :input').css("background-color", "gainsboro");
            element.find('td.AM').find('.DiscountNon :input').val("");

            element.find('td.AO').find('.DiscountNon :input').attr('readonly', 'readonly');
            element.find('td.AO').find('.DiscountNon :input').css("background-color", "gainsboro");
            element.find('td.AO').find('.DiscountNon :input').val("");

            // PXP-6147 Fady Massoud Disable AMEX ESA FEES INPUT
            // AuthApproved, FailedRequests, CreditCompleted, Chargebacks, Retrieval
            var Office = $('#ContentPlaceHolder1_HdnOffice').val();
            var Bank = $('#ContentPlaceHolder1_HdnBank').val();
            var WoodforestUID = $('#ContentPlaceHolder1_HdnWoodforestUID').val();
            //code changes done for PXP-10225 by koshlendra
            var BBVAUID = $('#ContentPlaceHolder1_HdnBBVAUID').val();

            if ((Office == "Irvine" && (Bank == WoodforestUID || Bank == WoodforestUID))) {
                element.find('td.AM').find('.AmexESA :input').attr('readonly', 'readonly');
                element.find('td.AM').find('.AmexESA :input').css("background-color", "gainsboro");
                element.find('td.AM').find('.AmexESA :input').val("");
            }
        }

        function DisableDiscountType(dropdown, skipAutoNumeric) {
            var element = $(dropdown);
            var col = element.parent().parent().children().index($(element).parent());
            col = col + 1;

            switch (element.val()) {
                case "4": //4 is blend
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;
                case "0": //0 is Tiered
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;

                case "1": //1 is Interchange
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;

                case "5": //7 is Marup Over Interchange
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;
                case "3": //3 is ERR
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;
                case "6": //0 is Actual
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css('background', '#F9F9F9');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 3);
                    }
                    break;
                case "7":
                    $("#ProFee tr.dynamic").each(function () {
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountMid :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").attr('readonly', 'readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").css("background-color", "gainsboro");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountNon :input").val("");
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").removeAttr('readonly');
                        $(this).find("td:nth-child(" + col + ")").find(".DiscountQual :input").css('background', '#F9F9F9');
                    });
                    if (!skipAutoNumeric) {
                        reconfigureDiscountQualDecimalsByDropdown(dropdown, 4);
                    }
                    break;
            }
        }

        function toggleInput(el, txtInput) {
            $('#' + txtInput).prop('disabled', $(el).prop('checked'));
        }

        function reconfigureDiscountQualDecimalsByDropdown(dropdown, decimals) {
            var dropdownToFieldMap = {
            '<%= VISACredit_PricingTypeID.ClientID %>': '<%= VISACredit_DiscountQual.ClientID %>',
            '<%= VISADebit_PricingTypeID.ClientID %>': '<%= VISADebit_DiscountQual.ClientID %>',
            '<%= MasterCardCredit_PricingTypeID.ClientID %>': '<%= MasterCardCredit_DiscountQual.ClientID %>',
            '<%= MasterCardDebit_PricingTypeID.ClientID %>': '<%= MasterCardDebit_DiscountQual.ClientID %>',
            '<%= AmexCredit_PricingTypeID.ClientID %>': '<%= AmexCredit_DiscountQual.ClientID %>',
            '<%= AmexDebit_PricingTypeID.ClientID %>': '<%= AmexDebit_DiscountQual.ClientID %>',
            '<%= DiscoverCredit_PricingTypeID.ClientID %>': '<%= DiscoverCredit_DiscountQual.ClientID %>',
            '<%= DiscoverDebit_PricingTypeID.ClientID %>': '<%= DiscoverDebit_DiscountQual.ClientID %>'
        };

        var dropdownId = $(dropdown).attr('id');
        var fieldId = dropdownToFieldMap[dropdownId];

        if (!fieldId) {
            return;
        }

        var $field = $('#' + fieldId);

        if ($field.length > 0) {
            try {
                var currentValue = null;

                try {
                    currentValue = $field.autoNumeric('get');
                    $field.autoNumeric('destroy');
                } catch (e) {
                    currentValue = $field.val();
                }

                if (decimals === 4) {
                    $field.autoNumeric('init', {
                        vMin: '0.0000',
                        vMax: '99.9999',
                        mDec: '4',
                        aPad: true,
                        aSign: '',
                        pSign: 's'
                    });
                } else {
                    $field.autoNumeric('init', {
                        vMin: '0.000',
                        vMax: '99.999',
                        mDec: '3',
                        aPad: true,
                        aSign: '',
                        pSign: 's'
                    });
                }

                if (currentValue) {
                    $field.autoNumeric('set', currentValue);
                }
            } catch (e) {
                console.log('Error reconfiguring decimals:', e);
            }
        }
    }

    function applyDiscountQualifiedDecimals() {
        var pricingMappings = [
            { dropdown: '#<%= VISACredit_PricingTypeID.ClientID %>', field: '#<%= VISACredit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= VISADebit_PricingTypeID.ClientID %>', field: '#<%= VISADebit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= MasterCardCredit_PricingTypeID.ClientID %>', field: '#<%= MasterCardCredit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= MasterCardDebit_PricingTypeID.ClientID %>', field: '#<%= MasterCardDebit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= AmexCredit_PricingTypeID.ClientID %>', field: '#<%= AmexCredit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= AmexDebit_PricingTypeID.ClientID %>', field: '#<%= AmexDebit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= DiscoverCredit_PricingTypeID.ClientID %>', field: '#<%= DiscoverCredit_DiscountQual.ClientID %>' },
            { dropdown: '#<%= DiscoverDebit_PricingTypeID.ClientID %>', field: '#<%= DiscoverDebit_DiscountQual.ClientID %>' }
        ];

        pricingMappings.forEach(function (mapping) {
            var selectedValue = $(mapping.dropdown).val();
            var $field = $(mapping.field);

            if ($field.length > 0) {
                try {
                    var currentValue = $field.val();

                    if (selectedValue === "7") {
                        $field.autoNumeric('init', {
                            vMin: '0.0000',
                            vMax: '99.9999',
                            mDec: '4',
                            aPad: true,
                            aSign: '',
                            pSign: 's'
                        });
                    } else {
                        $field.autoNumeric('init', {
                            vMin: '0.000',
                            vMax: '99.999',
                            mDec: '3',
                            aPad: true,
                            aSign: '',
                            pSign: 's'
                        });
                    }

                    if (currentValue) {
                        $field.autoNumeric('set', currentValue);
                    }
                } catch (e) {
                    console.log('Error applying decimals on load:', e);
                }
            }
        });
    }

    </script>

    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner">
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                        <asp:Panel ID="pnlTools" runat="server">
                            <div class="tbrtools">
                                <div class="tbrtoolsleft">
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/edit.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                                    CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue_error.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/refresh.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </asp:Panel>
                        <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                        <asp:Panel ID="pnlTransactionInformation" runat="server">
                            <fieldset>
                                <legend>Transaction Information</legend>
                                <table width="100%">
                                    <tr>
                                        <td class="lblCenter">
                                            <b>Sales Information</b>
                                        </td>
                                         <td >
                                            <b>Risk Evaluation</b>
                                        </td>
                                        <td class="lblCenter">
                                            <b>Transaction Type Percentage</b>
                                        </td>
                                        <td class="lblCenter">
                                            <b>How is transaction completed?</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <table cellpadding="0" cellspacing="2">
                                                <tr>
                                                    <td class="lblRight">Monthly Volume:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="TinfoAverageMonthlyVMCVolume" runat="server" ValueText="0"
                                                            MinDecimalPlaces="2" Width="100px">
                                                        </ig:WebNumericEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Monthly Avg Ticket:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="TinfoAverageVMCTicket" runat="server" ValueText="0" Width="100px"
                                                            MinDecimalPlaces="2">
                                                        </ig:WebNumericEditor>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Monthly High Tickets:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="TinfoHighestTicketAmount" runat="server" ValueText="0"
                                                            MinDecimalPlaces="2" Width="100px">
                                                        </ig:WebNumericEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Seasonal Highest Volume:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="TinfoSesonalHighestVolume" runat="server" ValueText="0"
                                                            MinDecimalPlaces="2" Width="100px">
                                                        </ig:WebNumericEditor>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="lblRight">Monthly Transaction Count:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor DataMode="Int" ID="MonthlyTransCount" MinValue="0" runat="server" ValueText="0" Width="100px"></ig:WebNumericEditor>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td class="lblRight">Monthly Profit:
                                                    </td>
                                                    <td>
                                                        <ig:WebNumericEditor ID="MonthlyProfit" runat="server" ValueText="0" Width="100px"
                                                            MinDecimalPlaces="2">
                                                        </ig:WebNumericEditor>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td ali-"left" valign="top">
                                             <asp:panel ID="pnlUWFulfillment" runat="server">
                                                <table>
                                                            <tr>
                                                                <th class="auto-style1"></th>
                                                                <th class="auto-style1">Total Volume(%)</th>
                                                                <th class="auto-style1">NDX Days</th>
                                                            </tr>
                                                            <tr>
                                                                <td>FulFillment Period 1</td>
                                                                <td class="periodvolume">
                                                                    <ig:WebPercentEditor MaxValue="100" ID="Period1Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                                        MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td class="periodndx">
                                                                    <asp:TextBox ID="Period1NDXDays" runat="server" MaxLength="50" Text="0" Width="85px"></asp:TextBox></td>

                                                            </tr>
                                                            <tr>
                                                                <td>FulFillment Period 2</td>
                                                                <td class="periodvolume">
                                                                    <ig:WebPercentEditor MaxValue="100" ID="Period2Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                                        MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td class="periodndx">
                                                                    <asp:TextBox ID="Period2NDXDays" runat="server" MaxLength="50" Text="0" Width="85px"></asp:TextBox></td>

                                                            </tr>
                                                            <tr>
                                                                <td>FulFillment Period 3</td>
                                                                <td class="periodvolume">
                                                                    <ig:WebPercentEditor MaxValue="100" ID="Period3Volume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                                        MinDecimalPlaces="2" Width="85px" ClientIDMode="Static">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td class="periodndx">
                                                                    <asp:TextBox ID="Period3NDXDays" runat="server" MaxLength="50" Text="0" Width="85px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Total</td>
                                                                <td>
                                                                    <ig:WebPercentEditor ID="TotalPeriodVolume" runat="server" ValueText="0" MinValue="0" MaxLength="5"
                                                                        MinDecimalPlaces="2" Width="85px" ClientIDMode="Static" ReadOnly="true">
                                                                    </ig:WebPercentEditor>
                                                                </td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                             </asp:panel>
                                        </td>
                                        <td align="left" valign="top">
                                            <table cellpadding="0" cellspacing="2">
                                                <tr>
                                                    <td class="lblRight">Face to Face:
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoStoreFrontSwipedPercent" runat="server" MinValue="0"
                                                            OnValueChange="TotalSalesType_ValueChange" ValueText="0.0000" Width="85px" MinDecimalPlaces="2">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Internet:
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoInterntPercent" runat="server" OnValueChange="TotalSalesType_ValueChange"  MinValue="0"
                                                            ValueText="0" Width="85px" MinDecimalPlaces="2">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight"> 
                                                        <asp:Label Text="Mail Order:" id="mailorderpercent" runat="server" />                                                        
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoMailOrderPercent" runat="server" OnValueChange="TotalSalesType_ValueChange" MinValue="0"
                                                            ValueText="0" Width="85px" MinDecimalPlaces="2">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>                                             
                                                <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Telephone Order:" id="telephoneorder" runat="server" />                                                        
                                                    </td>                                                   
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoTelephoneOrderPercent" runat="server"
                                                            MinDecimalPlaces="2" OnValueChange="TotalSalesType_ValueChange" ValueText="0" MinValue="0"
                                                            Width="85px">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>                                               
                                                 <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Off Premise:" id="OffPremise" runat="server" Visible="false" />                                                        
                                                    </td>                                                   
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoOffPremisePercent" runat="server" MinValue="0"
                                                            MinDecimalPlaces="2" OnValueChange="TotalSalesType_ValueChange" ValueText="0"
                                                            Width="85px" Visible="false">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Trade Show:" id="TradeShow" runat="server" Visible="false" />                                                        
                                                    </td>                                                   
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoTradeShowPercent" runat="server" MinValue="0"
                                                            MinDecimalPlaces="2" OnValueChange="TotalSalesType_ValueChange" ValueText="0"
                                                            Width="85px" Visible="false">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Other:" id="lblOther" runat="server" Visible="false" />                                                        
                                                    </td>                                                   
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoOtherPercent" runat="server" MinValue="0"
                                                            MinDecimalPlaces="2" OnValueChange="TotalSalesType_ValueChange" ValueText="0"
                                                            Width="85px" Visible="false">
                                                            <ClientEvents TextChanged="CalTotalTransactionType_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                 <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Other Specify:" id="Otherspecify" runat="server" Visible="false" />                                                        
                                                    </td>                                                   
                                                    <td>
                                                        <asp:TextBox ID="Specify" runat="server" MaxLength="50" Width="85px" Visible="false"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        <b>Total:</b>
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor ID="txtTotalSalesType" runat="server" Enabled="False" ValueText="0"
                                                            MinDecimalPlaces="2" Width="85px">
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                            </table>
                                            &nbsp;
                                        </td>
                                        <td align="left" valign="top">
                                            <table cellpadding="0" cellspacing="2">
                                                <tr>
                                                    <td class="lblRight">
                                                         <asp:Label Text="Electronic data capture (swiped):" id="Edatacapture" runat="server" />                                                         
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoElectronicDataCaptureSwipedPercent" MinValue="0"
                                                            MinDecimalPlaces="2" runat="server" ValueText="0" Width="85px">
                                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Manual entry with Impr:" id="ManualentrywithImpr" runat="server" />                                                        
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoManualEntryWithImprintPercent" runat="server" MinValue="0"
                                                            MinDecimalPlaces="2" ValueText="0" Width="85px">
                                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        <asp:Label Text="Manual entry, no card present, no imprint:" id="Manualentrynocardpresent" runat="server" /> 
                                                        
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoManualEntryNoCardNoImprintPercent" MinDecimalPlaces="2" MinValue="0"
                                                            runat="server" ValueText="0" Width="85px">
                                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">Voice Authorization and Capture:
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor MaxValue="100" ID="TinfoVoiceAuthPercent" runat="server" ValueText="0" MinValue="0"
                                                            MinDecimalPlaces="2" Width="85px">
                                                            <ClientEvents TextChanged="CalTotalTransCompleted_TextChanged" />
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        <b>Total:</b>
                                                    </td>
                                                    <td>
                                                        <ig:WebPercentEditor ID="txtTotalTransCompleted" runat="server" Enabled="False" MinDecimalPlaces="2"
                                                            ValueText="0" Width="85px">
                                                        </ig:WebPercentEditor>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                        <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="1080px" SelectedIndex="0">
                            <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />

                            <Tabs>
                                <ig:ContentTabItem Text="Account Fees" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <table width="100%">
                                            <tr>
                                                <td width="50%" valign="top">
                                                    <fieldset>
                                                        <legend>Account Fees</legend>
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr style="background-color: gainsboro;">
                                                                            <td colspan="2"><b>General</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">Application Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ApplicationFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trIntlAssociationFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Intl. Associate Fee:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="IntlAssociationFee" Width="85px">
                                                                                    <asp:ListItem Value="False">Disabled</asp:ListItem>
                                                                                    <asp:ListItem Value="True">Enabled</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trDuesAssociationFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Dues & Association Fee:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="DuesAssociationFee" Width="85px">
                                                                                    <asp:ListItem Value="False">Disabled</asp:ListItem>
                                                                                    <asp:ListItem Value="True">Enabled</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trAnnualFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Annual Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="AnnualFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="3" MaxValue="10000" MinValue="0" MaxLength="8">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trEarlyTerminationFee" runat="server">
                                                                            <td style="width: 145px" class="lblRight">Early Termination Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="EarlyTerminationFee" ClientIDMode="Static" runat="server" ValueText="0" Width="85px" MaxValue="1000000000" MinValue="0" MaxLength="15"
                                                                                    MinDecimalPlaces="3" ReadOnly="true">
                                                                                </ig:WebNumericEditor>
                                                                                <asp:CheckBox runat="server" ID="EarlyTerminationWaived" Text="Waived" onclick="toggleInput(this, 'EarlyTerminationFee')" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trVoiceAuthFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Voice Auth Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="VoiceAuthFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trAvsVoiceAuthFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">AVS Voice Auth Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="AvsVoiceAuthFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trVRUARUFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">VRU/ARU Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="VRUARUFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="3" MaxValue="10000" MinValue="0" MaxLength="8">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trRegulatoryFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Regulatory Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="RegulatoryFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trAvsFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Electronic AVS Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="AvsFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="3" MaxValue="10000" MinValue="0" MaxLength="8">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trGeneralInvoice" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">General Invoice:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="GeneralInvoice" Width="85px">
                                                                                    <asp:ListItem Value="False">Disabled</asp:ListItem>
                                                                                    <asp:ListItem Value="True">Enabled</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trGenerateInvoiceIn" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Generate Invoice In:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="GeneralInvoiceCurrency" Width="85px">
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trMeritusFees">
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr style="background-color: gainsboro;">
                                                                            <td colspan="2"><b>Monthly Fees</b></td>
                                                                        </tr>
                                                                        <tr id="trSource" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Source:</td>
                                                                            <td>
                                                                                <asp:DropDownList runat="server" ID="Source" Width="85px">
                                                                                    <asp:ListItem Value="2">FMA</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Bank</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">Statement Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="StatementFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">Monthly Min Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="MonthlyMinimumFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                                &nbsp;
                                                                                <asp:DropDownList runat="server" ID="MonthlyMinimumTypeID" Width="125px">
                                                                                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                                                    <asp:ListItem Value="1">Transaction Only</asp:ListItem>
                                                                                    <asp:ListItem Value="2">Discount Only</asp:ListItem>
                                                                                    <asp:ListItem Value="3">Discount + Transaction</asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                        </tr>
                                                                        <%--Code changes for PXP-18232 strat--%>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">High Risk Monitoring Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="HighRiskMonitoringFee" runat="server" ValueText="0" Width="85px" MaxValue="999" MinValue="0"  MaxLength="3"  DataMode="Int" >
                                                                                </ig:WebNumericEditor>
                                                                                <asp:CheckBox runat="server" ID="HighRiskMonitoringFeeWaived" Text="Waived" onclick="toggleInput(this, 'HighRiskMonitoringFeeCB')" />
                                                                            </td>
                                                                        </tr>
                                                                        <%--Code changes for PXP-18232 end--%>
                                                                        <tr id="trMerchantClub" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Merchant Club Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="MerchantClub" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPCIFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">PCI Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="PCIFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="3" MaxValue="10000" MinValue="0" MaxLength="8">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trCBInsuFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">CB Insu. Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="CBInsuFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPCIInsuFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">PCI Insu. Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="PCIInsuFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPciNonComply" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">PCI Non-Compl. Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="PCINonComply" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr style="background-color: gainsboro;">
                                                                            <td colspan="2"><b>Batch Payment Fees</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">ACH Return Fee:
                                                                            </td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ReturnItemFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">ACH Batch Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="ACHBatchFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trBACSReturnFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">BACS Return Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="BACSReturnFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trBACSTransFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">BACS Trans Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="BACSTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trEFTReturnFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">EFT Return Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="EFTReturnFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trEFTTransFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">EFT Trans Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="EFTTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPaymentReturnFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Payment Return Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="PaymentReturnFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trPaymentTransFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">Payment Trans Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="PaymentTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trWIREReturnFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">WIRE Return Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="WIREReturnFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trWIRETransFee" runat="server" visible="false">
                                                                            <td style="width: 145px" class="lblRight">WIRE Trans Fee:</td>
                                                                            <td>
                                                                                <ig:WebNumericEditor ID="WIRETransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                    MinDecimalPlaces="3">
                                                                                </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr runat="server" id="trOtherFees" visible="false">
                                                                <td>
                                                                    <table width="100%">
                                                                        <tr style="background-color: gainsboro;">
                                                                            <td colspan="2"><b>Other Fees</b></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">1. Other Fee:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="OtherFeeName1" runat="server" MaxLength="50" Width="85px"></asp:TextBox>&nbsp;
                                                                        <ig:WebNumericEditor ID="OtherFeeAmount1" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="9"
                                                                            MinDecimalPlaces="4">
                                                                        </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">2. Other Fee:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="OtherFeeName2" runat="server" MaxLength="50" Width="85px"></asp:TextBox>&nbsp;
                                                                        <ig:WebNumericEditor ID="OtherFeeAmount2" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="9"
                                                                            MinDecimalPlaces="4">
                                                                        </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">3. Other Fee:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="OtherFeeName3" runat="server" MaxLength="50" Width="85px"></asp:TextBox>&nbsp;
                                                                        <ig:WebNumericEditor ID="OtherFeeAmount3" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="9"
                                                                            MinDecimalPlaces="4">
                                                                        </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 145px" class="lblRight">4. Other Fee:
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="OtherFeeName4" runat="server" MaxLength="50" Width="85px"></asp:TextBox>&nbsp;
                                                                        <ig:WebNumericEditor ID="OtherFeeAmount4" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="9"
                                                                            MinDecimalPlaces="4">
                                                                        </ig:WebNumericEditor>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                                <td width="50%" valign="top">
                                                    <asp:Panel runat="server" ID="pnlProducts">
                                                        <fieldset>
                                                            <legend>Product Fees</legend>
                                                            <table  id="Table_ProductFee" width="100%">
                                                                <tr runat="server" id="trGateway">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>
                                                                                    <asp:Label ID="lblGatewayHeader" runat="server" Text="Label"></asp:Label> </b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Trans Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="GatewayTransFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="4" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"></ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="GatewayTransWaived" Text="Waived" onclick="toggleInput(this, 'GatewayTransFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="GatewayMonthlyFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="GatewayMonthlyWaived" Text="Waived" onclick="toggleInput(this, 'GatewayMonthlyFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="GatewaySetupFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="GatewaySetupWaived" Text="Waived" onclick="toggleInput(this, 'GatewaySetupFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <%--Ani: PXP6123 Start--%>
                                                                 <tr runat="server" id="trThirdParty">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Third Party Fees - Billed by Provider</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Trans Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="ThirdPartyTransFee" runat="server" ValueText="0" Width="85px" MinDecimalPlaces="4" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"></ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="ThirdPartyTransWaived" Text="Waived" onclick="toggleInput(this, 'ThirdPartyTransFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="ThirdPartyMonthlyFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="ThirdPartyMonthlyWaived" Text="Waived" onclick="toggleInput(this, 'ThirdPartyMonthlyFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="ThirdPartySetupFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="ThirdPartySetupWaived" Text="Waived" onclick="toggleInput(this, 'ThirdPartySetupFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <%--Ani: PXP6123 End--%>
                                                                <tr runat="server" id="trDebit">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Debit</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Access Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="DebitMonthlyFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Cash Back:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="DebitCashBackMax" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Authorization Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="DebitTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Network Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="DebitAccessFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Debit Discount:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebPercentEditor MaxValue="100" ID="DebitDiscount" runat="server" ValueText="0" MinValue="0" MaxLength="7"
                                                                                        MinDecimalPlaces="4" Width="85px">
                                                                                    </ig:WebPercentEditor>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                 <%-- PXP-6183 abarua--%>
                                                                <tr runat="server" id="trEBT">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Equipment and Entitlement Fees Section</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Cash Back Max:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="EBTCashBackMax" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight"> EBT Trans. Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="EBTTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td style="width: 145px" class="lblRight">WEX/Voyager Auth Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="PetroleumWEXTransFee" runat="server" ValueText="0" Width="85px" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <%-- PXP-6183 abarua END--%>
                                                                <%-- DM-4228 ini --%>
                                                                <tr runat="server" id="trRDRsetup" visible="true">
                                                                    <td>
                                                                         <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Rapid Dispute Resolution (Third Party)</b></td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td style="width: 145px" class="lblRight">Dispute Resolution Handling Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="RDRHandlingFee" runat="server" 
                                                                                        ValueText="0" Width="85px" ClientIDMode="Static" 
                                                                                        MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="RDRHandlingWaived" 
                                                                                        Text="Waived" 
                                                                                        onclick="toggleInput(this, 'RDRHandlingFee')" />
                                                                                </td>
                                                                            </tr>
                                                                         </table>
                                                                    </td>
                                                                </tr>
                                                                <%-- DM-4228 end %>
                                                                <%-- DM-2957 ini --%>
                                                                <tr runat="server" id="trRDR" visible="true">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Paysafe Rapid Dispute Resolution (Reseller)</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Trans Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="RDRTransFee" runat="server" 
                                                                                        ValueText="0" Width="85px" ClientIDMode="Static" 
                                                                                        MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="RDRTransWaived" 
                                                                                        Text="Waived" 
                                                                                        onclick="toggleInput(this, 'RDRTransFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="RDRMonthlyFee" runat="server" 
                                                                                        ValueText="0" Width="85px" ClientIDMode="Static" 
                                                                                        MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="RDRMonthlyWaived" 
                                                                                        Text="Waived" 
                                                                                        onclick="toggleInput(this, 'RDRMonthlyFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:</td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="RDRSetupFee" runat="server" 
                                                                                        ValueText="0" Width="85px" ClientIDMode="Static" 
                                                                                        MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="RDRSetupWaived" 
                                                                                        Text="Waived" 
                                                                                        onclick="toggleInput(this, 'RDRSetupFee')" />
                                                                                </td>
                                                                            </tr>                                                                                                                                                       
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <%-- DM-2957 end --%>
                                                                <tr runat="server" id="trFraudXP" visible="false">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Fraud XP</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Trans Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="FraudXPTransFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="FraudXPTransWaived" Text="Waived" onclick="toggleInput(this, 'FraudXPTransFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="FraudXPMonthlyFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="FraudXPMonthlyWaived" Text="Waived" onclick="toggleInput(this, 'FraudXPMonthlyFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="FraudXPSetupFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="FraudXPSetupWaived" Text="Waived" onclick="toggleInput(this, 'FraudXPSetupFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trCBMS" visible="false">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>CBMS</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Tier 1:
                                                                                </td>

                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="CBMSMonthly1" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="CBMSMonthly1Waived" Text="Waived" onclick="toggleInput(this, 'CBMSMonthly1')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Tier 2:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="CBMSMonthly2" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="CBMSMonthly2Waived" Text="Waived" onclick="toggleInput(this, 'CBMSMonthly2')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="CBMSSetupFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="4">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="CBMSSetupWaived" Text="Waived" onclick="toggleInput(this, 'CBMSSetupFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trCBMSPlus" visible="false">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>CBMS Plus</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Alert Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="CBMSPlusAlertFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="CBMSPlusAlertWaived" Text="Waived" onclick="toggleInput(this, 'CBMSPlusAlertFee')" />
                                                                                </td>

                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trApriva" visible="false">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Apriva Wireless</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Trans Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="WirelessPerTrans" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="WirelessPerTransWaived" Text="Waived" onclick="toggleInput(this, 'WirelessPerTrans')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="WirelessServiceFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="WirelessServiceWaived" Text="Waived" onclick="toggleInput(this, 'WirelessServiceFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="WirelessStmntFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="WirelessStmntWaived" Text="Waived" onclick="toggleInput(this, 'WirelessStmntFee')" />
                                                                                </td>
                                                                            </tr>
                                                                             <tr>
                                                                                <td style="width: 145px" class="lblRight">Device Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="WirelessPerDeviceFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="WirelessPerDeviceWaived" Text="Waived" onclick="toggleInput(this, 'WirelessPerDeviceFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trUpdateXP" visible="false">
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr style="background-color: gainsboro;">
                                                                                <td colspan="2"><b>Update XP</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Monthly Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="AUMonthlyFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="AUMonthlyWaived" Text="Waived" onclick="toggleInput(this, 'AUMonthlyFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Setup Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="AUSetupFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="AUSetupWaived" Text="Waived" onclick="toggleInput(this, 'AUSetupFee')" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 145px" class="lblRight">Per Match Fee:
                                                                                </td>
                                                                                <td>
                                                                                    <ig:WebNumericEditor ID="AUPerMatchFee" runat="server" ValueText="0" Width="85px" ClientIDMode="Static" MaxValue="10000" MinValue="0" MaxLength="8"
                                                                                        MinDecimalPlaces="3">
                                                                                    </ig:WebNumericEditor>
                                                                                    <asp:CheckBox runat="server" ID="AUPerMatchWaived" Text="Waived" onclick="toggleInput(this, 'AUPerMatchFee')" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </fieldset>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                            <Tabs>
                                <ig:ContentTabItem Text="Processing Fees" EnableDynamicUpdatePanel="False">
                                    <Template>

                                        <asp:Panel ID="PanelProcessingFee" runat="server" Height="" Width="">
                                            <table id="ProFee" align="left" frame="box">
                                                <tr>
                                                    <th style="background-color: gainsboro;"></th>
                                                    <th class="VI" align="center" colspan="2" style="background-color: gainsboro;" title="VISA">VI</th>
                                                    <th class="VE" align="center" style="background-color: gainsboro;" title="VISA Electron">VE</th>
                                                    <th class="MC" align="center" colspan="2" style="background-color: gainsboro;" title="Master Card">MC</th>
                                                    <th class="MD" align="center" style="background-color: gainsboro;" title="Maestro">MD</th>
                                                    <th class="AM" align="center" colspan="2" style="background-color: gainsboro;" title="Amex">AM</th>
                                                    <th class="AO" align="center" colspan="2" style="background-color: gainsboro;" title="AmexOP">AO</th>
                                                    <th class="AB" align="center" colspan="2" style="background-color: gainsboro;" title="AmexOB">AB</th>
                                                    <th class="DI" align="center" colspan="2" style="background-color: gainsboro;" title="Discover">DI</th>
                                                    <th class="DC" align="center" style="background-color: gainsboro;" title="Diners Club">DC</th>
                                                    <th class="JC" align="center" style="background-color: gainsboro;" title="JCB">JC</th>
                                                    <tr>
                                                        <th class="auto-style1" width="150px" style="background-color: gainsboro;">Fee Type</th>
                                                        <th class="VI" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="VI" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="VE" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="MC" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="MC" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="MD" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="AM" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="AM" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="AO" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="AO" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="AB" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="AB" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="DI" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="DI" width="60px" align="center" style="background-color: gainsboro;">Debit</th>
                                                        <th class="DC" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                        <th class="JC" width="60px" align="center" style="background-color: gainsboro;">Credit</th>
                                                    </tr>
                                                    <tr class="optimal dropbatchtype">
                                                        <td>Batch Type</td>
                                                        <td class="VI">
                                                            <div>
                                                            <asp:DropDownList ID="VISACredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList>
                                                                </div>
                                                                </td>
                                                        <td class="VI">
                                                            <div><asp:DropDownList ID="VISADebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="VE">
                                                            <div><asp:DropDownList ID="VISAElectronDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="MC">
                                                            <div><asp:DropDownList ID="MasterCardCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="MC">
                                                            <div><asp:DropDownList ID="MasterCardDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="MD">
                                                            <div><asp:DropDownList ID="MaestroDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="AM">
                                                            <div><asp:DropDownList ID="AmexCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="AM">
                                                            <div><asp:DropDownList ID="AmexDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="AO">
                                                            <div><asp:DropDownList ID="AmexOPCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="AO">
                                                            <div><asp:DropDownList ID="AmexOPDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="AB">
                                                            <div><asp:DropDownList ID="AmexOBCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div>
                                                        </td>
                                                        <td class="AB">
                                                            <div><asp:DropDownList ID="AmexOBDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div>
                                                        </td>
                                                        <td class="DI">
                                                            <div><asp:DropDownList ID="DiscoverCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="DI">
                                                            <div><asp:DropDownList ID="DiscoverDebit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="DC">
                                                            <div><asp:DropDownList ID="DinersClubCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">N/A</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                        <td class="JC">
                                                            <div><asp:DropDownList ID="JCBCredit_BatchTypeID" runat="server" Width="49px">
                                                                <asp:ListItem Value="1">NA</asp:ListItem>
                                                                <asp:ListItem Value="2">PTF-Post to FMA</asp:ListItem>
                                                                <asp:ListItem Value="3">DNP-Do Not Post</asp:ListItem>
                                                                <asp:ListItem Value="4">PAB-Post and Backout</asp:ListItem>
                                                                <asp:ListItem Value="5">PFO-Post Fees Only</asp:ListItem>
                                                            </asp:DropDownList></div></td>
                                                    </tr>
                                                    <tr class="dropdisctype">
                                                        <td>Discount Type</td>
                                                        <td class="VI">
                                                            <div>
                                                                <asp:DropDownList ID="VISACredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div>
                                                                <asp:DropDownList ID="VISADebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div>
                                                                <asp:DropDownList ID="VISAElectronDebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div>
                                                                <asp:DropDownList ID="MasterCardCredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div>
                                                                <asp:DropDownList ID="MasterCardDebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div>
                                                                <asp:DropDownList ID="MaestroDebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div>
                                                                <asp:DropDownList ID="AmexCredit_PricingTypeID" runat="server" Width="49px">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div>
                                                                <asp:DropDownList ID="AmexDebit_PricingTypeID" runat="server" Width="49px">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div>
                                                                <asp:DropDownList ID="AmexOPCredit_PricingTypeID" runat="server" Width="49px">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div>
                                                                <asp:DropDownList ID="AmexOPDebit_PricingTypeID" runat="server" Width="49px">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div>
                                                                <asp:DropDownList ID="AmexOBCredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <div>
                                                        </td>
                                                        <td class="AB">
                                                            <div>
                                                                <asp:DropDownList ID="AmexOBDebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div>
                                                                <asp:DropDownList ID="DiscoverCredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div>
                                                                <asp:DropDownList ID="DiscoverDebit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div>
                                                                <asp:DropDownList ID="DinersClubCredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div>
                                                                <asp:DropDownList ID="JCBCredit_PricingTypeID" runat="server" Width="49px" onchange="DisableDiscountType(this)">
                                                                    <asp:ListItem Value="4">Blend</asp:ListItem>
                                                                    <asp:ListItem Value="0">Tiered</asp:ListItem>
                                                                    <asp:ListItem Value="1">IC Plus</asp:ListItem>
                                                                    <asp:ListItem Value="5">Mark-up Over IC</asp:ListItem>
                                                                    <asp:ListItem Value="3">ERR</asp:ListItem>
                                                                    <asp:ListItem Value="6">Actual</asp:ListItem>
                                                                    <asp:ListItem Value="7">Flat Rate</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr class="dynamic">
                                                        <td>Discount-Qualified (%)</td>

                                                        <td class="VI">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="VISACredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="VISADebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="VISAElectronDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="MasterCardCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="MasterCardDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="MaestroDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexOPCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexOPDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexOBCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="AmexOBDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="DiscoverCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="DiscoverDebit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="DinersClubCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="DiscountQual">
                                                                <asp:TextBox ID="JCBCredit_DiscountQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="dynamic">
                                                        <td>Discount-Mid Qualified (%)</td>
                                                        <td class="VI">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="VISACredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="VISADebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="VISAElectronDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="MasterCardCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="MasterCardDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="MaestroDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexOPCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexOPDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexOBCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="AmexOBDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="DiscoverCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="DiscoverDebit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="DinersClubCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="DiscountMid">
                                                                <asp:TextBox ID="JCBCredit_DiscountMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="dynamic">

                                                        <td>Discount-Non Qualified (%)</td>
                                                        <td class="VI">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="VISACredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="VISADebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="VISAElectronDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="MasterCardCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="MasterCardDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="MaestroDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexOPCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexOPDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexOBCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="AmexOBDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="DiscoverCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="DiscoverDebit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="DinersClubCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="DiscountNon">
                                                                <asp:TextBox ID="JCBCredit_DiscountNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>



                                                    </tr>
                                                    <%--<tr>
                                                        <td>Blended Discount</td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox36" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox37" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox38" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox39" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox40" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox41" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox42" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox43" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox44" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox45" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox46" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Blend"><asp:TextBox  ID="TextBox47" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                      </tr>
                                                    <tr>
                                                        <td>IC Plus Discount</td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox48" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox49" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox50" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox51" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox52" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox53" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox54" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox55" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox56" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox57" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox58" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="Interchange"><asp:TextBox  ID="TextBox59" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        
                                                        
                                                      </tr>
                                                    <tr>
                                                        <td>ERR Discount - Q</td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox60" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox61" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox62" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox63" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox64" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox65" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox66" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox67" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox68" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox69" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox70" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox71" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                       
                                                      </tr>
                                                    <tr>
                                                        <td>ERR Discount - N</td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox72" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox73" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox74" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox75" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox76" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox77" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox78" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox79" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox80" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox81" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox82" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                        <td><div class="ProcFee"><asp:TextBox  ID="TextBox83" runat="server" BorderStyle="None"  Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox></div></td>
                                                      </tr>--%>
                                                    <%--class updated for PXP-7472 by koshlendra --%>
                                                    <tr class="Office numeric">
                                                        <td>Auth Approved</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_AuthApproved" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Auth Reversal</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_AuthReversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                     <%--class updated for PXP-7472 by koshlendra --%>
                                                    <tr class="Office numeric">
                                                        <td>Failed Request</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_FailedRequests" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">

                                                        <td>TDS Enrollment</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_TDSEnrollments" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Settlement Completed Qual.</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_SettlementCompletedQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Settlement Completed Mid</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_SettlementCompletedMidQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Settlement Completed Non</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_SettlementCompletedNonQual" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Settlement Cancelled</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_SettlementCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                   <%--class updated for PXP-7472 by koshlendra --%>
                                                    <tr class="Office numeric">
                                                        <td>Credit Completed</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_CreditCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Credit Cancelled</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_CreditCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Payment Completed</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_PaymentCompleted" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Payment Cancelled</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_PaymentCancelled" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Payment Declined</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_PaymentDeclined" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="numeric dynamic">
                                                        <td>Chargebacks</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_Chargebacks" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Reversal</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_Reversal" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="numeric dynamic">
                                                        <td>Retrieval</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_Retrieval" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>


                                                    </tr>
                                                    <tr class="numeric dynamic" style="display: none;">
                                                        <td>Fraud Dispute Fee</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_FraudDisputeFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>


                                                    </tr>
                                                        <%--class updated for PXP-7472 by koshlendra --%>
                                                      <tr class="numeric dynamic">
                                                        <td>Settlement Fee</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                             <%--class updated for PXP-7472 by koshlendra --%>
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                               <%--class updated for PXP-7472 by koshlendra --%>
                                                            <div class="AmexESA">
                                                                <asp:TextBox ID="AmexDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_SettlementFee" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal dynamic">
                                                        <td>Uncollectable Insu Fee (%)</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_UncollectedInsurance" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                    <tr class="optimal numeric">
                                                        <td>Mis-use Auth Fee</td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISACredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISADebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="VE">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="VISAElectronDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MasterCardDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="MD">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="MaestroDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AM">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AO">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOPDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="AB">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="AmexOBDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DI">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DiscoverDebit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="DC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="DinersClubCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td class="JC">
                                                            <div class="ProcFee">
                                                                <asp:TextBox ID="JCBCredit_MisUseAuth" runat="server" BorderStyle="None" Font-Names="Verdana" Font-Size="8pt" HorizontalAlign="Right" Width="49px"></asp:TextBox>
                                                            </div>
                                                        </td>

                                                    </tr>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                        </ig:WebTab>
                        <asp:HiddenField ID="DisableCardTypes" runat="server" />
                        <asp:HiddenField ID="Brand" runat="server" />
                        <asp:HiddenField ID="HiddenEditMode" runat="server" />
                         <asp:HiddenField ID="HdnOffice" runat="server" />
                        <asp:HiddenField ID="HdnBank" runat="server" />
                        <asp:HiddenField ID="HdnWoodforestUID" runat="server" />
                        <asp:HiddenField ID="HdnBBVAUID" runat="server" /> <%--Code added for PXP-10225 by koshlendra--%>

                    </asp:Panel>
                    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="250px" Width="400px"
                        Modal="true" InitialLocation="centered" WindowState="Hidden" Moveable="false">
                        <ContentPane>
                            <Template>
                                <uc3:wuConfirm runat="server" ID="confirm" />
                            </Template>
                        </ContentPane>
                        <Header CaptionText="Confirm Values" CloseBox-Visible="false">
                        </Header>
                    </ig:WebDialogWindow>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
<!--

    function addZero(vNumber) {
        return ((vNumber < 10) ? "0" : "") + vNumber
    }

    function formatDate(vDate, vFormat) {
        var vDay = addZero(vDate.getDate());
        var vMonth = addZero(vDate.getMonth() + 1);
        var vYearLong = addZero(vDate.getFullYear());
        var vYearShort = addZero(vDate.getFullYear().toString().substring(3, 4));
        var vYear = (vFormat.indexOf("yyyy") > -1 ? vYearLong : vYearShort)
        var vHour = addZero(vDate.getHours());
        var vMinute = addZero(vDate.getMinutes());
        var vSecond = addZero(vDate.getSeconds());
        var vDateString = vFormat.replace(/dd/g, vDay).replace(/MM/g, vMonth).replace(/y{1,4}/g, vYear)
        vDateString = vDateString.replace(/hh/g, vHour).replace(/mm/g, vMinute).replace(/ss/g, vSecond)
        return vDateString
    }

    function CalTotalTransactionType_TextChanged(oEdit, newText, oEvent) {
        var edit1 = igedit_getById('<%=TinfoStoreFrontSwipedPercent.ClientID %>');
        var edit2 = igedit_getById('<%=TinfoInterntPercent.ClientID %>');
        var edit3 = igedit_getById('<%=TinfoMailOrderPercent.ClientID %>');
        var edit4 = igedit_getById('<%=TinfoTelephoneOrderPercent.ClientID %>');
        var edit5 = igedit_getById('<%=txtTotalSalesType.ClientID %>');

            var TinfoStoreFrontSwipedPercent;
            var TinfoInterntPercent;
            var TinfoMailOrderPercent;
            var TinfoTelephoneOrderPercent;

            if (edit1.getValue() == null)
                TinfoStoreFrontSwipedPercent = 0;
            else
                TinfoStoreFrontSwipedPercent = edit1.getValue();

            if (edit2.getValue() == null)
                TinfoInterntPercent = 0;
            else
                TinfoInterntPercent = edit2.getValue();

            if (edit3.getValue() == null)
                TinfoMailOrderPercent = 0;
            else
                TinfoMailOrderPercent = edit3.getValue();

            if (edit4.getValue() == null)
                TinfoTelephoneOrderPercent = 0;
            else
                TinfoTelephoneOrderPercent = edit4.getValue();

            edit5.setValue(TinfoStoreFrontSwipedPercent + TinfoInterntPercent + TinfoMailOrderPercent + TinfoTelephoneOrderPercent);
        }

        function CalTotalTransCompleted_TextChanged(oEdit, newText, oEvent) {
            var edit1 = igedit_getById('<% =TinfoElectronicDataCaptureSwipedPercent.ClientID %>');
            var edit2 = igedit_getById('<%=TinfoManualEntryWithImprintPercent.ClientID %>');
            var edit3 = igedit_getById('<%=TinfoManualEntryNoCardNoImprintPercent.ClientID %>');
            var edit4 = igedit_getById('<%=TinfoVoiceAuthPercent.ClientID %>');
            var edit5 = igedit_getById('<%=txtTotalTransCompleted.ClientID %>');

            var TinfoElectronicDataCaptureSwipedPercent;
            var TinfoManualEntryWithImprintPercent;
            var TinfoManualEntryNoCardNoImprintPercent;
            var TinfoVoiceAuthPercent;

            if (edit1.getValue() == null)
                TinfoElectronicDataCaptureSwipedPercent = 0;
            else
                TinfoElectronicDataCaptureSwipedPercent = edit1.getValue();

            if (edit2.getValue() == null)
                TinfoManualEntryWithImprintPercent = 0;
            else
                TinfoManualEntryWithImprintPercent = edit2.getValue();

            if (edit3.getValue() == null)
                TinfoManualEntryNoCardNoImprintPercent = 0;
            else
                TinfoManualEntryNoCardNoImprintPercent = edit3.getValue();

            if (edit4.getValue() == null)
                TinfoVoiceAuthPercent = 0;
            else
                TinfoVoiceAuthPercent = edit4.getValue();


            edit5.setValue(TinfoElectronicDataCaptureSwipedPercent + TinfoManualEntryWithImprintPercent + TinfoManualEntryNoCardNoImprintPercent + TinfoVoiceAuthPercent);
        }

    // -->
    </script>

</asp:Content>

