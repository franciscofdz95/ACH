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
  * LegalFilingsCollections template
  *********************************************
  -->
  <xsl:template name="LegalFilingsCollections">

    <!-- Section title -->
    <xsl:call-template name="SectionTitle">
      <xsl:with-param name="title" select="'Legal Filings and Collections'" />
      <xsl:with-param name="color" select="$titleColor" />
    </xsl:call-template>

    <xsl:if test="prd:CollectionData">
       <!-- CollectionSection -->
       <xsl:call-template name="CollectionSection" />
    </xsl:if>

    <xsl:if test="prd:Bankruptcy">

       <xsl:if test="prd:CollectionData">
	    <!-- back to top graphic -->
	    <xsl:call-template name="BackToTop" />
       </xsl:if>

	<!-- BankruptcySection -->
	<xsl:call-template name="BankruptcySection" />
    </xsl:if>
  
    <xsl:if test="prd:TaxLien">

       <xsl:if test="prd:CollectionData or prd:Bankruptcy">
	    <!-- back to top graphic -->
	    <xsl:call-template name="BackToTop" />
       </xsl:if>

      <!-- TaxLienSection -->
      <xsl:call-template name="TaxLienSection" />
    </xsl:if>
      
    <xsl:if test="prd:JudgmentOrAttachmentLien">

       <xsl:if test="prd:CollectionData or prd:Bankruptcy or prd:TaxLien">
	    <!-- back to top graphic -->
	    <xsl:call-template name="BackToTop" />
       </xsl:if>
      <!-- JudgmentSection -->
      <xsl:call-template name="JudgmentSection" />
    </xsl:if>
    
    <br />
      
  </xsl:template>
    
  
  <!--
  *********************************************
  * CollectionSection template
  *********************************************
  -->
  <xsl:template name="CollectionSection">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="7" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <a name="collections"><b><font color="#ffffff">Collections</font></b></a>
                    </td>
                  </tr>

                  <!-- collection header  -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="8%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date<br />Placed</b></font></td>
                    <td align="center" width="20%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Status</b></font></td>
                    <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Original<br />Balance</b></font></td>
                    <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Outstanding<br />Balance</b></font></td>
                    <td align="center" width="8%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Date<br />Closed</b></font></td>
                    <td align="center" width="25%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Agency</b></font></td>
                    <td align="center" width="15%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Agency<br />Phone</b></font></td>
                  </tr>
			
				<!-- CollectionData template -->
				<xsl:apply-templates select="prd:CollectionData">
					<xsl:sort order="descending" select="prd:DatePlacedForCollection" />
				</xsl:apply-templates>
				
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
  * CollectionData template
  *********************************************
  -->
  <xsl:template match="prd:CollectionData" xml:space="preserve">



    <!-- StreetAddress -->
     <xsl:variable name="streetAddress">
	<xsl:variable name="StreetAddress">
	  <xsl:value-of select="prd:StreetAddress" />
	</xsl:variable>
	
	<xsl:call-template name="convertcase">
	   <xsl:with-param name="toconvert" select="$StreetAddress" />
	   <xsl:with-param name="conversion" select="'proper'" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="status">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:AccountStatus/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateCollStatus">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="amountPlaced">
      <xsl:choose>		              
        <xsl:when test="prd:AmountPlacedForCollection">		    		   		   
          <xsl:value-of select="number(prd:AmountPlacedForCollection)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="amountPaid">
      <xsl:choose>		              
        <xsl:when test="prd:AmountPaid">		    		   		   
          <xsl:value-of select="number(prd:AmountPaid)" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="outstanding">
      <xsl:choose>		              
        <xsl:when test="($amountPlaced &gt; 0) and ($amountPaid &lt;= $amountPlaced)">		    		   		   
          <xsl:value-of select="format-number(($amountPlaced - $amountPaid), '$###,###,##0')" />
        </xsl:when>

        <xsl:when test="($amountPlaced &gt; 0) and ($amountPaid &gt; $amountPlaced)">		    		   		   
          <xsl:value-of select="'$0'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'Undisclosed'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="dateClosed">
      <xsl:choose>		              
        <xsl:when test="prd:DateClosed and number(prd:DateClosed) != 0">		    		   		   
    		   <xsl:call-template name="FormatDate">
    		     <xsl:with-param name="pattern" select="'mo/year'" />
    		     <xsl:with-param name="value" select="prd:DateClosed" />
    		   </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <xsl:variable name="agencyName">
      <xsl:choose>		              
        <xsl:when test="prd:CollectionAgencyInfo/prd:AgencyName">		    		   		   
          <xsl:value-of select="translate(prd:CollectionAgencyInfo/prd:AgencyName, 'amp;amp;', 'amp;')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'Undisclosed'" />
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    
    <xsl:variable name="agencyPhone">
      <xsl:choose>		              
        <xsl:when test="prd:CollectionAgencyInfo/prd:PhoneNumber">		    		   		   
			  <xsl:call-template name="FormatPhone">
			    <xsl:with-param name="value" select="prd:CollectionAgencyInfo/prd:PhoneNumber" />
			  </xsl:call-template>
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
     
    <!-- collection data  -->
    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/year'" />
		     <xsl:with-param name="value" select="prd:DatePlacedForCollection" />
		   </xsl:call-template>
        </font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$status" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="format-number($amountPlaced, '$###,###,##0')" /></font>
            </td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="88%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$outstanding" /></font>
            </td>
            <td width="12%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$dateClosed" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$agencyName" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$agencyPhone" /></font>
      </td>

    </tr>

  </xsl:template>


  <!--
  *********************************************
  * BankruptcySection template
  *********************************************
  -->
  <xsl:template name="BankruptcySection">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="9" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <a name="bankruptcies"><b><font color="#ffffff">Bankruptcies</font></b></a>
                    </td>
                  </tr>

                  <!-- bankruptcy header  -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>File<br />Date</b></font></td>
                    <td align="center" width="13%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Type</b></font></td>
                    <td align="center" width="15%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Status</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Liability<br />Amount</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Asset<br />Amount</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Exempt<br />Amount</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Owner</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Number</b></font></td>
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Jurisdiction</b></font></td>
                  </tr>

				<!-- bankruptcy template -->
				<xsl:apply-templates select="prd:Bankruptcy">
					<xsl:sort order="descending" select="prd:DateFiled" />
				</xsl:apply-templates>
				
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
  * Bankruptcy template
  *********************************************
  -->
  <xsl:template match="prd:Bankruptcy" xml:space="preserve">
    <xsl:variable name="status">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalAction/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingStatus">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="filingType">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalType/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingType">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>
    
    <xsl:variable name="liabilityAmount">
      <xsl:choose>		              
        <xsl:when test="prd:LiabilityAmount">		    		   		   
          <xsl:value-of select="format-number(prd:LiabilityAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="assetAmount">
      <xsl:choose>		              
        <xsl:when test="prd:AssetAmount">		    		   		   
          <xsl:value-of select="format-number(prd:AssetAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
        
    <xsl:variable name="exemptAmount">
      <xsl:choose>		              
        <xsl:when test="prd:ExemptAmount">		    		   		   
          <xsl:value-of select="format-number(prd:ExemptAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="owner">
      <xsl:choose>		              
        <xsl:when test="prd:Owner">		    		   		   
          <xsl:value-of select="prd:Owner" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="jurisdiction">
      <xsl:choose>		              
        <xsl:when test="prd:FilingLocation">		    		   		   
          <xsl:value-of select="prd:FilingLocation" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="filingNumber">
      <xsl:value-of select="prd:DocumentNumber" />
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
     
    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
		     <xsl:with-param name="value" select="prd:DateFiled" />
		   </xsl:call-template>
        </font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingType" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$status" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$liabilityAmount" /></font>
            </td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="88%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$assetAmount" /></font>
            </td>
            <td width="12%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="88%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$exemptAmount" /></font>
            </td>
            <td width="12%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
        <xsl:value-of select="$owner" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingNumber" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$jurisdiction" /></font>
      </td>

    </tr>

  </xsl:template>


  <!--
  *********************************************
  * TaxLienSection template
  *********************************************
  -->
  <xsl:template name="TaxLienSection">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="9" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <a name="taxliens"><b><font color="#ffffff">Tax Liens</font></b></a>
                    </td>
                  </tr>

                  <!-- tax lien header  -->
                  <tr bgcolor="#ffffff">
                    <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>File<br />Date</b></font></td>
                    <td align="center" width="16%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Type</b></font></td>
                    <td align="center" width="15%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Status</b></font></td>
                    <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Amount</b></font></td>
                    <td align="center" width="22%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Number</b></font></td>
                    <td align="center" width="25%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Jurisdiction</b></font></td>
                  </tr>

				<!-- TaxLien template -->
				<xsl:apply-templates select="prd:TaxLien">
					<xsl:sort order="descending" select="prd:DateFiled" />
				</xsl:apply-templates>
				
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
  * TaxLien template
  *********************************************
  -->
  <xsl:template match="prd:TaxLien" xml:space="preserve">
    <xsl:variable name="status">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalAction/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingStatus">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="filingType">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalType/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingType">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>
   
    <xsl:variable name="amount">
      <xsl:choose>		              
        <xsl:when test="prd:LiabilityAmount">		    		   		   
          <xsl:value-of select="format-number(prd:LiabilityAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="jurisdiction">
      <xsl:choose>		              
        <xsl:when test="prd:FilingLocation">		    		   		   
          <xsl:value-of select="prd:FilingLocation" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="filingNumber">
      <xsl:value-of select="prd:DocumentNumber" />
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
     
    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
		     <xsl:with-param name="value" select="prd:DateFiled" />
		   </xsl:call-template>
        </font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingType" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$status" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$amount" /></font>
            </td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingNumber" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$jurisdiction" /></font>
      </td>

    </tr>

  </xsl:template>


  <!--
  *********************************************
  * JudgmentSection template
  *********************************************
  -->
  <xsl:template name="JudgmentSection">
    <table width="100%" border="0" cellspacing="0" cellpadding="1">
      <tr>
        <td bgcolor="{$borderColor}">
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td bgcolor="#ffffff">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td bgcolor="{$borderColor}" colspan="9" align="left" valign="middle" height="23">
                      <img src="../images/spacer.gif" border="0" width="5" height="1" alt=""/>
                      <a name="judgments"><b><font color="#ffffff">Judgments</font></b></a>
                    </td>
                  </tr>

                    <!-- judgment header  -->
                    <tr bgcolor="#ffffff">
                      <td align="center" width="10%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>File<br />Date</b></font></td>
                      <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Type</b></font></td>
                      <td align="center" width="11%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Status</b></font></td>
                      <td align="center" width="12%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Amount</b></font></td>
                      <td align="center" width="17%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Plaintiff</b></font></td>
                      <td align="center" width="18%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Filing<br />Number</b></font></td>
                      <td align="center" width="20%"><font size="1" style="FONT-FAMILY: 'verdana';"><b>Jurisdiction</b></font></td>
                    </tr>

				<!-- JudgmentOrAttachmentLien template -->
				<xsl:apply-templates select="prd:JudgmentOrAttachmentLien">
					<xsl:sort order="descending" select="prd:DateFiled" />
				</xsl:apply-templates>
				
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
  * JudgmentOrAttachmentLien template
  *********************************************
  -->
  <xsl:template match="prd:JudgmentOrAttachmentLien" xml:space="preserve">
    <xsl:variable name="status">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalAction/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingStatus">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="filingType">
       <xsl:variable name="code">
      		<xsl:value-of select="prd:LegalType/@code" />
      	</xsl:variable>

	<xsl:call-template name="translateLegalFilingType">
	   <xsl:with-param name="code" select="$code" />
	</xsl:call-template>
    </xsl:variable>

    <xsl:variable name="amount">
      <xsl:choose>		              
        <xsl:when test="prd:LiabilityAmount">		    		   		   
          <xsl:value-of select="format-number(prd:LiabilityAmount, '$###,###,##0')" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'$0'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="jurisdiction">
      <xsl:choose>		              
        <xsl:when test="prd:FilingLocation">		    		   		   
          <xsl:value-of select="prd:FilingLocation" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="plaintiff">
      <xsl:choose>		              
        <xsl:when test="prd:PlaintiffName">		    		   		   
          <xsl:value-of select="prd:PlaintiffName" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="''" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>
    
    <xsl:variable name="filingNumber">
      <xsl:value-of select="prd:DocumentNumber" />
    </xsl:variable>
    
    <xsl:variable name="bgColor">
      <xsl:choose>		              
        <xsl:when test="position() mod 2 = 1">		    		   		   
          <xsl:value-of select="'#e5f5fa'" />
        </xsl:when>

        <xsl:otherwise>
          <xsl:value-of select="'#ffffff'" />
        </xsl:otherwise>
      </xsl:choose>    
    </xsl:variable>

    <tr>
      <td height="20" bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';">
		   <xsl:call-template name="FormatDate">
		     <xsl:with-param name="pattern" select="'mo/dt/year'" />
		     <xsl:with-param name="value" select="prd:DateFiled" />
		   </xsl:call-template>
        </font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingType" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$status" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td width="85%" align="right"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$amount" /></font>
            </td>
            <td width="15%">
            </td>
          </tr>
        </table>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$plaintiff" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$filingNumber" /></font>
      </td>

      <td bgcolor="{normalize-space($bgColor)}" align="center"><font size="1" style="FONT-FAMILY: 'verdana';"><xsl:value-of select="$jurisdiction" /></font>
      </td>

    </tr>

  </xsl:template>

</xsl:stylesheet>