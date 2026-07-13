<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:rsp="http://www.experian.com/NetConnectResponse"
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
  * Global variables and parameters
  *********************************************
  <xsl:param name="systemYear" select="'2006'" />
  -->
  <xsl:param name="product" select="'BPR'" />
  <xsl:param name="baseProduct" select="''" />

  <xsl:variable name="SingleLineHighCredit">
      <xsl:if test="//prd:BusinessProfile/prd:ExecutiveElements/prd:SingleLineHighCredit and number(//prd:BusinessProfile/prd:ExecutiveElements/prd:SingleLineHighCredit) != 0">		    		   		   
        <xsl:value-of select="//prd:BusinessProfile/prd:ExecutiveElements/prd:SingleLineHighCredit" />
      </xsl:if>
  </xsl:variable>
  <xsl:param name="singleLineHighCredit" select="$SingleLineHighCredit" />

  <xsl:variable name="titleColor">
    <xsl:value-of select="'#015CAE'" />
  </xsl:variable>

  <xsl:variable name="borderColor">
    <xsl:value-of select="'#015CAE'" />
  </xsl:variable>

  <!--
  *********************************************
  * include template
  *********************************************
  -->
  <xsl:include href="../common/ReportHeader.xslt" />
  <xsl:include href="../common/Util.xslt" />
  <xsl:include href="../common/case.xslt" />
  <xsl:include href="../common/BackToTop.xslt" />
  <xsl:include href="../common/CompanyInformation.xslt" />
  <xsl:include href="../common/MatchingNameAndAddress.xslt" />
  <xsl:include href="TradeFilingSummary.xslt" />
  <xsl:include href="../common/ErrorProcessing.xslt" />
  <xsl:include href="../common/CorporateLinkage.xslt" />
  <xsl:include href="CurrentDBT.xslt" />
  <xsl:include href="QuarterlyDBT.xslt" />
  <xsl:include href="MonthlyDBT.xslt" />
  <xsl:include href="PerformanceAnalysis.xslt" />
  <xsl:include href="ProfileSummary.xslt" />
  <xsl:include href="LegalFilingsCollections.xslt" />
  <xsl:include href="PaymentExperiences.xslt" />
  <xsl:include href="TradePaymentTotals.xslt" />
  <xsl:include href="MonthlyPaymentTrends.xslt" />
  <xsl:include href="QuarterlyPaymentTrends.xslt" />
  <xsl:include href="Inquiries.xslt" />
  <xsl:include href="UCCProfile.xslt" />
  <xsl:include href="CommercialFinance.xslt" />
  <xsl:include href="CompanyBackground.xslt" />
  <xsl:include href="AdditionalCompanyBackground.xslt" />
  <xsl:include href="StandardAndPoors.xslt" />
  <xsl:include href="BusinessStatement.xslt" />
  <xsl:include href="../common/ReportFooter.xslt" />


  <!--
  *********************************************
  * Initial template
  *********************************************
  -->  
  <xsl:template match="/">
    <html>
      <head>
        <title>Business Profile Report</title>

        <style type="text/css">
            td {font-size: 9pt; font-family: 'arial';}
        </style>
      </head>

      <body>
        <a name="top"></a>
        <basefont size="2" color="#486c92">
        <table width="715" border="0" cellspacing="0" cellpadding="0" align="center">
          <tr>
            <td>      
               
              <!-- BusinessProfile template -->
              <xsl:apply-templates select="//prd:BusinessProfile" />
              
            </td>
          </tr>
        </table>    
        </basefont>
      </body>
    </html>
  </xsl:template>


  <!--
  *********************************************
  * Business Profile template
  *********************************************
  -->
  <xsl:template match="prd:BusinessProfile" xml:space="preserve">
    <!-- Report Date  -->
    <xsl:variable name="reportDate">
      <xsl:value-of select="prd:BusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

    <!-- Report Header -->
    <xsl:call-template name="ReportHeader">
      <xsl:with-param name="reportType" select="'Business Profile'" />
      <xsl:with-param name="reportName" select="prd:BusinessNameAndAddress/prd:BusinessName" />
    </xsl:call-template>

    <br />

    <xsl:choose>
      <xsl:when test="normalize-space(prd:BusinessNameAndAddress/prd:ProfileType/@code) = 'NO RECORD' ">
        <xsl:call-template name="businessNotFound" />
      </xsl:when>

      <xsl:otherwise>

	    <!-- Identifying Information -->
	    <xsl:call-template name="CompanyInformation" />
	
	    <br />
    
	    <xsl:if test="prd:BusinessNameAndAddress/prd:MatchingNameAndAddress">
	      <!-- Matching Name And Address -->
	      <xsl:apply-templates select="prd:BusinessNameAndAddress/prd:MatchingNameAndAddress" />
  
	      <br />
	    </xsl:if>

	    <xsl:if test="prd:CorporateLinkage">
	      <!-- Corporate Linkage -->
	      <xsl:call-template name="CorporateLinkage" />
	    </xsl:if>

	    <!-- Profile Summary -->
	    <xsl:call-template name="ProfileSummary" />
	
	    <xsl:if test="prd:CollectionData or prd:Bankruptcy or prd:TaxLien or prd:JudgmentOrAttachmentLien">
	      <!-- LegalFilingsCollections -->
	      <xsl:call-template name="LegalFilingsCollections" />
	    </xsl:if>
	  
	    <xsl:if test="prd:TradePaymentExperiences">  
	      <!-- TradePaymentInformation -->
	      <xsl:call-template name="PaymentExperiences">
	        <xsl:with-param name="title" select="'Trade Payment Information'" />
	      </xsl:call-template>
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:PaymentTotals">
	      <!-- TradePaymentTotals -->
	      <xsl:call-template name="TradePaymentTotals" />
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:AdditionalPaymentExperiences">  
	      <!-- TradePaymentInformation -->
	      <xsl:call-template name="PaymentExperiences">
	        <xsl:with-param name="title" select="'Additional Payment Experiences'" />
	      </xsl:call-template>
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:IndustryPaymentTrends">
	      <!-- MonthlyPaymentTrends -->
	      <xsl:call-template name="MonthlyPaymentTrends" />
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:QuarterlyPaymentTrends">
	      <!-- QuarterlyPaymentTrends -->
	      <xsl:call-template name="QuarterlyPaymentHistory" />
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:Inquiry">
	      <!-- QuarterlyPaymentTrends -->
	      <xsl:call-template name="Inquiries" />
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:GovernmentFinancialExperiences">  
	      <!-- GovernmentFinancialExperiences -->
	      <xsl:call-template name="PaymentExperiences">
	        <xsl:with-param name="title" select="'Government Financial Profile'" />
	      </xsl:call-template>
	      <br />
	    </xsl:if>
	
	    <xsl:if test="prd:UCCFilingsSummaryCounts or prd:UCCFilings">
	      <!-- UCCProfile -->
	      <xsl:call-template name="UCCProfile" />
	      <br />
	    </xsl:if>
	  
	    <xsl:if test="prd:CommercialBankInformation or prd:InsuranceData or prd:LeasingInformation">
	      <!-- CommercialFinance -->
	      <xsl:call-template name="CommercialFinance" />
	    </xsl:if>
	
	    <xsl:if test="prd:CorporateInformation or prd:CorporateOwnerInformation or prd:CorporateLinkageSummary or prd:CorporateLinkageNameAndAddress or prd:DemographicInformation or prd:KeyPersonnelExecutiveInformation">
	      <!-- CompanyBackground -->
	      <xsl:call-template name="CompanyBackground" />
	      <br />
	    </xsl:if>

	    <xsl:if test="prd:StandardAndPoorsFinancialInformation">
	      <!-- StandardAndPoorsFinancialInformation -->
	      <xsl:call-template name="StandardAndPoors" />
	      <xsl:call-template name="BackToTop" />
	    </xsl:if>

	    <xsl:if test="prd:ConsumerStatement">
	      <!-- BusinessStatement -->
	      <xsl:call-template name="BusinessStatement" />
	      <br />
	    </xsl:if>
	
	    <!-- is it a limited report?  -->
	    <xsl:variable name="reportEnd">
	       <xsl:choose>		              
	         <xsl:when test="normalize-space(prd:BillingIndicator/@code) = 'A' ">
	            <xsl:value-of select="'End of limited'" />
	         </xsl:when>
	
	         <xsl:otherwise>
	            <xsl:value-of select="'End of report'" />
	         </xsl:otherwise>
	       </xsl:choose>    
	    </xsl:variable>
	
	    <!-- Report Footer -->
	    <xsl:call-template name="ReportFooter">
	      <xsl:with-param name="reportType" select="'BPR'" />
	      <xsl:with-param name="reportDate" select="$reportDate" />
	      <xsl:with-param name="reportEnd" select="$reportEnd" />
	    </xsl:call-template>
      </xsl:otherwise>
    </xsl:choose>    

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