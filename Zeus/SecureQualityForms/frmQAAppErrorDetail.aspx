<%@ Page Language="C#" Title="Application Error Detail" AutoEventWireup="true" ValidateRequest="false"
    Inherits="frmQAAppErrorDetail" MasterPageFile="~/MasterPageQuality.master" CodeBehind="frmQAAppErrorDetail.aspx.cs" %>

<%@ MasterType VirtualPath="~/MasterPageQuality.master" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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

    </script>

    <div id="contentpage" width="100%">
        <table width="100%">
            <tr>
                <td>
                    <div class="dialog">
                        <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="100%">
                            <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
                            <Tabs>
                                <ig:ContentTabItem Text="Application Error Details" EnableDynamicUpdatePanel="False">
                                    <Template>
                                        <asp:UpdatePanel ID="pnl" runat="server">
                                            <ContentTemplate>
                                                <%--  <div class="boxNoBottomBorder">
                                <div class="popupHdr">
                                    <<%--div>
                                        Add/Edit Agent Allocation
                                    </div>--%>
                                </div>
                               <%-- <asp:Panel runat="server" ID="pnlPrivateLabel" CssClass="ftleft" Visible="false">
                                    <span style="display: inline;">Private Label:&nbsp;</span><asp:Label ID="lblPL" runat="server"></asp:Label></asp:Panel>
                            </div>--%>
                                                <div class="tbrtools">
                                                    <div class="tbrtoolsleft">
                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                                                                        AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="9">
                                                                        <Appearance>
                                                                            <Image Url="~/Images/edit.png" />
                                                                        </Appearance>
                                                                        <ClientSideEvents Click="btnEdit_Click" />
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                                                                        OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="10">
                                                                        <Appearance>
                                                                            <Image Url="~/Images/add2.png" />
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="myclick" ClientIDMode="Static" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                                                        CausesValidation="False" CommandName="Save" OnClick="tbrTools_ButtonClicked"
                                                                        ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="11">
                                                                        <Appearance>
                                                                            <Image Url="~/Images/disk_blue.png" />
                                                                        </Appearance>
                                                                        <ClientSideEvents Click="btnSave_Click" />
                                                                    </igtxt:WebImageButton>

                                                                </td>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="btnDelete" runat="server" Text="Delete" CommandName="Delete" AccessKey="a"
                                                                        OnClick="tbrTools_ButtonClicked" CausesValidation="False" OnClientClick="return ConfirmDelete();" TabIndex="12">
                                                                        <Appearance>
                                                                            <Image Url="~/Images/delete.png" />
                                                                        </Appearance>
                                                                        <ClientSideEvents Click="DeleteConfirmation_Click" />
                                                                    </igtxt:WebImageButton>
                                                                </td>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                                                        AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False"
                                                                        TabIndex="13">
                                                                        <Appearance>
                                                                            <Image Url="~/Images/disk_blue_error.png" />
                                                                        </Appearance>
                                                                    </igtxt:WebImageButton>
                                                                </td>

                                                                <td></td>
                                                                <td>
                                                                    <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                                                        AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="14">
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
                                                <asp:Panel ID="pnlQAAppErrorDetail" runat="server" Width="100%">
                                                    <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                                                    <asp:Label ID="lblQAAppErrorID" runat="server" Visible="false"></asp:Label>
                                                    <div>
                                                        <fieldset class="QaAppErrorDetailinfo">
                                                            <legend>Application Errors Add/Edit</legend>
                                                            <table width="100%">
                                                                <tr class="item">
                                                                    <td class="lblRight">Date Created:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                        <asp:Label runat="server" ID="DateCreated" Width="225px" DataFormatString="{0:MM/dd/yyyy HH:mm tt}"></asp:Label>

                                                                    </td>
                                                                    <td class="lblRight">MID:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                        <asp:Label runat="server" ID="MID" Width="100px" Visible="true"></asp:Label>
                                                                        <asp:TextBox ID="MIDTxt" runat="server" TextMode="singleline" MaxLength="20" Width="200px" TabIndex="1" Visible="false" onKeyPress="CheckNumeric()"></asp:TextBox>
                                                                    </td>

                                                                    <td class="lblRight">Department:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                         <asp:DropDownList ID="Dept" runat="server" Width="125px" TabIndex="2"></asp:DropDownList>
                                                                    </td>

                                                                    <td class="lblRight">Rep:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                        <asp:TextBox ID="Rep" runat="server" TextMode="singleline" MaxLength="50" Width="125px" TabIndex="3"></asp:TextBox>
                                                                    </td>
                                                                </tr>


                                                                <tr class="item">
                                                                    <td class="lblRight">Date Error Found:</td>
                                                                    <td class="tiinput">
                                                                        <asp:TextBox ID="DateErrorFound" runat="server" MaxLength="10" Width="125px" EnableViewState="False"
                                                                            TabIndex="4" DataFormatString="{0:MM/dd/yyyy}"></asp:TextBox>
                                                                        <cc1:CalendarExtender ID="calDateErrorFound" runat="server" Enabled="True" PopupButtonID="imgDateErrorFound"
                                                                            TargetControlID="DateErrorFound" Format="MM/dd/yyyy">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:ImageButton ID="imgDateErrorFound" runat="Server" AlternateText="Click to show calendar"
                                                                            CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" TabIndex="5" />
                                                                    </td>

                                                                    <td class="lblRight">Error Found By:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                        <asp:DropDownList ID="ErrorFoundBy" runat="server" Width="125px" TabIndex="6"></asp:DropDownList>
                                                                    </td>
                                                                    <td class="lblRight">Category:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                         <asp:DropDownList ID="Category" runat="server" Width="125px" TabIndex="7"></asp:DropDownList>
                                                                    </td>

                                                                    <td class="lblRight">Sub-Category:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                        <asp:DropDownList ID="SubCategory" runat="server" Width="131px" TabIndex="8"></asp:DropDownList>
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td class="lblRight">Error Occurred Stage:
                                                                    </td>
                                                                    <td class="tiinput">
                                                                       <asp:DropDownList ID="ErrorOccuredStage" runat="server" Width="131px" TabIndex="9"></asp:DropDownList>

                                                                    </td>

                                                                    <td class="lblRight">Description:
                                                                    </td>
                                                                    <td class="tiinput" colspan="3">
                                                                        <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" MaxLength="500" Width="380px" Height="32px" TabIndex="10"></asp:TextBox>


                                                                    </td>
                                                                </tr>
                                                            </table>
                                                    </div>

                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnCancel" runat="server" />
                                                <asp:PostBackTrigger ControlID="myclick" runat="server" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </Template>

                                </ig:ContentTabItem>
                            </Tabs>
                        </ig:WebTab>
                    </div>
                </td>
            </tr>
        </table>
        <script type="text/javascript" language="javascript">
            function pageLoad() {
                $('#ContentPlaceHolder1_TabControl_tmpl0_imgDateErrorFound').on("click", function () {

                    var offset = $('#ContentPlaceHolder1_TabControl_tmpl0_DateErrorFound').offset();
                    if (offset != undefined || offset != null) {
                        var topOffset = offset.top;
                        var calenderOffsetTop = topOffset + 20;
                        $('#ContentPlaceHolder1_TabControl_tmpl0_calDateErrorFound_popupDiv').css({ "position": "fixed", "top": calenderOffsetTop + "px", "left": "310px" });

                    }
                });
            }
        </script>
    </div>

</asp:Content>
