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
  * CurrentDBT template
  *********************************************
  -->
  <xsl:template name="CurrentDBT">
  
    <xsl:variable name="currentDBT">
      <xsl:choose>		              
        <xsl:when test="normalize-space(prd:ExecutiveSummary/prd:BusinessDBT/@code) != ''">		    		   		   
          <xsl:value-of select="number(prd:ExecutiveSummary/prd:BusinessDBT/@code)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="dbtPointer">
      <xsl:choose>		              
        <xsl:when test="$currentDBT &gt;= 0 and $currentDBT &lt;= 15">		    		   		   
          <xsl:value-of select="1" />
        </xsl:when>

        <xsl:when test="$currentDBT &gt;= 16 and $currentDBT &lt;= 50">		    		   		   
          <xsl:value-of select="2" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="3" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="accountBalance">
      <xsl:choose>		              
        <xsl:when test="prd:ExecutiveElements/prd:CurrentAccountBalance">		    		   		   
          <xsl:value-of select="format-number(prd:ExecutiveElements/prd:CurrentAccountBalance, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="allIndustryDBT">
      <xsl:choose>		              
        <xsl:when test="prd:ExecutiveSummary/prd:AllIndustryDBT">		    		   		   
          <xsl:value-of select="number(prd:ExecutiveSummary/prd:AllIndustryDBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="industryDBT">
      <xsl:choose>		              
        <xsl:when test="prd:ExecutiveSummary/prd:IndustryDBT">		    		   		   
          <xsl:value-of select="number(prd:ExecutiveSummary/prd:IndustryDBT)" />
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
                <!-- box header -->
                <table bgcolor="#ffffff" width="100%" border="0" cellspacing="0" cellpadding="0">
                        
                  <tr>
                    <td height="23" bgcolor="{$borderColor}" align="left" valign="middle" colspan="3">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <font color="#ffffff"><b>Current DBT range compared to all industries*</b></font>
                    </td>
                  </tr>  

                  <tr>
                    <td height="10" valign="bottom" colspan="3">
                      <img src="../images/spacer.gif" border="0" width="1" height="9" alt=""/></td>
                  </tr>  

                  <tr>
                    <td colspan="3" align="center">
                      <table width="300" border="0" cellspacing="0" cellpadding="0">
                        <tr>

                        <xsl:choose>		              
                          <xsl:when test="normalize-space($currentDBT) != ''">		    		   		   
                            <xsl:choose>		              
                              <xsl:when test="$dbtPointer = 1">		    		   		   
                                <td width="240" align="center" nowrap="nowrap"><font size="3"><b><xsl:value-of select="$currentDBT" /> DBT</b></font><br />
                                  (<xsl:value-of select="$accountBalance" /> Balance)</td>
                                <td width="60"></td>
                              </xsl:when>
                      
                              <xsl:otherwise>
                                <td width="300" align="right">
                                  <table border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                      <td align="center" nowrap="nowrap">
                                       <font size="3"><b><xsl:value-of select="$currentDBT" /> DBT</b></font><br />
                                       (<xsl:value-of select="$accountBalance" /> Balance)</td>
                                    </tr>
                                  </table>  
                                </td>
                              </xsl:otherwise>
                            </xsl:choose>    
    
                          </xsl:when>
                  
                          <xsl:otherwise>
                            <td width="300" align="center" nowrap="nowrap"><b>Current DBT cannot be calculated.</b><br />
                              <img src="../images/spacer.gif" border="0" width="1" height="12" alt=""/></td>
                          </xsl:otherwise>
                        </xsl:choose>    

                        </tr>
                      </table>    
                    </td>
                  </tr>  

                  <xsl:if test="normalize-space($currentDBT) != ''">		    		   		   
                    <tr>
                      <td colspan="3" align="center">
                        <table width="300" border="0" cellspacing="0" cellpadding="0">
                          <tr>
                            <td width="240"></td>
  
                            <td width="33"></td>
  
                            <td width="27"></td>
                          </tr>
  
                          <tr>
                            <xsl:call-template name="PointerLoop">
                              <xsl:with-param name="dbtPointer" select="$dbtPointer" />
                            </xsl:call-template>
                          </tr>
  
                        </table>    
                      </td>
                    </tr>  
                  </xsl:if>
  
                  <tr>
                    <td colspan="3" align="center">
                      <table width="300" border="0" cellspacing="0" cellpadding="0">
                        
                        <!-- color bar-->
                        <tr>
                          <td bgcolor="#000000">
                            <table width="300" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td height="25" valign="middle" width="240" align="center" bgcolor="#00aa00"><font size="1" color="#000000">
                                  0-15</font></td>

                                <td height="25" valign="middle" width="33" align="center" bgcolor="#ffff00"><font size="1" color="#000000">
                                  16-50</font></td>

                                <td height="25" valign="middle" width="27" align="center" bgcolor="#ff0000"><font size="1" color="#000000">
                                  51+</font></td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <!-- end color bar-->

                        <!-- space -->
                        <tr>
                          <td><img src="../images/spacer.gif" border="0" width="1" height="1" alt=""/></td>
                        </tr>

                        <!-- color bar labels -->
                        <tr>
                          <td>
                            <table width="300" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td width="240" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>80%</b></font></td>

                                <td width="33" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>11%</b></font></td>

                                <td width="27" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><b>9%</b></font></td>
                              </tr>
                            </table>
                          </td>
                        </tr>
                        <!-- end color bar labels -->
                      </table>    
                    </td>
                  </tr>  

                  <xsl:choose>		              
                    <xsl:when test="normalize-space($currentDBT) != ''">		    		   		   
                      <tr>
                        <td width="1%">
                          <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/></td>
                        <td width="98%">  
                          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                       
                            <tr>  
                              <td align="center" width="100%">
                                <b>% of US businesses falling within DBT range</b><br /><img src="../images/spacer.gif" border="0" width="1" height="10" alt=""/><br />
                              </td>
                            </tr>
              
                            <tr>  
                              <td nowrap="nowrap">
                                <b>DBT Norms:</b><br />
                                All industries: <b><xsl:value-of select="$allIndustryDBT" /> DBT</b> 
                                <xsl:text disable-output-escaping="yes">&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;&amp;nbsp;</xsl:text>
                                
                                <xsl:if test="normalize-space($industryDBT) != ''">
                                  Same industry: <b><xsl:value-of select="$industryDBT" /> DBT</b>
                                </xsl:if>
                              </td>
                            </tr>
    
                            <tr>
                              <td>
                                <img src="../images/spacer.gif" border="0" width="1" height="3" alt=""/></td>
                            </tr>
                            
                          </table> 
                        </td>
                        <td width="1%">
                          <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/></td>
                      </tr>    
                    </xsl:when>
                    
                    <xsl:otherwise>
                      <tr>
                        <td colspan="3">
                          <img src="../images/spacer.gif" border="0" width="1" height="8" alt=""/></td>
                      </tr>
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
  * PointerLoop template
  *********************************************
  -->
  <xsl:template name="PointerLoop">
    <xsl:param name="dbtPointer" select="''" />
    <xsl:param name="index" select="1" />
    
    <xsl:variable name="total" select="3" />
    
    <xsl:choose>		              
      <xsl:when test="$dbtPointer = $index">		    		   		   
        <td align="center" height="17" valign="middle"><img src="../images/triangle_blue.gif" border="0" width="3" height="14" alt=""/></td>
      </xsl:when>

      <xsl:otherwise>
        <td align="center"></td>
      </xsl:otherwise>
    </xsl:choose>    

    <!-- Test condition and call template if less than number -->
    <xsl:if test="$index &lt; $total">
      <xsl:call-template name="PointerLoop">
        <xsl:with-param name="index" select="($index + 1)" />
        <xsl:with-param name="dbtPointer" select="$dbtPointer" />
      </xsl:call-template>
    </xsl:if>
    
  </xsl:template>

      
</xsl:stylesheet>
