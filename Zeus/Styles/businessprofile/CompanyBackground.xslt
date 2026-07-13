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
  * CompanyBackground template
  *********************************************
  -->
  <xsl:template name="CompanyBackground">
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Company Background Information'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>
    
    <xsl:if test="prd:CorporateInformation or prd:CorporateOwnerInformation">
      <!-- BankingRelationship -->
      <xsl:call-template name="CorporateRegistration" />
    </xsl:if>

    <xsl:if test="prd:CorporateLinkageSummary or prd:DemographicInformation or prd:CorporateLinkageNameAndAddress or prd:KeyPersonnelExecutiveInformation">
      
      <xsl:if test="prd:CorporateInformation or prd:CorporateOwnerInformation">
        <!-- back to top graphic -->
        <xsl:call-template name="BackToTop" />
      </xsl:if>

      <!-- AdditionalCompanyBackground -->
      <xsl:call-template name="AdditionalCompanyBackground" />
    </xsl:if>
    

  </xsl:template>


  <!--
  *********************************************
  * CorporateRegistration template
  *********************************************
  -->
  <xsl:template name="CorporateRegistration">
  
    <xsl:variable name="stateName">
      <xsl:choose>		              
        <xsl:when test="prd:CorporateInformation/prd:StateOfOrigin">		    		   		   
			  <xsl:call-template name="TranslateState">
			    <xsl:with-param name="value" select="prd:CorporateInformation/prd:StateOfOrigin" />
			    <xsl:with-param name="upperCase" select="true()" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
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
                      <b><font color="#ffffff">Corporate Registration</font></b></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>

                  <tr>
                    <td bgcolor="#ffffff" align="center">
                      <table width="98%" border="0" cellspacing="0" cellpadding="1">

                        <tr>
                          <td align="left" valign="middle" height="30">
                            THE FOLLOWING INFORMATION WAS PROVIDED BY THE STATE OF <xsl:value-of select="$stateName" />
                          </td>
                        </tr>
      
                        <!-- rows of CorporateInformation -->
                        <xsl:apply-templates select="prd:CorporateInformation" />

                        <xsl:if test="prd:CorporateInformation and prd:CorporateOwnerInformation">
                          <tr>
                            <td>
                              <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
                          </tr>
                        </xsl:if>

                        <!-- rows of CorporateOwnerInformation -->
                        <xsl:apply-templates select="prd:CorporateOwnerInformation" />

                      </table>
                    </td>
                  </tr>

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
  * CorporateInformation template
  *********************************************
  -->
  <xsl:template match="prd:CorporateInformation" xml:space="preserve">

    <xsl:variable name="state">
      <xsl:choose>		              
        <xsl:when test="prd:StateOfOrigin">		    		   		   
          <xsl:value-of select="prd:StateOfOrigin" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateIncorporated">
      <xsl:choose>		              
        <xsl:when test="prd:IncorporatedDate and number(prd:IncorporatedDate) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:IncorporatedDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
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

    <xsl:variable name="section1Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($state) != '' or normalize-space($dateIncorporated) != '' or normalize-space($currentStatus) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
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

    <xsl:variable name="dbaName">
      <xsl:choose>		              
        <xsl:when test="prd:DBAName">		    		   		   
          <xsl:value-of select="prd:DBAName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="charterNumber">
      <xsl:choose>		              
        <xsl:when test="prd:CharterNumber">		    		   		   
          <xsl:value-of select="prd:CharterNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="federalTaxID">
      <xsl:choose>		              
        <xsl:when test="prd:FederalTaxID">		    		   		   
          <xsl:value-of select="concat(substring(prd:FederalTaxID, 1, 2), '-', substring(prd:FederalTaxID, 3, 7))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="stateTaxID">
      <xsl:choose>		              
        <xsl:when test="prd:StateTaxID">		    		   		   
          <xsl:value-of select="prd:StateTaxID" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section2Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($businessType) != '' or normalize-space($dbaName) != '' or normalize-space($charterNumber) != '' or normalize-space($federalTaxID) != '' or normalize-space($stateTaxID) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="agentName">
      <xsl:choose>		              
        <xsl:when test="prd:AgentInformation/prd:Name">		    		   		   
          <xsl:value-of select="prd:AgentInformation/prd:Name" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="agentAddress">
      
      <xsl:variable name="street">
        <xsl:choose>		              
          <xsl:when test="prd:AgentInformation/prd:StreetAddress">		    		   		   
            <xsl:value-of select="prd:AgentInformation/prd:StreetAddress" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="city">
        <xsl:choose>		              
          <xsl:when test="prd:AgentInformation/prd:City">		    		   		   
            <xsl:value-of select="prd:AgentInformation/prd:City" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="agentState">
        <xsl:choose>		              
          <xsl:when test="prd:AgentInformation/prd:State">		    		   		   
            <xsl:value-of select="prd:AgentInformation/prd:State" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>
      
      <xsl:variable name="zip">
        <xsl:choose>		              
          <xsl:when test="prd:AgentInformation/prd:Zip">		    		   		   
            <xsl:value-of select="prd:AgentInformation/prd:Zip" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>
      
      <xsl:variable name="cityComma">
        <xsl:choose>                  
          <xsl:when test="normalize-space($city) != '' and normalize-space($agentState) != ''">                    
            <xsl:value-of select="concat(normalize-space($city), ',')" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="$city" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>
      
      <xsl:choose>		              
        <xsl:when test="normalize-space($street) != '' or normalize-space($city) != '' or normalize-space($agentState) != '' or normalize-space($zip) != ''">		    		   		   
          <xsl:value-of select="concat($street, $cityComma, $agentState, $zip)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section3Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($agentName) != '' or normalize-space($agentAddress) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="normalize-space($state) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>State of Origin: </b><xsl:value-of select="$state" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($dateIncorporated) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Date of Incorporation: </b><xsl:value-of select="$dateIncorporated" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($currentStatus) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Current Status: </b><xsl:value-of select="$currentStatus" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="boolean(number($section1Presents)) and boolean(number($section2Presents))">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>
         
    <xsl:if test="normalize-space($businessType) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Business Type: </b><xsl:value-of select="$businessType" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($dbaName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>DBA: </b><xsl:value-of select="$dbaName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($charterNumber) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Charter Number: </b><xsl:value-of select="$charterNumber" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($federalTaxID) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Federal Tax ID: </b><xsl:value-of select="$federalTaxID" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($stateTaxID) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>State Tax ID: </b><xsl:value-of select="$stateTaxID" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="(boolean(number($section1Presents)) or boolean(number($section2Presents))) and boolean(number($section3Presents))">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>
         
    <xsl:if test="normalize-space($agentName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Agent: </b><xsl:value-of select="$agentName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($agentAddress) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Agent Address: </b><xsl:value-of select="$agentAddress" /></font></td>
      </tr>
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * CorporateOwnerInformation template
  *********************************************
  -->
  <xsl:template match="prd:CorporateOwnerInformation" xml:space="preserve">
    <xsl:variable name="ownerName">
      <xsl:choose>		              
        <xsl:when test="prd:Name">		    		   		   
          <xsl:value-of select="prd:Name" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="ownerAddress">
      
      <xsl:variable name="street">
        <xsl:choose>		              
          <xsl:when test="prd:Address">		    		   		   
            <xsl:value-of select="prd:Address" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="city">
        <xsl:choose>		              
          <xsl:when test="prd:City">		    		   		   
            <xsl:value-of select="prd:City" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="ownerState">
        <xsl:choose>		              
          <xsl:when test="prd:State">		    		   		   
            <xsl:value-of select="prd:State" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>
      
      <xsl:variable name="zip">
        <xsl:choose>		              
          <xsl:when test="prd:Zip">		    		   		   
            <xsl:value-of select="prd:Zip" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>
      
      <xsl:choose>		              
        <xsl:when test="normalize-space($street) != '' or normalize-space($city) != '' or normalize-space($ownerState) != '' or normalize-space($zip) != ''">		    		   		   
          <xsl:value-of select="concat($street, $city, $ownerState, $zip)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="normalize-space($ownerName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Corporate Owner: </b><xsl:value-of select="$ownerName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($ownerAddress) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Address: </b><xsl:value-of select="$ownerAddress" /></font></td>
      </tr>
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * TranslateBusinessType template
  * Value has to be 1 chr
  *********************************************
  -->
  <xsl:template name="TranslateBusinessType">
    <xsl:param name="value" select="''" />
    <xsl:param name="upperCase" select="false()" />
      
    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="$value= 'C'">		    		   		   
          <xsl:value-of select="'Corporation'" />
        </xsl:when>

        <xsl:when test="$value= 'G'">		    		   		   
          <xsl:value-of select="'General Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'H'">		    		   		   
          <xsl:value-of select="'Chain Store'" />
        </xsl:when>

        <xsl:when test="$value= 'I'">		    		   		   
          <xsl:value-of select="'Institutions'" />
        </xsl:when>

        <xsl:when test="$value= 'L'">		    		   		   
          <xsl:value-of select="'Limited Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'F'">		    		   		   
          <xsl:value-of select="'Fortune 1000'" />
        </xsl:when>

        <xsl:when test="$value= 'P'">		    		   		   
          <xsl:value-of select="'Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'R'">		    		   		   
          <xsl:value-of select="'Residential'" />
        </xsl:when>

        <xsl:when test="$value= 'S'">		    		   		   
          <xsl:value-of select="'Sole Proprietor'" />
        </xsl:when>

        <xsl:when test="$value= 'X'">		    		   		   
          <xsl:value-of select="'S Corporation'" />
        </xsl:when>

        <xsl:when test="$value= 'Y'">		    		   		   
          <xsl:value-of select="'Corporation'" />
        </xsl:when>

      </xsl:choose>    
    </xsl:variable>

    <xsl:choose>		              
      <xsl:when test="$upperCase">		    		   		   
        <xsl:value-of select="translate($result, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="$result" />
      </xsl:otherwise>
    </xsl:choose>    
    
  </xsl:template>
  
</xsl:stylesheet>