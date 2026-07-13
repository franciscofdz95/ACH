<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucTicket" CodeBehind="wucTicket.ascx.cs" %>

<%@ Register Src="~/UserControls/wucMerchants.ascx" TagName="wucMerchants" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucAgent.ascx" TagName="wucAgent" TagPrefix="uc2" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%--<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>--%>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="wucContact.ascx" TagName="wucContact" TagPrefix="uc3" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/wucMerchantTicketClone.ascx" TagName="wucMerchantTicketClone" TagPrefix="uc5" %>

<link href="../css/bootstrap.zeus.css" rel="stylesheet" />
<script src="../js/bootstrap.min.js"></script>

<style>
    .dgl-clone .igdw_HeaderButtonArea {
        display: none;
    }
</style>

<script id="Infragistics" type="text/javascript">

    function CopyConfirmation_Click(oButton, oEvent) {
        var x = confirm("Are you sure to copy the ticket info?");
        if (!x) {
            oEvent.cancel = true;
        }
    }

    //Fady Massoud 12-22-2020
    //PXP-15777 Scavenger Removal
    //$(document).ready(function () {
    //    load();

    //});

    //        this is a really stupid bug. basically, when you're using jquery calls mixed with microsoft ajax (ie update panel stuff), the jquery calls dont work
    //        anymore after and update panel is triggered. because the jquery does not know when the request ended. so the fix is to manually call the jquery code
    //        when the end request event is fired by the update panel.
    //        
    //        http://zeemalik.wordpress.com/2007/11/27/how-to-call-client-side-javascript-function-after-an-updatepanel-asychronous-ajax-request-is-over/

    //function load() {
    //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    //}

    function EndRequestHandler() {
        setupTicketResize();
    }


    $(document).ready(function () {
        var obj = document.getElementById('<%=Solution.ClientID%>');

        if (obj) {
            var oldstuff = obj.value;

            if (oldstuff != '') {
                obj.value = htmlEncode(oldstuff);
            }
        }

        var ticketsource = $("#ContentPlaceHolder1_ticket1_ddlTicketSource").val();
        if ($("#ContentPlaceHolder1_ticket1_BusinessDBAName").val() == "" && (ticketsource == "i" || ticketsource == "e")) {
            $("#ContentPlaceHolder1_ticket1_ChkMerchant").prop('disabled', true);
        }

        if ($("#ContentPlaceHolder1_ticket1_AgentDBA").val() == "" && (ticketsource == "i" || ticketsource == "e")) {
            $("#ContentPlaceHolder1_ticket1_chkAgent").prop('disabled', true);
        }

        //Fady Massoud 12-22-2020
        //PXP-15777 Scavenger Removal
        //showscavengerfields($('#ChkSender'));    

    });
    //Fady Massoud 12-22-2020
    //History & PXP-15780 Notes & PXP-15871 Doc uploader
    function tabclick(id) {
        //console.log(id);
        document.getElementById('<%=HiddenTabId.ClientID%>').value = id + "tab";
            if (id != "ticket") {
                //console.log("postback");
                document.getElementById('<%=LinkButton1.ClientID%>').click();
        }
    };

    $(function () {
        var Isnew = '<%=this.Adding%>';
            if (Isnew == "True") {
                // console.log("New Ticket");
                $("#notes").hide();
                $("#uploader").hide();
                $("#history").hide();
            }
            else {
                //To set last clicked Tab after postback       
                var selectedTab = $("#<%=HiddenTabId.ClientID%>");

                var tabId = selectedTab.val() != "" ? selectedTab.val() : "tickettab";
                // console.log("selected tab " + tabId);
                //var currentab = document.getElementById(tabId);
                $('#dvTab a[href="#' + tabId + '"]').tab('show');
            }
        });
    //Fady Massoud 12-22-2020
    //PXP-15777 Scavenger removal
    //function showscavengerfields(checkbox) {
    //    var element = $(checkbox);
    //    if (element.prop('checked') == true) {
    //        $("#EmailOutFromEmail").css("display", "inline");
    //        $("#lblFromEmailAddress").css("display", "inline");
    //        $("#lblToEmailAddress").css("display", "inline");
    //        $("#EmailOutToEmail").css("display", "inline");
    //    }

    //    else if (element.prop('checked') == false) {
    //        $("#EmailOutFromEmail").css("display", "none");
    //        $("#lblFromEmailAddress").css("display", "none");
    //        $("#lblToEmailAddress").css("display", "none");
    //        $("#EmailOutToEmail").css("display", "none");
    //    }
    //    else {
    //        $("#EmailOutFromEmail").css("display", "none");
    //        $("#lblFromEmailAddress").css("display", "none");
    //        $("#lblToEmailAddress").css("display", "none");
    //        $("#EmailOutToEmail").css("display", "none");
    //    }
    //}

    function btnSave_Click(oButton, oEvent) {

        var txt = document.getElementById('<%=Problem.ClientID%>');
            var dept = document.getElementById('<%=DepartmentID.ClientID%>');
            var status = document.getElementById('<%=StatusID.ClientID%>');
            var cat = document.getElementById('<%=CategoryID.ClientID%>');
            var parent = document.getElementById('<%=ParentID.ClientID%>');
            var obj = document.getElementById('<%=Solution.ClientID%>');
            var officeid = document.getElementById('<%=OfficeID.ClientID%>');

            var calendarBehavior1 = document.getElementById('<%=DueDate.ClientID%>');
            var calendartime = document.getElementById('<%=DueDateTime.ClientID%>');
        var time = calendartime.options[calendartime.selectedIndex].value;

        var buttonid = oButton.getElementAt(0).id;

        if (obj) {
            var oldstuff = obj.value;
            if (oldstuff != '') {
                obj.value = htmlDecode(oldstuff);
            }
        }

        var errormessage = "";

        if (txt.value == "" || dept.selectedIndex <= 0 || status.selectedIndex <= 0 || cat.selectedIndex <= 0 || parent.selectedIndex <= 0 || officeid.selectedIndex <= 0) {
            errormessage = 'Department is required.\nCategory is required.\nSub-Category is required.\nSubject is required.\nStatus is required.\nOffice is required.';
        }

        if ((calendarBehavior1.value == "" || calendarBehavior1.value == null) && status.value.toUpperCase() != "F6433994-587E-46B1-9FE6-4FD85E6A7520" && buttonid == "myclick") {
            errormessage += '\nDue Date is required.';
        }
        else if (calendarBehavior1.value != "" && calendarBehavior1.value != null && (time == -1)) {
            errormessage += '\nTime is missing for Due Date.';
        }

        if (errormessage != "") {
            alert(errormessage);
            oEvent.cancel = true;
        }

    }

    function KeyDownHandler(btn) {
        var btn1 = document.getElementById(btn);
        if (event.keyCode == 13) {
            event.returnValue = false;
            event.cancel = true;
            btn1.click();
        }
    }


    function Check(txt) {

        if (txt) {
            var obj = document.getElementById(txt);

            if (obj) {
                var oldstuff = obj.value;

                if (oldstuff == '') {
                    alert("Please enter Notes.");
                    return false;
                } else {
                    obj.value = htmlEncode(oldstuff);
                    return true;
                }
            }

        }
        return false;
    }
    function htmlEncode(value) {
        return $('<div/>').text(value).html();
    }

    function htmlDecode(value) {
        return $('<div/>').html(value).text();
    }

    function chkSolution_Click() {
        var sol = document.getElementById('<%=Solution.ClientID %>');
            var solText = sol.value;
            var cbSolution = document.getElementById('<%=chkSolution.ClientID%>');
            var notes = document.getElementById('<%=Description.ClientID %>');

        if (cbSolution.checked == true) {
            sol.value = notes.value;
        }
        else {
            sol.value = solText;
        }
    }

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


    function confirmAction(chk) {

        var checked = chk.checked;

        if (checked) {
            if (confirm("Are you sure, you want to make the document private?")) {

                $('#<%=hdnDocuments.ClientID%>').val('True');
                    __doPostBack(chk.id, '');
                    return true;
                }
                else {
                    chk.checked = !checked;
                    $('#<%=hdnDocuments.ClientID%>').val('');
                    return false;
                }
            }
            else {
                if (confirm("Are you sure, you want to remove the document as private?")) {

                    $('#<%=hdnDocuments.ClientID%>').val('False');
                    __doPostBack(chk.id, '');
                    return true;
                }
                else {
                    chk.checked = !checked;
                    $('#<%=hdnDocuments.ClientID%>').val('');
                return false;
            }
        }
    }


