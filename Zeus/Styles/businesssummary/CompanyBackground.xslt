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
  * CompanyBackground template
  *********************************************
  -->
  <xsl:template name="CompanyBackground">

    <xsl:variable name="filingStatusCode">
      <xsl:value-of select="normalize-space(prd:BusinessSummary/prd:CorporateFilingStatus/@code)" />
    </xsl:variable>

    <xsl:variable name="filingStatus">
      <xsl:choose>		              
        <xsl:when test="$filingStatusCode = '' or normalize-space(prd:BusinessSummary/prd:CorporateFilingStatus) = '' ">
          <xsl:value-of select="'Not Available'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="prd:BusinessSummary/prd:CorporateFilingStatus" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateIncorporation">
      <xsl:choose>		              
        <xsl:when test="prd:BusinessSummary/prd:DateOfIncorporation and normalize-space(prd:BusinessSummary/prd:DateOfIncorporation) != '' and normalize-space(prd:BusinessSummary/prd:DateOfIncorporation) != '00000000'  ">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/dt/year'" />
	     <xsl:with-param name="value" select="prd:BusinessSummary/prd:DateOfIncorporation" />
	   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'Not Available'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="keyPersonnel1">
      <xsl:choose>		              
        <xsl:when test="prd:BusinessSummary/prd:KeyPersonnel1 and normalize-space(prd:BusinessSummary/prd:KeyPersonnel1) != ''">
          <xsl:value-of select="prd:BusinessSummary/prd:KeyPersonnel1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'None Reported'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="keyPersonnel2">
      <xsl:choose>		              
        <xsl:when test="prd:BusinessSummary/prd:KeyPersonnel2 and normalize-space(prd:BusinessSummary/prd:KeyPersonnel2) != ''">
          <xsl:text disable-output-escaping="yes">&lt;br&gt;</xsl:text>
          <xsl:value-of select="prd:BusinessSummary/prd:KeyPersonnel2" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="keyPersonnel3">
      <xsl:choose>		              
        <xsl:when test="prd:BusinessSummary/prd:KeyPersonnel3 and normalize-space(prd:BusinessSummary/prd:KeyPersonnel3) != ''">
          <xsl:text disable-output-escaping="yes">&lt;br&gt;</xsl:text>
          <xsl:value-of select="prd:BusinessSummary/prd:KeyPersonnel3" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- begin Company Background -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="#0099cc"><b>Company Background Information</b></font>
        </td>
      </tr>

      <tr>
        <td width="100%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">

            <tr>
              <td>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="40%">
                            Filing Status:</td>
                          <td width="60%" align="right" nowrap="nowrap"><b><xsl:value-of select="$filingStatus" /></b></td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="40%">
                            Date of Incorporation:</td>
                          <td width="60%" align="right" nowrap="nowrap"><b><xsl:value-of select="$dateIncorporation" /></b></td>
                        </tr>
                      </table>
                    </td>
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="40%" valign="top">
                            Principal Officer(s):</td>
                          <td width="60%" align="right" nowrap="nowrap"><b><xsl:value-of select="$keyPersonnel1" />
                            <xsl:value-of select="$keyPersonnel2" disable-output-escaping="yes" />
                            <xsl:value-of select="$keyPersonnel3" disable-output-escaping="yes" /></b></td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <!-- end Company Background -->
  </xsl:template>

</xsl:stylesheet>