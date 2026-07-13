// Global variables
var _allAttributes = [];
var _allOperators = [];
var _allcontrolValues = [];
var _MaxRules = 10;

var FirstLoad = false;
function pageLoad() {
    if (FirstLoad) {
        RulesUI_Render();
    }
}

function GetAllAttributes() {
    return new Promise((mySuccess, myReject) => {
        $.ajax({
            type: "POST",
            url: "frmMerchantProducts.aspx/GetRuleAttributes",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (value) {

                mySuccess(value.d)
            },
            error: function (errordata) {
                myReject("Error process on GetRuleAttributes")
            }
        });
    });
}
function GetAllOperators() {
    return new Promise((mySuccess, myReject) => {
        $.ajax({
            type: "POST",
            url: "frmMerchantProducts.aspx/GetRuleOperators",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (value) {
                mySuccess(value.d)
            },
            error: function (errordata) {
                myReject("Error process on GetRuleOperators")
            }
        });
    });
}
function GetAllControlValues() {
    return new Promise((mySuccess, myReject) => {
        $.ajax({
            type: "POST",
            url: "frmMerchantProducts.aspx/GetRuleControlValues",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (value) {
                mySuccess(value.d)
            },
            error: function (errordata) {
                myReject("Error process on GetRuleControlValues")
            }
        });
    });
}

function _promiseSelectAttribute(tableID) {
    return new Promise((mySuccess, myReject) => {
        var _optionAttribute = '<select class="form-select form-select-sm mb-2" name="Attribute" id="SelectAttribute_' + tableID + '"  onchange="SetOperatorOptionsByAttribute(this.value,\'' + tableID + '\')"><option value="0" disabled selected>Select Attribute </option>';

        _allAttributes.forEach((object, index) => {
            _optionAttribute += '<option value="' + object.ID + '">' + object.Name + '</option> '
        });
        _optionAttribute += '</select>';
        mySuccess(_optionAttribute);
    });
};
function _promiseSelectOperatorsByAttributeID(attribute, ruleID) {
    return new Promise((mySuccess, myReject) => {

        let _operators = _allOperators.filter((attr) => attr.AttributeId == attribute);

        var _optionAttribute = '<select class="form-select form-select-sm mb-2" name="Operator" id="SelectOperator_' + ruleID + '"><option value="0" disabled selected>Select Operator</option>';
        _operators.forEach((object, index) => {
            _optionAttribute += '<option value="' + object.ID + '">' + object.Name + '</option> '
        });
        _optionAttribute += '</select>';
        mySuccess(_optionAttribute);
    });
}
function _promiseSelectControlValuesByAttributeID(attribute, ruleID) {
    return new Promise((mySuccess, myReject) => {
        let _controls = _allcontrolValues.filter((attr) => attr.AttributeId == attribute);

        var _element = "";

        if (attribute == 2) { // Transaction Date
            _element += '<input class="form-control form-control-sm" type="date" name="Value" ps-uicontrolid="2" id="inputValueAttribute_' + ruleID + '" />';
        } else if (attribute == 3) { // Transaction Amount
            _element += '<input class="form-control form-control-sm" type="number" name="Value" ps-uicontrolid="2" id="inputValueAttribute_' + ruleID + '" />';
        } else if (attribute == 1) { // BIN
            _element += '<input class="form-control form-control-sm" type="number" name="Value" ps-uicontrolid="2" id="inputValueAttribute_' + ruleID + '" onKeyUp="ValidateBINLength(this.value, this.id)" />';
        } else {
            _element = '<select class="form-select form-select-sm mb-2" name="Value" ps-uicontrolid="1" id="SelectValueControl_' + ruleID + '"><option value="0" disabled selected>Select Value</option>';
            _controls.forEach((object, index) => {
                _element += '<option value="' + object.ID + '">' + object.DefaultValue.trimEnd() + '</option> '
            });
            _element += '</select>';
        }
        mySuccess(_element);
    });
}

