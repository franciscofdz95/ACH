<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="True" Inherits="frmLeads" Title="Leads Search" CodeBehind="frmLeads.aspx.cs" %>


<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/wucAgent.ascx" TagName="wucAgent" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucAppointments.ascx" TagName="Appointments" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucAddtoOutlook.ascx" TagName="AddtoOutlook" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucLeadNotes.ascx" TagName="LeadNotes" TagPrefix="uc4" %>

<%@ MasterType VirtualPath="~/MasterPageSales.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript" src="../js/jquery.toggle_heading.js"></script>

    <script type="text/javascript">

        function closeWindow() {
            oWebDialogWindow2 = $find('<% =WebDialogWindow5.ClientID %>');
            oWebDialogWindow2.set_windowState($IG.DialogWindowState.Hidden);
        }

        function ClientCheck() {
            var valid = false;
            var gv = document.getElementById('<%=grd.ClientID %>');
            var ddp = document.getElementById('<%=ddpStatus.ClientID %>');
            var ddpAct = document.getElementById('<%=ddpAction.ClientID %>');

            for (var i = 0; i < gv.all.length; i++) {
                var node = gv.all[i];
                if (node != null && node.type == "checkbox" && node.checked) {
                    valid = true;
                    break;
                }
            }
            var message = "";

            if (!valid)
                message = "- Please select at least one lead to continue.\n";

            if (ddpAct.selectedIndex < 0)
                message += "- Please select an action.";
            else if (ddpAct.selectedIndex == 2 && ddp.selectedIndex == 0) {
                if (ddpAct.selectedIndex == 0)
                    message += "- Please select a rep.\n";
                else
                    message += "- Please select a status.\n";
            }

            if (message.length == 0)
                return true;
            else {
                alert(message);
                return false;
            }
        }

        function SelectAll(chk) {

            $('#<%=grd.ClientID %>').find("input:checkbox").each(function () {
                if (this != chk) {
                    this.checked = chk.checked;
                }

            });
        }

    </script>

    <div id="contentpage" style="width:100px;">
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="true"
            ShowSummary="false" ValidationGroup="AddLead" />
        <asp:ValidationSummary ID="ValidationSummary2" runat="server" ShowMessageBox="true"
            ShowSummary="false" ValidationGroup="SearchLead" />
        <asp:ValidationSummary ID="ValidationSummary3" runat="server" ShowMessageBox="true"
            ShowSummary="false" ValidationGroup="UploadLead" />
        <div class="dialog">
            <table>
                <tr>
                    <td valign="top">
                        <ig:WebTab ID="WebTab1" runat="server" Width="950px">
                            <Tabs>
                                <ig:ContentTabItem runat="server" Text="Search">
                                    <Template>
                                        <fieldset>

                                            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="" DefaultButton="btnSearch">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="6" valign="top">
                                                            <fieldset style="width: 93%;">
                                                                <legend><font size='1'><i>Select Date</i></font></legend>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td class="lblRight">Start Date:
                                                                        </td>
                                                                        <td>
                                                                            <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                                                                Width="105px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                            </ig:WebDatePicker>
                                                                        </td>
                                                                        <td class="lblRight">End Date:</td>
                                                                        <td>
                                                                            <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                                                                Width="105px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                            </ig:WebDatePicker>
                                                                        </td>
                                                                        <td colspan="2">
                                                                            <asp:RadioButtonList runat="server" ID="DateType" RepeatDirection="Horizontal">
                                                                                <asp:ListItem Text="Date Created" Value="0" Selected="true">
                                                                                </asp:ListItem>
                                                                                <asp:ListItem Text="Date Assigned" Value="1">
                                                                                </asp:ListItem>
                                                                                <asp:ListItem Text="Follow-up Date" Value="2">
                                                                                </asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </fieldset>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">DBA Name:
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessDBAName" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                                                        </td>
                                                        <td class="lblRight" valign="bottom">First Name:</td>
                                                        <td valign="bottom">
                                                            <asp:TextBox ID="BusinessContact" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                                                        </td>
                                                        <td class="lblRight" valign="bottom">Last Name:</td>
                                                        <td valign="bottom">
                                                            <asp:TextBox ID="BusinessLastContact" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                                                        </td>
                                                        <td class="lblRight">Office Phone:</td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessPhone" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Lead ID:</td>
                                                        <td>
                                                            <asp:TextBox ID="LeadID" runat="server" Width="120px" EnableViewState="False" ValidationGroup="SearchLead"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="LeadID"
                                                                ValidationGroup="SearchLead" ErrorMessage="Please enter a valid Lead ID." MaximumValue="100000"
                                                                MinimumValue="1" Type="Integer" Display="dynamic" Text="*"></asp:RangeValidator>
                                                        </td>
                                                        <td class="lblRight">ZID:</td>
                                                        <td>
                                                            <asp:TextBox ID="MerchantID" runat="server" Width="120px" EnableViewState="False"
                                                                ValidationGroup="SearchLead"></asp:TextBox>
                                                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="MerchantID"
                                                                ErrorMessage="Please enter a valid ZID." MaximumValue="10000000" MinimumValue="1"
                                                                Type="Integer" Display="dynamic" Text="*" ValidationGroup="SearchLead"></asp:RangeValidator>
                                                        </td>
                                                        <td class="lblRight">Fax:</td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessFax" runat="server" Width="120px" EnableViewState="False"></asp:TextBox></td>
                                                        <td class="lblRight">Email:</td>
                                                        <td>
                                                            <asp:TextBox ID="BusinessEmailAddress" runat="server" Width="120px" EnableViewState="False"></asp:TextBox></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Source:</td>
                                                        <td>
                                                            <asp:DropDownList ID="SourceID" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                        <td class="lblRight">State:</td>
                                                        <td>
                                                            <asp:DropDownList ID="BusinessState" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                        <td class="lblRight">Status:</td>
                                                        <td>
                                                            <asp:DropDownList ID="StatusID" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                        <td class="lblRight">Time Zone:</td>
                                                        <td>
                                                            <asp:DropDownList ID="TimeZone" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Probability of Closure:</td>
                                                        <td>
                                                            <asp:DropDownList ID="FollowUpStatusUID" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                        <td>Agent DBA:</td>
                                                        <td>
                                                            <asp:TextBox ID="AgentDBA" runat="server" Enabled="false" Width="125px"></asp:TextBox>
                                                            <asp:HiddenField ID="AgentUID" runat="server" />
                                                        </td>
                                                        <td>Agent ID:</td>
                                                        <td>
                                                            <asp:TextBox ID="AgentID" runat="server" Enabled="false" Width="105px"></asp:TextBox>
                                                            <asp:LinkButton OnClick="btnAgentSelect_Click" CausesValidation="false" ID="lbSelectAgent"
                                                                runat="server">Select</asp:LinkButton>
                                                        </td>
                                                        <td>Assigned To:
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="AssignedUserID" runat="server" Width="125px">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Lead Origin:</td>
                                                        <td>
                                                            <asp:DropDownList ID="LeadOriginID" runat="server" Width="125px">
                                                            </asp:DropDownList></td>
                                                        <td class="lblRight">Origin Details:</td>
                                                        <td>
                                                            <asp:TextBox ID="SourceDescription" runat="server" Width="125px"></asp:TextBox>
                                                        </td>
                                                   </tr>
                                                </table>
                                                <div style="text-align: center;">
                                                    <br />
                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click"
                                                        ValidationGroup="SearchLead"></asp:Button>
                                                    &nbsp;
                                                    <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" CausesValidation="false"
                                                        ValidationGroup="SearchLead"></asp:Button>
                                                    &nbsp;
                                                     <asp:Button ID="btnAdd" runat="server" Text="Add New Lead" OnClick="btnAdd_Click"
                                                            CausesValidation="false" AccessKey="A"></asp:Button>
                                                    <br />
                                                    <div style="clear: both;">
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </fieldset>
                                        <fieldset style="width: 98%;">
                                            <legend>Search Results</legend>
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                       &nbsp;
                                                    </td>
                                                    <td align="right">
                                                        <asp:Panel runat="server" ID="pnlAction" DefaultButton="btnSave" Visible="false">
                                                            <table cellpadding="3" cellspacing="3">
                                                                <tr>
                                                                    <td><b>Bulk Action:</b>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddpAction" runat="server" OnSelectedIndexChanged="ddpAction_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem Selected="True" Value="Assign">Assign</asp:ListItem>
                                                                            <asp:ListItem Value="Status">Set Status</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <b>
                                                                            <asp:Label runat="server" ID="lblAction" Text="Assign To:"></asp:Label></b>
                                                                    </td>
                                                                    <asp:Panel runat="server" ID="PnlStatus" Visible="false">
                                                                        <td>
                                                                            <asp:DropDownList ID="ddpStatus" runat="server">
                                                                            </asp:DropDownList>
                                                                            <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddpStatus"
                                                                                PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                                                                IsSorted="true" QueryPattern="Contains">
                                                                            </cc1:ListSearchExtender>
                                                                        </td>
                                                                    </asp:Panel>
                                                                    <asp:Panel runat="server" ID="pnlAgents" Visible="false" HorizontalAlign="left">
                                                                        <td><b>Assign To:</b></td>
                                                                        <td>
                                                                            <asp:DropDownList ID="AssignedUser" runat="server" Width="100px">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </asp:Panel>
                                                                    <td>
                                                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" AccessKey="v"
                                                                            OnClientClick="javascript:return ClientCheck();"></asp:Button>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <hr />
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                                                &nbsp; No Leads...
                                            </asp:Panel>
                                            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                                                <table width="100%">
                                                    <tr>
                                                        <td class="lblLeft"></td>
                                                        <td class="lblRight">
                                                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label runat="server" ID="lblError" SkinID="error"></asp:Label>
                                                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                                                AlternatingRowStyle-CssClass="alt" DataKeyNames="LeadID,ID" OnRowDataBound="grd_RowDataBound"
                                                                OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                                                OnSorting="grd_Sorting" DataSourceID="odsLeads">
                                                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkSelectAll" onclick="javascript:SelectAll(this);" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox runat="server" ID="chkAssign" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="5px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Lead ID" SortExpression="ID">
                                                                        <ItemTemplate>
                                                                            <asp:HyperLink NavigateUrl='<%# String.Format("~/SecureLeadForms/frmLeadsDetail.aspx?Adding=false&LeadUID={0}", Eval("LeadID"))  %>'
                                                                                runat="server" ID="hypLeadID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="10px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="DBA Name" SortExpression="DBAName">
                                                                        <ItemStyle Width="80px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("DBAName") %>' ID="lblDBA" Width="99%" Height="99%"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:TextBox runat="server" Text='<%#Eval("DBAName") %>' ID="txtDBA" Width="60px">
                                                                            </asp:TextBox>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Follow Up" SortExpression="FollowUpDate">
                                                                        <ItemStyle Width="60px" />
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("FollowUpDate","{0:MM/dd/yyyy}") %>' ID="lblFollowUp"
                                                                                Width="60px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <ig:WebDatePicker ID="FollowUpDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                                                                Width="60px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px" Value='<%#Eval("FollowUpDate","{0:MM/dd/yyyy}") %>'>
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                                                    SlideOpenDuration="1" />
                                                                            </ig:WebDatePicker>
                                                                        </EditItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField HeaderText="Agent" DataField="AgentDBA" SortExpression="AgentDBA" ReadOnly="true">
                                                                        <ItemStyle Width="100px" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="Assigned to" SortExpression="AssignedUser">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="lblAssignedTo" Width="100px" Text='<%#Eval("AssignedUser") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList runat="server" ID="ddpAssignedTo" Width="100px">
                                                                            </asp:DropDownList>
                                                                        </EditItemTemplate>
                                                                        <ItemStyle Width="100px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" Text='<%#Eval("Status") %>' ID="lblStatus"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <EditItemTemplate>
                                                                            <asp:DropDownList runat="server" ID="ddpStatus" Width="100px">
                                                                            </asp:DropDownList>
                                                                            <asp:Label runat="server" Text='<%#Eval("Status") %>' ID="lblStatus1" Visible="false"></asp:Label>
                                                                        </EditItemTemplate>
                                                                        <ItemStyle Width="90px" />
                                                                    </asp:TemplateField>
                                                                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy hh:mm tt}"
                                                                        ReadOnly="True" SortExpression="DateCreated">
                                                                        <ItemStyle Width="65px" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="LeadID" Visible="False" />
                                                                    <asp:BoundField HeaderText="Source" DataField="Source" ReadOnly="true">
                                                                        <ItemStyle Width="100px" />
                                                                    </asp:BoundField>                                                                    
                                                                    <asp:BoundField DataField="LeadOrigin" HeaderText="Lead Origin" SortExpression="LeadOrigin" />
                                                                    <asp:BoundField DataField="SourceDescription" HeaderText="Origin Details" SortExpression="SourceDescription" />
                                                                    <%--<asp:TemplateField HeaderText="Actions">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="lnkEdit" ImageUrl="~/images/edit.png" runat="server" CommandName="EditLead"
                                                                                ToolTip="Edit" CausesValidation="false"></asp:ImageButton>
                                                                            <asp:ImageButton ID="lnkUpdate" ImageUrl="~/images/disk_blue.png" runat="server"
                                                                                CommandName="UpdateLead" ToolTip="Update" CausesValidation="true"></asp:ImageButton>
                                                                            <asp:ImageButton ID="lnkCancel" ImageUrl="~/images/Cancel.jpg" runat="server" CommandName="CancelLead"
                                                                                ToolTip="Cancel" CausesValidation="false"></asp:ImageButton>
                                                                            <asp:ImageButton ID="lnkFollow" ImageUrl="~/images/Calendar_scheduleHS.png" runat="server"
                                                                                ToolTip="Follow Up" CommandName="Followup" CausesValidation="false" Visible="false"></asp:ImageButton>
                                                                            <asp:ImageButton ID="lnkAdd" ImageUrl="~/images/outlook-icon.jpg" runat="server"
                                                                                ToolTip="Outlook" CommandName="Outlook" CausesValidation="false"></asp:ImageButton>
                                                                            <asp:ImageButton ID="lnkNotes" ImageUrl="~/images/document_add.png" runat="server"
                                                                                ToolTip="Notes" CommandName="Notes" CausesValidation="false"></asp:ImageButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle Width="75px" />
                                                                    </asp:TemplateField>--%>
                                                                </Columns>
                                                                <PagerStyle CssClass="pgr" />
                                                                <AlternatingRowStyle CssClass="alt" />
                                                            </asp:GridView>
                                                            <asp:ObjectDataSource ID="odsLeads" runat="server" SelectMethod="GetLeadsPaging"
                                                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                                                SelectCountMethod="GetLeadsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsLeads_Selecting">
                                                                <SelectParameters>
                                                                    <asp:Parameter Name="prms" Type="Object" />
                                                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                                                    <asp:Parameter Name="ControlID" Type="String" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                                Visible="false" DataSourceID="odsLeads">
                                                                <Columns>
                                                                    <asp:BoundField HeaderText="Lead ID" DataField="ID" />
                                                                    <asp:BoundField DataField="InitialDate" HeaderText="Initial Date" SortExpression="InitialDate"></asp:BoundField>
                                                                    <asp:BoundField DataField="DBAName" HeaderText="DBA Name" SortExpression="DBAName"></asp:BoundField>
                                                                    <asp:BoundField DataField="ContactName" HeaderText="First Name" SortExpression="ContactName"></asp:BoundField>
                                                                    <asp:BoundField DataField="ContactLastName" HeaderText="Last Name" SortExpression="ContactLastName"></asp:BoundField>
                                                                    <asp:BoundField DataField="LastContactDate" HeaderText="Last Contact Date"></asp:BoundField>
                                                                    <asp:BoundField DataField="Status" HeaderText="Lead Status" SortExpression="Status"></asp:BoundField>
                                                                    <asp:BoundField DataField="LeadType" HeaderText="Lead Type" SortExpression="LeadType"></asp:BoundField>
                                                                    <asp:BoundField DataField="Email" HeaderText="Contact Email" SortExpression="Email"></asp:BoundField>
                                                                    <asp:BoundField DataField="PhoneNumber" HeaderText="Contact Phone Number" SortExpression="PhoneNumber"></asp:BoundField>
                                                                    <asp:BoundField DataField="AppointmentDate" HeaderText="Appointment Date" SortExpression="AppointmentDate"></asp:BoundField>
                                                                    <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" SortExpression="LeadType"></asp:BoundField>
                                                                    <asp:BoundField DataField="AssignedUser" HeaderText="Assign To" SortExpression="Email"></asp:BoundField>
                                                                    <asp:BoundField DataField="ClosureReason" HeaderText="Closure reason" SortExpression="PhoneNumber"></asp:BoundField>
                                                                    <asp:BoundField DataField="Datetheleadwasassigned" HeaderText="Date the lead was assigned" SortExpression="Datetheleadwasassigned"></asp:BoundField>
                                                                </Columns>
                                                            </asp:GridView>
                                                            <hr />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" style="width: 85%;">
                                                            <table width="100%" style="vertical-align: top;">
                                                                <tr valign="top">
                                                                    <td align="left" style="width: 33%;" valign="top">
                                                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                                            <span style="height: 25px; vertical-align: middle;">
                                                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                                                    Excel</span>
                                                                        </asp:LinkButton>&nbsp;&nbsp;
                                                                    </td>
                                                                    <td align="right" valign="top">Export:&nbsp;</td>
                                                                    <td align="left" valign="top">
                                                                        <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                                            <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                                            <asp:ListItem Value="1">All Pages</asp:ListItem>
                                                                        </asp:RadioButtonList></td>
                                                                    <td align="right" style="width: 33%;"></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td class="lblRight" valign="top" style="width: 15%;">Page Size:
                                                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                                <asp:ListItem Selected="True">10</asp:ListItem>
                                                                <asp:ListItem>15</asp:ListItem>
                                                                <asp:ListItem>20</asp:ListItem>
                                                                <asp:ListItem>25</asp:ListItem>
                                                                <asp:ListItem>50</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <br />
                                        </fieldset>
                                    </Template>
                                </ig:ContentTabItem>
                                <ig:ContentTabItem runat="server" Text="Upload">
                                    <Template>
                                        <fieldset>
                                            <div style="height: 215px;">
                                                <table cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td colspan="2">
                                                            <span style="font-size: x-small;">File Format: "DBA", "FirstName", "LastName", "Phone",
                                                                "Email","ProcessingVolume","AgentID","DBA","Phone","Website","Addr1","City","State","Country","ApptStartDateTime (mm/dd/yyyy hh:mm:ss)","ApptEndDateTime (mm/dd/yyyy hh:mm:ss)","Notes","ReferenceNumber","Notepad","Currency"
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <span style="font-size: xx-small;">Note: files must contain title(s) in the header and
                                                                be comma separated.</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2"></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Download Template:
                                                        </td>
                                                        <td>
                                                            <asp:HyperLink runat="server" ID="lnkTemplate" Target="_self" NavigateUrl="~/SecureLeadForms/UploadedFiles/Template/LeadsTemplate.xlsx"
                                                                Font-Underline="true" Font-Bold="true">Lead Template</asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Upload List:</td>
                                                        <td>
                                                            <asp:FileUpload ID="Leads" runat="server" Width="222px" />
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please select a file"
                                                                Display="None" ControlToValidate="Leads" ValidationGroup="UploadLead"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Lead Source:</td>
                                                        <td>
                                                            <asp:DropDownList ID="UploadSource" runat="server" Width="155px" ValidationGroup="UploadLead">
                                                            </asp:DropDownList>
                                                           <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please select a Source"
                                                                Display="None" ControlToValidate="UploadSource" ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="UploadLead" Text="*"></asp:CompareValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Lead Origin:</td>
                                                        <td>
                                                           <asp:DropDownList ID="UploadOrigin" runat="server" Width="155px">
                                                           </asp:DropDownList>
                                                            <%--<asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Please select a Origin"
                                                                Display="None" ControlToValidate="UploadOrigin" ValueToCompare="-1" Operator="NotEqual"
                                                                ValidationGroup="UploadLead" Text="*"></asp:CompareValidator>--%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight">Origin details:</td>
                                                        <td>
                                                            <asp:TextBox ID="SourceDesc" Text="" Width="150px" runat="server" ValidationGroup="UploadLead"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="lblRight"></td>
                                                        <td>
                                                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click"
                                                                ValidationGroup="UploadLead" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </fieldset>
                                    </Template>
                                </ig:ContentTabItem>
                            </Tabs>
                        </ig:WebTab>
                    </td>
                </tr>
            </table>
            <%--            <uc1:Appointments runat="server" ID="wucAppointments1" />
            --%>
            <ig:WebDialogWindow ID="WebDialogWindow5" runat="server" Height="230px" Width="500px"
                InitialLocation="Centered" WindowState="Hidden" Modal="true">
                <ContentPane>
                    <Template>
                        <uc2:AddtoOutlook runat="server" SoftLinkDates="true" ID="wucOutlook1" />
                    </Template>
                </ContentPane>
                <Header CaptionText="Appointments" CloseBox-Visible="false">
                </Header>
            </ig:WebDialogWindow>
            <ig:WebDialogWindow ID="WebDialogWindow6" runat="server" Height="230px" Width="500px"
                InitialLocation="Centered" WindowState="Hidden" Modal="true">
                <ContentPane>
                    <Template>
                        <uc4:LeadNotes runat="server" ID="LeadsNotes1" />
                    </Template>
                </ContentPane>
                <Header CaptionText="Lead Notes" CloseBox-Visible="false">
                </Header>
            </ig:WebDialogWindow>
            <ig:WebDialogWindow ID="dlgAgent" runat="server" Height="500px" Width="700px" Modal="true"
                InitialLocation="Centered" WindowState="Hidden">
                <ContentPane>
                    <Template>
                        <uc2:wucAgent ID="grdAgent" runat="server" />
                    </Template>
                </ContentPane>
                <Header CaptionText="Agents">
                </Header>
            </ig:WebDialogWindow>
            <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="150px" InitialLocation="Centered" ClientIDMode="Static"
                Modal="True" Width="400px" WindowState="Hidden">
                <ContentPane EnableRelativeLayout="true">
                    <Template>
                        <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom: 25px; margin-top: 25px">
                            <table cellspacing="5" width="100%" align="center">
                                <tr>
                                    <td align="center">
                                        <asp:Label runat="server" ID="lblError1" Text="" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button runat="server" Text="Ok" ID="btnOk" OnClick="btnOk_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </Template>
                </ContentPane>
                <Header CaptionText="Alert" CloseBox-Visible="false"></Header>
            </ig:WebDialogWindow>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">

       <fieldset>
        <legend>Quick Create</legend>
        <asp:Panel runat="server" ID="pnlDetail" DefaultButton="btnAddLead" HorizontalAlign="Left" Width="150px">
            <table cellspacing="5">
                <tr>
                    <td colspan="2">
                        <uc3:wucMessage ID="WucMessage1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">DBA/Company:
                    </td>
                    <td>
                        <asp:TextBox ID="DBAName" runat="server" MaxLength="50" Width="110px" ValidationGroup="AddLead"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">First Name:
                    </td>
                    <td>
                        <asp:TextBox ID="ContactName" runat="server" Width="110px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Last Name:
                    </td>
                    <td>
                        <asp:TextBox ID="ContactLastName" runat="server" Width="110px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="lblRight">Office Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="PhoneNumber" runat="server" MaxLength="25" Width="110px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="lblRight">Email:
                    </td>
                    <td>
                        <asp:TextBox ID="Email" runat="server" MaxLength="50" Width="110px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="lblRight" style="width: 300px">Volume:</td>
                    <td>
                        <ig:WebNumericEditor ID="ProcessingVolume" runat="server" ValueText="0" Width="110px"
                            NullText="0.00" DataMode="Decimal">
                        </ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">Monthly Profit:</td>
                    <td>
                        <ig:WebNumericEditor ID="MonthlyProfit" runat="server" ValueText="0" Width="110px"
                            NullText="0.00" DataMode="Decimal">
                        </ig:WebNumericEditor>
                    </td>
                </tr>
                <tr>

                    <td class="lblRight">Lead Source:</td>
                    <td>
                        <asp:DropDownList ID="Source" runat="server" Width="115px" ValidationGroup="AddLead">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnAddLead" runat="server" Text="Add" OnClick="btnAddLead_Click"
                            ValidationGroup="AddLead"></asp:Button>&nbsp;
                                            <asp:Button ID="btnClearLead" runat="server" Text="Clear" OnClick="btnClearLead_Click"
                                                CausesValidation="false"></asp:Button>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </fieldset>

</asp:Content>
