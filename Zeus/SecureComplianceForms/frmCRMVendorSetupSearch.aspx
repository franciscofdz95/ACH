<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageCompliance.Master" AutoEventWireup="true" CodeBehind="frmCRMVendorSetupSearch.aspx.cs" Inherits="frmCRMVendorSetupSearch" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>


    <script type="text/javascript" language="javascript">


        function CheckNumeric() {
            var key;
            key = event.which ? event.which : event.keyCode;
            if ((key >= 48 && key <= 57) || key == 13) {
                event.returnValue = true;
            }
            else {
                alert("Please enter Numeric only");
                event.returnValue = false;
            }
        }

        function Open(N1, N2, A1, A2) {
            document.getElementById(N1).style.display = 'none';
            document.getElementById(A1).style.display = 'none';
            document.getElementById(N2).style.display = '';
            document.getElementById(A2).style.display = '';
        }

        function Close(N1, N2, A1, A2) {
            document.getElementById(N1).style.display = '';
            document.getElementById(A1).style.display = '';
            document.getElementById(N2).style.display = 'none';
            document.getElementById(A2).style.display = 'none';
        }

        $(document).ready(function () {
            loadTicketSearch();
           $('.filterNumeric').on('focusout', function (e) {
                var $this = $(this);
                $this.val($this.val().replace(/[^0-9]/g, ''));
            }).on('paste', function (e) {
                var $this = $(this);
                setTimeout(function () {
                    $this.val($this.val().replace(/[^0-9]/g, ''));
                }, 5);
            });
        });


        function loadTicketSearch() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ticketSearchRequestHandler);
        }

        function ticketSearchRequestHandler() {
            $('.zeustooltip').tooltip({
                content: function () {
                    return $(this).prop('title');
                }
            });
        }



    </script>
    <%--START: Replace CRM to TPP for PXP-8417 by Ali Khan --%>
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                        <legend>TPP Search</legend>
                        <asp:ValidationSummary runat="server" ID="validSum1" ShowMessageBox="true" ShowSummary="false" />
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td align="right">TPP Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CRMName"  runat="server"  Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">TPP ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CRMID" runat="server" Width="75px" EnableViewState="False" MaxLength="7" onKeyPress="CheckNumeric()" CssClass="filterNumeric"
                                            TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">TPP TYPE:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CRMType" runat="server" Width="90px">
                                            <asp:ListItem Selected="True" Text="CRM" Value="CRM" />
                                            <asp:ListItem Text="Gateway" Value="Gateway" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Created By:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="UserCreated" runat="server" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">PCI Validation Date:</td>
                                    <td>
                                        <asp:TextBox ID="PCIValidationDate" runat="server" Width="173px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calPCIValidationDate" runat="server" Enabled="True" PopupButtonID="imgPCIValidationDate"
                                            TargetControlID="PCIValidationDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgPCIValidationDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Last Scanned Date:</td>
                                    <td>
                                        <asp:TextBox ID="LastScannedDate" runat="server" Width="173px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calLastScannedDate" runat="server" Enabled="True" PopupButtonID="imgLastScannedDate"
                                            TargetControlID="LastScannedDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgLastScannedDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                    AccessKey="h" TabIndex="9">
                                                    <Appearance>
                                                        <Image Url="~/Images/Check.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                                <script type="text/javascript">
                                                    $('#ContentPlaceHolder1_btnSearch__5').on("click", function () {
                                                        $('#pnlBusy').css("display", "block");
                                                    });
                                                </script>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" CausesValidation="false"
                                                    AccessKey="l" TabIndex="10">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CausesValidation="false"
                                                    AccessKey="a" TabIndex="11">
                                                    <Appearance>
                                                        <Image Url="~/Images/add2.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </div>
                        </asp:Panel>
                    </fieldset>
                    <br />

                    <div style="display: none" id="pnlBusy">
                        <asp:Image runat="server" ID="imgBusy" Style="width: 30px;" ImageUrl="~/Images/loading.gif" /><br />
                        Searching...
                    </div>
                    <fieldset>
                        <legend>Search Results</legend>

                        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="CRMUID" OnRowDataBound="grd_RowDataBound"
                                OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                OnSorting="grd_Sorting" DataSourceID="odsCRM" ClientIDMode="Static" >
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField SortExpression="PriorityID" Visible="false">
                                        <ItemTemplate></ItemTemplate>
                                        <ItemStyle Width="40px" />
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkSort" runat="server" CommandName="Sort" CommandArgument="PriorityID">
                                                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/sortboth.png" BorderStyle="None" Width="5px" />
                                            </asp:LinkButton>
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField Visible="false">
                                        <ItemStyle Width="10px" />
                                        <ItemTemplate>
                                            <asp:Image ID="img" runat="server" AlternateText="Attention Required" ImageUrl="~/Images/msg.gif" ToolTip="Attention Required" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ID" SortExpression="CRMID">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypTID" runat="server" CssClass="zeustooltip" NavigateUrl='<%# "~/SecureComplianceForms/frmCRMVendorSetupDetail.aspx?CRMUID=" + Eval("CRMUID") + "&Adding=false"  %>' Text='<%# Eval("CRMID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CRMUID" HeaderText="UID" ItemStyle-CssClass="togle" SortExpression="CRMUID">
                                        <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CRMName" HeaderText="Name" ItemStyle-CssClass="togle" SortExpression="CRMName">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CRMType" HeaderText="TPP Type" ItemStyle-CssClass="togle" SortExpression="CRMType">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TPPCertifiedflag" HeaderText="TPP Certified flag" SortExpression="TPPCertifiedflag">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Comments" SortExpression="Description">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CertifiedDate" HeaderText="Status Date" SortExpression="CertifiedDate">
                                        <ItemStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AcceptTransactions" HeaderText="Accept Transactions" SortExpression="AcceptTransactions">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="PCIValidationDate" HeaderText="PCI Validation Date" SortExpression="PCIValidationDate">
                                        <ItemStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LastScannedDate" HeaderText="Last Scanned Date" SortExpression="LastScannedDate">
                                        <ItemStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CreatedDate"  HeaderText="Date Created" SortExpression="CreatedDate">
                                        <ItemStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UpdateDate"  HeaderText="Date Updated" SortExpression="UpdateDate">
                                        <ItemStyle Width="65px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CreatedBy" HeaderText="User Created" SortExpression="CreatedBy">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CRMUID" HeaderText="CRM UID" Visible="False" />
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsCRM" runat="server" SelectMethod="GetCRMPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetCRMPagingCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsCRM_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
                                            </asp:LinkButton>&nbsp;&nbsp;
                                        </td>
                                        <td align="right">Export:&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="right" style="width: 33%;"></td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>
                    </fieldset>
                    <br />
                </td>
            </tr>
        </table>
    </div>
     <%--END: Replace CRM to TPP for PXP-8417 by Ali Khan --%>
    <script type="text/javascript">
        ticketSearchRequestHandler();

    </script>
   
</asp:Content>