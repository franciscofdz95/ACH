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
  * CreditSnapshot template
  *********************************************
  -->
  <xsl:template name="CreditSnapshot">
    <xsl:variable name="riskCategoryName">
      <xsl:value-of select="prd:BusinessSummary/prd:RiskCategory" />
    </xsl:variable>

    <xsl:variable name="riskCategoryCode">
      <xsl:value-of select="prd:BusinessSummary/prd:RiskCategory/@code" />
    </xsl:variable>

    <xsl:variable name="riskCategoryColor">
      <xsl:choose>
        <xsl:when test="$riskCategoryCode = 'A'">
          <xsl:value-of select="'#009900'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ff0000'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="riskCategoryText">
	<xsl:call-template name="RiskCategoryDescriptionCodeTable">
	   <xsl:with-param name="code" select="$riskCategoryCode" />
	</xsl:call-template>
    </xsl:variable>


    <!-- begin Credit Snapshot -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="#0099cc">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0099cc" align="left" valign="middle" height="20">
                      <font color="#ffffff"><b><xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;</xsl:text>Credit Snapshot As Of 
			    <xsl:call-template name="FormatDate">
			      <xsl:with-param name="pattern" select="'mo-dt-year'" />
			      <xsl:with-param name="value" select="prd:BusinessNameAndAddress/prd:ProfileDate" />
			    </xsl:call-template>
                      </b></font></td>
                  </tr>

                  <tr>
                    <td height="9" valign="bottom">
                      <img src="../images/global/spacer.gif" border="0" width="1" height="9" alt="" /></td>
                  </tr>

                  <tr>
                    <td bgcolor="#ffffff" align="center">
                      <table width="98%" border="0" cellspacing="0" cellpadding="2">

                        <tr>
                          <td align="center"><b><font size="3" color="{$riskCategoryColor}"><xsl:value-of select="$riskCategoryName" /></font></b></td>
                        </tr>

                        <tr>
                          <td>
                            <img src="../images/spacer.gif" width="0" height="1" alt="" /></td>
                        </tr>

                        <tr>
                          <td align="left">
                            <xsl:value-of select="$riskCategoryText" disable-output-escaping="yes" /></td>
                        </tr>

                      </table>
                    </td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="2" alt="" /></td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <!-- end Credit Snapshot -->

  </xsl:template>

</xsl:stylesheet>