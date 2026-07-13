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
  <xsl:variable name="Product">
    <xsl:value-of select="'BPRBOP'" />
  </xsl:variable>
   
  <xsl:param name="product" select="$Product" />
  <xsl:param name="baseProduct" select="'BPR'" />

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
  <xsl:include href="../common/ErrorProcessing.xslt" />
  <xsl:include href="../common/ReportFooter.xslt" />

  <xsl:include href="BusinessProfile.xslt" />
  <xsl:include href="../ownerprofile/CreditProfile.xslt" />

  <!--
  *********************************************
  * Initial template
  *********************************************
  -->  
  <xsl:template match="/">
    <html>
      <head>
        <title>BPR and BOP</title>

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

              <!-- BusinessPRofile template -->
              <xsl:apply-templates select="//prd:BusinessProfile" mode="header" />
              
              <!-- Intelliscore template -->
              <xsl:apply-templates select="//prd:BusinessProfile" mode="BPR">
                <xsl:with-param name="standalone" select="0" /> 
              </xsl:apply-templates>

              <xsl:if test="//prd:CreditProfile"> 
                <!-- Back to Top graphic -->
                <xsl:call-template name="BackToTop" />
              </xsl:if>

              <!-- BOP template -->
              <xsl:apply-templates select="//prd:CreditProfile">
                <xsl:with-param name="standalone" select="0" /> 
              </xsl:apply-templates>
              
              <xsl:apply-templates select="//prd:BusinessProfile" mode="footer" />
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
  <xsl:template match="prd:BusinessProfile" mode="header" xml:space="preserve">

    <!-- Report Header -->
    <xsl:call-template name="ReportHeader">
      <xsl:with-param name="reportType" select="'BPR and BOP'" />
      <xsl:with-param name="reportName" select="prd:BusinessNameAndAddress/prd:BusinessName" />
    </xsl:call-template>

    <br />
  </xsl:template>
  
  <!--
  *********************************************
  * Footer template
  *********************************************
  -->
  <xsl:template match="prd:BusinessProfile" mode="footer" xml:space="preserve">
    <!-- Report Date  -->
    <xsl:variable name="reportDate">
      <xsl:value-of select="prd:BusinessNameAndAddress/prd:ProfileDate" />
    </xsl:variable>

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
      <xsl:with-param name="reportType" select="$product" />
      <xsl:with-param name="reportDate" select="$reportDate" />
      <xsl:with-param name="reportEnd" select="$reportEnd" />
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