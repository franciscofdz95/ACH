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
  **********************************
  * Standard & Poors template
  **********************************
  -->
  <xsl:template name="StandardAndPoors">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Corporate Financial Information'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <!-- S&P Financial count -->
    <xsl:variable name="countSPFinancial">
      <xsl:value-of select="count(//prd:StandardAndPoorsFinancialInformation)" />
    </xsl:variable>

    <!-- detail column width value -->
    <xsl:variable name="detailColWidth">
      <xsl:value-of select="concat(66 div $countSPFinancial, '%')" />
    </xsl:variable>

    <!-- Current Date -->
    <xsl:variable name="currentDate">
      <xsl:value-of select="//prd:StandardAndPoorsFinancialInformation/prd:CurrentDate" />
    </xsl:variable>

    <!-- Fiscal Year End Date -->
    <xsl:variable name="fiscalYearEndDate">
      <xsl:value-of select="//prd:StandardAndPoorsFinancialInformation/prd:FiscalYearEndDate" />
    </xsl:variable>

    <!-- Display Date -->
    <xsl:variable name="displayDate">
        <xsl:choose>		              
          <xsl:when test="substring($fiscalYearEndDate,7,2) > 50">		    		   		   
            <xsl:value-of select="concat(substring($fiscalYearEndDate,1,2), '/', substring($fiscalYearEndDate,4,2), '/19', substring($fiscalYearEndDate,7,2) )" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:value-of select="concat(substring($fiscalYearEndDate,1,2), '/', substring($fiscalYearEndDate,4,2), '/20', substring($fiscalYearEndDate,7,2) )" />
          </xsl:otherwise>
        </xsl:choose>    
    </xsl:variable>

    <!-- Balance sheet -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="32">
                    <td bgcolor="{$borderColor}" colspan="{$countSPFinancial + 3}" align="center" valign="middle">
                        <b><font color="#ffffff">  Balance sheet for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>
                        Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</font></b>
                    </td>
                  </tr>
                </table>

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr bgcolor="#ffffff" height="20">
                    <td width="1%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>

                    <td width="31%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                    
                    <!-- Balance sheet header template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="header">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <td width="2%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                  </tr>
                </table>

	           <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="20">

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'1%'" />
                    	<xsl:with-param name="rows" select="'19'" />
                    </xsl:call-template> 

                    <!-- Balance sheet row names template -->
                    <xsl:call-template name="balanceSheetRowNames">
                    </xsl:call-template> 

                    <!-- Balance sheet details template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="balanceSheet">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'2%'" />
                    	<xsl:with-param name="rows" select="'19'" />
                    </xsl:call-template> 
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <xsl:call-template name="BackToTop" />

    <!-- Operating Statement -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="32">
                    <td bgcolor="{$borderColor}" colspan="{$countSPFinancial + 3}" align="center" valign="middle">
                        <b><font color="#ffffff">  Operating statement for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>
                        Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</font></b>
                    </td>
                  </tr>
                </table>

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr bgcolor="#ffffff" height="20">
                    <td width="1%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>

                    <td width="31%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                    
                    <!-- Operating Statement header template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="header">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <td width="2%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                  </tr>
                </table>

	           <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="20">

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'1%'" />
                    	<xsl:with-param name="rows" select="'9'" />
                    </xsl:call-template> 

                    <!-- Operating Statement row names template -->
                    <xsl:call-template name="operatingStatementRowNames">
                    </xsl:call-template> 

                    <!-- Operating Statement details template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="operatingStatement">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'2%'" />
                    	<xsl:with-param name="rows" select="'9'" />
                    </xsl:call-template> 
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <xsl:call-template name="BackToTop" />

    <!-- Critical Data -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="32">
                    <td bgcolor="{$borderColor}" colspan="{$countSPFinancial + 3}" align="center" valign="middle">
                        <b><font color="#ffffff">  Critical data and ratios for fiscal year ending: <xsl:value-of select="$currentDate" /><br/>
                        Data current through: <xsl:value-of select="$displayDate" />  ($ Thousands)</font></b>
                    </td>
                  </tr>
                </table>

                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr bgcolor="#ffffff" height="20">
                    <td width="1%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>

                    <td width="31%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                    
                    <!-- Critical Data header template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="header">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <td width="2%"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></td>
                  </tr>
                </table>

	           <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="20">

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'1%'" />
                    	<xsl:with-param name="rows" select="'10'" />
                    </xsl:call-template> 

                    <!-- Critical Data row names template -->
                    <xsl:call-template name="criticalDataRowNames">
                    </xsl:call-template> 

                    <!-- Critical Data details template -->
                    <xsl:apply-templates select="prd:StandardAndPoorsFinancialInformation" mode="criticalData">
                    	<xsl:with-param name="colWidth" select="$detailColWidth" />
                    	<xsl:sort order="descending" select="prd:BalanceSheetYearEnd" />
                    </xsl:apply-templates>

                    <!--Blank column template -->
                    <xsl:call-template name="blankColumn">
                    	<xsl:with-param name="colWidth" select="'2%'" />
                    	<xsl:with-param name="rows" select="'10'" />
                    </xsl:call-template> 
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
  ********************************************************************
  * StandardAndPoorsFinancialInformation header template
  ********************************************************************
  -->
  <xsl:template match="prd:StandardAndPoorsFinancialInformation" mode="header" xml:space="preserve">
    <xsl:param name="colWidth" select="'22%'" />

    <xsl:variable name="tmpYear">
      <xsl:choose>		              
        <xsl:when test="(prd:BalanceSheetYearEnd) and (string(number(prd:BalanceSheetYearEnd)) != 'NaN')">		    		   		   
          <xsl:value-of select="number(prd:BalanceSheetYearEnd)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

                    <td align="right" width="{$colWidth}"><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$tmpYear" /></b></font></td>

  </xsl:template>


  <!--
  ******************************************************************************
  * StandardAndPoorsFinancialInformation Balance Sheet template
  ******************************************************************************
  -->
  <xsl:template match="prd:StandardAndPoorsFinancialInformation" mode="balanceSheet" xml:space="preserve">
    <xsl:param name="colWidth" select="'22%'" />

    <xsl:variable name="cashAndEquivalent">
      <xsl:choose>		              
        <xsl:when test="prd:CashandEquivalent and normalize-space(prd:CashandEquivalent) != ''">
          <xsl:value-of select="prd:CashandEquivalent" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="receivablesNet">
      <xsl:choose>		              
        <xsl:when test="prd:ReceivablesNet and normalize-space(prd:ReceivablesNet) != ''">
          <xsl:value-of select="prd:ReceivablesNet" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="inventory">
      <xsl:choose>		              
        <xsl:when test="prd:Inventory and normalize-space(prd:Inventory) != ''">
          <xsl:value-of select="prd:Inventory" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="otherCurrentAssets">
      <xsl:choose>		              
        <xsl:when test="prd:OtherCurrentAssets and normalize-space(prd:OtherCurrentAssets) != ''">
          <xsl:value-of select="prd:OtherCurrentAssets" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalCurrentAssets">
      <xsl:choose>		              
        <xsl:when test="prd:TotalCurrentAssets and normalize-space(prd:TotalCurrentAssets) != ''">
          <xsl:value-of select="prd:TotalCurrentAssets" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="fixedAssets">
      <xsl:choose>		              
        <xsl:when test="prd:FixedAssets and normalize-space(prd:FixedAssets) != ''">
          <xsl:value-of select="prd:FixedAssets" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="investments">
      <xsl:choose>		              
        <xsl:when test="prd:Investments and normalize-space(prd:Investments) != ''">
          <xsl:value-of select="prd:Investments" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="otherAssets">
      <xsl:choose>		              
        <xsl:when test="prd:OtherAssets and normalize-space(prd:OtherAssets) != ''">
          <xsl:value-of select="prd:OtherAssets" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalAssets">
      <xsl:choose>		              
        <xsl:when test="prd:TotalAssets and normalize-space(prd:TotalAssets) != ''">
          <xsl:value-of select="prd:TotalAssets" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="debtDuein1Year">
      <xsl:choose>		              
        <xsl:when test="prd:DebtDuein1Year and normalize-space(prd:DebtDuein1Year) != ''">
          <xsl:value-of select="prd:DebtDuein1Year" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="notesPayable">
      <xsl:choose>		              
        <xsl:when test="prd:NotesPayable and normalize-space(prd:NotesPayable) != ''">
          <xsl:value-of select="prd:NotesPayable" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="accountsPayable">
      <xsl:choose>		              
        <xsl:when test="prd:AccountsPayable and normalize-space(prd:AccountsPayable) != ''">
          <xsl:value-of select="prd:AccountsPayable" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="taxesPayable">
      <xsl:choose>		              
        <xsl:when test="prd:TaxesPayable and normalize-space(prd:TaxesPayable) != ''">
          <xsl:value-of select="prd:TaxesPayable" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="otherCurrentLiabilities">
      <xsl:choose>		              
        <xsl:when test="prd:OtherCurrentLiabilities and normalize-space(prd:OtherCurrentLiabilities) != ''">
          <xsl:value-of select="prd:OtherCurrentLiabilities" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalCurrentLiabilities">
      <xsl:choose>		              
        <xsl:when test="prd:TotalCurrentLiabilities and normalize-space(prd:TotalCurrentLiabilities) != ''">
          <xsl:value-of select="prd:TotalCurrentLiabilities" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="longTermDebt">
      <xsl:choose>		              
        <xsl:when test="prd:LongTermDebt and normalize-space(prd:LongTermDebt) != ''">
          <xsl:value-of select="prd:LongTermDebt" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="otherLiabilities">
      <xsl:choose>		              
        <xsl:when test="prd:OtherLiabilities and normalize-space(prd:OtherLiabilities) != ''">
          <xsl:value-of select="prd:OtherLiabilities" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="netWorth">
      <xsl:choose>		              
        <xsl:when test="prd:NetWorth and normalize-space(prd:NetWorth) != ''">
          <xsl:value-of select="prd:NetWorth" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalLiabilitiesandNetWorth">
      <xsl:choose>		              
        <xsl:when test="prd:TotalLiabilitiesandNetWorth and normalize-space(prd:TotalLiabilitiesandNetWorth) != ''">
          <xsl:value-of select="prd:TotalLiabilitiesandNetWorth" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

		        <td width="{$colWidth}">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
			          <tr><td height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($cashAndEquivalent, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($receivablesNet, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($inventory, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($otherCurrentAssets, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($totalCurrentAssets, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($fixedAssets, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($investments, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($otherAssets, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($totalAssets, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($debtDuein1Year, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($notesPayable, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($accountsPayable, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($taxesPayable, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($otherCurrentLiabilities, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($totalCurrentLiabilities, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($longTermDebt, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($otherLiabilities, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($netWorth, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($totalLiabilitiesandNetWorth, '###,###,##0')" /></font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  *************************************************************************************
  * StandardAndPoorsFinancialInformation Operating Statement template
  *************************************************************************************
  -->
  <xsl:template match="prd:StandardAndPoorsFinancialInformation" mode="operatingStatement" xml:space="preserve">
    <xsl:param name="colWidth" select="'22%'" />

    <xsl:variable name="netSales">
      <xsl:choose>		              
        <xsl:when test="prd:NetSales and normalize-space(prd:NetSales) != ''">
          <xsl:value-of select="prd:NetSales" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="costofGoodsSold">
      <xsl:choose>		              
        <xsl:when test="prd:CostofGoodsSold and normalize-space(prd:CostofGoodsSold) != ''">
          <xsl:value-of select="prd:CostofGoodsSold" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="grossIncomeonSales">
      <xsl:choose>		              
        <xsl:when test="prd:GrossIncomeonSales and normalize-space(prd:GrossIncomeonSales) != ''">
          <xsl:value-of select="prd:GrossIncomeonSales" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="expenses">
      <xsl:choose>		              
        <xsl:when test="prd:Expenses and normalize-space(prd:Expenses) != ''">
          <xsl:value-of select="prd:Expenses" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="pre-TaxIncome">
      <xsl:choose>		              
        <xsl:when test="prd:Pre-TaxIncome and normalize-space(prd:Pre-TaxIncome) != ''">
          <xsl:value-of select="prd:Pre-TaxIncome" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="taxes">
      <xsl:choose>		              
        <xsl:when test="prd:Taxes and normalize-space(prd:Taxes) != ''">
          <xsl:value-of select="prd:Taxes" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="afterTaxes">
      <xsl:choose>		              
        <xsl:when test="prd:AfterTaxes and normalize-space(prd:AfterTaxes) != ''">
          <xsl:value-of select="prd:AfterTaxes" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="extraordinaryIncome">
      <xsl:choose>		              
        <xsl:when test="prd:ExtraordinaryIncome and normalize-space(prd:ExtraordinaryIncome) != ''">
          <xsl:value-of select="prd:ExtraordinaryIncome" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="netIncome">
      <xsl:choose>		              
        <xsl:when test="prd:NetIncome and normalize-space(prd:NetIncome) != ''">
          <xsl:value-of select="prd:NetIncome" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

		        <td width="{$colWidth}">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($netSales, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($costofGoodsSold, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($grossIncomeonSales, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($expenses, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($pre-TaxIncome, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($taxes, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($afterTaxes, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($extraordinaryIncome, '###,###,##0')" /></font></td></tr>
			          <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($netIncome, '###,###,##0')" /></font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  **************************************************************************
  * StandardAndPoorsFinancialInformation Critical Data template
  **************************************************************************
  -->
  <xsl:template match="prd:StandardAndPoorsFinancialInformation" mode="criticalData" xml:space="preserve">
    <xsl:param name="colWidth" select="'22%'" />

    <xsl:variable name="tangibleNetWorth">
      <xsl:choose>		              
        <xsl:when test="prd:TangibleNetWorth and normalize-space(prd:TangibleNetWorth) != ''">
          <xsl:value-of select="prd:TangibleNetWorth" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="netWorkingCapital">
      <xsl:choose>		              
        <xsl:when test="prd:NetWorkingCapital and normalize-space(prd:NetWorkingCapital) != ''">
          <xsl:value-of select="prd:NetWorkingCapital" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="currentRatio">
      <xsl:choose>		              
        <xsl:when test="prd:CurrentRatio and normalize-space(prd:CurrentRatio) != ''">
          <xsl:value-of select="number(prd:CurrentRatio)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalDebttoTangibleNetWorth">
      <xsl:choose>		              
        <xsl:when test="prd:TotalDebttoTangibleNetWorth and normalize-space(prd:TotalDebttoTangibleNetWorth) != ''">
          <xsl:value-of select="number(prd:TotalDebttoTangibleNetWorth)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="currentDebttoTangibleNetWorth">
      <xsl:choose>		              
        <xsl:when test="prd:CurrentDebttoTangibleNetWorth and normalize-space(prd:CurrentDebttoTangibleNetWorth) != ''">
          <xsl:value-of select="number(prd:CurrentDebttoTangibleNetWorth)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="afterTaxIncometoTangibleNetWorth">
      <xsl:choose>		              
        <xsl:when test="prd:AfterTaxIncometoTangibleNetWorth and normalize-space(prd:AfterTaxIncometoTangibleNetWorth) != ''">
          <xsl:value-of select="number(prd:AfterTaxIncometoTangibleNetWorth)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="afterTaxIncometoNetSales">
      <xsl:choose>		              
        <xsl:when test="prd:AfterTaxIncometoNetSales and normalize-space(prd:AfterTaxIncometoNetSales) != ''">
          <xsl:value-of select="number(prd:AfterTaxIncometoNetSales)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="netSalestoInventory">
      <xsl:choose>		              
        <xsl:when test="prd:NetSalestoInventory and normalize-space(prd:NetSalestoInventory) != ''">
          <xsl:value-of select="number(prd:NetSalestoInventory)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="costofGoodsSoldtoInventory">
      <xsl:choose>		              
        <xsl:when test="prd:CostofGoodsSoldtoInventory and normalize-space(prd:CostofGoodsSoldtoInventory) != ''">
          <xsl:value-of select="number(prd:CostofGoodsSoldtoInventory)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="averageDaysSalesOutstanding">
      <xsl:choose>		              
        <xsl:when test="prd:AverageDaysSalesOutstanding and normalize-space(prd:AverageDaysSalesOutstanding) != ''">
          <xsl:value-of select="number(prd:AverageDaysSalesOutstanding)*.1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

		        <td width="{$colWidth}">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
		                <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($tangibleNetWorth, '###,###,##0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($netWorkingCapital, '###,###,##0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($currentRatio, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($totalDebttoTangibleNetWorth, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($currentDebttoTangibleNetWorth, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($afterTaxIncometoTangibleNetWorth, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($afterTaxIncometoNetSales, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($netSalestoInventory, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($costofGoodsSoldtoInventory, '##,###,##0.0')" /></font></td></tr>
		                <tr><td  height="20" bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($averageDaysSalesOutstanding, '##,###,##0.0')" /></font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  *********************************************************************************************
  * StandardAndPoorsFinancialInformation Balance Sheet Row Names template
  *********************************************************************************************
  -->
  <xsl:template name="balanceSheetRowNames">

		        <td width="31%">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
			          <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Cash and equivalent</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Receivables - net</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Inventory</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Other current assets</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Total current assets</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Fixed assets - net</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Investments</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Other assets</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Total assets</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Debt due in 1 year</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Notes payable</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Accounts payable</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Taxes payable</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Other current liabilities</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Total current liabilities</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Long term debt</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Other liabilities</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net worth</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Total liab. and net worth</font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  ****************************************************************************************************
  * StandardAndPoorsFinancialInformation Operating Statement Row Names template
  ****************************************************************************************************
  -->
  <xsl:template name="operatingStatementRowNames">

		        <td width="31%">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net sales</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Cost of goods sold</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Gross income on sales</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Expenses</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Pre-tax income</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Taxes</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">After tax income</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Extraord. inc. &amp; discont'd ops</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net income</font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  ******************************************************************************************
  * StandardAndPoorsFinancialInformation Critical Data Row Names template
  ******************************************************************************************
  -->
  <xsl:template name="criticalDataRowNames">

		        <td width="31%">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net worth</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net working capital</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Current ratio (times)</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">% Total debt to n.w.</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">% Current debt to n.w.</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">% After tax inc. to n.w.</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">% After tax inc. to net sales</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Net sales to inventory (times)</font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">CGS to inventory (times)</font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';">Avg. days sales outstanding (days)</font></td></tr>
		          </table>
		        </td>

  </xsl:template>


  <!--
  ****************************************************************************
  * StandardAndPoorsFinancialInformation Blank Column template
  ****************************************************************************
  -->
  <xsl:template name="blankColumn">
    <xsl:param name="colWidth" select="'1%'" />
    <xsl:param name="rows" select="'19'" />

		        <td width="{$colWidth}">
		          <table width="100%" border="0" cellspacing="0" cellpadding="0">
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
      <xsl:choose>		              
        <xsl:when test="$rows = 10">		    		   		   
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
        </xsl:when>
        <xsl:when test="$rows = 19">		    		   		   
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
		                 <tr><td height="20" bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:text disable-output-escaping="yes">&#160;</xsl:text></font></td></tr>
        </xsl:when>
        <xsl:otherwise>
        </xsl:otherwise>
      </xsl:choose>

		          </table>
		        </td>    
  </xsl:template>

</xsl:stylesheet>