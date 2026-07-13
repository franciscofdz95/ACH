<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true"
    Inherits="frmOnlineApps" Title="Online Apps" CodeBehind="frmOnlineApps.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAgent.ascx" TagName="wucAgent" TagPrefix="uc2" %>
<%@ Register Src="~/UserControls/wucAgentSelector.ascx" TagName="AgentSelector" TagPrefix="uc1" %>
<%@ MasterType VirtualPath="~/MasterPageHome.master" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        

    <div id="contentpage">
        <fieldset style="width:999px;">
            <legend>Online Apps Search</legend>
            <asp:Panel ID="pnlSearch" runat="server" Height="" Width="999px">
                <table width="100%">
                    <tr>
                        <td colspan="8">
                            <asp:Label runat="server" ID="lblError" SkinID="Error"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            Start Date:
                        </td>
                        <td>
                            <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                        </td>
                        <td class="lblRight">
                            End Date:
                        </td>
                        <td>
                            <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                    SlideOpenDuration="1" />
                            </ig:WebDatePicker>
                        </td>
                        <td class="lblRight">
                            DBA:
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessDBAName" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            MLE:
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessLegalName" runat="server" EnableViewState="False" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <%-- <td class="lblRight">
                            Partner:
                        </td>
                        <td>
                            <asp:DropDownList ID="AgentAgentID" runat="server" Width="125px">
                            </asp:DropDownList>
                        </td>--%>
                        <td class="lblRight">
                            Contact Name:
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessContact" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            ZID:
                        </td>
                        <td>
                            <asp:TextBox ID="MerchantID" runat="server" EnableViewState="False" Width="120px"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            Phone:
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessPhone" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            Email:
                        </td>
                        <td>
                            <asp:TextBox ID="BusinessEmailAddress" runat="server" Width="120px" EnableViewState="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="lblRight">
                            RetID:
                        </td>
                        <td>
                            <asp:TextBox ID="RetrievalID" runat="server" EnableViewState="False" Width="120px"></asp:TextBox>
                        </td>
                        <td class="lblRight">
                            Last Step:
                        </td>
                        <td>
                            <asp:DropDownList ID="TabStop" runat="server" Width="125px">
                                <asp:ListItem Value="-1">ALL</asp:ListItem>
                                <asp:ListItem Value="0">Business</asp:ListItem>
                                <asp:ListItem Value="1">Owners</asp:ListItem>
                                <asp:ListItem Value="3">Processing</asp:ListItem>
                                <asp:ListItem Value="4">Merchant Bank</asp:ListItem>
                                <asp:ListItem Value="5">Documents Upload</asp:ListItem>
                                <%--<asp:ListItem Value="5">Program Guide</asp:ListItem>
                                <asp:ListItem Value="6">Personal Guarantee</asp:ListItem>
                                <asp:ListItem Value="7">Application</asp:ListItem>
                                <asp:ListItem Value="6">Summary</asp:ListItem>--%>
                                <asp:ListItem Value="6">Completed</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            Agent DBA:
                        </td>
                        <td>
                            <asp:TextBox ID="AgentDBA" runat="server" Enabled="false" Width="120px"></asp:TextBox>
                            <asp:HiddenField ID="AgentUID" runat="server" />
                        </td>
                        <td class="lblRight">
                            Agent ID:
                        </td>
                        <td>
                            <asp:TextBox ID="AgentID" runat="server" Enabled="false" Width="75px"></asp:TextBox>
                            <asp:LinkButton OnClick="btnAgentSelect_Click" CausesValidation="false" ID="lbSelectAgent"
                                runat="server">Select</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <div>
                <center>
                    <table>
                        <tr>
                            <td>
                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                    AccessKey="h">
                                    <Appearance>
                                        <Image Url="~/Images/Check.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                            <td>
                                <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                    AccessKey="l">
                                    <Appearance>
                                        <Image Url="~/Images/delete.png" />
                                    </Appearance>
                                </igtxt:WebImageButton>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
        </fieldset>
        <br />
        <fieldset style="width:999px;">
            <legend>Search Results</legend>
            <asp:Panel ID="pnlRecords" runat="server" Height="" Width="999px" Visible="false">
                <table width="100%">
                    <tr>
                        <td class="lblLeft">
                            Page Size:
                            <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                <asp:ListItem Selected="True">10</asp:ListItem>
                                <asp:ListItem>25</asp:ListItem>
                                <asp:ListItem>50</asp:ListItem>
                                <asp:ListItem>100</asp:ListItem>
                                <asp:ListItem>250</asp:ListItem>
                                <asp:ListItem>500</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="lblRight">
                            <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text="Label"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False" Width="997px"
                                Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                                AlternatingRowStyle-CssClass="alt" DataKeyNames="MerchantAppUID" OnRowDataBound="grd_RowDataBound"
                                OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging" AllowSorting="True"
                                OnSorting="grd_Sorting" DataSourceID="odsApps">
                                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                    LastPageText="&raquo;" />
                                <Columns>
                                    <asp:TemplateField HeaderText="ZID" SortExpression="MerchantID">
                                        <ItemTemplate>
                                             <asp:HyperLink NavigateUrl='<%#  "~/SecureMerchantManagementForms/frmMerchantProfile.aspx?MerchantAppUID=" + Eval("MerchantAppUID") + "&Adding=false"  %>'
                                                            runat="server" ID="hypZID" Text='<%# Eval("MerchantID") %>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RetrievalID" SortExpression="RetrievalID">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnRetrievalID" runat="server" CommandName="RetrievalID"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="BusinessDBAName" HeaderText="DBA Name" SortExpression="BusinessDBAName"
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="BusinessLegalName" HeaderText="MLE" SortExpression="BusinessLegalName"
                                        ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="AgentID" HeaderText="Agent ID" SortExpression="AgentID"
                                        ItemStyle-Width="30px" />
                                    <asp:BoundField DataField="AgentDBA" HeaderText="Agent DBA" SortExpression="AgentDBA"
                                        ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="FeeName" HeaderText="Fee Profile" SortExpression="FeeName"
                                        ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="BusinessContact" HeaderText="Contact Name"
                                        ItemStyle-Width="60px" />
                                    <asp:BoundField DataField="BusinessEmailAddress" HeaderText="Email"
                                        ItemStyle-Width="35px" ItemStyle-Wrap="true" />
                                    <asp:BoundField DataField="BusinessPhone" HeaderText="Phone">
                                        <ItemStyle Width="30px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DateCreated" HeaderText="Date" DataFormatString="{0:MM-dd-yy HH:mm tt}"
                                        SortExpression="DateCreated">
                                        <ItemStyle Width="50px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Last Step" ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Literal ID="ltlLastStep" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="MerchantAppUID" Visible="false" />
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="odsApps" runat="server" SelectMethod="GetMerchantAppsOnlinePaging"
                                TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                SelectCountMethod="GetMerchantAppsOnlinePagingRowCount" StartRowIndexParameterName="CurrentPage"
                                OldValuesParameterFormatString="original_{0}" OnSelecting="odsApps_Selecting">
                                <SelectParameters>
                                    <asp:Parameter Name="prms" Type="Object" />
                                    <asp:Parameter Name="PageSize" Type="Int32" />
                                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                                    <asp:Parameter Name="ControlID" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;
                                        </td>
                                        <td align="right">
                                            Export:&nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rdExport" runat="server" RepeatColumns="2">
                                                <asp:ListItem Selected="true" Value="0">Current Page</asp:ListItem>
                                                <asp:ListItem Value="1">All Pages</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td align="right" style="width: 33%;">
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                No data...
            </asp:Panel>
            <br />
        </fieldset>
    </div>
    <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="700px" Width="800px"
        Modal="true" InitialLocation="centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <div style="margin: 5px 5px 5px 5px">
                    <ig:WebTab ID="WebTab1" runat="server" Height="100%" Width="100%">
                        <Tabs>
                            <ig:ContentTabItem runat="server" Text="Summary">
                                <Template>
                                    <fieldset class="Summary">
                                        <asp:DetailsView CssClass="SummaryTable" ID="dvApplication" AutoGenerateRows="false"
                                            runat="server">
                                            <FieldHeaderStyle CssClass="SummaryField" />
                                            <RowStyle CssClass="SummaryRow" />
                                            <AlternatingRowStyle CssClass="AlternatingSummaryRow" />
                                            <Fields>                                             
                                               <asp:TemplateField HeaderText="Business Contact">
                                                    <ItemTemplate>
                                                        <%# Eval("BusinessContact")%>
                                                        &nbsp;<%# Eval("BusinessContactLastName") %>                                                       
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="MLE" DataField="BusinessLegalName" />
                                                <asp:BoundField HeaderText="DBA" DataField="BusinessDBAName" />
                                                <asp:TemplateField HeaderText="Business Address">
                                                    <ItemTemplate>
                                                        <%# Eval("BusinessAddress") %>
                                                        ,
                                                        <%# Eval("BusinessCity") %>
                                                        ,
                                                        <%# Eval("BusinessState") %>
                                                        &nbsp;<%# Eval("BusinessZip") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mailing Address">
                                                    <ItemTemplate>
                                                        <%# Eval("BusinessMailingAddress")%>
                                                        ,
                                                        <%# Eval("BusinessMailingCity") %>
                                                        ,
                                                        <%# Eval("BusinessMailingState") %>
                                                        &nbsp;<%# Eval("BusinessMailingZip") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Business Phone" DataField="BusinessPhone" />
                                                <asp:BoundField HeaderText="Fax" DataField="BusinessFax" />
                                                <asp:BoundField HeaderText="Email" DataField="BusinessEmailAddress" />
                                                <asp:BoundField HeaderText="Website" DataField="BusinessWebsite" />
                                                <asp:BoundField HeaderText="Federal Tax ID" DataField="BusinessTaxID" />
                                                <asp:BoundField HeaderText="Bank" DataField="BankName" />
                                                <asp:BoundField HeaderText="Account Number" DataField="AccountNumber" />
                                                <asp:BoundField HeaderText="Routing Number" DataField="RoutingNumber" />
                                                <asp:BoundField HeaderText="Products / Services Sold" DataField="MerchantSells" />
                                                <asp:BoundField HeaderText="Avg. Monthly Processing Volume" DataField="TinfoAverageMonthlyVMCVolume"
                                                    DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Avg. Ticket" DataField="TinfoAverageVMCTicket" DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Highest Ticket" DataField="TinfoHighestTicketAmount"
                                                    DataFormatString="{0:0.00}" />
                                                <asp:BoundField HeaderText="Swiped Sales" DataField="TinfoStoreFrontSwipedPercent"
                                                    DataFormatString="{0:F}%" />
                                                <asp:BoundField HeaderText="Internet Sales" DataField="TinfoInterntPercent" DataFormatString="{0:F}%" />
                                                <asp:BoundField HeaderText="Mail Order Sales" DataField="TinfoMailOrderPercent" DataFormatString="{0:F}%" />
                                                <asp:BoundField HeaderText="Telephone Order Sales" DataField="TinfoTelephoneOrderPercent"
                                                    DataFormatString="{0:F}%" />
                                            </Fields>
                                        </asp:DetailsView>
                                    </fieldset>
                                </Template>
                            </ig:ContentTabItem>
                            <ig:ContentTabItem runat="server" Text="Owners">
                                <Template>
                                    <fieldset class="Summary">
                                        <legend>First Owner</legend>
                                        <asp:DetailsView CssClass="SummaryTable" ID="dvOwners1" AutoGenerateRows="false"
                                            runat="server">
                                            <FieldHeaderStyle CssClass="SummaryField" />
                                            <RowStyle CssClass="SummaryRow" />
                                            <AlternatingRowStyle CssClass="AlternatingSummaryRow" />
                                            <Fields>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <%# Eval("FirstName") %>
                                                        <%# Eval("LastName") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Social Security #" DataField="SSN" />
                                                <asp:BoundField HeaderText="Date of birth" DataField="DOB" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="Title" DataField="Title" />
                                                <asp:BoundField HeaderText="Ownership %" DataField="PercentOwnership" DataFormatString="{0:F}%" />
                                                <asp:BoundField HeaderText="Driver's License #" DataField="DriversLicense" />
                                                <asp:BoundField HeaderText="Driver's License State" DataField="DriversLicenseState" />
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%# Eval("Address1") %>
                                                        <%# Eval("Address2") %>
                                                        ,
                                                        <%# Eval("City") %>
                                                        ,
                                                        <%# Eval("State") %>
                                                        &nbsp;<%# Eval("Zip") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Home Phone" DataField="HomePhone" />
                                            </Fields>
                                        </asp:DetailsView>
                                    </fieldset>
                                    <fieldset id="fsSecondOwner" visible="false" class="Summary" runat="server">
                                        <legend>Second Owner</legend>
                                        <asp:DetailsView CssClass="SummaryTable" ID="dvOwners2" AutoGenerateRows="false"
                                            runat="server">
                                            <FieldHeaderStyle CssClass="SummaryField" />
                                            <RowStyle CssClass="SummaryRow" />
                                            <AlternatingRowStyle CssClass="AlternatingSummaryRow" />
                                            <Fields>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <%# Eval("FirstName") %>
                                                        <%# Eval("LastName") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Social Security #" DataField="SSN" />
                                                <asp:BoundField HeaderText="Date of birth" DataField="DOB" DataFormatString="{0:d}" />
                                                <asp:BoundField HeaderText="Title" DataField="Title" />
                                                <asp:BoundField HeaderText="Ownership %" DataField="PercentOwnership" DataFormatString="{0:F}%" />
                                                <asp:BoundField HeaderText="Driver's License #" DataField="DriversLicense" />
                                                <asp:BoundField HeaderText="Driver's License State" DataField="DriversLicenseState" />
                                                <asp:TemplateField HeaderText="Address">
                                                    <ItemTemplate>
                                                        <%# Eval("Address1") %>
                                                        <%# Eval("Address2") %>
                                                        ,
                                                        <%# Eval("City") %>
                                                        ,
                                                        <%# Eval("State") %>
                                                        &nbsp;<%# Eval("Zip") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Home Phone" DataField="HomePhone" />
                                            </Fields>
                                        </asp:DetailsView>
                                    </fieldset>
                                </Template>
                            </ig:ContentTabItem>
                            <ig:ContentTabItem runat="server" Text="Risk Checks">
                                <Template>
                                    <asp:DetailsView CssClass="SummaryTable" ID="dvDetails" AutoGenerateRows="false"
                                        runat="server">
                                        <FieldHeaderStyle CssClass="SummaryField" />
                                        <RowStyle CssClass="SummaryRow" />
                                        <AlternatingRowStyle CssClass="AlternatingSummaryRow" />
                                        <Fields>
                                            <asp:BoundField HeaderText="Device Status" DataField="io_ResultCode" />
                                            <asp:BoundField HeaderText="Device Reason" DataField="io_Reason" />
                                            <asp:BoundField HeaderText="DeviceID" DataField="io_DeviceID" />
                                            <asp:BoundField HeaderText="Country Match" DataField="mm_countryMatch" />
                                            <asp:BoundField HeaderText="Country Code" DataField="mm_countryCode" />
                                            <asp:BoundField HeaderText="High Risk Country" DataField="mm_highRiskCountry" />
                                            <asp:BoundField HeaderText="Distance from Billing" DataField="mm_distance" />
                                            <asp:BoundField HeaderText="IP Region" DataField="mm_ip_region" />
                                            <asp:BoundField HeaderText="IP City" DataField="mm_ip_city" />
                                            <asp:BoundField HeaderText="IP Latitude" DataField="mm_ip_latitude" />
                                            <asp:BoundField HeaderText="IP Longitude" DataField="mm_ip_longitude" />
                                            <asp:BoundField HeaderText="ISP" DataField="mm_ip_isp" />
                                            <asp:BoundField HeaderText="Org" DataField="mm_ip_org" />
                                            <asp:BoundField HeaderText="Anonymous Proxy " DataField="mm_anonymousProxy" />
                                            <asp:BoundField HeaderText="Proxy Score" DataField="mm_proxyScore" />
                                            <asp:BoundField HeaderText="Free Email" DataField="mm_freeMail" />
                                            <asp:BoundField HeaderText="High Risk Email" DataField="mm_carderEmail" />
                                            <asp:BoundField HeaderText="Score" DataField="mm_score" />
                                            <asp:BoundField HeaderText="Explanation" DataField="mm_explanation" />
                                            <asp:BoundField HeaderText="Risk Score" DataField="mm_riskScore" />
                                        </Fields>
                                        <EmptyDataTemplate>
                                            <asp:Label runat="server" ID="lblData" Text=" No Data..."></asp:Label>
                                        </EmptyDataTemplate>
                                    </asp:DetailsView>
                                </Template>
                            </ig:ContentTabItem>
                            <ig:ContentTabItem runat="server" Text="Documents">
                                <Template>
                                    <asp:GridView ID="grdDocuments" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                        AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer"
                                        OnRowCommand="grd_RowCommand" Font-Names="Segoe UI, Verdana, Arial, Sans-Serif"
                                        Font-Size="X-Small" DataKeyNames="DocID" OnRowDataBound="grdDocuments_RowDataBound">
                                        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                        <EmptyDataRowStyle Font-Bold="true" ForeColor="Black" BorderWidth="1px" BorderColor="Black" BorderStyle="Solid" />
                                        <EmptyDataTemplate>
                                            No Documents Founds.....
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:BoundField DataField="DocID" HeaderText="Doc ID" Visible="False" />
                                            <asp:TemplateField HeaderText="Document Name">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="hypOrigName" runat="server" Target="_blank" Text='<%# Eval("OrigName") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Group Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblGroupType" Text='<%# Eval("DocTypeGroupName") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="Type" Text='<%# Eval("DocTypeName") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:DropDownList runat="server" ID="ddpType">
                                                    </asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDescription" Text='<%#Eval("Description") %>'></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" />
                                            <asp:BoundField DataField="ContentSize" HeaderText="Size" ReadOnly="True" />
                                            <asp:BoundField DataField="DocDate" HeaderText="Document Date" DataFormatString="{0:MM/dd/yyy hh:mm:ss tt}"
                                                ReadOnly="True" />
                                            <asp:TemplateField HeaderText="Actions">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDel" runat="server" CommandName="Erase" Width="10px" Style="text-align: center;"
                                                        CausesValidation="false">
                                                        <asp:Image ID="Img6" runat="server" ImageUrl="~/Images/delete.png" ToolTip="Delete"
                                                            ImageAlign="middle" />
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="footer" />
                                        <PagerStyle CssClass="pgr" />
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>                                   
                                </Template>
                            </ig:ContentTabItem>
                        </Tabs>
                    </ig:WebTab>
                </div>
            </Template>
        </ContentPane>
        <Header CaptionText="Application">
        </Header>
    </ig:WebDialogWindow>
    <ig:WebDialogWindow ID="dlgAgent" runat="server" Height="500px" Width="700px" Modal="true"
        InitialLocation="Centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <uc2:wucAgent ID="grdAgent" runat="server" />
            </Template>
        </ContentPane>
        <Header CaptionText="Agents">
        </Header>
    </ig:WebDialogWindow>
    <br />

 
</asp:Content>
