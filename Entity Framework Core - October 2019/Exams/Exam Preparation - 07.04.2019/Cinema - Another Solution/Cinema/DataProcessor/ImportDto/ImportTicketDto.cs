using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Ticket")]
    public class ImportTicketDto
    {
        [XmlElement("Price")]
        [Range(0, double.MaxValue), Required]
        public decimal Price { get; set; }

        [Required]
        [XmlElement("ProjectionId")]
        public int ProjectionId { get; set; }
    }
}
