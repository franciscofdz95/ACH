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
  * CommercialFinance template
  *********************************************
  -->
  <xsl:template name="CorporateLinkage">
    <xsl:if test="prd:CorporateLinkage">
    	<!--<xsl:apply-templates select="prd:CorporateLinkage"></xsl:apply-templates>-->
      <!-- BankingRelationship -->
      <xsl:call-template name="CorporateLinkageSec" />

      <!-- back to top image -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * CorporateLinkage template
  *********************************************
  -->
  <xsl:template name="CorporateLinkageSec">
  	<xsl:variable name="selfBIN">
  		<xsl:value-of select="normalize-space(prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)"></xsl:value-of>
  	</xsl:variable>
	<xsl:variable name="subsidiariesMoreThan10">
		<xsl:choose>
		<xsl:when test="prd:CorporateLinkage[prd:LinkageRecordType/@code='3' and prd:ReturnLimitExceeded/@code !='N']">
			<xsl:value-of select="'true'"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="'false'"/>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="branchesMoreThan10">
		<xsl:choose>
		<xsl:when test="prd:CorporateLinkage[prd:LinkageRecordType/@code='4' and prd:ReturnLimitExceeded/@code !='N']">
			<xsl:value-of select="'true'"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="'false'"/>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="notes">
		<xsl:choose>
		<xsl:when test="$subsidiariesMoreThan10='true'">
			<xsl:choose>
			<xsl:when test="$branchesMoreThan10='true'">
				<xsl:value-of select="'* The inquired upon business has more than 10 subsidiaries and branches.'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="'* The inquired upon business has more than 10 subsidiaries.'"/>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:when>
		<xsl:otherwise>
			<xsl:choose>
			<xsl:when test="$branchesMoreThan10='true'">
				<xsl:value-of select="'* The inquired upon business has more than 10 branches.'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
			</xsl:choose>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<table class="section dataTable linkageDetails" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th colspan="3">
					<a name="CorporateLinkageLink" style="background:none"><a class="report_section_title">Corporate Linkage</a></a>
				</th>
			</tr>
			<tr class="subtitle">
				<th>Business Name</th>
				<th>Location</th>
				<th>BIN</th>
			</tr>
		</thead>
		<tbody>
		  	<xsl:apply-templates mode="ultimate" select="prd:CorporateLinkage[prd:LinkageRecordType/@code='1']">
		  		<xsl:with-param name="selfBIN" select="$selfBIN"></xsl:with-param>
		  	</xsl:apply-templates>
		  	<xsl:apply-templates mode="immediateParent" select="prd:CorporateLinkage[prd:LinkageRecordType/@code='2']">
		  		<xsl:with-param name="selfBIN" select="$selfBIN"></xsl:with-param>
		  	</xsl:apply-templates>
		  	<xsl:if test="prd:CorporateLinkage[prd:LinkageRecordType/@code='3']">
		  		<tr><td colspan="3"><xsl:call-template name="nbsp"/></td></tr>
				<tr><td colspan="3" class="label">Subsidiaries of the inquired upon business:<xsl:if test="$subsidiariesMoreThan10='true'"><xsl:value-of select="'*'"></xsl:value-of></xsl:if></td></tr>
			  	<xsl:apply-templates mode="subsidiaries" select="prd:CorporateLinkage[prd:LinkageRecordType/@code='3']">
			  		<xsl:with-param name="selfBIN" select="$selfBIN"></xsl:with-param>
			  	</xsl:apply-templates>
			</xsl:if>
		  	<xsl:if test="prd:CorporateLinkage[prd:LinkageRecordType/@code='4']">
		  		<tr><td colspan="3"><xsl:call-template name="nbsp"/></td></tr>
				<tr><td colspan="3" class="label">Branches of the inquired upon business:<xsl:if test="$branchesMoreThan10='true'"><xsl:value-of select="'*'"></xsl:value-of></xsl:if></td></tr>
			  	<xsl:apply-templates mode="branches" select="prd:CorporateLinkage[prd:LinkageRecordType/@code='4']">
			  	</xsl:apply-templates>
		  	</xsl:if>
		  	<xsl:if test="$notes!=''">
		  		<tr><td colspan="3" class="value"><br/><div class="indent2"><xsl:value-of select="$notes"/></div></td></tr>
			  	<tr><td colspan="3" class="label"><div class="indent2"><a href="#" class="viewCorpsLinkageDetail">See the complete hierarchy by clicking here.</a></div></td></tr>
		  	</xsl:if>
	  	</tbody>
  	</table>
  </xsl:template>


  <xsl:template match="prd:CorporateLinkage" mode="ultimate">
  	<xsl:param name="selfBIN"></xsl:param>
	<xsl:variable name="curBIN">
		<xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"/>
	</xsl:variable>
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:MatchingBusinessIndicator)='Y'">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
			<tr><td colspan="3" class="label"><xsl:choose>
					<xsl:when test="$selfBIN = $curBIN">The inquired upon business, <xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>, is the Ultimate Parent</xsl:when>
					<xsl:otherwise>Ultimate Parent of the inquired upon business and the top entity within the corporate family:</xsl:otherwise>
				</xsl:choose></td></tr>
			<tr class="even">
				<td><xsl:choose>
					<xsl:when test="$selfBIN = $curBIN">
						<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:element name="a">
							<xsl:attribute name="target"><xsl:value-of select="'_top'"/></xsl:attribute>
							<xsl:attribute name="href">
								<xsl:value-of select="concat('../../search/showExpandedSearchPage?es_bin=',$curBIN,'&amp;option=search')"></xsl:value-of>
							</xsl:attribute>
							<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="address" select="normalize-space(prd:LinkageCompanyAddress)"></xsl:with-param>
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:with-param name="country">
						<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
							<xsl:value-of select="normalize-space(prd:LinkageCountryCode)"></xsl:value-of>
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template match="prd:CorporateLinkage" mode="immediateParent">
  	<xsl:param name="selfBIN"></xsl:param>
	<xsl:variable name="curBIN">
		<xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"/>
	</xsl:variable>
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:MatchingBusinessIndicator)='Y'">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
	  		<tr><td colspan="3"><xsl:call-template name="nbsp"/></td></tr>
			<tr><td colspan="3" class="label">Immediate Parent of the inquired upon business:</td></tr>
			<tr class="even">
				<td><xsl:choose>
					<xsl:when test="$selfBIN = $curBIN">
						<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:element name="a">
							<xsl:attribute name="target"><xsl:value-of select="'_top'"/></xsl:attribute>
							<xsl:attribute name="href">
								<xsl:value-of select="concat('../../search/showExpandedSearchPage?es_bin=',$curBIN,'&amp;option=search')"></xsl:value-of>
							</xsl:attribute>
							<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="address" select="normalize-space(prd:LinkageCompanyAddress)"></xsl:with-param>
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:with-param name="country">
						<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
							<xsl:value-of select="normalize-space(prd:LinkageCountryCode)"></xsl:value-of>
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>
  <xsl:template match="prd:CorporateLinkage" mode="subsidiaries">
  	<xsl:param name="selfBIN"></xsl:param>
	<xsl:variable name="curBIN">
		<xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"/>
	</xsl:variable>
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:LinkageRecordBIN)=normalize-space(../prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
			<tr>
				<xsl:choose>
				<xsl:when test="position() mod 2 = 1">
				<xsl:attribute name="class"><xsl:value-of select="'even'"/></xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
				<xsl:attribute name="class"><xsl:value-of select="'odd'"/></xsl:attribute>
				</xsl:otherwise>
				</xsl:choose>
				<td><xsl:choose>
					<xsl:when test="$selfBIN = $curBIN">
						<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:element name="a">
							<xsl:attribute name="target"><xsl:value-of select="'_top'"/></xsl:attribute>
							<xsl:attribute name="href">
								<xsl:value-of select="concat('../../search/showExpandedSearchPage?es_bin=',$curBIN,'&amp;option=search')"></xsl:value-of>
							</xsl:attribute>
							<xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/>
						</xsl:element>
					</xsl:otherwise>
				</xsl:choose></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="address" select="normalize-space(prd:LinkageCompanyAddress)"></xsl:with-param>
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:with-param name="country">
						<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
							<xsl:value-of select="normalize-space(prd:LinkageCountryCode)"></xsl:value-of>
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>

  <xsl:template match="prd:CorporateLinkage" mode="branches">
  	<xsl:choose>
	  	<xsl:when test="normalize-space(prd:LinkageRecordBIN)=normalize-space(../prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)">
	  		<!-- @TODO No Details provided in BRD when the given business is Ultimate Parent -->
	  	</xsl:when>
	  	<xsl:otherwise>
			<tr>
				<xsl:choose>
				<xsl:when test="position() mod 2 = 1">
				<xsl:attribute name="class"><xsl:value-of select="'even'"/></xsl:attribute>
				</xsl:when>
				<xsl:otherwise>
				<xsl:attribute name="class"><xsl:value-of select="'odd'"/></xsl:attribute>
				</xsl:otherwise>
				</xsl:choose>
				<td><xsl:value-of select="normalize-space(prd:LinkageCompanyName)"/></td>
				<td><xsl:call-template name="FormatCityStateCountry">
					<xsl:with-param name="address" select="normalize-space(prd:LinkageCompanyAddress)"></xsl:with-param>
					<xsl:with-param name="city" select="normalize-space(prd:LinkageCompanyCity)"></xsl:with-param>
					<xsl:with-param name="state" select="normalize-space(prd:LinkageCompanyState)"></xsl:with-param>
					<xsl:with-param name="country">
						<xsl:if test="normalize-space(prd:LinkageCountryCode)!='USA'">
							<xsl:value-of select="normalize-space(prd:LinkageCountryCode)"></xsl:value-of>
						</xsl:if>
					</xsl:with-param>
				</xsl:call-template></td>
				<td><xsl:value-of select="normalize-space(prd:LinkageRecordBIN)"></xsl:value-of></td>
			</tr>
	  	</xsl:otherwise>
  	</xsl:choose>
  </xsl:template>

  <xsl:template name="FormatCityStateCountry">
    <xsl:param name="address"/>
    <xsl:param name="country"/>
    <xsl:param name="city"/>
    <xsl:param name="state"/>
    <xsl:if test="$address!=''">
    	<xsl:value-of select="concat($address,' - ')"/>
    </xsl:if>
    <xsl:variable name="field1">
    	<xsl:choose>
    	<xsl:when test="$country!='USA'">
    		<xsl:value-of select="normalize-space($city)"></xsl:value-of>
    	</xsl:when>
    	<xsl:otherwise>
    		<xsl:value-of select="normalize-space($state)"></xsl:value-of>
    	</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="field2">
    	<xsl:choose>
    	<xsl:when test="$country!='USA'">
    		<xsl:value-of select="normalize-space($state)"></xsl:value-of>
    	</xsl:when>
    	<xsl:otherwise>
    		<xsl:value-of select="normalize-space($country)"></xsl:value-of>
    	</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>

	<xsl:choose>
	<xsl:when test="$field1!=''">
		<xsl:choose>
		<xsl:when test="$field2!=''">
			<xsl:value-of select="concat($field1,',',$field2)"></xsl:value-of>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$field1"></xsl:value-of>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:when>
	<xsl:otherwise>
		<xsl:choose>
		<xsl:when test="$field2!=''">
			<xsl:value-of select="$field2"></xsl:value-of>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="''"></xsl:value-of>
		</xsl:otherwise>
		</xsl:choose>
	</xsl:otherwise>
	</xsl:choose>
  </xsl:template>

</xsl:stylesheet>