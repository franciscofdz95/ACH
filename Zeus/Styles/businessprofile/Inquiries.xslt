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
  * Inquiries template
  *********************************************
  -->
  <xsl:template name="Inquiries">
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
                    <td bgcolor="{$borderColor}" colspan="10" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <b><font color="#ffffff">Summary of Inquiries</font></b></td>
                  </tr>

                  <!-- Column Headers -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="19%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Business<br />Category</b></font></td>
                    
                    <!-- make year month header -->
                    <xsl:apply-templates select="prd:Inquiry[last()]/prd:InquiryCount" mode="header" />
                  </tr>
                  
                  <!-- row of inquiry count -->
                  <xsl:apply-templates select="prd:Inquiry" mode="BPR" />
                  
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
  * Inquiry template
  *********************************************
  -->
  <xsl:template match="prd:Inquiry" mode="BPR" xml:space="preserve">

    <xsl:variable name="bold">
      <xsl:choose>		              
        <xsl:when test="position() = last()">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="category">
      <xsl:choose>		              
        <xsl:when test="boolean(number($bold))">		    		   		   
          <xsl:value-of select="'Totals'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="normalize-space(prd:InquiryBusinessCategory)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="boolean(number($bold))">    
      <tr>
        <td bgcolor="{$borderColor}" colspan="10">
          <img src="../images/spacer.gif" width="0" height="1" alt=""/></td>
      </tr>
    </xsl:if>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="100%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:if test="boolean(number($bold))">
                <xsl:text disable-output-escaping="yes">&lt;b&gt;</xsl:text>
              </xsl:if>    
            
              <xsl:value-of select="$category" />
              
              <xsl:if test="boolean(number($bold))">
                <xsl:text disable-output-escaping="yes">&lt;/b&gt;</xsl:text>
              </xsl:if>
              </font>
            </td>
          </tr>
        </table>
      </td>

      <xsl:apply-templates select="prd:InquiryCount" mode="count">
        <xsl:with-param name="bold" select="boolean(number($bold))" />
        <xsl:with-param name="bgColor" select="$bgColor" />
      </xsl:apply-templates>
      
    </tr>
  </xsl:template>


  <!--
  *********************************************
  * InquiryCount template
  *********************************************
  -->
  <xsl:template match="prd:InquiryCount" mode="header" xml:space="preserve">
    <xsl:variable name="date">
      <xsl:variable name="month">
  		   <xsl:call-template name="FormatMonth">
  		     <xsl:with-param name="monthValue" select="number(substring(prd:Date, 5, 2))" />
  		     <xsl:with-param name="upperCase" select="true()" />
  		   </xsl:call-template>
      </xsl:variable>		    		   		   

      <xsl:value-of select="concat(normalize-space($month), normalize-space(substring(prd:Date, 3, 2)))" />
    </xsl:variable>

    <td align="center" width="9%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$date" /></b></font></td>
  </xsl:template>


  <!--
  *********************************************
  * InquiryCount template
  *********************************************
  -->
  <xsl:template match="prd:InquiryCount" mode="count" xml:space="preserve">
    <xsl:param name="bold" select="false()" />
    <xsl:param name="bgColor" select="'#ffffff'" />

    <xsl:variable name="count">
      <xsl:choose>
        <xsl:when test="number(prd:Count) &gt; 0">
          <xsl:value-of select="number(prd:Count)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>      		              
    </xsl:variable>
    
    <td height="20" align="center" bgcolor="{normalize-space($bgColor)}" width="9%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';">
      <xsl:if test="$bold">
        <xsl:text disable-output-escaping="yes">&lt;b&gt;</xsl:text>
      </xsl:if>    

      <xsl:value-of select="$count" />
    
      <xsl:if test="$bold">
        <xsl:text disable-output-escaping="yes">&lt;/b&gt;</xsl:text>
      </xsl:if>
      </font>
    </td>
  </xsl:template>
  
</xsl:stylesheet>