function ValidateBINLength(BIN, ID) {
    if (BIN.length > 6) {
        $("#" + ID).val(BIN.substring(0, 6));
    }
}
function _getRulesByMerchant() {
    return new Promise(function (success, rejected) {
        $.ajax({
            type: "POST",
            url: "frmMerchantProducts.aspx/GetRulesByProducID",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (value) {
                success(value.d);
            },
            error: function (dataError) { rejected(dataError); }
        });
    })
};
function _promiseSaveAllRules(data) {
    return new Promise((mySuccess, myReject) => {
        $.ajax({
            type: "POST",
            url: "frmMerchantProducts.aspx/SaveAllRules",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ rules: data }),
            dataType: "json",
            success: function (data) {
                mySuccess(data.d);
            },
            error: function (dataError) {
                myReject(false);
            }
        });
    });
}

$(document).ready(function () {

    // //Get all rules and settings
    GetAllAttributes().then((values) => {
        _allAttributes = values;
        GetAllOperators().then((values) => {
            _allOperators = values;
            GetAllControlValues().then((values) => {
                _allcontrolValues = values;

                RulesUI_Render();
            });
        });
    });

    FirstLoad = true;

    $('[ps-name="btnSaveRule"]').on('click', function (e) {
        SaveRules();
    });
});

function RulesUI_Render() {
    //Get all rules and settings
    _getRulesByMerchant()
        .then((jsonData) => {
            if (jsonData.length > 0) {
                jsonData.forEach(function (item, indexUp) {

                    let _idTable = AddRule();

                    $("#" + _idTable + " tbody tr .inputRuleName").val(item.Name).attr("id", "RuleID_" + item.Id);
                    $("#" + _idTable).attr("ps-ruleid", item.Id)

                    let _IDButtonAdd = $("#" + _idTable + " tbody tr:last td:last a").attr("id");
                    item.RuleDetails.forEach((item, index) => {
                        //AddAttribute(_tableId)
                        if (index > 0) {
                            $("#" + _IDButtonAdd).click();
                        }
                        var _tableId = "tableAttributes_" + indexUp + "_" + (Number(index) + 1);
                        SetOperatorOptionsByAttribute(item.AttributeId, _tableId);
                    });
                });
                return (jsonData);
            }
        })
        .then((jsonData) => {
            setTimeout(() => {
                if (jsonData.length > 0) {
                    jsonData.forEach(function (item, indexUp) {
                        if (item.Id == 0) {
                            $("#TableRuleID" + indexUp).addClass("ruleIdcss_" + item.Id);
                        } else {
                            $("#TableRuleID" + indexUp).addClass("ruleIdcss_" + item.Id).addClass("hasRuleID");
                            $("#TableRuleID" + indexUp + " .hoverChange").hide();
                        }
                        item.RuleDetails.forEach((itemDetail, index) => {

                            var _tableId = "tableAttributes_" + indexUp + "_" + (Number(index) + 1);
                            $("#" + _tableId).addClass("ruleDetail_" + itemDetail.Id);
                            $("#" + _tableId).attr("ps-ruleid", item.Id).attr("ps-detailid", itemDetail.Id);
                            $("#SelectAttribute_" + _tableId + " option[value=" + itemDetail.AttributeId + "] ").attr("selected", "selected");
                            $("#SelectOperator_" + _tableId + " option[value=" + itemDetail.OperatorId + "] ").attr("selected", "selected");
                            if (itemDetail.UIControlId == 2) {
                                $("#inputValueAttribute_" + _tableId).attr("ps-uicontrolid", "2").val(itemDetail.UIControlValue);
                            } else {
                                $("#SelectValueControl_" + _tableId).attr("ps-uicontrolid", "1");
                                $("#SelectValueControl_" + _tableId + " option[value=" + itemDetail.ControlValueId + "] ").attr("selected", "selected");
                            }
                        });
                        UpdateBehaviorTable(indexUp, false);
                    });
                }
            }, 900);
        }).then(() => {
            UpdateSubscribeButton();
        });
}

