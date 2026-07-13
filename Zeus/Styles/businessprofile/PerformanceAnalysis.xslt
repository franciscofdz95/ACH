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
  * PerformanceAnalysis template
  *********************************************
  -->
  <xsl:template name="PerformanceAnalysis">
    <xsl:apply-templates select="prd:ExecutiveSummary" />
  </xsl:template>


  <!--
  *********************************************
  * ExecutiveSummary template
  *********************************************
  -->
  <xsl:template match="prd:ExecutiveSummary" xml:space="preserve">
    <xsl:variable name="predictedDBT">
      <xsl:choose>		              
        <xsl:when test="(prd:PredictedDBT) and (string(number(prd:PredictedDBT)) != 'NaN')">		    		   		   
          <xsl:value-of select="number(prd:PredictedDBT)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="predictedDate">
      <xsl:choose>		              
        <xsl:when test="(prd:PredictedDBTDate) and (string(number(prd:PredictedDBTDate)) != 'NaN')">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
    		     <xsl:with-param name="value" select="prd:PredictedDBTDate" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="paymentTrend">
      <xsl:choose>		              
        <xsl:when test="prd:PaymentTrendIndicator and normalize-space(prd:PaymentTrendIndicator/@code) != ''">		    		   		   
    		   <xsl:call-template name="TranslatePaymentTrend">
    		     <xsl:with-param name="value" select="prd:PaymentTrendIndicator/@code" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="industryComparison">
      <xsl:choose>		              
        <xsl:when test="prd:IndustryPaymentComparison and normalize-space(prd:IndustryPaymentComparison/@code) != ''">		    		   		   
          <xsl:value-of select="concat('Has paid ', prd:IndustryPaymentComparison, ' similar firms')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="purchasingTerms">
      <xsl:variable name="term1">
        <xsl:choose>		              
          <xsl:when test="prd:CommonTerms">		    		   		   
            <xsl:value-of select="normalize-space(prd:CommonTerms)" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>                
      </xsl:variable>

      <xsl:variable name="term2">
        <xsl:choose>		              
          <xsl:when test="prd:CommonTerms2">		    		   		   
            <xsl:value-of select="normalize-space(prd:CommonTerms2)" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>                
      </xsl:variable>

      <xsl:variable name="term3">
        <xsl:choose>		              
          <xsl:when test="prd:CommonTerms3">		    		   		   
            <xsl:value-of select="normalize-space(prd:CommonTerms3)" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>                
      </xsl:variable>

      <xsl:variable name="delimiter1">
        <xsl:choose>		              
          <xsl:when test="normalize-space($term1) != ''">		    		   		   
            <xsl:choose>		              
              <xsl:when test="normalize-space($term2) != '' and normalize-space($term3) != ''">		    		   		   
                <xsl:value-of select="', '" />
              </xsl:when>
      
              <xsl:when test="normalize-space($term2) != '' or normalize-space($term3) != ''">		    		   		   
                <xsl:value-of select="' and '" />
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>   
                         
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>                
      </xsl:variable>

      <xsl:variable name="delimiter2">
        <xsl:choose>		              
          <xsl:when test="normalize-space($term2) != ''">		    		   		   
            <xsl:choose>		              
              <xsl:when test="normalize-space($term1) != '' and normalize-space($term3) != ''">		    		   		   
                <xsl:value-of select="', and '" />
              </xsl:when>
      
              <xsl:when test="normalize-space($term3) != ''">		    		   		   
                <xsl:value-of select="' and '" />
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="''" />
              </xsl:otherwise>
            </xsl:choose>   
                         
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="''" />
          </xsl:otherwise>
        </xsl:choose>                
      </xsl:variable>

      <xsl:value-of select="concat(normalize-space($term1), ' ', normalize-space($delimiter1), ' ', normalize-space($term2), ' ', normalize-space($delimiter2), ' ', $term3)" />

    </xsl:variable>

    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td valign="top" height="20">
          <font color="{$borderColor}"><b>Performance Analysis</b></font>
        </td>
      </tr>  
      
      <xsl:if test="normalize-space($predictedDBT) != ''">
        <tr>  
          <td height="20">
            <b>Predicted DBT for <xsl:value-of select="normalize-space($predictedDate)" />:</b> 
            <xsl:value-of select="$predictedDBT" /> DBT</td>
        </tr>
      </xsl:if>

      <xsl:if test="normalize-space($paymentTrend) != ''">
        <tr>
          <td height="38">
            <b>Payment Trend Indication:</b><br /> 
            <xsl:value-of select="$paymentTrend" /></td>
        </tr>
      </xsl:if>

      <xsl:if test="normalize-space($industryComparison) != ''">
        <tr>
          <td height="38">
            <b>Industry payment comparison:</b><br /> 
            <xsl:value-of select="$industryComparison" /></td>
        </tr>
      </xsl:if>

      <xsl:if test="normalize-space($purchasingTerms) != ''">
        <tr>
          <td height="38">
            <b>Most Frequent Industry Purchasing Terms:</b><br /> 
            <xsl:value-of select="$purchasingTerms" /></td>
        </tr>
      </xsl:if>

    </table>
  </xsl:template>


  <!--
  *********************************************
  * TranslatePaymentTrend template
  * Value has to be 1 chr
  *********************************************
  -->
  <xsl:template name="TranslatePaymentTrend">
    <xsl:param name="value" select="''" />
    <xsl:param name="upperCase" select="false()" />
      
    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="$value= 'B'">		    		   		   
          <xsl:value-of select="'Payments Are Increasingly Late But Still Better Than The Industry Average'" />
        </xsl:when>

        <xsl:when test="$value= 'I'">		    		   		   
          <xsl:value-of select="'Are Improving Toward Term Requirements'" />
        </xsl:when>

        <xsl:when test="$value= 'L'">		    		   		   
          <xsl:value-of select="'Payments Are Increasingly Late'" />
        </xsl:when>

        <xsl:when test="$value= 'N'">		    		   		   
          <xsl:value-of select="'Show No Identifiable Trend'" />
        </xsl:when>

        <xsl:when test="$value= 'P'">		    		   		   
          <xsl:value-of select="'Payments Are Improving Toward Term Requirements But Are Still Slower Than The Industry Average'" />
        </xsl:when>

        <xsl:when test="$value= 'S'">		    		   		   
          <xsl:value-of select="'Are Stable'" />
        </xsl:when>

      </xsl:choose>    
    </xsl:variable>
    
    
    <xsl:choose>		              
      <xsl:when test="$upperCase">		    		   		   
        <xsl:value-of select="translate($result, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="$result" />
      </xsl:otherwise>
    </xsl:choose>    
    
  </xsl:template>
  

</xsl:stylesheet>
