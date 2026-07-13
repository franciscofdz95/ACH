<%@ Page Title="Product Management" Language="C#" MasterPageFile="~/MasterPageReports.master" AutoEventWireup="true"
    CodeBehind="frmProductManagement.aspx.cs" Inherits="frmProductManagement" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
    <%@ MasterType VirtualPath="~/MasterPageReports.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            
            $('#MeritusBrand').change(function () {
                toggleOptimal(this);
            });

            toggleOptimal($('#MeritusBrand'));
        });

        function toggleOptimal(el) {
            if ($(el).prop('checked')) {
                $('.hideOptimal').css('visibility', 'visible');
            } else {
                $('.hideOptimal').css('visibility', 'hidden');
            }
        }
    </script>
<asp:ValidationSummary ID="ValidationSummary1" runat="server"></asp:ValidationSummary>
    <asp:Panel runat="server" ID="pnlToolbar" CssClass="tbrtools">
        <div class="tbrtools">
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
                            CausesValidation="False" CommandName="Save" OnClick="tbrTools_ButtonClicked"
                            ClickOnEnterKey="false" ClickOnSpaceKey="false" TabIndex="123">
                            <Appearance>
                                <Image Url="~/Images/disk_blue.png" />
                            </Appearance>
                            <ClientSideEvents Click="btnSave_Click" />
                        </igtxt:WebImageButton>
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
                    <td>
                        <igtxt:WebImageButton ID="btnRefresh" runat="server" Text="Refresh" CommandName="Refresh"
                            AccessKey="r" OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                            <Appearance>
                                <Image Url="~/Images/refresh.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                     <td>
                        <igtxt:WebImageButton ID="btnPreview" runat="server" Text="Preview" CommandName="Preview"
                            OnClick="tbrTools_ButtonClicked" CausesValidation="False" TabIndex="125">
                            <Appearance>
                                <Image Url="~/Images/End.gif" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td align="right">
                        <asp:HyperLink ID="BackToProducts" runat="server" NavigateUrl="~/SecureReports/frmProducts.aspx" Text="Back to Products"></asp:HyperLink>
                    </td>
                </tr>
            </table>
        </div>
    </asp:Panel>
    <div>
        <fieldset>
            <legend>Settings</legend>
            <table>
                <tr>
                    <td valign="top">
                        <table>
                            <tr>
                                <td valign="top">
                                    Name:
                                </td>
                                <td colspan="3">
                                    <asp:TextBox Width="200" MaxLength="50" ID="ProductName" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="ProductNameValidator" runat="server" ControlToValidate="ProductName" ErrorMessage="Product Name is required."></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Status:
                                </td>
                                <td colspan="3">
                                    <asp:DropDownList ID="StatusList" runat="server">
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Category:</td>
                                <td colspan="3">
                                    <asp:DropDownList ID="CategoryID" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td valign="top">Ticket Creation:</td>
                                <td colspan="3">
                                    <asp:CheckBox ID="CreateTicket" runat="server" Text="Include" />
                                </td>
                            </tr>
                            <tr>
                                <td>Brand:</td>
                                <td>
                                     <!--PXP-7231(Meritus word replacement with paysafe) By Sanidhya kumar-->
                                    <asp:CheckBox ID="MeritusBrand" runat="server" Text="Paysafe" ClientIDMode="Static" />
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="OptimalBrand" runat="server" Text="Optimal" />
                                </td>
                            </tr>
                            <tr class="hideOptimal">
                                <td valign="top">
                                    Portal:
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="ApexVisible" Text="Apex" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="InsightVisible" Text="Insight" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="PaymentXPVisible" Text="PaymentXP" />
                                </td>
                                <td>
                                    <asp:CheckBox runat="server" ID="ApplicationXPVisible" Text="ApplicationXP" />
                                </td>
                            </tr>
                            <tr class="hideOptimal">
                                <td valign="top">Private Label:</td>
                                <td colspan="3">
                                    <asp:CheckBox ID="IsNOTVisibleOnPrivateLabel" runat="server" Text="Exclude" />
                                </td>
                            </tr>
                           
                        </table>
                    </td>
                    
                </tr>
            </table>
        </fieldset>
        <fieldset>
            <legend>Content</legend>
            <table>
                <tr>
                    <td valign="top">
                        Summary:
                    </td>
                    <td>
                        <asp:TextBox ID="ProductDescription" MaxLength="2000" TextMode="MultiLine" runat="server" Width="700"
                            Rows="6"></asp:TextBox><asp:RequiredFieldValidator ID="ProductDescriptionValidatorReq" runat="server" ControlToValidate="ProductDescription" ErrorMessage="Product Description is required."></asp:RequiredFieldValidator>
                            
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        Full Description:
                    </td>
                    <td valign="top">
                        <ighedit:WebHtmlEditor ID="MarketingContent" runat="server" Width="700px" FontFormattingList="Heading 1=<h1>&Heading 2=<h2>&Heading 3=<h3>&Heading 4=<h4>&Heading 5=<h5>&Normal=<p>"
                            FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia" FontSizeList="1,2,3,4,5,6,7"
                            FontStyleList="Blue Underline=color:blue;text-decoration:underline;&Red Bold=color:red;font-weight:bold;&ALL CAPS=text-transform:uppercase;&all lowercase=text-transform:lowercase;&Reset="
                            SpecialCharacterList="&#937;,&#931;,&#916;,&#934;,&#915;,&#936;,&#928;,&#920;,&#926;,&#923;,&#958;,&#956;,&#951;,&#966;,&#969;,&#949;,&#952;,&#948;,&#950;,&#968;,&#946;,&#960;,&#963;,&szlig;,&thorn;,&THORN;,&#402,&#1046;,&#1064;,&#1070;,&#1071;,&#1078;,&#1092;,&#1096;,&#1102;,&#1103;,&#12362;,&#12354;,&#32117;,&AElig;,&Aring;,&Ccedil;,&ETH;,&Ntilde;,&Ouml;,&aelig;,&aring;,&atilde;,&ccedil;,&eth;,&euml;,&ntilde;,&cent;,&pound;,&curren;,&yen;,&#8470;,&#153;,&copy;,&reg;,&#151;,@,&#149;,&iexcl;,&#14;,&#8592;,&#8593;,&#8594;,&#8595;,&#8596;,&#8597;,&#8598;,&#8599;,&#8600;,&#8601;,&#18;,&brvbar;,&sect;,&uml;,&ordf;,&not;,&macr;,&para;,&deg;,&plusmn;,&laquo;,&raquo;,&middot;,&cedil;,&ordm;,&sup1;,&sup2;,&sup3;,&frac14;,&frac12;,&frac34;,&iquest;,&times;,&divide;"
                            UseLineBreak="True" Height="500px">
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
            </table>
        </fieldset>
    </div>
</asp:Content>
