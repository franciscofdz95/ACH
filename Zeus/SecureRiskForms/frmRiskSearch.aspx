<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true"
    Inherits="frmRiskSearch" Title="Batch Search" CodeBehind="frmRiskSearch.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        input, select, textarea {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
    </style>
    <script language="JavaScript" type="text/javascript">
        function CollapseExpand(object, txt, object1) {
            var div = document.getElementById(object);
            var object2 = document.getElementById(object1);
            if (txt == null) {
                if (div.style.display == "none") {
                    div.style.display = "inline";
                    object2.src = "../Images/minus.JPG";
                }
                else {
                    div.style.display = "none";
                    object2.src = "../Images/plus.JPG";
                }
            }
            else {
                div.style.display = txt;
                if (txt == 'none')
                    object2.src = "../Images/plus.JPG";
                else
                    object2.src = "../Images/minus.JPG";
            }
        }

        function expandAll(txt) {
            var gridViewCtlId = document.getElementById('<%=grdRisk.ClientID%>');
            if (null != gridViewCtlId) {
                var i = 1;
                var j = 2;
                for (; i < (gridViewCtlId.rows.length - 1); i = i + 2) {

                    if (txt == '1') {
                        if (j < 10)
                            j = '0' + j;
                        CollapseExpand(gridViewCtlId.rows[i].cells[14].innerText, 'inline', 'ctl00_ContentPlaceHolder1_grdRisk_ctl' + j + '_img1');
                        j++;
                    }
                    else {
                        if (j < 10)
                            j = '0' + j;
                        CollapseExpand(gridViewCtlId.rows[i].cells[14].innerText, 'none', 'ctl00_ContentPlaceHolder1_grdRisk_ctl' + j + '_img1');
                        j++;
                    }
                }
            }
        }
    </script>
    <div id="contentpage">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
        <table width="100%">
            <tr>
                <td>
                    <fieldset>
                    <legend>Filters</legend>
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                               <table>
                                   <tr>
                                    <td class="lblRight">
                                        Begin:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedBeginDate" runat="server" Width="90px" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedBeginDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedBeginDate"
                                            TargetControlID="SearchCreatedBeginDate" Format="MM/dd/yyyy" >
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedBeginDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                    </td>
                                    <td class="lblRight">
                                        End Date:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="SearchCreatedEndDate" runat="server" Width="90px" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:TextBox>
                                        <cc1:CalendarExtender ID="calSearchCreatedEndDate" runat="server" Enabled="True" PopupButtonID="imgSearchCreatedEndDate"
                                            TargetControlID="SearchCreatedEndDate" Format="MM/dd/yyyy">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgSearchCreatedEndDate" runat="Server" AlternateText="Click to show calendar"
                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                </td>
                                <td>
                                    ZID:
                                </td>
                                <td>
                                    <asp:TextBox ID="ZID" runat="server" Width="65px"></asp:TextBox>
                                    <asp:RangeValidator ID="RangeValidator1" runat="server" Display="None" ErrorMessage="Please enter a valid ZID"
                                    MaximumValue="10000000" MinimumValue="1" Type="Integer" ControlToValidate="ZID"></asp:RangeValidator>
                                </td>
                                <td>
                                    DBA:
                                </td>
                                <td>
                                    <asp:TextBox ID="DBA" runat="server" Width="75px"></asp:TextBox>
                                </td>
                                <td>
                                    Batch Status
                                </td>
                                <td>
                                    <asp:DropDownList ID="BatchStatus" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Held By:
                                </td>
                                <td>
                                    <asp:DropDownList ID="HeldBy" runat="server">
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    Released By:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ReleasedBy" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                            <div>
                                 <center>
                                     <table>
                                         <tr >
                                            <td >
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                                AccessKey="h">
                                                <Appearance>
                                                <Image Url="~/Images/Check.png" />
                                                </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td >
                                                <igtxt:WebImageButton ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click"
                                                    AccessKey="l">
                                                <Appearance>
                                            <Image Url="~/Images/delete.png" />
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
                </td>
             </tr>
        </table>
        <div style="text-align: center;">
            <br />
        </div>
    
    <fieldset>
        <div style="width: 100%">
            <div class="buckethdrleft">
                <asp:Panel runat="server" ID="pnl1">
                    <a id="lnkExpandAll" onmouseover="this.style.cursor='pointer';" onclick="expandAll('1')">
                        Expand All</a> | <a id="lnkCollapseAll" onmouseover="this.style.cursor='pointer';"
                            onclick="expandAll('0')">Collapse All</a>
                </asp:Panel>
            </div>
            <div class="buckethdright">
                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>&nbsp;</div>
        </div>
        <asp:Panel runat="server" ID="pnl" ScrollBars="vertical" Height="500px" Width="100%">
            <asp:GridView ID="grdRisk" runat="server" OnRowCommand="grdRisk_RowCommand" ShowFooter="true"
                AutoGenerateColumns="false" CssClass="mGrid" OnRowDataBound="grdRisk_RowDataBound"
                AllowSorting="true" OnSorting="grd_Sorting" GridLines="horizontal" DataKeyNames="MerchantAppUID,BatchID,Voided By"
                Width="98.8%">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <FooterStyle HorizontalAlign="Right" CssClass="footer" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img runat="server" id='img1' src="../Images/minus.JPG" onmouseover="this.style.cursor='pointer';"
                                alt="img" />
                        </ItemTemplate>
                        <ItemStyle Width="20px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="MerchantAppUID" Visible="False" />
                    <asp:BoundField DataField="BatchID" Visible="False" />
                    <asp:BoundField DataField="Voided By" Visible="False" />
                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy hh:mm tt}" />
                    <asp:TemplateField HeaderText="MID" SortExpression="MID">
                        <ItemTemplate>
                            <asp:HyperLink NavigateUrl='<%#  "~/SecureRiskForms/frmRiskBatchDetails.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&BatchID=" +  Eval("BatchID") %>'
                                runat="server" ID="hypZID" Text='<%# Eval("MID") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ZID" HeaderText="ZID" SortExpression="ZID" />
                    <asp:BoundField DataField="DBA Name" HeaderText="DBA Name" SortExpression="DBA Name" />
                    <asp:BoundField DataField="Legal Name" HeaderText="MLE" SortExpression="Legal Name" />
                    <asp:BoundField DataField="MCC" HeaderText="MCC" SortExpression="MCC" />
                    <asp:BoundField DataField="Agent Dba" HeaderText="Agent Dba" SortExpression="Agent Dba" />
                    <asp:BoundField DataField="Source ID" HeaderText="Source ID" SortExpression="Source ID" />
                    <asp:BoundField DataField="BatchTotal" HeaderText="Batch Total" DataFormatString="{0:0.00}"
                        SortExpression="BatchTotal">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SalesCount" HeaderText="Sales Cnt" SortExpression="SalesCount">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="SalesAmount" HeaderText="Sales Amt" DataFormatString="{0:0.00}"
                        SortExpression="SalesAmount">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreditCount" HeaderText="Credit Cnt" SortExpression="CreditCount">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CreditAmount" HeaderText="Credit Amt" DataFormatString="{0:0.00}"
                        SortExpression="CreditAmount">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Dates" HeaderText="Held By/Released By" SortExpression="Dates" />
                    <asp:BoundField DataField="Approved Volume" HeaderText="Approved Volume" DataFormatString="{0:0.00}"
                        SortExpression="Approved Volume">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MTD Volume" HeaderText="MTD Volume" DataFormatString="{0:0.00}"
                        SortExpression="MTD Volume">
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:BoundField>
                    <asp:BoundField DataField="BatchID" HeaderText="BatchID" SortExpression="BatchID" />
                    <asp:TemplateField HeaderText="Batch Status" SortExpression="Violations">
                        <ItemTemplate>
                            <asp:Label Text='<% # Eval("Violations") %>' runat="server" ID="lblViolations"></asp:Label>
                            </td> </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="19">
                                    <div id="<%# Eval("BatchID") %>" style="display: inline;">
                                        <asp:GridView ID="gvBatch" runat="server" GridLines="none" Font-Names="Verdana" Font-Size="x-small"
                                            EmptyDataText="No data." Width="40%" AutoGenerateColumns="false" DataKeyNames="MerchantAppUID,BatchID">
                                            <Columns>
                                                <asp:BoundField DataField="MerchantAppUID" Visible="false" />
                                                <asp:BoundField DataField="BatchID" Visible="false" />
                                                <asp:BoundField DataField="RiskID" HeaderText="Risk ID" />
                                                <asp:BoundField DataField="RiskException" HeaderText="Risk Exception" />
                                                <asp:BoundField DataField="Threshold" HeaderText="Threshold" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:Panel>
        <%--         <div class="bucketfooter">
            <div class="bucketfooterleft">
                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                <asp:LinkButton ID="btnPDF" runat="server" OnClick="btnExportPDF_Click">
                    <span style="height: 25px; vertical-align: middle;">
                        <asp:Image ID="Image2" runat="server" SkinID="SavePDF" /></span><span style="margin-left: 5px;">Save
                            PDF</span></asp:LinkButton>
            </div>
        </div>--%>
        <asp:Label runat="server" ID="noData" Text="No Data..."></asp:Label>
    </fieldset>
    <br />      
    </div>
</asp:Content>
