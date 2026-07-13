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
	<!-- Added for premier profile on July 06, 2012 -->


  <!--
  *********************************************
  * SpellDigitNumber template
  * Formats figure numeric value to spelled string
  *********************************************
  -->
  <xsl:template name="SpellDigitNumber">
    <xsl:param name="upperCase" select="false()" />
    <xsl:param name="value" />

    <xsl:variable name="result">
      <xsl:choose>
        <xsl:when test="number($value) = 1">
          <xsl:text disable-output-escaping="yes">One</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 2">
          <xsl:text disable-output-escaping="yes">Two</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 3">
          <xsl:text disable-output-escaping="yes">Three</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 4">
          <xsl:text disable-output-escaping="yes">Four</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 5">
          <xsl:text disable-output-escaping="yes">Five</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 6">
          <xsl:text disable-output-escaping="yes">Six</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 7">
          <xsl:text disable-output-escaping="yes">Seven</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 8">
          <xsl:text disable-output-escaping="yes">Eight</xsl:text>
        </xsl:when>
        <xsl:when test="number($value) = 9">
          <xsl:text disable-output-escaping="yes">Nine</xsl:text>
        </xsl:when>
        <xsl:otherwise>
          <xsl:text disable-output-escaping="yes">Unknown</xsl:text>
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
  ****************************************************
  * RiskClassByScore
  * Determine risk class by a given model's score
  ****************************************************
   -->
  <xsl:template name="RiskClassByScore">
    <xsl:param name="model" />
    <xsl:param name="score" />
	<xsl:choose>
		<xsl:when test="$model=$ipV1Model or $model=$ipV1ScoreOnlyModel">
			<xsl:choose>
  				<xsl:when test="$score &lt; 11">
					<xsl:value-of select="5"></xsl:value-of>
	    		</xsl:when>
	    		<xsl:when test="$score &lt; 26">
					<xsl:value-of select="4"></xsl:value-of>
   				</xsl:when>
   				<xsl:when test="$score &lt; 51">
					<xsl:value-of select="3"></xsl:value-of>
				</xsl:when>
				<xsl:when test="$score &lt; 76">
					<xsl:value-of select="2"></xsl:value-of>
	    		</xsl:when>
	    		<xsl:when test="$score &lt; 101">
					<xsl:value-of select="1"></xsl:value-of>
   				</xsl:when>
   			</xsl:choose>
   		</xsl:when>
   		<xsl:when test="$model=$ciModel or $model=$ciScoreOnlyModel">
   			<xsl:choose>
   				<xsl:when test="$score &lt; 32.48">
					<xsl:value-of select="5"></xsl:value-of>
	    		</xsl:when>
	    		<xsl:when test="$score &lt; 58.64">
					<xsl:value-of select="4"></xsl:value-of>
   				</xsl:when>
   				<xsl:when test="$score &lt; 70.20">
					<xsl:value-of select="3"></xsl:value-of>
				</xsl:when>
				<xsl:when test="$score &lt; 77.72">
					<xsl:value-of select="2"></xsl:value-of>
   				</xsl:when>
   				<xsl:when test="$score &lt; 100.01">
					<xsl:value-of select="1"></xsl:value-of>
				</xsl:when>
			</xsl:choose>
   		</xsl:when>
   		<xsl:otherwise>
			<xsl:value-of select="''"></xsl:value-of>
   		</xsl:otherwise>
	</xsl:choose>
  </xsl:template>

  <!--
  ***********************************************
  * Formats address info into one single line
  ***********************************************
  -->
  <xsl:template name="FormatAddressLine">
    <xsl:param name="businessName" />
    <xsl:param name="street1" />
    <xsl:param name="street2" />
    <xsl:param name="city" />
    <xsl:param name="state"/>
    <xsl:param name="zip"/>
    <xsl:param name="zipExt"/>
    <xsl:if test="$businessName!=''">
    <xsl:value-of select="concat($businessName,', ')"/>
    </xsl:if>
    <xsl:if test="$street1!=''">
	<xsl:value-of select="concat($street1,', ')"/>
	</xsl:if>
	<xsl:if test="street2!=''">
	<xsl:value-of select="concat($street2,', ')"/>
	</xsl:if>
		<xsl:value-of select="$city"></xsl:value-of>
		<xsl:if test="$city!='' and $state!=''">, </xsl:if>
		<xsl:value-of select="$state"></xsl:value-of>
		<xsl:if test="($city!='' or $state!='') and $zip!=''"> </xsl:if>
		<xsl:value-of select="$zip"></xsl:value-of>
		<xsl:if test="$zipExt!=''">-</xsl:if>
		<xsl:value-of select="$zipExt"></xsl:value-of>
  </xsl:template>

  <!--
  ***********************************************
  * Formats address info in the following layout
  * 123 Main St
  * Apt 123
  * Los Angeles, CA 92020-1234
  ***********************************************
  -->
  <xsl:template name="FormatAddress">
    <xsl:param name="street1" />
    <xsl:param name="street2" />
    <xsl:param name="city" />
    <xsl:param name="state"/>
    <xsl:param name="zip"/>
    <xsl:param name="zipExt"/>
	<div><xsl:value-of select="$street1"></xsl:value-of></div>
	<xsl:if test="street2!=''">
	<div><xsl:value-of select="$street2"></xsl:value-of></div>
	</xsl:if>
	<div>
		<xsl:value-of select="$city"></xsl:value-of>
		<xsl:if test="$city!='' and $state!=''">, </xsl:if>
		<xsl:value-of select="$state"></xsl:value-of>
		<xsl:if test="($city!='' or $state!='') and $zip!=''"><xsl:value-of select="' '"></xsl:value-of></xsl:if>
		<xsl:value-of select="$zip"></xsl:value-of>
		<xsl:if test="$zipExt!=''">-</xsl:if>
		<xsl:value-of select="$zipExt"></xsl:value-of>
	</div>
  </xsl:template>

 <!--
  *********************************************
  * TranslateBusinessType template
  * Value has to be 1 chr
  *********************************************
  -->
  <xsl:template name="TranslateBusinessType">
    <xsl:param name="value" select="''" />
    <xsl:param name="upperCase" select="false()" />

    <xsl:variable name="result">
      <xsl:choose>
        <xsl:when test="$value= 'C'">
          <xsl:value-of select="'Corporation'" />
        </xsl:when>

        <xsl:when test="$value= 'G'">
          <xsl:value-of select="'General Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'H'">
          <xsl:value-of select="'Chain Store'" />
        </xsl:when>

        <xsl:when test="$value= 'I'">
          <xsl:value-of select="'Institutions'" />
        </xsl:when>

        <xsl:when test="$value= 'L'">
          <xsl:value-of select="'Limited Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'F'">
          <xsl:value-of select="'Fortune 1000'" />
        </xsl:when>

        <xsl:when test="$value= 'P'">
          <xsl:value-of select="'Partnership'" />
        </xsl:when>

        <xsl:when test="$value= 'R'">
          <xsl:value-of select="'Residential'" />
        </xsl:when>

        <xsl:when test="$value= 'S'">
          <xsl:value-of select="'Sole Proprietor'" />
        </xsl:when>

        <xsl:when test="$value= 'X'">
          <xsl:value-of select="'S Corporation'" />
        </xsl:when>

        <xsl:when test="$value= 'Y'">
          <xsl:value-of select="'Corporation'" />
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

  <xsl:template name="dummyChart">
  	  <xsl:param name="altText" select="''" />
  	  <xsl:param name="width" select="''" />
  	  <xsl:element name="img">
  	  	<xsl:attribute name="alt">
  	  		<xsl:value-of select="$altText"></xsl:value-of>
  	  	</xsl:attribute>
  	  	<xsl:attribute name="title">
  	  		<xsl:value-of select="$altText"></xsl:value-of>
  	  	</xsl:attribute>
  	  	<xsl:if test="$width!=''">
	  	  	<xsl:attribute name="width">
	  	  		<xsl:value-of select="$width"></xsl:value-of>
	  	  	</xsl:attribute>
  	  	</xsl:if>
  	  	<xsl:attribute name="src">
  	  		<xsl:value-of select="'../images/default_bar_chart.gif'"></xsl:value-of>
  	  	</xsl:attribute>
  	  </xsl:element>
  </xsl:template>

  <xsl:template name="nbsp">
	<xsl:text disable-output-escaping="yes"><![CDATA[&nbsp;]]></xsl:text>
  </xsl:template>

 	<!-- Ends for Added for premier profile on July 06, 2012 -->
</xsl:stylesheet>