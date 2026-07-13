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
  <xsl:template name="IdentifyingInformationBOP">
    <xsl:param name="index" />
    <xsl:param name="underline" select="1" />    
    <xsl:param name="titleSize" select="3" />    

    <xsl:variable name="href">
      <xsl:value-of select="concat('#BOP', $index - 1) " />
    </xsl:variable>

    <xsl:if test="contains($product, 'BOP') and normalize-space($product) != 'BOP' ">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td valign="top" height="20" align="center"><font size="{$titleSize}" color="{$titleColor}"><b><a name="{$href}"><xsl:if test="boolean(number($underline))"><xsl:text disable-output-escaping="yes">&lt;u&gt;</xsl:text></xsl:if>Business Owner Profile<xsl:if test="boolean(number($underline))"><xsl:text disable-output-escaping="yes">&lt;/u&gt;</xsl:text></xsl:if></a></b></font></td>
        </tr>
      </table>
    </xsl:if>

    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Identifying Information'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <!-- inquiry  -->
    <xsl:variable name="inquiry">
      <xsl:if test="$product = 'BOP' ">
         <xsl:value-of select="../prd:BusinessProfile/prd:InputSummary/prd:Inquiry" />
      </xsl:if>
      <xsl:if test="$product = 'SBIBOP' or $product = 'IPBOP' or $product = 'IPBPRBOP'">
         <xsl:value-of select="../prd:Intelliscore/prd:InputSummary/prd:Inquiry" />
      </xsl:if>
    </xsl:variable>

    <!-- inqf bin  -->
    <xsl:variable name="inqfBin">
    	<xsl:value-of select="substring-before(substring-after($inquiry, 'INQF/'), '/')" />
    </xsl:variable>

    <!-- narq name  -->
    <xsl:variable name="narqName">
    	<xsl:value-of select="substring-before(substring-after($inquiry, 'NARQ/'), ';')" />
    </xsl:variable>

    <!-- narq address  -->
    <xsl:variable name="narqAddr1">
    	<xsl:value-of select="substring-before(substring-after($inquiry, 'CA-'), '/')" />
    </xsl:variable>

    <!-- narq city state zip  -->
    <xsl:variable name="narqAddr2">
    	<xsl:value-of select="substring-before(substring-after(substring-after($inquiry, 'CA-'), '/'), '/') " />
    </xsl:variable>

    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">

                <table width="100%" border="0" cellspacing="0" cellpadding="1">
                  <tr>
                    <!-- signatory etc column -->
                    <td width="38%" valign="top">
                      <font size="1" style="FONT-FAMILY: 'verdana';">
                        <b><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="prd:ConsumerIdentity/prd:Name/prd:Gen" /></b>
                        <br />

                        <xsl:apply-templates select="prd:AddressInformation[position() &lt; 4]" />

                      </font></td>
                    <!-- end signatory  etc column -->

                    <!-- SSN etc column -->
                    <td width="25%" valign="top">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <td width="23%" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
                            <b>SSN: </b></font></td>
                          <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	                        <xsl:choose>		              
	                          <xsl:when test="prd:SSN and prd:SSN/prd:VariationIndicator/@code = ' '">		    		   		   
	                            <xsl:value-of select="concat('XXX-XX-', substring(prd:SSN[prd:VariationIndicator/@code = ' ']/prd:Number, 6))" />
	                          </xsl:when>
	
	                          <xsl:otherwise>
	                            <xsl:value-of select="''" />
	                          </xsl:otherwise>
	                        </xsl:choose>
	                      </font>
                          </td>
                        </tr>

                        <xsl:if test="prd:ConsumerIdentity and ((prd:ConsumerIdentity[1]/prd:DOB and normalize-space(prd:ConsumerIdentity[1]/prd:DOB) != '') or (prd:ConsumerIdentity[1]/prd:YOB and normalize-space(prd:ConsumerIdentity[1]/prd:YOB) != ''))">
                          <xsl:choose>		              
                            <xsl:when test="prd:ConsumerIdentity[1]/prd:DOB">
	                       <tr>
	                         <td nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	                           <b>DOB:</b></font></td>
	                         <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">

	                         <xsl:call-template name="FormatDate">
	                          	<xsl:with-param name="pattern" select="'mo/dt/year'" />
	                          	<xsl:with-param name="value" select="prd:ConsumerIdentity[1]/prd:DOB" />
	                          	<xsl:with-param name="yearDigit" select="4" />
	                          	<xsl:with-param name="isYearLast" select="true()" />
	                         </xsl:call-template>
	                         </font></td>
                              </tr>
                            </xsl:when>

                            <xsl:otherwise>
	                       <tr>
	                         <td nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	                           <b>YOB:</b></font></td>
	                         <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	                           <xsl:value-of select="prd:ConsumerIdentity[1]/prd:YOB" /></font></td>
                              </tr>
                            </xsl:otherwise>
                          </xsl:choose>    
                        </xsl:if>
                      </table>
                    </td>
                    <!-- end SSN etc column -->

                    <!-- employer etc column -->
                    <td width="37%" valign="top">
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">

				<xsl:choose>
				  <xsl:when test="prd:EmploymentInformation">
				    <xsl:apply-templates select="prd:EmploymentInformation" />
				  </xsl:when>
				  <xsl:otherwise>
		                    <tr>
		                      <td>
		                        <img src="../images/spacer.gif" width="1" height="1" border="0" alt="" /></td>
		                    </tr>
				  </xsl:otherwise>
				</xsl:choose>
