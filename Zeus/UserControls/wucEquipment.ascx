<%@ Control Language="C#" AutoEventWireup="True"
    Inherits="wucEquipment" CodeBehind="wucEquipment.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucQIRlookup.ascx" TagName="wucQIRlookup" TagPrefix="uc9" %> 

<script type="text/javascript" language="javascript">

    function ShowEquipment() {
        document.getElementById('<% = grd.ClientID%>').selectedIndex = -1;
        oWebDialogWindow2 = $find('<% =WebDialogWindow1.ClientID %>');
        oWebDialogWindow2.set_windowState($IG.DialogWindowState.Normal);

        return false;
    }
    function disableEquipmentPaging(isEditMode) {
        if (isEditMode.toLowerCase() == 'false') {

            $('#ContentPlaceHolder1_WucEquipment_pnlGrid .pgr').find('a').each(function () {
                $(this).removeAttr("href");
            });
        }
    }
</script>

<table width="100%">
    <tr>
        <td>
            <asp:Label ID="lblError" runat="server" Font-Size="10pt" ForeColor="Red"></asp:Label>

            <asp:Panel ID="Panel1" runat="server" Height="" Width="">
                <table>
                    <tr>
                                    <td class="lblRight">Merchant<br /> EMV Compliant:</td>
                                    <td rowspan="2">
                                       <asp:DropDownList ID="EMVComplianceMerchant" runat="server" OnSelectedIndexChanged="EMVComplianceMerchant_SelectedIndexChanged">
                                            <asp:ListItem Value="">--Select--</asp:ListItem>
                                            <asp:ListItem Value="1">EMV Compliant</asp:ListItem>
                                            <asp:ListItem Value="0">Non EMV Compliant</asp:ListItem> 
                                       </asp:DropDownList>
                                    </td>
                     </tr>
                 </table>
            </asp:Panel>

            <asp:Panel ID="pnlGrid" runat="server" Height="" Width="">
                <asp:GridView ID="grd" runat="server" PageSize="15" AllowPaging="true" CssClass="mGrid"
                    SelectedRowStyle-BackColor="#fffacd" DataKeyNames="UID,EquipmentRecordID" AutoGenerateColumns="false"
                    Font-Size="X-Small" Font-Names="verdana"  OnPageIndexChanging="grd_PageIndexChanging" OnRowDataBound="grd_RowDataBound"
                    OnRowCommand="grd_RowCommand">
                    <FooterStyle CssClass="footer" />
                    <PagerStyle CssClass="pgr" />
                    <AlternatingRowStyle CssClass="alt" />
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                    <Columns>
                        <asp:BoundField HeaderText="Equipment Record ID" DataField="EquipmentRecordID" ReadOnly="true" ItemStyle-Width="60px"/> <%--Row added for PXP-8430 by koshlendra--%>                          
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="Select" CommandArgument='<%#Eval("UID") %>' CommandName="select" Text="select" CausesValidation="false"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="Delete" CommandArgument='<%#Eval("UID") %>' CommandName="erase"
                                    Text="delete" CausesValidation="false"></asp:LinkButton>
                               <asp:LinkButton runat="server" ID="ViewVarSheet" CommandArgument='<%#Eval("UID") %>' CommandName="ViewVarSheet" Text="VAR Sheet" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="UID" Visible="false"></asp:BoundField>
                        <asp:BoundField DataField="EquipmentType" HeaderText="Type"></asp:BoundField>
                        <asp:BoundField DataField="EquipmentMaker" HeaderText="Maker"></asp:BoundField>
                        <asp:BoundField DataField="Model" HeaderText="Model"></asp:BoundField>
                        <asp:BoundField DataField="EMVCompliance" HeaderText="EMV"></asp:BoundField>
                        <asp:BoundField DataField="SerialNumber" HeaderText="Serial No."></asp:BoundField>
                        <asp:BoundField DataField="TerminalNumber" HeaderText="TID"></asp:BoundField>
                        <%--Niranjan PXP-8045--%>
                        <asp:BoundField DataField="ApplicationID" HeaderText="Application ID"></asp:BoundField>
                        <asp:BoundField DataField="DeployType" HeaderText="Deploy Type"></asp:BoundField>
                        <asp:BoundField DataField="DownloadNumber" HeaderText="Download No"></asp:BoundField>
                        <asp:BoundField DataField="TerminalShippings" HeaderText="Terminal Shipping"></asp:BoundField>
                        <asp:BoundField DataField="ShippingTrackingNumber" HeaderText="Tracking No"></asp:BoundField>
                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created"></asp:BoundField>
                        <asp:BoundField DataField="UserCreated" HeaderText="Created By"></asp:BoundField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID="pnlEquipment" runat="server">
                <div class="leftcolumn" style="width: 40%;">
                    <fieldset style="height: 190px;">
                        <legend>Equipment Information</legend>
                        <table>

                            <tr>
                                <td class="lblRight">Search:</td>
                                <td> <div style="position:relative;"><asp:TextBox ID="tbEquipmentSearch" runat="server" Enabled="true"></asp:TextBox></div></td>
                                <td> <asp:Button ID="btnLookup" runat="server" Text="Lookup Equipment" CausesValidation="false"
                                        OnClick="btnLookup_Click" /></td>

                            </tr>

                            <tr>
                                <td colspan="3">
                                    <hr />

                                </td>

                            </tr>
                            <tr>
                                <td class="lblRight">Type:
                                </td>
                                <td>
                                    <asp:TextBox ID="Type" runat="server" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="ItemUID" runat="server" />
                                    <asp:HiddenField ID="EquipmentTypeUID" runat="server" />
                                    <asp:HiddenField ID="EquipmentMakerUID" runat="server" />
                                    <asp:HiddenField ID="IsNewItem" Value="0" runat="server" />

                                </td>
                                <td>
                                   
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">Maker:</td>
                                <td>
                                    <asp:TextBox ID="Maker" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td class="lblRight">Model:</td>
                                <td>
                                    <asp:TextBox ID="Model" runat="server" Enabled="false"></asp:TextBox>
                                </td>
                                <td></td>
                            </tr>                             
                            <tr> <%--PXP-2319--%>
                                <td class="lblRight">Equipment<br /> EMV Compliant:</td>
                                <td>
                                     <asp:DropDownList ID="EMVCompliance" runat="server">
                                        <asp:ListItem Value="">--Select--</asp:ListItem>
                                        <asp:ListItem Value="1">EMV Compliant</asp:ListItem>
                                        <asp:ListItem Value="0">Non EMV Compliant</asp:ListItem> 
                                   </asp:DropDownList>
                                </td>
                            </tr>
                             <tr>
                                <td style="text-align:right;">
                                    <span>QIR vendor:</span>
                                </td>
                                <td colspan="2">
                                    <uc9:wucQIRlookup ID="QIUlookup" runat="server"></uc9:wucQIRlookup>
                                    
                                    </td>
                                 
                            </tr>

                        </table>
                    </fieldset>
                </div>
                <div class="rightcolumn" style="width: 60%;">
                    <fieldset style="height: 190px;">
                        <legend>Other Information</legend>
                        <table width="100%">
                            <tr>
                                <td class="lblRight">Terminal Status:</td>
                                <td>
                                    <asp:DropDownList ID="TerminalStatusUID" runat="server" Width="155px">
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">Application ID:</td>
                                <td>
                                    <asp:TextBox ID="ApplicationID" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblRight">TID</td>
                                <td>
                                    <asp:TextBox ID="TerminalNumber" runat="server" Width="150px"></asp:TextBox></td>
                                <td class="lblRight">Download No:</td>
                                <td>
                                    <asp:TextBox ID="DownloadNumber" MaxLength="10" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblRight">Terminal Shipping:</td>
                                <td>
                                    <asp:DropDownList ID="ShippingUID" runat="server" Width="155px">
                                    </asp:DropDownList>
                                </td>
                                <td class="lblRight">Tracking Number:</td>
                                <td>
                                    <asp:TextBox ID="ShippingTrackingNumber" runat="server" Width="150px"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="lblRight">Serial Number:</td>
                                <td>
                                    <asp:TextBox ID="SerialNumber" runat="server" Width="150px"></asp:TextBox>
                                </td>
                               <%-- Niranjan PXP-8045--%>
                                <td class="lblRight">Deploy Type:</td>
                                <td>
                                     <asp:DropDownList ID="DeployTypeID" runat="server" Width="150px">                                        
                                   </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblRight">Terminal Number:</td>
                                <td>
                                    <asp:TextBox ID="TerminalNumberVAR" runat="server" Width="150px" MaxLength="4"
                                        oninput="this.value = this.value.replace(+[^0-9]/g, '').slice(0,4);"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revTerminalNumber" runat="server" 
                                        ControlToValidate="TerminalNumberVAR"
                                        ValidationExpression="^\d{4}$"
                                        ErrorMessage="Terminal Number must be exactly 4 digits"
                                        Display="Dynamic" ForeColor="Red" Text="*">
                                    </asp:RegularExpressionValidator>
                                </td>
                                                            <td class="lblRight">Store Number:</td>
                                <td>
                                    <asp:TextBox ID="StoreNumberVAR" runat="server" Width="150px" MaxLength="4"
                                        oninput="this.value = this.value.replace(+[^0-9]/g, '').slice(0,4);"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="revStoreNumber" runat="server" 
                                        ControlToValidate="StoreNumberVAR"
                                        ValidationExpression="^\d{4}$"
                                        ErrorMessage="Store Number must be exactly 4 digits"
                                        Display="Dynamic" ForeColor="Red" Text="*">
                                    </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </asp:Panel>
        </td>
    </tr>    
    <tr>
        <td>
            <asp:Panel ID="pnlEquipmentDocs" runat="server" Height="" Width="" Visible="false">
                <fieldset>
                    <legend>Equipment Documents</legend>
                    <table width="100%">
                        <tr>
                            <td align="left">Page Size:
                                        <asp:DropDownList runat="server" AutoPostBack="true" ID="ddlPageSize" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="25">25</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="100">100</asp:ListItem>
                                        </asp:DropDownList>
                            </td>
                            <td align="right">
                                <span>Total Records Found:
                                            <asp:Literal ID="litRecordCount" runat="server">0</asp:Literal>
                                </span>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="grdDocuments" OnPageIndexChanging="grdDocuments_PageIndexChanging"
                        runat="server" AutoGenerateColumns="False" CssClass="mGrid" AlternatingRowStyle-CssClass="alt"
                        PagerStyle-CssClass="pgr" FooterStyle-CssClass="footer" Font-Names="verdana" PageSize="5"
                        Font-Size="X-Small" OnRowDataBound="grdDocuments_RowDataBound" DataKeyNames="DocID" AllowPaging="True">
                        <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                        <Columns>
                            <asp:BoundField DataField="DocID" HeaderText="Doc ID" Visible="False" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/pdf.png" />
                                </ItemTemplate>
                            </asp:TemplateField>
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
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblDescription" Text='<%#Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Source" HeaderText="Source" ReadOnly="True" />
                            <asp:BoundField DataField="ContentSize" HeaderText="Size" ReadOnly="True" />
                            <asp:BoundField DataField="DocDate" HeaderText="Document Date" DataFormatString="{0:MM/dd/yyy hh:mm:ss tt}"
                                ReadOnly="True" />
                            <asp:TemplateField HeaderText="User Uploaded">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("UserCreated") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle CssClass="footer" />
                        <PagerStyle CssClass="pgr" />
                        <AlternatingRowStyle CssClass="alt" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ods" runat="server" SelectMethod="GetDocumentPaging" EnablePaging="True"
                        MaximumRowsParameterName="PageSize" SelectCountMethod="GetDocumentPagingCount"
                        StartRowIndexParameterName="CurrentPage" OldValuesParameterFormatString="original_{0}"
                        OnSelecting="ods_Selecting" TypeName="DatamerchantAppPaging">
                        <SelectParameters>
                            <asp:Parameter Name="prms" Type="Object" />
                            <asp:Parameter Name="PageSize" Type="Int32" />
                            <asp:Parameter Name="CurrentPage" Type="Int32" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </fieldset>
            </asp:Panel>
        </td>
    </tr>
