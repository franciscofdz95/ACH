<%@ Page Language="C#" MasterPageFile="~/MasterPageAgent.master" AutoEventWireup="true"
    Inherits="frmSearchAgents" Title="Search Agent" CodeBehind="frmSearchAgents.aspx.cs" %>

<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" language="javascript">
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
     <style type="text/css">  
     input, select, textarea
        {
            box-sizing: border-box;
            -moz-box-sizing: border-box;
            -webkit-box-sizing: border-box;
        }
      </style>
    <div id="contentpage">
        <table width="100%">
            <tr>
                <td>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                        <fieldset>
                            <legend>Agent Search</legend>                           
                            <table cellspacing="0" width="100%">
                                <tr>
                                    <td class="lblRight">
                                        Last Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentLastName" runat="server" Width="125px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        Agent DBA:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentDBA" runat="server" EnableViewState="False" Width="125px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        Status:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="StatusUID" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Tax Reg #:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentTaxID" runat="server" EnableViewState="False" Width="125px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        First Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentFirstName" runat="server" EnableViewState="False" Width="125px"></asp:TextBox>
                                    </td>
                                    <td class="lblRight">
                                        Agent ID:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentUID" runat="server" EnableViewState="False" Width="125px"></asp:TextBox>
                                        <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="AgentUID"
                                            Display="None" ErrorMessage="Invalid ID" MaximumValue="2147483647" MinimumValue="1"
                                            Type="Integer"></asp:RangeValidator>
                                    </td>
                                    <td class="lblRight">
                                        Phone #:
                                    </td>
                                    <td>
                                        <ig:WebMaskEditor ID="AgentPhone" runat="server" InputMask="##############################" PromptChar=' ' ShowMaskOnFocus="False" Width="125px">
                                        </ig:WebMaskEditor>
                                    </td>
                                    <td class="lblRight">
                                        Contact Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="AgentContactFirstName" runat="server" Width="125px" EnableViewState="False"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        Category:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="AgentCategoryUID" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Type As:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="AgentTypeUID" runat="server" Width="125px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="lblRight">
                                        Activated Start Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                    <td class="lblRight">
                                        Activated End Date:
                                    </td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchEndDate" runat="server" EnableAppStyling="False" NullDateLabel=""
                                            Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                            <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1"
                                                SlideOpenDuration="1" />
                                        </ig:WebDatePicker>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        SS Rep:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="PrimaryContactUID" runat="server" Width="125px" Height="19px">
                                        </asp:DropDownList>
                                    </td>
                                     <td class="lblRight">
                                       FMA ID:</td>
                                    <td>
                                        <asp:TextBox ID="FMAID" runat="server" MaxLength="15" Width="125px" onKeyPress ="CheckNumeric()"></asp:TextBox>
                                         <%--<asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="FMAID"
                                            Display="dynamic" ErrorMessage="Invalid FMA ID" MaximumValue="9,223,372,036,854,775,807" MinimumValue="1"
                                            Type="Integer"></asp:RangeValidator>--%>
                                    </td>
                                    <td class="lblRight" id="tdAgentEdgeLabel" runat="server" visible="false">
                                        Edge ID:</td>
                                    <td id="tdAgentEdgeText" runat="server" visible="false">
                                        <asp:TextBox ID="AgentEdgeID" runat="server" MaxLength="25" />
                                    </td>  
                                </tr>
                            </table>
                            <div>
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="tbrTools_ButtonClicked"
                                                    AccessKey="h" Text="Search">
                                                    <Appearance>
                                                        <Image Url="~/Images/Check.png" />
                                                    </Appearance>
                                                </igtxt:WebImageButton>
                                            </td>
                                            <td>
                                                <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="tbrTools_ButtonClicked"
                                                    AccessKey="l" Text="Clear" CausesValidation="False">
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
                    </asp:Panel>
                    <br />
                    <fieldset>
                        <legend>Search Results</legend>
                        <asp:Panel ID="pnlRecords" runat="server" Height="" Width="" Visible="false">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td class="lblLeft">
                                                Page Size:
                                                <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                                    <asp:ListItem Selected="true">10</asp:ListItem>
                                                    <asp:ListItem>15</asp:ListItem>
                                                    <asp:ListItem>25</asp:ListItem>
                                                    <asp:ListItem>50</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="lblRight">
                                                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="grd" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                        DataSourceID="odsAgents" Font-Names="Verdana" Font-Size="X-Small" CssClass="mGrid"
                                        PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt" DataKeyNames="AgentID"
                                        OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand" OnPageIndexChanging="grd_PageIndexChanging"
                                        AllowSorting="True" OnSorting="grd_Sorting">
                                        <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                                            LastPageText="&raquo;" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Agent ID" SortExpression="ID">
                                                <ItemTemplate>
                                                    <asp:HyperLink NavigateUrl='<%#  "~/SecureAgentManagementForms/frmAgent.aspx?Adding=false&AgentUID=" + Eval("AgentID") + "&Adding=false"  %>'
                                                        runat="server" ID="hypAgentID" Text='<%# Eval("ID") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle Width="65px" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AgentFMAID" HeaderText="FMA ID" SortExpression="AgentFMAID">
                                                <ItemStyle Width="225px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AgentFullName" HeaderText="Agent Name" SortExpression="AgentFullName">
                                                <ItemStyle Width="225px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DBA" HeaderText="Agent DBA" SortExpression="DBA">
                                                <ItemStyle Width="225px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                <ItemStyle Width="75px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="UserUpdated" HeaderText="User Updated" SortExpression="UserUpdated">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="DateUpdated" HeaderText="Date Updated" SortExpression="DateUpdated">
                                                <ItemStyle Width="100px" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="AgentID" Visible="False" />
                                        </Columns>
                                    </asp:GridView>
                                    &nbsp;
                                    <asp:ObjectDataSource ID="odsAgents" runat="server" SelectMethod="GetAgentsPaging"
                                        TypeName="DataMerchantAppPaging" EnablePaging="True" MaximumRowsParameterName="PageSize"
                                        SelectCountMethod="GetAgentsPagingRowCount" StartRowIndexParameterName="CurrentPage"
                                        OldValuesParameterFormatString="original_{0}" OnSelecting="odsAgents_Selecting">
                                        <SelectParameters>
                                            <asp:Parameter Name="prms" Type="Object" />
                                            <asp:Parameter Name="PageSize" Type="Int32" />
                                            <asp:Parameter Name="CurrentPage" Type="Int32" />
                                            <asp:Parameter Name="ControlID" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="bucketfooter">
                                <table width="100%">
                                    <tr>
                                        <td align="left" style="width: 33%;">
                                            <asp:LinkButton ID="btnExpExcel" runat="server" OnClick="btnExport_Click">
                                                <span style="height: 25px; vertical-align: middle;">
                                                    <asp:Image ID="Image2" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                                        Excel</span></asp:LinkButton>&nbsp;&nbsp;&nbsp;
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
                        </asp:Panel>
                        <asp:Panel ID="pnlNoRecords" runat="server" Height="" Width="" Visible="true">
                            No data...
                        </asp:Panel>
                    </fieldset>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
