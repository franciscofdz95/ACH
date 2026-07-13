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
  <xsl:param name="systemYear" select="'2007'" />
  -->
  <xsl:param name="product" select="'BSUM'" />

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
  <xsl:include href="IdentifyingInformation.xslt" />
  <xsl:include href="../common/CorporateLinkage.xslt" />
  <xsl:include href="ReportSummary.xslt" />
  <xsl:include href="CreditSnapshot.xslt" />
  <xsl:include href="CompanyBackground.xslt" />
  <xsl:include href="LegalFilingsCollections.xslt" />
  <xsl:include href="TradeInformation.xslt" />
  <xsl:include href="../common/ReportFooter.xslt" />


  <!--
  *********************************************
  * Initial template
  *********************************************
  -->  
  <xsl:template match="/">
    <html>
      <head>
        <title>Business Summary Report</title>

          <style type="text/css">
            td {font-size: 9pt; font-family: 'arial';}
          </style>

      </head>
      <body>
        <basefont size="2" color="#486c92">
        <table width="715" border="0" cellspacing="0" cellpadding="0" align="center">
          <tr>
            <td>      
               
              <!-- BusinessSummary template -->
              <xsl:apply-templates select="//prd:Products/prd:BusinessSummary" />
              
            </td>
          </tr>
        </table>    
        </basefont>
      </body>
    </html>
  </xsl:template>


  <!--
  *********************************************
  * BusinessSummary template
  *********************************************
  -->
  <xsl:template match="prd:BusinessSummary" xml:space="preserve">
    <!-- Report Date  -->
    <xsl:variable name="reportDate">
      <xsl:value-of select="prd:BusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

    <!-- Report Header -->
    <xsl:call-template name="ReportHeader">
      <xsl:with-param name="reportType" select="'Business Summary'" />
      <xsl:with-param name="reportName" select="prd:BusinessNameAndAddress/prd:BusinessName" />
    </xsl:call-template>

    <br />
    <xsl:choose>
      <xsl:when test="normalize-space(prd:BusinessNameAndAddress/prd:ProfileType/@code) = 'NO RECORD' ">
        <xsl:call-template name="businessNotFound" />
      </xsl:when>

      <xsl:otherwise>

	    <!-- Identifying Information -->
	    <xsl:call-template name="IdentifyingInformation" />

	    <br />
  
	    <xsl:if test="prd:CorporateLinkage">
	      <!-- Corporate Linkage -->
	      <xsl:call-template name="CorporateLinkage" />
	    </xsl:if>

	    <!-- Report Summary -->
	    <xsl:call-template name="ReportSummary" />
	
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
	      <xsl:with-param name="reportType" select="'BSUM'" />
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