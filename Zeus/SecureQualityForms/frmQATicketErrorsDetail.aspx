<%@ Page Title="Ticket Error Details" Language="C#" MasterPageFile="~/MasterPageQuality.Master" AutoEventWireup="true" CodeBehind="frmQATicketErrorsDetail.aspx.cs" Inherits="frmQATicketErrorsDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ MasterType VirtualPath="~/MasterPageQuality.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type='text/javascript' src="../js/bootstrap.min.js"></script>
    <script type='text/javascript' src="../js/AutoComplete.js"></script>
    <script type="text/javascript" language="javascript">

        function DeleteConfirmation_Click(oButton, oEvent) {
            var x = confirm("Are you sure you want to delete this record?");
            if (!x) {
                oEvent.cancel = true;
            }
        }

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
    </script>
    <div id="contentpage">
        <div class="dialog">
            <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="100%" Height="190px">
                <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
                <Tabs>
                    <ig:ContentTabItem Text="Ticket Error Details" EnableDynamicUpdatePanel="False">
                        <Template>
                            <asp:UpdatePanel ID="updatepnlTicketErrorsDetail" runat="server">
                                <ContentTemplate>
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
                                                        <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" AccessKey="s" CausesValidation="False"
                                                            CommandName="Save" OnClick="tbrTools_ButtonClicked" ClickOnEnterKey="false" Enabled="false"
                                                            ClickOnSpaceKey="false" TabIndex="123">
                                                            <Appearance>
                                                                <Image Url="~/Images/disk_blue.png" />
                                                            </Appearance>
                                                            <ClientSideEvents Click="btnSave_Click" />
                                                        </igtxt:WebImageButton>
                                                    </td>
                                                    <td>
                                                        <igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" AccessKey="a"
                                                            OnClick="tbrTools_ButtonClicked" CausesValidation="False" OnClientClick="return ConfirmDelete();">
                                                            <Appearance>
                                                                <Image Url="~/Images/delete.png" />
                                                            </Appearance>
                                                            <ClientSideEvents Click="DeleteConfirmation_Click" />
                                                        </igtxt:WebImageButton>
                                                    </td>
                                                    <td>
                                                        <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="true"
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

                                    <asp:Panel ID="pnlTicketErrorsDetail" runat="server">
                                        <fieldset style="height: auto;">
                                            <legend>Ticket Errors Add/Edit</legend>
                                            <table border="0" width="100%">
                                                <tr class="item">
                                                    <td class="lblRight">Created Date:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:Label runat="server" ID="DateCreated" Width="150px" Font-Size="8.5pt" Font-Names="Tahoma"></asp:Label></td>
                                                    <td class="lblRight">ZID:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:Label runat="server" ID="ZID" Width="100px" Font-Size="8.5pt" Font-Names="Tahoma"></asp:Label>
                                                        <asp:TextBox ID="txtZID" runat="server" MaxLength="9" Width="100px" TabIndex="1" Visible="false" onKeyPress="CheckNumeric()"></asp:TextBox>
                                                    </td>
                                                    <td class="lblRight">Ticket ID:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:TextBox ID="TicketID" runat="server" MaxLength="9" Width="100px" TabIndex="2" onKeyPress="CheckNumeric()"></asp:TextBox>
                                                    </td>
                                                    <td class="lblRight">Rep:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:TextBox ID="Rep" runat="server" MaxLength="50" Width="175px" TabIndex="3"></asp:TextBox>
                                                    </td>

                                                </tr>
                                                <tr class="item">
                                                    <td class="lblRight">Date Error Found:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:TextBox ID="DateErrorFound" runat="server" Width="100px" EnableViewState="False" MaxLength="10"
                                                            TabIndex="4"></asp:TextBox>

                                                        <cc1:CalendarExtender ID="calsearchDateErrorFound" runat="server" PopupButtonID="imgSearchDateErrorFound"
                                                            TargetControlID="DateErrorFound" Format="MM/dd/yyyy" Enabled="false">
                                                        </cc1:CalendarExtender>
                                                        <asp:ImageButton ID="imgSearchDateErrorFound" runat="Server" AlternateText="Click to show calendar"
                                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" Enabled="false" TabIndex="5" />
                                                    </td>

                                                    <td class="lblRight">Category:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:DropDownList ID="Category" runat="server" Width="175px" TabIndex="6"></asp:DropDownList>
                                                    </td>
                                                    <td class="lblRight">Sub-Category:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:DropDownList ID="SubCategory" runat="server" Width="175px" TabIndex="7"></asp:DropDownList>
                                                    </td>
                                                    <td class="lblRight">Description:
                                                    </td>
                                                    <td class="tiinput">
                                                        <asp:TextBox ID="Description" runat="server" Width="175px" TextMode="MultiLine"
                                                            TabIndex="8" MaxLength="500"></asp:TextBox>
                                                    </td>

                                                </tr>
                                            </table>
                                        </fieldset>
                                    </asp:Panel>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnCancel" />
                                    <asp:PostBackTrigger ControlID="btnSave" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </Template>
                    </ig:ContentTabItem>
                </Tabs>
            </ig:WebTab>
        </div>
    </div>
    <script type="text/javascript" language="javascript">
        function pageLoad() {
            $('#ContentPlaceHolder1_TabControl_tmpl0_imgSearchDateErrorFound').on("click", function () {
                var offset = $('#ContentPlaceHolder1_TabControl_tmpl0_DateErrorFound').offset();
                if (offset != undefined || offset != null) {
                    var topOffset = offset.top;
                    var calenderOffsetTop = topOffset + 20;
                    $('#ContentPlaceHolder1_TabControl_tmpl0_calsearchDateErrorFound_popupDiv').css({ "position": "fixed", "top": calenderOffsetTop + "px", "left": "290px" });

                }
            });
        }
    </script>
</asp:Content>
