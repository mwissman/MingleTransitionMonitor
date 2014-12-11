<?xml version="1.0" encoding="ISO-8859-1"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" xmlns:html="http://www.w3.org/Profiles/XHTML-transitional">

    <xsl:output method="html" indent="yes"/>
    <xsl:include href="toolkit.xsl"/>
    <xsl:preserve-space elements='a root'/>


    <!--
    ====================================================
        Create the page structure
    ====================================================
-->
    <xsl:template match="test-results">
        <HTML>
            <HEAD>
                <style type="text/css">
                    body {
                    font:normal 68% verdana,arial,helvetica;
                    color:#000000;
                    }

                    span.covered {
                    background: #00df00;
                    border:#9c9c9c 1px solid;
                    height: 11px;
                    display: inline-block;
                    }
                    span.uncovered {
                    background: #df0000;
                    border-top:#9c9c9c 1px solid;
                    border-bottom:#9c9c9c 1px solid;
                    border-right:#9c9c9c 1px solid;
                    height: 11px;
                    display: inline-block;
                    margin-top: -1px;
                    }
                    span.ignored {
                    background: #ffff00;
                    border-top:#9c9c9c 1px solid;
                    border-bottom:#9c9c9c 1px solid;
                    border-right:#9c9c9c 1px solid;
                    height: 11px;
                    display: inline-block;
                    margin-top: -1px;
                    }

                    td {
                    FONT-SIZE: 68%;
                    BORDER-BOTTOM: #dcdcdc 1px solid;
                    BORDER-RIGHT: #dcdcdc 1px solid;
                    }
                    p {
                    line-height:1.5em;
                    margin-top:0.5em;
                    margin-bottom:1.0em;
                    }
                    h1 {
                    MARGIN: 0px 0px 5px;
                    FONT: 165% verdana,arial,helvetica;
                    }
                    h2 {
                    MARGIN-TOP: 1em;
                    MARGIN-BOTTOM: 0.5em;
                    FONT: bold 125% verdana,arial,helvetica;
                    }
                    h3 {
                    MARGIN-BOTTOM: 0.5em;
                    FONT: bold 115% verdana,arial,helvetica;
                    }
                    h4 {
                    MARGIN-BOTTOM: 0.5em;
                    FONT: bold 100% verdana,arial,helvetica;
                    }
                    h5 {
                    MARGIN-BOTTOM: 0.5em;
                    FONT: bold 100% verdana,arial,helvetica
                    }
                    h6 {
                    MARGIN-BOTTOM: 0.5em;
                    FONT: bold 100% verdana,arial,helvetica
                    }
                    .Error {
                    font-weight:bold;
                    color:red;
                    }
                    .Failure {
                    font-weight:bold;
                    color:red;
                    }
                    .Ignored {
                    font-weight:bold;
                    }
                    .FailureDetail {
                    font-size: -1;
                    padding-left: 2.0em;
                    background:#cdcdcd;
                    }
                    .Pass {
                    padding-left:2px;
                    }
                    .TableHeader {
                    background: #efefef;
                    color: #000;
                    font-weight: bold;
                    horizontal-align: center;
                    }
                    a:visited {
                    color: ##1f66bd;
                    }
                    a {
                    color: ##1f66bd;
                    }
                    a:active {
                    color: ##1f66bd;
                    }
                    a.summarie {
                    color:#000;
                    text-decoration: none;
                    }
                    a.summarie:active {
                    color:#000;
                    text-decoration: none;
                    }
                    a.summarie:visited {
                    color:#000;
                    text-decoration: none;
                    }
                    .description {
                    margin-top:1px;
                    padding:3px;
                    background-color:#dcdcdc;
                    color:#000;
                    font-weight:normal;
                    }
                    .method{
                    color:#000;
                    font-weight:normal;
                    padding-left:5px;
                    }
                    a.method{
                    text-decoration: none;
                    color:#000;
                    font-weight:normal;
                    padding-left:5px;
                    }
                    a.Failure, a.Error {
                    font-weight:bold;
                    color:red;
                    text-decoration: none;
                    }
                    a.Failure:visited, a.Error:visited {
                    font-weight:bold;
                    color:red;
                    text-decoration: none;
                    }
                    a.Failure:active, a.Error:active {
                    font-weight:bold;
                    color:red;
                    text-decoration: none;
                    }
                    a.error {
                    font-weight:bold;
                    color:red;
                    }
                    a.error:visited {
                    font-weight:bold;
                    color:red;
                    }
                    a.error:active {
                    font-weight:bold;
                    color:red;
                    /*text-decoration: none;
                    padding-left:5px;*/
                    }
                    a.ignored {
                    font-weight:bold;
                    text-decoration: none;
                    padding-left:5px;
                    }
                    a.ignored:visited {
                    font-weight:bold;
                    text-decoration: none;
                    padding-left:5px;
                    }
                    a.ignored:active {
                    font-weight:bold;
                    text-decoration: none;
                    padding-left:5px;
                    }
                </style>
                <script language="JavaScript">
                    <![CDATA[   
      function Toggle(id) {
      var element = document.getElementById(id);

         if ( element.style.display == "none" )
            element.style.display = "block";
         else 
            element.style.display = "none";
      }

      function ToggleImage(id) {
      var element = document.getElementById(id);

         if ( element.innerText   == "-" )
            element.innerText   = "+";
         else 
            element.innerText = "-";
      }
    ]]>
                </script>
            </HEAD>
            <body text="#000000" bgColor="#ffffff">
                <a id="#top"></a>
                <xsl:call-template name="header"/>

                <!-- Summary part -->
                <xsl:call-template name="summary"/>
                <hr size="1" width="95%" align="left"/>

                <!-- Package List part -->
                <xsl:call-template name="packagelist"/>
                <hr size="1" width="95%" align="left"/>

                <!-- For each testsuite create the part -->
                <xsl:call-template name="testsuites"/>
                <hr size="1" width="95%" align="left"/>

            </body>
        </HTML>
    </xsl:template>



    <!-- ================================================================== -->
    <!-- Write a list of all packages with an hyperlink to the anchor of    -->
    <!-- of the package name.                                               -->
    <!-- ================================================================== -->
    <xsl:template name="packagelist">
        <h2 id=":i18n:TestSuiteSummary">TestSuite Summary</h2>
        <table border="0" cellpadding="2" cellspacing="0" width="95%">
            <xsl:call-template name="packageSummaryHeader"/>
            <!-- list all packages recursively -->
            <xsl:for-each select="//test-suite[(child::results/test-case)]">
                <xsl:sort select="@name"/>
                <xsl:variable name="total" select="count(child::results/test-case)"/>
                <xsl:variable name="errorCount" select="count(child::results/test-case[@result='Error'])"/>
                <xsl:variable name="ignoredCount" select="count(child::results/test-case[@result='Ignored'])"/>
                <xsl:variable name="failureCount" select="count(child::results/test-case[@result='Failure'])"/>
                <xsl:variable name="allFailureCount" select="$errorCount + $failureCount"/>
                <xsl:variable name="successCount" select="$total - $errorCount - $ignoredCount - $failureCount"/>
                <xsl:variable name="timeCount" select="translate(@time,',','.')"/>

                <!-- write a summary for the package -->
                <tr valign="top">
                    <!-- set a nice color depending if there is an error/failure -->
                    <xsl:attribute name="class">
                        <xsl:choose>
                            <xsl:when test="$allFailureCount &gt; 0">Failure</xsl:when>
                            <xsl:otherwise>Pass</xsl:otherwise>
                        </xsl:choose>
                    </xsl:attribute>
                    <td width="25%">
                        <a href="#{generate-id(@name)}">
                            <xsl:attribute name="class">
                                <xsl:choose>
                                    <xsl:when test="$allFailureCount &gt; 0">Failure</xsl:when>
                                </xsl:choose>
                            </xsl:attribute>
                            <xsl:value-of select="@name"/>
                        </a>
                    </td>
                    <td nowrap="nowrap" width="6%" align="right">
                        <xsl:variable name="successRate" select="$successCount div $total"/>
                        <b>
                            <xsl:call-template name="display-percent">
                                <xsl:with-param name="value" select="$successRate"/>
                            </xsl:call-template>
                        </b>
                    </td>
                    <td width="20%" height="9px">
                        <xsl:if test="round($successCount * 200 div $total )!=0">
                            <span class="covered">
                                <xsl:attribute name="style">
                                    width:<xsl:value-of select="round($successCount * 200 div $total )"/>px
                                </xsl:attribute>
                            </span>
                        </xsl:if>
                        <xsl:if test="round($ignoredCount * 200 div $total )!=0">
                            <span class="ignored">
                                <xsl:attribute name="title">
                                    Total:<xsl:value-of select="$total"/>,AllFailures:<xsl:value-of select="$allFailureCount"/>,Ignored:<xsl:value-of select="$ignoredCount"/>
                                </xsl:attribute>
                                <xsl:attribute name="style">
                                    width:<xsl:value-of select="round($ignoredCount * 200 div $total )"/>px;
                                    margin-left:<xsl:value-of select="round($successCount * 200 div $total )"/>px;
                                </xsl:attribute>
                            </span>
                        </xsl:if>
                        <xsl:if test="round($allFailureCount * 200 div $total )!=0">
                            <span class="uncovered">
                                <xsl:attribute name="style">
                                    width:<xsl:value-of select="round($allFailureCount * 200 div $total )"/>px;
                                    <xsl:choose>
                                    <xsl:when test="round($ignoredCount * 200 div $total )=0">
                                        margin-left:<xsl:value-of select="round($successCount * 200 div $total )"/>px;
                                    </xsl:when>
                                    <xsl:otherwise>
                                        margin-left:<xsl:value-of select="round($ignoredCount * 200 div $total )"/>px;
                                    </xsl:otherwise>
                                    </xsl:choose>
                                </xsl:attribute>
                            </span>
                        </xsl:if>
                    </td>
                    <td>
                        <xsl:value-of select="$total"/>
                    </td>
                    <td>
                        <xsl:value-of select="$ignoredCount"/>
                    </td>
                    <td>
                        <xsl:value-of select="$errorCount"/>
                    </td>
                    <td>
                        <xsl:value-of select="$failureCount"/>
                    </td>
                    <td>
                        <xsl:call-template name="display-time">
                            <xsl:with-param name="value" select="$timeCount"/>
                        </xsl:call-template>
                    </td>
                </tr>
            </xsl:for-each>
        </table>
    </xsl:template>

    <xsl:template name="testsuites">
        <table border="0" cellpadding="1" cellspacing="1" width="95%">
            <xsl:for-each select="//test-suite[(child::results/test-case)]">
                <xsl:sort select="@name"/>
                <tr>
                    <td colspan="5" style="border-right: none;">
                        <!-- create an anchor to this class name -->
                        <a id="{generate-id(@name)}"></a>
                        <h3>
                            <xsl:value-of select="@name"/>
                        </h3>

                        <!-- Header -->
                        <xsl:call-template name="classesSummaryHeader"/>
                    </td>
                </tr>
                <!-- match the testcases of this package -->
                <xsl:apply-templates select="results/test-case">
                    <xsl:sort select="@name" />
                </xsl:apply-templates>
                <tr>
                    <td colspan="5" style="border:none;height:12px;"></td>
                </tr>
                
            </xsl:for-each>
        </table>
    </xsl:template>


    <xsl:template name="dot-replace">
        <xsl:param name="package"/>
        <xsl:choose>
            <xsl:when test="contains($package,'.')">
                <xsl:value-of select="substring-before($package,'.')"/>_<xsl:call-template name="dot-replace">
                    <xsl:with-param name="package" select="substring-after($package,'.')"/>
                </xsl:call-template>
            </xsl:when>
            <xsl:otherwise>
                <xsl:value-of select="$package"/>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

</xsl:stylesheet>
