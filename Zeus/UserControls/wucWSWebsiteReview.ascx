<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucWSWebsiteReview.ascx.cs" Inherits="ZeusWeb.UserControls.wucWSWebsiteReview" %>
<%@ Register Src="wucMessage.ascx" TagName="wucMessage" TagPrefix="uc2" %>
<uc2:wucMessage ID="wucWsReviewMessage2" runat="server" />
<style>
    .mGrid td {
        padding: 0px;
        border: solid 1px #c1c1c1;
        color: #000000;
    }

    .mGrid td {
        padding: 2px 0 2px 0;
    }
</style>
<asp:Panel ID="pnlWebsiteReview" runat="server">
    <div style="padding-left: 100px; padding-top: 30px; padding-right: 30px;">
        <div style="width: 100%; text-align: center">
            <h2><b>Website Review</b></h2>
        </div>
        <div style="width: 100%">
            <asp:Label ID="lh6" runat="server">DM: Number URLs Submitting to CU to Date</asp:Label>
            <asp:TextBox ID="rh6" runat="server" Width="100%"></asp:TextBox>
        </div>
        <table style="width: 100%" class="mGrid" border="1" cellspacing="0">
            <tbody>
                <tr>
                    <td style="width: 100%;">
                        <asp:Label ID="lq1" runat="server">Is the website active?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq1" runat="server" Width="200px" onchange="ShowQuestion(1)">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq2">
                    <td>
                        <asp:Label ID="lq2" runat="server">Is the Website domain Public? </asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq2" runat="server" Width="200px">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>

                <tr id="tq3">
                    <td>
                        <asp:Label ID="lq3" runat="server">Is the website registered to the merchant of record?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_lq3" runat="server" Width="200px">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq3a">
                    <td colspan="2">
                        <asp:TextBox ID="rq3" runat="server" Width="100%" maxlength="120"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq4">
                    <td>
                        <asp:Label ID="lq4" runat="server">Is the website's content password-protected?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq4" runat="server" Width="200px">
                            <asp:ListItem Value="No">No</asp:ListItem>
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq5">
                    <td>
                        <asp:Label ID="lq5" runat="server">Does the website redirect the viewer to an unrelated website?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq5" runat="server" Width="200px">
                            <asp:ListItem Value="No">No</asp:ListItem>
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq6">
                    <td>
                        <asp:Label ID="lq6" runat="server">Provide unrelated URL(s):</asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq6a">
                    <td colspan="2">
                        <asp:TextBox ID="rq6" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq7">
                    <td>
                        <asp:Label ID="lq7" runat="server">Is the website intended for e-commerce?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq7" runat="server" Width="200px" onchange="ShowQuestion(7)">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq8">
                    <td>
                        <asp:Label ID="lq8" runat="server">The DBA Name and Legal Name on the application must be listed on the website.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq8" runat="server" Width="200px">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq9">
                    <td>
                        <asp:Label ID="lq9" runat="server">The products/services offered must be clearly described. Ingredients list/link, if applicable, must be provided</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq9" runat="server" Width="200px">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq10">
                    <td>
                        <asp:Label ID="lq10" runat="server">The use of endorsements, TV/media clips, trademarks, logos, or likenesses associated with companies or individuals (ex: Oprah, Dr. OZ, Google, MSNBC, Facebook, etc.) is prohibhited unless the merchant has the written consent of the entity being published.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq10" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq11">
                    <td>
                        <asp:Label ID="lq11" runat="server">Unrealistic, false, or misleading claims made on the website are prohibited. (All claims must be substantiated directly on the website, which can include links, or by providing supporting documents.)</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq11" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq12">
                    <td>
                        <asp:Label ID="lq12" runat="server">Conveying a false sense of urgency that causes a customer to unjustly rush to place an order (ex: countdown timer, product availability counter, ""X number available per day"", etc.) is prohibited.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq12" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq13">
                    <td>
                        <asp:Label ID="lq13" runat="server">The website's order checkout page must be present, functional, secure and lists full business address including country. i.e. USA.  Any expired certificate must be updated.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq13" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq14">
                    <td>
                        <asp:Label ID="lq14" runat="server">The billing descriptor is disclosed on the checkout page of the website. For <b>5968/5964</b> - Descriptor must be the first 22 characters of URL after the prefix</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq14" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq15">
                    <td>
                        <asp:Label ID="lq15" runat="server">Hidden/forced up-sells are prohibited (including a pre-selected sale option).  Cardholders must re-enter their card data, delivery address and select the order/submit button prior to purchasing any up-sells. Initial purchase amount and related costs are totaled in a single sale</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq15" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq16">
                    <td>
                        <asp:Label ID="lq16" runat="server">Cross-sells of products/services provided by other vendors are prohibited.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq16" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq17">
                    <td>
                        <asp:Label ID="lq17" runat="server">The delivery methods and shipping times must be clearly stated.  Deliveries are available to the United States.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq17" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq18">
                    <td>
                        <asp:Label ID="lq18" runat="server">The products/services must be clearly priced and listed with a currency symbol, using numerical format. (This requirement applies to the full website, including the Terms & Conditions.)</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq18" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq19">
                    <td>
                        <asp:Label ID="lq19" runat="server">The Terms & Conditions must be clearly described and easy-to-read (limited usage of italics, 12pt font size, no confusing verbiage, large and easy to navigate box/window if applicable, or not obscured by any surrounding color contrast or distracting graphics). <b>Hyperlinks to Terms & Conditions and Privacy Policy must be on the Terms on the checkout page</b></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq19" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq20">
                    <td>
                        <asp:Label ID="lq20" runat="server">The Refund/Return Policy must be present. Return address must match what is on record.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq20" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq21">
                    <td>
                        <asp:Label ID="lq21" runat="server">The Privacy Policy must be present.  The sharing of card data with other vendors is prohibited such as provisions that auto enrolls cardholders in auto-dialing/Robocalls</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq21" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq22">
                    <td>
                        <asp:Label ID="lq22" runat="server">A toll-free customer service telephone number must be listed on the website, operational, and based out of the country where the company is located.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq22" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq23">
                    <td>
                        <asp:Label ID="lq23" runat="server">One of the following must be available on the website and must match what is on record: business mailing address, e-mail address, online chat.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq23" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq25b">
                    <td>
                        <asp:Label ID="lb25b" runat="server">Are there accepted card brands that are visible in the checkout page?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq25b" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq24">
                    <td colspan="2">
                        <asp:TextBox ID="rq24" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq25">
                    <td colspan="2">
                        <asp:TextBox ID="rq25" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq26">
                    <td>
                        <asp:Label ID="lq26" runat="server">Does the merchant offer a trial or have a negative option continuity offer?</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq26" runat="server" Width="200px" onchange="ShowQuestion(26)">
                            <asp:ListItem Value="Yes">Yes</asp:ListItem>
                            <asp:ListItem Value="No">No</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq27">
                    <td>
                        <asp:Label ID="lq27" runat="server">The trial period must begin on the date that the product is received by the cardholder and the trial offers must allow at least 4 days for shipping.  Disclosure of shipping times must be separate from the trial period.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq27" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq28">
                    <td>
                        <asp:Label ID="lq28" runat="server">How long is the trial period?</asp:Label>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq28a">
                    <td colspan="2">
                        <asp:TextBox ID="rq28" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq29">
                    <td>
                        <asp:Label ID="lq29" runat="server">The trial period is within 7 to 30 days.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq29" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq30">
                    <td>
                        <asp:Label ID="lq30" runat="server">For Direct Marketing: No more than 1 trial option at the checkout page or recurring plans greater than 30 Days.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq30" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq31">
                    <td>
                        <asp:Label ID="lq31" runat="server">For Direct Marketing: Billing models across the MLE must be consistent (SS Only, CO Only, Mixed Billing Model)</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq31" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq32">
                    <td>
                        <asp:Label ID="lq32" runat="server">The merchant is prohibited from marketing its products/services as ""risk-free"", ""free"", or ""no cost"".</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq32" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq33">
                    <td>
                        <asp:Label ID="lq33" runat="server">The terms of the sale must be placed directly above or to the left of the order/submit button.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq33" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq34">
                    <td>
                        <asp:Label ID="lq34" runat="server">The merchant must have a functional Opt-In feature present next to hyperlinks to Terms & Conditions and Privacy Policy , located above the order/submit button on the order checkout page, that requires customers to agree to the terms of the sale.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq34" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq35">
                    <td>
                        <asp:Label ID="lq35" runat="server">The terms being disclosed on the order checkout page must include Payment Page Terms and Conditions** verbiage on the checkout pages. Please see below for what the trial offer Terms vebiage must include.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq35" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq36">
                    <td>
                        <asp:Label ID="lq36" runat="server">Full product price is not billed twice in a 30-day period</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq36" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq37">
                    <td>
                        <asp:Label ID="lq37" runat="server">The complete terms on the order checkout page must be consistent with the Terms & Conditions. Terms of sale on the checkout page must reflect only the selected purchase option. Customers should not have the option to agree to terms prior to the offer selection</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq37" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq38">
                    <td>
                        <asp:Label ID="lq38" runat="server">The Cancellation Policy must be present. The merchant must provide a direct link to an online cancellation procedure for recurring payment transactions on the website where the cardholder made the initial purchase.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq38" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq39">
                    <td>
                        <asp:Label ID="lq39" runat="server">The merchant cannot require customers to return their ""trial"" sample to receive a refund or cancel a subscription.</asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="rq39" runat="server" Width="200px">
                            <asp:ListItem Value="Pass">Pass</asp:ListItem>
                            <asp:ListItem Value="Fail">Fail</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="tq40">
                    <td colspan="2">
                        <asp:TextBox ID="rq40" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq41">
                    <td colspan="2">
                        <asp:TextBox ID="rq41" runat="server" Width="100%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr id="tq42">
                    <td colspan="2">
                        <asp:Label ID="lq42" runat="server"><b>Additional Comments:</b><br />
                Website URL:
                        </asp:Label>
                    </td>
                </tr>
                <tr id="tq42a">
                    <td colspan="2">
                        <asp:TextBox ID="rq42" runat="server" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr id="tq43">
                    <td colspan="2">
                        <asp:Label ID="lq43" runat="server"><b>**Payment Page Terms and Conditions Verbiage:</b><br />
                o Your trial will begin upon receipt of PRODUCT <br />
