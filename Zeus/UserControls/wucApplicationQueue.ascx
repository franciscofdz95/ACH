<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucApplicationQueue" CodeBehind="wucApplicationQueue.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>

<script language="JavaScript" type="text/javascript">
    function CollapseExpand(object, txt, object1, UID) {
        var div = document.getElementById(object);
        var object2 = document.getElementById(object1);

        if (div.style.display == "none") {
            div.style.display = "inline";
            object2.src = "../Images/minus.JPG";
        }
        else {
            div.style.display = "none";
            object2.src = "../Images/plus.JPG";
        }

        var name = UID + '1';
        var divtag = document.getElementById(name);
        if (txt == 'Reschedule Training') {
            document.getElementById(UID).style.display = "inline";
            divtag.style.display = "inline";
        }
        else {
            document.getElementById(UID).style.display = "none";
            divtag.style.display = "none";
        }
    }

    function TogglePanel(ddp, UID) {
        var name = UID + '1';
        var div = document.getElementById(name);
        var ddlist = document.getElementById(ddp);

        if (ddlist.options[ddlist.selectedIndex].value == 'Reschedule Training') {
            document.getElementById(UID).style.display = "inline";
            div.style.display = "inline";
        }
        else {
            document.getElementById(UID).style.display = "none";
            div.style.display = "none";
        }
    }
</script>
<asp:UpdatePanel runat="server" ID="pnlDate">
    <contenttemplate>
