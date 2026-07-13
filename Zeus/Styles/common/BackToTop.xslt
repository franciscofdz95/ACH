<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
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
  * Back To Top template
  *********************************************
  -->
  <xsl:template name="BackToTop">

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
	<tr bgcolor="#ffffff">
		<td align="right" valign="bottom" height="20"><a href="#top"><img src="../images/button_top.gif" border="0" width="38" height="16" alt="Back to top" /></a></td>
	</tr>
    </table>

  </xsl:template>

</xsl:stylesheet>