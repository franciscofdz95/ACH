<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:my="http://www.experian.com/BIS/int"
                xmlns:prd="http://www.experian.com/ARFResponse"
                exclude-result-prefixes="my">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="xml" indent="yes"/>

	<my:BalanceSheetMap>
		<mapelement key="prd:CashandEquivalent">Cash and equivalent</mapelement>
		<mapelement key="prd:ReceivablesNet">Receivables - net</mapelement>
		<mapelement key="prd:Inventory">Inventory</mapelement>
		<mapelement key="prd:OtherCurrentAssets">Other current assets</mapelement>
		<mapelement key="prd:TotalCurrentAssets">Total current assets</mapelement>
		<mapelement key="prd:FixedAssets">Fixed assets - net</mapelement>
		<mapelement key="prd:Investments">Investments</mapelement>
		<mapelement key="prd:OtherAssets">Other assets</mapelement>
		<mapelement key="prd:TotalAssets">Total assets</mapelement>
		<mapelement key="prd:DebtDuein1Year">Debt due in 1 year</mapelement>
		<mapelement key="prd:NotesPayable">Notes payable</mapelement>
		<mapelement key="prd:AccountsPayable">Accounts payable</mapelement>
		<mapelement key="prd:TaxesPayable">Taxes payable</mapelement>
		<mapelement key="prd:OtherCurrentLiabilities">Other current liabilities</mapelement>
		<mapelement key="prd:TotalCurrentLiabilities">Total current liabilities</mapelement>
		<mapelement key="prd:LongTermDebt">Long term debt</mapelement>
		<mapelement key="prd:OtherLiabilities">Other liabilities</mapelement>
		<mapelement key="prd:NetWorth">Net worth</mapelement>
		<mapelement key="prd:TotalLiabilitiesandNetWorth">Total liab. and net worth</mapelement>
	</my:BalanceSheetMap>
	<my:OperatingStatementMap>
		<mapelement key="prd:NetSales">Net sales</mapelement>
		<mapelement key="prd:CostofGoodsSold">Cost of goods sold</mapelement>
		<mapelement key="prd:GrossIncomeonSales">Gross income on sales</mapelement>
		<mapelement key="prd:Expenses">Expenses</mapelement>
		<mapelement key="prd:Pre-TaxIncome">Pre-tax income</mapelement>
		<mapelement key="prd:Taxes">Taxes</mapelement>
		<mapelement key="prd:AfterTaxes">After tax income</mapelement>
		<mapelement key="prd:ExtraordinaryIncome">Extraord. inc. &amp; discont'd ops</mapelement>
		<mapelement key="prd:NetIncome">Net income</mapelement>
	</my:OperatingStatementMap>
	<my:CriticalData>
		<mapelement key="prd:NetWorth">Net worth</mapelement>
		<mapelement key="prd:NetWorkingCapital">Net working capital</mapelement>
		<mapelement key="prd:CurrentRatio">Current ratio (times)</mapelement>
		<mapelement key="prd:TotalDebttoTangibleNetWorth">% Total debt to n.w.</mapelement>
		<mapelement key="prd:CurrentDebttoTangibleNetWorth">% Current debt to n.w.</mapelement>
		<mapelement key="prd:AfterTaxIncometoTangibleNetWorth">% After tax inc. to n.w.</mapelement>
		<mapelement key="prd:AfterTaxIncometoNetSales">% After tax inc. to net sales</mapelement>
		<mapelement key="prd:NetSalestoInventory">Net sales to inventory (times)</mapelement>
		<mapelement key="prd:CostofGoodsSoldtoInventory">CGS to inventory (times)</mapelement>
		<mapelement key="prd:AverageDaysSalesOutstanding">Avg. days sales outstanding (days)</mapelement>
	</my:CriticalData>

  <!--
  **********************************
  * Standard & Poors template
  **********************************
  -->
  <xsl:template name="CompanyFinancialInformation">
    <xsl:if test="prd:CorporateFinancialInformation">
    	<!--<xsl:apply-templates select="prd:CorporateLinkage"></xsl:apply-templates>-->
     <!--S&P Financial count-->
	<xsl:variable name="countSPFinancial">
      <xsl:value-of select="count(//prd:CorporateFinancialInformation)" />
    </xsl:variable>
      <!-- BankingRelationship -->
	<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th>
					<xsl:attribute name="colspan"><xsl:value-of select="$countSPFinancial+1"/></xsl:attribute>
					<div><a class="report_section_title">Corporate Financial Information</a></div>
				</th>
			</tr>
		</thead>
		<tbody>
	      <xsl:call-template name="StandardAndPoors" >
	      	<xsl:with-param name="countSPFinancial" select="$countSPFinancial"></xsl:with-param>
	      </xsl:call-template>
	    </tbody>
    </table>

      <!-- back to top image -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>

  <xsl:template name="StandardAndPoors">
  	<xsl:param name="countSPFinancial"></xsl:param>
    <!--detail column width value-->
	<xsl:variable name="detailColWidth">
      <xsl:value-of select="concat(66 div $countSPFinancial, '%')" />
    </xsl:variable>
    <!--

     Current Date
    --><xsl:variable name="currentDate">
      <xsl:value-of select="//prd:CorporateFinancialInformation/prd:CurrentDate" />
    </xsl:variable><!--

     Fiscal Year End Date
    --><xsl:variable name="fiscalYearEndDate">
      <xsl:value-of select="//prd:CorporateFinancialInformation/prd:FiscalYearEndDate" />
    </xsl:variable><!--

     Display Date
    --><xsl:variable name="displayDate">
        <xsl:choose>
          <xsl:when test="substring($fiscalYearEndDate,7,2) > 50">
            <xsl:value-of select="concat(substring($fiscalYearEndDate,1,2), '/', substring($fiscalYearEndDate,4,2), '/19', substring($fiscalYearEndDate,7,2) )" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat(substring($fiscalYearEndDate,1,2), '/', substring($fiscalYearEndDate,4,2), '/20', substring($fiscalYearEndDate,7,2) )" />
          </xsl:otherwise>
        </xsl:choose>
    </xsl:variable>

    <!--Balance sheet-->
    <tr class="subtitle"><th><xsl:attribute name="colspan"><xsl:value-of select="$countSPFinancial+1"/></xsl:attribute><div style="padding:5px 0">Balance sheet for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</div></th></tr>
    <tr>
    	<td></td>
    	<xsl:for-each select="prd:CorporateFinancialInformation">
    		<td class="label rightAlign">
    		<xsl:choose>
		        <xsl:when test="(prd:BalanceSheetYearEnd) and (string(number(prd:BalanceSheetYearEnd)) != 'NaN')">
		          <xsl:value-of select="number(prd:BalanceSheetYearEnd)" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="''" />
		        </xsl:otherwise>
	        </xsl:choose>
	        </td>
    	</xsl:for-each>
    </tr>
	<xsl:variable name="c_node" select="prd:CorporateFinancialInformation"/>
	<xsl:for-each select="document('SPMap.xml')/root/BalanceSheetMap/mapelement">
	<tr>
		<xsl:attribute name="class">
			<xsl:choose>
				<xsl:when test="position() mod 2 = 1">
					<xsl:value-of select="concat('even',' ',current()/@cssClass)"></xsl:value-of>
				</xsl:when>
				<xsl:when test="position() mod 2 = 0">
					<xsl:value-of select="concat('odd',' ',current()/@cssClass)"></xsl:value-of>
				</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:variable name="sPath"><xsl:value-of select="current()/@key"/></xsl:variable>
		<td><xsl:value-of select="text()"/></td>
		 <xsl:for-each select="$c_node">
		<td class="rightAlign">
			<xsl:for-each select="current()/*">
				<xsl:variable name="tagname" select="name(current())"/>
				<xsl:if test="$tagname=$sPath">
					<xsl:value-of select="format-number(current()/text(), '###,###,##0')"/>
				</xsl:if>
			</xsl:for-each>
		</td>
		</xsl:for-each>
	</tr>
	</xsl:for-each>

	<xsl:comment>Leave an empty line here</xsl:comment>
	<tr><td/></tr>
    <!-- Operating Statement -->
    <tr class="subtitle"><th><xsl:attribute name="colspan"><xsl:value-of select="$countSPFinancial+1"/></xsl:attribute><div style="padding:5px 0">Operating statement for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</div></th></tr>
    <tr>
    	<td></td>
    	<xsl:for-each select="prd:CorporateFinancialInformation">
    		<td class="label rightAlign">
    		<xsl:choose>
		        <xsl:when test="(prd:BalanceSheetYearEnd) and (string(number(prd:BalanceSheetYearEnd)) != 'NaN')">
		          <xsl:value-of select="number(prd:BalanceSheetYearEnd)" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="''" />
		        </xsl:otherwise>
	        </xsl:choose>
	        </td>
    	</xsl:for-each>
    </tr>
	<xsl:variable name="c_node1" select="prd:CorporateFinancialInformation"/>
	<xsl:for-each select="document('SPMap.xml')/root/OperatingStatementMap/mapelement">
	<tr>
		<xsl:attribute name="class">
			<xsl:choose>
				<xsl:when test="position() mod 2 = 1">
					<xsl:value-of select="'even'"></xsl:value-of>
				</xsl:when>
				<xsl:when test="position() mod 2 = 0">
					<xsl:value-of select="'odd'"></xsl:value-of>
				</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:variable name="sPath"><xsl:value-of select="current()/@key"/></xsl:variable>
		<td><xsl:value-of select="text()"/></td>
		 <xsl:for-each select="$c_node1">
		<td class="rightAlign">
			<xsl:for-each select="current()/*">
				<xsl:variable name="tagname" select="name(current())"/>
				<xsl:if test="$tagname=$sPath">
					<xsl:value-of select="format-number(current()/text(), '###,###,##0')"/>
				</xsl:if>
			</xsl:for-each>
		</td>
		</xsl:for-each>
	</tr>
	</xsl:for-each>

	<xsl:comment>Leave an empty line here</xsl:comment>
	<tr><td/></tr>
    <!-- Critical data and ratios -->
    <tr class="subtitle"><th><xsl:attribute name="colspan"><xsl:value-of select="$countSPFinancial+1"/></xsl:attribute><div style="padding:5px 0">Critical data and ratios for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</div></th></tr>
    <tr>
    	<td></td>
    	<xsl:for-each select="prd:CorporateFinancialInformation">
    		<td class="label rightAlign">
    		<xsl:choose>
		        <xsl:when test="(prd:BalanceSheetYearEnd) and (string(number(prd:BalanceSheetYearEnd)) != 'NaN')">
		          <xsl:value-of select="number(prd:BalanceSheetYearEnd)" />
		        </xsl:when>

		        <xsl:otherwise>
		          <xsl:value-of select="''" />
		        </xsl:otherwise>
	        </xsl:choose>
	        </td>
    	</xsl:for-each>
    </tr>
	<xsl:variable name="c_node2" select="prd:CorporateFinancialInformation"/>
	<xsl:for-each select="document('SPMap.xml')/root/CriticalData/mapelement">
	<tr>
		<xsl:attribute name="class">
			<xsl:choose>
				<xsl:when test="position() mod 2 = 1">
					<xsl:value-of select="'even'"></xsl:value-of>
				</xsl:when>
				<xsl:when test="position() mod 2 = 0">
					<xsl:value-of select="'odd'"></xsl:value-of>
				</xsl:when>
			</xsl:choose>
		</xsl:attribute>
		<xsl:variable name="sPath"><xsl:value-of select="current()/@key"/></xsl:variable>
		<td><xsl:value-of select="text()"/></td>
		 <xsl:for-each select="$c_node2">
		<td class="rightAlign">
			<xsl:for-each select="current()/*">
				<xsl:variable name="tagname" select="name(current())"/>
				<xsl:if test="$tagname=$sPath">
					<xsl:choose>
						<xsl:when test="$tagname = 'NetWorth' or $tagname = 'NetWorkingCapital'">
							<xsl:value-of select="format-number(current()/text(), '###,###,##0')"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="format-number(number(current()/text()) div 10, '#,##0.0')"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:if>
			</xsl:for-each>
		</td>
		</xsl:for-each>
	</tr>
	</xsl:for-each>
  </xsl:template>
</xsl:stylesheet>