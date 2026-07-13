<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucOwnerUW" CodeBehind="wucOwnerUW.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc7" %>

<script language="javascript" type="text/javascript">

    function CheckNumeric() {
        var key;
        key = event.which ? event.which : event.keyCode;

        if ((key >= 48 && key <= 57) || key == 13) {
            return true;
        }
        else {
            return false;
        }
    }

</script>

<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
    </legend>
    <table cellspacing="2" width="100%">

        <tr>
            <td colspan="8"></td>
        </tr>

        <tr>
            <td class="lblRight">Owner Name:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="lblFullName" runat="server" Text="" Width="150px"></asp:TextBox>
            </td>
            <td class="lblRight">Property:</td>
            <td>
                <asp:DropDownList ID="NameAddressSSNSummary" runat="server" Width="100px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="0">Waived</asp:ListItem>
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                </asp:DropDownList>
            </td>

            <td class="lblRight">Ownership:</td>
            <td>
                <asp:TextBox ReadOnly="true" ID="PercentOwnership" runat="server" Text="" Width="150px"></asp:TextBox></td>
            <td class="lblRight">Guaranty:</td>
            <td>
                <asp:DropDownList ID="NameAddressPhoneSummary" runat="server" Width="100px">
                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem Value="1">Personal</asp:ListItem>
                    <asp:ListItem Value="2">Corporate</asp:ListItem>
                    <asp:ListItem Value="0">Waived</asp:ListItem>
                </asp:DropDownList>
            </td>

        </tr>

        <tr>
            <td rowspan="2"  class="lblRight" style="vertical-align: top;">Inquiry:</td>
            <td rowspan="2"  style="vertical-align: top;">
                <asp:TextBox runat="server" ID="Inquiry" TextMode="MultiLine" Rows="3" Width="150px"
                    Enabled="true"></asp:TextBox>
            </td>
            <td class="lblRight" style="vertical-align: top;">SSN:</td>
            <td style="vertical-align: top;">
                <asp:TextBox ID="SSN" runat="server" Text="" Width="95px" ReadOnly="true"></asp:TextBox>
            </td>

        </tr>
        <tr>
            <td colspan="2" style="vertical-align: top;">
                <asp:Panel runat="server" ID="pnlBeneficialOwner">
                    <asp:CheckBox ID="BeneficialOwner" runat="server" Text="Equity/Ownership" />
                </asp:Panel>
            </td>
            <td colspan="2" style="vertical-align: top;">
                <asp:Panel runat="server" ID="pnlAuthorizedSignature">
                    <asp:CheckBox ID="AuthorizedSignature" runat="server" Text="Controller" />
                </asp:Panel>
            </td>
            <td colspan="2">
                <asp:CheckBox ID="CBRWaived"  runat="server" Text="CBR Waived"/>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <asp:HiddenField runat="server" ID="BusinessProfileID" />
                <asp:HiddenField runat="server" ID="CreditReportDate" />
                <uc7:wucMessage runat="server" ID="WucMessage1" />
                <asp:HiddenField runat="server" ID="hidAddress" />
            </td>
            <td class="lblRight">Credit Report:</td>
            <td colspan="2">
                <%--<asp:Button ID="btnGet" runat="server" Text="Get" OnClick="btnGet_Click"></asp:Button>--%><%-- DM-2524 --%>
                <asp:Button ID="btnView" runat="server" Text="View" OnClick="btnView_Click" Visible="false" />
                <asp:Button ID="History" runat="server" Text="Add" OnClick="Button1_Click"></asp:Button>
                <%--<asp:Button ID="btnAddDoc" runat="server" Text="Save as PDF" OnClick="btnAddDoc_Click" Visible="false"/>--%>
                <asp:Button ID="btnClear" runat="server" Text="Clear" OnClick="btnClear_Click" Visible="false" />
            </td>

        </tr>
        <tr>
            <td colspan="8">
                <fieldset>
                    <legend>Credit History</legend>
                    <asp:GridView ID="grdCreditHistory" runat="server" AutoGenerateColumns="False"
                        Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                        AlternatingRowStyle-CssClass="alt" ClientIDMode="Static" OnRowDataBound="grdCreditHistory_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Creditdate" HeaderText="Credit Date">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="CreditType" HeaderText="Credit Type">
                                <ItemStyle Width="75px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Creditscore" HeaderText="Credit Score">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="NoofTrades" HeaderText="# of Trades">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="OldestTrade" HeaderText="Oldest Trade">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Notes" HeaderText="Notes">
                                <ItemStyle Width="200px" />
                            </asp:BoundField>

                        </Columns>
                        <EmptyDataTemplate>
                            No Records Found....
                        </EmptyDataTemplate>
                    </asp:GridView>
                </fieldset>
            </td>
            <%-- <td colspan="4">
                <fieldset>
                    <legend>Experian</legend>
                  <%--  <table style="width: 100%;">
                        <tr>
                            <td class="lblRight">Credit Score:</td>
                            <td>
                                <asp:TextBox ID="CreditScore" runat="server" Width="100px" Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td class="lblRight"># of Trades:</td>
                            <td>
                                <asp:TextBox ID="NoofTrades" runat="server" Width="100px" Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">Credit Date:</td>
                            <td class="lblRight">
                                <ig:WebDatePicker ID="CreditDate" runat="server" Width="105px" BackColor="#EFF3FF"
                                    Enabled="false" ReadOnly="true" BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1"/>
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                            <td class="lblRight">Oldest Trade:</td>
                            <td>
                                <ig:WebDatePicker ID="OldestTrade" runat="server" NullText="" Width="105px" DisplayModeFormat="MM/dd/yyyy"
                                    Enabled="false" ReadOnly="true">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight" valign="top">Notes:
                            </td>
                            <td colspan="3" align="left">
                                <asp:TextBox runat="server" ID="TradeStatus" TextMode="MultiLine" Rows="4" Width="95%"
                                    Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
            <td colspan="4">
                <fieldset>
                    <legend>TransUnion</legend>
                <%--    <table style="width: 100%;">
                        <tr>
                            <td class="lblRight">Credit Score:</td>
                            <td>
                                <asp:TextBox ID="TCreditScore" runat="server" Width="100px" Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td class="lblRight"># of Trades:</td>
                            <td>
                                <asp:TextBox ID="TNoofTrades" runat="server" Width="100px" Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">Credit Date:</td>
                            <td class="lblRight">
                                <ig:WebDatePicker ID="TCreditDate" runat="server" Width="105px" BackColor="#EFF3FF"
                                    Enabled="false" ReadOnly="true" BorderStyle="Solid" BorderWidth="1px">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                            <td class="lblRight">Oldest Trade:</td>
                            <td>
                                <ig:WebDatePicker ID="TOldestTrade" runat="server" NullText="" Width="105px" DisplayModeFormat="MM/dd/yyyy"
                                    Enabled="false" ReadOnly="true">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight" valign="top">Notes:
                            </td>
                            <td colspan="3" align="left">
                                <asp:TextBox runat="server" ID="TTradeStatus" TextMode="MultiLine" Rows="4" Width="95%"
                                    Enabled="false" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>--%>
        </tr>
        <tr>
            <td colspan="8"></td>
        </tr>
    </table>
    <ig:WebDialogWindow ID="WebDialogWindow2" runat="server" Height="475px" Width="700px"
        Modal="True" InitialLocation="Centered" UseBodyAsParent="True" WindowState="Hidden">
        <ContentPane>
            <Template>
                <asp:Panel runat="server" ID="pnl1">
                    <fieldset>
                        <legend>Credit Details</legend>
                    <table width="100%">
                        <tr>
                            <td colspan="6">
                                <asp:Label runat="server" ID="lblError" ForeColor="red"></asp:Label></td>
                        </tr>

                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblUID" Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblOwerUID" Visible="false"></asp:Label>

                            </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="lblRight">CreditDate:</td>
                            <td>
                                <asp:Label ID="lblCreditDate" Text="Credit Date" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}" Width="150px" runat="server"></asp:Label>
                            </td>
                            <td class="lblRight">Credit Type:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCreditTypeID" runat="server" Width="100px">
                                    <asp:ListItem Value="-1" Text="-- Select --"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Experian"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="TransUnion"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Call Credit"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="D&B"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="lblRight">Credit Score:</td>
                            <td>

                                <ig:WebNumericEditor ID="CreditScore0" runat="server" ValueText="0" DataMode="Int" NullText="0" Width="100px" MaxValue="1000" HorizontalAlign="Left">
                                </ig:WebNumericEditor>

                            </td>

                        </tr>
                        <tr>
                            <td class="lblRight">Oldest Trade:
                            </td>
                            <td>
                                <ig:WebDatePicker ID="OldestTrade" runat="server"
                                    NullText="" Width="100px" DisplayModeFormat="MM/dd/yyyy">
                                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                        SlideOpenDuration="1" />
                                </ig:WebDatePicker>
                            </td>

                            <td class="lblRight">No of Trades:</td>
                            <td>

                                <ig:WebNumericEditor ID="NoofTrades0" HorizontalAlign="Left" runat="server" DataMode="Int" ValueText="0" Width="100px"
                                    NullText="0">
                                </ig:WebNumericEditor>

                            </td>
                            <td class="lblRight">Notes:</td>
                            <td>
                                <asp:TextBox ID="txtNotes" TextMode="MultiLine" Width="100px" runat="server"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td colspan="6" align="center">
                                <asp:Button runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add" Enabled="false" Visible="false" />
                                <asp:Button runat="server" ID="btnSave" OnClick="btnSave_Click" Text="Save & Close" />
                                <asp:Button runat="server" ID="btnCancel" OnClick="btnClose_Click" Text="Cancel & Close" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6"></td>
                        </tr>
                        </table>
                </fieldset>
                    <fieldset>
                        <legend>Credit History</legend>
                    <table width="100%">

                        <tr>
                            <td colspan="6">
                                <asp:Panel runat="server" ID="pnlCreditHistory" Height="250px" Width="100%" ScrollBars="vertical">
                                    <asp:GridView runat="server" CssClass="mGrid" ID="grdOwner" Font-Names="Verdana" Font-Size="X-Small" PagerStyle-CssClass="pgr" AutoGenerateColumns="false"
                                        OnRowDataBound="grdOwner_RowDataBound" AlternatingRowStyle-CssClass="alt" ClientIDMode="Static" OnRowCommand="grdOwner_RowCommand">

                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-Width="10px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="creditID" runat="server" Text="Edit" CommandName="Select"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Creditdate" HeaderText="Credit Date">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="CreditType" HeaderText="Credit Type">
                                                <ItemStyle Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Creditscore" HeaderText="Credit Score">
                                                <ItemStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="NoofTrades" HeaderText="# of Trades">
                                                <ItemStyle Width="70px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="OldestTrade" HeaderText="Oldest Trade">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Notes" HeaderText="Notes">
                                                <ItemStyle Width="150px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UID" ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            No Records Found....
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6"></td>
                        </tr>
                    </table>
                        </fieldset>
                </asp:Panel>
            </Template>
        </ContentPane>
        <Header CaptionText="Credit History">
        </Header>
    </ig:WebDialogWindow>
    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="180px" Width="330px"
        Modal="True" InitialLocation="Centered" UseBodyAsParent="True" WindowState="hidden">
        <ContentPane>
            <Template>
                <table width="100%">
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="padding-left: 3px; padding-right: 3px;">
                            <asp:Label runat="server" ID="lbl"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button runat="server" ID="Button1" Text="Ok" OnClick="btnGet1_Click" />
                            <asp:Button runat="server" ID="btnOk" Text="Ok" OnClick="btnGet1_Click" Visible="false" />
                            <asp:Button runat="server" ID="btnNo" Text="Cancel" OnClick="btnNo_Click" />
                            <asp:Button runat="server" ID="btnGetOld" Text="Get existing report" OnClick="btnGetOld_Click" />

                            <script type="text/javascript">
                                // this prevents the submit button from being clicked multiple times.
                                $('#<%= Button1.ClientID %>').on("click", function (event) {
                                    $(this).attr('value', 'Processing...');
                                    $(this).unbind(event);

                                    <%= GetSubmitPostBack() %>;
                                });
                            </script>
                        </td>
                    </tr>
                </table>
            </Template>
        </ContentPane>
    </ig:WebDialogWindow>
</fieldset>
<br />
