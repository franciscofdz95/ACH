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
  * UCCProfile template
  *********************************************
  -->
  <xsl:template name="UCCProfile">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'UCC Profile'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <xsl:if test="prd:UCCFilings">
      <xsl:choose>
        <xsl:when test="count(prd:UCCFilings) &gt; 10">
          The number of UCC Filings is summarized with the 10 most recent listed below.
          <br /><br />
          A full UCC detail report is available the same business day by entering
          UCC-<xsl:value-of select="prd:BusinessNameAndAddress/prd:ExperianFileNumber" />-<xsl:value-of select="prd:InputSummary/prd:InquiryTransactionNumber" /><br />
          into the Business ID Number field on the BizApps order page.
        </xsl:when>
        
        <xsl:otherwise>
          The UCC Filings are summarized and listed below.
        </xsl:otherwise>
      </xsl:choose>
    </xsl:if>
    
    <!-- UCCFilingsSummaryCounts -->
    <xsl:apply-templates select="prd:UCCFilingsSummaryCounts" />

    <xsl:if test="prd:UCCFilings">
      <!-- back to top graphic -->
      <xsl:call-template name="BackToTop" />
    
      <xsl:call-template name="UCCFilingSection" />
    </xsl:if>
  </xsl:template>

  <!--
  *********************************************
  * UCCFilingsSummaryCounts template
  *********************************************
  -->
  <xsl:template match="prd:UCCFilingsSummaryCounts" xml:space="preserve">
    
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td height="23" bgcolor="{$borderColor}" colspan="2" align="center" valign="middle">
                      <b><font color="#ffffff">UCC Summary</font></b></td>

                    <td height="23" bgcolor="{$borderColor}" colspan="5" align="center" valign="middle">
                      <b><font color="#ffffff">Filings</font></b></td>
                  </tr>

                  <!-- Column Headers -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="20%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date Range</b></font></td>
                    <td align="center" width="10%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Year</b></font></td>
                    <td align="center" width="14%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Cautionary<br />UCCs<sup>**</sup></b></font></td>
                    <td align="center" width="14%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Total<br />Filed</b></font></td>
                    <td align="center" width="14%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Released /<br />Termination</b></font></td>
                    <td align="center" width="14%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Continuous</b></font></td>
                    <td align="center" width="14%" rowspan="1"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Amended /<br />Assigned</b></font></td>
                  </tr>
                  
                  <!-- MostRecent6Months -->
                  <xsl:apply-templates select="prd:MostRecent6Months" />

                  <!-- Previous6Months -->
                  <xsl:apply-templates select="prd:Previous6Months" />

                  <tr>
                    <td bgcolor="{$borderColor}" colspan="7">
                      <img src="../images/spacer.gif" width="0" height="1" alt=""/></td>
                  </tr>

                  <!-- total line -->
                  <tr>
                    <td height="20" bgcolor="#e5f5fa">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="75%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Total</b></font>
                          </td>
                          <td width="25%">
                          </td>
                        </tr>
                      </table>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b></b></font>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="sum(.//prd:FilingsWithDerogatoryCollateral)" /></b></font>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="sum(.//prd:FilingsTotal)" /></b></font>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <b><xsl:value-of select="sum(.//prd:ReleasesAndTerminationsTotal)" /></b></font>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <b><xsl:value-of select="sum(.//prd:ContinuationsTotal)" /></b></font>
                    </td>
              
                    <td height="20" bgcolor="#e5f5fa" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
                      <b><xsl:value-of select="sum(.//prd:AmendedAndAssignedTotal)" /></b></font>
                    </td>
              
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

    <table width="100%" border="0" cellspacing="2" cellpadding="0">
      <tr>
        <td>
            <font size="1" style="FONT-FAMILY: 'verdana';"><i>
            ** Cautionary UCC Filings include one or more of the following collateral:<br/>
            Accounts, Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, Notes Receivable or Proceeds.
            </i></font>
        </td>    
      </tr>
    </table>
        
  </xsl:template>


  <!--
  ************************************************************
  * MostRecent6Months | Previous6Months template
  ************************************************************
  -->
  <xsl:template match="prd:MostRecent6Months | prd:Previous6Months" xml:space="preserve">
    <xsl:variable name="month">
	   <xsl:call-template name="FormatMonth">
	     <xsl:with-param name="monthValue" select="number(substring(prd:StartDate, 5, 2))" />
	     <xsl:with-param name="upperCase" select="true()" />
	   </xsl:call-template>
    </xsl:variable>		    		   		   

    <xsl:variable name="nextMonth">
	   <xsl:call-template name="FormatMonth">
	     <xsl:with-param name="monthValue" select="number(substring(prd:StartDate, 5, 2)) + 5" />
	     <xsl:with-param name="upperCase" select="true()" />
	   </xsl:call-template>
    </xsl:variable>		    		   		   

    <xsl:variable name="dateRange">
      <xsl:choose>		              
        <xsl:when test="name() = 'MostRecent6Months'">		    		   		   
          <xsl:value-of select="concat($month, ' - PRESENT')" />
        </xsl:when>

        <xsl:when test="position() != last()">
          <xsl:value-of select="concat($month, ' - ', $nextMonth)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="concat('PRIOR TO ', $month)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="year">
      <xsl:value-of select="substring(prd:StartDate, 1, 4)" />
    </xsl:variable>

    <xsl:variable name="cautionary">
      <xsl:choose>		              
        <xsl:when test="prd:FilingsWithDerogatoryCollateral and number(prd:FilingsWithDerogatoryCollateral) != 0">		    		   		   
          <xsl:value-of select="number(prd:FilingsWithDerogatoryCollateral)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="total">
      <xsl:choose>		              
        <xsl:when test="prd:FilingsTotal and number(prd:FilingsTotal) != 0">		    		   		   
          <xsl:value-of select="number(prd:FilingsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="released">
      <xsl:choose>		              
        <xsl:when test="prd:ReleasesAndTerminationsTotal and number(prd:ReleasesAndTerminationsTotal) != 0">		    		   		   
          <xsl:value-of select="number(prd:ReleasesAndTerminationsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="continuous">
      <xsl:choose>		              
        <xsl:when test="prd:ContinuationsTotal and number(prd:ContinuationsTotal) != 0">		    		   		   
          <xsl:value-of select="number(prd:ContinuationsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="amended">
      <xsl:choose>		              
        <xsl:when test="prd:AmendedAndAssignedTotal and number(prd:AmendedAndAssignedTotal) != 0">		    		   		   
          <xsl:value-of select="number(prd:AmendedAndAssignedTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="name() = 'MostRecent6Months' or position() mod 2 = 0">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="75%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$dateRange" /></font>
            </td>
            <td width="25%">
            </td>
          </tr>
        </table>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$year" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$cautionary" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$total" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$released" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$continuous" /></font>
      </td>

      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$amended" /></font>
      </td>

    </tr>
  </xsl:template>


  <!--
  *********************************************
  * UCCFilingSection template
  *********************************************
  -->
  <xsl:template name="UCCFilingSection">
    
    <xsl:variable name="max">
      <xsl:choose>		              
        <xsl:when test="count(prd:UCCFilings) &gt; 10">		    		   		   
          <xsl:value-of select="10" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="count(prd:UCCFilings)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

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
                      <a name="uccfilings"><b><font color="#ffffff">UCC Filings</font></b></a></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>
      
                  <!-- row of UCC filings -->
                  <xsl:apply-templates select="prd:UCCFilings[position() &lt;= number($max)]">
                    <xsl:sort order="descending" select="prd:DateFiled" />
                  </xsl:apply-templates>
                  
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
  * UCCFilings template
  *********************************************
  -->
  <xsl:template match="prd:UCCFilings" xml:space="preserve">

    <xsl:variable name="legalAction">
      <xsl:choose>		              
        <xsl:when test="prd:LegalAction">		    		   		   
          <xsl:value-of select="prd:LegalAction" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateFiled">
      <xsl:choose>		              
        <xsl:when test="prd:DateFiled">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:DateFiled" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="fileNumber">
      <xsl:choose>		              
        <xsl:when test="prd:DocumentNumber">		    		   		   
          <xsl:value-of select="prd:DocumentNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalDate">
      <xsl:choose>		              
        <xsl:when test="prd:OriginalUCCFilingsInfo/prd:DateFiled">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:OriginalUCCFilingsInfo/prd:DateFiled" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalNumber">
      <xsl:choose>		              
        <xsl:when test="prd:OriginalUCCFilingsInfo/prd:DocumentNumber">		    		   		   
          <xsl:value-of select="prd:OriginalUCCFilingsInfo/prd:DocumentNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="originalState">
      <xsl:choose>		              
        <xsl:when test="prd:OriginalUCCFilingsInfo/prd:FilingState">		    		   		   
          <xsl:value-of select="prd:OriginalUCCFilingsInfo/prd:FilingState" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="jurisdiction">
      <xsl:choose>		              
        <xsl:when test="prd:FilingLocation">		    		   		   
          <xsl:value-of select="prd:FilingLocation" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="securedParty">
      <xsl:choose>		              
        <xsl:when test="prd:SecuredParty">		    		   		   
          <xsl:value-of select="prd:SecuredParty" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="collateral">
      <xsl:choose>		              
        <xsl:when test="prd:CollateralCodes/prd:Collateral[normalize-space(@code) != '']">		    		   		   
          <xsl:call-template name="JoinNodeset">
            <xsl:with-param name="nodeset" select="prd:CollateralCodes/prd:Collateral[normalize-space(@code) != '']" />
            <xsl:with-param name="order" select="'ascending'" />
            <xsl:with-param name="delimiter" select="', '" />
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
              <b>UCC <xsl:value-of select="$legalAction" /> 
              Date: </b><xsl:value-of select="$dateFiled" /></font>
            </td>
          </tr>
        
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Filing Number: </b><xsl:value-of select="$fileNumber" /></font>
            </td>
          </tr>
        
          <xsl:if test="normalize-space($originalDate) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Original Filing Date: </b><xsl:value-of select="$originalDate" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($originalNumber) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Original Filing Number: </b><xsl:value-of select="$originalNumber" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <xsl:if test="normalize-space($originalState) != ''">
            <tr>
              <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                <b>Original Filing State: </b><xsl:value-of select="$originalState" /></font>
              </td>
            </tr>
          </xsl:if>
          
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Jurisdiction: </b><xsl:value-of select="$jurisdiction" /></font>
            </td>
          </tr>
        
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Secured Party: </b><xsl:value-of select="$securedParty" /></font>
            </td>
          </tr>
        
          <tr>
            <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
              <b>Collateral: </b><xsl:value-of select="$collateral" /></font>
            </td>
          </tr>
        </table>
      </td>
    </tr>
        
  </xsl:template>
    
</xsl:stylesheet>