<!--
                        <xsl:apply-templates select="prd:EmploymentInformation" />
-->
                      </table>
                    </td>
                    <!-- end employer etc column -->

                  </tr>

                  <tr>
                    <td colspan="3">
                      <img src="../images/spacer.gif" border="0" width="1" height="1" alt="" /></td>
                  </tr>
                </table>

                <xsl:if test="starts-with($inquiry, 'NARQ') ">
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <!-- Divider Line -->
                    <tr>
                      <td colspan="3" bgcolor="{$borderColor}">
                        <img src="../images/spacer.gif" width="1" height="1" border="0" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3" valign="top"><font color="{$borderColor}" size="1" style="FONT-FAMILY: 'verdana';">
                       <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*</b></font><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$narqName" /></b>

                       <xsl:if test="normalize-space($narqAddr1)">
                            <br />
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;&amp;nbsp;</xsl:text><xsl:value-of select="$narqAddr1" />
                       </xsl:if>

                       <xsl:if test="normalize-space($narqAddr2)">
                            <br />
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;&amp;nbsp;</xsl:text><xsl:value-of select="$narqAddr2" />
                       </xsl:if>

                        </font>
                      </td>
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="10" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3"><font color="{$borderColor}" size="1" style="FONT-FAMILY: 'verdana';">
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*</b>Business name and address reflect the inquiry information and have not been verified by Experian.</font></td>
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /></td>
                    </tr>

                  </table>
                </xsl:if>

                <xsl:if test="false()"> <!--  starts-with($inquiry, 'INQF') " -->
                  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <!-- Divider Line -->
                    <tr>
                      <td colspan="3" bgcolor="{$borderColor}">
                        <img src="../images/spacer.gif" width="1" height="1" border="0" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3" valign="top"><font color="{$borderColor}" size="1" style="FONT-FAMILY: 'verdana';">
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*</b>
<!--
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*Business ID Number: <xsl:value-of select="$inqfBin" /></b>
-->
                        </font>
                      </td>
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="10" alt="" /></td>
                    </tr>

                    <tr>
                      <td colspan="3"><font color="{$borderColor}" size="1" style="FONT-FAMILY: 'verdana';">
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*</b>Business name and address reflect the inquiry information and have not been verified by Experian.</font></td>
<!--
                        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><b>*</b>Business ID Number reflects the inquiry information and has not been verified by Experian.</font></td>
-->
                    </tr>

                    <tr>
                      <td colspan="3">
                        <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /></td>
                    </tr>

                  </table>
                </xsl:if>

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
  * AddressInformation template
  *********************************************
  -->
  <xsl:template match="prd:AddressInformation" xml:space="preserve">

    <!-- prd:Zip field -->
    <xsl:variable name="zip">
       <xsl:value-of select="prd:Zip"/>
    </xsl:variable>

    <!-- prd:Zip field length -->
    <xsl:variable name="zipLength">
       <xsl:value-of select="string-length(normalize-space(prd:Zip))"/>
    </xsl:variable>

<!--
         <xsl:when test="number(substring(normalize-space(prd:Zip), string-length(prd:Zip)-8)) != 'NaN' and number(substring(normalize-space(prd:Zip), string-length(prd:Zip)-8)) > 0">
           <xsl:value-of select="concat(substring(normalize-space(prd:Zip), 1, string-length(prd:Zip)-4), '-', substring(normalize-space(prd:Zip), string-length(prd:Zip)-3) ) " />
