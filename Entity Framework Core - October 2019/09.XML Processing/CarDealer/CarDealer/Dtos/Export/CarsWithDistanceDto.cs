namespace CarDealer.Dtos.Export
{
    using System;
    using System.Xml.Serialization;
    
    //for problem 19

    [XmlType("car")]
    public class CarsWithDistanceDto
    {
        [XmlAttribute("make")]
        public string Make { get; set; }

        [XmlAttribute("model")]
        public string Model { get; set; }

        [XmlAttribute("travelled-distance")]
        public long TravelledDistance { get; set; }
    }

    //for problem 14

    //[XmlType("car")]
    //public class CarsWithDistanceDto
    //{
    //    [XmlElement(ElementName = "make")]
    //    public string Make { get; set; }

    //    [XmlElement(ElementName = "model")]
    //    public string Model { get; set; }

    //    [XmlElement(ElementName = "travelled-distance")]
    //    public long TravelledDistance { get; set; }
    //}
}
