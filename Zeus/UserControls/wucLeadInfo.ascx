<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLeadInfo.ascx.cs" Inherits="wucLeadInfo" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<style type="text/css">
    input, select, textarea
    {
        box-sizing: border-box;
        -moz-box-sizing: border-box;
        -webkit-box-sizing: border-box;
    }
</style>

<script type="text/javascript">

    $(document.body).on('change', '#<%=Country.ClientID %>', function () {
        var country = $(this).val();
        if (country != "US") {
            $("#<%=State.ClientID %>").css("display", "none");
            $("#<%=Province.ClientID %>").css("display", "inline");
        }

        if (country == "US" || country == "") {
            $("#<%=State.ClientID %>").css("display", "inline");
            $("#<%=Province.ClientID %>").css("display", "none");
        }

    });

    function DisplayClosureCode() {

        var status = $("#<%=StatusID.ClientID %>").val();
        //Not Workable
        if (status == "6ed2aa9d-bcb0-4f04-9080-e3731628d585") {
            $("#<%=ClosureCode.ClientID %>").css("display", "inline");
            $("#<%=lblClosureCode.ClientID %>").css("display", "inline");
        }

        else {
            $("#<%=ClosureCode.ClientID %>").css("display", "none");
            $("#<%=lblClosureCode.ClientID %>").css("display", "none");
        }
    }

    $(document).ready(function () {
        $("#<%=StatusID.ClientID %>").ready(function () {
            DisplayClosureCode();

        });

        $("#<%=StatusID.ClientID %>").change(function () {
            DisplayClosureCode();
        });

    });


</script>