function SaveRules(tableId) {
    if (validateRules(tableId)) {
        return;
    }
    var rulesEdit = [];
    var rulesAdd = [];


    var obj = {};
    obj.Id = $("#TableRuleID" + tableId).attr("ps-ruleid");
    obj.OrderId = $("#TableRuleID" + tableId).find('[name="orderid"]').attr("ps-order");
    obj.Name = $("#TableRuleID" + tableId).find(".inputRuleName").val();
    obj.RuleDetails = [];

    $("#TableRuleID" + tableId).find(".tableAttribute").each((id, eld) => {
        var detail = {};
        detail.Id = $(eld).find('[ps-detailid]').attr('ps-detailid');
        detail.ProdcutRuleId = obj.Id;
        detail.OrderId = $(eld).attr('ps-detailorderid');
        detail.AttributeId = $(eld).find('[name="Attribute"]').val();
        detail.OperatorId = $(eld).find('[name="Operator"]').val();
        detail.UIControlId = $(eld).find('[name="Value"]').attr("ps-uicontrolid");
        detail.UIControlValue = "";
        detail.ControlValueId = 0;
        if (detail.UIControlId == 2) {
            detail.UIControlValue = $(eld).find('[name="Value"]').val();
        } else {
            detail.ControlValueId = $(eld).find('[name="Value"]').val();
        }
        //if is black
        if (detail.OperatorId == 5) {
            detail.UIControlValue = "";
            detail.ControlValueId = 0;
        }
        if (!detail.Id) {
            detail.id = 0
        }
        obj.RuleDetails.push(detail);
    });

    if (!obj.Id || obj.Id == "0") {
        obj.Id = 0
        rulesAdd.push(obj);
    } else {
        rulesEdit.push(obj);
    }


    _promiseSaveAllRules({
        RulesDelete: [],
        RulesEdit: rulesEdit,
        RulesAdd: rulesAdd,
        DetailsDelete: [],
    }).then((rs) => {
        if (rs && rs.RulesAdd.length > 0) {
            $("#TableRuleID" + tableId).addClass("ruleIdcss_" + rs.RulesAdd[0].Id).addClass("hasRuleID").attr("ps-ruleid", rs.RulesAdd[0].Id)
            for (var i = 0; i < rs.RulesAdd[0].RuleDetails.length; i++) {
                var aItem = rs.RulesAdd[0].RuleDetails[i];
                $("#TableRuleID" + tableId).find("[ps-detailorderid='" + aItem.OrderId + "']").find(".table")
                    .addClass("ruleDetail_" + aItem.Id)
                    .attr("ps-ruleid", aItem.ProductRuleId)
                    .attr("ps-detailid", aItem.Id)
            }
        }
        if (rs && rs.RulesEdit.length > 0) {
            for (var i = 0; i < rs.RulesEdit[0].RuleDetails.length; i++) {
                var eItem = rs.RulesEdit[0].RuleDetails[i];
                $("#TableRuleID" + tableId).find("[ps-detailorderid='" + eItem.OrderId + "']").find(".table")
                    .addClass("ruleDetail_" + eItem.Id)
                    .attr("ps-ruleid", eItem.ProductRuleId)
                    .attr("ps-detailid", eItem.Id)
            }
        }
        UpdateBehaviorTable(tableId);
        $("#TableRuleID0 .hoverChange").hide();
    });
}

