<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmTicketTemplates.aspx.cs" MasterPageFile="~/MasterPageReports.master" Title="Ticket Template Management" Inherits="frmTicketTemplates" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucTicketTemplate.ascx"  TagName="wucTicketTemplate" TagPrefix="uc1"%>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                <div class="title">
                &nbsp;&nbsp;Ticket Template Management
                <hr class="line" />
            </div>
    <div>
        <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CausesValidation="False"
            AccessKey="a" OnClick="btnAdd_Click">
            <Appearance>
                <Image Url="~/Images/add2.png" />
            </Appearance>
        </igtxt:WebImageButton>
        <br /><br />
    </div>
    <div>
        <fieldset>
            <legend>Ticket Templates</legend>
            <asp:Panel runat="server" ID="pnlPageSize" Visible="false">
            <table style="width: 100%">
                <tr>
                    <td class="lblLeft" style="vertical-align: bottom">Page Size:
                        <asp:DropDownList ID="cboPageSize2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize2_SelectedIndexChanged">
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem Selected="True">10</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>100</asp:ListItem>
                            <asp:ListItem>250</asp:ListItem>
                            <asp:ListItem>500</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="lblRight">Total Records Found:
                        <asp:Label ID="lblRowCount" runat="server" Text="">0</asp:Label>
                    </td>
                </tr>
            </table>
            </asp:Panel>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PageSize="10" DataSourceID="ods"
                OnRowCommand="GridView1_RowCommand" AllowPaging="true" AllowSorting="true" OnSorting="GridView1_Sorting"
                AlternatingRowStyle-CssClass="alt" PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" OnPageIndexChanging="GridView1_PageIndexChanging"
                Font-Names="verdana" Font-Size="X-Small" OnRowDataBound="GridView1_RowDataBound" DataKeyNames="TicketTemplateID">
                <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="«" LastPageText="»" />                
                <EmptyDataTemplate>
                    No Records Found....
                </EmptyDataTemplate>
                <Columns>                    
                    <asp:TemplateField HeaderText="Template ID" SortExpression="TicketTemplateID">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="lbView" CommandName="view" CommandArgument='<%# Eval("TicketTemplateID")%>' Text='<%# Eval("TicketTemplateID")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TicketTemplateName" HeaderText="Title" SortExpression="TicketTemplateName" />
                    <asp:BoundField HeaderText="Description" DataField="Description" SortExpression="Description" />
                    <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" />
                    <asp:BoundField HeaderText="Category" DataField="Category" SortExpression="Category" />
                    <asp:BoundField DataField="Subcategory" HeaderText="Subcategory" SortExpression="Subcategory" />
                    <asp:BoundField HeaderText="Active" DataField="Active" SortExpression="Active" />
                    <asp:BoundField DataField="DateUpdated" DataFormatString="{0:MM/dd/yyy}" HeaderText="Date Updated" SortExpression="DateUpdated" />
                    <asp:BoundField DataField="UserUpdated" HeaderText="Updated By" SortExpression="UserUpdated" />
                </Columns>
                <EmptyDataTemplate>
                    No Records Found..
                </EmptyDataTemplate>
                <FooterStyle CssClass="footer" />
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
            </asp:GridView>
            <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetTicketTemplatesPaging"
                EnablePaging="True" MaximumRowsParameterName="PageSize" SelectCountMethod="GetTicketTemplatesPagingCount"
                StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                OnSelecting="ods_Selecting" TypeName="DataMerchantAppPaging">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                    <asp:Parameter Name="PageSize" Type="Int32" />
                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
        <ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="580px" Width="600px" 
            Modal="true" InitialLocation="Centered" UseBodyAsParent="false" WindowState="Hidden" MaintainLocationOnScroll="true">
            <ContentPane>
                <Template>                  
                  <uc1:wucTicketTemplate ID="TicketTemplate" runat="server" />
                    <br />
                </Template>
            </ContentPane>
             <Header CaptionText="Ticket Template Management" CloseBox-Visible="false">
                            </Header>
        </ig:WebDialogWindow>
    </div>
</asp:Content>
