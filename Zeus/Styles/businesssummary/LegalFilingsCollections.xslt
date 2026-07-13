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
  * LegalFilingsCollections template
  *********************************************
  -->
  <xsl:template name="LegalFilingsCollections">

    <xsl:variable name="bankruptcy">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:BankruptcyFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:BankruptcyFlag/@code) = 'Y'">
          <xsl:value-of select="'Yes'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="taxLien">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:TaxLienFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:TaxLienFlag/@code) = 'Y'">
          <xsl:value-of select="'Yes'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="judgment">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:JudgmentFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:JudgmentFlag/@code) = 'Y'">
          <xsl:value-of select="'Yes'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="collection">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:CollectionFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:CollectionFlag/@code) = 'Y'">
          <xsl:value-of select="'Yes'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="ucc">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:UCCFilingsFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:UCCFilingsFlag/@code) = 'Y'">
          <xsl:value-of select="'Yes'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- begin Company Background -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="#0099cc"><b>Legal Filings and Collections</b></font>
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
                          <td width="95%">
                            Bankruptcy filings:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$bankruptcy" /></b></td>
                        </tr>
                      </table>
                    </td>  
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="95%">
                            Tax lien filings:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$taxLien" /></b></td>
                        </tr>
                      </table>
                    </td>  
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="95%">
                            Judgment filings:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$judgment" /></b></td>
                        </tr>
                      </table>
                    </td>  
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="95%">
                            Collection filings:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$collection" /></b></td>
                        </tr>
                      </table>
                    </td>  
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="95%">
                            UCC filings:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$ucc" /></b></td>
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