<asp:Panel ID="pnlLeadInfo" runat="server" Width="100%">

    <fieldset>
        <legend>Leads Information</legend>
        <div class="bucketbdy">
            <table id="TABLE1" cellspacing="1" width="100%">
                <tr>
                    <td class="lblRight">Status:
                    </td>
                    <td>
                        <asp:DropDownList ID="StatusID" runat="server" TabIndex="1" Width="125px">
                        </asp:DropDownList>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="StatusID" Display="None" ErrorMessage="Please select a Status" Operator="NotEqual" ValueToCompare="-1"></asp:CompareValidator>
                    </td>
                    <td class="lblRight">
                        <asp:Label ID="lblClosureCode" runat="server">Closure Reason: </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ClosureCode" runat="server" OnClientClick="DisplayClosureCode()" TabIndex="75" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">&nbsp;</td>
                    <td>&nbsp;</td>
                    <td class="lblRight"># of ZIDs: </td>
                    <td>
                        <asp:LinkButton ID="hypZID" runat="server" CausesValidation="false" OnClick="hypZID_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">DBA:
                    </td>
                    <td>
                        <asp:TextBox ID="DBAName" runat="server" MaxLength="50" Width="125px" TabIndex="2"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="DBA Name is required"
                            Display="None" ControlToValidate="DBAName"></asp:RequiredFieldValidator>
                    </td>
                    <td class="lblRight">
                        <asp:Label runat="server" Text="Assigned To:" Width="130px"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="AssignedUserID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="AssignedUserID_SelectedIndexChanged" TabIndex="80" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">LID:</td>
                    <td>
                        <asp:TextBox ID="LeadID" runat="server" Enabled="false" MaxLength="50" ReadOnly="true" TabIndex="160" Width="125px"></asp:TextBox>
                    </td>
                    <td class="lblRight">Probability of Close:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="FollowupStatusID" runat="server" TabIndex="210" Width="125px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Addr1:
                    </td>
                    <td>
                        <asp:TextBox ID="Address1" runat="server" MaxLength="50" Width="125px" TabIndex="10"></asp:TextBox>
                    </td>
                    <td class="lblRight">DBA Phone:
                    </td>
                    <td>
                      <%--  <ig:WebMaskEditor ID="PhoneNumber"  runat="server" TabIndex="90" Width="125px">
                        </ig:WebMaskEditor>--%>
                         <asp:TextBox ID="PhoneNumber" runat="server" MaxLength="20" Width="125px" TabIndex="90"></asp:TextBox>
                    </td>
                    <td class="lblRight">Lead Type: </td>
                    <td>
                        <asp:DropDownList ID="LeadTypeUID" runat="server" TabIndex="170" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">Processing Volume: </td>
                    <td>
                        <ig:WebNumericEditor ID="ProcessingVolume" runat="server" DataMode="Decimal" MaxValue="9999999999.99" NullText="0.00" TabIndex="220" ValueText="0" Width="125px"></ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Addr2:
                    </td>
                    <td>
                        <asp:TextBox ID="Address2" runat="server" MaxLength="50" Width="125px" TabIndex="20"></asp:TextBox>
                    </td>
                    <td class="lblRight">Fax Number:
                    </td>
                    <td>
                        <ig:WebMaskEditor ID="FaxNumber" runat="server" InputMask="##############################" PromptChar=" " ShowMaskOnFocus="False" TabIndex="100" Width="125px">
                        </ig:WebMaskEditor>
                    </td>
                    <td class="lblRight">Source:</td>
                    <td>
                        <asp:DropDownList ID="SourceID" runat="server" TabIndex="180" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td align="right">&nbsp;Future Volume:</td>
                    <td align="left">
                        <ig:WebNumericEditor ID="ExpectedNetRevenue" runat="server" DataMode="Decimal" MaxValue="9999999999.99" NullText="0.00" TabIndex="230" ValueText="0.00" Width="125px"></ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">City:
                    </td>
                    <td>
                        <asp:TextBox ID="City" runat="server" MaxLength="25" Width="125px" TabIndex="30"></asp:TextBox>
                    </td>
                    <td class="lblRight">Website: </td>
                    <td>
                        <asp:TextBox ID="Url" runat="server" MaxLength="50" TabIndex="120" Width="125px"></asp:TextBox>
                    </td>
                    <td class="lblRight" width="130px">Lead Origin:</td>
                    <td width="125px">
                        <asp:DropDownList ID="OriginID" runat="server" TabIndex="190" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight" width="130px">Previous Profit:
                    </td>
                    <td width="200px">
                        <ig:WebNumericEditor ID="OtherProvidersProfit" runat="server" ValueText="0" Width="125px" MaxValue="9999999999.99" TabIndex="240"
                            NullText="0.00" DataMode="Decimal">
                        </ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Country:
                    </td>
                    <td>
                        <asp:DropDownList ID="Country" runat="server" TabIndex="40" Width="125px" AutoPostBack="false">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">Proc. Currency:</td>
                    <td align="left">
                        <asp:DropDownList ID="Currency" runat="server" TabIndex="130" Width="125px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">Origin Details: </td>
                    <td>
                        <asp:TextBox ID="SourceDescription" runat="server" MaxLength="50" TabIndex="200" Width="125px"></asp:TextBox>
                    </td>
                    <td class="lblRight">Monthly Profit: </td>
                    <td>
                        <ig:WebNumericEditor ID="MonthlyProfit" runat="server" DataMode="Decimal" MaxValue="9999999999.99" NullText="0.00" TabIndex="250" ValueText="0" Width="125px"></ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        <asp:Label Width="130px" Text="State/Region:" runat="server"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="State" runat="server" Width="125px" TabIndex="50">
                            <asp:ListItem Value="--">--Select--</asp:ListItem>
                            <asp:ListItem Value="AL">AL</asp:ListItem>
                            <asp:ListItem Value="AK">AK</asp:ListItem>
                            <asp:ListItem Value="AZ">AZ</asp:ListItem>
                            <asp:ListItem Value="AR">AR</asp:ListItem>
                            <asp:ListItem Value="CA">CA</asp:ListItem>
                            <asp:ListItem Value="CO">CO</asp:ListItem>
                            <asp:ListItem Value="CT">CT</asp:ListItem>
                            <asp:ListItem Value="DE">DE</asp:ListItem>
                            <asp:ListItem Value="DC">DC</asp:ListItem>
                            <asp:ListItem Value="FL">FL</asp:ListItem>
                            <asp:ListItem Value="GA">GA</asp:ListItem>
                            <asp:ListItem Value="HI">HI</asp:ListItem>
                            <asp:ListItem Value="IA">IA</asp:ListItem>
                            <asp:ListItem Value="ID">ID</asp:ListItem>
                            <asp:ListItem Value="IL">IL</asp:ListItem>
                            <asp:ListItem Value="IN">IN</asp:ListItem>
                            <asp:ListItem Value="KS">KS</asp:ListItem>
                            <asp:ListItem Value="KY">KY</asp:ListItem>
                            <asp:ListItem Value="LA">LA</asp:ListItem>
                            <asp:ListItem Value="ME">ME</asp:ListItem>
                            <asp:ListItem Value="MD">MD</asp:ListItem>
                            <asp:ListItem Value="MA">MA</asp:ListItem>
                            <asp:ListItem Value="MI">MI</asp:ListItem>
                            <asp:ListItem Value="MN">MN</asp:ListItem>
                            <asp:ListItem Value="MO">MO</asp:ListItem>
                            <asp:ListItem Value="MS">MS</asp:ListItem>
                            <asp:ListItem Value="MT">MT</asp:ListItem>
                            <asp:ListItem Value="NE">NE</asp:ListItem>
                            <asp:ListItem Value="NV">NV</asp:ListItem>
                            <asp:ListItem Value="NH">NH</asp:ListItem>
                            <asp:ListItem Value="NJ">NJ</asp:ListItem>
                            <asp:ListItem Value="NM">NM</asp:ListItem>
                            <asp:ListItem Value="NY">NY</asp:ListItem>
                            <asp:ListItem Value="NC">NC</asp:ListItem>
                            <asp:ListItem Value="ND">ND</asp:ListItem>
                            <asp:ListItem Value="OH">OH</asp:ListItem>
                            <asp:ListItem Value="OK">OK</asp:ListItem>
                            <asp:ListItem Value="OR">OR</asp:ListItem>
                            <asp:ListItem Value="PA">PA</asp:ListItem>
                            <asp:ListItem Value="RI">RI</asp:ListItem>
                            <asp:ListItem Value="SC">SC</asp:ListItem>
                            <asp:ListItem Value="SD">SD</asp:ListItem>
                            <asp:ListItem Value="TN">TN</asp:ListItem>
                            <asp:ListItem Value="TX">TX</asp:ListItem>
                            <asp:ListItem Value="UT">UT</asp:ListItem>
                            <asp:ListItem Value="VT">VT</asp:ListItem>
                            <asp:ListItem Value="VA">VA</asp:ListItem>
                            <asp:ListItem Value="WA">WA</asp:ListItem>
                            <asp:ListItem Value="WV">WV</asp:ListItem>
                            <asp:ListItem Value="WI">WI</asp:ListItem>
                            <asp:ListItem Value="WY">WY</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="Province" runat="server" Style="display: none" Width="125px" TabIndex="51"></asp:TextBox>
                    </td>
                    <td class="lblRight">Submitted By:</td>
                    <td>
                        <asp:TextBox ID="SubmittedBy" runat="server" TabIndex="140" Width="125px"></asp:TextBox>
                    </td>
                    <td class="lblRight">Reference #: </td>
                    <td>
                        <asp:TextBox ID="ReferenceNumber" runat="server" MaxLength="50" TabIndex="205" Width="125px"></asp:TextBox>
                    </td>
                    <td class="lblRight">Base Rate %:</td>
                    <td>
                        <asp:TextBox ID="BaseRate" runat="server" MaxLength="50" Style="text-align: right;" TabIndex="260" Width="125px"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="BaseRate" Display="Dynamic" ErrorMessage="Enter valid Base Rate" MaximumValue="100000" MinimumValue="0" Type="Double">*</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Postal Code:

                    </td>
                    <td>
                        <asp:TextBox ID="ZipCode" runat="server" MaxLength="50" Width="125px" TabIndex="60"></asp:TextBox>
                    </td>
                    <td class="lblRight">Date Created:</td>
                    <td>
                        <asp:Label ID="DateCreated" runat="server" Text="Label" Width="150px"></asp:Label>
                    </td>
                    <td class="lblRight">User Created: </td>
                    <td>
                        <asp:Label ID="UserCreated" runat="server" Text="Label"></asp:Label>
                    </td>
                    <td class="lblRight">Chargeback Ratio %: </td>
                    <td>
                        <asp:TextBox ID="ChargebackRatio" runat="server" MaxLength="50" Style="text-align: right;" TabIndex="270" Width="125px"></asp:TextBox>
                        <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="ChargebackRatio" Display="Dynamic" ErrorMessage="Enter valid Chargeback Ratio" MaximumValue="100000" MinimumValue="0" Type="Double">*</asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Panel ID="AgentSelect" runat="server">
                            <uc1:AgentSelector ID="wucAgentSelector" runat="server" DBATabIndex="70" DBAWidth="125" IDTabIndex="150" IDWidth="85" LayoutStyle="horizontal" lblDBAWidth="130" lblIDWidth="130" />
                        </asp:Panel>
                    </td>
                    <td class="auto-style1">&nbsp;</td>
                    <td>
                        <asp:TextBox ID="LeadUID" runat="server" BorderStyle="None" Enabled="False" ForeColor="White" Visible="False"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="MerchantAppUID" runat="server" BorderStyle="None" Enabled="False" ForeColor="White" Visible="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </fieldset>
</asp:Panel>
<ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="400px" Width="400px"
    Modal="true" InitialLocation="centered" WindowState="Hidden">
    <ContentPane>
        <Template>
            <div class="title">
                Merchants List
                    <hr class="line">
            </div>
            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="MerchantAppUID"
                                OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True" OnSorting="grd_Sorting" DataSourceID="odsMerchants">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                        <ItemTemplate>
                                            <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                                                runat="server" ID="hypZID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="35px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsMerchants" runat="server" SelectMethod="GetMerchantAppsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetMerchantAppsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsMerchants_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </Template>
    </ContentPane>
    <Header CaptionText="List of Merchants">
    </Header>
</ig:WebDialogWindow>
