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
  <xsl:template name="AdditionalBusinessFacts">

    <xsl:if test="prd:CorporateRegistration or prd:Competitors">
	<table class="section" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th colspan="2"><a class="report_section_title">Additional Business Facts</a></th>
			</tr>
		</thead>
	    <xsl:if test="prd:CorporateRegistration">
	    	<xsl:apply-templates select="prd:CorporateRegistration"></xsl:apply-templates>
	    </xsl:if>
		<xsl:if test="prd:Competitors"><thead>
			<tr class="subtitle"><th colspan="2">Competitors</th></tr>
		</thead>
		<tbody><xsl:for-each select="prd:Competitors">
			<tr><td colspan="2" class="firstColumn" style="height:18px; vertical-align:middle;"><xsl:value-of select="prd:CompetitorName"/></td></tr>
		</xsl:for-each></tbody></xsl:if>
	</table>
    <xsl:call-template name="BackToTop" />
	</xsl:if>
  </xsl:template>


  <!--
  *********************************************
  * CorporateRegistration template
  *********************************************
  -->
  <xsl:template match="prd:CorporateRegistration">
    <xsl:variable name="stateName">
      <xsl:choose>
        <xsl:when test="normalize-space(prd:StateOfOrigin)">
			  <xsl:call-template name="TranslateState">
			    <xsl:with-param name="value" select="prd:StateOfOrigin" />
			    <xsl:with-param name="upperCase" select="true()" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="stateStatement">
      <xsl:choose>
        <xsl:when test="normalize-space($stateName) = 'CALIFORNIA'">
          <xsl:value-of select="'THIS DATA IS FOR INFORMATION PURPOSES ONLY. CERTIFICATION CAN ONLY BE OBTAINED THROUGH THE SACRAMENTO OFFICE OF THE CALIFORNIA SECRETARY OF STATE.'" />
        </xsl:when>

        <xsl:when test="normalize-space($stateName) = 'NORTH CAROLINA'">
          <xsl:value-of select="'THIS DATA IS FOR INFORMATION PURPOSES ONLY. CERTIFICATION CAN ONLY BE OBTAINED THROUGH THE NORTH CAROLINA DEPARTMENT OF THE SECRETARY OF STATE.'" />
        </xsl:when>

        <xsl:otherwise>
          		<xsl:value-of select="concat('THE FOLLOWING INFORMATION WAS PROVIDED BY THE STATE OF ', $stateName, '.')" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="statusFlag">
      <xsl:choose>
        <xsl:when test="prd:StatusFlag and normalize-space(prd:StatusFlag/@code) != ''">
          <xsl:value-of select="prd:StatusFlag" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="statusDescription">
      <xsl:choose>
        <xsl:when test="prd:StatusDescription">
          <xsl:value-of select="prd:StatusDescription" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="currentStatus">
      <xsl:choose>
        <xsl:when test="normalize-space($statusFlag) != '' and normalize-space($statusDescription) != ''">
          <xsl:value-of select="concat($statusFlag, ' - ', $statusDescription)" />
        </xsl:when>

        <xsl:when test="normalize-space($statusFlag) != ''">
          <xsl:value-of select="$statusFlag" />
        </xsl:when>

        <xsl:when test="normalize-space($statusDescription) != ''">
          <xsl:value-of select="$statusDescription" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="profileFlag">
      <xsl:choose>
        <xsl:when test="prd:ProfitFlag and normalize-space(prd:ProfitFlag/@code) != ''">
          <xsl:value-of select="prd:ProfitFlag" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="businessTypeCode">
      <xsl:choose>
        <xsl:when test="prd:BusinessType and normalize-space(prd:BusinessType/@code) != ''">
    		   <xsl:call-template name="TranslateBusinessType">
    		     <xsl:with-param name="value" select="normalize-space(prd:BusinessType/@code)" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="businessType">
      <xsl:choose>
        <xsl:when test="normalize-space($profileFlag) != '' and normalize-space($businessTypeCode) != ''">
          <xsl:value-of select="concat($businessTypeCode, ' - ', $profileFlag)" />
        </xsl:when>

        <xsl:when test="normalize-space($profileFlag) != ''">
          <xsl:value-of select="$profileFlag" />
        </xsl:when>

        <xsl:when test="normalize-space($businessTypeCode) != ''">
          <xsl:value-of select="$businessTypeCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

		<thead>
			<tr class="subtitle">
				<th colspan="2">
					<xsl:comment>For left side label</xsl:comment>
					<a name="CorporateRegistration">Corporate Registration</a>
				</th>
				<!--<th colspan="2">Additional filings are available click here</th>-->
			</tr>
		</thead>
		<tbody>
			<xsl:if test="prd:StateOfOrigin">
			<tr><td class="firstColumn" style="padding-top:2px;padding-bottom:2px;"><xsl:value-of select="$stateStatement"/></td></tr>
			<tr><td colspan="2"></td></tr>
			</xsl:if>
			<tr>
			<td colspan="2" class="firstColumn">
				<table>
					<tbody>
						<xsl:if test="prd:StateOfOrigin">
						<tr>
							<td class="label">State of Origin:</td>
							<td class=""><xsl:value-of select="normalize-space(prd:StateOfOrigin)"></xsl:value-of></td>
						</tr>
						</xsl:if>
						<xsl:if test="prd:StateOfOrigin">
						<tr>
							<td class="label">Date of Incorporation:</td>
							<td class="">
								<xsl:call-template name="FormatDate">
									<xsl:with-param name="pattern" select="'mo/dt/year'"/>
									<xsl:with-param name="value" select="normalize-space(prd:IncorporatedDate)"/>
								</xsl:call-template>
							</td>
						</tr>
						</xsl:if>
					    <xsl:if test="normalize-space($currentStatus) != ''">
						<tr>
							<td class="label">Current Status:</td>
							<td class=""><xsl:value-of select="normalize-space($currentStatus)"></xsl:value-of></td>
						</tr>
						</xsl:if>
						<xsl:if test="$businessType != ''">
						<tr>
							<td class="label">Business Type:</td>
							<td class=""><xsl:value-of select="$businessType"></xsl:value-of></td>
						</tr>
						</xsl:if>
						<xsl:if test="prd:CharterNumber">
						<tr>
							<td class="label">Charter Number:</td>
							<td class=""><xsl:value-of select="normalize-space(prd:CharterNumber)"></xsl:value-of></td>
						</tr>
						</xsl:if>
						<xsl:if test="prd:AgentInformation/prd:Name">
						<tr>
							<td class="label">Agent:</td>
							<td class=""><xsl:value-of select="normalize-space(prd:AgentInformation/prd:Name)"></xsl:value-of></td>
						</tr>
						</xsl:if>
						<xsl:if test="prd:AgentInformation/prd:StreetAddress or prd:AgentInformation/prd:City or prd:AgentInformation/prd:State">
						<tr>
							<td class="label">Agent Address:</td>
							<td class="">
								<xsl:value-of select="normalize-space(prd:AgentInformation/prd:StreetAddress)"></xsl:value-of>
								<xsl:value-of select="concat(' ',normalize-space(prd:AgentInformation/prd:City))"></xsl:value-of>
								<xsl:if test="normalize-space(prd:AgentInformation/prd:City)!='' and normalize-space(prd:AgentInformation/prd:State)!=''">, </xsl:if>
								<xsl:value-of select="normalize-space(prd:AgentInformation/prd:State)"></xsl:value-of>
							</td>
						</tr>
						</xsl:if>
					</tbody>
				</table>
			</td>
			</tr>
		</tbody>
  </xsl:template>
  <!-- @TODO business License Filing & Additional Doing Business as names -->
</xsl:stylesheet>