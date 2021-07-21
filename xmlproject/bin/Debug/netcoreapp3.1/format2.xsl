<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<body>
				<h2>Results</h2>
				<table border="1">
					<tr>
						<th>Company Name</th>
						<th>Market Value (x$1000)</th>
						<th>Put/Call</th>
					</tr>
					<xsl:for-each select="result/opStock">
						<xsl:sort select="sum(pcs/value)" data-type="number" order="descending"/>
						<tr>
							<td>
								<xsl:value-of select="namae"/>
							</td>
							<td>
								<xsl:value-of select="sum(pcs/value)"/>
							</td>
							<td></td>
						</tr>

						<xsl:choose>
							<xsl:when test="sum(pcs/value[../putCall/text() = 'SH']) != 0">
								<tr>
									<td></td>
									<td>
										<xsl:value-of select="sum(pcs/value[../putCall/text() = 'SH'])"/>
									</td>
									<td>SH</td>
								</tr>
							</xsl:when>
							<xsl:otherwise>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="sum(pcs/value[../putCall/text() = 'Call']) != 0">
								<tr>
									<td></td>
									<td>
										<xsl:value-of select="sum(pcs/value[../putCall/text() = 'Call'])"/>
									</td>
									<td>Call</td>
								</tr>
							</xsl:when>
							<xsl:otherwise>
							</xsl:otherwise>
						</xsl:choose>

						<xsl:choose>
							<xsl:when test="sum(pcs/value[../putCall/text() = 'Put']) != 0">
								<tr>
									<td></td>
									<td>
										<xsl:value-of select="sum(pcs/value[../putCall/text() = 'Put'])"/>
									</td>
									<td>Put</td>
								</tr>
							</xsl:when>
							<xsl:otherwise>
							</xsl:otherwise>
						</xsl:choose>

					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
