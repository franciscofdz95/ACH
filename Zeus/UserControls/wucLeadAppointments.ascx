<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucLeadAppointments.ascx.cs" Inherits="ZeusWeb.UserControls.wucLeadAppointments" %>
<script type="text/Javascript">

    function CheckActive(lnk) {
        var lnk1 = document.getElementById(lnk);

        if (lnk.src.indexOf('checkbox')) {
            lnk.src = "../Images/Uncheck.JPG";
            if (confirm('Are you sure, you want to delete the appointment?') == false) {
                lnk.src = "../Images/checkbox.JPG";
                return false;
            }
            return true;
        }
        else {
            lnk.src = "../Images/checkbox.JPG";
            return false;
        }
    }

    function checkActive(chk) {
        return;
    }
</script>
<fieldset>
    <legend>
        <asp:Label ID="lblTitle" runat="server" Text="Leads Followup"></asp:Label></legend>

    <asp:UpdatePanel runat="server" ID="pnlDate">
        <ContentTemplate>
            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="">
                <table width="100%">
                    <tr>
                        <td style="text-align: left;">Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="250">250</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
            </asp:DropDownList>
                        </td>
                        <td align="right">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>

                        <td colspan="2">
                            <asp:GridView ID="grdAppointments" Width="100%" AutoGenerateColumns="False" runat="server"
                                OnRowDataBound="grdAppointments_RowDataBound" DataKeyNames="AppointmentID,LeadID,LeadUID" OnRowCommand="grdAppointments_RowCommand"
                                Font-Names="Verdana" OnPageIndexChanging="grdAppointments_PageIndexChanging"
                                Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                AllowPaging="true">
                                <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ID" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkID" runat="server" CausesValidation="false" CommandName="ID"></asp:LinkButton>
                                            <asp:HiddenField ID="hdnRecordMode" runat="server" Value="New" />
                                        </ItemTemplate>
                                        <ItemStyle Width="35px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnLeadID" runat="server" CommandName="ID"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="10px" />
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="DBA" DataField="DBAName">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Start Date" DataField="StartDateTime" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="End Date" DataField="EndDateTime" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Notes" HeaderText="Notes"> <ItemStyle Width="400px" /></asp:BoundField>

                                    <%--                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Created" DataField="UserCreated">
                                            <ItemStyle Width="60px" />
                                        </asp:BoundField>--%>
                                    <asp:BoundField HeaderText="Lead Status" DataField="LeadStatus">
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="lnkActive" ImageUrl="~/images/checkbox.jpg" runat="server" CommandName="Active"
                                                OnClientClick="return CheckActive(this);" ToolTip="Active" CausesValidation="false"></asp:ImageButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="35px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="AppointmentID" Visible="False" />
                                </Columns>
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsGetUserLeadAppointments" runat="server" SelectMethod="GetUserLeadAppointments"
                                EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetUserLeadAppointmentsCount"
                                StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                                OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                </SelectParameters>
                            </asp:ObjectDataSource>

                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="">
                No data...
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</fieldset>
