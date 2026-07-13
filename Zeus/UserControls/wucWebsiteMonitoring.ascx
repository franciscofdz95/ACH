<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucWebsiteMonitoring" Codebehind="wucWebsiteMonitoring.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div style="width: 98%">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                ShowSummary="False" ValidationGroup="WebsiteURL" />
            <asp:CheckBox ID="EnableWebsiteMonitoring" runat="server" Text="Decline transaction if website content changed" /><br />
            <asp:CheckBox ID="URLReferrerValidate" runat="server" Text="Decline transaction if source URL is not approved" /><br />
            <br />
            &nbsp;<table>
                <tr>
                    <td>
                        URL:</td>
                    <td>
                        <asp:TextBox ID="URL" runat="server" MaxLength="255" Width="450px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="URL"
                            Display="None" ErrorMessage="Please enter valid URL" ValidationExpression="(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"
                            ValidationGroup="WebsiteURL"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ErrorMessage=""></asp:CustomValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        URL Type:</td>
                    <td>
                        <asp:DropDownList ID="URLTypeID" runat="server">
                            <asp:ListItem Value="2">Source URL</asp:ListItem>
                            <asp:ListItem Value="1">Terms and Conditions URL</asp:ListItem>
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>
                        Crawl URL?:</td>
                    <td>
                        <asp:CheckBox ID="CrawlURL" runat="server" Checked="True" /></td>
                </tr>
                <tr>
                    <td>
                        Check TC?:</td>
                    <td>
                        <asp:CheckBox ID="CheckTC" runat="server" /></td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnAddURL" runat="server" Text="Add URL" OnClick="btnAddURL_Click"
                            ValidationGroup="WebsiteURL" /></td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="grd" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsWebsiteURLs" Width="100%"
                DataKeyNames="URLID" OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:BoundField DataField="URL" HeaderText="URL" />
                    <asp:BoundField DataField="btnAddURLID" Visible="False" />
                    <asp:BoundField DataField="URLType" HeaderText="URL Type" />
                    <asp:TemplateField HeaderText="Crawl URL">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCrawlURL" runat="server" AutoPostBack="True" OnCheckedChanged="chkCrawlURL_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Check TC">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCheckTC" runat="server" AutoPostBack="True" OnCheckedChanged="chkCheckTC_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Button ID="btnOn" runat="server" Text="On" CausesValidation="false" Width="75px" />
                            <asp:Button ID="btnOff" runat="server" Text="Off" CausesValidation="false" Width="75px" />
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnCrawler" runat="server" Text="Create Crawler Request" CausesValidation="false" />
                        </ItemTemplate>
                        <ItemStyle Width="150px" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkDelete" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsWebsiteURLs" runat="server" SelectMethod="GetWebsiteURLs"
                TypeName="PaymentXP.DataObjects.DataRisk" OldValuesParameterFormatString="original_{0}"
                OnSelecting="odsWebsiteURLs_Selecting">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <table width="100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="URLReferrer" runat="server" Visible="false"></asp:Label></td>
                    <td align="right">
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="CustomValidator2" runat="server" Display="Dynamic" ErrorMessage=""></asp:CustomValidator><br />
            <br />
            <table width="100%">
                <tr>
                    <td align="left">
                        <b>CRAWLER HISTORY</b></td>
                    <td align="right">
                        Filter:
                        <asp:DropDownList ID="lstURLs" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lstURLs_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grd2" runat="server" AutoGenerateColumns="False" Font-Names="Verdana"
                Font-Size="X-Small" CssClass="mGrid" DataSourceID="odsCrawlers" Width="100%"
                DataKeyNames="CrawlerID" OnRowDataBound="grd2_RowDataBound" OnRowCommand="grd2_RowCommand"
                AllowPaging="True" OnPageIndexChanging="grd2_PageIndexChanging">
                <PagerStyle CssClass="pgr" />
                <AlternatingRowStyle CssClass="alt" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnEnbaled" runat="server" Text="Button" CausesValidation="false"
                                Width="75px" />
                        </ItemTemplate>
                        <ItemStyle Width="75px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CrawlerID" HeaderText="ID" />
                    <asp:TemplateField HeaderText="URL">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkURL" runat="server" Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Snap Shot">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkScreenShot" runat="server" Target="_blank">View</asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Report">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkReport" runat="server" Target="_blank">View</asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="HTML">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkHTML" runat="server" Target="_blank">View</asp:HyperLink>
                        </ItemTemplate>
                        <ItemStyle Width="75px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="StatusName" HeaderText="Status">
                        <ItemStyle Width="85px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ContentSize" HeaderText="Content Size">
                        <ItemStyle Width="85px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateRequested" HeaderText="Date Requested">
                        <ItemStyle Width="85px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="DateProcessed" HeaderText="Date Processed">
                        <ItemStyle Width="85px" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="odsCrawlers" runat="server" SelectMethod="GetCrawlersPaging"
                TypeName="PaymentXP.DataObjects.DataRisk" OnSelecting="odsCrawlers_Selecting"
                MaximumRowsParameterName="PageSize" SelectCountMethod="GetCrawlersPagingCount"
                StartRowIndexParameterName="CurrentPage" EnablePaging="True">
                <SelectParameters>
                    <asp:Parameter Name="prms" Type="Object" />
                    <asp:Parameter Name="PageSize" Type="Int32" />
                    <asp:Parameter Name="CurrentPage" Type="Int32" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CausesValidation="false"
                OnClick="btnRefresh_Click" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
