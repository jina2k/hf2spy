using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Wmhelp.XPath2;

namespace xmlproject
{
    class Program
    {
        static void Main(string[] args)
        {
            string arg1 = "";
            Console.WriteLine("Hello, welcome to hf2spy. Please choose between citadel, credit suisse (cs), and other.");
            while (arg1 == "")
            {
                arg1 = Console.ReadLine();
                arg1 = arg1.ToLower();
                if (arg1 != "citadel" && arg1 != "cs" && arg1 != "other")
                {
                    Console.WriteLine("Please enter a valid command: (citadel, cs, other)");
                    arg1 = "";
                }
            }

            Console.WriteLine("You have chosen: " + arg1);

            var doc = new XDocument();
            string fileloc = "";

            while (fileloc == "")
            {
                if (arg1 == "citadel")
                {
                    string filename = "citadelmay2021.xml";
                    fileloc = Path.Combine(Environment.CurrentDirectory, @"Data\", filename);
                }
                else if (arg1 == "cs")
                {
                    string filename = "creditsuissemay2021.xml";
                    fileloc = Path.Combine(Environment.CurrentDirectory, @"Data\", filename);
                }
                else //other
                {
                    while (fileloc == "")
                    {
                        Console.WriteLine("Please enter the directory/location of the XML file of the hedge fund, or drag the file into the console application.");
                        Thread.Sleep(5000);
                        fileloc = Console.ReadLine();
                    }
                }



                try
                {
                    XmlReader reader = XmlReader.Create(fileloc);
                }
                catch (ArgumentNullException)
                {
                    // The input value is null.
                    Console.WriteLine("Input value is null, please try again.");
                    fileloc = "";
                }
                catch (SecurityException)
                {
                    // The XmlReader does not have sufficient permissions 
                    // to access the location of the XML data.
                    Console.WriteLine("XmlReader cannot access location due to insufficient permissions, please move file into a compatible directory.");
                    fileloc = "";
                }
                catch (FileNotFoundException)
                {
                    // The underlying file of the path cannot be found
                    Console.WriteLine("Cannot find file, please try again.");
                    fileloc = "";
                }
                catch (IOException)
                {
                    Console.WriteLine("Filename syntax is incorrect, please try again.");
                    fileloc = "";
                }
                catch (UnauthorizedAccessException)
                {
                    Console.WriteLine("Access to " + fileloc + " is denied. Please move file into the same hard drive as the console application, or into the console application folder if possible.");
                    fileloc = "";
                }
            }
            Console.WriteLine("Loading " + fileloc + "...");

            XDocument hedgie = XDocument.Load(fileloc);
            
            
            hedgie.Root.Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            hedgie.Descendants().Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            foreach (var elem in hedgie.Descendants())
                elem.Name = elem.Name.LocalName;

            string xslt = @"<?xml version='1.0'?>
            <xsl:stylesheet xmlns:xsl='http://www.w3.org/1999/XSL/Transform' version='1.0'>
            <xsl:output indent='yes'/>
            <xsl:key name='cusip-link' match='informationTable/infoTable' use='cusip'/>
                <xsl:template match='/'>
                    <Root>
                        <xsl:for-each select='informationTable/infoTable[count(. | key(""cusip-link"", cusip)[1]) = 1]'>
                            <infoTable cusip='{cusip}'>
                                <xsl:for-each select='key(""cusip-link"", cusip)'>
                                    <pcs>
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
                                    </pcs>
                                </xsl:for-each>
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

            newDocument.Save("hedgefile.xml"); //storing output in hedgefile.xml

            XNode spydoc = XDocument.Load(Path.Combine(Environment.CurrentDirectory, @"Data\", "sandp500holdings.xml"));

            //following code below can use modified doc as-is without saving the file first

            Console.WriteLine("Please wait a few minutes...");
            XElement body = new XElement("result",
                (from stock in spydoc.XPath2SelectElements("//Stock")
                 from infotable in newDocument.XPath2SelectElements("//infoTable")
                 where (bool)XPath2Expression.Evaluate(@"$s/CUSIP = $i/@cusip", new { s = stock , i = infotable })
                 select new XElement("opStock",
                     stock.Element("CompanyName"),
                     stock.Element("CUSIP"),
                     stock.Element("Ticker"),
                     infotable.Elements("pcs")
                     )));

            XDocument result = new XDocument(
                new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='format.xsl'"),
                body);

            
            result.Save("results.xml");

        }
    }
}
