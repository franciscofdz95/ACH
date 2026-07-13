<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="wucAddtoOutlook" Codebehind="wucAddtoOutlook.ascx.cs" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="Infragistics45.WebUI.WebDataInput.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.WebUI.WebDataInput" TagPrefix="igtxt" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.EditorControls" TagPrefix="ig" %>
<%@ Register Assembly="Infragistics45.Web.v19.2, Version=19.2.20192.8, Culture=neutral, PublicKeyToken=7dd5c3163f2cd0cb" Namespace="Infragistics.Web.UI.LayoutControls" TagPrefix="ig" %>
<script type="text/Javascript">

    function CheckActive(lnk)
    {
        var lnk1 = document.getElementById(lnk);
        
        if(lnk.src.indexOf('checkbox'))
        {
            lnk.src = "../Images/Uncheck.JPG";
            if(confirm('Are you sure, you want to delete the appointment?') == false)
            {
                lnk.src = "../Images/checkbox.JPG";
                return false;
            }
            return true;
        }
        else
        {
            lnk.src = "../Images/checkbox.JPG";
            return false;
        }
    }

    // NOTE: this is currently not used.. used an update panel instead :)
    function sync_end_date() {
        
        // since we're using infragistics, they create the calendar using a table with nested inputs. 
        // i dont have time to dig into it to find the proper ClientID of the input element.
        // so i'm hard coding for now. keep an eye on this.
        var start_date_input = '#ctl00_ContentPlaceHolder1_WebDialogWindow5_tmpl_wucOutlook1_StartDateTime_input';
        var end_date_input = '#ctl00_ContentPlaceHolder1_WebDialogWindow5_tmpl_wucOutlook1_EndDateTime_input';
        var start_time_input = '#ctl00_ContentPlaceHolder1_WebDialogWindow5_tmpl_wucOutlook1_StartTime';
        var end_time_input = '#ctl00_ContentPlaceHolder1_WebDialogWindow5_tmpl_wucOutlook1_EndTime';
        
        // the new start date
        var new_start_date = $(start_date_input).val();
        
        // set the new enddate to the new start date.
        $(end_date_input).val(new_start_date);
        
        // getting the value of the start time.
        // must include the radix to get it right!
        var new_start_time = parseInt( $(start_time_input).val(), 10);
        
        // default new end time to new start time
        var new_end_time = new_start_time + 0;
        
        // increment value of 1 hour
        var increment_value = 10000;
        
        //alert('current start time input: ' +  $(start_time_input).val());
        //alert('current start time: ' + new_start_time);
        
        if( new_start_time >= 230000) {
        
            alert('no change');
        
            // kinda complicated. we dont do anything if the time is 11:00pm or 11:30pm. we'll need to increment
            // the day also. not impossible but just crunched for time.
            if( var_start_time == 230000 ) {
                // var_end_time = 000000;
                // TODO: roll over to next day
            } else if( var_start_time == 233000 ) {
                // var_end_time = 003000;
                // TODO: roll over to next day
            }
        
        } else {
        
            
            new_end_time = new_end_time + increment_value;
            
            new_end_time = '000000' + new_end_time;
            
            //alert(new_end_time);
            
            //alert(new_end_time.length)
            
            //new_end_time = new_end_time.substring( new_end_time.length - 6, 6);
            
            new_end_time = new_end_time.substring( new_end_time.length - 6);
            
            
            //alert('new end time: ' + new_end_time);
        }
        
        // setting our new end time.
        $(end_time_input).val(new_end_time);
        
    }
    
    function checkActive(chk)
    {
      return ;                       
    }
    
</script>

