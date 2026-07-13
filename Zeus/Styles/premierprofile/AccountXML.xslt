<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet
  version="1.0"
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
    * Account Data template
    * This template produces account data information
    * which could be used outside of the report area.
    *********************************************
    -->
  <xsl:template name="Account_Data_XML">
  	<xsl:param name="product" select="''" />

		<div id="div_account_xml" style="display:none;">
			<xml id="account_xml">
						<BusinessNameAndAddress>
							<BusinessName>
								<xsl:value-of select="//prd:ExpandedBusinessNameAndAddress/prd:LegalName/prd:LegalBusinessName | //prd:ExpandedBusinessNameAndAddress/prd:BusinessName" />
							</BusinessName>
							<ExperianFileNumber>
								<xsl:value-of select="normalize-space(//prd:ExpandedBusinessNameAndAddress/prd:ExperianBIN)" />
							</ExperianFileNumber>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:StreetAddress"/>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:City"/>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:State"/>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:Zip"/>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:PhoneNumber"/>
							<xsl:copy-of select="//prd:CompanyFinancialInformation/prd:FileEstablishedDate"/>
						</BusinessNameAndAddress>
						<ExecutiveSummary>
							<xsl:copy-of select="//prd:ExecutiveSummary/prd:CurrentDBT"/>
							<xsl:copy-of select="//prd:ExecutiveSummary/prd:PaymentTrendIndicator"/>
						</ExecutiveSummary>
						<ExecutiveElements>
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:BankruptcyCount"/>
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:TaxLienCount"/>
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:JudgmentCount"/>
							<CollectionCount>
								<xsl:value-of select="//prd:ExpandedCreditSummary/prd:CollectionCount"/>
							</CollectionCount>
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:LegalBalance"/>			<!-- @TODO cannot find where it is -->
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:SingleLineHighCredit"/>	<!-- @TODO cannot find where it is -->
							<xsl:copy-of select="//prd:ExpandedCreditSummary/prd:UCCDerogatoryCount"/>
							<TaxID>
								<xsl:value-of select="//prd:ExpandedBusinessNameAndAddress/prd:TaxID"/>
							</TaxID>
						</ExecutiveElements>
						<PaymentTotalsNew>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalAccountBalance/prd:Amount"/>
							<TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalHighCreditAmount"/>
							</TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT30"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT60"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT90"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT90Plus"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT"/>
						</PaymentTotalsNew>
						<PaymentTotalsContinous>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalAccountBalance/prd:Amount"/>
							<TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalHighCreditAmount/prd:Amount"/>
							</TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT30"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT60"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT90"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT90Plus"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT"/>
						</PaymentTotalsContinous>
						<PaymentTotalsCombined>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalAccountBalance/prd:Amount"/>
							<TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalHighCreditAmount/prd:Amount"/>
							</TotalHighCreditAmount>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT30"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT60"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT90"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT90Plus"/>
							<xsl:copy-of select="//prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT"/>
						</PaymentTotalsCombined>
						<DemographicInformation>
							<xsl:choose>
								<xsl:when test="//prd:DemographicInformation/prd:PrimarySICCode">
									<xsl:copy-of select="//prd:DemographicInformation/prd:PrimarySICCode"/>
								</xsl:when>
								<xsl:otherwise>
									<PrimarySICCode>
										<xsl:value-of select="//prd:BusinessNameAndAddress/prd:SIC/@code"/>
									</PrimarySICCode>
								</xsl:otherwise>
							</xsl:choose>
							<xsl:copy-of select="//prd:DemographicInformation/prd:EmployeeSizeOrLowRange"/>
							<xsl:copy-of select="//prd:DemographicInformation/prd:HighEmployeeRange"/>
							<xsl:copy-of select="//prd:DemographicInformation/prd:SalesRevenueOrLowRange"/>
							<xsl:copy-of select="//prd:DemographicInformation/prd:HighRangeOfSales"/>
						</DemographicInformation>
						<ScoreFactors>
							<xsl:copy-of select="//prd:ScoreFactors[number(prd:ModelCode) != $fsrModel]/prd:ScoreFactor"/>
						</ScoreFactors>
						<ModelCode>
							<xsl:copy-of select="//prd:ModelInformation[number(prd:ModelCode) != $fsrModel]/prd:ModelCode"/>
						</ModelCode>
						<Score>
							<xsl:copy-of select="//prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) != $fsrModel]/prd:ScoreInfo/prd:Score"/>
						</Score>
						<PercentileRanking>
							<xsl:copy-of select="//prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) != $fsrModel]/prd:PercentileRanking"/>
						</PercentileRanking>
						<premierProfile>
							<websiteurl><xsl:value-of select="normalize-space(//prd:ExpandedBusinessNameAndAddress/prd:WebsiteURL)"/></websiteurl>
							<DoingBusinessAs><xsl:value-of select="normalize-space(//prd:DoingBusinessAs/prd:DBAName)"/></DoingBusinessAs>
							<isNonProfit><xsl:value-of select="normalize-space(//prd:BusinessFacts/prd:NonProfitIndicator)"/></isNonProfit>
							<isPublic><xsl:value-of select="normalize-space(//prd:BusinessFacts/prd:PublicIndicator)"/></isPublic>
							<ticker><xsl:value-of select="normalize-space(//prd:Stocks/prd:TickerSymbol)"/></ticker>
							<matchingAddress>
							<xsl:copy-of select="//prd:ExpandedBusinessNameAndAddress/prd:MatchingBranchAddress"/>
							</matchingAddress>
							<sales><xsl:value-of select="number(//prd:BusinessFacts/prd:SalesRevenue)"/></sales>
							<noEmployee><xsl:value-of select="number(//prd:BusinessFacts/prd:EmployeeSize)"/></noEmployee>
							<naics><xsl:value-of select="normalize-space(//prd:NAICSCodes/prd:NAICS/@code)"/></naics>
							<sic><xsl:value-of select="normalize-space(//prd:SICCodes/prd:SIC/@code)"/></sic>
							<xsl:if test="normalize-space(//prd:ExpandedCreditSummary/prd:OFACMatch/@code)='Y'
											or number(//prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=11
											or number(//prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=12
											or number(//prd:CommercialFraudShieldSummary/prd:OFACMatchCode/@code)=13
											">
							<OFACMatch>true</OFACMatch>
							</xsl:if>
						</premierProfile>
			</xml>
		</div>
	</xsl:template>
</xsl:stylesheet>