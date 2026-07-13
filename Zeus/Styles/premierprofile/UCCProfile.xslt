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
  * UCCProfile template
  *********************************************
  -->
  <xsl:template name="UCCProfile">
    <!-- Section title -->
  	<xsl:if test="prd:UCCFilings">
	<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th colspan="7"><a name="UCC Filing"><a class="report_section_title">Uniform Commercial Code (UCC) Filings</a></a></th>
			</tr>
		</thead>

	  	<xsl:if test="prd:UCCFilingsSummaryCounts">
		<thead>
			<tr class="subtitle">
				<th colspan="7">UCC Filing Summary</th>
			</tr>
			<tr class="datahead">
				<td>Date Range</td>
				<td>Year</td>
				<td>Cautionary<br />UCCs<sup>**</sup></td>
				<td>Total<br/>Filed</td>
				<td>Released /<br />Termination</td>
				<td>Continuous</td>
				<td>Amended /<br />Assigned</td>
			</tr>
		</thead>
		<tbody>
	    	<!-- UCCFilingsSummaryCounts -->
	    	<xsl:apply-templates select="prd:UCCFilingsSummaryCounts" />
	    </tbody>
	    </xsl:if>

	    <xsl:if test="prd:UCCFilings">
		<tbody>
	      <xsl:call-template name="UCCFilingSection" />
	    </tbody>
	    </xsl:if>
	</table>
    <xsl:call-template name="BackToTop" />
	</xsl:if>
  </xsl:template>


  <!--
  *********************************************
  * UCCFilingsSummaryCounts template
  *********************************************
  -->
  <xsl:template match="prd:UCCFilingsSummaryCounts" xml:space="preserve">

                  <!-- MostRecent6Months -->
                  <xsl:apply-templates select="prd:MostRecent6Months" >
                  	<xsl:with-param name="previousRowCount" select="0"/>
                  </xsl:apply-templates>

                  <!-- Previous6Months -->
                  <xsl:apply-templates select="prd:Previous6Months" >
                  	<xsl:with-param name="previousRowCount" select="count(prd:MostRecent6Months)"/>
                  </xsl:apply-templates>

                  <!-- total line -->
                  <tr class="summary">
                    <td>Total</td>
                    <td><xsl:call-template name="nbsp"/></td>
                    <td><xsl:value-of select="sum(.//prd:FilingsWithDerogatoryCollateral)" /></td>
                    <td><xsl:value-of select="sum(.//prd:FilingsTotal)" /></td>
                    <td><xsl:value-of select="sum(.//prd:ReleasesAndTerminationsTotal)" /></td>
                    <td><xsl:value-of select="sum(.//prd:ContinuationsTotal)" /></td>
                    <td><xsl:value-of select="sum(.//prd:AmendedAndAssignedTotal)" /></td>
                  </tr>

      <tr>
        <td colspan="7">
            <p style="font-size:9px;padding:5px 0">** Cautionary UCC Filings include one or more of the following collateral:<br/>
            Accounts, Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, Notes Receivable or Proceeds.</p>
        </td>
      </tr>

  </xsl:template>


  <!--
  ************************************************************
  * MostRecent6Months | Previous6Months template
  ************************************************************
  -->
  <xsl:template match="prd:MostRecent6Months | prd:Previous6Months">
  	<xsl:param name="previousRowCount"/>
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
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="total">
      <xsl:choose>
        <xsl:when test="prd:FilingsTotal and number(prd:FilingsTotal) != 0">
          <xsl:value-of select="number(prd:FilingsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="released">
      <xsl:choose>
        <xsl:when test="prd:ReleasesAndTerminationsTotal and number(prd:ReleasesAndTerminationsTotal) != 0">
          <xsl:value-of select="number(prd:ReleasesAndTerminationsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="continuous">
      <xsl:choose>
        <xsl:when test="prd:ContinuationsTotal and number(prd:ContinuationsTotal) != 0">
          <xsl:value-of select="number(prd:ContinuationsTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="amended">
      <xsl:choose>
        <xsl:when test="prd:AmendedAndAssignedTotal and number(prd:AmendedAndAssignedTotal) != 0">
          <xsl:value-of select="number(prd:AmendedAndAssignedTotal)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <tr>
		<xsl:attribute name="class">
			<xsl:choose>
				<xsl:when test="(position()+$previousRowCount) mod 2=1"><xsl:value-of select="'even'"></xsl:value-of></xsl:when>
				<xsl:when test="(position()+$previousRowCount) mod 2=0"><xsl:value-of select="'odd'"></xsl:value-of></xsl:when>
			</xsl:choose>
		</xsl:attribute>
      <td><xsl:value-of select="$dateRange" disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$year"  disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$cautionary"  disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$total"  disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$released"  disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$continuous"  disable-output-escaping="yes"/></td>
      <td><xsl:value-of select="$amended"  disable-output-escaping="yes"/></td>
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


    <xsl:variable name="UCCCount">
      <xsl:value-of select="count(prd:UCCFilings)" />
    </xsl:variable>

    <tr><td colspan="7" style="padding:0"><table width="100%" cellspacing="0" cellpadding="0" style="width:100%">
      <tr class="subtitle">
      	<th>UCC Details</th>
      </tr>

      <tr>
        <td>
          <div style="clear: both; height: 5px;"><xsl:call-template name="nbsp"/></div>

          <!-- row of UCC filings -->
          <xsl:apply-templates select="prd:UCCFilings" mode="rowlevel">
            <xsl:sort order="descending" select="prd:DateFiled" />
            <xsl:with-param name="start" select="1"/>
            <xsl:with-param name="end" select="$max"/>
          </xsl:apply-templates>
        </td>
      </tr>

      <xsl:if test="($UCCCount) &gt; $max">
        <xsl:call-template name="UCCFilingSectionGTTen" />
      </xsl:if>
    </table></td></tr>
  </xsl:template>






  <!--
    *********************************************
    * UCCFilingSectionGTTen template
    *********************************************
    -->
    <xsl:template name="UCCFilingSectionGTTen">

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
        <!--<tr>
          <td>
            <img src="../images/reports/spacer.gif" width="0" height="3" alt=""/></td>
        </tr>-->
        <tr class="hidden_on_print">
          <td style="padding-left: 13px;">
          	<a href="#addlUcc"><span id="toggleUCC"><img src="../images/reports/minus.gif" border="0" width="11" height="11"/></span></a>
            <b class="indent1" style="padding-left: 5px;">View Additional UCC Details</b>
          </td>
        </tr>

		<tr><td style="width:100%;padding:0">
      	<div id="UCCFilingSectionGTTen" class="hidden_on_screen show_on_print" style="width:100%;padding:0">

	      <table width="100%" border="0" cellspacing="0" cellpadding="0" style="width:100%">
            <tr class="subtitle">
              <th align="left" valign="middle" height="23">
                <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                <a name="uccfilings">UCC Details (Continued)</a></th>
            </tr>

            <tr>
              <td>
                <div style="clear: both; height: 5px;"><xsl:call-template name="nbsp"/></div>
                <!-- row of UCC filings -->
                <xsl:apply-templates select="prd:UCCFilings" mode="rowlevel">
                  <xsl:sort order="descending" select="prd:DateFiled" />
                  <xsl:with-param name="start" select="number($max) + 1"/>
                  <xsl:with-param name="end" select="count(prd:UCCFilings)"/>
                </xsl:apply-templates>
              </td>
            </tr>

	      </table>
      </div>
      </td></tr>
    </xsl:template>




  <!--
  *********************************************
  * UCCFilings template
  *********************************************
  -->
  <xsl:template match="prd:UCCFilings" xml:space="preserve" mode="rowlevel">
    <xsl:param name="start" select="1" />
    <xsl:param name="end" select="999999999"/>

    <xsl:if test="position() &gt;= $start and position() &lt;= $end">
      <xsl:variable name="float">
        <xsl:choose>
          <xsl:when test="position() mod 2 = 0">
            <xsl:value-of select="'float:right;'" />
          </xsl:when>

          <xsl:otherwise>
            <xsl:value-of select="'float:left;'" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:variable>

      <xsl:apply-templates select="." mode="columnlevel">
        <xsl:with-param name="float" select="$float"/>
      </xsl:apply-templates>

      <xsl:if test="position() = $end and $end mod 2 = 1">
        <div style="vertical-align:top; width:48%; float:right;"><xsl:call-template name="nbsp"/></div>
        <div style="clear:both;"><xsl:call-template name="nbsp"/></div>
      </xsl:if>

      <xsl:if test="position() mod 2 = 0">
        <div style="clear:both;"><xsl:call-template name="nbsp"/></div>
      </xsl:if>

    </xsl:if>
  </xsl:template>

  <xsl:template match="prd:UCCFilings" xml:space="preserve" mode="columnlevel">
    <xsl:param name="float" select="''" />

    <xsl:variable name="legalAction">
      <xsl:choose>
        <xsl:when test="prd:LegalAction">
          <xsl:value-of select="prd:LegalAction" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
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
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="fileNumber">
      <xsl:choose>
        <xsl:when test="prd:DocumentNumber">
          <xsl:value-of select="prd:DocumentNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="originalDate">
      <xsl:choose>
        <xsl:when test="prd:OriginalUCCFilingsInfo/prd:DateFiled and normalize-space(prd:OriginalUCCFilingsInfo/prd:DateFiled)!=''">
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
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="securedParty">
      <xsl:choose>
        <xsl:when test="prd:SecuredParty">
          <xsl:value-of select="prd:SecuredParty" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
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

    <xsl:variable name="stateStatement">
      <xsl:choose>
        <xsl:when test="contains(normalize-space($jurisdiction), 'SEC OF STATE N CAROL')">
          <xsl:value-of select="'THIS DATA IS FOR INFORMATION PURPOSES ONLY. CERTIFICATION CAN ONLY BE OBTAINED THROUGH THE NORTH CAROLINA DEPARTMENT OF THE SECRETARY OF STATE.'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

      <div style="vertical-align:top; width:48%; {$float}">
        <table width="100%" border="0" cellspacing="0" cellpadding="1">

          <xsl:if test="position() &gt; 1">
            <tr>
              <td>
                <img src="../images/spacer.gif" width="1" height="10" alt=""/></td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($stateStatement) != ''">
            <tr>
              <td align="left" valign="middle" height="30">
                <xsl:value-of select="$stateStatement"  disable-output-escaping="yes"/>
              </td>
            </tr>
          </xsl:if>

          <tr>
            <td align="left" style="height:16px">
              <b>UCC <xsl:value-of select="$legalAction"  disable-output-escaping="yes"/>
              Date: </b><xsl:value-of select="$dateFiled"  disable-output-escaping="yes"/>
            </td>
          </tr>

          <tr>
            <td align="left" style="height:16px">
              <b>Filing Number: </b><xsl:value-of select="$fileNumber" disable-output-escaping="yes"/>
            </td>
          </tr>

          <xsl:if test="normalize-space($originalDate) != ''">
            <tr>
              <td align="left" style="height:16px">
                <b>Original Filing Date: </b><xsl:value-of select="$originalDate"  disable-output-escaping="yes"/>
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($originalNumber) != ''">
            <tr>
              <td align="left" style="height:16px">
                <b>Original Filing Number: </b><xsl:value-of select="$originalNumber"  disable-output-escaping="yes"/>
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="normalize-space($originalState) != ''">
            <tr>
              <td align="left" style="height:16px">
                <b>Original Filing State: </b><xsl:value-of select="$originalState"  disable-output-escaping="yes"/>
              </td>
            </tr>
          </xsl:if>

          <tr>
            <td align="left" style="height:16px">
              <b>Jurisdiction: </b><xsl:value-of select="$jurisdiction"  disable-output-escaping="yes"/>
            </td>
          </tr>

          <tr>
            <td align="left" style="height:16px">
              <b>Secured Party: </b><xsl:value-of select="$securedParty"  disable-output-escaping="yes"/>
            </td>
          </tr>

          <xsl:if test="normalize-space($collateral) != ''">
          <tr>
            <td align="left" style="height:16px">
              <b>Collateral: </b><xsl:value-of select="$collateral"  disable-output-escaping="yes"/>
            </td>
          </tr>
          </xsl:if>
        </table>
      </div>

  </xsl:template>

</xsl:stylesheet>




