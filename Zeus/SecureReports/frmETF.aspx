<%@ Page Language="C#" AutoEventWireup="true" Inherits="frmETF" MasterPageFile="~/MasterPageReports.master"
    Title="Merchant ETF Report" CodeBehind="frmETF.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/wucQuickNotes_Tickets.ascx" TagName="Notes_Tickets"
    TagPrefix="uc5" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucACHGrid2.ascx" TagName="wucACHGrid2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        var IsPostBack = '<%=IsPostBack.ToString() %>';
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                if (IsPostBack == 'True') {
                    if (document.getElementById("<%=pnlRecords.ClientID %>") != null)
                        document.getElementById("<%=pnlRecords.ClientID %>").scrollTop = strPos;
                }
                else {
                    document.cookie = "yPos=!~0~!";
                }
            }
        }

        function SetDivPosition() {
            var intY = 0;

            if (document.getElementById("<%=pnlRecords.ClientID %>") != null)
                intY = document.getElementById("<%=pnlRecords.ClientID %>").scrollTop;

            document.cookie = "yPos=!~" + intY + "~!";
        }

        function CollapseExpand(object, txt, object1) {
            var div = document.getElementById(object);
            var object2 = document.getElementById(object1);
            if (txt == null) {
                if (div.style.display == "none") {
                    div.style.display = "inline";
                    object2.src = "../Images/close.gif";
                }
                else {
                    div.style.display = "none";
                    object2.src = "../Images/open.gif";
                }
            }
            else {
                div.style.display = txt;
                if (txt == 'none')
                    object2.src = "../Images/open.gif";
                else
                    object2.src = "../Images/close.gif";
            }
        }     
        
    </script>
    <div class="contentpage">
        <uc6:wucMessage runat="server" ID="wucMessage1" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
        <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="927px">
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
            <table width="100%">
                <tr>
                    <td style="width: 893px">
                        <ig:WebTab ID="tabReport" runat="server" Width="1231" SelectedIndex="0" Height="650px">
                            <Tabs>
                                <ig:ContentTabItem runat="server" Text="Search">
                                    <Template>
                                        <asp:Panel ID="pnlGrid" runat="server" Height="" Width="" DefaultButton="btnSearch">
                                            <fieldset>
                                                <legend>
                                                    <img runat="server" id='img1' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                                        alt="img" />&nbsp;ETF Merchant Search</legend>
                                                <div id="div1" style="display: inline;" runat="server">
                                                    <table cellpadding="" cellspacing="5" width="100%">
                                                        <tr>
                                                            <td class="lblRight">
                                                                From Date:
                                                            </td>
                                                            <td>
                                                                <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                                                    Width="85px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                        SlideOpenDuration="1" />
                                                                </ig:WebDatePicker>
                                                            </td>
                                                            <td class="lblRight">
                                                                End Date:
                                                            </td>
                                                            <td>
                                                                <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                                                    Width="85px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                        SlideOpenDuration="1" />
                                                                </ig:WebDatePicker>
                                                            </td>
                                                            <td class="lblRight">
                                                                <asp:Label runat="server" ID="Label1" Text="DBA:" Width="65px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtDBA" runat="server" Width="95px"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">
                                                                <asp:Label runat="server" ID="lblText" Text="Closure Codes:" Width="75px"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="MerchantClosureCodeUID" runat="server" Width="180px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">
                                                                ZID:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtZID" runat="server" Width="80px" ValidationGroup="searchValid"></asp:TextBox>
                                                                <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtZID"
                                                                    ErrorMessage="Please enter a valid ZID." MaximumValue="100000" MinimumValue="1"
                                                                    Type="Integer" Display="None" ValidationGroup="searchValid"></asp:RangeValidator>
                                                            </td>
                                                            <td class="lblRight">
                                                                Status:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="StatusUID" runat="server" Width="85px">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td colspan="4">
                                                                <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                                                    IDWidth="110px" DBAWidth="95px" lblDBAWidth="80px" lblIDWidth="115px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="10" align="center">
                                                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                                    ValidationGroup="searchValid" />&nbsp;
                                                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="lblLeft">
                                                                    Page Size:
                                                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                                                        <asp:ListItem>25</asp:ListItem>
                                                                        <asp:ListItem>50</asp:ListItem>
                                                                        <asp:ListItem>100</asp:ListItem>
                                                                        <asp:ListItem>250</asp:ListItem>
                                                                        <asp:ListItem>500</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td class="lblRight">
                                                                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                            DataSourceID="odsEtf" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                                            OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand" AllowSorting="True"
                                                            OnSorting="grd_Sorting" DataKeyNames="UID" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging">
                                                            <SelectedRowStyle BackColor="#fffacd" />
                                                            <HeaderStyle CssClass="gvFixedHeader" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkUID" runat="server" CommandName="UID" CausesValidation="false">Select</asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="40px" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="ID" HeaderText="ZID" SortExpression="ID" />
                                                                <asp:BoundField DataField="DBA" HeaderText="DBA Name" SortExpression="DBA" />
                                                                <asp:BoundField DataField="AgentID" HeaderText="Agent ID" SortExpression="AgentID" />
                                                                <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" SortExpression="AgentDBA" />
                                                                <asp:BoundField DataField="SuggestedETF" HeaderText="Calculated ETF" SortExpression="SuggestedETF"
                                                                    DataFormatString="{0:0.00}" />
                                                                <asp:BoundField DataField="EarlyTerminationFee" HeaderText="Suggested ETF" SortExpression="EarlyTerminationFee"
                                                                    DataFormatString="{0:0.00}" />
                                                                <asp:BoundField DataField="ActualETF" HeaderText="Actual ETF" SortExpression="ActualETF"
                                                                    DataFormatString="{0:0.00}" />
                                                                <asp:BoundField DataField="Balance" HeaderText="Balance ETF" SortExpression="Balance"
                                                                    DataFormatString="{0:0.00}" />
                                                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" />
                                                                <asp:BoundField DataField="ClosureCode" HeaderText="Closure Code" SortExpression="ClosureCode" />
                                                                <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="ApprovedDate"
                                                                    HeaderText="Approved Date" SortExpression="ApprovedDate">
                                                                    <ItemStyle Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="PendingCancelledDate"
                                                                    HeaderText="Pending Cancellation Date" SortExpression="PendingCancelledDate">
                                                                    <ItemStyle Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="CancelledDate"
                                                                    HeaderText="Cancelled Date" SortExpression="CancelledDate">
                                                                    <ItemStyle Width="60px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="ETFApprovedDate"
                                                                    HeaderText="ETF Approved Date" SortExpression="ETFApprovedDate">
                                                                    <ItemStyle Width="60px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pgr" />
                                                            <FooterStyle CssClass="footer" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                        </asp:GridView>
                                                        <asp:ObjectDataSource ID="odsEtf" runat="server" SelectMethod="GetETFMerchantsPaging"
                                                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                                            SelectCountMethod="GetETFMerchantsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsEtf_Selecting">
                                                            <SelectParameters>
                                                                <asp:Parameter Name="prms" Type="Object" />
                                                                <asp:Parameter Name="PageSize" Type="Int32" />
                                                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                                            </SelectParameters>
                                                        </asp:ObjectDataSource>
                                                        <div class="bucketfooter">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="left" style="width: 33%;">
                                                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                                            <span style="height: 25px; vertical-align: middle;">
                                                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                                                    Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right">
                                                                        Export:&nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td align="right" style="width: 33%;">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                                                        No Records Found...
                                                    </asp:Panel>
                                                </div>
                                            </fieldset>
                                        </asp:Panel>
                                    </Template>
                                </ig:ContentTabItem>
                                <ig:ContentTabItem runat="server" Text="Calculated ETF Preview">
                                    <Template>
                                        <asp:Panel ID="pnlEFTPreview" runat="server" DefaultButton="btnCalculateETFPreview">
                                            <fieldset>
                                                <legend>Calculated ETF Preview</legend>
                                                <table>
                                                    <tr>
                                                        <td class="lblRight">
                                                            ZID
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtZIDPreview" runat="server"></asp:TextBox>
                                                        </td>
                                                        <td class="lblRight">
                                                            Contract Months:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtContractMonthsPreview" Text="36" runat="server" Width="100px"
                                                                Style="text-align: right;"></asp:TextBox>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContractMonthsPreview"
                                                                Display="Dynamic" ErrorMessage="Invalid Contract Months" ValidationExpression="^\d{1,2}$">*</asp:RegularExpressionValidator>
                                                        </td>
                                                        <td>
                                                            <asp:Button runat="server" ID="btnCalculateETFPreview" Text="Calculate" OnClick="btnCalculateETFPreview_Click" />
                                                            <b>Calculated ETF:</b>
                                                            <asp:Label ID="lblCalculateETFPreview" runat="server" Text=""></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </asp:Panel>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                        </ig:WebTab>
                        <asp:UpdatePanel runat="server" ID="panel1">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="pnlETF" Visible="false">
                                    <asp:Label ID="lblError2" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                    <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 70%; vertical-align: top">
                                                <fieldset style="height: 220px;">
                                                    <legend>&nbsp;ETF Information</legend>
                                                    <table border="0" cellpadding="0" cellspacing="5" width="100%">
                                                        <tr>
                                                            <td class="lblRight">
                                                                ETF Status:
                                                            </td>
                                                            <td colspan="3" align="left">
                                                                <asp:DropDownList ID="ETFStatusUID" runat="server" Width="105px">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">
                                                                Contract Months:
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="ContractMonths" Text="36" runat="server" Width="100px" Style="text-align: right;"></asp:TextBox>
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="ContractMonths"
                                                                    Display="Dynamic" ErrorMessage="Invalid Contract Months" ValidationExpression="^\d{1,2}$">*</asp:RegularExpressionValidator>
                                                            </td>
                                                            <td class="lblRight">
                                                                Calculated ETF:
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="SuggestedETF" runat="server" Text="0" Width="100px" Height="15px"
                                                                    Style="vertical-align: top; text-align: right; background-color: #F9F9F9; border-color: #ADC3DE;
                                                                    border-width: 1px; border-style: Solid; font-family: Tahoma; font-size: 8.5pt;"
                                                                    OnClick="SuggestedETF_Click" CausesValidation="false">
                                                                </asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblRight">
                                                                Remaining Months:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="RemainingMonths" runat="server" Width="100px" Height="15px" Style="vertical-align: top;
                                                                    text-align: right; background-color: #F9F9F9; border-color: #ADC3DE; border-width: 1px;
                                                                    border-style: Solid; font-family: Tahoma; font-size: 8.5pt;"></asp:Label>
                                                            </td>
                                                            <td class="lblRight">
                                                                Actual ETF:
                                                            </td>
                                                            <td>
                                                                <ig:WebNumericEditor ID="ActualETF" runat="server" ValueText="0" Width="100px">
                                                                </ig:WebNumericEditor>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnCalculate" runat="server" Text="Recalculate Etf" OnClick="btnCalculate_Click"
                                                                    CausesValidation="false" />
                                                            </td>
                                                            <td>
                                                            </td>
                                                            <td>
                                                                <asp:Panel runat="server" ID="pnlApp" Visible="false">
                                                                    <asp:Button runat="server" ID="btnApprove" Text="Approve" Visible="false" OnClick="btnApprove_Click" />
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                            </td>
                                                            <td class="lblRight">
                                                                Balance:
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="Balance" runat="server" Width="100px" Height="15px" Style="vertical-align: top;
                                                                    text-align: right; background-color: #F9F9F9; border-color: #ADC3DE; border-width: 1px;
                                                                    border-style: Solid; font-family: Tahoma; font-size: 8.5pt;"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <asp:Panel ID="pnlApprove" runat="server" Visible="false">
                                                            <tr>
                                                                <td colspan="2">
                                                                </td>
                                                                <td class="lblRight" valign="top">
                                                                    Adjust Balance:
                                                                </td>
                                                                <td valign="top">
                                                                    <ig:WebNumericEditor ID="ACHAmount" runat="server" ValueText="0" Width="100px">
                                                                    </ig:WebNumericEditor>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                </td>
                                                                <td>
                                                                    <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click" />
                                                                </td>
                                                            </tr>
                                                        </asp:Panel>
                                                    </table>
                                                </fieldset>
                                            </td>
                                            <td valign="top">
                                                <fieldset style="height: 220px;">
                                                    <legend>&nbsp;ETF Status History</legend>
                                                    <asp:Panel runat="server" ID="Status" Width="400px" Height="100%">
                                                        <asp:GridView ID="grdStatus" AllowPaging="true" PageSize="4" runat="server" AutoGenerateColumns="False"
                                                            CssClass="mGrid" OnPageIndexChanging="grdStatus_PageIndexChanging">
                                                            <PagerStyle CssClass="pgr" />
                                                            <FooterStyle CssClass="footer" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                            <SelectedRowStyle BackColor="#fffacd" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-Width="80px" />
                                                                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance"
                                                                    DataFormatString="{0:0.00}" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="right" />
                                                                <asp:BoundField DataField="UserCreated" HeaderText="User" SortExpression="UserCreated"
                                                                    ItemStyle-Width="70px" />
                                                                <asp:BoundField DataFormatString="{0:MM/dd/yyyy hh:mm tt}" DataField="StatusChangedDate"
                                                                    HeaderText="Date" SortExpression="StatusChangedDate">
                                                                    <ItemStyle Width="60px" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </fieldset>
                                            </td>
                                        </tr>
                                    </table>
                                    <fieldset>
                                        <legend>Invoice History</legend>
                                        <asp:Panel ID="pnlInvoice" runat="server" Height="" Width="">
                                            <uc2:wucACHGrid2 ID="grdACH" runat="server">
                                            </uc2:wucACHGrid2>
                                        </asp:Panel>
                                        <br />
                                        <asp:Panel ID="pnlAch" runat="server" Height="100%" Width="100%">
                                            <div class="title">
                                                &nbsp;&nbsp;Post Invoice (ACH)
                                                <hr class="line" />
                                            </div>
                                            <div class="indentedcontent20">
                                                <table cellpadding="0" cellspacing="5">
                                                    <tr>
                                                        <td class="lblRight">
                                                            Category:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cboCategory" runat="server" Width="200px" ValidationGroup="PostACH">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">
                                                            Type:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="cboType" runat="server" Width="200px">
                                                                <asp:ListItem Value="27">ACH Charge</asp:ListItem>
                                                                <asp:ListItem Value="22">ACH Refund</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">
                                                            Post Date:
                                                        </td>
                                                        <td>
                                                            <ig:WebDatePicker ID="txtPostDate" runat="server" NullDateLabel="" EnableAppStyling="False"
                                                                Width="200px" AllowNull="False">
                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                    SlideOpenDuration="1" />
                                                            </ig:WebDatePicker>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">
                                                            Amount:
                                                        </td>
                                                        <td>
                                                            <ig:WebNumericEditor ID="txtAmount" runat="server" DataMode="Decimal" ValueText="0"
                                                                Width="195px">
                                                            </ig:WebNumericEditor>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">
                                                            Ref No:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtRefID" runat="server" MaxLength="50" Width="195px"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Button runat="server" ID="btnAddinvoice" Text="Add Invoice" OnClick="btnAddinvoice_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>
                                </asp:Panel>
                                <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="320px" Width="500px"
                                    Modal="True" InitialLocation="Centered" WindowState="Hidden">
                                    <ContentPane>
                                        <Template>
                                            <fieldset>
                                                <legend>Calculated ETF Details</legend>
                                                <asp:GridView ID="grdDetails" runat="server" AutoGenerateColumns="False" CssClass="mGrid">
                                                    <PagerStyle CssClass="pgr" />
                                                    <FooterStyle CssClass="footer" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                    <SelectedRowStyle BackColor="#fffacd" />
                                                    <EmptyDataTemplate>
                                                        No Records Found...
                                                    </EmptyDataTemplate>
                                                    <Columns>
                                                        <asp:BoundField DataField="Month" HeaderText="Month" SortExpression="Month" />
                                                        <asp:BoundField DataField="Year" HeaderText="Year" SortExpression="Year" />
                                                        <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" DataFormatString="{0:0.00}" />
                                                    </Columns>
                                                </asp:GridView>
                                            </fieldset>
                                        </Template>
                                    </ContentPane>
                                    <Header CaptionText="">
                                    </Header>
                                </ig:WebDialogWindow>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top">
                        <asp:Panel runat="server" ID="lblQuick" Visible="false">
                            <uc5:Notes_Tickets runat="server" ID="Notes_Tickets" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
