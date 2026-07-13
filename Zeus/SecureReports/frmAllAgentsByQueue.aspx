<%@ Page Language="C#" AutoEventWireup="true"
    Inherits="frmAllAgentsByQueue" MasterPageFile="~/MasterPageReports.master" Codebehind="frmAllAgentsByQueue.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
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
                   object2.src = "../Images/minus.JPG";
                }
                else
                {
                   div.style.display = "none";
                   object2.src = "../Images/plus.JPG";
                }   
            }
            else
            {
                div.style.display = txt;
                if(txt == 'none')
                    object2.src = "../Images/plus.JPG";
                else
                    object2.src = "../Images/minus.JPG";
            }
        }
        
        function expandAll(txt)
        {            
            var gridViewCtlId = document.getElementById('<%=grd.ClientID%>');
            if (null != gridViewCtlId)
            {
                var i = 1;
                var j = 2;
                for(;i<(gridViewCtlId.rows.length - 1);i=i+2)
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

    <table width="100%">
        <tr>
            <td>
                <div class="dialog">
                    <fieldset>
                        <legend>Agent Call Summary Report By Queue (Agent breakdown view)</legend>
                        <br />
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <th class="lblRight">
                                    Agents:
                                </th>
                                <td>
                                    All Agents
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <th class="lblRight">
                                    Start Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="StartDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="80px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td>
                                    <asp:DropDownList ID="StartTime" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <th class="lblRight">
                                    Queues:
                                </th>
                                <td>
                                    <asp:DropDownList ID="Queues" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 20px;">
                                </td>
                                <th class="lblRight">
                                    End Time:</th>
                                <td>
                                    <ig:WebDatePicker ID="EndDateTime" runat="server" NullDateLabel="" EnableAppStyling="False"
                                        Width="80px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                </td>
                                <td>
                                    <asp:DropDownList ID="EndTime" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" align="center">
                                    <br />
                                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                                    <asp:Button runat="server" ID="btnClear" Text="Clear" OnClick="btnClear_Click" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <div style="width: 100%">
                            <div class="buckethdrleft">
                                <asp:Panel runat="server" ID="pnl1">
                                    <a id="lnkExpandAll" onmouseover="this.style.cursor='pointer';" onclick="expandAll('1')">
                                        Expand All</a> | <a id="lnkCollapseAll" onmouseover="this.style.cursor='pointer';"
                                            onclick="expandAll('0')">Collapse All</a> |
                                    <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                        <span style="height: 25px; vertical-align: middle;">
                                            <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                Excel</span></asp:LinkButton>
                                </asp:Panel>
                            </div>
                            <div class="buckethdright">
                                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>
                                &nbsp;</div>
                        </div>
                        <br />
                        <%--<div style="float: left; width: 100%">--%>
                        <asp:GridView runat="server" ShowFooter="true" AutoGenerateColumns="false" CssClass="mGrid"
                            OnRowDataBound="grd_RowDataBound" AllowSorting="true" OnSorting="grd_Sorting"
                            GridLines="horizontal" DataKeyNames="Ext" ID="grd" Style="float: left; width: 100%;">
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
                                <asp:BoundField DataField="Ext" HeaderText="Ext" SortExpression="Ext" />
                                <asp:BoundField DataField="Name" HeaderText="Agent ID" SortExpression="Name" />
                                <asp:TemplateField HeaderText="Total IncomingCalls" SortExpression="IncomingCalls">
                                    <ItemTemplate>
                                        <asp:Label Text='<% # Eval("IncomingCalls") %>' runat="server" ID="lblName"></asp:Label>
                                        </td> </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="3">
                                                <div id='div1' style="display: inline;" runat="server">
                                                    <asp:GridView ID="grdQueue" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                        CssClass="mGrid" OnRowDataBound="grdQueue_RowDataBound" Height="100%" EmptyDataText="No data.">
                                                        <PagerStyle CssClass="pgr" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                        <FooterStyle HorizontalAlign="Right" CssClass="footer" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="Time" DataField="Time" ItemStyle-Width="60px" />
                                                            <asp:BoundField DataField="IncomingCalls" HeaderText="Inc. Calls" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="AvgLenofInboundCalls" HeaderText="Avg Call Time (sec)"
                                                                ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="AvgInboundHoldTime" HeaderText="Avg Hold Time(sec)" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="ActualInboundTalkTime" HeaderText="Total Call Time (sec)"
                                                                ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="CallsfromMerchant" HeaderText="Merchant Queue Calls" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="CallsfromAgent" HeaderText="Agent Queue Calls" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="DCACalls" HeaderText="DCA Queue Calls" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="DCABillingCalls" HeaderText="DCA Billing Queue Calls" ItemStyle-HorizontalAlign="right" />
                                                            <asp:BoundField DataField="DCACancelCalls" HeaderText="DCA Cancel Queue Calls" ItemStyle-HorizontalAlign="right" />
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label runat="server" ID="noData" Text="No Data..." Style="float: left;"></asp:Label>
                        <%-- </div>--%>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
