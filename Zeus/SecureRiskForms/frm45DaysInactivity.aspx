<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frm45DaysInactivity" Title="45 Days Inactivity" Codebehind="frm45DaysInactivity.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td align="left">
                <asp:Panel ID="Panel1" runat="server" Height="" Width="">
                    <fieldset>
                        <legend>45 Days Inactivity</legend>
                        <asp:Panel ID="pnlSearch" runat="server" Height="" Width="">
                            <table cellspacing="2">
                                <tr>
                                    <td class="lblRight">
                                        Date:</td>
                                    <td>
                                        <ig:WebDatePicker ID="SearchBeginDate" runat="server" EnableAppStyling="False"
                                            NullDateLabel="" Width="125px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                                        <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <igtxt:WebImageButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" Text="Search"
                                AccessKey="h">
                                <Appearance>
                                    <Image Url="~/Images/Check.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            &nbsp;
                            <igtxt:WebImageButton ID="btnClear" runat="server" OnClick="btnClear_Click" Text="Clear"
                                AccessKey="l">
                                <Appearance>
                                    <Image Url="~/Images/delete.png" />
                                </Appearance>
                            </igtxt:WebImageButton>
                            &nbsp;&nbsp;
                        </asp:Panel>
                    </fieldset>
                    <br />
                    <fieldset>
                        <legend>
                            <asp:Label ID="lblTitle" runat="server" Text="Resuts"></asp:Label></legend>
                        <asp:Panel runat="server" ID="pnl1" Width="">
                            <div style="height: 5px">
                            </div>
                            <div class="buckethdright">
                                <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server" Text=""></asp:Label>&nbsp;
                            </div>
                            <asp:Panel ID="pnlGrid" runat="server" Height="400px" Width="100%" ScrollBars="vertical"
                                onscroll="SetDivPosition()">
                                <asp:GridView ID="grd" runat="server" CssClass="mGrid" Width="99%">
                                    <FooterStyle CssClass="footer" />
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                </asp:GridView>
                            </asp:Panel>
                            <br />
                            <div class="bucketfooterleft">
                                <asp:LinkButton ID="btnExcel" runat="server" OnClick="btnExport_Click">
                                    <span style="height: 25px; vertical-align: middle;">
                                        <asp:Image ID="Image1" runat="server" SkinID="SaveExcel" /></span><span style="margin-left: 5px;">Save
                                            Excel</span></asp:LinkButton>&nbsp;&nbsp;
                            </div>
                        </asp:Panel>
                        <asp:Label runat="server" ID="NoData" Text="NoData..." Visible="false"></asp:Label>
                    </fieldset>
                </asp:Panel>
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">  
        var IsPostBack= '<%=IsPostBack.ToString() %>';
        window.onload = function () {
            var strCook = document.cookie;
            if (strCook.indexOf("!~") != 0) {
                var intS = strCook.indexOf("!~");
                var intE = strCook.indexOf("~!");
                var strPos = strCook.substring(intS + 2, intE);
                if (IsPostBack == 'True') {
                    if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                        document.getElementById("<%=pnlGrid.ClientID %>").scrollTop = strPos;
                    }
                }
                else {
                    document.cookie = "yPos=!~0~!";
                }
            }
        }  
        function SetDivPosition()
        {
            var intY = 0;
            if (document.getElementById("<%=pnlGrid.ClientID %>") != null) {
                document.getElementById("<%=pnlGrid.ClientID %>").scrollTop;
            }
            document.cookie = "yPos=!~" + intY + "~!";  
        }    
    </script>

</asp:Content>
