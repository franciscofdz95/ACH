<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageTicket.master"
    Inherits="frmTicketSearch" CodeBehind="frmTicketSearch.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
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
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>



                    <fieldset>
                        <legend>Ticket Search</legend>
                        <asp:ValidationSummary runat="server" ID="validSum1" ShowMessageBox="true" ShowSummary="false" />
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">Begin Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchBeginDate" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchBeginDate" runat="server" Enabled="True" PopupButtonID="imgSearchBeginDate"
                                            TargetControlID="SearchBeginDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchBeginDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">End Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchEndDate" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchEndDate" runat="server" Enabled="True" PopupButtonID="imgSearchEndDate"
                                            TargetControlID="SearchEndDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchEndDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">DBA Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessDBAName" runat="server" Width="75px" EnableViewState="False"
                                            TabIndex="1"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">MID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SettlePlatformMid" runat="server" Width="100px" EnableViewState="False"
                                            TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Assign To:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="AssignToUID" runat="server" Width="130px" TabIndex="8">
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
                                    <td class="lblRight">Callback Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="CallbackDate" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calCallbackDate" runat="server" Enabled="True" PopupButtonID="imgCallbackDate"
                                            TargetControlID="CallbackDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgCallbackDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">ZID:
                                    <td>
                                        <asp:TextBox ID="MerchantID" runat="server" Width="75px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="5" MaxLength="9"></asp:TextBox></td>
                                        <td class="lblRight">FMA ID:</td>
                                        <td>
                                            <asp:TextBox ID="FMAID" runat="server" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                                TabIndex="5" MaxLength="15"></asp:TextBox></td>
                                        <td class="lblRight">Department:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="DepartmentID" runat="server" Width="130px" TabIndex="7">
                                            </asp:DropDownList>
                                        </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Source:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Source" runat="server" Width="90px" TabIndex="8">
                                            <asp:ListItem Value="-1">All</asp:ListItem>
                                            <asp:ListItem Value="i">Zeus</asp:ListItem>
                                            <asp:ListItem Value="a">Apex</asp:ListItem>
                                            <asp:ListItem Value="m">Insight</asp:ListItem>
                                            <asp:ListItem Value="x">Payment XP</asp:ListItem>
                                            <asp:ListItem Value="e">Scavenger</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Date Created:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="DateCreated" runat="server" Width="90px"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calDateCreated" runat="server" Enabled="True" PopupButtonID="imgDateCreated"
                                            TargetControlID="DateCreated">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgDateCreated" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="BusinessContact" runat="server" Width="75px" EnableViewState="False"
                                            TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td align="right">Severity:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Priority" runat="server" Width="100px" TabIndex="9">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="Low">Sev 3 - Low</asp:ListItem>
                                            <asp:ListItem Value="Medium">Sev 2 - Medium</asp:ListItem>
                                            <asp:ListItem Value="High">Sev 1 - High</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Status:
                                    </td>
                                    <td rowspan="5">
                                        <style type="text/css">
                                            .ticketsearchstatusuidlist label
                                            {
                                                padding-right: 20px;
                                                vertical-align: bottom;
                                            }
                                        </style>
                                        <asp:CheckBoxList runat="server" RepeatDirection="Vertical" CssClass="ticketsearchstatusuidlist" RepeatLayout="Flow" ID="StatusUIDList"></asp:CheckBoxList>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">Origin:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Origin" Width="90px" runat="server">
                                            <asp:ListItem Value="0">All</asp:ListItem>
                                            <asp:ListItem Value="1">Inbound</asp:ListItem>
                                            <asp:ListItem Value="2">Outbound</asp:ListItem>
                                            <asp:ListItem Value="3">Email</asp:ListItem>
                                            <asp:ListItem Value="4">Internal</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="right">Tags:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="Tags" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Ticket ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TicketID" runat="server" Width="75px" EnableViewState="False" MaxLength="7" onKeyPress="CheckNumeric()"
                                            TabIndex="2"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label runat="server" ID="lbl" Width="72px" Text="Time Zone:"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="TimeZone" runat="server" Width="100px" TabIndex="8" onchange="timezonechangeHandler(this)">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="CategoryID" AutoPostBack="true" runat="server" Width="90px"
                                            TabIndex="6" OnSelectedIndexChanged="CategoryID_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td colspan="5">
                                        <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                            IDWidth="75" DBAWidth="90" lblDBAWidth="93px" lblIDWidth="150px" />
                                    </td>
                                </tr>
                                <tr>
                                    <asp:Panel runat="server" ID="pnlCat" Visible="false">
                                        <td class="lblRight">Sub-Category:
                                        </td>
                                        <td colspan="3">
                                            <asp:DropDownList ID="SubCategory" runat="server" Width="290px" TabIndex="6">
                                            </asp:DropDownList>
                                        </td>
                                    </asp:Panel>


                                </tr>
                                <tr>
                                    <td class="lblRight">Origin Dept:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="OriginDept" AutoPostBack="true" runat="server" Width="90px"
                                            TabIndex="6">
                                        </asp:DropDownList>
                                    </td>

                                    <td class="lblRight">Issues/Notes:</td>
                                    <td>
                                        <asp:TextBox ID="IssuesNotes" runat="server" EnableViewState="False" MaxLength="255" Width="90px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Office ID:</td>
                                    <td>
                                        <%--PXP:5768: Ani: Zeus: Rename 'Cambridge' office as 'London'--%>
                                        <asp:DropDownList ID="OfficeID" runat="server" Width="75px">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                            <asp:ListItem Value="1">Irvine (US)</asp:ListItem>
                                            <asp:ListItem Value="2">Montreal (CAN)</asp:ListItem>
                                            <asp:ListItem Value="3">London (UK)</asp:ListItem>
                                            <asp:ListItem Value="4">Gatineau (CAN)</asp:ListItem>
                                            <asp:ListItem Value="5">Los Angeles (US)</asp:ListItem>
                                            <asp:ListItem Value="6">Dallas (US)</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>

                                    <td class="lblRight">Email From:</td>

                                    <td>
                                        <asp:TextBox ID="ScavengerEmailFrom" runat="server" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td class="lblRight">MLE:</td>
                                    <td>
                                        <asp:TextBox ID="MLEName" runat="server" Width="90px"></asp:TextBox>
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
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="TicketUID" OnRowDataBound="grd_RowDataBound"
                                OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                OnSorting="grd_Sorting" DataSourceID="odsTickets" ClientIDMode="Static" >
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField SortExpression="PriorityID">
                                        <ItemTemplate></ItemTemplate>
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
                                    <asp:TemplateField HeaderText="ID" SortExpression="TicketID">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypTID" runat="server" CssClass="zeustooltip" NavigateUrl='<%# "~/SecureTicketForms/frmTicketDetail.aspx?TicketUID=" + Eval("TicketUID") + "&Adding=false"  %>' Text='<%# Eval("TicketID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ZID" HeaderText="ZID" ItemStyle-CssClass="togle" Visible="false">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MerchantID" HeaderText="MID" ItemStyle-CssClass="togle" Visible="false">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ParentCategory" HeaderText="Category" SortExpression="ParentCategory">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Category" HeaderText="Sub-Category" SortExpression="Category">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="UserName" HeaderText="Assign To" SortExpression="UserName">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Problem" HeaderText="Problem" ItemStyle-CssClass="togle" Visible="false">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <%--Do not format this part. Very Important as this takes the space while rendinreing used in More link--%>
                                    <asp:TemplateField ItemStyle-Width="80px" HeaderText="Subject" SortExpression="Problem">
                                        <HeaderTemplate>
                                            Issue
                                        <span class="headmoreless fakea" onclick="ToggleHeadMoreLess(this, event, '<%= grd.ClientID %>')">More</span>
                                        </HeaderTemplate><ItemTemplate><p style="margin: 0; padding: 0;" class="minimize"><asp:Literal runat="server" ID="litSolution" Text='<%# Eval("Problem") %>' Mode="Encode"></asp:Literal></p></ItemTemplate>
                                        <ItemStyle Width="80px"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM-dd-yy HH:mm tt}" HeaderText="Date Created" SortExpression="DateCreated">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DueDate" DataFormatString="{0:MM-dd-yy HH:mm tt}" HeaderText="Due Date" ItemStyle-CssClass="togle" SortExpression="DueDate">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LastChangedDate" DataFormatString="{0:MM-dd-yy HH:mm tt}" HeaderText="Date Updated" SortExpression="LastChangedDate">
                                        <ItemStyle Width="60px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Days" HeaderText="Days Aged" SortExpression="Days">
                                        <ItemStyle Width="40px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Priority" Visible="false" ItemStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblPri" Text='<%# Bind("Priority") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="40px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Tags" HeaderText="Tags" ItemStyle-CssClass="togle" SortExpression="Tags">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="RepeatCallIndicator" HeaderText="Repeat Call Indicator" ItemStyle-CssClass="togle" SortExpression="RepeatCallIndicator" Visible="false">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
                                    <asp:TemplateField HeaderText="Office" SortExpression="OfficeID">
                                        <ItemTemplate>
                                            <asp:Label ID="LabelOffice" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" />
                                    </asp:TemplateField>                                    
                                    <asp:BoundField DataField="ScavengerEmailFrom" HeaderText="Email From"/>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsTickets" runat="server" SelectMethod="GetTicketsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetTicketsPagingCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsTickets_Selecting">
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

    <script type="text/javascript">
        ticketSearchRequestHandler();

    </script>
</asp:Content>
