<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true"
    Inherits="frmLeadsDetail" ValidateRequest="false" Title="Leads Detail" CodeBehind="frmLeadsDetail.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/wucLeadCategories.ascx" TagName="wucLeadCategories"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucLeadNotes.ascx" TagName="wucLeadNotes" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucAddtoOutlook.ascx" TagName="AddtoOutlook" TagPrefix="uc1" %>
<%@ Register Src="~/UserControls/wucLeadInfo.ascx" TagName="LeadInfo" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="../UserControls/wucContact.ascx" TagName="wucContact" TagPrefix="uc4" %>
<%@ Register Src="~/UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc5" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            var AppsFeesElement = $("#<%= dvAppsFees.ClientID %>");

            AppsFees_OnChange(AppsFeesElement);
        });

        function AppsFees_OnChange(Element) {

            var selectedItem = $(Element).children("option").filter(":selected").val();
            var apexLink = $("#<%= interlinkApex.ClientID %>");
            var appLink = $("#<%= interlinkApp.ClientID %>");

            if (selectedItem == "0000-0000-0000-0000") //Create new template
            {
                apexLink.show();
                appLink.hide();
            }
            else if (selectedItem == "-1") {
                appLink.hide();
                apexLink.hide();
            }
            else {
                apexLink.hide();
                appLink.show();
            }

        }

        </script>
     <script type="text/javascript">
        function btnConvertLead_Click(oButton, oEvent) {
            if ($('#ContentPlaceHolder1_LeadInfo1_wucAgentSelector_AgentDBA').val() == "") {
                alert("AgentID is required");
                oEvent.cancel = true;
            }
            else {
                var x = confirm("Do you want to continue?");
                if (!x) {
                    oEvent.cancel = true;
                }
                else {
                    return true;
                }
            }
          }
    
        </script>

    <div id="contentpage">
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
                            <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                                CommandName="Save" OnClick="tbrTools_ButtonClicked">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                                AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/disk_blue_error.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                                AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/refresh.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnPDF" runat="server" Text="App PDF" CommandName="PDF"
                                AccessKey="p" OnClick="tbrTools_ButtonClicked" CausesValidation="False" Visible="False">
                                <Appearance>
                                    <Image Url="~/Images/document_out.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnCreateApp" runat="server" Text="Create App" CommandName="Create App"
                                AccessKey="t" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <Appearance>
                                    <Image Url="~/Images/document_add.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                        <td>
                            <igtxt:WebImageButton ID="btnConvert" runat="server" Text="Convert Lead" CommandName="Convert Lead"
                                AccessKey="v" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                                <ClientSideEvents Click="btnConvertLead_Click" />
                                <Appearance>
                                    <Image Url="~/Images/document_add.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>        
        <uc5:wucMessage runat="server" ID="wucMessage1" />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="10pt" ForeColor="Red"></asp:ValidationSummary>
        <div class="dialog" style="padding-right: 10px;">
            <asp:Panel ID="pnlDetail" runat="server" Width="100%">
                <uc1:LeadInfo runat="server" ID="LeadInfo1" />
                <div class="twocolumn">
                    <div class="leftcolumn">
                        <uc1:AddtoOutlook runat="server" ID="wucOutlook1" />
                    </div>
                    <div class="rightcolumn">
                        <uc4:wucContact ID="wucContact1" ControlContactType="Lead" runat="server" />
                        <fieldset style="height: 203px">
                            <legend>Notepad</legend>
                            <br />
                            <asp:TextBox ID="BusinessType" runat="server" Height="150px" MaxLength="255" TextMode="MultiLine" style="resize:none;"
                                Width="99%" ></asp:TextBox>
                        </fieldset>
                    </div>
                </div>
                <div class="twocolumn">
                    <%--this resets the page flow--%>
                    <div style="clear: both;">
                        <!-- -->
                    </div>
                    <uc2:wucLeadNotes runat="server" ID="wucLeadNotes1" />
                </div>
                <div class="twocolumn">
                    <%--this resets the page flow--%>
                    <div style="clear: both;">
                        <!-- -->
                    </div>
                    <uc3:wucLeadCategories ID="categories" runat="server" />
                </div>
                
                <div class="twocolumn" style="display: none;">
                    <div class="leftcolumn">
                    </div>
                    <div class="rightcolumn">
                        <fieldset style="height: 217px">
                            <legend>Emails</legend>
                            <asp:Panel ID="pnlCommunications" runat="server" Width="100%" Height="192px" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:Button ID="btnAddCommunication" runat="server" Text="Add Email" OnClick="btnAddCommunication_Click"
                                                CausesValidation="False" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Label runat="server" ID="lblData" Visible="false" Text="No Data...."></asp:Label>
                                <asp:UpdatePanel ID="pnlComm" runat="server">
                                    <ContentTemplate>
                                        <asp:GridView ID="grdCommunications" runat="server" OnPageIndexChanging="grdCommunications_OnPageIndexChanging"
                                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                            AllowPaging="true" AlternatingRowStyle-CssClass="alt" Width="99%" OnRowDataBound="grdCommunications_RowDataBound"
                                            PageSize="1" AutoGenerateColumns="False">
                                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkEmailID" runat="server" Text='<%#Eval("ID")%>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="75px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Subject" HeaderText="Subject">
                                                    <ItemStyle Width="75px" />
                                                </asp:BoundField>
                                                <asp:BoundField HeaderText="Body" DataField="Body">
                                                    <ItemStyle Width="100px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="To" HeaderText="To"></asp:BoundField>
                                                <asp:BoundField DataField="Cc" HeaderText="Cc" Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="Bcc" HeaderText="Bcc" Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="DateCreated" HeaderText="Date Created">
                                                    <ItemStyle Width="140px" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="UserCreated" HeaderText="User Created"></asp:BoundField>
                                                <asp:BoundField DataField="CommunicationID" Visible="false"></asp:BoundField>
                                                <asp:BoundField DataField="LeadsUID" Visible="false" HeaderText="LeadsUID"></asp:BoundField>
                                            </Columns>
                                        </asp:GridView>
                                        <ig:WebDialogWindow ID="WebDialogWindow3" runat="server" Height="400px" Width="600px"
                                            Modal="True" InitialLocation="Centered" WindowState="hidden">
                                            <ContentPane>
                                                <Template>
                                                    <table cellspacing="5" cellpadding="0" border="0">
                                                        <tr>
                                                            <td colspan="2"></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Subject
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtSubject" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Body
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBody" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>To
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtTo" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Cc
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtCc" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Bcc
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBcc" runat="server" Width="150px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <br />
                                                                <asp:Button ID="btnComOK" OnClick="btnComOK_Click" Style="width: 50px" CausesValidation="false"
                                                                    Text="OK" runat="server" />&nbsp;
                                                                <asp:Button ID="btnComCancel" OnClick="btnComCancel_Click" Style="width: 50px" CausesValidation="false"
                                                                    Text="Cancel" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </Template>
                                            </ContentPane>
                                        </ig:WebDialogWindow>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </fieldset>
                    </div>
                </div>
                <br />
            </asp:Panel>
        </div>
    </div>
    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="310px" Width="340px"
        Modal="true" InitialLocation="centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <div class="title">
                    Agent Fees Information
                    <hr class="line">
                </div>
                <table cellpadding="2" cellspacing="2" style="margin: 5px 0px 5px 0px">
                    <tr>
                        <td colspan="2">Select a fee template or create a new fee template, to create a new application
                            using the existing lead details.
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight" style="width: 130px">Select a Fee Template :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="dvAppsFees">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Panel runat="server" ID="pnlNoFees">
                                <asp:Button runat="server" ID="interlinkApex" OnClick="interlinkApex_Click" Style="display: none;" CausesValidation="false"
                                    Text="Create Fee Template"></asp:Button>
                                <asp:Button runat="server" ID="interlinkApp" OnClick="interlinkApp_Click" Style="display: none;" CausesValidation="false"
                                    Text="Create Application"></asp:Button>
                                <asp:Button runat="server" ID="lnkClose" OnClick="lnkClose_Click" Text="Close" CausesValidation="false"></asp:Button>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </Template>
        </ContentPane>
        <Header CaptionText="New Application">
        </Header>
    </ig:WebDialogWindow>
    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="170px" InitialLocation="Centered" ClientIDMode="Static"
    Modal="True" Width="400px" WindowState="Hidden">
    <ContentPane EnableRelativeLayout="true">
        <Template>
            <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom:20px; margin-top:20px">
                <table cellspacing="5" width="100%" align="center">
                    <tr>
                        <td align="center">
                            <asp:Label runat="server" ID="lblMessage" Text="" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" Text="Ok" ID="btnOk" OnClick="btnOk_Click" />
                       &nbsp;
                            <asp:Button runat="server" Text="Cancel" ID="btnCan" OnClick="btnCan_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </Template>
    </ContentPane>
    <Header CaptionText="Confirm" CloseBox-Visible="false"></Header>
</ig:WebDialogWindow>

</asp:Content>
