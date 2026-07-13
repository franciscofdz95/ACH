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
  * LegalFilings template
  *********************************************
  -->
  <xsl:template name="LegalFilings">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Legal Filings'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <xsl:if test="count(prd:PublicRecord[(prd:Status/@code = 13) or (prd:Status/@code &gt;= 15 and prd:Status/@code &lt;= 17) or (prd:Status/@code &gt;= 22 and prd:Status/@code &lt;= 29)]) &gt; 0">
      <!-- Bankruptcies -->
      <xsl:call-template name="Bankruptcies" />

      <!-- Back to Top grapic -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

    <xsl:if test="count(prd:PublicRecord[(prd:Status/@code = 5) or (prd:Status/@code = 18) or (prd:Status/@code &gt;= 30 and prd:Status/@code &lt;= 38)]) &gt; 0">
      <!-- Tax Liens -->
      <xsl:call-template name="TaxLiens" />

      <!-- Back to Top grapic -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

    <xsl:if test="count(prd:PublicRecord[(prd:Status/@code = 14) or (prd:Status/@code &gt;= 0 and prd:Status/@code &lt;= 4) or (prd:Status/@code &gt;= 6 and prd:Status/@code &lt;= 11)]) &gt; 0">
      <!-- Judgments -->
      <xsl:call-template name="Judgments" />

      <!-- Back to Top grapic -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * Bankruptcies template
  *********************************************
  -->
  <xsl:template name="Bankruptcies">

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
                      <b><font color="#ffffff">Bankruptcies</font></b>
                    </td>
                  </tr>

                  <!-- Column header -->
                  <xsl:call-template name="LegalColumnHeader" />

                  <!-- Bankruptcies -->
                  <xsl:apply-templates select="prd:PublicRecord[(prd:Status/@code = 13) or (prd:Status/@code &gt;= 15 and prd:Status/@code &lt;= 17) or (prd:Status/@code &gt;= 22 and prd:Status/@code &lt;= 29)]" />

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
  * TaxLiens template
  *********************************************
  -->
  <xsl:template name="TaxLiens">

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
                      <b><font color="#ffffff">Tax Liens</font></b>
                    </td>
                  </tr>

                  <!-- Column header -->
                  <xsl:call-template name="LegalColumnHeader" />

                  <!-- Bankruptcies -->
                  <xsl:apply-templates select="prd:PublicRecord[(prd:Status/@code = 5) or (prd:Status/@code = 18) or (prd:Status/@code &gt;= 30 and prd:Status/@code &lt;= 38)]" />

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
  * Judgments template
  *********************************************
  -->
  <xsl:template name="Judgments">

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
                      <b><font color="#ffffff">Judgments</font></b>
                    </td>
                  </tr>

                  <!-- Column header -->
                  <xsl:call-template name="LegalColumnHeader" />

                  <!-- Bankruptcies -->
                  <xsl:apply-templates select="prd:PublicRecord[(prd:Status/@code = 14) or (prd:Status/@code &gt;= 0 and prd:Status/@code &lt;= 4) or (prd:Status/@code &gt;= 6 and prd:Status/@code &lt;= 11)]" />

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
  <xsl:template name="LegalColumnHeader">
    <!-- clumn header  -->
    <tr bgcolor="#ffffff">
      <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date Filed</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="15%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Reference No</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="20%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Court</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Amount</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="20%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Plaintiff</b></font>
      </td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="20%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Status</b></font>
      </td>
    </tr>

  </xsl:template>


  <!--
  *********************************************
  * PublicRecord template
  *********************************************
  -->
  <xsl:template match="prd:PublicRecord" xml:space="preserve">

    <xsl:variable name="filingDate">
      <xsl:choose>		              
        <xsl:when test="prd:FilingDate and (prd:FilingDate != 'null')">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:FilingDate" />
	            <xsl:with-param name="yearDigit" select="2" />
	     	     <xsl:with-param name="isYearLast" select="true()" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="referenceNo">
      <xsl:choose>		              
        <xsl:when test="prd:ReferenceNumber">		    		   		   
          <xsl:value-of select="prd:ReferenceNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="court">
      <xsl:choose>		              
        <xsl:when test="prd:Court">		    		   		   
          <xsl:value-of select="prd:Court/@name" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="amount">
      <xsl:choose>		              
        <xsl:when test="prd:Amount and (string(number(prd:Amount)) != 'NaN')">		    		   		   
          <xsl:value-of select="format-number(prd:Amount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="plaintiff">
      <xsl:choose>		              
        <xsl:when test="prd:PlaintiffName">		    		   		   
          <xsl:value-of select="prd:PlaintiffName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
    <xsl:variable name="status">
      <xsl:choose>		              
        <xsl:when test="prd:Status">		    		   		   
          <xsl:value-of select="prd:Status" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- This color alternation must be in one line -->
    <xsl:variable name="bgcolor"><xsl:choose><xsl:when test="position() mod 2 = 1"><xsl:value-of select="'#e5f5fa'" /></xsl:when><xsl:otherwise><xsl:value-of select="'#ffffff'" /></xsl:otherwise></xsl:choose></xsl:variable>

    <tr>
      <td height="20" bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$filingDate" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$referenceNo" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$court" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:value-of select="$amount" /></font></td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$plaintiff" /></font></td>

      <td width="1%" bgcolor="{$bgcolor}"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="{$bgcolor}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$status" /></font></td>

    </tr>
  
  </xsl:template>
  
</xsl:stylesheet>