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
  * IdentifyingInformation template
  *********************************************
  -->
  <xsl:template name="CompanyInformation">
  
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Identifying Information'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="4">
            <tr>
              <td bgcolor="#ffffff">

                <table width="100%" border="0" cellspacing="0" cellpadding="0">

                  <!-- business data section -->  
                  <xsl:choose>		              
                    <xsl:when test="prd:BusinessNameAndAddress">
                      <xsl:apply-templates select="prd:BusinessNameAndAddress" />
                    </xsl:when>
                    <xsl:otherwise>
                      <tr>
                        <td colspan="2">
                          <font color="#ff0000" size="1"><b>No business data available</b></font>
                        </td>
                      </tr>  
                    </xsl:otherwise>
                  </xsl:choose>    
                  <!-- end business data section -->  
                </table>
              </td>
            </tr>
          </table>
          <!-- end inner white box -->
        </td>
      </tr>  
    </table>

  </xsl:template>


  <!--
  *********************************************
  * BusinessNameAndAddress template
  *********************************************
  -->
  <xsl:template match="prd:BusinessNameAndAddress" xml:space="preserve">

    <!-- StreetAddress -->
     <xsl:variable name="streetAddress">
	<xsl:variable name="StreetAddress">
	  <xsl:value-of select="prd:StreetAddress" />
	</xsl:variable>
	
	<xsl:call-template name="convertcase">
	   <xsl:with-param name="toconvert" select="$StreetAddress" />
	   <xsl:with-param name="conversion" select="'proper'" />
	</xsl:call-template>
    </xsl:variable>

    <!-- City -->
     <xsl:variable name="city">
	<xsl:variable name="City">
	  <xsl:value-of select="normalize-space(prd:City)" />
	</xsl:variable>
	
	<xsl:call-template name="convertcase">
	   <xsl:with-param name="toconvert" select="$City" />
	   <xsl:with-param name="conversion" select="'proper'" />
	</xsl:call-template>
    </xsl:variable>

    <!-- SIC -->
     <xsl:variable name="sic">
	<xsl:variable name="SIC">
	  <xsl:value-of select="normalize-space(prd:SIC)" />
	</xsl:variable>

	<xsl:call-template name="convertcase">
	   <xsl:with-param name="toconvert" select="$SIC" />
	   <xsl:with-param name="conversion" select="'proper'" />
	</xsl:call-template>
    </xsl:variable>
    <tr>
	<td width="100%" valign="top" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
	This information is the primary name and address for the business you inquired on. All data in this report pertains to the business.</font></td>
    </tr>
    <tr>
      <td colspan="2">
        <img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/></td>
    </tr>
    
    <tr>
      <!-- company name etc column -->
      <td width="50%" valign="top">
        <font size="1" style="font-family: 'verdana';">
          <b><xsl:value-of select="prd:BusinessName" /></b>
          <br />
          <xsl:value-of select="$streetAddress" /> 
          <br />
          <xsl:value-of select="normalize-space($city)" /><xsl:if test="normalize-space($city) != '' and normalize-space(prd:State) != ''">,</xsl:if> 
          <xsl:value-of select="prd:State" />
          <xsl:text disable-output-escaping="yes"> </xsl:text>
          <xsl:call-template name="FormatZip">
            <xsl:with-param name="value" select="concat(prd:Zip, prd:ZipExtension)" />
          </xsl:call-template>

          <xsl:if test="prd:PhoneNumber">
				  <br />
				  <xsl:call-template name="FormatPhone">
				    <xsl:with-param name="value" select="translate(prd:PhoneNumber, '-', '')" />
				  </xsl:call-template>
          </xsl:if>
        </font>            

        <xsl:if test="($product = 'BPR' or $product = 'CIBPR') and ../prd:ConsumerStatement">
          <br /><img src="../images/spacer.gif" border="0" width="1" height="8" /><br />
          <font color="#ff0000" size="1" style="FONT-FAMILY: 'verdana';">
          <b>Business Statement on File</b>
          <a href="#BusinessStatement">details</a>
			  </font>
        </xsl:if>

        <xsl:if test="$product = 'CI' and ../prd:IntelliscoreScoreInformation/prd:CustomerDispute/prd:Statement/@code = 'Y'">
          <br /><img src="../images/spacer.gif" border="0" width="1" height="8" /><br />
          <font color="#ff0000" size="1" style="FONT-FAMILY: 'verdana';">
          <b>Business Statement on File</b></font>
        </xsl:if>

      </td>
      <!-- end company name etc column -->
      
      <!-- file number etc column -->
      <td width="50%" valign="top">

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td colspan="2">
              <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
	            <td width="40%" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	              <b>Business Identification Number:</b></font></td>
	            <td width="60%" align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	              <xsl:value-of select="prd:ExperianFileNumber" /></font></td>
          	   </tr>
          	 </table>
            </td>
          </tr>

          <xsl:if test="$product != 'CIBPR'">
	          <xsl:if test="../prd:IntelliscoreScoreInformation/prd:ProfileNumber">
	            <tr>
	              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <b>Full Report Number:</b></font></td>
	              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                FR-<xsl:value-of select="../prd:IntelliscoreScoreInformation/prd:ProfileNumber" /></font></td>
	            </tr>
	          </xsl:if>
	
	          <xsl:if test="not(../prd:IntelliscoreScoreInformation/prd:ProfileNumber) and ../prd:BusinessSummary/prd:TransactionNumber">
	            <tr>
	              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <b>Full Report Number:</b></font></td>
	              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                FR-<xsl:value-of select="../prd:BusinessSummary/prd:TransactionNumber" /></font></td>
	            </tr>
	          </xsl:if>
          </xsl:if>

          <tr>
            <td width="40%" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
              <b>Experian File Established:</b></font></td>
            <td width="60%" align="right" nowrap="nowrap">
              <font size="1" style="font-family: 'verdana';">
              <xsl:choose>		              
                <xsl:when test="prd:FileEstablishFlag/@code = 'P'">
	                 Prior to 01/1977
                </xsl:when>

                <xsl:otherwise>
                  <xsl:if test="prd:FileEstablishDate">
                    <xsl:call-template name="FormatDate">
                      <xsl:with-param name="pattern" select="'mo/year'" />
                      <xsl:with-param name="value" select="prd:FileEstablishDate" />
                    </xsl:call-template>
                  </xsl:if>
                </xsl:otherwise>
                
              </xsl:choose>    
              </font>
            </td> 
          </tr>

          <xsl:if test="../prd:ExecutiveElements/prd:YearofIncorporation">
            <tr>
              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
                <b>Date of Incorporation:</b></font></td>
              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
		     <xsl:with-param name="value" select="../prd:ExecutiveElements/prd:YearofIncorporation" />
		   </xsl:call-template>
                </font></td>
            </tr>
          </xsl:if>

          <xsl:if test="prd:SIC">
            <tr>
              <td colspan="2">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
	            <tr>
	              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <b>SIC Code:</b></font></td> 
	              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
	                <xsl:value-of select="translate($sic, 'amp;amp;', 'amp;')" /><xsl:if test="prd:SIC/@code != ''"> - <xsl:value-of select="prd:SIC/@code" /></xsl:if></font></td>
	            </tr>
                </table>
              </td>
            </tr>
          </xsl:if>

          <xsl:if test="..//prd:ExecutiveElements/prd:TaxID">
            <tr>
              <td nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
                <b>Tax ID:</b></font></td>
              <td align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
                <xsl:value-of select="concat(substring(..//prd:ExecutiveElements/prd:TaxID, 1, 2), '-', substring(..//prd:ExecutiveElements/prd:TaxID, 3))" /></font></td>
            </tr>
          </xsl:if>

        </table>

      </td>
      <!-- end file number etc column -->

    </tr>

  </xsl:template>


  <!--
  ***********************************************
  * ProprietorNameAndAddress template
  ***********************************************
  -->
  <xsl:template match="prd:ProprietorNameAndAddress" xml:space="preserve">
    <xsl:variable name="ownerCount">
      <xsl:value-of select="count(../prd:ProprietorNameAndAddress)" />
    </xsl:variable>

    <tr>
      <!-- owner name etc column -->
      <td width="50%" valign="top">
        <font size="1" style="font-family: 'verdana';">
          <xsl:value-of select="prd:ProprietorName" />
          <br />
          <xsl:value-of select="prd:StreetAddress" /> 
          <br />
          <xsl:value-of select="normalize-space(prd:City)" /><xsl:if test="prd:City and prd:State">, </xsl:if>
          <xsl:value-of select="prd:State" />
          <xsl:text disable-output-escaping="yes"> </xsl:text>
          <xsl:value-of select="prd:Zip" /> 
        </font>            
      </td>
      <!-- end owner name etc column -->
      
      <!-- SSN etc column -->
      <td width="50%" valign="top">

        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <xsl:if test="prd:SSN">
            <tr>
              <td width="40%" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
                <b>Principal
                
                SSN:</b></font></td>
              <td width="60%" align="right" nowrap="nowrap"><font size="1" style="font-family: 'verdana';">
                <xsl:value-of select="concat('XXX-X', substring(prd:SSN, 5, 1), '-', substring(prd:SSN, 6))" /></font></td>
            </tr>
          </xsl:if>

        </table>

      </td>
      <!-- end SSN etc column -->

    </tr>

    <xsl:if test="position() &lt; $ownerCount">
      <!-- space row -->
      <tr>
        <td colspan="2">
          <img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/></td>
      </tr>  
                    
      <!-- divider line -->
      <tr>
        <td height="1" bgcolor="{$borderColor}" colspan="2"><img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
      </tr>

      <!-- space row -->
      <tr>
        <td colspan="2">
          <img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/></td>
      </tr>  
      
    </xsl:if>

  </xsl:template>

</xsl:stylesheet>
