<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucCompliance.ascx.cs" Inherits="wucCompliance" %>

<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="wucContact.ascx" TagName="wucContact" TagPrefix="uc3" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc4" %>
<script id="Infragistics" type="text/javascript">
    
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
<%--START: Replace CRM to TPP,Added Vendor UID and Name ComboBox and Replaced Type Textbox to Dropdown and Change Design  for PXP-8417 by Ali Khan --%>
<style>
    .ticketinfo .item {
        width: 350px;
        height: 35px;
    }
</style>
<div class="dialog">
    <ig:WebTab runat="server" ID="TabControl" Enabled="true" Width="1100px">
        <PostBackOptions EnableAjax="true" EnableAsyncUpdateAllTabs="true" EnableLoadOnDemand="false" />
        <Tabs>
            <ig:ContentTabItem Text="TPP Details" EnableDynamicUpdatePanel="False">
                <Template>
                    <asp:UpdatePanel ID="pnl" runat="server">
                        <ContentTemplate>
                            <div class="boxNoBottomBorder">
                                <div class="popupHdr">
                                    <div>
                                        Add/Edit TPP
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
                                                    $('#<%= myclick.ClientID %>').on("click", function (event) {
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
                            <asp:Panel ID="pnlCRMDetail" runat="server">
                                <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                                <uc4:wucMessage ID="WucMessage1" runat="server" />
                                <table border="0" width="100%">
                                    <tr>
                                        <td valign="top" width="380px">
                                            <fieldset class="ticketinfo">
                                                <legend>TPP Information</legend>
                                                <div class="bucketbdy">
                                                    <asp:Panel runat="server" ID="pnlID">
                                                        <div class="item">
                                                            <div class="tilabel">
                                                                TPP ID:
                                                            </div>
                                                            <div class="tiinput">
                                                                <asp:Label runat="server" ID="CRMID"></asp:Label>
                                                            </div>
                                                        </div>
                                                        <div class="item">
                                                            <div class="tilabel">
                                                                Vendor UID:
                                                            </div>
                                                            <div class="tiinput" style="width: 250px">
                                                                <asp:Label runat="server" ID="CRMUID"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="item">
                                                        <div id="Div4" class="tilabel">
                                                            Name:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="CRMName" runat="server" TextMode="singleline" Width="175px" TabIndex="122"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="item">
                                                        <div id="Div3" class="tilabel">
                                                            Type:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="CRMType" runat="server" Width="175px">
                                                                <asp:ListItem Selected="True" Text="CRM" Value="CRM" />
                                                                <asp:ListItem Text="Gateway" Value="Gateway" />
                                                            </asp:DropDownList>
                                                        </div>

                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            TPP Certified flag:
                                                        </div>
                                                        <%--<div class="tiinput" style="margin-bottom: 1px">
                                                            <asp:CheckBox ID="TPPCertifiedflag" runat="server"  AutoPostBack="true" />
                                                        </div>--%>
                                                        <div class="tiinput">
                                                            <asp:DropDownList ID="StatusUID" runat="server" Width="175px" AutoPostBack="true" OnSelectedIndexChanged="StatusUID_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <%--<span class="required">*</span>--%>
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Comments:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox runat="server" ID="Description" Rows="3" Width="172px" TextMode="MultiLine"></asp:TextBox>
                                                        </div>

                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Accept Transactions:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:CheckBox ID="AcceptTransactions" runat="server" AutoPostBack="true" />
                                                        </div>

                                                    </div>

                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Status Date:
                                                        </div>
                                                        <div class="tiinput">
                                                            <asp:TextBox ID="CertifiedDate" runat="server" Width="173px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calCallbackDate" runat="server" Enabled="True" PopupButtonID="imgCallbackDate"
                                                                TargetControlID="CertifiedDate" Format="MM/dd/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <asp:ImageButton ID="imgCallbackDate" runat="Server" AlternateText="Click to show calendar"
                                                                CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                        </div>
                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                        </div>
                                                        <div class="tiinput">
                                                        </div>

                                                    </div>
                                                    <div class="item">
                                                        <div class="tilabel">
                                                            PCI Validation Date:
                                                        </div>
                                                        <div class="tiinput" style="margin-left:3px">
                                                            <asp:TextBox ID="PCIValidationDate" runat="server" Width="173px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calPCIValidationDate" runat="server" Enabled="True" PopupButtonID="imgPCIValidationDate"
                                                                TargetControlID="PCIValidationDate" Format="MM/dd/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <asp:ImageButton ID="imgPCIValidationDate" runat="Server" AlternateText="Click to show calendar"
                                                                CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                        </div>
                                                    </div>

                                                    <div class="item">
                                                        <div class="tilabel">
                                                            Last Scanned Date:
                                                        </div>
                                                        <div class="tiinput" style="margin-left:3px">
                                                            <asp:TextBox ID="LastScannedDate" runat="server" Width="173px"></asp:TextBox>
                                                            <cc1:CalendarExtender ID="calLastScannedDate" runat="server" Enabled="True" PopupButtonID="imgLastScannedDate"
                                                                TargetControlID="LastScannedDate" Format="MM/dd/yyyy">
                                                            </cc1:CalendarExtender>
                                                            <asp:ImageButton ID="imgLastScannedDate" runat="Server" AlternateText="Click to show calendar"
                                                                CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </fieldset>                                          
                                            <asp:HiddenField runat="server" ID="NoteCount" />
                                            <asp:HiddenField runat="server" ID="DocsCount" />
                                        </td>

                                    </tr>
                                </table>
                            </asp:Panel>                              
                             <%--Code added for PXP-8410 by koshlendra start--%>
                            <asp:Panel ID="pnlCRMChangeHistory" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                         <fieldset>
                        <legend>Change History</legend>
                          <table width="100%">
                                <tr>
                                    <td class="lblLeft">Page Size:
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
                        <asp:GridView CssClass="mGrid" ID="grdChangeHistory"  OnPageIndexChanging="grdChangeHistory_PageIndexChanging" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" AllowPaging="true" PageSize="10">
                            <Columns>
                                <asp:TemplateField HeaderText="Field">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ChangeHistoryField") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate> 
                                         <asp:DropDownList ID="ddlChangeType" runat="server" AutoPostBack="true" OnPreRender="ddlChangeType_PreRender" OnRowDataBound="grdChangeHistory_RowDataBound"
                                            OnSelectedIndexChanged="ddlChangeType_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Bind("OldValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOldNameHeader" runat="server" Text="Old Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNewNameHeader" runat="server" Text="New Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" />
                                <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                            </Columns>
                            <EmptyDataTemplate>
                                No changes for selected field.
                            </EmptyDataTemplate>
                        </asp:GridView>                            
                           
                    </fieldset>
                </asp:Panel> 
                            <%--Code added for PXP-8410 by koshlendra End--%>                                           
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

        if ($('#<%=CRMName.ClientID%>').attr("readonly") != "readonly") {
            var ParamArray = [['Type', '<%=CRMType.ClientID%>']];
            fnAutoComplete.init('#<%=CRMName.ClientID%>', '../ajax/AjaxWebservice.asmx/GetTPPName', 'TPPNameFragment', ParamArray);
        }

    </script>
</div>
<%--END: Replace CRM to TPP,Added Vendor UID and Name ComboBox and Replaced Type Textbox to Dropdown and Change Design  for PXP-8417 by Ali Khan --%>
