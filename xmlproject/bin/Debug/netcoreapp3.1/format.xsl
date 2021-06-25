<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:template match="/">
	<html>
	<body>
	<h2>Results</h2>
	<table border="1">
		<tr>
			<th>Company Name</th>
			<th>Ticker</th>
			<th>Market Value (x$1000)</th>
			<th>Put/Call</th>
		</tr>
		<xsl:for-each select="result/opStock">
			<xsl:sort select="value/text()" data-type="number" order="descending"/>
			<tr>
				<td>
				<xsl:value-of select="CompanyName"/>
				</td>
				<td>
				<xsl:value-of select="Ticker"/>
				</td>
				<td>
				<xsl:value-of select="value"/>
				</td>
				<td>
				<xsl:value-of select="putCall"/>
				</td>
			</tr>
		</xsl:for-each>
	</table>
	</body>
	</html>
</xsl:template>
</xsl:stylesheet>
