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
  * Payment Experiences template
  *********************************************
  -->
  <xsl:template name="PublicRecord">
	<xsl:if test="prd:Bankruptcy | prd:TaxLien | prd:JudgmentOrAttachmentLien">
	<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th colspan="6"><a name="Public Record"><a class="report_section_title">Legal Filings</a></a></th>
			</tr>
		</thead>
		<xsl:call-template name="PublicRecordBankruptcy"/>
		<xsl:call-template name="PublicRecordTaxLien"/>
		<xsl:call-template name="PublicRecordJudgmentOrAttachmentLien"/>
	</table>
	<xsl:call-template name="BackToTop" />
	</xsl:if>
  </xsl:template>

  <xsl:template name="PublicRecordBankruptcy">
	<xsl:if test="prd:Bankruptcy">
			<tr class="subtitle">
				<th colspan="6">Bankruptcy</th>
			</tr>
			<tr class="datahead">
				<td>File Date</td>
				<td>Filing Type</td>
				<td colspan="2">Status</td>
				<td>Filing Number</td>
				<td>Jurisdiction</td>
			</tr>
			<xsl:for-each select="prd:Bankruptcy">
				<xsl:sort order="descending" select="prd:DateFiled" />
				<xsl:variable name="FilingType">
					<xsl:choose>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='12'">Chapter 7 - Involuntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='13'">Chapter 7 - Voluntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='14'">Chapter 7</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='15'">Chapter 11 - Involuntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='16'">Chapter 11 - Voluntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='17'">Chapter 11</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='18'"></xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='19'"></xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='20'">Chapter 13</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='22'">Chapter 7</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='23'">Chapter 11</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='24'">Chapter 7</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='25'">Chapter 11</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='26'">Chapter 13</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='27'">Chapter 13</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='32'">Chapter 10</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='33'">Chapter 10</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='34'">Chapter 10</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='40'">Chapter 9</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='41'">Chapter 9 - Involuntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='42'">Chapter 9 - Voluntary</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='44'">Chapter 7</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='45'">Chapter 7</xsl:when>
						<xsl:otherwise><xsl:call-template name="nbsp"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="StatusText">
					<xsl:choose>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='12'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='13'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='14'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='15'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='16'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='17'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='18'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='19'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='20'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='22'">Dismissed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='23'">Dismissed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='24'">Discharged</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='25'">Discharged</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='26'">Completed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='27'">Dismissed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='32'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='33'">Dismissed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='34'">Discharged</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='40'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='41'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='42'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='44'">Dismissed</xsl:when>
						<xsl:when test="prd:LegalType/@code='01' and prd:LegalAction/@code='45'">Discharged</xsl:when>
						<xsl:otherwise><xsl:call-template name="nbsp"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
			<tr>
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2=1"><xsl:value-of select="'even'"></xsl:value-of></xsl:when>
						<xsl:when test="position() mod 2=0"><xsl:value-of select="'odd'"></xsl:value-of></xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<td>
	    		   <xsl:call-template name="FormatDate">
	    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
	    		     <xsl:with-param name="value" select="prd:DateFiled" />
	    		   </xsl:call-template>
				</td>
				<td><xsl:value-of select="$FilingType"></xsl:value-of></td>
				<td colspan="2"><xsl:value-of select="$StatusText"></xsl:value-of></td>
				<td><xsl:value-of select="normalize-space(prd:DocumentNumber)"></xsl:value-of></td>
				<td><xsl:value-of select="normalize-space(prd:FilingLocation)"></xsl:value-of></td>
			</tr>
			</xsl:for-each>
			<xsl:comment>Leave an empty line</xsl:comment>
			<tr><td></td></tr>
	</xsl:if>
  </xsl:template>

  <xsl:template name="PublicRecordTaxLien">
	<xsl:if test="prd:TaxLien">
			<tr class="subtitle">
				<th colspan="7">Tax Liens</th>
			</tr>
			<tr class="datahead">
				<td>File Date</td>
				<td>Filing Type</td>
				<td>Status</td>
				<td>Amount</td>
				<!--<td>Filed by</td>-->
				<td>Filing Number</td>
				<td>Jurisdiction</td>
			</tr>

			<xsl:for-each select="prd:TaxLien">
			<xsl:sort order="descending" select="prd:DateFiled" />
			<tr>
				<xsl:if test="not(prd:Owner)">
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2=1"><xsl:value-of select="'even'"></xsl:value-of></xsl:when>
						<xsl:when test="position() mod 2=0"><xsl:value-of select="'odd'"></xsl:value-of></xsl:when>
					</xsl:choose>
				</xsl:attribute>
				</xsl:if>
				<xsl:variable name="FilingType">
					<xsl:choose>
						<xsl:when test="prd:LegalType/@code='02' and prd:LegalAction/@code='06'">Federal Tax Lien</xsl:when>
						<xsl:when test="prd:LegalType/@code='02' and prd:LegalAction/@code='07'">Federal Tax Lien</xsl:when>
						<xsl:when test="prd:LegalType/@code='03' and prd:LegalAction/@code='06'">State Tax Lien</xsl:when>
						<xsl:when test="prd:LegalType/@code='03' and prd:LegalAction/@code='07'">State Tax Lien</xsl:when>
						<xsl:when test="prd:LegalType/@code='04' and prd:LegalAction/@code='06'">County Tax Lien</xsl:when>
						<xsl:when test="prd:LegalType/@code='04' and prd:LegalAction/@code='07'">County Tax Lien</xsl:when>
						<xsl:otherwise><xsl:call-template name="nbsp"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<xsl:variable name="StatusText">
					<xsl:choose>
						<xsl:when test="prd:LegalType/@code='02' and prd:LegalAction/@code='06'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='02' and prd:LegalAction/@code='07'">Released</xsl:when>
						<xsl:when test="prd:LegalType/@code='03' and prd:LegalAction/@code='06'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='03' and prd:LegalAction/@code='07'">Released</xsl:when>
						<xsl:when test="prd:LegalType/@code='04' and prd:LegalAction/@code='06'">Filed</xsl:when>
						<xsl:when test="prd:LegalType/@code='04' and prd:LegalAction/@code='07'">Released</xsl:when>
						<xsl:otherwise><xsl:call-template name="nbsp"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<td>
	    		   <xsl:call-template name="FormatDate">
	    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
	    		     <xsl:with-param name="value" select="prd:DateFiled" />
	    		   </xsl:call-template>
				</td>
				<td><xsl:value-of select="$FilingType"></xsl:value-of></td>
				<td><xsl:value-of select="$StatusText"></xsl:value-of></td>
				<td>
					<xsl:choose>
					  <xsl:when test="prd:LiabilityAmount">
					    <xsl:value-of select="format-number(prd:LiabilityAmount, '$###,###,##0')" />
					  </xsl:when>
					  <xsl:otherwise>
					    <xsl:value-of select="'N/A'" />
					  </xsl:otherwise>
					</xsl:choose>
				</td>
				<!--<td><xsl:value-of select="normalize-space(prd:Owner)"></xsl:value-of></td>-->
				<td><xsl:value-of select="normalize-space(prd:DocumentNumber)"></xsl:value-of></td>
				<td><xsl:value-of select="normalize-space(prd:FilingLocation)"></xsl:value-of></td>
			</tr>
			<xsl:if test="prd:Owner">
			<tr>
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2=1"><xsl:value-of select="'even'"></xsl:value-of></xsl:when>
						<xsl:when test="position() mod 2=0"><xsl:value-of select="'odd'"></xsl:value-of></xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<td><xsl:call-template name="nbsp"/></td><td colspan="5"><span class="label"><xsl:value-of select="'Filed by: '"/></span><xsl:value-of select="normalize-space(prd:Owner)"></xsl:value-of></td>
			</tr>
			</xsl:if>
			</xsl:for-each>
			<xsl:comment>Leave an empty line</xsl:comment>
			<tr><td></td></tr>
	</xsl:if>
  </xsl:template>

  <xsl:template name="PublicRecordJudgmentOrAttachmentLien">
	<xsl:if test="prd:JudgmentOrAttachmentLien">
			<tr class="subtitle">
				<th colspan="6">Judgments</th>
			</tr>
			<tr class="datahead">
				<td>File Date</td>
				<td>Plaintiff</td>
				<td>Status</td>
				<td>Amount</td>
				<td>Filing Number</td>
				<td>Jurisdiction</td>
			</tr>

			<xsl:for-each select="prd:JudgmentOrAttachmentLien">
			<xsl:sort order="descending" select="prd:DateFiled" />
			<tr>
				<xsl:attribute name="class">
					<xsl:choose>
						<xsl:when test="position() mod 2=1"><xsl:value-of select="'even'"></xsl:value-of></xsl:when>
						<xsl:when test="position() mod 2=0"><xsl:value-of select="'odd'"></xsl:value-of></xsl:when>
					</xsl:choose>
				</xsl:attribute>
				<xsl:variable name="StatusText">
					<xsl:choose>
						<xsl:when test="prd:LegalAction/@code='01'">Filed</xsl:when>
						<xsl:when test="prd:LegalAction/@code='07'">Released</xsl:when>
						<xsl:when test="prd:LegalAction/@code='10'">Satisfied</xsl:when>
						<xsl:when test="prd:LegalAction/@code='11'">Abstract</xsl:when>
						<xsl:otherwise><xsl:call-template name="nbsp"/></xsl:otherwise>
					</xsl:choose>
				</xsl:variable>
				<td>
					<xsl:call-template name="FormatDate">
					  <xsl:with-param name="pattern" select="'mo/dt/year'" />
					  <xsl:with-param name="value" select="prd:DateFiled" />
					</xsl:call-template>
				</td>
				<td><xsl:value-of select="normalize-space(prd:PlaintiffName)"></xsl:value-of></td>
				<td><xsl:value-of select="$StatusText"></xsl:value-of></td>
				<td>
					<xsl:choose>
					  <xsl:when test="prd:LiabilityAmount">
					    <xsl:value-of select="format-number(prd:LiabilityAmount, '$###,###,##0')" />
					  </xsl:when>
					  <xsl:otherwise>
					    <xsl:value-of select="'N/A'" />
					  </xsl:otherwise>
					</xsl:choose>
				</td>
				<td><xsl:value-of select="normalize-space(prd:DocumentNumber)"></xsl:value-of></td>
				<td><xsl:value-of select="normalize-space(prd:FilingLocation)"></xsl:value-of></td>
			</tr>
			</xsl:for-each>
	</xsl:if>
  </xsl:template>
</xsl:stylesheet>