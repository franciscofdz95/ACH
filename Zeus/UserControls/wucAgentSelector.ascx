<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="Control_wucAgentSelector" CodeBehind="wucAgentSelector.ascx.cs" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/UserControls/wucAgent.ascx" TagName="wucAgent" TagPrefix="uc2" %>
    
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetails" runat="server">
    
    <asp:Panel runat="server" CssClass="AgentSelectorHorizontal" ID="pnlAgent">
        <div class="ASDIVAgentDBA">
            <asp:Label CssClass="ASLBLAgentDBA lblspace" ID="lblAgentDBA" Text="Agent DBA:" runat="server"></asp:Label>
            <asp:TextBox CssClass="ASTBAgentDBA" ID="AgentDBA" runat="server" ReadOnly="true"></asp:TextBox>
            <asp:HiddenField runat="server" ID="AgentDBA1" />
        </div>
        <div class="ASDIVAgentID">
            <asp:Label CssClass="ASLBLAgentID lblspace" ID="lblAgentID" Text="Agent ID:" runat="server"></asp:Label>
            <asp:TextBox CssClass="ASTBAgentID" ID="AgentID" runat="server"></asp:TextBox>
            <asp:LinkButton OnClick="btnAgentSelect_Click" CausesValidation="false" ID="lbSelectAgent"
                runat="server">Select</asp:LinkButton>
           
        </div>
    </asp:Panel>
    
    <div style="display:none;">
    <asp:TextBox ID="AgentSelectorMasterAgentUID" runat="server"></asp:TextBox>
    <asp:TextBox ID="AgentUID" runat="server"></asp:TextBox>
    </div>
    
    <%--<asp:HiddenField ID="AgentUID" runat="server"/>--%>
    
    <ig:WebDialogWindow ID="dlgAgent" runat="server" Height="500px" Width="700px" Modal="true"
        InitialLocation="Centered" WindowState="Hidden">
        <ContentPane>
            <Template>
                <uc2:wucAgent ID="grdAgent" runat="server" />
            </Template>
        </ContentPane>
        <Header CaptionText="Agents">
        </Header>
    </ig:WebDialogWindow>
    
    <script type="text/javascript">
        $(document).ready(function () {

            
            $('#<%= AgentID.ClientID %>').on("keydown", function (e) {
                if (e.keyCode == '13') {
                    e.preventDefault();
                }


                //backspace and delete
                if (e.keyCode == '8' || e.keyCode == '46') {
                    $('#<%= AgentUID.ClientID %>').val('');
                    $('#<%= AgentDBA.ClientID %>').val('');
                }

            });
        


            $('#<%= AgentID.ClientID %>').on({
                cut: function () {
                    $('#<%= AgentUID.ClientID %>').val('');
                    $('#<%= AgentDBA.ClientID %>').val('');
                }
            });


            $('#<%= AgentID.ClientID %>').on("keyup", function (e) {

                SearchAgent();
            });

            function SearchAgent() {
                var id = $('#<%= AgentID.ClientID %>').val();
                var masteragentuid = $('#<%= AgentSelectorMasterAgentUID.ClientID %>').val();

                $.ajax({
                    type: "POST",
                    url: "../ajax/AjaxWebservice.asmx/GetAgent",
                    data: "{id: " + id + ",AgentSelectorMasterAgentUID: '" + masteragentuid + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        $('#<%= AgentDBA.ClientID %>').val(msg.d.AgentDBA);
                        $('#<%= AgentUID.ClientID %>').val(msg.d.AgentUID);
                        $('#<%= AgentDBA1.ClientID %>').val(msg.d.AgentDBA);
                    }

                });
            }

          

        });
    </script>

</asp:Panel>

    </ContentTemplate>
</asp:UpdatePanel>
