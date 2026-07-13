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
  * QuarterlyPaymentHistory template
  *********************************************
  -->
  <xsl:template name="QuarterlyPaymentHistory">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Quarterly Payment Trends'" />
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
                    <td height="35" bgcolor="{$borderColor}" colspan="4" align="center" valign="middle">
                      <b><font color="#ffffff">Payment History - Quarterly Averages</font></b></td>

                    <td height="35" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle">
                      <b><font color="#ffffff">Account Status<br />
                      Days Beyond Terms</font></b></td>
                  </tr>

                  <!-- Column Headers -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="15%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Quarter</b></font></td>
                    <td align="center" width="15%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Months</b></font></td>
                    <td align="center" width="10%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>DBT</b></font></td>
                    <td align="center" width="20%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Balance</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Cur</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>1-30</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>31-60</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>61-90</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>91+</b></font></td>
                  </tr>
                  
                  <!-- IndustryPaymentTrends template -->
                  <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:MostRecentQuarter" />
                  <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:PriorQuarter" />                  
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
  * MostRecentQuarter | PriorQuarter template
  *********************************************
  -->
  <xsl:template match="prd:MostRecentQuarter | prd:PriorQuarter" xml:space="preserve">
    
    <xsl:variable name="quarter">
      <xsl:value-of select="concat('Q', prd:QuarterWithinYear/@code, ' - ', substring(prd:YearOfQuarter, 3, 2))" />
    </xsl:variable>

    <xsl:variable name="months">
      <xsl:choose>		              
        <xsl:when test="prd:QuarterWithinYear/@code = 1">		    		   		   
          <xsl:value-of select="'JAN - MAR'" />
        </xsl:when>

        <xsl:when test="prd:QuarterWithinYear/@code = 2">		    		   		   
          <xsl:value-of select="'APR - JUN'" />
        </xsl:when>

        <xsl:when test="prd:QuarterWithinYear/@code = 3">		    		   		   
          <xsl:value-of select="'JUL - SEP'" />
        </xsl:when>

        <xsl:when test="prd:QuarterWithinYear/@code = 4">		    		   		   
          <xsl:value-of select="'OCT - DEC'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="DBT">
      <xsl:choose>		              
        <xsl:when test="prd:DBT">		    		   		   
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="balance">
      <xsl:choose>		              
        <xsl:when test="prd:TotalAccountBalance">		    		   		   
          <xsl:value-of select="concat(prd:TotalAccountBalance/prd:Modifier/@code, format-number(prd:TotalAccountBalance/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="current">
      <xsl:choose>		              
        <xsl:when test="prd:CurrentPercentage and number(prd:CurrentPercentage) != 0">		    		   		   
          <xsl:value-of select="format-number(prd:CurrentPercentage div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT30">
      <xsl:choose>		              
        <xsl:when test="prd:DBT30 and number(prd:DBT30) != 0">		    		   		   
          <xsl:value-of select="format-number(prd:DBT30 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT60">
      <xsl:choose>		              
        <xsl:when test="prd:DBT60 and number(prd:DBT60) != 0">		    		   		   
          <xsl:value-of select="format-number(prd:DBT60 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT90">
      <xsl:choose>		              
        <xsl:when test="prd:DBT90 and number(prd:DBT90) != 0">		    		   		   
          <xsl:value-of select="format-number(prd:DBT90 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT90Plus">
      <xsl:choose>		              
        <xsl:when test="prd:DBT90Plus and number(prd:DBT90Plus) != 0">		    		   		   
          <xsl:value-of select="format-number(prd:DBT90Plus div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="name() = 'MostRecentQuarter' or position() mod 2 = 0">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$quarter" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$months" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$DBT" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="67%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$balance" /></font>
            </td>
            <td width="33%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$current" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT30" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT60" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT90" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT90Plus" /></font>
      </td>
    </tr>

  </xsl:template>

</xsl:stylesheet>