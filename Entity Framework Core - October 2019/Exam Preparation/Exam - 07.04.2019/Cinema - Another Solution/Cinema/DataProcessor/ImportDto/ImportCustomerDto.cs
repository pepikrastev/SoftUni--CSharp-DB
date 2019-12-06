using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Customer")]
    public class ImportCustomerDto
    {
        [XmlElement("FirstName")]
        [MinLength(3), MaxLength(20), Required]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        [MinLength(3), MaxLength(20), Required]
        public string LastName { get; set; }

        [XmlElement("Age")]
        [Range(12, 110), Required]
        public int Age { get; set; }

        [XmlElement("Balance")]
        [Range(0, double.MaxValue), Required]
        public decimal Balance { get; set; }

        [XmlArray("Tickets")]
        public ImportTicketDto[] Tickets { get; set; } 
    }
}
