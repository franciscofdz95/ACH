<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:prd="http://www.experian.com/ARFResponse">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="xml" indent="yes"/>


  <!--
  *********************************************
  * Business Demographic template
  *********************************************
  -->
  <xsl:template name="BusinessDemographic">
    <xsl:param name="reportName" select="''" />
	<xsl:param name="businessName"/>
    <!-- Section title -->
    <!--<xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Quarterly Payment Trends'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>-->

	<table class="section" width="100%" cellspacing="0">
		<colgroup style="width:50%"/>
		<colgroup style="width:7%"/>
		<colgroup style="width:43%" />
		<thead>
			<tr>
				<th class="doubleheightTitle">
					<div class="smallTitle">Business Name</div>
					<div class="titleLabel">
						<!--<span><xsl:value-of select="$reportName" /></span>-->
						<span><xsl:value-of select="$businessName"/></span> <!-- @TODO To be Added -->
					</div>
				</th>
				<th class="doubleheightTitle" style="padding:0;height:49px">
					<!-- temporarily remove the verified legal name indicator, requested by Pete on Dec08,2011 -->
					<!--<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:LegalName/prd:LegalBusinessName">
						<div class="verifiedLegalName"></div>
					</xsl:if>-->
				</th>
				<th class="doubleheightTitle">
					<div class="smallTitle">Business Identification Number</div>
					<div class="titleLabel"><xsl:value-of select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)" /></div>
				</th>
			</tr>

			<!--<tr>
				<th class="smallTitle">Business Name</th>
				<th class="smallTitle">Business Identification Number</th>
			</tr>-->
			<!--<tr>
				<th class="titleLabel">
					<span><xsl:value-of select="$reportName" /></span>
					<span><xsl:value-of select="prd:ExpandedBusinessNameAndAddress/prd:LegalName/prd:LegalBusinessName | prd:ExpandedBusinessNameAndAddress/prd:BusinessName" /></span>  @TODO To be Added
				</th>
				<th class="titleLabel"><xsl:value-of select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)" /></th>
			</tr>-->
		</thead>
		<tbody>
			<tr>
				<td class="firstColumn">	<!-- first column table -->
					<table border="0">
						<colgroup class="label"></colgroup>
						<colgroup class=""></colgroup>
						<tbody>
							<xsl:if test="prd:DoingBusinessAs/prd:DBAName">
							<tr>
								<td class="label">Doing Business As:</td>
								<td>
										<!-- @TODO To be Added -->
									<div><xsl:value-of select="normalize-space(prd:DoingBusinessAs/prd:DBAName)"></xsl:value-of></div>
									<xsl:if test="normalize-space(prd:DoingBusinessAs/prd:PrimaryDBAFlag)='Y'">
									<div><a>Additional names on file. Click here for complete list.</a></div>
									</xsl:if>
								</td>
							</tr>
							</xsl:if>
							<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:StreetAddress or prd:ExpandedBusinessNameAndAddress/prd:City or prd:ExpandedBusinessNameAndAddress/prd:State or prd:ExpandedBusinessNameAndAddress/prd:Zip">
							<tr>
								<td class="label">Primary Address:</td>
								<td class="viewMapWrap">
									<div class="addressDetail"><xsl:call-template name="FormatAddress">
									    <xsl:with-param name="street1" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:StreetAddress)" />
									    <xsl:with-param name="city" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:City)" />
									    <xsl:with-param name="state" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:State)" />
									    <xsl:with-param name="zip" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:Zip)" />
									    <xsl:with-param name="zipExt" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ZipExtension)" />
									</xsl:call-template></div>
								</td>
							</tr>
							</xsl:if>
						</tbody>
					</table>
				</td>
				<td><xsl:call-template name="nbsp"/></td>
				<td class="firstColumn">	<!-- second column table -->
					<table border="0">
						<colgroup class="label"></colgroup>
						<colgroup class=""></colgroup>
						<tbody>
							<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:WebsiteURL">
							<tr>
								<td class="label">Website:</td>
								<td><xsl:value-of select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:WebsiteURL)"></xsl:value-of></td>
							</tr>
							</xsl:if>
							<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:PhoneNumber">
							<tr>
								<td class="label">Phone:</td>
								<td>
									<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:PhoneNumber">
									<xsl:call-template name="FormatPhone">
									    <xsl:with-param name="value" select="translate(normalize-space(prd:ExpandedBusinessNameAndAddress/prd:PhoneNumber), '-', '')" />
									</xsl:call-template>
									</xsl:if>
							  	</td>
							</tr>
							</xsl:if>
							<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:TaxID">
							<tr>
								<td class="label">Tax ID:</td>
								<td>
									<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:TaxID">
					                <xsl:value-of select="concat(substring(normalize-space(prd:ExpandedBusinessNameAndAddress/prd:TaxID), 1, 2),
					                	'-',
					                	substring(normalize-space(prd:ExpandedBusinessNameAndAddress/prd:TaxID), 3))" />
				                	</xsl:if>
								</td>
							</tr>
							</xsl:if>
						</tbody>
					</table>
				</td>
			</tr>
			<xsl:if test="normalize-space(prd:ExpandedCreditSummary/prd:OFACMatch/@code)='Y'
							or number(prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=11
							or number(prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=12
							or number(prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=13
							or normalize-space(prd:ExpandedCreditSummary/prd:VictimStatement/@code)='Y'">
			<tr>
				<td colspan="3" class="firstColumn">
					<span style="color:#ed1951;padding-left:5px;font-weight:bold">Possible OFAC or Victim Statement on file</span>
					<a class="fraudBizIDLink" target="_top" href="../../search/showFraudSearchPage" style="text-decoration:none;display:none">
						<span style="color:#ed1951;font-weight:bold">, see the details by clicking here.</span>
					</a>
				</td>
			</tr>
			</xsl:if>
			<!--<xsl:if test="normalize-space(prd:ExpandedCreditSummary/prd:VictimStatement/@code)='Y'">
			<tr>
				<td colspan="3">
					<span style="color:#ed1951;padding-left:5px;font-weight:bold">Possible Victim Statement on file</span>
					<a class="fraudBizIDLink" target="_top" href="../../search/showFraudSearchPage" style="text-decoration:none;display:none">
						<span style="color:#ed1951;font-weight:bold">, see the details by clicking here.</span>
					</a>
				</td>
			</tr>
			</xsl:if>-->
			<!-- Matching Branch Address and Matching Branch BIN -->
			<xsl:if test="prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress">
			<tr class="section">
				<td class="firstColumn">
					<table border="0">
						<colgroup class="label"></colgroup>
						<colgroup class=""></colgroup>
						<tbody>
							<tr>
								<td class="label">Matching Branch Address:</td>
								<td class="viewMapWrap">
									<div class="addressDetail">
									<xsl:call-template name="FormatAddress">
									    <xsl:with-param name="street1" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingStreetAddress)" />
									    <xsl:with-param name="city" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingCity)" />
									    <xsl:with-param name="state" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingState)" />
									    <xsl:with-param name="zip" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZip)" />
									    <xsl:with-param name="zipExt" select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingZipExtension)" />
									</xsl:call-template>
									</div>
									<div class="hidden_on_print"><a class="ViewMapLink hidden_on_print" href="#"><b>View Map</b></a></div>
								</td>
							</tr>
							<tr>
								<td class="label">Matching Branch BIN:</td>
								<td><xsl:value-of select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress/prd:MatchingBranchBIN)"></xsl:value-of></td>
							</tr>
						</tbody>
					</table>
				</td>
				<td><xsl:call-template name="nbsp"/></td>
				<td>
					Business often have multiple branches or alternate addresses. This location most closely matched your inquiry.
				</td>
			</tr>
			</xsl:if>
			<!-- Ultimate Parent -->
			<xsl:if test="prd:CorporateLinkage">
			<tr class="section">
				<td class="firstColumn">
					<xsl:choose>
						<xsl:when test="normalize-space(prd:CorporateLinkage[prd:LinkageRecordType/@code='1']/prd:LinkageRecordBIN) =
								normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
							<xsl:call-template name="nbsp"/>
						</xsl:when>
						<xsl:otherwise>
					<table border="0">
						<colgroup class="label"></colgroup>
						<colgroup class=""></colgroup>
						<tbody>
							<tr>
								<td class="label">Ultimate Parent:</td>
								<td>
									<xsl:apply-templates select="prd:CorporateLinkage[prd:LinkageRecordType/@code='1']" mode="DemographicUltimate"/>
								</td>
							</tr>
						</tbody>
					</table>
						</xsl:otherwise>
					</xsl:choose>
				</td>
				<td style="text-align:center"><a href="#" class="viewCorpsLinkageDetail"><img style="height:34px;border:none">
					<xsl:attribute name="src">
						<xsl:value-of select="concat($basePath,'tree.gif')"/>
					</xsl:attribute>
				</img>
				</a></td>
				<td>
					<div>
						<xsl:choose>
							<xsl:when test="normalize-space(prd:CorporateLinkage[prd:LinkageRecordType/@code='1']/prd:LinkageRecordBIN) =
								normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
								This business is the ultimate parent.
							</xsl:when>
							<xsl:otherwise>
								This business is a member of a corporate family.
							</xsl:otherwise>
						</xsl:choose>
						<br/><b><a href="#CorporateLinkageLink">See the corporate hierarchy by clicking here</a></b>
					</div>
				</td>
			</tr>
			</xsl:if>
		</tbody>
	</table>
  </xsl:template>

	<xsl:template match="prd:CorporateLinkage" mode="DemographicUltimate">
		<xsl:value-of select="prd:LinkageCompanyName"></xsl:value-of>
	</xsl:template>

</xsl:stylesheet>