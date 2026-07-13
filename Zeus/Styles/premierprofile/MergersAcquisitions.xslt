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
  * MergersAcquisitions template
  *********************************************
  -->
  <!-- @TODO no details provided in BRD yet -->
  <xsl:template name="MergersAcquisitions">
    <xsl:if test="prd:CorporateLinkage">
      <xsl:call-template name="CorporateLinkageSec" /><!--

       back to top image
      --><xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * CorporateRegistration template
  *********************************************
  -->
  <xsl:template name="CorporateLinkageSec">
	<xsl:variable name="subsidiariesHead" select="true()"></xsl:variable>
	<xsl:variable name="subsidiariesMoreThan10" select="prd:CorporateLinkage[normalize-space(prd:MatchingBusinessIndicator)='Y' and normalize-space(prd:LinkageRecordType)='3']"></xsl:variable>
	<xsl:variable name="branchesHead" select="true()"></xsl:variable>
	<xsl:variable name="branchesMoreThan10" select="prd:CorporateLinkage[normalize-space(prd:MatchingBusinessIndicator)='Y' and normalize-space(prd:LinkageRecordType)='4']"></xsl:variable>
  	<xsl:apply-templates mode="ultimate" select="normalize-space(prd:CorporateLinkage/prd:LinkageRecordType)='1'">
  		<xsl:apply-templates match="." mode="ultimate"></xsl:apply-templates>
  	</xsl:apply-templates>
  	<xsl:apply-templates mode="immediateParent" select="normalize-space(prd:CorporateLinkage/prd:LinkageRecordType)='2'">
  		<xsl:apply-templates match="." mode="immediateParent"></xsl:apply-templates>
  	</xsl:apply-templates>
  	<xsl:apply-templates mode="subsidiaries" select="normalize-space(prd:CorporateLinkage/prd:LinkageRecordType)='3'">
  		<xsl:apply-templates match="." mode="subsidiaries"></xsl:apply-templates>
  	</xsl:apply-templates>
  	<xsl:if test="$subsidiariesMoreThan10">
  		<xsl:call-template name="subsidiariesComments"></xsl:call-template>
  	</xsl:if>
  	<xsl:apply-templates mode="branches" select="normalize-space(prd:CorporateLinkage/prd:LinkageRecordType)='4'">
  		<xsl:apply-templates match="." mode="branches"></xsl:apply-templates>
  	</xsl:apply-templates>
  	<xsl:if test="$branchesMoreThan10">
  		<xsl:call-template name="branchesComments"></xsl:call-template>
  	</xsl:if>
  </xsl:template>


  <xsl:template match="prd:CorporateLinkage" mode="ultimate">
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:MatchingBusinessIndicator)='Y'">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr class="subtitle">
				<th colspan="3">Corporate Linkage</th>
			</tr>
		</thead>
		<tbody>
			<tr><td colspan="3">Ultimate Parent of the inquired upon business and the top entity within the corporate family:</td></tr>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
		</tbody>
	</table>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template match="prd:CorporateLinkage" mode="immediateParent">
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:MatchingBusinessIndicator)='Y'">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr class="subtitle">
				<th colspan="3">Corporate Linkage</th>
			</tr>
		</thead>
		<tbody>
			<tr><td colspan="3">Immediate Parent of the inquired upon business:</td></tr>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
		</tbody>
	</table>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template match="prd:CorporateLinkage" mode="subsidiaries">
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:LinkageRecordBIN)=normalize-space(../prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
			<xsl:if test='$subsidiariesHead'>
	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr class="subtitle">
				<th colspan="3">Corporate Linkage</th>
			</tr>
		</thead>
		<tbody>
			<tr><td colspan="3">Subsidiaries of the inquired upon business:<xsl:if test="$subsidiariesMoreThan10"><xsl:value-of select="'*'"></xsl:value-of></xsl:if></td></tr>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
		</tbody>
	</table>
				<xsl:variable name="subsidiariesHead" select="false()"></xsl:variable>
			</xsl:if>
			<xsl:if test='$subsidiariesHead=false()'>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
			</xsl:if>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template name="subsidiariesComments">
			<tr>
				<td class="label" colspan="3">* The inquired upon business has more than 10 subsidiaries. See the complete hierarchy by clicking <a href="#" class="MoreSubsidiariesLink">here</a>.</td>
			</tr>
  </xsl:template>

  <xsl:template match="prd:CorporateLinkage" mode="branches">
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:LinkageRecordBIN)=normalize-space(../prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
			<xsl:if test='$subsidiariesHead'>
			<tr><td colspan="3" class="label">Branches of the inquired upon business:<xsl:if test="$branchesMoreThan10"><xsl:value-of select="'**'"></xsl:value-of></xsl:if></td></tr>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
				<xsl:variable name="branchesHead" select="false()"></xsl:variable>
			</xsl:if>
			<xsl:if test='$branchesHead=false()'>
			<tr>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"></xsl:value-of></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
					<xsl:with-param name="country" select="normalize-space(prd:LinkageCountryCode)"></xsl:with-param>
					</xsl:if>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
			</xsl:if>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template name="branchesComments">
			<tr>
				<td class="label" colspan="3">** The inquired upon business has more than 10 brnaches. See the complete hierarchy by clicking <a href="#" class="MoreBranchesLink">here</a>.</td>
			</tr>
  </xsl:template>


  <xsl:template name="FormatCityStateCountry">
    <xsl:param name="city" />
    <xsl:param name="state"/>
    <xsl:param name="country"/>

	<xsl:if test="$country!=''">
		<xsl:param name="city" select="normalize-space($state)"/>
		<xsl:param name="state" select="normalize-space($country)"/>
	</xsl:if>
	<xsl:choose>
	<xsl:when test="$city!=''">
		<xsl:choose>
		<xsl:when test="$state!=''">
			<xsl:value-of select="concat($city,',',$state)"></xsl:value-of>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="city"></xsl:value-of>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:when>
	<xsl:otherwise>
		<xsl:choose>
		<xsl:when test="$state!=''">
			<xsl:value-of select="$state"></xsl:value-of>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="''"></xsl:value-of>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:otherwise>
	</xsl:choose>
  </xsl:template>

</xsl:stylesheet>