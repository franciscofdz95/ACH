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
  * ConsumerStatement template
  *********************************************
  -->
  <xsl:template name="ConsumerStatement">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Consumer Statement'" />
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
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td bgcolor="{$borderColor}" align="left" valign="middle" height="20">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt="" />
                      <b><font color="#ffffff">Disputes and Supplemental Data Related to Transactional Information</font></b>
                    </td>
                  </tr>  
                </table>

                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="4">
                  <tr bgcolor="#ffffff">
                    <td valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <xsl:value-of select="prd:Statement/prd:StatementText/prd:MessageText" /></font></td>
                  </tr>  
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
