using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace csv2xml
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, @"Data\", "sandp500holdings.csv"));

            XElement xml = new XElement("Root",
                from str in lines
                let columns = str.Split(',')
                select new XElement("Stock",
                    new XElement("CompanyName", columns[0]),
                    new XElement("Ticker", columns[1]),
                    new XElement("CUSIP", columns[2])
                )
            );

            xml.Save(Path.Combine(Environment.CurrentDirectory, @"Data\", "sandp500holdings.xml"));
        }
    }
}
