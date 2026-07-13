<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:prd="http://www.experian.com/ARFResponse">
	<!--
  *********************************************
  * Output method
  *********************************************
  -->
	<xsl:output method="xml" indent="yes"/>
	<!--
  *********************************************
  * Risk Dashboard template
  *********************************************
  -->
	<xsl:template name="RiskDashboard">
		<xsl:variable name="BankruptcyCount">
			<xsl:choose>
				<xsl:when test="prd:ExpandedCreditSummary/prd:BankruptcyFilingCount">
					<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:BankruptcyFilingCount)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="TaxLienCount">
			<xsl:choose>
				<xsl:when test="prd:ExpandedCreditSummary/prd:TaxLienFilingCount">
					<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:TaxLienFilingCount)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="JudgmentCount">
			<xsl:choose>
				<xsl:when test="prd:ExpandedCreditSummary/prd:JudgmentFilingCount">
					<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:JudgmentFilingCount)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="HighRiskAlert">
			<xsl:choose>
        			<xsl:when test="prd:ExpandedCreditSummary/prd:CommercialFraudRiskIndicatorCount and (string(number(prd:ExpandedCreditSummary/prd:CommercialFraudRiskIndicatorCount)) != 'NaN')">
					<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CommercialFraudRiskIndicatorCount)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="0"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="textBoxHeight">
			<!-- xsl:choose>
		<xsl:when test="number(prd:ExpandedCreditSummary/prd:CurrentDBT) &gt; 60">	< ! - - @TODO ??? 60 range shall be adjusted??? - - >
			<xsl:value-of select="70+14+40+45+16"/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="45+16+14+60"/>
		</xsl:otherwise>
		</xsl:choose-->
			<xsl:value-of select="45+16+14+60"/>
		</xsl:variable>
		<xsl:variable name="alertClass">
			<xsl:choose>
				<xsl:when test="(prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=11] | prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=12] |
							 prd:CommercialFraudShieldSummary[number(prd:OFACMatchCode/@code)=13]) or
							 prd:CommercialFraudShieldSummary[prd:BusinessVictimStatementIndicator/@code='Y']">
					<xsl:value-of select="'scoreHighRisk'"/>
				</xsl:when>
				<xsl:when test="number(prd:ExpandedCreditSummary/prd:CommercialFraudRiskIndicatorCount) &gt; 0">
					<xsl:value-of select="'scoreMedRisk'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'scoreLowRisk'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="creditRecommendation">
			<xsl:choose>
				<xsl:when test="prd:ScoreTrendsCreditLimit/prd:CreditLimitAmount">
					<xsl:value-of select="format-number(number(prd:ScoreTrendsCreditLimit/prd:CreditLimitAmount),'$###,###,##0')"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'N/A'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<!-- <span style="float:left;width:74%;padding:0;margin:0;"> -->
		<table class="section" height="100%" cellspacing="0" cellpadding="0" style="width:100%;height:100%">
			<colgroup style="width:43%"/>
			<colgroup style="width:19%"/>
			<colgroup style="width:19%"/>
			<colgroup style="width:19%"/>
			<thead>
				<tr>
					<th colspan="4">
						<a class="report_section_title">Risk Dashboard</a>
					</th>
					<!--<th>Credit Assessment</th>-->
				</tr>
			</thead>
			<tbody>
				<tr>
					<td>
						<table style="width:100%;height:100%" height="100%" cellspacing="0" cellpadding="0">
							<tr class="subtitle">
								<th colspan="2" class="centerLabel">Risk Scores and Credit Limit Recommendation</th>
							</tr>
							<tr>
								<td align="center">
									<xsl:apply-templates select="prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) != $fsrModel]" mode="RiskDashboard"/>
								</td>
								<td align="center">
									<xsl:apply-templates select="prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) = $fsrModel]" mode="RiskDashboard"/>
								</td>
							</tr>
							<tr>
								<td colspan="2" style="text-align:center;vertical-align:middle;height:20px;" class="bottomborder">
									<xsl:choose>
										<xsl:when test="prd:IntelliscoreScoreInformation[number(prd:ScoreInfo/prd:Score) div 100 != $score999 and number(prd:ScoreInfo/prd:Score) div 100 != $score998]">
											<b>Score range: 1 - 100 percentile</b>
										</xsl:when>
										<xsl:otherwise>
											<xsl:call-template name="nbsp"/>
										</xsl:otherwise>
									</xsl:choose>
								</td>
							</tr>
							<tr>
								<td colspan="2" style="line-height:20px;height:20px;padding:0px 0 0 10px;vertical-align:middle;">
								Credit Limit Recommendation: <b>
										<xsl:value-of select="$creditRecommendation"/>
									</b>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table cellspacing="0" cellpadding="0" height="100%" style="width:100%;height:100%">
							<tr class="subtitle">
								<th class="centerLabel">
									<a href="#DBTDetails" style="text-decoration:none">Days Beyond Terms</a>
								</th>
							</tr>
							<tr>
								<td align="center" class="leftborder">
									<xsl:attribute name="style"><xsl:value-of select="concat('height:',$textBoxHeight,'px')"/></xsl:attribute>
									<a href="#DBTDetails" style="text-decoration:none">
										<xsl:choose>
											<xsl:when test="prd:ExpandedCreditSummary/prd:CurrentDBT">
												<xsl:choose>
													<!-- @TODO not clear from BRD, has to check again  -->
													<xsl:when test="normalize-space(prd:ExpandedCreditSummary/prd:CurrentDBT)='' and normalize-space(prd:ExpandedCreditSummary/prd:CurrentTradelineCount)='0'">
														<br/>
														<div>Company DBT</div>
														<div class="grayOuterBox" style="width:80px;margin-top:0px">
															<div class="whiteInnerBox">
																<div class="grayInnerBox" style="height:40px">
																	<div class="verticalMiddleBox" style="width:100%;height:100%">
																		<div class="wrapInner">
																			<div class="innerText">DBT Unavailable</div>
																		</div>
																	</div>
																</div>
															</div>
														</div>
													</xsl:when>
													<xsl:otherwise>
														<br/>
														<div>Company DBT</div>
														<xsl:choose>
															<xsl:when test="number(prd:ExpandedCreditSummary/prd:CurrentDBT) &lt; 6">
																<div class="MiddlePad MiddlePadGreen">
																	<!--<div class="title">DBT</div>-->
																	<div class="value">
																		<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"/>
																	</div>
																</div>
															</xsl:when>
															<xsl:when test="number(prd:ExpandedCreditSummary/prd:CurrentDBT) &lt; 16">
																<div class="MiddlePad MiddlePadYellow">
																	<!--<div class="title">DBT</div>-->
																	<div class="value">
																		<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"/>
																	</div>
																</div>
															</xsl:when>
															<xsl:otherwise>
																<div class="MiddlePad MiddlePadRed">
																	<!--<div class="title">DBT</div>-->
																	<div class="value">
																		<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"/>
																	</div>
																</div>
															</xsl:otherwise>
														</xsl:choose>
														<!--<div><xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"></xsl:value-of></div>-->
													</xsl:otherwise>
												</xsl:choose>
											</xsl:when>
											<xsl:otherwise>
												<br/>
												<div>Company DBT</div>
												<div class="grayOuterBox" style="width:80px;margin-top:0px">
													<div class="whiteInnerBox">
														<div class="grayInnerBox" style="height:40px">
															<div class="verticalMiddleBox" style="width:100%;height:100%">
																<div class="wrapInner">
																	<div class="innerText">DBT Unavailable</div>
																</div>
															</div>
														</div>
													</div>
												</div>
											</xsl:otherwise>
										</xsl:choose>
										<xsl:if test="prd:ExecutiveSummary/prd:IndustryDBT and string(number(prd:ExecutiveSummary/prd:IndustryDBT))!='NaN'">
											<div>Industry DBT: <xsl:value-of select="format-number(prd:ExecutiveSummary/prd:IndustryDBT,'###,##0')"/>
											</div>
										</xsl:if>
									</a>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table height="100%" cellspacing="0" cellpadding="0" style="width:100%;height:100%">
							<tr class="subtitle">
								<th class="centerLabel">
									<a href="#Public Record" style="text-decoration:none">Derogatory Legal</a>
								</th>
							</tr>
							<tr>
								<td align="center" class="leftborder">
									<xsl:attribute name="style"><xsl:value-of select="concat('height:',$textBoxHeight,'px')"/></xsl:attribute>
									<a href="#Public Record" style="text-decoration:none">
										<br/>
										<div>Original Filings</div>
										<!--@TODO Shall numeric validation be present here for all 3 fields?-->
										<xsl:choose>
											<xsl:when test="$BankruptcyCount &gt; 0">
												<div class="MiddlePad scoreHighRisk">
													<!--<div class="title">Filings</div>-->
													<div class="value">
														<xsl:value-of select="$BankruptcyCount+$TaxLienCount+$JudgmentCount"/>
													</div>
												</div>
											</xsl:when>
											<xsl:otherwise>
												<xsl:choose>
													<xsl:when test="$TaxLienCount+$JudgmentCount &gt; 0">
														<div class="MiddlePad scoreMedRisk">
															<!--<div class="title">Filings</div>-->
															<div class="value">
																<xsl:value-of select="$BankruptcyCount+$TaxLienCount+$JudgmentCount"/>
															</div>
														</div>
													</xsl:when>
													<xsl:otherwise>
														<div class="MiddlePad scoreLowRisk">
															<!--<div class="title">Filings</div>-->
															<div class="value">0</div>
														</div>
													</xsl:otherwise>
												</xsl:choose>
											</xsl:otherwise>
										</xsl:choose>
									</a>
								</td>
							</tr>
						</table>
					</td>
					<td>
						<table height="100%" cellspacing="0" cellpadding="0" style="width:100%;height:100%">
							<tr class="subtitle">
								<th class="centerLabel">
									<a href="#CommercialFraudShield" style="text-decoration:none">Fraud Alerts</a>
								</th>
							</tr>
							<tr>
								<td align="center" class="leftborder">
									<xsl:attribute name="style"><xsl:value-of select="concat('height:',$textBoxHeight,'px')"/></xsl:attribute>
									<a href="#CommercialFraudShield" style="text-decoration:none">
										<br/>
										<div>High Risk Alerts</div>
										<div class="MiddlePad {$alertClass}">
											<div class="value">
												<xsl:value-of select="$HighRiskAlert"/>
											</div>
										</div>
									</a>
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</tbody>
		</table>
		<!-- </span>
	<span style="clear:both"/> -->
	</xsl:template>
	<xsl:template match="prd:IntelliscoreScoreInformation" mode="RiskDashboard">
		<xsl:variable name="model">
			<xsl:value-of select="number(prd:ModelInformation/prd:ModelCode)"/>
		</xsl:variable>
		<xsl:variable name="score">
			<xsl:value-of select="number(prd:ScoreInfo/prd:Score) div 100"/>
		</xsl:variable>
		<xsl:variable name="scoreText">
			<xsl:choose>
				<xsl:when test="contains($score, '.')">
					<xsl:value-of select="normalize-space(substring-before($score, '.'))"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$score"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="riskClass">
			<xsl:choose>
				<xsl:when test="$model = $fsrModel">
					<xsl:value-of select="prd:RiskClass"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="RiskClassByScore">
						<xsl:with-param name="model" select="$model"/>
						<xsl:with-param name="score" select="$score"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="action">
			<xsl:choose>
				<xsl:when test="$riskClass = 1">
					<xsl:value-of select="$lowRiskText"/>
				</xsl:when>
				<xsl:when test="$riskClass = 2">
					<xsl:value-of select="$lowMedRiskText"/>
				</xsl:when>
				<xsl:when test="$riskClass = 3">
					<xsl:value-of select="$medRiskText"/>
				</xsl:when>
				<xsl:when test="$riskClass = 4">
					<xsl:value-of select="$medHighRiskText"/>
				</xsl:when>
				<xsl:when test="$riskClass = 5">
					<xsl:value-of select="$highRiskText"/>
				</xsl:when>
				<xsl:when test="prd:Action">
					<xsl:value-of select="normalize-space(prd:Action)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="actionColor">
			<xsl:choose>
				<xsl:when test="$riskClass = 1">
					<xsl:value-of select="$lowRiskColor"/>
				</xsl:when>
				<xsl:when test="$riskClass = 2">
					<xsl:value-of select="lowMedRiskColor"/>
				</xsl:when>
				<xsl:when test="$riskClass = 3">
					<xsl:value-of select="medRiskColor"/>
				</xsl:when>
				<xsl:when test="$riskClass = 4">
					<xsl:value-of select="medHighRiskColor"/>
				</xsl:when>
				<xsl:when test="$riskClass = 5">
					<xsl:value-of select="highRiskColor"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'#cccccc'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="scoreRiskClass">
			<xsl:choose>
				<xsl:when test="$riskClass = 1">
					<xsl:value-of select="'scoreLowRisk'"/>
				</xsl:when>
				<xsl:when test="$riskClass = 2">
					<xsl:value-of select="'scoreLowMedRisk'"/>
				</xsl:when>
				<xsl:when test="$riskClass = 3">
					<xsl:value-of select="'scoreMedRisk'"/>
				</xsl:when>
				<xsl:when test="$riskClass = 4">
					<xsl:value-of select="'scoreMedHighRisk'"/>
				</xsl:when>
				<xsl:when test="$riskClass = 5">
					<xsl:value-of select="'scoreHighRisk'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'scoreUnkownRisk'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="scoreName">
			<xsl:choose>
				<xsl:when test="$model=$ipV1Model or $model=$ipV1ScoreOnlyModel">
					<xsl:value-of select="'Intelliscore Plus'"/>
				</xsl:when>
				<xsl:when test="$model=$ciModel or $model=$ciScoreOnlyModel">
					<xsl:value-of select="'Commercial Intelliscore'"/>
				</xsl:when>
				<xsl:when test="$model=$fsrModel">
					<xsl:value-of select="'Financial Stability Risk'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="'Score'"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="predictText">
			<xsl:choose>
				<xsl:when test="$model=$ipV1Model or $model=$ipV1ScoreOnlyModel or $model=$ciModel or $model=$ciScoreOnlyModel">
					<xsl:value-of select="'serious future risk'"/>
				</xsl:when>
				<xsl:when test="$model=$fsrModel">
					<xsl:value-of select="'financial stability risk'"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>
		<xsl:attribute name="style"><xsl:value-of select="'height: 94px; width: 50%;'"/></xsl:attribute>
		<a href="#Score{$model}" style="text-decoration:none">
			<br/>
			<div>
				<xsl:value-of select="$scoreName"/>
			</div>
			<xsl:choose>
				<xsl:when test="$score = $score999">
					<div style="float:left; margin: 5px 3pt 5px 15px;">
						<div style="text-align:left;">
						Score unavailable.<br/>
						Information on file not proven to predict
						<xsl:value-of select="$predictText"/>.
					</div>
					</div>
				</xsl:when>
				<xsl:when test="$score = $score998">
					<div style="float:left; margin: 5px 3pt 5px 15px;">
						<div style="text-align:left;">
						Score unavailable.<br/>
						Bankruptcy on file.
					</div>
					</div>
				</xsl:when>
				<xsl:otherwise>
					<div style="float:left; margin: 5px 0pt 5px 15px;">
						<xsl:attribute name="class"><xsl:value-of select="concat('MiddlePad ',$scoreRiskClass)"/></xsl:attribute>
						<div class="value">
							<xsl:value-of select="$scoreText"/>
						</div>
					</div>
					<div style="margin: 20px 10px 0pt 0pt; width: 70px; float: right;">
						<xsl:value-of select="$action"/>
					</div>
				</xsl:otherwise>
			</xsl:choose>
		</a>
	</xsl:template>
</xsl:stylesheet>
