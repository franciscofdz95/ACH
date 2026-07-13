<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucQuickPartnerSearch.ascx.cs" Inherits="ZeusWeb.UserControls.SidePanel.wucQuickPartnerSearch" %>


    

<fieldset style="width: 320px;">
    <legend>Quick Search</legend>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnl" DefaultButton="btnSelect">
                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="QuickAgent" runat="server" />
                <table cellspacing="2">
                    <tr>
                        <td align="right" valign="top">
                            ID:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="AgentID" runat="server" Width="110px" ValidationGroup="QuickAgent"></asp:TextBox>
                            <asp:RangeValidator ID="RangeValidator1" ValidationGroup="QuickAgent" runat="server"
                                ControlToValidate="AgentID" ErrorMessage="Please enter a valid Partner ID." MaximumValue="100000"
                                MinimumValue="1" Type="Integer" Display="None"></asp:RangeValidator>
                        </td>
                        <td align="right" valign="top">
                            <label for='<%= tbDBA.ClientID %>'>
                                DBA:
                            </label>
                        </td>
                        <td align="left">
                            <div class="ui-widget">
                                <asp:TextBox runat="server" Width="110px" ID="tbDBA"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top">
                            First:
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbFirst" runat="server" Width="110px" ValidationGroup="QuickAgent"></asp:TextBox>
                        </td>
                        <td align="right" valign="top">
                            Last:
                        </td>
                        <td align="left">
                            <asp:TextBox runat="server" Width="110px" ID="tbLast"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">

                            <div style="display:none">
                            <asp:TextBox runat="server" ID="hidAgentUID" />
                                </div>

                            <asp:LinkButton ID="btnSelect" runat="server" Text="Search" ValidationGroup="QuickAgent"
                                OnClick="btnSelect_Click" />
                            &nbsp;
                            <asp:LinkButton ID="btnClear" runat="server" Text="Clear" CausesValidation="false"
                                OnClick="btnClear_Click" />&nbsp;
                        </td>
                    </tr>
                </table>
           
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
         


    <script type="text/javascript">
        $(document).ready(function () {
            load();
          
        });


        //        this is a really stupid bug. basically, when you're using jquery calls mixed with microsoft ajax (ie update panel stuff), the jquery calls dont work
        //        anymore after and update panel is triggered. because the jquery does not know when the request ended. so the fix is to manually call the jquery code
        //        when the end request event is fired by the update panel.
        //        
        //        http://zeemalik.wordpress.com/2007/11/27/how-to-call-client-side-javascript-function-after-an-updatepanel-asychronous-ajax-request-is-over/

        function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }

        function EndRequestHandler() {
            
                /* begin block */

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
                             url: "../ajax/AjaxWebservice.asmx/GetAgentDBA",
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

                                 //alert(mytags);

                                 $('#<%= tbDBA.ClientID %>').autocomplete({
                                     source: mytags,
                                     select: function (event, ui) {

                                         event.preventDefault();

                                         var dbaname = ui.item.value;

                                         var arr = dbaname.split(":");


                                         $('#<%= tbDBA.ClientID %>').val($.trim(arr[1]));
                                            $('#<%= AgentID.ClientID %>').val($.trim(arr[0]));

                                        }
                                 });

                             }

                         });
                        }

                /* end block */

                /* begin block */

                    $('#<%= tbFirst.ClientID %>').on("keydown", function (e) {

                    if (e.keyCode == '13') {
                        e.preventDefault();
                    }

                    //alert(e.which);
                    //backspace and delete
                    if (e.keyCode == '8' || e.keyCode == '46') {
                        $('#<%= tbFirst.ClientID %>').val('');
                         }

                        });

                     $('#<%= tbFirst.ClientID %>').bind({
                    cut: function () {
                        $('#<%= tbFirst.ClientID %>').val('');
                         }
                     });

                $('#<%= tbFirst.ClientID %>').on("keyup",function (e) {
                    SearchAgentFirst();
                });

                function SearchAgentFirst() {
                    var fragment = $('#<%= tbFirst.ClientID %>').val();

                         $.ajax({
                             type: "POST",
                             url: "../ajax/AjaxWebservice.asmx/GetAgentFirst",
                             data: "{FirstFragment: '" + fragment + "'}",
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

                                 $('#<%= tbFirst.ClientID %>').autocomplete({
                                     source: mytags,
                                     select: function (event, ui) {

                                         event.preventDefault();

                                         var firstname = ui.item.value;

                                         var arr = firstname.split(":");

                                         $('#<%= tbFirst.ClientID %>').val($.trim(arr[1]));
                                            $('#<%= AgentID.ClientID %>').val($.trim(arr[0]));

                                        }
                                 });

                             }

                         });
                        }

                /* end block */

                /* begin block */

                    $('#<%= tbLast.ClientID %>').on("keydown", function (e) {

                    if (e.keyCode == '13') {
                        e.preventDefault();
                    }

                    //alert(e.which);
                    //backspace and delete
                    if (e.keyCode == '8' || e.keyCode == '46') {
                        $('#<%= tbLast.ClientID %>').val('');
                         }

                        });

                     $('#<%= tbLast.ClientID %>').bind({
                    cut: function () {
                        $('#<%= tbLast.ClientID %>').val('');
                         }
                     });

                $('#<%= tbLast.ClientID %>').on("keyup", function (e) {
                    SearchAgentLast();
                });

                function SearchAgentLast() {
                    var fragment = $('#<%= tbLast.ClientID %>').val();

                         $.ajax({
                             type: "POST",
                             url: "../ajax/AjaxWebservice.asmx/GetAgentLast",
                             data: "{LastFragment: '" + fragment + "'}",
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

                                 $('#<%= tbLast.ClientID %>').autocomplete({
                                     source: mytags,
                                     select: function (event, ui) {

                                         event.preventDefault();

                                         var lastname = ui.item.value;

                                         var arr = lastname.split(":");

                                         $('#<%= tbLast.ClientID %>').val($.trim(arr[1]));
                                            $('#<%= AgentID.ClientID %>').val($.trim(arr[0]));

                                        }
                                 });

                             }

                         });
                        }

                /* end block */
                   
        }

        // for first load.
        this.EndRequestHandler();

    </script>

</fieldset>