function validateRules(tableId) {
    $("#TableRuleID" + tableId).find("input").removeClass("error");
    $("#TableRuleID" + tableId).find("select").removeClass("error");
    var mError = false;

    $("#TableRuleID" + tableId).find("[name='RuleName']:visible").each((i, el) => {
        var mVal = $(el).val().trim();
        if (!mVal) {
            $(el).addClass("error");
            mError = true;
        }
    });

    $("#TableRuleID" + tableId).find("[name='Attribute']:visible").each((i, el) => {
        var mVal = $(el).val();
        if (!mVal) {
            $(el).addClass("error");
            mError = true;
        }
    });

    $("#TableRuleID" + tableId).find("[name='Operator']:visible").each((i, el) => {
        var mVal = $(el).val();
        if (!mVal) {
            $(el).addClass("error");
            mError = true;
        }
    });

    $("#TableRuleID" + tableId).find("[name='Value']:visible").each((i, el) => {
        var operatorId = $(el).closest('tr').find("[name='Operator']:visible").val();
        if (operatorId == 5) {
            var elType = $(el).prop('nodeName');
            if (elType == 'SELECT') {
                $(el).val(0);
            } else {
                $(el).val("");
            }
        } else {
            var mVal = $(el).val();
            if (!mVal) {
                $(el).addClass("error");
                mError = true;
            }
        }
    });
    return mError;
}

function UpdateBehaviorTable(tableId) {
    $("#firstbuttonAdd_" + tableId).after(' <a   class="btn btn-outline-warning btn-sm" id="firstbuttonEdit_' + tableId + '" onclick="EditRuleAttributes(\'' + tableId + '\')"> <i class="fa fa-pencil-square-o fa-2x" aria-hidde="true"></i></a> ');
    $("#TableRuleID" + tableId + " select").prop("disabled", true);
    $("#TableRuleID" + tableId + "  input").prop("disabled", true);
    $("#firstbuttonAdd_" + tableId).hide();
    $("#firstbuttonSave_" + tableId).hide();
    $("#firstbuttonDelete_" + tableId).hide();
    $("#firstbuttonEdit_" + tableId).show();
    UpdateSubscribeButton();
}
function EditRuleAttributes(tableID) {
    $("#firstbuttonAdd_" + tableID).show();
    $("#firstbuttonSave_" + tableID).show();
    if ($("#TableRuleID" + tableID + " .tableAttribute").length > 1) {
        $("#firstbuttonDelete_" + tableID).show();
    }
    $("#TableRuleID" + tableID + "  select").prop("disabled", false);
    $("#TableRuleID" + tableID + "  input").prop("disabled", false);
    $("#firstbuttonEdit_" + tableID).hide();

}

function SetSelectOptionsAttribute(tableRuleID) {
    _promiseSelectAttribute(tableRuleID).then(function (Options) {
        $("#" + tableRuleID).find("tr:last td:first").append(Options);
    });
}
function SetOperatorOptionsByAttribute(attributeID, tableRuleID) {
    $("#SelectAttribute_" + tableRuleID).removeClass("error");
    _promiseSelectOperatorsByAttributeID(attributeID, tableRuleID).then((selectOperators) => {
        $("#tdOperator_" + tableRuleID + " select").remove();
        $("#tdOperator_" + tableRuleID).append(selectOperators);
    });
    _promiseSelectControlValuesByAttributeID(attributeID, tableRuleID).then((valuesOptions) => {
        $("#tdValue_" + tableRuleID + " select").remove();
        $("#tdValue_" + tableRuleID + " input").remove();
        $("#tdValue_" + tableRuleID).append(valuesOptions);
    });
}

