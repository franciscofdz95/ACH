<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="frmWeeklySummaryReport" MasterPageFile="~/MasterPageSales.master" Title="Inside Sales Weekly Report" Codebehind="frmWeeklySummaryReport.aspx.cs" %>

<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/wucSalesWeeklyGrid.ascx" TagName="SalesWeekly" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

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
            var gridViewCtlId = document.getElementById('<%=grdCat.ClientID%>');
            if (null != gridViewCtlId)
            {
             
                var i = 0;
                var j = 2;
                for(;i<(gridViewCtlId.rows.length);i=i+1)
                {  
                    if(txt == '1')                    
                    {                    
                      if(j<10)
                       j ='0'+j;                        
                      CollapseExpand(gridViewCtlId.id+'_ctl'+j+'_div1','inline',gridViewCtlId.id+'_ctl'+j+'_img1');
                      j++;
                    }
                    else
                    {
                      if(j<10)
                        j ='0'+j; 
                      CollapseExpand(gridViewCtlId.id+'_ctl'+j+'_div1','none',gridViewCtlId.id+'_ctl'+j+'_img1'); 
                      j++;
                    }
                }
            }
        }
    </script>

    <div id="contentpage">
        <div class="dialog" style="padding-right: 10px;">
            <fieldset>
                <legend>Inside Sales Summary Weekly Report</legend>
                <br />
                <asp:Panel runat="server" ID="pnlSearch">
                    <table>
                        <tr>
                            <%--  <td class="lblRight">
                            Agent:</td>
                        <td>
                            <asp:DropDownList ID="AgentAgentID" runat="server" Width="300px" OnSelectedIndexChanged="AgentAgentID_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentAgentID"
                                PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                IsSorted="true" QueryPattern="Contains">
                            </cc1:ListSearchExtender>
                        </td>--%>
                            <td colspan="2">
                                <asp:Panel runat="server" ID="AgentSelect">
                                    <uc3:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="2" IDWidth="120"
                                        DBAWidth="120" lblDBAWidth="70" lblIDWidth="65" />
                                </asp:Panel>
                            </td>
                            <td class="lblRight">
                                &nbsp; Period:</td>
                            <td>
                                <asp:DropDownList ID="Date" runat="server" Width="150px" AutoPostBack="true">
                                </asp:DropDownList>
                            </td>
                            <td>
                                &nbsp;
                                <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                                &nbsp;
                                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br />
                <asp:Panel runat="server" ID="pnlSales">
                    <div style="width: 100%">
                        <div class="buckethdrleft">
                            <asp:Panel runat="server" ID="pnl1">
                                <a id="lnkExpandAll" onmouseover="this.style.cursor='pointer';" onclick="expandAll('1')">
                                    Expand All</a> | <a id="lnkCollapseAll" onmouseover="this.style.cursor='pointer';"
                                        onclick="expandAll('0')">Collapse All</a>
                            </asp:Panel>
                        </div>
                        <div class="buckethdright">
                            &nbsp;</div>
                    </div>
                    <asp:GridView ID="grdCat" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                        HorizontalAlign="left" Font-Names="Verdana" Font-Size="X-Small" OnRowDataBound="grdCat_RowDataBound"
                        ShowHeader="false" BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Category" SortExpression="Name">
                                <ItemTemplate>
                                    <br />
                                    <div class="title" style="width: 100%;">
                                        <img runat="server" id='img1' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                            alt="img" />&nbsp;
                                        <asp:Label runat="server" ID="lblName" ForeColor="#0a94d6" Font-Bold="true"></asp:Label>
                                        <hr class="line" />
                                    </div>
                                    <div id="div1" style="display: inline;" runat="server">
                                        <uc1:SalesWeekly runat="server" ID="SalesWeekly1" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Label runat="server" ID="lblData" Text="No Records..." Visible="true"></asp:Label>
            </fieldset>
        </div>
    </div>
</asp:Content>