<fieldset class="add-to-outlook">
    <legend>Follow Up</legend>
    <asp:Panel ID="pnlAppointments" CssClass="bucketbdy" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Label runat="server" ID="ErrorMess" Font-Bold="True" Visible="false" ForeColor="red"></asp:Label>
                <div class="fieldblock">
                    <div class="fblabel">
                        Start Time:
                    </div>
                    <ig:WebDatePicker ID="StartDateTime" runat="server" EnableAppStyling="False"
                        AutoPostBack-ValueChanged="true" CssClass="fbdate" NullDateLabel="" Width="105px"
                        BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    <asp:DropDownList ID="StartTime" runat="server" CssClass="fbtime" AutoPostBack="true"
                        OnSelectedIndexChanged="StartTime_SelectedIndexChanged">
                        <asp:ListItem Value="000000">12:00 AM</asp:ListItem>
                        <asp:ListItem Value="003000">12:30 AM</asp:ListItem>
                        <asp:ListItem Value="010000">1:00 AM</asp:ListItem>
                        <asp:ListItem Value="013000">1:30 AM</asp:ListItem>
                        <asp:ListItem Value="020000">2:00 AM</asp:ListItem>
                        <asp:ListItem Value="023000">2:30 AM</asp:ListItem>
                        <asp:ListItem Value="030000">3:00 AM</asp:ListItem>
                        <asp:ListItem Value="033000">3:30 AM</asp:ListItem>
                        <asp:ListItem Value="040000">4:00 AM</asp:ListItem>
                        <asp:ListItem Value="043000">4:30 AM</asp:ListItem>
                        <asp:ListItem Value="050000">5:00 AM</asp:ListItem>
                        <asp:ListItem Value="053000">5:30 AM</asp:ListItem>
                        <asp:ListItem Value="060000">6:00 AM</asp:ListItem>
                        <asp:ListItem Value="063000">6:30 AM</asp:ListItem>
                        <asp:ListItem Value="070000">7:00 AM</asp:ListItem>
                        <asp:ListItem Value="073000">7:30 AM</asp:ListItem>
                        <asp:ListItem Value="080000">8:00 AM</asp:ListItem>
                        <asp:ListItem Value="083000">8:30 AM</asp:ListItem>
                        <asp:ListItem Value="090000">9:00 AM</asp:ListItem>
                        <asp:ListItem Value="093000">9:30 AM</asp:ListItem>
                        <asp:ListItem Value="100000">10:00 AM</asp:ListItem>
                        <asp:ListItem Value="103000">10:30 AM</asp:ListItem>
                        <asp:ListItem Value="110000">11:00 AM</asp:ListItem>
                        <asp:ListItem Value="113000">11:30 AM</asp:ListItem>
                        <asp:ListItem Value="120000">12:00 PM</asp:ListItem>
                        <asp:ListItem Value="123000">12:30 PM</asp:ListItem>
                        <asp:ListItem Value="130000">1:00 PM</asp:ListItem>
                        <asp:ListItem Value="133000">1:30 PM</asp:ListItem>
                        <asp:ListItem Value="140000">2:00 PM</asp:ListItem>
                        <asp:ListItem Value="143000">2:30 PM</asp:ListItem>
                        <asp:ListItem Value="150000">3:00 PM</asp:ListItem>
                        <asp:ListItem Value="153000">3:30 PM</asp:ListItem>
                        <asp:ListItem Value="160000">4:00 PM</asp:ListItem>
                        <asp:ListItem Value="163000">4:30 PM</asp:ListItem>
                        <asp:ListItem Value="170000">5:00 PM</asp:ListItem>
                        <asp:ListItem Value="173000">5:30 PM</asp:ListItem>
                        <asp:ListItem Value="180000">6:00 PM</asp:ListItem>
                        <asp:ListItem Value="183000">6:30 PM</asp:ListItem>
                        <asp:ListItem Value="190000">7:00 PM</asp:ListItem>
                        <asp:ListItem Value="193000">7:30 PM</asp:ListItem>
                        <asp:ListItem Value="200000">8:00 PM</asp:ListItem>
                        <asp:ListItem Value="203000">8:30 PM</asp:ListItem>
                        <asp:ListItem Value="210000">9:00 PM</asp:ListItem>
                        <asp:ListItem Value="213000">9:30 PM</asp:ListItem>
                        <asp:ListItem Value="220000">10:00 PM</asp:ListItem>
                        <asp:ListItem Value="223000">10:30 PM</asp:ListItem>
                        <asp:ListItem Value="230000">11:00 PM</asp:ListItem>
                        <asp:ListItem Value="233000">11:30 PM</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="fieldblock">
                    <div class="fblabel">
                        End Time:
                    </div>
                    <ig:WebDatePicker ID="EndDateTime" runat="server" EnableAppStyling="False" NullDateLabel=""
                        CssClass="fbdate" Width="105px" BackColor="#EFF3FF" BorderStyle="Solid" BorderWidth="1px">
                    <CalendarAnimation FadeCloseDuration="1" FadeOpenDuration="1" SlideCloseDuration="1" SlideOpenDuration="1" /></ig:WebDatePicker>
                    <asp:DropDownList ID="EndTime" runat="server" CssClass="fbtime">
                        <asp:ListItem Value="000000">12:00 AM</asp:ListItem>
                        <asp:ListItem Value="003000">12:30 AM</asp:ListItem>
                        <asp:ListItem Value="010000">1:00 AM</asp:ListItem>
                        <asp:ListItem Value="013000">1:30 AM</asp:ListItem>
                        <asp:ListItem Value="020000">2:00 AM</asp:ListItem>
                        <asp:ListItem Value="023000">2:30 AM</asp:ListItem>
                        <asp:ListItem Value="030000">3:00 AM</asp:ListItem>
                        <asp:ListItem Value="033000">3:30 AM</asp:ListItem>
                        <asp:ListItem Value="040000">4:00 AM</asp:ListItem>
                        <asp:ListItem Value="043000">4:30 AM</asp:ListItem>
                        <asp:ListItem Value="050000">5:00 AM</asp:ListItem>
                        <asp:ListItem Value="053000">5:30 AM</asp:ListItem>
                        <asp:ListItem Value="060000">6:00 AM</asp:ListItem>
                        <asp:ListItem Value="063000">6:30 AM</asp:ListItem>
                        <asp:ListItem Value="070000">7:00 AM</asp:ListItem>
                        <asp:ListItem Value="073000">7:30 AM</asp:ListItem>
                        <asp:ListItem Value="080000">8:00 AM</asp:ListItem>
                        <asp:ListItem Value="083000">8:30 AM</asp:ListItem>
                        <asp:ListItem Value="090000">9:00 AM</asp:ListItem>
                        <asp:ListItem Value="093000">9:30 AM</asp:ListItem>
                        <asp:ListItem Value="100000">10:00 AM</asp:ListItem>
                        <asp:ListItem Value="103000">10:30 AM</asp:ListItem>
                        <asp:ListItem Value="110000">11:00 AM</asp:ListItem>
                        <asp:ListItem Value="113000">11:30 AM</asp:ListItem>
                        <asp:ListItem Value="120000">12:00 PM</asp:ListItem>
                        <asp:ListItem Value="123000">12:30 PM</asp:ListItem>
                        <asp:ListItem Value="130000">1:00 PM</asp:ListItem>
                        <asp:ListItem Value="133000">1:30 PM</asp:ListItem>
                        <asp:ListItem Value="140000">2:00 PM</asp:ListItem>
                        <asp:ListItem Value="143000">2:30 PM</asp:ListItem>
                        <asp:ListItem Value="150000">3:00 PM</asp:ListItem>
                        <asp:ListItem Value="153000">3:30 PM</asp:ListItem>
                        <asp:ListItem Value="160000">4:00 PM</asp:ListItem>
                        <asp:ListItem Value="163000">4:30 PM</asp:ListItem>
                        <asp:ListItem Value="170000">5:00 PM</asp:ListItem>
                        <asp:ListItem Value="173000">5:30 PM</asp:ListItem>
                        <asp:ListItem Value="180000">6:00 PM</asp:ListItem>
                        <asp:ListItem Value="183000">6:30 PM</asp:ListItem>
                        <asp:ListItem Value="190000">7:00 PM</asp:ListItem>
                        <asp:ListItem Value="193000">7:30 PM</asp:ListItem>
                        <asp:ListItem Value="200000">8:00 PM</asp:ListItem>
                        <asp:ListItem Value="203000">8:30 PM</asp:ListItem>
                        <asp:ListItem Value="210000">9:00 PM</asp:ListItem>
                        <asp:ListItem Value="213000">9:30 PM</asp:ListItem>
                        <asp:ListItem Value="220000">10:00 PM</asp:ListItem>
                        <asp:ListItem Value="223000">10:30 PM</asp:ListItem>
                        <asp:ListItem Value="230000">11:00 PM</asp:ListItem>
                        <asp:ListItem Value="233000">11:30 PM</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="fieldblockmulti">
                    <div class="fblabel">
                        Notes: (Required)
                    </div>
                    <div class="fbnotes">
                        <asp:TextBox ID="txtAppointmentNotes" TextMode="MultiLine" runat="server"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="fieldblock">
            <div class="fblabel">
                &nbsp;
            </div>
            <div class="fbbutton">
                <asp:Button ID="btnAddAppointment" Width="120px" runat="server" Text="Add"
                    OnClick="btnAddAppointment_Click" AccessKey="t" CausesValidation="False" />
                &nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click"
                    AccessKey="c" CausesValidation="False" />
            </div>
        </div>
        <asp:Panel runat="server" ID="pnlApp1" Visible="false" Height="200px">
            <br />
            <table width="96%">
                <tr>
                    <td>
                        <%--
                        Page Size:
                        <asp:DropDownList ID="cboPageSize" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cboPageSize_SelectedIndexChanged">
                            <asp:ListItem Selected="True">5</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                        </asp:DropDownList>--%>
                    </td>
                    <td align="right">
                        <asp:Label ID="lblRecordCount" SkinID="RecordCount" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:UpdatePanel ID="pnl1" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="grdAppointments" Width="100%" AutoGenerateColumns="False" runat="server"
                                    OnRowDataBound="grdAppointments_RowDataBound" DataKeyNames="AppointmentID" OnRowCommand="grdAppointments_RowCommand"
                                    Font-Names="Verdana" OnPageIndexChanging="grdAppointments_PageIndexChanging"
                                    Font-Size="X-Small" CssClass="mGrid" PagerStyle-CssClass="pgr" AlternatingRowStyle-CssClass="alt"
                                    AllowPaging="true" PageSize="2">
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="&#171;" LastPageText="&#187;" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" Visible="false">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkID" runat="server" CausesValidation="false" CommandName="ID"></asp:LinkButton>
                                                <asp:HiddenField ID="hdnRecordMode" runat="server" Value="New" />
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Start Date" DataField="StartDateTime" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="End Date" DataField="EndDateTime" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Notes" HeaderText="Notes"></asp:BoundField>
                                        <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MM/dd/yyyy hh:mm tt}">
                                            <ItemStyle Width="80px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Created" DataField="UserCreated">
                                            <ItemStyle Width="60px" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkActive" ImageUrl="~/images/checkbox.jpg" runat="server" CommandName="Active"
                                                    OnClientClick="return CheckActive(this);" ToolTip="Active" CausesValidation="false">
                                                </asp:ImageButton>
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="AppointmentID" Visible="False" />
                                    </Columns>
                                    <PagerStyle CssClass="pgr" />
                                    <AlternatingRowStyle CssClass="alt" />
                                </asp:GridView>
                                <asp:GridView ID="grd" Width="100%" AutoGenerateColumns="False" runat="server" Font-Names="Verdana"
                                    Font-Size="X-Small" AllowPaging="true">
                                    <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" FirstPageText="�" LastPageText="�" />
                                    <Columns>
                                        <asp:BoundField HeaderText="ID" ItemStyle-Width="30px" Visible="false"></asp:BoundField>
                                        <asp:BoundField HeaderText="Start Date" HeaderStyle-Width="70px">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="End Date" HeaderStyle-Width="70px">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Notes" HeaderStyle-Width="40px">
                                            <ItemStyle Width="230px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Date Created" HeaderStyle-Width="80px">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="User Created" HeaderStyle-Width="80px">
                                            <ItemStyle Width="85px" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="Active" HeaderStyle-Width="40px">
                                            <ItemStyle Width="60px" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <ig:WebDialogWindow ID="WebDialogWindow1" Modal="true" runat="server" Height="230px"
                                    Width="350px" InitialLocation="Centered" WindowState="Hidden">
                                    <ContentPane>
                                        <Template>
                                            <br />
                                            <asp:Label runat="server" ID="appErr" Text="" SkinID="Required"></asp:Label>
                                            <table style="vertical-align: middle;" align="center">
                                                <tr>
                                                    <td class="lblRight">
                                                        Start Date:
                                                    </td>
                                                    <td>
                                                        <ig:WebDateTimeEditor ID="igtbl_StartDateTime" runat="server" DisplayModeFormat="g"
                                                            EditModeFormat="g">
                                                        </ig:WebDateTimeEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        End Date:
                                                    </td>
                                                    <td>
                                                        <ig:WebDateTimeEditor ID="igtbl_EndDateTime" runat="server" DisplayModeFormat="g"
                                                            EditModeFormat="g">
                                                        </ig:WebDateTimeEditor>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        Notes:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtAppNotes" runat="server" Text="" TextMode="multiLine" Rows="3"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="lblRight">
                                                        Active:
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkAppNotes" runat="server" AutoPostBack="true" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:HiddenField ID="hdnAppId" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <br />
                                                        <asp:Button ID="btnOK" runat="server" Text="OK" Width="50px" OnClick="btnOK_Click"
                                                            CausesValidation="false" />
                                                        <asp:Button ID="btnCancelApp" runat="server" Text="Cancel" Width="60px" Style="text-align: center;"
                                                            OnClick="btnCancelApp_Click" CausesValidation="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </Template>
                                    </ContentPane>
                                    <Header CaptionText="Appointments">
                                    </Header>
                                </ig:WebDialogWindow>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:Panel>
</fieldset>
