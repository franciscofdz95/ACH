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

  <xsl:variable name="yAxisHeightQtr" select="100" />
  <xsl:variable name="xAxisWidthQtr" select="270" />
  <xsl:variable name="topMarginQtr" select="18" />
  <xsl:variable name="leftMarginQtr" select="19" />
  <xsl:variable name="barWidthQtr" select="34" />
  <xsl:variable name="betweenBarsQtr" select="16" />


  <!--
  *********************************************
  * QuarterlyDBT template
  *********************************************
  -->
  <xsl:template name="QuarterlyDBT">

    <xsl:variable name="total" select="count(prd:QuarterlyPaymentTrends/prd:PriorQuarter) + count(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter)" />

    <xsl:variable name="startYear">
      <xsl:choose>		              
        <xsl:when test="$total &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter/prd:YearOfQuarter) - 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="number(substring(prd:BusinessNameAndAddress/prd:ProfileDate, 1, 4)) - 1" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
    <xsl:variable name="startQuarter">
      <xsl:choose>		              
        <xsl:when test="$total &gt; 0">		    		   		   
          <xsl:value-of select="number(prd:QuarterlyPaymentTrends/prd:MostRecentQuarter/prd:QuarterWithinYear/@code)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:variable name="month">
            <xsl:value-of select="number(substring(prd:BusinessNameAndAddress/prd:ProfileDate, 5, 2))" />
          </xsl:variable>
          <xsl:choose>		              
            <xsl:when test="$month &gt;= 1 and $month &lt;= 3">		    		   		   
              <xsl:value-of select="4" />
            </xsl:when>
    
            <xsl:when test="$month &gt;= 4 and $month &lt;= 6">		    		   		   
              <xsl:value-of select="1" />
            </xsl:when>
    
            <xsl:when test="$month &gt;= 7 and $month &lt;= 9">		    		   		   
              <xsl:value-of select="2" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="3" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
  
  <!-- begin Quarterly DBT trends -->
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
                      <font color="#ffffff"><b>Quarterly DBT trends (previous 5 quarters)</b></font>
                    </td>
                  </tr>

                  <tr>
                    <td width="1%" valign="bottom">
                      <img src="../images/spacer.gif" border="0" width="15" height="1" alt=""/></td>

                    <td width="99%" align="left">
                      <table bgcolor="#ffffff" width="{$xAxisWidthQtr + 20}" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                          <!-- Y axis labels -->
                          <td width="{$leftMarginQtr}">
                            <table width="{$leftMarginQtr}" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td align="center" width="{$leftMarginQtr}" height="{$topMarginQtr}"> </td>
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
                                <td width="1" height="{$topMarginQtr}" valign="bottom"><img src="../images/spacer.gif" border="0" width="1" height="{$topMarginQtr -1}" alt=""/></td>
                              </tr>

                              <tr>
                                <td width="1" height="{$yAxisHeightQtr}" valign="bottom" bgcolor="{$borderColor}"><img src="../images/spacer.gif" border="0" width="1" height="{$yAxisHeightQtr}" alt=""/></td>
                              </tr>
                            </table>
                          </td>

                          <!-- chat area -->
                          <td width="{$xAxisWidthQtr}">
                            <table width="{$xAxisWidthQtr}" border="0" cellspacing="0" cellpadding="0">
                              <tr>

                                <xsl:if test="$total &lt; 5">
                                  <xsl:call-template name="NAQuarterLoop">
                                    <xsl:with-param name="startQuarter" select="$startQuarter" />
                                    <xsl:with-param name="startYear" select="$startYear" />
                                    <xsl:with-param name="times" select="5 - $total" />
                                  </xsl:call-template>
                                </xsl:if>		    		   		   

                                <!-- Bar template -->
                                <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:PriorQuarter" mode="bars">
                                  <xsl:sort order="descending" select="position()" />
                                </xsl:apply-templates>                  
                                
                                <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:MostRecentQuarter" mode="bars" />
              
                                <td width="20"> </td>

                              </tr>
                            </table>
                          </td>
                          <!-- end chat area -->
                        </tr>


                        <tr>
                          <td height="1"><img src="../images/spacer.gif" border="0" width="{$leftMarginQtr}" height="1" alt=""/></td>
                          <td colspan="2" height="1" bgcolor="{$borderColor}"><img src="../images/spacer.gif" border="0" height="1" alt=""/></td>
                        </tr>

                        <tr>
                          <td align="center"><font size="1" style="FONT-FAMILY: 'verdana';">0</font></td>
                          <td width="1"><img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
                          <td>

                            <table width="{$xAxisWidthQtr}" border="0" cellspacing="0" cellpadding="0">
                              <tr>

                                <xsl:if test="$total &lt; 5">
                                  <xsl:call-template name="NAQuarterLoop">
                                    <xsl:with-param name="type" select="'dates'" />
                                    <xsl:with-param name="startQuarter" select="$startQuarter" />
                                    <xsl:with-param name="startYear" select="$startYear" />
                                    <xsl:with-param name="times" select="5 - $total" />
                                  </xsl:call-template>
                                </xsl:if>		    		   		   

                                <!-- DBT dates template -->
                                <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:PriorQuarter" mode="dbtDates">
                                  <xsl:sort order="descending" select="position()" />
                                </xsl:apply-templates>                  
                                    
                                <xsl:apply-templates select="prd:QuarterlyPaymentTrends/prd:MostRecentQuarter" mode="dbtDates" />

                                <td width="20"></td>

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
    <!-- end Quarterly DBT trends -->

  </xsl:template>


  <!--
  *********************************************
  * MostRecentQuarter | PriorQuarter template
  * mode = bars
  *********************************************
  -->
  <xsl:template match="prd:MostRecentQuarter | prd:PriorQuarter" mode="bars" xml:space="preserve">

    <xsl:variable name="quarterlyDBT">
      <xsl:choose>		              
        <xsl:when test="prd:DBT">		    		   		   
          <xsl:value-of select="number(prd:DBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <td width="{$betweenBarsQtr}"> </td>

    <td width="{$barWidthQtr}">
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td valign="middle" height="{$yAxisHeightQtr - $quarterlyDBT + $topMarginQtr}" align="center"><img src="../images/spacer.gif" border="0" width="1" height="{$yAxisHeightQtr - $quarterlyDBT + $topMarginQtr - 30}" alt=""/><br /><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$quarterlyDBT" /></b></font></td>
        </tr>
        <tr>
          <td height="{normalize-space($quarterlyDBT)}" bgcolor="#ff944c"><img src="../images/spacer.gif" border="0" width="1" height="{normalize-space($quarterlyDBT)}" alt=""/></td>
        </tr>
      </table>
    </td>
  
  </xsl:template>
    
  
  <!--
  *********************************************
  * MostRecentQuarter | PriorQuarter template
  * mode = DBT dates
  *********************************************
  -->
  <xsl:template match="prd:MostRecentQuarter | prd:PriorQuarter" mode="dbtDates" xml:space="preserve">

    <xsl:variable name="quarter">
      <xsl:choose>		              
        <xsl:when test="prd:YearOfQuarter">		    		   		   
          <xsl:value-of select="concat(prd:QuarterWithinYear/@code, 'Q', substring(prd:YearOfQuarter, 3, 2))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'N/A'" />
        </xsl:otherwise>
      </xsl:choose>    

    </xsl:variable>

    <td width="{$betweenBarsQtr}"> </td>

    <td width="{$barWidthQtr}" align="center" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$quarter" /></font></td>
  
  </xsl:template>
    

  <!--
  *********************************************
  * NAQuarterLoop template
  *********************************************
  -->
  <xsl:template name="NAQuarterLoop">
    <xsl:param name="times" select="5" />
    <xsl:param name="startQuarter" select="0" />
    <xsl:param name="startYear" select="0" />
    <xsl:param name="type" select="'bars'" />
    <xsl:param name="index" select="0" />
    
    <xsl:variable name="nextQuarter">
      <xsl:choose>		              
        <xsl:when test="($startQuarter + 1) &gt; 4">		    		   		   
          <xsl:value-of select="$startQuarter - 4 + 1" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="$startQuarter + 1" />
        </xsl:otherwise>
      </xsl:choose>    
      
    </xsl:variable>    
    
    <xsl:variable name="nextYear">
      <xsl:choose>		              
        <xsl:when test="($startQuarter + 1) &gt; 4">		    		   		   
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
          <xsl:value-of select="concat('Q', $startQuarter,substring($startYear, 3, 2))" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <td width="{$betweenBarsQtr}"> </td>

    <xsl:choose>		              
      <xsl:when test="$type = 'bars'">		    		   		   
        <td width="{$barWidthQtr}" align="center" valign="middle"><img src="../images/spacer.gif" border="0" width="1" height="80" alt=""/><br /><font size="1" style="FONT-FAMILY: 'verdana';"><b><xsl:value-of select="$result" /></b></font></td>
      </xsl:when>

      <xsl:otherwise>
        <td width="{$barWidthQtr}" align="center" valign="top"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$result" /></font></td>
      </xsl:otherwise>
    </xsl:choose>    
  
    <!-- Test condition and call template if less than number -->
    <xsl:if test="$index + 1 &lt; $times">
      <xsl:call-template name="NAQuarterLoop">
        <xsl:with-param name="index" select="($index + 1)" />
        <xsl:with-param name="type" select="$type" />
        <xsl:with-param name="startQuarter" select="$nextQuarter" />
        <xsl:with-param name="startYear" select="$nextYear" />
        <xsl:with-param name="times" select="$times" />
      </xsl:call-template>
    </xsl:if>
  </xsl:template>  
        
</xsl:stylesheet>
