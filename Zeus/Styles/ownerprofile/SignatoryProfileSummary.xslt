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
  * SignatoryProfileSummary template
  *********************************************
  -->
  <xsl:template name="SignatoryProfileSummary">
    <xsl:param name="position" />
    
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Executive Summary'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    
      <!-- ProfileSummary template -->
      <xsl:apply-templates select="prd:ProfileSummary">
        <xsl:with-param name="position" select="$position" />
      </xsl:apply-templates>       
    </table>

  </xsl:template>
  
  
  <!--
  *********************************************
  * ProfileSummary template
  *********************************************
  -->
  <xsl:template match="prd:ProfileSummary" xml:space="preserve">
    <xsl:param name="position" />

    <tr>
      <!-- trade payment status column -->
      <td width="49%" valign="top">
        <xsl:call-template name="ScorexPlusScore">
           <xsl:with-param name="position" select="$position" />
        </xsl:call-template>
        <br />
        <xsl:call-template name="TradePaymentStatus" />
        <br />
        <xsl:call-template name="DelinquencyDetail" />
      </td>

      <td width="2%" rowspan="2">
        
      </td>

      <!-- legal, trade counts etc. column -->
      <td width="49%" rowspan="2" valign="top">
        <xsl:call-template name="LegalFilingsInquiries" />
      </td>
    </tr>

  </xsl:template>


  <!--
  *********************************************
  * ScorexPlus Score template
  *********************************************
  -->
  <xsl:template name="ScorexPlusScore">
    <xsl:param name="position" />

    <!-- scorexplus score  -->
    <xsl:variable name="scorexplusScore">
      <xsl:choose>
        <xsl:when test="(contains($product, 'BOP') and not(contains($product, 'SBI')) and not(contains($product, 'IP'))) and ../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk and string(number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk)) != 'NaN'">
          <xsl:value-of select="number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk)" />
        </xsl:when>

        <xsl:when test="(contains($product, 'SBI') or contains($product, 'IP')) and ../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk and string(number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk)) != 'NaN'">
          <xsl:value-of select="number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:DerivedRisk)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <!-- negative score factor 1  -->
    <xsl:variable name="riskFactorCode1">
       <xsl:choose>
         <xsl:when test="(contains($product, 'BOP') and not(contains($product, 'SBI')) and not(contains($product, 'IP'))) and ../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode1">
	  	<xsl:value-of select="number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode1)" />
         </xsl:when>

         <xsl:when test="(contains($product, 'SBI') or contains($product, 'IP')) and ../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode1">
	  	<xsl:value-of select="number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode1)" />
         </xsl:when>

         <xsl:otherwise>
	  	<xsl:value-of select="''" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <!-- negative score factor 2  -->
    <xsl:variable name="riskFactorCode2">
       <xsl:choose>
         <xsl:when test="(contains($product, 'BOP') and not(contains($product, 'SBI')) and not(contains($product, 'IP'))) and ../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode2">
	  	<xsl:value-of select="number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode2)" />
         </xsl:when>

         <xsl:when test="(contains($product, 'SBI') or contains($product, 'IP')) and ../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode2">
	  	<xsl:value-of select="number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode2)" />
         </xsl:when>

         <xsl:otherwise>
	  	<xsl:value-of select="''" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <!-- negative score factor 3  -->
    <xsl:variable name="riskFactorCode3">
       <xsl:choose>
         <xsl:when test="(contains($product, 'BOP') and not(contains($product, 'SBI')) and not(contains($product, 'IP'))) and ../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode3">
	  	<xsl:value-of select="number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode3)" />
         </xsl:when>

         <xsl:when test="(contains($product, 'SBI') or contains($product, 'IP')) and ../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode3">
	  	<xsl:value-of select="number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode3)" />
         </xsl:when>

         <xsl:otherwise>
	  	<xsl:value-of select="''" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <!-- negative score factor 4  -->
    <xsl:variable name="riskFactorCode4">
       <xsl:choose>
         <xsl:when test="(contains($product, 'BOP') and not(contains($product, 'SBI')) and not(contains($product, 'IP'))) and ../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode4">
	  	<xsl:value-of select="number(../../prd:BusinessProfile/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode4)" />
         </xsl:when>

         <xsl:when test="(contains($product, 'SBI') or contains($product, 'IP')) and ../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode4">
	  	<xsl:value-of select="number(../../prd:Intelliscore/prd:SmallBusinessAdvisorySummary[$position]/prd:RiskScoreFactorCode4)" />
         </xsl:when>

         <xsl:otherwise>
	  	<xsl:value-of select="''" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <xsl:variable name="unscorable">
       <xsl:choose>
         <xsl:when test="number($scorexplusScore) >= 9000">
	  	<xsl:value-of select="'true'" />
         </xsl:when>

         <xsl:otherwise>
	  	<xsl:value-of select="'false'" />
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>
  
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
        
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="3" align="left" valign="middle" height="20">
                      <b><font color="#ffffff"><xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;</xsl:text>ScorexPLUS<sup>SM</sup> score</font></b>
                    </td>
                  </tr>  

                  <tr bgcolor="#ffffff">
                    <td colspan="3"><img src="../images/spacer.gif" border="0" width="1" height="3" alt=""/></td>
                  </tr>  

                  <tr bgcolor="#ffffff">
                    <td width="2%">
                    	<img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
                    <td width="98%" colspan="2" align="left" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
                    	<b>ScorexPLUS<sup>SM</sup> score: </b><xsl:value-of select="$scorexplusScore" /></font></td>
                  </tr>  

                  <tr bgcolor="#ffffff">
                    <td colspan="3"><img src="../images/spacer.gif" border="0" width="1" height="3" alt=""/></td>
                  </tr>  

                  <tr bgcolor="#ffffff">
                    <td colspan="3" align="center">
                      <table cellspacing="0" cellpadding="0">
                        <tr>
                          <td><font size="1" style="FONT-FAMILY: 'verdana';">
                              <b>Risk Category</b></font></td>
                          <td>
                            <img src="../images/global/spacer.gif" border="0" width="10" height="1" alt="" /></td>
                          <td><font size="1" style="FONT-FAMILY: 'verdana';">
                              <b>Score Range</b></font></td>
                        </tr>
                        <tr>
                          <td align="left" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
                              Low</font></td>
                          <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                              780 - 900</font></td>
                        </tr>
                        <tr>
                          <td align="left" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
                              Low - Medium</font></td>
                          <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                              681 - 779</font></td>
                        </tr>
                        <tr>
                          <td align="left" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
                              Medium</font></td>
                          <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                              620 - 680</font></td>
                        </tr>
                        <tr>
                          <td align="left" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
                              Medium - High</font></td>
                          <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                              521 - 619</font></td>
                        </tr>
                        <tr>
                          <td align="left" colspan="2"><font size="1" style="FONT-FAMILY: 'verdana';">
                              High</font></td>
                          <td align="left"><font size="1" style="FONT-FAMILY: 'verdana';">
                              300 - 520</font></td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                  
                  <tr bgcolor="#ffffff">
                    <td colspan="3">
                      <img src="../images/global/spacer.gif" border="0" width="1" height="3" alt="" /></td>
                  </tr>  

	          <xsl:choose>
	          	<xsl:when test="$unscorable = 'true'">
	                  <tr bgcolor="#ffffff">
	                    <td width="2%">
	                      <img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
	                    <td width="98%" align="left" colspan="2" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
	                      <b>Unscorable due to the following reason:</b></font></td>
	                  </tr>

	                  <tr bgcolor="#ffffff">
	                    <td colspan="3">
	                      <img src="../images/global/spacer.gif" border="0" width="1" height="2" alt="" /></td>
	                  </tr>

			    <tr>
			      <td bgcolor="#e5f5fa" width="2%">
			        <img src="../images/global/spacer.gif" border="0" width="1" height="22" alt="" /></td>
			      <td bgcolor="#e5f5fa" width="2%">
			        <img src="../images/global/spacer.gif" border="0" width="1" alt="" /></td>
			      <td colspan="1" bgcolor="#e5f5fa" align="left" valign="middle"><font size="1" style="FONT-FAMILY: 'verdana';">
			        <img src="../images/ball_bullet_small.gif" border="0" width="6" height="6" alt="" />
			        <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>
			        <xsl:call-template name="translateUnscorableScorexPlus">
			        	<xsl:with-param name="code" select="number($scorexplusScore)" />
			        </xsl:call-template>
			        </font></td>
			    </tr>
	             	</xsl:when>

	          	<xsl:when test="not(normalize-space($riskFactorCode1)) and not(normalize-space($riskFactorCode2)) and not(normalize-space($riskFactorCode3)) and not(normalize-space($riskFactorCode4)) ">
	             	</xsl:when>

	             	<xsl:otherwise>
	                  <tr bgcolor="#ffffff">
	                    <td width="2%">
	                      <img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
	                    <td width="98%" align="left" colspan="2" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';">
	                      <b>Negative Score Factors</b></font></td>
	                  </tr>

	                  <tr bgcolor="#ffffff">
	                    <td colspan="3">
	                      <img src="../images/global/spacer.gif" border="0" width="1" height="2" alt="" /></td>
	                  </tr>

	                  <xsl:if test="string(number($riskFactorCode1)) != 'NaN' and number($riskFactorCode1) > 0 ">
		                   <xsl:call-template name="NegativeScoreFactors">
			                   	<xsl:with-param name="code" select="number($riskFactorCode1)" />
			                   	<xsl:with-param name="index" select="1" />
		                   </xsl:call-template>
	                  </xsl:if>

	                  <xsl:if test="string(number($riskFactorCode2)) != 'NaN' and number($riskFactorCode2) > 0 ">
		                   <xsl:call-template name="NegativeScoreFactors">
			                   	<xsl:with-param name="code" select="number($riskFactorCode2)" />
			                   	<xsl:with-param name="index" select="2" />
		                   </xsl:call-template>
	                  </xsl:if>

	                  <xsl:if test="string(number($riskFactorCode3)) != 'NaN' and number($riskFactorCode3) > 0 ">
		                   <xsl:call-template name="NegativeScoreFactors">
			                   	<xsl:with-param name="code" select="number($riskFactorCode3)" />
			                   	<xsl:with-param name="index" select="3" />
		                   </xsl:call-template>
	                  </xsl:if>

	                  <xsl:if test="string(number($riskFactorCode4)) != 'NaN' and number($riskFactorCode4) > 0 ">
		                   <xsl:call-template name="NegativeScoreFactors">
			                   	<xsl:with-param name="code" select="number($riskFactorCode4)" />
			                   	<xsl:with-param name="index" select="4" />
		                   </xsl:call-template>
	                  </xsl:if>
	          	</xsl:otherwise>
	          </xsl:choose>

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
  * ConsumerAdverseAction template
  *********************************************
  -->
  <xsl:template name="NegativeScoreFactors">
    <xsl:param name="code" select="''" />
    <xsl:param name="index" select="''" />

    <!-- negative score factor description  -->
    <xsl:variable name="factorDescription">
    	<xsl:call-template name="translateArfScorexRiskFactorTable">
    		<xsl:with-param name="code" select="number($code)" />
    	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="bgColor">
      <xsl:choose>
        <xsl:when test="$index mod 2 = 1">
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td bgcolor="{normalize-space($bgColor)}" width="2%">
        <img src="../images/global/spacer.gif" border="0" width="1" height="22" alt="" /><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td bgcolor="{normalize-space($bgColor)}" width="2%">
        <img src="../images/global/spacer.gif" border="0" height="3" width="1" alt="" /><img src="../images/ball_bullet_small.gif" border="0" width="6" height="6" alt="" /><xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>
      <td colspan="1" bgcolor="{normalize-space($bgColor)}" align="left" valign="middle"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="normalize-space($factorDescription)" /></font></td>
    </tr>

  </xsl:template>


  <!--
  *********************************************
  * TradePaymentStatus template
  *********************************************
  -->
  <xsl:template name="TradePaymentStatus">
    <xsl:variable name="oldestDate">
	   <xsl:call-template name="FormatDate">
	     <xsl:with-param name="pattern" select="'mo/year'" />
	     <xsl:with-param name="value" select="normalize-space(concat(substring(prd:OldestTradeOpenDate, 3, 2), substring(prd:OldestTradeOpenDate, 1, 2), '00'))" />
	     <xsl:with-param name="yearDigit" select="2" />
	     <xsl:with-param name="isYearLast" select="false()" />
	   </xsl:call-template>
       
    </xsl:variable>

    <xsl:variable name="paidTrades">
      <xsl:choose>		              
        <xsl:when test="prd:PaidAccounts">		    		   		   
          <xsl:value-of select="number(prd:PaidAccounts)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="satisfactoryTrades">
      <xsl:choose>		              
        <xsl:when test="prd:SatisfactoryAccounts">		    		   		   
          <xsl:value-of select="number(prd:SatisfactoryAccounts)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="previousDelinquent">
      <xsl:choose>		              
        <xsl:when test="prd:WasDelinquentDerog">		    		   		   
          <xsl:value-of select="number(prd:WasDelinquentDerog)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="presentDelinquent">
      <xsl:choose>		              
        <xsl:when test="prd:NowDelinquentDerog">		    		   		   
          <xsl:value-of select="number(prd:NowDelinquentDerog)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalTrades">
      <xsl:choose>		              
        <xsl:when test="prd:TotalTradeItems">		    		   		   
          <xsl:value-of select="number(prd:TotalTradeItems)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">

                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">

                  <tr>
                    <td colspan="2" bgcolor="{$borderColor}" align="left" valign="middle" height="20">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt="" />
                      <b><font color="#ffffff">Trade Payment Status</font></b></td>
                  </tr>  
                </table>

                <!-- Trade Payment Status data -->
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr height="20">
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" /></td>
                    <td bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Oldest trade opened</b></font>
                    </td>
                    <td bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$oldestDate" /></font>
                    </td>
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

                  <tr height="20">
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                    <td bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Paid trades</b></font>
                    </td>
                    <td bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$paidTrades" /></font>
                    </td>
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

                  <tr height="20">
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                    <td bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Satisfactory trades</b></font>
                    </td>

                    <td bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$satisfactoryTrades" /></font>
                    </td>
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

                  <tr height="20">
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                    <td bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Previously delinquent/derogatory</b></font>
                    </td>

                    <td bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$previousDelinquent" /></font>
                    </td>
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

                  <tr height="20">
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                    <td bgcolor="#ffffff" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Presently delinquent/derogatory</b></font>
                    </td>

                    <td bgcolor="#ffffff" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$presentDelinquent" /></font>
                    </td>
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

                  <tr height="20">
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                    <td bgcolor="#e5f5fa" align="left"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Total trades</b></font>
                    </td>

                    <td bgcolor="#e5f5fa" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$totalTrades" /></font>
                    </td>
                    <td bgcolor="#e5f5fa" width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" /></td>
                  </tr>

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
  * DelinquencyDetail template
  *********************************************
  -->
  <xsl:template name="DelinquencyDetail">
    <xsl:variable name="satisfactoryTrades">
      <xsl:choose>		              
        <xsl:when test="prd:SatisfactoryAccounts">		    		   		   
          <xsl:value-of select="number(prd:SatisfactoryAccounts)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="past30">
      <xsl:choose>		              
        <xsl:when test="prd:DelinquenciesOver30Days and string(number(prd:DelinquenciesOver30Days)) != 'NaN'">		    		   		   
          <xsl:value-of select="number(prd:DelinquenciesOver30Days)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="past60">
      <xsl:choose>		              
        <xsl:when test="prd:DelinquenciesOver60Days and string(number(prd:DelinquenciesOver60Days )) != 'NaN'">		    		   		   
          <xsl:value-of select="number(prd:DelinquenciesOver60Days)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="past90">
      <xsl:choose>		              
        <xsl:when test="prd:DelinquenciesOver90Days and string(number(prd:DelinquenciesOver90Days )) != 'NaN'">		    		   		   
          <xsl:value-of select="number(prd:DelinquenciesOver90Days)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- blue box border -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <!-- inner white box -->
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">

                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td height="20" bgcolor="{$borderColor}" align="left" valign="middle">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt="" />
                      <font color="#ffffff" size="2"><b>Delinquency Detail</b></font>
                    </td>
                  </tr>  

                  <tr>
                    <td height="13" valign="bottom">
                      <img src="../images/spacer.gif" border="0" width="1" height="12" alt="" /></td>
                  </tr>  

                  <!-- matrix row -->
                  <tr>
                    <td align="center">
                    
                      <table width="100%" border="0" cellspacing="1" cellpadding="0">

                        <!-- color bar-->
                        <tr>
                          <td>
                            <table width="300" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td height="25" width="75" bgcolor="#0d5b0d" align="center" valign="middle">
                                  <font size="2" color="#ffffff">
                                  <b><xsl:value-of select="$satisfactoryTrades" /></b></font></td>
                               
                                <td height="25" width="75" bgcolor="#af2384" align="center" valign="middle">
                                  <font size="2" color="#ffffff">
                                  <b><xsl:value-of select="$past30" /></b></font></td>
                               
                                <td height="25" width="75" bgcolor="#eb7d11" align="center" valign="middle">
                                  <font size="2" color="#ffffff">
                                  <b><xsl:value-of select="$past60" /></b></font></td>
                               
                                <td height="25" width="75" bgcolor="#ff0000" align="center" valign="middle">
                                  <font size="2" color="#ffffff">
                                  <b><xsl:value-of select="$past90" /></b></font></td>
                              </tr>

                              <tr>
                                <td colspan="4"><img src="../images/spacer.gif" border="0" width="1" height="2" alt="" /></td>
                              </tr>
                              
                              <tr>
                                <td width="75" align="center" valign="top">
                                  <font size="1" style="FONT-FAMILY: 'verdana';"><b>
                                  Satisfactory</b></font></td>

                                <td width="75" align="center" valign="top">
                                  <font size="1" style="FONT-FAMILY: 'verdana';"><b>
                                  30 days<br />past due</b></font></td>

                                <td width="75" align="center" valign="top">
                                  <font size="1" style="FONT-FAMILY: 'verdana';"><b>
                                  60 days<br />past due</b></font></td>

                                <td width="75" align="center" valign="top">
                                  <font size="1" style="FONT-FAMILY: 'verdana';"><b>
                                  90+ days<br />past due</b></font></td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <!-- end color bar-->
                  
                      </table>
                    </td>
                  </tr>                  
                  
                  <tr>
                    <td height="10" valign="bottom">
                      <img src="../images/spacer.gif" border="0" width="1" height="5" alt="" /></td>
                  </tr>  

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
  * LegalFilingsInquiries template
  *********************************************
  -->
  <xsl:template name="LegalFilingsInquiries">

    <xsl:variable name="totalLegal">
      <xsl:choose>		              
        <xsl:when test="prd:PublicRecordsCount and string(number(prd:PublicRecordsCount)) != 'NaN' ">		    		   		   
          <xsl:value-of select="number(prd:PublicRecordsCount)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="pastDueAmount">
      <xsl:choose>		              
        <xsl:when test="prd:PastDueAmount and string(number(prd:PastDueAmount)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number(prd:PastDueAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="inDispute">
      <xsl:choose>		              
        <xsl:when test="prd:DisputedAccountsExcluded and string(number(prd:DisputedAccountsExcluded)) != 'NaN'">		    		   		   
          <xsl:value-of select="number(prd:DisputedAccountsExcluded)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="monthlyPayment">
      <xsl:choose>		              
        <xsl:when test="prd:MonthlyPayment and string(number(prd:MonthlyPayment)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number(prd:MonthlyPayment, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="installmentCount">
      <xsl:value-of select="count(../prd:TradeLine/prd:RevolvingOrInstallment[@code = 'I'])" />
    </xsl:variable>

    <xsl:variable name="installmentBalance">
      <xsl:choose>		              
        <xsl:when test="prd:InstallmentBalance and string(number(prd:InstallmentBalance)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number(prd:InstallmentBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="revolvingCount">
      <xsl:value-of select="count(../prd:TradeLine/prd:RevolvingOrInstallment[@code = 'R'])" />
    </xsl:variable>

    <xsl:variable name="revolvingBalance">
      <xsl:choose>		              
        <xsl:when test="prd:RevolvingBalance and string(number(prd:RevolvingBalance)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number(prd:RevolvingBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="revolvingPercent">
      <xsl:choose>		              
        <xsl:when test="prd:RevolvingAvailablePercent and string(number(prd:RevolvingAvailablePercent)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number((prd:RevolvingAvailablePercent div 100), '##0%')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="realEstateBalance">
      <xsl:choose>		              
        <xsl:when test="prd:RealEstateBalance and string(number(prd:RealEstateBalance)) != 'NaN'">		    		   		   
          <xsl:value-of select="format-number(prd:RealEstateBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalInquiries">
      <xsl:choose>		              
        <xsl:when test="prd:TotalInquiries">		    		   		   
          <xsl:value-of select="number(prd:TotalInquiries)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="previous6MonthInquiries">
      <xsl:choose>		              
        <xsl:when test="prd:InquiriesDuringLast6Months">		    		   		   
          <xsl:value-of select="number(prd:InquiriesDuringLast6Months)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>


    <!-- begin legal filings and collections -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Legal Filings</b></font>
        </td>
      </tr>  
                                   
      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Total legal filings:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$totalLegal" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td><br /></td>
      </tr>
      
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Delinquent Payment Information</b></font>
        </td>
      </tr>  
      
      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Past due amount:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$pastDueAmount" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                In dispute:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$inDispute" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td><br /></td>
      </tr>
      
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Payment Information</b></font>
        </td>
      </tr>  

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Monthly payments:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$monthlyPayment" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Installment loan balance: (<xsl:value-of select="$installmentCount" />)</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$installmentBalance" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Revolving charge balance: (<xsl:value-of select="$revolvingCount" />)</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$revolvingBalance" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Revolving credit percent available:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$revolvingPercent" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Real Estate loan balance:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$realEstateBalance" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td><br /></td>
      </tr>
      
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Inquiries</b></font>
        </td>
      </tr>  

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Total number of inquiries:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$totalInquiries" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

      <tr>
        <td height="20">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="95%">
                Inquiries previous 6 months:</td>
              <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$previous6MonthInquiries" /></b></td>
            </tr>
          </table>
        </td>  
      </tr>

    </table>
    <!-- end legal filings and collections -->
  

  </xsl:template>
  
</xsl:stylesheet>
