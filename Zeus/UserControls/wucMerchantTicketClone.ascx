<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucMerchantTicketClone" CodeBehind="wucMerchantTicketClone.ascx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<style>
    .ps-modal {
        position: fixed; /* Stay in place */
        z-index: 1; /* Sit on top */
        padding-top: 97px; /* Location of the box */
        left: 0;
        top: 0;
        width: 100%; /* Full width */
        height: 100%; /* Full height */
        overflow: auto; /* Enable scroll if needed */
        background-color: rgb(0,0,0); /* Fallback color */
        background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
    }

    .ps-modal-content {
        background-color: #fefefe;
        margin: auto;
        padding: 19px;
        border: 1px solid #888;
        width: 80%;
    }
</style>

<script type="text/javascript" language="javascript">
    function validate(btn, txt1, txt2) {
        var btn1 = document.getElementById(btn);
        var txtA = document.getElementById(txt1);
        var txtB = document.getElementById(txt2);
        var s;
        if (txtA.value != " ") {
            s = parseInt(txtA.value);
            if (s < 1) {
                alert('Enter valid ACHID');
                return false;
            }
        }
        if (txtB.value != " ") {
            s = parseInt(txtB.value);
            if (s < 1) {
                alert('Enter valid MerchantID');
                return false;
            }
        }
    }

    function CheckNumeric() {
        var key;
        key = event.which ? event.which : event.keyCode;
        if ((key >= 48 && key <= 57) || key == 13) {
            event.returnValue = true;
        }
        else {
            alert("Please enter Numeric only");
            event.returnValue = false;
        }
    }

</script>


