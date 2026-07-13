<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucDescriptor.ascx.cs" 
     Inherits="ZeusWeb.UserControls.wucDescriptor" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucMessage.ascx" TagName="wucMessage" TagPrefix="uc1" %>

<asp:Panel runat="server" ID="pnlRecords">
    <table width="100%">
        <tr>
            <td class="lblLeft">Page Size:
                                    <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                                        <asp:ListItem Selected="True">3</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                    </asp:DropDownList>
            </td>
            <td class="lblRight">
                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="grdDescriptor" runat="server" AutoGenerateColumns="False" CssClass="mGrid" PageSize="3"
                    Font-Names="Verdana" Font-Size="X-Small" DataKeyNames="MerchantDescriptorID" AllowPaging="True" OnPageIndexChanging="grdDescriptor_PageIndexChanging"
                    OnRowDeleting="grdDescriptor_RowDeleting" OnRowDataBound="grdDescriptor_RowDataBound" ShowHeaderWhenEmpty="True">
                    <PagerStyle CssClass="pgr" />
                    <FooterStyle CssClass="footer" />
                    <AlternatingRowStyle CssClass="alt" />
                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="&laquo;"
                        LastPageText="&raquo;" />
                    <Columns>
                        <asp:CommandField DeleteImageUrl="~/Images/delete.png" ButtonType="Image" ShowDeleteButton="true"
                            DeleteText="Delete" ItemStyle-Width="8px" >
                        <ItemStyle Width="8px" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="Descriptor" SortExpression="Descriptor">
                           
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Descriptor") %>'></asp:Label>
                                <asp:Label ID="Label2" runat="server" Text='<%# Bind("Descriptor2") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="130px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserCreated" HeaderText="Created By" ItemStyle-Width="80px" SortExpression="UserCreated">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateCreated" HeaderText="Created On" ItemStyle-Width="80px" SortExpression="DateCreated">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UserModified" HeaderText="Modified By" ItemStyle-Width="80px" SortExpression="UserModified">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DateModified" HeaderText="Modified On" ItemStyle-Width="80px" SortExpression="DateModified">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DescriptorType" HeaderText="Descriptor Type" ItemStyle-Width="80px" SortExpression="DescriptorType">
                            <ItemStyle Width="80px" />
                        </asp:BoundField>
                         <asp:BoundField DataField="MerchantDescriptorID"  ItemStyle-CssClass="hideGridColumn"  HeaderStyle-CssClass="hideGridColumn"></asp:BoundField>
                      </Columns>
                    <EmptyDataTemplate>
                        No Records Found....
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <br />
</asp:Panel>
<asp:Panel runat="server" ID="pnlAdd">
    <asp:TextBox ID="CreditDescriptor" MaxLength="22" runat="server" Width="230px"
        Enabled="false"></asp:TextBox>
    <asp:HiddenField ID="lblDescriptor" runat="server"></asp:HiddenField>
    <asp:Button ID="btnAdd" runat="server" Text="Add Descriptor" OnClick="btnAdd_Click" />
