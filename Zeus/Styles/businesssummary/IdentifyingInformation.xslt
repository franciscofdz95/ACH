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
  * IdentifyingInformation template
  *********************************************
  -->
  <xsl:template name="IdentifyingInformation">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Identifying Information'"/>
    </xsl:call-template>
    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="#0099cc">
          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="4">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <!-- business data section -->
                  <xsl:choose>
                    <xsl:when test="prd:BusinessNameAndAddress">
                      <xsl:apply-templates select="prd:BusinessNameAndAddress"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <tr>
                        <td colspan="2">
                          <font color="#ff0000" size="1">
                            <b>No business data available</b>
                          </font>
                        </td>
                      </tr>
                    </xsl:otherwise>
                  </xsl:choose>
                  <!-- end business data section -->
                </table>
              </td>
            </tr>
          </table>
          <!-- end inner white box -->
        </td>
      </tr>
    </table>


    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /><br />
		Experian Business Summary is recommended for review of low dollar accounts. For other
		credit decisions, check Experian Business Profile by using the FR-Q number at top right
		corner when used the same business day there will be no charge for this Experian Business Summary.
        </td>
      </tr>
    </table>


  </xsl:template>
  <!--
  *********************************************
  * BusinessNameAndAddress template
  *********************************************
  -->
  <xsl:template match="prd:BusinessNameAndAddress" xml:space="preserve">
    <tr>
      <!-- company name etc column -->
      <td width="50%" valign="top">
        <font size="1" style="FONT-FAMILY: 'verdana';">
          <b>
            <xsl:value-of select="prd:BusinessName"/>
          </b>
          <xsl:if test="prd:StreetAddress">
            <br/>
            <xsl:value-of select="prd:StreetAddress"/>
          </xsl:if>
          <br/>
          <xsl:value-of select="normalize-space(prd:City)"/>, 
          <xsl:value-of select="prd:State"/>
          <xsl:text disable-output-escaping="yes"/>
          <xsl:value-of select="prd:Zip"/>
          <br/>
          <xsl:if test="prd:PhoneNumber">
            <xsl:call-template name="FormatPhone">
              <xsl:with-param name="value" select="translate(prd:PhoneNumber, '-', '')"/>
            </xsl:call-template>
          </xsl:if>
        </font>
      </td>
      <!-- end company name etc column -->
      <!-- file number etc column -->
      <td width="50%" valign="top">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">

          <tr>
            <td colspan="2">
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
	          <tr>
	            <td width="40%" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	              <b>Business Identification Number:</b></font></td>
	            <td width="60%" align="right" nowrap="nowrap">
	              <font size="1" style="FONT-FAMILY: 'verdana';">
	              <xsl:value-of select="prd:ExperianFileNumber"/></font></td>
	          </tr>
	        </table>
            </td>
          </tr>

          <xsl:if test="../prd:BusinessSummary/prd:TransactionNumber">
            <tr>
              <td width="40%" nowrap="nowrap">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                  <b>Full Report Number:</b></font></td>
              <td width="60%" align="right" nowrap="nowrap">
                <font size="1" style="FONT-FAMILY: 'verdana';">FR-<xsl:value-of select="../prd:BusinessSummary/prd:TransactionNumber"/></font></td>
            </tr>
          </xsl:if>

          <tr>
            <td width="40%" nowrap="nowrap">
              <font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Experian File Established:</b></font></td>
            <td width="60%" align="right" nowrap="nowrap">
              <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:choose>
                  <xsl:when test="prd:FileEstablishFlag/@code = 'P'">
                  PRIOR TO 01/1977
                </xsl:when>
                  <xsl:otherwise>
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="pattern" select="'mo/year'"/>
                      <xsl:with-param name="value" select="prd:FileEstablishDate"/>
                    </xsl:call-template>
                  </xsl:otherwise>
                </xsl:choose>
              </font>
            </td>
          </tr>

          <xsl:if test="prd:SIC">
            <tr>
              <td colspan="2">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
	            <tr>
	              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <b>SIC Code:</b></font></td> 
	              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <xsl:value-of select="translate(prd:SIC, 'amp;amp;', 'amp;')" /><xsl:if test="prd:SIC/@code != ''"> - <xsl:value-of select="prd:SIC/@code" /></xsl:if></font></td>
	            </tr>
                </table>
              </td>
            </tr>
          </xsl:if>
        </table>
      </td>
      <!-- end file number etc column -->
    </tr>
  </xsl:template>

</xsl:stylesheet>