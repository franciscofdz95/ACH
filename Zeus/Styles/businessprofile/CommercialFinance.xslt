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
  * CommercialFinance template
  *********************************************
  -->
  <xsl:template name="CommercialFinance">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Commercial Finance Relationships'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>
    
    <xsl:if test="prd:CommercialBankInformation">
	<!-- BankingRelationship -->
	<xsl:call-template name="BankingRelationships" />
    </xsl:if>
    
    <xsl:if test="prd:LeasingInformation">

	<xsl:if test="prd:CommercialBankInformation">
	    <!-- back to top graphic -->
	    <xsl:call-template name="BackToTop" />
	</xsl:if>

	<xsl:call-template name="LeasingRelationships" />
    </xsl:if>
  
    <xsl:if test="prd:InsuranceData">

	<xsl:if test="prd:CommercialBankInformation or prd:LeasingInformation">
	    <!-- back to top graphic -->
	    <xsl:call-template name="BackToTop" />
	</xsl:if>

	<!-- InsuranceRelationships -->
	<xsl:call-template name="InsuranceRelationships" />
    </xsl:if>
    
    <br />
      
  </xsl:template>


  <!--
  *********************************************
  * BankingRelationships template
  *********************************************
  -->
  <xsl:template name="BankingRelationships">
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
                      <b><font color="#ffffff">Banking Relationships</font></b></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
                  <!-- row of CommercialBankInformation -->
                  <xsl:apply-templates select="prd:CommercialBankInformation" />
                  
                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
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
  * CommercialBankInformation template
  *********************************************
  -->
  <xsl:template match="prd:CommercialBankInformation" xml:space="preserve">

    <xsl:variable name="institution">
      <xsl:choose>		              
        <xsl:when test="prd:InstitutionName">		    		   		   
          <xsl:value-of select="prd:InstitutionName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNDISCLOSED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="address">
      <xsl:choose>		              
        <xsl:when test="prd:Address">		    		   		   
          <xsl:value-of select="prd:Address" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="phone">
      <xsl:choose>		              
        <xsl:when test="prd:Phone and string-length(normalize-space(prd:Phone)) = 10">		    		   		   
			  <xsl:call-template name="FormatPhone">
			    <xsl:with-param name="value" select="prd:Phone" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="accountType">
      <xsl:choose>		              
        <xsl:when test="prd:Relationship and normalize-space(prd:Relationship/@code) != ''">		    		   		   
          <xsl:value-of select="prd:Relationship" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNDISCLOSED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateOpened">
      <xsl:choose>		              
        <xsl:when test="prd:OpenDate and number(prd:OpenDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:OpenDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="profileDate">
      <xsl:choose>		              
        <xsl:when test="prd:ProfileDate and number(prd:ProfileDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:ProfileDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="accountRating">
      <xsl:choose>		              
        <xsl:when test="prd:AccountRating">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:AccountRating/@code = 'N'">		    		   		   
              <xsl:value-of select="concat(prd:AccountRating, ' AS OF ', $profileDate)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="prd:AccountRating" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="figureNumber">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:FiguresInBalance/@code) != ''">		    		   		   
          <xsl:value-of select="prd:FiguresInBalance/@code" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="balanceHighDigit">
      <xsl:choose>		              
        <xsl:when test="prd:BalanceAmount and number(prd:BalanceAmount) &gt; 0">		    		   		   
          <xsl:value-of select="substring(number(prd:BalanceAmount), 1, 1)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="accountBalance">
      <xsl:choose>		              
        <xsl:when test="$figureNumber &gt; 0">		    		   		   
          <xsl:choose>		              
            <xsl:when test="$balanceHighDigit &gt; 0">		    		   		   
              
              <xsl:choose>		              
                <xsl:when test="$balanceHighDigit &lt;= 3">		    		   		   
                  <xsl:value-of select="concat('LOW ', $figureNumber, ' FIGURES')" />
                </xsl:when>
        
                <xsl:when test="$balanceHighDigit &lt;= 6">		    		   		   
                  <xsl:value-of select="concat('MID ', $figureNumber, ' FIGURES')" />
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="concat('HIGH ', $figureNumber, ' FIGURES')" />
                </xsl:otherwise>
              </xsl:choose>    
              
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

    <xsl:variable name="dateClosed">
      <xsl:choose>		              
        <xsl:when test="prd:CloseDate and number(prd:CloseDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:CloseDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td bgcolor="#ffffff" align="center">
        <table width="98%" border="0" cellspacing="0" cellpadding="1">

          <xsl:if test="position() &gt; 1">
            <tr>
              <td>
                <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Financial Institution: </b><xsl:value-of select="$institution" /></font>
            </td>
          </tr>
        
          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Address: </b><xsl:value-of select="$address" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Phone: </b><xsl:value-of select="$phone" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Account Type: </b><xsl:value-of select="$accountType" /></font>
            </td>
          </tr>
        
          <xsl:if test="normalize-space($dateOpened) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Date Opened: </b><xsl:value-of select="$dateOpened" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($accountRating) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Account Rating: </b><xsl:value-of select="$accountRating" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($accountBalance) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Account Balance: </b><xsl:value-of select="$accountBalance" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($dateClosed) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Note: </b>Account Closed as of <xsl:value-of select="$dateClosed" /></font>
              </td>
            </tr>
          </xsl:if>
          
        </table>
      </td>
    </tr>
        
  </xsl:template>


  <!--
  *********************************************
  * LeasingRelationships template
  *********************************************
  -->
  <xsl:template name="LeasingRelationships">
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
                      <b><font color="#ffffff">Leasing Relationships</font></b></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
                  <!-- row of LeasingInformation -->
                  <xsl:apply-templates select="prd:LeasingInformation" />
                  
                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
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
  * LeasingInformation template
  *********************************************
  -->
  <xsl:template match="prd:LeasingInformation" xml:space="preserve">

    <xsl:variable name="company">
      <xsl:choose>		              
        <xsl:when test="prd:LeasingName">		    		   		   
          <xsl:value-of select="prd:LeasingName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="cityState">
      <xsl:choose>		              
        <xsl:when test="prd:CityAndState">		    		   		   
          <xsl:value-of select="prd:CityAndState" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="zip">
      <xsl:choose>
        <xsl:when test="prd:Zip and string-length(normalize-space(prd:Zip)) &lt; 6">
          <xsl:value-of select="prd:Zip" />
        </xsl:when>

        <xsl:when test="prd:Zip and string-length(normalize-space(prd:Zip)) &gt; 5">
			  <xsl:call-template name="FormatZip">
			    <xsl:with-param name="value" select="prd:Zip" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="address">
      <xsl:value-of select="concat($cityState, ' ', $zip)" />
    </xsl:variable>

    <xsl:variable name="phone">
      <xsl:choose>		              
        <xsl:when test="prd:PhoneNumber and string-length(normalize-space(prd:PhoneNumber)) = 10">		    		   		   
			  <xsl:call-template name="FormatPhone">
			    <xsl:with-param name="value" select="prd:PhoneNumber" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalDate">
      <xsl:choose>		              
        <xsl:when test="prd:LeaseCommencementDate and number(prd:LeaseCommencementDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/year'" />
    		     <xsl:with-param name="value" select="prd:LeaseCommencementDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalAmount">
      <xsl:choose>		              
        <xsl:when test="prd:OriginalLeaseAmount">		    		   		   
          <xsl:value-of select="format-number(prd:OriginalLeaseAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNDISCLOSED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="productType">
      <xsl:choose>		              
        <xsl:when test="prd:LeaseProductType">		    		   		   
          <xsl:value-of select="prd:LeaseProductType" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="closedDate">
      <xsl:choose>		              
        <xsl:when test="prd:LeaseCloseDate and number(prd:LeaseCloseDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/year'" />
    		     <xsl:with-param name="value" select="prd:LeaseCloseDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="term">
      <xsl:choose>		              
        <xsl:when test="prd:LeaseTerm">		    		   		   
          <xsl:value-of select="concat(number(prd:LeaseTerm), ' MONTHS')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNDISCLOSED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="paymentsPerYear">
      <xsl:choose>		              
        <xsl:when test="prd:PaymentsPerYear">		    		   		   
          <xsl:value-of select="number(prd:PaymentsPerYear)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="paymentInterval">
      <xsl:choose>		              
        <xsl:when test="$paymentsPerYear &gt;= 1">		    		   		   
          <xsl:value-of select="concat(normalize-space($paymentsPerYear), '/YEAR')" />
        </xsl:when>

        <xsl:when test="prd:PaymentInterval/@code != ''">                     
          <xsl:value-of select="prd:PaymentInterval" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="paymentType">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:PaymentType/@code) != ''">		    		   		   
          <xsl:value-of select="prd:PaymentType" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="comment">
      <xsl:choose>		              
        <xsl:when test="prd:CommentCode">		    		   		   
          <xsl:value-of select="prd:CommentCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="currentDueDate">
      <xsl:choose>		              
        <xsl:when test="prd:CurrentDueDate and number(prd:CurrentDueDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:CurrentDueDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="scheduledAmount">
      <xsl:choose>		              
        <xsl:when test="prd:ScheduledAmountDue">		    		   		   
          <xsl:value-of select="concat(prd:ScheduledAmountDue/prd:Modifier/@code, format-number(prd:ScheduledAmountDue/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="paymentsOverdue">
      <xsl:choose>		              
        <xsl:when test="prd:NumberOfPaymentsOverdue and number(prd:NumberOfPaymentsOverdue) &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:NumberOfPaymentsOverdue)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="overdueAmount">
      <xsl:choose>		              
        <xsl:when test="prd:PaymentOverdueAmount">		    		   		   
          <xsl:value-of select="concat(prd:PaymentOverdueAmount/prd:Modifier/@code, format-number(prd:PaymentOverdueAmount/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="remainingBalance">
      <xsl:choose>		              
        <xsl:when test="prd:TotalRemainingBalance">		    		   		   
          <xsl:value-of select="concat(prd:TotalRemainingBalance/prd:Modifier/@code, format-number(prd:TotalRemainingBalance/prd:Amount, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="currentPayments">
      <xsl:choose>		              
        <xsl:when test="prd:NumberOfCurrentPayments and number(prd:NumberOfCurrentPayments) &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:NumberOfCurrentPayments)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="latePayments">
      <xsl:choose>		              
        <xsl:when test="prd:NumberOfLatePayments and number(prd:NumberOfLatePayments) &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:NumberOfLatePayments)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td bgcolor="#ffffff" align="center">
        <table width="98%" border="0" cellspacing="0" cellpadding="1">

          <xsl:if test="position() &gt; 1">
            <tr>
              <td>
                <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Leasing Company: </b><xsl:value-of select="$company" /></font>
            </td>
          </tr>
        
          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Address: </b><xsl:value-of select="$address" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Phone: </b><xsl:value-of select="$phone" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($originalDate) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Origination Date: </b><xsl:value-of select="$originalDate" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($originalAmount) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Original Lease Amount: </b><xsl:value-of select="$originalAmount" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($productType) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Lease Product Type: </b><xsl:value-of select="$productType" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($closedDate) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Lease Close Date: </b><xsl:value-of select="$closedDate" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($term) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Lease Term: </b><xsl:value-of select="$term" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($paymentInterval) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Payment Interval: </b><xsl:value-of select="$paymentInterval" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($paymentType) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Payment Type: </b><xsl:value-of select="$paymentType" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($comment) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Comment: </b><xsl:value-of select="$comment" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($currentDueDate) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Current Due Date: </b><xsl:value-of select="$currentDueDate" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($scheduledAmount) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Current Scheduled Amount Due: </b><xsl:value-of select="$scheduledAmount" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($paymentsOverdue) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Number of Payments Overdue: </b><xsl:value-of select="$paymentsOverdue" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($overdueAmount) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Amount of Overdue Payments: </b><xsl:value-of select="$overdueAmount" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($remainingBalance) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Remaining Balance: </b><xsl:value-of select="$remainingBalance" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($currentPayments) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Current Payments: </b><xsl:value-of select="$currentPayments" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($latePayments) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Late Payments: </b><xsl:value-of select="$latePayments" /></font>
              </td>
            </tr>
          </xsl:if>
          
        </table>
      </td>
    </tr>
        
  </xsl:template>


  <!--
  *********************************************
  * InsuranceRelationships template
  *********************************************
  -->
  <xsl:template name="InsuranceRelationships">
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
                      <b><font color="#ffffff">Insurance Bond Relationships</font></b></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
                  <!-- row of InsuranceData -->
                  <xsl:apply-templates select="prd:InsuranceData" />
                  
                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
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
  * InsuranceData template
  *********************************************
  -->
  <xsl:template match="prd:InsuranceData" xml:space="preserve">

    <xsl:variable name="company">
      <xsl:choose>		              
        <xsl:when test="prd:InstitutionName">		    		   		   
          <xsl:value-of select="prd:InstitutionName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNDISCLOSED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="address">
      <xsl:choose>		              
        <xsl:when test="prd:Address">		    		   		   
          <xsl:value-of select="prd:Address" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="phone">
      <xsl:choose>		              
        <xsl:when test="prd:Phone and string-length(normalize-space(prd:Phone)) = 10">		    		   		   
			  <xsl:call-template name="FormatPhone">
			    <xsl:with-param name="value" select="prd:Phone" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="type">
      <xsl:choose>		              
        <xsl:when test="prd:Relationship and normalize-space(prd:Relationship/@code) != ''">		    		   		   
          <xsl:value-of select="prd:Relationship" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'UNSPECIFIED'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td bgcolor="#ffffff" align="center">
        <table width="98%" border="0" cellspacing="0" cellpadding="1">

          <xsl:if test="position() &gt; 1">
            <tr>
              <td>
                <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Bonding Company: </b><xsl:value-of select="$company" /></font>
            </td>
          </tr>
        
          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Address: </b><xsl:value-of select="$address" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Phone: </b><xsl:value-of select="$phone" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Bond Type: </b><xsl:value-of select="$type" /></font>
            </td>
          </tr>
          
        </table>
      </td>
    </tr>
        
  </xsl:template>

</xsl:stylesheet>