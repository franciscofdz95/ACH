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


  <xsl:template name="CreditRiskScoreDetails">

    <xsl:variable name="score">
      <xsl:value-of select="number(prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) != $fsrModel]/prd:ScoreInfo/prd:Score) div 100" />
    </xsl:variable>

    <xsl:variable name="currentDBT">
    	<xsl:value-of select="number(prd:ExpandedCreditSummary/prd:CurrentDBT)"></xsl:value-of>
    </xsl:variable>


	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<colgroup style="width:50%"/>
		<colgroup style="width:50%" />
		<thead>
			<tr>
				<th colspan="4">
					<xsl:comment>For left side label</xsl:comment>
					<a name="RiskScore" style="background:none"><a class="report_section_title">Credit Risk Score and Credit Limit Recommendation</a></a>
				</th>
			</tr>
		</thead>
		<tbody>
			<xsl:apply-templates select="prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) != $fsrModel]" mode="ScoreDetails" />

			<xsl:apply-templates select="prd:IntelliscoreScoreInformation[number(prd:ModelInformation/prd:ModelCode) = $fsrModel]" mode="ScoreDetails" />

			<tr class="subtitle">
				<th colspan="2">Credit Limit Recommendation</th>
			</tr>
			<tr>
				<td colspan="2"><table style="width:99%;"><tr>
					<td style="width:30%; padding:2px 3px 2px 5px;">
						<div class="label" style=""><b>Credit Limit Recommendation</b></div>
						<xsl:choose>
							<xsl:when test="$score=$score999">
								<div>
									Not available - This report does not include the data elements needed to create the credit limit recommendation.
								</div>
							</xsl:when>
							<xsl:when test="$score=$score998">
								<div>
									Not available - A credit limit recommendation is not available for a business with a bankruptcy filing within the last 24 months.
								</div>
							</xsl:when>
							<xsl:when test="$currentDBT &gt; 60">
								<div>
									Not available - A credit limit recommendation is not available for a business with a current DBT &gt; 60.
								</div>
							</xsl:when>
							<xsl:otherwise>
								<div style="padding-top:10px;text-align:center;">
									<xsl:if test="prd:ScoreTrendsCreditLimit/prd:CreditLimitAmount">
										<xsl:value-of select="format-number(number(prd:ScoreTrendsCreditLimit/prd:CreditLimitAmount),'$###,###,##0')"></xsl:value-of>
									</xsl:if>
								</div>
							</xsl:otherwise>
						</xsl:choose>
					</td>
					<td style="width:70%; padding:2px 5px 2px 2px;">
						This recommendation compares this business against similar businesses in the Experian business credit database.  It is based on trade information, industry, age of business and the Intelliscore Plus.  The recommendation is a guide.  The final decision must be made based on your company's business policies.
					</td>
				</tr></table></td>
			</tr>
		</tbody>
	</table>
  </xsl:template>



  <!--
  *********************************************
  * Business Facts template
  *********************************************
  -->
  <xsl:template match="prd:IntelliscoreScoreInformation" mode="ScoreDetails">
  	<xsl:variable name="model">
  		<xsl:choose>
  			<xsl:when test="prd:ModelInformation/prd:ModelCode">
  				<xsl:value-of select="number(prd:ModelInformation/prd:ModelCode)"/>
 			</xsl:when>
 			<xsl:otherwise>
 				<xsl:value-of select="-1"/>
 			</xsl:otherwise>
  		</xsl:choose>
  	</xsl:variable>

    <xsl:variable name="score">
      <xsl:value-of select="number(prd:ScoreInfo/prd:Score) div 100" />
    </xsl:variable>

    <xsl:variable name="scoreText">
      <xsl:choose>
        <xsl:when test="contains($score, '.')">
          <xsl:value-of select="normalize-space(substring-before($score, '.'))" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="$score" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="liklihoodText">
      <xsl:choose>
        <xsl:when test="$model=$fsrModel">
          <xsl:value-of select="'financial stability risk'"></xsl:value-of>
        </xsl:when>

        <xsl:otherwise>
        	<xsl:value-of select="'serious credit delinquencies'"></xsl:value-of>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="scoreHeaderText">
      <xsl:value-of select="concat('This score predicts the likelihood of ', $liklihoodText, ' within the next 12 months.  The score uses tradeline and collections information, public filings as well as other variables to predict future risk.  Higher scores indicate lower risk.')"></xsl:value-of>
    </xsl:variable>

    <xsl:variable name="scoreName">
       	<xsl:choose>
       		<xsl:when test="$model=$ipV1Model or $model=$ipV1ScoreOnlyModel">
       			<xsl:value-of select="'Intelliscore Plus'"></xsl:value-of>
       		</xsl:when>
       		<xsl:when test="$model=$ciModel or $model=$ciScoreOnlyModel">
       			<xsl:value-of select="'Commercial Intelliscore'" />
       		</xsl:when>
       		<xsl:when test="$model=$fsrModel">
       			<xsl:value-of select="'Financial Stability Risk'" />
       		</xsl:when>
       		<xsl:otherwise>
       			<xsl:value-of select="'Score'" />
       		</xsl:otherwise>
       	</xsl:choose>
    </xsl:variable>

  	<xsl:variable name="scoreMeterClass">
  		<xsl:choose>
       		<xsl:when test="$model=$ipV1Model or $model=$ipV1ScoreOnlyModel">
  				<xsl:value-of select="'meter214'"/>
  			</xsl:when>
       		<xsl:when test="$model=$ciModel or $model=$ciScoreOnlyModel">
  				<xsl:value-of select="'meter210'"/>
  			</xsl:when>
       		<xsl:when test="$model=$fsrModel">
       			<xsl:value-of select="'meter223'" />
       		</xsl:when>
  		</xsl:choose>
  	</xsl:variable>

	<xsl:variable name="riskClass">
      <xsl:choose>
        <xsl:when test="$model = $fsrModel">
          <xsl:value-of select="prd:RiskClass" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:call-template name="RiskClassByScore">
            <xsl:with-param name="model" select="$model" />
            <xsl:with-param name="score" select="$score" />
          </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
	</xsl:variable>

    <xsl:variable name="action">
      <xsl:choose>
        <xsl:when test="prd:Action">
          <xsl:value-of select="normalize-space(prd:Action)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="actionUpperCase">
      <xsl:value-of select="translate($action, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
    </xsl:variable>

    <xsl:variable name="customAction">
      <xsl:choose>
        <xsl:when test="$model = $fsrModel and $score!=$score998 and $score!=$score999
                        and $actionUpperCase != $lowRiskText
                        and $actionUpperCase != 'LOW TO MEDIUM RISK'
                        and $actionUpperCase != $medRiskText
                        and $actionUpperCase != 'MEDIUM TO HIGH RISK'
                        and $actionUpperCase != $highRiskText">
          <xsl:value-of select="$action" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

	<xsl:variable name="actionColor">
		<xsl:choose>
			<xsl:when test="$riskClass = 1">
				<xsl:value-of select="$lowRiskColor"></xsl:value-of>
			</xsl:when>
			<xsl:when test="$riskClass = 2">
				<xsl:value-of select="lowMedRiskColor"></xsl:value-of>
			</xsl:when>
			<xsl:when test="$riskClass = 3">
				<xsl:value-of select="medRiskColor"></xsl:value-of>
			</xsl:when>
			<xsl:when test="$riskClass = 4">
				<xsl:value-of select="medHighRiskColor"></xsl:value-of>
			</xsl:when>
			<xsl:when test="$riskClass = 5">
				<xsl:value-of select="highRiskColor"></xsl:value-of>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="'#cccccc'"></xsl:value-of></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

	<xsl:variable name="actionImage">
		<xsl:choose>
			<xsl:when test="$riskClass = 1">
				<xsl:value-of select="concat($basePath,'low-risk.gif')"/>
			</xsl:when>
			<xsl:when test="$riskClass = 2">
				<xsl:value-of select="concat($basePath,'low-medium-risk.gif')"/>
			</xsl:when>
				<xsl:when test="$riskClass = 3">
				<xsl:value-of select="concat($basePath,'medium-risk.gif')"/>
			</xsl:when>
			<xsl:when test="$riskClass = 4">
				<xsl:value-of select="concat($basePath,'medium-high-risk.gif')"/>
			</xsl:when>
			<xsl:when test="$riskClass = 5">
				<xsl:value-of select="concat($basePath,'high-risk.gif')"/>
			</xsl:when>
			<xsl:otherwise><xsl:value-of select="''"></xsl:value-of></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>

    <xsl:variable name="percentRanking">
      <xsl:choose>
        <xsl:when test="prd:PercentileRanking">
          <xsl:value-of select="format-number(prd:PercentileRanking div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="Probability">
      <xsl:choose>
        <xsl:when test="prd:Probability">
          <xsl:value-of select="concat(prd:Probability/prd:Amount, ':1')" />	<!-- ??? or prd:Probability/prd:Text ???  -->
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'???'" />	<!-- What to do here ??? -->
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="scoreBarTitle">
       	<xsl:choose>
       		<xsl:when test="$model=$fsrModel">
		       	<xsl:choose>
		       		<xsl:when test="$score=$score998">
		       			<xsl:value-of select="concat('Current ', $scoreName, ' Score: Score and Risk Class Unavailable due to Bankruptcy (', $score998, ')')" />
		       		</xsl:when>
		       		<xsl:when test="$score=$score999">
		       			<xsl:value-of select="concat('Current ', $scoreName, ' Score: Score and Risk Class Unavailable (', $score999, ')')" />
		       		</xsl:when>
		       	</xsl:choose>
       		</xsl:when>

       		<xsl:otherwise>
		       	<xsl:choose>
		       		<xsl:when test="$score=$score998">
		       			<xsl:value-of select="concat('Bankruptcy: ', $score998)" />
		       		</xsl:when>
		       		<xsl:when test="$score=$score999">
		       			<xsl:value-of select="concat('Score unavailable: ', $score999)" />
		       		</xsl:when>
		       		<xsl:otherwise>
		       			<xsl:value-of select="concat('Current Score: ', $score)" />
		       		</xsl:otherwise>
		       	</xsl:choose>
       		</xsl:otherwise>
       	</xsl:choose>
    </xsl:variable>

    <xsl:variable name="unscorableText">
       	<xsl:choose>
       		<xsl:when test="$model=$fsrModel">
		       	<xsl:choose>
		       		<xsl:when test="$score=$score998">
		       			<xsl:value-of select="concat('This score predicts the likelihood of ',$liklihoodText,' within the next 12 months. This report includes a bankruptcy within the last 24 months. Therefore a ', $scoreName, ' score cannot be created.')" />
		       		</xsl:when>
		       		<xsl:when test="$score=$score999">
		       			<xsl:value-of select="concat('This score predicts the likelihood of ',$liklihoodText,' within the next 12 months. Information on file not proven to predict ',$liklihoodText,'. Therefore a ', $scoreName, ' score cannot be created.')" />
		       		</xsl:when>
		       	</xsl:choose>
       		</xsl:when>

       		<xsl:otherwise>
		       	<xsl:choose>
		       		<xsl:when test="$score=$score998">
		       			<xsl:value-of select="'This report includes a bankruptcy within the last 24 months. Therefore an Intelliscore Plus cannot be created.'" />
		       		</xsl:when>
		       		<xsl:when test="$score=$score999">
		       			<xsl:value-of select="'This report does not include elements statistically proven to predict serious future delinquency. Therefore an Intelliscore Plus cannot be calculated.'" />
		       		</xsl:when>
		       	</xsl:choose>
       		</xsl:otherwise>
       	</xsl:choose>
    </xsl:variable>

	<tr class="subtitle">
		<th colspan="2">
			<a name="Score{$model}" style="background:none"><div class="smallLabel">Credit Risk Score: <xsl:value-of select="$scoreName"></xsl:value-of></div></a>
		</th>
	</tr>
	<xsl:if test="$model != $fsrModel">
		<tr><td colspan="2" style="padding:5px 0 10px 5px;"><div><b><xsl:value-of select="$scoreHeaderText" disable-output-escaping="yes"></xsl:value-of></b></div></td></tr>
	</xsl:if>

	<tr>
      <xsl:choose>
        <xsl:when test="$model = $fsrModel and $score != $score998 and $score != $score999">
          <td class="label graphicTitle" style="padding:5px 0 0 5px;"><xsl:value-of select="$scoreName"></xsl:value-of> Score: <xsl:value-of select="$scoreText"></xsl:value-of></td>
          <td class="label graphicTitle" style="padding:5px 0 0 0;">Risk Class: <xsl:value-of select="$riskClass"></xsl:value-of></td>
        </xsl:when>

        <xsl:otherwise>
          <td colspan="2" class="label graphicTitle" style="padding:5px 0 0 5px;">
	          <xsl:value-of select="$scoreBarTitle" />
          </td>
        </xsl:otherwise>
      </xsl:choose>
	</tr>
	<tr>
		<td style="padding:0 0 0 5px;">
			<div>
				<xsl:attribute name="class">
					<xsl:value-of select="concat('scoreGraphic',' ',$scoreMeterClass)"/>
				</xsl:attribute>
				<xsl:if test="$score!=$score998 and $score!=$score999">
					<div class="scoreValue">
						<xsl:attribute name="style">
							<xsl:value-of select="concat('left:',$scoreText*$scoreMeterWidth div 100+42-16,'px')"></xsl:value-of>
						</xsl:attribute>
						<xsl:value-of select="$scoreText"></xsl:value-of>
					</div>
					<div>
						<xsl:attribute name="class">scoreValueArrow</xsl:attribute>
						<xsl:attribute name="style">
							<xsl:value-of select="concat('left:',$scoreText*$scoreMeterWidth div 100+42-6,'px')"></xsl:value-of>
						</xsl:attribute>
						<xsl:call-template name="nbsp"/>
					</div>
				</xsl:if>
				<div class="scoreMeter"><xsl:call-template name="nbsp"/></div>
			</div>

			<xsl:if test="$model = $fsrModel and $score != $score998 and $score != $score999">
				<div style="padding-bottom:10px;"><xsl:value-of select="$scoreHeaderText" disable-output-escaping="yes"></xsl:value-of></div>
				<xsl:if test="prd:PubliclyHeldCompany and prd:PubliclyHeldCompany/@code = 'Y'">
					<div style="padding-bottom:10px;">
					Please note, this business is publicly traded.  Publicly traded companies
					are required to publish financial details including balance sheet, income
					statement and cash flow information that should be considered in conjunction
					with this score when assessing financial stability risk.
					</div>
				</xsl:if>
			</xsl:if>

			<xsl:if test="../prd:ScoreFactors/prd:ScoreFactor">
				<div class="label">Factors lowering the score</div>
				<ul class="list" style="padding:0px;margin:0px 10px 10px">
					<xsl:for-each select="../prd:ScoreFactors[number(prd:ModelCode) = $model]/prd:ScoreFactor">
						<li><xsl:value-of select="text()"></xsl:value-of></li>
					</xsl:for-each>
				</ul>
			</xsl:if>
		</td>
		<td>
			<xsl:choose>
				<xsl:when test="$score=$score998 or $score=$score999">
					<div class="label" style="padding-top:7px">Unscorable business</div>
					<div><xsl:value-of select="$unscorableText"></xsl:value-of></div>
				</xsl:when>
				<xsl:otherwise>
					<xsl:if test="$model != $fsrModel">
						<div class="label" style="padding-top:5px"><xsl:value-of select="$scoreName"></xsl:value-of> Risk Assessment</div>
						<div style="">Action or risk threshold, based on<br/> your company's thresholds:</div>
					</xsl:if>
					<div style="padding-top:3px;">
						<!--<div>
							<xsl:attribute name="style">
								<xsl:value-of select="concat('width:120px;margin-top:2px;padding:2px;color:$reportTextColor;font-weight:bold;text-align:center;background-color:',$actionColor)"/>
							</xsl:attribute>
							<div style="background-color:white;padding:2px">
								<div>
									<xsl:attribute name="style">
										<xsl:value-of select="concat('height:100%;padding:2px;background-color:',$actionColor)"/>
									</xsl:attribute>
									<xsl:value-of select="$action"></xsl:value-of>
								</div>
							</div>
						</div>-->
						<xsl:element name="img">
							<xsl:attribute name="style">border:none</xsl:attribute>
							<xsl:attribute name="src">
								<xsl:value-of select="$actionImage"></xsl:value-of>
							</xsl:attribute>
						</xsl:element>

						<!--<xsl:attribute name="class">
							<xsl:choose>
								<xsl:when test="contains($actionClass, 'LOW MED')">
									<xsl:value-of select="'LowMedScoreText'"></xsl:value-of>
								</xsl:when>
								<xsl:when test="contains($actionClass, 'LOW')">
									<xsl:value-of select="'LowScoreText'"></xsl:value-of>
								</xsl:when>
								<xsl:when test="contains($actionClass, 'MED')">
									<xsl:value-of select="'MedScoreText'"></xsl:value-of>
								</xsl:when>
								<xsl:when test="contains($actionClass, 'MED HIGH')">
									<xsl:value-of select="'MedHighScoreText'"></xsl:value-of>
								</xsl:when>
								<xsl:when test="contains($actionClass, 'HIGH')">
									<xsl:value-of select="'HighScoreText'"></xsl:value-of>
								</xsl:when>
								<xsl:otherwise>Unknown</xsl:otherwise>
							</xsl:choose>
						</xsl:attribute>-->
						<!--<xsl:value-of select="$action"></xsl:value-of>-->
					</div>

					<xsl:if test="$model = $fsrModel">
						<div style="padding-top:2px;">The risk class groups scores by risk into ranges of similar performance.
						Range 5 is the highest risk, range 1 is the lowest risk.</div>
					</xsl:if>

					<br/>
					<xsl:if test="$percentRanking!=''">
					<div class="label" style="padding-top:14px">
						<xsl:if test="$model != $fsrModel">
							<xsl:value-of select="$scoreName"></xsl:value-of>
						</xsl:if>
						Industry Risk Comparison</div>
					<div><b><xsl:value-of select="$percentRanking"/></b> of businesses indicate a higher likelihood of

				       	<xsl:choose>
				       		<xsl:when test="$model != $fsrModel">
				       			severe delinquency.
				       		</xsl:when>
				       		<xsl:when test="$model = $fsrModel">
				       			financial stability risk.
				       		</xsl:when>
				       	</xsl:choose>
					</div>
					<br/>
					</xsl:if>

					<xsl:comment>
					**** Below is disabled as we don't have data yet. ****
					<!-- <div class="label"><xsl:value-of select="$scoreName"></xsl:value-of> Delinquency Rate</div>
					<div>At the <b>50th</b> percentile good/bad odds are <xsl:value-of select="$Probability"></xsl:value-of>
						< ! - -<span><b>...</b></span>--><!-- @TODO 30:1 - - >
						with an average delinquency rate of <b><xsl:value-of select="$percentRanking"></xsl:value-of></b>
					</div> -->
					</xsl:comment>
				</xsl:otherwise>
			</xsl:choose>
		</td>
	</tr>

	<xsl:if test="normalize-space($customAction) != ''">
		<tr>
			<td colspan="2">
				<div style="border:1px solid {$borderColor}; margin: 0pt auto 10px; width: 80%; vertical-align: middle; line-height: 30px; text-align: center; padding:0 5px;"><b>Action or risk based on your company's specific score thresholds: </b><xsl:value-of select="$customAction"></xsl:value-of></div>
			</td>
		</tr>
	</xsl:if>

	<xsl:if test="../prd:ScoreTrendsCreditLimit and $model != $fsrModel">
		<tr class="subtitle">
			<th colspan="2">Quarterly Score Trends</th>
		</tr>
			<xsl:variable name="ScoreTrendNots">
				<xsl:choose>
				<xsl:when test="../prd:ScoreTrendsCreditLimit/prd:PriorQuarter[number(prd:Score) &lt; 0 or number(prd:Score) &gt; 100] or
				../prd:ScoreTrendsCreditLimit/prd:MostRecentQuarter[number(prd:Score) &lt; 0 or number(prd:Score) &gt; 100]">
					<xsl:value-of select="'* No score average available for this quarter'"/>
				</xsl:when>
				<xsl:otherwise>
				<xsl:value-of select="''"></xsl:value-of>
				</xsl:otherwise>
				</xsl:choose>
				<xsl:value-of select="''"></xsl:value-of>
			</xsl:variable>
		<tr>
			<td>
				<!-- Quarterly Score Trends Chart Here-->
				<!-- <img class="print_only fusion_chart_print" src="fusion_chart_print.QuarterlyScoreTrendsChart.gif">
					<xsl:attribute name="style">
						<xsl:value-of select="concat('width:400px;height:',$FusionChartHeight)"></xsl:value-of>
					</xsl:attribute>
				</img> -->
				<xsl:call-template name="dummyChart">
					<xsl:with-param name="altText" select="'Quarterly Score Trends Chart'"></xsl:with-param>
					<xsl:with-param name="width" select="'400px'"></xsl:with-param>
				</xsl:call-template>
				<div class="fusion_chart" chart_type="Column3D" id="QuarterlyScoreTrendsChart">
					<xsl:attribute name="style">
						<xsl:value-of select="concat('width:400px;height:',$FusionChartHeight)"></xsl:value-of>
					</xsl:attribute>
					<chart caption='Quarterly Score Trends' xAxisName='Month' yAxisName='Score' showValues='1' yAxisMinValue='0' yAxisMaxValue='100' slantLabels='1' labelDisplay='Rotate' paletteColors="67A8DB,67A8DB,67A8DB,67A8DB,67A8DB," exportEnabled="1">
							<xsl:for-each select="../prd:ScoreTrendsCreditLimit/prd:PriorQuarter">
								<xsl:sort select="position()" order="descending"/>
								<xsl:apply-templates select="current()"/>
							</xsl:for-each>
							<xsl:for-each select="../prd:ScoreTrendsCreditLimit/prd:MostRecentQuarter">
								<xsl:apply-templates select="current()"/>
							</xsl:for-each>
					</chart>
				</div>
				<div style="text-align:center;width:400px"><xsl:value-of select="$ScoreTrendNots"></xsl:value-of></div>
			</td>
			<td style="vertical-align:middle">
				The Quarterly Score Trends provide a view of the likelihood of delinquency over the past 12 months for this business. The trends will indicate if the score improved, remained stable, fluctuated or declined over the last 12 months.
			</td>
		</tr>
	</xsl:if>

  </xsl:template>


  <xsl:template match="prd:PriorQuarter | prd:MostRecentQuarter">
	<set>
		<xsl:variable name="noScoreNote">
			<xsl:choose>
				<xsl:when test="number(prd:Score) &lt; 0 or number(prd:Score) &gt; 100">
					<xsl:value-of select="'*'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:attribute name="name"><xsl:value-of select="prd:Quarter"/><xsl:value-of select="$noScoreNote"/></xsl:attribute>
		<xsl:attribute name="value">
		<xsl:choose>
			<xsl:when test="number(prd:Score) &lt; 0 or number(prd:Score) &gt; 100">
			<xsl:value-of select="''"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="prd:Score"/>
			</xsl:otherwise>
		</xsl:choose>
		</xsl:attribute>
		<xsl:call-template name="nbsp"/>
	</set>
  </xsl:template>
</xsl:stylesheet>