o For all MasterCard transactions ONLY: within X DAYS, you will receive an email requiring your response to activate the monthly auto-ship program. If you are happy with PRODUCT, you are required to consent to the monthly auto-ship program in order to receive additional PRODUCT</asp:Label>
                    </td>
                </tr>
            </tbody>
        </table>
        <asp:Button ID="pdfbtn" runat="server" Text="Submit" OnClick="btnGeneratePdf_Click" />
    </div>
</asp:Panel>
<script language="javascript" type="text/jscript">
	function ShowQuestion(val) {
		if (val == 1) {
			var r1 = $("#ContentPlaceHolder1_wucWSWebsiteReview1_rq1").val();
			if (r1 == 'Yes') {
				$("#tq2").show();
				$("#tq3").show();
				$("#tq3a").show();
				$("#tq4").show();
				$("#tq5").show();
				$("#tq6").show();
				$("#tq6a").show();
				$("#tq7").show();
			} else {
				$("#tq2").hide();
				$("#tq3").hide();
				$("#tq3a").hide();
				$("#tq4").hide();
				$("#tq5").hide();
				$("#tq6").hide();
				$("#tq6a").hide();
				$("#tq7").hide();
				$("#ContentPlaceHolder1_wucWSWebsiteReview1_rq7").val("No").change();
			}
		}
		if (val == 7) {
			var r7 = $("#ContentPlaceHolder1_wucWSWebsiteReview1_rq7").val();
			if (r7 == 'Yes') {
				$("#tq8").show();
				$("#tq9").show();
				$("#tq10").show();
				$("#tq11").show();
				$("#tq12").show();
				$("#tq13").show();
				$("#tq14").show();
				$("#tq15").show();
				$("#tq16").show();
				$("#tq17").show();
				$("#tq18").show();
				$("#tq19").show();
				$("#tq20").show();
				$("#tq21").show();
				$("#tq22").show();
				$("#tq23").show();
				$("#tq24").show();
				$("#tq25").show();
				$("#tq26").show();
			} else {
				$("#tq8").hide();
				$("#tq9").hide();
				$("#tq10").hide();
				$("#tq11").hide();
				$("#tq12").hide();
				$("#tq13").hide();
				$("#tq14").hide();
				$("#tq15").hide();
				$("#tq16").hide();
				$("#tq17").hide();
				$("#tq18").hide();
				$("#tq19").hide();
				$("#tq20").hide();
				$("#tq21").hide();
				$("#tq22").hide();
				$("#tq23").hide();
				$("#tq24").hide();
				$("#tq25").hide();
				$("#tq26").hide();
				$("#ContentPlaceHolder1_wucWSWebsiteReview1_rq26").val("No").change();
			}
		}
		if (val == 26) {
			var r26 = $("#ContentPlaceHolder1_wucWSWebsiteReview1_rq26").val();
			if (r26 == 'Yes') {
				$("#tq27").show();
				$("#tq28").show();
				$("#tq28a").show();
				$("#tq29").show();
				$("#tq30").show();
				$("#tq31").show();
				$("#tq32").show();
				$("#tq33").show();
				$("#tq34").show();
				$("#tq35").show();
				$("#tq36").show();
				$("#tq37").show();
				$("#tq38").show();
				$("#tq39").show();
				$("#tq40").show();
				$("#tq41").show();
			} else {
				$("#tq27").hide();
				$("#tq28").hide();
				$("#tq28a").hide();
				$("#tq29").hide();
				$("#tq30").hide();
				$("#tq31").hide();
				$("#tq32").hide();
				$("#tq33").hide();
				$("#tq34").hide();
				$("#tq35").hide();
				$("#tq36").hide();
				$("#tq37").hide();
				$("#tq38").hide();
				$("#tq39").hide();
				$("#tq40").hide();
				$("#tq41").hide();
			}
		}
	}
</script>
