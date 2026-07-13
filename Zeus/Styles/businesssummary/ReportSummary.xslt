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
  * ReportSummary template
  *********************************************
  -->
  <xsl:template name="ReportSummary">

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    
      <tr>
        <!-- credit snapshot column -->
        <td width="49%" valign="top">

          <!-- CreditSnapshot -->
          <xsl:call-template name="CreditSnapshot" />
          <br />

        </td>

        <td width="2%">
          <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        </td>

        <!-- legal, trade counts etc. column -->
        <td width="49%" valign="top">

          <!-- LegalFilingsCollections -->
          <xsl:call-template name="LegalFilingsCollections" />
          <br />

        </td>
      </tr>

      <tr>
        <!-- credit snapshot column -->
        <td width="49%" valign="top">

          <!-- CompanyBackground -->
          <xsl:call-template name="CompanyBackground" />
          <br />

        </td>

        <td width="2%">
          <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
        </td>

        <!-- legal, trade counts etc. column -->
        <td width="49%" valign="top">

          <!-- TradeInformation -->
          <xsl:call-template name="TradeInformation" />
          <br />

        </td>
      </tr>
    </table>

  </xsl:template>

</xsl:stylesheet>