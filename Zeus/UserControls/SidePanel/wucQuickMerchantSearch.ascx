<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucQuickMerchantSearch.ascx.cs"
    Inherits="wucQuickMerchantSearch" %>
<fieldset style="width: 320px;">
    <legend>Quick Search</legend>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnl" DefaultButton="btnSelect">
                <table cellspacing="2">
                    <tr style="vertical-align:top;">
                        <td align="right">
                            ZID:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="ZID" runat="server" Width="110px" CssClass="qckclr"></asp:TextBox>
                        </td>
                        <td align="right">
                            <label for='<%= tbDBA.ClientID %>'>
                                DBA:
                            </label>
                        </td>
                        <td align="left">
                            <div class="ui-widget">
                                <asp:TextBox runat="server" Width="110px" ID="tbDBA" CssClass="qckclr"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr style="vertical-align:top;">
                        <td align="right">
                            MID:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="MID" runat="server" Width="110px" CssClass="qckclr"></asp:TextBox>
                        </td>
                        <td align="right">
                            Legal:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" Width="110px" ID="tbLegal" CssClass="qckclr"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="vertical-align:top;">
                        <td align="right">
                            FMAID:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="FMAID" runat="server" Width="110px" CssClass="qckclr" MaxLength="15"></asp:TextBox>
                        </td>
                        <td colspan="2">

                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="3">
                            <asp:Label runat="server" ID="lblErr" ClientIDMode="Static" SkinID="error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td colspan="3">
                            <asp:LinkButton ID="btnSelect" runat="server" Text="Search" OnClick="btnSelect_Click" CausesValidation="false" />
                            &nbsp;
                            <a href="javascript:clearSearch()">Clear</a>
                        </td>
                    </tr>
                </table>
                <script type="text/javascript">
                    $(document).ready(function () {

                        $('#<%= tbDBA.ClientID %>').on("keydown", function (e) {
                            if (e.keyCode == '13') {
                                e.preventDefault();
                            }

                            //alert(e.which);
                            //backspace and delete
                            if (e.keyCode == '8' || e.keyCode == '46') {
                                $('#<%= tbDBA.ClientID %>').val('');
                            }

                        });


                        $('#<%= tbDBA.ClientID %>').bind({
                            cut: function () {
                                $('#<%= tbDBA.ClientID %>').val('');
                            }
                        });



                        $('#<%= tbDBA.ClientID %>').on("keyup", function (e) {
                            SearchMerchantDBA();
                        });


                        function SearchMerchantDBA() {
                            var fragment = $('#<%= tbDBA.ClientID %>').val();

                            $.ajax({
                                type: "POST",
                                url: "../ajax/AjaxWebservice.asmx/GetMerchantDBA",
                                data: "{BusinessDBANameFragment: '" + fragment + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {

                                    //var availableTags = msg.d.split(",");

                                    var availableTags = msg.d;

                                    var mytags = [];

                                    for (var i = 0; i < availableTags.length; i++) {

                                        var arr = availableTags[i].split(':');

                                        mytags.push(arr[0] + ": " + arr[1]);
                                    }

                                    $('#<%= tbDBA.ClientID %>').autocomplete({
                                        source: mytags,
                                        select: function (event, ui) {

                                            event.preventDefault();

                                            var dbaname = ui.item.value;

                                            var arr = dbaname.split(":");

                                            $('#<%= tbDBA.ClientID %>').val($.trim(arr[1]));
                                            $('#<%= ZID.ClientID %>').val($.trim(arr[0]));
                                        }
                                    });

                                }

                            });
                        }



                        $('#<%= tbLegal.ClientID %>').on("keydown", function (e) {
                            if (e.keyCode == '13') {
                                e.preventDefault();
                            }

                            //alert(e.which);
                            //backspace and delete
                            if (e.keyCode == '8' || e.keyCode == '46') {
                                $('#<%= tbLegal.ClientID %>').val('');
                            }

                        });

                        $('#<%= tbLegal.ClientID %>').bind({
                            cut: function () {
                                $('#<%= tbLegal.ClientID %>').val('');
                            }
                        });

                        $('#<%= tbLegal.ClientID %>').on("keyup", function (e) {
                            SearchMerchantLegal();
                        });

                        function SearchMerchantLegal() {
                            var fragment = $('#<%= tbLegal.ClientID %>').val();

                            $.ajax({
                                type: "POST",
                                url: "../ajax/AjaxWebservice.asmx/GetMerchantLegal",
                                data: "{BusinessLegalNameFragment: '" + fragment + "'}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {

                                    //var availableTags = msg.d.split(",");

                                    var availableTags = msg.d;

                                    var mytags = [];

                                    for (var i = 0; i < availableTags.length; i++) {

                                        var arr = availableTags[i].split(':');

                                        mytags.push(arr[0] + ": " + arr[1]);
                                    }

                                    $('#<%= tbLegal.ClientID %>').autocomplete({
                                        source: mytags,
                                        select: function (event, ui) {

                                            event.preventDefault();

                                            var Legalname = ui.item.value;

                                            var arr = Legalname.split(":");

                                            $('#<%= tbLegal.ClientID %>').val($.trim(arr[1]));
                                            $('#<%= ZID.ClientID %>').val($.trim(arr[0]));
                                        }
                                    });

                                }

                            });
                        }



                    });

                    function clearSearch() {
                        $('.qckclr').val('');
                        $('#lblErr').html("");
                    }
                </script>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
