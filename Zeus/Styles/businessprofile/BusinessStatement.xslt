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
  * BusinessStatement template
  *********************************************
  -->
  <xsl:template name="BusinessStatement">
  
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Business Statement'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <xsl:apply-templates select="prd:ConsumerStatement" />
        </td>
      </tr>  
    </table>
    
  </xsl:template>


  <!--
  *********************************************
  * ConsumerStatement template
  *********************************************
  -->
  <xsl:template match="prd:ConsumerStatement" xml:space="preserve">
    <a NAME="#BusinessStatement"></a>

    <xsl:if test="position() &gt; 1">
      <br /><img src="../images/spacer.gif" border="0" width="1" height="10" alt=""/><br />         
    </xsl:if>
    
    <xsl:value-of select="prd:Text" />
  </xsl:template>

</xsl:stylesheet>