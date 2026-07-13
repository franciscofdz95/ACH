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
  * PaymentExperiences template
  *********************************************
  -->
  <xsl:template name="PaymentExperiences">
    <xsl:param name="title" />
    
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="$title" />
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
                    <td height="35" bgcolor="{$borderColor}" colspan="7" align="center" valign="middle"><b><font color="#ffffff">Trade Payment Experiences<br />(Trade Lines With an (*) after date are newly reported)</font></b></td>
                    <td height="35" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle"><b><font color="#ffffff">Account Status<br />Days Beyond Terms</font></b></td>
                    <td height="35" bgcolor="{$borderColor}"></td>
                  </tr>

                  <tr bgcolor="#ffffff">
                    <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Business<br />Category</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date<br />Reported</b></font></td>
                    <td align="center" width="7%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Last<br />Sale</b></font></td>

                    <td width="9%">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr bgcolor="#ffffff">
                          <td width="25%"><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
                          <td width="75%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Payment<br />Terms</b></font></td>
                        </tr>
                      </table>
                    </td>

                    <td align="center" width="2%"><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>

                    <td width="8%" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Recent<br />High<br />Credit</b></font></td>

                    <td width="10%">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr bgcolor="#ffffff">
                          <td width="20%"><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
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

                  <xsl:choose>
                    <xsl:when test="$title = 'Trade Payment Information'">
                      <!-- TradePaymentExperiences template -->
                      <xsl:apply-templates select="prd:TradePaymentExperiences[not(prd:ITIPConsolidatedNumber)]" />
                    </xsl:when>  
                    
                    <xsl:when test="$title = 'Government Financial Profile'">
                      <!-- GovernmentFinancialExperiences template -->
                      <xsl:apply-templates select="prd:GovernmentFinancialExperiences" />
                    </xsl:when>  

                    <xsl:otherwise>
                      <!-- AdditionalPaymentExperiences template -->
                      <xsl:apply-templates select="prd:AdditionalPaymentExperiences[not(prd:ITIPConsolidatedNumber)]" />
                    </xsl:otherwise>
                  </xsl:choose>
				
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
  * TradePaymentExperiences template
  *********************************************
  -->
  <xsl:template match="prd:TradePaymentExperiences | prd:AdditionalPaymentExperiences | prd:GovernmentFinancialExperiences" xml:space="preserve">
    <xsl:variable name="businessCategory">
      <xsl:value-of select="concat(prd:PaymentIndicator/@code, translate(prd:BusinessCategory, 'amp;amp;', 'amp;'))" />
    </xsl:variable>
    
    <xsl:variable name="dateLastSales">
      <xsl:choose>		              
        <xsl:when test="prd:DateLastActivity and number(prd:DateLastActivity ) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/year'" />
    		     <xsl:with-param name="value" select="prd:DateLastActivity" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="paymentTerms">
      <xsl:choose>		              
        <xsl:when test="prd:Terms and string(number(prd:Terms)) = 'NaN'">		    		   		   
          <xsl:value-of select="prd:Terms" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="recentHighCredit">
      <xsl:choose>		              
        <xsl:when test="prd:RecentHighCredit and number(prd:RecentHighCredit/prd:Amount) != 0">		    		   		   
          <xsl:value-of select="concat(prd:RecentHighCredit/prd:Modifier/@code, format-number(prd:RecentHighCredit/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="balance">
      <xsl:choose>		              
        <xsl:when test="prd:AccountBalance">		    		   		   
          <xsl:value-of select="concat(prd:AccountBalance/prd:Modifier/@code, format-number(prd:AccountBalance/prd:Amount, '$###,###,##0'))" />
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
        <xsl:when test="prd:Comments">		    		   		   
          <xsl:value-of select="prd:Comments" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
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

    <xsl:variable name="hiLiteColor">
      <xsl:choose>		              
        <xsl:when test="number($singleLineHighCredit) = number(prd:RecentHighCredit/prd:Amount)">
          <xsl:value-of select="'#fff000'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$bgColor" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$businessCategory" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/year'" />
		     <xsl:with-param name="value" select="prd:DateReported" />
		   </xsl:call-template>
        </font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$dateLastSales" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$paymentTerms" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>

      <td  bgcolor="{normalize-space($hiLiteColor)}" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:if test="number($singleLineHighCredit) = number(prd:RecentHighCredit/prd:Amount)"><a name="highestcredit"></a></xsl:if><xsl:value-of select="$recentHighCredit" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$balance" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$current" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT30" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT60" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT90" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$DBT90Plus" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$comments" /></font>
      </td>
    </tr>

  </xsl:template>

</xsl:stylesheet>