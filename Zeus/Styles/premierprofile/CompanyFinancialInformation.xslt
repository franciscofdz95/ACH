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
  * Business Facts template
  *********************************************
  -->
  <xsl:template name="CompanyFinancialInformation">
    <xsl:if test="prd:StandardAndPoorsFinancialInformation">
    	<!--<xsl:apply-templates select="prd:CorporateLinkage"></xsl:apply-templates>-->
      <!-- BankingRelationship -->
      <xsl:call-template name="CompanyFinancialInformation" />

      <!-- back to top image -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>

  <xsl:template match="prd:StandardAndPoorsFinancialInformation">
    <xsl:variable name="fiscalEnd">
      <xsl:variable name="month">
  		   <xsl:call-template name="FormatMonth">
  		     <xsl:with-param name="monthValue" select="number(substring(prd:FiscalYearEndDate, 5, 2))" />
  		     <xsl:with-param name="upperCase" select="true()" />
  		   </xsl:call-template>
      </xsl:variable>

      <xsl:value-of select="concat(normalize-space($month), normalize-space(substring(prd:FiscalYearEndDate, 3, 2)))" />
    </xsl:variable>
    <xsl:variable name="currentDate">
      <xsl:variable name="month">
  		   <xsl:call-template name="FormatMonth">
  		     <xsl:with-param name="monthValue" select="number(substring(prd:CurrentDate, 5, 2))" />
  		     <xsl:with-param name="upperCase" select="true()" />
  		   </xsl:call-template>
      </xsl:variable>

      <xsl:value-of select="concat(normalize-space($month), normalize-space(substring(prd:CurrentDate, 3, 2)))" />
    </xsl:variable>
	<table class="section" width="100%" cellspacing="0">
		<colgroup style="width:50%"/>
		<colgroup style="width:50%" />
		<thead>
			<tr>
				<th colspan="4"><a class="report_section_title">Company Financial Information</a></th>
			</tr>
			<tr class="subtitle">
				<th colspan="2">Balance sheet for fiscal year ending:<xsl:value-of select="$fiscalEnd"></xsl:value-of></th>
				<th colspan="2">Data current through:<xsl:value-of select="$currentDate"></xsl:value-of> ($ Thousands)</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td>Cash and equivalent</td>
				<td><xsl:value-of select="prd:CashandEquivalent"></xsl:value-of></td>
				<td><xsl:value-of select="prd:CashandEquivalent"></xsl:value-of></td>
			</tr>
			<tr>
				<td colspan="2">
					<!-- @TODO To Be Decided -->
				</td>
			</tr>
			<tr>
				<td>	<!-- first column table -->
					<table class="firstColumn" border="0">
						<colgroup class="label"></colgroup>
						<colgroup class="value"></colgroup>
						<tbody>
							<tr>
								<td class="label">Experian File Establish Date:</td>
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:FileEstablishedDate)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">State of Incorporation(<a href="#">see details</a>):</td> <!-- The link will take the user to the Corporate Registration Section when present -->
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:StateOfIncorporation)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">Date of Incorporation:</td>
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:DateOfIncorporation)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">Business Type:</td>
								<td class="rightalign">
									<!-- 5520 - Business Type Code
												C = Corporation
												P = Partnership
												S = Sole Proprietor -->
									<xsl:attribute name="code">
										<xsl:value-of select="normalize-space(prd:BusinessTypeIndicator/@code)"></xsl:value-of>
									</xsl:attribute>
									<xsl:value-of select="normalize-space(prd:BusinessTypeIndicator)"></xsl:value-of>
								</td>
							</tr>
							<tr>
								<td class="label">Years in Business:</td>
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:YearsInBusiness)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">Company Contracts and Titles:</td>
								<td class="rightalign">
									<xsl:for-each select="prd:KeyPersonnelExecutiveInformation">
									<div><xsl:value-of select="normalize-space(prd:KeyPersonnelExecutiveInformation/prd:Title)"></xsl:value-of></div>
									</xsl:for-each>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
				<td>	<!-- second column table -->
					<table class="firstColumn" border="0">
						<colgroup class="label"></colgroup>
						<colgroup class="value"></colgroup>
						<tbody>
							<tr>
								<td class="label">SIC Code:</td>
								<!-- @TODO ???? how to just show one?which one? -->
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:SICCodes/prd:SICCode)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">NAICS Code:</td>
								<!-- @TODO ???? how to just show one?which one? -->
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:NAICSCodes/prd:NAICSCode)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">Number of Employees:</td>
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:EmployeeSize)"></xsl:value-of></td>
							</tr>
							<tr>
								<td class="label">Sales:</td>
								<td class="rightalign"><xsl:value-of select="normalize-space(prd:SalesRevenue)"></xsl:value-of></td>
							</tr>
							<xsl:if test="normalize-space(prd:NonProfitIndicator)='N'">
							<tr>
								<td class="label">Non-Profit:</td>
								<td class="rightalign">Yes</td>
							</tr>
							</xsl:if>
							<xsl:if test="normalize-space(prd:PublicIndicator)='P'">
							<tr>
								<td class="label">Public Company:</td>
								<td class="rightalign">Yes</td>
							</tr>
							<!-- @TODO release 2? -->
							<!--<tr>
								<td class="label">Stock Exchange & Symbol:</td>
								<td class="rightalign"><xsl:value-of select="concate(normalize-space(prd:StockExchange/prd:StockExchangeDescription),normalize-space(prd:StockExchange/prd:StickerSymbol))"></xsl:value-of></td>
							</tr>-->
							<!--<tr>
								<td class="label">Other Exchanges:</td>
								<td class="rightalign"><xsl:value-of select="concate(normalize-space(prd:StockExchange/prd:StockExchangeDescription),normalize-space(prd:StockExchange/prd:StickerSymbol))"></xsl:value-of></td>
							</tr>-->
							</xsl:if>
							<tr></tr>	<!-- Intentionally putting a space line here -->
							<xsl:if test="prd:Fortune1000">
							<tr>
								<td class="label">Fortune 1000 Ranking</td>
								<td class="rightalign"></td>
							</tr>
							<xsl:if test="prd:Fortune1000/prd:Year1">
							<tr>
								<td class="label"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year1/prd:Year)"></xsl:value-of></td>
								<td class="rightalign"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year1/prd:Rank)"></xsl:value-of></td>
							</tr>
							</xsl:if>
							<xsl:if test="prd:Fortune1000/prd:Year2">
							<tr>
								<td class="label"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year2/prd:Year)"></xsl:value-of></td>
								<td class="rightalign"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year2/prd:Rank)"></xsl:value-of></td>
							</tr>
							</xsl:if>
							<xsl:if test="prd:Fortune1000/prd:Year3">
							<tr>
								<td class="label"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year3/prd:Year)"></xsl:value-of></td>
								<td class="rightalign"><xsl:value-of select="normalize-space(../prd:Fortune1000/prd:Year3/prd:Rank)"></xsl:value-of></td>
							</tr>
							</xsl:if>
							</xsl:if>
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table>
  </xsl:template>
</xsl:stylesheet>