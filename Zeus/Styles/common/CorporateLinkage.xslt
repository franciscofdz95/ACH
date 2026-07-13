<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
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
  * CorporateLinkage template
  *********************************************
  -->
  <xsl:template name="CorporateLinkage">
    <xsl:param name="productOverride" select="'No'" />

    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Corporate Linkage'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <!-- tmpProduct -->
    <xsl:variable name="tmpProduct">
      <xsl:choose>		              
        <xsl:when test="$product = 'SBCSScore'">		    		   		   
          <xsl:value-of select="'SBCS'" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="'Business Profile'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="$product = 'IP' or $product = 'IPBOP' or $product = 'CI' or $product = 'SBCSScore' or $product = 'BSUM' or (($product = 'IPBPRCon' or $product = 'IPBPRBOP' ) and $productOverride = 'Yes') ">
	      <table width="100%" border="0" cellspacing="0" cellpadding="1">
	        <tr>
	          <td bgcolor="{$borderColor}">
	            <table width="100%" border="0" cellspacing="0" cellpadding="0">
	              <tr>
	                <td bgcolor="#ffffff">
	                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
	                    <tr>
	                      <td>
	                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
	                          <tr>
	                            <td width="60%" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
	                              Corporate linkage data such as Global Ultimate, Immediate
	                              Parent, Additional Branch Locations(s) and Subsidiary Information    
	                              is available on this business. Order a <xsl:value-of select="$tmpProduct" /> Report to    
	                              get more information.                              
	                              </font>
	                            </td>
	                            <td width="40%"><img src="../images/global/spacer.gif" height="2"/></td>
	                          </tr>
	                        </table>
	                      </td>
	                    </tr>
	                  </table>
	                </td>
	              </tr>
	            </table>
	          </td>
	        </tr>
	      </table>
	      <br/>
      </xsl:when>
      <xsl:otherwise>
	    <!-- corporate linkage display message -->
	      <table width="100%" border="0" cellspacing="0" cellpadding="0">
	        <tr>
	          <td width="2%" valign="middle" height="20" align="left"><img id="cltoggle" height="11" width="11" border="0" src="../images/expanded.gif"/><img height="1" width="3" border="0" src="../images/spacer.gif"/></td>
	          <td width="98%" ><font size="1">The following section displays the corporate linkage of this business.</font></td>
	        </tr>
	      </table>
	
	    <!-- blue box border -->
	    <table width="100%" border="0" cellspacing="0" cellpadding="1">
	      <tr>
	        <td bgcolor="{$borderColor}">
	
	          <!-- inner white box -->
	          <table width="100%" border="0" cellspacing="0" cellpadding="0">
	            <tr>
	              <td bgcolor="#ffffff">
	
	                <table width="100%" border="0" cellspacing="0" cellpadding="0">
	
	
	                  <!-- Corporate Linkage Details template -->
	
	                  <xsl:apply-templates select="prd:CorporateLinkage">
	                  </xsl:apply-templates>
	
	                  <!-- end business data section -->  
	                </table>
	              </td>
	            </tr>
	          </table>
	          <!-- end inner white box -->
	        </td>
	      </tr>  
	    </table>
	    <xsl:call-template name="BackToTop" />
      </xsl:otherwise>
    </xsl:choose>    

  </xsl:template>

  
  <!--
  *********************************************
  * CorporateLinkage Details template
  *********************************************
  -->
  <xsl:template match="prd:CorporateLinkage" xml:space="preserve">

    <!-- Ultimate Parent (1) count -->
    <xsl:variable name="countUltimateParent">
      <xsl:value-of select="count(//prd:CorporateLinkage/prd:LinkageRecordType/@code[.='1'])" />
    </xsl:variable>

    <!-- Parent (2) count -->
    <xsl:variable name="countParent">
      <xsl:value-of select="count(//prd:CorporateLinkage/prd:LinkageRecordType/@code[.='2'])" />
    </xsl:variable>

    <!-- Subsidiary (3) count -->
    <xsl:variable name="countSubsidiary">
      <xsl:value-of select="count(//prd:CorporateLinkage/prd:LinkageRecordType/@code[.='3'])" />
    </xsl:variable>

    <!-- Branch (4) count -->
    <xsl:variable name="countBranch">
      <xsl:value-of select="count(//prd:CorporateLinkage/prd:LinkageRecordType/@code[.='4'])" />
    </xsl:variable>

    <!-- BIN -->
    <xsl:variable name="BIN">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageRecordBIN">		    		   		   
          <xsl:value-of select="prd:LinkageRecordBIN" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- BusinessName -->
    <xsl:variable name="businessName">
      <xsl:choose>		              
        <xsl:when test="//prd:BusinessNameAndAddress/prd:BusinessName">		    		   		   
	      <xsl:value-of select="normalize-space(//prd:BusinessNameAndAddress/prd:BusinessName)" />
        </xsl:when>

        <xsl:when test="//prd:SBCSBusinessNameAndAddress/prd:BusinessName">		    		   		   
	      <xsl:value-of select="normalize-space(//prd:SBCSBusinessNameAndAddress/prd:BusinessName)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CompanyName -->
    <xsl:variable name="companyName">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageCompanyName">		    		   		   
	      <xsl:value-of select="normalize-space(prd:LinkageCompanyName)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CompanyAddress -->
    <xsl:variable name="companyAddress">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageCompanyAddress">		    		   		   
	      <xsl:value-of select="normalize-space(prd:LinkageCompanyAddress)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CompanyCity-->
    <xsl:variable name="companyCity">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageCompanyCity">		    		   		   
	      <xsl:value-of select="normalize-space(prd:LinkageCompanyCity)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CompanyState-->
    <xsl:variable name="companyState">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageCompanyState">		    		   		   
	      <xsl:value-of select="normalize-space(prd:LinkageCompanyState)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CountryCode -->
    <xsl:variable name="countryCode">
      <xsl:choose>		              
        <xsl:when test="prd:LinkageCountryCode">		    		   		   
	      <xsl:value-of select="normalize-space(prd:LinkageCountryCode)" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- CityStateCountry -->
    <xsl:variable name="cityStateCountry">
      <xsl:choose>		              
        <xsl:when test="normalize-space($countryCode) != ''">
	        <xsl:if test="$companyCity">
	          <xsl:value-of select="concat(normalize-space($companyCity), ', ', $countryCode)" />
	        </xsl:if>
	        <xsl:if test="normalize-space($companyCity) = ''">
	          <xsl:value-of select="$countryCode" />
	        </xsl:if>
        </xsl:when>
        <xsl:when test="$companyCity and $companyState">
          <xsl:value-of select="concat(normalize-space($companyCity), ', ', $companyState)" />
        </xsl:when>
        <xsl:when test="$companyCity">
          <xsl:value-of select="$companyCity" />
        </xsl:when>
        <xsl:when test="$companyState">
          <xsl:value-of select="$companyState" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- MatchingBusinessIndicator -->
    <xsl:variable name="matchingBusinessIndicator">
      <xsl:choose>
        <xsl:when test="prd:MatchingBusinessIndicator/@code and normalize-space(prd:MatchingBusinessIndicator/@code) = 'Y'">                     
          <xsl:value-of select="'Y'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- ReturnLimitExceeded -->
    <xsl:variable name="returnLimitExceeded">
      <xsl:choose>                  
        <xsl:when test="prd:ReturnLimitExceeded/@code and normalize-space(prd:ReturnLimitExceeded/@code) = 'Y'">                     
          <xsl:value-of select="'Y'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:if test="prd:LinkageRecordType/@code = '1'">
		        <tr>
		          <td>
		            <table width="100%" border="0" cellspacing="0" cellpadding="4">
       <xsl:choose>
         <xsl:when test="normalize-space($matchingBusinessIndicator) = 'Y'">                     
		              <tr>
		                <td width="100%" height="20"><font size="1" style="FONT-FAMILY: 'verdana';">The inquired upon business, 
		                <b><xsl:value-of select="normalize-space($businessName)" /></b>, 
		                is an Ultimate Parent.</font></td>
		              </tr>
         </xsl:when>
         <xsl:otherwise>
		              <tr>
		                <td width="100%" height="20"><b><font color="#015CAE">Ultimate Parent</font></b>:
		                  <font size="1"> The following is the ultimate parent of the inquired upon business.</font>
		                </td>
		              </tr>
         </xsl:otherwise>
       </xsl:choose>    
		            </table>
		          </td>
		        </tr>
    </xsl:if>

    <xsl:if test="prd:LinkageRecordType/@code = '2'">
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="100%" height="20"><b><font color="#015CAE">Immediate Parent</font></b>:
                              <font size="1">The following is the immediate parent of the inquired upon business.</font>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
    </xsl:if>

    <xsl:if test="prd:LinkageRecordType/@code = '3' and  not(prd:LinkageRecordType[attribute::code='3']=preceding::prd:LinkageRecordType[attribute::code='3'])">
		<xsl:if test="$countUltimateParent > 0 or $countParent > 0">
                    <!-- divider line -->
                    <tr>
                      <td bgcolor="#015CAE"><img src="../images/global/spacer.gif" width="0" height="1" /></td>
                    </tr>
		</xsl:if>
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="100%" height="20"><b><font color="#015CAE">Subsidiaries</font></b><xsl:if test="normalize-space($returnLimitExceeded) = 'Y'"><sup>+</sup></xsl:if>:
                               <font size="1">The following are subsidiaries of the inquired upon business.</font>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
    </xsl:if>

    <xsl:if test="prd:LinkageRecordType/@code = '4' and  not(prd:LinkageRecordType[attribute::code='4']=preceding::prd:LinkageRecordType[attribute::code='4'])">
		<xsl:if test="$countUltimateParent > 0 or $countParent > 0 or $countSubsidiary > 0">
                    <!-- divider line -->
                    <tr>
                      <td bgcolor="#015CAE"><img src="../images/global/spacer.gif" width="0" height="1" /></td>
                    </tr>
		</xsl:if>
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="100%" height="20"><b><font color="#015CAE">Branches / Alternate Addresses</font></b><xsl:if test="normalize-space($returnLimitExceeded) = 'Y'"><sup>++</sup></xsl:if>:
                               <font size="1">The following are branches or alternate addresses of the inquired upon business.</font>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
    </xsl:if>

    <xsl:if test="prd:LinkageRecordType/@code != '1' or normalize-space($matchingBusinessIndicator) = 'N'">
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="20%" valign="top" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
                              <b><xsl:value-of select="$companyName" /></b>
    		<xsl:choose>
    			<xsl:when test="normalize-space($companyAddress) != ''">
                              <br />
                              <xsl:value-of select="$companyAddress" />
    			</xsl:when>
    			<xsl:otherwise>
    			</xsl:otherwise>
    		</xsl:choose>
                              <br />
                              <xsl:value-of select="$cityStateCountry" />
                              <br />
                              BIN: <xsl:value-of select="$BIN" />
		              </font></td></tr>
		            </table>
		          </td>
		        </tr>
    </xsl:if>

    <xsl:if test="position()=last() and $countBranch >= 10 and normalize-space($returnLimitExceeded) = 'Y' ">
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="100%" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                              <b><sup>++</sup></b> present designates there are more than 10 branches or alternate addresses associated to the headquarters business.</font></td>
                          </tr>
                        </table>
                      </td>
                    </tr>
    </xsl:if>

    <xsl:if test="$countSubsidiary >= 10 and normalize-space($returnLimitExceeded) = 'Y' and prd:LinkageRecordType/@code = '3' and  not(prd:LinkageRecordType[attribute::code='3']=following::prd:LinkageRecordType[attribute::code='3']) ">
                    <tr>
                      <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="4">
                          <tr>
                            <td width="100%" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                              <b><sup>+</sup></b> present designates there are more than 10 subsidiaries associated to the headquarters business.</font></td>
                          </tr>
                        </table>
                      </td>
                    </tr>
    </xsl:if>

  </xsl:template>
</xsl:stylesheet>