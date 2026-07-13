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
  * HTML Model?
  *********************************************
  -->
  <xsl:template name="isHTMLModel">
    <xsl:variable name="mcModel">
       <xsl:choose>
         <xsl:when test="contains(prd:InputSummary/prd:Inquiry, 'MC-') ">
      		<xsl:value-of select="substring-before(substring-after(prd:InputSummary/prd:Inquiry, 'MC-'), '/') " />
      	  </xsl:when>
         <xsl:otherwise>
            <xsl:value-of select="''" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <!-- Is HTML Model?  -->
    <xsl:variable name="HTMLModel">
       <xsl:choose>
         <xsl:when test="prd:IntelliscoreScoreInformation/prd:ModelInformation/prd:ModelCode ">
	       <xsl:choose>
	         <xsl:when test="number(prd:IntelliscoreScoreInformation/prd:ModelInformation/prd:ModelCode) &lt; 210 ">
	            <xsl:value-of select="'false'" />
	         </xsl:when>
	         <xsl:otherwise>
	            <xsl:value-of select="'true'" />
	         </xsl:otherwise>
	       </xsl:choose>
         </xsl:when>

         <xsl:when test="$mcModel">
	       <xsl:choose>
	         <xsl:when test="number($mcModel) &lt; 210 ">
	            <xsl:value-of select="'false'" />
	         </xsl:when>
	         <xsl:otherwise>
	            <xsl:value-of select="'true'" />
	         </xsl:otherwise>
	       </xsl:choose>
         </xsl:when>

         <xsl:otherwise>
            <xsl:value-of select="'true'" />
         </xsl:otherwise>
       </xsl:choose>    
    </xsl:variable>
    <xsl:value-of select="$HTMLModel" />

  </xsl:template>


  <!--
  *********************************************
  * No HTML Report template
  *********************************************
  -->
  <xsl:template name="NoHTMLReport">
    <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
            
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="5" height="20" alt=""/></td>
      </tr>    

      <tr>
        <td>  
          <b>An HTML formatted report is unavailable for this model!</b>
        </td>
      </tr>
    </table>

  </xsl:template>


  <!--
  *********************************************
  * Business owner not found  template
  *********************************************
  -->
  <xsl:template name="businessOwnerNotFound">
    <xsl:param name="msg" />

    <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
            
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="5" height="20" alt=""/></td>
      </tr>    

      <tr>
        <td bgcolor="#e5f5fa" align="center">
          <b>Business owner not found</b>
        </td>
      </tr>

      <xsl:if test="normalize-space($msg) != '' ">
	      <tr>
	        <td>
	          <img src="../images/spacer.gif" border="0" width="5" height="20" alt=""/></td>
	      </tr>

	      <tr>
	        <td>  
	          <b>Business owner: </b><xsl:value-of select="$msg" />
	        </td>
	      </tr>
      </xsl:if>

    </table>

  </xsl:template>


  <!--
  *********************************************
  * Business not found  template
  *********************************************
  -->
  <xsl:template name="businessNotFound">
    <xsl:param name="msg" />

    <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
            
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="5" height="20" alt=""/></td>
      </tr>    

      <tr>
        <td bgcolor="#e5f5fa" align="center">  
          <font color="red"><b>No Matching Data</b></font>
        </td>
      </tr>

      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="5" height="20" alt=""/></td>
      </tr>

      <tr>
        <td>
          Entering complete business name, address, city, state and zip will improve matching. Please also confirm your spelling.<br /><br/>

          No information was found for the search data submitted.
        </td>
      </tr>

    </table>

  </xsl:template>

</xsl:stylesheet>