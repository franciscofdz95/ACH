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
  * TradePaymentTotals template
  *********************************************
  -->
  <xsl:template name="TradePaymentTotals">

    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Trade Payment Totals'" />
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

                    <td height="35" bgcolor="{$borderColor}" colspan="6" align="center" valign="middle">
                      <b><font color="#ffffff">Trade Payment Experiences</font></b></td>
                    <td height="35" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle">
                      <b><font color="#ffffff">Account Status<br />
                      Days Beyond Terms</font></b></td>
                    <td height="35" bgcolor="{$borderColor}"></td>
                  </tr>

                  <tr bgcolor="#ffffff">
                    <td align="center" width="18%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Type</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Lines<br />Reported</b></font></td>
                    <td align="center" width="3%"><font size="1" style="font-family: 'verdana';"><b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
                    <td align="center" width="3%"><font size="1" style="font-family: 'verdana';"><b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>

                    <td width="12%">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr bgcolor="#ffffff">
                          <td width="20%"></td>
                          <td width="80%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Recent<br />High<br />Credit</b></font></td>
                        </tr>
                      </table>
                    </td>

                    <td width="12%">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr bgcolor="#ffffff">
                          <td width="20%"></td>
                          <td width="80%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Balance</b></font></td>
                        </tr>
                      </table>
                    </td>

                    <td align="center" width="6%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Cur</b></font></td>
                    <td align="center" width="6%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>1-30</b></font></td>
                    <td align="center" width="6%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>31-60</b></font></td>
                    <td align="center" width="6%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>61-90</b></font></td>
                    <td align="center" width="6%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>91+</b></font></td>
                    <td align="left" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Comments</b></font></td>
                  </tr>

				<!-- ContinouslyReportedTradeLines template -->
				<xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:ContinouslyReportedTradeLines" mode="tpt">
	  				<xsl:with-param name="type" select="'Continuously Reported'" />
				</xsl:apply-templates>
	              
				<!-- NewlyReportedTradeLines template -->
				<xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:NewlyReportedTradeLines" mode="tpt">
	  				<xsl:with-param name="type" select="'Newly Reported'" />
				</xsl:apply-templates>

                  <tr>
                    <td bgcolor="{$borderColor}" colspan="12">
                      <img src="../images/spacer.gif" width="0" height="1" alt=""/></td>
                  </tr>

				<!-- CombinedTradeLines template -->
				<xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:CombinedTradeLines" mode="tpt">
	  				<xsl:with-param name="type" select="'Trade Lines Totals'" />
				</xsl:apply-templates>
                  
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
  * Trade Lines template
  *********************************************
  -->
  <xsl:template match="prd:NewlyReportedTradeLines | prd:ContinouslyReportedTradeLines | prd:CombinedTradeLines" mode="tpt" xml:space="preserve">
    <xsl:param name="type" />
    
    <xsl:variable name="linesReported">
      <xsl:choose>		              
        <xsl:when test="prd:NumberOfLines">		    		   		   
          <xsl:value-of select="number(prd:NumberOfLines)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="recentHighCredit">
      <xsl:choose>
        <xsl:when test="$linesReported = 0">		    		   		   
          <xsl:value-of select="''" />
        </xsl:when>

        <xsl:when test="prd:TotalHighCreditAmount and number(prd:TotalHighCreditAmount/prd:Amount) != 0 and $linesReported != 0">		    		   		   
          <xsl:value-of select="concat(prd:TotalHighCreditAmount/prd:Modifier/@code, format-number(prd:TotalHighCreditAmount/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="balance">
      <xsl:choose>		              
        <xsl:when test="$linesReported = 0">		    		   		   
          <xsl:value-of select="''" />
        </xsl:when>

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
    
    <xsl:variable name="comments">
      <xsl:choose>		              
        <xsl:when test="$linesReported = 0">		    		   		   
          <xsl:value-of select="''" />
        </xsl:when>

        <xsl:when test="prd:DBT and number(prd:DBT) != 0">		    		   		   
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="$type = 'Newly Reported'">		    		   		   
          <xsl:value-of select="'#ffffff'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$type" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$linesReported" /></font>
      </td>
      <td  bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="font-family: 'verdana';"><b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
      <td  bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="font-family: 'verdana';"><b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="right">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="88%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$recentHighCredit" /></font>
            </td>
            <td width="12%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="right">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="88%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$balance" /></font>
            </td>
            <td width="12%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$current" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$DBT30" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$DBT60" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$DBT90" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$DBT90Plus" /></font>
      </td>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">DBT: <xsl:value-of select="$comments" /></font>
      </td>
    </tr>

  </xsl:template>
  
</xsl:stylesheet>