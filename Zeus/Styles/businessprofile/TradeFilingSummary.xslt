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
  * TradeFilingSummary template
  *********************************************
  -->
  <xsl:template name="TradeFilingSummaryBPR">

    <xsl:call-template name="FilingSummaryBPR" />
    <br />
    <xsl:call-template name="TradeSummaryBPR" />

  </xsl:template>


  <!--
  *********************************************
  * FilingSummary template
  *********************************************
  -->
  <xsl:template name="FilingSummaryBPR">
    <xsl:variable name="limitedReport">
      <xsl:choose>                    
        <xsl:when test="prd:ExecutiveElements">                            
          <xsl:value-of select="0" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="1" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="scorable">
       <xsl:choose>
         <xsl:when test="$product = 'CI' ">
           <xsl:choose>
             <xsl:when test="not (prd:IntelliscoreScoreInformation) or prd:IntelliscoreScoreInformation/prd:LimitedProfile/@code = 'Y' ">
                <xsl:value-of select="'false'" />
             </xsl:when>
    
             <xsl:otherwise>
                <xsl:value-of select="'true'" />
             </xsl:otherwise>
           </xsl:choose>
         </xsl:when>
         
         <xsl:otherwise>
           <xsl:choose>
             <xsl:when test="not (prd:ExecutiveSummary) and not(prd:PaymentTrends) and not(prd:IndustryPaymentTrends) and not(prd:QuarterlyPaymentTrends) ">
                <xsl:value-of select="'false'" />
             </xsl:when>
    
             <xsl:otherwise>
                <xsl:value-of select="'true'" />
             </xsl:otherwise>
           </xsl:choose>
         </xsl:otherwise>
       </xsl:choose>
    </xsl:variable>

    <xsl:variable name="bankruptcy">
      <xsl:choose>
        <xsl:when test="$limitedReport = 1">
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="number(prd:ExecutiveElements/prd:BankruptcyCount) = 0 and prd:ExecutiveElements/prd:BankruptcyFlag and prd:ExecutiveElements/prd:BankruptcyFlag = 'Y'">        
          <xsl:value-of select="'Closed'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(prd:ExecutiveElements/prd:BankruptcyCount)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:variable name="bankruptcyDates">
      <xsl:choose>                    
        <xsl:when test="$bankruptcy &gt; 0 and prd:ExecutiveElements/prd:EarliestBankruptcyDate and number(prd:ExecutiveElements/prd:EarliestBankruptcyDate) != 0">
        
          <xsl:variable name="oldDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="pattern" select="'mo/yr'" />
              <xsl:with-param name="value" select="prd:ExecutiveElements/prd:EarliestBankruptcyDate" />
            </xsl:call-template>
          </xsl:variable>
        
          <xsl:variable name="recentDate">
            <xsl:choose>                      
              <xsl:when test="prd:ExecutiveElements/prd:MostRecentBankruptcyDate and number(prd:ExecutiveElements/prd:MostRecentBankruptcyDate) != 0">
                   <xsl:call-template name="FormatDate">
                     <xsl:with-param name="pattern" select="'mo/yr'" />
                     <xsl:with-param name="value" select="prd:ExecutiveElements/prd:MostRecentBankruptcyDate" />
                   </xsl:call-template>
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>                
          </xsl:variable>
                           
          <xsl:choose>                    
            <xsl:when test="normalize-space($oldDate) != normalize-space($recentDate) and normalize-space($recentDate) != ''">                             
              <xsl:value-of select="concat('(FILED ', normalize-space($oldDate), '-', normalize-space($recentDate), ')')" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat('(FILED ', normalize-space($oldDate), ')')" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="lien">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="number(prd:ExecutiveElements/prd:TaxLienCount) = 0 and prd:ExecutiveElements/prd:TaxLienFlag and prd:ExecutiveElements/prd:TaxLienFlag = 'Y'">                             
          <xsl:value-of select="'Released'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(prd:ExecutiveElements/prd:TaxLienCount)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="lienDates">
      <xsl:choose>                    
        <xsl:when test="$lien &gt; 0 and prd:ExecutiveElements/prd:EarliestTaxLienDate and number(prd:ExecutiveElements/prd:EarliestTaxLienDate) != 0">       
        
          <xsl:variable name="oldDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="pattern" select="'mo/yr'" />
              <xsl:with-param name="value" select="prd:ExecutiveElements/prd:EarliestTaxLienDate" />
            </xsl:call-template>
          </xsl:variable>
        
          <xsl:variable name="recentDate">
            <xsl:choose>                      
              <xsl:when test="prd:ExecutiveElements/prd:MostRecentTaxLienDate and number(prd:ExecutiveElements/prd:MostRecentTaxLienDate) != 0">                               
                   <xsl:call-template name="FormatDate">
                     <xsl:with-param name="pattern" select="'mo/yr'" />
                     <xsl:with-param name="value" select="prd:ExecutiveElements/prd:MostRecentTaxLienDate" />
                   </xsl:call-template>
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>                
          </xsl:variable>
                           
          <xsl:choose>                    
            <xsl:when test="normalize-space($oldDate) != normalize-space($recentDate) and normalize-space($recentDate) != ''">                             
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), '-', normalize-space($recentDate), ')')" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), ')')" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="judgment">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="number(prd:ExecutiveElements/prd:JudgmentCount) = 0 and prd:ExecutiveElements/prd:JudgmentFlag and prd:ExecutiveElements/prd:JudgmentFlag = 'Y'">                              
          <xsl:value-of select="'Satisfied'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(prd:ExecutiveElements/prd:JudgmentCount)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="judgmentDates">
      <xsl:choose>                    
        <xsl:when test="$judgment &gt; 0 and prd:ExecutiveElements/prd:EarliestJudgmentDate and number(prd:ExecutiveElements/prd:EarliestJudgmentDate) != 0">         
        
          <xsl:variable name="oldDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="pattern" select="'mo/yr'" />
              <xsl:with-param name="value" select="prd:ExecutiveElements/prd:EarliestJudgmentDate" />
            </xsl:call-template>
          </xsl:variable>
        
          <xsl:variable name="recentDate">
            <xsl:choose>                      
              <xsl:when test="prd:ExecutiveElements/prd:MostRecentJudgmentDate and number(prd:ExecutiveElements/prd:MostRecentJudgmentDate) != 0">                             
                   <xsl:call-template name="FormatDate">
                     <xsl:with-param name="pattern" select="'mo/yr'" />
                     <xsl:with-param name="value" select="prd:ExecutiveElements/prd:MostRecentJudgmentDate" />
                   </xsl:call-template>
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>                
          </xsl:variable>
                           
          <xsl:choose>                    
            <xsl:when test="normalize-space($oldDate) != normalize-space($recentDate) and normalize-space($recentDate) != ''">                             
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), '-', normalize-space($recentDate), ')')" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), ')')" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalCollection">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(prd:ExecutiveElements/prd:CollectionCount)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="collectionDates">
      <xsl:choose>                    
        <xsl:when test="$totalCollection &gt; 0 and prd:ExecutiveElements/prd:EarliestCollectionDate and number(prd:ExecutiveElements/prd:EarliestCollectionDate) != 0">          
        
          <xsl:variable name="oldDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="pattern" select="'mo/yr'" />
              <xsl:with-param name="value" select="prd:ExecutiveElements/prd:EarliestCollectionDate" />
            </xsl:call-template>
          </xsl:variable>
        
          <xsl:variable name="recentDate">
            <xsl:choose>                      
              <xsl:when test="prd:ExecutiveElements/prd:MostRecentCollectionDate and number(prd:ExecutiveElements/prd:MostRecentCollectionDate) != 0">                             
                   <xsl:call-template name="FormatDate">
                     <xsl:with-param name="pattern" select="'mo/yr'" />
                     <xsl:with-param name="value" select="prd:ExecutiveElements/prd:MostRecentCollectionDate" />
                   </xsl:call-template>
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>                
          </xsl:variable>
                           
          <xsl:choose>                    
            <xsl:when test="normalize-space($oldDate) != normalize-space($recentDate) and normalize-space($recentDate) != ''">                             
              <xsl:value-of select="concat('(PLACED ', normalize-space($oldDate), '-', normalize-space($recentDate), ')')" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat('(PLACED  ', normalize-space($oldDate), ')')" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="sumLegal">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'$0'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:LegalBalance">                               
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:LegalBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="ucc">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(prd:ExecutiveElements/prd:UCCFilings)" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="uccDates">
      <xsl:choose>                    
        <xsl:when test="$ucc &gt; 0 and prd:ExecutiveElements/prd:EarliestUCCDate and number(prd:ExecutiveElements/prd:EarliestUCCDate) != 0">        
        
          <xsl:variable name="oldDate">
            <xsl:call-template name="FormatDate">
              <xsl:with-param name="pattern" select="'mo/yr'" />
              <xsl:with-param name="value" select="prd:ExecutiveElements/prd:EarliestUCCDate" />
            </xsl:call-template>
          </xsl:variable>
        
          <xsl:variable name="recentDate">
            <xsl:choose>                      
              <xsl:when test="prd:ExecutiveElements/prd:MostRecentUCCDate and number(prd:ExecutiveElements/prd:MostRecentUCCDate) != 0">                               
                   <xsl:call-template name="FormatDate">
                     <xsl:with-param name="pattern" select="'mo/yr'" />
                     <xsl:with-param name="value" select="prd:ExecutiveElements/prd:MostRecentUCCDate" />
                   </xsl:call-template>
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>                
          </xsl:variable>
                           
          <xsl:choose>                    
            <xsl:when test="normalize-space($oldDate) != normalize-space($recentDate) and normalize-space($recentDate) != ''">                             
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), '-', normalize-space($recentDate), ')')" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat('(FILED  ', normalize-space($oldDate), ')')" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="derogUCC">
      <xsl:choose>
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'No'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:UCCDerogatoryCount and number(prd:ExecutiveElements/prd:UCCDerogatoryCount) &gt; 0">                             
          <xsl:value-of select="'Yes**'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'No'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
   
  
    <!-- begin legal filings and collections -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Legal Filings and Collections</b></font>
        </td>
      </tr>  
          
      <tr>
        <td width="100%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                               
            <tr>  
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Bankruptcy filings: <xsl:value-of select="$bankruptcyDates" /></td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$bankruptcy" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
               <xsl:choose>
                  <xsl:when test="$product = 'CI' or (($product = 'CIBPR' or $product = 'BPR') and $scorable = 'false')">
                     Tax lien filings:
                  </xsl:when>

                  <xsl:otherwise>
                     Tax lien filings: <xsl:if test="string(number($lien)) != 'NaN' and number($lien) &gt; 0"><a href="#taxliens"><font size="1">details</font></a></xsl:if>
                  </xsl:otherwise>
               </xsl:choose>
               <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$lienDates" /></td>

                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$lien" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
               <xsl:choose>
                  <xsl:when test="$product = 'CI' or (($product = 'CIBPR' or $product = 'BPR') and $scorable = 'false')">
                     Judgment filings:
                  </xsl:when>

                  <xsl:otherwise>
                     Judgment filings: <xsl:if test="string(number($judgment)) != 'NaN' and number($judgment) &gt; 0"><a href="#judgments"><font size="1">details</font></a></xsl:if>
                  </xsl:otherwise>
               </xsl:choose>
               <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$judgmentDates" /></td>

                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$judgment" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
               <xsl:choose>
                  <xsl:when test="$product = 'CI' or (($product = 'CIBPR' or $product = 'BPR') and $scorable = 'false')">
                     Total collections:
                  </xsl:when>

                  <xsl:otherwise>
                     Total collections:  <xsl:if test="string(number($totalCollection)) != 'NaN' and number($totalCollection) &gt; 0"><a href="#collections"><font size="1">details</font></a></xsl:if>
                  </xsl:otherwise>
               </xsl:choose>
               <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$collectionDates" /></td>

                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$totalCollection" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Sum of legal filings:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$sumLegal" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
               <xsl:choose>
                  <xsl:when test="$product = 'CI' or (($product = 'CIBPR' or $product = 'BPR') and $scorable = 'false')">
                     UCC filings:
                  </xsl:when>

                  <xsl:otherwise>
                     UCC filings:  <xsl:if test="string(number($ucc)) != 'NaN' and number($ucc) &gt; 0"><a href="#uccfilings"><font size="1">details</font></a></xsl:if>
                  </xsl:otherwise>
               </xsl:choose>
               <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text><xsl:value-of select="$uccDates" /></td>

                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$ucc" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Cautionary UCC filings present?</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$derogUCC" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

          </table>
        </td>
      </tr>    
    </table>
    <!-- end legal filings and collections -->
    
  </xsl:template>


  <!--
  *********************************************
  * TradeSummary template
  *********************************************
  -->
  <xsl:template name="TradeSummaryBPR">
    <xsl:variable name="limitedReport">
      <xsl:choose>                    
        <xsl:when test="prd:ExecutiveElements">                            
          <xsl:value-of select="0" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="1" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="scorable">
       <xsl:choose>                   
         <xsl:when test="not (prd:IntelliscoreScoreInformation) or prd:IntelliscoreScoreInformation/prd:LimitedProfile/@code = 'Y' ">                              
            <xsl:value-of select="'false'" />
         </xsl:when>

         <xsl:otherwise>
            <xsl:value-of select="'true'" />
         </xsl:otherwise>
       </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="DBT">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:CurrentDBT) and (string(number(prd:ExecutiveElements/prd:CurrentDBT)) != 'NaN')">                               
          <xsl:value-of select="number(prd:ExecutiveElements/prd:CurrentDBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="monthlyDBT">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:MonthlyAverageDBT) and (string(number(prd:ExecutiveElements/prd:MonthlyAverageDBT)) != 'NaN')">                             
          <xsl:value-of select="number(prd:ExecutiveElements/prd:MonthlyAverageDBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="high6MonthDBT">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:HighestDBT6Months) and (string(number(prd:ExecutiveElements/prd:HighestDBT6Months)) != 'NaN')">                             
          <xsl:value-of select="number(prd:ExecutiveElements/prd:HighestDBT6Months)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="high5QuarterDBT">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'0'" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:HighestDBT5Quarters) and (string(number(prd:ExecutiveElements/prd:HighestDBT5Quarters)) != 'NaN')">                             
          <xsl:value-of select="number(prd:ExecutiveElements/prd:HighestDBT5Quarters)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="continuousTrade">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'Not on File'" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:ActiveTradelineCount) and (string(number(prd:ExecutiveElements/prd:ActiveTradelineCount)) != 'NaN')">                               
          <xsl:value-of select="number(prd:ExecutiveElements/prd:ActiveTradelineCount)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="continuousTradeBalance">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'Not on File'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:CurrentAccountBalance">                              
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:CurrentAccountBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalTrade">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="''" />
        </xsl:when>

        <xsl:when test="(prd:ExecutiveElements/prd:AllTradelineCount) and (string(number(prd:ExecutiveElements/prd:AllTradelineCount)) != 'NaN')">                             
          <xsl:value-of select="concat('(',number(prd:ExecutiveElements/prd:AllTradelineCount), ')')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'(0)'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="totalTradeBalance">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'$0'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:AllTradelineBalance">                            
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:AllTradelineBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="last5QuarterBalance">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'$0'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:AverageBalance5Quarters">                            
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:AverageBalance5Quarters, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="recentHighCredit">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'$0'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:SingleLineHighCredit">                               
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:SingleLineHighCredit, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="balanceRange">
      <xsl:choose>                    
        <xsl:when test="$limitedReport = 1">                               
          <xsl:value-of select="'$0 - $0'" />
        </xsl:when>

        <xsl:when test="prd:ExecutiveElements/prd:LowBalance6Months">                              
          <xsl:value-of select="concat(format-number(prd:ExecutiveElements/prd:LowBalance6Months, '$###,###,##0'), ' - ', format-number(prd:ExecutiveElements/prd:HighBalance6Months, '$###,###,##0'))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0 - $0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <!-- begin trade information -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Trade Information</b></font>
        </td>
      </tr>  
          
      <tr>
        <td width="100%" valign="top">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">

            <xsl:if test="$product = 'CI'">
                <tr>
                  <td height="20">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td width="95%">
                          Current Days Beyond Terms (DBT):</td>
                        <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$DBT" /></b></td>
                      </tr>
                    </table>
                  </td>  
                </tr>
            </xsl:if>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Monthly average DBT:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$monthlyDBT" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Highest DBT previous 6 months:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$high6MonthDBT" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Highest DBT previous 5 quarters:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$high5QuarterDBT" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Total continuous trades:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$continuousTrade" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Current continuous trade balance:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$continuousTradeBalance" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Trade balance of all trades <xsl:value-of select="$totalTrade" />:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$totalTradeBalance" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      Average balance previous 5 quarters:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$last5QuarterBalance" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      <xsl:choose>
                        <xsl:when test="$product = 'CI' or ($product = 'CIBPR' and $scorable = 'false')">
                          Highest credit amount extended:
                        </xsl:when>

                        <xsl:otherwise>
                           Highest credit amount extended:  <xsl:if test="normalize-space($recentHighCredit) != '$0'"><a href="#highestcredit"><font size="1">details</font></a></xsl:if>
                        </xsl:otherwise>
                      </xsl:choose>
                      <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text></td>

                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$recentHighCredit" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>

            <tr>
              <td height="20">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="95%">
                      6 month balance range:</td>
                    <td width="5%" align="right" nowrap="nowrap"><b><xsl:value-of select="$balanceRange" /></b></td>
                  </tr>
                </table>
              </td>  
            </tr>
          </table>
        </td>
      </tr>    
    </table>
    <!-- end trade information -->
    
  </xsl:template>

</xsl:stylesheet>