<div class="dialog">
    <fieldset>
        <legend>Merchant List</legend>
        <table width="100%">
            <tr>
                <td class="lblRight">DBA:</td>
                <td>
                    <asp:TextBox runat="server" ID="txtDBA" Text="" Width="80px" EnableViewState="False"
                        TabIndex="1"></asp:TextBox></td>
                <td class="lblRight">ZID:</td>
                <td>
                    <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="70px"
                        MaxLength="7" TabIndex="2" ValidationGroup="grpMerchant"></asp:TextBox></td>
                <td class="lblRight">MID:</td>
                <td>
                    <asp:TextBox ID="SettlePlatformMid" runat="server" EnableViewState="False" Width="90px"
                        TabIndex="7"></asp:TextBox></td>
                <td>Partner DBA:</td>
                <td>
                    <asp:TextBox ID="AgentDBA" runat="server" EnableViewState="False" TabIndex="7" Width="90px"></asp:TextBox></td>
            </tr>
            <tr>
                <td class="lblRight">Legal:</td>
                <td>
                    <asp:TextBox ID="BusinessLegalName" runat="server" Width="80px" TabIndex="5" EnableViewState="False"></asp:TextBox></td>
                <td class="lblRight">AID:</td>
                <td>
                    <asp:TextBox ID="AchID" runat="server" ValidationGroup="grpMerchant" TabIndex="6"
                        EnableViewState="False" MaxLength="7" Width="70px"></asp:TextBox></td>
                <td class="lblRight">Bank:</td>
                <td>
                    <asp:DropDownList ID="MerchantAppTypeUID" runat="server" Width="95px" TabIndex="3">
                    </asp:DropDownList></td>
                <td>Partner ID:</td>
                <td>
                    <asp:TextBox ID="AgentID" runat="server" EnableViewState="False" TabIndex="7" Width="90px"></asp:TextBox>

                </td>
            </tr>
            <tr>
                <td>FMAID:</td>
                <td>
                    <asp:TextBox ID="FMAID" runat="server" EnableViewState="False" Width="80px" MaxLength="15"></asp:TextBox></td>
                <td>MLE:</td>
                <td>
                    <asp:TextBox ID="MLEName" runat="server" EnableViewState="False" Width="70px"></asp:TextBox></td>
                <td colspan="4">&nbsp;</td>

            </tr>
            <tr>
                <td colspan="8" style="height: 10px;"></td>
            </tr>
            <tr>
                <td colspan="8" align="center">
                    <asp:Button runat="server" ID="btnSearch" Text="Search" CausesValidation="false"
                        Width="70px" TabIndex="8" OnClick="btnSearch_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnCloneTicket" Text="Clone Ticket" Width="90px" Enabled="false" CausesValidation="false"
                        TabIndex="5" OnClick="btnCloneTicket_Click"></asp:Button>
                    <asp:Button runat="server" ID="btnClose" Text="Close" Width="50px" CausesValidation="false"
                        TabIndex="5" OnClick="btnClose_Click"></asp:Button>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="lblRight" colspan="8">
                    <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                        <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            Font-Names="Verdana" Font-Size="X-Small" OnPageIndexChanging="grd_PageIndexChanging"
                            OnRowDataBound="grd_RowDataBound" DataSourceID="odsTransactions" OnRowCommand="grd_RowCommand"
                            AllowSorting="True" OnSorting="grd_Sorting">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfMerchantAppUID" runat="server" Value='<%# Eval("MerchantAppUID") %>' />
                                        <asp:CheckBox ID="chkCloneTicket" runat="server" OnCheckedChanged="chkCloneTicket_Click" AutoPostBack="true" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle Width="10px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                    <ItemTemplate>                                        
                                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false&PostBackURL=" + PostBackURL  %>'
                                            runat="server" ID="hypZID" Target="_blank"  Text='<%#Eval("ID") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="45px" />
                                </asp:TemplateField>                              
                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SettlePlatformMid" HeaderText="MID" SortExpression="SettlePlatformMid">
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AgentFirstLastName" HeaderText="Partner Name" SortExpression="AgentFirstLastName">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="FMAID" HeaderText="FMAID" SortExpression="FMAID">
                                    <ItemStyle Width="45px" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Bank" HeaderText="Acq. Bank" SortExpression="Bank">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="ZID Status" SortExpression="Status">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ACHStatus" HeaderText="ACH/DD Status" SortExpression="ACHStatus">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <FooterStyle CssClass="footer" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsTransactions" runat="server" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            OnSelecting="odsTransactions_Selecting" StartRowIndexParameterName="CurrentPage"
                            TypeName="DataMerchantAppPaging">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                                <asp:Parameter Name="ControlID" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </asp:Panel>
                    <asp:Panel ID="pnlNoRecords" runat="server" Height="300px" Width="" Visible="true">
                        No data...
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="lblRight" colspan="8">
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:Panel ID="Panel1" runat="server" Height="" Width="" Visible="true">
                        <asp:GridView ID="grd2" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                            CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            Font-Names="Verdana" Font-Size="X-Small"
                            OnRowDataBound="grd2_RowDataBound"
                            AllowSorting="False">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hfMerchantAppUID" runat="server" Value='<%# Eval("MerchantAppUID") %>' />
                                        <asp:CheckBox ID="chkCloneTicket" runat="server" Enabled="false"/>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10px" />
                                    <ItemStyle Width="10px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ZID" SortExpression="ID">
                                    <ItemTemplate>                                        
                                        <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false&PostBackURL=" + PostBackURL  %>'
                                            runat="server" ID="hypZID" Target="_blank"  Text='<%#Eval("ID") %>'></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="45px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName">
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SettlePlatformMid" HeaderText="MID" SortExpression="SettlePlatformMid">
                                    <ItemStyle Width="70px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="AgentFirstLastName" HeaderText="Partner Name" SortExpression="AgentFirstLastName">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <%--<asp:BoundField DataField="FMAID" HeaderText="FMAID" SortExpression="FMAID">
                                    <ItemStyle Width="45px" />
                                </asp:BoundField>--%>
                                <asp:BoundField DataField="Bank" HeaderText="Acq. Bank" SortExpression="Bank">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="ZID Status" SortExpression="Status">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="ACHStatus" HeaderText="ACH/DD Status" SortExpression="ACHStatus">
                                    <ItemStyle Width="225px" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle CssClass="pgr" />
                            <FooterStyle CssClass="footer" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </fieldset>

    <asp:UpdatePanel runat="server" class="ps-modal" ID="dlgOK" Visible="false">
        <ContentTemplate>
            <asp:UpdateProgress ID="updateProgress" runat="server">
                <ProgressTemplate>
                    <div style="position: fixed; text-align: center; height: 100%; width: 100%; top: 0; right: 0; left: 0; z-index: 9999999; background-color: #000000; opacity: 0.7;">
                        <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="~/images/ajax-loader.gif" AlternateText="Loading ..." ToolTip="Loading ..." Style="padding: 10px; position: fixed; top: 45%; left: 50%;" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel runat="server" ID="pnlOK" class="ps-modal-content" Style="width: 500px; height: 467px;">
                <fieldset>
                    <legend>Issue</legend>
                    <div class="item" style="margin-right: 5px;">
                        <asp:TextBox ID="txtIssue" runat="server" TextMode="MultiLine" Width="100%" Height="100px" TabIndex="123"></asp:TextBox>
                    </div>
                    <asp:Label runat="server" ID="issueError" Visible="false" Style="color: red"> </asp:Label>
                </fieldset>
                <fieldset>
                    <legend>Due Date</legend>
                    <div class="item">
                        <div class="tiinput">
                            <asp:TextBox ID="txtDueDate" runat="server" Width="83px" OnTextChanged="CloneDueDate_ValueChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:DropDownList ID="ddlDueDateTime" runat="server" Width="75px" AutoPostBack="true" OnSelectedIndexChanged="CloneDueDate_ValueChanged">
                            </asp:DropDownList>
                            <span class="required">*</span>
                            <cc1:CalendarExtender ID="CloneCalDueDate" runat="server" Enabled="True" PopupButtonID="CloneImgDueDate"
                                TargetControlID="txtDueDate" Format="MM/dd/yyyy">
                            </cc1:CalendarExtender>
                            <asp:ImageButton ID="CloneImgDueDate" runat="Server" AlternateText="Click to show calendar"
                                CausesValidation="false" ImageUrl="~/images/Calendar_scheduleHS.png" />
                        </div>
                        <asp:Label runat="server" ID="dueDateError" Visible="false" Style="color: red"> </asp:Label>
                    </div>
                </fieldset>
                <fieldset>
                    <legend>Clone Options</legend>
                    <p>Select 'Notes' to copy the existing ticket notes to each ticket clone</p>
                    <div class="item">
                        <div class="tilabel">
                            Notes:
                        <asp:CheckBox ID="chbNotesClone" runat="server" AutoPostBack="true"></asp:CheckBox>
                        </div>
                    </div>
                    <p>Select 'Attachments' to copy the existing Attachments notes to each ticket clone</p>
                    <div class="item">
                        <div class="tilabel">
                            Attachments:
                        <asp:CheckBox ID="chbAttachmentsClone" runat="server" AutoPostBack="true"></asp:CheckBox>
                        </div>
                    </div>
                </fieldset>
                <p>Are you sure you want to clone tickets with the ticket issue above?</p>
                <div style="float: right; padding: 3px; margin: 5px;">
                    <asp:Button runat="server" ID="btnOK" OnClick="btnOk_Click" Text="Yes" />
                    <asp:Button runat="server" ID="btnCancel" OnClick="btnCancel_Click" Text="No" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlResult" class="ps-modal-content" Visible="false" Style="width: 500px; height: 467px;">
                <fieldset>
                    <legend>Tickets Cloned Successfully</legend>
                    <p>Tickets Created</p>
                    <asp:GridView ID="grdResult" runat="server" AllowPaging="False" AutoGenerateColumns="False"
                        CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                        Font-Names="Verdana" Font-Size="X-Small"
                        OnRowDataBound="grdResult_RowDataBound"
                        AllowSorting="False">
                        <Columns>
                            <asp:TemplateField HeaderText="ZID">
                                <ItemTemplate>                                        
                                    <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false&PostBackURL=" + PostBackURL  %>'
                                        runat="server" ID="hypZID" Target="_blank"  Text='<%#Eval("ID") %>'></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="45px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name">
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE">
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Ticket ID">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="labTicketID" CssClass="fakea zeustooltip"><%# Eval("TicketID") %></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="70px" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle CssClass="alt" />
                    </asp:GridView>
                </fieldset>
                <div style="float: right; padding: 3px; margin: 5px;">
                    <asp:Button runat="server" ID="btnDone" OnClick="btnDone_Click" Text="Close" />
                </div>
            </asp:Panel> 
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel runat="server" class="ps-modal" ID="mdError" Visible="false">
        <asp:Panel runat="server" ID="mdPanel1" class="ps-modal-content" Style="width: 300px; height: 50px;">
            <asp:Label runat="server" ID="mdMsg"></asp:Label>
            <div style="text-align: center; padding: 10px; margin: 10px;">
                <asp:Button runat="server" ID="mdClose" OnClick="mdClose_Click" Text="Close" />
            </div>
        </asp:Panel>
    </asp:Panel>
</div>
