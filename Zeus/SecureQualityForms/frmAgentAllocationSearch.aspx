<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgentAllocationSearch.aspx.cs" Inherits="frmAgentAllocationSearch" MasterPageFile="~/MasterPageAllocations.Master" Title="Agent Allocation Search" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <style type="text/css">
        input, select, textarea {
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
    </script>
    <div id="contentpage" style="width: 1180px;">
        <table width="100%">
            <tr>
                <td>

                    <fieldset>
                        <legend>Agent Allocation Search</legend>
                        <asp:Label ID="lblMessage" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                        <asp:ValidationSummary runat="server" ID="validSum1" ShowMessageBox="true" ShowSummary="false" />
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">Created Begin:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedBeginDate" runat="server" Width="90px" TabIndex="1" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedBeginDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedBeginDate"
                                            TargetControlID="SearchCreatedBeginDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedBeginDate" runat="Server" AlternateText="Click to show calendar" TabIndex="2"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Created End:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedEndDate" runat="server" Width="90px" TabIndex="3" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedEndDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedEndDate"
                                            TargetControlID="SearchCreatedEndDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedEndDate" runat="Server" AlternateText="Click to show calendar" TabIndex="4"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Agent DBA:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentDBAName" runat="server" MaxLength="100" Width="75px" EnableViewState="False"
                                            TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Agent ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentID" runat="server" MaxLength="5" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="6"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Rep Type:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="RepType" runat="server" MaxLength="20" Width="100px" EnableViewState="False"
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">WFB Allocations:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="WFBAllocation" runat="server" MaxLength="4" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="8"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <%--<td class="lblRight">Date Created:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchDateCreated" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchDateCreated" runat="server" Enabled="True" PopupButtonID="imgSearchDateCreated"
                                            TargetControlID="SearchDateCreated">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchDateCreated" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>--%>
                                    <td class="lblRight">User Created:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserCreated" runat="server" MaxLength="30" Width="90px" EnableViewState="False"
                                            TabIndex="9"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Date Updated:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchDateUpdated" runat="server" Width="90px" TabIndex="10" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchDateUpdated" runat="server" Enabled="True" PopupButtonID="imgSearchDateUpdated"
                                            TargetControlID="SearchDateUpdated" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchDateUpdated" runat="Server" AlternateText="Click to show calendar" TabIndex="11"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">User Updated:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="UserUpdated" runat="server" MaxLength="30" Width="75px" EnableViewState="False"
                                            TabIndex="12"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Source Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SourceNames" runat="server" Width="100px" MaxLength="20" TabIndex="13"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Reserve %:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="ReservePercentage" runat="server" MaxLength="8" Width="100px" EnableViewState="False"
                                            TabIndex="14"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">BBVA Allocations:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BBVAAllocation" runat="server" MaxLength="4" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="15"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">CFG Allocation:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CFGAllocations" runat="server" MaxLength="30" Width="90px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Status:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cboStatus" runat="server" MaxLength="30" Width="90px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                    AccessKey="h" TabIndex="16">
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
                                                    AccessKey="l" TabIndex="17">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CausesValidation="false"
                                                    AccessKey="a" TabIndex="18">
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
                                            <asp:ListItem>100</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                            </table>

                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="AgentKeyID" OnRowDataBound="grd_RowDataBound"
                                OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                OnSorting="grd_Sorting" DataSourceID="odsAgentAllocations" ClientIDMode="Static">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField HeaderText="AgentKeyID">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypAgentKeyID" runat="server" CssClass="zeustooltip" NavigateUrl='<%# "~/SecureQualityForms/frmAgentAllocationDetail.aspx?AgentKeyID=" + Eval("AgentKeyID") + "&AgentID=" + Eval("AgentID") + "&SourceName=" + Eval("SourceName") + "&Adding=false"  %>' Text='<%# Eval("AgentKeyID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AgentID" HeaderText="Agent ID" ItemStyle-CssClass="togle" SortExpression="AgentID">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AgentDBADisplayName" HeaderText="Agent DBA Display Name" SortExpression="AgentDBADisplayName">
                                        <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RepType" HeaderText="Rep Type" ItemStyle-CssClass="togle" SortExpression="RepType">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM-dd-yy HH:mm tt}" HeaderText="Date Created" SortExpression="DateCreated">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateUpdated" DataFormatString="{0:MM-dd-yy HH:mm tt}" HeaderText="Date Updated" ItemStyle-CssClass="togle" SortExpression="DateUpdated">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserUpdated" HeaderText="User Updated" SortExpression="UserUpdated">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <%-- <asp:BoundField DataField="SortOrder" HeaderText="Sort Order" ItemStyle-CssClass="togle">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>--%>
                                    <asp:BoundField DataField="CFGAllocations" HeaderText="CFG Allocation" SortExpression="CFGAllocations">
                                        <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AllocationBBVA" HeaderText="BBVA Allocation" SortExpression="AllocationBBVA">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Allocation" HeaderText="WF Allocation" SortExpression="Allocation">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SourceName" HeaderText="Source Name" SortExpression="SourceName">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReservePercentage" HeaderText="Reserve Percentage" SortExpression="ReservePercentage">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <%--<Column added for DM-320>--%>
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                        <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsAgentAllocations" runat="server" SelectMethod="GetAgentAllocationsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetAgentAllocationsPagingCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsAgentAllocations_Selecting">
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
                                                    <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save Excel</span>
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

    <script type="text/javascript">

</script>
</asp:Content>

