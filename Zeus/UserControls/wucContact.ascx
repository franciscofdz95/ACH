<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucContact.ascx.cs"
    Inherits="ZeusWeb.UserControls.wucContact" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactPhoneCountryCode_0").change(function () {
            $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactCountryCodeDisplay_0").val($(this).val());
        });

        $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactPhoneCountryCode_1").change(function () {
            $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactCountryCodeDisplay_1").val($(this).val());
        });

        $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactPhoneCountryCode_2").change(function () {
            $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactCountryCodeDisplay_2").val($(this).val());
        });

        $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactPhoneCountryCode_3").change(function () {
            $("#ContentPlaceHolder1_wucContact1_gvPhone_ContactCountryCodeDisplay_3").val($(this).val());
        });
    });
</script>

<asp:UpdatePanel runat="server" ID="pnlUpdate">
    <ContentTemplate>
        <fieldset class="dialog">
            <legend><asp:Literal runat="server" ID="litTitle"></asp:Literal></legend>

            <asp:Panel runat="server" ID="pnlMain">
            
            
            <asp:Panel runat="server" ID="pnlView">
                <table class="contacttable">
                    <tr>
                        <td class="left">
                            Contact Name:
                        </td>
                        <td class="right">
                            <asp:DropDownList ID="ddlName" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlName_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Title:
                        </td>
                        <td class="right">
                            <asp:Literal runat="server" ID="litContactType"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="trLitAccessLevel" runat="server">
                        <td class="left">
                            Access Level:
                        </td>
                        <td class="right">
                            <asp:Literal runat="server" ID="litAccessLevel"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Contact Email:
                        </td>
                        <td class="right">
                            <asp:Literal runat="server" ID="litEmailList"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Contact Phone#:
                        </td>
                        <td class="right">
                            <table>
                                <tr>                        
                                    <td>  
                                        <asp:BulletedList ID="blCountry" runat="server"></asp:BulletedList>
                                    </td>
                                    <td>
                                        <asp:BulletedList ID="blPhone" runat="server"></asp:BulletedList>
                                    </td>
                                    <td>
                                        <asp:BulletedList ID="blExt" runat="server"></asp:BulletedList>
                                    </td>
                                 </tr>
                             </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlEdit">
                <div style="text-align: center;">
                    <asp:Label runat="server" ID="lblError" Style="color: Red"></asp:Label>
                </div>
                <table class="contacttable">
                    <tr id="trContacts" runat="server">
                        <td class="left">
                            Contacts:
                        </td>
                        <td class="right">
                            <asp:DropDownList ID="ddlContacts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlContacts_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hidNewContactID" runat="server" Value="0" />
                            <asp:LinkButton runat="server" ID="lbRemoveContact" OnClientClick="return confirm('Are you sure you want to delete this contact?')"
                                OnClick="lbRemoveContact_Click">Remove this contact</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Primary:
                        </td>
                        <td class="right">
                            <asp:CheckBox ID="cbIsPrimary" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Contact Name:
                        </td>
                        <td class="right">
                            <table class="smalltable">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="tbFirstname" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbMiddlename" Width="40px" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="tbLastname" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        First
                                    </td>
                                    <td>
                                        Mi.
                                    </td>
                                    <td>
                                        Last
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Title:
                        </td>
                        <td class="right">
                            <asp:DropDownList ID="ddlTitle" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trDdlAccessLevel" runat="server">
                        <td class="left">
                            Access Level:
                        </td>
                        <td class="right">
                            <asp:DropDownList ID="ddlAccessLevel" runat="server">
                                <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">All Access</asp:ListItem>
                                <asp:ListItem Value="2">Information Only Access</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Contact Email:
                        </td>
                        <td class="right">
                            <asp:GridView ID="gvEmail" runat="server" AutoGenerateColumns="False" DataKeyNames="EmailAddressID"
                                OnRowDeleting="gvEmail_RowDeleting" ShowFooter="True" EnableModelValidation="True"
                                ShowHeader="False" BorderStyle="None" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Email">
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" ID="lbAddNewEmail" CausesValidation="false" OnClick="lbAddNewEmail_Click">Add New Email</asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="tbEmailAddress" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>
                                            <asp:HiddenField ID="hidEmailAddressID" Value='<%# Eval("EmailAddressID") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:LinkButton runat="server" ID="lbAddNewEmail" CausesValidation="false" OnClick="lbAddNewEmail_Click">Add New Email</asp:LinkButton>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                            Contact Phone#:
                        </td>
                        <td class="right">
                            <asp:GridView ID="gvPhone" runat="server" AutoGenerateColumns="False" DataKeyNames="PhoneID"
                                OnRowDeleting="gvPhone_RowDeleting" ShowFooter="True" OnRowDataBound="gvPhone_RowDataBound"
                                EnableModelValidation="True" ShowHeader="False" BorderStyle="None" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="Phone">
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" ID="lbAddNewPhone" CausesValidation="false" OnClick="lbAddNewPhone_Click">Add New Phone</asp:LinkButton>
                                        </FooterTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPhoneType" runat="server">
                                                <asp:ListItem Value="1">Home</asp:ListItem>
                                                <asp:ListItem Value="2">Cell</asp:ListItem>
                                                <asp:ListItem Value="3">Work</asp:ListItem>
                                                <asp:ListItem Value="4">Fax</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ContactPhoneCountryCode" runat="server" Width="45px" AutoPostBack="True" OnSelectedIndexChanged="ContactPhoneCountryCode_OnSelectedIndexChanged">
                                            </asp:DropDownList>

                                            <asp:TextBox ID="ContactCountryCodeDisplay" runat="server" Width="40px"></asp:TextBox>                                                       
                                            <ig:WebMaskEditor ID="tbPhoneNumber" runat="server"  Width="78px"
                                                Value='<%# Bind("PhoneNumber") %>' InputMask="############" PromptChar=' ' ShowMaskOnFocus="False">
                                            </ig:WebMaskEditor>
                                            <asp:HiddenField ID="hidPhoneID" Value='<%# Eval("PhoneID") %>' runat="server" />
                                            <ig:WebMaskEditor ID="ContactPhoneExt" runat="server" InputMask="000000" CssClass="text igte_Edit" Width="42px" ShowMaskOnFocus="False"></ig:WebMaskEditor>                                        
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <asp:LinkButton runat="server" ID="lbAddNewPhone" CausesValidation="false" OnClick="lbAddNewPhone_Click">Add New Phone</asp:LinkButton>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td class="left">
                        </td>
                        <td class="right" style="text-align: right;">
                            <asp:LinkButton runat="server" ID="lbAddNewContact" CausesValidation="false" OnClick="lbAddNewContact_Click">Add New Contact</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lbSave" Visible="false" CausesValidation="false"
                                OnClick="lbSave_Click">Save Contact</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lbCancel" Visible="false" CausesValidation="false"
                                OnClick="lbCancel_Click">Cancel</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table class="contacttable" runat="server" id="tabNotificationEmails">
                <tr>
                    <td class="left">
                        Notification Emails:
                    </td>
                    <td class="right">
                        <asp:TextBox runat="server" ID="NotificationEmails" Rows="3" Width="250px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                     <td class="left">
                     <asp:Label ID="lbDisableRDRVerifi" runat="server" Visible="false">
                         Disable Send RDR-Verifi:
                     </asp:Label>   
                    </td>
                    <td class="right">
                        <asp:CheckBox ID="CheckBoxRDRVerifi" runat="server" Visible="false" />
                    </td>
                </tr>
            </table>


            </asp:Panel>

        </fieldset>
    </ContentTemplate>
</asp:UpdatePanel>
