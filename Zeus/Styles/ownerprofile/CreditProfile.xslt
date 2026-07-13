<?xml version="1.0" encoding="UTF-8"?>
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
  * include template
  *********************************************
  -->
  <xsl:include href="IdentifyingInformation.xslt" />
  <xsl:include href="SignatoryProfileSummary.xslt" />
  <xsl:include href="ConsumerStatement.xslt" />
  <xsl:include href="OFACWarning.xslt" />
  <xsl:include href="FraudShield.xslt" />
  <xsl:include href="LegalFilings.xslt" />
  <xsl:include href="Inquiries.xslt" />
  <xsl:include href="TradeInformation.xslt" />


  <!--
  *********************************************
  * CreditProfile template
  *********************************************
  -->
  <xsl:template match="prd:CreditProfile" xml:space="preserve">
    <xsl:param name="standalone" select="1" />

    <xsl:choose>
      <xsl:when test="normalize-space(../prd:BusinessProfile/prd:BusinessNameAndAddress/prd:ProfileType/@code) = 'NO RECORD' and normalize-space(../prd:BusinessProfile/prd:ProprietorNameAndAddress[number(position())]/prd:ProfileType/@code) = 'NO RECORD'   ">
        <xsl:call-template name="businessOwnerNotFound">
           <xsl:with-param name="msg" select="prd:InformationalMessage[prd:MessageNumber = '07']/prd:MessageText" />
        </xsl:call-template>
      </xsl:when>

      <xsl:otherwise>
        <xsl:if test="position() &gt; 1">
          <!-- Back to Top graphic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="not(boolean(number($standalone)))">
          <br />
        </xsl:if>

        <!-- Identifying Information -->
        <xsl:call-template name="IdentifyingInformationBOP">
          <xsl:with-param name="index" select="position()" />
          <xsl:with-param name="underline" select="0" />
          <xsl:with-param name="titleSize" select="4" />
        </xsl:call-template>
    
        <br />
    
        <xsl:if test="prd:ProfileSummary">
          <!-- Signatory Profile Summary -->
        <xsl:call-template name="SignatoryProfileSummary">
            <xsl:with-param name="position" select="position()" />
        </xsl:call-template>
  
          <!-- Back to Top grapic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="prd:Statement">
          <!-- Consumer Statement -->
          <xsl:call-template name="ConsumerStatement" />
    
          <!-- Back to Top grapic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="prd:InformationalMessage and number(prd:InformationalMessage/prd:MessageNumber) = 57 and starts-with(prd:InformationalMessage/prd:MessageText, '1200 ')  ">
          <!-- Consumer Statement -->
          <xsl:call-template name="OFACWarning" />
    
          <!-- Back to Top grapic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="prd:FraudServices/prd:Indicator">
          <!-- Fraud Shield -->
          <xsl:call-template name="FraudShield" />
    
          <!-- Back to Top grapic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="prd:PublicRecord">
    
          <!-- Legal Filings -->
          <xsl:call-template name="LegalFilings" />
        </xsl:if>
    
        <xsl:if test="prd:Inquiry">
    
          <!-- Inquiries -->
          <xsl:call-template name="InquiriesBOP" />
    
          <!-- Back to Top grapic -->
          <xsl:call-template name="BackToTop" />
        </xsl:if>
    
        <xsl:if test="prd:TradeLine">
    
          <!-- TradeInformation -->
          <xsl:call-template name="TradeInformation" />
          <br />
        </xsl:if>
    
      </xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


</xsl:stylesheet>