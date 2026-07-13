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
  * ProfileSummary template
  *********************************************
  -->
  <xsl:template name="ProfileSummary">
  
    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Executive Summary'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
    
      <tr>
        <!-- trade payment status column -->
        <td width="49%" valign="top">

	    <xsl:choose>		              
	      <xsl:when test="not (prd:ExecutiveSummary) and not(prd:PaymentTrends) and not(prd:IndustryPaymentTrends) and not(prd:QuarterlyPaymentTrends) ">
	         <xsl:call-template name="ResearchTips" />
	      </xsl:when>
	
	      <xsl:otherwise>
	         <xsl:call-template name="ExecSummary" />
	      </xsl:otherwise>
	    </xsl:choose>

        </td>
    
        <td width="2%">
        </td>
    
        <!-- legal, trade counts etc. column -->
        <td width="49%" valign="top">

          <xsl:call-template name="TradeFilingSummaryBPR" />
          <br />
          
          <xsl:if test="prd:ExecutiveSummary ">
            <!-- PerformanceAnalysis -->
            <xsl:call-template name="PerformanceAnalysis" />
            <!-- br / -->
          </xsl:if>
          
        </td>
      </tr>

      <xsl:if test="prd:ExecutiveSummary and prd:ExecutiveElements">
        <tr>
          <td colspan="3">
            <font size="1"><i>
            * Days Beyond Terms (DBT) is a dollar weighted calculation of the average number of 
              days that payment was made beyond the invoice due date based on trades on file that 
              have been updated in the previous 3 months.
            </i></font>

            <xsl:choose>		              
              <xsl:when test="prd:ExecutiveElements/prd:UCCDerogatoryCount and number(prd:ExecutiveElements/prd:UCCDerogatoryCount) &gt; 0">
                <br /><img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/><br />
              </xsl:when>
  
              <xsl:otherwise>
                <br /><br />
              </xsl:otherwise>
            </xsl:choose>   
          </td>
        </tr>
      </xsl:if>
        
      <xsl:if test="prd:ExecutiveElements/prd:UCCDerogatoryCount and number(prd:ExecutiveElements/prd:UCCDerogatoryCount) &gt; 0">
        <tr>
          <td colspan="3">
            <font size="1"><i>
            ** Cautionary UCC Filings include one or more of the following collateral:<br/>
            Accounts, Accounts Receivables, Contract Rights, Hereafter Acquired Property, Inventory, Leases, Notes Receivable or Proceeds. 
            </i></font>
            <br /><br />
          </td>
        </tr>
      </xsl:if>    
          
    </table>

    <xsl:if test="prd:CollectionData or prd:Bankruptcy or prd:TaxLien or prd:JudgmentOrAttachmentLien">
      <!-- back to top graphic -->
      <xsl:call-template name="BackToTop" />
    </xsl:if>

  </xsl:template>
    
  
  <!--
  *********************************************
  * ExecSummary template
  *********************************************
  -->
  <xsl:template name="ExecSummary">

	<!-- CurrentDBT -->
	<xsl:if test="prd:ExecutiveSummary">
		 <xsl:call-template name="CurrentDBT" />
		 <br />
	</xsl:if>
	
	<!-- QuarterlyDBT -->
	<xsl:if test="prd:QuarterlyPaymentTrends">
		 <xsl:call-template name="QuarterlyDBT" />
		 <br />
	</xsl:if>
	
	<!-- MonthlyDBT -->
	<xsl:if test="prd:PaymentTrends">
		 <xsl:call-template name="MonthlyDBT" />
		 <!-- br / -->
	</xsl:if>

  </xsl:template>
    
  
  <!--
  *********************************************
  * ResearchTips template
  *********************************************
  -->
  <xsl:template name="ResearchTips">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
        
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td height="23" bgcolor="{$borderColor}" align="left" valign="middle">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <font color="#ffffff" size="2"><b>Research Tips</b></font>
                    </td>
                  </tr>  

                  <tr>
                    <td height="10" valign="bottom">
                      <img src="../images/spacer.gif" border="0" width="1" height="9" alt=""/></td>
                  </tr>  
                </table>

                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/></td>
                      
                    <td width="98%">  
                      <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                   
                        <tr>  
                          <td colspan="2">
              
                            Additional information may be available on this business.
                            <br /><br />
<!--
                            Other recommended searches:<br />
                            <br />
-->
                          </td>
                        </tr>

<!--
                        <tr>
                          <td align="right"><img src="../images/ball_bullet_small.gif" border="0" width="6" height="6" alt=""/></td>
                          <td>
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>Order Small Business Intelliscore<br />
                          </td>
                        </tr>
              
                        <tr>
                          <td width="2%" align="right"><img src="../images/ball_bullet_small.gif" border="0" width="6" height="6" alt=""/></td>
                          <td width="98%">
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>Order Business Owner Profile
                          </td>
                        </tr>
              
                        <tr>
                          <td align="right"><img src="../images/ball_bullet_small.gif" border="0" width="6" height="6" alt=""/></td>
                          <td>
                            <xsl:text disable-output-escaping="yes">&amp;nbsp;</xsl:text>Order Public Record<br />
                          </td>
                        </tr>
              
                        <tr>
                          <td colspan="2">
                            <img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/></td>
                        </tr>
-->
                      </table> 
                    </td>
                    <td width="1%">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/></td>
                  </tr>    
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

</xsl:stylesheet>