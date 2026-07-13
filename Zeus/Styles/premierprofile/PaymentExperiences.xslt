<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:prd="http://www.experian.com/ARFResponse">
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
	<xsl:template name="PaymentExperiences">
		<xsl:if test="prd:CollectionData | prd:PaymentTotals | prd:TradePaymentExperiences | prd:AdditionalPaymentExperiences">
			<!--<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th><a class="report_section_title" name="Payment Experiences">Payment Experiences</a></th>
			</tr>
		</thead>
		<tbody>
		-->
			<xsl:if test="prd:CollectionData">
				<xsl:call-template name="PaymentExperiencesCollections"/>
				<xsl:call-template name="BackToTop"/>
				<!--<xsl:comment>Leave an empty line to separate next section</xsl:comment>
			<tr><td></td></tr>
		-->
			</xsl:if>
			<xsl:call-template name="PaymentExperiencesPaymentTotals"/>
			<!--</tbody>
	</table>
	-->
			<xsl:call-template name="BackToTop"/>
		</xsl:if>
		<xsl:call-template name="PaymentExperiencesTradePaymentExperiences"/>
		<xsl:call-template name="PaymentExperiencesAdditionalPaymentExperiences"/>
		<!--<xsl:call-template name="PaymentExperiencesPaymentTrending"/>-->
	</xsl:template>
	<xsl:template name="PaymentExperiencesCollections">
		<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
			<thead>
				<tr>
					<th colspan="7">
						<a name="TradeCollections">
							<a class="report_section_title">Collection Experiences</a>
						</a>
					</th>
				</tr>
			</thead>
			<tbody>
				<!-- <tr><td style="padding:0"><table cellspacing="0" cellpadding="0" style="width:100%"> -->
				<!--<tr class="subtitle">
				<th colspan="7">Collections</th>
			</tr>-->
				<tr class="datahead">
					<td>Date Placed</td>
					<td style="width:80px">Status</td>
					<td>Original Balance</td>
					<td>Outstanding Balance</td>
					<td>Date Closed</td>
					<td>Agency</td>
					<td style="width:95px">Agency Phone</td>
				</tr>
				<xsl:for-each select="prd:CollectionData">
					<xsl:sort order="descending" select="prd:DatePlacedForCollection"/>
					<xsl:variable name="rowStatus">
						<xsl:if test="position() mod 2=1">
							<xsl:value-of select="'even'"/>
						</xsl:if>
						<xsl:if test="position() mod 2=0">
							<xsl:value-of select="'odd'"/>
						</xsl:if>
					</xsl:variable>
					<xsl:variable name="DatePlacedForCollection">
						<xsl:choose>
							<xsl:when test="prd:DatePlacedForCollection and number(prd:DatePlacedForCollection) != 0">
								<xsl:call-template name="FormatDate">
									<xsl:with-param name="pattern" select="'mo/dt/year'"/>
									<xsl:with-param name="value" select="prd:DatePlacedForCollection"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="nbsp"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="DateClosed">
						<xsl:choose>
							<xsl:when test="prd:DateClosed and number(prd:DateClosed) != 0">
								<xsl:call-template name="FormatDate">
									<xsl:with-param name="pattern" select="'mo/dt/year'"/>
									<xsl:with-param name="value" select="prd:DateClosed"/>
								</xsl:call-template>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'-'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="agencyname">
						<xsl:choose>
							<xsl:when test="prd:CollectionAgencyInfo/prd:AgencyName">
								<xsl:value-of select="normalize-space(prd:CollectionAgencyInfo/prd:AgencyName)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="nbsp"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="phone">
						<xsl:choose>
							<xsl:when test="prd:CollectionAgencyInfo/prd:PhoneNumber and string-length(normalize-space(prd:CollectionAgencyInfo/prd:PhoneNumber)) = 10">
								<xsl:choose>
									<xsl:when test="normalize-space(prd:CollectionAgencyInfo/prd:PhoneNumber)!='0000000000'">
										<xsl:call-template name="FormatPhone">
											<xsl:with-param name="value" select="prd:CollectionAgencyInfo/prd:PhoneNumber"/>
										</xsl:call-template>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:call-template name="nbsp"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<xsl:variable name="StatusText">
						<xsl:choose>
							<xsl:when test="prd:AccountStatus/@code='00'">Open</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='01'">Open</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='02'">Open</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='03'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='04'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='05'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='06'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='07'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='08'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='09'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='10'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='11'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code='12'">Closed</xsl:when>
							<xsl:when test="prd:AccountStatus/@code=''"/>
						</xsl:choose>
					</xsl:variable>
					<tr>
						<xsl:if test="not(prd:ClientOfCollectionAgency/prd:ClientName and normalize-space(prd:ClientOfCollectionAgency/prd:ClientName)!='')">
							<xsl:attribute name="class"><xsl:value-of select="$rowStatus"/></xsl:attribute>
						</xsl:if>
						<td>
							<xsl:value-of select="$DatePlacedForCollection"/>
						</td>
						<td>
							<xsl:value-of select="normalize-space(prd:AccountStatus)"/>
						</td>
						<td>
							<xsl:value-of select="format-number(number(prd:AmountPlacedForCollection),'$###,###,##0')"/>
						</td>
						<td>
							<xsl:value-of select="format-number(number(prd:AmountPlacedForCollection)-number(prd:AmountPaid),'$###,###,##0')"/>
						</td>
						<td>
							<xsl:value-of select="$DateClosed"/>
						</td>
						<td>
							<xsl:value-of disable-output-escaping="yes" select="$agencyname"/>
						</td>
						<td>
							<xsl:value-of disable-output-escaping="yes" select="$phone"/>
						</td>
					</tr>
					<xsl:if test="prd:ClientOfCollectionAgency/prd:ClientName and normalize-space(prd:ClientOfCollectionAgency/prd:ClientName)!=''">
						<tr>
							<xsl:attribute name="class"><xsl:value-of select="$rowStatus"/></xsl:attribute>
							<td colspan="7">
								<div style="text-align:left">
									<span class="indent1">
										<b>Original Credit Grantor: </b>
										<xsl:value-of select="normalize-space(prd:ClientOfCollectionAgency/prd:ClientName)"/>
									</span>
									<!--<xsl:if test="prd:ClientOfCollectionAgency/prd:ContactName and normalize-space(prd:ClientOfCollectionAgency/prd:ContactName)!=''">
				<span class="indent1"><b>Contact: </b><xsl:value-of select="normalize-space(prd:ClientOfCollectionAgency/prd:ContactName)"></xsl:value-of></span>
				</xsl:if>-->
									<!--<xsl:choose>
			        <xsl:when test="prd:ClientOfCollectionAgency/prd:PhoneNumber and normalize-space(prd:ClientOfCollectionAgency/prd:PhoneNumber)!=''">
						<span class="indent1"><b>Phone: </b><xsl:call-template name="FormatPhone"><xsl:with-param name="value" select="normalize-space(prd:ClientOfCollectionAgency/prd:PhoneNumber)" /></xsl:call-template></span>
			        </xsl:when>
			        <xsl:otherwise>
			          <xsl:call-template name="nbsp"/>
			        </xsl:otherwise>
			    </xsl:choose>-->
								</div>
							</td>
						</tr>
					</xsl:if>
				</xsl:for-each>
			</tbody>
		</table>
	</xsl:template>
	<xsl:template name="PaymentExperiencesPaymentTotals">
		<xsl:if test="prd:PaymentTotals">
			<xsl:variable name="ContinouslyReportedTradeLines_NumberOfLines">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines and string(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines))!='NaN'">
						<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines),'###,###,##0')"/>
						<!--<xsl:choose>
        		<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines)=0">
        			<xsl:call-template name="nbsp"/>
        		</xsl:when>
        		<xsl:otherwise>
        			<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:NumberOfLines),'###,###,##0')"/>
        		</xsl:otherwise>
        	</xsl:choose>-->
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_DBT">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT),'###,###,##0')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_TotalHighCreditAmount">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalHighCreditAmount">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalHighCreditAmount/prd:Amount)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalHighCreditAmount/prd:Modifier/@code),
        				format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalHighCreditAmount/prd:Amount),'$###,###,###,##0'))"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_Balance">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalAccountBalance">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalAccountBalance/prd:Amount)=0">
								<xsl:value-of select="'$0'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalAccountBalance/prd:Modifier/@code),
        				format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:TotalAccountBalance/prd:Amount),'$###,###,###,##0'))"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'$0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_CurrentPercentage">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:CurrentPercentage">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:CurrentPercentage)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:CurrentPercentage) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_DBT30">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT30">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT30)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT30) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_DBT60">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT60">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT60)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT60) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_DBT90">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT90">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT90)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT90) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="ContinouslyReportedTradeLines_DBT90Plus">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120 and prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT121Plus">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT121Plus)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT121Plus)) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:ContinouslyReportedTradeLines/prd:DBT120)) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_NumberOfLines">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines and string(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines))">
						<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines),'###,###,##0')"/>
						<!--<xsl:choose>
        		<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines)=0">
        			<xsl:call-template name="nbsp"/>
        		</xsl:when>
        		<xsl:otherwise>
        			<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:NumberOfLines),'###,###,##0')"/>
        		</xsl:otherwise>
        	</xsl:choose>-->
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_DBT">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT),'###,###,##0')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_TotalHighCreditAmount">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalHighCreditAmount">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalHighCreditAmount/prd:Amount)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalHighCreditAmount/prd:Modifier/@code),
        				format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalHighCreditAmount/prd:Amount),'$###,###,###,##0'))"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_Balance">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalAccountBalance">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalAccountBalance/prd:Amount)=0">
								<xsl:value-of select="'$0'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalAccountBalance/prd:Modifier/@code),
        				format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:TotalAccountBalance/prd:Amount),'$###,###,###,##0'))"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="'$0'"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_CurrentPercentage">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:CurrentPercentage">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:CurrentPercentage)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:CurrentPercentage) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_DBT30">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT30">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT30)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT30) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_DBT60">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT60">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT60)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT60) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_DBT90">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT90">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT90)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT90) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<xsl:variable name="NewlyReportedTradeLines_DBT90Plus">
				<xsl:choose>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120 and prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT121Plus">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT121Plus)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT121Plus)) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:when test="prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120">
						<xsl:choose>
							<xsl:when test="number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120)=0">
								<xsl:call-template name="nbsp"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:NewlyReportedTradeLines/prd:DBT120)) div 100,'##0%')"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:call-template name="nbsp"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>
			<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
				<thead>
					<tr>
						<th colspan="10">
							<a class="report_section_title" name="Payment Experiences">Trade Payment Summary</a>
						</th>
					</tr>
				</thead>
				<tbody>
					<!--<tr><td style="padding:0"><table cellspacing="0" cellpadding="0" style="width:100%">
			<tr class="subtitle">
				<th colspan="9">Trade Payment Summary</th>
			</tr>-->
					<tr class="datahead">
						<td>Trade Line Type</td>
						<td>Lines Reported</td>
						<td>DBT</td>
						<td>Recent High Credit</td>
						<td>Balance</td>
						<td>Current</td>
						<td style="width:33px">01-30</td>
						<td style="width:33px">31-60</td>
						<td style="width:33px">61-90</td>
						<td>91+</td>
					</tr>
					<tr class="even">
						<td>Continuous</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_NumberOfLines" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_DBT" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_TotalHighCreditAmount" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_Balance" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_CurrentPercentage" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_DBT30" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_DBT60" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_DBT90" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$ContinouslyReportedTradeLines_DBT90Plus" disable-output-escaping="yes"/>
						</td>
						<!-- @TODO no DBT91+ in schema -->
					</tr>
					<tr>
						<td>New</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_NumberOfLines" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_DBT" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_TotalHighCreditAmount" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_Balance" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_CurrentPercentage" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_DBT30" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_DBT60" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_DBT90" disable-output-escaping="yes"/>
						</td>
						<td>
							<xsl:value-of select="$NewlyReportedTradeLines_DBT90Plus" disable-output-escaping="yes"/>
						</td>
						<!-- @TODO no DBT91+ in schema -->
					</tr>
					<tr class="summary">
						<td>Combined Trade</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines and string(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines))!='NaN'">
									<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines),'###,###,##0')"/>
									<!--<xsl:choose>
						        <xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines)=0">
						        	<xsl:call-template name="nbsp"/>
						        </xsl:when>
						        <xsl:otherwise>
									<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:NumberOfLines),'###,###,##0')"></xsl:value-of>
								</xsl:otherwise>
							</xsl:choose>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>0</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT),'###,###,##0')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalHighCreditAmount">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalHighCreditAmount/prd:Amount)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalHighCreditAmount/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalHighCreditAmount/prd:Amount),'$###,###,###,##0'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalAccountBalance">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalAccountBalance/prd:Amount)=0">
											<xsl:value-of select="'$0'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalAccountBalance/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:TotalAccountBalance/prd:Amount),'$###,###,###,##0'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>$0</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:CurrentPercentage">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:CurrentPercentage)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:CurrentPercentage) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT30">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT30)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT30) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT60">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT60)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT60) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT90">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT90)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT90) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120 and prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT121Plus">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT121Plus)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT121Plus)) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:CombinedTradeLines/prd:DBT120)) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<!-- @TODO no DBT91+ in schema -->
					</tr>
					<tr class="odd">
						<td colspan="10">
							<xsl:call-template name="nbsp"/>
						</td>
					</tr>
					<xsl:comment>Leave an empty line here</xsl:comment>
					<xsl:if test="prd:PaymentTotals/prd:AdditionalTradeLines">
						<tr>
							<td>Additional</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:NumberOfLines and string(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:NumberOfLines))!='NaN'">
										<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:NumberOfLines),'###,###,##0')"/>
										<!--<xsl:choose>
					        <xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:NumberOfLines)=0">
					        	<xsl:call-template name="nbsp"/>
					        </xsl:when>
					        <xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:NumberOfLines),'###,###,##0')"></xsl:value-of>
							</xsl:otherwise>
						</xsl:choose>-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>0</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT),'###,###,##0')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalHighCreditAmount">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalHighCreditAmount/prd:Amount)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalHighCreditAmount/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalHighCreditAmount/prd:Amount),'$###,###,###,##0'))"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalAccountBalance">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalAccountBalance/prd:Amount)=0">
												<xsl:value-of select="'$0'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalAccountBalance/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:TotalAccountBalance/prd:Amount),'$###,###,###,##0'))"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:text>$0</xsl:text>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:CurrentPercentage">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:CurrentPercentage)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:CurrentPercentage) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT30">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT30)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT30) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT60">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT60)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT60) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT90">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT90)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT90) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120 and prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT121Plus">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT121Plus)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT121Plus)) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:when test="prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120">
										<xsl:choose>
											<xsl:when test="number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120)=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:AdditionalTradeLines/prd:DBT120)) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<!-- @TODO no DBT91+ in schema -->
						</tr>
					</xsl:if>
					<tr class="summary">
						<td>Total Trade</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:NumberOfLines and string(number(prd:PaymentTotals/prd:TradeLines/prd:NumberOfLines))!='NaN'">
									<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:NumberOfLines),'###,###,##0')"/>
									<!--<xsl:choose>
					        <xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:NumberOfLines)=0">
					        	<xsl:call-template name="nbsp"/>
					        </xsl:when>
					        <xsl:otherwise>
								<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:NumberOfLines),'###,###,##0')"></xsl:value-of>
							</xsl:otherwise>
						</xsl:choose>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>0</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:DBT),'###,###,##0')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:TotalHighCreditAmount">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:TotalHighCreditAmount/prd:Amount)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:TradeLines/prd:TotalHighCreditAmount/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:TradeLines/prd:TotalHighCreditAmount/prd:Amount),'$###,###,###,##0'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:TotalAccountBalance">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:TotalAccountBalance/prd:Amount)=0">
											<xsl:value-of select="'$0'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="concat(normalize-space(prd:PaymentTotals/prd:TradeLines/prd:TotalAccountBalance/prd:Modifier/@code),
			        				format-number(number(prd:PaymentTotals/prd:TradeLines/prd:TotalAccountBalance/prd:Amount),'$###,###,###,##0'))"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:text>$0</xsl:text>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:CurrentPercentage">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:CurrentPercentage)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:CurrentPercentage) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
									<!--<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:CurrentPercentage) div 100,'##0%')"></xsl:value-of>-->
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT30">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT30)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:DBT30) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT60">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT60)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:DBT60) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT90">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT90)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number(number(prd:PaymentTotals/prd:TradeLines/prd:DBT90) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<td>
							<xsl:choose>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT120 and prd:PaymentTotals/prd:TradeLines/prd:DBT121Plus">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:TradeLines/prd:DBT121Plus)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:TradeLines/prd:DBT120)+number(prd:PaymentTotals/prd:TradeLines/prd:DBT121Plus)) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:when test="prd:PaymentTotals/prd:TradeLines/prd:DBT120">
									<xsl:choose>
										<xsl:when test="number(prd:PaymentTotals/prd:TradeLines/prd:DBT120)=0">
											<xsl:call-template name="nbsp"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="format-number((number(prd:PaymentTotals/prd:TradeLines/prd:DBT120)) div 100,'##0%')"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="nbsp"/>
								</xsl:otherwise>
							</xsl:choose>
						</td>
						<!-- @TODO no DBT91+ in schema -->
					</tr>
				</tbody>
			</table>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PaymentExperiencesTradePaymentExperiences">
		<xsl:if test="prd:TradePaymentExperiences">
			<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
				<thead>
					<tr>
						<th colspan="12">Trade Payment - New and Continuously Reported Trade Details</th>
					</tr>
					<tr class="subtitle">
						<th colspan="6">
							<div style="padding:5px 0">
								<div style="text-align:center">Payment Experiences</div>
								<div style="text-align:center">(Trade Lines with an (*) after the date are newly reported)</div>
							</div>
						</th>
						<th colspan="5">
							<div style="text-align:center">Account Status<br/>Days Beyond Terms</div>
						</th>
						<th/>
					</tr>
					<tr class="datahead">
						<td>Business Category</td>
						<td>Date Reported</td>
						<td style="width:48px">Last Sale</td>
						<td>Payment Terms</td>
						<td>Recent High Credit</td>
						<td>Balance</td>
						<td>Cur</td>
						<td style="width:33px">1-30</td>
						<td style="width:33px">31-60</td>
						<td style="width:33px">61-90</td>
						<td style="width:33px">91+</td>
						<td style="width:70px">Comments</td>
					</tr>
				</thead>
				<tbody>
					<xsl:for-each select="prd:TradePaymentExperiences">
						<tr>
							<xsl:attribute name="class"><xsl:choose><xsl:when test="position() mod 2 =1"><xsl:value-of select="'even'"/></xsl:when><xsl:when test="position() mod 2 =0"><xsl:value-of select="'odd'"/></xsl:when></xsl:choose></xsl:attribute>
							<td>
								<xsl:value-of select="prd:BusinessCategory"/>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:DateReported and number(prd:DateReported) != 0">
										<xsl:call-template name="FormatDate">
											<xsl:with-param name="pattern" select="'mo/year'"/>
											<xsl:with-param name="value" select="prd:DateReported"/>
										</xsl:call-template>
										<xsl:choose>
											<xsl:when test="prd:NewlyReportedIndicator and normalize-space(prd:NewlyReportedIndicator/@code) != '' and normalize-space(prd:NewlyReportedIndicator/@code) != 'N'">
												<xsl:value-of select="'*'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
										<!--<xsl:variable name="month">
							<xsl:call-template name="FormatMonth">
								<xsl:with-param name="monthValue"
									select="number(substring(prd:DateReported, 5, 2))" />
								<xsl:with-param name="upperCase" select="true()" />
							</xsl:call-template>
						</xsl:variable>-->
										<!--<xsl:value-of
							select="concat(normalize-space($month), substring(normalize-space(prd:DateReported), 3, 2))" />-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:DateLastActivity and number(prd:DateLastActivity) != 0">
										<xsl:call-template name="FormatDate">
											<xsl:with-param name="pattern" select="'mo/year'"/>
											<xsl:with-param name="value" select="prd:DateLastActivity"/>
										</xsl:call-template>
										<!--<xsl:variable name="month">
							<xsl:call-template name="FormatMonth">
								<xsl:with-param name="monthValue"
									select="number(substring(prd:DateLastActivity, 5, 2))" />
								<xsl:with-param name="upperCase" select="true()" />
							</xsl:call-template>
						</xsl:variable>-->
										<!--<xsl:value-of
							select="concat(normalize-space($month), substring(normalize-space(prd:DateLastActivity), 3, 2))" />-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="normalize-space(prd:Terms)='0000000'">
										<xsl:call-template name="nbsp"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="normalize-space(prd:Terms)"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:RecentHighCredit">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:RecentHighCredit/prd:Amount)"/>
										</xsl:variable>
										<!--<xsl:value-of select="concat(normalize-space(prd:RecentHighCredit/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>-->
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat(normalize-space(prd:RecentHighCredit/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:AccountBalance">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:AccountBalance/prd:Amount)"/>
										</xsl:variable>
										<xsl:value-of select="concat(normalize-space(prd:AccountBalance/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
										<!--<xsl:choose>
								<xsl:when test="$amount=0">
									<xsl:text><xsl:call-template name="nbsp"/></xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(normalize-space(prd:AccountBalance/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
								</xsl:otherwise>
							</xsl:choose>-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:CurrentPercentage">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:CurrentPercentage)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:CurrentPercentage) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT30">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT30)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT60">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT60)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT90">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT90)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT90Plus">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT90Plus)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:Comments and normalize-space(prd:Comments)!=''">
										<xsl:value-of select="prd:Comments"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
					</xsl:for-each>
				</tbody>
			</table>
			<xsl:call-template name="BackToTop"/>
		</xsl:if>
	</xsl:template>
	<xsl:template name="PaymentExperiencesAdditionalPaymentExperiences">
		<xsl:if test="prd:AdditionalPaymentExperiences">
			<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
				<thead>
					<tr>
						<th colspan="12">Trade Payment - Additional Trade Details</th>
					</tr>
				</thead>
				<tbody>
					<tr class="subtitle">
						<th colspan="6">
							<div style="text-align:center">Payment Experiences</div>
							<div style="text-align:center">(Trade Lines with an (*) after the date are newly reported)</div>
						</th>
						<th colspan="5">
							<div style="text-align:center">Account Status<br/>Days Beyond Terms</div>
						</th>
						<th/>
					</tr>
					<tr class="datahead">
						<td>Business Category</td>
						<td>Date Reported</td>
						<td>Last Sale</td>
						<td>Payment Terms</td>
						<td>Recent High Credit</td>
						<td>Balance</td>
						<td>Cur</td>
						<td style="width:33px">1-30</td>
						<td style="width:33px">31-60</td>
						<td style="width:33px">61-90</td>
						<td style="width:33px">91+</td>
						<td style="width:70px">Comments</td>
					</tr>
					<xsl:for-each select="prd:AdditionalPaymentExperiences">
						<tr>
							<xsl:attribute name="class"><xsl:choose><xsl:when test="position() mod 2 =1"><xsl:value-of select="'even'"/></xsl:when><xsl:when test="position() mod 2 =0"><xsl:value-of select="'odd'"/></xsl:when></xsl:choose></xsl:attribute>
							<td>
								<xsl:value-of select="prd:BusinessCategory"/>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:DateReported and number(prd:DateReported) != 0">
										<xsl:call-template name="FormatDate">
											<xsl:with-param name="pattern" select="'mo/year'"/>
											<xsl:with-param name="value" select="prd:DateReported"/>
										</xsl:call-template>
										<xsl:choose>
											<xsl:when test="prd:NewlyReportedIndicator and normalize-space(prd:NewlyReportedIndicator/@code) != '' and normalize-space(prd:NewlyReportedIndicator/@code) != 'N'">
												<xsl:value-of select="'*'"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="''"/>
											</xsl:otherwise>
										</xsl:choose>
										<!--<xsl:variable name="month">
							<xsl:call-template name="FormatMonth">
								<xsl:with-param name="monthValue"
									select="number(substring(prd:DateReported, 5, 2))" />
								<xsl:with-param name="upperCase" select="true()" />
							</xsl:call-template>
						</xsl:variable>-->
										<!--<xsl:value-of
							select="concat(normalize-space($month), substring(normalize-space(prd:DateReported), 3, 2))" />-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:DateLastActivity and number(prd:DateLastActivity) != 0">
										<xsl:call-template name="FormatDate">
											<xsl:with-param name="pattern" select="'mo/year'"/>
											<xsl:with-param name="value" select="prd:DateLastActivity"/>
										</xsl:call-template>
										<!--<xsl:variable name="month">
							<xsl:call-template name="FormatMonth">
								<xsl:with-param name="monthValue"
									select="number(substring(prd:DateLastActivity, 5, 2))" />
								<xsl:with-param name="upperCase" select="true()" />
							</xsl:call-template>
						</xsl:variable>-->
										<!--<xsl:value-of
							select="concat(normalize-space($month), substring(normalize-space(prd:DateLastActivity), 3, 2))" />-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="normalize-space(prd:Terms)='0000000'">
										<xsl:call-template name="nbsp"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="normalize-space(prd:Terms)"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:RecentHighCredit">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:RecentHighCredit/prd:Amount)"/>
										</xsl:variable>
										<!--<xsl:value-of select="concat(normalize-space(prd:RecentHighCredit/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>-->
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="concat(normalize-space(prd:RecentHighCredit/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:AccountBalance">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:AccountBalance/prd:Amount)"/>
										</xsl:variable>
										<xsl:value-of select="concat(normalize-space(prd:AccountBalance/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
										<!--<xsl:choose>
								<xsl:when test="$amount=0">
									<xsl:text><xsl:call-template name="nbsp"/></xsl:text>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="concat(normalize-space(prd:AccountBalance/prd:Modifier/@code),format-number($amount,'$###,###,##0'))"/>
								</xsl:otherwise>
							</xsl:choose>-->
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:CurrentPercentage">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:CurrentPercentage)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number(number(prd:CurrentPercentage) div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT30">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT30)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT60">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT60)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT90">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT90)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td class="rightalign">
								<xsl:choose>
									<xsl:when test="prd:DBT90Plus">
										<xsl:variable name="amount">
											<xsl:value-of select="number(prd:DBT90Plus)"/>
										</xsl:variable>
										<xsl:choose>
											<xsl:when test="$amount=0">
												<xsl:call-template name="nbsp"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="format-number($amount div 100,'##0%')"/>
											</xsl:otherwise>
										</xsl:choose>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
							<td>
								<xsl:choose>
									<xsl:when test="prd:Comments and normalize-space(prd:Comments)!=''">
										<xsl:value-of select="prd:Comments"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:call-template name="nbsp"/>
									</xsl:otherwise>
								</xsl:choose>
							</td>
						</tr>
					</xsl:for-each>
				</tbody>
			</table>
			<xsl:call-template name="BackToTop"/>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
