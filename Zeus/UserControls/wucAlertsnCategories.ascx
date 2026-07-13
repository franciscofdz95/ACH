<%@ Control Language="C#" AutoEventWireup="True" Inherits="wucAlertsnCategories"
    CodeBehind="wucAlertsnCategories.ascx.cs" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Src="~/UserControls/wucAlertContacts.ascx" TagName="wucAlertContacts" TagPrefix="uc1" %>

<script language="JavaScript" type="text/javascript">
    function CollapseExpand(object, txt, object1) {

        //alert(object);
        //alert(object1);

        var div = document.getElementById(object);
        var object2 = document.getElementById(object1);
        if (txt == null) {
            if (div.style.display == "none") {
                div.style.display = "inline";
                object2.src = "../Images/close.gif";
            }
            else {
                div.style.display = "none";
                object2.src = "../Images/open.gif";
            }
        }
        else {
            div.style.display = txt;
            if (txt == 'none')
                object2.src = "../Images/open.gif";
            else
                object2.src = "../Images/close.gif";
        }
    }

    function expandAll(txt) {
        var gridViewCtlId = document.getElementById('<%=grdCat.ClientID%>');

        if (gridViewCtlId != null) {

            var i = 0;
            var j = 0;
            for (; i < (gridViewCtlId.rows.length); i++) {
                if (txt == '1') {
                    //if (j < 10)
                    //    j = '0' + j;
                    CollapseExpand(gridViewCtlId.id + '_div1_' + j, 'inline', gridViewCtlId.id +'_img1_' + j);
                    j++;
                }
                else {
                    //if (j < 10)
                    //    j = '0' + j;
                    CollapseExpand(gridViewCtlId.id + '_div1_' + j, 'none', gridViewCtlId.id +  '_img1_' + j);
                    j++;
                }
            }
        }
    }

    function CloseMCC() {
        oWebDialogWindow1 = $find('<% =dlgContacts.ClientID %>'); 
        oWebDialogWindow1.set_windowState($IG.DialogWindowState.Hidden);
    }
</script>

<asp:Label ID="lblError" runat="server" EnableViewState="false" Font-Size="10pt" ForeColor="Red"></asp:Label>
<fieldset>
    <legend>Language Options</legend>
     <asp:Panel ID="pnlLanguageOption" runat="server" Height="" Width="100%">
         <table>
             <tr>
                 <td>Language</td>
                 <td>
                     <asp:DropDownList ID="AlertLanguage" runat="server">
                         <asp:ListItem Value="1">English</asp:ListItem>
                         <asp:ListItem Value="2">French</asp:ListItem>
                     </asp:DropDownList>
                 </td>
                 
            </tr>
         </table>
     </asp:Panel>
    
</fieldset>
<fieldset>
    <legend>Alert Options</legend>
    <br />
    <div style="width: 100%">
        <div class="buckethdrleft">
            <asp:Panel runat="server" ID="pnl1">
                <a id="lnkExpandAll" onmouseover="this.style.cursor='pointer';" onclick="expandAll('1')">
                    Expand All</a> | <a id="lnkCollapseAll" onmouseover="this.style.cursor='pointer';"
                        onclick="expandAll('0')">Collapse All</a>
            </asp:Panel>
        </div>
        <div class="buckethdright">
            &nbsp;</div>
    </div>
    <asp:Panel ID="pnlDetail" runat="server" Height="" Width="100%">
        <asp:GridView ID="grdCat" AutoGenerateColumns="false" runat="server" AllowSorting="true"
            HorizontalAlign="left" OnSorting="grd_Sorting" Font-Names="Verdana" Font-Size="X-Small"
            OnRowDataBound="grdCat_RowDataBound" DataKeyNames="CategoryID" ShowHeader="false"
            BorderColor="white" BorderStyle="None" GridLines="none" BorderWidth="0px" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Category" SortExpression="Name">
                    <ItemTemplate>
                        <br />
                        <div class="title" style="width: 100%;">
                            <img runat="server" id='img1' src="../Images/close.gif" onmouseover="this.style.cursor='pointer';"
                                alt="img" />&nbsp;
                            <asp:Label Text='<% # Eval("Name") %>' runat="server" ID="lblName" ForeColor="#0a94d6"
                                Font-Bold="true"></asp:Label>
                            <hr class="line" />
                        </div>
                        <div id="div1" style="display: inline;" runat="server">
                            <asp:GridView ID="gvAlerts" runat="server" GridLines="none" Font-Names="Verdana"
                                OnRowCommand="gvAlerts_OnRowCommand" Font-Size="x-small" OnRowDataBound="gvAlerts_RowDataBound"
                                EmptyDataText="No Alerts..." Style="table-layout: fixed" Width="100%" AutoGenerateColumns="false"
                                DataKeyNames="CategoryID,AlertID" CssClass="mGrid">
                                <PagerStyle CssClass="pgr" />
                                <AlternatingRowStyle CssClass="alt" />
                                <HeaderStyle HorizontalAlign="center" />
                                <Columns>
                                    <asp:BoundField DataField="Name" HeaderText="Alert Name" HeaderStyle-Width="200px"
                                        ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" HeaderStyle-Width="400px"
                                        ItemStyle-Width="400px" />
                                    <asp:TemplateField HeaderText="Enabled" ItemStyle-HorizontalAlign="left" HeaderStyle-Width="50px"
                                        ItemStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:HiddenField runat="server" ID="hdnCurrentContacts" Value='<%#Eval("ContactEmailIds") %>' />
                                            <asp:HiddenField runat="server" ID="hdnUpdateContacts" Value='<%#Eval("ContactEmailIds") %>' />
                                            <asp:CheckBox runat="server" ID="Ischecked" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="15px" HeaderStyle-Width="15px" ItemStyle-VerticalAlign="Top">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkEmailTo" Text="To:" CausesValidation="false"
                                                Enabled="false" CommandName="EmailTo" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="150px">
                                        <ItemTemplate>
                                            <div style="word-wrap: break-word;">
                                                <asp:Label runat="server" ID="lblEmailTo" Text='<%#Eval("EmailTo") %>'></asp:Label></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Conditions" ItemStyle-HorizontalAlign="center" ItemStyle-Width="100px"
                                        Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblInt" runat="server" Width="50px"></asp:Label>
                                            <asp:TextBox runat="server" ID="txtInt" Width="50px"></asp:TextBox>
                                            <br />
                                            <asp:Label ID="lblPer" runat="server" Width="50px"></asp:Label>
                                            <ig:WebPercentEditor MaxValue="100" ID="txtPer" runat="server" MinDecimalPlaces="2"
                                                Width="50px"></ig:WebPercentEditor>
                                            <br />
                                            <asp:Label ID="lblAmount" runat="server" Width="50px"></asp:Label>
                                            <ig:WebNumericEditor ID="txtAmount" runat="server" ValueText="0" Width="50px"></ig:WebNumericEditor>
                                            <br />
                                            <asp:Label ID="lblTinyint" runat="server" Width="50px"></asp:Label>
                                            <asp:RadioButtonList runat="server" ID="rdtinyint" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <ig:WebDialogWindow ID="dlgContacts" runat="server" Height="450px" InitialLocation="Centered"
                    Modal="True" Width="800px" WindowState="Hidden"><ContentPane ScrollBars="Hidden"><Template><uc1:wucAlertContacts ID="pnlAlertContacts" runat="server" /></Template></ContentPane><Header CaptionText="Alert Contacts"></Header></ig:WebDialogWindow>
    </asp:Panel>
    <asp:Label runat="server" ID="noData" Text="No Data..."></asp:Label>
</fieldset>
