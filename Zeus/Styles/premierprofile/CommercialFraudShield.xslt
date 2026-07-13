<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:prd="http://www.experian.com/ARFResponse">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->

  <!--
  *********************************************
  * Business Facts template
  *********************************************
  -->
  <xsl:template name="CommercialFraudShield">
  	<xsl:param name="reportName"/>
	<table class="section" width="100%" cellspacing="0">
		<thead>
			<tr>
				<th colspan="3" class="doubleheightTitle">
					<xsl:comment>For left side label</xsl:comment>
					<div class="titleLabel" style="float:left"><a name="CommercialFraudShield" style="height:16px"><a class="report_section_title">Commercial Fraud Shield</a></a></div>
					<a class="fraudBizIDLink" target="_top" href="../../search/showFraudSearchPage" style="float:right;height:16px;display:none">
						<div class="smallTitle">For a complete check on all application data click here.</div>
					</a>
					<div style="clear:both"></div>
					<div style="float:left">
						<span class="label indent1">Evaluation for:
							<!--<xsl:value-of select="$reportName"/>-->
							<!--<xsl:if test="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)!=''">
								<xsl:value-of select="', '"/>
							</xsl:if>-->
							<!--<xsl:choose>
							<xsl:when test="prd:CommercialFraudShieldSummary/prd:MatchingBusinessIndicator/@code='P'">
							--><xsl:call-template name="FormatAddressLine">
								<xsl:with-param name="businessName"><xsl:value-of select="$reportName"></xsl:value-of></xsl:with-param>
							    <xsl:with-param name="street1" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:StreetAddress)" />
							    <xsl:with-param name="city" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:City)" />
							    <xsl:with-param name="state" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:State)" />
							    <xsl:with-param name="zip" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:Zip)" />
							    <xsl:with-param name="zipExt" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ZipExtension)" />
							</xsl:call-template>
						    <!--</xsl:when>
						    <xsl:otherwise>
							<xsl:call-template name="FormatAddressLine">
								<xsl:with-param name="businessName"><xsl:value-of select="$reportName"></xsl:value-of></xsl:with-param>
							</xsl:call-template>
						    </xsl:otherwise>
						    </xsl:choose>
						--></span>
					</div>
				</th>
			</tr>
			<tr class="subtitle">
				<th colspan="2">Business Alerts</th>
				<th style="width:180px">Verification Triggers</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td colspan="2">	<!-- first column table -->
					<table class="firstColumn dataTable" border="0">
                        <colgroup class="label" style="width:200px"/>
                        <colgroup class="value" style="width:50px"/>
                        <colgroup class="" style="width:400px"/>
						<tbody>
	                        <tr style="height:40px">
								<td class="label" valign="middle" style="vertical-align:middle">Active Business Indicator:</td>
								<td valign="middle" style="vertical-align:middle">
									<div>
										<xsl:attribute name="class">
										<xsl:choose>
											<xsl:when test="prd:CommercialFraudShieldSummary[normalize-space(prd:ActiveBusinessIndicator/@code)='A']">
												<xsl:value-of select="'ActiveBusniessIndicator'"/>
											</xsl:when>
											<xsl:when test="prd:CommercialFraudShieldSummary[normalize-space(prd:ActiveBusinessIndicator/@code)='I']">
												<xsl:value-of select="'InActiveBusniessIndicator'"/>
											</xsl:when>
										</xsl:choose>
										</xsl:attribute>
									</div>
								</td>
								<td valign="middle" style="vertical-align:middle">
									<div><xsl:choose>
											<xsl:when test="prd:CommercialFraudShieldSummary[normalize-space(prd:ActiveBusinessIndicator/@code)='A']">
												Experian shows this business as active
											</xsl:when>
											<xsl:when test="prd:CommercialFraudShieldSummary[normalize-space(prd:ActiveBusinessIndicator/@code)='I']">
												Experian shows this business as inactive
											</xsl:when>
										</xsl:choose></div>
								</td>
							</tr>
							<xsl:if test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
							<!--<tr>
								<td colspan="2"><input type="checkbox" class="required" name="OFACStatementRead"/><label for="OFACStatementRead">A possible OFAC match has been found. By checking this box I am certifying that I understand I cannot take any adverse action on this applicant based on any type of OFAC result</label></td>
							</tr>-->
							</xsl:if>
	                        <tr style="height:40px">
								<td class="label" valign="middle" style="vertical-align:middle">Possible OFAC Match:</td>
								<td valign="middle" style="vertical-align:middle">
									<div>
										<xsl:choose>
										<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
										prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
											<xsl:attribute name="class">
												<xsl:value-of select="'SmallPad SmallPadRed'"/>
											</xsl:attribute>
											<div class="value">Yes</div>
										</xsl:when>
										<!--<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode)=1]">
											<xsl:attribute name="class">
												<xsl:value-of select="'SmallPad SmallPadGreen'"/>
											</xsl:attribute>
											<div class="value">No</div>
										</xsl:when>-->
										<xsl:otherwise>
											<xsl:attribute name="class">
												<xsl:value-of select="'SmallPad SmallPadGreen'"/>
											</xsl:attribute>
											<div class="value">No</div>
											<!--<xsl:attribute name="style">
												<xsl:value-of select="'color:white;background-color:gray;text-align:center;margin-top:15px;margin-right:10px;float:right;width:30px'"/>
											</xsl:attribute>
										                    N/A
										--></xsl:otherwise>
										</xsl:choose>
									</div>
								</td>
								<td valign="middle" style="vertical-align:middle">
									<xsl:choose>
									<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
										prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
										<div>Possible OFAC Match Found - Use restrictions apply</div>
									</xsl:when>
									<!--<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode)=1]">
										<div style="margin-top:15px">No OFAC match found</div>
									</xsl:when>-->
									<xsl:otherwise>
										<div>No OFAC match found</div>
										<!--<div style="margin-top:15px">No OFAC match performed</div>-->
									</xsl:otherwise>
									</xsl:choose>
								</td>
							</tr>
	                        <tr style="height:40px">
								<td class="label" valign="middle" style="vertical-align:middle">Business Victim Statement:</td>
								<td valign="middle" style="vertical-align:middle">
									<div>
										<xsl:choose>
											<xsl:when test="prd:CommercialFraudShieldSummary[prd:BusinessVictimStatementIndicator/@code='Y']">
												<xsl:attribute name="class">
													<xsl:value-of select="'SmallPad SmallPadRed'"/>
												</xsl:attribute>
												<div class="value">Yes</div>
											</xsl:when>
											<xsl:otherwise>
												<xsl:attribute name="class">
													<xsl:value-of select="'SmallPad SmallPadGreen'"/>
												</xsl:attribute>
												<div class="value">No</div>
											</xsl:otherwise>
										</xsl:choose>
									</div>
								</td>
								<td valign="middle" style="vertical-align:middle">
									<div>
										<xsl:choose>
											<xsl:when test="prd:CommercialFraudShieldSummary[prd:BusinessVictimStatementIndicator/@code='Y']">
												<div>Victim statements on file</div>
											</xsl:when>
											<xsl:otherwise>
												<div>No Victim statements on file</div>
											</xsl:otherwise>
										</xsl:choose>
									</div>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
				<td>	<!-- Verification Triggers -->
					<xsl:choose>
					<xsl:when test="not(prd:CommercialFraudShieldHighRiskTrigger) or prd:CommercialFraudShieldSummary/prd:BusinessRiskTriggersIndicator/@code='N'">
					The primary Business Name, Address, and Phone Number on Experian File were reviewed for High Risk indicators, no High Risk indicators were found.
					</xsl:when>
					<xsl:otherwise>
					<xsl:if test="prd:CommercialFraudShieldHighRiskTrigger">
						<xsl:for-each select="prd:CommercialFraudShieldHighRiskTrigger/prd:HighRiskTriggerStatement">
							<p><xsl:value-of select="normalize-space(current()/text())"/></p>
						</xsl:for-each>
					</xsl:if>
					</xsl:otherwise>
					</xsl:choose>
				</td>
			</tr>
		</tbody>
		<xsl:if test="prd:OFACDetail | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
										prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
		<thead>
			<tr class="subtitle">
				<th colspan="3">OFAC Details</th>
			</tr>
		</thead>
		<tbody>
			<tr><td colspan="3" class="Label" style="font-style:italic">
			<p class="firstColumn">Below are the details of a possible OFAC match. Any action taken regarding a commercial entity must be taken based on a complete investigation of the commercial entity and not based solely on the OFAC information.</p>
			<p class="firstColumn">Experian recommends reviewing the match to determine if the match is strong enough to warrant a call to the U.S.  Department of the Treasury for verification.  To investigate further, contact the U.S. Treasury Department Compliance Program Division at 800 540 6322.</p>
			<br/>
			<!-- Below are the details of a possible OFAC match. Any action taken regarding a commercial entity must be taken based on
			<span style="color:#d6373e">a complete investgation</span> of the commercial entity and not based solely on the OFAC information. -->
			</td></tr>
			<xsl:variable name="inputStr">
				<xsl:choose>
					<xsl:when test="prd:OFACDetail/prd:OFACDetailStatement">
						<xsl:for-each select="prd:OFACDetail/prd:OFACDetailStatement">
							<xsl:value-of select="current()/text()"/>
						</xsl:for-each>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:if test="$inputStr!=''">
				<xsl:call-template name="parseOFACdetail">
					<xsl:with-param name="inputStr" select="$inputStr"/>
				</xsl:call-template>
			</xsl:if>
		</tbody>
		</xsl:if>
		<xsl:if test="prd:ExpandedCreditSummary/prd:VictimStatement/@code='Y'">
		<thead>
			<tr class="subtitle">
				<th colspan="3">Victim Statement Details</th>
			</tr>
		</thead>
		<tbody>
			<tr class="firstColumn">
				<td colspan="3"><xsl:value-of select="prd:ExpandedCreditSummary/prd:VictimStatement"></xsl:value-of></td>
			</tr>
		</tbody>
		</xsl:if>
	</table>

	<xsl:if test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
		prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
	<table class="section" width="100%" cellspacing="0">
		<colgroup width="33%" style="width:33%"/>
		<colgroup width="33%" style="width:33%"/>
		<colgroup width="34%" style="width:34%"/>
			<thead>
				<tr>
					<th colspan="3">
						<xsl:choose>
							<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11]">
								<div>Evaluation for: Matching Name, <xsl:value-of select="$reportName"/></div>
							</xsl:when>
							<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12]">
								<div>Evaluation for: Matching Company Address, <xsl:call-template name="FormatAddressLine">
									    <xsl:with-param name="street1" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingStreetAddress)" />
									    <xsl:with-param name="city" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingCity)" />
									    <xsl:with-param name="state" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingState)" />
									    <xsl:with-param name="zip" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZip)" />
									    <xsl:with-param name="zipExt" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZipExtension)" />
									</xsl:call-template></div>
							</xsl:when>
							<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
								<div>Evaluation for: Matching Name and Company Address, <xsl:call-template name="FormatAddressLine">
										<xsl:with-param name="businessName"><xsl:value-of select="$reportName"></xsl:value-of></xsl:with-param>
									    <xsl:with-param name="street1" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingStreetAddress)" />
									    <xsl:with-param name="city" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingCity)" />
									    <xsl:with-param name="state" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingState)" />
									    <xsl:with-param name="zip" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZip)" />
									    <xsl:with-param name="zipExt" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZipExtension)" />
									</xsl:call-template></div>
							</xsl:when>
						</xsl:choose>
					</th>
				</tr>
				<tr class="subtitle">
					<th colspan="2">Business Alerts</th>
					<th style="width:180px">Verification Triggers</th>
				</tr>
			</thead>
			<tbody>
				<tr>
					<td colspan="2">	<!-- first column table -->
						<table class="firstColumn dataTable" border="0">
	                        <colgroup class="label" style="width:200px"/>
	                        <colgroup class="value" style="width:50px"/>
	                        <colgroup class="value" style="width:400px"/>
							<tbody>
		                        <tr style="height:40px">
									<td class="label" valign="middle" style="vertical-align:middle">Possible OFAC Match:</td>
									<td valign="middle" style="vertical-align:middle">
										<div>
											<xsl:if test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
											prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
												<xsl:attribute name="class">
													<xsl:value-of select="'SmallPad SmallPadRed'"/>
													<xsl:value-of select="SmallPadRed"/>
												</xsl:attribute>
												<div class="value">Yes</div>
											</xsl:if>
											<xsl:if test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode)=0]">
												<xsl:attribute name="class">
													<xsl:value-of select="'SmallPad SmallPadGreen'"/>
												</xsl:attribute>
												<div class="value">No</div>
											</xsl:if>
										</div>
									</td>
									<td valign="middle" style="vertical-align:middle">
										<xsl:choose>
											<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11]">
												<div>OFAC match to company name only</div>
											</xsl:when>
											<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12]">
												<div>OFAC match to company address only</div>
											</xsl:when>
											<xsl:when test="prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]">
												<div>OFAC match to company name and address</div>
											</xsl:when>
											<xsl:otherwise><div>No match attempt made - Not Available</div></xsl:otherwise>
										</xsl:choose>
									</td>
								</tr>
							</tbody>
						</table>
					</td>
					<td>	<!-- Verification Triggers -->
						<xsl:choose>
						<xsl:when test="not(prd:CommercialFraudShieldHighRiskTrigger) or prd:CommercialFraudShieldSummary/prd:BusinessRiskTriggersIndicator/@code='N'">
							The primary business Name, Address, and Phone Number on Experian File were reviewed for high risk indicators, No High risk indicators were found.
						</xsl:when>
						<xsl:otherwise>
							<xsl:for-each select="prd:CommercialFraudShieldHighRiskTrigger/prd:HighRiskTriggerStatement">
								<p><xsl:value-of select="normalize-space(current()/text())"/></p>
							</xsl:for-each>
						</xsl:otherwise>
						</xsl:choose>
					</td>
				</tr>
			</tbody>
	</table>
	</xsl:if>
  </xsl:template>

	<xsl:template name="parseOFACdetail">
	  	<xsl:param name="inputStr"/>
	
		<xsl:variable name="OFACDesc">
			<xsl:value-of select="$inputStr"/>
		</xsl:variable>

		<xsl:variable name="OFACHeader">
			<xsl:choose>
				<xsl:when test="contains($OFACDesc, '(a.k.a.')">
					<xsl:value-of select="substring-before($OFACDesc, '(a.k.a.')" />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(aka')">
					<xsl:value-of select="substring-before($OFACDesc, '(aka')" />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(f.k.a.')">
					<xsl:value-of select="substring-before($OFACDesc, '(f.k.a.')" />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(fka')">
					<xsl:value-of select="substring-before($OFACDesc, '(fka')" />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(n.k.a.')">
					<xsl:value-of select="substring-before($OFACDesc, '(n.k.a.')" />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(nka')">
					<xsl:value-of select="substring-before($OFACDesc, '(nka')" />
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="OFACaka">
			<xsl:choose>
				<xsl:when test="contains($OFACDesc, '(a.k.a.')">
					<xsl:value-of
						select="substring-before(substring-after($OFACDesc, '(a.k.a.'), ')' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(aka')">
					<xsl:value-of
						select="concat('aka ', substring-before(substring-after($OFACDesc, '(aka'), ')' )) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(f.k.a.')">
					<xsl:value-of
						select="concat('f.k.a. ', substring-before(substring-after($OFACDesc, '(f.k.a.'), ')' )) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(fka')">
					<xsl:value-of
						select="concat('fka ', substring-before(substring-after($OFACDesc, '(fka'), ')' )) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(n.k.a.')">
					<xsl:value-of
						select="concat('n.k.a. ', substring-before(substring-after($OFACDesc, '(n.k.a.'), ')' )) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(nka')">
					<xsl:value-of
						select="concat('nka ', substring-before(substring-after($OFACDesc, '(nka'), ')' )) " />
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="OFAClines">
			<xsl:choose>
				<xsl:when test="contains($OFACDesc, '(a.k.a.')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(a.k.a.'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(aka')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(aka'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(f.k.a.')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(f.k.a.'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(fka')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(fka'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(n.k.a.')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(n.k.a.'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="contains($OFACDesc, '(nka')">
					<xsl:value-of
						select="substring-after(substring-after(substring-after($OFACDesc, '(nka'), ')'), ' ' ) " />
				</xsl:when>

				<xsl:when test="position() > 1">
					<xsl:value-of select="$OFACDesc" />
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="''" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

	
			<xsl:if test="normalize-space($OFACHeader)">
				<tr>
					<td align="left" valign="top" colspan="3" class="firstColumn">
						<b>OFAC Record:<xsl:call-template name="nbsp"/></b>
						<xsl:value-of select="$OFACHeader" />
					</td>
				</tr>

				<!-- space row -->
				<tr>
					<td colspan="3">
						<img src="../images/spacer.gif" border="0" width="1"
							height="5" alt="" />
					</td>
				</tr>
			</xsl:if>

			<xsl:if test="normalize-space($OFACaka)">
				<tr>
					<td align="left" valign="top" colspan="3" class="firstColumn">
						<b>AKA:</b>
						<br />
						<xsl:call-template name="OFACLoop">
							<xsl:with-param name="buffer" select="$OFACaka" />
							<xsl:with-param name="type" select="'AKA'" />
							<xsl:with-param name="index" select="1" />
						</xsl:call-template>
					</td>
				</tr>

				<!-- space row -->
				<tr>
					<td colspan="3">
						<img src="../images/spacer.gif" border="0" width="1"
							height="5" alt="" />
					</td>
				</tr>
			</xsl:if>

			<tr>
				<td align="left" valign="top" colspan="3" class="firstColumn">
				<!-- <td align="left" valign="top" width="50%"> -->
					<xsl:call-template name="OFACLoop">
						<xsl:with-param name="buffer" select="$OFAClines" />
						<xsl:with-param name="type" select="'LINES'" />
						<xsl:with-param name="index" select="1" />
					</xsl:call-template>
				</td>
			</tr>

			<!-- space row -->
			<tr>
				<td colspan="2">
					<img src="../images/spacer.gif" border="0" width="1"
						height="5" alt="" />
				</td>
			</tr>

	</xsl:template>


	<!--
		********************************************* * OFACLoop template
		*********************************************
	-->
	<xsl:template name="OFACLoop">
		<xsl:param name="buffer" />
		<xsl:param name="type" />
		<xsl:param name="index" />

		<xsl:variable name="tmpMsgLine">
			<xsl:choose>
				<xsl:when test="contains($buffer, ';')">
					<xsl:value-of select="substring-before($buffer, ';')" />
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="$buffer" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="msgLine">
			<xsl:choose>
				<xsl:when test="$type = 'AKA'">
					<xsl:choose>
						<xsl:when test="contains($tmpMsgLine, 'a.k.a.')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'a.k.a.') " />
						</xsl:when>

						<xsl:when test="contains($tmpMsgLine, 'aka')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'aka') " />
						</xsl:when>

						<xsl:when test="contains($tmpMsgLine, 'f.k.a.')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'f.k.a.') " />
						</xsl:when>

						<xsl:when test="contains($tmpMsgLine, 'fka')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'fka') " />
						</xsl:when>

						<xsl:when test="contains($tmpMsgLine, 'n.k.a.')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'n.k.a.') " />
						</xsl:when>

						<xsl:when test="contains($tmpMsgLine, 'nka')">
							<xsl:value-of select="substring-after($tmpMsgLine, 'nka') " />
						</xsl:when>

						<xsl:otherwise>
							<xsl:value-of select="$tmpMsgLine" />
						</xsl:otherwise>
					</xsl:choose>
				</xsl:when>

				<xsl:otherwise>
					<xsl:value-of select="$tmpMsgLine" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="not (contains($buffer, ';')) and contains($buffer, '[')">
					<xsl:value-of select="normalize-space(substring-before($msgLine,'['))" />
					<br />
					[
					<xsl:value-of select="normalize-space(substring-after($msgLine,'['))" />
					<br />
			</xsl:when>

			<xsl:otherwise>
				<xsl:value-of select="normalize-space($msgLine)" />
				<br />
			</xsl:otherwise>
		</xsl:choose>

		<!-- Test condition and call OFACLoop template if semi-colon found -->
		<xsl:if test="contains($buffer, ';')">
			<xsl:call-template name="OFACLoop">
				<xsl:with-param name="buffer" select="substring-after($buffer, ';') " />
				<xsl:with-param name="type" select="$type" />
				<xsl:with-param name="index" select="$index+1" />
			</xsl:call-template>
		</xsl:if>

	</xsl:template>

</xsl:stylesheet>