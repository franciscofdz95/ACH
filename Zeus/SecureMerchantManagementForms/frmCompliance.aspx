<%@ Page Title="Compliance" Language="C#" MasterPageFile="~/MasterPageMerchant.master"
    AutoEventWireup="true" Inherits="SecureMerchantManagementForms_frmCompliance"
    CodeBehind="frmCompliance.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%--<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>--%>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc1" %>
<%@ Register Src="../UserControls/wucWSComplianceGrid.ascx" TagName="wucWSComplianceGrid"
    TagPrefix="uc2" %>
<%@ Register Src="../UserControls/wucWSComplianceEdit.ascx" TagName="wucWSComplianceEdit"
    TagPrefix="uc3" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc4" %>
<%@ Register Src="../UserControls/wucWSWebsiteReview.ascx" TagName="wucWSWebsiteReview"
    TagPrefix="uc5" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">    
    <asp:Panel ID="pnlGreenBanner" runat="server">
    <span class="ftrightGreen">Tilled Account</span>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlBanner">
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
    <uc4:wucMessage ID="wucMessage2" runat="server" />
    <asp:Panel ID="pnlTools" runat="server">
    </asp:Panel>
    <uc1:wucBusinessInfo ID="wucBusinessInfo1" runat="server" />
    <div style="padding: 10px; display: none;">
        <ig:WebTab ID="WebTab1" runat="server" Width="900px"  SelectedIndex="1">
            <Tabs>
                <ig:ContentTabItem runat="server"  Text="Website Review">
                    <Template>
                        <div style="padding: 10px;">
                            <div class="title">
                                Current Review
                                <hr class="line">
                            </div>
                            <uc2:wucWSComplianceGrid ID="wucWSComplianceGrid1" runat="server" />
                            <asp:Panel runat="server" ID="pnlButton" Visible="false">
                                <asp:Button runat="server" ID="btnCreate" Text="Initiate Website Review" OnClick="btnCreate_Click" />
                            </asp:Panel>
                            <div style="height: 20px;">
                                <!-- -->
                            </div>
                            <div class="title">
                                Review History
                                <hr class="line">
                            </div>
                            <uc2:wucWSComplianceGrid ID="wucWSComplianceGrid2" runat="server" />
                        </div>
                    </Template>
                </ig:ContentTabItem>
                <ig:ContentTabItem runat="server" Text="Checklist">
                    <Template>
                        <uc3:wucWSComplianceEdit ID="wucWSComplianceEdit1" runat="server" />
                    </Template>
                </ig:ContentTabItem>
            </Tabs>
        </ig:WebTab>
    </div>
    <uc5:wucWSWebsiteReview ID="wucWSWebsiteReview1" runat="server" />
    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="160px" Width="400px"
        Modal="True" InitialLocation="Centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <p style="text-align: center">
                    The status of this website review will be: <b>
                        <asp:Literal runat="server" ID="litResult"></asp:Literal></b>
                </p>
                <p style="text-align: center">
                    Are you sure you want to submit?
                </p>
                <div style="text-align: center;">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnComplianceCancel_Click" />&nbsp;<asp:Button
                        ID="btnSubmit" runat="server" Text="Yes, Submit" OnClick="btnComplianceSubmit_Click" />
                </div>
            </Template>
        </ContentPane>
        <Header CaptionText="Website Compliance, Are you sure?">
        </Header>
    </ig:WebDialogWindow>
    <ig:WebDialogWindow ID="WebDialogWindow3" runat="server" Height="590px" Width="800px" OnStateChanged="WebDialogWindow3_StateChanged"
        Modal="True" InitialLocation="Centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <table class="emailtable">
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            To:
                        </td>
                        <td class="myright">
                            <asp:TextBox runat="server" Width="300px" ID="tbTo"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            From:
                        </td>
                        <td class="myright">
                            <asp:TextBox runat="server" Width="300px" ID="tbFrom"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            CC:
                        </td>
                        <td class="myright">
                            <asp:TextBox runat="server" Width="300px" ID="tbCC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            BCC:
                        </td>
                        <td class="myright">
                            <asp:TextBox runat="server" Width="300px" ID="tbBCC"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            Subject:
                        </td>
                        <td class="myright">
                            <asp:TextBox runat="server" Width="600px" ID="tbSubject"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            Attachments:
                        </td>
                        <td class="myright">
                            <asp:PlaceHolder ID="phA" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft" style="vertical-align:top;font:bold;">
                            Body:
                        </td>
                        <td class="myright">
                            <ighedit:WebHtmlEditor ID="tbBody" runat="server" Width="600" Height="300" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
                                FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia" FontSizeList="1,2,3,4,5,6,7"
                                FontStyleList="Blue Underline=color:blue;text-decoration:underline;&Red Bold=color:red;font-weight:bold;&ALL CAPS=text-transform:uppercase;&all lowercase=text-transform:lowercase;&Reset="
                                SpecialCharacterList="&#937;,&#931;,&#916;,&#934;,&#915;,&#936;,&#928;,&#920;,&#926;,&#923;,&#958;,&#956;,&#951;,&#966;,&#969;,&#949;,&#952;,&#948;,&#950;,&#968;,&#946;,&#960;,&#963;,&szlig;,&thorn;,&THORN;,&#402,&#1046;,&#1064;,&#1070;,&#1071;,&#1078;,&#1092;,&#1096;,&#1102;,&#1103;,&#12362;,&#12354;,&#32117;,&AElig;,&Aring;,&Ccedil;,&ETH;,&Ntilde;,&Ouml;,&aelig;,&aring;,&atilde;,&ccedil;,&eth;,&euml;,&ntilde;,&cent;,&pound;,&curren;,&yen;,&#8470;,&#153;,&copy;,&reg;,&#151;,@,&#149;,&iexcl;,&#14;,&#8592;,&#8593;,&#8594;,&#8595;,&#8596;,&#8597;,&#8598;,&#8599;,&#8600;,&#8601;,&#18;,&brvbar;,&sect;,&uml;,&ordf;,&not;,&macr;,&para;,&deg;,&plusmn;,&laquo;,&raquo;,&middot;,&cedil;,&ordm;,&sup1;,&sup2;,&sup3;,&frac14;,&frac12;,&frac34;,&iquest;,&times;,&divide;"
                                UseLineBreak="True" >
                                <Toolbar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False">
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="DoubleSeparator" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Bold" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Italic" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Underline" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Strikethrough" />
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Separator" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Subscript" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Superscript" />
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Separator" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Undo" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Redo" />
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Separator" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="JustifyLeft" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="JustifyCenter" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="JustifyRight" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="JustifyFull" />
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Separator" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="UnorderedList" />
                                    <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="OrderedList" />
                                    <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="Separator" />
                                    <ighedit:ToolbarDialogButton runat="server" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Type="FontColor">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:ToolbarDialogButton>
                                    <ighedit:ToolbarDialogButton runat="server" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Type="FontHighlight">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:ToolbarDialogButton>
                                    <ighedit:ToolbarDialogButton runat="server" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Type="SpecialCharacter">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" InternalDialogType="SpecialCharacterPicker" Type="InternalWindow" />
                                    </ighedit:ToolbarDialogButton>
                                    <ighedit:ToolbarMenuButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Type="InsertTable">
                                        <Menu Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False">
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" InternalDialogType="InsertTable" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="InsertColumnRight" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="InsertColumnLeft" Font-Bold="False"
                                                Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="InsertRowAbove" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="InsertRowBelow" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="DeleteRow" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="DeleteColumn" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="IncreaseColspan" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="DecreaseColspan" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="IncreaseRowspan" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="DecreaseRowspan" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" InternalDialogType="CellProperties" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties" Font-Bold="False" Font-Italic="False"
                                                Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                                <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                    Font-Underline="False" InternalDialogType="ModifyTable" />
                                            </ighedit:HtmlBoxMenuItem>
                                        </Menu>
                                    </ighedit:ToolbarMenuButton>
                                    <ighedit:ToolbarDialogButton runat="server" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Type="FindReplace">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" InternalDialogType="FindReplace" />
                                    </ighedit:ToolbarDialogButton>
                                </Toolbar>
                                <DropDownStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <ProgressBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <DownlevelTextArea Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Columns="0" Rows="0" Wrap="True" />
                                <RightClickMenu Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False">
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Cut" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Copy" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="Paste" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="PasteHtml" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" InternalDialogType="CellProperties" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" InternalDialogType="ModifyTable" />
                                    </ighedit:HtmlBoxMenuItem>
                                    <ighedit:HtmlBoxMenuItem runat="server" Act="InsertImage" Font-Bold="False" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                        <Dialog Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" />
                                    </ighedit:HtmlBoxMenuItem>
                                </RightClickMenu>
                                <TextWindow Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Width="100%" />
                                <DownlevelLabel Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                                <TabStrip Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" />
                            </ighedit:WebHtmlEditor>
                        </td>
                    </tr>
                    <tr>
                        <td class="myleft">
                        </td>
                        <td class="myright">
                            <asp:Button runat="server" ID="btnSubmitEmail" OnClick="Button1_Click" Text="Send Email">
                            </asp:Button>
                        </td>
                    </tr>
                </table>

                <asp:HiddenField runat="server" ID="hidPDFLocation" />
                <asp:HiddenField runat="server" ID="hidPDFCleanName" />
            </Template>
        </ContentPane>
        <Header CaptionText="Send Email To Merchant">
        </Header>
    </ig:WebDialogWindow>
</asp:Content>
