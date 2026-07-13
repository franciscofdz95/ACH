<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
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
  * ReportHeader template
  *********************************************
  -->
  <xsl:template name="ReportHeader">
    <xsl:param name="reportType" />
    <xsl:param name="reportName" />
    <xsl:param name="reportDate" select="prd:BusinessNameAndAddress/prd:ProfileDate" />
    <xsl:param name="reportTime" select="prd:BusinessNameAndAddress/prd:ProfileTime" />
    <xsl:param name="SubscriberNumber" select="prd:InputSummary/prd:SubscriberNumber" />
    <xsl:param name="InquiryTransactionNumber" select="prd:InputSummary/prd:InquiryTransactionNumber" />
    <xsl:param name="inquiry" select="prd:InputSummary/prd:Inquiry" />
    <xsl:param name="reference" />

    <xsl:variable name="Inquiry">
      <xsl:choose>		              
        <xsl:when test="contains($inquiry, 'INQF')">
          <xsl:variable name="inqINQF">
            <xsl:variable name="tmpINQF">
              <xsl:value-of select="substring-after(substring-after($inquiry, 'INQF/'), '/')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$tmpINQF" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>

          <xsl:text disable-output-escaping="yes"> </xsl:text><xsl:text disable-output-escaping="yes">Business ID Number:</xsl:text>
          <xsl:text disable-output-escaping="yes"> </xsl:text>
          <xsl:value-of select="concat(substring-before(substring-after($inquiry, 'INQF/'), '/'), ' / ', $inqINQF)" />
        </xsl:when>

        <xsl:when test="contains($inquiry, 'NARQ') and $product = 'CFIBP' ">
          <xsl:variable name="inqNARQ">
            <xsl:variable name="tmpNARQ">
              <xsl:value-of select="substring-after($inquiry, 'CA-//')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$tmpNARQ" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>
          <xsl:value-of select="$inqNARQ" />
        </xsl:when>

        <xsl:when test="contains($inquiry, 'NARQ')">
          <xsl:variable name="inqNARQ">
            <xsl:variable name="tmpNARQ">
              <xsl:value-of select="substring-after($inquiry, 'NARQ/')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$tmpNARQ" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>
          <xsl:value-of select="$inqNARQ" />

<!--
          <xsl:variable name="inqName">
            <xsl:variable name="InqName">
              <xsl:value-of select="substring-before(substring-after($inquiry, 'NARQ/'), ';')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$InqName" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="inqAddr">
            <xsl:variable name="InqAddr">
              <xsl:value-of select="substring-before(substring-after($inquiry, 'CA-'), '/')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$InqAddr" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>

          <xsl:variable name="inqShort">
              <xsl:value-of select="substring-after($inquiry, 'CA-')" />
          </xsl:variable>

          <xsl:variable name="inqCitySt">
            <xsl:variable name="InqCitySt">
              <xsl:value-of select="substring-before(substring-after($inqShort, '/'), '/')" />
            </xsl:variable>

	     <xsl:call-template name="convertcase">
	        <xsl:with-param name="toconvert" select="$InqCitySt" />
	        <xsl:with-param name="conversion" select="'lower'" />
	     </xsl:call-template>
          </xsl:variable>

          <xsl:value-of select="$inqName" />

          <xsl:if test="string-length($inqAddr) > 0">
             <xsl:value-of select="concat(' / ' , $inqAddr)" />
          </xsl:if>

          <xsl:if test="string-length($inqCitySt) > 0">
             <xsl:value-of select="concat(' / ' , $inqCitySt)" />
          </xsl:if>
-->

        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$inquiry" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="ProfileTime">
      <xsl:value-of select="prd:BusinessNameAndAddress/prd:ProfileTime" />
    </xsl:variable>

    <xsl:variable name="colspan">
      <xsl:choose>		              
        <xsl:when test="prd:InputSummary/prd:Comments">		    		   		   
          <xsl:value-of select="'1'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'2'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>

        <td valign="middle" width="98%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <!-- report title, company -->
            <tr>
              <td colspan="2">
                <font size="3" color="#193385"><b>
                <xsl:value-of select="$reportType" /> - 
                <xsl:value-of select="$reportName" />
                </b></font>
              </td>
            </tr>

            <tr>
              <td colspan="2" height="15">
                <img src="/images/spacer.gif" width="1" height="15" alt=""/></td>
            </tr>

            <tr>
	       <td width="60%" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	         <b>Subcode:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$SubscriberNumber" /></font></td>
	       <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	         <xsl:if test="$product != 'CFI' and $product != 'CFIBP' and $product != 'CFICOMBO' ">
		         <b>Ordered:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
			    <xsl:call-template name="FormatDate">
			      <xsl:with-param name="pattern" select="'mo/dt/year'" />
			      <xsl:with-param name="value" select="normalize-space($reportDate)" />
			    </xsl:call-template>
			    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="normalize-space($reportTime)" />
			    <xsl:text disable-output-escaping="yes">&amp;nbsp;CST</xsl:text>
	         </xsl:if>
                </font>
	       </td>
            </tr>

            <xsl:choose>		              
              <xsl:when test="substring($product, 1, 3) = 'CFI' and normalize-space($reference) != '' ">
	            <tr>
		       <td width="60%" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
		         <b>Customer reference:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$reference" /></font></td>
		       <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
		         <b>Transaction number:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$InquiryTransactionNumber" /></font></td>
	            </tr>
              </xsl:when>
              <xsl:otherwise>
	            <tr>
		       <td colspan="2" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
		         <b>Transaction number:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$InquiryTransactionNumber" /></font></td>
	            </tr>
              </xsl:otherwise>
            </xsl:choose>		              

            <tr>
	       <td colspan="2"><font size="1" style="font-family: 'verdana';">
	         <b>Search Inquiry:</b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$Inquiry" /></font></td>
            </tr>

          </table>
        </td>

        <!-- logo gif -->
        <td valign="top" align="right">
          <img src="../images/logo_experian.gif" alt="Experian (sm) - A world of insight" />
        </td>
      </tr>
    </table>
  
  </xsl:template>
      
</xsl:stylesheet>