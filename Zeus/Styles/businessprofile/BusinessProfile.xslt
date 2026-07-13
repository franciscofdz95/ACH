<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:prd="http://www.experian.com/ARFResponse">

  <!--
  *********************************************
  * import template
  *********************************************
  -->
  <xsl:import href="../common/CorporateLinkage.xslt" />


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
  * include template
  *********************************************
  -->
  <xsl:include href="../common/CompanyInformation.xslt" />
  <xsl:include href="TradeFilingSummary.xslt" />
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
  <xsl:include href="BusinessStatement.xslt" />
  <xsl:include href="StandardAndPoors.xslt" />



  <!--
  *********************************************
  * BusinessProfile template
  *********************************************
  -->
  <xsl:template match="prd:BusinessProfile | prd:Intelliscore" mode="BPR" xml:space="preserve">
    <xsl:param name="standalone" select="1" />

    <xsl:choose>
      <xsl:when test="normalize-space(prd:BusinessNameAndAddress/prd:ProfileType/@code) = 'NO RECORD' ">
        <xsl:call-template name="businessNotFound" />
      </xsl:when>

      <xsl:otherwise>

        <xsl:if test="not(boolean(number($standalone)))">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" height="20" align="center"><font size="4" color="{$titleColor}"><b><a name="BPR">Business Profile</a></b></font></td>
            </tr>
          </table>
        </xsl:if>

        <!-- Identifying Information -->
        <xsl:call-template name="CompanyInformation" />
    
        <br />
    
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
    
        <xsl:if test="prd:CorporateInformation or prd:CorporateOwnerInformation or prd:CorporateLinkageSummary or prd:CorporateLinkageNameAndAddress or prd:DemographicInformation or   prd:KeyPersonnelExecutiveInformation">
          <!-- CompanyBackground -->
          <xsl:call-template name="CompanyBackground" />
        <br />
        </xsl:if>

        <xsl:if test="prd:StandardAndPoorsFinancialInformation">
	       <!-- StandardAndPoorsFinancialInformation -->
	       <xsl:call-template name="StandardAndPoors" />
        </xsl:if>

        <xsl:if test="prd:ConsumerStatement">
          <!-- BusinessStatement -->
          <xsl:call-template name="BusinessStatement" />
          <br />
        </xsl:if>
    
      </xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


</xsl:stylesheet>