using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace xmlgroup
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileloc = Path.Combine(Environment.CurrentDirectory, @"Data\", "citadelmay2021.xml");
            XDocument hedgie = XDocument.Load(fileloc);

            hedgie.Root.Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            hedgie.Descendants().Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            foreach (var elem in hedgie.Descendants())
                elem.Name = elem.Name.LocalName;

            string xslt = @"<?xml version='1.0'?>
            <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>
            <xsl:output indent='yes'/>
                <xsl:template match='/'>
                    <Root>
                        <xsl:for-each select='informationTable/infoTable'>
                            <infoTable>
                                <cusip>
                                <xsl:value-of select='cusip'/>
                                </cusip>
                                <value>
                                <xsl:value-of select='value'/>
                                </value>
                                <xsl:choose>
                                    <xsl:when test='not(putCall)'>
                                        <putCall>SH</putCall>
                                    </xsl:when>
                                    <xsl:otherwise>
                                        <putCall><xsl:value-of select='putCall'/></putCall>
                                    </xsl:otherwise>
                                </xsl:choose>
                            </infoTable>
                        </xsl:for-each>
                    </Root>
                </xsl:template>
            </xsl:stylesheet>";


            var newDocument = new XDocument();

            using (var stringReader = new StringReader(xslt))
            {
                using (XmlReader xsltReader = XmlReader.Create(stringReader))
                {
                    var transformer = new XslCompiledTransform();
                    transformer.Load(xsltReader);
                    using (XmlReader oldDocumentReader = hedgie.CreateReader())
                    {
                        using (XmlWriter newDocumentWriter = newDocument.CreateWriter())
                        {
                            transformer.Transform(oldDocumentReader, newDocumentWriter);
                        }
                    }
                }
            }

            newDocument.Save("hedgefile.xml"); //testing output
        }
    }
}
