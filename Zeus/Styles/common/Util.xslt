<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

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
  * SectionTitle template
  *********************************************
  -->
  <xsl:template name="SectionTitle">
    <xsl:param name="title" />
    <xsl:param name="color" select="'#193385'" />

    <!-- section title -->
    <table width="100%" border="0" cellspacing="0" cellpadding="0">
      <tr>
        <td>
          <font size="3" color="{$color}"><b>
          <xsl:value-of select="$title" />
          </b></font>
        </td>
      </tr>
    </table>

  </xsl:template>


  <!--
  *********************************************
  * FormatDate template
  * Pattern: 'year' or 'yr' for year
  *          'dt' for day
  *          'mo' for month
  * Value has to be in format 'YYYYMMDD', 'MMDDYYYY', 'YYMMDD', or 'MMDDYY'
  *********************************************
  -->
  <xsl:template name="FormatDate">    
    <xsl:param name="pattern" />
    <xsl:param name="value" />
    <xsl:param name="yearDigit" select="4" />
    <xsl:param name="isYearLast" select="false()" />
    <xsl:param name="delimiter" select="'/'" />

    <xsl:variable name="yyyy">
      <xsl:choose>		              
        <xsl:when test="$yearDigit = 4">
          <xsl:choose>		              
            <xsl:when test="$isYearLast">
              <xsl:value-of select="substring($value, 5, 4)" />
            </xsl:when>

            <xsl:otherwise>
              <xsl:value-of select="substring($value, 1, 4)" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:choose>		              
            <xsl:when test="$isYearLast">
              <xsl:choose>		              
                <xsl:when test="substring($value, 5, 2) &gt; 40">
                  <xsl:value-of select="normalize-space(concat('19', substring($value, 5, 2)))" />
                </xsl:when>

                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(concat('20', substring($value, 5, 2)))" />
                </xsl:otherwise>
              </xsl:choose>    
            </xsl:when>

            <xsl:otherwise>
              <xsl:choose>
                <xsl:when test="substring($value, 1, 2) &gt; 40">
                  <xsl:value-of select="normalize-space(concat('19', substring($value, 1, 2)))" />
                </xsl:when>
        
                <xsl:otherwise>
                  <xsl:value-of select="normalize-space(concat('20', substring($value, 1, 2)))" />
                </xsl:otherwise>
              </xsl:choose>    
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
 
    <xsl:variable name="yy">
      <xsl:choose>		              
        <xsl:when test="$yearDigit = 4">
          <xsl:choose>		              
            <xsl:when test="$isYearLast">
              <xsl:value-of select="substring($value, 7, 2)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="substring($value, 3, 2)" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>

        <xsl:otherwise>
          <xsl:choose>		              
            <xsl:when test="$isYearLast">
              <xsl:value-of select="substring($value, 5, 2)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="substring($value, 1, 2)" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
 
    <xsl:variable name="MM">
      <xsl:choose>		              
        <xsl:when test="$isYearLast">
          <xsl:value-of select="substring($value, 1, 2)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:choose>		              
            <xsl:when test="$yearDigit = 4">
              <xsl:value-of select="substring($value, 5, 2)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="substring($value, 3, 2)" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
 
    <xsl:variable name="dd">
      <xsl:choose>		              
        <xsl:when test="$isYearLast">
          <xsl:value-of select="substring($value, 3, 2)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:choose>		              
            <xsl:when test="$yearDigit = 4">
              <xsl:value-of select="substring($value, 7, 2)" />
            </xsl:when>
    
            <xsl:otherwise>
              <xsl:value-of select="substring($value, 5, 2)" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:choose>		              
      <xsl:when test="number($yyyy) = 0">
        <xsl:value-of select="''" />
      </xsl:when>
      
      <xsl:when test="number($MM) = 0">
        <xsl:choose>		              
          <xsl:when test="contains($pattern, 'year')">
            <xsl:value-of select="$yyyy" />
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:value-of select="$yy" />
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:when>
      
      <xsl:when test="number($dd) = 0">
        <xsl:choose>		              
          <xsl:when test="starts-with(normalize-space($pattern), 'year') or starts-with(normalize-space($pattern), 'yr')">
            <xsl:choose>                  
              <xsl:when test="contains($pattern, 'year')">
                <xsl:value-of select="concat(normalize-space($yyyy), $delimiter, normalize-space($MM))" />
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="concat(normalize-space($yy), $delimiter, normalize-space($MM))" />
              </xsl:otherwise>
            </xsl:choose>    
          </xsl:when>
  
          <xsl:otherwise>
            <xsl:choose>                  
              <xsl:when test="contains($pattern, 'year')">
                <xsl:value-of select="concat(normalize-space($MM), $delimiter, normalize-space($yyyy))" />
              </xsl:when>
      
              <xsl:otherwise>
                <xsl:value-of select="concat(normalize-space($MM), $delimiter, normalize-space($yy))" />
              </xsl:otherwise>
            </xsl:choose>    
          </xsl:otherwise>
        </xsl:choose>    
      </xsl:when>
      
      <xsl:when test="contains($pattern, 'year')">
        <xsl:value-of select="translate(translate(translate($pattern, 'dt', $dd), 'mo', $MM), 'year', $yyyy)" />
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="translate(translate(translate($pattern, 'dt', $dd), 'mo', $MM), 'yr', $yy)" />
      </xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  ************************************************
  * FormatPhone template
  * Value has to be in format 9999999999
  ************************************************
  -->
  <xsl:template name="FormatPhone">
    <xsl:param name="value" />
    <xsl:value-of select="concat('(',substring($value, 1, 3), ') ', substring($value, 4, 3), '-', substring($value, 7, 4))" />
  </xsl:template>


  <!--
  ***********************************************
  * FormatZip template
  * Value has to be in format 999999999 or 99999
  ***********************************************
  -->
  <xsl:template name="FormatZip">
    <xsl:param name="value" />
    
    <xsl:choose>		              
      <xsl:when test="string-length(normalize-space($value)) &gt; 5">
        <xsl:value-of select="concat(substring($value, 1, 5), '-', normalize-space(substring($value, 6)))" />
      </xsl:when>

      <xsl:otherwise>
        <xsl:value-of select="$value" />
      </xsl:otherwise>
    </xsl:choose>    
  </xsl:template>


  <!--
  *********************************************
  * FormatMonth template
  * Value has to be 1 to 12
  *********************************************
  -->
  <xsl:template name="FormatMonth">
    <xsl:param name="monthValue" select="1" />
    <xsl:param name="full" select="false()" />
    <xsl:param name="upperCase" select="false()" />
    
    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="number($monthValue) = 1">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'January'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Jan'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 2">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'February'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Feb'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 3">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'March'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Mar'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 4">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'April'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Apr'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 5">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'May'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'May'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 6">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'June'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Jun'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 7">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'July'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Jul'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 8">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'August'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Aug'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 9">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'September'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Sep'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 10">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'October'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Oct'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:when test="$monthValue = 11">
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'November'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Nov'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:when>
  
        <xsl:otherwise>
          <xsl:choose>		              
            <xsl:when test="$full">
              <xsl:value-of select="'December'" />
            </xsl:when>
      
            <xsl:otherwise>
              <xsl:value-of select="'Dec'" />
            </xsl:otherwise>
          </xsl:choose>    
        </xsl:otherwise>
  
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
  
  
  <!--
  *********************************************
  * JoinNodeset template
  *********************************************
  -->
  <xsl:template name="JoinNodeset">
    <xsl:param name="nodeset" />
    <xsl:param name="order" />
    <xsl:param name="delimiter" select="'&lt;br /&gt;'" />
    
    <xsl:for-each select="$nodeset">
      <xsl:sort select="position()" order="{$order}" data-type="text"/>
      <xsl:if test="position() &gt; 1">
        <xsl:value-of select="$delimiter" />
      </xsl:if>
      <xsl:value-of select="." />
    </xsl:for-each>
  </xsl:template>


  <!--
  *********************************************
  * translate collection status template
  * code is status code
  *********************************************
  -->
  <xsl:template name="translateCollStatus">
    <xsl:param name="code" select="''" />
      
    <xsl:choose>		              
	<xsl:when test="number($code) = 0">
      		<xsl:text disable-output-escaping="yes">Open Account</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 1">
      		<xsl:text disable-output-escaping="yes">Disputed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 2">
      		<xsl:text disable-output-escaping="yes">Payment Plan</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 3">
      		<xsl:text disable-output-escaping="yes">Paid in Full</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 4">
      		<xsl:text disable-output-escaping="yes">Settlement Paid Full</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 5">
      		<xsl:text disable-output-escaping="yes">Closed, Partial Payment</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 6">
      		<xsl:text disable-output-escaping="yes">Closed, Uncollected</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 7">
      		<xsl:text disable-output-escaping="yes">Closed, Out of Business</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 8">
      		<xsl:text disable-output-escaping="yes">Closed, Bankruptcy</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 9">
      		<xsl:text disable-output-escaping="yes">Closed, Creditors Request</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 10">
      		<xsl:text disable-output-escaping="yes">Closed, Uncollected Judgment</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 11">
      		<xsl:text disable-output-escaping="yes">Closed, Judgment Satisfied</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 12">
      		<xsl:text disable-output-escaping="yes">Closed, Cannot Locate</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Unknown</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate legal filing type template
  * code is status code
  *********************************************
  -->
  <xsl:template name="translateLegalFilingType">
    <xsl:param name="code" select="''" />
      
    <xsl:choose>		              
	<xsl:when test="number($code) = 1">
      		<xsl:text disable-output-escaping="yes">Bankruptcy</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 2">
      		<xsl:text disable-output-escaping="yes">Federal Tax Lien</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 3">
      		<xsl:text disable-output-escaping="yes">State Tax Lien</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 4">
      		<xsl:text disable-output-escaping="yes">County Tax Lien</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 5">
      		<xsl:text disable-output-escaping="yes">Judgment</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 6">
      		<xsl:text disable-output-escaping="yes">Attachment Lien</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 7">
      		<xsl:text disable-output-escaping="yes">Bulk Transfer</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 8">
      		<xsl:text disable-output-escaping="yes">UCC</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Unknown</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate legal filing status template
  * code is status code
  *********************************************
  -->
  <xsl:template name="translateLegalFilingStatus">
    <xsl:param name="code" select="''" />
      
    <xsl:choose>		              
	<xsl:when test="number($code) = 1">
      		<xsl:text disable-output-escaping="yes">Filed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 2">
      		<xsl:text disable-output-escaping="yes">Amended</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 3">
      		<xsl:text disable-output-escaping="yes">Assigned</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 4">
      		<xsl:text disable-output-escaping="yes">Partial Release</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 5">
      		<xsl:text disable-output-escaping="yes">Full Release</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 6">
      		<xsl:text disable-output-escaping="yes">Filed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 7">
      		<xsl:text disable-output-escaping="yes">Released</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 8">
      		<xsl:text disable-output-escaping="yes">Terminated</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 9">
      		<xsl:text disable-output-escaping="yes">Continued</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 10">
      		<xsl:text disable-output-escaping="yes">Satisfied</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 11">
      		<xsl:text disable-output-escaping="yes">Abstract</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 12">
      		<xsl:text disable-output-escaping="yes">Chapter 7 Involuntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 13">
      		<xsl:text disable-output-escaping="yes">Chapter 7 Voluntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 14">
      		<xsl:text disable-output-escaping="yes">Chapter 7</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 15">
      		<xsl:text disable-output-escaping="yes">Chapter 11 Involuntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 16">
      		<xsl:text disable-output-escaping="yes">Chapter 11 Voluntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 17">
      		<xsl:text disable-output-escaping="yes">Chapter 11</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 18">
      		<xsl:text disable-output-escaping="yes">Involuntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 19">
      		<xsl:text disable-output-escaping="yes">Voluntary</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 20">
      		<xsl:text disable-output-escaping="yes">Chapter 13</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 21">
      		<xsl:text disable-output-escaping="yes">Vacated</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 22">
      		<xsl:text disable-output-escaping="yes">Chapter 7 Dismissed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 23">
      		<xsl:text disable-output-escaping="yes">Chapter 11 Dismissed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 24">
      		<xsl:text disable-output-escaping="yes">Chapter 7 Discharged</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 25">
      		<xsl:text disable-output-escaping="yes">Chapter 11 Discharged</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 26">
      		<xsl:text disable-output-escaping="yes">Chapter 13 Completed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 27">
      		<xsl:text disable-output-escaping="yes">Chapter 13 Dismissed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 28">
      		<xsl:text disable-output-escaping="yes">Chapter 12</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 29">
      		<xsl:text disable-output-escaping="yes">Chapter 12 Dismissed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 30">
      		<xsl:text disable-output-escaping="yes">Chapter 12 Discharged</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 31">
      		<xsl:text disable-output-escaping="yes">UCC Filed 10 Years</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 32">
      		<xsl:text disable-output-escaping="yes">Chapter 10</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 33">
      		<xsl:text disable-output-escaping="yes">Chapter 10 Dismissed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 34">
      		<xsl:text disable-output-escaping="yes">Chapter 10 Discharged</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 35">
      		<xsl:text disable-output-escaping="yes">Chapter 10 Closed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 36">
      		<xsl:text disable-output-escaping="yes">Chapter 7 Closed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 37">
      		<xsl:text disable-output-escaping="yes">Chapter 11 Closed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 38">
      		<xsl:text disable-output-escaping="yes">Chapter 12 Closed</xsl:text>
    	</xsl:when>
    	<xsl:when test="number($code) = 39">
      		<xsl:text disable-output-escaping="yes">Chapter 13 Closed</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Unknown</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate fraud shield summary template
  * code is FraudServices indicator
  *********************************************
  -->
  <xsl:template name="translateFraudServicesIndicator">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="number($code) = 1">
      		<xsl:text disable-output-escaping="yes">Inquiry/onfile current address conflict</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 2">
      		<xsl:text disable-output-escaping="yes">Inquiry address 1st reported &lt; 90 days</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 3">
      		<xsl:text disable-output-escaping="yes">Inquiry current address not onfile</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 4">
      		<xsl:text disable-output-escaping="yes">Inquiry SSN has not been issued</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 5">
      		<xsl:text disable-output-escaping="yes">Inquiry SSN recorded as deceased</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 6">
      		<xsl:text disable-output-escaping="yes">Inquiry age younger than SSN issue date</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 7">
      		<xsl:text disable-output-escaping="yes">Credit established before age 18</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 8">
      		<xsl:text disable-output-escaping="yes">Credit established prior to SSN issue date</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9">
      		<xsl:text disable-output-escaping="yes">More than 3 inquiries in last 30 days</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 10">
      		<xsl:text disable-output-escaping="yes">Inquiry address: alert</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 11">
      		<xsl:text disable-output-escaping="yes">Inquiry address: non-residential</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 12">
      		<xsl:text disable-output-escaping="yes">Security statement present on report</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 13">
      		<xsl:text disable-output-escaping="yes">High probability SSN belongs to another</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 14">
      		<xsl:text disable-output-escaping="yes">Inquiry SSN format is invalid</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 15">
      		<xsl:text disable-output-escaping="yes">Inquiry address: cautious</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 16">
      		<xsl:text disable-output-escaping="yes">Onfile address: alert</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 17">
      		<xsl:text disable-output-escaping="yes">Onfile address: non-residential</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 18">
      		<xsl:text disable-output-escaping="yes">Onfile address: cautious</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 19">
      		<xsl:text disable-output-escaping="yes">Current address rpt by new trade only</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 20">
      		<xsl:text disable-output-escaping="yes">Current address rpt by trade open &lt; 90 days</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 21">
      		<xsl:text disable-output-escaping="yes">Telephone number inconsistent w/address</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 22">
      		<xsl:text disable-output-escaping="yes">Drivers license inconsistent w/onfile</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">No record found</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate unscorable ScorexPlus template
  * code is ScorexPlus unscorable score
  *********************************************
  -->
  <xsl:template name="translateUnscorableScorexPlus">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="number($code) = 9000">
      		<xsl:text disable-output-escaping="yes">The Credit profile contains more than 500 tradelines, inquiries, and public records</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9001">
      		<xsl:text disable-output-escaping="yes">One or more tradelines on the Credit Profile has a deceased status</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9003">
      		<xsl:text disable-output-escaping="yes">The Credit Profile does not contain any valid tradelines</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9005">
      		<xsl:text disable-output-escaping="yes">Credit profile has had no activity in the last 12 months</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9006">
      		<xsl:text disable-output-escaping="yes">Block Consumer: The file belongs to an individual on Experian's block consumer file. These are names of individuals Experian will not sell</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9007">
      		<xsl:text disable-output-escaping="yes">Edit rejects</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9008">
      		<xsl:text disable-output-escaping="yes">Insufficient Information: Unable to identify consumer due to insufficient information</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">No record found</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate unscorable ScorexPlus template
  * code is ScorexPlus unscorable score
  *********************************************
  -->
  <xsl:template name="translateArfScorexRiskFactorTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="number($code) = 1">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 2">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Automotive Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 3">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 5">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 6">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Real-Estate Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 7">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 8">
      		<xsl:text disable-output-escaping="yes">Lack Of Open Retail Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 10">
      		<xsl:text disable-output-escaping="yes">Number Of Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 11">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Opened Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 13">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Opened Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 14">
      		<xsl:text disable-output-escaping="yes">Number Of Collection Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 16">
      		<xsl:text disable-output-escaping="yes">Number Of Finance Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 18">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Opened Finance Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 19">
      		<xsl:text disable-output-escaping="yes">Number Of Real-Estate Trades</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 20">
      		<xsl:text disable-output-escaping="yes">Number Of Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 23">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Opened Auto Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 24">
      		<xsl:text disable-output-escaping="yes">Number Of Retail Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 28">
      		<xsl:text disable-output-escaping="yes">Balances On Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 30">
      		<xsl:text disable-output-escaping="yes">Credit Limits/Loan Amounts On Open Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 31">
      		<xsl:text disable-output-escaping="yes">Credit Limits On Open Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 34">
      		<xsl:text disable-output-escaping="yes">Loan Amounts On Real-Estate Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 35">
      		<xsl:text disable-output-escaping="yes">Available Credit On Open Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 36">
      		<xsl:text disable-output-escaping="yes">Available Credit On Open Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 37">
      		<xsl:text disable-output-escaping="yes">Monthly Payment On Open Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 40">
      		<xsl:text disable-output-escaping="yes">Number Of Accounts With High Balance-To-Limit Ratios</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 41">
      		<xsl:text disable-output-escaping="yes">Ratio Of Balance To Limit On Open Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 42">
      		<xsl:text disable-output-escaping="yes">Ratio Of Balance To Limit On Open Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 43">
      		<xsl:text disable-output-escaping="yes">Ratio Of Balance To Limit On Open Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 47">
      		<xsl:text disable-output-escaping="yes">Ratio Of Balance To Limit On Open Retail Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 50">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 51">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Auto Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 52">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 54">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 55">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Real-Estate Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 56">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 57">
      		<xsl:text disable-output-escaping="yes">Number Of Never-Delinquent Bankcard Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 58">
      		<xsl:text disable-output-escaping="yes">Number Of Current Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 59">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Delinquent Installment Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 60">
      		<xsl:text disable-output-escaping="yes">Number Of Current Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 61">
      		<xsl:text disable-output-escaping="yes">Number Of Delinquent Or Derogatory Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 63">
      		<xsl:text disable-output-escaping="yes">Number Of Recently Delinquent Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 64">
      		<xsl:text disable-output-escaping="yes">Number Of Never-Delinquent Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 67">
      		<xsl:text disable-output-escaping="yes">Number Of Accounts With Past-Due Balances</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 70">
      		<xsl:text disable-output-escaping="yes">Age Of Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 72">
      		<xsl:text disable-output-escaping="yes">Age Of Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 80">
      		<xsl:text disable-output-escaping="yes">Number Of Inquiries</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 81">
      		<xsl:text disable-output-escaping="yes">Number Of Recent Inquiries</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 85">
      		<xsl:text disable-output-escaping="yes">Presence Of Derogatory Public Record</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 87">
      		<xsl:text disable-output-escaping="yes">Balances On Public Record Information</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 90">
      		<xsl:text disable-output-escaping="yes">Recency Of Delinquent Or Derogatory Account</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 93">
      		<xsl:text disable-output-escaping="yes">Number Of Never-Delinquent Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 94">
      		<xsl:text disable-output-escaping="yes">Number Of Never-Delinquent Real-Estate Accounts</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 95">
      		<xsl:text disable-output-escaping="yes">Presence Of Delinquent Or Derogatory Retail Revolving Accounts</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">No record found</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * TranslateState template
  * Value has to be 2 char state
  *********************************************
  -->
  <xsl:template name="TranslateState">
    <xsl:param name="value" select="''" />
    <xsl:param name="upperCase" select="false()" />
      
    <xsl:variable name="state">
      <xsl:value-of select="translate($value, 'abcdefghijklmnopqrstuvwxyz', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ')" />
    </xsl:variable>
    
    <xsl:variable name="result">
      <xsl:choose>		              
        <xsl:when test="$state = 'AL'">
          <xsl:value-of select="'Alabama'" />
        </xsl:when>
  
        <xsl:when test="$state = 'AK'">
          <xsl:value-of select="'Alaska'" />
        </xsl:when>
  
        <xsl:when test="$state = 'AS'">
          <xsl:value-of select="'American Samoa'" />
        </xsl:when>
  
        <xsl:when test="$state = 'AZ'">
          <xsl:value-of select="'Arizona'" />
        </xsl:when>
  
        <xsl:when test="$state = 'AR'">
          <xsl:value-of select="'Arkansas'" />
        </xsl:when>
  
        <xsl:when test="$state = 'CA'">
          <xsl:value-of select="'California'" />
        </xsl:when>
  
        <xsl:when test="$state = 'CO'">
          <xsl:value-of select="'Colorado'" />
        </xsl:when>
  
        <xsl:when test="$state = 'CT'">
          <xsl:value-of select="'Connecticut'" />
        </xsl:when>
  
        <xsl:when test="$state = 'DE'">
          <xsl:value-of select="'Delaware'" />
        </xsl:when>
  
        <xsl:when test="$state = 'DC'">
          <xsl:value-of select="'District of Columbia'" />
        </xsl:when>
  
        <xsl:when test="$state = 'FM'">
          <xsl:value-of select="'Federated States of Micronesia'" />
        </xsl:when>
  
        <xsl:when test="$state = 'FL'">
          <xsl:value-of select="'Florida'" />
        </xsl:when>
  
        <xsl:when test="$state = 'GA'">
          <xsl:value-of select="'Georgia'" />
        </xsl:when>
  
        <xsl:when test="$state = 'GU'">
          <xsl:value-of select="'Guam'" />
        </xsl:when>
  
        <xsl:when test="$state = 'HI'">
          <xsl:value-of select="'Hawaii'" />
        </xsl:when>
  
        <xsl:when test="$state = 'ID'">
          <xsl:value-of select="'Idaho'" />
        </xsl:when>
  
        <xsl:when test="$state = 'IL'">
          <xsl:value-of select="'Illinois'" />
        </xsl:when>
  
        <xsl:when test="$state = 'IN'">
          <xsl:value-of select="'Indiana'" />
        </xsl:when>
  
        <xsl:when test="$state = 'IA'">
          <xsl:value-of select="'Iowa'" />
        </xsl:when>
  
        <xsl:when test="$state = 'KS'">
          <xsl:value-of select="'Kansas'" />
        </xsl:when>
  
        <xsl:when test="$state = 'KY'">
          <xsl:value-of select="'Kentucky'" />
        </xsl:when>
  
        <xsl:when test="$state = 'LA'">
          <xsl:value-of select="'Louisiana'" />
        </xsl:when>
  
        <xsl:when test="$state = 'ME'">
          <xsl:value-of select="'Maine'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MH'">
          <xsl:value-of select="'Marshall Islands'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MD'">
          <xsl:value-of select="'Maryland'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MA'">
          <xsl:value-of select="'Massachusetts'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MI'">
          <xsl:value-of select="'Michigan'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MN'">
          <xsl:value-of select="'Minnesota'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MS'">
          <xsl:value-of select="'Mississippi'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MO'">
          <xsl:value-of select="'Missouri'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MT'">
          <xsl:value-of select="'Montana'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NE'">
          <xsl:value-of select="'Nebraska'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NV'">
          <xsl:value-of select="'Nevada'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NH'">
          <xsl:value-of select="'New Hampshire'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NJ'">
          <xsl:value-of select="'New Jersey'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NM'">
          <xsl:value-of select="'New Mexico'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NY'">
          <xsl:value-of select="'New York'" />
        </xsl:when>
  
        <xsl:when test="$state = 'NC'">
          <xsl:value-of select="'North Carolina'" />
        </xsl:when>
  
        <xsl:when test="$state = 'ND'">
          <xsl:value-of select="'North Dakota'" />
        </xsl:when>
  
        <xsl:when test="$state = 'MP'">
          <xsl:value-of select="'Northern Mariana Island'" />
        </xsl:when>
  
        <xsl:when test="$state = 'OH'">
          <xsl:value-of select="'Ohio'" />
        </xsl:when>
  
        <xsl:when test="$state = 'OK'">
          <xsl:value-of select="'Oklahoma'" />
        </xsl:when>
  
        <xsl:when test="$state = 'OR'">
          <xsl:value-of select="'Oregon'" />
        </xsl:when>
  
        <xsl:when test="$state = 'PW'">
          <xsl:value-of select="'Palau'" />
        </xsl:when>
  
        <xsl:when test="$state = 'PA'">
          <xsl:value-of select="'Pennsylvania'" />
        </xsl:when>
  
        <xsl:when test="$state = 'PR'">
          <xsl:value-of select="'Puerto Rico'" />
        </xsl:when>
  
        <xsl:when test="$state = 'RI'">
          <xsl:value-of select="'Rhode Island'" />
        </xsl:when>
  
        <xsl:when test="$state = 'SC'">
          <xsl:value-of select="'South Carolina'" />
        </xsl:when>
  
        <xsl:when test="$state = 'SD'">
          <xsl:value-of select="'South Dakota'" />
        </xsl:when>
  
        <xsl:when test="$state = 'TN'">
          <xsl:value-of select="'Tennessee'" />
        </xsl:when>
  
        <xsl:when test="$state = 'TX'">
          <xsl:value-of select="'Texas'" />
        </xsl:when>
  
        <xsl:when test="$state = 'UT'">
          <xsl:value-of select="'Utah'" />
        </xsl:when>
  
        <xsl:when test="$state = 'VT'">
          <xsl:value-of select="'Vermont'" />
        </xsl:when>
  
        <xsl:when test="$state = 'VI'">
          <xsl:value-of select="'Virgin Islands'" />
        </xsl:when>
  
        <xsl:when test="$state = 'VA'">
          <xsl:value-of select="'Virginia'" />
        </xsl:when>
  
        <xsl:when test="$state = 'WA'">
          <xsl:value-of select="'Washington'" />
        </xsl:when>
  
        <xsl:when test="$state = 'WV'">  
          <xsl:value-of select="'West Virginia'" />
        </xsl:when>
  
        <xsl:when test="$state = 'WI'">
          <xsl:value-of select="'Wisconsin'" />
        </xsl:when>
  
        <xsl:when test="$state = 'WY'">
          <xsl:value-of select="'Wyoming'" />
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


  <!--
  *********************************************
  * translate AuthAddressHighRiskTable
  *********************************************
  -->
  <xsl:template name="AuthAddressHighRiskTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No address high risk information found</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NP' ">
      		<xsl:text disable-output-escaping="yes">Test not in profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YA' ">
      		<xsl:text disable-output-escaping="yes">A high risk business was identified at this address</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthAddressVerifTable
  *********************************************
  -->
  <xsl:template name="AuthAddressVerifTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'A' ">
      		<xsl:text disable-output-escaping="yes">Address ambiguous</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'B' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - residential address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'BB' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - business address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'BM' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - mixed use address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'E' ">
      		<xsl:text disable-output-escaping="yes">Matching records exceed maximum defined on profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'H' ">
      		<xsl:text disable-output-escaping="yes">House number not found on street</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'I' ">
      		<xsl:text disable-output-escaping="yes">Incomplete or blank address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No match to name - residential address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NA' ">
      		<xsl:text disable-output-escaping="yes">Data not available</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NB' ">
      		<xsl:text disable-output-escaping="yes">No match to name - business address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NM' ">
      		<xsl:text disable-output-escaping="yes">No match to name - mixed use address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NP' ">
      		<xsl:text disable-output-escaping="yes">Test not in profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NS' ">
      		<xsl:text disable-output-escaping="yes">Standardization database has expired - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'R' ">
      		<xsl:text disable-output-escaping="yes">Road name - City/Zip mismatch</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'S' ">
      		<xsl:text disable-output-escaping="yes">Match to surname - residential address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SB' ">
      		<xsl:text disable-output-escaping="yes">Match to surname - business address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SM' ">
      		<xsl:text disable-output-escaping="yes">Match to surname - mixed use address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SX' ">
      		<xsl:text disable-output-escaping="yes">Standardization database has expired - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'T' ">
      		<xsl:text disable-output-escaping="yes">City - State mismatch</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'U' ">
      		<xsl:text disable-output-escaping="yes">Address unverifiable - not in database</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'UR' ">
      		<xsl:text disable-output-escaping="yes">Address residential - name match unavailable</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'Y' ">
      		<xsl:text disable-output-escaping="yes">Match to full name - residential address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YB' ">
      		<xsl:text disable-output-escaping="yes">Match to full name - business address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YM' ">
      		<xsl:text disable-output-escaping="yes">Match to full name - mixed use address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'Z' ">
      		<xsl:text disable-output-escaping="yes">City/State - Zip mismatch</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = '00' ">
      		<xsl:text disable-output-escaping="yes">unknown message code - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthAddressTypeTable
  *********************************************
  -->
  <xsl:template name="AuthAddressTypeTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'C' ">
      		<xsl:text disable-output-escaping="yes">Single company</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'E' ">
      		<xsl:text disable-output-escaping="yes">Test error</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'EB' ">
      		<xsl:text disable-output-escaping="yes">Seasonal - business</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'EM' ">
      		<xsl:text disable-output-escaping="yes">Seasonal - residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'EX' ">
      		<xsl:text disable-output-escaping="yes">Seasonal - mixed use</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'M' ">
      		<xsl:text disable-output-escaping="yes">Residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No information available</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NA' ">
      		<xsl:text disable-output-escaping="yes">Data not available</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NP' ">
      		<xsl:text disable-output-escaping="yes">Test not in profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'O' ">
      		<xsl:text disable-output-escaping="yes">Office building</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'P' ">
      		<xsl:text disable-output-escaping="yes">Post office box</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'S' ">
      		<xsl:text disable-output-escaping="yes">Residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SE' ">
      		<xsl:text disable-output-escaping="yes">Seasonal - residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'V' ">
      		<xsl:text disable-output-escaping="yes">Vacant - unknown type</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'VB' ">
      		<xsl:text disable-output-escaping="yes">Vacant - business</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'VM' ">
      		<xsl:text disable-output-escaping="yes">Vacant - residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'VS' ">
      		<xsl:text disable-output-escaping="yes">Vacant - residential</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'VX' ">
      		<xsl:text disable-output-escaping="yes">Vacant - mixed use</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'X' ">
      		<xsl:text disable-output-escaping="yes">Mixed use</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = '00' ">
      		<xsl:text disable-output-escaping="yes">unknown message code - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthAddressUnitTable
  *********************************************
  -->
  <xsl:template name="AuthAddressUnitTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'EU' ">
      		<xsl:text disable-output-escaping="yes">Unit number is extra - not expected at this address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'MU' ">
      		<xsl:text disable-output-escaping="yes">Unit number is missing - expected at this address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'WU' ">
      		<xsl:text disable-output-escaping="yes">Unit number wrong - unit number does not match unit number at this address</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthPhoneHighRiskTable
  *********************************************
  -->
  <xsl:template name="AuthPhoneHighRiskTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No phone high risk information found</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NP' ">
      		<xsl:text disable-output-escaping="yes">Test not in profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YP' ">
      		<xsl:text disable-output-escaping="yes">A high risk business was identified for the address at this phone</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthPhoneVerifTable
  *********************************************
  -->
  <xsl:template name="AuthPhoneVerifTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'A' ">
      		<xsl:text disable-output-escaping="yes">Match to address only - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'AB' ">
      		<xsl:text disable-output-escaping="yes">Match to address only - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'AM' ">
      		<xsl:text disable-output-escaping="yes">Match to address only - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'B' ">
      		<xsl:text disable-output-escaping="yes">Match to business name and address - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'BB' ">
      		<xsl:text disable-output-escaping="yes">Match to business name and address - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'BM' ">
      		<xsl:text disable-output-escaping="yes">Match to business name and address - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'C' ">
      		<xsl:text disable-output-escaping="yes">Probable cellular phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'D' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'DB' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'DM' ">
      		<xsl:text disable-output-escaping="yes">Match to business name - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'E' ">
      		<xsl:text disable-output-escaping="yes">Matching records exceed maximum defined on profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'F' ">
      		<xsl:text disable-output-escaping="yes">Match to full name only - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'FB' ">
      		<xsl:text disable-output-escaping="yes">Match to full name only - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'FM' ">
      		<xsl:text disable-output-escaping="yes">Match to full name only - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'H' ">
      		<xsl:text disable-output-escaping="yes">Match to surname and address - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'HB' ">
      		<xsl:text disable-output-escaping="yes">Match to surname and address - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'HM' ">
      		<xsl:text disable-output-escaping="yes">Match to surname and address - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'I' ">
      		<xsl:text disable-output-escaping="yes">Phone is incorrect length</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'IA' ">
      		<xsl:text disable-output-escaping="yes">Invalid area code</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'M' ">
      		<xsl:text disable-output-escaping="yes">Phone missing</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'MA' ">
      		<xsl:text disable-output-escaping="yes">Match to header data</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No match to name or address - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NA' ">
      		<xsl:text disable-output-escaping="yes">Data not available</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NB' ">
      		<xsl:text disable-output-escaping="yes">No match to name or address - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NM' ">
      		<xsl:text disable-output-escaping="yes">No match to name or address - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NP' ">
      		<xsl:text disable-output-escaping="yes">Test not in profile</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'NS' ">
      		<xsl:text disable-output-escaping="yes">Standardization database has expired - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'P' ">
      		<xsl:text disable-output-escaping="yes">Probable pager</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'S' ">
      		<xsl:text disable-output-escaping="yes">Match to surname only - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SB' ">
      		<xsl:text disable-output-escaping="yes">Match to surname only - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'SM' ">
      		<xsl:text disable-output-escaping="yes">Match to surname only - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'U' ">
      		<xsl:text disable-output-escaping="yes">Phone unverifiable - not in database</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'X' ">
      		<xsl:text disable-output-escaping="yes">Prefix - Zip mismatch</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'Y' ">
      		<xsl:text disable-output-escaping="yes">Match to full name and address - residential phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YB' ">
      		<xsl:text disable-output-escaping="yes">Match to full name and address - business phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'YM' ">
      		<xsl:text disable-output-escaping="yes">Match to full name and address - mixed use phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = '00' ">
      		<xsl:text disable-output-escaping="yes">unknown message code - contact Help Desk</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthPhoneUnitTable
  *********************************************
  -->
  <xsl:template name="AuthPhoneUnitTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'EU' ">
      		<xsl:text disable-output-escaping="yes">Unit number is extra - not expected at the address for this phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'MU' ">
      		<xsl:text disable-output-escaping="yes">Unit number is missing - expected for the address for this phone</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'WU' ">
      		<xsl:text disable-output-escaping="yes">Unit number wrong - unit number does not match unit number for the address at this phone</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthOFACValidationTable
  *********************************************
  -->
  <xsl:template name="AuthOFACValidationTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="number($code) = 1 ">
      		<xsl:text disable-output-escaping="yes">No Match</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 2 ">
      		<xsl:text disable-output-escaping="yes">Match to full name only</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 3 ">
      		<xsl:text disable-output-escaping="yes">Match to SSN only</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 4 ">
      		<xsl:text disable-output-escaping="yes">Match to name and SSN</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 5 ">
      		<xsl:text disable-output-escaping="yes">Match to name and DOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 6 ">
      		<xsl:text disable-output-escaping="yes">Match to name and YOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 7 ">
      		<xsl:text disable-output-escaping="yes">Match to SSN and DOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 8 ">
      		<xsl:text disable-output-escaping="yes">Match to SSN and YOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 9 ">
      		<xsl:text disable-output-escaping="yes">Match to name, SSN and  DOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 10 ">
      		<xsl:text disable-output-escaping="yes">Match to name, SSN, and YOB</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 11 ">
      		<xsl:text disable-output-escaping="yes">Match to company name only</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 12 ">
      		<xsl:text disable-output-escaping="yes">Match to company address only</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 13 ">
      		<xsl:text disable-output-escaping="yes">Match to company name and address</xsl:text>
    	</xsl:when>
	<xsl:when test="number($code) = 14 ">
      		<xsl:text disable-output-escaping="yes">Match to surname and first name</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  *********************************************
  * translate AuthTaxIDTable
  *********************************************
  -->
  <xsl:template name="AuthTaxIDTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'AB' ">
      		<xsl:text disable-output-escaping="yes">Match to business address only</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'B' ">
      		<xsl:text disable-output-escaping="yes">Match to business name and address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'DB' ">
      		<xsl:text disable-output-escaping="yes">Match to business name only</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'I' ">
      		<xsl:text disable-output-escaping="yes">Invalid format</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'M' ">
      		<xsl:text disable-output-escaping="yes">Tax ID missing</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'N' ">
      		<xsl:text disable-output-escaping="yes">No match to business name or address</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'U' ">
      		<xsl:text disable-output-escaping="yes">Unverifiable - not in database</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>


  <!--
  ********************************************************
  * translate RiskCategoryDescriptionCodeTable
  ********************************************************
  -->
  <xsl:template name="RiskCategoryDescriptionCodeTable">
    <xsl:param name="code" select="''" />

    <xsl:choose>		              
	<xsl:when test="normalize-space($code) = 'A' ">
      		<xsl:text disable-output-escaping="yes">This company is credit active, and pays its bills no later than 9 days late on average. there are no derogatory legal records on file for this company.</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'B' ">
      		<xsl:text disable-output-escaping="yes">This company has previously filed for bankruptcy.</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'C' ">
      		<xsl:text disable-output-escaping="yes">This company is credit active.  Based on the current payment performance and/or legal records on file for this company, Experian suggests further investigation if the credit amount requested warrants.</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'S' ">
      		<xsl:text disable-output-escaping="yes">Risk based on seriously derogatory payment performance and/or seriously derogatory legal records on file, Experian strongly recommends further investigation prior to making any credit or business decisions. 15% of businesses fall into this higher risk category.</xsl:text>
    	</xsl:when>
	<xsl:when test="normalize-space($code) = 'W' ">
      		<xsl:text disable-output-escaping="yes">Derogatory payment performance information and/or derogatory legal records exist on file for this company. Experian recommends further investigation prior to making credit or business decisions.</xsl:text>
    	</xsl:when>
	<xsl:otherwise>
      		<xsl:text disable-output-escaping="yes">Not available</xsl:text>
	</xsl:otherwise>
    </xsl:choose>    

  </xsl:template>

</xsl:stylesheet>