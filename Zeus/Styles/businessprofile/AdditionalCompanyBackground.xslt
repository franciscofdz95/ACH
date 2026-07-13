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
  * AdditionalCompanyBackground template
  *********************************************
  -->
  <xsl:template name="AdditionalCompanyBackground">
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
                      <b><font color="#ffffff">Additional Company Background Information</font></b></td>
                  </tr>

                  <tr>
                    <td>
                      <img src="../images/spacer.gif" width="0" height="3" alt=""/></td>
                  </tr>

                  <tr>
                    <td bgcolor="#ffffff" align="center">
                      <table width="98%" border="0" cellspacing="0" cellpadding="1">


                        <xsl:if test="prd:KeyPersonnelExecutiveInformation">
                          <tr>
                            <td align="left">
                              <b><font color="{$borderColor}">Key Personnel</font></b></td>
                          </tr>

                          <tr>
                            <td align="left">
                              <table border="0" cellspacing="0" cellpadding="0">
        
                                <!-- rows of KeyPersonnelExecutiveInformation -->
                                <xsl:apply-templates select="prd:KeyPersonnelExecutiveInformation" />
        
                              </table>
                            </td>
                          </tr>
                        </xsl:if>
  
                        <xsl:if test="prd:KeyPersonnelExecutiveInformation and (prd:CorporateInformation or prd:CorporateLinkageSummary or prd:DemographicInformation or prd:AdditionalBusinessNameInformation)">
                          <tr>
                            <td>
                              <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
                          </tr>
                        </xsl:if>

                        <!-- 
                            CorporateLinkageSummary section is involved with other 3 section.
                            We need to check other 3 to determine if this section needs to be handled 
                        -->
                        <xsl:if test="prd:CorporateInformation or prd:CorporateLinkageSummary or prd:DemographicInformation or prd:AdditionalBusinessNameInformation">
                          <!-- rows of CorporateLinkageSummary -->
                          <xsl:call-template name="OperatingInformation" />
                        </xsl:if>
  

                        <xsl:if test="(prd:KeyPersonnelExecutiveInformation or prd:CorporateInformation or prd:CorporateLinkageSummary or prd:AdditionalBusinessNameInformation) and prd:DemographicInformation">
                          <tr>
                            <td>
                              <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
                          </tr>
                        </xsl:if>

                        <!-- rows of DemographicInformation -->
                        <xsl:apply-templates select="prd:DemographicInformation" />

                        <xsl:if test="(prd:KeyPersonnelExecutiveInformation or prd:CorporateInformation or prd:CorporateLinkageSummary or prd:DemographicInformation or prd:AdditionalBusinessNameInformation) and prd:CorporateLinkageNameAndAddress">
                          <tr>
                            <td>
                              <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
                          </tr>
                        </xsl:if>

                        <xsl:if test="prd:CorporateLinkageNameAndAddress">
                          <tr>
                            <td align="left">
                              <b><font color="{$borderColor}">Affiliated Companies</font></b></td>
                          </tr>

                          <!-- rows of DemographicInformation -->
                          <xsl:apply-templates select="prd:CorporateLinkageNameAndAddress" />
                        </xsl:if>

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
  * KeyPersonnelExecutiveInformation template
  *********************************************
  -->
  <xsl:template match="prd:KeyPersonnelExecutiveInformation" xml:space="preserve">

    <xsl:variable name="label">
      <xsl:choose>		              
        <xsl:when test="position() &gt; 1">		    		   		   
          <xsl:value-of select="''" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'Principal(s):'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="name">
      <xsl:variable name="title">
        <xsl:choose>		              
          <xsl:when test="prd:Title">		    		   		   
            <xsl:value-of select="prd:Title" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="mi">
        <xsl:choose>		              
          <xsl:when test="string-length(prd:Name) > 60 and normalize-space(substring(prd:Name,21,1)) != '' ">		    		   		   
            <xsl:value-of select="concat(substring(prd:Name,21,1), '.')" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="' '" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:variable name="tmpName">
        <xsl:choose>		              
          <xsl:when test="prd:NameFlag/@code = '0' and string-length(prd:Name) > 60 ">		    		   		   
            <xsl:value-of select="concat(substring(prd:Name,1,20), $mi, substring(prd:Name,22,40), substring(prd:Name,62, 4))" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="prd:Name" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:variable>

      <xsl:choose>		              
        <xsl:when test="prd:Name and normalize-space($title) != ''">		    		   		   
          <xsl:value-of select="concat(normalize-space($tmpName), ', ', translate($title, 'amp;amp;', 'amp;'))" />
        </xsl:when>

        <xsl:when test="prd:Name">
          <xsl:value-of select="$tmpName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
        <b><xsl:value-of select="$label" /><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></b></font></td>
      <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="normalize-space($name)" /></font></td>
    </tr>
          
  </xsl:template>


  <!--
  *********************************************
  * OperatingInformation template
  *********************************************
  -->
  <xsl:template name="OperatingInformation">

    <xsl:variable name="businessType">
      <xsl:choose>		              
        <xsl:when test="prd:CorporateInformation/prd:BusinessType and normalize-space(prd:CorporateInformation/prd:BusinessType/@code) != ''">		    		   		   
    		   <xsl:call-template name="TranslateBusinessType">
    		     <xsl:with-param name="value" select="normalize-space(prd:CorporateInformation/prd:BusinessType/@code)" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="priorName">
      <xsl:choose>		              
        <xsl:when test="prd:AdditionalBusinessNameInformation/prd:PriorName">		    		   		   
          <xsl:value-of select="prd:AdditionalBusinessNameInformation/prd:PriorName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section1Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($businessType) != '' or normalize-space($priorName) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="locationType">
      <xsl:choose>		              
        <xsl:when test="prd:DemographicInformation/prd:Location and normalize-space(prd:DemographicInformation/prd:Location/@code) != ''">		    		   		   
          <xsl:value-of select="prd:DemographicInformation/prd:Location" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="divisionName">
      <xsl:choose>		              
        <xsl:when test="prd:AdditionalBusinessNameInformation/prd:DivisionName">		    		   		   
          <xsl:value-of select="prd:AdditionalBusinessNameInformation/prd:DivisionName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="numberSubsidiaries">
      <xsl:choose>		              
        <xsl:when test="prd:CorporateLinkageSummary/prd:NumberOfSubsidiaries">		    		   		   
          <xsl:value-of select="number(prd:CorporateLinkageSummary/prd:NumberOfSubsidiaries)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="numberBranches">
      <xsl:choose>		              
        <xsl:when test="prd:CorporateLinkageSummary/prd:NumberOfBranches">		    		   		   
          <xsl:value-of select="number(prd:CorporateLinkageSummary/prd:NumberOfBranches)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="numberCustomers">
      <xsl:choose>		              
        <xsl:when test="prd:DemographicInformation/prd:CustomerCount and number(prd:DemographicInformation/prd:CustomerCount) &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:DemographicInformation/prd:CustomerCount)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section2Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($locationType) != '' or normalize-space($divisionName) != '' or normalize-space($numberSubsidiaries) != '' or normalize-space($numberBranches) != '' or normalize-space($numberCustomers) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td align="left">
        <b><font color="{$borderColor}">Operating Information</font></b></td>
    </tr>

    <xsl:if test="normalize-space($businessType) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Business Type: </b><xsl:value-of select="$businessType" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($priorName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Prior Company Name: </b><xsl:value-of select="$priorName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="boolean(number($section1Presents)) and boolean(number($section2Presents))">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>
         
    <xsl:if test="normalize-space($locationType) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Location Type: </b><xsl:value-of select="$locationType" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($divisionName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Division Name: </b><xsl:value-of select="$divisionName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($numberSubsidiaries) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Affiliated Subsidiaries: </b><xsl:value-of select="$numberSubsidiaries" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($numberBranches) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Affiliated Locations: </b><xsl:value-of select="$numberBranches" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($numberCustomers) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Number of Customers: </b><xsl:value-of select="$numberCustomers" /></font></td>
      </tr>
    </xsl:if>
          
  </xsl:template>


  <!--
  *********************************************
  * DemographicInformation template
  *********************************************
  -->
  <xsl:template match="prd:DemographicInformation" xml:space="preserve">

    <xsl:variable name="primarySICCode">
      <xsl:choose>		              
        <xsl:when test="prd:PrimarySICCode">		    		   		   
          <xsl:value-of select="prd:PrimarySICCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="primarySICDescription">
      <xsl:choose>		              
        <xsl:when test="prd:SICDescription">		    		   		   
          <xsl:value-of select="translate(prd:SICDescription, 'amp;amp;', 'amp;')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="primarySIC">
      <xsl:choose>		              
        <xsl:when test="normalize-space($primarySICCode) != '' and normalize-space($primarySICDescription) != ''">		    		   		   
          <xsl:value-of select="concat($primarySICDescription, ' - ', $primarySICCode)" />
        </xsl:when>

        <xsl:when test="normalize-space($primarySICDescription) != ''">		    		   		   
          <xsl:value-of select="$primarySICDescription" />
        </xsl:when>

        <xsl:when test="normalize-space($primarySICCode) != ''">		    		   		   
          <xsl:value-of select="$primarySICCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="secondarySICCode">
      <xsl:choose>		              
        <xsl:when test="prd:SecondarySICCode">		    		   		   
          <xsl:value-of select="prd:SecondarySICCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="secondarySICDescription">
      <xsl:choose>		              
        <xsl:when test="prd:SecondarySICDescription">		    		   		   
          <xsl:value-of select="translate(prd:SecondarySICDescription, 'amp;amp;', 'amp;')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="secondarySIC">
      <xsl:choose>		              
        <xsl:when test="normalize-space($secondarySICCode) != '' and normalize-space($secondarySICDescription) != ''">		    		   		   
          <xsl:value-of select="concat($secondarySICDescription, ' - ', $secondarySICCode)" />
        </xsl:when>

        <xsl:when test="normalize-space($secondarySICDescription) != ''">		    		   		   
          <xsl:value-of select="$secondarySICDescription" />
        </xsl:when>

        <xsl:when test="normalize-space($secondarySICCode) != ''">		    		   		   
          <xsl:value-of select="$secondarySICCode" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="additionalSIC">
      <xsl:choose>		              
        <xsl:when test="prd:AdditionalSICCodes/prd:SICCode">		    		   		   
          <xsl:call-template name="JoinNodeset">
            <xsl:with-param name="nodeset" select="prd:AdditionalSICCodes/prd:SICCode" />
            <xsl:with-param name="order" select="'descending'" />
            <xsl:with-param name="delimiter" select="' - '" />
          </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section1Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($primarySIC) != '' or normalize-space($secondarySIC) != '' or normalize-space($additionalSIC) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="yearsInBusiness">
      <xsl:choose>		              
        <xsl:when test="prd:YearsInBusinessIndicator and normalize-space(prd:YearsInBusinessIndicator/@code) != ''">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:YearsInBusinessIndicator/@code = 0">		    		   		   
              <xsl:value-of select="concat(number(prd:YearsInBusinessOrLowRange) , ' to ', number(prd:HighRangeYears))" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="number(prd:YearsInBusinessOrLowRange)" />
            </xsl:otherwise>
          </xsl:choose>          
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="employeeSize">
      <xsl:choose>		              
        <xsl:when test="prd:EmployeeIndicator and normalize-space(prd:EmployeeIndicator/@code) != ''">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:EmployeeIndicator/@code = 0">		    		   		   
              <xsl:value-of select="concat(format-number(prd:EmployeeSizeOrLowRange, '#,##0') , ' to ', format-number(prd:HighEmployeeRange, '#,##0'))" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="format-number(prd:EmployeeSizeOrLowRange, '#,##0')" />
            </xsl:otherwise>
          </xsl:choose>          
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="sales">
      <xsl:choose>		              
        <xsl:when test="prd:SalesIndicator and normalize-space(prd:SalesIndicator/@code) != ''">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:SalesIndicator/@code = 0">		    		   		   
              <xsl:value-of select="concat(format-number(prd:SalesRevenueOrLowRange, '$#,##0') , ' to ', format-number(prd:HighRangeOfSales, '$#,##0'))" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="format-number(prd:SalesRevenueOrLowRange, '$#,##0')" />
            </xsl:otherwise>
          </xsl:choose>          
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="netWorth">
      <xsl:choose>		              
        <xsl:when test="prd:NetWorthIndicator and normalize-space(prd:NetWorthIndicator/@code) != ''">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:NetWorthIndicator/@code = 0">		    		   		   
              <xsl:value-of select="concat(prd:NetWorthAmountOrLowRange/prd:Modifier, format-number(prd:NetWorthAmountOrLowRange/prd:Amount, '$#,##0') , ' to ', prd:HighRangeOrNetWorth/prd:Modifier, format-number(prd:HighRangeOrNetWorth/prd:Amount, '$#,##0'))" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat(prd:NetWorthAmountOrLowRange/prd:Modifier, format-number(prd:NetWorthAmountOrLowRange/prd:Amount, '$#,##0'))" />
            </xsl:otherwise>
          </xsl:choose>          
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section2Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($yearsInBusiness) != '' or normalize-space($employeeSize) != '' or normalize-space($sales) != '' or normalize-space($netWorth) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="buildingSize">
      <xsl:choose>		              
        <xsl:when test="prd:BuildingSize and prd:BuildingSize &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:BuildingSize)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="buildingOwnership">
      <xsl:choose>		              
        <xsl:when test="prd:BuildingOwnership and normalize-space(prd:BuildingOwnership/@code) != ''">		    		   		   
          <xsl:value-of select="prd:BuildingOwnership" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="section3Presents">
      <xsl:choose>		              
        <xsl:when test="normalize-space($buildingSize) != '' or normalize-space($buildingOwnership) != ''">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="0" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="normalize-space($primarySIC) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Primary SIC Code: </b><xsl:value-of select="$primarySIC" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($secondarySIC) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Secondary SIC Code: </b><xsl:value-of select="$secondarySIC" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($additionalSIC) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Additional SIC Code: </b><xsl:value-of select="$additionalSIC" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="boolean(number($section1Presents)) and boolean(number($section2Presents))">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>
         
    <xsl:if test="normalize-space($yearsInBusiness) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Years in Business: </b><xsl:value-of select="$yearsInBusiness" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($employeeSize) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Number of Employees: </b><xsl:value-of select="$employeeSize" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($sales) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Sales: </b><xsl:value-of select="$sales" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($netWorth) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Net Worth: </b><xsl:value-of select="$netWorth" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="(boolean(number($section1Presents)) or boolean(number($section2Presents))) and boolean(number($section3Presents))">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>
         
    <xsl:if test="normalize-space($buildingSize) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Location Square Footage: </b><xsl:value-of select="$buildingSize" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($buildingOwnership) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Ownership: </b><xsl:value-of select="$buildingOwnership" /></font></td>
      </tr>
    </xsl:if>

  </xsl:template>


  <!--
  *********************************************
  * CorporateLinkageNameAndAddress template
  *********************************************
  -->
  <xsl:template match="prd:CorporateLinkageNameAndAddress" xml:space="preserve">
    <xsl:variable name="locationType">
      <xsl:choose>		              
        <xsl:when test="prd:RecordType and normalize-space(prd:RecordType/@code) != ''">		    		   		   
          <xsl:value-of select="prd:RecordType" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="fileNumber">
      <xsl:choose>		              
        <xsl:when test="prd:ExperianFileNumber">		    		   		   
          <xsl:value-of select="prd:ExperianFileNumber" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="companyName">
      <xsl:choose>		              
        <xsl:when test="prd:Name">		    		   		   
          <xsl:value-of select="prd:Name" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="address">
      
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

      <xsl:variable name="state">
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
        <xsl:when test="normalize-space($street) != '' or normalize-space($city) != '' or normalize-space($state) != '' or normalize-space($zip) != ''">		    		   		   
          <xsl:value-of select="concat($street, $city, $state, $zip)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="stateIncorporation">
      <xsl:choose>		              
        <xsl:when test="prd:IncorporationState">		    		   		   
			  <xsl:call-template name="TranslateState">
			    <xsl:with-param name="value" select="prd:IncorporationState" />
			    <xsl:with-param name="upperCase" select="true()" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateIncorporation">
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

    <xsl:variable name="publicRecord">
      <xsl:choose>		              
        <xsl:when test="prd:SignificantPublicRecordIndicator">		    		   		   
          <xsl:choose>		              
            <xsl:when test="prd:SignificantPublicRecordIndicator = 'Y'">		    		   		   
              <xsl:value-of select="'YES'" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="'NO'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="position() &gt; 1">
      <tr>
        <td>
          <img src="../images/spacer.gif" width="0" height="10" alt=""/></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($locationType) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Location Type: </b><xsl:value-of select="$locationType" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($fileNumber) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Experian File No: </b><xsl:value-of select="$fileNumber" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($companyName) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Company Name: </b><xsl:value-of select="$companyName" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($address) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Address: </b><xsl:value-of select="$address" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($stateIncorporation) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>State of Incorporation: </b><xsl:value-of select="$stateIncorporation" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($dateIncorporation) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Date of Incorporation: </b><xsl:value-of select="$dateIncorporation" /></font></td>
      </tr>
    </xsl:if>

    <xsl:if test="normalize-space($publicRecord) != ''">
      <tr>
        <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Significant Derogatory Filings: </b><xsl:value-of select="$publicRecord" /></font></td>
      </tr>
    </xsl:if>

  </xsl:template>

</xsl:stylesheet>