</asp:Panel>
<ig:WebDialogWindow ID="WebDialogWindow3" runat="server" Height="500px" Width="450px"
    InitialLocation="Centered" Modal="true" UseBodyAsParent="true" WindowState="Hidden">
    <ContentPane>
        <Template>
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="tabcontent">
                        <uc1:wucMessage runat="server" ID="message1" />
                        <br />
                        <asp:Panel ID="Panel3" runat="server" DefaultButton="Save">
                            <p>
                                Requirements for a Valid Descriptor
                            </p>
                              <asp:Panel runat="server" ID="pnlMeritusIrvineOnly">
                                <ol>
                                    <%--code change doen for PXP-13243[Zeus: HR Descriptor Validation at 17 Characters] by koshlendra start--%>
                                    <li>Alphabets, number, special chars examples: dashes, dots, underscores, colon, Forward slash, Period </li>
                                    <li>Max. 22 chars </li>
                                    <li>Free format </li>
                                     <li>Validate for duplication within the first 17 characters(excluding special characters)</li>
                                    <%--code change doen for PXP-13243[Zeus: HR Descriptor Validation at 17 Characters] by koshlendra end--%>
                                </ol>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlMeritus">
                                <ol>
                                    <li>Alpha numeric only </li>
                                    <li>21 characters max </li>
                                    <li>One character must be an *
                                        <ol>
                                            <li>This * can only be in position 4, 8, or 13.
                                                <ul>
                                                    <li>Ex. ABC*XYZCompany </li>
                                                    <li>Ex. ABCDEFG*XYZCompany </li>
                                                    <li>Ex. ABCDEFGHIJKL*XYZCompa</li>
                                                </ul>
                                            </li>
                                        </ol>
                                    </li>
                                </ol>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="pnlOptimal">
                                <ul>
                                    <li>

                                        <b>Allied Irish Bank / Barclay / Credit Guard/ Credorax</b><br />

                                        Max length for Line 1 is 25 and 13 for line 2.
                                    </li>
                                    <li>

                                        <b>EPX</b><br />

                                        The max length for both fields combined depends on brand: VI is 25, MC/DI is 22 and AM is 20.
                                    </li>
                                    <li>

                                        <b>MONERIS</b><br />

                                        Max length is 22 characters for the Account Name + "/" + Line 1. Moneris will do this concatenation on their end.
                                    </li>
                                    <li>

                                        <b>TSYS</b><br />

                                        Max length for Line 1 is 25 and 13 for line 2.
                                    </li>
                                    <li>
                                        <b>VANTIV
                                        </b>
                                        <br />
                                        Max length for Line 1 is 12 and 10 for line 2, address and city fields are required.
                                    </li>
                                </ul>
                            </asp:Panel>

                            <table>

                                <tr>
                                    <td colspan="3">
                                        <asp:Label runat="server" ID="lblErrorDes" ForeColor="red"></asp:Label>
                                       </td>
                                </tr>
                                <tr>
                                    <td class="lblRight">
                                        <b>
                                            <asp:Literal runat="server" ID="litDescr" Text="Descriptor"></asp:Literal>:</b>
                                    </td>
                                    <td>
                                        <%--changes done for PXP-13483 by koshlendra start--%>
                                        <asp:TextBox ID="ValDescriptor" runat="server" ClientMode="Static" AutoPostBack="true" MaxLength="22" OnTextChanged="ValDescriptor_TextChanged"
onkeyup="textCounter(this);" onkeydown="textCounter(this);"></asp:TextBox>
<label for="ValDescriptor" style="font-weight: bold" id="counterId"></label>
                                        <%--changes done for PXP-13483 by koshlendra start--%>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnValidate" runat="server" Text="Validate" OnClick="btnValidate_Click" Visible="false" OnClientClick="return ShowVDescriptor(this); " />
                                    </td>
                                </tr>

                                <asp:PlaceHolder runat="server" ID="phOptimal">
                                    <tr>
                                        <td class="lblRight">
                                            <b>Descriptor Line 2:</b>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="ValDescriptorLine2" runat="server" MaxLength="75"></asp:TextBox>
                                        </td>
                                    </tr>
                                </asp:PlaceHolder>
                                <tr>
                                    <td class="lblRight">
                                        <b>Descriptor Type:</b>
                                    </td>
                                    <td>
                                        <%--changes done for PXP-13483 by koshlendra start--%>
                                        <asp:DropDownList ID="DescriptorTypeID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DescriptorTypeID_SelectedIndexChanged"></asp:DropDownList>
                                        <%--changes done for PXP-13483 by koshlendra end--%>
                                         </td>
                                </tr>
                            </table>
                            <br />
                            <center>
                                <asp:Button ID="Save" runat="server" CausesValidation="False" OnClientClick="return ShowVDescriptor(this);"
                                    OnClick="btnSave_Click" Text="Save" />
                                <asp:Button ID="Close" runat="server" CausesValidation="False" OnClick="btnClose_Click"
                                    Text="Close" />
                            </center>
                        </asp:Panel>
                    </div>
                    <div>
                        <asp:GridView ID="grdExistingDescriptors" AutoGenerateColumns="false" runat="server" AllowSorting="true"
                                    AlternatingRowStyle-CssClass="alt" HorizontalAlign="left"
                                    DataKeyNames="MerchantID, BusinessLegalName,Descriptor,MerchantDescriptorTypeID" CssClass="mGrid" ShowHeader="true" EmptyDataText="No Matching Descriptors found."
                                    BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%"
                                    ShowFooter="false" AllowPaging="false">
                                    <Columns>
                                        <asp:BoundField DataField="MerchantID" HeaderText="ZID" HeaderStyle-Width="40px" />
                                        <asp:BoundField DataField="BusinessLegalName" HeaderText="Legal Name" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="Descriptor" HeaderText="Descriptor" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-Width="120px" />

                                    </Columns>
                                    <PagerStyle CssClass="pgr" />
                                    <FooterStyle CssClass="footer" />
                                    <PagerSettings Mode="NumericFirstLast" />
                                </asp:GridView>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Save" />
                    <asp:PostBackTrigger ControlID="Close" />
                </Triggers>
            </asp:UpdatePanel>
        </Template>
    </ContentPane>
    <Header CaptionText="Descriptor">
    </Header>