<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="Label"></asp:Label>
    </legend>    
    <asp:Panel runat="server" ID="Date">
        <table>
            <tr>
                <td>Conditional Due Date:</td>
                <td>
                    <ig:WebDatePicker ID="ConditionalDueDate" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                        AutoPostBack-ValueChanged="true" Width="100px" EnableAppStyling="False" OnValueChanged="ConditionalDueDate_ValueChanged">
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" />
                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" />
                    </ig:WebDatePicker>
                </td>
                <td></td>
                <td>
                    <asp:CheckBox runat='server' ID="AllDates" Text="All Dates" AutoPostBack="true" OnCheckedChanged="AllDates_CheckedChanged" /></td>
            </tr>
        </table>
        <hr />
    </asp:Panel>
    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
        <table width="100%">
            <tr>
                <td class="lblLeft">Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="true">10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>150</asp:ListItem>
                                <asp:ListItem>200</asp:ListItem>
                                <asp:ListItem>250</asp:ListItem>
                                <asp:ListItem>300</asp:ListItem>
                            </asp:DropDownList></td>
                <td class="lblRight">
                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>
        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt" DataKeyNames="UID" OnRowDataBound="grd_RowDataBound"
            OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
            OnSorting="grd_Sorting" DataSourceID="odsMerchants">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <img runat="server" id='img1' src="../Images/Plus.JPG" onmouseover="this.style.cursor='pointer';"
                            alt="img" />
                    </ItemTemplate>
                    <ItemStyle Width="20px" />
                </asp:TemplateField>
                <asp:BoundField DataField="MerchantAppUID" HeaderText="MerchantAppUID" Visible="False" />
                <asp:BoundField DataField="StatusUID" HeaderText="StatusUID" Visible="False" />
                <asp:BoundField DataField="UID" HeaderText="UID" Visible="False" />
                <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hypZID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width="45px" />
                </asp:TemplateField>
                <asp:BoundField DataField="FMAID" HeaderText="FMA ID" SortExpression="FMAID" Visible="false">
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="DBA" SortExpression="BusinessDBAName">
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmUnderwritingPending.aspx?MerchantAppUID=" + Eval("UID") + "&Adding=false&PostBackURL=" + PostBackURL  %>'
                            runat="server" ID="hypDBAName" Text='<%#Eval("BusinessDBAName") %>'></asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle Width="163px" />
                </asp:TemplateField>
                <asp:BoundField DataField="MLE" HeaderText="MLE" SortExpression="MLE">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="DateCreated" HeaderText="Received Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DateCreated">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="StatusChangedDate" HeaderText="Queue Date"  DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="StatusChangedDate">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="EquipmentType" HeaderText="Equipment Type" SortExpression="EquipmentType">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Bank" HeaderText="Bank" SortExpression="Bank">
                    <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="AgentFullName" HeaderText="Agent Name" SortExpression="AgentFullName">
                    <ItemStyle Width="168px" />
                </asp:BoundField>
                <asp:BoundField DataField="MonthlyVolume" HeaderText="Volume" DataFormatString="{0:0.00}" SortExpression="MonthlyVolume">
                    <ItemStyle Width="80px" HorizontalAlign="Right" />
                </asp:BoundField>                
                <asp:BoundField DataField="VolumeLevel" HeaderText="Volume Level" SortExpression="VolumeLevel" Visible="false">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="AssignToName" HeaderText="Assigned To" SortExpression="AssignToName" Visible="false">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="SS Rep" SortExpression="RMRep">
                    <ItemTemplate>
                        <asp:Label Text='<% # Eval("RMRep")%>' runat="server" ID="lblAssignedTo"></asp:Label>
                        </td> </tr>
                                <tr>
                                    <td></td>
                                    <td colspan="12">
                                        <div id='<%# Eval("ID") %>' style="display: none; border: none 0px;">
                                            <asp:Panel runat="server" ID="pnlTraining">
                                                <table style="border: none 0px;">
                                                    <tr style="border: none 0px;">
                                                        <td style="border: none 0px;">Training Conducted By:
                                                        </td>
                                                        <td style="border: none 0px;">
                                                            <asp:DropDownList ID="ddpBy" runat="server" Width="130px">
                                                                <asp:ListItem Selected="true">-- Select --</asp:ListItem>
                                                                <asp:ListItem>Internal</asp:ListItem>
                                                                <asp:ListItem>Partner</asp:ListItem>
                                                                <asp:ListItem>Client Refused Training</asp:ListItem>
                                                                <asp:ListItem>Reschedule Training</asp:ListItem>
                                                                <asp:ListItem>Unable to reach</asp:ListItem>
                                                                <asp:ListItem>Completed Training</asp:ListItem>
                                                            </asp:DropDownList>&nbsp;
                                                        </td>
                                                        <td style="border: none 0px;">
                                                            <div id='<%# Eval("UID") %>1' style="display: inline;">
                                                                Training Date:
                                                            </div>
                                                        </td>
                                                        <td style="border: none 0px;">
                                                            <div id='<%# Eval("UID") %>' style="display: inline;">
                                                                <ig:WebDatePicker ID="TrainingDate" runat="server" NullDateLabel="" NullValueRepresentation="Null"
                                                                    Width="100px" EnableAppStyling="False"><CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr style="border: none 0px;">
                                                        <td colspan="4" style="border: none 0px;">
                                                            <asp:Button runat="server" ID="btnSave" Text="Save" OnClick="btnSave_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                    </ItemTemplate>
                    <ItemStyle Width="125px" />
                </asp:TemplateField>
                <asp:BoundField DataField="ConditionalDueDate" HeaderText="Due Date" SortExpression="ConditionalDueDate" DataFormatString="{0:MM/dd/yyyy}">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="PendedBy" HeaderText="Previously Pended By" SortExpression="PendedBy">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="RMRep" HeaderText="SS Rep" SortExpression="RMRep">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
                <asp:BoundField DataField="PremierRepName" HeaderText="SS QA Rep" SortExpression="PremierRepName" Visible="false">
                    <ItemStyle Width="90px" />
                </asp:BoundField>
            </Columns>
            <PagerStyle CssClass="pgr" />
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
        <asp:ObjectDataSource ID="odsMerchants" runat="server" SelectMethod="GetQueuesPaging"
            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
            SelectCountMethod="GetQueuesPagingRowCount" StartRowIndexParameterName="CurrentPage"
            OldValuesParameterFormatString="original_{0}" OnSelecting="odsMerchants_Selecting">
            <SelectParameters>
                <asp:Parameter Name="prms" Type="Object" />
                <asp:Parameter Name="PageSize" Type="Int32" />
                <asp:Parameter Name="CurrentPage" Type="Int32" />
                <asp:Parameter Name="ControlID" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </asp:Panel>
    <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="">
        No data...
    </asp:Panel>   
</fieldset>
</contenttemplate>
</asp:UpdatePanel>
<table width="100%" style="vertical-align: top;">
    <tr valign="top">
        <td align="left" style="width: 33%;" valign="top">
            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                <span style="height: 25px; vertical-align: middle;">
                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                                                    Excel</span>
            </asp:LinkButton>&nbsp;&nbsp;
        </td>
        <td align="right" valign="middle">Export:&nbsp;</td>
        <td align="left" valign="middle">
            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                <asp:ListItem Value="0">Current Page</asp:ListItem>
                <asp:ListItem Selected="true" Value="1">All Pages</asp:ListItem>
            </asp:RadioButtonList></td>
        <td align="right" style="width: 33%;"></td>
    </tr>
</table>