</table>
<ig:WebDialogWindow ID="WebDialogWindow1" runat="server" Height="500px" Width="600px"
    InitialLocation="Centered" Modal="True" WindowState="Hidden">
    <ContentPane>
        <Template>
            <contenttemplate></contenttemplate>
            <div class="tabcontent">
                <fieldset>
                    <legend>Equipment Search</legend>
                    <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                        <table width="100%">
                            <tr>
                                <td class="lblRight">Type:</td>
                                <td>
                                    <asp:DropDownList ID="cboType" runat="server" Width="105px">
                                    </asp:DropDownList></td>
                                <td class="lblRight">Maker:</td>
                                <td>
                                    <asp:DropDownList ID="cboMaker" runat="server" Width="105px">
                                    </asp:DropDownList></td>
                                <td class="lblRight">Model:</td>
                                <td>
                                    <asp:TextBox ID="txtModel" runat="server" EnableViewState="False" Width="100px"></asp:TextBox></td>
                            </tr>
                        </table>
                        <br />
                        <center>
                            <asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click"
                                Text="Search" />
                            <asp:Button ID="btnReset" runat="server" CausesValidation="False" OnClick="btnReset_Click"
                                Text="Reset" /></center>
                    </asp:Panel>
                </fieldset>
                <fieldset>
                    <legend>Search Results</legend>
                    <asp:Panel runat="server" ID="pnl1">
                        <asp:GridView ID="grdSearch" runat="server" CssClass="mGrid" AutoGenerateColumns="false"
                            Font-Size="X-Small" Font-Names="verdana" OnRowCommand="grdSearch_Rowcommand"
                            DataKeyNames="UID" >
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                            <FooterStyle CssClass="footer" />
                            <PagerStyle CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                            <Columns>
                                <asp:BoundField DataField="UID" Visible="false"></asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnSelect" Text="Select" runat="server" CommandName="Select"
                                            CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="EquipmentType" HeaderText="Type"></asp:BoundField>
                                <asp:BoundField DataField="EquipmentMaker" HeaderText="Maker"></asp:BoundField>
                                <asp:BoundField DataField="Model" HeaderText="Model"></asp:BoundField>
                                <asp:BoundField DataField="EMVCompliance" HeaderText="EMVCompliance"></asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Panel ID="nosearchrecords" runat="server">
                        no data..
                    </asp:Panel>
                </fieldset>
            </div>
        </Template>
    </ContentPane>
    <Header CaptionText="Equipment Lookup">
    </Header>
</ig:WebDialogWindow>
