<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="UserControls_wucMeritusAlertsDetail" Codebehind="wucMeritusAlertsDetail.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<asp:Panel runat="server" ID="pnlDetail">
    <div class="MAEdit">
        <asp:UpdatePanel ID="pnl" runat="server">
            <ContentTemplate>
                <table>
                    <tr>
                        <td valign="top">
                            <div class="editblock">
                                <div class="left">
                                    AlertID:</div>
                                <div class="right">
                                    <asp:Label runat="server" ID="AlertID"></asp:Label></div>
                            </div>
                            <div class="editblock">
                                <div class="left">
                                    Alert Date:</div>
                                <div class="right">
                                    <ig:WebDatePicker ID="AlertDate" runat="server" EnableAppStyling="False" NullDateLabel="">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                    </ig:WebDatePicker>
                                </div>
                            </div>
                            <div class="editblock">
                                <div class="left">
                                    Disabled Date:</div>
                                <div class="right">
                                    <ig:WebDatePicker ID="DisabledDate" runat="server" EnableAppStyling="False" NullDateLabel="">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                            SlideOpenDuration="1" />
                                    </ig:WebDatePicker>
                                </div>
                            </div>
                        </td>
                        <td valign="top">
                            <div class="editwide">
                                <div class="left">
                                    <span class="required">*</span>Subject:</div>
                                <div class="right">
                                    <asp:TextBox runat="server" CssClass="apply" MaxLength="255" ID="Subject"></asp:TextBox></div>
                            </div>
                            <div class="editwide">
                                <div class="left">
                                    <span class="required">*</span>Portal:</div>
                                <div class="right">
                                    <asp:DropDownList ID="PortalUID" runat="server" AutoPostBack="True" Style="width: 355px"
                                        OnSelectedIndexChanged="PortalUID_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="editwide">
                                <div class="left">
                                    <span class="required">*</span>Url:</div>
                                <div class="right">
                                    <asp:TextBox runat="server" CssClass="apply" ID="URL" MaxLength="255" ReadOnly="True"></asp:TextBox></div>
                            </div>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div style="padding: 10px;">
            <span class="required">*</span>Html Content:
            <ighedit:WebHtmlEditor ID="HTMLContent" runat="server" Width="700px" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
                FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia" FontSizeList="1,2,3,4,5,6,7"
                FontStyleList="Blue Underline=color:blue;text-decoration:underline;&Red Bold=color:red;font-weight:bold;&ALL CAPS=text-transform:uppercase;&all lowercase=text-transform:lowercase;&Reset="
                SpecialCharacterList="&#937;,&#931;,&#916;,&#934;,&#915;,&#936;,&#928;,&#920;,&#926;,&#923;,&#958;,&#956;,&#951;,&#966;,&#969;,&#949;,&#952;,&#948;,&#950;,&#968;,&#946;,&#960;,&#963;,&szlig;,&thorn;,&THORN;,&#402,&#1046;,&#1064;,&#1070;,&#1071;,&#1078;,&#1092;,&#1096;,&#1102;,&#1103;,&#12362;,&#12354;,&#32117;,&AElig;,&Aring;,&Ccedil;,&ETH;,&Ntilde;,&Ouml;,&aelig;,&aring;,&atilde;,&ccedil;,&eth;,&euml;,&ntilde;,&cent;,&pound;,&curren;,&yen;,&#8470;,&#153;,&copy;,&reg;,&#151;,@,&#149;,&iexcl;,&#14;,&#8592;,&#8593;,&#8594;,&#8595;,&#8596;,&#8597;,&#8598;,&#8599;,&#8600;,&#8601;,&#18;,&brvbar;,&sect;,&uml;,&ordf;,&not;,&macr;,&para;,&deg;,&plusmn;,&laquo;,&raquo;,&middot;,&cedil;,&ordm;,&sup1;,&sup2;,&sup3;,&frac14;,&frac12;,&frac34;,&iquest;,&times;,&divide;"
                UseLineBreak="True" Height="270px" ImageDirectory="../ig_common/Images/htmleditor/">
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
            <asp:CheckBox runat="server" ID="DisplaySplashScreen" Text="Display Splash Screen" />
            &nbsp;&nbsp;
            <asp:CheckBox runat="server" ID="ExcludePrivateLabels" Text="Exclude Private Labels" />
        </div>
</asp:Panel>