function DeleteRule(tableID) {
    var z = confirm("Are you sure you want to delete this Rule?");
    if (z == true) {
        if ($(".hasRuleID").not(".desactiveTable").length == 1 && $(".hasRuleID").not(".desactiveTable").prop("id") == tableID) {
            alert("You must have at leat 1 rule added.");
            return;
        } else if ($(".rulecss").not(".desactiveTable").length == 1) {
            alert("You must have at leat 1 rule added.");
            return;
        }


        var obj = {};
        obj.Id = $("#" + tableID).attr("ps-ruleid");
        obj.RuleDetails = [];

        $("#" + tableID).find(".tableAttribute").each((id, eld) => {
            var detail = {};
            detail.Id = $(eld).find('[ps-detailid]').attr('ps-detailid');
            if (detail.id) {
                obj.RuleDetails.push(detail);
            }
        });

        if (!obj.Id || obj.Id == "0") {
            $("#" + tableID).hide().addClass("desactiveTable");

            toggleBtnDelete();
            toggleBtnAddRule();

            $(".rulecss").not(".desactiveTable").each(function (index, el) {
                $(el).find("tr:first td:first").attr("name", "orderid").attr("ps-order", "" + (index + 1)).html("<b>Rule #" + (index + 1) + "</b>");
            });
        } else {
            var rulesDelete = [];
            rulesDelete.push(obj);

            _promiseSaveAllRules({
                RulesDelete: rulesDelete,
                RulesEdit: [],
                RulesAdd: [],
                DetailsDelete: [],
            }).then(() => {
                $("#" + tableID).hide().addClass("desactiveTable");

                toggleBtnDelete();
                toggleBtnAddRule();

                $(".rulecss").not(".desactiveTable").each(function (index, el) {
                    $(el).find("tr:first td:first").attr("name", "orderid").attr("ps-order", "" + (index + 1)).html("<b>Rule #" + (index + 1) + "</b>");
                });
            });
        }
    }
}
function DeleteAttribute(tableID) {
    let _attributes = $("#TableRuleID" + tableID + " .tableAttribute").length;
    if (_attributes > 1) {
        let detailid = $("#TableRuleID" + tableID + "  .tableAttribute:last table").attr("ps-detailid");
        if (detailid) {
            var DeleteRuleDetailIDs = [];
            DeleteRuleDetailIDs.push(detailid);
            _promiseSaveAllRules({
                RulesDelete: [],
                RulesEdit: [],
                RulesAdd: [],
                DetailsDelete: DeleteRuleDetailIDs,
            }).then(() => {
                $("#TableRuleID" + tableID + " .tableAttribute:last").remove();

                $("#firstbuttonAdd_" + tableID).show();
                if ($("#TableRuleID" + tableID + " .tableAttribute").length == 1) {
                    $("#firstbuttonDelete_" + tableID).hide();
                }

                $("#TableRuleID" + tableID + " .tableAttribute").each(function (index, el) {
                    $(el).attr("ps-detailorderid", "" + (index + 1));
                });
            });
        } else {
            $("#TableRuleID" + tableID + " .tableAttribute:last").remove();

            $("#firstbuttonAdd_" + tableID).show();
            if ($("#TableRuleID" + tableID + " .tableAttribute").length == 1) {
                $("#firstbuttonDelete_" + tableID).hide();
            }

            $("#TableRuleID" + tableID + " .tableAttribute").each(function (index, el) {
                $(el).attr("ps-detailorderid", "" + (index + 1));
            });
        }
    }
}

