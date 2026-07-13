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

  <xsl:variable name="yAxisHeightMon" select="100" />
  <xsl:variable name="xAxisWidthMon" select="270" />
  <xsl:variable name="topMarginMon" select="18" />
  <xsl:variable name="leftMarginMon" select="19" />
  <xsl:variable name="barWidthMon" select="30" />
  <xsl:variable name="betweenBarsMon" select="8" />

  <!--
  *********************************************
  * MonthlyDBT template
  *********************************************
  -->
  <xsl:template name="MonthlyDBT">
    <xsl:variable name="total" select="count(prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:PriorMonth) + count(prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth) + count(prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:ContinouslyReportedTradeLines/prd:DBT)" />

    <xsl:variable name="month">
      <xsl:value-of select="number(substring(prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth/prd:Date, 5, 2)) - 1" />
    </xsl:variable>

    <xsl:variable name="startYear">
      <xsl:choose>		              
        <xsl:when test="$month &lt; 7">		    		   		   
          <xsl:value-of select="number(substring(prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth/prd:Date, 1, 4)) - 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(substring(prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth/prd:Date, 1, 4))" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
    <xsl:variable name="startMonth">
      <xsl:choose>		              
        <xsl:when test="$month - 6 &lt; 0">		    		   		   
          <xsl:value-of select="$month - 5 + 12" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$month - 5" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
    <!-- begin Monthly DBT trends -->
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">

          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">

                  <tr>
                    <td height="23" bgcolor="{$borderColor}" align="left" colspan="2" valign="middle">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <font color="#ffffff"><b>7 month DBT trends</b></font>
                    </td>
                  </tr>

                  <tr>
                    <td width="1%" valign="bottom">
                      <img src="../images/spacer.gif" border="0" width="15" height="1" alt=""/></td>

                    <td width="99%" align="left">
                      <table bgcolor="#ffffff" width="{$xAxisWidthMon + 20}" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <!-- Y axis labels -->
                          <td width="{$leftMarginMon}">
                            <table width="{$leftMarginMon}" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td align="center" width="{$leftMarginMon}" height="{$topMarginMon}"> </td>
                              </tr>

                              <tr>
                                <td align="center" valign="top" height="34"><font size="1" style="FONT-FAMILY: 'verdana';">90+</font></td>
                              </tr>

                              <tr>
                                <td align="center" valign="top" height="33"><font size="1" style="FONT-FAMILY: 'verdana';">60</font></td>
                              </tr>

                              <tr>
                                <td align="center" valign="top" height="33"><font size="1" style="FONT-FAMILY: 'verdana';">30</font></td>
                              </tr>

                            </table>
                          </td>

                          <!-- Y axis line -->
                          <td width="1">
                            <table width="1" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td width="1" height="{$topMarginMon}" valign="bottom"><img src="../images/spacer.gif" border="0" width="1" height="{$topMarginMon -1}" alt=""/></td>
                              </tr>

                              <tr>
                                <td width="1" height="{$yAxisHeightMon}" valign="bottom" bgcolor="{$borderColor}"><img src="../images/spacer.gif" border="0" width="1" height="{$yAxisHeightMon}" alt=""/></td>
                              </tr>
                            </table>
                          </td>

                          <!-- chat area -->
                          <td width="{$xAxisWidthMon}">
                            <table width="{$xAxisWidthMon}" border="0" cellspacing="0" cellpadding="0">
                              <tr>

                                <xsl:if test="$total &lt; 7">
                                  <xsl:call-template name="NAMonthLoop">
                                    <xsl:with-param name="startMonth" select="$startMonth" />
                                    <xsl:with-param name="startYear" select="$startYear" />
                                    <xsl:with-param name="times" select="7 - $total" />
                                  </xsl:call-template>
                                </xsl:if>		    		   		   

                                <!-- Bar template -->
                                <xsl:apply-templates select="prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:PriorMonth" mode="bars">
                                  <xsl:sort order="descending" select="position()" />
                                </xsl:apply-templates>                  
                                
                                <xsl:apply-templates select="prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth" mode="bars" />
              
                                <xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:ContinouslyReportedTradeLines" mode="bars" />

                                <td width="6"> </td>

                              </tr>
                            </table>
                          </td>
                          <!-- end chat area -->
                        </tr>


                        <tr>
                          <td height="1"><img src="../images/spacer.gif" border="0" width="{$leftMarginMon}" height="1" alt=""/></td>
                          <td colspan="2" height="1" bgcolor="{$borderColor}"><img src="../images/spacer.gif" border="0" height="1" alt=""/></td>
                        </tr>

                        <tr>
                          <td align="center"><font size="1" style="FONT-FAMILY: 'verdana';">0</font></td>
                          <td width="1"><img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
                          <td>

                            <table width="{$xAxisWidthMon}" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td width="2" valign="bottom"><img src="../images/spacer.gif" border="0" width="2" height="1" alt=""/></td>

                                <xsl:if test="$total &lt; 7">
                                  <xsl:call-template name="NAMonthLoop">
                                    <xsl:with-param name="type" select="'dates'" />
                                    <xsl:with-param name="startMonth" select="$startMonth" />
                                    <xsl:with-param name="startYear" select="$startYear" />
                                    <xsl:with-param name="times" select="7 - $total" />
                                  </xsl:call-template>
                                </xsl:if>		    		   		   

                                <!-- DBT dates template -->
                                <xsl:apply-templates select="prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:PriorMonth" mode="dbtDates">
                                  <xsl:sort order="descending" select="position()" />
                                </xsl:apply-templates>                  
                                
                                <xsl:apply-templates select="prd:PaymentTrends[not(prd:ITIPConsolidatedNumber)]/prd:CurrentMonth" mode="dbtDates" />

                                <xsl:apply-templates select="prd:PaymentTotals[not(prd:ITIPConsolidatedNumber)]/prd:ContinouslyReportedTradeLines" mode="dbtDates" />

                              </tr>
                            </table>


                          </td>
                        </tr>

                      </table>
                    </td>
                  </tr>

                  <tr>
                    <td valign="bottom" colspan="2">
                      <img src="../images/spacer.gif" border="0" width="1" height="5" alt=""/></td>
                  </tr>

                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    <!-- end Monthly DBT trends -->

  </xsl:template>
  
  
  
  <!--
  *********************************************
  * CurrentMonth | PriorMonth template
  * mode = bars
  *********************************************
  -->
  <xsl:template match="prd:CurrentMonth | prd:PriorMonth | prd:ContinouslyReportedTradeLines" mode="bars" xml:space="preserve">

    <xsl:variable name="monthlyDBT">
      <xsl:choose>

        <xsl:when test="prd:DBT">		    		   		   
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <td width="{$betweenBarsMon}"> </td>

    <td width="{$barWidthMon}">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td valign="middle" height="{$yAxisHeightMon - $monthlyDBT + $topMarginMon}" align="center"><img src="../images/spacer.gif" border="0" width="1" height="{$yAxisHeightMon - $monthlyDBT + $topMarginMon - 30}" alt=""/><br /><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$monthlyDBT" /></b></font></td>
        </tr>
        <tr>
          <td height="{normalize-space($monthlyDBT)}" bgcolor="#ff944c"><img src="../images/spacer.gif" border="0" width="1" height="{normalize-space($monthlyDBT)}" alt=""/></td>
        </tr>
      </table>
    </td>
  
  </xsl:template>
    
  
  <!--
  *********************************************
  * CurrentMonth | PriorMonth template
  * mode = DBT dates
  *********************************************
  -->
  <xsl:template match="prd:CurrentMonth | prd:PriorMonth | prd:ContinouslyReportedTradeLines" mode="dbtDates" xml:space="preserve">

    <xsl:variable name="tmpMonth">
	<xsl:value-of select="number(substring(prd:Date, 5, 2)) - 1" />
    </xsl:variable>
	
    <xsl:variable name="tmpYear">
	<xsl:choose>		              
	  <xsl:when test="number($tmpMonth) = 0">		    		   		   
	    <xsl:value-of select="number(substring(prd:Date, 1, 4)) - 1" />
	  </xsl:when>
	
	  <xsl:otherwise>
	    <xsl:value-of select="number(substring(prd:Date, 1, 4))" />
	  </xsl:otherwise>
	</xsl:choose>    
    </xsl:variable>

    <xsl:variable name="newMonth">
	<xsl:choose>		              
	  <xsl:when test="number($tmpMonth) = 0">		    		   		   
	    <xsl:value-of select="12" />
	  </xsl:when>
	
	  <xsl:otherwise>
	    <xsl:value-of select="$tmpMonth" />
	  </xsl:otherwise>
	</xsl:choose>    
    </xsl:variable>

    <xsl:variable name="dateReported">
      <xsl:choose>		              
        <xsl:when test="name() = 'ContinouslyReportedTradeLines'">
          <xsl:value-of select="'Current'" />
        </xsl:when>

        <xsl:when test="prd:Date">
          <xsl:variable name="month">
            <xsl:call-template name="FormatMonth">
      		    <xsl:with-param name="monthValue" select="number($newMonth)" />
      		  </xsl:call-template>
          </xsl:variable>		    		   		   

          <xsl:value-of select="concat(normalize-space($month), substring(normalize-space($tmpYear), 3, 2))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>

    </xsl:variable>

    <td width="40" align="center" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="normalize-space($dateReported)" /></font></td>
  
  </xsl:template>



  <!--
  *********************************************
  * NAMonthLoop template
  *********************************************
  -->
  <xsl:template name="NAMonthLoop">
    <xsl:param name="times" select="7" />
    <xsl:param name="startMonth" select="0" />
    <xsl:param name="startYear" select="0" />
    <xsl:param name="type" select="'bars'" />
    <xsl:param name="index" select="0" />
    
    <xsl:variable name="nextMonth">
      <xsl:choose>		              
        <xsl:when test="($startMonth + 1) &gt; 12">		    		   		   
          <xsl:value-of select="$startMonth - 12 + 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$startMonth + 1" />
        </xsl:otherwise>
      </xsl:choose>    
      
    </xsl:variable>    
    
    <xsl:variable name="nextYear">
      <xsl:choose>		              
        <xsl:when test="($startMonth + 1) &gt; 12">		    		   		   
          <xsl:value-of select="$startYear + 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="normalize-space($startYear)" />
        </xsl:otherwise>
      </xsl:choose>    
          
    </xsl:variable>

    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="$type = 'bars'">		    		   		   
          <xsl:value-of select="'N/A'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:variable name="month">
            <xsl:call-template name="FormatMonth">
      		    <xsl:with-param name="monthValue" select="number($startMonth)" />
      		  </xsl:call-template>
          </xsl:variable>		    		   		   

          <xsl:choose>		              
            <xsl:when test="$times = 7 and $index = $times - 1">		    		   		   
              <xsl:value-of select="'Current'" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="concat(normalize-space($month), substring($startYear, 3, 2))" />
            </xsl:otherwise>
          </xsl:choose>    

        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:choose>		              
      <xsl:when test="$type = 'bars'">		    		   		   
        <td width="{$betweenBarsMon}"> </td>
        <td width="{$barWidthMon}" align="left" valign="middle"><img src="../images/spacer.gif" border="0" width="1" height="80" alt=""/><br /><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$result" /></b></font></td>
      </xsl:when>

      <xsl:otherwise>
        <td width="40" align="center" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$result" /></font></td>
      </xsl:otherwise>
    </xsl:choose>    
  
    <!-- Test condition and call template if less than number -->
    <xsl:if test="$index + 1 &lt; $times">
      <xsl:call-template name="NAMonthLoop">
        <xsl:with-param name="index" select="($index + 1)" />
        <xsl:with-param name="type" select="$type" />
        <xsl:with-param name="startMonth" select="$nextMonth" />
        <xsl:with-param name="startYear" select="$nextYear" />
        <xsl:with-param name="times" select="$times" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>  
    
</xsl:stylesheet>
