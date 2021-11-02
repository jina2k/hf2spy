using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using Wmhelp.XPath2;
using Saxon.Api;

namespace xmlproject
{
    class Program
    {
        static void Main()
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
                        if (fileloc[0] == '"' && fileloc[fileloc.Length-1] == '"')
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
            //remove namespace before converting to xmldoc
            hedgie.Root.Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            hedgie.Descendants().Attributes().Where(e => e.IsNamespaceDeclaration).Remove();
            foreach (var elem in hedgie.Descendants())
                elem.Name = elem.Name.LocalName;

            //convert
            XmlDocument hedgiexml = new XmlDocument();
            hedgiexml.LoadXml(hedgie.ToString());

            Processor processor = new Processor();
            XdmNode input = processor.NewDocumentBuilder().Wrap(hedgiexml);
            XsltCompiler compiler = processor.NewXsltCompiler();
            
            //Path.Combine(Environment.CurrentDirectory
            Xslt30Transformer transformer = compiler.Compile(new Uri(Path.Combine(Environment.CurrentDirectory, "xslt2test.xsl"))).Load30();
            DomDestination domde = new DomDestination();

            transformer.ApplyTemplates(input, domde);

            domde.XmlDocument.Save("hedgefile.xml");

            var newDocument = new XDocument();
            using (var nodeReader = new XmlNodeReader(domde.XmlDocument))
            {
                nodeReader.MoveToContent();
                newDocument = XDocument.Load(nodeReader);
            }

            Console.WriteLine("spy? (y/n)");

            string arg4 = "";
            arg4 = Console.ReadLine().ToLower();
            while (arg4 != "y" && arg4 != "n")
            {
                Console.WriteLine("Invalid selection, please type y or n to indicate yes or no.");
                arg4 = Console.ReadLine().ToLower();
            }

            if (arg4 == "y")
            {
                XNode spydoc = XDocument.Load(Path.Combine(Environment.CurrentDirectory, @"Data\", "sandp500holdings.xml"));

                //following code below can use modified doc as-is without saving the file first

                Console.WriteLine("Please wait a few minutes...");
                XElement body = new XElement("result",
                    (from stock in spydoc.XPath2SelectElements("//Stock")
                     from infotable in newDocument.XPath2SelectElements("//infoTable")
                     where (bool)XPath2Expression.Evaluate(@"$s/CUSIP = $i/@cusip", new { s = stock, i = infotable })
                     select new XElement("opStock",
                         stock.Element("CompanyName"),
                         stock.Element("CUSIP"),
                         stock.Element("Ticker"),
                         infotable.Element("totalval"),
                         infotable.Elements("allvals")
                         )));

                XDocument result = new XDocument(
                    new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='format.xsl'"),
                    body);


                result.Save("results.xml");
            }
            else
            {
                Console.WriteLine("Please wait a few minutes...");
                XElement body = new XElement("result",
                    (from infotable in newDocument.XPath2SelectElements("//infoTable")
                     select new XElement("opStock",
                        infotable.Element("namae"),
                        infotable.Element("totalval"),
                        infotable.Elements("allvals")
                        )));

                XDocument result = new XDocument(
                    new XProcessingInstruction("xml-stylesheet", "type='text/xsl' href='format2.xsl'"),
                    body);


                result.Save("results.xml");
            }
            Console.WriteLine("Completed.");

            Process.Start("C:\\Program Files\\Internet Explorer\\iexplore.exe", Path.Combine(Environment.CurrentDirectory, "results.xml"));
        }
    }
}