-->

    <!-- get year from date extension -->
    <xsl:variable name="formattedZip">
       <xsl:choose>
         <xsl:when test="string(number(substring(normalize-space($zip), $zipLength - 8))) != 'NaN' and number(substring(normalize-space($zip), $zipLength - 8)) > 0">         
           <xsl:choose>
             <xsl:when test="$zipLength &gt; 12">
               <xsl:value-of select="concat(substring(normalize-space($zip), 1, $zipLength - 13), ', ', substring(normalize-space($zip), $zipLength - 11, 8), '-', substring(normalize-space($zip), $zipLength - 3) ) " />
             </xsl:when>
             <xsl:otherwise>
               <xsl:value-of select="concat(substring(normalize-space($zip), $zipLength - 11, 8), '-', substring(normalize-space($zip), $zipLength - 3) ) " />
             </xsl:otherwise>
           </xsl:choose>    
         </xsl:when>

         <xsl:when test="substring(normalize-space($zip), $zipLength - 5, 1) = ' ' and string(number(substring(normalize-space($zip), $zipLength - 4))) != 'NaN' and number(substring(normalize-space($zip), $zipLength - 4)) > 0">
           <xsl:choose>
             <xsl:when test="$zipLength &gt; 8">
               <xsl:value-of select="concat(substring(normalize-space($zip), 1, $zipLength - 9), ', ', substring(normalize-space($zip), $zipLength - 7) ) " />
             </xsl:when>
             <xsl:otherwise>
               <xsl:value-of select="substring(normalize-space($zip), $zipLength - 7)  " />
             </xsl:otherwise>
           </xsl:choose>    
         </xsl:when>

         <xsl:otherwise>
           <xsl:value-of select="$zip" />
         </xsl:otherwise>
       </xsl:choose>    
    </xsl:variable>

    <xsl:if test="position() &gt; 1">
      <br />
    </xsl:if>

    <xsl:if test="string-length(normalize-space(prd:StreetSuffix)) &gt; 2">
      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="normalize-space(prd:StreetSuffix)" /> 
      <br />
    </xsl:if>
    <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="normalize-space($formattedZip)" /> 
    <br />
      
  </xsl:template>


  <!--
  *********************************************
  * EmploymentInformation template
  *********************************************
  -->
  <xsl:template match="prd:EmploymentInformation" xml:space="preserve">

    <xsl:variable name="lastDate">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/year'" />
	     <xsl:with-param name="value" select="normalize-space(concat(substring(prd:LastUpdatedDate, 3, 2), substring(prd:LastUpdatedDate, 1, 2), '00'))" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="false()" />
	   </xsl:call-template>
    </xsl:variable>

    <tr>
      <xsl:choose>
        <xsl:when test="position() = 1">
	   <td align="right" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
	     <b>Employer:<xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;</xsl:text></b></font></td>
	 </xsl:when>

	 <xsl:otherwise>
	   <td>
	     <img src="../images/spacer.gif" border="0" width="16" height="1" alt="" /></td>
	 </xsl:otherwise>
      </xsl:choose>    

    <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
      <xsl:value-of select="prd:Name" /></font></td>
    </tr>

    <xsl:if test="prd:AddressFirstLine and normalize-space(prd:AddressFirstLine) != '' ">
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="16" height="1" alt="" /></td>
        <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
          <xsl:value-of select="prd:AddressFirstLine" />
            <xsl:if test="not (prd:AddressSecondLine) and not (AddressExtraLine) and prd:Zip and normalize-space(prd:Zip) != '' "><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="prd:Zip" /></xsl:if>
        </font></td>
      </tr>
    </xsl:if>

    <xsl:if test="prd:AddressSecondLine and normalize-space(prd:AddressSecondLine) != '' ">
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="16" height="1" alt="" /></td>
        <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
          <xsl:value-of select="prd:AddressSecondLine" />
            <xsl:if test="not (AddressExtraLine) and prd:Zip and normalize-space(prd:Zip) != '' "><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="prd:Zip" /></xsl:if>
          </font></td>
      </tr>
    </xsl:if>

    <xsl:if test="prd:AddressExtraLine and normalize-space(prd:AddressExtraLine) != '' ">
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="16" height="1" alt="" /></td>
        <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
          <xsl:value-of select="prd:AddressExtraLine" />
            <xsl:if test="prd:Zip and normalize-space(prd:Zip) != '' "><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="prd:Zip" /></xsl:if>
          </font></td>
      </tr>
    </xsl:if>

    <xsl:if test="$lastDate and normalize-space($lastDate) != '' ">
      <tr>
        <td>
          <img src="../images/spacer.gif" border="0" width="16" height="1" alt="" /></td>
        <td align="left" nowrap="nowrap"><font size="1" style="FONT-FAMILY: 'verdana';">
          <b>Last Updated:<xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;</xsl:text></b><xsl:value-of select="$lastDate" />
          </font></td>
      </tr>
    </xsl:if>

    <tr>
      <td colspan="2">
        <img src="../images/spacer.gif" border="0" width="1" height="10" alt="" /></td>
    </tr>

  </xsl:template>

</xsl:stylesheet>