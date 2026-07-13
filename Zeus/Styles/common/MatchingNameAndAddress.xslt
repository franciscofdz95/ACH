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
  * MatchingNameAndAddress template
  *********************************************
  -->
  <xsl:template match="prd:MatchingNameAndAddress" xml:space="preserve">

    <xsl:variable name="name">
      <xsl:choose>                  
        <xsl:when test="prd:MatchingBusinessName">                    
          <xsl:value-of select="prd:MatchingBusinessName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="street">
      <xsl:choose>                  
        <xsl:when test="prd:MatchingStreetAddress">                     
          <xsl:value-of select="prd:MatchingStreetAddress" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="city">
      <xsl:choose>                  
        <xsl:when test="prd:MatchingCity">                     
          <xsl:value-of select="prd:MatchingCity" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="state">
      <xsl:choose>                  
        <xsl:when test="prd:MatchingState">                     
          <xsl:value-of select="prd:MatchingState" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="zip">
      <xsl:choose>                  
        <xsl:when test="prd:MatchingZip">                     
          <xsl:call-template name="FormatZip">
            <xsl:with-param name="value" select="concat(prd:MatchingZip, prd:MatchingZipExtension)" />
          </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Matching Name and Address'" />
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

                <table width="100%" border="0" cellspacing="0" cellpadding="4">
                  
                  <tr>
                    <td width="100%" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <i>This business is linked to the one displayed above. The report 
                         shows the best view of the business.</i></font></td>
                  </tr>

                  <tr>
                    <td width="100%" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <b><xsl:value-of select="$name" /></b>

                      <xsl:if test="normalize-space($street) != ''">
                        <br />
                        <xsl:value-of select="$street" />
                      </xsl:if>
        
                      <xsl:if test="normalize-space($city) != ''">
                        <br />
                        <xsl:value-of select="concat(normalize-space($city), ', ', normalize-space($state), ' ', normalize-space($zip))" />
                      </xsl:if>
                      </font>
                    </td>

                  </tr>

                </table>
              </td>
            </tr>
          </table>
          <!-- end inner white box -->
        </td>
      </tr>  
    </table>

  </xsl:template>


</xsl:stylesheet>
