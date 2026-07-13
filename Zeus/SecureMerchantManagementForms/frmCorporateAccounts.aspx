<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmCorporateAccounts" Title="Corporate Account" CodeBehind="frmCorporateAccounts.aspx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucSelectMerchant.ascx" TagName="wucSelectMerchant"
    TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link rel="Stylesheet" type="text/css" href="../css/modalPopLite.css" />
    <script type="text/javascript" src="../js/modalPopLite.js"></script>
    <script type="text/javascript" src="../js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="../js/jquery.dataTables.fnReloadAjax.js"></script>
    <script type="text/javascript" src="../js/jquery.blockUI.js"></script>

    <script language="javascript" type="text/jscript">

        var _modal;

        $(document).ready(function () {
            _modal = $('#popup-wrapper').modalPopLite({ isModal: true });

            $('#EnableMCP').on("click", function () {
                confirmEnableMCP();
            });
        });


        function removeProcessingMerchant(zid) {
            alert(zid);
        }

        function confirmEnableMCP() {
            var conf;
            if ($('#EnableMCP').prop('checked')) {
                conf = confirm("Are you sure you want to enable MCP for this merchant account?");

                if (conf) {
                    return true;
                } else {
                    $('#EnableMCP').prop('checked', false);
                    return false;
                }
            } else {
                conf = confirm("Are you sure you want to disable MCP for this merchant account?");

                if (conf) {
                    return true;
                } else {
                    $('#EnableMCP').prop('checked', true);
                    return false;
                }
            }
        }
        function closeModal() {
            _modal[0].closeModal();
        }

        function openModal() {
            _modal[0].openModal();
        }

        function searchMcp() {
        }

        function addProcessing() {
            var conf = confirm("Are you sure you want to add ZID " + $('#MCPZID').val() + " as a MCP merchant?");
            return conf;
        }

        function removeProcessing() {
        }

        function selectProcessingMerchant(zid) {
            _modal[0].closeModal();
            $('#MCPZID').val(zid);
        }

        function clearSearchParams() {
            $('.clr').val('');
        }
    </script>

    <div id="contentpage">            
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlTools" runat="server">
        </asp:Panel>
        <uc4:wucbusinessinfo id="WucBusinessInfo1" runat="server" />
        <br />

        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <table width="100%">
            <tr>
                <td valign="top" style="width: 49%">
                    <asp:Panel ID="pnlCorporateAccounts" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                        <fieldset>
                            <legend>PMLE Accounts</legend>

                            <asp:Panel runat="server" ID="pnlCorporate">
                                Enter ZID:
                                        <asp:TextBox ID="txtZID" runat="server" Width="75px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtZID"
                                    Display="None" ErrorMessage="Please enter ZID or use the Lookup link to select a merchant."></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtZID"
                                    Display="None" ErrorMessage="Invalid ZID" MaximumValue="2147483640" MinimumValue="1"
                                    Type="Integer"></asp:RangeValidator>
                                <asp:HiddenField ID="HookTableKeyUID" runat="server"></asp:HiddenField>
                                <asp:LinkButton ID="btnAdd" runat="server" OnClick="btnAdd_Click">Add</asp:LinkButton>
                                |
                                        <asp:LinkButton ID="btnAddCorporate" runat="server" OnClientClick="ShowHookTable();"
                                            CausesValidation="False">Lookup Merchant</asp:LinkButton>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlNoCorporate">
                                This account is not part of a corporate structure.
                            </asp:Panel>

                            <asp:GridView CssClass="mGrid" ID="grdFamily" runat="server" OnRowDataBound="grdFamily_RowDataBound" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MerchantAppID" HeaderText="ZID" />
                                    <asp:TemplateField HeaderText="DBA Name">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hyp1" runat="server" Text='<%# Bind("BusinessDBAName") %>'></asp:HyperLink>
                                            <asp:Label ID="lDBA" runat="server" Text='<%# Bind("BusinessDBAName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkRemove" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to unlink this ZID?')" OnClick="btnRemove_Click" CommandArgument='<%# Bind("MerchantAppID") %>'>Remove</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>


                        </fieldset>
                    </asp:Panel>
                </td>
                <td valign="top" style="width: 49%">
                    <asp:Panel ID="pnlMCPAccounts" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <fieldset>
                                    <legend>Multi-Currency Processing</legend>
                                    <table>
                                        <tr runat="server" id="trchkMCP" visible="true">
                                            <td>
                                                <asp:CheckBox runat="server" ID="EnableMCP" Text="Enable Multi-Currency Processing" ClientIDMode="Static" OnCheckedChanged="EnableMCP_CheckedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr runat="server" id="trSearchMCP" visible="false">
                                            <td>Enter ZID:<asp:TextBox ID="MCPZID" ClientIDMode="Static" runat="server" MaxLength="9" Width="75px"></asp:TextBox>
                                                <asp:LinkButton runat="server" Text="Add" OnClientClick="addProcessing()" ID="lnkAddMCP" OnClick="lnkAddMCP_Click" CausesValidation="false"></asp:LinkButton>
                                                |
                                        <a href="javascript:openModal()">Lookup Merchant</a>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label runat="server" ID="lblAddMessage" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:GridView ID="grdProcessingMerchants" runat="server" AutoGenerateColumns="False" OnRowCommand="grdProcessingMerchants_RowCommand"
                                                    Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                    AlternatingRowStyle-CssClass="alt" ClientIDMode="Static" OnRowDataBound="grdProcessingMerchants_RowDataBound">
                                                    <Columns>
                                                        <asp:BoundField DataField="MCPIndicator" HeaderText="Type" />
                                                        <asp:BoundField DataField="ZID" HeaderText="ZID">
                                                            <ItemStyle Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="FMAID" HeaderText="FMA">
                                                            <ItemStyle Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                DBA
                                                            </HeaderTemplate>
                                                            <ItemStyle Width="125px" />
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hyp1" runat="server" Text='<%# Bind("BusinessDBAName") %>'></asp:HyperLink>
                                                                <asp:Label ID="lDBA" runat="server" Text='<%# Bind("BusinessDBAName") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Currency" HeaderText="Currency">
                                                            <ItemStyle Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkRemoveMCP" Text="Remove" CommandName="RemoveMCP" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to remove this MCP merchant?')"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </td>
            </tr>

        </table>
        <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="375px" Width="500px"
            Modal="True" InitialLocation="Centered" WindowState="Hidden">
            <ContentPane>
                <Template>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <uc1:wucSelectMerchant ID="WucSelectMerchant1" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Template>
            </ContentPane>
            <Header CaptionText="Merchant Search">
            </Header>
        </ig:WebDialogWindow>

        <div id="popup-wrapper" style="width: 500px; padding: 0px;">
            <div class="ig_Header igdw_HeaderArea" style="overflow: hidden;">
                <table cellpadding="0" cellspacing="0" style="width: 100%; height: 100%; table-layout: fixed;">
                    <tr>
                        <td class="igdw_HeaderCornerLeft">&nbsp;</td>
                        <td class="igdw_HeaderContent" style="white-space: nowrap; overflow: hidden;"><span class="igdw_HeaderCaption">Merchant Search</span></td>
                        <td class="igdw_HeaderContent igdw_HeaderButtonArea" style="width: 15px; text-align: right; white-space: nowrap;">
                            <img alt="Close" src="../ig_res/Default/images/igdw_Close.gif" onclick="closeModal()" /></td>
                        <td class="igdw_HeaderCornerRight">&nbsp;</td>
                    </tr>
                </table>
            </div>
            <div class="dialog" style="padding-top: 10px; padding-bottom: 5px;">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <legend>Merchant Search</legend>
                            <table>
                                <tr>
                                    <td class="lblRight">FMA ID:</td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="clr" ID="FMAID" Width="125px" MaxLength="15"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Merchant DBA:</td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="clr" ID="MerchantDBA" Width="125px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">ZID:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="clr" ID="ZID" Width="125px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Legal Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" CssClass="clr" ID="LegalName" Width="125px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">MID:
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox runat="server" CssClass="clr" ID="MID" Width="125px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblSearchError" ForeColor="Red"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="3">
                                        <asp:Button runat="server" Text="Search" EnableViewState="false" CausesValidation="false" ID="btnSearchProcessing" OnClick="btnSearchProcessing_Click" />
                                        <input type="button" value="Clear" onclick="clearSearchParams()" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <fieldset>
                            <legend>Results</legend>
                            <div>
                                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                            </div>
                            <asp:GridView ID="grdProcMerchantResults" runat="server" AutoGenerateColumns="False"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" ClientIDMode="Static">
                                <Columns>
                                    <asp:BoundField DataField="ZID" HeaderText="ZID">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="FMAID" HeaderText="FMA">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA">
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Currency" HeaderText="Currency">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a href="javascript:selectProcessingMerchant('<%# Eval("ZID") %>')">Select</a>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function Field2Str(fieldvalue) {
            if (fieldvalue == null)
                return '';
            else
                return fieldvalue;
        }

        function ShowHookTable() {
            oWebDialogWindow2 = $find('<% =WebDialogWindow2.ClientID %>'); oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);
        }

    </script>
</asp:Content>
