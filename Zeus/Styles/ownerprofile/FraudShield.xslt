<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
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
  * FraudShield template
  *********************************************
  -->
  <xsl:template name="FraudShield">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Fraud Shield'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">

                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="4">
                        
                  <tr>
                    <td bgcolor="{$borderColor}" align="left" valign="middle" height="20">
                      <b><font color="#ffffff">Fraud Shield Summary</font></b>
                    </td>
                  </tr>  

                  <tr bgcolor="#ffffff">
                    <td valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <xsl:apply-templates select="prd:FraudServices/prd:Indicator" />
                    </font></td>
                  </tr>  
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

  </xsl:template>


  <!--
  *********************************************
  * FraudShield Summary template
  *********************************************
  -->
  <xsl:template match="prd:FraudServices/prd:Indicator">

    <!-- fraud service indicator  -->
    <xsl:variable name="fraudServiceIndcator">
      <xsl:choose>
        <xsl:when test="string(number(.)) != 'NaN' and number(.) > 0">
          <xsl:value-of select="number(.)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="$fraudServiceIndcator != ''">
	    <!-- Fraud Service description  -->
	    <xsl:variable name="fraudServiceDescription">
	    	<xsl:call-template name="translateFraudServicesIndicator">
	    		<xsl:with-param name="code" select="number($fraudServiceIndcator)" />
	    	</xsl:call-template>
	    </xsl:variable>
	
	    <xsl:if test="position() &gt; 1">
	      <br />
	    </xsl:if>
	
	    <xsl:value-of select="normalize-space($fraudServiceDescription)" /> 
    </xsl:if>
      
  </xsl:template>

</xsl:stylesheet>