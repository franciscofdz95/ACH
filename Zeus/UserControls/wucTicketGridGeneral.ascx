<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucTicketGridGeneral.ascx.cs" Inherits="ZeusWeb.UserControls.wucTicketGridGeneral" %>

<%@ Register Assembly="Infragistics45.WebUI.WebHtmlEditor.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebHtmlEditor" TagPrefix="ighedit" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" 
    Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb"
    Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<link href="../css/font-awesome-4.2.0/css/font-awesome.min.css" rel="stylesheet">
<script>

    function SelectAllCheckboxes(chk) {

        $('#grdTick').find("input:checkbox").each(function () {
            if (this != chk) {
                this.checked = chk.checked;
            }

        });
    }

    function ClientCheck() {
        
        var valid = false;
        var ddpuser = $('#ddpUsers');
        var ddpAct = $("#'<%= ddlAction.ClientID %>'"); 

        $('#grdTick').find("input:checkbox").each(function () {
            if (this.checked) {
                valid = true;
            }

        });

        var message = "";

        if (!valid && $(ddpAct)[0].selectedIndex > 0) {
            message = "Please select at least one ticket to continue.\n";
            $(ddpAct)[0].selectedIndex = 0;
            $(ddpAct)[0].onchange();
        }
        else if ($(ddpAct)[0].selectedIndex == 0) {
            message += "Please select an action.";
        }
        else if ($(ddpuser)[0].selectedIndex == 0) {
            message += "Please select a user.\n";
        }

        if (message.length == 0)
            return true;
        else {
            alert(message);
            return false;
        }
        
    }

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlActions" runat="server">
            <table>
                <tr>
                    <td colspan="2" style="vertical-align: top;">
                        <table>
                            <tr>
                                <td style="vertical-align: top;">Category:</td>
                                <td style="vertical-align: top;">
                                    <asp:DropDownList ID="ddlTicketCategory" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlTicketCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:CheckBox runat="server" ID="chkNewApp" Text="Include New App Tickets" Checked="false"
                                        AutoPostBack="True" OnCheckedChanged="chkNewApp_CheckedChanged" Style="vertical-align: top;" />
                                    &nbsp;&nbsp;
                        <asp:CheckBox runat="server" ID="chkOpDispute" Text="Show Ops. Dispute Tickets" Checked="false"
                            AutoPostBack="True" Style="vertical-align: top;" OnCheckedChanged="chkOpDispute_CheckedChanged" Visible="false" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="2" style="vertical-align: top;">
                        <table>
                            <tr>
                                <td style="vertical-align: top;">Office:</td>
                                <td style="vertical-align: top;">
                                    <asp:CheckBoxList ID="lstOfficeAccess" runat="server" RepeatColumns="1" Style="margin-top: -5px;" Width="125px" AutoPostBack="true" OnSelectedIndexChanged="lstOfficeAccess_SelectedIndexChanged">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="3" style="vertical-align: top;">
                        <table>
                            <tr>
                                <td style="vertical-align: top; text-align: left;">
                                    <asp:Label runat="server" Text="Bulk Action:" ID="lblAction" ClientIDMode="Static" Width="70px"></asp:Label></td>
                                <td style="vertical-align: top;">
                                    <asp:DropDownList ID="ddlAction" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td runat="server" id="PnlUsers" style="display: none; vertical-align: top;">
                                    <asp:Label runat="server" ID="Label1" Text="Assign To:"></asp:Label>
                                    <asp:DropDownList ID="ddpUsers" runat="server" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <cc1:ListSearchExtender ID="ListSearchExtender2" runat="server" TargetControlID="ddpUsers"
                                        PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                        IsSorted="true" QueryPattern="Contains">
                                    </cc1:ListSearchExtender>
                                    <asp:Button ID="Button1" runat="server" Text="Save" OnClick="btnSave_Click"
                                        OnClientClick="javascript:return ClientCheck();"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel runat="server" ID="pnlPage">

                <table style="width: 100%;">
                    <tr>
                        <td>Page Size
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">5</asp:ListItem>
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                            <asp:ListItem>500</asp:ListItem>
                                        </asp:DropDownList>
                        </td>
                        <td class="lblRight">Ticket Count:<asp:Label ID="lblTicketCount" runat="server" Text=""></asp:Label>&nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </asp:Panel>
        <asp:GridView ID="grdTick" runat="server" OnRowDataBound="grdTick_RowDataBound" OnRowCommand="grdTick_RowCommand"
            AutoGenerateColumns="False" Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" DataSourceID="odsTick" AlternatingRowStyle-CssClass="alt"
            DataKeyNames="TicketUID,TicketID,MerchantUID,CategoryID,Category,Department,ParentID,Solution" AllowSorting="true" AllowPaging="true" PageSize="10" ClientIDMode="Static">
            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
            <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
            <Columns>
                <asp:TemplateField SortExpression="PriorityID">
                    <ItemTemplate></ItemTemplate>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lnkSort" runat="server" CommandName="Sort" CommandArgument="PriorityID">                   
                 <span class="fa fa-sort"></span>   
                        </asp:LinkButton>
                    </HeaderTemplate>
                </asp:TemplateField>
                <asp:TemplateField Visible="false">
                    <ItemStyle Width="10px" />
                    <ItemTemplate>
                        <asp:Image runat="server" ID="img" ImageUrl="~/Images/msg.gif" AlternateText="Attention Required"
                            ToolTip="Attention Required" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAll" runat="server" ToolTip="Select All" onclick="javascript:SelectAllCheckboxes(this);" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAction" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle Width="10px" />
                    <ItemStyle Width="10px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" ItemStyle-Width="40px" SortExpression="Status">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnID" runat="server" Text="Assign" CommandName="Assign" OnClientClick="return confirm('Are you sure you want to work on this ticket?');"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ID" SortExpression="TicketID" ItemStyle-Width="40px">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="labID" CssClass="fakea zeustooltip"><%# Eval("TicketID") %></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="40px"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="DBAName" HeaderText="DBA" SortExpression="DBAName">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:BoundField DataField="AgentFullName" HeaderText="Agent Name" SortExpression="AgentFullName">
                    <ItemStyle Width="80px" />
                </asp:BoundField>
                <asp:TemplateField ItemStyle-Width="80px" HeaderText="Subject" SortExpression="Problem">
                    <HeaderTemplate>
                        Issue
                                        <span class="headmoreless fakea" onclick="ToggleHeadMoreLess(this, event, '<%= grdTick.ClientID %>')">More</span>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <p style="margin: 0; padding: 0;" class="minimize">
                            <asp:Literal runat="server" ID="litSolution" Text='<%# Eval("Problem") %>' Mode="Encode"></asp:Literal>
                        </p>
                    </ItemTemplate>

                    <ItemStyle Width="80px"></ItemStyle>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Priority" Visible="false" SortExpression="Priority">

                    <ItemTemplate>
                        <asp:Label ID="lblPriority" Text='<%# Bind("Priority") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="60px" />
                </asp:TemplateField>
                <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DateCreated">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="DueDate" HeaderText="Due Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                    SortExpression="DueDate">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="Days" HeaderText="Days Aged" SortExpression="Days">
                    <ItemStyle Width="40px" />
                </asp:BoundField>
                <asp:BoundField DataField="UserCreated" HeaderText="User Created" SortExpression="UserCreated">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="LastChanged" HeaderText="Date Updated" SortExpression="LastChanged"
                    DataFormatString="{0:MM-dd-yy HH:mm tt}">
                    <ItemStyle Width="60px" />
                </asp:BoundField>
                <asp:BoundField DataField="TicketUID" HeaderText="Ticket UID" Visible="False" />
                <asp:BoundField DataField="PrivateLabelUID" HeaderText="Private Label UID" Visible="False" />
                <%--DM-1916--%>
                <%--<asp:TemplateField HeaderText="Office" SortExpression="OfficeID" Visible="False">
                    <ItemTemplate>
                        <asp:Label ID="LabelOffice" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="70px" />
                </asp:TemplateField>--%>
                <asp:BoundField DataField="MerchantUID" HeaderText="MerchantUID" Visible="false" />
                <asp:BoundField DataField="CategoryID" HeaderText="CategoryID" Visible="false" />
                <asp:BoundField DataField="Category" HeaderText="Category" Visible="false" />
                <asp:BoundField DataField="Department" HeaderText="Department" Visible="false" />
                <asp:BoundField DataField="ParentID" HeaderText="ParentID" Visible="false" />
                <asp:BoundField DataField="Solution" HeaderText="Solution" Visible="false" />
            </Columns>

            <PagerStyle CssClass="pgr"></PagerStyle>
        </asp:GridView>
        <asp:ObjectDataSource ID="odsTick" runat="server" SelectMethod="GetTicketsByRole"
            TypeName="PaymentXP.DataObjects.DataTicket" OldValuesParameterFormatString="original_{0}"
            OnSelecting="odsTick_Selecting">
            <SelectParameters>
                <asp:Parameter Name="prms" Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:Label ID="lblTickets" runat="server" Text="  No Tickets.." Visible="false"></asp:Label>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="430px" InitialLocation="Centered" ClientIDMode="Static"
            Modal="True" Width="800px" WindowState="Hidden">
            <ContentPane EnableRelativeLayout="true">
                <Template>
                    <fieldset style="height: 320px;">
                        <legend>
                            <asp:Label runat="server" ID="lblHeader" Text="Close Ticket Notes"></asp:Label></legend>
                        <div style="display: none" id="pnlBusy">
                            <asp:Image runat="server" ID="imgBusy" Style="width: 30px;" ImageUrl="~/Images/loading.gif" /><br />
                            Processing...
                        </div>
                        <div class="tnbox">
                            <asp:Label ID="lblError" runat="server" CssClass="gen_error"></asp:Label>
                            <br />
                            <asp:Panel runat="server" ID="pnlNotes" Visible="false" HorizontalAlign="left" CssClass="section">
                                <div id="mynotediv" class="ui-widget-content" style="height: 250px; overflow: hidden; border-top: none; border-left: none; border-right: none; border-bottom: solid 1px #ababab; padding-top: 25px; word-break: break-all">
                                    <ighedit:WebHtmlEditor EnableViewState="true"
                                        ID="Description"
                                        runat="server"
                                        FontFormattingList="Heading 1=&lt;h1&gt;&amp;Heading 2=&lt;h2&gt;&amp;Heading 3=&lt;h3&gt;&amp;Heading 4=&lt;h4&gt;&amp;Heading 5=&lt;h5&gt;&amp;Normal=&lt;p&gt;"
                                        FontNameList="Arial,Verdana,Tahoma,Courier New,Georgia"
                                        FontSizeList="1,2,3,4,5,6,7"
                                        FontStyleList="Blue Underline=color:blue;text-decoration:underline;&amp;Red Bold=color:red;font-weight:bold;&amp;ALL CAPS=text-transform:uppercase;&amp;all lowercase=text-transform:lowercase;&amp;Reset="
                                        Height="100%"
                                        ImageDirectory="../ig_common/Images/htmleditor/"
                                        SpecialCharacterList="Ω,Σ,Δ,Φ,Γ,Ψ,Π,Θ,Ξ,Λ,ξ,μ,η,φ,ω,ε,θ,δ,ζ,ψ,β,π,σ,ß,þ,Þ,&amp;#402,Ж,Ш,Ю,Я,ж,ф,ш,ю,я,お,あ,絵,Æ,Å,Ç,Ð,Ñ,Ö,æ,å,ã,ç,ð,ë,ñ,¢,£,¤,¥,№,,©,®,,@,,¡,,←,↑,→,↓,↔,↕,↖,↗,↘,↙,,¦,§,¨,ª,¬,¯,¶,°,±,«,»,·,¸,º,¹,²,³,¼,½,¾,¿,×,÷"
                                        JavaScriptDirectory="../ig_htmleditor/"
                                        UseLineBreak="True"
                                        Width="100%">
                                        <Toolbar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
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
                                            <ighedit:ToolbarDialogButton runat="server" Type="FontColor"></ighedit:ToolbarDialogButton>
                                        </Toolbar>
                                        <DropDownStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                        <ProgressBar Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                        <DownlevelTextArea Columns="0" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Rows="0" Wrap="True" />
                                        <RightClickMenu Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False">
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="Cut"></ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="Copy"></ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="Paste"></ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="PasteHtml"></ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="CellProperties">
                                                <Dialog InternalDialogType="CellProperties" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="TableProperties">
                                                <Dialog InternalDialogType="ModifyTable" />
                                            </ighedit:HtmlBoxMenuItem>
                                            <ighedit:HtmlBoxMenuItem runat="server" Act="InsertImage"></ighedit:HtmlBoxMenuItem>
                                        </RightClickMenu>
                                        <TextWindow Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Width="100%" />
                                        <DownlevelLabel Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" />
                                        <TabStrip BorderColor="DimGray" DesignImageName="" DesignText=" " Enabled="false" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HighlightBackColor="Menu" HighlightBorderColor="Menu" HighlightBorderStyle="None" HighlightForeColor="Menu" HtmlImageName="" HtmlText=" " />
                                    </ighedit:WebHtmlEditor>

                                </div>
                                <br />
                                <asp:Button ID="btnSave" runat="server" CausesValidation="False" OnClick="btnSave_Click" TabIndex="127" Text="Save" Width="90px" Enabled="true" ClientIDMode="Static" OnClientClick="DisplayProcessing()" />
                                <asp:Button ID="btnClear" runat="server" CausesValidation="False" OnClick="btnClose_Click" TabIndex="128" Text="Cancel" Width="90px" />
                            </asp:Panel>
                            <script type="text/javascript">
                                function DisplayProcessing() {
                                    $('#pnlBusy').css("display", "block");
                                };
                            </script>
                        </div>
                    </fieldset>
                </Template>
            </ContentPane>
            <Header CaptionText="Bulk Close Tickets" CloseBox-Visible="false">
            </Header>
        </ig:WebDialogWindow>        
        <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="150px" InitialLocation="Centered" ClientIDMode="Static"
            Modal="True" Width="400px" WindowState="Hidden">
            <ContentPane EnableRelativeLayout="true">
                <Template>
                    <div style="align-content: center; align-items: center; vertical-align: central; margin-bottom: 25px; margin-top: 25px">
                        <table cellspacing="5" width="100%" align="center">
                            <tr>
                                <td align="center">
                                    <asp:Label runat="server" ID="lblError1" Text="" Font-Names="Verdana" Font-Size="X-Small"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" Text="Ok" ID="btnOk" OnClick="btnOk_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </Template>
            </ContentPane>
            <Header CaptionText="Alert" CloseBox-Visible="false"></Header>
        </ig:WebDialogWindow>
    </ContentTemplate>
</asp:UpdatePanel>

