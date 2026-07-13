<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucConditions" CodeBehind="wucConditions.ascx.cs" %>
<%@ Register Src="wucDocumentGrid.ascx" TagName="wucDocumentGrid" TagPrefix="uc1" %>
<%@ Register Src="wucNotes.ascx" TagName="wucNotes" TagPrefix="uc3" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Reference Control="~/UserControls/wucDocumentGrid.ascx" %>
<script language="javascript" type="text/javascript">
    function checkInfo(chkAllInfo, chkReceived) {
        if (!document.getElementById(chkReceived).checked)
            document.getElementById(chkAllInfo).checked = false;
    }
 
</script>
<asp:UpdatePanel runat="server" ID="pnlUpdate">
       <ContentTemplate>
        <asp:Label Visible="false" Text="No Conditions...<br/><br/>" ID="lblNoData" runat="server"></asp:Label>
        <asp:Panel runat="server" ID="pnlConditions">
            <br />
            <div class="title">
                &nbsp;&nbsp;Conditions
                <hr class="line" />
            </div>
            <div class="indentedcontent20">
                <table>
                  <%--  <tr>
                        <td>
                          </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkHistory" Enabled="true" runat="server" AutoPostBack="True" OnCheckedChanged="chkHistory_CheckedChanged"
                                Text="Show History" TextAlign="right" />  
                                <asp:CheckBox ID="ConditionalApprovalAP" style="display:none;" runat="server" Text="Conditional Approval - AP"
                                TextAlign="right" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox runat="server" Text="All Information Received (Move App to Received Status)"
                                ID="chkReceivedStatus" OnCheckedChanged="chkReceivedStatus_CheckedChanged" 
                                Style="vertical-align: text-top;" TextAlign="right" />
                                </td>
                    </tr>
                </table>
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label runat="server" ID="lblError" Font-Bold="true" Font-Size="Small"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <asp:Panel runat="server" ID="pnlHistory">
                            <td align="left">
                                Type:
                                <asp:DropDownList ID="cboType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboType_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Value="-1">All</asp:ListItem>
                                    <asp:ListItem Text="SS" Value="SS"></asp:ListItem>
                                    <asp:ListItem Text="CU" Value="CU"></asp:ListItem>
                                </asp:DropDownList>&nbsp;&nbsp;
                            </td>
                        </asp:Panel>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="false" Visible="false" OnRowDataBound="grd_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ConditionDetailID" HeaderText="ID">
                                        <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ConditionName" HeaderText="Condition(s)">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ReceivedInfo" HeaderText="AR Received">
                                        <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="EmailText" HeaderText="Description">
                                        <ItemStyle Width="70px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="CUDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}"
                                        HtmlEncode="false" HeaderText="Requested Date">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ARDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}"
                                        HtmlEncode="false" HeaderText="Received Date">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Type" HeaderText="Type">
                                        <ItemStyle Width="20px" />
                                    </asp:BoundField>
                                </Columns>
                            </asp:GridView>
                            <script type="text/javascript">

                                // this prevents the submit button from being clicked multiple times.
                                function callfunc(lnk) {
                                    <%-- DM-832 by Jorge --%>
                                    var txtDescription = $('[id$="_grdConditions"] tr[class="footer"] [id$="_Email"]');
                                    txtDescription.val($('<div />').text(txtDescription.val()).html());
                                    var txtComments = $('[id$="_grdConditions"] tr[class="footer"] [id$="_Comments"]');
                                    txtComments.val($('<div />').text(txtComments.val()).html());                                    

                                    code = $(lnk).attr('href').replace(/javascript:/, ''); // save postback function
                                    $(lnk).attr('href', '');
                                    if (code)
                                        $('#ContentPlaceHolder1_Conditions_grdConditions_lnkInsert').click();
                                    else
                                        return false;
                                }
                            </script>

                            <asp:GridView ID="grdConditions" runat="server" CssClass="mGrid" AutoGenerateColumns="False"
                                Width="99.5%" Font-Size="X-Small" Font-Names="verdana" OnRowDataBound="grdConditions_RowDataBound"
                                OnRowCommand="grdConditions_RowCommand" ShowFooter="true">
                                <FooterStyle CssClass="footer" />
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                <RowStyle VerticalAlign="Top" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID">
                                        <FooterTemplate>
                                         <asp:Button ID="lnkInsert" runat="server"  style="display:none;" CommandName="Insert"></asp:Button>
                                            <asp:LinkButton ID="lnkID" Text="Insert" CommandName="text" runat="server" OnClientClick='callfunc(this); return false;' />
                                            <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" CommandName="CancelCom"></asp:LinkButton>

                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblID" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Width="30px" />
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Condition(s)">
                                        <FooterTemplate>
                                            <asp:DropDownList runat="server" ID="ConditionName" Width="200px">
                                            </asp:DropDownList>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Width="200px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="160px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Required" Visible="False">
                                        <FooterTemplate>
                                            <asp:CheckBox runat="server" ID="NeedMore" Checked="true" />
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkNeedMore" Checked="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Received">
                                        <FooterTemplate>
                                            <asp:CheckBox runat="server" ID="Received" Enabled="false" />
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chkReceived" Checked="true" OnCheckedChanged="chkReceived_CheckedChanged"
                                                AutoPostBack="true" />
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="Email" Enabled="true" Width="99%" Height="80px" TextMode="multiline"
                                                Rows="5"></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtEmail" Enabled="true" Width="99%" Height="80px"
                                                TextMode="multiline" Rows="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comments">
                                        <FooterTemplate>
                                            <asp:TextBox runat="server" ID="Comments" Enabled="true" Width="99%" Height="80px"
                                                TextMode="multiline" Rows="5" ></asp:TextBox>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtComments" Enabled="true" Width="99%" Height="80px"
                                                TextMode="multiline" Rows="5"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Agent Note" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtAgentNote" Enabled="true" Width="99%" Height="80px"
                                                TextMode="multiline" Rows="5" Text='<%# Bind("AgentNote") %>' ReadOnly="true"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle Width="200px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CUDate" DataFormatString="{0:MM/dd/yyyy hh:mm:ss tt}"
                                        HtmlEncode="False" HeaderText="Date Created">
                                        <ItemStyle Width="75px" />
                                    </asp:BoundField>
                                    <asp:BoundField HtmlEncode="False" HeaderText="Received Date">
                                        <ItemStyle Width="75px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                            <asp:Literal runat="server" ID="litDocs"></asp:Literal>
                                            <uc1:wucDocumentGrid ID="WucDocumentGrid1" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div id="div" class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click" CommandArgument="grd">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click">Add New Condition</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                        </td>
                    </tr>
                </table>
            </div>
            <br />
             <%--   Making the section  invisible for TFS5995. Not removing the entire content as users might require it back--%>
            <asp:Panel ID="pnlEmailBody" runat="server" Visible="false">
                <div class="title">
                    &nbsp;&nbsp;Email Body
                    <hr class="line" />
                </div>
                <div class="indentedcontent20">
                    <asp:TextBox runat="server" ID="txtEmail" TextMode="multiLine" Rows="4" Width="99%"></asp:TextBox>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlCredit" runat="server" Visible="false">
                <asp:PlaceHolder runat="server" ID="pnlEmail" Visible="false">
                    <div class="title">
                        &nbsp;&nbsp;Resend Email
                        <hr class="line" />
                    </div>
                    <div class="indentedcontent20">
                        <asp:Label runat="server" ID="lblEmail" Text="Email Address: "></asp:Label>
                        <asp:TextBox runat="server" ID="txtEmailAdd" Width="150px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmailAdd"
                            Display="Dynamic" ErrorMessage="Please enter a valid email address" Text="*"
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>&nbsp;
                        <asp:Button runat="server" ID="btnResend" Text="Resend Email" Width="100px" OnClick="btnResend_Click" />
                    </div>
                </asp:PlaceHolder>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlNotes">
            <br />
         <%--   Making the section  invisible for TFS5995--%>
            <uc3:wucNotes ID="WucNotes1" runat="server" frmPage="1" Visible="false"/>
        </asp:Panel>
   </ContentTemplate>
    <triggers>
        <asp:PostBackTrigger ControlID="btnExpExcel" />
        <asp:PostBackTrigger ControlID="btnResend" />
    </triggers>
</asp:UpdatePanel>