</script>

<div class="container dialog">
    <%-- <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="1100px">
        <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
        <Tabs>
            <ig:ContentTabItem Text="Ticket Details" EnableDynamicUpdatePanel="False">
                <Template>--%>
    <asp:HiddenField ID="HiddenTabId" runat="server" />
    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="tab_Click" Style="display: none;"></asp:LinkButton>
    <div id="dvTab" style="width: 1100px;">
        <asp:UpdatePanel runat="server" ID="pnlError">
            <ContentTemplate>
                <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
        <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#tickettab" onclick="tabclick(this.id);" id="ticket">Ticket Details</a></li>
            <li><a href="#notestab" onclick="tabclick(this.id);" id="notes">Notes</a></li>
            <li><a href="#uploadertab" onclick="tabclick(this.id);" id="uploader">Document Uploader</a></li>
            <li><a href="#historytab" onclick="tabclick(this.id);" id="history">History</a></li>
        </ul>
        <div class="tab-content">
            <div id="tickettab" class="tab-pane in active">
                <asp:UpdatePanel ID="pnl" runat="server">
                    <ContentTemplate>
                        <div class="boxNoBottomBorder">
                            <div class="popupHdr">
                                <div>
                                    Add/Edit Tickets
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
                                                $('#<%= myclick.ClientID %>').bind("click", function (event) {
                                                    $(this).attr('value', 'Processing...');
                                                    $(this).unbind(event);

                                                    <%= GetSubmitPostBack() %>;
                                                });
                                            </script>
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

                                        <td>
                                            <igtxt:WebImageButton ID="clickClose" runat="server" Text="Close Ticket" Enabled="false"
                                                AccessKey="s" CausesValidation="False" CommandName="SaveClose" OnClick="tbrTools_ButtonClicked"
                                                ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="126">
                                                <Appearance>
                                                    <Image Url="~/Images/disk_blue.png" />
                                                </Appearance>
                                                <ClientSideEvents Click="btnSave_Click" />
                                            </igtxt:WebImageButton>
                                            <igtxt:WebImageButton ID="btnSaveClose" runat="server" Text="Close Ticket" AccessKey="s"
                                                CausesValidation="False" CommandName="SaveClose" OnClick="tbrTools_ButtonClicked"
                                                ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="126" Visible="false">
                                                <Appearance>
                                                    <Image Url="~/Images/disk_blue.png" />
                                                </Appearance>
                                                <ClientSideEvents Click="btnSave_Click" />
                                            </igtxt:WebImageButton>
                                            <script type="text/javascript">
                                                // this prevents the submit button from being clicked multiple times.
                                                $('#<%= clickClose.ClientID %>').bind("click", function (event) {
                                                    $(this).attr('value', 'Processing...');
                                                    $(this).unbind(event);
                                                    <%= GetSavenClosePostBack() %>;
                                                });
                                            </script>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                                                <Appearance>
                                                    <Image Url="~/Images/refresh.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnCopy" runat="server" Text="Copy" CommandName="Copy"
                                                AccessKey="o" OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="true">
                                                <Appearance>
                                                    <Image Url="~/Images/copy.png" />
                                                </Appearance>
                                                <ClientSideEvents Click="CopyConfirmation_Click" />
                                            </igtxt:WebImageButton>
                                        </td>
                                        <td>
                                            <igtxt:WebImageButton ID="btnClone" runat="server" Text="Clone Ticket" CommandName="Clone"
                                                AccessKey="t" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                                                <Appearance>
                                                    <Image Url="~/Images/clone.png" />
                                                </Appearance>
                                            </igtxt:WebImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:Panel ID="pnlTicketDetail" runat="server">
                            <uc4:wucMessage ID="WucMessage1" runat="server" />
                            <table border="0" width="100%">
                                <tr>
                                    <td valign="top" width="380px">
                                        <fieldset class="ticketinfo">
                                            <legend>Ticket Information</legend>
                                            <div class="bucketbdy">
                                                <asp:Panel runat="server" ID="pnlID">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Ticket ID:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:Label runat="server" ID="TicketID"></asp:Label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Ticket Source:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="ddlTicketSource" runat="server" Width="175px" AutoPostBack="true"
                                                            TabIndex="100" OnSelectedIndexChanged="ddlTicketSource_SelectedIndexChanged">
                                                            <asp:ListItem Value="i">Zeus</asp:ListItem>
                                                            <asp:ListItem Value="a">Apex</asp:ListItem>
                                                            <asp:ListItem Value="m">Insight</asp:ListItem>
                                                            <asp:ListItem Value="x">Payment XP</asp:ListItem>
                                                            <asp:ListItem Value="e">Scavenger</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" ID="pnlTickettemplate" CssClass="item" Visible="false">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Ticket Template:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="ddlTicketTemplate" runat="server" Width="175px" AutoPostBack="true"
                                                                TabIndex="101" OnSelectedIndexChanged="ddlTicketTemplate_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        MLE Ticket:
                                                    </div>
                                                    <div class="tiinput" style="margin-bottom: 1px">
                                                        <asp:CheckBox ID="IsMLETicket" runat="server" OnCheckedChanged="IsMLETicket_CheckedChanged" AutoPostBack="true" />
                                                    </div>

                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Office:
                                                    </div>
                                                    <div class="tiinput" style="margin-bottom: 1px">
                                                        <%--PXP:5768: Ani: Zeus: Rename 'Cambridge' office as 'London'--%>
                                                        <asp:DropDownList runat="server" ID="OfficeID" Width="175px">
                                                            <asp:ListItem Value="">-- Select --</asp:ListItem>
                                                            <asp:ListItem Value="1">Irvine (US)</asp:ListItem>
                                                            <asp:ListItem Value="2">Montreal (CAN)</asp:ListItem>
                                                            <asp:ListItem Value="3">London (UK)</asp:ListItem>
                                                            <asp:ListItem Value="4">Gatineau (CAN)</asp:ListItem>
                                                            <asp:ListItem Value="5">Los Angeles (US)</asp:ListItem>
                                                            <asp:ListItem Value="6">Dallas (US)</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Origin:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="Origin" runat="server" Width="175px" onChange="ToggleRepeat(this)">
                                                            <asp:ListItem Value="0">Not Set</asp:ListItem>
                                                            <asp:ListItem Value="1">Inbound Call</asp:ListItem>
                                                            <asp:ListItem Value="2">Outbound Call</asp:ListItem>
                                                            <asp:ListItem Value="3">Email</asp:ListItem>
                                                            <asp:ListItem Value="4">Internal</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </div>
                                                <asp:Panel ID="pnlNonMLE" runat="server">

                                                    <div class="item">
                                                        <div class="tilabel">
                                                            <asp:HyperLink Enabled="false" runat="server" ID="hypMerch" Target="_blank" Text='DBA Name'></asp:HyperLink>:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="BusinessDBAName" runat="server" Width="130px" Enabled="false" TabIndex="102"></asp:TextBox>
                                                            <asp:HiddenField ID="MerchantAppUID" runat="server" />
                                                            <asp:LinkButton ID="btnMerSelect" runat="server" Text="Select" CausesValidation="false" TabIndex="103" OnClick="btnMerSelect_Click" Style="vertical-align: bottom;" />
                                                            <asp:LinkButton runat="server" ID="lbRemoveMerchant" CausesValidation="false" Visible="false" OnClick="lbRemoveMerchant_Click"><span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Private Label:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="PrivateLabelUID" runat="server" Width="175px" TabIndex="103" OnSelectedIndexChanged="PrivateLabelUID_SelectedIndexChanged" AutoPostBack="true">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            ZID:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ReadOnly="true" ID="ZID" ClientIDMode="Static" runat="server" Width="170px"></asp:TextBox>


                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            FMA ID:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ReadOnly="true" ID="MerchantFMAID" runat="server" Width="170px"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            MID:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ReadOnly="true" ID="MerchantID" runat="server" Width="170px" TabIndex="104"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Acq Bank:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ReadOnly="true" ID="Bank" runat="server" Width="170px" TabIndex="105"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            ACH ID:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox runat="server" ID="ACHID" ReadOnly="true" Width="170px" TabIndex="106"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            ACH Bank:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="BankID" runat="server" Enabled="false" Width="175px" TabIndex="107">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Agent:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="AgentDBA" runat="server" MaxLength="50" Width="130px" TabIndex="108"
                                                                Enabled="false"></asp:TextBox>
                                                            <%-- PXP-8889 Attachment Download Error --%>
                                                            <asp:HiddenField ID="AgentID" runat="server" />
                                                            <asp:HiddenField ID="AgentUID" runat="server" />
                                                            <asp:LinkButton ID="btnAgentSelect" runat="server" TabIndex="2" Text="Select" CausesValidation="false"
                                                                OnClick="btnAgentSelect_Click" Style="vertical-align: bottom;" />
                                                        </div>
                                                    </div>
                                                    <div class="item" id="divAgentAction" runat="server" visible="false">
                                                        <div id="Div2" class="tilabel">
                                                            Assign to Agent:
                                                        </div>
                                                        <div class="tiinput" style="margin-bottom: 1px">
                                                            <asp:CheckBox ID="chkAgentAction" runat="server" />

                                                        </div>

                                                    </div>
                                                </asp:Panel>

                                                <asp:Panel ID="pnlMLE" runat="server" Visible="false">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            MLE:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="MLEName" runat="server" Width="150px" Enabled="false" TabIndex="102"></asp:TextBox>
                                                            <asp:LinkButton ID="btnMLESelect" runat="server" CausesValidation="false" Text="Select"
                                                                TabIndex="103" Style="vertical-align: bottom;" OnClick="btnMLESelect_Click"></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Acq Bank:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="MLEAcqBankUID" runat="server" Enabled="false" Width="175px" TabIndex="105">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </asp:Panel>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Department:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="DepartmentID" runat="server" Width="175px" TabIndex="109" AutoPostBack="true"
                                                            OnSelectedIndexChanged="DepartmentID_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" ID="pnlCategory" CssClass="item" Visible="false">
                                                    <div class="tilabel">
                                                        Category:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="ParentID" runat="server" Width="175px" TabIndex="110" OnSelectedIndexChanged="ParentID_SelectedIndexChanged"
                                                            AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </asp:Panel>
                                                <asp:Panel runat="server" ID="pnlSubCategory" CssClass="item" Visible="false">
                                                    <div class="tilabel">
                                                        Sub-Category:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="CategoryID" runat="server" Width="175px" TabIndex="111"
                                                            OnSelectedIndexChanged="CategoryID_SelectedIndexChanged" AutoPostBack="true">
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </asp:Panel>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Status:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="StatusID" runat="server" Width="175px" TabIndex="112" AutoPostBack="true"
                                                            OnSelectedIndexChanged="StatusID_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Assign To:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="UserID" AutoPostBack="true" runat="server" Width="175px" TabIndex="113"
                                                            OnSelectedIndexChanged="UserID_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <asp:Panel runat="server" ID="pnlManagerApproval" CssClass="item">
                                                    <div class="tilabel" style="text-align: right">ManagerApproval:</div>
                                                    <asp:CheckBox runat="server" ID="chkManagerApproval" Enabled="false" />
                                                </asp:Panel>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Due Date:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:TextBox ID="DueDate" runat="server" Width="83px" OnTextChanged="DueDate_ValueChanged" AutoPostBack="true"></asp:TextBox>
                                                        <asp:DropDownList ID="DueDateTime" runat="server" Width="75px" AutoPostBack="true" OnSelectedIndexChanged="DueDate_ValueChanged">
                                                        </asp:DropDownList>
                                                        <span class="required">*</span>
                                                        <cc1:CalendarExtender ID="calDueDate" runat="server" Enabled="True" PopupButtonID="imgDueDate"
                                                            TargetControlID="DueDate" Format="MM/dd/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <asp:ImageButton ID="imgDueDate" runat="Server" AlternateText="Click to show calendar"
                                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                    </div>
                                                </div>
                                                <div id="pnlChange" style="display: none;" runat="server">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Change Reason:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList runat="server" ID="ChangeReasonID" AutoPostBack="true" Width="175px"
                                                                TabIndex="115" OnSelectedIndexChanged="ChangeReasonID_SelectedIndexChanged" Style="border: solid 1px red;">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="pblChangeText" style="display: none;" runat="server">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox runat="server" ID="ChangeReason" Width="170px" Style="border: solid 1px red;"
                                                                TabIndex="116"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Callback Date:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:TextBox ID="CallbackDate" runat="server" Width="173px"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="calCallbackDate" runat="server" Enabled="True" PopupButtonID="imgCallbackDate"
                                                            TargetControlID="CallbackDate" Format="MM/dd/yyyy">
                                                        </cc1:CalendarExtender>
                                                        <asp:ImageButton ID="imgCallbackDate" runat="Server" AlternateText="Click to show calendar"
                                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Client Time Zone:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="TimeZone" runat="server" Width="175px" TabIndex="117" onchange="timezonechangeHandler(this)">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div class="tilabel">
                                                        Severity:
                                                    </div>
                                                    <div class="tiinput">
                                                        <asp:DropDownList ID="Priority" runat="server" Width="175px" TabIndex="118">
                                                            <asp:ListItem Value="Low">Sev 3 - Low</asp:ListItem>
                                                            <asp:ListItem Value="Medium">Sev 2 - Medium</asp:ListItem>
                                                            <asp:ListItem Value="High">Sev 1 - High</asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="item">
                                                    <div id="Div1" class="tilabel">
                                                        Tags:
                                                    </div>
                                                    <div class="tiinput" style="margin-bottom: 1px">
                                                        <asp:TextBox ID="Tags" runat="server" TextMode="singleline" Width="175px" TabIndex="122"></asp:TextBox>
                                                    </div>

                                                </div>

                                                <asp:Panel runat="server" ID="pnlAux">
                                                    <div style="clear: both; height: 30px;">
                                                        <div style="border-top: solid 1px #d0d0bf; height: 1px; position: relative; top: 12px;">
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Date Created:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:Label ID="DateCreated" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <%-- Niranjan : PXP-6929--%>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Created By:
                                                        </div>
                                                        <div class="tiinput" style="word-break: break-all; overflow: auto;">
                                                            <asp:Label ID="CreatedBy" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            User Created:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:Label ID="UserCreated" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Origin Dept:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:Label ID="OriginDept" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <%--  <asp:Panel runat="server" ID="pnlScavenger" Visible="false" Wrap="true">
                                                    <div class="item" style="height: 100%">
                                                        <div class="tilabel">
                                                            Email From:
                                                        </div>
                                                        <div style="max-width: 170px; word-wrap: break-word; float: left;">
                                                            <asp:Label ID="ScavengerEmailFrom" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="item" style="height: 100%">
                                                        <div class="tilabel">
                                                            Email To:
                                                        </div>
                                                        <div style="max-width: 170px; word-wrap: break-word; float: left">
                                                            <asp:Label ID="ScavengerEmailTo" runat="server" Width="170px"></asp:Label>
                                                        </div>
                                                    </div>
                                                </asp:Panel>--%>
                                                    <asp:HiddenField ID="TicketSource" runat="server" />
                                                    <asp:Panel runat="server" ID="pnlAgentSubmittedBy">
                                                        <div class="tilabel">
                                                            Submitted By:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:Label ID="AgentSubmittedBy" runat="server" TabIndex="116" Width="170px"></asp:Label>
                                                        </div>
                                                    </asp:Panel>
                                                </asp:Panel>
                                            </div>
                                        </fieldset>
                                        <asp:Panel runat="server" ID="Ticketcontact" Visible="false">
                                            <fieldset class="ticketinfo">
                                                <legend>Contact Information</legend>
                                                <div class="section">
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Contact Name:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="BusinessContact" runat="server" Width="170px" TabIndex="118"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Email:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="BusinessEmailAddress" runat="server" MaxLength="50" Width="170px"
                                                                TabIndex="119"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Phone:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="BusinessPhone" runat="server" MaxLength="25" Width="170px" TabIndex="120"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Fax:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="BusinessFax" runat="server" MaxLength="25" Width="170px" TabIndex="121"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="OtherContact">
                                            <div class="section">
                                                <uc3:wucContact ID="wucContact1" runat="server" />
                                            </div>
                                        </asp:Panel>
                                        <%-- <asp:Panel ID="pnlHistory" runat="server">
                                                <fieldset id="fldSH">
                                                    <legend>Status History</legend>
                                                    <asp:BulletedList ID="statusHistory" runat="server">
                                                    </asp:BulletedList>
                                                </fieldset>
                                            </asp:Panel>--%>
                                        <asp:HiddenField runat="server" ID="NoteCount" />
                                        <asp:HiddenField runat="server" ID="DocsCount" />
                                    </td>
                                    <td valign="top">
                                        <div id="fldTN">
                                            <fieldset>
                                                <legend>Issue<span class="required">*</span>
                                                </legend>
                                                <div id="problembox" class="section">
                                                    <asp:TextBox ID="Problem" runat="server" TextMode="MultiLine" Width="100%" Height="100px" TabIndex="123"></asp:TextBox>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <ig:WebDialogWindow ID="dlgClone" runat="server" Height="550px" Width="700px" Modal="true" CssClass="dgl-clone"
                                InitialLocation="Centered" WindowState="Hidden">
                                <ContentPane>
                                    <Template>
                                        <uc5:wucMerchantTicketClone ID="grdCloneMerchants" runat="server" />
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Merchants">
                                </Header>
                            </ig:WebDialogWindow>
                            <ig:WebDialogWindow ID="dlgcontrol" runat="server" Height="550px" Width="700px" Modal="false"
                                InitialLocation="Centered" WindowState="Hidden">
                                <ContentPane>
                                    <Template>
                                        <uc1:wucMerchants ID="grdMerchants" runat="server" />
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Merchants">
                                </Header>
                            </ig:WebDialogWindow>
                            <ig:WebDialogWindow ID="dlgAgent" runat="server" Height="500px" Width="700px" Modal="false"
                                InitialLocation="Centered" WindowState="Hidden">
                                <ContentPane>
                                    <Template>
                                        <uc2:wucAgent ID="grdAgent" runat="server" />
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Agents">
                                </Header>
                            </ig:WebDialogWindow>
                            <ig:WebDialogWindow ID="dlgConfirm" runat="server" Height="180px" InitialLocation="Centered" ClientIDMode="Static"
                                Modal="True" Width="420px" WindowState="Hidden">
                                <ContentPane>
                                    <Template>
                                        <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom: 10px; margin-top: 10px">
                                            <table cellspacing="3" width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" ID="lblErr" CssClass="gen_error"></asp:Label>
                                                        <asp:Label runat="server" ID="lblSuccess" CssClass="gen_Succ"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-left: 5px; padding-right: 5px;">
                                                        <asp:Label runat="server" ID="lblMessage" Text="Press OK to copy the selected items or Press Cancel to close." Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                                        <br />
                                                        <asp:CheckBox runat="server" ID="chkNotes" Text="Notes" /><br />
                                                        <asp:CheckBox runat="server" ID="chkDocuments" Text="Documents" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button runat="server" Text="OK" ID="btnOk" OnClick="btnOk_Click" CommandArgument="Yes" />
                                                        &nbsp;
                                                <asp:Button runat="server" Text="Cancel" ID="btnCan" OnClick="btnOk_Click" CommandArgument="No" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </Template>
                                </ContentPane>
                                <Header CaptionText="Merchants">
                                </Header>
                            </ig:WebDialogWindow>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnCancel" />
                        <%--<asp:PostBackTrigger ControlID="btnSubmit" />--%>
                        <asp:PostBackTrigger ControlID="myclick" />
                        <asp:PostBackTrigger ControlID="clickClose" />
                    </Triggers>
                </asp:UpdatePanel>
                <%-- Code added by koshlendra for PXP-7622 Start --%>
                <%--<asp:Panel ID="pnlFileAttachOnTicketcreation" runat="server" Visible="false">
                    <fieldset>
                        <legend>File Attachments</legend>


                        <br />
                        <p style="color: black; font-size: 8pt;">
                            <b>Note: </b>Max file size cannot exceed 40MB.
                        </p>
                        <table width="100%">
                            <tr>
                                <td class="lblRight">Document Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlDocumentType" ClientIDMode="Static" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">Choose File:
                                </td>
                                <td>
                                    <asp:FileUpload ID="fupDocument" runat="server" Width="500px" AllowMultiple="true" />
                                    &nbsp;<asp:Button ID="btnUpload" runat="server" Width="90px" CssClass="Button" Text="Add"
                                        OnClick="btnUpload_Click" CausesValidation="false" UseSubmitBehavior="false" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:ListBox ID="lstCustomAttachments" runat="server" Height="75px" Width="100%"></asp:ListBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="lblRight">Comments:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbFilesDescription" ClientIDMode="Static" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                </asp:Panel>--%>
                <%--Code added by koshlendra for PXP-7622 End--%>
            </div>

            <div id="notestab" class="tab-pane">
                <asp:UpdatePanel runat="server" ID="pnlNotes">
                    <ContentTemplate>
                        <asp:Panel ID="pnlTicketNotes" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                            <fieldset>
                                <legend>Ticket Notes</legend>
                                <div class="tnbox">
                                    <asp:Panel ID="Panel1" runat="server" CssClass="section solutionbox" Style="display: none;">
                                        Solution:<br />
                                        <div>
                                            <asp:Label ID="Solution" TextMode="MultiLine" TabIndex="124" runat="server"></asp:Label>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="pnlNoteEntry" CssClass="section">
                                        <div style="padding-bottom: 2px">
                                            <asp:CheckBox ID="chkAgent" Text="Email Agent" Style="vertical-align: text-top;"
                                                runat="server" ToolTip="If checked, this note will be visible to Partner/Merchant through emails and portals." />
                                            <asp:CheckBox ID="ChkMerchant" Text="Email Merchant" Style="vertical-align: text-top;"
                                                runat="server" ToolTip="If checked, this note will be visible to Partner/Merchant through emails and portals." />
                                            <asp:CheckBox runat="server" ID="cbEmailFT" Checked="false" TabIndex="126" Text="Email PS Mgr"
                                                onclick="chkMer_Click()" />
                                            <%--<asp:CheckBox ID="ChkSender" ClientIDMode="Static" Visible="false" Text="Email Sender" Style="vertical-align: text-top;"
                                                            runat="server" ToolTip="If checked, this note will be visible to the email IDs provided." AutoPostBack="true" OnCheckedChanged="ChkSender_CheckedChanged" />--%>
                                        </div>
                                        <%--  <table id="tblEmailSender" runat="server" visible="false">
                                                        <tr>
                                                            <td class="lblLeft">
                                                                <asp:Label ID="lblFromEmailAddress" runat="server" Text="Email From" ClientIDMode="Static" Width="60px"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="EmailOutFromEmail" runat="server" ClientIDMode="Static" Style="margin-bottom: 0px" Width="625px" EnableViewState="true"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblLeft">
                                                                <asp:Label ID="lblToEmailAddress" runat="server" Text="Email To" ClientIDMode="Static" Width="60px"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="EmailOutToEmail" runat="server" ClientIDMode="Static" Width="625px"></asp:TextBox></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="lblLeft">
                                                                <asp:Label ID="lblCCEmailAddress" runat="server" Text="Email CC" ClientIDMode="Static" Width="60px"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="EmailOutCCEmail" runat="server" ClientIDMode="Static" Width="625px"></asp:TextBox></td>
                                                        </tr>
                                                    </table>--%>
                                        <div id="mynotediv" class="ui-widget-content" style="height: 250px; overflow: hidden; border-top: none; border-left: none; border-right: none; border-bottom: solid 1px #ababab; padding-top: 25px; word-break: break-all">

                                            <ighedit:WebHtmlEditor EnableViewState="true"
                                                ID="Description"
                                                runat="server"
                                                FontFormattingList="Heading 1=&lt;h1&gt;&amp;Heading 2=&lt;h2&gt;&amp;Heading 3=&lt;h3&gt;&amp;Heading 4=&lt;h4&gt;&amp;Heading 5=&lt;h5&gt;&amp;Normal=&lt;p&gt;"
                                                FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia"
                                                FontSizeList="1,2,3,4,5,6,7"
                                                FontStyleList="Blue Underline=color:blue;text-decoration:underline;&amp;Red Bold=color:red;font-weight:bold;&amp;ALL CAPS=text-transform:uppercase;&amp;all lowercase=text-transform:lowercase;&amp;Reset="
                                                Height="100%"
                                                ImageDirectory="../ig_common/Images/htmleditor/"
                                                SpecialCharacterList="Ω,Σ,Δ,Φ,Γ,Ψ,Π,Θ,Ξ,Λ,ξ,μ,η,φ,ω,ε,θ,δ,ζ,ψ,β,π,σ,ß,þ,Þ,&amp;#402,Ж,Ш,Ю,Я,ж,ф,ш,ю,я,お,あ,絵,Æ,Å,Ç,Ð,Ñ,Ö,æ,å,ã,ç,ð,ë,ñ,¢,£,¤,¥,№,,©,®,,@,,¡,,←,↑,→,↓,↔,↕,↖,↗,↘,↙,,¦,§,¨,ª,¬,¯,¶,°,±,«,»,·,¸,º,¹,²,³,¼,½,¾,¿,×,÷"
                                                JavaScriptDirectory="../ig_htmleditor/"
                                                UseLineBreak="True"
                                                Width="100%">
                                                <Toolbar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                    <ighedit:ToolbarImage runat="server" Type="DoubleSeparator" />
                                                    <ighedit:ToolbarButton runat="server" Type="Bold" />
                                                    <ighedit:ToolbarButton runat="server" Type="Italic" />
                                                    <ighedit:ToolbarButton runat="server" Type="Underline" />
                                                    <ighedit:ToolbarButton runat="server" Type="Strikethrough" />
                                                    <ighedit:ToolbarImage runat="server" Type="Separator" />
                                                    <ighedit:ToolbarButton runat="server" Type="Subscript" />
                                                    <ighedit:ToolbarButton runat="server" Type="Superscript" />
                                                    <ighedit:ToolbarImage runat="server" Type="Separator" />
                                                    <ighedit:ToolbarButton runat="server" Type="Undo" />
                                                    <ighedit:ToolbarButton runat="server" Type="Redo" />
                                                    <ighedit:ToolbarImage runat="server" Type="Separator" />
                                                    <ighedit:ToolbarButton runat="server" Type="JustifyLeft" />
                                                    <ighedit:ToolbarButton runat="server" Type="JustifyCenter" />
                                                    <ighedit:ToolbarButton runat="server" Type="JustifyRight" />
                                                    <ighedit:ToolbarButton runat="server" Type="JustifyFull" />
                                                    <ighedit:ToolbarImage runat="server" Type="Separator" />
                                                    <ighedit:ToolbarButton runat="server" Type="UnorderedList" />
                                                    <ighedit:ToolbarButton runat="server" Type="OrderedList" />
                                                    <ighedit:ToolbarImage runat="server" Type="Separator" />
                                                    <ighedit:ToolbarDialogButton runat="server" Type="FontColor"></ighedit:ToolbarDialogButton>
                                                </Toolbar>
                                                <DropDownStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <ProgressBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <DownlevelTextArea Columns="0" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Rows="0" Wrap="True" />
                                                <RightClickMenu Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Cut"></ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Copy"></ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Paste"></ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="PasteHtml"></ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties">
                                                        <Dialog InternalDialogType="CellProperties" />
                                                    </ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties">
                                                        <Dialog InternalDialogType="ModifyTable" />
                                                    </ighedit:HtmlBoxMenuItem>
                                                    <ighedit:HtmlBoxMenuItem runat="server" Act="InsertImage"></ighedit:HtmlBoxMenuItem>
                                                </RightClickMenu>
                                                <TextWindow Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100%" />
                                                <DownlevelLabel Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                                <TabStrip BorderColor="DimGray" DesignImageName="" DesignText=" " Enabled="false" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HighlightBackColor="Menu" HighlightBorderColor="Menu" HighlightBorderStyle="None" HighlightForeColor="Menu" HtmlImageName="" HtmlText=" " />
                                            </ighedit:WebHtmlEditor>

                                        </div>
                                        <div>
                                            <asp:CheckBox ID="chkSolution" runat="server" Checked="false" TabIndex="126" Text="Set as Solution" OnCheckedChanged="chkSolution_CheckedChanged" AutoPostBack="true" />
                                        </div>
                                        <asp:Button ID="btnAddNotes" runat="server" CausesValidation="False" OnClick="btnAddNotes_Click" TabIndex="127" Text="Add Notes" Width="90px" />
                                        <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClick="btnClearNotes_Click" TabIndex="128" Text="Clear Notes" Width="90px" />
                                        <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExpExcel_Click">
                                            <span style="height: 25px; vertical-align: middle;">
                                                <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Export</span>
                                        </asp:LinkButton>
                                    </asp:Panel>
                                    <asp:CheckBox runat="server" ID="IsFTMerchant" Style="display: none;" />
                                    <asp:GridView ID="grdTicketNotes" runat="server" AutoGenerateColumns="False" GridLines="None"
                                        Width="100%" ShowHeader="False" DataKeyNames="TicketNotesUID" Font-Names="Verdana"
                                        Font-Size="X-Small" CssClass="ticketgrid" PagerStyle-CssClass="pgr" PageSize="5"
                                        AlternatingRowStyle-CssClass="alt" AllowPaging="true" OnPageIndexChanging="grdTicketNotes_PageIndexChanging"
                                        OnRowDataBound="grdTicketNotes_RowDataBound">
                                        <PagerSettings FirstPageText="&#171;" LastPageText="&#187;" />
                                        <Columns>
                                            <asp:BoundField HeaderText="UID" DataField="TicketNotesUID" Visible="False"></asp:BoundField>
                                            <asp:TemplateField HeaderText="MyTemplate">
                                                <ItemTemplate>
                                                    <asp:Panel runat="server" ID="pnlBlock">
                                                        <div class="ti_title">
                                                            <div>
                                                                <asp:Label runat="server" CssClass="datedisplay" ID="labDateCreated"></asp:Label>
                                                                <asp:Literal runat="server" ID="litUserCreated"></asp:Literal>
                                                                says...
                                                            </div>
                                                        </div>
                                                        <asp:Label runat="server" ID="lblFeedBackRequired" CssClass="RequiresFeedBack">Requires Attention</asp:Label>
                                                        <div class="ti_body" style="word-break: break-all; word-wrap: break-word;">
                                                            <asp:Literal runat="server" ID="litBody"></asp:Literal>
                                                        </div>
                                                        <div style="text-align: right;">
                                                            <asp:CheckBox ID="chkIsSolution" CssClass="cbsolution" Text="Set As Solution" ToolTip="Check this box to set note as solution"
                                                                runat="server" AutoPostBack="true" OnCheckedChanged="chkIsSolution_CheckedChanged" />
                                                            <asp:CheckBox ID="chkReqAtt" CssClass="cbsolution" Text="Email Partner" ToolTip="Check this box to send an email to the partner"
                                                                runat="server" AutoPostBack="true" Visible="False" />
                                                            <asp:CheckBox ID="ChkAccessToPartner" CssClass="cbsolution" Text="Email Agent" ToolTip="If checked, this note will be visible to Partner/Merchant through emails and portals."
                                                                runat="server" AutoPostBack="true" Visible="True" Enabled="False" />
                                                            <asp:CheckBox ID="ChkAccessToMerchant" CssClass="cbsolution" Text="Email Merchant" ToolTip="If checked, this note will be visible to Partner/Merchant through emails and portals."
                                                                runat="server" AutoPostBack="true" Visible="True" Enabled="False" />
                                                            <asp:CheckBox ID="ChkAccessToSender" CssClass="cbsolution" Text="Email Sender" ToolTip="If checked, this note will be visible to Sender of Scavenger emails."
                                                                runat="server" AutoPostBack="true" Visible="False" Enabled="False" />

                                                        </div>
                                                        <asp:HiddenField runat="server" ID="hidTicketUID" Value='<%# Eval("TicketNotesUID") %>' />
                                                    </asp:Panel>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>
                                </div>
                            </fieldset>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div id="uploadertab" class="tab-pane">

                <asp:Panel ID="pnlDocumentUploader" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                    <fieldset>
                        <legend>File Attachments</legend>
                        <br />
                        <p style="color: black; font-size: 8pt;">
                            <b>Note: </b>Max file size cannot exceed 40MB.
                        </p>
                        <table width="100%">
                            <tr>
                                <td width="95px" class="lblRight">Choose File:
                                </td>
                                <td>
                                    <div id="mydropzoneid" class="dropzone mydz"></div>
                                    <%-- PXP-8889 Attachment Download Error --%>
                                    <script type="text/javascript">
                                        Dropzone.autoDiscover = false;

                                        var myDropzone = new Dropzone('div#mydropzoneid', {
                                            init: function () {
                                                this.on("processing", function (file) {
                                                    this.options.url = '<%= String.Format("{0}ajax/ControlServer.aspx?control=uploadfile&MDocSourceID={1}&Username={2}&DefaultDocTypeID={3}&PrimaryKeyID={4}&PrimaryKeyUID={5}&AgentId={6}", WebUtil.GetBaseUrl(), 3, UserSessions.CurrentUser.UserName, 10046, UserSessions.CurrentTicket == null?"0":UserSessions.CurrentTicket.TicketID, UserSessions.CurrentTicket == null? "0":UserSessions.CurrentTicket.TicketUID,AgentID.Value==""?"0":AgentID.Value) %>' + "&DocTypeID=" + $("#lstDocumentTypes").val() + "&Description=" + $("#tbFileDescription").val() + "&MerchantAppID=" + $("#ZID").val() + "&IsPrivate=" + $('#IsPrivateFile').is(':checked');
                                                }),
                                                    //Fady Massoud 12-25-2020
                                                    //Bug PXP-15835 Dropzone get disable after success upload
                                                    this.on("complete", function (file) {
                                                        this.removeFile(file);
                                                    }),
                                                    this.on("queuecomplete", function (file) {
                                                        document.getElementById('<%=lbRefresh.ClientID%>').click();
                                                            //this.removeAllFiles(true);                                                                                                                  
                                                        });
                                            },
                                            autoProcessQueue: false,
                                            maxFiles: 99, // keep this high, otherwise it will discard the file from the queue.
                                            parallelUploads: 99,
                                            clickable: true,
                                            acceptedFiles: '<%= ConfigurationManager.AppSettings["AcceptableUploadMimeTypes"] %>',
                                            error: function (file, errorMessage) {
                                                alert(errorMessage);
                                                this.removeFile(file);
                                            },
                                            paramName: "newlyuploadedfile",
                                            url: '<%= String.Format("{0}ajax/ControlServer.aspx?control=uploadfile&MDocSourceID={1}&Username={2}&DefaultDocTypeID={3}&PrimaryKeyID={4}&PrimaryKeyUID={5}&AgentId={6}", WebUtil.GetBaseUrl(), 3, UserSessions.CurrentUser.UserName, 10046, UserSessions.CurrentTicket == null?"0":UserSessions.CurrentTicket.TicketID, UserSessions.CurrentTicket == null? "0":UserSessions.CurrentTicket.TicketUID,AgentID.Value==""?"0":AgentID.Value) %>'
                                            //complete: function (file) {                                                       
                                            //    if ($('#contentpage').length) {
                                            //        __doPostBack('ctl00$ContentPlaceHolder1$ticket1$lbRefresh', '');                                                                         
                                            //    } else {
                                            //       __doPostBack('ticket1$lbRefresh', '');
                                            //    }                                                       
                                            //},
                                            //success: function (file) {
                                            //    this.removeFile(file);                                                       
                                            //}
                                        });
                                    </script>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="lblRight">Comments:
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="tbFileDescription" ClientIDMode="Static" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">Document Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="lstDocumentTypes" ClientIDMode="Static" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr runat="server" ID="IsPrivateFileRow" ClientIDMode="Static" Visible="false" >
                                <td class="lblRight">Private:
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="IsPrivateFile" ClientIDMode="Static" Enabled="false" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                                <td>
                                    <%--<asp:Button runat="server" Text="Upload" ID="btnSubmit" Style="display: none;" OnClick="btnSubmit_Click" />--%>

                                    <button type="button" id="multibegin">Begin Upload</button>


                                    <script type="text/javascript">

                                        $('#multibegin').on('click', function () {
                                            myDropzone.processQueue();
                                        });


                                    </script>
                                </td>
                            </tr>
                        </table>
                        <asp:HiddenField runat="server" ID="hdnDocuments" />
                        <asp:LinkButton runat="server" ID="lbRefresh" OnClick="lbRefresh_Click">Refresh Documents</asp:LinkButton>
                        <asp:UpdatePanel ID="Updategrid" UpdateMode="Conditional" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lbRefresh" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:GridView runat="server" ID="gvFile" CssClass="mGrid" OnRowDataBound="gvFile_RowDataBound"
                                    OnPreRender="gvFile_PreRender" DataKeyNames="DocID" AutoGenerateColumns="False" ShowHeaderWhenEmpty="true">
                                    <EmptyDataTemplate>No Data...</EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Private">
                                            <ItemTemplate>
                                                <asp:CheckBox runat="server" ID="IsPrivate" onclick="return confirmAction(this);" OnCheckedChanged="IsPrivate_CheckedChanged" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Date Uploaded" DataField="DocDate" />
                                        <asp:TemplateField HeaderText="File">
                                            <ItemTemplate>
                                                <asp:HiddenField runat="server" ID="HidDocID" Value='<%# Bind("DocID") %>' />
                                                <asp:HiddenField runat="server" ID="HidPKID" Value='<%# Bind("PrimaryKeyID") %>' />
                                                <asp:HiddenField runat="server" ID="HidOrigName" Value='<%# Bind("OrigName") %>' />
                                                <asp:HyperLink ID="litHyp" runat="server" Target="_blank" Text='<%# Eval("OrigName") %>'></asp:HyperLink>
                                                <asp:Label ID="lblComment" runat="server" Style="display: block;"></asp:Label>
                                                <asp:Label ID="lblOrigName" ToolTip="Private Document" runat="server" Text='<%# Eval("OrigName") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:Label runat="server" ID="Type" Text='<%# Eval("DocTypeName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="User Uploaded" DataField="UserCreated" />
                                        <asp:BoundField DataField="ContentSize" HeaderText="Size" ReadOnly="True" />
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </fieldset>
                </asp:Panel>
            </div>

            <div id="historytab" class="tab-pane">
                <%--</Template>
            </ig:ContentTabItem>
            <ig:ContentTabItem runat="server" EnableDynamicUpdatePanel="False" Text="History">
                <Template>--%>
                <asp:UpdatePanel ID="upChangelog" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="Panel2" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                            <fieldset>
                                <legend>Change History</legend>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <b>Select Field:</b><asp:DropDownList ID="ddlChangeType" runat="server" AutoPostBack="true" OnPreRender="ddlChangeType_PreRender"
                                                OnSelectedIndexChanged="ddlChangeType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView CssClass="mGrid" ID="grdChange" runat="server" AutoGenerateColumns="False" ShowHeaderWhenEmpty="false" OnRowDataBound="grdChange_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Field Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblNameHeader" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                        </HeaderTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" />
                                                    <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    No changes for selected field.
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <%--</Template>
            </ig:ContentTabItem>
        </Tabs>
    </ig:WebTab>--%>
    </div>
    <script>
        var setupTicketResize = function () {
            $("#mynotediv").resizable({
                maxHeight: 9999,
                maxWidth: 9999,
                minHeight: 100,
                minWidth: 400
            });
        };

        setupTicketResize();
    </script>
</div>
