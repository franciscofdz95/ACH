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
  * report footer template
  *********************************************
  -->
  <xsl:template name="ReportFooter">
    <xsl:param name="reportType" select="''" />
    <xsl:param name="reportDate" select="''" />
    <xsl:param name="skipBackToTop" select="'0'" />
    <xsl:param name="reportEnd" select="'End of report'" />
    <xsl:param name="reportCount" select="'1 of 1 report'" />

    <!-- get year from date extension -->
    <xsl:variable name="reportYear">
       <xsl:value-of select="substring(normalize-space($reportDate), 1, 4)"/>
    </xsl:variable>

   <xsl:if test="(not($product = 'BPR') and not($product = 'PPR') and not($product = 'CFI')) and $skipBackToTop = 0">
	 <!-- back to top graphic -->
	 <xsl:call-template name="BackToTop" />
   </xsl:if>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">

      <tr>
        <td colspan="2" align="center"><i><b>
        Experian prides itself on the depth and accuracy of the data maintained on our databases. Reporting your customer's payment behavior to Experian will further strengthen and enhance the power of the information available for making sound credit decisions. Give credit where credit is due. Call 1-800-520-1221, option #4 for more information.
        </b></i></td>
      </tr>

      <tr>
        <td align="left">
          <img height="12" width="1" border="0" src="../images/spacer.gif" alt=""/><br/>
          <xsl:value-of select="$reportEnd" />
        </td>
        <td align="right">
          <img height="12" width="1" border="0" src="../images/spacer.gif" alt=""/><br/>
          <i><xsl:value-of select="$reportCount" /></i>
        </td>
      </tr>

      <tr>
        <td colspan="2">
          <br /></td>
      </tr>
          
      <tr>
        <td colspan="2"><font size="1"><i>
          The information herein is furnished in confidence for your exclusive use for 
          legitimate business purposes and shall not be reproduced. Neither Experian 
          Information Solutions, Inc., nor their sources or distributors warrant such 
          information nor shall they be liable for your use or reliance upon it. 
          </i></font>
        </td>
      </tr>

      <tr>
        <td colspan="2">
          <img height="5" width="1" border="0" src="../images/spacer.gif" alt=""/></td>
      </tr>

      <tr>
        <td colspan="2" height="1" bgcolor="#0099cc">
          <img height="1" width="1" border="0" src="../images/spacer.gif" alt=""/></td>
      </tr>

      <xsl:choose>
        <xsl:when test="not($product='BPR') and not($product='CI') and not($product='CIBPR') and not($product = 'CFI') ">
	      <tr>
	        <td colspan="2">
		      <!-- Back to Top grapic -->
		      <xsl:call-template name="BackToTop" />
	        </td>
	      </tr>
        </xsl:when>

        <xsl:otherwise>
	      <tr>
	        <td colspan="2">
	          <img height="20" width="1" border="0" src="../images/spacer.gif" alt=""/></td>
	      </tr>
        </xsl:otherwise>
      </xsl:choose>

      <tr>
        <td colspan="2"><font size="1">
          <xsl:text disable-output-escaping="yes">&amp;copy;</xsl:text> 
          Experian 
          <xsl:value-of select="$reportYear" />. All rights reserved. <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
          <a href="javascript:void(0);" onClick="window.open('http://www.experian.com/privacy/index.html','privacy', 'width=0,height=0,left=20,top=20,status=no,toolbar=no,scrollbars=yes,menubar=no,location=no,'); return false;">Privacy policy</a>.
          <br />
          
          Experian and the Experian marks herein are service marks or registered trademarks of Experian.

<!--
          <xsl:if test="$reportType='SBI'">
            <br />
            
            ScorexPLUS<sup>SM</sup> Score is the service mark of Experian-Scorex, LLC.
          </xsl:if>
-->
          </font>
        </td>
      </tr>

    </table>

  </xsl:template>

</xsl:stylesheet>