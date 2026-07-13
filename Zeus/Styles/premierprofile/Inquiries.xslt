<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:prd="http://www.experian.com/ARFResponse">


  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="xml" indent="yes"/>


  <!--
  *********************************************
  * Inquiries template
  *********************************************
  -->
  <xsl:template name="Inquiries">
    <!-- Section title -->
  	<xsl:if test="prd:Inquiry">
	<table class="section dataTable" width="100%" cellspacing="0" cellpadding="0">
		<thead>
			<tr>
				<th><a class="report_section_title">Inquiries</a></th>
			</tr>
		</thead>
	</table>

    <table class="section subsection dataTable" width="100%" border="0" cellspacing="0" cellpadding="1">
    	<tr class="subtitle">
    		<th colspan="10">Summary of Inquiries</th>
    	</tr>
      	<tr class="datahead">
                    <td><div class="label">Business<br/>Category</div></td>
                    <!-- make year month header -->
                    <xsl:apply-templates select="prd:Inquiry[last()]/prd:InquiryCount" mode="header" />
      	</tr>

                  <!-- row of inquiry count -->
                  <xsl:apply-templates select="prd:Inquiry" mode="BPR" >
                  </xsl:apply-templates>

    </table>
      <!-- back to top image -->
      <xsl:call-template name="BackToTop" />
	</xsl:if>
  </xsl:template>


  <!--
  *********************************************
  * Inquiry template
  *********************************************
  -->
  <xsl:template match="prd:Inquiry" mode="BPR">
    <xsl:variable name="category">
      <xsl:choose>
        <xsl:when test="position()=last()">
          <xsl:value-of select="'Totals'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="normalize-space(prd:InquiryBusinessCategory)" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <tr>
      <xsl:attribute name="class">
      <xsl:choose>
        <xsl:when test="position()=last()">
          <xsl:value-of select="'summary'"/>
        </xsl:when>
        <xsl:when test="position() mod 2 = 1">
          <xsl:value-of select="'even'"/>
        </xsl:when>
        <xsl:otherwise>
	      <xsl:value-of select="'odd'"/>
        </xsl:otherwise>
      </xsl:choose>
      </xsl:attribute>
      <td><div><xsl:value-of select="$category" /></div></td>
      <xsl:apply-templates select="prd:InquiryCount" mode="count">
      </xsl:apply-templates>
    </tr>
  </xsl:template>


  <!--
  *********************************************
  * InquiryCount template
  *********************************************
  -->
  <xsl:template match="prd:InquiryCount" mode="header">
    <xsl:variable name="date">
      <xsl:variable name="month">
  		   <xsl:call-template name="FormatMonth">
  		     <xsl:with-param name="monthValue" select="number(substring(prd:Date, 5, 2))" />
  		     <xsl:with-param name="upperCase" select="true()" />
  		   </xsl:call-template>
      </xsl:variable>

      <xsl:value-of select="concat(normalize-space($month), normalize-space(substring(prd:Date, 3, 2)))" />
    </xsl:variable>

    <td><div class="label"><xsl:value-of select="$date" /></div></td>
  </xsl:template>


  <!--
  *********************************************
  * InquiryCount template
  *********************************************
  -->
  <xsl:template match="prd:InquiryCount" mode="count">
    <xsl:variable name="count">
      <xsl:choose>
        <xsl:when test="number(prd:Count) &gt; 0">
          <xsl:value-of select="number(normalize-space(prd:Count))" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:call-template name="nbsp"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <td><div><xsl:value-of disable-output-escaping="yes" select="$count" /></div></td>
  </xsl:template>

</xsl:stylesheet>