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
  * Trade Information template
  *********************************************
  -->
  <xsl:template name="TradeInformation">

    <xsl:variable name="numberTrades">
      <xsl:choose>
        <xsl:when test="prd:BusinessSummary/prd:NumberOfTradeLines and number(prd:BusinessSummary/prd:NumberOfTradeLines) != 'NaN'">
          <xsl:value-of select="number(prd:BusinessSummary/prd:NumberOfTradeLines)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'Not Available'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="bank">
      <xsl:choose>
        <xsl:when test="normalize-space(prd:BusinessSummary/prd:BankFlag/@code) = ''">
          <xsl:value-of select="'None Reported'" />
        </xsl:when>

        <xsl:when test="normalize-space(prd:BusinessSummary/prd:BankFlag/@code) = 'Y'">
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
          <font color="#0099cc"><b>Trade Information</b></font>
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
                            Total number of trades:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$numberTrades" /></b></td>
                        </tr>
                      </table>
                    </td>  
                  </tr>

                  <tr>
                    <td height="20">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="95%">
                            Bank information:</td>
                          <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$bank" /></b></td>
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