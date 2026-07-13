<%@ Page Language="C#" MasterPageFile="~/MasterPageSales.master" AutoEventWireup="true" Inherits="frmCommunicationsDetail"
    Title="Communications Detail" Codebehind="frmCommunicationsDetail.aspx.cs" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="contentpage">
        <div class="tbrtools">
            <div class="tbrtoolsleft">
                <igtxt:WebImageButton ID="btnSend" runat="server" Text="Send" CommandName="Send"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" AccessKey="n">
                    <Appearance>
                        <Image Url="~/Images/mail2.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;
                <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" AccessKey="e">
                    <Appearance>
                        <Image Url="~/Images/edit.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnAdd" AccessKey="a" runat="server" Text="Add" CommandName="Add"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                    <Appearance>
                        <Image Url="~/Images/add2.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false"
                    CommandName="Save" OnClick="tbrTools_ButtonClicked" AccessKey="s">
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
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" AccessKey="r">
                    <Appearance>
                        <Image Url="~/Images/refresh.png" />
                    </Appearance>
                </igtxt:WebImageButton>
                &nbsp;<igtxt:WebImageButton ID="btnClose" runat="server" Text="Close" CommandName="Close"
                    OnClick="tbrTools_ButtonClicked" CausesValidation="False" AccessKey="o">
                    <Appearance>
                        <Image Url="~/Images/delete.png" />
                    </Appearance>
                </igtxt:WebImageButton>
            </div>
           
        </div>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Size="10pt"></asp:Label>
        <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Green"></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="10pt" />
        <div class="dialog" style="padding-right: 10px;">
            <asp:TextBox ID="MerchantAppUID" runat="server" BorderStyle="None" Enabled="False"
                ForeColor="White" Visible="False"></asp:TextBox><asp:TextBox ID="CommunicationID"
                    runat="server" BorderStyle="None" Enabled="False" ForeColor="White" Visible="False"></asp:TextBox>
            <asp:TextBox ID="LeadID" runat="server" BorderStyle="None" Enabled="False" ForeColor="White"
                Visible="False"></asp:TextBox>
            <asp:Panel ID="pnlDetail" runat="server" Height="100%" Width="100%">
                <fieldset>
                    <legend>&nbsp;Add/Edit Emails</legend>
                    <div class="bucketbdy">
                        <table width="100%" cellspacing="5">
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                </td>
                                <td class="lblEdit">
                                    <asp:RadioButtonList ID="lstType" runat="server" RepeatColumns="2" AutoPostBack="True"
                                        OnSelectedIndexChanged="lstType_SelectedIndexChanged" Visible="False">
                                        <asp:ListItem Selected="true">Email</asp:ListItem>
                                        <asp:ListItem>Fax</asp:ListItem>
                                    </asp:RadioButtonList></td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    Time Sent:</td>
                                <td class="lblEdit">
                                    <asp:Label ID="TimeSent" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    DBA Name:</td>
                                <td>
                                    <asp:TextBox ID="DBAName" runat="server" Enabled="False" MaxLength="50" ReadOnly="True"
                                        Width="250px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    To:</td>
                                <td class="lblEdit">
                                    <asp:TextBox ID="To" runat="server" Width="400px"></asp:TextBox>
                                    <asp:Label ID="lblEFax" runat="server" Text="@efaxsend.com" Visible="False"></asp:Label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="To"
                                        Display="None" ErrorMessage="Recipient(s) required"></asp:RequiredFieldValidator></td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    CC:</td>
                                <td>
                                    <asp:TextBox ID="Cc" runat="server" Width="400px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    BCC:</td>
                                <td>
                                    <asp:TextBox ID="Bcc" runat="server" Width="400px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblEdit" style="width: 100px">
                                    Subject:</td>
                                <td>
                                    <asp:TextBox ID="Subject" runat="server" Width="400px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Subject"
                                        Display="None" ErrorMessage="Subject required"></asp:RequiredFieldValidator></td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
                <br />
                <fieldset>
                    <legend>&nbsp;Email Body</legend>
                    <div class="bucketbdy">
                        <ighedit:WebHtmlEditor ID="txtHTMLBody" runat="server" Width="100%" Height="150px"
                            UploadedFilesDirectory="~/forms">
                            <Toolbar>
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="DoubleSeparator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Bold" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Italic" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Underline" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Strikethrough" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Subscript" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Superscript" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Cut" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Copy" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Paste" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Undo" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Redo" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="JustifyLeft" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="JustifyCenter" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="JustifyRight" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="JustifyFull" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Indent" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Outdent" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="UnorderedList" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="OrderedList" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarDialogButton runat="server" Type="InsertRule">
                                    <Dialog InternalDialogType="InsertRule" />
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="RowSeparator" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="DoubleSeparator" />
                                <ighedit:ToolbarDialogButton runat="server" Type="FontColor">
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarDialogButton runat="server" Type="FontHighlight">
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarDialogButton runat="server" Type="SpecialCharacter">
                                    <Dialog InternalDialogType="SpecialCharacterPicker" Type="InternalWindow" />
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarMenuButton runat="server" Type="InsertTable">
                                    <Menu>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties">
                                            <Dialog InternalDialogType="InsertTable" />
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="InsertColumnRight">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="InsertColumnLeft">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="InsertRowAbove">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="InsertRowBelow">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="DeleteRow">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="DeleteColumn">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="IncreaseColspan">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="DecreaseColspan">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="IncreaseRowspan">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="DecreaseRowspan">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties">
                                            <Dialog InternalDialogType="CellProperties" />
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties">
                                            <Dialog InternalDialogType="ModifyTable" />
                                        </ighedit:HtmlBoxMenuItem>
                                    </Menu>
                                </ighedit:ToolbarMenuButton>
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="ToggleBorders" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="InsertLink" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="RemoveLink" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    RaisePostback="True" Type="Save" />
                                <ighedit:ToolbarUploadButton runat="server" Type="Open">
                                    <Upload Filter="*.htm,*.html,*.asp,*.aspx" Height="350px" Mode="File" Width="500px" />
                                </ighedit:ToolbarUploadButton>
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Preview" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="Separator" />
                                <ighedit:ToolbarDialogButton runat="server" Type="FindReplace">
                                    <Dialog InternalDialogType="FindReplace" />
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarDialogButton runat="server" Type="InsertBookmark">
                                    <Dialog InternalDialogType="InsertBookmark" />
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarUploadButton runat="server" Type="InsertImage">
                                    <Upload Height="420px" Width="500px" />
                                </ighedit:ToolbarUploadButton>
                                <ighedit:ToolbarUploadButton runat="server" Type="InsertFlash">
                                    <Upload Filter="*.swf" Height="440px" Mode="Flash" Width="500px" />
                                </ighedit:ToolbarUploadButton>
                                <ighedit:ToolbarUploadButton runat="server" Type="InsertWindowsMedia">
                                    <Upload Filter="*.asf,*.wma,*.wmv,*.wm,*.avi,*.mpg,*.mpeg,*.m1v,*.mp2,*.mp3,*.mpa,*.mpe,*.mpv2,*.m3u,*.mid,*.midi,*.rmi,*.aif,*.aifc,*.aiff,*.au,*.snd,*.wav,*.cda,*.ivf"
                                        Height="400px" Mode="WindowsMedia" Width="500px" />
                                </ighedit:ToolbarUploadButton>
                                <ighedit:ToolbarDialogButton runat="server" Type="Help">
                                    <Dialog InternalDialogType="Text" />
                                </ighedit:ToolbarDialogButton>
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="CleanWord" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="WordCount" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="PasteHtml" />
                                <ighedit:ToolbarMenuButton runat="server" Type="Zoom">
                                    <Menu>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom25">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom50">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom75">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom100">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom200">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom300">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom400">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom500">
                                        </ighedit:HtmlBoxMenuItem>
                                        <ighedit:HtmlBoxMenuItem runat="server" Act="Zoom600">
                                        </ighedit:HtmlBoxMenuItem>
                                    </Menu>
                                </ighedit:ToolbarMenuButton>
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="TogglePositioning" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="BringForward" />
                                <ighedit:ToolbarButton runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="SendBackward" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="RowSeparator" />
                                <ighedit:ToolbarImage runat="server" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Type="DoubleSeparator" />
                                <ighedit:ToolbarDropDown runat="server" Type="FontName">
                                </ighedit:ToolbarDropDown>
                                <ighedit:ToolbarDropDown runat="server" Type="FontSize">
                                </ighedit:ToolbarDropDown>
                                <ighedit:ToolbarDropDown runat="server" Type="FontFormatting">
                                </ighedit:ToolbarDropDown>
                                <ighedit:ToolbarDropDown runat="server" Type="FontStyle">
                                </ighedit:ToolbarDropDown>
                                <ighedit:ToolbarDropDown runat="server" Type="Insert">
                                    <Items>
                                        <ighedit:ToolbarDropDownItem runat="server" Act="Greeting" />
                                        <ighedit:ToolbarDropDownItem runat="server" Act="Signature" />
                                    </Items>
                                </ighedit:ToolbarDropDown>
                            </Toolbar>
                            <RightClickMenu>
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
                        </ighedit:WebHtmlEditor>
                        <asp:CheckBoxList ID="lstAttachments" runat="server" CssClass="lbledit">
                        </asp:CheckBoxList>
                        <asp:RadioButtonList ID="RadioButtonList2" runat="server" CssClass="lbledit" Visible="False">
                            <asp:ListItem>Blank</asp:ListItem>
                            <asp:ListItem>New Merchant Welcome</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </fieldset>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
