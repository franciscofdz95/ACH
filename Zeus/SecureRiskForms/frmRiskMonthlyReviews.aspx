<%@ Page Language="C#" MasterPageFile="~/MasterPageRisk.master" AutoEventWireup="true" Inherits="frmRiskMonthlyReviews" Title="Risk Monthly Reviews" Codebehind="frmRiskMonthlyReviews.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    
        <fieldset class="dialog" style="width: 40%;">
            <legend>Monthly Reviews</legend>
           <asp:GridView ID="grdReviews" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                            Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                            DataSourceID="odsgrdReviewsSource">
                            <Columns>
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="Name,Type" DataNavigateUrlFormatString="~/forms/riskreviews/{0}.{1}"
                                    Target="_blank" DataTextField="Name" HeaderText="Report Name" SortExpression="Name" />
                                <asp:BoundField DataField="Size" HeaderText="Size" SortExpression="Size">
                                    <ItemStyle HorizontalAlign="Center" Width="200px" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="odsgrdReviewsSource" runat="server" OldValuesParameterFormatString="original_{0}"
                            SelectMethod="GetForms" TypeName="LocalForms">
                            <SelectParameters>
                                <asp:Parameter DefaultValue="~/forms/riskreviews" Name="path" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
        </fieldset>
    
</asp:Content>
