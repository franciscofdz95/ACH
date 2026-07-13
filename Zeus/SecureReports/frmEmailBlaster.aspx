<%@ Page Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true" Inherits="frmEmailBlaster" Title="Email Blaster" Codebehind="frmEmailBlaster.aspx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucEmailBlaster.ascx" TagName="wucEmailBlaster"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        function CopyConfirmation_Click(oButton, oEvent)
        {
           var x = confirm("Are you sure to copy the email template?");
           if(!x)
            oEvent.cancel = true;
        }
    </script>

    <div id="contentpage">
        <asp:Panel runat="server" ID="pnlToolbar" Visible="false" CssClass="tbrtools">
            <div class="tbrtoolsleft">
               &nbsp;<igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                    AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/edit.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add"
                    AccessKey="a" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/add2.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                    AccessKey="s" CommandName="Save" OnClick="tbrTools_ButtonClicked">
                    <Appearance>
                        <Image Url="~/Images/disk_blue.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                    AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/disk_blue_error.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                    AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/refresh.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;
                <igtxt:WebImageButton ID="btnCopy" runat="server" Text="Copy" CommandName="Copy"
                    AccessKey="o" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/copy.png" />
                    </Appearance>
                    <ClientSideEvents Click="CopyConfirmation_Click" />
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnSend" runat="server" Text="Send" CommandName="SendEmail"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" ToolTip="Send Email">
                    <Appearance>
                        <Image Url="../Images/arrow_right.png" />
                    </Appearance>
                    <ClientSideEvents Click="validateEmail" />
                </igtxt:WebImageButton>
                &nbsp; &nbsp;&nbsp;
                <asp:HyperLink runat="server" Visible="false" Style="line-height: 32px; height: 32px;"
                    NavigateUrl="~/SecureReports/frmEmailBlaster.aspx" ID="hypBack">Back To Search</asp:HyperLink>
            </div>
        </asp:Panel>
        <uc2:wucMessage ID="WucMessage1" runat="server"></uc2:wucMessage>
        <asp:Panel runat="server" ID="pnlEdit">
            <div class="title">
                &nbsp;&nbsp;
                <asp:Label runat="server" ID="lblEmailBlaster" Text="Edit Email Blaster"></asp:Label>
                <hr class="line" />
            </div>
            <uc1:wucEmailBlaster ID="wucEmailBlaster1" runat="server" />
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSearch">
            <div class="title">
                &nbsp;&nbsp;Search Email Blaster
                <hr class="line" />
            </div>
            <table cellspacing="1" width="100%">
                <tr>
                    <td class="lblRight">
                        <asp:Label runat="server" ID="Label1" Text="Email Blaster ID:" Width="90px" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="EmailTemplateID" Width="150px"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        <asp:Label runat="server" ID="Label2" Text="Subject:" Width="50px" />
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="Subject" Width="150px"></asp:TextBox>
                    </td>
                    <td class="lblRight">
                        Type:
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="rblEmail" Width="155px">
                            <asp:ListItem Text="All" Value="-1" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Merchants" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Agents" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        User Created:</td>
                    <td>
                        <asp:DropDownList ID="UserUID" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Portals:
                    </td>
                    <td>
                        <asp:DropDownList ID="PortalUID" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">
                        Bank:
                    </td>
                    <td>
                        <asp:DropDownList ID="MerchantTypeUID" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                    <%--                    <td class="lblRight">
                        Agents:
                    </td>
                    <td>
                        <asp:DropDownList ID="AgentUID" runat="server" Width="155px">
                        </asp:DropDownList><cc1:ListSearchExtender ID="ListSearchExtender1" runat="server"
                            TargetControlID="AgentUID" PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt"
                            PromptPosition="Top" IsSorted="true" QueryPattern="Contains">
                        </cc1:ListSearchExtender>
                    </td>--%>
                    <td class="lblRight">
                        Agent Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="AgentTypeUID" runat="server" Width="155px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel runat="server" ID="AgentSelect">
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="horizontal"
                                IDWidth="110" DBAWidth="150" lblDBAWidth="98" lblIDWidth="68" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <div style="clear: both;">
                <!-- -->
            </div>
            <div style="text-align: center; padding-top: 5px;">
                <br />
                <igtxt:WebImageButton ID="WebImageButton1" runat="server" OnClick="btnSearch_Click"
                    Text="Search" AccessKey="h">
                    <Appearance>
                        <Image Url="~/Images/Check.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;
                <igtxt:WebImageButton ID="WebImageButton2" runat="server" OnClick="btnClear_Click"
                    Text="Clear" CausesValidation="false" AccessKey="l">
                    <Appearance>
                        <Image Url="~/Images/delete.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;
                <igtxt:WebImageButton ID="btnNew" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/add2.png" />
                    </Appearance>
                </igtxt:WebImageButton>
            </div>
            <br />
            <div class="title">
                &nbsp;&nbsp;Email Templates
                <hr class="line" />
            </div>
            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">
                            Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="True">10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>250</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grdEmail" runat="server" OnRowCommand="grdEmail_RowCommand" DataKeyNames="EmailBlasterID"
                                AutoGenerateColumns="false" Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid"
                                OnRowDataBound="grdEmail_RowDataBound" DataSourceID="odsEmails" AllowPaging="true"
                                OnPageIndexChanging="grdEmail_PageIndexChanging" AllowSorting="True" OnSorting="grdEmail_Sorting">
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <FooterStyle CssClass="footer" />
                                <SelectedRowStyle BackColor="#fffacd" />
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnView" runat="server" CausesValidation="false" CommandName="View"
                                                Text="View"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Subject" HeaderText="Subject"></asp:BoundField>
                                    <asp:TemplateField HeaderText="HTMLContent">
                                        <ItemTemplate>
                                            <asp:Label ID="lblHtmlContent" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:BoundField DataField="CommunicationID" Visible="false"></asp:BoundField>--%>
                                    <asp:BoundField DataField="DateCreated" DataFormatString="{0:MM/dd/yy hh:mm tt}"
                                        HeaderText="Date Created"></asp:BoundField>
                                    <asp:BoundField DataField="UserCreated" HeaderText="User Created"></asp:BoundField>
                                    <asp:TemplateField>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:Image runat="server" ID="imgAtt" AlternateText="" ToolTip="Attachment" ImageUrl="~/Images/icon_attachment.png" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsEmails" runat="server" SelectMethod="GetEmailBlasterPaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetEmailBlasterPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsEmails_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                        </td>
                                        <td align="right">
                                            Export:&nbsp;</td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td align="right" style="width: 33%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label runat="server" ID="lblEmail" Text="No Emails..." Visible="false"></asp:Label>
            <br />
        </asp:Panel>
    </div>
</asp:Content>
