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
  * MonthlyPaymentTrends template
  *********************************************
  -->
  <xsl:template name="MonthlyPaymentTrends">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Monthly Payment Trends'" />
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
                    <td height="35" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle">
                      <b><font color="#ffffff">Payment Trends Analysis

                      <xsl:if test="prd:BusinessNameAndAddress/prd:SIC/@code != ''">
                        <br />
                        <xsl:value-of select="concat(translate(prd:BusinessNameAndAddress/prd:SIC, 'amp;amp;', 'amp;'), ' INDUSTRY SIC: ', prd:BusinessNameAndAddress/prd:SIC/@code)" />
                      </xsl:if> 

                      </font></b></td>

                    <td height="35" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle">
                      <b><font color="#ffffff">Account Status<br />
                      Days Beyond Terms</font></b></td>
                  </tr>

                  <!-- Column Headers -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="15%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date Reported</b></font></td>
                    <td align="center" width="18%" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Industry</b></font><br />

                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="50%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Cur</b></font></td>
                          <td width="50%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>DBT</b></font></td>
                        </tr>
                      </table>

                    </td>
                    <td align="center" width="9%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Business<br />DBT</b></font></td>
                    <td align="center" width="18%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Balance</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Cur</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>1-30</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>31-60</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>61-90</b></font></td>
                    <td align="center" width="8%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>91+</b></font></td>
                  </tr>
                  
                  <!-- IndustryPaymentTrends template -->

                  <xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:ContinouslyReportedTradeLines" mode="mpt" />
                  <xsl:apply-templates select="prd:IndustryPaymentTrends/prd:CurrentMonth" mode="mpt" />
                  <xsl:apply-templates select="prd:IndustryPaymentTrends/prd:PriorMonth" mode="mpt" />
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
  * CurrentMonth | PriorMonth template
  *********************************************
  -->
  <xsl:template match="prd:ContinouslyReportedTradeLines | prd:CurrentMonth | prd:PriorMonth" mode="mpt" xml:space="preserve">
    <xsl:variable name="position">
      <xsl:value-of select="position()" />
    </xsl:variable>

    <xsl:variable name="tmpMonth">
	<xsl:value-of select="number(substring(prd:Date, 5, 2)) - 1" />
    </xsl:variable>
	
    <xsl:variable name="tmpYear">
	<xsl:choose>		              
	  <xsl:when test="number($tmpMonth) = 0">		    		   		   
	    <xsl:value-of select="number(substring(prd:Date, 1, 4)) - 1" />
	  </xsl:when>
	
	  <xsl:otherwise>
	    <xsl:value-of select="number(substring(prd:Date, 1, 4))" />
	  </xsl:otherwise>
	</xsl:choose>    
    </xsl:variable>

    <xsl:variable name="newMonth">
	<xsl:choose>		              
	  <xsl:when test="number($tmpMonth) = 0">		    		   		   
	    <xsl:value-of select="12" />
	  </xsl:when>
	
	  <xsl:otherwise>
	    <xsl:value-of select="$tmpMonth" />
	  </xsl:otherwise>
	</xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateReported">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines'">		    		   		   
          <xsl:value-of select="'CURRENT'" />
        </xsl:when>

        <xsl:when test="prd:Date">
          <xsl:variable name="month">
            <xsl:call-template name="FormatMonth">
      		    <xsl:with-param name="monthValue" select="number($newMonth)" />
      		    <xsl:with-param name="upperCase" select="true()" />
      		  </xsl:call-template>
          </xsl:variable>		    		   		   

          <xsl:value-of select="concat(normalize-space($month), substring(normalize-space($tmpYear), 3, 2))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="industryCurrent">
      <xsl:choose>		              
        <xsl:when test="prd:CurrentPercentage and name() != 'ContinouslyReportedTradeLines'">
          <xsl:value-of select="format-number(prd:CurrentPercentage div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="industryDBT">
      <xsl:choose>		              
        <xsl:when test="prd:DBT and name() != 'ContinouslyReportedTradeLines'">		    		   		   
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="businessDBT">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:DBT">
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT">
          <xsl:value-of select="number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT)" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT">
          <xsl:value-of select="number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="businessCurrent">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:CurrentPercentage and number(prd:CurrentPercentage) != 0">
          <xsl:value-of select="format-number(prd:CurrentPercentage div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:CurrentPercentage and number(../../prd:PaymentTrends/prd:CurrentMonth/prd:CurrentPercentage) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:CurrentPercentage div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:CurrentPercentage and number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:CurrentPercentage) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:CurrentPercentage div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="balance">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:TotalAccountBalance">
          <xsl:value-of select="concat(prd:TotalAccountBalance/prd:Modifier/@code, format-number(prd:TotalAccountBalance/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:TotalAccountBalance">
          <xsl:value-of select="concat(../../prd:PaymentTrends/prd:CurrentMonth/prd:TotalAccountBalance/prd:Modifier/@code, format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:TotalAccountBalance/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:TotalAccountBalance">
          <xsl:value-of select="concat(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:TotalAccountBalance/prd:Modifier/@code, format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:TotalAccountBalance/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT30">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:DBT30 and number(prd:DBT30) != 0">
          <xsl:value-of select="format-number(prd:DBT30 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT30 and number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT30) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT30 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT30 and number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT30) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT30 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT60">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:DBT60 and number(prd:DBT60) != 0">
          <xsl:value-of select="format-number(prd:DBT60 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT60 and number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT60) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT60 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT60 and number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT60) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT60 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT90">
      <xsl:choose>
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:DBT90 and number(prd:DBT90) != 0">
          <xsl:value-of select="format-number(prd:DBT90 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90 and number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90 div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90 and number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90 div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="DBT90Plus">
      <xsl:choose>
        <xsl:when test="name() = 'ContinouslyReportedTradeLines' and prd:DBT90Plus and number(prd:DBT90Plus) != 0">
          <xsl:value-of select="format-number(prd:DBT90Plus div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth' and ../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90Plus and number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90Plus) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:CurrentMonth/prd:DBT90Plus div 100, '##0%')" />
        </xsl:when>

        <xsl:when test="name() = 'PriorMonth' and ../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90Plus and number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90Plus) != 0">
          <xsl:value-of select="format-number(../../prd:PaymentTrends/prd:PriorMonth[number($position)]/prd:DBT90Plus div 100, '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines'">
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:when test="name() = 'CurrentMonth'">
          <xsl:value-of select="'#ffffff'" />
        </xsl:when>

        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$dateReported" /></font>
      </td>

      <td height="20" width="9%" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$industryCurrent" /></font>
      </td>

      <td height="20" width="9%" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$industryDBT" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$businessDBT" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="70%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$balance" /></font>
            </td>
            <td width="30%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$businessCurrent" /></font>
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