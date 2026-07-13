<%@ Page Title="Ticket Errors Search" Language="C#" MasterPageFile="~/MasterPageQuality.Master" AutoEventWireup="true" CodeBehind="frmQATicketErrorsSearch.aspx.cs" Inherits="frmQATicketErrorsSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ MasterType VirtualPath="~/MasterPageQuality.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <div id="contentpage" style="width:1180px;">
        <table width="100%">
            <tr>
                <td>

                    <fieldset>
                        <legend>Ticket Errors Search</legend>
                        <div runat="server" id="dvMessage" visible="false" style="height:35px; color:green; font-size:12px;">
                            </div>
                        <asp:ValidationSummary runat="server" ID="validSum1" ShowMessageBox="true" ShowSummary="false" />
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%" cellpadding="2" cellspacing="2">
                                <tr>
                                    <td class="lblRight">Created Begin:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedBeginDate" runat="server" Width="90px" MaxLength="10" TabIndex="1"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedBeginDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedBeginDate"
                                            TargetControlID="SearchCreatedBeginDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedBeginDate" runat="Server" AlternateText="Click to show calendar" TabIndex="2"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Created End:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedEndDate" runat="server" Width="90px" MaxLength="10" TabIndex="3"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedEndDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedEndDate"
                                            TargetControlID="SearchCreatedEndDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedEndDate" runat="Server" AlternateText="Click to show calendar" TabIndex="4"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">ZID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZID" runat="server" Width="100px" EnableViewState="False"
                                            TabIndex="5" MaxLength="9" onKeyPress="CheckNumeric()"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Ticket ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTicketID" runat="server" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="6"  MaxLength="9"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Rep:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRep" runat="server" Width="100px" EnableViewState="False"
                                            TabIndex="7" MaxLength="50"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Date Error Found:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDateErrorFound" runat="server" Width="100px" EnableViewState="False" MaxLength="10"
                                            TabIndex="8"></asp:TextBox>

                                         <cc1:CalendarExtender ID="calsearchDateErrorFound" runat="server" Enabled="True" PopupButtonID="imgSearchDateErrorFound"
                                            TargetControlID="txtDateErrorFound">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchDateErrorFound" runat="Server" AlternateText="Click to show calendar" TabIndex="9"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />

                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Category" runat="server" Width="110px" TabIndex="10"></asp:DropDownList>
                                    </td>
                                     <td class="lblRight">Sub-Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="SubCategory" runat="server" Width="110px" TabIndex="11"></asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" Width="150px" EnableViewState="False"
                                            TabIndex="12" MaxLength="500"></asp:TextBox>
                                    </td>
                                    <td class="lblRight" colspan="6">
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                    AccessKey="h" TabIndex="13">
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
                                                    AccessKey="l" TabIndex="14">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CausesValidation="false"
                                                    AccessKey="a" TabIndex="15">
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
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>500</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            
                            <asp:GridView ID="grdTicketErrorsSearch" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="100%"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="QATicketErrorID" OnRowDataBound="grdTicketErrorsSearch_RowDataBound"
                                OnRowCommand="grdTicketErrorsSearch_RowCommand" OnPageIndexChanging="grdTicketErrorsSearch_PageIndexChanging" AllowSorting="True"
                                OnSorting="grdTicketErrorsSearch_Sorting" DataSourceID="odsQATicketErrorsSearch" ClientIDMode="Static" >
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField HeaderText="QATicketErrorID" HeaderStyle-Width="30px">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypAQATicketErrorRowID" runat="server" CssClass="zeustooltip" NavigateUrl='<%# "~/SecureQualityForms/frmQATicketErrorsDetail.aspx?QATicketErrorID=" + Eval("QATicketErrorID") + "&Adding=false"  %>' Text='<%# Eval("QATicketErrorRowID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DateErrorFound" HeaderText="Date Error Found" ItemStyle-CssClass="togle" SortExpression="DateErrorFound" DataFormatString="{0:MM/dd/yyyy}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ZID" HeaderText="ZID" SortExpression="ZID">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TicketID" HeaderText="Ticket ID" ItemStyle-CssClass="togle" SortExpression="TicketID">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rep" HeaderText="Rep" SortExpression="Rep">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="SubCategory" HeaderText="Sub-Category" SortExpression="SubCategory">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                     <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="togle" SortExpression="Description">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" SortExpression="DateCreated" DataFormatString="{0:MM/dd/yyyy HH:mm tt}">
                                        <ItemStyle Width="100px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsQATicketErrorsSearch" runat="server" SelectMethod="GetQATicketErrorsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetQATicketErrorsPagingCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsQATicketErrors_Selecting">
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
</asp:Content>
