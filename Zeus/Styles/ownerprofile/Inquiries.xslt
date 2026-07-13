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
  * Inquiries template
  *********************************************
  -->
  <xsl:template name="InquiriesBOP">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Inquiries'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="11" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt="" />
                      <b><font color="#ffffff">Inquiry Details</font></b>
                    </td>
                  </tr>

                  <!-- Column header -->
                  <xsl:call-template name="InquiryColumnHeader" />

                  <!-- Bankruptcies -->
                  <xsl:apply-templates select="prd:Inquiry" />

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
  * ColumnHeader template
  *********************************************
  -->
  <xsl:template name="InquiryColumnHeader">
    <!-- clumn header  -->
    <tr bgcolor="#ffffff">
      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="left" width="32%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Name</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="left" width="50%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Account Type</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="18%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date of Inquiry</b></font>
      </td>

    </tr>

  </xsl:template>


  <!--
  *********************************************
  * Inquiry template
  *********************************************
  -->
  <xsl:template match="prd:Inquiry" xml:space="preserve">

    <xsl:variable name="name">
      <xsl:choose>		              
        <xsl:when test="prd:SubscriberDisplayName">		    		   		   
          <xsl:value-of select="prd:SubscriberDisplayName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="date">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/dt/year'" />
	     <xsl:with-param name="value" select="prd:Date" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="true()" />
	   </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="accountType">
      <xsl:choose>		              
        <xsl:when test="prd:Type">		    		   		   
          <xsl:value-of select="prd:Type" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- This color alternation must be in one line -->
    <xsl:variable name="bgcolor"><xsl:choose><xsl:when test="position() mod 2 = 1"><xsl:value-of select="'#e5f5fa'" /></xsl:when><xsl:otherwise><xsl:value-of select="'#ffffff'" /></xsl:otherwise></xsl:choose></xsl:variable>

    <tr>
      <td height="20" width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$name" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$accountType" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
       	 <xsl:value-of select="$date" />          
        </font></td>
    </tr>

  </xsl:template>

</xsl:stylesheet>