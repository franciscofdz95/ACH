<%@ Page Language="C#" MasterPageFile="~/MasterPageHome.master" AutoEventWireup="true"
    Inherits="frmHome" Title="Zeus Home" CodeBehind="frmHome.aspx.cs"  %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ MasterType VirtualPath="~/MasterPageHome.master" %>
<%@ Register Src="../UserControls/wucTicketGridGeneral.ascx" TagName="wucTicketGridGeneral"
    TagPrefix="uc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            loadfrmhome();

        });


        //        this is a really stupid bug. basically, when you're using jquery calls mixed with microsoft ajax (ie update panel stuff), the jquery calls dont work
        //        anymore after and update panel is triggered. because the jquery does not know when the request ended. so the fix is to manually call the jquery code
        //        when the end request event is fired by the update panel.
        //        
        //        http://zeemalik.wordpress.com/2007/11/27/how-to-call-client-side-javascript-function-after-an-updatepanel-asychronous-ajax-request-is-over/

        function loadfrmhome() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(homeEndRequestHandler);
        }

        function homeEndRequestHandler() {

            $("[id*='ui-tooltip-']").remove();
            $('.zeustooltip').tooltip({
                content: function () {
                    return $(this).prop('title');
                }
            });
        }
    </script>

    <div id="contentpage">
        
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Button runat="server" ID="btnRefresh" OnClick="btnRefresh_Click1" Text="Refresh Screen" />
                <fieldset>
                    <legend>Open/Pending Department Tickets</legend>
                    <uc1:wucTicketGridGeneral ID="wucTicketGridGeneral1" runat="server" />
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>
      <%--  <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <uc2:wucTicketGridSummary ID="wucTicketGridSummary1" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>

    <script type="text/javascript">
        homeEndRequestHandler();
    </script>


</asp:Content>
