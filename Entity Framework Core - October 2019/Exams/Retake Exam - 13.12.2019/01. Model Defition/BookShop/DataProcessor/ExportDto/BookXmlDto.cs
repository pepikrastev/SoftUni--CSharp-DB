using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace BookShop.DataProcessor.ExportDto
{
    [XmlType("Book")]
    public class BookXmlDto
    {
        [XmlAttribute]
        public string Pages { get; set; }

        [XmlElement]
        public string Name { get; set; }

        [XmlElement]
        public string Date { get; set; }
    }
}
