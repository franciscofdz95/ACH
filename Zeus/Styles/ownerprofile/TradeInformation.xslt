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
  * TradeInformation template
  *********************************************
  -->
  <xsl:template name="TradeInformation">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Trade Information'" />
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
                    <td bgcolor="{$borderColor}" colspan="19" align="left" valign="middle" height="20">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt="" />
                      <b><font color="#ffffff"><xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;</xsl:text>Trade Payment Experience</font></b>
                    </td>
                  </tr>

                  <!-- tradelines -->
                  <xsl:apply-templates select="prd:TradeLine" />
  
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

    <!-- Legends -->
    <xsl:call-template name="Legends" />

  </xsl:template>


  <!--
  *********************************************
  * TradeColumnHeader template
  *********************************************
  -->
  <xsl:template name="TradeColumnHeader">
    <!-- clumn header  -->
    <tr bgcolor="#ffffff">
      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="left" width="22%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Name</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="9%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Revolving / <br/>Installment</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="7%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date<br/>Opened</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="7%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Open /<br />Closed</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="9%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Balance /<br />As of</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="9%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Historical<br />High<br />Balance</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="9%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Monthly<br />Payment</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="7%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date of<br />Last<br />Payment</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td align="center" width="11%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Amount<br />Past Due</b></font></td>

      <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

    </tr>

  </xsl:template>


  <!--
  *********************************************
  * TradeLine template
  *********************************************
  -->
  <xsl:template match="prd:TradeLine" xml:space="preserve">

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
  
    <xsl:variable name="revInstall">
      <xsl:choose>		              
        <xsl:when test="prd:RevolvingOrInstallment">		    		   		   
          <xsl:value-of select="prd:RevolvingOrInstallment" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateOpened">
      <xsl:choose>		              
        <xsl:when test="prd:OpenDate and string-length(normalize-space(prd:OpenDate)) &lt; 4 and substring(prd:OpenDate, string-length(prd:OpenDate)) = '1' ">
          <xsl:value-of select="'Greater than 5 years'" />
        </xsl:when>

        <xsl:when test="prd:OpenDate and string-length(normalize-space(prd:OpenDate)) &lt; 4 and substring(prd:OpenDate, string-length(prd:OpenDate)) = '2' ">
          <xsl:value-of select="'Greater than 10 years'" />
        </xsl:when>

        <xsl:otherwise>
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/year'" />
	     <xsl:with-param name="value" select="normalize-space(concat(substring(prd:OpenDate, 3, 2), substring(prd:OpenDate, 1, 2), '00'))" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="false()" />
	   </xsl:call-template>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="openClosed">
      <xsl:choose>		              
        <xsl:when test="prd:OpenOrClosed">		    		   		   
          <xsl:value-of select="prd:OpenOrClosed" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="balance">
      <xsl:choose>		              
        <xsl:when test="prd:BalanceAmount and (string(number(prd:BalanceAmount)) != 'NaN') and number(prd:BalanceAmount) > 0 ">		    		   		   
          <xsl:value-of select="format-number(prd:BalanceAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="balanceDate">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/dt/year'" />
	     <xsl:with-param name="value" select="prd:BalanceDate" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="true()" />
	   </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="balanceYear">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'year'" />
	     <xsl:with-param name="value" select="prd:BalanceDate" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="true()" />
	   </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="balanceMonth">
      <xsl:value-of select="substring(prd:BalanceDate, 1, 2)" />
    </xsl:variable>

    <xsl:variable name="highBalance">
      <xsl:choose>		              
        <xsl:when test="prd:Amount and prd:Amount/prd:Qualifier/@code = 'H' and number(prd:Amount[prd:Qualifier/@code = 'H']/prd:Value) > 0 ">		    		   		   
          <xsl:value-of select="format-number(prd:Amount[prd:Qualifier/@code = 'H']/prd:Value, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalLoanAmt">
      <xsl:choose>		              
        <xsl:when test="prd:Amount and prd:Amount/prd:Qualifier/@code = 'O' and number(prd:Amount[prd:Qualifier/@code = 'O']/prd:Value) > 0 ">		    		   		   
          <xsl:value-of select="format-number(prd:Amount[prd:Qualifier/@code = 'O']/prd:Value, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="creditLimit">
      <xsl:choose>		              
        <xsl:when test="prd:Amount and prd:Amount/prd:Qualifier/@code = 'L' and number(prd:Amount[prd:Qualifier/@code = 'L']/prd:Value) > 0 ">		    		   		   
          <xsl:value-of select="format-number(prd:Amount[prd:Qualifier/@code = 'L']/prd:Value, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="monthlyPayment">
      <xsl:choose>		              
        <xsl:when test="prd:MonthlyPaymentAmount and (string(number(prd:MonthlyPaymentAmount)) != 'NaN')">		    		   		   
          <xsl:value-of select="format-number(prd:MonthlyPaymentAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="lastPaymentDate">
      <xsl:choose>		              
        <xsl:when test="prd:LastPaymentDate">		    		   		   
      	   <xsl:call-template name="FormatDate">
      	     <xsl:with-param name="pattern" select="'mo/dt/year'" />
      	     <xsl:with-param name="value" select="prd:LastPaymentDate" />
      	     <xsl:with-param name="yearDigit" select="2" />
      	     <xsl:with-param name="isYearLast" select="true()" />
      	   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="amountPastDue">
      <xsl:choose>		              
        <xsl:when test="prd:AmountPastDue and (string(number(prd:AmountPastDue)) != 'NaN')">		    		   		   
          <xsl:value-of select="format-number(prd:AmountPastDue, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>


    <!-- Column header -->
    <xsl:call-template name="TradeColumnHeader" />

    <!-- This color alternation must be in one line -->
    <xsl:variable name="bgcolor"><xsl:choose><xsl:when test="position() mod 2 = 1"><xsl:value-of select="'#e5f5fa'" /></xsl:when><xsl:otherwise><xsl:value-of select="'#ffffff'" /></xsl:otherwise></xsl:choose></xsl:variable>

    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="2" alt="" /></td>
    </tr>

    <tr>
      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$name" /></font></td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$revInstall" /></font></td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$dateOpened" /></font></td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$openClosed" /></font></td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:value-of select="$balance" /><br />
              <xsl:value-of select="$balanceDate" />
              </font></td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:value-of select="$highBalance" />
              </font></td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:value-of select="$monthlyPayment" />
              </font></td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$lastPaymentDate" /></font></td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td height="20" bgcolor="#e5f5fa" align="center">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:value-of select="$amountPastDue" />
              </font></td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

    </tr>

    <xsl:if test="normalize-space($originalLoanAmt) != '' ">
	    <tr>
	      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>
	
	      <td bgcolor="#e5f5fa" colspan="18">
	        <font size="1" style="FONT-FAMILY: 'verdana';"><b>Original Loan Amount:</b>
	        <xsl:value-of select="$originalLoanAmt" /></font></td>
	    </tr>
	
	    <tr>
	      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
	    </tr>
    </xsl:if>

    <xsl:if test="normalize-space($creditLimit) != '' ">
	    <tr>
	      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>
	
	      <td bgcolor="#e5f5fa" colspan="18">
	        <font size="1" style="FONT-FAMILY: 'verdana';"><b>Credit Limit:</b>
	        <xsl:value-of select="$creditLimit" /></font></td>
	    </tr>
	
	    <tr>
	      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
	    </tr>
    </xsl:if>

    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="#e5f5fa" colspan="18">
        <font size="1" style="FONT-FAMILY: 'verdana';"><b>Transactional Relationship:</b>
        <xsl:value-of select="prd:ECOA" /></font></td>
    </tr>

    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
    </tr>

    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="#e5f5fa" colspan="18">
        <font size="1" style="FONT-FAMILY: 'verdana';"><b>Trade Category:</b>
        <xsl:value-of select="prd:KOB" /></font></td>
    </tr>
    
    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
    </tr>
    
    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td bgcolor="#e5f5fa" colspan="18">
        <table border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td nowrap="nowrap" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Payment Status: 
              <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
            <td>
              <font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="prd:Status" /></font></td>
          </tr>
        </table>
      </td>
    </tr>

    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
    </tr>
    
    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td colspan="18" bgcolor="#e5f5fa">
        <table border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td nowrap="nowrap" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Special Comments: 
              <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
            <td>
              <font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="prd:SpecialComment" /></font></td>
          </tr>
        </table>
      </td>
    </tr>

    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
    </tr>
    
    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td colspan="18" bgcolor="#e5f5fa">
        <table border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td nowrap="nowrap" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><b>
              Payment Indicator (current month <xsl:value-of select="concat(normalize-space($balanceMonth), '/', normalize-space($balanceYear))" />): 
              <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
            <td>
              <font size="1" style="FONT-FAMILY: 'verdana';">
              <xsl:call-template name="PaymentIndicatorTraslation">
                <xsl:with-param name="code" select="substring(normalize-space(prd:PaymentProfile), 1, 1)" />
              </xsl:call-template>
              </font></td>
          </tr>
        </table>
      </td>
    </tr>

    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
    </tr>
    
    <tr>
      <td width="1%" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

      <td colspan="17" bgcolor="#e5f5fa" align="center">
        <table width="100%" border="0" cellpadding="0" cellspacing="0">
          <tr>
            <td width="61%" nowrap="nowrap" valign="top" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><b>
              24 Month Payment History</b></font></td>
            <td width="39%" nowrap="nowrap" valign="top" align="right"><font size="1" style="FONT-FAMILY: 'verdana';">
              <a style="text-decoration: none" href="#legend">Click here for legend</a></font></td>
          </tr>

          <tr>
            <td colspan="2">
              <table width="100%" bgcolor="{$borderColor}" border="0" cellpadding="1" cellspacing="0">
                <tr>
                  <td bgcolor="{$borderColor}">
                    <table width="100%" border="0" cellpadding="0" cellspacing="1">
                      <tr>
                        <xsl:call-template name="PaymentHistoryLoop">
                          <xsl:with-param name="type" select="'month'" />
                          <xsl:with-param name="startMonth" select="$balanceMonth" />
                        </xsl:call-template>
                      </tr>
                      <tr>
                        <xsl:call-template name="PaymentHistoryLoop">
                          <xsl:with-param name="type" select="'year'" />
                          <xsl:with-param name="startMonth" select="$balanceMonth" />
                          <xsl:with-param name="startYear" select="$balanceYear" />
                        </xsl:call-template>
                      </tr>
                      <tr>
                        <xsl:call-template name="PaymentHistoryLoop">
                          <xsl:with-param name="type" select="'char'" />
                          <xsl:with-param name="length" select="string-length(normalize-space(prd:PaymentProfile))" />
                          <xsl:with-param name="indicators" select="normalize-space(prd:PaymentProfile)" />
                        </xsl:call-template>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            
            </td>
          </tr>
        </table>
      </td>
    </tr>
        
    <tr>
      <td colspan="19" bgcolor="#e5f5fa"><img src="../images/spacer.gif" border="0" width="1" height="10" alt="" /></td>
    </tr>
  
  </xsl:template>


  <!--
  *********************************************
  * PaymentIndicatorTraslation template
  *********************************************
  -->
  <xsl:template name="PaymentIndicatorTraslation">
    <xsl:param name="code" />

    <xsl:choose>
      <xsl:when test="$code = '1'">
        <xsl:value-of select="'30 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '2'">
        <xsl:value-of select="'60 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '3'">
        <xsl:value-of select="'90 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '4'">
        <xsl:value-of select="'120 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '5'">
        <xsl:value-of select="'150 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '6'">
        <xsl:value-of select="'180 days past the due date'" />
      </xsl:when>

      <xsl:when test="$code = '7'">
        <xsl:value-of select="'Chapter 13 Bankruptcy (Petitioned, Discharged, Reaffirmation of debt rescinded)'" />
      </xsl:when>

      <xsl:when test="$code = '8'">
        <xsl:value-of select="'Foreclosure, voluntary surrender or repossession'" />
      </xsl:when>

      <xsl:when test="$code = '9'">
        <xsl:value-of select="'Collections, charge-off or bankruptcy'" />
      </xsl:when>

      <xsl:when test="$code = 'B'">
        <xsl:value-of select="'Account condition change, payment code not applicable'" />
      </xsl:when>

      <xsl:when test="$code = 'C'">
        <xsl:value-of select="'Current'" />
      </xsl:when>

      <xsl:when test="$code = '0'">
        <xsl:value-of select="'Current with zero balance - update received'" />
      </xsl:when>

      <xsl:when test="$code = 'N'">
        <xsl:value-of select="'Current account/Zero balance - no update received'" />
      </xsl:when>

      <xsl:when test="$code = '-'">
        <xsl:value-of select="'No history reported for that month'" />
      </xsl:when>

      <xsl:when test="$code = 'Blank'">
        <xsl:value-of select="'No history maintained; see payment status comment'" />
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="''" />
      </xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * 24 month Payment History template
  *********************************************
  -->
  <xsl:template name="PaymentHistoryLoop">
    <xsl:param name="type" select="''" />
    <xsl:param name="startMonth" select="0" />
    <xsl:param name="startYear" select="0" />
    <xsl:param name="index" select="1" />
    <xsl:param name="length" select="0" />
    <xsl:param name="indicators" select="''" />
    
    <xsl:variable name="total" select="24" />
        
    <xsl:variable name="nextMonth">
      <xsl:choose>		              
        <xsl:when test="($startMonth - 1) &lt;= 0">		    		   		   
          <xsl:value-of select="$startMonth - 1 + 12" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$startMonth - 1" />
        </xsl:otherwise>
      </xsl:choose>    
      
    </xsl:variable>    
    
    <xsl:variable name="nextYear">
      <xsl:choose>		              
        <xsl:when test="($startMonth - 1) &lt;= 0">		    		   		   
          <xsl:value-of select="$startYear - 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="normalize-space($startYear)" />
        </xsl:otherwise>
      </xsl:choose>    
          
    </xsl:variable>

    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="$type = 'month'">		    		   		   
          <xsl:call-template name="FormatMonth">
            <xsl:with-param name="monthValue" select="$nextMonth" />
          </xsl:call-template>
        </xsl:when>

        <xsl:when test="$type = 'year'">		    		   		   
          <xsl:value-of select="substring($nextYear, 3, 2)" />
        </xsl:when>

        <xsl:when test="$type = 'char'">		    		   		   
          <xsl:choose>		              
            <xsl:when test="$index &lt;= ($length - 1)">		    		   		   
              <xsl:value-of select="substring($indicators, ($index + 1) , 1)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="''" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- This color alternation must be in one line -->
    <xsl:variable name="bgcolor"><xsl:choose><xsl:when test="$type = 'month'"><xsl:value-of select="'#ffaa33'" /></xsl:when><xsl:when test="$type = 'year'"><xsl:value-of select="'#ffffbb'" /></xsl:when><xsl:when test="$type = 'char' and ($result = 'C' or $result = '0' or $result = '-' or $result = 'N' or $result = '')"><xsl:value-of select="'#0d5b0d'" /></xsl:when><xsl:otherwise><xsl:value-of select="'#ff0000'" /></xsl:otherwise></xsl:choose></xsl:variable>

    <!-- This color alternation must be in one line -->
    <xsl:variable name="fontColor"><xsl:choose><xsl:when test="not($type = 'char')"><xsl:value-of select="'#193385'" /></xsl:when><xsl:otherwise><xsl:value-of select="'#ffffff'" /></xsl:otherwise></xsl:choose></xsl:variable>

    <td height="15" width="27" align="center" bgcolor="{$bgcolor}" valign="middle">
      <font size="1" style="FONT-FAMILY: 'verdana';" color="{$fontColor}">  
      <xsl:value-of select="$result" />
      </font></td>    
    
    <!-- Test condition and call template if less than number -->
    <xsl:if test="$index &lt; $total">
      <xsl:call-template name="PaymentHistoryLoop">
        <xsl:with-param name="index" select="($index + 1)" />
        <xsl:with-param name="type" select="$type" />
        <xsl:with-param name="startMonth" select="$nextMonth" />
        <xsl:with-param name="startYear" select="$nextYear" />
        <xsl:with-param name="indicators" select="$indicators" />
        <xsl:with-param name="length" select="$length" />
      </xsl:call-template>
    </xsl:if>
  
  </xsl:template>  
  
  
  <!--
  *********************************************
  * Legends template
  *********************************************
  -->
  <xsl:template name="Legends">
    <a name="legend" />
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td colspan="5"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
      </tr>
  
      <tr>
        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>1</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'1'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="32%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>2</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'2'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>3</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'3'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>
      </tr>

      <tr>
        <td colspan="5"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
      </tr>
  
      <tr>
        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>4</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'4'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="32%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>5</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'5'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>6</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'6'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>
      </tr>
   
      <tr>
        <td colspan="5"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
      </tr>
  
      <tr>
        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>7</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'7'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="32%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>8</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'8'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="33%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>9</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'9'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>
      </tr>
   
      <tr>
        <td colspan="5"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
      </tr>
  
      <tr>
        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#ff0000" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>B</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'B'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="32%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0d5b0d" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>C</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'C'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="33%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0d5b0d" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>0</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'0'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>
      </tr>
   
      <tr>
        <td colspan="5"><img src="../images/spacer.gif" border="0" width="1" height="3" alt="" /></td>
      </tr>
  
      <tr>
        <td width="33%">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0d5b0d" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>N</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'N'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="32%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0d5b0d" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b>-</b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'-'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>

        <td width="1%"><img src="../images/spacer.gif" border="0" width="3" height="1" alt="" /></td>

        <td width="33%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td valign="top" align="center">
                <table width="13" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="#0d5b0d" width="13" height="13"  valign="middle" align="center">
                      <font size="1" color="#ffffff" style="FONT-FAMILY: 'verdana';"><b> </b></font></td>
                  </tr>
                </table>
              </td>
      
              <td width="1%"><img src="../images/spacer.gif" border="0" width="4" height="1" alt="" /></td>

              <td width="98%">
                <font size="1" style="FONT-FAMILY: 'verdana';">
                <xsl:call-template name="PaymentIndicatorTraslation">
                  <xsl:with-param name="code" select="'Blank'" />
                </xsl:call-template></font></td>
            </tr>
         </table>
        </td>
      </tr>
   </table>
  </xsl:template>
  
</xsl:stylesheet>