<xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='2.0'>
	<xsl:output indent='yes'/>
	<xsl:template match='informationTable'>
		<Root>
			<xsl:for-each-group select='infoTable' group-adjacent='cusip'>
				<infoTable cusip='{current-grouping-key()}'>
					<namae>
						<xsl:value-of select='nameOfIssuer'/>
					</namae>
					<totalval>
						<xsl:value-of select='sum(current-group()/value)'/>
					</totalval>
					<allvals>
						<xsl:for-each-group select='current-group()' group-by='(putCall, "Shares")[1]'>
							<putCall otype='{current-grouping-key()}'>
								<xsl:value-of select='sum(current-group()/value)'/>
							</putCall>
						</xsl:for-each-group>
					</allvals>
				</infoTable>
			</xsl:for-each-group>
		</Root>
	</xsl:template>
</xsl:stylesheet>
