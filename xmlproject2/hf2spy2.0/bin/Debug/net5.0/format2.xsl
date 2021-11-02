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
							<xsl:sort select="totalval" data-type="number" order="descending"/>
							<tr>
								<td>
									<xsl:value-of select="namae"/>
								</td>
								<td>
									<xsl:value-of select="totalval"/>
								</td>
								<td></td>
							</tr>

							<xsl:choose>
								<xsl:when test="(allVals/putCall/@otype='Shares') != 0">
									<tr>
										<td></td>
										<td>
											<xsl:value-of select="allVals/putCall/@otype='Shares'"/>
										</td>
										<td>SH</td>
									</tr>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>

							<xsl:choose>
								<xsl:when test="(allVals/putCall/@otype='Call') != 0">
									<tr>
										<td></td>
										<td>
											<xsl:value-of select="(allVals/putCall/@otype='Call')"/>
										</td>
										<td>Call</td>
									</tr>
								</xsl:when>
								<xsl:otherwise>
								</xsl:otherwise>
							</xsl:choose>

							<xsl:choose>
								<xsl:when test="(allVals/putCall/@otype='Put') != 0">
									<tr>
										<td></td>
										<td>
											<xsl:value-of select="(allVals/putCall/@otype='Put')"/>
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