</ig:WebDialogWindow>

<script type="text/javascript">

    function ShowVDescriptor(id) {

        var n = $(id).attr("ID").lastIndexOf("_");
        var str1 = $(id).attr("ID").substring(0, n + 1);

        var txt1 = $('#' + str1 + "ValDescriptor");

        var descriptor = txt1.val().trim();
        $('#' + str1 + "lblErrorDes").val('');

        var dateformat1 = /^[a-zA-Z0-9 .]{3}[*][a-zA-Z0-9 .]{1,17}$/
        var dateformat2 = /^[a-zA-Z0-9 .]{7}[*][a-zA-Z0-9 .]{1,13}$/
        var dateformat3 = /^[a-zA-Z0-9 .]{12}[*][a-zA-Z0-9 .]{1,8}$/

        // PXP-3736 : remove descriptor regualr expression validation for irvine office only
        var crtlOffice = document.getElementById("ContentPlaceHolder1_WucBusinessInfo1_OfficeID");
        var merchantOffice = crtlOffice.options[crtlOffice.selectedIndex].text;

        if (merchantOffice == "Irvine (US)") { 
            if (descriptor != '' && descriptor.length <= 22) { //max. 22 char
                $('#' + str1 + "Descriptor").val(descriptor);
                $('#' + str1 + "lblDescriptor").val(descriptor);
                return true;
            }
            else {
                $('#' + str1 + "lblErrorDes").val("Enter valid descriptor");
                alert($('#' + str1 + "lblErrorDes").val());
                $('#' + str1 + "ValDescriptor").val('');
                return false;
            }
        }
        else {  

            if ((descriptor != '') && (dateformat1.test(descriptor) || dateformat2.test(descriptor) || dateformat3.test(descriptor))) {
                $('#' + str1 + "Descriptor").val(descriptor);
                $('#' + str1 + "lblDescriptor").val(descriptor);
                return true;
            }
            else {
                $('#' + str1 + "lblErrorDes").val("Enter valid descriptor");
                alert($('#' + str1 + "lblErrorDes").val());
                $('#' + str1 + "ValDescriptor").val('');
                return false;
            }
        }
    }

    function CloseVDescriptor(id) {
        var n = $(id).attr("ID").lastIndexOf("_");
        var str1 = $(id).attr("ID").substring(0, n + 1);

        var txt1 = $('#' + str1 + "ValDescriptor");
        txt1.val('');

        str1 = getIndex(str1);

        oWebDialogWindow3 = $find(str1 + "WebDialogWindow3");
        oWebDialogWindow3.set_windowState($IG.DialogWindowState.Hidden);
    }

    function getIndex(str) {
        //str       - the string
        //c         - the character or string to search for
        //n         - which occurrence
        var strCopy = str; //make a copy of the string
        var str2 = '';

        var index;
        var n = 0;
        while (n < 2) {
            index = strCopy.indexOf("_");
            str2 += strCopy.substring(0, index + 1);
            strCopy = strCopy.substring(index + 1);
            n++;
        }

        return str2;
    }
    function textCounter(field) {
        var i = field.value.length;
        document.getElementById('counterId').innerHTML = "(" + i + ")";
    }

</script>
