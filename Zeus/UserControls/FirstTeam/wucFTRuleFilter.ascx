<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFTRuleFilter.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucFTRuleFilter" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<table width="100%">
    <tr>
        <td style="text-align: left;">
            Page Size:
            <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="25">25</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="250">250</asp:ListItem>
                <asp:ListItem Value="500">500</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="text-align: right;">
            Total Record Count:
            <asp:Label runat="server" ID="lblRowCount">0</asp:Label>
        </td>
    </tr>
</table>
<asp:GridView ID="GridView1" CssClass="mGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
    OnRowCommand="grd_RowCommand" OnPreRender="GridView1_PreRender" AlternatingRowStyle-CssClass="alt"
    PagerStyle-CssClass="pgr" OnPageIndexChanging="GridView1_PageIndexChanging" OnRowDataBound="GridView1_RowDataBound"
    OnSorting="grd_Sorting" EnableModelValidation="True">
    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
    <Columns>
        <asp:TemplateField HeaderText="Global Enabled" ItemStyle-Width="60px">
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="cbGlobalEnabled" />
            </ItemTemplate>
            <ItemStyle Width="60px"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Merchant Enabled" ItemStyle-Width="60px">
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="cbMerchantEnabled" AutoPostBack="true" OnCheckedChanged="cbMerchantEnabled_CheckedChanged" />
                <asp:HiddenField runat="server" ID="hidMerchantID" Value='<%# Bind("MerchantID") %>' />
                <asp:HiddenField runat="server" ID="hidMRuleID" Value='<%# Bind("MRuleID") %>' />
            </ItemTemplate>
            <ItemStyle Width="60px"></ItemStyle>
        </asp:TemplateField>
        <asp:BoundField HeaderText="Rule Name" DataField="RuleNameNice" ItemStyle-Width="130px">
            <ItemStyle Width="130px"></ItemStyle>
        </asp:BoundField>
        <asp:TemplateField HeaderText="ZID" SortExpression="MERCHANTID">
            <ItemTemplate>
                <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                    runat="server" ID="hypZID" Text='<%# Eval("MerchantID") %>'></asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="DBA Name" SortExpression="DBANAME">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Bind("BusinessDBAName") %>' ID="lblBusinessDBAName"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="MLE" SortExpression="LEGALNAME">
            <ItemTemplate>
                <asp:Label runat="server" Text='<%# Bind("BusinessLegalName") %>' ID="lblBusinessLegalName"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="Label1" runat="server" Text='<%# Bind("FTRepUsername") %>'></asp:Label>
            </ItemTemplate>
            <HeaderTemplate>
                PS Rep
                <asp:DropDownList runat="server" ID="ddlFTRep" AutoPostBack="true" Visible="false"
                    OnSelectedIndexChanged="ddlFTRep_SelectedIndexChanged">
                </asp:DropDownList>
            </HeaderTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Start Date" SortExpression="STARTDATE">
            <ItemTemplate>
                <ig:WebDatePicker ID="tbStartDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                    OnTextChanged="tbStartDate_TextChanged" DataMode="Date" Width="100px" BackColor="#EFF3FF"
                    BorderStyle="Solid" BorderWidth="1px">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />
                </ig:WebDatePicker>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="End Date" SortExpression="ENDDATE">
            <ItemTemplate>
                <ig:WebDatePicker ID="tbEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                    OnTextChanged="tbEndDate_TextChanged" DataMode="Date" Width="100px" BackColor="#EFF3FF"
                    BorderStyle="Solid" BorderWidth="1px">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                        SlideOpenDuration="1" />
                </ig:WebDatePicker>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Use Default Params">
            <ItemTemplate>
                <asp:CheckBox OnCheckedChanged="cbUseDefaultParams_CheckedChanged" runat="server"
                    Checked='<%# Bind("UseDefaultParams") %>' AutoPostBack="true" ID="cbUseDefaultParams" />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Params">
            <ItemTemplate>
                <asp:Panel runat="server" ID="pnlGrid">
                    <asp:GridView runat="server" ID="grdParams" Width="100%" AutoGenerateColumns="False"
                        EnableModelValidation="True" ShowHeader="False">
                        <Columns>
                            <asp:BoundField DataField="ParamName" ItemStyle-Width="50%" HeaderText="ParamName" />
                            <asp:TemplateField HeaderText="ParamValue">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hidMerchantID" Value='<%#  Bind("MerchantID")  %>' />
                                    <asp:HiddenField runat="server" ID="hidMRuleParamID" Value='<%#  Bind("MRuleParamID")  %>' />
                                    <asp:HiddenField runat="server" ID="hidMRuleID" Value='<%#  Bind("MRuleID")  %>' />
                                    <asp:TextBox ID="tbParamValue" runat="server" AutoPostBack="true" Text='<%# Bind("ParamValue") %>'
                                        OnTextChanged="tbParamValue_TextChanged"></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle Width="50%" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField Visible="false">
            <ItemTemplate>
                <asp:LinkButton ID="lbSnooze" runat="server" CommandName="snooze" OnClick="lbSnooze_Click">Snooze</asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <PagerStyle CssClass="pgr"></PagerStyle>
</asp:GridView>
<asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetMRuleMerchantPaging"
    EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetMRuleMerchantPagingCount"
    StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
    OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
    <SelectParameters>
        <asp:Parameter Name="prms" Type="Object" />
        <asp:Parameter Name="PageSize" Type="Int32" />
        <asp:Parameter Name="CurrentPage" Type="Int32" />
    </SelectParameters>
</asp:ObjectDataSource>
