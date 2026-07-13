<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucEmailBlaster" Codebehind="wucEmailBlaster.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<div class="contentpage">
    <asp:Panel ID="pnlDetail" runat="server" Width="100%">
        <uc2:wucMessage ID="WucMessage1" runat="server"></uc2:wucMessage>
        <asp:Label ID="lblMessage" runat="server" Font-Size="10pt" ForeColor="Green"></asp:Label>
        <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>
        <table width="100%" cellspacing="5">
            <tr>
                <td valign="top">
                    <fieldset style="width: 201px;">
                        <legend>
                            <asp:Label runat="server" ID="lblFields" Text="merchant fields:"></asp:Label></legend>
                        <asp:ListBox ID="lstProperties" runat="server" Height="200px" Width="200px"></asp:ListBox></fieldset>
                </td>
                <td valign="top" align="left">
                    <fieldset>
                        <legend>Send Email To</legend>
                        <asp:RadioButtonList runat="server" ID="rblEmail" RepeatDirection="Horizontal" AutoPostBack="true"
                            OnSelectedIndexChanged="rblEmail_SelectedIndexChanged">
                            <asp:ListItem Text="Merchants" Value="0" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Agents" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                    <asp:Panel runat="server" ID="pnlAgent" Visible="true">
                        <fieldset>
                            <legend>Agents</legend>
                            <%--<asp:DropDownList ID="AgentUID" runat="server" Width="400px">
                            </asp:DropDownList>
                            <cc1:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="AgentUID"
                                PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                IsSorted="true" QueryPattern="Contains">
                            </cc1:ListSearchExtender>--%>
                            <uc1:AgentSelector runat="server" ID="wucAgentSelector" LayoutStyle="Horizontal" IDWidth="75"
                                DBAWidth="75" lblDBAWidth="70px" lblIDWidth="70px"  />
                            <br />
                            <asp:CheckBox ID="IncludeSubAgents" runat="server" Text="Include Sub-Agents" />
                        </fieldset>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="pnlMerType" Visible="true">
                        <fieldset>
                            <legend>Bank</legend>
                            <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="400px">
                            </asp:DropDownList>
                        </fieldset>
                    </asp:Panel>
                    <fieldset>
                        <legend>
                            <asp:Label runat="server" ID="lblSelect" Text="Portals"></asp:Label></legend>
                        <asp:CheckBoxList ID="lstPortals" runat="server" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                        <asp:CheckBoxList ID="AgentTypeUID" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                            Visible="false">
                        </asp:CheckBoxList>
                    </fieldset>
                    <fieldset>
                        <legend>Other options</legend>
                        <asp:RadioButtonList runat="server" ID="rbMail" RepeatDirection="Horizontal" AutoPostBack="true"
                            OnSelectedIndexChanged="rblMail_SelectedIndexChanged">
                            <asp:ListItem Text="No Mail Merge" Value="0" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Mail Merge" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </fieldset>
                </td>
            </tr>
        </table>
        <fieldset>
            <legend>Email Message</legend>Subject:<br />
            <asp:TextBox ID="Subject" runat="server" Width="400px"></asp:TextBox><br />
            <br />
            Body:<br />
            <ighedit:WebHtmlEditor ID="txtHTMLBody" runat="server" Width="600px" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
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
        </fieldset>
        <fieldset>
            <legend>Send Test Email</legend>Email Address:<br />
            <asp:TextBox ID="txtTestEmailAddress" runat="server" Width="400px"></asp:TextBox><asp:Button
                ID="btnSendTestEmail" runat="server" Text="Send Test Email" OnClick="btnSendTestEmail_Click" />
        </fieldset>
        <fieldset>
            <legend>
                <asp:Label ID="lblIDList" runat="server" Text=""></asp:Label></legend>
            <asp:TextBox ID="IDList" runat="server" Height="400px" MaxLength="8000" TextMode="MultiLine"
                Width="100%"></asp:TextBox>
        </fieldset>
        <asp:HiddenField ID="EmailBlasterID" runat="server" />
        <asp:HiddenField ID="To" runat="server"></asp:HiddenField>
        <div style="display: none">
            <fieldset>
                <legend>Attachments</legend>
                <table width="550" cellspacing="2">
                    <tr>
                        <td class="lblRight">
                            Attachments:</td>
                        <td class="lblRight">
                            <asp:FileUpload ID="fuplAttachments" runat="server" Height="20px" Width="100%" />
                            <asp:HiddenField ID="lblMsg" runat="server" />
                            <asp:HiddenField ID="hdnIsload" runat="server" />
                        </td>
                        <td>
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Add" Width="70px"
                                CausesValidation="False" OnClientClick="javascript:showWait();" />
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                        </td>
                        <td class="lblRight">
                            <asp:ListBox ID="lstCustomAttachments" runat="server" Height="75px" Width="550px"></asp:ListBox>
                        </td>
                        <td valign="top">
                            <asp:Button ID="btnRemove" runat="server" OnClick="btnRemove_Click" Text="Remove"
                                Width="70px" CausesValidation="False" OnClientClick="clear()" />
                            <br />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
        <br />
        <asp:Button ID="btnOpen" runat="server" CausesValidation="False" OnClick="btnOpen_Click"
            Style="display: none;" />

        <script type="text/javascript">          
        
        function ListBox1_DoubleClick() 
        {	       
           var lb = document.getElementById('<%=lstCustomAttachments.ClientID %>');
           if(lb.options.length > 0)
            document.getElementById('<%=btnOpen.ClientID %>').click();        
           else
            return false; 
        }

        function ListBox2_DoubleClick()
        {
            var txtItem = "";
            var lb = document.getElementById('<%=lstProperties.ClientID %>');
            
            if(lb.options.length == 0)
                return;
                
            for (var i=lb.options.length-1; i>=0; i--)
            {                
                if(lb.options[i].selected)
                {
                    txtItem = lb.options[i].value;                   
                    break;
                }
            }
            var editor = iged_getById('<% =txtHTMLBody.ClientID %>');
            if(!editor)
                return;
            var text  =  editor.getText();  
            text += ' ' + '[[' + txtItem + ']]';   
            editor.setText(text);                       
        }
        
        function validateEmail(oButton, oEvent)
        {           
            document.getElementById('<%=lblError.ClientID %>').innerText = '';
            document.getElementById('<%=lblMessage.ClientID %>').innerText = '';
            var isTest = '0';
            var text = oButton.getText();
            if(text.toLowerCase().indexOf("test") != -1)
                isTest = '1';
            var error = '';     
                        
            if(document.getElementById('<%=Subject.ClientID %>') != null)
            {
                if(Field2Str(document.getElementById('<%=Subject.ClientID %>').value == ''))
                    error += 'Please enter subject.  ';
            }            
            
            if(error == '')
            {
                if(isTest == '1' && document.getElementById('<%=To.ClientID %>') != null)
                {
                   var text = prompt('Please enter To address',''); 
                   if(text == null || text == '') 
                   {
                        alert('Please enter recipient.');
                        oEvent.cancel = true;
                        return false;
                   }
                   else
                   {                      
                        var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;   
                        if(emailPattern.test(text))
                             document.getElementById('<%=To.ClientID %>').value = text;
                        else    
                         {
                            alert('Enter valid email address.');
                            oEvent.cancel = true;
                            return false;
                         }                      
                   }
                }   
                var x = confirm("Are you sure, you want to send this email"); 
                if(!x)
                oEvent.cancel = true;              
            }
            else
            {
                alert(error);                
                oEvent.cancel = true;
            }
        }

        function clearFields()
        {
            var lb = document.getElementById('<%=lstCustomAttachments.ClientID %>');
            for (var i=lb.options.length-1; i>=0; i--)
            {
                lb.options[i] = null;
            }
            lb.selectedIndex = -1;
            
            document.getElementById('<%=To.ClientID %>').value = '';           
            document.getElementById('<%=Subject.ClientID %>').value = '';
            document.getElementById('<%=lblError.ClientID %>').innerText = '';
            document.getElementById('<%=lblMessage.ClientID %>').innerText = '';
            
            var edit = iged_getById('<% =txtHTMLBody.ClientID %>');
            edit.setText('');    
        }
        
        function Field2Str(fieldvalue)
        {
            if (fieldvalue == null)
                return '';
            else
                return fieldvalue;
        }
    
        function showWait()
        {
            document.forms[0].ListBox1Hidden.value = "";
            if (document.getElementById('<%=fuplAttachments.ClientID %>').value.length > 0)
            {
                document.getElementById('<%=lblMsg.ClientID %>').value = document.getElementById('<%=fuplAttachments.ClientID %>').value;
            }
        }

      
        </script>

    </asp:Panel>
</div>
