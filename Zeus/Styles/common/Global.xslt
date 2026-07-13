<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet
  version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns:prd="http://www.experian.com/BIS">

  <!--
  *********************************************
  * Output method
  *********************************************
  -->
  <xsl:output method="html"
    doctype-public="-//W3C//DTD HTML 4.0 Transitional//EN"
    doctype-system="http://www.w3c.org/TR/xhtml/DTD/xhtml1-strict.dtd"
    indent="yes" encoding="ISO-8859-1" />


  <!--
  *********************************************
  * Global variables and parameters
  *********************************************
  -->
  <xsl:variable name="titleColor">
    <xsl:value-of select="'#015CAE'" />
  </xsl:variable>

  <xsl:variable name="textColor">
    <xsl:value-of select="'#015CAE'" />
  </xsl:variable>

  <xsl:variable name="borderColor">
    <xsl:value-of select="'#015CAE'" />
  </xsl:variable>
  <xsl:variable name="subtitleColor">
    <xsl:value-of select="'#347dbe'" />
  </xsl:variable>
  <xsl:variable name="subtitleColorPP">
    <xsl:value-of select="'#b3b3b3'" />
  </xsl:variable>
  <xsl:variable name="scoreMeterWidth">
  	<xsl:value-of select="300"></xsl:value-of>
  </xsl:variable>

	<!-- Added for premier profile on July 06, 2012 -->
  <!-- text color for new reports (PPR, BOB, etc.) -->
  <xsl:variable name="reportTextColor">
    <xsl:value-of select="'#595959'" />
  </xsl:variable>

  <xsl:variable name="zebraStripColor">
  	<xsl:value-of select="'#e5f5fa'"></xsl:value-of>
  </xsl:variable>

  <xsl:variable name="alternateColor">
    <xsl:value-of select="'#e5f5fa'" />
  </xsl:variable>

  <xsl:variable name="lowRiskColor">
    <xsl:value-of select="'#209a5c'" />
  </xsl:variable>

  <xsl:variable name="lowMedRiskColor">
    <xsl:value-of select="'#8aca64'" />
  </xsl:variable>

  <xsl:variable name="medRiskColor">
    <xsl:value-of select="'#f4e35b'" />
  </xsl:variable>

  <xsl:variable name="medHighRiskColor">
    <xsl:value-of select="'#ee7240'" />
  </xsl:variable>

  <xsl:variable name="highRiskColor">
    <xsl:value-of select="'#d6373e'" />
  </xsl:variable>

  <xsl:variable name="lowRiskText">
    <xsl:value-of select="'LOW RISK'" />
  </xsl:variable>

  <xsl:variable name="lowMedRiskText">
    <xsl:value-of select="'LOW-MEDIUM RISK'" />
  </xsl:variable>

  <xsl:variable name="medRiskText">
    <xsl:value-of select="'MEDIUM RISK'" />
  </xsl:variable>

  <xsl:variable name="medHighRiskText">
    <xsl:value-of select="'MEDIUM-HIGH RISK'" />
  </xsl:variable>

  <xsl:variable name="highRiskText">
    <xsl:value-of select="'HIGH RISK'" />
  </xsl:variable>

  <xsl:variable name="ciModel">
    <xsl:value-of select="210" />
  </xsl:variable>

  <xsl:variable name="ciScoreOnlyModel">
    <xsl:value-of select="211" />
  </xsl:variable>

  <xsl:variable name="sbiModel">
    <xsl:value-of select="212" />
  </xsl:variable>

  <xsl:variable name="sbiScoreOnlyModel">
    <xsl:value-of select="213" />
  </xsl:variable>

  <xsl:variable name="ipV1Model">
    <xsl:value-of select="214" />
  </xsl:variable>

  <xsl:variable name="ipV1ScoreOnlyModel">
    <xsl:value-of select="215" />
  </xsl:variable>

  <xsl:variable name="ipV2Model">
    <xsl:value-of select="224" />
  </xsl:variable>

  <xsl:variable name="ipV2ScoreOnlyModel">
    <xsl:value-of select="225" />
  </xsl:variable>

  <xsl:variable name="fsrModel">
    <xsl:value-of select="223" />
  </xsl:variable>

  <xsl:variable name="score999">
    <xsl:value-of select="999" />
  </xsl:variable>

  <xsl:variable name="score998">
    <xsl:value-of select="998" />
  </xsl:variable>

  <xsl:variable name="basePath">
  	<xsl:value-of select="'../images/reports/'"></xsl:value-of>
  </xsl:variable>

 	<!-- Ends for Added for premier profile on July 06, 2012 -->


  <!-- This only applies to IP score models -->
  <!--<xsl:variable name="segmentUsed">
    <xsl:variable name="filler">
      <xsl:choose>
        <xsl:when test="//prd:IntelliscoreScoreInformation/prd:Filler">
          <xsl:value-of select="//prd:IntelliscoreScoreInformation/prd:Filler" />
        </xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>

    <xsl:choose>
      <xsl:when test="starts-with(normalize-space($filler), 'L')">
        <xsl:value-of select="1" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'M')">
        <xsl:value-of select="2" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'S')">
        <xsl:value-of select="3" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'X')">
        <xsl:value-of select="4" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'D')">
        <xsl:value-of select="5" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'B') and normalize-space($filler) != 'BK'">
        <xsl:value-of select="6" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'P')">
        <xsl:value-of select="7" />
      </xsl:when>
      <xsl:when test="normalize-space($filler) = 'BK'">
        <xsl:value-of select="98" />
      </xsl:when>
      <xsl:when test="starts-with(normalize-space($filler), 'N')">
        <xsl:value-of select="99" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="0" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>-->

  <!-- This only applies to IP score models -->
  <!--<xsl:variable name="isCommercial">
    <xsl:choose>
      <xsl:when test="number($segmentUsed) != 0 and (number($segmentUsed) = 6 or number($segmentUsed) = 7)">
        <xsl:value-of select="0" />
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="1" />
      </xsl:otherwise>
    </xsl:choose>
  </xsl:variable>-->

</xsl:stylesheet>