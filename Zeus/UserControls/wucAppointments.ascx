<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucAppointments" Codebehind="wucAppointments.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebSchedule.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebSchedule" TagPrefix="igsch" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/wucAgent.ascx" TagName="wucAgent" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script id="igClientScript" type="text/javascript">
    
    function WebScheduleInfo1_ActivityDialogOpening(oScheduleInfo, oEvent, oDialog, oActivity)
    {
        if (oActivity.getDataKey() != null)
        {  
            window.location = "frmLeadsDetail.aspx?Adding=false&PostBackURL=frmAppointments.aspx&LeadID=" + oActivity.getDataKey();
        }
            
        oEvent.cancel = true;
    }

    function wmvMain_DblClick(oMonthView, oEvent, element)
    {
        oEvent.cancel = true;
    }
    
</script>

<asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
    <fieldset>
        <legend>Appointment Search</legend>
        <table width="70%">
            <tr>
                <td class="lblRight">
                    <asp:Label ID="Label1" runat="server" Width="80px" Text="DBA Name:" EnableViewState="False"
                        Style="text-align: right;"></asp:Label></td>
                <td class="lblLeft">
                    <asp:TextBox ID="DBAName" runat="server" Width="100px" EnableViewState="False"></asp:TextBox></td>
                <%--  <td class="lblRight">
                    <asp:Label ID="Label5" runat="server" Text="Agent:" EnableViewState="False" Style="text-align: right;"></asp:Label></td>
                <td class="lblLeft">
                  <asp:DropDownList ID="AgentID" runat="server" Width="250px">
                    </asp:DropDownList>
                    <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentID"
                        PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                        IsSorted="true" QueryPattern="Contains">
                    </cc1:ListSearchExtender>                    
                </td>--%>
                <td class="lblRight">
                    Partner DBA:</td>
                <td class="lblLeft">
                    <asp:TextBox ID="AgentDBA" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                </td>
                <td class="lblRight">
                    Partner ID:</td>
                <td class="lblLeft">
                    <asp:TextBox ID="AgentID" runat="server" Enabled="false" Width="100px"></asp:TextBox>
                    <asp:LinkButton OnClick="btnAgentSelect_Click" CausesValidation="false" ID="lbSelectAgent"
                        runat="server">Select</asp:LinkButton>
                </td>
                <td class="lblLeft">
                    <asp:CheckBox ID="Active" runat="server" Checked="True" Text="Active" TextAlign="Left" /></td>
                <td>
                    <asp:HiddenField ID="AgentUID" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="8" align="center">
                    <br />
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        AccessKey="h"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear" AccessKey="l">
                    </asp:Button>
                    <%--  <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                        AccessKey="h">
                        <Appearance>
                            <Image Url="~/Images/Check.png" />
                        </Appearance>
                    </igtxt:WebImageButton>
                    &nbsp;
                    <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                        AccessKey="l">
                        <Appearance>
                            <Image Url="~/Images/delete.png" />
                        </Appearance>
                    </igtxt:WebImageButton>--%>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Panel>
<fieldset>
    <legend class="toggle_heading" id="APPOINTMENTS">
        <img alt="toggle open/close" src="../Images/close.gif" />Appointments of a Month</legend>
    <table width="100%">
        <tr>
            <td class="lblEdit">
                <asp:Panel runat="server" ID="App" Visible="false">
                    &nbsp;
                    <asp:Label ID="Label3" runat="server" BackColor="Green" ForeColor="White">Active</asp:Label>&nbsp;
                    <asp:Label ID="Label6" runat="server" BackColor="Red" ForeColor="White">InActive</asp:Label>
                </asp:Panel>
            </td>
            <td class="lblRight">
                &nbsp;
                <asp:Label ID="lblRecordCount" runat="server" SkinID="RecordCount"></asp:Label>&nbsp;
            </td>
        </tr>
        <tr>
            
            <td valign="top" colspan="2">                
                <igsch:WebMonthView ID="wmvMain" runat="server" WebScheduleInfoID="WebScheduleInfo1"
                    StyleSheetFileName="/ig_common/20072CLR20/Styles/Office2007Blue/ig_webmonthview.css"
                    StyleSheetDirectory="" StyleSetPath="" StyleSetName="" Width="100%" Height="100%"
                    Style="overflow: visible;">
                    <ClientEvents MouseUp="wmvMain_MouseUp" />
                </igsch:WebMonthView>
            </td>
        </tr>
    </table>
    <igsch:WebScheduleInfo ID="WebScheduleInfo1" runat="server" OnActiveDayChanged="WebScheduleInfo1_ActiveDayChanged">
        <ClientEvents ActivityDialogOpening="WebScheduleInfo1_ActivityDialogOpening" />
    </igsch:WebScheduleInfo>
</fieldset>
<br />
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
