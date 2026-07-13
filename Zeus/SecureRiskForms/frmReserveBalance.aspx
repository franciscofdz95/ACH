<%@ Page Title="Reserve Balances" Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="True"
    CodeBehind="frmReserveBalance.aspx.cs" Inherits="ZeusWeb.frmReserveBalance" %>

<%@ Register Src="~/UserControls/Reserve/wucSummaryGrid.ascx" TagName="wucSummaryGrid"
    TagPrefix="uc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/jscript">

        $(document).ready(function () {

            //init date pickers
            $("#<%=AsOfDate.ClientID %> ").datepicker();
        });
    </script>
    <fieldset>
        <legend>Reserve Balances</legend>

        <div>

            <span style="float: left;">As of
                <asp:TextBox ID="AsOfDate" Width="90px" runat="server"></asp:TextBox><asp:Button ID="ApplyAsOfDate" Text="Apply" runat="server" OnClick="ApplyAsOfDate_Click" />
            </span>
            <span style="float: right;">
                <asp:CheckBox runat="server" ID="cbRHAM" Checked="true" AutoPostBack="true" Text="Include Reserves Held at Paysafe" OnCheckedChanged="cbRHAM_CheckedChanged" />
            </span>


        </div>




        <uc1:wucSummaryGrid ID="wucSummaryGrid1" HasJournal="True" runat="server" />
    </fieldset>
</asp:Content>