function AddRule() {
    var _table = $("#tableDemo").clone().removeClass("error").removeClass("demo");
    _table.find("input").removeClass("error").removeClass("demo").removeAttr("hidden");
    let _cssNumer = Number($(".rulecss").length);
    let _numTables = $(".rulecss").not(".desactiveTable").length + 1;
    _table.attr("Id", "TableRuleID" + _cssNumer).addClass("rulecss").removeAttr("hidden")
        .find("tr:first td:first").attr("name", "orderid").attr("ps-order", "" + _numTables).html("<b>Rule #" + (_numTables) + "</b>");
    _table.find("tr:last td:last")
        .append('<a  class="btn btn-outline-danger btn-sm btnDeletecss" onclick="DeleteRule(\'TableRuleID' + _cssNumer + '\')"> <i class="fa fa-times fa-2x " aria-hidde="true"></i> </a>');

    let rTable;
    if (_numTables <= _MaxRules) {
        if ($(".rulecss").length == 0) {
            $("#tableDemo").after(_table);
        } else {
            $(".rulecss").last().after(_table);
        }
        $(".desactiveTable").hide();
        $(".rulecss").not(".desactiveTable").show();
        addRuleOperations(1, _cssNumer);
        rTable = "TableRuleID" + _cssNumer;
    }
    toggleBtnDelete()
    toggleBtnAddRule();

    UpdateSubscribeButton();
    return rTable;
}
function AddAttribute(tableID) {

    let _attributePosotion = $("#TableRuleID" + tableID + " .tableAttribute").length;
    let _id = "tableAttributes_" + tableID + "_" + (Number(_attributePosotion) + 1);

    var _operations =
        '<tr id="trTable_' + _id + '" class="tableAttribute">								' +
        '	<td colspan="12">                                                               ' +
        '		<table id="' + _id + '"  class="table">                                     ' +
        '			<tr class="table-primary">                                              ' +
        '				<th colspan="3" style="width: 200px">Attribute</th>                 ' +
        '				<th colspan="3" style="width: 100px">Operator</th>                  ' +
        '				<th colspan="3" style="width: 200px">Value</th>                     ' +
        '			</tr>                                                                   ' +
        '			<tr>                                                                    ' +
        '				<td colspan="3" style="width: 200px">                               ' +
        '				</td>                                                               ' +
        '				<td id="tdOperator_' + _id + '" colspan="3" style="width: 100px">   ' +
        '					<select name="Operator" class="form-select form-select-sm mb-2">                ' +
        '					<option value="0" selected disabled>Select Operator</option>    ' +
        '					</select>                                                       ' +
        '				</td>                                                               ' +
        '				<td id="tdValue_' + _id + '" colspan="3" style="width: 200px">      ' +
        '					<input name="Value" type="text" class="form-control form-control-sm"/>       ' +
        '					<select name="Value" hidden>                                                 ' +
        '					<option value="0" selected disabled>Select value</option>       ' +
        '					</select>                                                       ' +
        '				</td>                                                               ' +
        '			</tr>																    ' +
        '		</table>																    ' +
        '	</td>																		    ' +
        '</tr>																			    ';

    $("#TableRuleID" + tableID + " tr:last").before(_operations);
    SetSelectOptionsAttribute(_id);
    let _attributes = $("#TableRuleID" + tableID + " .tableAttribute").length;

    if (_attributes == 1) {
        $("#firstbuttonAdd_" + tableID).show();
        $("#firstbuttonDelete_" + tableID).hide();
    } else if (_attributes > 1 && _attributes < 7) {
        $("#firstbuttonDelete_" + tableID).show();
    } else {
        $("#firstbuttonAdd_" + tableID).hide();
    }

    $("#TableRuleID" + tableID + " .tableAttribute").each(function (index, el) {
        $(el).attr("ps-detailorderid", "" + (index + 1));
    });
}
function EditAttribute(tableID) { }

