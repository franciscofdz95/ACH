<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                 xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                 xmlns:prd="http://www.experian.com/ARFResponse">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="xml" indent="yes"/>


  <!--
  *********************************************
  * CommercialFinance template
  *********************************************
  -->
	<xsl:template name="CommercialFinance">
		<!-- Section title -->

	<xsl:if test="prd:CommercialBankInformation or prd:InsuranceData or prd:LeasingInformation">
	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<th><a class="report_section_title">Commercial Finance Relationships</a></th>
		</thead>
		<tbody>
		<xsl:if test="prd:CommercialBankInformation">
			<xsl:call-template name="BankingRelationships" />
		</xsl:if>

		<xsl:if test="prd:InsuranceData">
			<!-- InsuranceRelationships -->
			<xsl:call-template name="InsuranceRelationships" />
		</xsl:if>
		<xsl:if test="prd:LeasingInformation"> <xsl:call-template
			name="LeasingRelationships" /> </xsl:if>
		<!-- back to top image -->
		</tbody>
	</table>
	<xsl:call-template name="BackToTop" />
	</xsl:if>
	</xsl:template>


  <!--
  *********************************************
  * BankingRelationships template
  *********************************************
  -->
  <xsl:template name="BankingRelationships">
  	<tr class="subtitle">
  		<th>Banking Relationships</th>
  	</tr>
  	<tr><td style="padding: 2px 5px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="width:100%">
		<tbody>
			<!-- row of CommercialBankInformation -->
			<xsl:apply-templates select="prd:CommercialBankInformation" />
		</tbody>
    </table>
	</td></tr>
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
      <xsl:variable name="figureText">
        <xsl:call-template name="SpellDigitNumber">
          <xsl:with-param name="upperCase" select="true()" />
          <xsl:with-param name="value" select="$figureNumber" />
        </xsl:call-template>
      </xsl:variable>

      <xsl:choose>
        <xsl:when test="$figureNumber &gt; 0">
          <xsl:choose>
            <xsl:when test="$balanceHighDigit &gt; 0">

              <xsl:choose>
                <xsl:when test="$balanceHighDigit &lt;= 3">
                  <xsl:value-of select="concat('LOW ', $figureText, ' FIGURES')" />
                </xsl:when>

                <xsl:when test="$balanceHighDigit &lt;= 6">
                  <xsl:value-of select="concat('MID ', $figureText, ' FIGURES')" />
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="concat('HIGH ', $figureText, ' FIGURES')" />
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

     <xsl:if test="position() &gt; 1">
       <tr>
         <td><xsl:call-template name="nbsp"/></td>
       </tr>
     </xsl:if>

          <tr>
            <td>
              <b>Financial Institution: </b><xsl:value-of select="$institution" />
            </td>
          </tr>

          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td >
                <b>Address: </b><xsl:value-of select="$address" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td>
                <b>Phone: </b><xsl:value-of select="$phone" />
              </td>
            </tr>
          </xsl:if>

          <tr>
            <td>
              <b>Account Type: </b><xsl:value-of select="$accountType" />
            </td>
          </tr>

          <xsl:if test="normalize-space($dateOpened) != ''">
            <tr>
              <td>
                <b>Date Opened: </b><xsl:value-of select="$dateOpened" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($accountRating) != ''">
            <tr>
              <td>
                <b>Account Rating: </b><xsl:value-of select="$accountRating" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($accountBalance) != ''">
            <tr>
              <td>
                <b>Account Balance: </b><xsl:value-of select="$accountBalance" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($dateClosed) != ''">
            <tr>
              <td>
                <b>Note: </b>ACCOUNT CLOSED AS OF <xsl:value-of select="$dateClosed" />
              </td>
            </tr>
          </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * LeasingRelationships template
  *********************************************
  -->
  <xsl:template name="LeasingRelationships">
  	<tr class="subtitle">
  		<th>Leasing Information</th>
  	</tr>
  	<tr><td style="padding: 2px 5px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="width:100%">
		<tbody>
          <xsl:apply-templates select="prd:LeasingInformation" />
		</tbody>
    </table>
	</td></tr>

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

          <xsl:if test="position() &gt; 1">
            <tr>
              <td><xsl:call-template name="nbsp"/></td>
            </tr>
          </xsl:if>

          <tr>
            <td>
              <b>Leasing Company: </b><xsl:value-of select="$company" />
            </td>
          </tr>

          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td>
                <b>Address: </b><xsl:value-of select="$address" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td>
                <b>Phone: </b><xsl:value-of select="$phone" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($originalDate) != ''">
            <tr>
              <td>
                <b>Origination Date: </b><xsl:value-of select="$originalDate" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($originalAmount) != ''">
            <tr>
              <td>
                <b>Original Lease Amount: </b><xsl:value-of select="$originalAmount" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($productType) != ''">
            <tr>
              <td>
                <b>Lease Product Type: </b><xsl:value-of select="$productType" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($closedDate) != ''">
            <tr>
              <td>
                <b>Lease Close Date: </b><xsl:value-of select="$closedDate" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($term) != ''">
            <tr>
              <td>
                <b>Lease Term: </b><xsl:value-of select="$term" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($paymentInterval) != ''">
            <tr>
              <td>
                <b>Payment Interval: </b><xsl:value-of select="$paymentInterval" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($paymentType) != ''">
            <tr>
              <td>
                <b>Payment Type: </b><xsl:value-of select="$paymentType" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($comment) != ''">
            <tr>
              <td>
                <b>Comment: </b><xsl:value-of select="$comment" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($currentDueDate) != ''">
            <tr>
              <td>
                <b>Current Due Date: </b><xsl:value-of select="$currentDueDate" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($scheduledAmount) != ''">
            <tr>
              <td>
                <b>Current Scheduled Amount Due: </b><xsl:value-of select="$scheduledAmount" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($paymentsOverdue) != ''">
            <tr>
              <td>
                <b>Number of Payments Overdue: </b><xsl:value-of select="$paymentsOverdue" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($overdueAmount) != ''">
            <tr>
              <td>
                <b>Amount of Overdue Payments: </b><xsl:value-of select="$overdueAmount" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($remainingBalance) != ''">
            <tr>
              <td>
                <b>Remaining Balance: </b><xsl:value-of select="$remainingBalance" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($currentPayments) != ''">
            <tr>
              <td>
                <b>Current Payments: </b><xsl:value-of select="$currentPayments" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($latePayments) != ''">
            <tr>
              <td>
                <b>Late Payments: </b><xsl:value-of select="$latePayments" />
              </td>
            </tr>
          </xsl:if>
  </xsl:template>


  <!--
  *********************************************
  * InsuranceRelationships template
  *********************************************
  -->
  <xsl:template name="InsuranceRelationships">
  <tr class="subtitle"><th>Insurance Bond Relationships</th></tr>
  <tr><td style="padding: 2px 5px;">
    <table width="100%" border="0" cellspacing="0" cellpadding="0" style="width:100%">
		<!-- row of InsuranceData -->
		<tbody>
		<xsl:apply-templates select="prd:InsuranceData" />
		</tbody>
    </table>
  </td></tr>
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

          <xsl:if test="position() &gt; 1">
            <tr>
              <td><xsl:call-template name="nbsp"/></td>
            </tr>
          </xsl:if>

          <tr>
            <td>
              <b>Bonding Company: </b><xsl:value-of select="$company" />
            </td>
          </tr>

          <xsl:if test="normalize-space($address) != ''">
            <tr>
              <td>
                <b>Address: </b><xsl:value-of select="$address" />
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($phone) != ''">
            <tr>
              <td>
                <b>Phone: </b><xsl:value-of select="$phone" />
              </td>
            </tr>
          </xsl:if>

          <tr>
            <td>
              <b>Bond Type: </b><xsl:value-of select="$type" />
            </td>
          </tr>
  </xsl:template>

</xsl:stylesheet>