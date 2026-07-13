<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:prd="http://www.experian.com/ARFResponse">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="html"
    doctype-public="-//W3C//DTD HTML 4.0 Transitional//EN"
    doctype-system="http://www.w3c.org/TR/xhtml/DTD/xhtml1-strict.dtd"
    indent="yes" encoding="UTF-8" />


  <!--
  *********************************************
  * Business Facts template
  *********************************************
  -->
  <xsl:template match="prd:BusinessFacts">
    <xsl:variable name="reportDate">
      <xsl:value-of select="../prd:ExpandedBusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

    <!-- get year from date extension -->
    <xsl:variable name="reportYear">
      <xsl:value-of select="substring(normalize-space($reportDate), 1, 4)"/>
    </xsl:variable>

  	<xsl:variable name="establishDate">
  		<xsl:choose>
  			<xsl:when test="prd:FileEstablishedFlag and prd:FileEstablishedFlag/@code='P'">
  				<xsl:variable name="tmpdate">
				<xsl:call-template name="FormatDate">
					<xsl:with-param name="pattern" select="'mo/year'"/>
					<xsl:with-param name="value" select="normalize-space(prd:FileEstablishedDate)"/>
				</xsl:call-template>
				</xsl:variable>
				<xsl:value-of select="concat('PRIOR TO ',$tmpdate)"/>
  			</xsl:when>
  			<xsl:otherwise>
				<xsl:call-template name="FormatDate">
					<xsl:with-param name="pattern" select="'mo/dt/year'"/>
					<xsl:with-param name="value" select="normalize-space(prd:FileEstablishedDate)"/>
				</xsl:call-template>
  			</xsl:otherwise>
  		</xsl:choose>
  	</xsl:variable>
  	<xsl:variable name="salesAmount">
		<xsl:choose>
			<xsl:when test="prd:SalesRevenue and number(prd:SalesRevenue) &gt; 0">
				<xsl:value-of select="format-number(number(prd:SalesRevenue),'$##,###,###,##0')"></xsl:value-of>
			</xsl:when>
			<xsl:otherwise>
				<xsl:variable name="SalesIndCode">
					<xsl:value-of select="normalize-space(prd:SalesIndicator/@code)"></xsl:value-of>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test="$SalesIndCode = 'A'">
						<xsl:value-of select="'$1 - $499,000'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'B'">
						<xsl:value-of select="'$500,000 - $999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'C'">
						<xsl:value-of select="'$1,000,000 - $4,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'D'">
						<xsl:value-of select="'$5,000,000 - $999,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'F'">
						<xsl:value-of select="'$10,000,000- $24,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'G'">
						<xsl:value-of select="'$25,000,000 - $74,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'H'">
						<xsl:value-of select="'$75,000,000 - $199,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'I'">
						<xsl:value-of select="'$200,000,000 - $499,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'J'">
						<xsl:value-of select="'$500,000,000 - $999,999,999'"></xsl:value-of>
					</xsl:when>
					<xsl:when test="$SalesIndCode = 'K'">
						<xsl:value-of select="'Over $1,000,000,000'"></xsl:value-of>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''"></xsl:value-of>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
  	</xsl:variable>
	<table class="section" width="100%" cellspacing="0">
		<colgroup style="width:50%"/>
		<colgroup style="width:50%" />
		<thead>
			<tr>
				<th colspan="2"><a class="report_section_title">Business Facts</a></th>
			</tr>
		</thead>
		<tbody>
			<xsl:if test="../prd:EnhancedBusinessDescription/prd:BusinessDescription">
			<tr>
				<td colspan="2">
					<div style="padding:10px 5px"><xsl:value-of select="../prd:EnhancedBusinessDescription/prd:BusinessDescription"/></div>
				</td>
			</tr>
			</xsl:if>
			<tr>
				<td>	<!-- first column table -->
					<div class="firstColumn">
							<xsl:if test="prd:FileEstablishedDate and normalize-space(prd:FileEstablishedDate)!='Not Available'">
							<div>
								<span class="label">Years on File:<xsl:call-template name="nbsp"/></span>
								<span class="value">
					              	<xsl:value-of select="($reportYear - number(substring(prd:FileEstablishedDate, 1, 4)))" /><xsl:if test="prd:FileEstablishedFlag/@code = 'P'">+ </xsl:if>
					              		(FILE ESTABLISHED <xsl:value-of select="$establishDate"/>)
								</span>
							</div>
							</xsl:if>
							<xsl:if test="../prd:CorporateRegistration/prd:StateOfOrigin">
							<div style="clear:both">
								<span class="label">
								<xsl:choose>
									<xsl:when test="../prd:CorporateRegistration">
										<a href="#CorporateRegistration" style="text-decoration:none">State of Incorporation</a>:<xsl:call-template name="nbsp"/>
									</xsl:when>
									<xsl:otherwise>
										State of Incorporation:<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
								</span> <!-- The link will take the user to the Corporate Registration Section when present -->
								<!-- @TODO workaround -->
								<span class="value"><xsl:value-of select="normalize-space(../prd:CorporateRegistration/prd:StateOfOrigin)"></xsl:value-of></span>
							</div>
							</xsl:if>
							<xsl:if test="prd:DateOfIncorporation">
							<div style="clear:both">
								<span class="label">Date of Incorporation:<xsl:call-template name="nbsp"/></span>
								<span class="value">
								<xsl:call-template name="FormatDate">
									<xsl:with-param name="pattern" select="'mo/dt/year'"/>
									<xsl:with-param name="value" select="normalize-space(prd:DateOfIncorporation)"/>
								</xsl:call-template>
								</span>
							</div>
							</xsl:if>
							<xsl:if test="../prd:CorporateRegistration/prd:BusinessType and normalize-space(../prd:CorporateRegistration/prd:BusinessType)!=''">
							<div style="clear:both">
								<span class="label">Business Type:<xsl:call-template name="nbsp"/></span>
								<span class="value">
									<!-- 5520 - Business Type Code
												C = Corporation
												P = Partnership
												S = Sole Proprietor -->
									<xsl:attribute name="code">
										<xsl:value-of select="normalize-space(normalize-space(../prd:CorporateRegistration/prd:BusinessType/@code))"></xsl:value-of>
									</xsl:attribute>
									<!-- @TODO workaround -->
									<xsl:value-of select="normalize-space(../prd:CorporateRegistration/prd:BusinessType)"></xsl:value-of>
									<!--<xsl:value-of select="normalize-space(prd:BusinessTypeIndicator)"></xsl:value-of>-->
								</span>
							</div>
							</xsl:if>
							<!--<xsl:if test="prd:YearsInBusiness and normalize-space(prd:YearsInBusiness)!='Not Available'">
							<div>
								<span class="label">Years in Business:</span>
								<span class="value"><xsl:value-of select="number(prd:YearsInBusiness)"></xsl:value-of></span>
							</div>
							</xsl:if>-->
							<xsl:if test="../prd:KeyPersonnelExecutiveInformation and (../prd:KeyPersonnelExecutiveInformation/prd:Name or ../prd:KeyPersonnelExecutiveInformation/prd:Title)">
							<div style="clear:both">
								<span class="label">Contacts:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign">
									<xsl:for-each select="../prd:KeyPersonnelExecutiveInformation">
									<span>
									<xsl:choose>
										<xsl:when test="prd:Title and normalize-space(prd:Title)!=''">
											<xsl:value-of select="concat(normalize-space(prd:Name), ' - ', normalize-space(prd:Title))"></xsl:value-of>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="normalize-space(prd:Name)"></xsl:value-of>
										</xsl:otherwise>
									</xsl:choose>
									</span>
									<br/>
									</xsl:for-each>
								</span>
							</div>
							</xsl:if>
							<div style="clear:both"></div>
					</div>
				</td>
				<td>	<!-- second column table -->
					<div class="firstColumn">
							<xsl:for-each select="../prd:SICCodes">
							<div>
								<xsl:if test="position()=1">
								<span class="label">SIC Code:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="concat(normalize-space(prd:SIC),' - ',normalize-space(prd:SIC/@code))"></xsl:value-of></span>
								</xsl:if>
								<xsl:if test="position() &gt; 1">
								<span class="value rightalign" style="clear:both"><xsl:value-of select="concat(normalize-space(prd:SIC),' - ',normalize-space(prd:SIC/@code))"></xsl:value-of></span>
								</xsl:if>
							</div>
							<div style="clear:both"/>
							</xsl:for-each>
							<xsl:for-each select="../prd:NAICSCodes">
							<div>
								<xsl:if test="position()=1">
								<span class="label">NAICS Code:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="concat(normalize-space(prd:NAICS),' - ',normalize-space(prd:NAICS/@code))"></xsl:value-of></span>
								</xsl:if>
								<xsl:if test="position() &gt; 1">
								<span class="value rightalign" style="clear:both"><xsl:value-of select="concat(normalize-space(prd:NAICS),' - ',normalize-space(prd:NAICS/@code))"></xsl:value-of></span>
								</xsl:if>
							</div>
							<div style="clear:both"/>
							</xsl:for-each>
							<xsl:if test="prd:EmployeeSize and number(prd:EmployeeSize) > 0">
							<div>
								<span class="label">Number of Employees:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="format-number(number(prd:EmployeeSize),'###,###,##0')"></xsl:value-of></span>
							</div>
							</xsl:if>
							<xsl:if test="$salesAmount!=''">
							<div>
								<span class="label">Sales:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign">
								<xsl:value-of select="$salesAmount"/>
								</span>
							</div>
							</xsl:if>
							<xsl:if test="normalize-space(prd:NonProfitIndicator/@code)='N'">
							<div>
								<span class="label">Non-Profit:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign">Yes</span>
							</div>
							</xsl:if>
							<xsl:if test="normalize-space(prd:PublicIndicator/@code)='1' or normalize-space(prd:PublicIndicator/@code)='Y'">
							<div>
								<span class="label">Public Company:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign">Yes</span>
							</div>
							</xsl:if>
							<!-- @TODO release 2? -->
							<xsl:for-each select="../prd:Stocks">
							<xsl:choose>
							<xsl:when test="position()=1">
							<div>
								<span class="label secondLevel">Stock Exchange &amp; Symbol:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="concat(normalize-space(prd:StockExchangeDescription),', ',normalize-space(prd:TickerSymbol))"></xsl:value-of></span>
							</div>
							</xsl:when>
							<xsl:when test="position()=2">
							<div>
								<span class="label secondLevel">Other Exchanges:<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="concat(normalize-space(prd:StockExchangeDescription),', ',normalize-space(prd:TickerSymbol))"></xsl:value-of></span>
							</div>
							</xsl:when>
							<xsl:otherwise>
							<div>
								<span class="label secondLevel"><xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"><xsl:value-of select="concat(normalize-space(prd:StockExchangeDescription),', ',normalize-space(prd:TickerSymbol))"></xsl:value-of></span>
							</div>
							</xsl:otherwise>
							</xsl:choose>
							</xsl:for-each>
							<xsl:comment>Intentionally putting a space line here</xsl:comment>
							<div><span colspan="2"></span></div>	<!-- Intentionally putting a space line here -->
							<xsl:if test="../prd:Fortune1000">
							<div>
								<span class="label">Fortune 1000 Ranking<xsl:call-template name="nbsp"/></span>
								<span class="value rightalign"></span>
							</div>
							<xsl:if test="../prd:Fortune1000/prd:CurrentYear">
							<div style="clear:both">
								<span class="label secondLevel"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:CurrentYear/prd:Year)"></xsl:value-of></span>
								<span class="value rightalign"><xsl:value-of select="number(../prd:Fortune1000/prd:CurrentYear/prd:Rank)"></xsl:value-of></span>
							</div>
							</xsl:if>
							<xsl:for-each select="../prd:Fortune1000/prd:PriorYear">
							<xsl:sort select="number(prd:Year)" order="descending" data-type="number"/>
							<div style="clear:both">
								<span class="label secondLevel"><xsl:value-of select="normalize-space(prd:Year)"></xsl:value-of></span>
								<span class="value rightalign"><xsl:value-of select="number(prd:Rank)"></xsl:value-of></span>
							</div>
							</xsl:for-each>
							</xsl:if>
					</div>
				</td>
			</tr>
		</tbody>
	</table>
  </xsl:template>
</xsl:stylesheet>