function addRuleOperations(ruleID, tableID) {
    var _id = "tableAttributes_" + tableID + "_" + ruleID;
    var attributetext;
    if (tableID == 0) {
        attributetext = '<th colspan="3" style="width: 200px">Attribute</th> <div class="hoverChange">Default/Recommended Rule: A potential starting rule for enrollment has been populated below. You may change the rules desired for enrollment at your discretion. Please note that a minimum of one rule with a rule name must be selected in order to complete your enrollment request.</div>';
    }
    else {
        attributetext = '<th colspan="3" style="width: 200px">Attribute</th>                ';
    }

    var _operations =
        '<tr id="trTable_' + _id + '" class="tableAttribute">								' +
        '	<td colspan="12">                                                               ' +
        '		<table id="' + _id + '"  class="table">                                     ' +
        '			<tr class="table-primary" id="rdrmsg">                                  ' + attributetext +
        '				<th colspan="3" style="width: 100px">Operator</th>                  ' +
        '				<th colspan="3" style="width: 200px">Value</th>                     ' +
        '			</tr>                                                                   ' +
        '			<tr>                                                                    ' +
        '				<td colspan="3" style="width: 200px">                               ' +
        '				</td>                                                               ' +
        '				<td id="tdOperator_' + _id + '" colspan="3" style="width: 100px">   ' +
        '					<select name="Operator" class="form-select form-select-sm mb-2">                ' +
        '					<option value="0" selected disabled>Select Operator</option>    ' +
        '					</select>                                                       ' +
        '				</td>                                                               ' +
        '				<td id="tdValue_' + _id + '" colspan="3" style="width: 200px">      ' +
        '					<input name="Value" type="text" class="form-control form-control-sm"/>       ' +
        '					<select name="Value" hidden>                                                 ' +
        '					<option value="0" selected disabled>Select value</option>       ' +
        '					</select>                                                       ' +
        '				</td>                                                               ' +
        '			</tr>																    ' +
        '		</table>																    ' +
        '	</td>																		    ' +
        '</tr>																			    ';


    $("#TableRuleID" + tableID + " tr:last").after(_operations);

    SetSelectOptionsAttribute(_id);
    AddButtonAttribute(tableID, _id);

    $("#TableRuleID" + tableID + " .tableAttribute").each(function (index, el) {
        $(el).attr("ps-detailorderid", "" + (index + 1));
    });
}
function AddButtonAttribute(tableID, trID) {
    let _buttonsAttribute =
        '<tr id="trbutton_' + trID + '"> ' +
        '    <td colspan="3"></td>' +
        '    <td colspan="5"></td>' +
        '    <td colspan="3" style="text-align: end;">' +
        ' <a  class="btn btn-outline-success btn-sm" id="firstbuttonAdd_' + tableID + '" onclick="AddAttribute(\'' + tableID + '\')"> <i class="fa fa-plus fa-2x " aria-hidde="true"></i> </a> ' +
        ' <a  class="btn btn-outline-danger  btn-sm" id="firstbuttonDelete_' + tableID + '" onclick="DeleteAttribute(\'' + tableID + '\')"><i class="fa fa-trash-o fa-2x " aria-hidde="true"></i> </a></td >    ' +
        '</tr>';
    $("#trTable_" + trID).after(_buttonsAttribute);
    $("#firstbuttonAdd_" + tableID).after(' <a   class="btn btn-outline-primary btn-sm hidden" id="firstbuttonSave_' + tableID + '" onclick="SaveRules(\'' + tableID + '\')"> <i class="fa fa-save  fa-2x" aria-hidde="true"></i></a> ');
    $("#firstbuttonAdd_" + tableID).show();
    $("#firstbuttonDelete_" + tableID).hide();
}

function toggleBtnAddRule() {
    if ($(".rulecss").not(".desactiveTable").length >= _MaxRules) {
        $("#btnAddRule").hide();
    } else {
        $("#btnAddRule").show();
    }
}
function toggleBtnDelete() {
    if ($(".rulecss").not(".desactiveTable").length > 1) {
        $(".btnDeletecss").show();
    }
    if ($(".hasRuleID").not(".desactiveTable").length == 1) {
        $(".hasRuleID").not(".desactiveTable").find(".btnDeletecss").hide()
    } else if ($(".rulecss").not(".desactiveTable").length == 1) {
        $(".btnDeletecss").hide();
    }
}

function UpdateSubscribeButton() {
    let _cssNumer = Number($(".rulecss").length);
    let _tablesDesactive = Number($(".desactiveTable").length);
    let hasRules = (_cssNumer - _tablesDesactive) > 0 && $(".hasRuleID").length > 0;
    let subscribed = $("#ContentPlaceHolder1_wucProductSubscription1_TabControl_tmpl3_rdrGrid_ManageSubscription_0")[0].value != "Subscribe";

    $("#ContentPlaceHolder1_wucProductSubscription1_TabControl_tmpl3_rdrGrid_ManageSubscription_0")
        .prop("disabled", !hasRules)
        .after($('#ruletxtMsg').length == 0 ? "<br><i id='ruletxtMsg'>At least 1 Rule must be created in order to subscribe.<i/>" : "");
    $("#ContentPlaceHolder1_wucProductSubscription1_TabControl_tmpl3_wucMerchantProductRuleSetup1_SendChangesRules").toggle(hasRules && subscribed);
}