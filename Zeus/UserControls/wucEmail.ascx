<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucEmail" Codebehind="wucEmail.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<asp:Panel ID="pnlDetail" runat="server" Width="100%">
    <asp:Label ID="lblMessage" runat="server" Font-Size="10pt" ForeColor="Green"></asp:Label>
    <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
    <div class="tabcontent">
        <fieldset>
            <legend>Email Information</legend>
            <table cellspacing="2">
                <tr>
                    <td class="lblRight">
                    </td>
                    <td class="lblRight">
                        <asp:RadioButtonList ID="lstType" runat="server" RepeatColumns="2" AutoPostBack="True"
                            Visible="False">
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Fax</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:HiddenField ID="hfdMerchant" runat="server" />
                        <asp:HiddenField ID="hfdAgent" runat="server" />
                        <asp:HiddenField ID="CommunicationID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        To:</td>
                    <td>
                        <asp:TextBox ID="To" runat="server" Width="400px"></asp:TextBox>&nbsp;
                       <%-- <asp:CheckBox ID="chkMerchant" runat="server" Text="To Merchant" />
                        <asp:CheckBox ID="chkAgent" runat="server" Text="To Partner" />--%>
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Cc:</td>
                    <td>
                        <asp:TextBox ID="Cc" runat="server" Width="400px"></asp:TextBox>&nbsp;&nbsp;</td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Bcc:</td>
                    <td>
                        <asp:TextBox ID="Bcc" runat="server" Width="400px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td class="lblRight">
                        Subject:</td>
                    <td>
                        <asp:TextBox ID="Subject" runat="server" Width="400px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Body</legend>
            <ighedit:WebHtmlEditor ID="txtHTMLBody" JavaScriptDirectory="../ig_htmleditor/" runat="server" Width="100%" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
                FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia" FontSizeList="1,2,3,4,5,6,7"
                FontStyleList="Blue Underline=color:blue;text-decoration:underline;&Red Bold=color:red;font-weight:bold;&ALL CAPS=text-transform:uppercase;&all lowercase=text-transform:lowercase;&Reset="
                SpecialCharacterList="Ω,Σ,Δ,Φ,Γ,Ψ,Π,Θ,Ξ,Λ,ξ,μ,η,φ,ω,ε,θ,δ,ζ,ψ,β,π,σ,ß,þ,Þ,&amp;#402,Ж,Ш,Ю,Я,ж,ф,ш,ю,я,お,あ,絵,Æ,Å,Ç,Ð,Ñ,Ö,æ,å,ã,ç,ð,ë,ñ,¢,£,¤,¥,№,,©,®,,@,,¡,,←,↑,→,↓,↔,↕,↖,↗,↘,↙,,¦,§,¨,ª,¬,¯,¶,°,±,«,»,·,¸,º,¹,²,³,¼,½,¾,¿,×,÷"
                UseLineBreak="True" Height="300px" ImageDirectory="../ig_common/Images/htmleditor/">
                <Toolbar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False">
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
                    <ighedit:ToolbarDialogButton runat="server" Type="FontColor">
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
                    <ighedit:HtmlBoxMenuItem runat="server" Act="Cut">
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="Copy">
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="Paste">
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="PasteHtml">
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties">
                        <Dialog InternalDialogType="CellProperties" />
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties">
                        <Dialog InternalDialogType="ModifyTable" />
                    </ighedit:HtmlBoxMenuItem>
                    <ighedit:HtmlBoxMenuItem runat="server" Act="InsertImage">
                    </ighedit:HtmlBoxMenuItem>
                </RightClickMenu>
                <TextWindow Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" Width="100%" />
                <DownlevelLabel Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
                <TabStrip Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                    Font-Underline="False" />
            </ighedit:WebHtmlEditor>
            <%--<ighedit:WebHtmlEditor ID="txtHTMLBody" runat="server" Width="100%" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
                FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia" FontSizeList="1,2,3,4,5,6,7"
                FontStyleList="Blue Underline=color:blue;text-decoration:underline;&Red Bold=color:red;font-weight:bold;&ALL CAPS=text-transform:uppercase;&all lowercase=text-transform:lowercase;&Reset="
                SpecialCharacterList="&#937;,&#931;,&#916;,&#934;,&#915;,&#936;,&#928;,&#920;,&#926;,&#923;,&#958;,&#956;,&#951;,&#966;,&#969;,&#949;,&#952;,&#948;,&#950;,&#968;,&#946;,&#960;,&#963;,&szlig;,&thorn;,&THORN;,&#402,&#1046;,&#1064;,&#1070;,&#1071;,&#1078;,&#1092;,&#1096;,&#1102;,&#1103;,&#12362;,&#12354;,&#32117;,&AElig;,&Aring;,&Ccedil;,&ETH;,&Ntilde;,&Ouml;,&aelig;,&aring;,&atilde;,&ccedil;,&eth;,&euml;,&ntilde;,&cent;,&pound;,&curren;,&yen;,&#8470;,&#153;,&copy;,&reg;,&#151;,@,&#149;,&iexcl;,&#14;,&#8592;,&#8593;,&#8594;,&#8595;,&#8596;,&#8597;,&#8598;,&#8599;,&#8600;,&#8601;,&#18;,&brvbar;,&sect;,&uml;,&ordf;,&not;,&macr;,&para;,&deg;,&plusmn;,&laquo;,&raquo;,&middot;,&cedil;,&ordm;,&sup1;,&sup2;,&sup3;,&frac14;,&frac12;,&frac34;,&iquest;,&times;,&divide;"
                UseLineBreak="True" Height="250px">
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
            </ighedit:WebHtmlEditor>--%>
        </fieldset>
        <%--  <fieldset>
            <legend>Attachments</legend>
            <asp:UpdatePanel ID="pnlAttach" runat="server">
                <ContentTemplate>
            <%--  <table width="550" cellspacing="2">
                <tr>
                    <td class="lblRight">
                        Attachments:</td>
                    <td class="lblRight">
                        <asp:FileUpload ID="fuplAttachments" runat="server" Height="20px" Width="100%" />
                        <asp:HiddenField ID="lblMsg" runat="server" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Height="20px" OnClick="btnUpload_Click"
                            Text="Add" Width="70px" CausesValidation="False" OnClientClick="javascript:showWait();" />
                    </td>
                </tr>
                <tr>
                    <td class="lblRight">
                    </td>
                    <td class="lblRight">
                        <asp:ListBox ID="lstCustomAttachments" runat="server"
                            Height="75px" Width="550px"></asp:ListBox>
                        <input type="hidden" name="ListBox1Hidden" />                      
                    </td>
                    <td valign="top">
                        <asp:Button ID="btnRemove" runat="server" Height="20px" OnClick="btnRemove_Click"
                            Text="Remove" Width="70px" CausesValidation="False" OnClientClick="clear()" />
                        <br />
                    </td>
                </tr>
            </table>
           </ContentTemplate>
                <Triggers>
                <asp:PostBackTrigger ControlID="btnUpload" />
                    <asp:AsyncPostBackTrigger ControlID="btnRemove" />                    
                </Triggers>
            </asp:UpdatePanel>
        </fieldset>--%>
    </div>
    <br />
    <center>
        <%--<asp:Button ID="btnSend" runat="server" Text="Send" CausesValidation="False" OnClick="btnSend_Click"
            OnClientClick="return validateEmail()" />&nbsp;--%>
        <input id="Button1" type="button"
                onclick="CloseEmail()" value="Close" />
    </center>

    <script type="text/javascript">
