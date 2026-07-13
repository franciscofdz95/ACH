<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucMerchantProductRuleSetup.ascx.cs" Inherits="ZeusWeb.UserControls.wucMerchantProductRuleSetup" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<link href="../css/StyleCustom.css" rel="stylesheet" />
<script src="../js/ProductsRules.js"></script>
<link href="../css/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet" />

<table width="100%" border="0" cellpadding="0" cellspacing="0" class="ps">
    <tr>
        <td style="vertical-align: top;" colspan="2" align="center">
            <fieldset style="min-height: 600px; width:95%" class="dialog">
                <legend>Rules Setup</legend>
                <div class="container container-fluid">
                    <div class="row">
                        <div class="col-11 text-left">
                            <a id="btnAddRule" class="btn btn-outline-success btn-sm" onclick="AddRule()">
                                <i class="fa fa-plus fa-1x " aria-hidde="true"></i>Add
                            </a>                       
                            <asp:LinkButton ID="SendChangesRules" CssClass="btn btn-sm btn-outline-success pull-right" runat="server" OnClick="SendChangesRules_Click" OnClientClick="return confirm('You are about send to these rule changes to Verifi. Please press OK to continue.');"><span class="fa fa-send"></span> Send Changes</asp:LinkButton>
                        </div>
                        
                    </div>
                </div>
                <hr />

                <table id="tableDemo" class="container-lg" width="100%" border="0" hidden>
                    <tr>
                        <td colspan="3"></td>
                        <td colspan="7"></td>
                        <td colspan="5"></td>

                    </tr>
                    <tr>
                        <td colspan="3" style="width: 130px">
                            <label><b>Rule Name : </b></label>
                        </td>
                        <td colspan="7">
                            <input name='RuleName' type="text" class="form-control form-control-sm  inputRuleName" style="width: 380px" maxlength="30"/>
                        </td>
                        <td colspan="5" style="text-align: end;"></td>
                    </tr>
                </table>

            </fieldset>
        </td>
    </tr>
</table>


