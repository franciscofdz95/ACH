<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:rsp="http://www.experian.com/NetConnectResponse"
                xmlns:prd="http://www.experian.com/ARFResponse">
  <!--
  ***********************************************
  * ReportCSS template
  * Write CSS style inside <head> element to format reports
  ***********************************************
  -->
  <xsl:template name="ReportCSS">
	<style type="text/css">
		/**
		 * Start css for new reports that works with iText
		 */
		@media screen
		{
			.report_container .print_only {display: none;}
			.report_container .hidden_on_screen {display:none}
		}

		@media print
		{
			.report_container .fusion_chart {display: none;}
			.report_container .show_on_print {display:block}
			.report_container .hidden_on_print {display:none;}

			.report_container img[src$="spacer.gif"] {
				visibility:hidden;
			}

			.report_container #report_footer_title {
				display: block;
				position: running(footertitle);
				margin-left: 6px;
				font-size: 10px;
			}

			.report_container #report_footer_counter {
				display: block;
				position: running(footercounter);
				padding:0;
				margin-right: -10px;
				font-size: 10px;
			}

			.report_container #page_number:before {
				content: counter(page);
			}

			.report_container #page_count:before {
				content: counter(pages);
			}

			/*Below may cause FS out of memory*/
			/*table.section {
				page-break-inside : avoid;
				-fs-keep-with-inline : keep;
				-fs-table-paginate: paginate;
			}*/
		}

		@page {
			@bottom-left {
			 	content: element(footertitle);
			}
			@bottom-right {
				content: element(footercounter);
			}

			size: 8.5in 11in;
			margin-right: 0in;
			margin-left: 0.375in;
			margin-top: 0.25in;
			margin-bottom: 0.5in;
			border: none;
			padding: 0;
			tex-align: center;
		}


		@media all {
		body .report_container,.report_container a,.report_container td,.report_container th {
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 11px;
			font-family: arial,verdana,helvetica, sans-serif;
		}

		.report_body sup {
			color			:	inherit;
			#color			:	#595959;
			font-size		:	smaller;
			vertical-align	:	super;
		}

		.report_container li{
			list-style:none;
		}

		.report_container a[href] {
			text-decoration: underline;
		}

		.report_container {
			width: 715px;
		}

		.report_container .product_title {
			font-size: 18px;
			font-family: arial, helvetica, sans-serif;
		}

		.report_container .report_section_title {
			font-size: 16px;
			font-family: arial, helvetica, sans-serif;
		}

		.report_container .font_size_2 {
			font-size: 12px;
			font-family: arial, helvetica, sans-serif;
		}

		.report_container .font_size_3 {
			font-size: 16px;
			font-family: arial, helvetica, sans-serif;
		}

		.report_container .font_size_4 {
			font-size: 18px;
			font-family: arial, helvetica, sans-serif;
		}

		.report_container ul.list {
			padding: 5px 0 0 6px;
			margin:0;
		}

		.report_container .list li {
			padding: 0 0 0 10px;
			margin: 0 0 5px 0;
			background: transparent url(<xsl:value-of select="$basePath"/>sprites_icons.png) no-repeat -95px -244px;
		}

		.report_container table.section {
			border: 1px solid <xsl:value-of select = "$borderColor"/>;
			align: left;
			valign: top;
			margin-top: 5px;
		}

 		.report_container .tableheight {
 			height		:		18px;
 		}
		.report_container table.subsection {
			margin-top: 0;
		}

		.report_container table tr.even td, .report_container table tr td.even {
			/*background-color	:	<xsl:value-of select = "$zebraStripColor"/>;*/
			border-bottom		:	1px solid #b3b3b3;
		}
		.report_container table tr.odd td, .report_container table tr td.odd {
			/*background-color	:	<xsl:value-of select = "$zebraStripColor"/>;*/
			border-bottom		:	1px solid #b3b3b3;
		}

		.report_container table.section thead th,.report_container table.section thead td, .report_container table.section .subtitle th, .report_container table.section .subtitle td{
			height	:	23px;
			vertical-align	:	middle;
		}
		.report_container table.dataTable tr td, .report_container table.dataTable tr th{
			height		: 	20px;
			padding		:	0 5px;
		}
		.report_container table.section thead th{
			/*background-color:	<xsl:value-of select="$titleColor"/>;*/
			/*background		:	#fff url(<xsl:value-of select="$basePath"/>titlebar.png) repeat-x 0 0;
			height			:	30px;*/
			background		:	#fff url(<xsl:value-of select="$basePath"/>blue-tile-3.gif) repeat-x 0 0;
			height			:	24px;
			color			:	#FFFFFF;
			font-size		:	12px;
			font-weight		:	bold;
		}

		.report_container table.section thead th a {
			height			:	24px;
			color			:	#FFFFFF;
			font-size		:	12px;
			padding-top		:	4px;
		}

		.report_container table.section thead th.doubleheightTitle {
			background		:	#fff url(<xsl:value-of select="$basePath"/>blue-tile-double-line.gif) repeat-x 0 0;
		}
		.report_container table.section thead th.doubleheightTitle, .report_container table.section thead th.doubleheightTitle a{
			height			:	54px;
		}

		.report_container table th a {
			font-size		:	12px;
		}

		.report_container table.section tr.subtitle th, .report_container table.section tr.subtitle th a{
			background-color:	<xsl:value-of select = "$subtitleColorPP"/>;
			background-image:	none;
			font-size		:	11px;
			font-weight		:	bold;
			/*color			:	<xsl:value-of select = "$reportTextColor"/>;*/
			color			:	black
		}

		.report_container .firstColumn {
			/*width: 100%;*/
			padding: 0 5px;
		}

		.report_container div span.label {
			clear		:		both;
			float		:		left;
		}

		.report_container div span.value {
			float		:		right;
		}

		.report_container table.section td {
			border-style		:		none;
			border-width		:		0px;
			vertical-align		:		top;
		}

		.report_container table.dataTable td{
			vertical-align		:		middle;
		}

		.report_container table.section td.label.forImage {
			vertical-align: middle;
		}

		.report_container table tr.datahead th, .report_container table tr.datahead td{
			background-color:	#FFFFFF;
			color			:	<xsl:value-of select = "$reportTextColor"/>;
			font-weight		:	bold;
			border-bottom	:	solid 1px <xsl:value-of select = "$borderColor"/>;
		}

		.report_container table tr.summaryhead th, .report_container table tr.summaryhead td{
			background-color:	#FFFFFF;
			color			:	<xsl:value-of select = "$reportTextColor"/>;
			border-bottom	:	solid 1px <xsl:value-of select = "$borderColor"/>;
			text-align		:	center;
			font-size		:	11px;
		}

		.report_container table tr.summary th, .report_container table tr.summary td {
			background-color:	#cccccc;
			font-weight		:	bold;
			height			:	20px;
			border-top		:	solid 1px #cccccc;	/*<xsl:value-of select = "$borderColor"/>;*/
			/*border-bottom	:	solid 1px <xsl:value-of select = "$borderColor"/>;*/
		}

		.report_container .label {
			font-weight: bold;
		}
		.report_container .secondLevel {
			padding-left	:	16pt;
		}

		.report_container .rightalign{
			text-align: right;
		}

		.report_container table.section th, .report_container table tr.subtitle td {
			text-align: left;
			padding-left: 5px;
		}

		.report_container table.section tbody {
			background-color: #FFFFFF;
		}

		.report_container table.section thead th .titleLabel {
			font-size: 16px;
		}

		.report_container table tr td.centerLabel,.report_container table tr th.centerLabel, .report_container table tr th .centerLabel,.report_container table tr td .centerLabel {
			text-align	:	center;
			width		:	100%;
		}

		.report_container table.section thead th .smallTitle {
			font-size	:	11px;
			font-weight	:	normal;
		}

		.report_container .GrayBox,.report_container .BigPad,.report_container .MiddlePad,.report_container .SmallPad,.report_container .scoreGraphic {
			font-family: arial, verdana, helvetica, sans-serif;
		}

		.report_container .verticalMiddleBox {
			#position	:	relative;
			display		:	table;
			overflow	:	hidden;
			text-align	:	center;
		}
		.report_container .verticalMiddleBox .wrapInner {
		    #position		:	absolute;
		    #top			:	50%;
		    #left			:	50%;
		    display			:	table-cell;
		    vertical-align	:	middle;
		}
		.report_container .verticalMiddleBox .wrapInner .innerText{
			#position		:	relative;
			#top			:	-50%;
			#left			:	-50%
		}

		.report_container table td.leftborder {
			border-left		:	solid 1px <xsl:value-of select = "$borderColor"/>;
		}

		.report_container table td.bottomborder {
			border-bottom		:	solid 1px <xsl:value-of select = "$borderColor"/>;
		}

		.report_container .grayOuterBox {
			/*background-color	:	#cccccc;*/
			text-align			:	center;
			font-size			:	10px;
			/*font-weight			:	bold;*/
			color				:	<xsl:value-of select = "$reportTextColor"/>;
			padding				:	4px!important;
		}
		.report_container .grayOuterBox .whiteInnerBox {
			background-color	:	#ffffff;
			padding				:	4px;
		}
		.report_container .grayOuterBox .whiteInnerBox .grayInnerBox {
			/*background-color	:	#cccccc;*/
			padding				:	4px;
		}


		.report_container .GrayBox {
			height		:	80px;
			color		:	#363636;
			margin		:	10px;
			font-weight	:	bold;
			font-size	:	8pt;
			background-color:	#cccccc;
		}
		.report_container img.verticalAlign {
			height		:	100%;
			width		:	1px;
			vertical-align	:	middle;
		}

		.report_container .BigPad {
			width: 111px;
			height: 111px;
			position: relative;
			margin: 10px;
		}

		.report_container .ActiveBusniessIndicator {
			background	: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -87px -73px;
			height		: 34px;
			width		: 34px;
			margin: 5px;
		}

		.report_container .InActiveBusniessIndicator {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -87px -32px;
			height		: 34px;
			width		: 34px;
			margin: 5px;
		}

		.report_container .BigPadGreen {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -752px;
		}

		.report_container .BigPadYellow {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -632px;
		}

		.report_container .BigPadRed {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -992px;
		}

		.report_container .BigPad .title {
			text-align: center;
			left: 		0px;
			color: 		white;
			font-size:	10pt;
			top: 		0px;
			font-weight:bold;
			width: 		105px;
			#position: 	relative;
			display:	table;
			height: 	30px;
			overflow:	hidden;
			padding	:	0 5px 0 0;
		}

		.report_container .BigPad .value {
			text-align: center;
			color: white;
			font-size: 40pt;
			top: 23px;
			left	:	0px;
			position: absolute;
			width	:	111px;
			font-weight : bold;
		}

		.report_container .BigPad .bottom {
			text-align: center;
			left: 0px;
			color: white;
			font-size: 10pt;
			top: 85px;
			position: absolute;
			font-weight: bold;
			width: 111px;
		}

		.report_container .MiddlePad {
			width: 56px;	/* For working with IE6 */
			height: 56px;
			margin: 5px auto;
			padding: 0 auto;
			line-height:56px;
		}

		.report_container .scoreLowRisk {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1718px;
		}
		.report_container .scoreLowMedRisk {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1787px;
		}
		.report_container .scoreMedRisk {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1858px;
		}
		.report_container .scoreMedHighRisk {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1932px;
		}
		.report_container .scoreHighRisk {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -2009px;
		}

		.report_container .MiddlePadGreen {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1718px;
			/*background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -177px;*/
		}

		.report_container .MiddlePadYellow {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -1858px;
			/*background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -113px;*/
		}

		.report_container .MiddlePadRed {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -2009px;
			/*background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat 0 -307px;*/
		}

		.report_container .MiddlePad .title {
			text-align: center;
			left: 0px;
			color: white;
			font-size: 8pt;
			top: 5px;
			height: 10px;
			position: absolute;
			font-weight: bold;
			width: 55px;
		}

		.report_container .MiddlePad .value {
			color: white;
			font-size: 26px;
		}
		.report_container .MiddlePad.scoreLowMedRisk .value,.report_container .MiddlePad.scoreMedRisk .value,.report_container .MiddlePad.MiddlePadYellow .value,.report_container .MiddlePad.scoreUnkownRisk .value {
			color	:	<xsl:value-of select = "$reportTextColor"/>;
		}

		.report_container .MiddlePad .bottom {
			text-align: center;
			left: 0px;
			color: white;
			font-size: 12pt;
			top: 85px;
			position: absolute;
			font-weight: bold;
			width: 111px;
		}

		.report_container .SmallPad {
			width: 34px;
			height: 34px;
			position: relative;
			margin: 5px;
		}

		.report_container .SmallPadGreen {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -87px -175px;
		}

		.report_container .SmallPadYellow {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -87px -111px;
		}

		.report_container .SmallPadRed {
			background: #fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -87px -306px;
		}

		.report_container .SmallPad .title {
			text-align: center;
			left: 0px;
			color: white;
			font-size: 8pt;
			top: 2px;
			height: 16px;
			position: absolute;
			width: 32px;
		}

		.report_container .SmallPad .value {
			text-align: center;
			color: white;
			font-size: 10pt;
			top: 7px;
			left: 0px;
			position: absolute;
			width: 32px;
			font-weight: bold;
		}

		.report_container .SmallPad .valueWithTitle {
			text-align: center;
			color: white;
			font-size: 12pt;
			top: 12px;
			left: 0px;
			position: absolute;
			width: 32px;
			font-weight: bold;
		}

		.report_container .scoreGraphic {
			width: 380px;
			height: 100px;
			position: relative;
			margin: 5px 30px 0px 10px;
			/*background: #fff url(<xsl:value-of select="$basePath"/>barMeter.png) no-repeat 0 0;*/
		}
		.report_container .meter210 {
			background: #fff url(<xsl:value-of select="$basePath"/>commercial-intelliscore-210.png) no-repeat 0 0;
		}
		.report_container .meter214 {
			background: #fff url(<xsl:value-of select="$basePath"/>intelliscore-plus-bar.png) no-repeat 0 0;
		}
		.report_container .meter223 {
			background: #fff url(<xsl:value-of select="$basePath"/>fsr-bar.png) no-repeat 0 0;
		}
		.report_container .dbtMeter {
			background: #fff url(<xsl:value-of select="$basePath"/>dbt-ranges.png) no-repeat 0 0;
		}

		.report_container .graphicTitle,
		.report_container .scoreGraphic .title {
			font-size		:	13px;
			font-family		:	verdana, arial, helvetica, sans-serif;
			position		:	relative;
		}

		.report_container .scoreGraphic .scoreMeter {
			position: absolute;
			left: 50px;
			top: 50px;
			width: 300px;
			height: 25px;
			/*background: #fff url(<xsl:value-of select="$basePath"/>color_meter_round.gif) no-repeat 0 0;*/
		}

		.report_container .scoreGraphic .LeftText {
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 7pt;
			text-align: right;
			position: absolute;
			top: 55px;
		}

		.report_container .scoreGraphic .RightText {
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 7pt;
			text-align: left;
			position: absolute;
			top: 55px;
			left: 355px;
		}

		.report_container .scoreGraphic .MinValue {
			position: absolute;
			top: 55px;
			left: 50px;
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 7pt;
		}

		.report_container .scoreGraphic .MaxValue {
			position: absolute;
			top: 55px;
			left: 330px;
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 7pt;
		}

		.report_container .scoreGraphic .BottomText {
			color: <xsl:value-of select = "$reportTextColor"/>;
			font-size: 7pt;
			text-align: center;
			position: absolute;
			width: 350px;
			top: 75px;
		}

		.report_container .scoreGraphic .scoreValueArrow {
			position: absolute;
			top: 20px;
			font-size: 7pt;
			width: 13px;
			height: 13px;
			background: transparent url(<xsl:value-of select="$basePath"/>barMeter.png) no-repeat -390px 0;
		}

		.report_container .scoreGraphic .scoreValue {
			position		:	absolute;
			top				:	3px;
			padding			:	5px 0 0 0;
			color			:	<xsl:value-of select = "$reportTextColor"/>;
			font-size		:	10px;
			width			:	34px;
			height			:	34px;
			font-weight		:	bold;
			text-align		:	center;
			/*background	:	#fff url(<xsl:value-of select="$basePath"/>colorPads.gif) no-repeat -86px -502px;*/
		}

		.report_container table.section td <xsl:text disable-output-escaping="yes">&gt;</xsl:text> div.GrayBox {
			padding 	:	0;
		}

		.report_container table tr.section<xsl:text disable-output-escaping="yes">&gt;</xsl:text>td {
			border-top: 1px solid <xsl:value-of select = "$borderColor"/>;
		}


		.report_container table.section td <xsl:text disable-output-escaping="yes">&gt;</xsl:text> div {
			/*padding: 0 5px;*/
		}

		.report_container .LowScoreText {
			color	:	#209a5c;
		}
		.report_container .LowMedScoreText {
			color	:	#8aca64;
		}
		.report_container .MedScoreText {
			color	:	#f4e35b;
		}
		.report_container .MedHighScoreText {
			color	:	#ee7240;
		}
		.report_container .HighScoreText {
			color	:	#d6373e;
		}

		.report_container .indent1 {
			padding-left	:		16pt;
		}
		.report_container .indent2 {
			padding-left	:		32pt;
		}
		.report_container .indent3 {
			padding-left	:		48pt;
		}
		.report_container .indent4 {
			padding-left	:		64pt;
		}

		.report_container .DBTChart {
			border			:		solid <xsl:value-of select = "$borderColor"/> 2px;
			width			:		350px;
			text-align		:		center;
			padding			:		20px 5px;
			height			:		100px;
			margin			:		20px;
			overflow		:		hidden;
		}

		.report_container .DBTChart .valueArrow {
			background		:		#fff url(<xsl:value-of select="$basePath"/>triangle_blue.gif) no-repeat 0 0;
			width			:		5px;
			height			:		14px;
		}

		.report_container .DBTChart .DBTmeter {
			height			:		20px;
			width			:		300px;
			test-align		:		center;
			padding-top		:		3px;
			margin			:		0 auto
		}
		.report_container .DBTChart .DBTmeterValue {
			height			:		20px;
			width			:		300px;
			test-align		:		center;
			clear			:		both;
			font-weight		:		bold;
			margin			:		0 auto
		}
		.report_container .DBTmeter div, .report_container .DBTmeterValue div {
			float			:		left;
		}

		.report_container .DBTChart .Green {
			width			:		240px;
		}
		.report_container .DBTChart .Yellow {
			width			:		33px;
		}
		.report_container .DBTChart .Red {
			width			:		27px;
		}
		.report_container .DBTmeter .Green {
			height			:		100%;
			background		:		#00aa00;

			filter			:		progid:DXImageTransform.Microsoft.gradient(GradientType="1", startColorstr='#00aa00', endColorstr='#ffff00');	/* IE */
			background		: 		-webkit-gradient(linear, left top, right top, from(#00aa00), to(#ffff00));
			background		:		-moz-linear-gradient(left,  #00aa00,  #ffff00);
			border-style	:		none;
		}
		.report_container .DBTmeter .Yellow {
			height			:		100%;
			background		:		#ffff00;

			filter			:		progid:DXImageTransform.Microsoft.gradient(GradientType="1", startColorstr='#ffff00', endColorstr='#ff0000');	/* IE */
			background		: 		-webkit-gradient(linear, left top, right top, from(#ffff00), to(#ff0000));
			background		:		-moz-linear-gradient(left,  #ffff00,  #ff0000);
			border-style	:		none;
		}
		.report_container .DBTmeter .Red {
			height			:		100%;
			background		:		#ff0000;
			border-style	:		none;
		}

		.report_container .verifiedLegalName {
			background		:		transparent url(<xsl:value-of select="$basePath"/>check.jpg) no-repeat 0 0px;
			height			:		56px;
			width			:		75px;
		}
		}
	</style>

  </xsl:template>

</xsl:stylesheet>