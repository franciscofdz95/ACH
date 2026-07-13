<%@ Control Language="C#" AutoEventWireup="true" Inherits="wucTicketTemplate"
    CodeBehind="wucTicketTemplate.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<script id="Infragistics" type="text/javascript">

    function btnSave_Click(oButton, oEvent) {
        var txt = document.getElementById('<%=Issue.ClientID%>');
        var title = document.getElementById('<%=TicketTemplateName.ClientID%>');
        var desc = document.getElementById('<%=Description.ClientID%>');
        var cat = document.getElementById('<%=SubCategoryID.ClientID%>');
        var parent = document.getElementById('<%=CategoryID.ClientID%>');
        var dept = document.getElementById('<%=DepartmentID.ClientID%>');

        var err = "";

        if (txt.value == "" || title.value == "" || desc.value == "" || cat.selectedIndex <= 0 || parent.selectedIndex <= 0 || dept.selectedIndex <= 0) {
            err = 'Title is required.\nDepartment is required.\nCategory is required.\nSub-Category is required.\nDescription is required.\nIssue is required.\n';
            oEvent.cancel = true;
        }
        
        if (err != "")
            alert(err);

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

<asp:UpdatePanel ID="pnlCustomerInfo" runat="server">
    <ContentTemplate>      
<div class="title">
    &nbsp;&nbsp;Ticket Template Management
    <hr class="line" />
</div>
        <div class="content">
<asp:Panel ID="pnlTools" runat="server">
    <div class="tbrtools">
        <div class="tbrtoolsleft">
            <table cellspacing="3" cellpadding="3">
                <tr>
                    <td>
                        <igtxt:WebImageButton ID="btnEdit" runat="server" Text="Edit" CommandName="Edit"
                            AccessKey="e" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/edit.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnAdd" runat="server" Text="Add" CommandName="Add" AccessKey="a"
                            OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/add2.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnSave" runat="server" Text="Save" Enabled="false" AccessKey="s"
                            CommandName="Save" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <ClientSideEvents Click="btnSave_Click" />
                            <Appearance>
                                <Image Url="~/Images/disk_blue.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <igtxt:WebImageButton ID="btnCancel" runat="server" Text="Cancel" Enabled="false"
                            AccessKey="c" CommandName="Cancel" OnClick="tbrTools_ButtonClicked" CausesValidation="False">
                            <Appearance>
                                <Image Url="~/Images/disk_blue_error.png" />
                            </Appearance>
                        </igtxt:WebImageButton>
                    </td>
                    <td>
                        <asp:LinkButton runat="server" ID="lnkBack" Text="Back to Templates" OnClick="lnkBack_Click" CausesValidation="false"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>  
</asp:Panel> 
<asp:Panel ID="pnlEdit" runat="server" Style="padding: 3px 3px 3px 3px;" Height="">
    <asp:Label runat="server" ID="lblError" CssClass="gen_error"></asp:Label>
    <table cellspacing="5" cellpadding="3">
        <tr>
            <td class="lblRight">Title:
            </td>
            <td>
                <asp:TextBox runat="server" ID="TicketTemplateName" Width="170px" TabIndex="1" MaxLength="50"></asp:TextBox> <span class="required">*</span>               
            </td>
         </tr>
        <tr>   
            <td class="lblRight">Status:
            </td>
            <td>
                <asp:DropDownList runat="server" ID="Status" Width="175px" TabIndex="5">
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>                    
                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
       <tr>
           <td class="lblRight">Office:</td>
           <td>
               <%--PXP:5768: Ani: Zeus: Rename 'Cambridge' office as 'London'--%>
                <asp:DropDownList runat="server" ID="OfficeID" Width="175px" TabIndex="10">
                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                    <asp:ListItem Value="1">Irvine (US)</asp:ListItem>
                    <asp:ListItem Value="2">Montreal (CAN)</asp:ListItem>
                    <asp:ListItem Value="3">London (UK)</asp:ListItem>
                    <asp:ListItem Value="4">Gatineau (CAN)</asp:ListItem>
                </asp:DropDownList>
           </td>
       </tr>
       <tr>
        <td class="lblRight">
            Department:
        </td>
        <td>
            <asp:DropDownList ID="DepartmentID" runat="server" Width="175px" TabIndex="15" AutoPostBack="true"
                OnSelectedIndexChanged="DepartmentID_SelectedIndexChanged">
            </asp:DropDownList><span class="required">*</span>
        </td>
       </tr>
       <asp:Panel runat="server" ID="pnlCategory" CssClass="item" Visible="false">
            <tr>
            <td class="lblRight">
                Category:
            </td>
            <td>
                <asp:DropDownList ID="CategoryID" runat="server" Width="175px" TabIndex="20" OnSelectedIndexChanged="CategoryID_SelectedIndexChanged"
                    AutoPostBack="true">
                </asp:DropDownList><span class="required">*</span>
            </td>
                </tr>
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlSubCategory" CssClass="item" Visible="false">
            <tr>
            <td class="lblRight">
                Sub-Category:
            </td>
            <td>
                <asp:DropDownList ID="SubCategoryID" runat="server" Width="175px" TabIndex="25">
                </asp:DropDownList><span class="required">*</span>
            </td>
                </tr>
        </asp:Panel>
         <tr>
            <td class="lblRight">Due Date:
            </td>
            <td>
                <asp:TextBox runat="server" ID="DueDays" Width="40px" TabIndex="30" MaxLength="2" onKeyPress="CheckNumeric()"></asp:TextBox>
            </td>
         </tr>
         <tr>
            <td class="lblRight" valign="top">Description:
            </td>
            <td valign="top">
                 <asp:TextBox ID="Description" runat="server" TextMode="MultiLine" Width="300px" Height="100px" MaxLength="200"
                  TabIndex="35"></asp:TextBox><span class="required">*</span>
            </td>
         </tr>
         <tr>
            <td class="lblRight" valign="top">Issue:
           </td>
            <td valign="top">
                 <asp:TextBox ID="Issue" runat="server" TextMode="MultiLine" Width="300px" Height="100px"
                 TabIndex="40"></asp:TextBox><span class="required">*</span>
            </td>
         </tr>
    </table>
</asp:Panel>
            </div>
</ContentTemplate> 
</asp:UpdatePanel>