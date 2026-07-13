<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/MasterPageSales.master" Inherits="frmPipelineStatusReport"
    Title="Pipeline Status Report" Codebehind="frmPipelineStatusReport.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="JavaScript" type="text/javascript">
        
        function CollapseExpand(object,txt,object1)
        {
            var div = document.getElementById(object); 
            var object2 = document.getElementById(object1);   
          
            if(txt == null)
            {            
                if(div.style.display == "none")
                {
                   div.style.display = "inline";
                   object2.src = "../Images/close.gif";
                }
                else
                {
                   div.style.display = "none";
                   object2.src = "../Images/open.gif";
                }   
            }
            else
            {
               div.style.display = txt;
                if(txt == 'none')
                    object2.src = "../Images/open.gif";
                else
                    object2.src = "../Images/close.gif";
            }
        }
        
        function expandAll(txt)
        {            
           var i = 1;
                            
           for(;i<5;i=i+1)
           {  
                if(txt == '1') 
                    CollapseExpand('ctl00_ContentPlaceHolder1_div'+i,'inline','ctl00_ContentPlaceHolder1_img'+i);
                else
                    CollapseExpand('ctl00_ContentPlaceHolder1_div'+i,'none','ctl00_ContentPlaceHolder1_img'+i);
           }
        }  
    
    </script>

    <div class="dialog">
        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
            <div class="title">
                &nbsp;&nbsp;Pipeline Status Report
                <hr class="line" />
            </div>
            <asp:ValidationSummary runat="server" ID="ValidSummary" ShowSummary="true" DisplayMode="BulletList"
                Visible="true" />
            <asp:BulletedList runat="server" ID="lblError" CssClass="errorlist">
            </asp:BulletedList>
            <table cellspacing="2">
                <tr>
                    <td class="lblRight">
                        Time Frame:</td>
                    <td>
                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlTimeFrame" OnSelectedIndexChanged="ddlTimeFrame_SelectedIndexChanged">
                            <asp:ListItem Value="-1">Select Time Frame</asp:ListItem>
                            <asp:ListItem Value="1">Last 30 days</asp:ListItem>
                            <asp:ListItem Value="2">Last 60 days</asp:ListItem>
                            <asp:ListItem Value="3">Last 90 days</asp:ListItem>
                            <asp:ListItem Value="4">Last Month</asp:ListItem>
                            <asp:ListItem Value="5">Month to Date</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Begin Date:</td>
                    <td align="left">
                        <ig:WebDatePicker id="SearchBeginDate" runat="server" backcolor="#EFF3FF" borderstyle="Solid"
                            borderwidth="1px" enableappstyling="False" nulldatelabel="" width="150px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        End Date:</td>
                    <td align="left">
                        <ig:WebDatePicker id="SearchEndDate" runat="server" backcolor="#EFF3FF" borderstyle="Solid"
                            borderwidth="1px" enableappstyling="False" nulldatelabel="" width="150px">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    </td>
                </tr>
                <tr>
                    <%--<td class="lblRight">
                        Agent:</td>
                    <td align="left">
                        <asp:DropDownList ID="AgentUID" runat="server" Width="300px">
                        </asp:DropDownList><cc1:listsearchextender id="ListSearchExtender1" runat="server"
                            targetcontrolid="AgentUID" prompttext="Type to search" promptcssclass="ListSearchExtenderPrompt"
                            promptposition="Top" issorted="true" querypattern="Contains">
                        </cc1:listsearchextender>
                    </td>--%>
                    <td colspan="2">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:agentselector runat="server" id="wucAgentSelector" layoutstyle="vertical" idwidth="105px"
                                dbawidth="145px" lbldbawidth="122px" lblidwidth="122px" />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" ID="lblchk" Width="120px" Text="Include Sub-Agents:"></asp:Label></td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkSubAgent" /></td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Include Inactive Agents:</td>
                    <td align="left">
                        <asp:CheckBox runat="server" ID="chkAgent" /></td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td align="left">
                        <div>
                            <br />
                            <igtxt:webimagebutton id="btnSearch" runat="server" onclick="btnSearch_Click" text="Search"
                                accesskey="h">
                                <Appearance>
                                    <Image Url="~/Images/Check.png" />
                                </Appearance>
                            </igtxt:webimagebutton>
                            &nbsp;
                            <igtxt:webimagebutton id="btnClear" runat="server" onclick="btnClear_Click" text="Clear"
                                causesvalidation="False" accesskey="l">
                                <Appearance>
                                    <Image Url="~/Images/delete.png" />
                                </Appearance>
                            </igtxt:webimagebutton>
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <br />
        <div class="title">
            &nbsp;&nbsp;Search Results
            <hr class="line" />
        </div>
        <asp:Label runat="server" ID="lblData" Text="No Data" Visible="true"></asp:Label>
        <asp:Panel runat="server" ID="pnl1" Width="99%" ScrollBars="none" Visible="false">
            <div style="width: 950px">
                <div class="buckethdrleft">
                    <asp:Panel runat="server" ID="Panel1" Width="" Height="" Visible="false">
                        &nbsp; <a id="lnkExpandAll" onmouseover="this.style.cursor='pointer';" onclick="expandAll('1')">
                            Expand All</a> | <a id="lnkCollapseAll" onmouseover="this.style.cursor='pointer';"
                                onclick="expandAll('0')">Collapse All</a>
                    </asp:Panel>
                </div>
            </div>
            <br />
            <br />
            <table width="100%">
                <tr>
                    <td class="lblLeft">
                        <asp:Panel runat="server" ID="pnlGrid1" Width="500px">
                            <div class="title" style="width: 500px;">
                                <img runat="server" id='img1' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                    alt="img" />&nbsp;
                                <asp:Label runat="server" ID="lblName" Text="All Leads" ForeColor="#0a94d6" Font-Bold="true"></asp:Label>
                                <hr class="line" />
                            </div>
                            <div id="div1" style="display: inline;" runat="server">
                                <asp:GridView runat="server" ID="grdPipelineStatus" AutoGenerateColumns="false" CssClass="mGrid"
                                    OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    AllowSorting="true" ShowFooter="true" EmptyDataText="No Data..." OnRowDataBound="grd_RowDataBound">
                                    <RowStyle VerticalAlign="Top" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Agent ID" SortExpression="AgentID" ItemStyle-Width="30px" 
                                            DataField="AgentID" HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" HeaderStyle-Width="100px" 
                                            ItemStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Active" SortExpression="ActiveCnt" ItemStyle-HorizontalAlign="right"
                                            DataField="ActiveCnt" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Dead" SortExpression="DeadCnt" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="DeadCnt"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Approved" SortExpression="ApprovedCnt" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="ApprovedCnt"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Total" SortExpression="TotalCnt" ItemStyle-HorizontalAlign="right"
                                            DataField="TotalCnt" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="* Created during Period" SortExpression="CreatedDateCnt"
                                            HeaderStyle-Width="30px" DataField="CreatedDateCnt" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label runat="server" ID="lbl" Text="* Date range search will only affect the 'Leads Created During Period' column. By nature, all other values will only show current data."
                                    Font-Italic="true" Font-Size="X-Small"></asp:Label>
                                <div class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                            Excel</span></asp:LinkButton>
                                            </td>
                                            <td align="right">
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
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblLeft">
                        <asp:Panel runat="server" ID="pnlGrid2" Width="800px">
                            <div class="title" style="width: 800px;">
                                <img runat="server" id='img2' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                    alt="img" />&nbsp;
                                <asp:Label runat="server" ID="Label1" Text="Active Leads" ForeColor="#0a94d6" Font-Bold="true"></asp:Label>
                                <hr class="line" />
                            </div>
                            <div id="div2" style="display: inline;" runat="server">
                                <asp:GridView runat="server" ID="GridView1" AutoGenerateColumns="false" CssClass="mGrid"
                                    OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    AllowSorting="true" ShowFooter="true" EmptyDataText="No Data..." OnRowDataBound="grd1_RowDataBound">
                                    <RowStyle VerticalAlign="Top" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Agent ID" SortExpression="AgentID" ItemStyle-Width="30px" 
                                            HeaderStyle-Width="30px" DataField="AgentID"></asp:BoundField>
                                        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" ItemStyle-Width="100px" 
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="New" SortExpression="New" ItemStyle-HorizontalAlign="right"
                                            HeaderStyle-Width="30px" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"
                                            DataField="New"></asp:BoundField>
                                        <asp:BoundField HeaderText="Assigned" SortExpression="Assigned" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Assigned"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="In Comm." SortExpression="In Communication" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="In Communication"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="No Answer" SortExpression="No Answer" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="No Answer"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Follow Up" SortExpression="Follow up" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Follow up"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Stmts Rec'd" SortExpression="Statements Received" DataField="Statements Received"
                                            ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="App Sent" SortExpression="Application Sent" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Application Sent"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="App Rec'd" SortExpression="Application Received" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Application Received"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="File In Review" SortExpression="File In Review" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" HeaderStyle-Width="30px"
                                            DataField="File In Review"></asp:BoundField>
                                        <asp:BoundField HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="right"
                                            HeaderStyle-Width="30px" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px">
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <div class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                            Excel</span></asp:LinkButton>
                                            </td>
                                            <td align="right">
                                                Page Size:
                                                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>100</asp:ListItem>
                                                    <asp:ListItem>250</asp:ListItem>
                                                    <asp:ListItem>500</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblLeft">
                        <asp:Panel runat="server" ID="pnlGrid3" Width="700px">
                            <div class="title" style="width: 700px;">
                                <img runat="server" id='img3' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                    alt="img" />&nbsp;
                                <asp:Label runat="server" ID="Label2" Text="Dead Leads" ForeColor="#0a94d6" Font-Bold="true"></asp:Label>
                                <hr class="line" />
                            </div>
                            <div id="div3" style="display: inline;" runat="server">
                                <asp:GridView runat="server" ID="GridView2" AutoGenerateColumns="false" CssClass="mGrid"
                                    OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    AllowSorting="true" ShowFooter="true" EmptyDataText="No Data..." OnRowDataBound="grd2_RowDataBound">
                                    <RowStyle VerticalAlign="Top" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Agent ID" SortExpression="AgentID" ItemStyle-Width="30px" 
                                            HeaderStyle-Width="30px" DataField="AgentID"></asp:BoundField>
                                        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" ItemStyle-Width="100px" 
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Not Interested" SortExpression="Not Interested" DataField="Not Interested"
                                            ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Withdrawn" SortExpression="Withdrawn" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Withdrawn"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Declined" SortExpression="Declined" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" DataField="Declined"
                                            HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Wrong Contact Info" SortExpression="Wrong Contact Info"
                                            DataField="Wrong Contact Info" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right"
                                            ItemStyle-Width="30px" HeaderStyle-Width="30px"></asp:BoundField>
                                        <asp:BoundField HeaderText="Total" SortExpression="Total" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" HeaderStyle-Width="30px" ItemStyle-Width="30px">
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <div class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image3" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                            Excel</span></asp:LinkButton>
                                            </td>
                                            <td align="right">
                                                Page Size:
                                                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>100</asp:ListItem>
                                                    <asp:ListItem>250</asp:ListItem>
                                                    <asp:ListItem>500</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td class="lblLeft">
                        <asp:Panel runat="server" ID="pnlGrid4" Width="300px">
                            <div class="title" style="width: 300px;">
                                <img runat="server" id='img4' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                    alt="img" />&nbsp;
                                <asp:Label runat="server" ID="Label3" Text="Approved Leads" ForeColor="#0a94d6" Font-Bold="true"></asp:Label>
                                <hr class="line" />
                            </div>
                            <div id="div4" style="display: inline;" runat="server">
                                <asp:GridView runat="server" ID="GridView3" AutoGenerateColumns="false" CssClass="mGrid"
                                    OnSorting="grd_Sorting" AllowPaging="true" OnPageIndexChanging="grd_PageIndexChanging"
                                    AllowSorting="true" ShowFooter="true" EmptyDataText="No Data..." OnRowDataBound="grd3_RowDataBound">
                                    <RowStyle VerticalAlign="Top" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Agent ID" SortExpression="AgentID" ItemStyle-Width="30px" 
                                            HeaderStyle-Width="30px" DataField="AgentID"></asp:BoundField>
                                        <asp:BoundField DataField="Agent" HeaderText="Agent" SortExpression="Agent" ItemStyle-Width="100px" 
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField HeaderText="Approved" SortExpression="Approved" ItemStyle-HorizontalAlign="right"
                                            FooterStyle-HorizontalAlign="right" ItemStyle-Width="30px" HeaderStyle-Width="30px"
                                            DataField="Approved"></asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <div class="bucketfooter">
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                <asp:LinkButton ID="LinkButton3" runat="server" OnClick="btnExport_Click">
                                                    <span style="height: 25px; vertical-align: middle;">
                                                        <asp:Image ID="Image4" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                            Excel</span></asp:LinkButton>
                                            </td>
                                            <td align="right">
                                                Page Size:
                                                <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                    <asp:ListItem Selected="True">10</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                    <asp:ListItem>100</asp:ListItem>
                                                    <asp:ListItem>250</asp:ListItem>
                                                    <asp:ListItem>500</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <br />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
</asp:Content>
