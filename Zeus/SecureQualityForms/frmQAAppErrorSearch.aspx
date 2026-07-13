<%@ Page Title="Application Errors Search" Language="C#" AutoEventWireup="true" CodeBehind="frmQAAppErrorSearch.aspx.cs" Inherits="frmQAAppErrorSearch" MasterPageFile="~/MasterPageQuality.master" %>

<%@ MasterType VirtualPath="~/MasterPageQuality.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <style type="text/css">
        input, select, textarea {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>--%>


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
                        <legend>Application Errors Search</legend>
                        <asp:Label ID="lblMessage" runat="server" Text="Label" ForeColor="Green"></asp:Label>
                        <asp:ValidationSummary runat="server" ID="validSum1" ShowMessageBox="true" ShowSummary="false" />
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                            <table width="100%">
                                <tr>
                                    <td class="lblRight">Created Begin:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedBeginDate" runat="server" Width="100px" DataFormatString="{0:MM/dd/yyyy HH:mm tt}" TabIndex="1"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedBeginDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedBeginDate"
                                            TargetControlID="SearchCreatedBeginDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedBeginDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" TabIndex="2" />
                                    </td>
                                    <td class="lblRight">Created End:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedEndDate" runat="server" Width="100px" DataFormatString="{0:MM/dd/yyyy HH:mm tt}" TabIndex="3"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedEndDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedEndDate"
                                            TargetControlID="SearchCreatedEndDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedEndDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" TabIndex="4" />
                                    </td>
                                    <td class="lblRight">MID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="MID" runat="server" MaxLength="20" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()"
                                            TabIndex="5"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">Department:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Dept" runat="server" Width="100px" TabIndex="6"></asp:DropDownList>
                                    </td>

                                    <td class="lblRight">Rep:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Rep" runat="server" MaxLength="50" Width="100px" EnableViewState="False"
                                            TabIndex="7"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">Date Error Found:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="DateErrorFound" runat="server" MaxLength="10" Width="100px" EnableViewState="False"
                                            TabIndex="8" DataFormatString="{0:MM/dd/yyyy}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchDateErrorFound" runat="server" Enabled="True" PopupButtonID="imgDateErrorFound"
                                            TargetControlID="DateErrorFound" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgDateErrorFound" runat="Server" AlternateText="Click to show calendar" TabIndex="9"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">Error Found By:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ErrorFoundBy" runat="server" Width="107px" TabIndex="10"></asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="Category" runat="server" Width="107px" TabIndex="11"></asp:DropDownList>
                                    </td>
                                    <td class="lblRight">Sub-Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="SubCategory" runat="server" Width="100px" TabIndex="12"></asp:DropDownList>
                                    </td>

                                    <td class="lblRight">Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="Description" runat="server" MaxLength="500" Width="100px" EnableViewState="False"
                                            TabIndex="13"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                    AccessKey="h" TabIndex="14">
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
                                                    AccessKey="l" TabIndex="15">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" CausesValidation="false"
                                                    AccessKey="a" TabIndex="16">
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

                            <asp:GridView ID="grdQAAppErrors" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="QAAppErrorKeyID" OnRowDataBound="grdQAAppErrors_RowDataBound"
                                OnPageIndexChanging="grdQAAppErrors_PageIndexChanging" AllowSorting="True"
                                OnRowCommand="grdQAAppErrors_RowCommand" OnSorting="grdQAAppErrors_Sorting" DataSourceID="odsQAAppErrors" ClientIDMode="Static">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField HeaderText="QAAppErrorID">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="hypQAAppErrorKeyID" runat="server" CssClass="zeustooltip" NavigateUrl='<%# "~/SecureQualityForms/frmQAAppErrorDetail.aspx?QAAppErrorKeyID=" + Eval("QAAppErrorKeyID") + "&MID=" + Eval("MID") + "&QAAppErrorID=" + Eval("QAAppErrorID") + "&Adding=false"  %>' Text='<%# Eval("QAAppErrorKeyID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DateErrorFound" HeaderText="Date Error Found" ItemStyle-CssClass="togle" DataFormatString="{0:MM/dd/yyyy}" SortExpression="DateErrorFound">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="MID" HeaderText="MID" SortExpression="MID">
                                        <ItemStyle Width="120px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rep" HeaderText="Rep" ItemStyle-CssClass="togle" SortExpression="Rep">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Dept" HeaderText="Department" SortExpression="Dept">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ErrorFoundBy" HeaderText="Error Found By" ItemStyle-CssClass="togle" SortExpression="ErrorFoundBy">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="ErrorOccuredStage" HeaderText="Error Occurred Stage" SortExpression="ErrorOccuredStage">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-CssClass="togle" SortExpression="Category">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>

                                    <asp:BoundField DataField="SubCategory" HeaderText="Sub-Category" SortExpression="SubCategory">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-CssClass="togle" SortExpression="Description">
                                        <ItemStyle Width="140px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yyyy HH:mm tt}" HeaderText="Date Created" SortExpression="DateCreated">
                                        <ItemStyle Width="90px" />
                                    </asp:BoundField>
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsQAAppErrors" runat="server" SelectMethod="GetQaAppErrorsPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetQAAppErrorsPagingCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsQAAppErrors_Selecting">
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