//        window.onload = clear;
        
//        function clear()
//        {
//            document.forms[0].ListBox1Hidden.value = "";
//        }
//        
//        function ListBox1_DoubleClick() 
//        {	         
//           document.forms[0].ListBox1Hidden.value = "doubleclicked";
//           document.forms[0].submit();
//        }

        function validateEmail()
        {
            var error = '';

            if(Field2Str(document.getElementById('<%=To.ClientID %>').value == ''))
                error += 'Please enter recipient address.  ';

            if(Field2Str(document.getElementById('<%=Subject.ClientID %>').value == ''))
                error += 'Please enter subject.  ';
                        
            if(error == '')
                return true;
            else
            {
                alert(error);
                return false;
            }
        }

        function clearFields()
        {
            
            //document.getElementById('').checked=false;
            //document.getElementById('').checked=false;
            document.getElementById('<%=To.ClientID %>').value = '';
            document.getElementById('<%=Cc.ClientID %>').value = '';
            document.getElementById('<%=Bcc.ClientID %>').value = '';
            document.getElementById('<%=Subject.ClientID %>').value = '';
            document.getElementById('<%=lblError.ClientID %>').innerText = '';
            document.getElementById('<%=lblMessage.ClientID %>').innerText = '';
            
            var edit = iged_getById('<% =txtHTMLBody.ClientID %>');
            edit.setText('');    
        }

        function copyMerchant()
        {
            var email1 = Field2Str(document.getElementById('<%=hfdMerchant.ClientID %>').value);
            var strTo = Field2Str(document.getElementById('<%=To.ClientID %>').value);

            //if(document.getElementById('').checked==true && email1 != '')
            //    strTo  += email1 + ',';
            //else
            //    strTo = strTo.replace(email1 + ',','');
               
           document.getElementById('<%=To.ClientID %>').value = strTo;
        }

        function copyAgent()
        {
            var email1 = Field2Str(document.getElementById('<%=hfdAgent.ClientID %>').value);
            var strTo = Field2Str(document.getElementById('<%=To.ClientID %>').value);

            //if(document.getElementById('').checked==true && email1 != '')
            //    strTo  += email1 + ',';
            //else
            //    strTo = strTo.replace(email1 + ',','');
               
               
            document.getElementById('<%=To.ClientID %>').value = strTo;
        }

//        function showWait()
//        {
//            document.forms[0].ListBox1Hidden.value = "";
//        }
    </script>

</asp:Panel>
