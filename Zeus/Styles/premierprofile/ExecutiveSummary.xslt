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
  <xsl:template name="ExecutiveSummary">
  	<xsl:apply-templates select="prd:ExpandedCreditSummary"></xsl:apply-templates>
	<xsl:call-template name="PaymentExperiencesPaymentTrending"/>
  	<!--<xsl:apply-templates select="prd:ExecutiveSummary"></xsl:apply-templates>-->
  </xsl:template>

  <xsl:template match="prd:ExpandedCreditSummary">
    <!--<xsl:variable name="score">
      <xsl:value-of select="(prd:ScoreInfo/prd:Score)" />
    </xsl:variable>-->

    <xsl:variable name="IndustryPaymentComparisonText">
    	<xsl:choose>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:IndustryPaymentComparison/@code) = 'F'">
    			<xsl:value-of select="'Has paid sooner than 50% of similar businesses'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:IndustryPaymentComparison/@code) = 'H'">
    			<xsl:value-of select="'Has paid slower than 50% of similar businesses'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:IndustryPaymentComparison/@code) = 'L'">
    			<xsl:value-of select="'Has paid slower than 70% of similar businesses'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:IndustryPaymentComparison/@code) = 'S'">
    			<xsl:value-of select="'Has paid the same as similar businesses'"></xsl:value-of>
    		</xsl:when>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="PaymentTrendText">
    	<xsl:choose>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'B'">
    		<xsl:value-of select="'Payments are increasing late, but better than industry'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'I'">
    		<xsl:value-of select="'Payments are improving'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'L'">
    		<xsl:value-of select="'Payments are increasingly late'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'N'">
    		<xsl:value-of select="'No payment trend identifiable'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'P'">
    		<xsl:value-of select="'Payments are improving, but slower than industry'"></xsl:value-of>
    		</xsl:when>
    		<xsl:when test="normalize-space(../prd:ExecutiveSummary/prd:PaymentTrendIndicator/@code) = 'S'">
    		<xsl:value-of select="'Payments are stable'"></xsl:value-of>
    		</xsl:when>
    		<xsl:otherwise><xsl:value-of select="'Payment trend indicator not available'"></xsl:value-of></xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="MostFreqPurchaseTerms">
    	<xsl:choose>
    	<xsl:when test="../prd:ExecutiveSummary/prd:CommonTerms or ../prd:ExecutiveSummary/prd:CommonTerms2 or ../prd:ExecutiveSummary/prd:CommonTerms3">
    	<xsl:value-of select="normalize-space(../prd:ExecutiveSummary/prd:CommonTerms)"/>
    	<xsl:if test="normalize-space(../prd:ExecutiveSummary/prd:CommonTerms) !='' and (normalize-space(../prd:ExecutiveSummary/prd:CommonTerms2) !='' or normalize-space(../prd:ExecutiveSummary/prd:CommonTerms3) !='')">
    		<xsl:value-of select="','"/>
    	</xsl:if>
    	<xsl:value-of select="normalize-space(../prd:ExecutiveSummary/prd:CommonTerms2)"/>
    	<xsl:if test="normalize-space(../prd:ExecutiveSummary/prd:CommonTerms2) !='' and normalize-space(../prd:ExecutiveSummary/prd:CommonTerms3)!=''">
    		<xsl:value-of select="','"/>
    	</xsl:if>
    	<xsl:value-of select="normalize-space(../prd:ExecutiveSummary/prd:CommonTerms3)"/>
    	</xsl:when>
    	<xsl:otherwise>
    		<xsl:value-of select="'Industry purchasing terms not available'"/>
    	</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="currentDBT">
    	<xsl:choose>
    		<xsl:when test="prd:CurrentDBT">
    			<xsl:value-of select="number(prd:CurrentDBT)"></xsl:value-of>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="-1"></xsl:value-of>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="currentDBTText">
		<xsl:choose>
			<xsl:when test="$currentDBT &lt; 0">
				<xsl:value-of select="'Not Available'"></xsl:value-of>	<!-- ???@TODO 0 or N/A -->
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="format-number($currentDBT,'###,##0')"></xsl:value-of>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
    <xsl:variable name="currentDBTIndex">
    	<xsl:choose>
    		<xsl:when test="$currentDBT &lt; 0">
    			<xsl:value-of select="0"/>
    		</xsl:when>
    		<xsl:when test="$currentDBT &lt; 6">
    			<xsl:value-of select="1"/>
    		</xsl:when>
    		<xsl:when test="$currentDBT &gt; 5 and $currentDBT &lt; 16">
    			<xsl:value-of select="2"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="3"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="currentDBTRangeText">
    	<xsl:choose>
    		<xsl:when test="$currentDBT &lt; 0">
    			<xsl:value-of select="''"/>
    		</xsl:when>
    		<xsl:when test="$currentDBT &lt; 6">
    			<xsl:value-of select="'80% of businesses have a DBT range of 0-5.'"/>
    		</xsl:when>
    		<xsl:when test="$currentDBT &gt; 5 and $currentDBT &lt; 16">
    			<xsl:value-of select="'11% of businesses have a DBT range of 6-15.'"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="'9% of businesses have a DBT range of 16+.'"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="AllTradelineCount">
    	<xsl:choose>
    		<xsl:when test="prd:AllTradelineCount">
    			<xsl:value-of select="number(prd:AllTradelineCount)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="AllCollectionCount">
    	<xsl:choose>
    		<xsl:when test="prd:CollectionCount">
    			<xsl:value-of select="number(prd:CollectionCount)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="TradeCollectionCount">
    	<!-- Woraround sumup all trade lines and open collections -->
    	<xsl:value-of select="$AllTradelineCount+$AllCollectionCount"/>
    </xsl:variable>
    <xsl:variable name="AllTradelineBalance">
    	<xsl:choose>
    		<xsl:when test="prd:AllTradelineBalance">
    			<xsl:value-of select="number(prd:AllTradelineBalance)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="AllCollectionBalance">
    	<xsl:choose>
    		<xsl:when test="prd:CollectionBalance">
    			<xsl:value-of select="number(prd:CollectionBalance)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="TradeCollectionBalance">
    	<!-- Woraround sumup all trade lines and open collections -->
    	<xsl:value-of select="$AllTradelineBalance+$AllCollectionBalance"/>
    </xsl:variable>
    <xsl:variable name="CurrentTradelineCount">
    	<xsl:choose>
    		<xsl:when test="prd:ActiveTradelineCount">
    			<xsl:value-of select="number(prd:ActiveTradelineCount)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="CurrentAccountBalance">
    	<xsl:choose>
    		<xsl:when test="prd:CurrentAccountBalance">
    			<xsl:value-of select="number(prd:CurrentAccountBalance)"/>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="0"/>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
    <xsl:variable name="SixMonthsAverageRange">
    	<xsl:choose>
	    	<xsl:when test="prd:HighBalance6Months and prd:LowBalance6Months">
    			<xsl:value-of select="concat(format-number(number(prd:LowBalance6Months),'$###,###,##0'),' - ',format-number(number(prd:HighBalance6Months),'$###,###,##0'))"/>
   			</xsl:when>
   			<xsl:otherwise>
   				<xsl:choose>
   					<xsl:when test="prd:HighBalance6Months">
   						<xsl:value-of select="concat(format-number(number(prd:HighBalance6Months),'$###,###,##0'),' -')"/>
   					</xsl:when>
   					<xsl:when test="prd:LowBalance6Months">
   						<xsl:value-of select="concat('- ', format-number(number(prd:LowBalance6Months),'$###,###,##0'))"/>
   					</xsl:when>
   					<xsl:otherwise>
   						<xsl:value-of select="'N/A'"/>
   					</xsl:otherwise>
   				</xsl:choose>
   			</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>

	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<colgroup style="width:31%"/>
		<colgroup style="width:1%"/>
		<colgroup style="width:38%"/>
		<colgroup style="width:1%"/>
		<colgroup style="width:29%"/>
		<thead>
			<tr>
				<th colspan="5"><a class="report_section_title">Payment and Legal Filings Summary</a></th>
			</tr>
			<tr class="subtitle">
				<th><a name="DBT" href="#DBTDetails" style="text-decoration:none">Payment Performance</a></th>
				<th></th>
				<th><a href="#TradeCollections" style="text-decoration:none">Trade and Collection Balance</a></th>
				<th></th>
				<th><a name="PublicRecord" href="#Public Record" style="text-decoration:none">Legal Filings</a></th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td style="padding:0 5px;">
					<div class="tableheight">
						<span class="label forImage">Current DBT:<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="$currentDBTText"/></span>
					</div>
					<div class="tableheight">
							<span class="label">
								<xsl:variable name="DBTDate">
								   <xsl:choose>
								   <xsl:when test="../prd:ExecutiveSummary/prd:PredictedDBTDate">
					    		   <xsl:call-template name="FormatDate">
					    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
					    		     <xsl:with-param name="value" select="../prd:ExecutiveSummary/prd:PredictedDBTDate" />
					    		   </xsl:call-template>
					    		   </xsl:when>
					    		   <xsl:otherwise>
					    		   	 <xsl:value-of select="''"/>
					    		   </xsl:otherwise>
					    		   </xsl:choose>
					    		</xsl:variable>
					    		<xsl:choose>
					    			<xsl:when test="$DBTDate!=''">
					    				<xsl:value-of select="concat('Predicted DBT as ',$DBTDate)"/>
					    			</xsl:when>
					    			<xsl:otherwise>
					    				<xsl:value-of select="'Predicted DBT'"/>
					    			</xsl:otherwise>
					    		</xsl:choose>
							:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign"><xsl:choose>
								<xsl:when test="../prd:ExecutiveSummary/prd:PredictedDBT">
									<xsl:value-of select="number(../prd:ExecutiveSummary/prd:PredictedDBT)"></xsl:value-of>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'N/A'"></xsl:value-of>
								</xsl:otherwise>
							</xsl:choose></span>
						</div>
						<div class="tableheight">
							<span class="label">Monthly Average DBT:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign"><xsl:value-of select="format-number(number(prd:MonthlyAverageDBT),'###,###,##0')"></xsl:value-of></span>
						</div>
						<div class="tableheight">
							<span class="label">Highest DBT Previous 6 Months:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign"><xsl:value-of select="format-number(number(prd:HighestDBT6Months),'###,###,##0')"></xsl:value-of></span>
						</div>
						<div class="tableheight">
							<span class="label">Highest DBT Previous 5 Quarters:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign"><xsl:value-of select="format-number(number(prd:HighestDBT5Quarters),'###,###,##0')"></xsl:value-of></span>
						</div>
						<div class="tableheight">
							<span colspan="2" class="label">Payment Trend Indication:<xsl:call-template name="nbsp"/></span>
						</div>
						<div class="tableheight">
							<span style="float:left" class="indent1"><xsl:value-of select="$PaymentTrendText"/></span>
						</div>
				</td>
				<td></td>
				<td style="padding:0 5px;">
					<div class="tableheight">
						<span class="label">Total trade and collection (<xsl:value-of select="format-number($TradeCollectionCount,'###,###,##0')"/>):<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="format-number($TradeCollectionBalance,'$###,###,##0')"/></span>
					</div>
					<div class="tableheight">
						<span class="label indent1">All trades (<xsl:value-of select="format-number($AllTradelineCount,'###,###,##0')"/>):<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="format-number($AllTradelineBalance,'$###,###,##0')"/></span>
					</div>
					<div class="tableheight">
						<span class="label indent1">All collections (<xsl:value-of select="format-number($AllCollectionCount,'###,###,##0')"/>):<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="format-number($AllCollectionBalance,'$###,###,##0')"/></span>
					</div>
					<div class="tableheight">
						<span class="label">Continuous trade (<xsl:value-of select="format-number($CurrentTradelineCount,'###,###,##0')"/>):<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="format-number($CurrentAccountBalance,'$###,###,##0')"/></span>
					</div>
					<div class="tableheight">
						<span class="label">6 month average:<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign"><xsl:value-of select="$SixMonthsAverageRange"/></span>
					</div>
					<div class="tableheight">
						<span class="label">Highest credit amount extended:<xsl:call-template name="nbsp"/></span>
						<span class="value rightalign">
								<xsl:choose>
									<xsl:when test="prd:SingleHighCredit">
										<xsl:value-of select="format-number(number(prd:SingleHighCredit),'$###,###,##0')"></xsl:value-of>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'N/A'"></xsl:value-of>
									</xsl:otherwise>
								</xsl:choose>
						</span>
					</div>
					<div class="tableheight">
						<span class="label">Most frequent industry purchasing terms:<xsl:call-template name="nbsp"/></span>
					</div>
					<div class="tableheight">
						<span style="float:left" class="indent1">
								<xsl:choose>
									<xsl:when test="$MostFreqPurchaseTerms!=''">
										<xsl:value-of select="$MostFreqPurchaseTerms"></xsl:value-of>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'N/A'"></xsl:value-of>
									</xsl:otherwise>
								</xsl:choose>
						</span>
					</div>
				</td>
				<td></td>
				<td style="padding:0 5px;">
					<div>
							<span class="label forImage">Bankruptcy:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
								<xsl:choose>
									<xsl:when test="prd:BankruptcyFilingCount and number(prd:BankruptcyFilingCount) &gt; 0">
										<!--<xsl:value-of select="number(prd:BankruptcyCount)"/>-->
										Yes
									</xsl:when>
									<xsl:otherwise>
										No
										<!--<xsl:value-of select="'0'"/>-->
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:if test="(number(prd:BankruptcyCount))=0">
								<div class="SmallPad SmallPadGreen">
									<div class="value">No</div>
								</div>
								</xsl:if>-->
								<!--<xsl:if test="(number(prd:BankruptcyCount)) &gt; 0">
								<div class="SmallPad SmallPadRed">
									<div class="value">Yes</div>
								</div>
								</xsl:if>-->
							</span>
						</div>
						<div>
							<span class="label forImage">Tax Lien filings:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
								<xsl:choose>
									<xsl:when test="prd:TaxLienFilingCount">
										<xsl:value-of select="number(prd:TaxLienFilingCount)"></xsl:value-of>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"/>
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:if test="(number(prd:TaxLienCount))=0">
								<div class="SmallPad SmallPadGreen">
									<div class="value">0</div>
								</div>
								</xsl:if>-->
								<!--<xsl:if test="(number(prd:TaxLienCount)) &gt; 0">
								<div class="SmallPad SmallPadRed">
									<div class="value"><xsl:value-of select="number(prd:TaxLienCount)"></xsl:value-of></div>
								</div>
								</xsl:if>-->
							</span>
						</div>
						<div>
							<span class="label forImage">Judgment filings:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
								<xsl:choose>
									<xsl:when test="prd:JudgmentFilingCount">
										<xsl:value-of select="number(prd:JudgmentFilingCount)"></xsl:value-of>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"/>
									</xsl:otherwise>
								</xsl:choose>
								<!--<xsl:if test="(number(prd:JudgmentCount))=0">
								<div class="SmallPad SmallPadGreen">
									<div class="value">0</div>
								</div>
								</xsl:if>-->
								<!--<xsl:if test="(number(prd:JudgmentCount)) &gt; 0">
								<div class="SmallPad SmallPadRed">
									<div class="value"><xsl:value-of select="number(prd:JudgmentCount)"></xsl:value-of></div>
								</div>
								</xsl:if>-->
							</span>
						</div>
						<div>
							<span class="label">Sum of legal filings:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
							<xsl:choose>
								<xsl:when test="prd:LegalBalance">
									<xsl:value-of select="format-number(number(prd:LegalBalance),'$###,###,##0')"></xsl:value-of>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'$0'"/>
								</xsl:otherwise>
							</xsl:choose>
							</span>
						</div>
						<div>
							<span class="label">UCC filings:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
								<xsl:choose>
									<xsl:when test="prd:UCCFilings">
										<xsl:value-of select="format-number(number(prd:UCCFilings),'###,##0')"></xsl:value-of>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="'0'"></xsl:value-of>
									</xsl:otherwise>
								</xsl:choose>
							</span>
						</div>
						<div>
							<span class="label">Cautionary UCC filings:<xsl:call-template name="nbsp"/></span>
							<span class="value rightalign">
								<xsl:choose>
								<xsl:when test="number(prd:UCCDerogCount) &gt; 0">Yes</xsl:when>
								<xsl:otherwise>No</xsl:otherwise>
								</xsl:choose>
							</span>
						</div>
						<!--<tr>
							<td colspan="2"><span class="comments">** Cautionary UCC Filings include one or more of the following collateral:
								Accounts, Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, Notes Receivable or Proceeds</span>
							</td>
						</tr>-->
				</td>
			</tr>
			<tr class="subtitle">
				<th colspan="5"><a name="DBTDetails">Industry Comparison</a></th>
			</tr>
			<tr>
				<td colspan="5">
					<table width="100%" cellspacing="0">
						<colgroup style="width:50%"/>
						<colgroup style="width:50%" />
						<tbody>
							<tr>
								<td style="padding:0 5px;">
									<!-- @TODO can't find information from BRD -->
									<div class="label">Industry DBT Range Comparison</div>
									<div class="value" style="padding-bottom:10px;">The current DBT of this business is <xsl:value-of select="$currentDBTText"/>. <xsl:value-of select="$currentDBTRangeText"/></div>
									<!-- @TODO can't find information from BRD -->
									<xsl:comment>DBT Chart</xsl:comment>
									<div class="label graphicTitle">DBT for this business: <xsl:value-of select="$currentDBTText"/></div>

									<div class="scoreGraphic dbtMeter">
										<div>
											<div class="scoreValueArrow">
												<xsl:if test="$currentDBTIndex=0">
												<xsl:attribute name="style">
													<xsl:value-of select="'display:none'"></xsl:value-of><!--	  Hidden, no DBT
												--></xsl:attribute>
												</xsl:if>
												<xsl:if test="$currentDBTIndex=1">
												<xsl:attribute name="style">
													<xsl:value-of select="concat('position:relative;left:',$currentDBT * 240 div 5 +2+26,'px')"></xsl:value-of>
												</xsl:attribute>
												</xsl:if>
												<xsl:if test="$currentDBTIndex=2">
												<xsl:attribute name="style">
													<xsl:value-of select="concat('position:relative;left:',240 + ($currentDBT - 6)*33 div (16-6) +2+26,'px')"></xsl:value-of>
												</xsl:attribute>
												</xsl:if>
												<xsl:if test="$currentDBTIndex=3">
												<xsl:attribute name="style">
													<xsl:value-of select="concat('position:relative;left:',240 + 33 + (27 div 2) +2+26,'px')"></xsl:value-of><!--	 No calculation for 51+, put it in middle-->
												</xsl:attribute>
												</xsl:if>
											</div>
											<!--<div class="DBTmeter">
												<div class="Green">0-15</div>
												<div class="Yellow">15-50</div>
												<div class="Red">51+</div>
											</div>-->
											<!--<div class="DBTmeterValue">
												<div class="Green">80%</div>
												<div class="Yellow">11%</div>
												<div class="Red">9%</div>
											</div>-->
											<!--<div class="bottomText">% of US businesses falling within DBT range</div>-->
										</div>
									</div>
								</td>
								<td>
									<xsl:if test="../prd:ExecutiveSummary/prd:AllIndustryDBT or ../prd:ExecutiveSummary/prd:IndustryDBT">
									<div class="label">DBT Norms</div>
									<div>
									<table>
										<colgroup style="width:10px"/>
										<colgroup style="width:80px"/>
										<colgroup style="width:60px"/>
										<tr>
											<td></td>
											<td>All industry:</td>
											<td><xsl:value-of select="number(../prd:ExecutiveSummary/prd:AllIndustryDBT)"></xsl:value-of></td>
										</tr>
										<tr>
											<td></td>
											<td>Same industry:</td>
											<td><xsl:value-of select="number(../prd:ExecutiveSummary/prd:IndustryDBT)"></xsl:value-of></td>
										</tr>
									</table>
									</div>
									<div></div>
									</xsl:if>
									<xsl:if test="$IndustryPaymentComparisonText!=''">
									<div class="label">Industry Payment Comparison</div>
									<div><span class="indent1"></span><xsl:value-of select="$IndustryPaymentComparisonText"></xsl:value-of></div>
									</xsl:if>
								</td>
							</tr>
						</tbody>
					</table>
				</td>
			</tr>
		</tbody>
	</table>
	<xsl:call-template name="BackToTop" />
  </xsl:template>


  <xsl:template name="PaymentExperiencesPaymentTrending">
    <xsl:if test="prd:IndustryPaymentTrends or prd:PaymentTrends">
	<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th colspan="10"><a class="report_section_title">Payment Trending</a></th>
			</tr>
		</thead>
		<tbody>
			<xsl:call-template name="paymentTrends"></xsl:call-template>
		</tbody>
	</table>
	</xsl:if>
  </xsl:template>

  <xsl:template name="paymentTrends">
    <xsl:variable name="currentDBT">
    	<xsl:choose>
    		<xsl:when test="prd:ExpandedCreditSummary/prd:CurrentDBT">
    			<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"></xsl:value-of>
    		</xsl:when>
    		<xsl:otherwise>
    			<xsl:value-of select="-1"></xsl:value-of>
    		</xsl:otherwise>
    	</xsl:choose>
    </xsl:variable>
		<xsl:if test="prd:PaymentTrends">
			<xsl:variable name="maxDBT">
			<xsl:for-each select="prd:PaymentTrends/prd:PriorMonth | prd:PaymentTrends/prd:CurrentMonth | prd:QuarterlyPaymentTrends/prd:PriorQuarter | prd:QuarterlyPaymentTrends/prd:MostRecentQuarter">
				<xsl:sort select="number(prd:DBT)" order="descending" data-type="number"></xsl:sort>
				<xsl:if test="position()=1">
					<!-- User floor instead of ceiling, difference is when all values are 0, we want 10, ceiling will just return 0 -->
					<xsl:value-of select="(floor(number(prd:DBT) div 10) +1)*10"/>
				</xsl:if>

			</xsl:for-each>
			</xsl:variable>
			<tr class="subtitle">
				<th colspan="10">DBT Trends</th>
			</tr>
			<tr>
				<td colspan="10" style="width:100%">
					<table cellspacing="0" cellpadding="0" style="width:100%"><tr><td>
									<xsl:comment>Monthly Chart</xsl:comment>
									<xsl:call-template name="dummyChart">
										<xsl:with-param name="altText" select="'Monthly Chart'"></xsl:with-param>
										<xsl:with-param name="width" select="'400px'"></xsl:with-param>
									</xsl:call-template>
									<!-- <img class="print_only fusion_chart_print" src="fusion_chart_print.DBTMonthlyChart.gif">
										<xsl:attribute name="style">
											<xsl:value-of select="concat('width:330px;height:',$FusionChartHeight)"></xsl:value-of>
										</xsl:attribute>
									</img> -->
									<div class="fusion_chart" chart_type="Column3D" id="DBTMonthlyChart" title="** Includes on or more of the followings colatteral: Accounts receivables, contract rights, hereafter acquired property, inventary, leases, notes, recievables or proceeds.">
										<xsl:attribute name="style">
											<xsl:value-of select="concat('width:330px;height:',$FusionChartHeight)"></xsl:value-of>
										</xsl:attribute>
									<chart caption='Monthly DBT Trends' showValues='1' slantLabels='1' labelDisplay='Rotate' yAxisMinValue='0'
										paletteColors="67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB," exportEnabled="1" >
										<xsl:attribute name="yAxisMaxValue"><xsl:value-of select="$maxDBT"/></xsl:attribute>
										<xsl:for-each select="prd:PaymentTrends/prd:PriorMonth">
										<xsl:sort select="position()" order="descending"></xsl:sort>
										<set>
											<xsl:attribute name="label">
										      <xsl:choose><xsl:when test="prd:Date">
										          <xsl:variable name="month">
										            <xsl:call-template name="FormatMonth">
										              <xsl:with-param name="monthValue" select="number(substring(prd:Date, 5, 2))" />
										              <xsl:with-param name="upperCase" select="true()" />
										            </xsl:call-template>
										          </xsl:variable>

										          <xsl:value-of select="concat(normalize-space($month), substring(normalize-space(prd:Date), 3, 2))" />
										        </xsl:when><xsl:otherwise>
										          <xsl:value-of select="'N/A'" />
										        </xsl:otherwise></xsl:choose>

											</xsl:attribute>
											<xsl:attribute name="value">
												<xsl:value-of select="number(prd:DBT)"></xsl:value-of>
											</xsl:attribute>
											<xsl:call-template name="nbsp"/>
										</set>
										</xsl:for-each>
										<set>
										<xsl:attribute name="name">
											<xsl:value-of select="'Current'"></xsl:value-of>
										</xsl:attribute>
										<xsl:attribute name="value">
											<xsl:choose>
												<xsl:when test="$currentDBT &lt; 0">
													<xsl:value-of select="''"></xsl:value-of>
												</xsl:when>
												<xsl:otherwise>
													<xsl:value-of select="$currentDBT"></xsl:value-of>
												</xsl:otherwise>
											</xsl:choose>
										</xsl:attribute>
											<xsl:call-template name="nbsp"/>
										</set>
									</chart>
									</div>
									<!--<div>**includes one or more of the following collateral:<br/>
										Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, notes, Receivables or Proceeds.
									</div>-->
								</td>
								<td>
									<xsl:comment>Quarterly DBT Trends</xsl:comment>
									<xsl:call-template name="dummyChart">
										<xsl:with-param name="altText" select="'Quarterly DBT Trends'"></xsl:with-param>
										<xsl:with-param name="width" select="'400px'"></xsl:with-param>
									</xsl:call-template>
									<!-- <img class="print_only fusion_chart_print" src="fusion_chart_print.DBTQuarterlyChart.gif">
										<xsl:attribute name="style">
											<xsl:value-of select="concat('width:330px;height:',$FusionChartHeight)"></xsl:value-of>
										</xsl:attribute>
									</img> -->
									<div class="fusion_chart" chart_type="Column3D" id="DBTQuarterlyChart">
										<xsl:attribute name="style">
											<xsl:value-of select="concat('width:330px;height:',$FusionChartHeight)"></xsl:value-of>
										</xsl:attribute>
									<chart caption='Quarterly DBT Trends' showValues='1' slantLabels='1' labelDisplay='Rotate' yAxisMinValue='0'
										paletteColors="67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB,67A8DB," exportEnabled="1">
										<xsl:attribute name="yAxisMaxValue"><xsl:value-of select="$maxDBT"/></xsl:attribute>
										<xsl:for-each select="prd:QuarterlyPaymentTrends/prd:PriorQuarter">
										<xsl:sort select="position()" order="descending"></xsl:sort>
										<set><xsl:attribute name="label">
												<xsl:value-of select="concat(normalize-space(prd:QuarterWithinYear/@code),'Q',substring(normalize-space(prd:YearOfQuarter), 3, 2))"></xsl:value-of>
											</xsl:attribute>
											<xsl:attribute name="value">
												<xsl:value-of select="number(prd:DBT)"></xsl:value-of>
											</xsl:attribute>
										<xsl:call-template name="nbsp"/></set>
										</xsl:for-each>
										<set><xsl:attribute name="label">
												<xsl:value-of select="concat(normalize-space(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter/prd:QuarterWithinYear/@code),'Q',substring(normalize-space(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter/prd:YearOfQuarter),3,2))"></xsl:value-of>
											</xsl:attribute>
											<xsl:attribute name="value">
												<xsl:value-of select="number(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter/prd:DBT)"></xsl:value-of>
											</xsl:attribute>
										<xsl:call-template name="nbsp"/></set>
									</chart>
									</div>
									<!--<div>**includes one or more of the following collateral:<br/>
										Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, notes, Receivables or Proceeds.
									</div>-->
								</td>
							</tr>
		</table></td></tr></xsl:if>
	<xsl:if test="prd:IndustryPaymentTrends">
			<tr class="subtitle">
				<th colspan="10">Monthly Payment Trends</th>
			</tr>
			<tr class="summaryhead">
				<th colspan="5"><div>Payment Trends Analysis</div><div><xsl:value-of select="concat(normalize-space(prd:SICCodes/prd:SIC),' - ',normalize-space(prd:SICCodes/prd:SIC/@code))"></xsl:value-of></div></th>
				<th colspan="5"><div>Account Status</div><div>Days Beyond Terms</div></th>
			</tr>
			<tr class="datahead">
				<td>Date Reported</td>
				<td colspan="2"><div style="text-align:center">Industry</div><div><div style="width:45%;float:left;text-align:center">Cur</div><div style="width:45%;float:left;text-align:center">DBT</div></div><div style="clear:both"/></td>
				<td>Business DBT</td>
				<td>Balance</td>
				<td>Cur</td>
				<td style="width:33px">1-30</td>
				<td style="width:33px">31-60</td>
				<td style="width:33px">61-90</td>
				<td style="width:33px">91+</td>
				<!--<td style="width:70px">Comments</td>-->
			</tr>
			<xsl:apply-templates select="prd:PaymentTrends/prd:CurrentMonth"/>
			<xsl:apply-templates select="prd:PaymentTrends/prd:PriorMonth"/>
			<xsl:comment>Leave an empty line here</xsl:comment>
			<tr><td></td></tr>

			<tr><td colspan="10" style="padding:0;margin:0;width:100%"><table cellspacing="0" cellpadding="0" width="100%" style="width:100%">
			<tr class="subtitle">
				<th colspan="9">Quarterly Payment Trends</th>
			</tr>
			<tr class="summaryhead">
				<th colspan="4">Payment History - Quarterly Averages</th>
				<th colspan="5"><div>Account Status</div><div>Days Beyond Terms</div></th>
			</tr>
			<tr class="datahead">
				<td>Quarter</td>
				<td>Months</td>
				<td>DBT</td>
				<td>Balance</td>
				<td>Cur</td>
				<td style="width:33px">1-30</td>
				<td style="width:33px">31-60</td>
				<td style="width:33px">61-90</td>
				<td style="width:33px">91+</td>
			</tr>
			<xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:MostRecentQuarter"/>
			<xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:PriorQuarter"/>
			</table></td></tr>
	</xsl:if>
	</xsl:template>

	<xsl:template match="prd:PaymentTrends/prd:CurrentMonth | prd:PaymentTrends/prd:PriorMonth">
	    <xsl:variable name="position">
	      <xsl:value-of select="position()" />
	    </xsl:variable>

	    <xsl:variable name="tmpMonth">
		<xsl:value-of select="number(substring(prd:Date, 5, 2))" />
	    </xsl:variable>

	    <xsl:variable name="tmpYear">
		<xsl:choose>
		  <xsl:when test="number($tmpMonth) = 0">
		    <xsl:value-of select="number(substring(prd:Date, 1, 4)) - 1" />
		  </xsl:when>

		  <xsl:otherwise>
		    <xsl:value-of select="number(substring(prd:Date, 1, 4))" />
		  </xsl:otherwise>
		</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="newMonth">
		<xsl:choose>
		  <xsl:when test="number($tmpMonth) = 0">
		    <xsl:value-of select="12" />
		  </xsl:when>

		  <xsl:otherwise>
		    <xsl:value-of select="$tmpMonth" />
		  </xsl:otherwise>
		</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="dateReported">
	      <xsl:choose>
	        <xsl:when test="name() = 'CurrentMonth'">
	          <xsl:value-of select="'CURRENT'" />
	        </xsl:when>

	        <xsl:when test="prd:Date">
	          <xsl:variable name="month">
	            <xsl:call-template name="FormatMonth">
	      		    <xsl:with-param name="monthValue" select="number(($newMonth -1 + 11) mod 12)+1" />
	      		    <xsl:with-param name="upperCase" select="true()" />
	      		  </xsl:call-template>
	          </xsl:variable>

	          <xsl:choose>
	          	<xsl:when test="$newMonth &gt; 1">
	          		<xsl:value-of select="concat(normalize-space($month), substring(normalize-space($tmpYear), 3, 2))" />
	          	</xsl:when>
	          	<xsl:otherwise>
	          		<xsl:value-of select="concat(normalize-space($month), substring(normalize-space($tmpYear - 1), 3, 2))" />
	          	</xsl:otherwise>
	          </xsl:choose>
	        </xsl:when>

	        <xsl:otherwise>
	          <xsl:value-of select="'N/A'" />
	        </xsl:otherwise>
	      </xsl:choose>
	    </xsl:variable>

    	<xsl:variable name="date">
    		<xsl:value-of select="prd:Date"/>
    	</xsl:variable>
	    <xsl:variable name="industryCurrent">
	      <xsl:choose>
	        <xsl:when test="../../prd:IndustryPaymentTrends/*[prd:Date/text()=$date]">
	          <xsl:value-of select="format-number(../../prd:IndustryPaymentTrends/*[prd:Date/text()=$date]/prd:CurrentPercentage div 100, '##0%')" />
	        </xsl:when>

	        <xsl:otherwise>
	          <xsl:value-of select="'N/A'" />
	        </xsl:otherwise>
	      </xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="industryDBT">
	      <xsl:choose>
	        <xsl:when test="../../prd:IndustryPaymentTrends/*[prd:Date/text()=$date]">
	          <xsl:value-of select="format-number(../../prd:IndustryPaymentTrends/*[prd:Date/text()=$date]/prd:DBT, '###,##0')" />
	        </xsl:when>

	        <xsl:otherwise>
	          <xsl:value-of select="'N/A'" />
	        </xsl:otherwise>
	      </xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="businessDBT">
	    	<xsl:choose>
	    		<xsl:when test="string(number(prd:DBT))!='NaN'">
	    			<xsl:value-of select="format-number(prd:DBT, '###,##0')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:value-of select="'N/A'" />
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="businessCurrent">
	    	<xsl:choose>
	    		<xsl:when test="string(number(prd:CurrentPercentage))!='NaN'">
	    			<xsl:value-of select="format-number(prd:CurrentPercentage div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:call-template name="nbsp"/>
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="balance">
	    	<xsl:choose>
	    		<xsl:when test="string(number(prd:TotalAccountBalance/prd:Amount))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:TotalAccountBalance/prd:Amount), '$###,###,##0')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:value-of select="'N/A'" />
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="DBT30">
	    	<xsl:choose>
	    		<xsl:when test="prd:DBT30 and number(prd:DBT30) != 0 and string(number(prd:DBT30))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:DBT30) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:call-template name="nbsp"/>
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="DBT60">
	    	<xsl:choose>
	    		<xsl:when test="prd:DBT60 and number(prd:DBT60) != 0 and string(number(prd:DBT60))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:DBT60) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:call-template name="nbsp"/>
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="DBT90">
	    	<xsl:choose>
	    		<xsl:when test="prd:DBT90 and number(prd:DBT90) != 0 and string(number(prd:DBT90))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:DBT90) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:call-template name="nbsp"/>
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

	    <xsl:variable name="DBT90Plus">
	    	<xsl:choose>
	    		<xsl:when test="prd:DBT120 and prd:DBT121Plus and number(prd:DBT120) != 0 and number(prd:DBT121Plus) != 0 and string(number(prd:DBT120))!='NaN' and string(number(prd:DBT121Plus))!='NaN'">
	    			<xsl:value-of select="format-number((number(prd:DBT120)+number(prd:DBT121Plus)) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:when test="prd:DBT120 and number(prd:DBT120) != 0 and string(number(prd:DBT120))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:DBT120) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:when test="prd:DBT121Plus and number(prd:DBT121Plus) != 0 and string(number(prd:DBT121Plus))!='NaN'">
	    			<xsl:value-of select="format-number(number(prd:DBT121Plus) div 100, '##0%')" />
	    		</xsl:when>
	    		<xsl:otherwise>
			        <xsl:call-template name="nbsp"/>
	    		</xsl:otherwise>
	    	</xsl:choose>
	    </xsl:variable>

			<tr>
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2 =1">
							<xsl:value-of select="'even'"/>
						</xsl:when>
						<xsl:when test="position() mod 2 =0">
							<xsl:value-of select="'odd'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<td><xsl:value-of select="$dateReported" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$industryCurrent" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$industryDBT" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$businessDBT" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$balance" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$businessCurrent" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT30" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT60" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT90" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT90Plus" disable-output-escaping="yes"/></td>
			</tr>
		</xsl:template>
		<xsl:template match="prd:QuarterlyPaymentTrends/prd:MostRecentQuarter | prd:QuarterlyPaymentTrends/prd:PriorQuarter">
		    <xsl:variable name="quarter">
		      <xsl:value-of select="concat('Q', prd:QuarterWithinYear/@code, ' - ', substring(prd:YearOfQuarter, 3, 2))" />
		    </xsl:variable>

		    <xsl:variable name="months">
		      <xsl:choose>
		        <xsl:when test="prd:QuarterWithinYear/@code = 1">
		          <xsl:value-of select="'JAN - MAR'" />
		        </xsl:when>

		        <xsl:when test="prd:QuarterWithinYear/@code = 2">
		          <xsl:value-of select="'APR - JUN'" />
		        </xsl:when>

		        <xsl:when test="prd:QuarterWithinYear/@code = 3">
		          <xsl:value-of select="'JUL - SEP'" />
		        </xsl:when>

		        <xsl:when test="prd:QuarterWithinYear/@code = 4">
		          <xsl:value-of select="'OCT - DEC'" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="'N/A'" />
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="DBT">
		      <xsl:choose>
		        <xsl:when test="prd:DBT">
		          <xsl:value-of select="number(prd:DBT)" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="'N/A'" />
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="balance">
		      <xsl:choose>
		        <xsl:when test="prd:TotalAccountBalance">
		          <xsl:value-of select="concat(prd:TotalAccountBalance/prd:Modifier/@code, format-number(prd:TotalAccountBalance/prd:Amount, '$###,###,##0'))" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="'$0'" />
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="current">
		      <xsl:choose>
		        <xsl:when test="prd:CurrentPercentage and number(prd:CurrentPercentage) != 0">
		          <xsl:value-of select="format-number(prd:CurrentPercentage div 100, '##0%')" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:call-template name="nbsp"/>
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="DBT30">
		      <xsl:choose>
		        <xsl:when test="prd:DBT30 and number(prd:DBT30) != 0 and string(number(prd:DBT30))!='NaN'">
		          <xsl:value-of select="format-number(prd:DBT30 div 100, '##0%')" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:call-template name="nbsp"/>
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="DBT60">
		      <xsl:choose>
		        <xsl:when test="prd:DBT60 and number(prd:DBT60) != 0 and string(number(prd:DBT60))!='NaN'">
		          <xsl:value-of select="format-number(prd:DBT60 div 100, '##0%')" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:call-template name="nbsp"/>
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="DBT90">
		      <xsl:choose>
		        <xsl:when test="prd:DBT90 and number(prd:DBT90) != 0 and string(number(prd:DBT90))!='NaN'">
		          <xsl:value-of select="format-number(prd:DBT90 div 100, '##0%')" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:call-template name="nbsp"/>
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="DBT90Plus">
		    	<xsl:choose>
		    		<xsl:when test="prd:DBT120 and prd:DBT121Plus and number(prd:DBT120) != 0 and number(prd:DBT121Plus) != 0 and string(number(prd:DBT120))!='NaN' and string(number(prd:DBT121Plus))!='NaN'">
		    			<xsl:value-of select="format-number((number(prd:DBT120)+number(prd:DBT121Plus)) div 100, '##0%')" />
		    		</xsl:when>
		    		<xsl:when test="prd:DBT120 and number(prd:DBT120) != 0 and string(number(prd:DBT120))!='NaN'">
		    			<xsl:value-of select="format-number(number(prd:DBT120) div 100, '##0%')" />
		    		</xsl:when>
		    		<xsl:when test="prd:DBT121Plus and number(prd:DBT121Plus) != 0 and string(number(prd:DBT121Plus))!='NaN'">
		    			<xsl:value-of select="format-number(number(prd:DBT121Plus) div 100, '##0%')" />
		    		</xsl:when>
		    		<xsl:otherwise>
				        <xsl:call-template name="nbsp"/>
		    		</xsl:otherwise>
		    	</xsl:choose>
		    </xsl:variable>

		    <xsl:variable name="bgColor">
		      <xsl:choose>
		        <xsl:when test="name() = 'MostRecentQuarter' or position() mod 2 = 0">
		          <xsl:value-of select="'#e5f5fa'" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="'#ffffff'" />
		        </xsl:otherwise>
		      </xsl:choose>
		    </xsl:variable>
			<tr>
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2 =1">
							<xsl:value-of select="'even'"/>
						</xsl:when>
						<xsl:when test="position() mod 2 =0">
							<xsl:value-of select="'odd'"/>
						</xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<td><xsl:value-of select="$quarter" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$months" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$balance" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$current" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT30" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT60" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT90" disable-output-escaping="yes"/></td>
				<td><xsl:value-of select="$DBT90Plus" disable-output-escaping="yes"/></td>
			</tr>

		</xsl:template>
</xsl:stylesheet>