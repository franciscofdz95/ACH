<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet
  version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns:ncr="http://www.experian.com/NetConnectResponse"
   xmlns:prd="http://www.experian.com/ARFResponse"
   >

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
  * Global variables and parameters
  *********************************************
  -->
  <xsl:variable name="baseProduct">
    <xsl:value-of select="'PPR'" />
  </xsl:variable>

  <xsl:variable name="baseProductName">
    <xsl:value-of select="'Premier Profile'" />
  </xsl:variable>

  <xsl:variable name="product">
    <xsl:value-of select="$baseProduct" />
  </xsl:variable>

  <!-- This is the official name of the report title.  Sometimes it will have additional
       text on top of $baseProductName -->
  <xsl:variable name="productTitle">
    <xsl:value-of select="$baseProductName" />
  </xsl:variable>

  <xsl:variable name="FusionChartHeight">
    <xsl:value-of select="'50px'" />
  </xsl:variable>

	<!-- This represents name of the target in search.  It could be a business name or an owner name.
	     This name will be used as part of the report title and PDF footer -->
	<xsl:variable name="searchName">
		<xsl:choose>
			<xsl:when test="//prd:ExpandedBusinessNameAndAddress/prd:BusinessName">
				<xsl:value-of select="normalize-space(//prd:ExpandedBusinessNameAndAddress/prd:BusinessName)" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when
						test="//prd:ExpandedBusinessNameAndAddress/prd:LegalName/prd:LegalBusinessName">
						<xsl:value-of
							select="normalize-space(//prd:ExpandedBusinessNameAndAddress/prd:LegalName/prd:LegalBusinessName)" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="''" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>


  <!--
  *********************************************
  * include template
  *********************************************
  -->
  <xsl:include href="../common/Global.xslt" />
  <xsl:include href="../common/ReportHeader.xslt" />
  <xsl:include href="../common/Util.xslt" />
  <xsl:include href="../common/Util2.xslt" />
  <xsl:include href="../common/case.xslt" />
  <xsl:include href="../common/BackToTop.xslt" />
  <xsl:include href="../common/ErrorProcessing.xslt" />
  <xsl:include href="../common/ReportFooter.xslt" />
  <xsl:include href="../common/ReportCSS.xslt"/>
  <xsl:include href="Demographic.xslt" />
  <xsl:include href="RiskDashboard.xslt" />
  <xsl:include href="BusinessFacts.xslt" />
  <xsl:include href="CommercialFraudShield.xslt" />
  <xsl:include href="CreditRiskScoreCreditLimitReccomendation.xslt" />
  <xsl:include href="ExecutiveSummary.xslt" />
  <xsl:include href="PaymentExperiences.xslt"/>
  <xsl:include href="PublicRecord.xslt"/>
  <xsl:include href="UCCProfile.xslt"/>
  <xsl:include href="CommercialFinance.xslt"/>
  <xsl:include href="AdditionalBusinessFacts.xslt"/>
  <xsl:include href="CorporateLinkage.xslt"/>
  <!--<xsl:include href="MergersAcquisitions.xslt"/>--> <!-- @TODO -->
  <xsl:include href="Inquiries.xslt"/>
  <xsl:include href="StandardAndPoors.xslt"/>
  <!--<xsl:include href="AccountXML.xslt"/>-->

  <!--
  *********************************************
  * Initial template
  *********************************************
  -->
  <xsl:template match="/">
    <html>
      <head>
        <title><xsl:value-of select="$baseProductName" /></title>
        <xsl:call-template name="ReportCSS"></xsl:call-template>

        <script language="Javascript">

		  function toggleView(flag)
			{
			  if (flag == 1){
				if (document.getElementById("UCCFilingSectionGTTen").style.display=='inline')
					{
				          document.getElementById("UCCFilingSectionGTTen").style.display='none';
					}
					  document.getElementById("minusDiv").style.display='none';
					  document.getElementById("plusDiv").style.display='inline';

					 }
			  else if (flag == 2){

				 if(document.getElementById("UCCFilingSectionGTTen").style.display='none')
					{
					  document.getElementById("UCCFilingSectionGTTen").style.display='inline';
					 }

					  document.getElementById("minusDiv").style.display='inline';
					  document.getElementById("plusDiv").style.display='none';

					 }
			 }

        </script>
		<!--<xsl:call-template name="FusionChartUtil"/>-->
      </head>

      <body class="useFSPrintPDF">
        <!--<xsl:call-template name="Initial_Data_Island"/>-->
        <!--<xsl:call-template name="Account_Data_XML"/>-->
        <a name="top"><font><!-- FOR IE7, DO NOT REMOVE THIS EMPTY TAG --></font></a>
        <font size="2" color="{$reportTextColor}">
        <table class="report_container" width="715" border="0" cellspacing="0" cellpadding="0" align="center">
          <tr>
            <td>

              <!-- Response template -->
              <xsl:apply-templates select="//prd:Products/prd:PremierProfile" />

            </td>
          </tr>
        </table>
        </font>
      </body>
    </html>
  </xsl:template>


  <!--
  *********************************************
  * Response template
  *********************************************
  -->
  <xsl:template match="prd:PremierProfile" xml:space="preserve">

    <!-- header template -->
    <xsl:apply-templates select="." mode="header" />

    <xsl:apply-templates select="." mode="PPR" />

    <xsl:apply-templates select="." mode="footer" />

  </xsl:template>


	<xsl:template match="prd:PremierProfile" mode="PPR" xml:space="preserve">
		<xsl:param name="standalone" select="1" />

		<xsl:choose>
			<xsl:when
				test="normalize-space(prd:BusinessNameAndAddress/prd:ProfileType/@code) = 'NO RECORD' ">
				<xsl:call-template name="businessNotFound" />
			</xsl:when>
			<!--
				@TODO Following template to be added
			-->
			<!--<xsl:when
				test="prd:ProcessingMessage/prd:ProcessingAction">
				<xsl:call-template name="processing" />
			</xsl:when>-->

			<xsl:otherwise>

				<!--<xsl:if test="not(boolean(number($standalone)))">
					<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td valign="top" height="20" align="center">
								<font size="4" color="{$titleColor}">
									<b>
										<a class="product_title" name="BPR">Premier Profile</a>
									</b>
								</font>
							</td>
						</tr>
					</table>
				</xsl:if>-->

				<!-- Identifying Information -->
				<xsl:call-template name="BusinessDemographic" >
			      <xsl:with-param name="reportName" select="$searchName" />
			      <xsl:with-param name="businessName" select="$searchName" />
				</xsl:call-template>
				<xsl:call-template name="BackToTop" />
				<xsl:call-template name="RiskDashboard" />
				<xsl:call-template name="BackToTop" />
				<!--<xsl:apply-templates select="prd:IntelliscoreScoreInformation" name="RiskDashboard" />-->
				<xsl:apply-templates select="//prd:BusinessFacts" />
				<xsl:call-template name="BackToTop" />
			  	<xsl:if test="prd:CommercialFraudShieldSummary">
					<xsl:call-template name="CommercialFraudShield">
				      <xsl:with-param name="reportName" select="$searchName" />
					</xsl:call-template>
				<xsl:call-template name="BackToTop" />
				</xsl:if>

				<xsl:if test="prd:IntelliscoreScoreInformation">
				<xsl:call-template name="CreditRiskScoreDetails"/>
				</xsl:if>

				<xsl:call-template name="BackToTop" />
				<xsl:call-template name="ExecutiveSummary" />
				<xsl:call-template name="BackToTop" />
				<xsl:call-template name="PaymentExperiences"/>
				<xsl:call-template name="PublicRecord"/>
				<xsl:call-template name="UCCProfile"/>
				<xsl:call-template name="CommercialFinance"/>
				<xsl:call-template name="AdditionalBusinessFacts"/>
				<xsl:call-template name="CorporateLinkage"/>
				<!--<xsl:call-template name="MergersAcquisitions"/>--> <!-- @TODO -->
				<xsl:call-template name="Inquiries"/>
				<xsl:call-template name="CompanyFinancialInformation"/>


				<br />

				<!--<xsl:if test="prd:CorporateLinkage">
					 Corporate Linkage
					<xsl:call-template name="CorporateLinkage" />
				</xsl:if>-->

				<!-- Profile Summary -->
				<!--<xsl:call-template name="ProfileSummary" />-->

				<!-- BPR Common Detail -->
				<!--<xsl:call-template name="BPRCommonDetail" />-->

			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
  <!--
  *********************************************
  * Header template
  *********************************************
  -->
  <xsl:template match="prd:PremierProfile" mode="header" xml:space="preserve">
    <!-- Report Header -->
    <xsl:call-template name="ReportHeader">
      <xsl:with-param name="reportType" select="$productTitle" />
      <xsl:with-param name="reportName" select="$searchName" />
      <xsl:with-param name="reportDate" select="prd:ExpandedBusinessNameAndAddress/prd:ProfileDate" />
      <xsl:with-param name="reportTime" select="prd:ExpandedBusinessNameAndAddress/prd:ProfileTime" />
    </xsl:call-template>

    <br />

  </xsl:template>


  <!--
  *********************************************
  * Footer template
  *********************************************
  -->
  <xsl:template match="prd:PremierProfile" mode="footer" xml:space="preserve">
    <!-- Report Footer -->
    <xsl:call-template name="ReportFooter">
      <xsl:with-param name="reportType" select="$product" />
    </xsl:call-template>
  </xsl:template>


  <!--
  *********************************************
  * this template overrides the built-in rule
  * do nothing with stuff I'm not interested in
  *********************************************
  -->
  <xsl:template match="text()|@*">
  </xsl:template>

</xsl:stylesheet>