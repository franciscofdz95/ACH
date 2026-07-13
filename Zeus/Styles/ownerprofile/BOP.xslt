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
  <xsl:param name="index" select="1" />
  <xsl:param name="product" select="'BOP'" />
  <xsl:param name="baseProduct" select="''" />

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
  <xsl:include href="../common/ErrorProcessing.xslt" />
  <xsl:include href="CreditProfile.xslt" />
  <xsl:include href="../common/ReportFooter.xslt" />


  <!--
  *********************************************
  * Initial template
  *********************************************
  -->  
  <xsl:template match="/">
    <html>
      <head>
        <title>BOP Report</title>

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
              <xsl:apply-templates select="//prd:CreditProfile[$index]" mode="header" />
 
              <!-- CreditProfiletemplate -->
              <xsl:apply-templates select="//prd:CreditProfile[$index]" />

              <xsl:apply-templates select="//prd:CreditProfile[$index]" mode="footer" />
 
            </td>
          </tr>
        </table>    
        </basefont>
      </body>
    </html>
  </xsl:template>

  <!--
  *********************************************
  * Header template
  *********************************************
  -->
  <xsl:template match="prd:CreditProfile" mode="header" xml:space="preserve">
    <!-- Report Date  -->
    <xsl:variable name="reportDate">
      <xsl:value-of select="../prd:BusinessProfile/prd:BusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

    <!-- Report Header -->
    <xsl:call-template name="ReportHeader">
      <xsl:with-param name="reportType" select="'Business Owner Profile'" />
      <xsl:with-param name="reportName" select="prd:ConsumerIdentity/prd:Name/prd:Gen" />
      <xsl:with-param name="reportDate" select="$reportDate" />
      <xsl:with-param name="reportTime" select="../prd:BusinessProfile/prd:BusinessNameAndAddress/prd:ProfileTime" />
      <xsl:with-param name="SubscriberNumber" select="../prd:BusinessProfile/prd:InputSummary/prd:SubscriberNumber" />
      <xsl:with-param name="InquiryTransactionNumber" select="../prd:BusinessProfile/prd:InputSummary/prd:InquiryTransactionNumber" />
      <xsl:with-param name="inquiry" select="../prd:BusinessProfile/prd:InputSummary/prd:Inquiry" />
    </xsl:call-template>
  
    <br />
    
  </xsl:template>
  
  <!--
  *********************************************
  * Footer template
  *********************************************
  -->
  <xsl:template match="prd:CreditProfile" mode="footer" xml:space="preserve">
    <!-- Report Date  -->
    <xsl:variable name="reportDate">
      <xsl:value-of select="../prd:BusinessProfile/prd:BusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

    <!-- is it a limited report?  -->
    <xsl:variable name="reportEnd">
       <xsl:choose>                     
         <xsl:when test="normalize-space(../prd:BusinessProfile/prd:BillingIndicator/@code) = 'A' ">
            <xsl:value-of select="'End of limited'" />
         </xsl:when>

         <xsl:otherwise>
            <xsl:value-of select="'End of report'" />
         </xsl:otherwise>
       </xsl:choose>    
    </xsl:variable>

    <!-- Report Footer -->
    <xsl:call-template name="ReportFooter">
      <xsl:with-param name="reportType" select="'BOP'" />
      <xsl:with-param name="reportDate" select="$reportDate" />
      <xsl:with-param name="reportEnd" select="$reportEnd" />
    </xsl:call-template>

  </xsl:template>
  

</xsl:stylesheet>