<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
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
  * Convert Case template
  *********************************************
  -->

<xsl:template name="convertcase">
	<xsl:param name="toconvert" />
	<xsl:param name="conversion" />

	<xsl:variable name="lcletters">abcdefghijklmnopqrstuvwxyz</xsl:variable>
	<xsl:variable name="ucletters">ABCDEFGHIJKLMNOPQRSTUVWXYZ</xsl:variable>
	
	<xsl:choose>
		<xsl:when test="$conversion='lower'">
			<xsl:value-of select="translate($toconvert,$ucletters,$lcletters)"/>
		</xsl:when>
		<xsl:when test="$conversion='upper'">
			<xsl:value-of select="translate($toconvert,$lcletters,$ucletters)"/>
		</xsl:when>
		<xsl:when test="$conversion='proper'">
			<xsl:call-template name="convertpropercase">
			<xsl:with-param name="toconvert">
				<xsl:value-of select="translate($toconvert,$ucletters,$lcletters)"/>
			</xsl:with-param>
			</xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="concat($conversion, $toconvert)" />
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

<xsl:template name="convertpropercase">
<xsl:param name="toconvert" />

<xsl:if test="string-length($toconvert) > 0">
	<xsl:variable name='f' select='substring($toconvert, 1, 1)' />
	<xsl:variable name='s' select='substring($toconvert, 2)' />
	
	<xsl:call-template name='convertcase'>
		<xsl:with-param name='toconvert' select='$f' />
		<xsl:with-param name='conversion'>upper</xsl:with-param>
	</xsl:call-template>

<xsl:choose>
	<xsl:when test="contains($s,' ')">
		<xsl:value-of select='substring-before($s," ")'/><xsl:text> </xsl:text>
		<xsl:call-template name='convertpropercase'>
		<xsl:with-param name='toconvert' select='normalize-space(substring-after($s," "))' />
		</xsl:call-template>
	</xsl:when>
	<xsl:when test="contains($s,'-')">
		<xsl:value-of select='substring-before($s,"-")'/><xsl:text>-</xsl:text>
		<xsl:call-template name='convertpropercase'>
		<xsl:with-param name='toconvert' select='normalize-space(substring-after($s,"-"))' />
		</xsl:call-template>
	</xsl:when>
	<xsl:otherwise>
		<xsl:value-of select='$s'/>
	</xsl:otherwise>
</xsl:choose>
</xsl:if>
</xsl:template>

</xsl:stylesheet>