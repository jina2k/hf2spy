using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

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
        
        }
    }
}
