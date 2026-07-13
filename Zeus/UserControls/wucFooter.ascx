<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucFooter.ascx.cs" Inherits="ZeusWeb.UserControls.wucFooter" %>
<div id="footer">
    <div class="menufooter">
        <div style="text-align: right; margin-top: 8px; margin-right: 5px">
            <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
        </div>
    </div>
</div>
<script type="text/javascript">

    $(document).ready(function () {

        // hide all side menu's that are not there, and that aren't "Horizontal Rules"
        $("#menuside li").each(function () {
            if (trim1($(this).text()) == "" && $(this).html().indexOf("<hr") == -1) {
                $(this).hide();
            }
        });

    });

</script>
