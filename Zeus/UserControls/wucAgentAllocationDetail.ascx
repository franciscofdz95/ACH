<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucAgentAllocationDetail.ascx.cs" Inherits="wucAgentAllocationDetail" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="wucContact.ascx" TagName="wucContact" TagPrefix="uc3" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<link href="../css/bootstrap.zeus.css" rel="stylesheet" />
<script src="../js/bootstrap.min.js"></script>

<script id="Infragistics" type="text/javascript">

    function DeleteConfirmation_Click(oButton, oEvent) {
        var x = confirm("Are you sure you want to delete this record?");
        if (!x) {
            oEvent.cancel = true;
        }
    }

    $(document).ready(function () {
        load();

    });
    //        this is a really stupid bug. basically, when you're using jquery calls mixed with microsoft ajax (ie update panel stuff), the jquery calls dont work
    //        anymore after and update panel is triggered. because the jquery does not know when the request ended. so the fix is to manually call the jquery code
    //        when the end request event is fired by the update panel.
    //        
    //        http://zeemalik.wordpress.com/2007/11/27/how-to-call-client-side-javascript-function-after-an-updatepanel-asychronous-ajax-request-is-over/

    function load() {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    }

    function EndRequestHandler() {
    }


</script>
<script type="text/javascript" language="javascript">
    function CheckNumeric() {
        var key;
        var controlid = event.target.id;
        if (controlid != undefined || controlid != null) {
            var isReadonly = $('#' + controlid).attr('readonly');
            if (isReadonly == null || isReadonly == undefined) {
                key = event.which ? event.which : event.keyCode;
                if ((key >= 48 && key <= 57) || key == 13) {
                    event.returnValue = true;
                }
                else {
                    alert("Please enter Numeric only");
                    event.returnValue = false;
                }
            }
        }
    }
    //function CheckNumeric() {
    //    var key;
    //    key = event.which ? event.which : event.keyCode;
    //    if ((key >= 48 && key <= 57) || key == 13) {
    //        event.returnValue = true;
    //    }
    //    else {
    //        alert("Please enter Numeric only");
    //        event.returnValue = false;
    //    }
    //}
</script>
<style>
    .AgentAllocationDetailinfo .item {
        width: 350px;
        height: 35px;
    }
