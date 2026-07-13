<%@ Page Language="C#" MasterPageFile="~/MasterPageMerchant.master" AutoEventWireup="true"
    Inherits="frmMerchantHistory" Title="Merchant History" CodeBehind="frmMerchantHistory.aspx.cs" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="../UserControls/wucBusinessInfo.ascx" TagName="wucBusinessInfo"
    TagPrefix="uc4" %>
<%@ MasterType VirtualPath="~/MasterPageMerchant.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="contentpage">    
        <asp:Panel ID="pnlGreenBanner" runat="server">
        <span class="ftrightGreen">Tilled Account</span>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlBanner"></asp:Panel>
        <asp:Panel runat="server" ID="pnlRollover"></asp:Panel>
        <asp:Panel ID="pnlTools" runat="server">
        </asp:Panel>
        <uc4:wucBusinessInfo ID="WucBusinessInfo1" runat="server" />
        <br />


        <asp:UpdatePanel ID="upChangelog" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel1" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                    <fieldset>
                        <legend>Change History</legend>
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboCHPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboCHPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblCHRecordCount" SkinID="CHRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                       <asp:GridView ID="grdChange" runat="server" AllowPaging="True" AutoGenerateColumns="false" 
                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr"
                            AlternatingRowStyle-CssClass="alt" DataKeyNames="ChangeHistoryKeyID" 
                            OnPageIndexChanging="grdChangeHistory_PageIndexChanging" AllowSorting="True"
                                OnSorting="grd_ChangeHistorySorting" DataSourceID="odsChangeHistory" ShowHeaderWhenEmpty="true">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                            <Columns>
                                <asp:TemplateField HeaderText="Field">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("ChangeHistoryField") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:DropDownList ID="ddlChangeType" runat="server" AutoPostBack="true" OnPreRender="ddlChangeType_PreRender" OnRowDataBound="grdChange_RowDataBound"
                                            OnSelectedIndexChanged="ddlChangeType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Bind("OldValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNameHeader2" runat="server" Text="Old Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNameHeader" runat="server" Text="New Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" SortExpression="ChangedDate"/>
                                <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                            </Columns>
                            <EmptyDataTemplate>
                                No changes for selected field.
                            </EmptyDataTemplate>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsChangeHistory" runat="server" SelectMethod="GetChangeHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetChangeHistoryPagingCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsChangeHistory_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>


        <asp:Panel ID="pnlCCHistory" runat="server">
            <fieldset>
                <legend>Credit Card Application Status History</legend>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboCCPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboCCPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblCCRecordCount" SkinID="CCRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Label runat="server" ID="lblStatus" Text="no data.." Visible="false"></asp:Label>
                        <asp:GridView ID="grd" runat="server" OnRowDataBound="grd_RowDataBound" OnPageIndexChanging="grdCC_PageIndexChanging" AllowSorting="True" OnSorting="grd_CCSorting" AutoGenerateColumns="false" Font-Names="Verdana" AllowPaging="true"
                            Font-Size="X-Small" CssClass="mGrid" DataKeyNames="ChangeHistoryKeyID" 
                             DataSourceID="odsCC" ShowHeaderWhenEmpty="true">
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                        
                        <columns>
                            <asp:BoundField DataField="Status" HeaderText="Status"/>
                            <asp:BoundField DataField="Changed By" HeaderText="Changed By"/>
                            <asp:BoundField DataField="Changed Date" HeaderText="Changed Date" SortExpression="Changed Date"/>
                        </columns>
                            </asp:GridView>
                         <asp:ObjectDataSource ID="odsCC" runat="server" SelectMethod="GetSearchStatusHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetSearchStatusHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsCC_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>

        <asp:Panel ID="pnlACHHistory" runat="server">
            <fieldset>
                <legend>ACH Application Status History</legend>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                         <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboACHHPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboACHHPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblACHHRecordCount" SkinID="ACHHRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Label runat="server" ID="Label1" Text="no data.." Visible="false"></asp:Label>
                        <asp:GridView ID="grd2" runat="server" OnRowDataBound="grd2_RowDataBound" OnPageIndexChanging="grdACHH_PageIndexChanging" AllowSorting="True" OnSorting="grd_ACHHSorting"  AutoGenerateColumns="false" Font-Names="Verdana" AllowPaging="true"
                            Font-Size="X-Small" CssClass="mGrid" DataKeyNames="ChangeHistoryKeyID" 
                             DataSourceID="odsACHH" ShowHeaderWhenEmpty="true">
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                        
                        <columns>
                            <asp:BoundField DataField="Status" HeaderText="Status"/>
                            <asp:BoundField DataField="Changed By" HeaderText="Changed By"/>
                            <asp:BoundField DataField="Changed Date" HeaderText="Changed Date" SortExpression="Changed Date"/>
                        </columns>
                            </asp:GridView>
                         <asp:ObjectDataSource ID="odsACHH" runat="server" SelectMethod="GetACHSearchStatusHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetACHSearchStatusHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsACHH_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>   
           
        <asp:Panel ID="pnlDocumentsHistory" runat="server">
            <fieldset>
                <legend>Documents Change History</legend>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboDocHPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboDocHPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblDocHRecordCount" SkinID="DocHRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:Label runat="server" ID="lblDocumentsStatus" Text="no data.." Visible="false"></asp:Label>
                        <asp:GridView ID="grd3" runat="server" OnRowDataBound="grd3_RowDataBound" OnPageIndexChanging="grdDocH_PageIndexChanging" AllowSorting="True" OnSorting="grd_DocHSorting"
                            AutoGenerateColumns="false" Font-Names="Verdana" AllowPaging="true" PageSize="10"
                            Font-Size="X-Small" CssClass="mGrid" DataKeyNames="ChangeHistoryKeyID" 
                             DataSourceID="odsDocH" ShowHeaderWhenEmpty="true">
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                           <columns>
                            <asp:BoundField DataField="Name" HeaderText="Name"/>
                               <asp:BoundField DataField="Old Document Type" HeaderText="Old Document Type"/>
                               <asp:BoundField DataField="New Document Type" HeaderText="New Document Type"/>
                               <asp:BoundField DataField="Old Description" HeaderText="Old Description"/>
                               <asp:BoundField DataField="New Description" HeaderText="New Description"/>
                               <asp:BoundField DataField="Deleted/Changed Date" HeaderText="Deleted/Changed Date" SortExpression="Deleted/Changed Date"/>
                            <asp:BoundField DataField="Deleted/Changed By" HeaderText="Deleted/Changed By"/>
                            <asp:BoundField DataField="Action" HeaderText="Action"/>
                        </columns>
                            </asp:GridView>
                         <asp:ObjectDataSource ID="odsDocH" runat="server" SelectMethod="GetSearchDocumentsChangeHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetSearchDocumentsChangeHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsDocH_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>
        
        <%--MD-2363 ini--%>
        <asp:UpdatePanel ID="RdrUpdatePanel" runat="server">
            <ContentTemplate>
                <asp:Panel ID="RdrMainPanel" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                    <fieldset>
                        <legend>RDR Rule Change History</legend>
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                    <asp:DropDownList ID="RdrPageSizeCombo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RdrPageSizeCombo_SelectedIndexChanged">
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem Selected="True">25</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>100</asp:ListItem>
                                        <asp:ListItem>250</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblRuleChangeHistoryRecordCount" SkinID="RDRRuleRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="RdrProductRulesGrid" runat="server" OnPageIndexChanging="RdrProductRulesGrid_PageIndexChanging" AllowSorting="True" OnSorting="RdrProductRulesGridSorting"
                            AutoGenerateColumns="false" Font-Names="Verdana" AllowPaging="true" PageSize="10"
                            Font-Size="X-Small" CssClass="mGrid" DataKeyNames="Id" 
                             DataSourceID="odsRdrProductRulesChangeHistory" ShowHeaderWhenEmpty="true"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                             <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                            <Columns>
                                <asp:TemplateField HeaderText="Id" ItemStyle-Width="50px">
                                    <ItemTemplate>
                                        <asp:Label ID="RdrProductRuleIDLabelCol" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:DropDownList ID="RdrProductFilterCol" runat="server" AutoPostBack="True"
                                            OnPreRender="RdrProductFilterCol_PreRender" 
                                            OnSelectedIndexChanged="RdrProductFilterCol_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                   
                                </asp:TemplateField>
                                <asp:BoundField DataField="Portal" HeaderText="Portal" >
                                <ItemStyle Width="20px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="FieldName" HeaderText="Field Name" >
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="Action" HeaderText="Action" >
                                <ItemStyle Width="20px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="OldValue" HeaderText="Old Value" >
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="NewValue" HeaderText="New Value" >
                                <ItemStyle Width="200px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" SortExpression="ChangedDate">
                                <ItemStyle Width="100px" />
                            </asp:BoundField>
                                <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" >
                                <ItemStyle Width="20px" />
                            </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsRdrProductRulesChangeHistory" runat="server" SelectMethod="GetProductRuleHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetProductRuleHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsRdrProductRulesChangeHistory_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--MD-2363 end--%>
        <%--Code added for PXP-8431 by koshlendra start--%>
        <asp:UpdatePanel ID="upRelationshipsCHangeHistory" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlRelatioshipsChangeHistory" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                    <fieldset>
                        <legend>Relationships Change History</legend>
                        <table width="100%">
                            <tr>
                                <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                </td>
                                <td class="lblRight">
                                    <asp:Label ID="lblRelationshipsHRecordCount" SkinID="RelationshipsHRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="grdRelationshipsChangeHistory" runat="server" OnPageIndexChanging="grdRelationshipsChangeHistory_PageIndexChanging"
                            AllowSorting="True" OnSorting="grd_RelationshipsChangeHistorySorting" AutoGenerateColumns="false" Font-Names="Verdana" AllowPaging="true" PageSize="10"
                            Font-Size="X-Small" CssClass="mGrid" DataKeyNames="ChangeHistoryKeyID"
                            DataSourceID="odsRelationshipsChangeHistory" ShowHeaderWhenEmpty="true"
                            PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt">
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                            <Columns>
                                <asp:TemplateField HeaderText="Field">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRelationShipsRecordId" runat="server" Text='<%# Bind("RelationShipRecordId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:DropDownList ID="ddlRelationShipsRecordId" runat="server" AutoPostBack="true" OnPreRender="ddlRelationShipsRecordId_PreRender" OnRowDataBound="grdRelationshipsChangeHistory_RowDataBound"
                                            OnSelectedIndexChanged="ddlRelationShipsRecordId_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Category" HeaderText="Category" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Bind("OldValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOldNameHeader" runat="server" Text="Old Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNewNameHeader" runat="server" Text="New Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" SortExpression="ChangedDate"/>
                                <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                                <asp:BoundField DataField="Action" HeaderText="Action" />
                            </Columns>
                            <EmptyDataTemplate>
                                No changes for selected field.
                            </EmptyDataTemplate>
                        </asp:GridView>
                             <asp:ObjectDataSource ID="odsRelationshipsChangeHistory" runat="server" SelectMethod="GetSelectRelationshipsChangeHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetSelectRelationshipsChangeHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsRelationshipsChangeHistory_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </asp:Panel> 
                </ContentTemplate>
            </asp:UpdatePanel>
        <%--Code added for PXP-8431 by koshlendra End--%>
        <%--Code added for PXP-8430 by koshlendra End--%>
         <asp:UpdatePanel ID="UpdatePanelEqChangeHistory" runat="server">
            <ContentTemplate>
                 <asp:Panel ID="pnlEquipmentsChangeHistory" Style="display: inline-block;" runat="server" Width="100%" Visible="true">
                         <fieldset>
                        <legend>Equipment Change History</legend>
                          <table width="100%">
                                <tr>
                                    <td class="lblLeft">Page Size:
                                        <asp:DropDownList ID="cboEHPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboEHPageSize_SelectedIndexChanged">
                                            <asp:ListItem>10</asp:ListItem>
                                            <asp:ListItem>15</asp:ListItem>
                                            <asp:ListItem>20</asp:ListItem>
                                            <asp:ListItem Selected="True">25</asp:ListItem>
                                            <asp:ListItem>50</asp:ListItem>
                                            <asp:ListItem>100</asp:ListItem>
                                            <asp:ListItem>250</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                    <asp:Label ID="lblEquipmentHRecordCount" SkinID="EquipmentHRecordCount" runat="server" Text="Label"></asp:Label>
                                </td>
                                </tr>
                            </table>  
                        <asp:GridView ID="grdEquipmentsChangeHistory" runat="server" AllowPaging="true" AutoGenerateColumns="false" 
                            Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataKeyNames="ChangeHistoryKeyID" OnPageIndexChanging="grdEquipmentsChangeHistory_PageIndexChanging" AllowSorting="True"
                             OnSorting="grd_EquipmentsChangeHistorySorting" DataSourceID="odsEquipmentsChangeHistory" ShowHeaderWhenEmpty="true">                               
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;" LastPageText="&raquo;" />
                            <Columns>
                                <asp:TemplateField HeaderText="Field">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEquipmentRecordId" runat="server" Text='<%# Bind("EquipmentRecordId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate> 
                                         <asp:DropDownList ID="ddlEquipmentsRecordId" runat="server" AutoPostBack="true" OnPreRender="ddlEquipmentsRecordId_PreRender" OnRowDataBound="grdEquipmentsChangeHistory_RowDataBound"
                                            OnSelectedIndexChanged="ddlEquipmentsRecordId_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FieldName" HeaderText="Field Name" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOldValue" runat="server" Text='<%# Bind("OldValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblOldNameHeader" runat="server" Text="Old Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblNewValue" runat="server" Text='<%# Bind("NewValue") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblNewNameHeader" runat="server" Text="New Value"></asp:Label>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ChangedDate" HeaderText="Changed Date" SortExpression="ChangedDate"/>
                                <asp:BoundField DataField="ChangedBy" HeaderText="Changed By" />
                                <asp:BoundField DataField="Action" HeaderText="Action" />
                            </Columns>
                            <EmptyDataTemplate>
                                No changes for selected field.
                            </EmptyDataTemplate>
                        </asp:GridView>                            
                           <asp:ObjectDataSource ID="odsEquipmentsChangeHistory" runat="server" SelectMethod="GetSelectEquipmentsChangeHistoryPaging"
                            TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                            SelectCountMethod="GetSelectEquipmentsChangeHistoryCount" StartRowIndexParameterName="CurrentPage"
                            OldValuesParameterFormatString="original_{0}" OnSelecting="odsEquipmentsChangeHistory_Selecting">
                            <SelectParameters>
                                <asp:Parameter Name="prms" Type="Object" />
                                <asp:Parameter Name="PageSize" Type="Int32" />
                                <asp:Parameter Name="CurrentPage" Type="Int32" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </asp:Panel>
            </ContentTemplate>
         </asp:UpdatePanel>
    </div>
</asp:Content>
