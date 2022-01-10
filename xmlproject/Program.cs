using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                        if (fileloc[0] == '"' && fileloc[fileloc.Length - 1] == '"')
                        {
                            fileloc = fileloc.Substring(1, fileloc.Length - 2);
                        }
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
                            <Stock>
                                <CUSIP>
                                    <xsl:value-of select='cusip'/>
                                </CUSIP>
                                <nameOfIssuer>
                                    <xsl:value-of select='nameOfIssuer'/>
                                </nameOfIssuer>
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
                            </Stock>
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

            Console.WriteLine("spy? (y/n)");

            string arg4 = "";
            arg4 = Console.ReadLine().ToLower();
            while (arg4 != "y" && arg4 != "n")
            {
                Console.WriteLine("Invalid selection, please type y or n to indicate yes or no.");
                arg4 = Console.ReadLine().ToLower();
            }

            XDocument spydoc = XDocument.Load(Path.Combine(Environment.CurrentDirectory, @"Data\", "sandp500holdings.xml"));
            XDocument result;
            string secondxslt = "";
            if (arg4 == "y")
            {

                //following code below can use modified doc as-is without saving the file first

                Console.WriteLine("Please wait a few seconds...");
                XElement unionTest = new XElement("result",
                        (from first in newDocument.Root.Descendants()
                                join second in spydoc.Root.Descendants()
                                on (string)first.Element("CUSIP") equals (string)second.Element("CUSIP")
                                select new XElement("Stock",
                                    first.Element("CUSIP"),
                                    second.Element("CompanyName"),
                                    second.Element("Ticker"),
                                    first.Elements("pcs"))));

                result = new XDocument(unionTest);
                secondxslt = @"<?xml version='1.0' encoding='utf-8'?>
                <xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
                <xsl:output indent='yes'/>
                <xsl:template match='/'>
	                <Root>
		            <xsl:for-each select='result/Stock'>
			            <xsl:sort select='sum(pcs/value)' data-type='number' order='descending'/>
                        <Stock>
                            <CompanyName><xsl:value-of select='CompanyName'/></CompanyName>
				            <Ticker><xsl:value-of select='Ticker'/></Ticker>
				            <totalval><xsl:value-of select='sum(pcs/value)'/></totalval>
                            <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""SH""]) != 0'>
                                    <pcs>
                                    <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""SH""])'/></value>
                                    <putCall>SH</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""Call""]) != 0'>
                                    <pcs>
                                    <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""Call""])'/></value>
                                    <putCall>Call</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""Put""]) != 0'>
                                    <pcs>
                                    <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""Put""])'/></value>
                                    <putCall>Put</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                        </Stock>
		            </xsl:for-each>
	                </Root>
                </xsl:template>
                </xsl:stylesheet>";
            }
            else
            {
                result = newDocument;
                Console.WriteLine("Please wait a few seconds...");
                secondxslt = @"<?xml version='1.0' encoding='utf-8'?>
                <xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'>
	            <xsl:template match='/'>
                    <Root>
					<xsl:for-each select='Root/Stock'>
						<xsl:sort select='sum(pcs/value)' data-type='number' order='descending'/>
                        <Stock>
						    <nameOfIssuer><xsl:value-of select='nameOfIssuer'/></nameOfIssuer>
                            <totalval><xsl:value-of select='sum(pcs/value)'/></totalval>
						    <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""SH""]) != 0'>
                                    <pcs>
                                        <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""SH""])'/></value>
                                        <putCall>SH</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""Call""]) != 0'>
                                    <pcs>
                                        <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""Call""])'/></value>
                                        <putCall>Call</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                            <xsl:choose>
                                <xsl:when test='sum(pcs/value[../putCall/text() = ""Put""]) != 0'>
                                    <pcs>
                                        <value><xsl:value-of select='sum(pcs/value[../putCall/text() = ""Put""])'/></value>
                                        <putCall>Put</putCall>
                                    </pcs>
                                </xsl:when>
                                <xsl:otherwise>
                                </xsl:otherwise>
                            </xsl:choose>
                        </Stock>
					</xsl:for-each>
				    </Root>
	            </xsl:template>
                </xsl:stylesheet>";
            }

            var finresult = new XDocument();

            using (var stringReader = new StringReader(secondxslt))
            {
                using (XmlReader xsltReader = XmlReader.Create(stringReader))
                {
                    var transformer = new XslCompiledTransform();
                    transformer.Load(xsltReader);
                    using (XmlReader oldDocumentReader = result.CreateReader())
                    {
                        using (XmlWriter newDocumentWriter = finresult.CreateWriter())
                        {
                            transformer.Transform(oldDocumentReader, newDocumentWriter);
                        }
                    }
                }
            }

            finresult.Save("sorted.xml");

            var json = "var results = " + JsonConvert.SerializeXNode(finresult, Newtonsoft.Json.Formatting.Indented, true);

            //setting it into a js file so it can be called, bypassing CORS
            File.WriteAllText("results.js",json);

            //Needs to output json onto webpage

            Console.WriteLine("Completed.");

            //automatically open up to default browser with results
            Process.Start(@"cmd.exe ", @"/c " + "output.html");
        }
    }
}