</style>
<div class="dialog">
    <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="1300px">
        <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
        <Tabs>
            <ig:ContentTabItem Text="Agent Allocation Details" EnableDynamicUpdatePanel="False">
                <Template>
                    <asp:UpdatePanel ID="pnl" runat="server">
                        <ContentTemplate>
                            <div class="boxNoBottomBorder">
                                <div class="popupHdr">
                                    <div>
                                        Add/Edit Agent Allocation
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="pnlPrivateLabel" CssClass="ftleft" Visible="false"><span style="display: inline;">Private Label:&nbsp;</span><asp:Label ID="lblPL" runat="server"></asp:Label></asp:Panel>
                            </div>
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
                                                    <ClientSideEvents Click="btnEdit_Click" />
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                                    <Appearance>
                                                        <Image Url="~/Images/add2.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="myclick" ClientIDMode="Static" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                                    CausesValidation="False" CommandName="Save" OnClick="tbrTools_ButtonClicked"
                                                    ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="123">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue.png" />
                                                    </Appearance>
                                                    <ClientSideEvents Click="btnSave_Click" />
                                                </igtxt:WebImageButton>
                                                <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" AccessKey="s" CausesValidation="False"
                                                    CommandName="Save" OnClick="tbrTools_ButtonClicked" Visible="false" ClickOnEnterKey="false"
                                                    ClickOnSpaceKey="false" TabIndex="123">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue.png" />
                                                    </Appearance>
                                                    <ClientSideEvents Click="btnSave_Click" />
                                                </igtxt:WebImageButton>
                                                <script type="text/javascript">
                                                    // this prevents the submit button from being clicked multiple times.
                                                    <%-- $('#<%= myclick.ClientID %>').on("click", function (event) {
                                                        $(this).attr('value', 'Processing...');
                                                        $(this).unbind(event);

                                                        <%= GetSubmitPostBack() %>;
                                                    });--%>
                                                </script>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" AccessKey="a"
                                                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" OnClientClick="return ConfirmDelete();" Visible="false">
                                                    <Appearance>
                                                        <Image Url="~/Images/delete.png" />
                                                    </Appearance>
                                                    <ClientSideEvents Click="DeleteConfirmation_Click" />
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False"
                                                    TabIndex="124">
                                                    <Appearance>
                                                        <Image Url="~/Images/disk_blue_error.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>

                                            <td></td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                                                    <Appearance>
                                                        <Image Url="~/Images/refresh.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <asp:Panel ID="pnlAgentAllocationDetail" runat="server">
                                <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                                <uc4:wucMessage ID="WucMessage1" runat="server" />
                                <table border="0" width="100%">
                                    <tr>
                                        <td valign="top" width="380px">
                                            <fieldset class="AgentAllocationDetailinfo">
                                                <legend>Agent Allocation Information</legend>
                                                <div class="bucketbdy">
                                                    <table style="height: 104px; width: 100%">
                                                        <tr class="item">
                                                            <td class="lblRight">Agent ID:
                                                            </td>
                                                            <td class="tiinput">
                                                                <asp:Label runat="server" ID="AgentID" Width="175px" Visible="true"></asp:Label>
                                                                <asp:TextBox ID="AgentIDTxt" runat="server" TextMode="singleline" MaxLength="5" Width="175px" TabIndex="1" Visible="false" onKeyPress="CheckNumeric()"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">Agent DBA:
                                                            </td>
                                                            <td class="tiinput">
                                                                <asp:TextBox ID="AgentDBADisplayName" runat="server" TextMode="singleline" MaxLength="100" Width="175px" TabIndex="2"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">Rep Type:
                                                            </td>
                                                            <td class="tiinput">
                                                                <asp:TextBox ID="RepType" runat="server" TextMode="singleline" MaxLength="20" Width="175px" TabIndex="3"></asp:TextBox>
                                                            </td>
                                                            <td class="lblRight">WFB Allocations:
                                                            </td>
                                                            <td class="tiinput">
                                                                <asp:TextBox ID="Allocation" runat="server" TextMode="singleline" MaxLength="4" Width="175px" TabIndex="4" onKeyPress="CheckNumeric()"></asp:TextBox>
                                                            </td>
                                        </td>
                                    </tr>
                                    <tr class="item">
                                        <td class="lblRight">Date Created:
                                        </td>
                                        <td class="tiinput">
                                            <asp:Label runat="server" ID="DateCreated" Width="175px"></asp:Label>
                                            <%--<asp:TextBox ID="DateCreated" runat="server" Width="173px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calDateCreated" runat="server" Enabled="True" PopupButtonID="imgCallbackDate"
                                                                TargetControlID="DateCreated" Format="MM/dd/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <asp:ImageButton ID="imgDateCreated" runat="Server" AlternateText="Click to show calendar"
                                                                CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />--%>
                                        </td>
                                        <td class="lblRight">
                                        User Created:
                                                            <td class="tiinput">
                                                                <asp:Label runat="server" ID="UserCreated"></asp:Label>
                                                            </td>
                                        <td class="lblRight">Source Name:
                                        </td>
                                        <td class="tiinput">
                                            <asp:TextBox ID="SourceName" runat="server" Width="175px" TabIndex="5" MaxLength="20">
                                            </asp:TextBox>
                                        </td>
                                        <td class="lblRight">Reserve %:
                                        </td>
                                        <td class="tiinput" style="margin-left: 3px">
                                            <asp:TextBox ID="ReservePercentage" runat="server" TextMode="singleline" MaxLength="8" Width="175px" TabIndex="6"></asp:TextBox>
                                            <asp:Label runat="server" ID="lblReservePercentageInfo" Width="175px" Visible="true" Text="Expected format [xxx.xxxx]"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="item">
                                        <td class="lblRight">BBVA Allocations:
                                        </td>
                                        <td class="tiinput" style="margin-left: 3px">
                                            <asp:TextBox ID="AllocationBBVA" runat="server" TextMode="singleline" MaxLength="4" Width="175px" TabIndex="7" onKeyPress="CheckNumeric()"></asp:TextBox>
                                        </td>
                                        <td class="lblRight">Status</td>
                                        <td class="tiinput">
                                            <asp:DropDownList ID="StatusId" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="lblRight">CFG Allocation: </td>
                                        <td class="tiinput">
                                            <asp:TextBox ID="CFGAllocations" runat="server" MaxLength="4" Width="100px" EnableViewState="False" onKeyPress="CheckNumeric()" TabIndex="16">
                                            </asp:TextBox>
                                        </td>
                                    </tr>
                                    </div>
                                            </fieldset>
                                            <asp:HiddenField runat="server" ID="NoteCount" />
                                    <asp:HiddenField runat="server" ID="DocsCount" />
                                    </td>

                                    </tr>
                                </table>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnCancel" />
                            <asp:PostBackTrigger ControlID="myclick" />
                        </Triggers>
                    </asp:UpdatePanel>
                </Template>
            </ig:ContentTabItem>
        </Tabs>
    </ig:WebTab>


    <script type='text/javascript' src="../js/bootstrap.min.js"></script>
    <script type='text/javascript' src="../js/AutoComplete.js"></script>
    <script type="text/javascript">

</script>
</div>
